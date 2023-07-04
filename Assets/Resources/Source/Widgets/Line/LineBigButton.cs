using System;
using UnityEngine;

using static Root;

public class LineBigButton : MonoBehaviour
{
    //Parent
    public Region region;

    //Children
    public GameObject frame;

    //Fields
    public Action<Highlightable> pressEvent;
    public BigButtonTypes buttonType;

    public void Initialise(Region region, BigButtonTypes buttonType, Action<Highlightable> pressEvent)
    {
        this.region = region;
        this.buttonType = buttonType;
        this.pressEvent = pressEvent;

        region.LBBigButton = this;
        region.bigButtons.Add(this);
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
