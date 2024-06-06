using UnityEngine;

using static Buff;

public class FlyingBuff : MonoBehaviour
{
    //Is this buff on player or the enemy
    public bool onPlayer;

    //I have no idea what that is
    public int dyingIndex;

    //Renderer of this buff
    public SpriteRenderer render;

    //Fly speed of the buff into the buff section
    public static int flySpeed = 4;

    //Amount of buffs per row
    public static int rowAmount = 7;

    public void Initiate(bool targettedPlayer)
    {
        onPlayer = targettedPlayer;
        (onPlayer ? Board.board.temporaryBuffsPlayer : Board.board.temporaryBuffsEnemy).Add(gameObject);
    }

    public void Update()
    {
        if (transform.localPosition.x < -322.5f || transform.localPosition.x > 322.5f) Destroy(gameObject);
        else if (onPlayer && !Board.board.temporaryBuffsPlayer.Contains(gameObject) || !onPlayer && !Board.board.temporaryBuffsEnemy.Contains(gameObject)) transform.position = Vector3.Lerp(transform.position, new Vector3(onPlayer ? -302.5f - (23 * Mathf.Abs(dyingIndex % rowAmount - rowAmount) + 1) : 302.5f + 23 * (Mathf.Abs(dyingIndex % rowAmount - rowAmount) + 1), 67.5f - 23 * (dyingIndex / rowAmount)), Time.deltaTime * flySpeed);
        else transform.position = Vector3.Lerp(transform.position, onPlayer ? new Vector3(-302.5f + 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount)) : new Vector3(302.5f - 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount)), Time.deltaTime * flySpeed);
    }

    public int Index() 
    {
        return onPlayer ? Board.board.temporaryBuffsPlayer.IndexOf(gameObject) : Board.board.temporaryBuffsEnemy.IndexOf(gameObject);
    }
    
    public static GameObject SpawnBuffObject(Vector3 position, string icon, Entity target)
    {
        var buff = Instantiate(Resources.Load<GameObject>("Prefabs/PrefabBuff"));
        buff.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Buttons/" + icon);
        buff.transform.parent = Board.board.window.desktop.transform;
        buff.transform.position = position;
        var fly = buff.GetComponent<FlyingBuff>();
        fly.Initiate(Board.board.player == target);
        buff.GetComponent<Highlightable>().Initialise(null, null, 
            (h) =>
            {
                var fb = h.GetComponent<FlyingBuff>();
                var buff = (fb.onPlayer ? Board.board.player.buffs : Board.board.enemy.buffs).Find(x => x.Item3 == h.gameObject);
                (target == Board.board.player ? Board.board.enemy : Board.board.player).RemoveBuff(buff);
            },
            (h) => () =>
            {
                var fb = h.GetComponent<FlyingBuff>();
                var buff = (fb.onPlayer ? Board.board.player.buffs : Board.board.enemy.buffs).Find(x => x.Item3 == h.gameObject);
                PrintBuffTooltip(target, target == Board.board.player ? Board.board.enemy : Board.board.player, buff);
            },
            null);
        return buff;
    }
}
