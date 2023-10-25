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
using System;

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

    public List<string> connections;

    [NonSerialized] public List<Site> connectionsLoaded;

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

    public static Site siteConnect;

    public void DrawPath(Site b)
    {
        var start = new Vector2(x * 19, y * 19);
        var end = new Vector2(b.x * 19, b.y * 19);
        int stepsMade = 0;
        var path = new GameObject("Path");
        path.transform.position = Vector2.Lerp(start, end, 0.5f);
        while ((int)Vector2.Distance(start, end) >= stepsMade)
        {
            var dot = new GameObject("PathDot", typeof(SpriteRenderer));
            dot.transform.parent = path.transform;
            dot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/PathDot");
            dot.GetComponent<SpriteRenderer>().color = Color.black;
            dot.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
            dot.transform.position = Vector2.Lerp(start, end, 1 / Vector2.Distance(start, end) * stepsMade);
            dot.transform.position = new Vector2((int)dot.transform.position.x, (int)dot.transform.position.y);
            stepsMade += 5;
        }
    }

    public void LoadConnections()
    {
        if (connections == null) return;
        if (connections.Count == 0) { connections = null; return; }
        connectionsLoaded = new();
        foreach (var connection in connections)
        {
            Site find = towns.Find(x => x.name == connection);
            if (find != null) { if (find.name.GetHashCode() < name.GetHashCode()) DrawPath(find); connectionsLoaded.Add(find); continue; }
            find = areas.Find(x => x.name == connection);
            if (find != null) { if (find.name.GetHashCode() < name.GetHashCode()) DrawPath(find); connectionsLoaded.Add(find); continue; }
            find = instances.Find(x => x.name == connection);
            if (find != null) { if (find.name.GetHashCode() < name.GetHashCode()) DrawPath(find); connectionsLoaded.Add(find); continue; }
            find = complexes.Find(x => x.name == connection);
            if (find != null) { if (find.name.GetHashCode() < name.GetHashCode()) DrawPath(find); connectionsLoaded.Add(find); continue; }
            Debug.Log("Coulnd't find a site named \"" + connection + "\"");
        }
    }
}