using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static SaveGame;
using static Coloring;

public class SiteHostileArea
{
    public void Initialise()
    {
        type ??= "HostileArea";
        if (eliteEncounters != null && eliteEncounters.Count > 0)
            recommendedLevel = (int)eliteEncounters.Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin);
        else if (commonEncounters != null && commonEncounters.Count > 0)
            recommendedLevel = (int)commonEncounters.Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin);
        if (eliteEncounters != null && eliteEncounters.Count == 0) eliteEncounters = null;
        if (rareEncounters != null && rareEncounters.Count == 0) rareEncounters = null;
        if (commonEncounters != null && commonEncounters.Count == 0) commonEncounters = null;
    }

    public void PrintSite()
    {
        SetAnchor(x * 19, y * 19);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("Site" + type,
            (h) =>
            {
                area = this;
                CDesktop.cameraDestination = new Vector2(x, y) - new Vector2(17, -9);
                CDesktop.queuedSiteOpen = "HostileArea";
                CDesktop.LockScreen();
            },
            (h) => () =>
            {
                SetAnchor(TopRight, h.window);
                AddRegionGroup();
                AddHeaderRegion(() =>
                {
                    AddLine(name, "Gray");
                });
                AddHeaderRegion(() =>
                {
                    AddLine("Recommended level: ", "Gray");
                    AddText(recommendedLevel + "", ColorEntityLevel(recommendedLevel));
                });
                if (commonEncounters != null && commonEncounters.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Common: ", "Gray");
                        foreach (var enemy in commonEncounters)
                        {
                            var race = Race.races.Find(x => x.name == enemy.who);
                            AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                        }
                    });
                if (eliteEncounters != null && eliteEncounters.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Elite: ", "Gray");
                        foreach (var enemy in eliteEncounters)
                        {
                            var race = Race.races.Find(x => x.name == enemy.who);
                            AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                        }
                    });
                if (rareEncounters != null && rareEncounters.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Rare: ", "Gray");
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

    public int x, y;
    public string name, zone, type, ambience;
    public bool specialClearBackground;
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
