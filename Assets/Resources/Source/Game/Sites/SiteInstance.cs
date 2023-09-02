using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static SaveGame;
using static Coloring;
using static SiteHostileArea;

public class SiteInstance
{
    public void Initialise()
    {
        var localAreas = wings.SelectMany(x => x.areas.Select(y => y.ContainsKey("AreaName") ? y["AreaName"] : ""));
        var temp = areas.FindAll(x => localAreas.Contains(x.name));
        temp.ForEach(x => x.instancePart = true);
    }

    public int x, y;
    public string name, zone, type, ambience;
    public bool complexPart;
    public List<string> description;
    public List<InstanceWing> wings;

    public (int, int) LevelRange()
    {
        var localAreas = wings.SelectMany(x => x.areas.Select(y => y.ContainsKey("AreaName") ? y["AreaName"] : ""));
        var temp = areas.FindAll(x => localAreas.Contains(x.name));
        if (temp.Count() == 0) return (0, 0);
        return (temp.Min(x => x.recommendedLevel), temp.Max(x => x.recommendedLevel));
    }

    public static SiteInstance instance;
    public static List<SiteInstance> instances, instancesSearch;

    public void PrintSite()
    {
        SetAnchor(x * 19, y * 19);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("Site" + type,
            (h) =>
            {
                instance = this;
                CDesktop.cameraDestination = new Vector2(x, y) - new Vector2(17, -9);
                CDesktop.queuedSiteOpen = "Instance";
                CDesktop.LockScreen();
            },
            (h) => () =>
            {
                SetAnchor(TopRight, h.window);
                AddRegionGroup();
                SetRegionGroupWidth(152);
                AddHeaderRegion(() => { AddLine(name, "Gray"); });
                AddPaddingRegion(() =>
                {
                    AddLine("Level range: ", "Gray");
                    var range = LevelRange();
                    AddText(range.Item1 + "", ColorEntityLevel(range.Item1));
                    AddText(" - ", "Gray");
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
