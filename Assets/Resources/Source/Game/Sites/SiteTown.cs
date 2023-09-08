using System.Collections.Generic;

using static Root;
using static Root.Anchor;

using static Faction;

public class SiteTown : Site
{
    //List of all transport paths provided in this town
    public List<Transport> transport;

    //List of NPC's that are inside of this town
    public List<Person> people;

    //Currently opened town
    public static SiteTown town;

    //EXTERNAL FILE: List containing all towns in-game
    public static List<SiteTown> towns;

    //List of all filtered towns by input search
    public static List<SiteTown> townsSearch;

    //Prints the site on the world map
    public override void PrintSite()
    {
        SetAnchor(x * 19, y * 19);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton(factions.Find(x => x.name == faction).Icon(),
            (h) => { QueueSiteOpen("Town"); },
            (h) => () =>
            {
                SetAnchor(TopRight, h.window);
                AddRegionGroup();
                AddHeaderRegion(() =>
                {
                    AddLine(name);
                });
            });
        });
    }
}
