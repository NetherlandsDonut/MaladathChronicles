using System.Collections.Generic;

using static Root;
using static FallingElement;

public class BufferBoard
{
    public BufferBoard()
    {
        fallingElements = new();
        field = new int[Board.board.field.GetLength(0), Board.board.field.GetLength(1)];
        Generate("Illegal");
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
    public void Generate(string cascading = "Allowed")
    {
        //Reset the buffer board
        Reset();

        //Generate new elements based on the amount of lacking elements on the board
        for (int i = 0; i < field.GetLength(0); i++)
        {
            var alreadyAdded = 0;
            for (int j = 0; j < field.GetLength(1); j++)
                if (Board.board.field[i, j] == -1)
                    field[i, field.GetLength(1) - 1 - alreadyAdded++] = random.Next(0, 20);
        }

        //Illegal First Row means that only one row of new elements is added
        //onto the buffer. To avoid making any unnessecary operations we are
        //checking only the highest elements on the board for cascading.
        //The fact that we are adding one row means that only one full row is lacking.
        //Thanks to this we know that we don't have to check sides as everything is flat.
        if (cascading == "IllegalFirstRow")
            for (int i = 0; i < field.GetLength(0); i++)
            {
                var L = field.GetLength(1) - 1;
                if (field[i, L] != -1)
                {
                    var topInBoard = -1;
                    for (int j = 0; j < field.GetLength(1); j++)
                        if (Board.board.field[i, j] != -1)
                        {
                            topInBoard = Board.board.field[i, j];
                            break;
                        }
                    while (i > 0 && field[i, L] % 10 == field[i - 1, L] % 10 || field[i, L] % 10 == topInBoard % 10)
                        field[i, L] = random.Next(0, 20);
                }
            }

        //Illegal means that all cascading is illegal.
        //This is used only on the full board reset.
        else if (cascading == "Illegal")
            for (int i = 0; i < field.GetLength(0); i++)
                for (int j = 0; j < field.GetLength(1); j++)
                    if (field[i, j] != -1)
                        while (i > 0 && field[i, j] % 10 == field[i - 1, j] % 10 || j > 0 && field[i, j] % 10 == field[i, j - 1] % 10)
                            field[i, j] = random.Next(0, 20);
    }

    //Get sprite for the element at coords [i, j]
    public string GetFieldButton(int i, int j) => bufferBoardButtonDictionary[field[i, j]];

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
                window.LBRegionGroup().regions[j + field.GetLength(1) - empty].bigButtons[i].gameObject.AddComponent<FallingElement>().Initiate(empty, delay);
            }
        }
    }

    //Currently used buffer board for combat
    public static BufferBoard bufferBoard;

    //IDs of specific elements possible on the board
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

    //Elements on this buffer board
    public int[,] field;

    //Window that this buffer board is connected to and is displayed at
    public Window window;
}