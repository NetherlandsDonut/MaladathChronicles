using System.Collections.Generic;

using static Root;
using static SiteInstance;
using static SiteHostileArea;

public class SiteComplex
{
    public void Initialise()
    {
        instances.FindAll(x => sites.Exists(y => (y["SiteType"] == "Raid" || y["SiteType"] == "Dungeon") && y["SiteName"] == x.name)).ForEach(x => x.complexPart = true);
        areas.FindAll(x => sites.Exists(y => y["SiteType"] == "HostileArea" && y["SiteName"] == x.name)).ForEach(x => x.complexPart = true);
    }

    public string name, zone, ambience;
    public List<string> description;
    public List<Dictionary<string, string>> sites;

    public static SiteComplex complex;
    public static List<SiteComplex> complexes;

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
                SpawnWindowBlueprint("HostileArea: " + area.name);
                SetDesktopBackground("Areas/Area" + (area.zone + area.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
                SpawnTransition();
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