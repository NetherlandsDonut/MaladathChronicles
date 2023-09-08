using System.Collections.Generic;

public class Realm
{
    //Name of the realm
    public string name;

    //Indicates whether the realm has permadeath rules on
    public bool hardcore;

    //Indicates whether the realm has pvp content
    public bool pvp;

    //EXTERNAL FILE: List containing all realms in-game
    public static List<Realm> realms;
}
