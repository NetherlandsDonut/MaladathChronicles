using System.Collections.Generic;

public class Class
{
    //Name of the class
    public string name;

    //Path to icon of the class
    public string icon;

    //List of starting equipement for new characters dependant on the entity's race
    //Keys provide information on the race of the character
    //Values provide a list of items that are to be given to the character
    //EXAMPLE: { "Troll": [ "Trapper's Shirt", "Worn Axe" ] } 
    public Dictionary<string, List<string>> startingEquipment;

    //Rules for calculating stats for this class
    //EXAMPLE: "Melee Attack Power per Agility": 2.0
    public Dictionary<string, double> rules;

    //List of abilities this class provides
    //Item1 provides information on the name of the ability provided
    //Item2 provides information at which level of a character the ability is granted
    //EXAMPLE: { "Item1": "Mail Proficiency", "Item2": 40 }
    public List<(string, int)> abilities;

    //Talent trees available to characters of this class
    public List<TalentTree> talentTrees;

    //Currently opened class
    public static Class spec;

    //EXTERNAL FILE: List containing all classes in-game
    public static List<Class> specs;

    //List of all filtered classes by input search
    public static List<Class> specsSearch;
}
