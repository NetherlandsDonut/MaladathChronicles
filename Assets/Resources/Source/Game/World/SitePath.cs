using UnityEngine;

using System.Linq;
using System.Collections.Generic;

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
        var name = "Path between " + a + " and " + b;
        if (pathsDrawn.Exists(x => x.name == name)) return null;
        var path = new GameObject(name);
        for (int i = 0; i < points.Count - 1; i++)
            PathStep(points[i], points[i + 1]);
        return path;

        void PathStep((int, int) a, (int, int) b)
        {
            var start = new Vector2(a.Item1 * 19, a.Item2 * 19);
            var end = new Vector2(b.Item1 * 19, b.Item2 * 19);
            var intermediatePointCount = 5;
            for (int stepsMade = 0; intermediatePointCount > stepsMade; )
            {
                var dot = new GameObject("PathDot", typeof(SpriteRenderer));
                dot.transform.parent = path.transform;
                dot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/PathDot");
                dot.GetComponent<SpriteRenderer>().color = Color.black;
                dot.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
                if (start.x == end.x || start.y == end.y)
                    dot.transform.position = Vector2.Lerp(start, end, 1f / intermediatePointCount * stepsMade);
                else if (end.x > start.x && end.y > start.y)
                    dot.transform.position = Bezier(start, (end - start) / 2 + start, (end - start) / 4 + start, end, 1f / intermediatePointCount * stepsMade);
                dot.transform.position = new Vector2((int)dot.transform.position.x, (int)dot.transform.position.y);
                stepsMade++;
            }
        }
    }
    
    //Path currently being built 
    public static GameObject pathTest;

    //EXTERNAL FILE: List containing all paths in-game
    public static List<SitePath> paths;
}