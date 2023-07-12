using System.Collections.Generic;

public class Ability
{
    //Passive ability
    public Ability(string name)
    {
        this.name = name;
    }

    //Active combat ability
    public Ability(string name, Dictionary<string, int> cost)
    {
        this.name = name;
        this.cost = cost;
    }

    public string name;
    public int levelRequired;
    public Dictionary<string, int> cost;

    public static List<Ability> abilities = new()
    {
        new Ability("Two Handed Axe Proficiency"),
        new Ability("Two Handed Mace Proficiency"),
        new Ability("Two Handed Sword Proficiency"),
        new Ability("One Handed Axe Proficiency"),
        new Ability("One Handed Mace Proficiency"),
        new Ability("One Handed Sword Proficiency"),
        new Ability("Fist Weapon Proficiency"),
        new Ability("Off Hand Proficiency"),
        new Ability("Polearm Proficiency"),
        new Ability("Dagger Proficiency"),
        new Ability("Staff Proficiency"),
        new Ability("Wand Proficiency"),
        new Ability("Totem Proficiency"),
        new Ability("Relic Proficiency"),
        new Ability("Libram Proficiency"),
        new Ability("Idol Proficiency"),
        new Ability("Shield Proficiency"),
        new Ability("Quiver Proficiency"),
        new Ability("Pouch Proficiency"),
        new Ability("Bow Proficiency"),
        new Ability("Crossbow Proficiency"),
        new Ability("Gun Proficiency"),
        new Ability("Cloth Proficiency"),
        new Ability("Leather Proficiency"),
        new Ability("Mail Proficiency"),
        new Ability("Plate Proficiency"),
        new Ability("Envenom", new()
        {
            { "Order", 1 }
        }),
        new Ability("Rupture", new()
        {
            { "Order", 1 }
        }),
        new Ability("Mutilate", new()
        {
            { "Order", 1 }
        }),
        new Ability("Kidney Shot", new()
        {
            { "Order", 1 }
        }),
        new Ability("Evasion", new()
        {
            { "Order", 1 }
        }),
        new Ability("Garrote", new()
        {
            { "Order", 1 }
        }),
    };
}
