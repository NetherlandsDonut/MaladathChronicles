using UnityEngine;

public class Froop : MonoBehaviour
{
    public bool turn;
    public int space;
    public SpriteRenderer render;

    public void Initiate()
    {
        turn = Board.board.playerTurn;
        space = turn ? Board.board.player.inventorySpace++ : Board.board.enemy.inventorySpace++;
    }

    public void Update()
    {
        if (render != null)
        {
            transform.position = Vector3.Lerp(transform.position, turn ? new Vector3(-305.5f + 23 * (space % 6), 108.5f - 23 * (space / 6)) : new Vector3(305.5f - 23 * (space % 6), 108.5f - 23 * (space / 6)), Time.deltaTime * 4);
        }
    }
}
