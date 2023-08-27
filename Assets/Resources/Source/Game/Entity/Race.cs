using System.Collections.Generic;

public class Race
{
    public int level;
    public string name, faction, kind, portrait, background;
    public double vitality;
    public Stats stats;
    public List<string> abilities, maleNames, femaleNames;
    public List<(int, string)> loot;

    public static Race race;
    public static List<Race> races, racesSearch;
}
