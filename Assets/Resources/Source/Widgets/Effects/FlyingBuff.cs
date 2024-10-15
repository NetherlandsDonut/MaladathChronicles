using System.Linq;
using UnityEngine;

using static Buff;

public class FlyingBuff : MonoBehaviour
{
    //Is this buff on player or the enemy
    public int onWho;

    //I have no idea what that is
    public int dyingIndex;

    //Renderer of this buff
    public SpriteRenderer render;

    //Fly speed of the buff into the buff section
    public static int flySpeed = 4;

    //Amount of buffs per row
    public static int rowAmount = 7;

    public void Initiate(int onWho)
    {
        this.onWho = onWho;
        Board.board.temporaryBuffs[onWho].Add(gameObject);
    }

    public void Update()
    {
        if (Board.board.temporaryBuffs[onWho].Contains(gameObject))
            transform.position = Vector3.Lerp(transform.position, new Vector3(onWho == 0 ? -302.5f - (23 * Mathf.Abs(dyingIndex % rowAmount - rowAmount) + 1) : 302.5f + 23 * (Mathf.Abs(dyingIndex % rowAmount - rowAmount) + 1), 67.5f - 23 * (dyingIndex / rowAmount)), Time.deltaTime * flySpeed);
        else
        {
            transform.position = Vector3.Lerp(transform.position, onWho == 0 ? new Vector3(-302.5f + 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount)) : new Vector3(302.5f - 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount)), Time.deltaTime * flySpeed);
            if (transform.localPosition.x < -322.5f || transform.localPosition.x > 322.5f) Destroy(gameObject);
        }
    }

    public int Index() => Board.board.temporaryBuffs[onWho].IndexOf(gameObject);
    
    public static GameObject SpawnBuffObject(Vector3 position, string icon, Entity target)
    {
        var buff = Instantiate(Resources.Load<GameObject>("Prefabs/PrefabBuff"));
        buff.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buttons/" + icon);
        buff.transform.parent = Board.board.window.desktop.transform;
            buff.transform.position = position;
            var fly = buff.GetComponent<FlyingBuff>();
        fly.Initiate(Board.board.participants.FindIndex(x => x.who == target));
        buff.GetComponent<Highlightable>().Initialise(null, null, 
            (h) =>
            {
                var fb = h.GetComponent<FlyingBuff>();
                var buff = Board.board.participants[fb.onWho].who.buffs.Find(x => x.flyingBuff == h.gameObject);
                Board.board.participants[fb.onWho].who.RemoveBuff(buff);
            },
            (h) => () =>
            {
                var fb = h.GetComponent<FlyingBuff>();
                var buff = Board.board.participants[fb.onWho].who.buffs.Find(x => x.flyingBuff == h.gameObject);
                PrintBuffTooltip(target, buff);
            },
            null);
        return buff;
    }
}
