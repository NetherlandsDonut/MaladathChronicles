using System.Linq;

using UnityEngine;

using static Root;
using static Sound;
using static Cursor;

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
    }

    //Switches map textures between ghost realm and normal world map
    public void SwitchMapTexture(bool deadOn)
    {
        CDesktop.windows.FindAll(x => x.title.StartsWith("Site: ")).ForEach(x => x.gameObject.SetActive(!x.title.Contains("SpiritHealer") ^ deadOn));
        if (deadOn) PlayAmbience("AmbienceGhost");
        if (ambience.clip != null)
        {
            var temp = ambience.clip.name;
            if (ambience.clip.name == "AmbienceGhost") StopAmbience();
        }
        texture.SetActive(!deadOn);
        textureDead.SetActive(deadOn);
        foreground.SetActive(!deadOn);
        foregroundDead.SetActive(deadOn);
    }

    public static int mapGridSize = 19;
    public static Vector2 mapCenteringOffset = new Vector2(-17, 9);

    //Bounds camera to be in a specified proximity of any sites in reach
    //Whenever camera is close enough to detect sites it will be dragged to their proximity
    public static void EnforceBoundary(int detectionRange = 700, int maxDistance = 250, float harshness = 0.001f)
    {
        var cameraDestinationScaled = CDesktop.cameraDestination * 19 + new Vector2(333, -183);
        var nearbySites = cameraBoundaryPoints.Select(x => (x.position, Vector2.Distance(new Vector2(x.position.x, x.position.y), cameraDestinationScaled))).ToList().FindAll(x => x.Item2 < detectionRange).OrderBy(x => x.Item2).ToList();
        if (nearbySites.Count > 0)
        {
            while (Vector2.Distance(new Vector2(nearbySites[0].position.x, nearbySites[0].position.y), cameraDestinationScaled) > maxDistance)
                cameraDestinationScaled = Vector3.Lerp(cameraDestinationScaled, nearbySites[0].position, harshness);
            CDesktop.cameraDestination = (cameraDestinationScaled - new Vector2(333, -183)) / 19;
        }
    }
}
