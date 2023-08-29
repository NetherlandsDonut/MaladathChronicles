using UnityEngine;

using static Root;
using static Root.CursorType;

using static Cursor;

public class Highlightable : MonoBehaviour
{
    public Region region;
    public Window window;
    public SpriteRenderer render;
    public bool over, pressed;

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
        if (FindTooltip() != null)
            if (window == null) transform.parent.GetComponent<Desktop>().SetTooltip(FindTooltip());
            else window.desktop.SetTooltip(FindTooltip());
        if (GetComponent<InputCharacter>() != null) cursor.SetCursor(Write);
        render.color -= new Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseExit()
    {
        if (cursor.render.sprite == null) return;
        if (FindTooltip() != null)
        {
            if ((window == null ? transform.parent.GetComponent<Desktop>() : window.desktop).tooltip == FindTooltip())
            {
                if (FindTooltip().window != null)
                {
                    //Sound.PlaySound("DesktopTooltipHide", 0.1f); 
                    CloseWindow(FindTooltip().window);
                }
                else (window == null ? transform.parent.GetComponent<Desktop>() : window.desktop).tooltip = null;
            }
        }
        if (!pressed) cursor.SetCursor(Default);
        render.color += new Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseDown()
    {
        if (cursor.render.sprite == null) return;
        pressed = true;
        if (FindTooltip() != null)
        {
            if ((window == null ? transform.parent.GetComponent<Desktop>() : window.desktop).tooltip == FindTooltip())
            {
                if (FindTooltip().window != null)
                {
                    //Sound.PlaySound("DesktopTooltipHide", 0.1f);
                    CloseWindow(FindTooltip().window);
                }
                else (window == null ? transform.parent.GetComponent<Desktop>() : window.desktop).tooltip = null;
            }
        }
        if (window == null) transform.parent.GetComponent<Desktop>().tooltip = null;
        else window.desktop.tooltip = null;
        cursor.SetCursor(Click);
        render.color -= new Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseUp()
    {
        if (cursor.render.sprite == null) return;
        cursor.SetCursor(Default);
        if (pressed) render.color += new Color(0.1f, 0.1f, 0.1f, 0);
        pressed = false;
    }
}
