using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Defines;

public class FlyingMissile : MonoBehaviour
{
    //Main graphical render of the missile
    public SpriteRenderer render;

    //Points on the travel path of the missile
    public Vector3 from, through1, through2, to;

    //Speed at which the missile is moving
    public float flySpeed;

    //Sum of time this missile is alive
    public float timeAlive;

    //Sprite used for the trail effect
    public string trailSprite;

    //How strong is the trail effect left by this missile
    public double trailStrength;

    //Indicates whether the missile spawns trails when it travels
    public int trailSpawns;

    //Determines whether the missile takes the path above targets
    public bool up;

    //Initiates this missile to start traveling
    public void Initiate(int entityFrom, int entityTo, float arc, double flySpeed, double trailStrength, string trailSprite)
    {
        flyingMissiles.Add(this);
        up = random.Next(0, 2) == 0;
        this.flySpeed = (float)flySpeed;
        this.trailStrength = trailStrength;
        this.trailSprite = trailSprite;
        from = Board.board.PortraitPosition(entityFrom);
        to = Board.board.PortraitPosition(entityTo);
        var intArc = (int)arc;
        var distance = Vector3.Distance(from, to);
        through1 = Vector3.MoveTowards(from, to, distance / 3) + new Vector3(random.Next(-intArc, intArc), random.Next(-intArc, intArc));
        through2 = Vector3.MoveTowards(from, to, distance / 3 * 2) + new Vector3(random.Next(-intArc, intArc), random.Next(-intArc, intArc));
        transform.position = from;
    }

    //Moves the missile in time and spawn trails on the way
    public void Update()
    {
        timeAlive += Time.deltaTime;
        if (Vector3.Distance(transform.position, to) < 19)
        {
            flyingMissiles.Remove(this);
            Destroy(gameObject);
        }
        else
        {
            if (timeAlive * 3 / defines.frameTime > trailSpawns)
            {
                SpawnTrail();
                trailSpawns++;
            }
            transform.position = Bezier(from, through1, through2, to, timeAlive * flySpeed);
        }
    }

    //Spawns a trail spot behind the missile
    public void SpawnTrail() => Shatter.SpawnTrailShatter(1, trailStrength, transform.position, trailSprite);

    //Spawns a new flying missile and automatically initiates it
    public static GameObject SpawnFlyingMissile(string sprite, int entityFrom, int entityTo, double arc, double flySpeed, double trailStrength)
    {
        var foo = Resources.Load<Sprite>("Sprites/Buttons/" + sprite);
        if (foo == null) return null;
        var missile = Instantiate(Resources.Load<GameObject>("Prefabs/PrefabMissile"));
        missile.GetComponent<SpriteRenderer>().sprite = foo;
        missile.transform.parent = Board.board.window.desktop.transform;
        missile.GetComponent<FlyingMissile>().Initiate(entityFrom, entityTo, (float)arc, flySpeed, trailStrength, sprite);
        return missile;
    }

    //List of active missiles
    public static List<FlyingMissile> flyingMissiles;
}
