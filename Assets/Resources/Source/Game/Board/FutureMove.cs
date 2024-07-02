using System.Linq;
using System.Collections.Generic;

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

    //Coords of the move on the board
    public int x, y;

    //Depth at which this move is calculated
    public int depth;

    //Ability used by an entity on this move
    public string ability;

    //Board at the end of this turn
    public FutureBoard board;

    //Possible continuations from this point
    public List<FutureMove> possibleSolves;

    //How much desired this outcome is by the player (Below zero means it's desired by the enemy)
    public double desiredness;

    //How desirable how is the most desirable outcome from this board
    public double MaxDesiredness(FutureBoard baseBoard, double previous = 0)
    {
        if (desiredness == 0) desiredness = board.Desiredness(baseBoard);
        return previous + desiredness + (possibleSolves.Count == 0 ? 0 : possibleSolves.Max(x => x.MaxDesiredness(board, desiredness)));
    }
}
