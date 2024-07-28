using UnityEngine;

using static Root;

public class WindowAnchor
{
    public WindowAnchor(Anchor anchor, float x = 0, float y = 0)
    {
        this.anchor = anchor;
        offset = new Vector2(x, y);
    }

    public Vector2 offset;
    public Anchor anchor;
}
