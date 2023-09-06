using UnityEngine;

using static Root;
using static Cursor;
using System.Linq;

public class MapGrid : MonoBehaviour
{
    void OnMouseDown()
    {
        var temp = cursor.transform.position;
        CDesktop.cameraDestination = new Vector2(temp.x - 333, temp.y + 183) / 19;
    }

    public static void EnforceBoundary()
    {
        var temp = CDesktop.cameraDestination;
        if (temp.x < 20) CDesktop.cameraDestination = new Vector2(20, temp.y);
        if (temp.x > 310) CDesktop.cameraDestination = new Vector2(310, temp.y);
        if (temp.x > 130 && temp.x < 180) CDesktop.cameraDestination = new Vector2(130, temp.y);
        if (temp.x < 225 && temp.x > 180) CDesktop.cameraDestination = new Vector2(225, temp.y);
        if (temp.y > 0) CDesktop.cameraDestination = new Vector2(temp.x, 0);
        if (temp.y < -236) CDesktop.cameraDestination = new Vector2(temp.x, -236);
        //if (CDesktop.cameraDestination.x > sites.Max(x => x.position.x) > 0) return;
        //if (amount.y > 0 && CDesktop.screen.transform.position.y - sites.Max(x => x.position.y) > 0) return;
        //if (amount.x < 0 && sites.Min(x => x.position.x) - CDesktop.screen.transform.position.x > 0) return;
        //if (amount.y < 0 && sites.Min(x => x.position.y) - CDesktop.screen.transform.position.y > 0) return;
    }
}
