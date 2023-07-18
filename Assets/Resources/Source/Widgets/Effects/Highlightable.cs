using UnityEngine;

using static Root;
using static Root.CursorType;

public class Highlightable : MonoBehaviour
{
    public Region region;
    public Window window;
    public SpriteRenderer render;
    public bool over, pressed, windowHandle;

    public void Initialise(Window window, Region region)
    {
        render = GetComponent<SpriteRenderer>();
        this.window = window;
        this.region = region;
    }

    public Tooltip FindTooltip()
    {
        if (TryGetComponent<LineSmallButton>(out var smallButton)) return smallButton.tooltip;
        if (TryGetComponent<LineBigButton>(out var bigButton)) return bigButton.tooltip;
        return region.tooltip;
    }

    public void OnMouseEnter()
    {
        if (cursor.render.sprite == null) return;
        if (FindTooltip() != null)
            window.desktop.SetTooltip(FindTooltip());
        if (GetComponent<InputCharacter>() != null) cursor.SetCursor(Write);
        if (!pressed || !windowHandle)
            render.color -= new UnityEngine.Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseExit()
    {
        if (cursor.render.sprite == null) return;
        if (FindTooltip() != null && window.desktop.tooltip == FindTooltip())
            if (FindTooltip().window != null) CloseWindow(FindTooltip().window);
            else window.desktop.tooltip = null;
        if (!pressed && !windowHandle) cursor.SetCursor(Default);
        if (!pressed || !windowHandle) render.color += new UnityEngine.Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseDown()
    {
        if (cursor.render.sprite == null) return;
        pressed = true;
        window.desktop.tooltip = null;
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
            if (t.position.x > screenX - 4 - window.xOffset) t.position = new Vector3(screenX - 4 - window.xOffset, t.position.y, t.position.z);
            if (t.position.y > -2) t.position = new Vector3(t.position.x, -2, t.position.z);
            if (t.position.y < 4 - screenY + window.PlannedHeight(true)) t.position = new Vector3(t.position.x, 4 - screenY + window.PlannedHeight(true), t.position.z);
        }
    }
}
