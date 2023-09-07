using System.Linq;

using UnityEngine;

using static Root;
using static Cursor;

public class MapGrid : MonoBehaviour
{
    void OnMouseDown()
    {
        var temp = cursor.transform.position;
        CDesktop.cameraDestination = new Vector2(temp.x - 333, temp.y + 183) / 19;
    }

    public static void EnforceBoundary()
    {
        var cameraDestinationScaled = CDesktop.cameraDestination * 19 + new Vector2(333, -183);
        var nearbySites = sites.Select(x => (x.position, Vector2.Distance(new Vector2(x.position.x, x.position.y), cameraDestinationScaled))).ToList().FindAll(x => x.Item2 < 700).OrderBy(x => x.Item2).ToList();
        if (nearbySites.Count > 0)
        {
            while (Vector2.Distance(new Vector2(nearbySites[0].Item1.x, nearbySites[0].Item1.y), cameraDestinationScaled) > 250)
                cameraDestinationScaled = Vector3.Lerp(cameraDestinationScaled, nearbySites[0].Item1, 0.001f);
            CDesktop.cameraDestination = (cameraDestinationScaled - new Vector2(333, -183)) / 19;
        }
        
        //var temp = CDesktop.cameraDestination;
        //if (temp.x < 20) CDesktop.cameraDestination = new Vector2(20, temp.y);
        //if (temp.x > 310) CDesktop.cameraDestination = new Vector2(310, temp.y);
        //if (temp.x > 130 && temp.x < 180) CDesktop.cameraDestination = new Vector2(130, temp.y);
        //if (temp.x < 225 && temp.x > 180) CDesktop.cameraDestination = new Vector2(225, temp.y);
        //if (temp.y > 0) CDesktop.cameraDestination = new Vector2(temp.x, 0);
        //if (temp.y < -236) CDesktop.cameraDestination = new Vector2(temp.x, -236);

        //if (CDesktop.cameraDestination.x > sites.Max(x => x.position.x) > 0) return;
        //if (amount.y > 0 && CDesktop.screen.transform.position.y - sites.Max(x => x.position.y) > 0) return;
        //if (amount.x < 0 && sites.Min(x => x.position.x) - CDesktop.screen.transform.position.x > 0) return;
        //if (amount.y < 0 && sites.Min(x => x.position.y) - CDesktop.screen.transform.position.y > 0) return;
    }
}
