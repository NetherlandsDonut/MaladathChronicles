using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Race;
using static Root;
using static MapGrid;
using static SiteArea;
using static SaveGame;
using static SitePath;
using static SiteComplex;
using static SiteInstance;
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
    //It's mostly utilised by areas
    public string faction;

    //Music track that will start playing when
    //the site is opened by the player
    public List<string> musicDay, musicNight;

    //Ambience track that will start playing when
    //the site is opened by the player
    public string ambienceDay, ambienceNight;

    //If set, when flight travelling to this site,
    //on map you will be put on this specified site
    public string convertDestinationTo;

    //Areas during nighttime usually change visuals.
    //Sites marked with this boolean as true keep it always the same.
    public bool noNightVariant;

    //Is fishing possible at this site
    public bool fishing;

    //Automatically calculated number that suggests
    //at which level player should enter this area
    [NonSerialized] public Dictionary<string, int> recommendedLevel;

    //Determines whether this area is part of an instance
    [NonSerialized] public bool instancePart;

    //Determines whether this area is part of a complex
    [NonSerialized] public bool complexPart;

    //List of quests that can be started here
    [NonSerialized] public List<Quest> questsStarted;

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
            var fT = areas.Find(x => x.name == name);
            if (fT != null) siteType = "Area";
            var fC = complexes.Find(x => x.name == name);
            if (fC != null) siteType = "Complex";
        }
        if (siteType == "Instance")
        {
            instance = (SiteInstance)this;
            area = null;
            complex = null;
            spiritHealer = null;
            if (staticPagination.ContainsKey("Instance"))
                staticPagination.Remove("Instance");
        }
        else if (siteType == "Area")
        {
            instance = null;
            area = (SiteArea)this;
            complex = null;
            spiritHealer = null;
        }
        else if (siteType == "Complex")
        {
            instance = null;
            area = null;
            complex = (SiteComplex)this;
            spiritHealer = null;
        }
        else if (siteType == "SpiritHealer")
        {
            instance = null;
            area = null;
            complex = null;
            spiritHealer = (SiteSpiritHealer)this;
        }
        mapGrid.queuedSiteOpen = name;
        mapGrid.queuedSiteTypeOpen = siteType;
        CDesktop.cameraDestination = new Vector2(x, y);
        CDesktop.LockScreen();
    }

    //Queue opening of this site.
    //After calling this the screen is locked and camera will pan there slowly.
    //After reaching the site the screen will change accordingly to the site type
    public void QueueSitePathTravel()
    {
        mapGrid.queuedPath = new();
        if (currentSave.currentSite == "")
        {
            var path = pathsDrawn[0];
            var site = FindSite(x => x.name == path.Item1.sites.Find(y => y != mapGrid.queuedSiteOpen));
            var newPoint = new Vector2(site.x, site.y);
            if (path.Item2 != null)
            {
                var queue = path.Item2.GetComponentsInChildren<Transform>().ToList().FindAll(x => x.name == "PathDot");
                if (queue.Count > 0)
                {
                    if (queue.Count > 1 && Vector2.Distance(queue[0].transform.position, newPoint) < Vector2.Distance(queue.Last().transform.position, newPoint)) queue.Reverse();
                    var forRemoval = queue.Take(queue.Count - pointsForRetreat - 2).ToList();
                    for (int i = 0; i < forRemoval.Count; i++)
                        UnityEngine.Object.Destroy(forRemoval[i].gameObject);
                    queue = queue.TakeLast(pointsForRetreat + 2).ToList();
                    if (queue.Count > 0) newPoint = queue.Last().position;
                    mapGrid.queuedPath.Add((path.Item1, queue));
                }
            }
        }
        else
        {
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

            //Respawn the site the player was at a moment ago so that it doesn't have
            //the green arrow indicating that the player would be still there
            Respawn("Site: " + current);
        }

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
            paths.Add(new SitePath()
            {
                sites = new() { sitePathBuilder.name, name },
                points = pathBuilder.Select(x => ((int)x.x, (int)x.y)).ToList()
            });
            paths.Last().Initialise();
            if (pathsConnectedToSite.ContainsKey(sitePathBuilder.name))
            {
                pathsConnectedToSite[sitePathBuilder.name].RemoveAll(x => x.sites.Contains(name) && x.sites.Contains(sitePathBuilder.name));
                pathsConnectedToSite[sitePathBuilder.name].Add(paths.Last());
            }
            else pathsConnectedToSite.Add(sitePathBuilder.name, new() { paths.Last() });
            if (pathsConnectedToSite.ContainsKey(name))
            {
                pathsConnectedToSite[name].RemoveAll(x => x.sites.Contains(name) && x.sites.Contains(sitePathBuilder.name));
                pathsConnectedToSite[name].Add(paths.Last());
            }
            else pathsConnectedToSite.Add(name, new() { paths.Last() });
            pathsDrawn.Add(paths.Last().DrawPath());
            sitePathBuilder = null;
        }
    }

    //Lead a path
    public void LeadPath()
    {
        var pathing = FindPath(FindSite(x => x.name == currentSave.currentSite), this);
        if (pathing != null) foreach (var path in pathing) pathsDrawn.Add(path.DrawPath());
    }

    //Lead a path from a specific site to this one
    public void LeadPathFrom(string site)
    {
        var pathing = FindPath(FindSite(x => x.name == site), this);
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
        else if (currentSave.currentSite == name) QueueSiteOpen(siteType);
    }

    //Site selected as the beginning of the path building
    public static Site sitePathBuilder;

    //List of points set during construction of a path between sites
    public static List<Vector2> pathBuilder;

    //Finds a site using a predicate
    public static Site FindSite(Predicate<Site> predicate)
    {
        var complex = complexes.Find(predicate);
        if (complex != null) return complex;
        var instance = instances.Find(predicate);
        if (instance != null) return instance;
        var area = areas.Find(predicate);
        if (area != null) return area;
        return null;
    }
}

public class Encounter
{
    //What race this encounter is
    public string who;

    //For which side this encounter is available,
    //leave this blank to be available to everyone
    public string side;

    //Level range of the encounter,
    //when a single level is desired just leave levelMax blank
    public int levelMin, levelMax;

    //Currently selected encounter
    public static Encounter encounter;
}