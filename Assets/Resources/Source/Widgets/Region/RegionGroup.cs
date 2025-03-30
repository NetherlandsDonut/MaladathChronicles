using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class RegionGroup : MonoBehaviour
{
    //Window this region group belongs to
    public Window window;

    //All the regions this region group has
    public List<Region> regions;

    //Region that will be stretched to match the set height of the group
    public Region stretchRegion;

    //Set width of the region group
    public int setWidth;

    //Set height of the region group
    public int setHeight;

    //Current height of the region group
    public int currentHeight;

    public void Initialise(Window window, bool header)
    {
        regions = new();
        this.window = window;
        if (header) { window.headerGroup = this; window.regionGroups.Insert(0, this); }
        else window.regionGroups.Add(this);
    }

    public int PlannedHeight()
    {
        var regionSum = regions.Sum(x => x.PlannedHeight());
        return regionSum;
    }

    public Region LBRegion() => regions.SafeLast();

    public int AutoWidth()
    {
        var regionMax = regions.Count == 0 ? 0 : regions.Max(x => x.AutoWidth());
        return setWidth != 0 ? setWidth : regionMax;
    }
}
