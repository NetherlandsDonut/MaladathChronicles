using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer render;
    public Sprite[] sprites;
    public float timer, time;
    public int index;
    public bool globalTimer;

    public static int globalIndex;

    public void Initiate(string what, bool global, float time = 0.02f)
    {
        sprites = Resources.LoadAll<Sprite>(what);
        render = GetComponent<SpriteRenderer>();
        globalTimer = global;
        this.time = time;
        if (!globalTimer) render.sprite = sprites[index];
        else render.sprite = sprites[globalIndex % sprites.Length];
    }

    public void FixedUpdate()
    {
        if (!globalTimer)
        {
            if (timer > 0) timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = time;
                render.sprite = sprites[index++];
                if (index == sprites.Length) index = 0;
            }
        }
        else render.sprite = sprites[globalIndex % sprites.Length];
    }
}
