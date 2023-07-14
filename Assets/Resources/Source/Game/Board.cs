using UnityEngine;
using System.Collections.Generic;

using static Race;
using static Root;
using static Root.Color;

public class Board
{
    public Board(int x, int y, string enemy)
    {
        bonusTurnStreak = 0;
        field = new int[x, y];
        player = currentSave.player;
        this.enemy = new Entity(races.Find(x => x.name == enemy));
        playerTurn = true;
    }

    public static Board board;

    public int bonusTurnStreak;
    public int[,] field;
    public Window window;
    public Entity player, enemy;
    public bool playerTurn, breakForEnemy, breakForCascade, enemyFinishedMoving;

    public void Reset()
    {
        field = new int[field.GetLength(0), field.GetLength(1)];
        for (int i = 0; i < field.GetLength(0); i++)
            for (int j = 0; j < field.GetLength(1); j++)
                field[i, j] = 0;
        StartAnimationFill();
    }

    public int animFrame;
    public void StartAnimationFill()
    {
        CDesktop.LockScreen();
    }

    public void AnimateFill()
    {
        for (int j = field.GetLength(1) - 1; j > 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == 0 && field[i, j - 1] != 0)
                {
                    (field[i, j], field[i, j - 1]) = (field[i, j - 1], 0);
                    for (int k = 0; k < field.GetLength(0); k++)
                        if (field[k, j] == 0) break;
                        else if (k == field.GetLength(0) - 1)
                            CDesktop.FocusedWindow().PlaySound("PutDownSmallWood", 0.04f);
                }
        for (int i = 0; i < field.GetLength(0); i++)
            if (field[i, 0] == 0)
                field[i, 0] = random.Next(11, 21);
        for (int j = field.GetLength(1) - 1; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == 0) return;
        for (int j = field.GetLength(1) - 1; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
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
        if (enemyFinishedMoving)
        {
            playerTurn = true;
            enemyFinishedMoving = false;
        }
        if (playerTurn)
            if (bonusTurnStreak != 0)
            {
                bonusTurnStreak = 0;
                CDesktop.UnlockScreen();
            }
            else playerTurn = false;
        else if (breakForEnemy)
        {
            breakForEnemy = false;
            bonusTurnStreak = 0;
            var list = board.FloodCount(random.Next(0, 8), random.Next(0, 8));
            board.FloodDestroy(list);
            if (bonusTurnStreak == 0)
            {
                bonusTurnStreak = -1;
                enemyFinishedMoving = true;
            }
        }
        else
        {
            animationTime = (float)(random.Next(4, 8) / 10.0) + 0.3f;
            breakForEnemy = true;
        }
    }

    public int fieldGetCounterX = 0;
    public int fieldGetCounterY = 0;

    public string GetFieldName(int x, int y) => boardNameDictionary[field[x, y]].ToString();
    public Root.Color GetFieldColor(int x, int y) => boardColorDictionary[field[x, y]];

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

    public void SelectDestroy(int x, int y)
    {
        window.PlaySound(collectSoundDictionary[field[x, y]].ToString(), 0.3f);
        SpawnShatter(window.LBRegionGroup.regions[y].bigButtons[x].transform.position + new Vector3(-17.5f, -17.5f), boardButtonDictionary[field[x, y]]);
        field[x, y] = 0;
        StartAnimationFill();
    }

    public void FloodDestroy(List<(int, int, int)> list)
    {
        window.PlaySound(collectSoundDictionary[list[0].Item3].ToString(), 0.3f);
        if (list.Count > 3)
        {
            bonusTurnStreak++;
            window.PlaySound("BonusMove" + (bonusTurnStreak > 4 ? 4 : bonusTurnStreak), 0.4f);
        }
        //var rand = random.Next(0, list.Count);
        foreach (var a in list)
        {
            SpawnShatter(window.LBRegionGroup.regions[a.Item2].bigButtons[a.Item1].transform.position + new Vector3(-17.5f, -17.5f), boardButtonDictionary[a.Item3]);
            //if (a == list[rand] && list.Count >= 4)
            //    field[a.Item1, a.Item2] -= list.Exists(x => x.Item3 < 11) ? -10 : 10;
            //else if (a.Item3 > 20)
            //{
            //    field[a.Item1 - 1, a.Item2] = a.Item3 % 20 + field[a.Item1 - 1, a.Item2] - field[a.Item1 - 1, a.Item2] % 10;
            //    field[a.Item1 + 1, a.Item2] = a.Item3 % 20 + field[a.Item1 - 1, a.Item2] - field[a.Item1 - 1, a.Item2] % 10;
            //    field[a.Item1, a.Item2 - 1] = a.Item3 % 20 + field[a.Item1 - 1, a.Item2] - field[a.Item1 - 1, a.Item2] % 10;
            //    field[a.Item1, a.Item2 + 1] = a.Item3 % 20 + field[a.Item1 - 1, a.Item2] - field[a.Item1 - 1, a.Item2] % 10;
            //    field[a.Item1 + 1, a.Item2 + 1] = a.Item3 % 20 + field[a.Item1 - 1, a.Item2] - field[a.Item1 - 1, a.Item2] % 10;
            //    field[a.Item1 + 1, a.Item2 - 1] = a.Item3 % 20 + field[a.Item1 - 1, a.Item2] - field[a.Item1 - 1, a.Item2] % 10;
            //    field[a.Item1 - 1, a.Item2 - 1] = a.Item3 % 20 + field[a.Item1 - 1, a.Item2] - field[a.Item1 - 1, a.Item2] % 10;
            //    field[a.Item1 - 1, a.Item2 + 1] = a.Item3 % 20 + field[a.Item1 - 1, a.Item2 + 1] - field[a.Item1 - 1, a.Item2 + 1] % 10;
            //    Foo(a.Item1 - 1, a.Item2 + 1);

            //    void Foo(int q, int w) => field[q, w] = a.Item3 % 20 + field[q, w] - field[q, w] % 10;
            //}
            //else
            field[a.Item1, a.Item2] = 0;
        }
        StartAnimationFill();
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
            if (field[x, y] > 20 && (x != i || y != j) || field[i, j] > 20 || field[i, j] != field[x, y] && field[i, j] != field[x, y] - 10 && field[i, j] - 10 != field[x, y] || positives.Contains((i, j, field[i, j]))) return;
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

    public static Dictionary<int, Root.Color> boardColorDictionary = new()
    {
        { 00, Black },
        { 01, LightGray },
        { 02, LightGray },
        { 03, LightGray },
        { 04, LightGray },
        { 05, LightGray },
        { 06, LightGray },
        { 07, LightGray },
        { 08, LightGray },
        { 09, LightGray },
        { 10, LightGray },
        { 11, LightGray },
        { 12, LightGray },
        { 13, LightGray },
        { 14, LightGray },
        { 15, LightGray },
        { 16, LightGray },
        { 17, LightGray },
        { 18, LightGray },
        { 19, LightGray },
        { 20, LightGray },
        { 21, LightGray },
        { 22, LightGray },
        { 23, LightGray },
        { 24, LightGray },
        { 25, LightGray },
        { 26, LightGray },
        { 27, LightGray },
        { 28, LightGray },
        { 29, LightGray },
        { 30, LightGray },
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
