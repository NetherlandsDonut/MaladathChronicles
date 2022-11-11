using System;
using UnityEngine;

using static Root.RegionBackgroundType;

public class LineCheckbox : MonoBehaviour
{
    //Parent
    public Region region;

    //Children
    public GameObject frame;

    //Fields
    public Bool value;

    public void Initialise(Region region, Bool value)
    {
        this.value = value;
        this.region = region;

        region.checkbox = this;
    }

    public void OnMouseUp()
    {
        if (region.backgroundType != Handle && region.backgroundType != Button)
        {
            value.Invert();
            region.regionGroup.window.Rebuild();
        }
    }
}
