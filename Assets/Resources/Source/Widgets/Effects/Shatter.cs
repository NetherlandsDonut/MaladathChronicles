using System;
using System.Collections;

using UnityEngine;

using static Root;

public class Shatter : MonoBehaviour
{
    public float time;
    public SpriteRenderer render;

    public void Initiate(float time, SpriteRenderer r = null)
    {
        this.time = time;
        if (r != null) render = r;
        if (render == null) StartCoroutine(SelfDestruct(time));
    }

    public void Update()
    {
        if (render != null)
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b, render.color.a - (0.3f * time));
            if (render.color.a <= 0) Destroy(gameObject);
        }
    }

    public IEnumerator SelfDestruct(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    public static void SpawnShatter(double speed, double amount, Vector3 position, string sprite, string block = "0000")
    {
        var shatter = new GameObject("Shatter", typeof(Shatter));
        shatter.GetComponent<Shatter>().Initiate(3);
        shatter.transform.parent = Board.board.window.desktop.transform;
        shatter.transform.position = position;
        shatter.layer = 1;
        var foo = Resources.Load<Sprite>("Sprites/Building/BigButtons/" + sprite);
        int x = (int)foo.textureRect.width, y = (int)foo.textureRect.height;
        var dot = Resources.Load<GameObject>("Prefabs/PrefabDot");
        var direction = RollDirection();
        if (amount > 1) amount = 1;
        else if (amount < 0) amount = 0;
        for (int i = 3; i < x - 2; i++)
            for (int j = 3; j < y - 2; j++)
                if (random.Next(0, (int)Math.Abs(amount * 10 - 10)) == 0)
                    SpawnDot(i, j, foo.texture.GetPixel(i, j));

        void SpawnDot(int c, int v, Color32 color)
        {
            var newObject = Instantiate(dot);
            newObject.GetComponent<Shatter>().Initiate(random.Next(1, 8) / 50.0f);
            newObject.transform.parent = shatter.transform;
            newObject.transform.localPosition = new Vector3(c, v);
            newObject.GetComponent<SpriteRenderer>().color = color;
            newObject.GetComponent<Rigidbody2D>().AddRelativeForce((direction / 2 + UnityEngine.Random.insideUnitCircle / 2) * (int)(100 * speed));
            if (block == "0000") direction = RollDirection();
        }

        Vector2 RollDirection()
        {
            var direction = UnityEngine.Random.insideUnitCircle;
            if (block[0] == '1' && block[2] == '1') direction = new Vector2(direction.x, 0);
            else if (block[0] == '1' && direction.y > 0 || block[2] == '1' && direction.y < 0) direction = new Vector2(direction.x, -direction.y);
            if (block[1] == '1' && block[3] == '1') direction = new Vector2(0, direction.y);
            else if (block[1] == '1' && direction.x > 0 || block[3] == '1' && direction.x < 0) direction = new Vector2(-direction.x, direction.y);
            return direction;
        }
    }
}
