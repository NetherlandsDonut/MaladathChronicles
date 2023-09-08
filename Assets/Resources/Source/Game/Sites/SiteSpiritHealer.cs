using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

public class SpiritHealer : Site
{
    //Currently opened spirit healer
    public static SpiritHealer spiritHealer;

    //EXTERNAL FILE: List containing all spirit healers in-game
    public static List<SpiritHealer> spiritHealers;

    //List of all filtered spirit healers by input search
    public static List<SpiritHealer> spiritHealersSearch;

    public override void PrintSite()
    {
        SetAnchor(x * 19, y * 19);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("SiteSpiritHealer",
            (h) => { QueueSiteOpen("SpiritHealer"); });
        });
    }
}
