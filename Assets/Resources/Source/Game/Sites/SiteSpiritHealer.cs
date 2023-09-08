using System.Collections.Generic;

using static Root;

public class SiteSpiritHealer : Site
{
    //Currently opened spirit healer
    public static SiteSpiritHealer spiritHealer;

    //EXTERNAL FILE: List containing all spirit healers in-game
    public static List<SiteSpiritHealer> spiritHealers;

    //List of all filtered spirit healers by input search
    public static List<SiteSpiritHealer> spiritHealersSearch;

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
