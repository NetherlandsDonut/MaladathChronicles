using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public float counter;
    public SpriteRenderer render;

    void Start()
    {
        if (gameObject.name.Contains("PathDot")) counter = 0.25f;
        else
        {
            var r = new System.Random((gameObject.transform.childCount == 0 ? gameObject.transform.parent : gameObject.transform).GetInstanceID());
            counter = r.Next(10, 30) * 0.01f + 0.3f;
        }
        render = GetComponent<SpriteRenderer>();
        render.color = new Color(render.color.r, render.color.g, render.color.b, 0);
    }

    void FixedUpdate()
    {
        if (counter > 0)
            counter -= Time.deltaTime;
        if (counter <= 0)
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b, render.color.a + 0.1f);
            if (render.color.a == 1) Destroy(this);
            counter = 0.01f;
        }
    }
}
