using UnityEngine;

using System.Linq;
using System.Collections.Generic;

using static Root;

public class SitePath
{
    //Initialisation method to fill automatic values
    public void Initialise()
    {
        foreach (var site in sites)
            if (pathsConnectedToSite.ContainsKey(site)) pathsConnectedToSite[site].Add(this);
            else pathsConnectedToSite.Add(site, new() { this });
    }

    public List<SitePath> PathsConnected(List<string> exclude)
    {
        if (exclude.Count(x => x == sites[0]) > 1 && exclude.Count(x => x == sites[1]) > 1) return new();
        var here = sites[exclude.Count(x => x == sites[0]) == 1 ? 0 : 1];
        return pathsConnectedToSite[here].Where(x => x != this).ToList();

    }

    //Sites connected with a path
    //This pathing will not work unless there are two
    //and only two sites in this list!
    public List<string> sites;

    //List of all points in between the two sites
    public List<(int, int)> points;

    //List of all points in between the two sites
    public static Dictionary<string, List<SitePath>> pathsConnectedToSite = new();

    //List of all active paths in the world
    public static List<GameObject> pathsDrawn = new();

    //Draws the path
    public GameObject DrawPath()
    {
        sites = sites.OrderBy(x => x).ToList();
        var a = sites[0];
        var b = sites[1];
        var name = "Path between \"" + a + "\" and \"" + b + "\"";
        var findPath = pathsDrawn.Find(x => x.name == name);
        if (findPath != null)
        {
            pathsDrawn.Remove(findPath);
            Object.Destroy(findPath);
        }
        var path = new GameObject(name);
        int stepsMade = 0;
        for (int i = 0; i < points.Count - 1; i++)
            PathStep(points[i], points[i + 1]);
        return path;

        void PathStep((int, int) a, (int, int) b)
        {
            var beginSteps = stepsMade;
            var start = new Vector2(a.Item1, a.Item2);
            var end = new Vector2(b.Item1, b.Item2);
            while ((int)Vector2.Distance(start, end) >= stepsMade - beginSteps)
                if (stepsMade++ % 10 == 0)
                {
                    var dot = new GameObject("PathDot", typeof(SpriteRenderer));
                    dot.transform.parent = path.transform;
                    dot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/PathDot");
                    dot.GetComponent<SpriteRenderer>().color = Color.black;
                    dot.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
                    dot.transform.position = Vector2.Lerp(start, end, 1 / Vector2.Distance(start, end) * (stepsMade - beginSteps));
                    dot.transform.position = new Vector2((int)dot.transform.position.x, (int)dot.transform.position.y);
                    var dotBorder = new GameObject("PathDotBorder", typeof(SpriteRenderer));
                    dotBorder.transform.parent = dot.transform;
                    dotBorder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/PathDotBorder");
                    dotBorder.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
                    dotBorder.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    dotBorder.transform.localPosition = Vector3.zero;
                    var dotShadow = new GameObject("PathDotShadow", typeof(SpriteRenderer));
                    dotShadow.transform.parent = dot.transform;
                    dotShadow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/PathDotShadow");
                    dotShadow.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
                    dotShadow.GetComponent<SpriteRenderer>().sortingOrder = -2;
                    dotShadow.transform.localPosition = Vector3.zero;
                }
        }
    }
    
    //Path currently being built 
    public static GameObject pathTest;

    //EXTERNAL FILE: List containing all paths in-game
    public static List<SitePath> paths;

    //Finds the shortest path between the two given sites
    public static List<SitePath> FindShortestPath(Site from, Site to)
    {
        var timeA = System.DateTime.Now;
        (List<SitePath>, int) bestPath = (null, maxPathLength);
        var startingPoints = paths.FindAll(x => x.sites.Contains(from.name));
        var scan = new List<(List<SitePath>, int)>();
        foreach (var direction in startingPoints)
            FindPath((new() { direction }, direction.points.Count), false);
        while (scan.Count > 0 && scan.Count < 20000) FindPath(scan[0]);
        Debug.Log((System.DateTime.Now - timeA).Milliseconds);
        return bestPath.Item1;

        void FindPath((List<SitePath>, int) pathing, bool removeFromScan = true)
        {
            if (removeFromScan) scan.RemoveAt(0);
            if (pathing.Item2 >= bestPath.Item2) return;
            else if (pathing.Item1.Last().sites.Contains(to.name)) bestPath = pathing;
            else
            {
                var alreadyVisitedSites = pathing.Item1.SelectMany(x => x.sites).ToList();
                alreadyVisitedSites.Insert(0, from.name);
                var connectedPaths = pathing.Item1.Last().PathsConnected(alreadyVisitedSites);
                scan.AddRange(connectedPaths.Select(x => (pathing.Item1.Append(x).ToList(), pathing.Item2 + x.points.Count)));
            }
        }
    }
}