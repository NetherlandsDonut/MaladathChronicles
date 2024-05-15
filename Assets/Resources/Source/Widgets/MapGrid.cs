using System.Linq;

using UnityEngine;

using static Site;
using static Root;
using static Sound;
using static Cursor;
using static SaveGame;
using static SitePath;

//Map grid is a class responsible for direct
//interactions with the map of the world in the scene
public class MapGrid : MonoBehaviour
{
    //Holds reference to the texture of the map
    public SpriteRenderer texture;

    //Holds reference to the texture of the map of the ghost realm
    public SpriteRenderer textureDead;

    //When this is true desktop on update will try to change map color slowly
    public bool updateTextureColors;

    //On mouse down pan the camera to the pressed square on the map
    void OnMouseDown()
    {
        var temp = cursor.transform.position;
        CDesktop.cameraDestination = new Vector2((int)temp.x, (int)temp.y);
    }

    //Updates color of the map texture based on time
    public void UpdateTextureColors(bool instant = false)
    {
        if (currentSave == null) { updateTextureColors = false; return; }
        var aim = currentSave.IsNight() ? nightColor : dayColor;
        if (texture.color == aim) { updateTextureColors = false; return; }
        texture.color = instant ? aim : Color.Lerp(texture.color, aim, Time.deltaTime);
    }
    
    //On mouse down pan the camera to the pressed square on the map
    void Update()
    {
        if (updateTextureColors) UpdateTextureColors();
        if (sitePathBuilder != null)
        {
            var foo = builderSpacing <= 0 ? 10 : builderSpacing;
            if (Vector2.Distance(pathBuilder.Last(), cursor.transform.position - new Vector3(10, -10)) > foo)
            {
                pathBuilder.Add(cursor.transform.position - new Vector3(10, -10));
                if (pathTest.Item2 != null) Destroy(pathTest.Item2);
                pathTest = new SitePath()
                {
                    sites = new() { sitePathBuilder.name, "?" },
                    points = pathBuilder.Select(x => ((int)x.x, (int)x.y)).ToList()
                }.DrawPath();
            }
        }
    }

    //Switches map textures between ghost realm and normal world map
    public void SwitchMapTexture(bool deadOn)
    {
        CDesktop.windows.FindAll(x => x.title.StartsWith("Site: ")).ForEach(x => x.gameObject.SetActive(!x.title.Contains("SpiritHealer") ^ deadOn));
        if (deadOn) PlayAmbience("AmbienceGhost");
        else if (ambience.clip != null && ambience.clip.name == "AmbienceGhost") StopAmbience();
        texture.gameObject.SetActive(!deadOn);
        textureDead.gameObject.SetActive(deadOn);
    }

    //Bounds camera to be in a specified proximity of any sites in reach
    //Whenever camera is close enough to detect sites it will be dragged to their proximity
    public static void EnforceBoundary(int detectionRange = 700, int maxDistanceWhileMoving = 225, float harshness = 0.0001f)
    {
        var cameraDestinationScaled = CDesktop.cameraDestination;
        var nearbySites = cameraBoundaryPoints.Select(x => (x, Vector2.Distance(new Vector2(x.x, x.y), cameraDestinationScaled))).ToList().FindAll(x => x.Item2 < detectionRange).OrderBy(x => x.Item2).ToList();
        if (nearbySites.Count > 0)
        {
            while (Vector2.Distance(new Vector2(nearbySites[0].x.x, nearbySites[0].x.y), cameraDestinationScaled) > maxDistanceWhileMoving)
                cameraDestinationScaled = Vector3.Lerp(cameraDestinationScaled, nearbySites[0].x, harshness);
            CDesktop.cameraDestination = cameraDestinationScaled;
        }
    }
}
