using System.Collections.Generic;

public class Race
{
    public int level;
    public string name, faction;
    public Stats stats;
    public List<string> abilities;

    public static List<Race> races = new()
    {
        new Race
        {
            level = 1,
            name = "Dwarf",
            faction = "Alliance",
            stats = new Stats(new()
            {
                { "Stamina", 1 },

                { "Strength", 1 },
                { "Agility", 1 },
                { "Intellect", 1 },

                { "Earth Mastery", 3 },
                { "Fire Mastery", 2 },
                { "Air Mastery", 3 },
                { "Water Mastery", 1 },
                { "Frost Mastery", 2 },
                { "Lightning Mastery", 2 },
                { "Arcane Mastery", 1 },
                { "Decay Mastery", 1 },
                { "Shadow Mastery", 1 },
                { "Order Mastery", 3 },
            }),
            abilities = new()
            {

            }
        },
        new Race
        {
            level = 1,
            name = "Gnome",
            faction = "Alliance",
            stats = new Stats(new()
            {
                { "Stamina", 1 },

                { "Strength", 1 },
                { "Agility", 1 },
                { "Intellect", 1 },

                { "Earth Mastery", 1 },
                { "Fire Mastery", 3 },
                { "Air Mastery", 2 },
                { "Water Mastery", 1 },
                { "Frost Mastery", 2 },
                { "Lightning Mastery", 1 },
                { "Arcane Mastery", 3 },
                { "Decay Mastery", 2 },
                { "Shadow Mastery", 3 },
                { "Order Mastery", 1 },
            }),
            abilities = new()
            {

            }
        },
        new Race
        {
            level = 1,
            name = "Human",
            faction = "Alliance",
            stats = new Stats(new()
            {
                { "Stamina", 1 },

                { "Strength", 1 },
                { "Agility", 1 },
                { "Intellect", 1 },

                { "Earth Mastery", 1 },
                { "Fire Mastery", 3 },
                { "Air Mastery", 2 },
                { "Water Mastery", 1 },
                { "Frost Mastery", 2 },
                { "Lightning Mastery", 1 },
                { "Arcane Mastery", 3 },
                { "Decay Mastery", 1 },
                { "Shadow Mastery", 2 },
                { "Order Mastery", 3 },
            }),
            abilities = new()
            {

            }
        },
        new Race
        {
            level = 1,
            name = "Night Elf",
            faction = "Alliance",
            stats = new Stats(new()
            {
                { "Stamina", 1 },

                { "Strength", 1 },
                { "Agility", 1 },
                { "Intellect", 1 },

                { "Earth Mastery", 2 },
                { "Fire Mastery", 1 },
                { "Air Mastery", 3 },
                { "Water Mastery", 3 },
                { "Frost Mastery", 1 },
                { "Lightning Mastery", 1 },
                { "Arcane Mastery", 3 },
                { "Decay Mastery", 1 },
                { "Shadow Mastery", 2 },
                { "Order Mastery", 2 },
            }),
            abilities = new()
            {

            }
        },
        new Race
        {
            level = 1,
            name = "Orc",
            faction = "Horde",
            stats = new Stats(new()
            {
                { "Stamina", 1 },

                { "Strength", 1 },
                { "Agility", 1 },
                { "Intellect", 1 },

                { "Earth Mastery", 3 },
                { "Fire Mastery", 3 },
                { "Air Mastery", 2 },
                { "Water Mastery", 1 },
                { "Frost Mastery", 1 },
                { "Lightning Mastery", 3 },
                { "Arcane Mastery", 1 },
                { "Decay Mastery", 2 },
                { "Shadow Mastery", 2 },
                { "Order Mastery", 1 },
            }),
            abilities = new()
            {

            }
        },
        new Race
        {
            level = 1,
            name = "Tauren",
            faction = "Horde",
            stats = new Stats(new()
            {
                { "Stamina", 1 },

                { "Strength", 1 },
                { "Agility", 1 },
                { "Intellect", 1 },

                { "Earth Mastery", 3 },
                { "Fire Mastery", 2 },
                { "Air Mastery", 3 },
                { "Water Mastery", 3 },
                { "Frost Mastery", 1 },
                { "Lightning Mastery", 1 },
                { "Arcane Mastery", 2 },
                { "Decay Mastery", 1 },
                { "Shadow Mastery", 1 },
                { "Order Mastery", 2 },
            }),
            abilities = new()
            {

            }
        },
        new Race
        {
            level = 1,
            name = "Troll",
            faction = "Horde",
            stats = new Stats(new()
            {
                { "Stamina", 1 },

                { "Strength", 1 },
                { "Agility", 1 },
                { "Intellect", 1 },

                { "Earth Mastery", 2 },
                { "Fire Mastery", 3 },
                { "Air Mastery", 2 },
                { "Water Mastery", 1 },
                { "Frost Mastery", 3 },
                { "Lightning Mastery", 2 },
                { "Arcane Mastery", 1 },
                { "Decay Mastery", 1 },
                { "Shadow Mastery", 3 },
                { "Order Mastery", 1 },
            }),
            abilities = new()
            {

            }
        },
        new Race
        {
            level = 1,
            name = "Forsaken",
            faction = "Horde",
            stats = new Stats(new()
            {
                { "Stamina", 1 },

                { "Strength", 1 },
                { "Agility", 1 },
                { "Intellect", 1 },

                { "Earth Mastery", 2 },
                { "Fire Mastery", 2 },
                { "Air Mastery", 2 },
                { "Water Mastery", 1 },
                { "Frost Mastery", 3 },
                { "Lightning Mastery", 1 },
                { "Arcane Mastery", 1 },
                { "Decay Mastery", 3 },
                { "Shadow Mastery", 3 },
                { "Order Mastery", 1 },
            }),
            abilities = new()
            {

            }
        },
        new Race
        {
            level = 21,
            name = "Kobold Digger",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 1 },

                { "Strength", 1 },
                { "Agility", 1 },
                { "Intellect", 1 },

                { "Earth Mastery", 4 },
                { "Fire Mastery", 1 },
                { "Air Mastery", 1 },
                { "Water Mastery", 1 },
                { "Frost Mastery", 1 },
                { "Lightning Mastery", 1 },
                { "Arcane Mastery", 1 },
                { "Decay Mastery", 1 },
                { "Shadow Mastery", 1 },
                { "Order Mastery", 1 },
            }),
            abilities = new()
            {

            }
        },
        new Race
        {
            level = 60,
            name = "Nefarian",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 40 },

                { "Strength", 40 },
                { "Agility", 40 },
                { "Intellect", 40 },

                { "Earth Mastery", 20 },
                { "Fire Mastery", 20 },
                { "Air Mastery", 20 },
                { "Water Mastery", 20 },
                { "Frost Mastery", 20 },
                { "Lightning Mastery", 20 },
                { "Arcane Mastery", 20 },
                { "Decay Mastery", 20 },
                { "Shadow Mastery", 20 },
                { "Order Mastery", 20 },
            }),
            abilities = new()
            {
                "Ass Spell",
                "Piss Ass Spell",
            }
        }
    };
}
