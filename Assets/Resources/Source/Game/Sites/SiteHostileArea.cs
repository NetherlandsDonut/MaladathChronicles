using System;
using System.Linq;
using System.Collections.Generic;

using static Root;

public class SiteHostileArea
{
    public void Initialise()
    {
        if (type == null) type = "HostileArea";
        if (eliteEncounters != null && eliteEncounters.Count > 0)
            recommendedLevel = (int)eliteEncounters.Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin);
        else if (commonEncounters != null && commonEncounters.Count > 0)
            recommendedLevel = (int)commonEncounters.Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin);
        if (eliteEncounters != null && eliteEncounters.Count == 0) eliteEncounters = null;
        if (rareEncounters != null && rareEncounters.Count == 0) rareEncounters = null;
        if (commonEncounters != null && commonEncounters.Count == 0) commonEncounters = null;
    }

    public Entity RollEncounter()
    {
        var encounters = commonEncounters.Select(x => (x.levelMax != 0 ? random.Next(x.levelMin, x.levelMax + 1) : x.levelMin, Race.races.Find(y => y.name == x.who))).ToList();
        if (random.Next(0, 100) < 1)
        {
            var rares = rareEncounters.FindAll(x => !currentSave.raresKilled.Contains(x.who));
            if (rares.Count > 0) encounters = rares.Select(x => (x.levelMax != 0 ? random.Next(x.levelMin, x.levelMax + 1) : x.levelMin, Race.races.Find(y => y.name == x.who))).ToList();
        }
        var find = encounters[random.Next(0, encounters.Count)];
        return new Entity(find.Item1, find.Item2);
    }

    public Entity RollBoss(Encounter boss)
    {
        return new Entity(boss.levelMax != 0 ? random.Next(boss.levelMin, boss.levelMax + 1) : boss.levelMin, Race.races.Find(x => x.name == boss.who));
    }

    public string name, zone, type, ambience;
    public List<Encounter> commonEncounters, rareEncounters, eliteEncounters;

    [NonSerialized] public int recommendedLevel;
    [NonSerialized] public bool instancePart, complexPart;

    public static SiteHostileArea area;
    public static List<SiteHostileArea> areas, areasSearch;
}

public class Encounter
{
    public string who;
    public int levelMin, levelMax, requiredProgress;
}
