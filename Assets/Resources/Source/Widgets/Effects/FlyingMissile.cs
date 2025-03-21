using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Defines;

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

    public void Initiate(bool fromPlayer, float arc, double flySpeed, double trailStrength, string trailSprite)
    {
        flyingMissiles.Add(this);
        up = random.Next(0, 2) == 0;
        this.flySpeed = (float)flySpeed;
        this.trailStrength = trailStrength;
        this.trailSprite = trailSprite;
        if (fromPlayer)
        {
            from = new Vector3(-148, 141);
            through1 = new Vector3(-155, 141 + (arc <= 40 && !up ? arc : -arc));
            through2 = new Vector3(5, 141 + (arc <= 40 && !up ? arc : -arc));
            to = new Vector3(148, 141);
            transform.position = from;
        }
        else
        {
            from = new Vector3(148, 141);
            through1 = new Vector3(155, 141 + (arc <= 40 && !up ? arc : -arc));
            through2 = new Vector3(-5, 141 + (arc <= 40 && !up ? arc : -arc));
            to = new Vector3(-148, 141);
            transform.position = from;
        }
    }

    public void Update()
    {
        timeAlive += Time.deltaTime;
        if ((to.x < 0 && transform.position.x <= to.x || to.x >= 0 && transform.position.x >= to.x)/* && (to.y < 0 && transform.position.y <= to.y || to.y >= 0 && transform.position.y >= to.y)*/)
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

    public static GameObject SpawnFlyingMissile(string sprite, bool fromPlayer, double arc, double flySpeed, double trailStrength)
    {
        var foo = Resources.Load<Sprite>("Sprites/Buttons/" + sprite);
        if (foo == null) return null;
        var missile = Instantiate(Resources.Load<GameObject>("Prefabs/PrefabMissile"));
        missile.GetComponent<SpriteRenderer>().sprite = foo;
        missile.transform.parent = Board.board.window.desktop.transform;
        missile.GetComponent<FlyingMissile>().Initiate(fromPlayer, (float)arc, flySpeed, trailStrength, sprite);
        return missile;
    }

    //List of active missiles
    public static List<FlyingMissile> flyingMissiles;
}
