using UnityEngine;

using static Shatter;

public class FlyingElement : MonoBehaviour
{
    public bool turn;
    public int index;
    public SpriteRenderer render;

    public static int flySpeed = 4;
    public static int rowAmount = 2;

    public void Initiate(bool forPlayer)
    {
        turn = forPlayer;
        if (turn) Board.board.temporaryElementsPlayer.Add(gameObject);
        else Board.board.temporaryElementsEnemy.Add(gameObject);
        index = turn ? Board.board.temporaryElementsPlayer.IndexOf(gameObject) : Board.board.temporaryElementsEnemy.IndexOf(gameObject);
    }

    public void Update()
    {
        Destroy(gameObject);
        if (transform.localPosition.y < -190.5f)
            Destroy(gameObject);
        else if (turn && !Board.board.temporaryElementsPlayer.Contains(gameObject) || !turn && !Board.board.temporaryElementsEnemy.Contains(gameObject))
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -162.5f - 10 - 19 * Mathf.Abs(index % rowAmount - rowAmount)), Time.deltaTime * flySpeed);
        else transform.position = Vector3.Lerp(transform.position, turn ? new Vector3(-204.5f + 19 * (index / rowAmount), -162.5f + 19 * (index % rowAmount)) : new Vector3(204.5f - 19 * (index / rowAmount), -162.5f + 19 * (index % rowAmount)), Time.deltaTime * flySpeed);
    }

    public static GameObject SpawnFlyingElement(double speed, double amount, Vector3 position, string sprite, bool forPlayer, string block = "0000")
    {
        SpawnShatter(speed, amount, position, sprite, block);
        return SpawnFlyingElement(position, sprite, forPlayer);
    }

    public static GameObject SpawnFlyingElement(Vector3 position, string sprite, bool forPlayer)
    {
        var element = Instantiate(Resources.Load<GameObject>("Prefabs/PrefabElement"));
        element.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buttons/" + sprite);
        element.transform.parent = Board.board.window.desktop.transform;
        element.transform.position = position + new Vector3(17, 17);
        element.GetComponent<FlyingElement>().Initiate(forPlayer);
        return element;
    }
}
