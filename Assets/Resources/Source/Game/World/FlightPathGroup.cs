using System.Collections.Generic;

public class FlightPathGroup
{
    //Which side is offered those flight paths, Alliance or the Horde
    public string side;

    //I don't know, some money thing
    public double priceMultiplier;

    //All sites that are interconnected
    public List<string> sitesConnected;

    //Currently opened flight path group
    public static FlightPathGroup flightPathGroup;

    //EXTERNAL FILE: List containing all flight path groups in-game
    public static List<FlightPathGroup> flightPathGroups;
}