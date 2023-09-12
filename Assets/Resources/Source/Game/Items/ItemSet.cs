using System.Linq;
using System.Collections.Generic;

using static Ability;

public class ItemSet
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        if (setBonuses != null)
            foreach (var bonus in setBonuses)
                if (bonus.abilitiesProvided != null)
                    foreach (var ability in bonus.abilitiesProvided)
                        if (!abilities.Exists(x => x.name == ability))
                            abilities.Insert(0, new Ability()
                            {
                                name = ability,
                                icon = "Ability" + ability,
                                events = new(),
                                tags = new()
                            });
    }

    //Set name
    public string name;

    //List of all set bonuses provided by this set
    //Each set bonus has it's required amount of pieces for it to be activated
    public List<SetBonus> setBonuses;

    //Provides information on how much equipped pieces from this set an entity has
    public int EquippedPieces(Entity entity) => entity.equipment.Count(x => x.Value.set == name);

    //Currently opened item set
    public static ItemSet itemSet;

    //EXTERNAL FILE: List containing all item sets in-game
    public static List<ItemSet> itemSets;

    //List of all filtered item sets by input search
    public static List<ItemSet> itemSetsSearch;
}

public class SetBonus
{
    //Amount of required pieces for the abilities to be provided to the wearer
    public int requiredPieces;

    //Description of the set bonus for the set
    public List<string> description;

    //List of all abilities provided when the wearer has at least the required amount of pieces
    public List<string> abilitiesProvided;
}
