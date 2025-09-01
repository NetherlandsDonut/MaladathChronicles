using UnityEngine;

public class CombatBuff
{
    public CombatBuff(Buff buff, int durationLeft, GameObject flyingBuff, int rank, Entity source)
    {
        this.buff = buff;
        this.durationLeft = durationLeft;
        this.flyingBuff = flyingBuff;
        this.rank = rank;
        this.source = new FrozenEntity(source);
    }

    public CombatBuff(CombatBuff combatBuff)
    {
        buff = combatBuff.buff;
        durationLeft = combatBuff.durationLeft;
        flyingBuff = combatBuff.flyingBuff;
        rank = combatBuff.rank;
        source = combatBuff.source;
    }

    //What buff is it
    public Buff buff;

    //Time left in turns for this buff
    public int durationLeft;

    //Flying buff representing this buff
    public GameObject flyingBuff;

    //Rank of the buff applied
    public int rank;

    //Entity who cast the buff with their stats frozen in time
    public FrozenEntity source;
}
