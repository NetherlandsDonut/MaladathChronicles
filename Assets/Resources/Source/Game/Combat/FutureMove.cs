using System.Linq;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class FutureMove
{
    public FutureMove(int x, int y, FutureBoard board)
    {
        this.x = x;
        this.y = y;
        ability = "";
        this.board = board;
        possibleSolves = new();
    }

    public FutureMove(string ability, FutureBoard board)
    {
        x = y = -1;
        this.ability = ability;
        this.board = board;
        possibleSolves = new();
    }

    public int x, y, depth;
    public string ability;
    public FutureBoard board;
    public List<FutureMove> possibleSolves;
    public double desiredness;

    public double MaxDesiredness(FutureBoard baseBoard, double previous = 0)
    {
        if (desiredness == 0) desiredness = board.Desiredness(baseBoard);
        return previous + desiredness + (possibleSolves.Count == 0 ? 0 : possibleSolves.Max(x => x.MaxDesiredness(board, desiredness)));
    }
}
