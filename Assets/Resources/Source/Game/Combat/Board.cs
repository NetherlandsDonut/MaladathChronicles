using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Race;
using static Root;


public class Board
{
    public Board(int x, int y, string enemy)
    {
        bonusTurnStreak = 0;
        field = new int[x, y];
        player = currentSave.player;
        player.Initialise(false);
        this.enemy = new Entity(races.Find(x => x.name == enemy));
        playerTurn = true;
        temporaryElementsPlayer = new();
        temporaryElementsEnemy = new();
        temporaryBuffsPlayer = new();
        temporaryBuffsEnemy = new();
        actions = new List<Action>();
    }

    //STATIC REFERENCE TO THE BOARD
    //THERE CAN BE ONLY ONE AT A TIME THANKS TO STATIC REF
    public static Board board;

    public int bonusTurnStreak;
    public int[,] field;
    public Window window;
    public Entity player, enemy;
    public bool playerTurn, breakForEnemy, breakForCascade, enemyFinishedMoving, playerFinishedMoving;
    public List<GameObject> temporaryElementsPlayer, temporaryElementsEnemy, temporaryBuffsPlayer, temporaryBuffsEnemy;
    public List<Action> actions;

    //ENDS THE CURRENT PLAYER'S TURN
    public void EndTurn()
    {
        if (playerTurn)
        {
            cursorEnemy.fadeIn = true;
            playerTurn = false;
            playerFinishedMoving = false;
            enemy.Cooldown();
            enemy.FlareBuffs();
        }
        else
        {
            cursorEnemy.fadeOut = true;
            playerTurn = true;
            enemyFinishedMoving = false;
            player.Cooldown();
            player.FlareBuffs();
        }
    }

    //RESETS THE BOARD TO BE EMPTY AND REFILLED AGAIN
    public void Reset()
    {
        field = new int[field.GetLength(0), field.GetLength(1)];
        for (int i = 0; i < field.GetLength(0); i++)
            for (int j = 0; j < field.GetLength(1); j++)
                field[i, j] = 0;
        CDesktop.LockScreen();
    }

    public void AnimateBoard()
    {
        //MOVE ELEMENTS DOWN WITH GRAVITY
        for (int j = field.GetLength(1) - 1; j > 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == 0 && field[i, j - 1] != 0)
                {
                    (field[i, j], field[i, j - 1]) = (field[i, j - 1], 0);
                    for (int k = 0; k < field.GetLength(0); k++)
                        if (field[k, j] == 0) break;
                        else if (k == field.GetLength(0) - 1)
                            PlaySound("PutDownSmallWood", 0.04f);
                }

        //SPAWN NEW ELEMENTS ON TOP OF THE BOARD
        for (int i = 0; i < field.GetLength(0); i++)
            if (field[i, 0] == 0)
                do field[i, 0] = random.Next(11, 21);
                while (FloodCount(i, 0).Count >= 3);

        //IF BOARD NEEDS TO FILLED UP DON'T DO ANY FURTHER STEPS AND RETURN TO BEGINNING
        for (int j = field.GetLength(1) - 1; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == 0) return;

        //DO ONE SCHEDULED ACTION AND RETURN AFTER TO DO ONE AT A TIME
        if (actions.Count > 0)
        {
            actions[0]();
            actions.RemoveAt(0);
            return;
        }

        //CASCADE FOR CURRENT PLAYER
        for (int j = 0; j < field.GetLength(1); j++)
            for (int i = 0; i < field.GetLength(0); i++)
            {
                var list = FloodCount(i, j);
                if (list.Count >= 3)
                {
                    if (!breakForCascade)
                    {
                        FloodDestroy(list);
                        breakForCascade = true;
                    }
                    else
                    {
                        breakForCascade = false;
                        animationTime += frameTime;
                    }
                    return;
                }
            }

        //IF IT'S ENEMY'S TURN..
        if (!playerTurn)
        {
            //IF ENEMY FINISHED MOVING END THEIR TURN
            if (enemyFinishedMoving)
            {
                //UNLESS THEY SCORED A BONUS MOVE
                if (bonusTurnStreak > 0)
                {
                    bonusTurnStreak = 0;
                    enemyFinishedMoving = false;
                }
                else
                    EndTurn();
            }

            //DO ANIMATION BREAK FOR ENEMY TO GIVE THEM TIME TO THINK
            else if (!breakForEnemy)
            {
                animationTime = (float)(random.Next(3, 5) / 10.0) + 0.3f;
                breakForEnemy = true;
            }

            //IF ENEMY WAS ALREADY ON THINKING BREAK MAKE THEM MOVE
            else
            {
                breakForEnemy = false;
                bonusTurnStreak = 0;
                temporaryElementsEnemy = new();

                //GET ALL POSSIBLE MOVES AND WHAT HAPPENS AFTER THEM THAT IS PREDICTABLE
                var differentFloodings = new List<(int, int, List<(int, int, int)>)>();
                for (int i = 0; i < field.GetLength(0); i++)
                    for (int j = 0; j < field.GetLength(1); j++)
                    {
                        var list2 = board.FloodCount(i, j);
                        if (!differentFloodings.Exists(x => x.Item3.All(y => list2.Contains((y.Item1, y.Item2, y.Item3)))))
                            differentFloodings.Add((i, j, list2));
                    }
                var newBoard = new FutureBoard(board);
                newBoard.enemyFinishedMoving = true;
                var baseDesiredness = newBoard.Desiredness(newBoard.enemy, enemy, newBoard.player, player);
                var possibleMoves = new List<FutureMove>();
                var abilities = enemy.actionBars.Select(x => Ability.abilities.Find(y => y.name == x.ability));
                foreach (var ability in abilities)
                    if (newBoard.enemy.actionBars.Find(x => x.ability == ability.name).cooldown == 0 && ability.EnoughResources(enemy))
                    {
                        newBoard = new FutureBoard(board);
                        possibleMoves.Add(new FutureMove(ability.name, newBoard));
                        newBoard.enemy.actionBars.Find(x => x.ability == ability.name).cooldown = ability.cooldown;
                        ability.futureEffects(false, newBoard);
                        newBoard.enemy.DetractResources(ability.cost);
                        while (!newBoard.finishedAnimation)
                            newBoard.AnimateBoard();
                    }
                foreach (var flooding in differentFloodings)
                {
                    newBoard = new FutureBoard(board);
                    possibleMoves.Add(new FutureMove(flooding.Item1, flooding.Item2, newBoard));
                    newBoard.FloodDestroy(flooding.Item3);
                    newBoard.enemyFinishedMoving = true;
                    while (!newBoard.finishedAnimation)
                        newBoard.AnimateBoard();
                }
                possibleMoves = possibleMoves.OrderByDescending(x => x.Desiredness(board, baseDesiredness)).ToList();
                var message = "";
                foreach (var move in possibleMoves)
                {
                    var resourceChange = 0;
                    foreach (var resource in move.board.enemy.resources)
                        resourceChange += resource.Value - board.enemy.resources[resource.Key];
                    message += move.desiredness.ToString("0.000") + (move.x != -1 ? " (" + boardNameDictionary[board.field[move.x, move.y]] + ")" : (move.ability != "" ? " <" + move.ability + ">" : "")) + (resourceChange >= 0 ? " +" : " ") + resourceChange + " resources\n";
                }
                Debug.Log(message);
                var bestMove = possibleMoves[0];
                if (bestMove.ability != "")
                {
                    var abilityObj = Ability.abilities.Find(x => x.name == bestMove.ability);
                    var actionBar = enemy.actionBars.Find(x => x.ability == bestMove.ability);
                    board.actions.Add(() =>
                    {
                        cursorEnemy.Move(CDesktop.globalRegions["EnemyActionBar" + enemy.actionBars.IndexOf(actionBar)].transform.position + new Vector3(139, -10));
                        animationTime += frameTime * 9;
                    });
                    board.actions.Add(() => { cursorEnemy.SetCursor(CursorType.Click); });
                    board.actions.Add(() =>
                    {
                        cursorEnemy.SetCursor(CursorType.Default);
                        AddRegionOverlay(CDesktop.globalRegions["EnemyActionBar" + enemy.actionBars.IndexOf(actionBar)], "Black", 0.1f);
                        animationTime += frameTime;
                        actionBar.cooldown = abilityObj.cooldown;
                        abilityObj.effects(false);
                        board.enemy.DetractResources(abilityObj.cost);
                    });
                }
                else
                {
                    board.actions.Add(() => { cursorEnemy.Move(window.LBRegionGroup.regions[bestMove.y].bigButtons[bestMove.x].transform.position); animationTime += frameTime * 9; });
                    board.actions.Add(() => { cursorEnemy.SetCursor(CursorType.Click); });
                    board.actions.Add(() =>
                    {
                        cursorEnemy.SetCursor(CursorType.Default);
                        var list1 = board.FloodCount(bestMove.x, bestMove.y);
                        board.FloodDestroy(list1);
                        board.enemyFinishedMoving = true;
                    });
                }
            }
        }

        //IF IT's PLAYER'S TURN..
        else
        {
            //IF PLAYER FINISHED MOVING TURN END THEIR TURN
            if (playerFinishedMoving)
            {
                //UNLESS THEY SCORED A BONUS MOVE
                if (bonusTurnStreak > 0)
                {
                    bonusTurnStreak = 0;
                    playerFinishedMoving = false;
                    CDesktop.UnlockScreen();
                }
                else
                    EndTurn();
            }

            //IF PLAYER IS STILL GOING TO MOVE UNLOCK THE SCREEN
            else CDesktop.UnlockScreen();
        }
    }

    public int fieldGetCounterX = 0;
    public int fieldGetCounterY = 0;

    public string GetFieldName(int x, int y) => boardNameDictionary[field[x, y]].ToString();

    public string GetFieldButton()
    {
        var r = boardButtonDictionary[field[fieldGetCounterX, fieldGetCounterY]];
        fieldGetCounterX++;
        if (fieldGetCounterX == field.GetLength(0))
            (fieldGetCounterX, fieldGetCounterY) = (0, fieldGetCounterY + 1);
        if (fieldGetCounterY == field.GetLength(1))
            fieldGetCounterY = 0;
        return r;
    }

    //DESTROYS A SINGLE TARGETED ELEMENT ON THE BOARD FOR THE CURRENT PLAYER
    public void SelectDestroy(int x, int y)
    {
        PlaySound(collectSoundDictionary[field[x, y]].ToString(), 0.3f);
        SpawnShatterElement(1, 0.5, window.LBRegionGroup.regions[y].bigButtons[x].transform.position + new Vector3(-17.5f, -17.5f), boardButtonDictionary[field[x, y]]);
        field[x, y] = 0;
        CDesktop.LockScreen();
    }

    //DESTROYS ALL ELEMENTS OF THE SAME KIND THAT ARE NEARBY OF THE TARGETED ONE
    public void FloodDestroy(List<(int, int, int)> list)
    {
        PlaySound(collectSoundDictionary[list[0].Item3].ToString(), 0.3f);
        if (list.Count > 3)
        {
            bonusTurnStreak++;
            PlaySound("BonusMove" + (bonusTurnStreak > 4 ? 4 : bonusTurnStreak), 0.4f);
        }
        foreach (var a in list)
        {
            SpawnShatterElement(1, 0.5, window.LBRegionGroup.regions[a.Item2].bigButtons[a.Item1].transform.position + new Vector3(-17.5f, -17.5f), boardButtonDictionary[a.Item3]);
            if (playerTurn) GiveResource(player, a.Item1, a.Item2);
            else GiveResource(enemy, a.Item1, a.Item2);
            field[a.Item1, a.Item2] = 0;
        }
        CDesktop.LockScreen();
    }

    public void GiveResource(Entity entity, int x, int y)
    {
        var element = "";
        switch (field[x, y] % 10)
        {
            case 1: element = "Earth"; break;
            case 2: element = "Fire"; break;
            case 3: element = "Water"; break;
            case 4: element = "Air"; break;
            case 5: element = "Lightning"; break;
            case 6: element = "Frost"; break;
            case 7: element = "Decay"; break;
            case 8: element = "Arcane"; break;
            case 9: element = "Order"; break;
            case 0: element = "Shadow"; break;
        }
        entity.resources[element]++;
    }

    public List<(int, int, int)> FloodCount(int x, int y)
    {
        var visited = new List<(int, int)>();
        var positives = new List<(int, int, int)>();
        Flood(x, y);
        return positives;

        void Flood(int i, int j)
        {
            if (visited.Contains((i, j))) return;
            visited.Add((i, j));
            if (field[i, j] != field[x, y] && field[i, j] != field[x, y] - 10 && field[i, j] - 10 != field[x, y] || positives.Contains((i, j, field[i, j]))) return;
            positives.Add((i, j, field[i, j]));
            if (i > 0) Flood(i - 1, j);
            if (j > 0) Flood(i, j - 1);
            if (i < field.GetLength(0) - 1) Flood(i + 1, j);
            if (j < field.GetLength(1) - 1) Flood(i, j + 1);
        }
    }

    public static Dictionary<int, string> boardNameDictionary = new()
    {
        { 00, "" },
        { 01, "Awakened Earth" },
        { 02, "Awakened Fire" },
        { 03, "Awakened Water" },
        { 04, "Awakened Air" },
        { 05, "Awakened Lightning" },
        { 06, "Awakened Frost" },
        { 07, "Awakened Decay" },
        { 08, "Awakened Arcane" },
        { 09, "Awakened Order" },
        { 10, "Awakened Shadow" },
        { 11, "Rousing Earth" },
        { 12, "Rousing Fire" },
        { 13, "Rousing Water" },
        { 14, "Rousing Air" },
        { 15, "Rousing Lightning" },
        { 16, "Rousing Frost" },
        { 17, "Rousing Decay" },
        { 18, "Rousing Arcane" },
        { 19, "Rousing Order" },
        { 20, "Rousing Shadow" },
        { 21, "Soul of Earth" },
        { 22, "Soul of Fire" },
        { 23, "Soul of Water" },
        { 24, "Soul of Air" },
        { 25, "Soul of Lightning" },
        { 26, "Soul of Frost" },
        { 27, "Soul of Decay" },
        { 28, "Soul of Arcane" },
        { 29, "Soul of Order" },
        { 30, "Soul of Shadow" },
    };

    public static Dictionary<int, string> boardButtonDictionary = new()
    {
        { 00, "OtherEmpty" },
        { 01, "ElementEarthAwakened" },
        { 02, "ElementFireAwakened" },
        { 03, "ElementWaterAwakened" },
        { 04, "ElementAirAwakened" },
        { 05, "ElementLightningAwakened" },
        { 06, "ElementFrostAwakened" },
        { 07, "ElementDecayAwakened" },
        { 08, "ElementArcaneAwakened" },
        { 09, "ElementOrderAwakened" },
        { 10, "ElementShadowAwakened" },
        { 11, "ElementEarthRousing" },
        { 12, "ElementFireRousing" },
        { 13, "ElementWaterRousing" },
        { 14, "ElementAirRousing" },
        { 15, "ElementLightningRousing" },
        { 16, "ElementFrostRousing" },
        { 17, "ElementDecayRousing" },
        { 18, "ElementArcaneRousing" },
        { 19, "ElementOrderRousing" },
        { 20, "ElementShadowRousing" },
        { 21, "ElementEarthSoul" },
        { 22, "ElementFireSoul" },
        { 23, "ElementWaterSoul" },
        { 24, "ElementAirSoul" },
        { 25, "ElementLightningSoul" },
        { 26, "ElementFrostSoul" },
        { 27, "ElementDecaySoul" },
        { 28, "ElementArcaneSoul" },
        { 29, "ElementOrderSoul" },
        { 30, "ElementShadowSoul" },
    };

    public static Dictionary<int, string> collectSoundDictionary = new()
    {
        { 00, "" },
        { 01, "ElementEarth" },
        { 02, "ElementFire" },
        { 03, "ElementWater" },
        { 04, "ElementAir" },
        { 05, "ElementLightning" },
        { 06, "ElementFrost" },
        { 07, "ElementDecay" },
        { 08, "ElementArcane" },
        { 09, "ElementOrder" },
        { 10, "ElementShadow" },
        { 11, "ElementEarth" },
        { 12, "ElementFire" },
        { 13, "ElementWater" },
        { 14, "ElementAir" },
        { 15, "ElementLightning" },
        { 16, "ElementFrost" },
        { 17, "ElementDecay" },
        { 18, "ElementArcane" },
        { 19, "ElementOrder" },
        { 20, "ElementShadow" },
        { 21, "ElementEarth" },
        { 22, "ElementFire" },
        { 23, "ElementWater" },
        { 24, "ElementAir" },
        { 25, "ElementLightning" },
        { 26, "ElementFrost" },
        { 27, "ElementDecay" },
        { 28, "ElementArcane" },
        { 29, "ElementOrder" },
        { 30, "ElementShadow" },
    };
}
