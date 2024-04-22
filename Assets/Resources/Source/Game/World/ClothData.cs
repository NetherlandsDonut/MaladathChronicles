using System.Collections.Generic;

public class ClothType
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {

    }

    public bool DoesLevelFit(int level) => dropStart <= level && level <= dropEnd;

    //Name of the cloth item
    public string item;

    //Rarity of this type of cloth
    public string rarity;

    //Required enemy category for this cloth item to drop
    public string category;

    //Level of enemies from which this cloth starts dropping
    public int dropStart;

    //Level of enemies from which the cloth stops dropping
    public int dropEnd;

    //This indicates maximum yield of cloth per one kill
    public int dropCount;

    //Currently opened cloth
    public static ClothType cloth;

    //EXTERNAL FILE: List containing all cloths in-game
    public static List<ClothType> clothTypes;

    //List of all filtered cloths by input search
    public static List<ClothType> clothTypesSearch;
}
