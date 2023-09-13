using UnityEngine;

using static Root;
using static Root.CursorType;

using static Cursor;
using System.Linq.Expressions;

public class Highlightable : MonoBehaviour
{
    public Region region;
    public Window window;
    public SpriteRenderer render;
    public bool pressed;

    public void Initialise(Window window, Region region)
    {
        render = GetComponent<SpriteRenderer>();
        this.window = window;
        this.region = region;
    }

    public Tooltip FindTooltip()
    {
        if (TryGetComponent<LineSmallButton>(out var smallButton))
            return smallButton.tooltip;
        if (TryGetComponent<LineBigButton>(out var bigButton))
            return bigButton.tooltip;
        if (TryGetComponent<FlyingBuff>(out var flyingBuff))
            return flyingBuff.tooltip;
        return region == null ? null : region.tooltip;
    }

    public void OnMouseEnter()
    {
        if (cursor.render.sprite == null) return;
        mouseOver = this;
        if (!pressed && FindTooltip() != null)
            CDesktop.SetTooltip(FindTooltip());
        if (GetComponent<InputCharacter>() != null)
            cursor.SetCursor(Write);
        else if (pressed) cursor.SetCursor(Click);
        render.color -= new Color(0.1f, 0.1f, 0.1f, 0);
        if (pressed) render.color -= new Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseExit()
    {
        //Not sure if this line is needed, check later
        if (cursor.render.sprite == null) return;
        SwitchMouseOver(this);
        if (mouseOver == this) mouseOver = null;
        CloseWindow("Tooltip");
        CDesktop.tooltip = null;
        cursor.SetCursor(Default);
        render.color += new Color(0.1f, 0.1f, 0.1f, 0);
        if (pressed) render.color += new Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseDown()
    {
        if (cursor.render.sprite == null) return;
        pressed = true;
        CloseWindow("Tooltip");
        CDesktop.tooltip = null;
        cursor.SetCursor(Click);
        render.color -= new Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseUp()
    {
        if (cursor.render.sprite == null) return;
        cursor.SetCursor(Default);
        if (pressed) render.color = new Color(1f, 1f, 1f, 1f);
        pressed = false;
    }
}
