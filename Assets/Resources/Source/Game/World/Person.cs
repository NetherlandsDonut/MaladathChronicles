using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class Person
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        var find = PersonType.personTypes.Find(x => x.type == type);
        if (find == null) Debug.Log("ERROR 005: Did not find person type: \"" + type + "\"");
        else
        {
            category = PersonCategory.personCategories.Find(y => y.category == find.category);
            if (category == null) Debug.Log("ERROR 006: Did not find person category: \"" + find.category + "\"");
        }
        if (itemsSold != null)
            foreach (var stockItem in itemsSold)
                stockItem.Initialise();
        if (voiceVariant == 0)
            if (new List<string> { "GoblinFemale", "HighElfMale", "HighElfFemale", "Broken" }.Contains(race + gender)) voiceVariant = 1;
            else voiceVariant = Root.random.Next(1, 4);
    }

    //Name of the person
    public string name;

    //Gender of the person
    public string gender;

    //Variant of the voice of the character
    public int voiceVariant;

    //Race of the person
    public string race;

    //Faction this person belongs to
    public string faction;

    //Type of this person
    public string type;

    //Is this person hidden in-game
    public bool hidden;

    //Based on this variable vendor's stock will be resupplied
    public List<StockItem> itemsSold;

    //General type of the person that is used for grouping npc's in towns
    [NonSerialized] public PersonCategory category;

    public List<StockItem> ExportStock() => itemsSold.Select(x => x.Copy()).ToList();

    //Play a sound by this vendor
    public string VoiceLine(string type) => race.Clean() + gender + voiceVariant + type;

    //Currently opened person
    public static Person person;
}

public class StockItem
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        if (maxAmount < amount) maxAmount = amount;
    }

    //Item that is being restocked
    public string item;

    //Current stock amount
    public int amount;

    //Aimed stock amount
    public int maxAmount;

    //Amount of time in minutes left for the supplier to be restocked with one piece of the item
    public int minutesLeft;

    //Speed of this item being restocked
    public int restockSpeed;

    public StockItem Copy()
    {
        var newItem = new StockItem();
        newItem.item = item;
        newItem.amount = amount;
        newItem.maxAmount = maxAmount;
        newItem.minutesLeft = minutesLeft;
        newItem.restockSpeed = restockSpeed;
        return newItem;
    }
}

public class PersonType
{
    //Title that is given to npc's of this type
    public string type;

    //Icon of this person type shown next to npc
    public string icon;

    //General type of the person that is used for grouping npc's in towns
    public string category;

    //Indicates whether this person type has different icons depending on faction
    public bool factionVariant;

    //Affiliation of the npc with a specific profession
    public string profession;

    //Range of the profession recipes this person can teach the player
    public int skillCap;

    //EXTERNAL FILE: List containing all person types in-game
    public static List<PersonType> personTypes;
}

public class PersonCategory
{
    //Category name
    public string category;

    //Icon of this person category shown next to npc
    public string icon;

    //Indicates whether persons of this category have different icons depending on faction
    public bool factionVariant;

    //Importance of this category in site tooltips, the lower the more privileged
    public int priority;

    //Currently opened person category
    public static PersonCategory personCategory;

    //EXTERNAL FILE: List containing all person categories in-game
    public static List<PersonCategory> personCategories;
}