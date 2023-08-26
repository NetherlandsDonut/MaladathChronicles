using System.Linq;
using System.Collections.Generic;

public class ItemSet
{
    public string name;
    public List<SetBonus> setBonuses;

    public int EquippedPieces(Entity entity) => entity.equipment.Count(x => x.Value.set == name);

    public static ItemSet itemSet;
    public static List<ItemSet> itemSets, itemSetsSearch;
}

public class SetBonus
{
    public int requiredPieces;
    public List<string> description;
    public List<string> abilitiesProvided;
}
