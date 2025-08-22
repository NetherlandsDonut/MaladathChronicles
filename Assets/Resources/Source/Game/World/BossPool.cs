using System.Collections.Generic;

public class BossPool
{
    //ID of the boss pool
    public string id;

    //Possible bosses that can be generated for this boss pool
    public List<string> possibleBosses;

    //Currently boss pool
    public static BossPool bossPool;

    //EXTERNAL FILE: List containing all boss pools in-game
    public static List<BossPool> bossPools;
}
