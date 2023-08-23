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
        else if (render.sprite.texture.name.Contains("Await")) SetCursor(CursorType.Default);
        var curScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var curPosition = (Vector2)CDesktop.screen.ScreenToWorldPoint(curScreenSpace);
        transform.position = new Vector3((int)System.Math.Round(curPosition.x), (int)System.Math.Round(curPosition.y), transform.position.z);
    }

    public void SetCursor(CursorType cursor)
    {
        if (cursor == CursorType.None) render.sprite = null;
        else render.sprite = Resources.Load<Sprite>("Sprites/Cursors/" + cursor);
    }
}
