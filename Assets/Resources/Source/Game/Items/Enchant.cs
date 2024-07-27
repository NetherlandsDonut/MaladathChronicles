using System.Linq;
using System.Collections.Generic;

public class Enchant
{
    //Name of the enchantment
    public string name;

    //Shortened name of the enchantment
    public string Name() => name.Split("-").Last().Trim();

    //Stats provided by this enchantment
    public Dictionary<string, int> gains;

    //List of item types this enchantment can be put on
    public string type;

    //Description of the enchantment to show on hover in the status bar
    //public Description description;

    //Currently opened enchantment
    public static Enchant enchant;

    //Currently selected enchantment target
    public static Item enchantmentTarget;

    //EXTERNAL FILE: List containing all enchantments in-game
    public static List<Enchant> enchants;

    //List of all filtered enchantments by input search
    public static List<Enchant> enchantsSearch;
}
