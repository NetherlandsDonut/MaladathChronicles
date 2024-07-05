
public class WorldBuff
{
    public WorldBuff() { }

    public WorldBuff(Buff buff, int rank, int minutesLeft)
    {
        this.buff = buff;
        this.rank = rank;
        this.minutesLeft = minutesLeft;
    }

    //Buff that this is
    public Buff buff;

    //Rank of the buff
    public int rank;

    //Amount of time left of the buff on the entity
    public int minutesLeft;
}
