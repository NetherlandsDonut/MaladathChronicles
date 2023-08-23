using System;
using UnityEngine;

using static Sound;
using static Cursor;

public class LineBigButton : MonoBehaviour
{
    //Parent
    public Region region;

    //Children
    public GameObject frame;

    //Fields
    public Tooltip tooltip;
    public Action<Highlightable> pressEvent;
    public string buttonType;

    public void Initialise(Region region, string buttonType, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip)
    {
        this.region = region;
        this.buttonType = buttonType;
        this.pressEvent = pressEvent;
        if (tooltip != null && gameObject != null)
            this.tooltip = new Tooltip(() => GetComponent<Highlightable>(), tooltip);

        region.LBBigButton = this;
        region.bigButtons.Add(this);
    }

    public void OnMouseUp()
    {
        if (cursor.render.sprite == null) return;
        if (pressEvent != null)
        {
            PlaySound("DesktopButtonPress", 0.6f);
            pressEvent(GetComponent<Highlightable>());
            region.regionGroup.window.Rebuild();
        }
    }
}
