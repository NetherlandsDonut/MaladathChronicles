using UnityEngine;

using System.Collections;

using static Root;
using System.Linq;

public class Shatter : MonoBehaviour
{
    //Time that the shatter effect will be delayed by
    public float delay;

    //Time that the shatter effect will be delayed by
    public float travelDelay;

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

    //Overall time this shatter is traveling
    private float travelDuration;

    //Travel start point
    private Vector3 start;

    //Travel destination
    private Vector3 destination;

    //Indicates whether the object has traveling enabled
    private bool travelEnabled;

    //Initiates the shatter effect
    public void Initiate(float speed, float delay = 0, SpriteRenderer r = null)
    {
        this.speed = speed;
        this.delay = delay;
        if (r != null) render = r;
        else render = GetComponent<SpriteRenderer>();
        if (render == null) StartCoroutine(SelfDestruct(speed));
        else
        {
            startingColor = render.color;
            aimColor = new Color(startingColor.r, startingColor.g, startingColor.b, 0);
        }
    }

    //Initiates the shatter effect
    public void Travel(Vector3 to, float travelDelay = 0)
    {
        travelEnabled = true;
        travelDelay += travelDelay * (random.Next(0, 50) / 100f);
        this.travelDelay = travelDelay;
        to += new Vector3(random.Next(-15, 15) - 2, random.Next(-15, 15) + 2);
        destination = to;
    }

    public void Update()
    {
        //If this object that has shatter on it is set to travel..
        if (travelEnabled && Defines.defines.animatedResourceParticles)
        {
            //Wait with moving the object till the travel delay is zeroed
            if (travelDelay > 0)
            {
                //Reduce the delay by the time elapsed between frames
                travelDelay -= Time.deltaTime;

                //If delay is zeroed, set the position of the object as the travel starting position.
                //This is done because, for example, if the object had gravity enabled then it was moving
                //downwards while the delay was being dealt with.
                //So the starting position was changed in that time and we are taking it into account here
                //by setting it later and not immediately on effect initialisation
                if (travelDelay <= 0) start = transform.position;
            }

            //If the travel delay is already dealt with..
            else
            {
                travelDuration += Time.deltaTime;
                transform.position = Vector3.Lerp(start, destination, travelDuration * (travelDuration > maxSpeed ? maxSpeed : travelDuration));

                //If the destination was reached..
                if (Vector3.Distance(transform.position, destination) < 2)
                {
                    //Get the object's rigidbody
                    var r = GetComponent<Rigidbody2D>();

                    //Turn off the gravity
                    r.gravityScale = 0;

                    //Set velocity to zero so the object will stop in place
                    r.linearVelocity = Vector2.zero;

                    //Apply force in a random direction to add a "bounce-off" effect to the object
                    r.AddRelativeForce(Random.insideUnitCircle * 240);

                    //Play a sound indicating that the object collided with the destination
                    if (random.Next(2) == 0) Sound.PlaySound("Sparkle" + random.Next(1, 4), 0.3f);

                    //Disable travel as the object completed it's travel
                    travelEnabled = false;
                }
            }
        }

        //Wait with executing the effect till the delay is zeroed
        if (delay > 0) delay -= Time.deltaTime;

        //If delay is already dealt with and renderer isn't null..
        else if (render != null)
        {
            //Extend the object duration as it's still alive
            duration += Time.deltaTime;
            
            //Update the color of the object making it fade away with time
            render.color = Color.Lerp(startingColor, aimColor, duration * speed);

            //Remove the object from the scene if it's already transparent
            if (render.color.a <= 0) Destroy(gameObject);
        }
    }

    //Set the object to self destruct in a specified amount of time
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
        var foo = Resources.Load<Sprite>("Sprites/Buttons/" + sprite);
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

    public static void SpawnShatter(double speed, double amount, Vector3 position, string sprite, bool travel)
    {
        var foo = Resources.Load<Sprite>("Sprites/ButtonsBig/" + sprite);
        if (foo == null) return;
        var shatter = new GameObject("Shatter", typeof(Shatter));
        shatter.GetComponent<Shatter>().Initiate(7);
        shatter.transform.parent = Board.board.window.desktop.transform;
        shatter.transform.position = position;
        shatter.layer = 1;
        int x = (int)foo.textureRect.width, y = (int)foo.textureRect.height;
        var dot = Resources.Load<GameObject>("Prefabs/PrefabDot");
        var direction = Random.insideUnitCircle;
        if (amount > 100) amount = 100;
        else if (amount < 0) amount = 0;
        for (int i = 2; i < x - 1; i++)
            for (int j = 2; j < y - 1; j++)
                if ((i + j) % (Defines.defines.animatedResourceParticles && travel && foo.texture.name.Contains("Rousing") ? 4 : 2) == 0 && random.Next(0, 100) < amount)
                    SpawnDot(i, j, foo.texture.GetPixel(i, j));

        void SpawnDot(int c, int v, Color32 color)
        {
            if (color.Grayscale() < 20) return;
            var newObject = Instantiate(dot);
            newObject.transform.parent = shatter.transform;
            newObject.transform.localPosition = new Vector3(c, v);
            newObject.GetComponent<SpriteRenderer>().color = color;
            var shat = newObject.GetComponent<Shatter>();
            if (travel && Defines.defines.animatedResourceParticles)
            {
                shat.Initiate(random.Next(1, 10) / 5f, random.Next(3, 5) / 3f);
                shat.Travel(Board.board.PortraitPosition(Board.board.whosTurn), 0.4f);
                newObject.GetComponent<Rigidbody2D>().AddRelativeForce((direction / 2 + Random.insideUnitCircle) * (int)(100 * speed));
            }
            else shat.Initiate(random.Next(1, 7), random.Next(1, 3) / 3f);
            newObject.GetComponent<Rigidbody2D>().AddRelativeForce((direction / 2 + Random.insideUnitCircle) * (int)(100 * speed));
            direction = Random.insideUnitCircle;
        }
    }

    //Maximum speed of flying shatter objects
    public static float maxSpeed = 1f;
}
