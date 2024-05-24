using System.Linq;
using System.Collections.Generic;

public class Recipe
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        reagents ??= new();
        results ??= new();
    }

    //Provides an icon for the recipe in the interface
    public string Icon()
    {
        if (results.Count == 0) return "TradeEnchanting";
        var find = Item.items.Find(x => x.name == results.Keys.First());
        if (find != null) return find.icon;
        return "OtherUnknown";
    }

    //Name of this recipe
    public string name;

    //Profession this recipe belongs to
    public string profession;

    //Thresholds of getting skill ups
    public int skillUpOrange, skillUpYellow, skillUpGreen, skillUpGray;

    //Price of training this recipe at a trainer
    public int trainingCost;

    //Indicates at what skill level of profession player can learn this recipe
    public int learnedAt;

    //Items required to fullfil the recipe and the items provided by finishing it
    public Dictionary<string, int> reagents, results;

    //Currently opened recipe
    public static Recipe recipe;

    //EXTERNAL FILE: List containing all recipes in-game
    public static List<Recipe> recipes;

    //List of all filtered recipes by input search
    public static List<Recipe> recipesSearch;
}
