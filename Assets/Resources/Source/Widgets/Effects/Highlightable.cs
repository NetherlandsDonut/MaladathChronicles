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
    public string pressedState;

    public void Initialise(Region region, Action<Highlightable> pressEvent, Action<Highlightable> rightPressEvent, Func<Highlightable, Action> tooltip)
    {
        render = GetComponent<SpriteRenderer>();
        pressedState = "None";
        window = region.regionGroup.window;
        this.pressEvent = pressEvent;
        this.rightPressEvent = rightPressEvent;
        if (tooltip != null)
            this.tooltip = new Tooltip(() => GetComponent<Highlightable>(), tooltip);
        this.region = region;
    }

    public void OnMouseEnter()
    {
        SetMouseOver(this);
        if (pressedState == "None" && tooltip != null)
            CDesktop.SetTooltip(tooltip);
        if (GetComponent<InputCharacter>() != null)
            cursor.SetCursor(Write);
        else if (pressedState != "None") cursor.SetCursor(Click);
        render.color = new Color(0.9f, 0.9f, 0.9f);
    }

    public void OnMouseExit()
    {
        SetMouseOver(null);
        CloseWindow("Tooltip");
        CDesktop.tooltip = null;
        cursor.SetCursor(Default);
        render.color = new Color(1f, 1f, 1f);
        pressedState = "None";
    }

    public void MouseDown(string key)
    {
        CloseWindow("Tooltip");
        CDesktop.tooltip = null;
        cursor.SetCursor(Click);
        render.color = new Color(0.8f, 0.8f, 0.8f);
        pressedState = key;
    }

    public void MouseUp()
    {
        cursor.SetCursor(Default);
        render.color = mouseOver == this ? new Color(0.9f, 0.9f, 0.9f) : new Color(1f, 1f, 1f);
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
        window.Respawn(true);
    }
}
