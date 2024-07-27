using UnityEngine;

public class FadeIn : MonoBehaviour
{
   public float counter;
   public bool initialised, random = true;
   public SpriteRenderer render;

   void Start()
   {
      render = GetComponent<SpriteRenderer>();
      render.color = new Color(render.color.r, render.color.g, render.color.b, 0);
   }

   void Update()
   {
      if (!initialised)
      {
         initialised = true;
         if (!random || gameObject.name.Contains("PathDot")) counter = 0.2f;
         else
         {
            var r = new System.Random((gameObject.transform.childCount == 0 ? gameObject.transform.parent : gameObject.transform).GetInstanceID());
            counter = r.Next(10, 30) * 0.01f + 0.3f;
         }
      }
      if (counter > 0) counter -= Time.deltaTime;
      if (counter <= 0)
      {
         render.color = new Color(render.color.r, render.color.g, render.color.b, render.color.a + 0.1f);
         if (render.color.a == 1) Destroy(this);
         counter = 0.015f;
      }
   }
}
