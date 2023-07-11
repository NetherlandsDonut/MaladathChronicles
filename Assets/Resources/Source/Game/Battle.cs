using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

using static Root;
using static Race;

public class Battle
{
    public Battle()
    {
        first = saveGames[0].player;
        second = new Entity(races.Find(x => x.name == "Nefarian"));
        turnFirst = true;
    }

    public void ComputerMove()
    {
        //turnFirst = false;
        //var list = Board.board.FloodCount(h.region.bigButtons.FindIndex(x => x.GetComponent<Highlightable>() == h), h.region.regionGroup.regions.IndexOf(h.region));
        //battle.first.health -= list.Count;
        //Board.board.FloodDestroy(h.window, list);
        //turnFirst = true;
    }

    public static Battle battle;

    public Entity first, second;
    public bool turnFirst;
}
