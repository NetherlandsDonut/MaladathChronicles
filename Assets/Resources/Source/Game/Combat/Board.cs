using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static BufferBoard;

public class Board
{
    public Board(int x, int y, Entity enemy, SiteHostileArea area = null)
    {
        bonusTurnStreak = 0;
        field = new int[x, y];
        player = currentSave.player;
        player.Initialise();
        this.enemy = enemy;
        playerTurn = true;
        this.area = area;
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
    public SiteHostileArea area;

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

    public void EndCombat(bool playerWon)
    {
        cursorEnemy.fadeOut = true;
        CloseDesktop("Game");
        if (playerWon)
        {
            if (currentSave.player.WillGetExperience(enemy.level))
                currentSave.player.ReceiveExperience(currentSave.player.ExperienceNeeded());
            if (area != null)
            {
                if (!currentSave.siteProgress.ContainsKey(area.name))
                    currentSave.siteProgress.Add(area.name, 1);
                else currentSave.siteProgress[area.name]++;
            }
            if (area != null && !area.instancePart)
            {
                SwitchDesktop("HostileAreaEntrance");
                CDesktop.Rebuild();
            }
            else if (area != null && area.instancePart)
            {
                SwitchDesktop("DungeonEntrance");
                CDesktop.Rebuild();
            }
            else SwitchDesktop("Map");
        }
        else
        {
            if (currentSave.hardcore) SpawnDesktopBlueprint("GameOver");
            else SpawnDesktopBlueprint("ReleaseSpirit");
        }
    }

    public void AnimateBoard()
    {
        //MOVE ELEMENTS DOWN WITH GRAVITY
        for (int j = field.GetLength(1) - 2; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] != 0)
                {
                    var zeroes = 0;
                    for (int q = 0; q + j < field.GetLength(1); q++)
                        if (field[i, j + q] == 0) zeroes++;
                    (field[i, j], field[i, j + zeroes]) = (0, field[i, j]);
                    if (zeroes > 0) window.LBRegionGroup.regions[j].bigButtons[i].gameObject.AddComponent<FallingElement>().Initiate(zeroes);
                }

        //IF BOARD IS NOT YET FULL RETURN AND DO PREVIOUS STEPS AGAIN
        for (int j = field.GetLength(1) - 1; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == 0)
                {
                    bufferBoard.FillBoard(field);
                    return;
                }

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

        //IF PLAYER DIED..
        if (player.health <= 0)
            EndCombat(false);

        //IF ENEMY DIED..
        else if (enemy.health <= 0)
            EndCombat(true);

        //IF IT'S ENEMY'S TURN..
        else if (!playerTurn)
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

                //CALCULATE SOLVINGS
                var firstLayerBase = new FutureBoard(board);
                var firstLayer = firstLayerBase.CalculateLayer().OrderByDescending(x => x.MaxDesiredness(firstLayerBase)).ToList();
                firstLayerBase.enemyFinishedMoving = true;
                var baseDesiredness = firstLayerBase.Desiredness();
                firstLayerBase.enemyFinishedMoving = false;
                var currentLayer = firstLayer;
                firstLayer.ForEach(x => x.depth = aiDepth);
                while (true)
                {
                    foreach (var solving in currentLayer)
                    {
                        solving.possibleSolves = solving.board.CalculateLayer();
                        foreach (var innerSolving in solving.possibleSolves)
                            if (innerSolving.depth > -1)
                                innerSolving.depth = innerSolving.board.player.health == 0 || innerSolving.board.enemy.health == 0 ? 0 : solving.depth - (solving.board.TurnEnded() ? 1 : 0);
                    }
                    currentLayer = currentLayer.SelectMany(x => x.possibleSolves.FindAll(y => y.depth > 0)).ToList();
                    if (currentLayer.Count == 0) break;
                }
                var results = new List<(FutureMove, double)>();
                foreach (var solving in firstLayer)
                    results.Add((solving, solving.MaxDesiredness(firstLayerBase, baseDesiredness)));
                results = results.OrderByDescending(x => x.Item2).ToList();

                var message = "";
                foreach (var solving1 in firstLayer)
                {
                    message += (solving1.board.playerTurn ? "P: " : "E: ") + solving1.desiredness.ToString("0.000") + " " + (solving1.x != -1 ? "(" + solving1.x + ", " + solving1.y + ") (" + boardNameDictionary[firstLayerBase.field[solving1.x, solving1.y]] + ")" : "") + (solving1.ability == "" ? "" : "<" + solving1.ability + ">") + "\n";
                    if (solving1.possibleSolves.Count > 0) foreach (var solving2 in solving1.possibleSolves)
                        {
                            message += "   " + (solving2.board.playerTurn ? "P: " : "E: ") + solving2.desiredness.ToString("0.000") + " " + (solving2.x != -1 ? "(" + solving2.x + ", " + solving2.y + ") (" + boardNameDictionary[solving1.board.field[solving2.x, solving2.y]] + ")" : "") + (solving2.ability == "" ? "" : "<" + solving2.ability + ">") + "\n";
                            if (solving2.possibleSolves.Count > 0) foreach (var solving3 in solving2.possibleSolves)
                                {
                                    message += "      " + (solving3.board.playerTurn ? "P: " : "E: ") + solving3.desiredness.ToString("0.000") + " " + (solving3.x != -1 ? "(" + solving3.x + ", " + solving3.y + ") (" + boardNameDictionary[solving2.board.field[solving3.x, solving3.y]] + ")" : "") + (solving3.ability == "" ? "" : "<" + solving3.ability + ">") + "\n";
                                    if (solving3.possibleSolves.Count > 0) foreach (var solving4 in solving3.possibleSolves)
                                        {
                                            message += "         " + (solving4.board.playerTurn ? "P: " : "E: ") + solving4.desiredness.ToString("0.000") + " " + (solving4.x != -1 ? "(" + solving4.x + ", " + solving4.y + ") (" + boardNameDictionary[solving3.board.field[solving4.x, solving4.y]] + ")" : "") + (solving4.ability == "" ? "" : "<" + solving4.ability + ">") + "\n";
                                            if (solving4.possibleSolves.Count > 0) foreach (var solving5 in solving4.possibleSolves)
                                                {
                                                    message += "            " + (solving5.board.playerTurn ? "P: " : "E: ") + solving5.desiredness.ToString("0.000") + " " + (solving5.x != -1 ? "(" + solving5.x + ", " + solving5.y + ") (" + boardNameDictionary[solving4.board.field[solving5.x, solving5.y]] + ")" : "") + (solving5.ability == "" ? "" : "<" + solving5.ability + ">") + "\n";
                                                    if (solving5.possibleSolves.Count > 0) foreach (var solving6 in solving5.possibleSolves)
                                                        {
                                                            message += "               " + (solving6.board.playerTurn ? "P: " : "E: ") + solving6.desiredness.ToString("0.000") + " " + (solving6.x != -1 ? "(" + solving6.x + ", " + solving6.y + ") (" + boardNameDictionary[solving5.board.field[solving6.x, solving6.y]] + ")" : "") + (solving6.ability == "" ? "" : "<" + solving6.ability + ">") + "\n";
                                                            if (solving6.possibleSolves.Count > 0) foreach (var solving7 in solving6.possibleSolves)
                                                                {
                                                                    message += "                  " + (solving7.board.playerTurn ? "P: " : "E: ") + solving7.desiredness.ToString("0.000") + " " + (solving7.x != -1 ? "(" + solving7.x + ", " + solving7.y + ") (" + boardNameDictionary[solving6.board.field[solving7.x, solving7.y]] + ")" : "") + (solving7.ability == "" ? "" : "<" + solving7.ability + ">") + "\n";
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                }
                //File.WriteAllText("asd.txt", message);
                Debug.Log(message);

                //EXECUTE
                var bestMove = results[0].Item1;
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
                    canUnlockScreen = true;
                }
                else
                    EndTurn();
            }

            //IF PLAYER IS STILL GOING TO MOVE UNLOCK THE SCREEN
            else canUnlockScreen = true;
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
        bufferBoard.Reset();
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
        bufferBoard.Reset();
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
        { 00, "Empty" },
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
        { 00, null },
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
