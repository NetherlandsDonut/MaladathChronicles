using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

public class SpiritHealer
{
    public int x, y;
    public string name, zone;

    public static SpiritHealer spiritHealer;
    public static List<SpiritHealer> spiritHealers, spiritHealersSearch;

    public void PrintSite()
    {
        SetAnchor(x * 19, y * 19);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("SiteSpiritHealer",
            (h) =>
            {
                CDesktop.cameraDestination = new Vector2(x, y) - new Vector2(17, -9);
                CDesktop.queuedSiteOpen = "SpiritHealer";
                CDesktop.LockScreen();
            });
        });
    }
}
