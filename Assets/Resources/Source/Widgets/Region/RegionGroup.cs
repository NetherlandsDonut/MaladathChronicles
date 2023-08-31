using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class RegionGroup : MonoBehaviour
{
    public Window window;
    public List<Region> regions;
    public Region LBRegion, stretchRegion;
    public int setWidth, setHeight, currentHeight, pagination, perPage;
    public Func<double> maxPaginationReq;
    public Func<int> maxPagination;

    public void Initialise(Window window, bool header, Func<double> maxPagination, int perPage)
    {
        regions = new();
        this.window = window;
        this.perPage = perPage;
        if (maxPaginationReq == null)
            maxPaginationReq = maxPagination;
        if (maxPaginationReq != null)
            this.maxPagination = () =>
            {
                var max = (int)Math.Ceiling(maxPaginationReq() / perPage);
                if (max < 1) return 1;
                else return max;
            };
        else this.maxPagination = () => 1;
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
