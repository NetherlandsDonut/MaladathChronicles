using UnityEngine;

public class Blinking : MonoBehaviour
{
    public bool blinked;
    public float counter;
    public SpriteRenderer render;

    void Start() => render = GetComponent<SpriteRenderer>();

    void FixedUpdate()
    {
        if (counter > 0) counter -= Time.deltaTime;
        if (counter <= 0)
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b, blinked ? 0 : 1);
            blinked ^= true;
            counter = 0.6f;
        }
    }
}
