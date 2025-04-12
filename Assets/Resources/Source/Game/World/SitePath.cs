using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Defines;
using static SaveGame;

public class SitePath
{
    //Initialisation method to fill automatic values
    public void Initialise()
    {
        var list = pathsConnectedToSite;
        foreach (var site in sites)
            if (!list.ContainsKey(site))
                list.Add(site, new() { this });
            else if (!list[site].Contains(this))
                list[site].Add(this);
    }

    //Sites connected with a path
    //This pathing will not work unless there are two
    //and only two sites in this list!
    public List<string> sites;

    //Price of taking this path
    public int price;

    //List of all points in between the two sites
    public List<(int, int)> points;

    //Collection of all paths connected to each site
    public static Dictionary<string, List<SitePath>> pathsConnectedToSite = new();

    //Collection of all transportation paths connected to each site
    public static Dictionary<string, List<TransportRoute>> transportationConnectedToSite = new();

    //List of all active paths in the world
    public static List<(SitePath, GameObject)> pathsDrawn = new();

    //Draws the path
    public (SitePath, GameObject) DrawPath(bool hidden = false)
    {
        if (points.Count == 0) return (this, null);
        var a = sites[0];
        var b = sites[1];
        var name = "Path between \"" + a + "\" and \"" + b + "\"";
        var findPath = pathsDrawn.Find(x => x.Item2.name == name);
        if (findPath.Item2 != null)
        {
            pathsDrawn.Remove(findPath);
            Object.Destroy(findPath.Item2);
        }
        var path = new GameObject(name);
        int stepsMade = 0;
        for (int i = 0; i < points.Count - 1; i++)
            PathStep(points[i], points[i + 1]);
        return (this, path);

        void PathStep((int, int) a, (int, int) b)
        {
            var beginSteps = stepsMade;
            var start = new Vector2(a.Item1, a.Item2);
            var end = new Vector2(b.Item1, b.Item2);
            while ((int)Vector2.Distance(start, end) >= stepsMade - beginSteps)
                if (stepsMade++ % defines.spaceBetweenPathPips == 0)
                    if (hidden)
                    {
                        var dot = new GameObject("PathDot");
                        dot.transform.parent = path.transform;
                        dot.transform.position = Vector2.Lerp(start, end, 1 / Vector2.Distance(start, end) * (stepsMade - beginSteps));
                        dot.transform.position = new Vector2((int)dot.transform.position.x, (int)dot.transform.position.y);
                    }
                    else
                    {
                        var dot = new GameObject("PathDot", typeof(SpriteRenderer), typeof(FadeIn));
                        if (Site.sitePathBuilder != null) Object.Destroy(dot.GetComponent<FadeIn>());
                        dot.transform.parent = path.transform;
                        dot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/PathDot");
                        dot.GetComponent<SpriteRenderer>().color = Color.black;
                        dot.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
                        dot.transform.position = Vector2.Lerp(start, end, 1 / Vector2.Distance(start, end) * (stepsMade - beginSteps));
                        dot.transform.position = new Vector2((int)dot.transform.position.x, (int)dot.transform.position.y);
                        var dotBorder = new GameObject("PathDotBorder", typeof(SpriteRenderer), typeof(FadeIn));
                        if (Site.sitePathBuilder != null) Object.Destroy(dotBorder.GetComponent<FadeIn>());
                        dotBorder.transform.parent = dot.transform;
                        dotBorder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/PathDotBorder");
                        dotBorder.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
                        dotBorder.GetComponent<SpriteRenderer>().sortingOrder = -1;
                        dotBorder.transform.localPosition = Vector3.zero;
                        var dotShadow = new GameObject("PathDotShadow", typeof(SpriteRenderer), typeof(FadeIn));
                        if (Site.sitePathBuilder != null) Object.Destroy(dotShadow.GetComponent<FadeIn>());
                        dotShadow.transform.parent = dot.transform;
                        dotShadow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/PathDotShadow");
                        dotShadow.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
                        dotShadow.GetComponent<SpriteRenderer>().sortingOrder = -2;
                        dotShadow.transform.localPosition = Vector3.zero;
                    }
        }
    }
    
    //Path currently being built 
    public static (SitePath, GameObject) pathTest;

    //EXTERNAL FILE: List containing all paths in-game
    public static List<SitePath> paths;

    //Finds the path with the least points in between the two given sites
    public static List<SitePath> FindPath(Site from, Site to, bool ignoreProgress = false)
    {
        if (from == null || to == null) return new();
        List<SitePath> bestPath = null;
        var possiblePaths = new List<List<SitePath>>();
        if (!pathsConnectedToSite.ContainsKey(from.name)) return new();
        possiblePaths = pathsConnectedToSite[from.name].Select(x => new List<SitePath> { x }).ToList();
        while (bestPath == null && possiblePaths.Count > 0) ContinuePaths();
        return bestPath;

        void ContinuePaths()
        {
            if (!defines.fasterPathfinding)
                possiblePaths = possiblePaths.OrderBy(x => x.Sum(y => y.points.Count)).ToList();
            var initialAmount = possiblePaths.Count;
            var allVisitedSites = possiblePaths.SelectMany(y => y.SelectMany(x => x.sites)).Distinct().ToList();
            for (int i = 0; i < initialAmount; i++)
            {
                var path = possiblePaths[i];
                if (path.Last().sites.Contains(to.name))
                {
                    bestPath = path;
                    return;
                }
                var visitedSites = path.SelectMany(x => x.sites).ToList();
                var newestSite = path.Last().sites[from.name == path.Last().sites[0] || visitedSites.Count(x => x == path.Last().sites[0]) == 2 ? 1 : 0];
                if (!ignoreProgress && !currentSave.siteVisits.ContainsKey(newestSite)) continue;
                var newPaths = pathsConnectedToSite[newestSite];
                foreach (var newPath in newPaths)
                    if (!allVisitedSites.Contains(newPath.sites.Find(x => x != newestSite)))
                        possiblePaths.Add(path.Concat(new List<SitePath> { newPath }).ToList());
            }
            possiblePaths.RemoveRange(0, initialAmount);
        }
    }
}