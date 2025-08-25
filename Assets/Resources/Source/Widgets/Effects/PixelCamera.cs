using UnityEngine;

public class PixelCamera : MonoBehaviour
{
    public int pixelsPerUnit = 1;

    public int actualWidth = 640;
    public int actualHeight = 360;

    //private Camera cam;

    //void Awake()
    //{
    //    cam = GetComponent<Camera>();
    //}

    //void Update()
    //{
    //    cam.orthographicSize = Root.screenY / 2 / (float)pixelsPerUnit;
    //    int scale = Screen.height / Root.screenY;
    //    actualHeight = Root.screenY * scale;
    //    actualWidth = Root.screenX * scale;
    //    Rect rect = cam.rect;
    //    rect.width = (float)actualWidth / Screen.width;
    //    rect.height = (float)actualHeight / Screen.height;
    //    rect.x = (1 - rect.width) / 2;
    //    rect.y = (1 - rect.height) / 2;
    //    cam.rect = rect;
    //}

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (Root.screenX <= 0) return;
        RenderTexture buffer = RenderTexture.GetTemporary(Root.screenX, Root.screenY, -1);
        buffer.filterMode = GameSettings.settings.pixelPerfectVision.Value() ? FilterMode.Point : FilterMode.Bilinear;
        source.filterMode = GameSettings.settings.pixelPerfectVision.Value() ? FilterMode.Point : FilterMode.Bilinear;
        Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}