using System;
using UnityEngine;
using System.Collections.Generic;

using static Root;
using static SaveGame;
using static FishingBufferBoard;

public class FishingBoard
{
    #region Initialisation

    public FishingBoard(int x, int y, Site site = null)
    {
        field = new int[x, y];
        fisher = currentSave.player;
        fisher.InitialiseFishing();
        this.site = site;
        temporarySpoils = new();
        temporaryBuffs = new();
        actions = new List<Action>();
    }

    public static void NewFishingBoard(Site site)
    {
        fishingBoard = new FishingBoard(6, 6, site);
        fishingBufferBoard = new FishingBufferBoard();
    }

    #endregion

    //STATIC REFERENCE TO THE BOARD
    //THERE CAN BE ONLY ONE AT A TIME THANKS TO STATIC REF
    public static FishingBoard fishingBoard;

    //Array of all elements on the board
    public int[,] field;

    //Reference to the window that contains the drawn board
    public Window window;

    //Fisher reference
    public Entity fisher;

    //Health bars for player and the enemy
    //public Dictionary<string, HealthBar> healthBars;

    //List of all flying elements that are docking in the fisher spoils region
    public List<GameObject> temporarySpoils;

    //List of all flying buffs that are docking in the player buff region
    public List<GameObject> temporaryBuffs;

    //Queue of actions to do on the board and the combatants
    public List<Action> actions;

    //Site where the player is fishing
    public Site site;

    //RESETS THE BOARD TO BE EMPTY AND REFILLED AGAIN
    public void Reset()
    {
        field = new int[field.GetLength(0), field.GetLength(1)];
        for (int i = 0; i < field.GetLength(0); i++)
            for (int j = 0; j < field.GetLength(1); j++)
                field[i, j] = -1;
        CDesktop.LockScreen();
    }

    public void EndFishing(string result)
    {

    }

    public void AnimateBoard()
    {
        //MOVE ELEMENTS DOWN WITH GRAVITY
        for (int j = field.GetLength(1) - 2; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] != -1)
                {
                    var empty = 0;
                    for (int q = 0; q + j < field.GetLength(1); q++)
                        if (field[i, j + q] == 0) empty++;
                    (field[i, j], field[i, j + empty]) = (-1, field[i, j]);
                    if (empty > 0) window.LBRegionGroup.regions[j].bigButtons[i].gameObject.AddComponent<FallingElement>().Initiate(empty);
                }

        //IF BOARD IS NOT YET FULL RETURN AND DO PREVIOUS STEPS AGAIN
        for (int j = field.GetLength(1) - 1; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == -1)
                {
                    fishingBufferBoard.FillBoard(field);
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
        //for (int j = 0; j < field.GetLength(1); j++)
        //    for (int i = 0; i < field.GetLength(0); i++)
        //    {
        //        var list = FloodCount(i, j);
        //        if (list.Count >= 3)
        //        {
        //            if (!breakForCascade)
        //            {
        //                FloodDestroy(list);
        //                breakForCascade = true;
        //            }
        //            else
        //            {
        //                breakForCascade = false;
        //                animationTime += defines.frameTime * 2;
        //            }
        //            return;
        //        }
        //    }

        ////IF PLAYER DIED..
        //if (player.health <= 0 && window.desktop.title == "Game")
        //    EndCombat("Lost");

        ////IF ENEMY DIED..
        //else if (enemy.health <= 0 && window.desktop.title == "Game")
        //    EndCombat("Won");

        //Unlock the screen so the fishing may continue
        canUnlockScreen = true;
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

    //DESTROYS ALL ELEMENTS OF THE SAME KIND THAT ARE NEARBY OF THE TARGETED ONE
    public void FloodDestroy(List<(int, int, int)> list)
    {
        //PlaySound(collectSoundDictionary[list[0].Item3].ToString(), 0.3f);
        //if (list.Count > 3)
        //{
        //    bonusTurnStreak++;
        //    PlaySound("BonusMove" + (bonusTurnStreak > 4 ? 4 : bonusTurnStreak), 0.4f);
        //    SpawnFallingText(new Vector2(0, 14), "Bonus Move", "White");
        //}
        //var types = list.Select(x => x.Item3).Distinct();
        //var foo = types.ToDictionary(x => Resource(x), x => list.Sum(y => y.Item3 == x ? 1 : 0));
        //foreach (var a in list)
        //{
        //    SpawnFlyingElement(1, 9, window.LBRegionGroup.regions[a.Item2].bigButtons[a.Item1].transform.position + new Vector3(-17.5f, -17.5f), boardButtonDictionary[a.Item3], board.playerTurn);
        //    field[a.Item1, a.Item2] = 0;
        //}
        //if (playerTurn) player.AddResources(foo);
        //else enemy.AddResources(foo);
        //bufferBoard.Reset();
        //CDesktop.LockScreen();
    }

    public string Resource(int id)
    {
        if (id == 11) return "Earth";
        else if (id == 12) return "Fire";
        else if (id == 13) return "Water";
        else if (id == 14) return "Air";
        else if (id == 15) return "Lightning";
        else if (id == 16) return "Frost";
        else if (id == 17) return "Decay";
        else if (id == 18) return "Arcane";
        else if (id == 19) return "Order";
        else if (id == 20) return "Shadow";
        else return "None";
    }

    public int ResourceReverse(string element)
    {
        if (element == "Earth") return 11;
        else if (element == "Fire") return 12;
        else if (element == "Water") return 13;
        else if (element == "Air") return 14;
        else if (element == "Lightning") return 15;
        else if (element == "Frost") return 16;
        else if (element == "Decay") return 17;
        else if (element == "Arcane") return 18;
        else if (element == "Order") return 19;
        else if (element == "Shadow") return 20;
        else return 0;
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
