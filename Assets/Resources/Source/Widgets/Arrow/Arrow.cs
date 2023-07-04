using UnityEngine;

public class Arrow : MonoBehaviour
{
    //Parent
    public Window window;
    public Region aRegion, bRegion;

    //Children
    public ArrowText arrowText;
    public SpriteRenderer outLine, stretchLine, inLine;

    //Fields
    public string text;
    public bool leftSide;
}
