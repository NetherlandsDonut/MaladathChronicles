using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public SpriteRenderer render;
    public Sprite[] sprites;
    public float timer;
    public int index;

    public void Initiate(string what)
    {
        sprites = Resources.LoadAll<Sprite>(what);
        render = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0.01f;
            render.sprite = sprites[index++];
            if (index == sprites.Length) index = 0;
        }
    }
}
