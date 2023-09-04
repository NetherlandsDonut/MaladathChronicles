using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static SaveGame;
using static SiteInstance;
using static SiteHostileArea;

public class SiteComplex
{
    public void Initialise()
    {
        instances.FindAll(x => sites.Exists(y => (y["SiteType"] == "Raid" || y["SiteType"] == "Dungeon") && y["SiteName"] == x.name)).ForEach(x => x.complexPart = true);
        areas.FindAll(x => sites.Exists(y => y["SiteType"] == "HostileArea" && y["SiteName"] == x.name)).ForEach(x => x.complexPart = true);
    }

    public int x, y;
    public string name, zone, ambience;
    public List<string> description;
    public List<Dictionary<string, string>> sites;

    public static SiteComplex complex;
    public static List<SiteComplex> complexes, complexesSearch;

    public void PrintSite()
    {
        SetAnchor(x * 19, y * 19);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("SiteComplex",
            (h) =>
            {
                complex = this;
                CDesktop.cameraDestination = new Vector2(x, y) - new Vector2(17, -9);
                CDesktop.queuedSiteOpen = "Complex";
                CDesktop.LockScreen();
            },
            (h) => () =>
            {
                SetAnchor(TopRight, h.window);
                AddRegionGroup();
                AddHeaderRegion(() => { AddLine(name, "Gray"); });
                AddPaddingRegion(() => { AddLine("Contains sites:", "DarkGray"); });
                complex = complexes.Find(x => x.name == name);
                foreach (var site in complex.sites)
                    AddHeaderRegion(() =>
                    {
                        AddLine(site["SiteName"], "DarkGray");
                        AddSmallButton("Site" + site["SiteType"], (h) => { });
                    });
            });
        });
    }

    public static void PrintComplexSite(Dictionary<string, string> site)
    {
        AddButtonRegion(() =>
        {
            AddLine(site["SiteName"], "", "Right");
            AddSmallButton("Site" + site["SiteType"], (h) => { });
        },
        (h) =>
        {
            if (site["SiteType"] == "HostileArea")
            {
                area = areas.Find(x => x.name == site["SiteName"]);
                var window = CDesktop.windows.Find(x => x.title.StartsWith("HostileArea: "));
                if (window != null)
                    if (window.title == "HostileArea: " + area.name) return;
                    else CloseWindow(window);
                CloseWindow("ComplexLeftSide");
                SpawnWindowBlueprint("HostileArea: " + area.name);
                SetDesktopBackground("Areas/Area" + (area.zone + area.name).Replace("'", "").Replace(".", "").Replace(" ", "") + (area.specialClearBackground && area.eliteEncounters.All(x => currentSave.elitesKilled.ContainsKey(x.who)) ? "Cleared" : ""));
            }
            else
            {
                CloseDesktop("ComplexEntrance");
                instance = instances.Find(x => x.name == site["SiteName"]);
                SpawnDesktopBlueprint("InstanceEntrance");
            }
        });
    }


}