using System;

using UnityEngine;

using static Root;
using static Root.CursorType;

using static Sound;
using static Cursor;

public class Highlightable : MonoBehaviour
{
    public Window window;
    public Region region;
    public Tooltip tooltip;
    public SpriteRenderer render;
    public Action<Highlightable> pressEvent, rightPressEvent;
    public Color defaultColor;
    public string pressedState;

    public void Initialise(Region region, Action<Highlightable> pressEvent, Action<Highlightable> rightPressEvent, Func<Highlightable, Action> tooltip)
    {
        render = GetComponent<SpriteRenderer>();
        pressedState = "None";
        this.pressEvent = pressEvent;
        this.rightPressEvent = rightPressEvent;
        if (tooltip != null)
            this.tooltip = new Tooltip(() => GetComponent<Highlightable>(), tooltip);
        this.region = region;
        if (region != null)
            window = region.regionGroup.window;
    }

    public void OnMouseEnter()
    {
        if (defaultColor.a == 0) defaultColor = GetComponent<SpriteRenderer>().color;
        if (cursor.IsNow("None")) return;
        SetMouseOver(this);
        if (pressedState == "None" && tooltip != null)
            CDesktop.SetTooltip(tooltip);
        if (GetComponent<InputCharacter>() != null)
            cursor.SetCursor(Write);
        else if (pressedState != "None") cursor.SetCursor(Click);
        render.color = defaultColor - new Color(0.1f, 0.1f, 0.1f, 0);
    }

    public void OnMouseExit()
    {
        if (cursor.IsNow("None")) return;
        SetMouseOver(null);
        CloseWindow("Tooltip");
        CDesktop.tooltip = null;
        if (cursor.IsNow("Click") || cursor.IsNow("Write"))
            cursor.SetCursor(Default);
        render.color = defaultColor;
        pressedState = "None";
    }

    public void MouseDown(string key)
    {
        if (cursor.IsNow("None")) return;
        CloseWindow("Tooltip");
        CDesktop.tooltip = null;
        cursor.SetCursor(Click);
        render.color = defaultColor - new Color(0.2f, 0.2f, 0.2f, 0);
        pressedState = key;
    }

    public void MouseUp(string key)
    {
        if (cursor.IsNow("None")) return;
        if (pressedState != key) return;
        cursor.SetCursor(Default);
        render.color = defaultColor - (mouseOver == this ? new Color(0.1f, 0.1f, 0.1f, 0) : new Color(0, 0, 0, 0));
        if (pressedState == "Left" && pressEvent != null)
        {
            PlaySound("DesktopButtonPress", 0.6f);
            pressEvent(this);
        }
        else if (pressedState == "Right" && rightPressEvent != null)
        {
            PlaySound("DesktopButtonPressRight", 0.3f);
            rightPressEvent(this);
        }
        pressedState = "None";
        if (window != null)
            window.Respawn(true);
    }
}
