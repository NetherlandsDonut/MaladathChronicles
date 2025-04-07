using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static MapGrid;
using static SiteTown;
using static SaveGame;
using static SitePath;
using static SiteCapital;
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

    //If set, when flight travelling to this site,
    //on map you will be put on this specified site
    public string convertFlightPathTo;

    //Areas during nighttime usually change visuals.
    //Sites marked with this boolean as true keep it always the same.
    public bool noNightVariant;

    //Is fishing possible at this site
    public bool fishing;

    //Tells program whether this area has a special
    //clear background that is shown only after clearing the area
    public bool specialClearBackground;

    //List of possible common encounters in this area
    public List<Encounter> commonEncounters;

    //List of possible rare encounters in this area
    public List<Encounter> rareEncounters;

    //List of special elite encounters in this area
    public List<Encounter> eliteEncounters;

    //Size of the area
    public int areaSize;

    //List of of progression points in the area
    public List<AreaProgression> progression;

    //Automatically calculated number that suggests
    //at which level player should enter this area
    [NonSerialized] public int recommendedLevel;

    //Determines whether this area is part of an instance
    [NonSerialized] public bool instancePart;

    //Determines whether this area is part of a complex
    [NonSerialized] public bool complexPart;

    //List of quests that can be started here
    [NonSerialized] public List<Quest> questsStarted;

    //Instance wings that store all the instance's areas
    public List<InstanceWing> wings;

    //List of items that can drop from enemies in this instance
    public List<string> zoneDrop;

    //Additional items inside of the exploration chest
    public Dictionary<string, int> chestBonus;

    //List of all sites that this complex contains
    //Keys provide information what type of site it is
    //Values provide information what is the name of the site
    //EXAMPLE: { "SiteType": "Raid", "SiteName": "Molten Core" }
    public List<Dictionary<string, string>> sites;

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
        if (siteType == null)
        {
            var fI = instances.Find(x => x.name == name);
            if (fI != null) siteType = "Instance";
            var fH = areas.Find(x => x.name == name);
            if (fH != null) siteType = "HostileArea";
            var fT = towns.Find(x => x.name == name);
            if (fT != null) siteType = "Town";
            var fC = complexes.Find(x => x.name == name);
            if (fC != null) siteType = "Complex";
        }
        if (siteType == "Instance")
        {
            instance = (SiteInstance)this;
            if (staticPagination.ContainsKey("Instance"))
                staticPagination.Remove("Instance");
        }
        else if (siteType == "HostileArea") area = (SiteHostileArea)this;
        else if (siteType == "Town")
        {
            town = (SiteTown)this;
            if (town.capital != null)
            {
                capitalThroughTown = town;
                capital = capitals.Find(x => x.name == town.capital);
                siteType = "Capital";
            }
        }
        else if (siteType == "Complex") complex = (SiteComplex)this;
        else if (siteType == "SpiritHealer") spiritHealer = (SiteSpiritHealer)this;
        mapGrid.queuedSiteOpen = siteType;
        CDesktop.cameraDestination = new Vector2(x, y);
        CDesktop.LockScreen();
    }

    //Queue opening of this site.
    //After calling this the screen is locked and camera will pan there slowly.
    //After reaching the site the screen will change accordingly to the site type
    public void QueueSitePathTravel()
    {
        mapGrid.queuedPath = new();
        var site = FindSite(x => x.name == currentSave.currentSite);
        var newPoint = new Vector2(site.x, site.y);
        foreach (var path in pathsDrawn)
        {
            if (path.Item2 == null) continue;
            var queue = path.Item2.GetComponentsInChildren<Transform>().ToList().FindAll(x => x.name == "PathDot");
            if (queue.Count > 0)
            {
                if (queue.Count > 1 && Vector2.Distance(queue[0].transform.position, newPoint) > Vector2.Distance(queue.Last().transform.position, newPoint)) queue.Reverse();
                newPoint = queue.Last().position;
                mapGrid.queuedPath.Add((path.Item1, queue));
            }
        }

        //?
        if (pathsDrawn.Count == 0) return;

        //Save current site for later use so that we can remember where we were
        var current = currentSave.currentSite;

        //Set current site to none because the player is traveling between sites
        currentSave.currentSite = "";

        //Respawn the map location
        Respawn("MapLocationInfo");

        //Respawn the site the player was at a moment ago so that it doesn't have
        //the green arrow indicating that the player would be still there
        Respawn("Site: " + current);

        //Site to be opened
        mapGrid.queuedSiteOpen = name;

        //Don't know if it's needed
        CDesktop.LockScreen();
    }

    //Can this site be seen on map
    public bool CanBeSeen()
    {
        if (currentSave.Visited(name)) return true;
        return paths.FindAll(x => x.sites.Contains(name)).Any(x => x.sites.Any(y => currentSave.Visited(y)));
    }

    public void BuildPath()
    {
        if (pathTest.Item2 != null)
            UnityEngine.Object.Destroy(pathTest.Item2);
        pathTest = (null, null);
        if (sitePathBuilder == null)
        {
            pathBuilder = new() { new Vector2(x, y) };
            sitePathBuilder = this;
        }
        else if (sitePathBuilder == this) sitePathBuilder = null;
        else
        {
            pathBuilder.Add(new Vector2(x, y));
            paths.RemoveAll(x => x.sites.Contains(sitePathBuilder.name) && x.sites.Contains(name));
            if (pathsConnectedToSite.ContainsKey(sitePathBuilder.name))
                pathsConnectedToSite[sitePathBuilder.name].RemoveAll(x => x.sites.Contains(name));
            if (pathsConnectedToSite.ContainsKey(name))
                pathsConnectedToSite[name].RemoveAll(x => x.sites.Contains(sitePathBuilder.name));
            paths.Add(new SitePath()
            {
                sites = new() { sitePathBuilder.name, name },
                points = pathBuilder.Select(x => ((int)x.x, (int)x.y)).ToList(),
                spacing = 10
            });
            paths.Last().Initialise();
            if (pathsConnectedToSite.ContainsKey(sitePathBuilder.name))
                pathsConnectedToSite[sitePathBuilder.name].Add(paths.Last());
            else pathsConnectedToSite.Add(sitePathBuilder.name, new() { paths.Last() });
            if (pathsConnectedToSite.ContainsKey(name))
                pathsConnectedToSite[name].Add(paths.Last());
            else pathsConnectedToSite.Add(name, new() { paths.Last() });
            pathsDrawn.Add(paths.Last().DrawPath());
            sitePathBuilder = null;
        }
    }

    //Lead 
    public void LeadPath()
    {
        var pathing = FindPath(FindSite(x => x.name == currentSave.currentSite), this);
        if (pathing != null) foreach (var path in pathing) pathsDrawn.Add(path.DrawPath());
    }

    //Lead provided in the argument path
    public static void LeadPath(SitePath path, bool hidden = false)  
    {
        if (path != null) pathsDrawn.Add(path.DrawPath(hidden));
    }

    public void ExecutePath(string siteType = null)
    {
        if (currentSave.currentSite != name && pathsDrawn.Count > 0) QueueSitePathTravel();
        else QueueSiteOpen(siteType);
    }

    //Site selected as the beginning of the path building
    public static Site sitePathBuilder;

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