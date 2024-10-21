using System.Collections.Generic;

public class Spec
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        if (abilities != null)
            foreach (var ability in abilities)
                if (!Ability.abilities.Exists(x => x.name == ability.Key))
                    Ability.abilities.Insert(0, new Ability()
                    {
                        name = ability.Key,
                        icon = "Ability" + ability.Key.Clean(),
                        events = new(),
                        tags = new()
                    });
        if (talentTrees != null)
            foreach (var tree in talentTrees)
                foreach (var talent in tree.talents)
                    if (!Ability.abilities.Exists(x => x.name == talent.ability))
                        Ability.abilities.Insert(0, new Ability()
                        {
                            name = talent.ability,
                            icon = "Ability" + talent.ability.Clean(),
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

    //Stats provided by the class to the character
    public Dictionary<string, int> stats;

    //Rules for calculating stats for this class
    //EXAMPLE: "Melee Attack Power per Agility": 2.0
    public Dictionary<string, double> rules;

    //List of abilities this class provides
    //Item1 provides information on the name of the ability provided
    //Item2 provides information about the rank of the ability
    //EXAMPLE: { "Item1": "Fireball", "Item2": 2 }
    public Dictionary<string, int> abilities;

    //Talent trees available to characters of this class
    public List<TalentTree> talentTrees;

    //Currently opened class
    public static Spec spec;

    //EXTERNAL FILE: List containing all specs in-game
    public static List<Spec> specs;

    //List of all filtered specs by input search
    public static List<Spec> specsSearch;
}
