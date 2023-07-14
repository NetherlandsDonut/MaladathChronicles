using UnityEngine;

public class Froop : MonoBehaviour
{
    public bool turn;
    public SpriteRenderer render;

    public void Initiate()
    {
        turn = Board.board.playerTurn;
        if (turn) Board.board.temporaryElementsPlayer.Add(gameObject);
        else Board.board.temporaryElementsEnemy.Add(gameObject);
    }

    public void Update()
    {
        if (render != null)
            transform.position = Vector3.Lerp(transform.position, turn ? new Vector3(-214.5f + 23 * (Index() / 4), -162.5f + 23 * (Index() % 4)) : new Vector3(214.5f - 23 * (Index() / 4), -162.5f + 23 * (Index() % 4)), Time.deltaTime * 4);
    }

    public int Index() => turn ? Board.board.temporaryElementsPlayer.IndexOf(gameObject) : Board.board.temporaryElementsEnemy.IndexOf(gameObject);
}
