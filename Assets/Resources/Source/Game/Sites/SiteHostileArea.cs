using System;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Anchor;

using static SaveGame;
using static Coloring;

public class SiteHostileArea : Site
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
        type ??= "HostileArea";
        if (commonEncounters != null)
            if (commonEncounters.Count > 0)
                recommendedLevel = (int)commonEncounters.Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin);
                else commonEncounters = null;
        if (rareEncounters != null && rareEncounters.Count == 0) rareEncounters = null;
        if (eliteEncounters != null)
            if (eliteEncounters.Count > 0)
                recommendedLevel = (int)eliteEncounters.Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin);
                else eliteEncounters = null;
    }

    //Function to print the site onto the map
    public override void PrintSite()
    {
        SetAnchor(x * 19, y * 19);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("Site" + type,
            (h) =>
            {
                QueueSiteOpen("HostileArea");
            },
            (h) => () =>
            {
                SetAnchor(TopRight, h.window);
                AddRegionGroup();
                AddHeaderRegion(() =>
                {
                    AddLine(name);
                });
                AddHeaderRegion(() =>
                {
                    AddLine("Recommended level: ");
                    AddText(recommendedLevel + "", ColorEntityLevel(recommendedLevel));
                });
                if (commonEncounters != null && commonEncounters.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Common: ");
                        foreach (var enemy in commonEncounters)
                        {
                            var race = Race.races.Find(x => x.name == enemy.who);
                            AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                        }
                    });
                if (eliteEncounters != null && eliteEncounters.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Elite: ");
                        foreach (var enemy in eliteEncounters)
                        {
                            var race = Race.races.Find(x => x.name == enemy.who);
                            AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                        }
                    });
                if (rareEncounters != null && rareEncounters.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Rare: ");
                        foreach (var enemy in rareEncounters)
                            AddSmallButton("OtherUnknown", (h) => { });
                    });
            });
        });
    }

    public Entity RollEncounter()
    {
        var encounters = commonEncounters.Select(x => (x.levelMax != 0 ? random.Next(x.levelMin, x.levelMax + 1) : x.levelMin, Race.races.Find(y => y.name == x.who))).ToList();
        if (random.Next(0, 100) < 1 && rareEncounters != null)
        {
            var rares = rareEncounters.FindAll(x => !currentSave.raresKilled.ContainsKey(x.who));
            if (rares.Count > 0) encounters = rares.Select(x => (x.levelMax != 0 ? random.Next(x.levelMin, x.levelMax + 1) : x.levelMin, Race.races.Find(y => y.name == x.who))).ToList();
        }
        var find = encounters[random.Next(0, encounters.Count)];
        return new Entity(find.Item1, find.Item2);
    }

    public Entity RollEncounter(Encounter boss)
    {
        return new Entity(boss.levelMax != 0 ? random.Next(boss.levelMin, boss.levelMax + 1) : boss.levelMin, Race.races.Find(x => x.name == boss.who));
    }

    //Tells program whether this area has a special
    //clear background that is shown only after clearing the area
    public bool specialClearBackground;

    //List of possible common encounters in this area
    public List<Encounter> commonEncounters;

    //List of possible rare encounters in this area
    public List<Encounter> rareEncounters;

    //List of special elite encounters in this area
    public List<Encounter> eliteEncounters;

    //Automatically calculated number that suggests
    //at which level player should enter this area
    [NonSerialized] public int recommendedLevel;

    //Determines whether this area is part of an instance
    [NonSerialized] public bool instancePart;

    //Determines whether this area is part of a complex
    [NonSerialized] public bool complexPart;

    //Currently opened hostile area
    public static SiteHostileArea area;

    //EXTERNAL FILE: List containing all hostile areas in-game
    public static List<SiteHostileArea> areas;

    //List of all filtered hostile areas by input search
    public static List<SiteHostileArea> areasSearch;
}

public class Encounter
{
    public string who;
    public int levelMin, levelMax, requiredProgress;
}
