using System;

using UnityEngine;

using static Root;
using static Root.CursorType;

using static Sound;
using static Cursor;
using static SaveGame;
using static SitePath;

public class Highlightable : MonoBehaviour
{
    public Window window;
    public Region region;
    public Tooltip tooltip;

    //Saved reference to the renderer to avoid
    public SpriteRenderer render;

    //Events regarding 
    public Action<Highlightable> pressEvent, rightPressEvent, middlePressEvent;

    //Used to come back to default colour when mouse exits the collider
    public Color defaultColor;

    //Indicates the current state of the button
    public string pressedState;

    public void Initialise(Region region, Action<Highlightable> pressEvent, Action<Highlightable> rightPressEvent, Func<Highlightable, Action> tooltip, Action<Highlightable> middlePressEvent)
    {
        render = GetComponent<SpriteRenderer>();
        pressedState = "None";
        this.pressEvent = pressEvent;
        this.rightPressEvent = rightPressEvent;
        this.middlePressEvent = middlePressEvent;
        if (this != null && tooltip != null) this.tooltip = new Tooltip(() => this, tooltip);
        this.region = region;
        if (region != null) window = region.regionGroup.window;
    }

    public void OnMouseEnter()
    {
        if (defaultColor.a == 0) defaultColor = GetComponent<SpriteRenderer>().color;
        if (cursor.IsNow(None)) return;
        SetMouseOver(this);
        if (pressedState == "None" && tooltip != null)
            CDesktop.SetTooltip(tooltip);
        if (GetComponent<InputCharacter>() != null)
            cursor.SetCursor(Write);
        else if (pressedState != "None") cursor.SetCursor(Click);
        else if (gameObject.name == "FishingPool") cursor.SetCursor(Hook);
        render.color = defaultColor - new Color(0.1f, 0.1f, 0.1f, 0);
        if (window != null && window.title.StartsWith("Site: "))
        {
            if (debug && Input.GetKey(KeyCode.LeftAlt))
            {
                var site = window.title[(window.title.IndexOf("Site: ") + 6)..];
                if (pathsConnectedToSite.ContainsKey(site))
                    pathsConnectedToSite[site].ForEach(x => pathsDrawn.Add(x.DrawPath()));
            }
            else if (window.title != "Site: " + currentSave.currentSite && rightPressEvent != null) rightPressEvent(null);
        }
    }

    public void OnMouseExit()
    {
        if (cursor.IsNow(None)) return;
        SetMouseOver(null);
        if (CloseWindow("Tooltip") && spellbookResourceBars != null)
            foreach (var bar in spellbookResourceBars)
                if (bar.Value != null) bar.Value.UpdateFluidBar();
        Root.tooltip = null;
        if (cursor.IsNow(Click) || cursor.IsNow(Write) || cursor.IsNow(Hook))
            cursor.SetCursor(Default);
        render.color = defaultColor;
        pressedState = "None";
        if (window != null && window.title.StartsWith("Site: "))
        {
            for (int i = 0; i < pathsDrawn.Count; i++)
                Destroy(pathsDrawn[i].Item2);
            pathsDrawn = new();
        }
    }

    public void MouseDown(string key)
    {
        if (cursor.IsNow(None)) return;
        if (CloseWindow("Tooltip") && spellbookResourceBars != null)
            foreach (var bar in spellbookResourceBars)
                if (bar.Value != null) bar.Value.UpdateFluidBar();
        Root.tooltip = null;
        cursor.SetCursor(Click);
        render.color = defaultColor - new Color(0.2f, 0.2f, 0.2f, 0);
        pressedState = key;
    }

    public void MouseUp(string key)
    {
        if (cursor.IsNow(None)) return;
        if (pressedState != key) return;
        cursor.SetCursor(Default);
        render.color = defaultColor - (mouseOver == this ? new Color(0.1f, 0.1f, 0.1f, 0) : new Color(0, 0, 0, 0));
        if (pressedState == "Left" && pressEvent != null)
        {
            pressEvent(this);
            var l = GetComponent<LineSmallButton>();
            if (l != null && l.texture == "OtherClose") PlaySound("DesktopButtonClose");
            else PlaySound("DesktopButtonLeft", 0.8f);
        }
        else if (pressedState == "Right" && rightPressEvent != null)
        {
            rightPressEvent(this);
            PlaySound("DesktopButtonRight", 0.5f);
        }
        else if (pressedState == "Middle" && middlePressEvent != null)
        {
            middlePressEvent(this);
            PlaySound("DesktopButtonRight", 0.5f);
        }
        pressedState = "None";
        if (window != null) window.Respawn(true);
    }
}
