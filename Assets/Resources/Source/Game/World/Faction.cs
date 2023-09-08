using System.Collections.Generic;

using static Assets;

public class Faction
{
    public string name, icon, side;

    public string Reputation(Entity entity)
    {
        return "";
    }

    public string Icon()
    {
        if (assets.factionIcons.Contains(icon + ".png")) return icon;
        else return "Faction" + side;
    }

    public static Faction faction;
    public static List<Faction> factions, factionsSearch;
}
