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
    public int setWidth, setHeight, currentHeight, pagination, perPage;
    public Func<double> maxPaginationReq;
    public Func<int> maxPagination;

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

    #region Static Pagination

    //Saved static pagination
    public static List<int> staticPagination;

    //Loads the static pagination at specific index
    public static int SavedStaticPagination(int index) => staticPagination.Count > index ? staticPagination[index] : 0;

    //Saves the pagination for the window total rebuilt
    public static void SaveStaticPagination(Window window) => staticPagination = window.regionGroups.Select(x => x.pagination).ToList();

    #endregion

}
