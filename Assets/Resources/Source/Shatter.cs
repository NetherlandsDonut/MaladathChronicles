using UnityEngine;
using System.Collections;

public class Shatter : MonoBehaviour
{
    public float time;
    public SpriteRenderer render;

    public void Initiate(float time)
    {
        this.time = time;
        render = GetComponent<SpriteRenderer>();
        StartCoroutine(SelfDestruct(time));
    }

    public void Update()
    {
        if (render != null)
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b, render.color.a - (0.2f * time));
            if (render.color.a <= 0) Destroy(gameObject);
        }
    }

    public IEnumerator SelfDestruct(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
