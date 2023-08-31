using UnityEngine;

using static Root;

public class FlyingMissile : MonoBehaviour
{
    public SpriteRenderer render;
    public Vector3 from, through1, through2, to;
    public float flySpeed, timeAlive;
    public bool up;

    public void Initiate(bool fromPlayer, float arc, double flySpeed)
    {
        flyingMissiles.Add(this);
        up = random.Next(0, 2) == 0;
        this.flySpeed = (float)flySpeed;
        if (fromPlayer)
        {
            from = new Vector3(-300, 141);
            through1 = new Vector3(-155, 141 + (arc <= 40 && !up ? arc : -arc));
            through2 = new Vector3(5, 141 + (arc <= 40 && !up ? arc : -arc));
            to = new Vector3(167, 141);
            transform.position = from;
        }
        else
        {
            from = new Vector3(167, 141);
            through1 = new Vector3(5, 141 + (arc <= 40 && !up ? arc : -arc));
            through2 = new Vector3(-155, 141 + (arc <= 40 && !up ? arc : -arc));
            to = new Vector3(-300, 141);
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
        else transform.position = Bezier(from, through1, through2, to, timeAlive * flySpeed);
    }

    public static GameObject SpawnFlyingMissile(string sprite, bool fromPlayer, double arc, double flySpeed)
    {
        var missile = Instantiate(Resources.Load<GameObject>("Prefabs/PrefabMissile"));
        missile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Buttons/" + sprite);
        missile.transform.parent = Board.board.window.desktop.transform;
        missile.GetComponent<FlyingMissile>().Initiate(fromPlayer, (float)arc, flySpeed);
        return missile;
    }

    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return (((-p0 + 3 * (p1 - p2) + p3) * t + (3 * (p0 + p2) - 6 * p1)) * t + 3 * (p1 - p0)) * t + p0;
    }
}
