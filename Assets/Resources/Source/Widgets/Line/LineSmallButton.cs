using UnityEngine;

public class LineSmallButton : MonoBehaviour
{
    //Sprite appearing as the button
    public string texture;

    //Frame of the button
    public GameObject frame;

    //Initialisation method
    public void Initialise(Region region, string texture)
    {
        this.texture = texture;
        region.smallButtons.Add(this);
    }
}
