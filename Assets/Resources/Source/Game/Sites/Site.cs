using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static MapGrid;
using static SaveGame;
using static SiteInstance;
using static SiteHostileArea;

public class Site
{
    //Coordinates of the site on the world map
    public int x, y;

    //Name of the site
    public string name;

    //Zone that this site resides in
    //It can be and outdoor zone like "Westfall"
    //or an instance or a complex like "Blackrock Mountain"
    public string zone;

    //Sub type of the site.
    //Hostiles areas for example can be an "Emerald Bough".
    public string type;

    //Faction that controls this site.
    //This is very optional and rare to be used.
    //It's mostly utilised by towns
    public string faction;

    //Ambience track that will start playing when
    //the site is opened by the player.
    //If this value is empty then current ambience
    //will not be interrupted and will continue to play
    public string ambience;

    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public virtual void Initialise();

    //Function to print the site onto the map
    public virtual void PrintSite();

    //Queue opening of this site.
    //After calling this the screen is locked and camera will pan there slowly.
    //After reaching the site the screen will change accordingly to the site type
    public static void QueueSiteOpen(string siteType)
    {
        if (siteType == "Instance") instance = site;
        else if (siteType == "HostileArea") area = site;
        else if (siteType == "Town") town = site;
        else if (siteType == "Complex") complex = site;
        else if (siteType == "SpiritHealer") spiritHealer = site;
        CDesktop.cameraDestination = coords + mapCenteringOffset;
        CDesktop.queuedSiteOpen = siteType;
        CDesktop.LockScreen();
    }
}