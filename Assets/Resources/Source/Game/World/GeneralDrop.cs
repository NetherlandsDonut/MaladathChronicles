using System.Collections.Generic;

public class GeneralDrop
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {

    }

    public bool DoesLevelFit(int level) => dropStart <= level && level <= dropEnd;

    //Required profession in order to drop this item
    public string requiredProfession;

    //Required skill of the profession
    public int requiredSkill;

    //Name of the cloth item
    public string item;

    //Rarity of this type of cloth (1 - 100)
    public double rarity;

    //Required enemy category for this cloth item to drop
    public string category;

    //Determines whether this drops regardless of whether other items dropped
    public bool inclusive;

    //Level of enemies from which this cloth starts dropping
    public int dropStart;

    //Level of enemies from which the cloth stops dropping
    public int dropEnd;

    //This indicates maximum yield of cloth per one kill
    public int dropCount;

    //Tags help program choose specific drops for specific occasions
    public List<string> tags;

    //Currently opened cloth
    public static GeneralDrop generalDrop;

    //EXTERNAL FILE: List containing all cloths in-game
    public static List<GeneralDrop> generalDrops;

    //List of all filtered cloths by input search
    public static List<GeneralDrop> generalDropsSearch;
}
