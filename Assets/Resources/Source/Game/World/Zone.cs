using System.Collections.Generic;

public class Zone
{
    //Name of the zone
    public string name;

    //Landmass this zone is residing on
    public string continent;

    //General ambience track played when entering sites
    //in this specific zone. It can be overwritten with
    //site-specific ambience tracks
    public string ambience, dayAmbience, nightAmbience;

    //Currently opened zone
    public static Zone zone;

    //EXTERNAL FILE: List containing all zones in-game
    public static List<Zone> zones;

    //List of all filtered zones by input search
    public static List<Zone> zonesSearch;
}
