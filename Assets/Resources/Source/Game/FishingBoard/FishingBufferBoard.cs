using System.Collections.Generic;

using static Root;

public class FishingBufferBoard
{
    public FishingBufferBoard()
    {
        fallingElements = new();
        field = new int[FishingBoard.fishingBoard.field.GetLength(0), FishingBoard.fishingBoard.field.GetLength(1)];
        Reset();
    }

    public void Reset()
    {
        field = new int[FishingBoard.fishingBoard.field.GetLength(0), FishingBoard.fishingBoard.field.GetLength(1)];
        for (int i = 0; i < field.GetLength(0); i++)
        {
            var column = new List<int>();
            for (int j = 0; j < field.GetLength(1); j++)
                if (FishingBoard.fishingBoard.field[i, j] == -1)
                {
                    int newElement;
                    newElement = 11;
                    column.Add(newElement);
                }
            for (int j = 0; j < column.Count; j++)
                field[i, field.GetLength(1) - 1 - j] = column[j];
        }
    }

    public int fieldGetCounterX = 0;
    public int fieldGetCounterY = 0;

    public string GetFieldButton()
    {
        var r = bufferBoardButtonDictionary[field[fieldGetCounterX, fieldGetCounterY]];
        fieldGetCounterX++;
        if (fieldGetCounterX == field.GetLength(0))
            (fieldGetCounterX, fieldGetCounterY) = (0, fieldGetCounterY + 1);
        if (fieldGetCounterY == field.GetLength(1))
            fieldGetCounterY = 0;
        return r;
    }

    public void FillBoard(int[,] field)
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            var zeroes = 0;
            for (int q = 0; q < field.GetLength(1); q++)
                if (field[i, q] == 0) zeroes++;
            for (int j = 0; j < zeroes; j++)
            {
                field[i, j] = this.field[i, j + field.GetLength(1) - zeroes];
                window.LBRegionGroup.regions[j + field.GetLength(1) - zeroes].bigButtons[i].gameObject.AddComponent<FallingElement>().Initiate(zeroes);
            }
        }
    }

    public static FishingBufferBoard fishingBufferBoard;

    public static Dictionary<int, string> bufferBoardButtonDictionary = new()
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

    public int[,] field;
    public Window window;
}