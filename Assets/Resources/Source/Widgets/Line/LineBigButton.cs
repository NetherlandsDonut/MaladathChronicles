using UnityEngine;

public class LineBigButton : MonoBehaviour
{
   public Region region;
   public GameObject frame;
   public string buttonType;

   public void Initialise(Region region, string buttonType)
   {
      this.region = region;
      this.buttonType = buttonType;
      region.LBBigButton = this;
      region.bigButtons.Add(this);
   }
}
