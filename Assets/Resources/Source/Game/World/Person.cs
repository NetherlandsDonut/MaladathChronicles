using System;
using System.Collections.Generic;
using System.Linq;
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
    }

    //Name of the person
    public string name;

    //Gender of the person
    public string gender;

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

    public List<StockItem> ExportStock()
    {
        return itemsSold.Select(x => x.Copy<StockItem>()).ToList();
    }

    //Play a sound by this vendor
    public string VendorSound(bool welcome)
    {
        //BASE IT ON RACE AND BOOL
        return "nosound";
    }

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