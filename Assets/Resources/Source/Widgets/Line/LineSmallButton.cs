using System;
using UnityEngine;

using static Root;
using static Sound;
using static Cursor;

public class LineSmallButton : MonoBehaviour
{
    public Region region;
    public GameObject frame;
    public string buttonType;

    public void Initialise(Region region, string buttonType)
    {
        this.region = region;
        this.buttonType = buttonType;
        this.pressEvent = pressEvent;
        this.rightPressEvent = rightPressEvent;
        if (tooltip != null)
            this.tooltip = new Tooltip(() => GetComponent<Highlightable>(), tooltip);
        region.LBSmallButton = this;
        region.smallButtons.Add(this);
    }

    public void OnMouseUp()
    {
        if (cursor.render.sprite == null) return;
        if (pressEvent != null && GetComponent<Highlightable>().over)
        {
            PlaySound(buttonType == "OtherClose" ? "DesktopButtonClose" : (buttonType == "ActionReroll" ? ("DesktopReroll" + random.Next(1, 3)) : "DesktopButtonPress"), 0.6f);
            pressEvent(GetComponent<Highlightable>());
            if (CDesktop.windows.Contains(region.regionGroup.window))
                region.regionGroup.window.Rebuild();
        }
    }

    public void OnRightMouseUp()
    {
        if (cursor.render.sprite == null) return;
        if (rightPressEvent != null && GetComponent<Highlightable>().over)
        {
            PlaySound(buttonType == "OtherClose" ? "DesktopButtonClose" : (buttonType == "ActionReroll" ? ("DesktopReroll" + random.Next(1, 3)) : "DesktopButtonPress"), 0.6f);
            pressEvent(GetComponent<Highlightable>());
            if (CDesktop.windows.Contains(region.regionGroup.window))
                region.regionGroup.window.Rebuild();
        }
    }
}
