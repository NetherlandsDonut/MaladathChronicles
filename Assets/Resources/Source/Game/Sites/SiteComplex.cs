using System.Collections.Generic;

using static SiteInstance;
using static SiteArea;

public class SiteComplex
{
    public void Initialise()
    {
        instances.FindAll(x => sites.Exists(y => (y.Item1 == "Raid" || y.Item1 == "Dungeon") && y.Item2 == x.name)).ForEach(x => x.complexPart = true);
        areas.FindAll(x => sites.Exists(y => y.Item1 == "HostileArea" && y.Item2 == x.name)).ForEach(x => x.complexPart = true);
    }

    public string name;
    public List<string> description;
    public List<(string, string)> sites;

    public static SiteComplex complex;
    public static List<SiteComplex> complexes;
}