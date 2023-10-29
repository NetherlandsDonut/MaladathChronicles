using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer render;
    public Sprite[] sprites;
    public float timer, time;
    public int index;

    public void Initiate(string what, float time = 0.02f)
    {
        sprites = Resources.LoadAll<Sprite>(what);
        render = GetComponent<SpriteRenderer>();
        this.time = time;
    }

    public void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = time;
            render.sprite = sprites[index++];
            if (index == sprites.Length) index = 0;
        }
    }
}
