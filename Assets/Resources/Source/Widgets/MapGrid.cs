using System.Linq;

using UnityEngine;

using static Root;
using static Cursor;

public class MapGrid : MonoBehaviour
{
    public GameObject texture, textureDead, foreground, foregroundDead;

    void OnMouseDown()
    {
        var temp = cursor.transform.position;
        CDesktop.cameraDestination = new Vector2(temp.x - 333, temp.y + 183) / 19;
    }

    public void SwitchMapTexture(bool deadOn)
    {
        texture.SetActive(!deadOn);
        textureDead.SetActive(deadOn);
        foreground.SetActive(!deadOn);
        foregroundDead.SetActive(deadOn);
    }

    public static void EnforceBoundary()
    {
        var cameraDestinationScaled = CDesktop.cameraDestination * 19 + new Vector2(333, -183);
        cameraBoundaryPoints.RemoveAll(x => x == null);
        var nearbySites = cameraBoundaryPoints.Select(x => (x.position, Vector2.Distance(new Vector2(x.position.x, x.position.y), cameraDestinationScaled))).ToList().FindAll(x => x.Item2 < 700).OrderBy(x => x.Item2).ToList();
        if (nearbySites.Count > 0)
        {
            while (Vector2.Distance(new Vector2(nearbySites[0].position.x, nearbySites[0].position.y), cameraDestinationScaled) > 250)
                cameraDestinationScaled = Vector3.Lerp(cameraDestinationScaled, nearbySites[0].position, 0.001f);
            CDesktop.cameraDestination = (cameraDestinationScaled - new Vector2(333, -183)) / 19;
        }
    }
}
