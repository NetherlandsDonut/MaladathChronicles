using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class RegionGroup : MonoBehaviour
{
    //Parent
    public Window window;

    //Children
    public List<Region> regions;
    public Region LBRegion, EXTRegion;

    //Fields
    public int setWidth, setHeight, currentHeight;

    public void Initialise(Window window, bool header)
    {
        regions = new();
        this.window = window;
        if (header) window.headerGroup = this;
        else window.regionGroups.Add(this);
        window.LBRegionGroup = this;
    }

    public int PlannedHeight()
    {
        var regionSum = regions.Sum(x => x.PlannedHeight());
        return regionSum;
    }

    public int AutoWidth()
    {
        var regionMax = regions.Max(x => x.AutoWidth());
        return setWidth != 0 ? setWidth : regionMax;
    }

    public void ResetContent()
    {
        currentHeight = 0;
        regions.ForEach(x => x.ResetContent());
    }
}
