using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talent
{
    public Talent(int row, int col, string ability, bool inherited = false, bool defaultTaken = false)
    {
        this.row = row;
        this.col = col;
        this.ability = ability;
        if (inherited && row > 0)
            this.inherited = inherited;
        this.defaultTaken = defaultTaken;
    }

    public int row, col;
    public string ability;
    public bool inherited, defaultTaken;
}
