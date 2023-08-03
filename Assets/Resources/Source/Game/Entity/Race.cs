using System.Collections.Generic;

public class Race
{
    public Race(string name, string faction, Stats stats, List<string> abilities)
    {
        this.name = name;
        this.faction = faction;
        this.stats = stats;
        this.abilities = abilities;
    }

    public Race(string name, string rarity, string portrait, double vitality, List<string> abilities, List<(int, string)> loot)
    {
        this.name = name;
        this.rarity = rarity;
        this.portrait = portrait;
        this.vitality = vitality;
        this.abilities = abilities;
        this.loot = loot;
    }

    public int level;
    public string name, faction, rarity, portrait;
    public double vitality;
    public Stats stats;
    public List<string> abilities;
    public List<(int, string)> loot;

    public static List<Race> races = new()
    {
        new Race("Dwarf", "Alliance",
            new Stats(new()
            {
                { "Stamina", 7 },
                { "Strength", 7 },
                { "Agility", 4 },
                { "Intellect", 5 },
                { "Spirit", 0 },

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
            new()
            {
                "Stoneform"
            }
        ),
        new Race("Gnome", "Alliance",
            new Stats(new()
            {
                { "Stamina", 3 },
                { "Strength", 2 },
                { "Agility", 6 },
                { "Intellect", 8 },
                { "Spirit", 0 },

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
            new()
            {

            }
        ),
        new Race("Human", "Alliance",
            new Stats(new()
            {
                { "Stamina", 6 },
                { "Strength", 6 },
                { "Agility", 6 },
                { "Intellect", 6 },
                { "Spirit", 0 },

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
            new()
            {

            }
        ),
        new Race("Night Elf", "Alliance",
            new Stats(new()
            {
                { "Stamina", 5 },
                { "Strength", 4 },
                { "Agility", 7 },
                { "Intellect", 8 },
                { "Spirit", 0 },

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
            new()
            {

            }
        ),
        new Race("Orc", "Horde",
            new Stats(new()
            {
                { "Stamina", 7 },
                { "Strength", 9 },
                { "Agility", 7 },
                { "Intellect", 5 },

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
            new()
            {

            }
        ),
        new Race("Tauren", "Horde",
            new Stats(new()
            {
                { "Stamina", 8 },
                { "Strength", 8 },
                { "Agility", 3 },
                { "Intellect", 6 },
                { "Spirit", 0 },

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
            new()
            {

            }
        ),
        new Race("Troll", "Horde",
            new Stats(new()
            {
                { "Stamina", 7 },
                { "Strength", 6 },
                { "Agility", 7 },
                { "Intellect", 6 },
                { "Spirit", 0 },

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
            new()
            {

            }
        ),
        new Race("Forsaken", "Horde",
            new Stats(new()
            {
                { "Stamina", 5 },
                { "Strength", 5 },
                { "Agility", 6 },
                { "Intellect", 6 },
                { "Spirit", 0 },

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
            new()
            {

            }
        ),
        new Race("Kobold Digger", "Common", "Kobold",
            1.00,
            new()
            {
                "Mortal Strike"
            },
            new()
            {

            }
        ),
        new Race("Lord Victor Nefarius", "Elite", "LordVictorNefarius",
            3.00,
            new()
            {

            },
            new()
            {

            }
        ),
        new Race("Nefarian", "Elite", "Nefarian",
            4.00,
            new()
            {
                "Shadowflame Breath"
            },
            new()
            {
                (100, "Nefarian\'s Loot")
            }
        ),
        new Race("Chief Ukorz Sandscalp", "Elite", "ChiefUkorz",
            2.20,
            new()
            {
                "Frostbolt",
                "Scorch"
            },
            new()
            {
                (100, "Ukorz Sandscalp\'s Loot")
            }
        ),
        new Race("Witch Doctor Zum'rah", "Elite", "SandTroll",
            1.80,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {
                (100, "Witch Doctor Zum'rah Loot")
            }
        ),
        new Race("Cannibal Ghoul", "Common", "Ghoul",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Necromancer", "Common", "Ghoul",
            0.90,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Cursed Mage", "Common", "Ghoul",
            0.90,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Rotting Sludge", "Common", "Ghoul",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Scarlet Cleric", "Common", "Ghoul",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Scourge Soldier", "Common", "SkeletalWarrior",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Putrid Gargoyle", "Common", "Gargoyle",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Scourge Warder", "Common", "SkeletalWarrior",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Noxious Plaguebat", "Common", "Plaguebat",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Death Cultist", "Common", "SkeletalWarrior",
            0.90,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Plaguehound Runt", "Common", "Plaguehound",
            0.90,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Torn Screamer", "Common", "Banshee",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Scarlet Warder", "Common", "Banshee",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Scarlet Enchanter", "Common", "Banshee",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Dark Summoner", "Common", "CryptFiend",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Living Decay", "Common", "CryptFiend",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Blighted Surge", "Common", "CryptFiend",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Plague Ravager", "Common", "CryptFiend",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Crypt Fiend", "Common", "CryptFiend",
            1.10,
            new()
            {
                "Venomous Bite",
                "Withering Cloud",
                "Web Burst",
            },
            new()
            {

            }
        ),
        new Race("Crypt Walker", "Common", "CryptFiend",
            1.10,
            new()
            {
                "Venomous Bite",
                "Withering Cloud",
                "Web Burst",
            },
            new()
            {

            }
        ),
        new Race("Plaguehound", "Common", "CryptFiend",
            1.00,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Plaguebat", "Common", "CryptFiend",
            0.80,
            new()
            {
                "Corruption",
                "Curse Of Agony",
            },
            new()
            {

            }
        ),
        new Race("Duskbat", "Common", "Duskwing",
            0.90,
            new()
            {
                "Putrid Bite",
                "Muscle Tear",
            },
            new()
            {

            }
        ),
        new Race("Wretched Zombie", "Common", "Duskwing",
            1.00,
            new()
            {
                "Putrid Bite",
                "Muscle Tear",
            },
            new()
            {

            }
        ),
        new Race("Rattlecage Skeleton", "Common", "Skeleton",
            1.00,
            new()
            {
                "Putrid Bite",
                "Muscle Tear",
            },
            new()
            {

            }
        )
    };
}
