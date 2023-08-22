using UnityEngine;
using System.Collections.Generic;

using static Root;
using static Root.Color;

public class Buff
{
    public string name, icon, dispelType;
    public List<string> tags;
    public bool stackable;
    public List<Event> events;
    public Description description;

    public static Entity Target(bool player)
    {
        return !player ? Board.board.player : Board.board.enemy;
    }

    public static FutureEntity Target(bool player, FutureBoard futureBoard)
    {
        return !player ? futureBoard.player : futureBoard.enemy;
    }

    public static Entity Caster(bool player)
    {
        if (Board.board == null) return currentSave.player;
        return player ? Board.board.player : Board.board.enemy;
    }

    public static FutureEntity Caster(bool player, FutureBoard futureBoard)
    {
        return player ? futureBoard.player : futureBoard.enemy;
    }

    public void ExecuteFutureEvents(FutureBoard board, string trigger)
    {
        if (events == null) return;
        foreach (var foo in events)
            foreach (var woo in foo.triggers)
                if (woo.ContainsKey("Trigger") && woo["Trigger"] == trigger)
                    foo.ExecuteEffects(null, board, icon);
    }

    public void ExecuteEvents(GameObject buffObject, string trigger)
    {
        if (events == null) return;
        foreach (var foo in events)
            foreach (var woo in foo.triggers)
                if (woo.ContainsKey("Trigger") && woo["Trigger"] == trigger)
                {
                    if (buffObject != null)
                        Board.board.actions.Add(() => { AddSmallButtonOverlay(buffObject, "OtherGlowFull", 1); });
                    foo.ExecuteEffects(Board.board, null, icon);
                }
    }

    public void PrintDescription(Entity effector, Entity other)
    {
        if (description != null) description.Print(effector, other, events);
        else AddHeaderRegion(() =>
        {
            SetRegionAsGroupExtender();
            AddLine("No description", DarkGray);
        });
    }

    public static List<Buff> buffs;
}