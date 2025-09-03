using UnityEngine;

public class MoveAwayQuickUse : MonoBehaviour
{
    //Moves the quick use window outside of the screen when the turn has ended
    public void Update()
    {
        if (Board.board == null) return;
        if (Board.board.finishedMoving)
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -190), Time.deltaTime * 4);
    }
}
