using UnityEngine;

using static Root;

public class WindowAnchor
{
   public WindowAnchor(Anchor anchor, float x = 0, float y = 0, Window magnet = null)
   {
      this.anchor = anchor;
      offset = new Vector2(x, y);
      this.magnet = magnet;
   }

   public Vector2 offset;
   public Anchor anchor;
   public Window magnet;
}
