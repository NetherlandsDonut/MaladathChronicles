using System.Collections.Generic;

using static SiteInstance;
using static SiteHostileArea;

public class SiteComplex
{
    public void Initialise()
    {
        instances.FindAll(x => sites.Exists(y => (y["SiteType"] == "Raid" || y["SiteType"] == "Dungeon") && y["SiteName"] == x.name)).ForEach(x => x.complexPart = true);
        areas.FindAll(x => sites.Exists(y => y["SiteType"] == "HostileArea" && y["SiteName"] == x.name)).ForEach(x => x.complexPart = true);
    }

    public string name;
    public List<string> description;
    public List<Dictionary<string, string>> sites;

    public static SiteComplex complex;
    public static List<SiteComplex> complexes;
}