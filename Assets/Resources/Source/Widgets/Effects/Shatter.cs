using UnityEngine;
using System.Collections;

using static Root;

public class Shatter : MonoBehaviour
{
    //Time that the shatter effect will be delayed by
    public float delayLeft;

    //Speed at which the shatter effect proceeds
    public float speed;

    //Renderer that will be affected by this effect
    public SpriteRenderer render;

    //Color at the beginning
    private Color startingColor;

    //Color at the end
    private Color aimColor;

    //Overall time this shatter lives
    private float duration;

    //Initiates the shatter effect
    public void Initiate(float speed, float delay = 0, SpriteRenderer r = null)
    {
        this.speed = speed;
        delayLeft = delay;
        if (r != null) render = r;
        else render = GetComponent<SpriteRenderer>();
        if (render == null) StartCoroutine(SelfDestruct(speed));
        else
        {
            startingColor = render.color;
            aimColor = new Color(startingColor.r, startingColor.g, startingColor.b, 0);
        }
    }

    public void Update()
    {
        if (delayLeft > 0) delayLeft -= Time.deltaTime;
        else if (render != null)
        {
            duration += Time.deltaTime;
            render.color = Color.Lerp(startingColor, aimColor, duration * speed);
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
        shatter.GetComponent<Shatter>().Initiate(7);
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
                if ((i + j) % 3 == 0 && random.Next(0, 100) < amount)
                    SpawnDot(i, j, foo.texture.GetPixel(i, j));

        void SpawnDot(int c, int v, Color32 color)
        {
            if (color.Grayscale() < 20) return;
            var newObject = Instantiate(dot);
            newObject.GetComponent<SpriteRenderer>().color = color;
            newObject.GetComponent<Shatter>().Initiate(random.Next(1, 7), random.Next(1, 3) / 3f);
            newObject.transform.parent = shatter.transform;
            newObject.transform.localPosition = new Vector3(c, v);
            newObject.GetComponent<Rigidbody2D>().AddRelativeForce((direction / 2 + Random.insideUnitCircle / 6) * (int)(100 * speed));
            direction = Random.insideUnitCircle;
        }
    }

    public static void SpawnShatter(double speed, double amount, Vector3 position, string sprite, string block = "0000")
    {
        var foo = Resources.Load<Sprite>("Sprites/Building/BigButtons/" + sprite);
        if (foo == null) return;
        var shatter = new GameObject("Shatter", typeof(Shatter));
        shatter.GetComponent<Shatter>().Initiate(7);
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
            if (color.Grayscale() < 20) return;
            var newObject = Instantiate(dot);
            newObject.GetComponent<SpriteRenderer>().color = color;
            newObject.GetComponent<Shatter>().Initiate(random.Next(1, 7), random.Next(1, 3) / 3f);
            newObject.transform.parent = shatter.transform;
            newObject.transform.localPosition = new Vector3(c, v);
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
