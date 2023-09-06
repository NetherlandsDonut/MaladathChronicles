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
}
