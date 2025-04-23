using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    //Render of the animated sprite
    public SpriteRenderer render;

    //Sprites between the object will be switching through
    public Sprite[] sprites;

    //How much time has passed since last change of the sprite
    public float timer;

    //Time that serves as the interval between swapping the sprite to a new one
    public float timeBetweenFrames;

    //Index of the sprite used at the moment
    public int index;

    //Does this animated sprite object use the global timer?
    public bool globalTimer;

    //Global index of animation updated in currently active desktop
    public static int globalIndex;

    //Initiates this animated sprite
    public void Initiate(string what, bool global, float timeBetweenFrames = 0.02f)
    {
        sprites = Resources.LoadAll<Sprite>(what);
        render = GetComponent<SpriteRenderer>();
        globalTimer = global;
        this.timeBetweenFrames = timeBetweenFrames;
        if (!globalTimer) render.sprite = sprites[index];
        else render.sprite = sprites[globalIndex % sprites.Length];
    }

    //Updates the rendered sprite to have a sprite based on animation timer
    public void FixedUpdate()
    {
        if (!globalTimer)
        {
            if (timer > 0) timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = timeBetweenFrames;
                render.sprite = sprites[index++];
                if (index == sprites.Length) index = 0;
            }
        }
        else render.sprite = sprites[globalIndex % sprites.Length];
    }
}
