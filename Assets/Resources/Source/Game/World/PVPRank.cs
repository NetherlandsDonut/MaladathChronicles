using System.Collections.Generic;

public class PVPRank
{
    //Tier of the ranking, the higher the better
    public int rank;

    //Names of the rank for alliance and horde characters
    public string nameAlliance, nameHorde;

    //Amount of honor required to achieve this rank
    public int honorRequired;

    //Currently opened pvp rank
    public static PVPRank pvpRank;

    //EXTERNAL FILE: List containing all pvp ranks in-game
    public static List<PVPRank> pvpRanks;

    //List of all filtered pvp ranks by input search
    public static List<PVPRank> pvpRanksSearch;
}
