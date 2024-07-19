using System.Collections.Generic;

using static Root;

public class BufferBoard
{
    public BufferBoard()
    {
        fallingElements = new();
        field = new int[Board.board.field.GetLength(0), Board.board.field.GetLength(1)];
        Generate(true);
    }

    //Resets the buffer board to be later filled
    //with items to fill empty slots in the board
    public void Reset()
    {
        field = new int[Board.board.field.GetLength(0), Board.board.field.GetLength(1)];
        for (int i = 0; i < field.GetLength(0); i++)
            for (int j = 0; j < field.GetLength(1); j++)
                field[i, j] = -1;
    }

    //Generates elements on the buffer board that will
    //then fall down into the board to fill empty spaces
    public void Generate(bool noCascades = false)
    {
        Reset();
        for (int i = 0; i < field.GetLength(0); i++)
        {
            var column = new List<int>();
            for (int j = 0; j < field.GetLength(1); j++)
                if (Board.board.field[i, j] == -1)
                {
                    int newElement;
                    do newElement = random.Next(0, 20);
                    while (noCascades && ((j > 0 && column[j - 1] % 10 == newElement % 10) || (i > 0 && field[i - 1, field.GetLength(1) - 1 - j] % 10 == newElement % 10)));
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

    //Fills the empty spaces in a specified field
    //with this buffer board generated beforehand
    public void FillBoard(int[,] field)
    {
        var delay = 0;
        for (int i = 0; i < field.GetLength(0); i++)
        {
            var empty = 0;
            for (int q = 0; q < field.GetLength(1); q++)
                if (field[i, q] == -1) empty++;
            if (empty > 0) delay++;
            for (int j = 0; j < empty; j++)
            {
                field[i, j] = this.field[i, j + field.GetLength(1) - empty];
                window.LBRegionGroup.regions[j + field.GetLength(1) - empty].bigButtons[i].gameObject.AddComponent<FallingElement>().Initiate(empty, delay);
            }
        }
    }

    public static BufferBoard bufferBoard;

    public static Dictionary<int, string> bufferBoardButtonDictionary = new()
    {
        { -1, null },
        { 00, "ElementShadowRousing" },
        { 01, "ElementEarthRousing" },
        { 02, "ElementFireRousing" },
        { 03, "ElementWaterRousing" },
        { 04, "ElementAirRousing" },
        { 05, "ElementLightningRousing" },
        { 06, "ElementFrostRousing" },
        { 07, "ElementDecayRousing" },
        { 08, "ElementArcaneRousing" },
        { 09, "ElementOrderRousing" },
        { 10, "ElementShadowAwakened" },
        { 11, "ElementEarthAwakened" },
        { 12, "ElementFireAwakened" },
        { 13, "ElementWaterAwakened" },
        { 14, "ElementAirAwakened" },
        { 15, "ElementLightningAwakened" },
        { 16, "ElementFrostAwakened" },
        { 17, "ElementDecayAwakened" },
        { 18, "ElementArcaneAwakened" },
        { 19, "ElementOrderAwakened" },
        { 20, "ElementShadowSoul" },
        { 21, "ElementEarthSoul" },
        { 22, "ElementFireSoul" },
        { 23, "ElementWaterSoul" },
        { 24, "ElementAirSoul" },
        { 25, "ElementLightningSoul" },
        { 26, "ElementFrostSoul" },
        { 27, "ElementDecaySoul" },
        { 28, "ElementArcaneSoul" },
        { 29, "ElementOrderSoul" },
    };

    public int[,] field;
    public Window window;
}