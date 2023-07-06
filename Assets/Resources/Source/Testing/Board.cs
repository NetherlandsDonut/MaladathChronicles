using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Color;
using static Root.SoundEffects;
using static Root.BigButtonTypes;
using static UnityEngine.Tilemaps.TilemapRenderer;

public class Board
{
    public Board(int x, int y)
    {
        //THERE ARE SOME ISSUES WITH THE BOARD NOT BEING SQUARE
        field = new int[x, y];
    }

    public static Board board;

    public int[,] field;

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
        animFrame = 0;
        CDesktop.LockScreen();
    }

    public void AnimateFill()
    {
        if (animFrame > 0)
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
        animFrame++;
        for (int j = field.GetLength(1) - 1; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == 0) return;
        CDesktop.UnlockScreen();
    }

    public int fieldGetCounterX = 0;
    public int fieldGetCounterY = 0;

    public string GetFieldName(int x, int y) => boardNameDictionary[field[x, y]].ToString();
    public Color GetFieldColor(int x, int y) => boardColorDictionary[field[x, y]];

    public BigButtonTypes GetFieldButton()
    {
        var r = boardButtonDictionary[field[fieldGetCounterX, fieldGetCounterY]];
        fieldGetCounterX++;
        if (fieldGetCounterX == field.GetLength(0))
            (fieldGetCounterX, fieldGetCounterY) = (0, fieldGetCounterY + 1);
        if (fieldGetCounterY == field.GetLength(1))
            fieldGetCounterY = 0;
        return r;
    }

    public void FloodDestroy(Window window, List<(int, int)> list)
    {
        window.PlaySound(collectSoundDictionary[field[list[0].Item1, list[0].Item2]].ToString(), 0.3f);
        foreach (var a in list)
            field[a.Item1, a.Item2] = 0;
        StartAnimationFill();
    }

    public void FloodDestroy(Window window, int x, int y)
    {
        window.PlaySound(collectSoundDictionary[field[x, y]].ToString(), 0.3f);
        foreach (var a in FloodCount(x, y))
            field[a.Item1, a.Item2] = 0;
        StartAnimationFill();
    }

    public List<(int, int)> FloodCount(int x, int y)
    {
        var visited = new List<(int, int)>();
        var positives = new List<(int, int)>();
        Flood(x, y);
        return positives;

        void Flood(int i, int j)
        {
            if (visited.Contains((i, j))) return;
            visited.Add((i, j));
            if (field[i, j] != field[x, y] || positives.Contains((i, j))) return;
            positives.Add((i, j));
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

    public static Dictionary<int, Color> boardColorDictionary = new()
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

    public static Dictionary<int, BigButtonTypes> boardButtonDictionary = new()
    {
        { 00, Empty },
        { 01, AwakenedEarth },
        { 02, AwakenedFire },
        { 03, AwakenedWater },
        { 04, AwakenedAir },
        { 05, AwakenedLightning },
        { 06, AwakenedFrost },
        { 07, AwakenedDecay },
        { 08, AwakenedArcane },
        { 09, AwakenedOrder },
        { 10, AwakenedShadow },
        { 11, RousingEarth },
        { 12, RousingFire },
        { 13, RousingWater },
        { 14, RousingAir },
        { 15, RousingLightning },
        { 16, RousingFrost },
        { 17, RousingDecay },
        { 18, RousingArcane },
        { 19, RousingOrder },
        { 20, RousingShadow },
        { 21, SoulOfEarth },
        { 22, SoulOfFire },
        { 23, SoulOfWater },
        { 24, SoulOfAir },
        { 25, SoulOfLightning },
        { 26, SoulOfFrost },
        { 27, SoulOfDecay },
        { 28, SoulOfArcane },
        { 29, SoulOfOrder },
        { 30, SoulOfShadow },
    };

    public static Dictionary<BigButtonTypes, string> boardButtonPathDictionary = new()
    {
        { Empty, "Empty" },
        { AwakenedEarth, "Elements/Earth/AwakenedEarth" },
        { AwakenedFire, "Elements/Fire/AwakenedFire" },
        { AwakenedWater, "Elements/Water/AwakenedWater" },
        { AwakenedAir, "Elements/Air/AwakenedAir" },
        { AwakenedLightning, "Elements/Lightning/AwakenedLightning" },
        { AwakenedFrost, "Elements/Frost/AwakenedFrost" },
        { AwakenedDecay, "Elements/Decay/AwakenedDecay" },
        { AwakenedArcane, "Elements/Arcane/AwakenedArcane" },
        { AwakenedOrder, "Elements/Order/AwakenedOrder" },
        { AwakenedShadow, "Elements/Shadow/AwakenedShadow" },
        { RousingEarth, "Elements/Earth/RousingEarth" },
        { RousingFire, "Elements/Fire/RousingFire" },
        { RousingWater, "Elements/Water/RousingWater" },
        { RousingAir, "Elements/Air/RousingAir" },
        { RousingLightning, "Elements/Lightning/RousingLightning" },
        { RousingFrost, "Elements/Frost/RousingFrost" },
        { RousingDecay, "Elements/Decay/RousingDecay" },
        { RousingArcane, "Elements/Arcane/RousingArcane" },
        { RousingOrder, "Elements/Order/RousingOrder" },
        { RousingShadow, "Elements/Shadow/RousingShadow" },
        { SoulOfEarth, "Elements/Earth/SoulEarth" },
        { SoulOfFire, "Elements/Fire/SoulFire" },
        { SoulOfWater, "Elements/Water/SoulWater" },
        { SoulOfAir, "Elements/Air/SoulAir" },
        { SoulOfLightning, "Elements/Lightning/SoulLightning" },
        { SoulOfFrost, "Elements/Frost/SoulFrost" },
        { SoulOfDecay, "Elements/Decay/SoulDecay" },
        { SoulOfArcane, "Elements/Arcane/SoulArcane" },
        { SoulOfOrder, "Elements/Order/SoulOrder" },
        { SoulOfShadow, "Elements/Shadow/SoulShadow" },
    };

    public static Dictionary<int, SoundEffects> collectSoundDictionary = new()
    {
        { 00, None },
        { 01, PickUpRocks },
        { 02, PickUpRocks },
        { 03, PickUpRocks },
        { 04, PickUpRocks },
        { 05, PickUpRocks },
        { 06, PickUpRocks },
        { 07, PickUpRocks },
        { 08, PickUpRocks },
        { 09, PickUpRocks },
        { 10, PickUpRocks },
        { 11, PickUpRocks },
        { 12, PickUpRocks },
        { 13, PickUpRocks },
        { 14, PickUpRocks },
        { 15, PickUpRocks },
        { 16, PickUpRocks },
        { 17, PickUpRocks },
        { 18, PickUpRocks },
        { 19, PickUpRocks },
        { 20, PickUpRocks },
        { 21, PickUpRocks },
        { 22, PickUpRocks },
        { 23, PickUpRocks },
        { 24, PickUpRocks },
        { 25, PickUpRocks },
        { 26, PickUpRocks },
        { 27, PickUpRocks },
        { 28, PickUpRocks },
        { 29, PickUpRocks },
        { 30, PickUpRocks },
    };
}
