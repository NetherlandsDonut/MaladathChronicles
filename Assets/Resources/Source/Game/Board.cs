using System.Linq;
using System.Collections.Generic;

using static Race;
using static Root;
using static Root.Color;
using static Root.SoundEffects;

public class Board
{
    public Board(int x, int y, string enemy)
    {
        field = new int[x, y];
        player = saveGames[0].player;
        this.enemy = new Entity(races.Find(x => x.name == enemy));
        playerTurn = true;
    }

    public static Board board;

    public int[,] field;
    public Window window;
    public Entity player, enemy;
    public bool playerTurn;

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
        if (player.health <= 0)
        {
            SwitchDesktop("TitleScreen");
            CloseDesktop("Game");
            CloseDesktop("Map");
        }
        else if (enemy.health <= 0)
        {
            SwitchDesktop("Map");
            CloseDesktop("Game");
        }
        else if (playerTurn)
            CDesktop.UnlockScreen();
        else
        {
            //animationTime = 3f;
            var list = board.FloodCount(random.Next(0, 8), random.Next(0, 8));
            player.health -= list.Count;
            board.FloodDestroy(list);
            if (list.Count < 4)
                playerTurn = true;
        }
    }

    public int fieldGetCounterX = 0;
    public int fieldGetCounterY = 0;

    public string GetFieldName(int x, int y) => boardNameDictionary[field[x, y]].ToString();
    public Color GetFieldColor(int x, int y) => boardColorDictionary[field[x, y]];

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

    public void FloodDestroy(List<(int, int, int)> list)
    {
        window.PlaySound(collectSoundDictionary[list[0].Item3].ToString(), 0.3f);
        foreach (var a in list)
            if (a == list[0] && list.Count >= 4 && a.Item3 > 10)
                field[a.Item1, a.Item2] -= 10;
            else field[a.Item1, a.Item2] = 0;
        StartAnimationFill();
    }

    public void FloodDestroy(int x, int y)
    {
        window.PlaySound(collectSoundDictionary[field[x, y]].ToString(), 0.3f);
        foreach (var a in FloodCount(x, y))
            field[a.Item1, a.Item2] = 0;
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
            if (field[i, j] != field[x, y] || positives.Contains((i, j, field[i, j]))) return;
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
