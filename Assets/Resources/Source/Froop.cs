using UnityEngine;

public class Froop : MonoBehaviour
{
    public bool turn;
    public int index;
    public SpriteRenderer render;

    public static int flySpeed = 6;
    public static int rowAmount = 3;

    public void Initiate()
    {
        turn = Board.board.playerTurn;
        if (turn) Board.board.temporaryElementsPlayer.Add(gameObject);
        else Board.board.temporaryElementsEnemy.Add(gameObject);
        index = turn ? Board.board.temporaryElementsPlayer.IndexOf(gameObject) : Board.board.temporaryElementsEnemy.IndexOf(gameObject);
    }

    public void Update()
    {
        if (transform.localPosition.y < -190.5f)
            Destroy(gameObject);
        else if (turn && !Board.board.temporaryElementsPlayer.Contains(gameObject) || !turn && !Board.board.temporaryElementsEnemy.Contains(gameObject))
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -162.5f - 10 - 23 * Mathf.Abs(index % rowAmount - rowAmount)), Time.deltaTime * flySpeed);
        else transform.position = Vector3.Lerp(transform.position, turn ? new Vector3(-214.5f + 23 * (index / rowAmount), -162.5f + 23 * (index % rowAmount)) : new Vector3(214.5f - 23 * (index / rowAmount), -162.5f + 23 * (index % rowAmount)), Time.deltaTime * flySpeed);
    }
}
