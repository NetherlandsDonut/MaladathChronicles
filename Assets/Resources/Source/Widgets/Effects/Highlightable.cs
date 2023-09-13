using UnityEngine;

using static Root;
using static Root.CursorType;

using static Cursor;
using System.Linq.Expressions;

public class Highlightable : MonoBehaviour
{
    public Region region;
    public Tooltip tooltip;
    public SpriteRenderer render;
    public Action<Highlightable> pressEvent, rightPressEvent;
    public string pressedState;

    public void Initialise(Region region, Action<Highlightable> pressEvent, Action<Highlightable> rightPressEvent, Func<Highlightable, Action> tooltip)
    {
        render = GetComponent<SpriteRenderer>();
        pressedState = "None";
        this.pressEvent = pressEvent;
        this.rightPressEvent = rightPressEvent;
        this.tooltip = tooltip;
        this.region = region;
    }

    public void OnMouseEnter()
    {
        mouseOver = this;
        if (pressed == "None" && tooltip != null)
            CDesktop.SetTooltip(tooltip);
        if (GetComponent<InputCharacter>() != null)
            cursor.SetCursor(Write);
        else if (pressed != "None") cursor.SetCursor(Click);
        render.color -= new Color(0.1f, 0.1f, 0.1f, 0);
        if (pressed != "None") render.color -= new Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseExit()
    {
        if (mouseOver == this)
            mouseOver = null;
        CloseWindow("Tooltip");
        CDesktop.tooltip = null;
        cursor.SetCursor(Default);
        render.color += new Color(0.1f, 0.1f, 0.1f, 0);
        if (pressed != "None") render.color += new Color(0.1f, 0.1f, 0.1f, 0);
    }
 
    public void OnMouseDown(int key = 0)
    {
        pressedState = key == 0 ? "Left" : (key == 1 ? "Right" : "Middle");
        CloseWindow("Tooltip");
        CDesktop.tooltip = null;
        cursor.SetCursor(Click);
        render.color -= new Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseUp()
    {
        cursor.SetCursor(Default);
        render.color = new Color(1f, 1f, 1f, 1f);
        pressedState = "None";
    }
}
