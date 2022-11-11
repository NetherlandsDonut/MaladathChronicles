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
    public Action<Highlightable> pressEvent;
    public SmallButtonTypes buttonType;

    public void Initialise(Region region, SmallButtonTypes buttonType, Action<Highlightable> pressEvent)
    {
        this.region = region;
        this.buttonType = buttonType;
        this.pressEvent = pressEvent;

        region.LBSmallButton = this;
        region.smallButtons.Add(this);
    }

    public void OnMouseUp()
    {
        if (cursor.render.sprite == null) return;
        if (pressEvent != null)
        {
            pressEvent(GetComponent<Highlightable>());
            region.regionGroup.window.Rebuild();
        }
    }
}
