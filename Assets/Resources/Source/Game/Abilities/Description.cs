using System.Linq;
using System.Collections.Generic;

using static Root;

public class Description
{
    public void Print(Entity effector, Entity other, List<Event> events)
    {
        if (regions.Count(x => x.isExtender) != 1)
        {
            regions.ForEach(x => x.isExtender = false);
            regions.Last().isExtender = true;
        }
        foreach (var region in regions)
            region.PrintRegion(effector, other);
    }

    public List<DescriptionRegion> regions;
}

public class DescriptionRegion
{
    public string regionType;
    public bool isExtender;
    public List<Dictionary<string, string>> contents;

    public void PrintRegion(Entity effector, Entity other)
    {
        if (regionType == "Header")
            AddHeaderRegion(() => PrintContents(effector, other));
        else if (regionType == "Padding")
            AddPaddingRegion(() => PrintContents(effector, other));
    }

    public void PrintContents(Entity effector, Entity other)
    {
        if (isExtender)
            SetRegionAsGroupExtender();
        AddLine("");
        foreach (var text in contents)
            AddText(text["Text"], ColorFromText(text["Color"]));
    }
}