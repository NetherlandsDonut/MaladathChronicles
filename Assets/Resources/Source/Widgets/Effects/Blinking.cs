using UnityEngine;

public class Blinking : MonoBehaviour
{
    //Did the sprite blink in last time on update?
    public bool blinked;

    //How much time has passed since the last blink in or blink out
    public float timer;
    public SpriteRenderer render;

    //Initialises the blinking object
    void Start() => render = GetComponent<SpriteRenderer>();

    //Blinks the sprite in and out
    void FixedUpdate()
    {
        if (timer > 0) timer -= Time.deltaTime;
        if (timer <= 0)
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b, blinked ? 0 : 1);
            blinked ^= true;
            timer = 0.6f;
        }
    }
}
