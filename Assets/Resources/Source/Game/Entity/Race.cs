using System.Collections.Generic;

using static Faction;

public class Race
{
    public int level;
    public string name, faction, startingSite, kind, portrait, background;
    public double vitality;
    public Stats stats;
    public List<string> abilities, maleNames, femaleNames;
    public List<(int, string)> loot;

    public Faction Faction()
    {
        if (faction == null) return null;
        return factions.Find(x => x.name == faction);
    }

    public static Race race;
    public static List<Race> races, racesSearch;
}
