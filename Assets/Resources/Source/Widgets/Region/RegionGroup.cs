using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class RegionGroup : MonoBehaviour
{
    //Window this region group belongs to
    public Window window;

    //All the regions this region group has
    public List<Region> regions;

    public Region LBRegion, stretchRegion;
    public int setWidth, setHeight, currentHeight, perPage;
    public Func<double> maxPaginationReq;
    public Func<int> maxPagination, pagination;

    public void Initialise(Window window, bool header, Func<double> maxPagination, int perPage)
    {
        regions = new();
        this.window = window;
        this.perPage = perPage;
        maxPaginationReq ??= maxPagination;
        if (maxPaginationReq != null)
            this.maxPagination = () =>
            {
                var max = (int)Math.Ceiling(maxPaginationReq() / perPage);
                if (max < 1) return 1;
                else return max;
            };
        else this.maxPagination = () => 1;
        pagination = () =>
        {
            if (window == null || !Root.staticPagination.ContainsKey(window.title)) return 0;
            var dx = window.regionGroups.IndexOf(this);
            if (dx == -1) dx = Root.staticPagination[window.title].Length - 1;
            return Root.staticPagination[window.title][dx];
        };
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
        var regionMax = regions.Count == 0 ? 0 : regions.Max(x => x.AutoWidth());
        return setWidth != 0 ? setWidth : regionMax;
    }
}
