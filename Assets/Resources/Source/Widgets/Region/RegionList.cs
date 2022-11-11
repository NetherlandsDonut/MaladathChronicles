using System;
using System.Collections.Generic;
using UnityEngine;

public class RegionList : MonoBehaviour
{
    //Parent
    public RegionGroup regionGroup;

    //Children
    public List<Region> regions;

    //Fields
    public int page;
    public Func<int> count;
    public Action<int> inDraw, outDraw;

    public void Initialise(RegionGroup regionGroup, Func<int> count, Action<int> inDraw, Action<int> outDraw)
    {
        regions = new();
        this.count = count;
        this.inDraw = inDraw;
        this.outDraw = outDraw;

        this.regionGroup = regionGroup;
        regionGroup.regionList = this;
    }
}
