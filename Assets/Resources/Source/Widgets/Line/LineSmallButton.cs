using System;
using UnityEngine;

using static Root;
using static Sound;
using static Cursor;

public class LineSmallButton : MonoBehaviour
{
    public Region region;
    public GameObject frame;
    public Tooltip tooltip;
    public Action<Highlightable> pressEvent;
    public string buttonType;

    public void Initialise(Region region, string buttonType, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip)
    {
        this.region = region;
        this.buttonType = buttonType;
        this.pressEvent = pressEvent;
        if (tooltip != null)
            this.tooltip = new Tooltip(() =>
            {
                if (gameObject != null)
                    return GetComponent<Highlightable>();
                else return null;
            }, tooltip);

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
}
