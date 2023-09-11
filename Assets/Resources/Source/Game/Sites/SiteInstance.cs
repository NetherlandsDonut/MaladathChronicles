using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Faction;
using static Coloring;
using static SaveGame;
using static SiteHostileArea;

public class SiteInstance : Site
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
        type ??= "Dungeon";
        if (faction != null)
            if (!factions.Exists(x => x.name == faction))
                factions.Insert(0, new Faction()
                {
                    name = faction,
                    icon = "Faction" + faction,
                    side = "Neutral"
                });
        if (wings != null)
            foreach (var wing in wings)
                if (wing.areas != null)
                    foreach (var area in wing.areas)
                        if (area.ContainsKey("AreaName"))
                            if (!areas.Exists(x => x.name == area["AreaName"]))
                                areas.Insert(0, new SiteHostileArea()
                                {
                                    name = area["AreaName"],
                                    commonEncounters = new(),
                                    rareEncounters = new(),
                                    eliteEncounters = new(),
                                    type = "HostileArea",
                                    zone = this.name
                                });
        var localAreas = wings.SelectMany(x => x.areas.Select(y => y.ContainsKey("AreaName") ? y["AreaName"] : ""));
        var temp = areas.FindAll(x => localAreas.Contains(x.name));
        temp.ForEach(x => x.instancePart = true);
        if (!Blueprint.windowBlueprints.Exists(x => x.title == type + ": " + name))
            Blueprint.windowBlueprints.Add(
                new Blueprint(type + ": " + name,
                    () =>
                    {
                        PlayAmbience(ambience);
                        SetAnchor(TopRight);
                        AddRegionGroup();
                        SetRegionGroupWidth(171);
                        SetRegionGroupHeight(354);
                        AddHeaderRegion(() =>
                        {
                            AddLine(name);
                            AddSmallButton("OtherClose",
                            (h) =>
                            {
                                var title = CDesktop.title;
                                CloseDesktop(title);
                                if (complexPart)
                                    SpawnDesktopBlueprint("ComplexEntrance");
                                else
                                {
                                    PlaySound("DesktopInstanceClose");
                                    SwitchDesktop("Map");
                                }
                            });
                        });
                        AddPaddingRegion(() =>
                        {
                            AddLine("Level range: ", "Gray");
                            var range = LevelRange();
                            AddText(range.Item1 + "", ColorEntityLevel(range.Item1));
                            AddText(" - ", "Gray");
                            AddText(range.Item2 + "", ColorEntityLevel(range.Item2));
                        });
                        foreach (var wing in wings)
                            PrintInstanceWing(this, wing);
                        AddPaddingRegion(() => { });
                    }
                )
            );
        if (x != 0 && y != 0)
            Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite()));
    }

    //Determines whether this instance is a part of a complex
    public bool complexPart;

    //Instance description showed in the left side of the screen
    public List<string> description;

    //Instance wings that store all the instance's areas
    public List<InstanceWing> wings;

    //Suggested level range for the player to enter this instance
    public (int, int) LevelRange()
    {
        var localAreas = wings.SelectMany(x => x.areas.Select(y => y.ContainsKey("AreaName") ? y["AreaName"] : ""));
        var temp = areas.FindAll(x => localAreas.Contains(x.name));
        if (temp.Count() == 0) return (0, 0);
        return (temp.Min(x => x.recommendedLevel), temp.Max(x => x.recommendedLevel));
    }

    //Currently opened instance
    public static SiteInstance instance;

    //EXTERNAL FILE: List containing all instances in-game
    public static List<SiteInstance> instances;

    //List of all filtered instances by input search
    public static List<SiteInstance> instancesSearch;

    //Function to print the site onto the map
    public override void PrintSite()
    {
        SetAnchor(x * 19, y * 19);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("Site" + type,
            (h) => { QueueSiteOpen("Instance"); },
            (h) => () =>
            {
                SetAnchor(TopRight, h.window);
                AddRegionGroup();
                SetRegionGroupWidth(152);
                AddHeaderRegion(() => { AddLine(name); });
                AddPaddingRegion(() =>
                {
                    AddLine("Level range: ");
                    var range = LevelRange();
                    AddText(range.Item1 + "", ColorEntityLevel(range.Item1));
                    AddText(" - ");
                    AddText(range.Item2 + "", ColorEntityLevel(range.Item2));
                });
                var areas = wings.SelectMany(x => x.areas.Select(y => SiteHostileArea.areas.Find(z => z.name == y["AreaName"])));
                var total = areas.SelectMany(x => x.commonEncounters ?? new()).Distinct().ToList();
                total.AddRange(areas.SelectMany(x => x.eliteEncounters ?? new()).Distinct().ToList());
                total.AddRange(areas.SelectMany(x => x.rareEncounters ?? new()).Distinct().ToList());
                var races = total.Select(x => Race.races.Find(y => y.name == x.who).portrait).Distinct().ToList();
                if (races.Count > 0)
                    for (int i = 0; i < Math.Ceiling(races.Count / 8.0); i++)
                    {
                        var ind = i;
                        AddPaddingRegion(() =>
                        {
                            for (int j = 0; j < 8 && j < races.Count - ind * 8; j++)
                            {
                                var jnd = j;
                                AddSmallButton(races[jnd + ind * 8], (h) => { });
                            }
                        });
                    }
            });
        });
    }

    public static void PrintInstanceWing(SiteInstance instance, InstanceWing wing)
    {
        if (instance.wings.Count > 1)
            AddHeaderRegion(() => { AddLine(wing.name); });
        var temp = wing.areas.Select(x => areas.Find(y => x.ContainsKey("AreaName") && y.name == x["AreaName"])).ToList();
        foreach (var area in temp)
            AddButtonRegion(() =>
            {
                var name = area != null ? area.name : "AREA NOT FOUND";
                AddLine(name, "", "Right");
            },
            (h) =>
            {
                if (area == null) return;
                SiteHostileArea.area = area;
                var window = CDesktop.windows.Find(x => x.title.StartsWith("HostileArea: "));
                if (window != null)
                    if (window.title == "HostileArea: " + area.name) return;
                    else CloseWindow(window);
                CloseWindow("InstanceLeftSide");
                SpawnWindowBlueprint("HostileArea: " + area.name);
                SetDesktopBackground("Areas/Area" + (instance.name + area.name).Replace("'", "").Replace(".", "").Replace(" ", "") + (area.specialClearBackground && area.eliteEncounters.All(x => currentSave.elitesKilled.ContainsKey(x.who)) ? "Cleared" : ""));
            });
    }
}

public class InstanceWing
{
    public string name;
    public List<Dictionary<string, string>> areas;
}
