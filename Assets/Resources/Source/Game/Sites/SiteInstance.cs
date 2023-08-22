using System.Linq;
using System.Collections.Generic;

using static SiteArea;

public class SiteInstance
{
    public void Initialise()
    {
        var temp = areas.FindAll(x => wings.Any(y => y.areas.Exists(z => z.Item1 == y.name)));
        temp.ForEach(x => x.instancePart = true);
    }

    public string name, type;
    public bool complexPart;
    public List<string> description;
    public List<InstanceWing> wings;

    public (int, int) LevelRange()
    {
        var temp = wings.SelectMany(x => areas.FindAll(y => x.areas.Exists(z => z.Item2 == y.name)));
        if (temp.Count() == 0) return (0, 0);
        return (temp.Min(x => x.recommendedLevel), temp.Max(x => x.recommendedLevel));
    }

    public static SiteInstance instance;
    public static List<SiteInstance> instances;
}

public class InstanceWing
{
    public string name;
    public List<(string, string)> areas;
}
