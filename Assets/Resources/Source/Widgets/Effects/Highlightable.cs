using UnityEngine;


using static Root;
using static Root.CursorType;

public class Highlightable : MonoBehaviour
{
    public Region region;
    public Window window;
    public Tooltip tooltip;
    public SpriteRenderer render;
    public bool over, pressed, windowHandle;

    public void Initialise(Window window, Region region)
    {
        render = GetComponent<SpriteRenderer>();
        this.window = window;
        this.region = region;
    }

    public void OnMouseEnter()
    {
        if (cursor.render.sprite == null) return;
        if (tooltip != null) window.desktop.SetTooltip(tooltip);
        if (GetComponent<InputCharacter>() != null) cursor.SetCursor(Write);
        if (!pressed || !windowHandle)
            render.color -= new UnityEngine.Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseExit()
    {
        if (cursor.render.sprite == null) return;
        if (tooltip != null && window.desktop.tooltip == tooltip)
            if (tooltip.window != null) CloseWindow(tooltip.window);
            else window.desktop.tooltip = null;
        if (!pressed && !windowHandle) cursor.SetCursor(Default);
        if (!pressed || !windowHandle) render.color += new UnityEngine.Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseDown()
    {
        if (cursor.render.sprite == null) return;
        pressed = true;
        if (window.desktop.FocusedWindow() != window)
            window.desktop.Focus(window);
        if (windowHandle)
        {
            cursor.SetCursor(Click);
            window.dragOffset = window.transform.position - Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }
        render.color -= new UnityEngine.Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseUp()
    {
        if (cursor.render.sprite == null) return;
        cursor.SetCursor(Default);
        if (pressed) render.color += new UnityEngine.Color(0.1f, 0.1f, 0.1f, 0);
        pressed = false;
    }

    public void OnMouseDrag()
    {
        if (cursor.render.sprite == null) return;
        if (windowHandle)
        {
            cursor.SetCursor(Click);
            var curScreenSpace = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            var curPosition = (Vector2)Camera.main.ScreenToWorldPoint(curScreenSpace) + window.dragOffset;
            var t = window.transform;
            t.position = new Vector3((int)curPosition.x, (int)curPosition.y, t.position.z);
            if (t.position.x < 2) t.position = new Vector3(2, t.position.y, t.position.z);
            if (t.position.x > 956 - window.xOffset) t.position = new Vector3(956 - window.xOffset, t.position.y, t.position.z);
            if (t.position.y > -2) t.position = new Vector3(t.position.x, -2, t.position.z);
            if (t.position.y < -536 + window.PlannedHeight(true)) t.position = new Vector3(t.position.x, -536 + window.PlannedHeight(true), t.position.z);
        }
    }
}
