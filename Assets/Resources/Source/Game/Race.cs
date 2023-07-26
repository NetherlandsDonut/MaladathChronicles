using System.Collections.Generic;

public class Race
{
    public int level;
    public double moneyLoot;
    public string name, faction, portrait;
    public Stats stats;
    public List<string> abilities;
    public List<(int, string)> loot;

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
            name = "Lord Victor Nefarius",
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
                "Shadowflame Breath"
            },
            loot = new()
            {
                (100, "Nefarian\'s Loot")
            }
        },
        new Race
        {
            level = 60,
            name = "Chief Ukorz Sandscalp",
            portrait = "ChiefUkorz",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 10 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Frostbolt",
                "Scorch"
            },
            loot = new()
            {
                (100, "Ukorz Sandscalp\'s Loot")
            }
        },
        new Race
        {
            level = 46,
            name = "Witch Doctor Zum'rah",
            portrait = "SandTroll",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {
                (100, "Witch Doctor Zum'rah Loot")
            }
        },
        new Race
        {
            level = 54,
            name = "Cannibal Ghoul",
            portrait = "Ghoul",
            faction = "Neutral",
            moneyLoot = 0.0454,
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 54,
            name = "Necromancer",
            portrait = "Ghoul",
            faction = "Neutral",
            moneyLoot = 0.0616,
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 54,
            name = "Cursed Mage",
            portrait = "Ghoul",
            faction = "Neutral",
            moneyLoot = 0.0616,
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 54,
            name = "Rotting Sludge",
            portrait = "Ghoul",
            faction = "Neutral",
            moneyLoot = 0.0616,
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 54,
            name = "Scarlet Cleric",
            portrait = "Ghoul",
            faction = "Neutral",
            moneyLoot = 0.1343,
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 53,
            name = "Scourge Soldier",
            portrait = "SkeletalWarrior",
            faction = "Neutral",
            moneyLoot = 0.0430,
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 54,
            name = "Putrid Gargoyle",
            portrait = "Gargoyle",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 54,
            name = "Scourge Warder",
            portrait = "SkeletalWarrior",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 54,
            name = "Noxious Plaguebat",
            portrait = "Plaguebat",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 53,
            name = "Death Cultist",
            portrait = "SkeletalWarrior",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 53,
            name = "Plaguehound Runt",
            portrait = "Plaguehound",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 53,
            name = "Torn Screamer",
            portrait = "Banshee",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 53,
            name = "Scarlet Warder",
            portrait = "Banshee",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 53,
            name = "Scarlet Enchanter",
            portrait = "Banshee",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 55,
            name = "Dark Summoner",
            portrait = "CryptFiend",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 55,
            name = "Living Decay",
            portrait = "CryptFiend",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 54,
            name = "Blighted Surge",
            portrait = "CryptFiend",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 55,
            name = "Plague Ravager",
            portrait = "CryptFiend",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 53,
            name = "Crypt Fiend",
            portrait = "CryptFiend",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Venomous Bite",
                "Poison Cloud",
                "Web Burst",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 55,
            name = "Crypt Walker",
            portrait = "CryptFiend",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Venomous Bite",
                "Poison Cloud",
                "Web Burst",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 55,
            name = "Plaguehound",
            portrait = "CryptFiend",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        },
        new Race
        {
            level = 53,
            name = "Plaguebat",
            portrait = "CryptFiend",
            faction = "Neutral",
            stats = new Stats(new()
            {
                { "Stamina", 3 },

                { "Strength", 10 },
                { "Agility", 10 },
                { "Intellect", 10 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }),
            abilities = new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            loot = new()
            {

            }
        }
    };
}
