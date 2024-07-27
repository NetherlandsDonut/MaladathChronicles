using UnityEngine;

public class LineSmallButton : MonoBehaviour
{
   public Region region;
   public GameObject frame;
   public string buttonType;

   public void Initialise(Region region, string buttonType)
   {
      this.region = region;
      this.buttonType = buttonType;
      region.LBSmallButton = this;
      region.smallButtons.Add(this);
   }
}
