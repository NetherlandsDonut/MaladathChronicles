using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Defines;

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

    //EXTERNAL FILE: List containing all paths in-game
    public static Dictionary<SitePath, (int distance, SitePath parent)> pathInfo;

    //Finds the path with the least points in between the two given sites
    public static List<SitePath> FindPath(Site from, Site to, bool ignoreProgress = false)
    {
        //If either the starting site or the end site don't exist, return an empty path
        if (from == null || to == null) return new();

        //The best path we are looking for
        List<SitePath> bestPath = new();

        //If no sites go out of the starting site just return an empty path
        if (!pathsConnectedToSite.ContainsKey(from.name)) return new();

        //Get all the branching paths from the starting site
        var possiblePaths = pathsConnectedToSite[from.name].ToList();

        //If we already reach the destination on the first branching
        //then just return the path that matches the condition and end it
        var findWinner = possiblePaths.Find(x => x.sites.Any(y => y == to.name));
        if (findWinner != null) return new() { findWinner };

        //From the starting branchings get the node information
        pathInfo = possiblePaths.ToDictionary(x => x, x => (x.points.Count, (SitePath)null));

        //Continue searching while best the path still wasn't chosen
        while (possiblePaths.Count > 0)
        {
            //Set the first path as the current one
            var currentPath = possiblePaths[0];

            //Remove this path from the list to not consider it again
            possiblePaths.Remove(currentPath);

            //Get sites that were connected by the parent path
            var previousSites = pathInfo[currentPath].parent?.sites;

            //We are at a place which the parent path knew about
            var whereAreWe = currentPath.sites.Find(x => previousSites == null ? x != from.name : !previousSites.Contains(x));

            //If we have never visited this site, don't continue
            if (!ignoreProgress && !SaveGame.currentSave.siteVisits.ContainsKey(whereAreWe)) continue;

            //New paths branching out from this path being taken
            var newPaths = pathsConnectedToSite[whereAreWe].Where(x => x != currentPath);

            //For each path that branches out..
            foreach (var path in newPaths)
            {
                //Add new entry to the node info if it doesn't exist yet
                if (!pathInfo.ContainsKey(path))
                {
                    //Add info about this path and calculate it's distance from the start
                    pathInfo.Add(path, (path.points.Count + pathInfo[currentPath].distance, currentPath));

                    //If this path is connected to the destination we are done
                    if (path.sites.Any(y => y == to.name))
                    {
                        if (bestPath.Count == 1 && pathInfo[bestPath[0]].distance > pathInfo[path].distance) bestPath[0] = path;
                        else if (bestPath.Count == 0) bestPath.Add(path);
                        break;
                    }

                    //Add this branched path to the list to be checked
                    if (bestPath.Count == 0 || pathInfo[bestPath[0]].distance > pathInfo[path].distance) possiblePaths.Add(path);
                }

                //If this path was taken before but is rechecked..
                else
                {
                    //If the new cost of travel is better than the last one..
                    var newCost = path.points.Count + pathInfo[currentPath].distance;
                    if (newCost < pathInfo[path].distance)
                    {
                        //Replace the old cost and parent to this one because a better path was found
                        pathInfo[path] = (newCost, currentPath);

                        //If this path isn't on the list to be checked, add it to it
                        if (!possiblePaths.Contains(path) && (bestPath.Count == 0 || pathInfo[bestPath[0]].distance > pathInfo[path].distance))
                            possiblePaths.Add(path);
                    }
                }
            }
        }

        //If best path was found there will be the final path in the bestPath list..
        if (bestPath.Count == 1)
        {
            var node = bestPath[0];
            while (pathInfo[node].parent != null)
            {
                node = pathInfo[node].parent;
                bestPath.Add(node);
            }
            bestPath.Reverse();
        }

        //Return the best path
        return bestPath;
    }
}