using System.Collections;
using UnityEngine;

using static Root;

public class Shatter : MonoBehaviour
{
    //Time that the shatter effect will be delayed by
    public float delayLeft;

    //Speed at which the shatter effect proceeds
    public float time;

    //Renderer that will be affected by this effect
    public SpriteRenderer render;

    //Initiates the shatter effect
    public void Initiate(float time, float delay = 0, SpriteRenderer r = null)
    {
        this.time = time;
        delayLeft = delay;
        if (r != null) render = r;
        if (render == null) StartCoroutine(SelfDestruct(time));
    }

    public void Update()
    {
        if (delayLeft > 0) delayLeft -= Time.deltaTime;
        else if (render != null)
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

    public static void SpawnTrailShatter(double speed, double amount, Vector3 position, string sprite)
    {
        var shatter = new GameObject("Shatter", typeof(Shatter));
        shatter.GetComponent<Shatter>().Initiate(3);
        shatter.transform.parent = Board.board.window.desktop.transform;
        shatter.transform.position = position + new Vector3(-2, -8);
        shatter.layer = 1;
        var foo = Resources.Load<Sprite>("Sprites/Building/Buttons/" + sprite);
        if (foo == null)
        {
            Destroy(shatter);
            return;
        }
        int x = (int)foo.textureRect.width, y = (int)foo.textureRect.height;
        var dot = Resources.Load<GameObject>("Prefabs/PrefabDot");
        var direction = Random.insideUnitCircle;
        if (amount > 100) amount = 100;
        else if (amount < 0) amount = 0;
        for (int i = 5; i < x - 4; i++)
            for (int j = 5; j < y - 4; j++)
                if ((i + j) % 2 == 0 && random.Next(0, 100) < amount)
                    SpawnDot(i, j, foo.texture.GetPixel(i, j));

        void SpawnDot(int c, int v, Color32 color)
        {
            var newObject = Instantiate(dot);
            newObject.GetComponent<Shatter>().Initiate(random.Next(1, 8) / 50.0f);
            newObject.transform.parent = shatter.transform;
            newObject.transform.localPosition = new Vector3(c, v);
            newObject.GetComponent<SpriteRenderer>().color = color;
            newObject.GetComponent<Rigidbody2D>().AddRelativeForce((direction / 2 + Random.insideUnitCircle / 6) * (int)(100 * speed));
            direction = Random.insideUnitCircle;
        }
    }

    public static void SpawnShatter(double speed, double amount, Vector3 position, string sprite, string block = "0000")
    {
        var foo = Resources.Load<Sprite>("Sprites/Building/BigButtons/" + sprite);
        if (foo == null) return;
        var shatter = new GameObject("Shatter", typeof(Shatter));
        shatter.GetComponent<Shatter>().Initiate(3);
        shatter.transform.parent = Board.board.window.desktop.transform;
        shatter.transform.position = position;
        shatter.layer = 1;
        int x = (int)foo.textureRect.width, y = (int)foo.textureRect.height;
        var dot = Resources.Load<GameObject>("Prefabs/PrefabDot");
        var direction = RollDirection();
        if (amount > 100) amount = 100;
        else if (amount < 0) amount = 0;
        for (int i = 2; i < x - 1; i++)
            for (int j = 2; j < y - 1; j++)
                if ((i + j) % 2 == 0 && random.Next(0, 100) < amount)
                    SpawnDot(i, j, foo.texture.GetPixel(i, j));

        void SpawnDot(int c, int v, Color32 color)
        {
            var newObject = Instantiate(dot);
            newObject.GetComponent<Shatter>().Initiate(random.Next(1, 8) / 60.0f, 0.5f);
            newObject.transform.parent = shatter.transform;
            newObject.transform.localPosition = new Vector3(c, v);
            newObject.GetComponent<SpriteRenderer>().color = color;
            newObject.GetComponent<Rigidbody2D>().AddRelativeForce((direction / 2 + Random.insideUnitCircle / 6) * (int)(100 * speed));
            if (block == "0000") direction = RollDirection();
        }

        Vector2 RollDirection()
        {
            var direction = Random.insideUnitCircle;
            if (block[0] == '1' && block[2] == '1') direction = new Vector2(direction.x, 0);
            else if (block[0] == '1' && direction.y > 0 || block[2] == '1' && direction.y < 0) direction = new Vector2(direction.x, -direction.y);
            if (block[1] == '1' && block[3] == '1') direction = new Vector2(0, direction.y);
            else if (block[1] == '1' && direction.x > 0 || block[3] == '1' && direction.x < 0) direction = new Vector2(-direction.x, direction.y);
            return direction;
        }
    }
}
