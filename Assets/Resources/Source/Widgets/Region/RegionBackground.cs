using UnityEngine;

public class RegionBackground : MonoBehaviour
{
    //The region this background belongs to
    public Region region;

    public void OnMouseUp() => region.OnMouseUp();
    public void Initialise(Region region) => this.region = region;
}
