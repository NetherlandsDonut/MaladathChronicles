using System.Linq;
using System.Collections.Generic;

using static Root;
using static SaveGame;
using static SiteHostileArea;

public class SiteInstance
{
    public void Initialise()
    {
        var localAreas = wings.SelectMany(x => x.areas.Select(y => y.ContainsKey("AreaName") ? y["AreaName"] : ""));
        var temp = areas.FindAll(x => localAreas.Contains(x.name));
        temp.ForEach(x => x.instancePart = true);
    }

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
