using UnityEngine;

using static Root.RegionBackgroundType;

public class LineCheckbox : MonoBehaviour
{
    //Region that this checkbox is asigned to
    public Region region;

    //Frame of the checkbox
    public GameObject frame;

    //Bool field asigned to this checkbox
    public Bool value;

    //Initialisation method
    public void Initialise(Region region, Bool value)
    {
        this.value = value;
        this.region = region;
        region.checkbox = this;
    }

    //Event called on interacting with the checkbox
    public void OnMouseUp()
    {
        //If the region is not interactable then invert the field value
        if (region.backgroundType != Button)
            if (region.backgroundType != ButtonRed)
                value.Invert();
    }
}
