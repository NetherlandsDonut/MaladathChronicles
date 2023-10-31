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
        if (currentSave.currentSite != name)
        {
            var refreshWindow = CDesktop.windows.Find(x => x.title == "Site: " + currentSave.currentSite);
            currentSave.currentSite = name;
            if (refreshWindow != null) refreshWindow.Respawn();
        }
        if (siteType == "Instance") instance = (SiteInstance)this;
        else if (siteType == "HostileArea") area = (SiteHostileArea)this;
        else if (siteType == "Town") town = (SiteTown)this;
        else if (siteType == "Complex") complex = (SiteComplex)this;
        else if (siteType == "SpiritHealer") spiritHealer = (SiteSpiritHealer)this;
        CDesktop.cameraDestination = new Vector2(x, y) * mapGridSize;
        CDesktop.queuedSiteOpen = siteType;
        CDesktop.LockScreen();
    }

    //Queue opening of this site.
    //After calling this the screen is locked and camera will pan there slowly.
    //After reaching the site the screen will change accordingly to the site type
    public void QueueSitePathTravel()
    {
        CDesktop.queuedPath = new();
        var site = FindSite(x => x.name == currentSave.currentSite);
        var newPoint = new Vector2(site.x, site.y) * 19;
        foreach (var path in pathsDrawn)
        {
            var queue = path.GetComponentsInChildren<Transform>().ToList().FindAll(x => x.name == "PathDot");
            if (queue.Count > 0)
            {
                if (queue.Count > 1 && Vector2.Distance(queue[0].transform.position, newPoint) > Vector2.Distance(queue.Last().transform.position, newPoint)) queue.Reverse();
                newPoint = queue.Last().position;
                CDesktop.queuedPath.AddRange(queue);
            }
        }
        var current = currentSave.currentSite;
        currentSave.currentSite = "";
        Respawn("Site: " + current);
        CDesktop.queuedSiteOpen = name;
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
            pathBuilder = new() { new Vector2(x * 19, y * 19) };
            sitePathBuilder = this;
        }
        else if (sitePathBuilder == this)
        {
            sitePathBuilder = null;
            var pathsFound = pathsDrawn.FindAll(x => x.name.Contains("\"" + name + "\""));
            pathsDrawn.RemoveAll(x => pathsFound.Contains(x));
            for (int i = 0; i < pathsFound.Count; i++)
                UnityEngine.Object.Destroy(pathsFound[i]);
            paths.RemoveAll(x => x.sites.Any(y => pathsFound.Exists(z => z.name.Contains("\"" + y + "\""))));
        }
        else
        {
            pathBuilder.Add(new Vector2(x * 19, y * 19));
            paths.Add(new SitePath()
            {
                sites = new() { sitePathBuilder.name, name },
                points = pathBuilder.Select(x => ((int)x.x, (int)x.y)).ToList()
            });
            pathsDrawn.Add(paths.Last().DrawPath());
            sitePathBuilder = null;
        }
    }

    public void LeadPath()
    {
        siteTravelPlan = this;
        var pathing = FindShortestPath(FindSite(x => x.name == currentSave.currentSite), this);
        if (pathing != null)
            foreach (var path in pathing)
                pathsDrawn.Add(path.DrawPath());
        else siteTravelPlan = null;
    }

    public void ExecutePath(string siteType)
    {
        if (currentSave.currentSite != name) QueueSitePathTravel();
        else QueueSiteOpen(siteType);
    }

    //Site selected as the beginning of the path building
    public static Site sitePathBuilder;

    //Site selected as the beginning of the path building
    public static Site siteTravelPlan;

    //List of points set during construction of a path between sites
    public static List<Vector2> pathBuilder;

    //Finds a site using a predicate
    public static Site FindSite(Predicate<Site> predicate)
    {
        var area = areas.Find(predicate);
        if (area != null) return area;
        var town = towns.Find(predicate);
        if (town != null) return town;
        var instance = instances.Find(predicate);
        if (instance != null) return instance;
        var complex = complexes.Find(predicate);
        if (complex != null) return complex;
        return null;
    }
}