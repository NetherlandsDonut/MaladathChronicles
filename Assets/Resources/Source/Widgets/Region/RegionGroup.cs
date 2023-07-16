using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class RegionGroup : MonoBehaviour
{
    //Parent
    public Window window;

    //Children
    public RegionList regionList;
    public List<Region> regions;
    public Region LBRegion, EXTRegion;

    //Fields
    public int setWidth, setHeight, currentHeight, pagination;

    //For header group
    public void Initialise(Window window)
    {
        regions = new();
        this.window = window;

        window.headerGroup = this;
        window.LBRegionGroup = this;
    }

    //For normal groups
    public void Initialise(Window window, int insert)
    {
        regions = new();
        this.window = window;

        window.LBRegionGroup = this;
        if (insert == -1) window.regionGroups.Add(this);
        else window.regionGroups.Insert(insert, this);
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
