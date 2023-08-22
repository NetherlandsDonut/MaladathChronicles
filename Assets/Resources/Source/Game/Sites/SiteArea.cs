using System;
using System.Linq;
using System.Collections.Generic;

using static Root;

public class SiteArea
{
    public void Initialise()
    {
        if (type == null) type = "HostileArea";
        if (bossEncounters != null && bossEncounters.Count > 0)
            recommendedLevel = (int)bossEncounters.Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin);
        else if (possibleEncounters != null && possibleEncounters.Count > 0)
            recommendedLevel = (int)possibleEncounters.Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin);
    }

    public Entity RollEncounter()
    {
        var encounters = possibleEncounters.Select(x => (x.levelMax != 0 ? random.Next(x.levelMin, x.levelMax + 1) : x.levelMin, Race.races.Find(y => y.name == x.who))).ToList();
        if (random.Next(0, 100) < 1)
        {
            var rares = encounters.FindAll(x => x.Item2.rarity == "Rare");
            rares = rares.FindAll(x => !currentSave.raresKilled.Contains(x.Item2.name));
            if (rares.Count > 0)
                encounters = new() { rares[random.Next(0, rares.Count)] };
        }
        var find = encounters[random.Next(0, encounters.Count)];
        return new Entity(find.Item1, find.Item2);
    }

    public Entity RollBoss(Encounter boss)
    {
        return new Entity(boss.levelMax != 0 ? random.Next(boss.levelMin, boss.levelMax + 1) : boss.levelMin, Race.races.Find(x => x.name == boss.who));
    }

    public string name, zone, type;
    public List<Encounter> possibleEncounters, bossEncounters;

    [NonSerialized] public int recommendedLevel;
    [NonSerialized] public bool instancePart, complexPart;

    public static SiteArea area;
    public static List<SiteArea> areas;
}

public class Encounter
{
    public string who;
    public int levelMin, levelMax, requiredProgress;
}
