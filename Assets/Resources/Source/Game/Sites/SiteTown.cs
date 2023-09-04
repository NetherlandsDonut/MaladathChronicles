using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

public class SiteTown
{
    public int x, y;
    public string name, zone, faction, ambience;
    public List<Transport> transport;
    public List<Vendor> vendors;

    public static SiteTown town;
    public static List<SiteTown> towns, townsSearch;

    public void PrintSite()
    {
        SetAnchor(x * 19, y * 19);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("Faction" + faction,
            (h) =>
            {
                town = this;
                CDesktop.cameraDestination = new Vector2(x, y) - new Vector2(17, -9);
                CDesktop.queuedSiteOpen = "Town";
                CDesktop.LockScreen();
            },
            (h) => () =>
            {
                SetAnchor(TopRight, h.window);
                AddRegionGroup();
                AddHeaderRegion(() =>
                {
                    AddLine(name, "Gray");
                });
            });
        });
    }
}
