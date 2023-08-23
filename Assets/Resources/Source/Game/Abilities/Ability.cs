using System.Collections.Generic;

using static Root;
using static Root.Color;

public class Ability
{
    #region Resource Check

    public bool EnoughResources(Entity entity) => EnoughResources(entity.resources);
    public bool EnoughResources(FutureEntity entity) => EnoughResources(entity.resources);

    private bool EnoughResources(Dictionary<string, int> resources)
    {
        foreach (var resource in cost)
            if (resources[resource.Key] < resource.Value)
                return false;
        return true;
    }

    #endregion

    #region Execution

    public void ExecuteFutureEvents(FutureBoard board, string trigger)
    {
        if (events == null) return;
        foreach (var foo in events)
            foreach (var woo in foo.triggers)
                if (woo.ContainsKey("Trigger") && woo["Trigger"] == trigger)
                    foo.ExecuteEffects(null, board, icon);
    }

    public void ExecuteEvents(string trigger)
    {
        if (events == null) return;
        foreach (var foo in events)
            foreach (var woo in foo.triggers)
                if (woo.ContainsKey("Trigger") && woo["Trigger"] == trigger)
                    foo.ExecuteEffects(Board.board, null, icon);
    }

    #endregion

    #region Description

    public void PrintDescription(Entity effector, Entity other, int width)
    {
        if (description != null) description.Print(effector, other, width);
        else AddHeaderRegion(() =>
        {
            SetRegionAsGroupExtender();
            AddLine("No description", DarkGray);
        });
    }

    #endregion

    public string name, icon;
    public int cooldown;
    public bool putOnEnd;
    public List<string> tags;
    public Dictionary<string, int> cost;
    public List<Event> events;
    public Description description;

    public static List<Ability> abilities;
}
