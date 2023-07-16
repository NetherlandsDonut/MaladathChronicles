using System.Collections.Generic;

public class Class
{
    public Class(string name, List<(string, int)> abilities)
    {
        this.name = name;
        this.abilities = abilities;
    }

    public string name;
    public List<(string, int)> abilities;

    public static List<Class> classes = new()
    {
        new Class("Rogue",
            new List<(string, int)>
            {
                ("One Handed Axe Proficiency", 1),
                ("One Handed Mace Proficiency", 1),
                ("One Handed Sword Proficiency", 1),
                ("Fist Weapon Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Dagger Proficiency", 1),
                ("Leather Proficiency", 1),
                ("Envenom", 1),
                ("Kidney Shot", 1),
                ("Mutilate", 1),
                ("Evasion", 1),
                ("Garrote", 1),
                ("Rupture", 1)
            }
        ),
        new Class("Hunter",
            new List<(string, int)>
            {
                ("Two Handed Axe Proficiency", 1),
                ("Two Handed Sword Proficiency", 1),
                ("One Handed Axe Proficiency", 1),
                ("One Handed Sword Proficiency", 1),
                ("Polearm Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Quiver Proficiency", 1),
                ("Pouch Proficiency", 1),
                ("Bow Proficiency", 1),
                ("Crossbow Proficiency", 1),
                ("Gun Proficiency", 1),
                ("Leather Proficiency", 1),
                ("Mail Proficiency", 40),
            }
        ),
        new Class("Warrior",
            new List<(string, int)>
            {
                ("Two Handed Axe Proficiency", 1),
                ("Two Handed Mace Proficiency", 1),
                ("Two Handed Sword Proficiency", 1),
                ("One Handed Axe Proficiency", 1),
                ("One Handed Mace Proficiency", 1),
                ("One Handed Sword Proficiency", 1),
                ("Fist Weapon Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Polearm Proficiency", 1),
                ("Shield Proficiency", 1),
                ("Mail Proficiency", 1),
                ("Plate Proficiency", 40),
            }
        ),
        new Class("Paladin",
            new List<(string, int)>
            {
                ("Two Handed Axe Proficiency", 1),
                ("Two Handed Mace Proficiency", 1),
                ("Two Handed Sword Proficiency", 1),
                ("One Handed Axe Proficiency", 1),
                ("One Handed Mace Proficiency", 1),
                ("One Handed Sword Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Polearm Proficiency", 1),
                ("Libram Proficiency", 1),
                ("Shield Proficiency", 1),
                ("Mail Proficiency", 1),
                ("Plate Proficiency", 40),
            }
        ),
        new Class("Druid",
            new List<(string, int)>
            {
                ("Two Handed Axe Proficiency", 1),
                ("Two Handed Mace Proficiency", 1),
                ("Two Handed Sword Proficiency", 1),
                ("One Handed Axe Proficiency", 1),
                ("One Handed Mace Proficiency", 1),
                ("One Handed Sword Proficiency", 1),
                ("Fist Weapon Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Polearm Proficiency", 1),
                ("Dagger Proficiency", 1),
                ("Staff Proficiency", 1),
                ("Wand Proficiency", 1),
                ("Totem Proficiency", 1),
                ("Libram Proficiency", 1),
                ("Idol Proficiency", 1),
                ("Shield Proficiency", 1),
                ("Quiver Proficiency", 1),
                ("Pouch Proficiency", 1),
                ("Bow Proficiency", 1),
                ("Crossbow Proficiency", 1),
                ("Gun Proficiency", 1),

                ("Leather Proficiency", 1),
            }
        ),
        new Class("Priest",
            new List<(string, int)>
            {
                ("Two Handed Axe Proficiency", 1),
                ("Two Handed Mace Proficiency", 1),
                ("Two Handed Sword Proficiency", 1),
                ("One Handed Axe Proficiency", 1),
                ("One Handed Mace Proficiency", 1),
                ("One Handed Sword Proficiency", 1),
                ("Fist Weapon Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Polearm Proficiency", 1),
                ("Dagger Proficiency", 1),
                ("Staff Proficiency", 1),
                ("Wand Proficiency", 1),
                ("Totem Proficiency", 1),
                ("Libram Proficiency", 1),
                ("Idol Proficiency", 1),
                ("Shield Proficiency", 1),
                ("Quiver Proficiency", 1),
                ("Pouch Proficiency", 1),
                ("Bow Proficiency", 1),
                ("Crossbow Proficiency", 1),
                ("Gun Proficiency", 1),

                ("Cloth Proficiency", 1),
            }
        ),
        new Class("Warlock",
            new List<(string, int)>
            {
                ("Two Handed Axe Proficiency", 1),
                ("Two Handed Mace Proficiency", 1),
                ("Two Handed Sword Proficiency", 1),
                ("One Handed Axe Proficiency", 1),
                ("One Handed Mace Proficiency", 1),
                ("One Handed Sword Proficiency", 1),
                ("Fist Weapon Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Polearm Proficiency", 1),
                ("Dagger Proficiency", 1),
                ("Staff Proficiency", 1),
                ("Wand Proficiency", 1),
                ("Totem Proficiency", 1),
                ("Libram Proficiency", 1),
                ("Idol Proficiency", 1),
                ("Shield Proficiency", 1),
                ("Quiver Proficiency", 1),
                ("Pouch Proficiency", 1),
                ("Bow Proficiency", 1),
                ("Crossbow Proficiency", 1),
                ("Gun Proficiency", 1),

                ("Cloth Proficiency", 1),
            }
        ),
        new Class("Mage",
            new List<(string, int)>
            {
                ("One Handed Sword Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Dagger Proficiency", 1),
                ("Staff Proficiency", 1),
                ("Wand Proficiency", 1),
                ("Cloth Proficiency", 1),
                ("Frostbolt", 1),
                ("Ice Lance", 1),
                ("Freezing Nova", 1),
                //("Blizzard", 1),
                ("Deep Freeze", 1),
            }
        ),
    };
}
