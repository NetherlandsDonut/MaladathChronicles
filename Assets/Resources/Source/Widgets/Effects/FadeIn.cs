using UnityEngine;

public class FadeIn : MonoBehaviour
{
    //Time between a step was done in making the object fade in
    public float timer;

    //Has the fade in been already initialised?
    public bool initialised;

    //Is the time for the fading in a bit randomised?
    public bool random = true;

    //Main render of the fader that is impacted
    public SpriteRenderer render;

    //Collider of the object that will be active
    //only after the object will become visible
    public BoxCollider2D boxCollider;

    //Starting function for the fader
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        render = GetComponent<SpriteRenderer>();
        render.color = new Color(render.color.r, render.color.g, render.color.b, 0);
    }

    //Function responsible for initialisation
    //and fading in step by step of the object
    void Update()
    {
        if (!initialised)
        {
            if (boxCollider != null)
                boxCollider.enabled = false;
            initialised = true;
            if (!random || gameObject.name.Contains("PathDot")) timer = 0.05f;
            else
            {
                var r = new System.Random((gameObject.transform.childCount == 0 ? gameObject.transform.parent : gameObject.transform).GetInstanceID());
                timer = r.Next(10, 30) * 0.01f + 0.3f;
            }
        }
        if (timer > 0) timer -= Time.deltaTime;
        if (timer <= 0)
        {
            var amount = 0.1f + render.color.a > 1 ? 1 - render.color.a : 0.1f;
            render.color = new Color(render.color.r, render.color.g, render.color.b, render.color.a + amount);
            if (render.color.a == 1f)
            {
                if (boxCollider != null)
                    boxCollider.enabled = true;
                Destroy(this);
            }
            timer = 0.015f;
        }
    }
}
