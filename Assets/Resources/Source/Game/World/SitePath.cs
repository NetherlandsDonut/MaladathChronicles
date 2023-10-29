using UnityEngine;

using System.Linq;
using System.Collections.Generic;

using static Root;
using static MapGrid;

public class SitePath
{
    //Sites connected with a path
    //This pathing will not work unless there are two
    //and only two sites in this list!
    public List<string> sites;

    //List of all points in between the two sites
    public List<(int, int)> points;

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
}