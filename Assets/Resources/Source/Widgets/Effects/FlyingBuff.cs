using UnityEngine;

using static Buff;

public class FlyingBuff : MonoBehaviour
{
    //On which entity in combat is this buff
    public int onWho;

    //What was the position of the buff on the list before it was removed
    //Used for positioning before removing from the scene
    public int dyingIndex;

    //Renderer of this buff
    public SpriteRenderer render;

    //Fly speed of the buff into the buff section
    public static int flySpeed = 4;

    //Amount of buffs per row
    public static int rowAmount = 7;

    //Initiates the flying buff variables
    public void Initiate(int onWho)
    {
        this.onWho = onWho;
        Board.board.temporaryBuffs[onWho].Add(gameObject);
    }

    //Instantly moves the buffs away from the view or into visibility
    //This is used during swapping of the spotlight
    public void InstantMove()
    {
        var participant = Board.board.participants[onWho];
        var stillActive = Board.board.temporaryBuffs[onWho].Contains(gameObject);
        if (participant.team == 1)
            if (stillActive)
            {
                if (Board.board.spotlightFriendly[0] == onWho) transform.position = new Vector3(-302.5f + 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount));
                else transform.position = new Vector3(-474.5f + 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount));
            }
            else transform.position = new Vector3(-474.5f + 23 * (dyingIndex % rowAmount), -162.5f + 23 * (dyingIndex / rowAmount));
        else if (participant.team == 2)
            if (stillActive)
            {
                if (Board.board.spotlightEnemy[0] == onWho) transform.position = new Vector3(302.5f - 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount));
                else transform.position = new Vector3(474.5f - 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount));
            }
            else transform.position = new Vector3(474.5f - 23 * (dyingIndex % rowAmount), -162.5f + 23 * (dyingIndex / rowAmount));
    }

    //Moves the buff object around the screen
    //Outside of it when the buff is not on selected entity and into the view on the contrary
    public void Update()
    {
        var participant = Board.board.participants[onWho];
        var stillActive = Board.board.temporaryBuffs[onWho].Contains(gameObject);
        if (participant.team == 1)
            if (stillActive)
                if (Board.board.spotlightFriendly[0] == onWho) transform.position = Vector3.Lerp(transform.position, new Vector3(-302.5f + 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount)), Time.deltaTime * flySpeed);
                else transform.position = Vector3.Lerp(transform.position, new Vector3(-474.5f + 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount)), Time.deltaTime * flySpeed);
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(-474.5f + 23 * (dyingIndex % rowAmount), -162.5f + 23 * (dyingIndex / rowAmount)), Time.deltaTime * flySpeed);
                if (transform.localPosition.x < -330) Destroy(gameObject);
            }
        else if (participant.team == 2)
            if (stillActive)
                if (Board.board.spotlightEnemy[0] == onWho) transform.position = Vector3.Lerp(transform.position, new Vector3(302.5f - 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount)), Time.deltaTime * flySpeed);
                else transform.position = Vector3.Lerp(transform.position, new Vector3(474.5f - 23 * (Index() % rowAmount), -162.5f + 23 * (Index() / rowAmount)), Time.deltaTime * flySpeed);
            else
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(474.5f - 23 * (dyingIndex % rowAmount), -162.5f + 23 * (dyingIndex / rowAmount)), Time.deltaTime * flySpeed);
                if (transform.localPosition.x > 330) Destroy(gameObject);
            }
    }

    //Tells the index of this buff on the buff list of the entity this buff is on
    public int Index() => Board.board.temporaryBuffs[onWho].IndexOf(gameObject);
    
    //Spawns a new flying buff
    public static GameObject SpawnBuffObject(Vector3 position, string icon, Entity target)
    {
        var buff = Instantiate(Resources.Load<GameObject>("Prefabs/PrefabBuff"));
        buff.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buttons/" + icon);
        buff.transform.parent = Board.board.window.desktop.transform;
        buff.transform.position = position;
        var fly = buff.GetComponent<FlyingBuff>();
        fly.Initiate(Board.board.participants.FindIndex(x => x.who == target));
        buff.GetComponent<Highlightable>().Initialise(null, null, null,
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
