using UnityEngine;

public class PixelCamera : MonoBehaviour
{
	public int referenceHeight = 360;
	public int pixelsPerUnit = 1;

	private int renderWidth;
	private int renderHeight;
	private int actualWidth;
	private int actualHeight;
	
	private Camera cam;

	void Awake ()
	{
		cam = GetComponent<Camera>();
	}

	void Update()
	{
		renderHeight = referenceHeight;
		cam.orthographicSize = (renderHeight / 2) / (float)pixelsPerUnit;
		int scale = Screen.height / renderHeight;
		actualHeight = renderHeight * scale;
		renderWidth = Screen.width / scale;			
		actualWidth = renderWidth * scale;
		Rect rect = cam.rect;
		rect.width = (float)actualWidth / Screen.width;
		rect.height = (float)actualHeight / Screen.height;
		rect.x = (1 - rect.width) / 2;
		rect.y = (1 - rect.height) / 2;
		cam.rect = rect;
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture buffer = RenderTexture.GetTemporary(renderWidth, renderHeight, -1);
		buffer.filterMode = FilterMode.Point;
		source.filterMode = FilterMode.Point;
		Graphics.Blit(source, buffer);
		Graphics.Blit(buffer, destination);
		RenderTexture.ReleaseTemporary(buffer);
	}
}