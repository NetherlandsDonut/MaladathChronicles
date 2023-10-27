using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static MapGrid;
using static SiteTown;
using static SaveGame;
using static SitePath;
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

    public bool CanBeSeen()
    {
        if (currentSave.siteVisits.ContainsKey(name)) return true;
        return paths.FindAll(x => x.sites.Contains(name)).Any(x => x.sites.Any(y => currentSave.siteVisits.ContainsKey(y)));
    }

    public void BuildPath()
    {
        if (pathTest != null)
            UnityEngine.Object.Destroy(pathTest);
        pathTest = null;
        if (sitePathBuilder == null)
        {
            pathBuilder = new() { new Vector2(x, y) + mapCenteringOffset };
            sitePathBuilder = this;
        }
        else if (sitePathBuilder == this) sitePathBuilder = null;
        else
        {
            pathBuilder.Add(new Vector2(x, y) + mapCenteringOffset);
            paths.Add(new SitePath()
            {
                sites = new() { sitePathBuilder.name, name },
                points = pathBuilder.Select(x => ((int)x.x, (int)x.y)).ToList()
            });
            pathsDrawn.Add(paths.Last().DrawPath());
            sitePathBuilder = null;
        }
    }

    public static Site sitePathBuilder;

    //List of points set during construction of a path between sites
    public static List<Vector2> pathBuilder;
}