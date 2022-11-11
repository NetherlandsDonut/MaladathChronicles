using UnityEngine;

using static Root;

public class Cursor : MonoBehaviour
{
    public SpriteRenderer render;

    void Awake() => UnityEngine.Cursor.visible = false;
    void Start() => render = GetComponent<SpriteRenderer>();

    void Update()
    {
        var curScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var curPosition = (Vector2)Camera.main.ScreenToWorldPoint(curScreenSpace);
        transform.position = new Vector3((int)curPosition.x, (int)curPosition.y, transform.position.z);
    }

    public void SetCursor(CursorType cursor)
    {
        if (cursor == CursorType.None) render.sprite = null;
        else render.sprite = Resources.Load<Sprite>("Sprites/Cursors/" + cursor);
    }
}
