using System.Collections.Generic;

using static Ability;

public class Class
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        if (abilities != null)
            foreach (var ability in abilities)
                if (!abilities.Exists(x => x.name == ability.Item1))
                    abilities.Insert(0, new Ability()
                    {
                        name = ability.Item1,
                        icon = "Ability" + ability.Item1.Replace(" ", ""),
                        events = new(),
                        tags = new()
                    });
        if (talentTrees != null)
            foreach (var tree in talentTrees)
                foreach (var talent in tree.talents)
                    if (!abilities.Exists(x => x.name == talent.ability))
                        abilities.Insert(0, new Ability()
                        {
                            name = talent.ability,
                            icon = "Ability" + talent.ability.Replace(" ", ""),
                            events = new(),
                            tags = new()
                        });
    }

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
