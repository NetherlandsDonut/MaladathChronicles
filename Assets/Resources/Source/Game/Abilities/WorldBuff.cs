using System;

public class WorldBuff
{
    //Constructor for deserialisation
    public WorldBuff() { }

    //Default constructor for adding new world buffs to entities
    public WorldBuff(Buff buff, int rank, int minutesLeft)
    {
        Buff = buff;
        this.buff = buff.name;
        this.rank = rank;
        this.minutesLeft = minutesLeft;
    }

    //Buff that this world buff is functioning as
    public string buff;

    //Buff that this world buff is functioning as
    [NonSerialized] public Buff Buff;

    //Rank of the buff
    public int rank;

    //Amount of time left of the buff on the entity
    public int minutesLeft;
}
