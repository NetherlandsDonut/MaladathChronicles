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
    public void DrawPath()
    {
        if (sites.Count != 2) return;
        sites = sites.OrderBy(x => x.name).ToList();
        var a = sites[0];
        var b = sites[1];
        var pathName = "Path between " + a.name + " and " + b.name;
        if (path.Exists(x => x.name == pathName)) return;
        var path = new GameObject(pathName);
        pathsDrawn.Add(path);
        for (int i = 0; i < points.Count - 1; i++)
            PathStep(points[i], points[i + 1]);

        void PathStep((int, int) a, (int, int) b)
        {
            var start = new Vector2(a.Item1 * 19, a.Item2 * 19);
            var end = new Vector2(b.Item1 * 19, b.Item2 * 19);
            for (int stepsMade = 0; (int)Vector2.Distance(start, end) >= stepsMade; )
            {
                var dot = new GameObject("PathDot", typeof(SpriteRenderer));
                dot.transform.parent = path.transform;
                dot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/PathDot");
                dot.GetComponent<SpriteRenderer>().color = Color.black;
                dot.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
                dot.transform.position = Vector2.Lerp(start, end, 1 / Vector2.Distance(start, end) * stepsMade);
                dot.transform.position = new Vector2((int)dot.transform.position.x, (int)dot.transform.position.y);
                stepsMade += 6;
            }
        }
    }
    
    //Path currently being built 
    public SitePath path;

    //EXTERNAL FILE: List containing all paths in-game
    public static List<SitePath> paths;
}