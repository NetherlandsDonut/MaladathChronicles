using UnityEngine;

using static Root;

public class Cursor : MonoBehaviour
{
    //Color of the cursor
    public string color;

    //Renderer of the cursor responsible for visuals
    public SpriteRenderer render;

    void Awake() => UnityEngine.Cursor.visible = false;
    void Start() => (render, color) = (GetComponent<SpriteRenderer>(), "None");

    void Update()
    {
        if (CDesktop.screenLocked) SetCursor(CursorType.Await);
        else if (render.sprite != null && IsNow(CursorType.Await)) SetCursor(CursorType.Default);
        if (CDesktop == null) return;
        var curScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var curPosition = (Vector2)CDesktop.screen.ScreenToWorldPoint(curScreenSpace);
        transform.position = new Vector3(curPosition.x, curPosition.y, transform.position.z);
    }

    //Resets the color of the cursor
    public void ResetColor()
    {
        render.color = Color.white;
        color = "None";
    }

    //Sets the color of the cursor to a different color from default white
    public void SetColor(string color)
    {
        render.color = Coloring.colors[color];
        this.color = color;
    }

    //Sets the style of the cursor
    public void SetCursor(CursorType type)
    {
        if (UnityEngine.Cursor.lockState == CursorLockMode.Locked) return;
        if (type == CursorType.None) render.sprite = null;
        else render.sprite = Resources.Load<Sprite>("Sprites/Cursor/" + type);
    }

    //Checks whether the cursor if now of a specific type
    public bool IsNow(CursorType type)
    {
        if (render.sprite == null) return type == CursorType.None;
        return render.sprite.texture.name.Contains(type.ToString());
    }
    
    //Player cursor in the game
    public static Cursor cursor;
}
