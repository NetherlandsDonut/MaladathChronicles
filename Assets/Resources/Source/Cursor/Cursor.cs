using UnityEngine;

using static Root;

public class Cursor : MonoBehaviour
{
    public SpriteRenderer render;

    void Awake() => UnityEngine.Cursor.visible = false;
    void Start() => render = GetComponent<SpriteRenderer>();

    void Update()
    {
        if (CDesktop.screenLocked) SetCursor(CursorType.Await);
        else if (render.sprite != null && IsNow(CursorType.Await)) SetCursor(CursorType.Default);
        var curScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var curPosition = (Vector2)CDesktop.screen.ScreenToWorldPoint(curScreenSpace);
        transform.position = new Vector3(curPosition.x, curPosition.y, transform.position.z);
    }

    public void SetCursor(CursorType type)
    {
        if (UnityEngine.Cursor.lockState == CursorLockMode.Locked) return;
        if (type == CursorType.None) render.sprite = null;
        else render.sprite = Resources.Load<Sprite>("Sprites/Cursor/" + type);
    }

    public bool IsNow(CursorType type)
    {
        if (render.sprite == null) return type == CursorType.None;
        return render.sprite.texture.name.Contains(type.ToString());
    }

    public static Cursor cursor;
}
