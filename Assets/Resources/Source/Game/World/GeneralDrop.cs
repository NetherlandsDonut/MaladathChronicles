using System.Collections.Generic;

public class GeneralDrop
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {

    }

    //Is target's level appropriate for this item to drop
    public bool DoesLevelFit(int level) => dropStart <= level && level <= dropEnd;

    //Required profession in order to drop this item
    public string requiredProfession;

    //Required skill of the profession
    public int requiredSkill;

    //Name of the item
    public string item;

    //Rarity of this item occuring
    public double rarity;

    //Required enemy category for this item to drop
    public string category;

    //Required enemy sub category for this item to drop
    public string subCategory;

    //Determines whether this drops regardless of whether other items dropped
    public bool inclusive;

    //Level of enemies from which this item starts dropping
    public int dropStart;

    //Level of enemies from which the item stops dropping
    public int dropEnd;

    //This indicates maximum yield of the item per one kill
    public int dropCount;

    //Tags help program choose specific drops for specific occasions
    public List<string> tags;

    //Currently opened general drop
    public static GeneralDrop generalDrop;

    //EXTERNAL FILE: List containing all general drops in-game
    public static List<GeneralDrop> generalDrops;

    //List of all filtered general drops by input search
    public static List<GeneralDrop> generalDropsSearch;
}
