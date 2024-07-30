public class AreaProgression
{
    //Type of the progression [Area, Boss, Treasure]
    public string type;

    //Name of the boss that is unlocked with this progression
    public string bossName;

    //Name of the area that is unlocked with this progression
    public string areaName;

    //Point in site exploration that this progression is made
    public int point;

    //When this is true then player can only unlock the
    //area that this progression unlocks when player has
    //all progression points like this one checked
    public bool all;
}
