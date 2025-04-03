using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Defines;
using System.Linq;

public class FlyingMissile : MonoBehaviour
{
    public SpriteRenderer render;
    public Vector3 from, through1, through2, to;
    public float flySpeed, timeAlive;
    public string trailSprite;
    public double trailStrength;

    //Indicates whether the missile spawns trails when it travels
    public int trailSpawns;

    //Determines whether the missile takes the path above targets
    public bool up;

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

    public void SpawnTrail()
    {
        Shatter.SpawnTrailShatter(1, trailStrength, transform.position, trailSprite);
    }

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
