using System.Collections.Generic;

using UnityEngine;

using static Root;
using static MapGrid;
using static SiteTown;
using static SaveGame;
using static SiteComplex;
using static SiteInstance;
using static SiteHostileArea;
using static SiteSpiritHealer;

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

    //List of all sites connected to this one
    //letting the player travel from this site to any of these directly
    public List<string> connectedSites;

    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public virtual void Initialise() { }

    //Function to print the site onto the map
    public virtual void PrintSite() { }

    //Returns path to a texture that is the background visual of this site
    public virtual string Background() => "";

    //Queue opening of this site.
    //After calling this the screen is locked and camera will pan there slowly.
    //After reaching the site the screen will change accordingly to the site type
    public void QueueSiteOpen(string siteType)
    {
        if (currentSave.siteVisits == null) currentSave.siteVisits = new();
        if (currentSave.siteVisits.ContainsKey(name)) currentSave.siteVisits[name]++;
        else currentSave.siteVisits.Add(name, 1);
        if (siteType == "Instance") instance = (SiteInstance)this;
        else if (siteType == "HostileArea") area = (SiteHostileArea)this;
        else if (siteType == "Town") town = (SiteTown)this;
        else if (siteType == "Complex") complex = (SiteComplex)this;
        else if (siteType == "SpiritHealer") spiritHealer = (SiteSpiritHealer)this;
        CDesktop.cameraDestination = new Vector2(x, y) + mapCenteringOffset;
        CDesktop.queuedSiteOpen = siteType;
        CDesktop.LockScreen();
    }
}