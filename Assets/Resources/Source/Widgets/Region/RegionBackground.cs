using UnityEngine;

public class RegionBackground : MonoBehaviour
{
    //Parent
    public Region region;

    public void OnMouseUp() => region.OnMouseUp();
    public void Initialise(Region region) => this.region = region;
}
