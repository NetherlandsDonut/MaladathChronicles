using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Color;
using static Root.SoundEffects;
using static Root.BigButtonTypes;

public class Board
{
    public Board(int x, int y)
    {
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
                        for (int k = 0; k < field.GetLength(1); k++)
                            if (field[k, j] == 0) break;
                            else if (k == field.GetLength(1) - 1)
                                CDesktop.FocusedWindow().PlaySound("GemFall", 0.04f);
                    }
        for (int i = 0; i < field.GetLength(0); i++)
            if (field[i, 0] == 0)
                field[i, 0] = random.Next(1, 4);
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
        { 0, "" },
        { 1, "Copper Coins" },
        { 2, "Silver Coins" },
        { 3, "Gold Coins" },
        { 4, "Skulls" },
    };

    public static Dictionary<int, Color> boardColorDictionary = new()
    {
        { 0, Black },
        { 1, Copper },
        { 2, Silver },
        { 3, Gold },
        { 4, LightGray },
    };

    public static Dictionary<int, BigButtonTypes> boardButtonDictionary = new()
    {
        { 0, Empty },
        { 1, CopperCoins },
        { 2, SilverCoins },
        { 3, GoldCoins },
        { 4, Skulls },
    };

    public static Dictionary<int, SoundEffects> collectSoundDictionary = new()
    {
        { 0, None },
        { 1, Coins },
        { 2, Coins },
        { 3, Coins },
        { 4, None },
    };
}
