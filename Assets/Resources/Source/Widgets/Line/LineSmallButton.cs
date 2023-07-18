using System;
using UnityEngine;
using static Root;

public class LineSmallButton : MonoBehaviour
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
        if (tooltip != null)
            this.tooltip = new Tooltip(() => GetComponent<Highlightable>(), tooltip);

        region.LBSmallButton = this;
        region.smallButtons.Add(this);
    }

    public void OnMouseUp()
    {
        if (cursor.render.sprite == null) return;
        if (pressEvent != null)
        {
            region.regionGroup.window.PlaySound("DesktopButtonPress", 0.6f);
            pressEvent(GetComponent<Highlightable>());
            region.regionGroup.window.Rebuild();
        }
    }
}
