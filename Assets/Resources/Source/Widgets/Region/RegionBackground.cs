using UnityEngine;

public class RegionBackground : MonoBehaviour
{
    //The region this background belongs to
    public Region region;

    //public void OnMouseUp()
    //{
        //if (region.checkbox != null)
        //{
        //    region.checkbox.value.Invert();
        //    region.regionGroup.window.Rebuild();
        //}
        //if (region.pressEvent != null && GetComponent<Highlightable>().over)
        //{
        //    PlaySound("DesktopButtonPress", 0.6f);
        //    pressEvent(GetComponent<Highlightable>());
        //    region.regionGroup.window.Rebuild();
        //}
    //}

    public void Initialise(Region region) => this.region = region;
}
