using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureMove
{
    public FutureMove(int x, int y, FutureBoard board)
    {
        this.x = x;
        this.y = y;
        ability = "";
        this.board = board;
    }

    public FutureMove(string ability, FutureBoard board)
    {
        x = y = -1;
        this.ability = ability;
        this.board = board;
    }

    public int x, y;
    public string ability;
    public FutureBoard board;
    public double desiredness;

    public double Desiredness(Board originalBoard, double baseDesiredness)
    {
        desiredness = board.Desiredness(board.enemy, originalBoard.enemy, board.player, originalBoard.player, baseDesiredness);
        return desiredness;
    }
}
