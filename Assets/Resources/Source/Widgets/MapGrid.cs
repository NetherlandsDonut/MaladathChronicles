using System.Linq;

using UnityEngine;

using static UnityEngine.KeyCode;

using static Root;
using static Sound;
using static Cursor;
using static SitePath;

//Map grid is a class responsible for direct
//interactions with the map of the world in the scene
public class MapGrid : MonoBehaviour
{
    //Holds reference to the texture of the map
    public GameObject texture;

    //Holds reference to the texture of the map of the ghost realm
    public GameObject textureDead;

    //Holds reference to the black mask areas of the map
    public GameObject foreground;

    //Holds reference to the black mask areas of the map of the ghost realm
    public GameObject foregroundDead;

    //On mouse down pan the camera to the pressed square on the map
    void OnMouseDown()
    {
        var temp = cursor.transform.position;
        CDesktop.cameraDestination = new Vector2(temp.x - 333, temp.y + 183) / 19;
        if (sitePathBuilder != null)
        {
            pathBuilder.Add(CDesktop.cameraDestination);
            if (path != null) Destroy(path);
            path = new SitePath()
            {
                sites = new() { sitePathBuilder.name, "?" },
                points = pathBuilder.Select(x => (x.x, x.y)).ToList();
            };
            path.DrawPath();
        }
    }

    //Switches map textures between ghost realm and normal world map
    public void SwitchMapTexture(bool deadOn)
    {
        CDesktop.windows.FindAll(x => x.title.StartsWith("Site: ")).ForEach(x => x.gameObject.SetActive(!x.title.Contains("SpiritHealer") ^ deadOn));
        if (deadOn) PlayAmbience("AmbienceGhost");
        else if (ambience.clip != null && ambience.clip.name == "AmbienceGhost")
            StopAmbience();
        texture.SetActive(!deadOn);
        textureDead.SetActive(deadOn);
        foreground.SetActive(!deadOn);
        foregroundDead.SetActive(deadOn);
    }

    public static int mapGridSize = 19;
    public static Vector2 mapCenteringOffset = new(-17, 9);

    //Bounds camera to be in a specified proximity of any sites in reach
    //Whenever camera is close enough to detect sites it will be dragged to their proximity
    public static void EnforceBoundary(int detectionRange = 700, int maxDistance = 7, int maxDistanceWhileMoving = 200, float harshness = 0.0001f)
    {
        var cameraDestinationScaled = CDesktop.cameraDestination * 19 + new Vector2(333, -180);
        var nearbySites = cameraBoundaryPoints.Select(x => (x, Vector2.Distance(new Vector2(x.x, x.y), cameraDestinationScaled))).ToList().FindAll(x => x.Item2 < detectionRange).OrderBy(x => x.Item2).ToList();
        if (nearbySites.Count > 0)
        {
            //for (int i = 0; i < 100 && Vector2.Distance(new Vector2(nearbySites[0].x.x, nearbySites[0].x.y), cameraDestinationScaled) > (Input.GetKey(W) || Input.GetKey(A) || Input.GetKey(S) || Input.GetKey(D) ? maxDistanceWhileMoving : maxDistance); i++)
            while (Vector2.Distance(new Vector2(nearbySites[0].x.x, nearbySites[0].x.y), cameraDestinationScaled) > (!GameSettings.settings.snapCamera.Value() || Input.GetKey(W) || Input.GetKey(A) || Input.GetKey(S) || Input.GetKey(D) ? maxDistanceWhileMoving : maxDistance))
                cameraDestinationScaled = Vector3.Lerp(cameraDestinationScaled, nearbySites[0].x, harshness);
            CDesktop.cameraDestination = (cameraDestinationScaled - new Vector2(333, -180)) / 19;
        }
    }
}
