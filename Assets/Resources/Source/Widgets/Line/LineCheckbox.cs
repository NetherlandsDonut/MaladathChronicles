using UnityEngine;

using static Root.RegionBackgroundType;

public class LineCheckbox : MonoBehaviour
{
    public Region region;
    public GameObject frame;
    public Bool value;

    public void Initialise(Region region, Bool value)
    {
        this.value = value;
        this.region = region;

        region.checkbox = this;
    }

    public void OnMouseUp()
    {
        if (region.backgroundType != Button && region.backgroundType != ButtonRed)
            value.Invert();
    }
}
