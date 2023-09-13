using System;
using UnityEngine;

using static Sound;
using static Cursor;

public class LineBigButton : MonoBehaviour
{
    public Region region;
    public GameObject frame;
    public string buttonType;

    public void Initialise(Region region, string buttonType, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip)
    {
        this.region = region;
        this.buttonType = buttonType;
        this.pressEvent = pressEvent;
        this.rightPressEvent = rightPressEvent;
        if (tooltip != null)
            this.tooltip = new Tooltip(() => GetComponent<Highlightable>(), tooltip);
        region.LBBigButton = this;
        region.bigButtons.Add(this);
    }

    public void OnMouseUp()
    {
        if (cursor.render.sprite == null) return;
        if (pressEvent != null && GetComponent<Highlightable>().over)
        {
            PlaySound("DesktopButtonPress", 0.6f);
            pressEvent(GetComponent<Highlightable>());
            region.regionGroup.window.Rebuild();
        }
    }
}
