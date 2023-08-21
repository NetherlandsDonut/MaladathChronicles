using System.Collections.Generic;

public class Race
{
    public Race(string name, string faction, Stats stats, List<string> abilities, List<string> maleNames, List<string> femaleNames)
    {
        this.name = name;
        this.faction = faction;
        this.stats = stats;
        this.abilities = abilities;
        this.maleNames = maleNames;
        this.femaleNames = femaleNames;
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
    public List<string> abilities, maleNames, femaleNames;
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
            },
            new()
            {
                "Alfer",
                "Bhorduggs",
                "Gourimmon",
                "Jindagg",
                "Thegneharn",
                "Brudriak",
                "Ringend",
                "Dulfak",
                "Irgam",
                "Suram",
                "Turrhum",
                "Imdiharm",
                "Drorth",
                "Ulokus",
                "Irrunil",
            },
            new()
            {
                "Twome",
                "Leni",
                "Thumen",
                "Dulgu",
                "Brehudi",
                "Ohdol",
                "Gedullun",
                "Sahge",
                "Teshia",
                "Brishua",
                "Nommy",
                "Myle",
                "Immil",
                "Jehdana",
                "Azahva",
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

            },
            new()
            {
                "Leekaz",
                "Thitlin",
                "Metkin",
                "Cebun",
                "Ibokin",
                "Danec",
                "Phibis",
                "Hindic",
                "Gneenbazz",
                "Gneerkic",
                "Oldeeck",
                "Ceecleez",
                "Gnalkafik",
                "Heeklorin",
                "Cencisosh",
            },
            new()
            {
                "Uttlee",
                "Suttlilizz",
                "Nittluk",
                "Gnybrelko",
                "Finklebrell",
                "Myfina",
                "Dotkikeck",
                "Minkabrock",
                "Ulillink",
                "Tholock",
                "Sibezz",
                "Sinky",
                "Hyllin",
                "Sossubles",
                "Pinki",
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

            },
            new()
            {
                "Harv",
                "Zavier",
                "Raoul",
                "Rodman",
                "Maurice",
                "Petrus",
                "Byrne",
                "Torge",
                "Roger",
                "Marsh",
                "Otger",
                "Radbert",
                "Ranier",
                "Wulf",
                "Valentin",
            },
            new()
            {
                "Eliane",
                "Christine",
                "Tine",
                "Ember",
                "Delaney",
                "Abbigail",
                "Delilah",
                "Arlissa",
                "Lisette",
                "Brandi",
                "Valerie",
                "Jennine",
                "Allesha",
                "Nicollete",
                "Celie",
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

            },
            new()
            {
                "Shyl'las",
                "Halnol",
                "Aladoral",
                "Kellor",
                "Fagorm",
                "Alendilad",
                "Ereran",
                "Halaess",
                "Fadries",
                "Sedas",
                "Kellorn",
                "Nytanelle",
                "Seanul",
                "Madearn",
                "Fahdron",
            },
            new()
            {
                "Kylyia",
                "Nheryn",
                "Emysa",
                "Esadya",
                "Ariase",
                "Faenai",
                "Alae",
                "Wyna",
                "Deyell",
                "Amanaya",
                "A'thae",
                "Telvana",
                "Tesdia",
                "Telania",
                "Lenea",
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

            },
            new()
            {
                "Drosh",
                "Zorl",
                "Ikzok",
                "Engus",
                "Horn",
                "Thurzath",
                "Em'gor",
                "Muggoth",
                "Rahlrigg",
                "Balguz",
                "Drurn",
                "Kul",
                "Bodush",
                "Uzul",
                "Kemgorm",
            },
            new()
            {
                "Alda",
                "Gohkiga",
                "Inkoret",
                "Melder",
                "Odam",
                "Masdu'ra",
                "Erzi",
                "Gunerldi",
                "Tergir",
                "Toram",
                "Zalda",
                "Tera",
                "Sherga",
                "Zahgu",
                "Grenomta",
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

            },
            new()
            {
                "Taru",
                "Alsoomse",
                "Agmar",
                "Agasham",
                "Hurlon",
                "Orox",
                "Gellhorn",
                "Harnor",
                "Gullem",
                "Luron",
                "Jo'hsu",
                "Kamar",
                "Mawago",
                "Montarr",
                "Kuruk",
            },
            new()
            {
                "Chianna",
                "Quana",
                "Wauza",
                "Nascha",
                "Adsila",
                "Hannaya",
                "Kaya",
                "Meyo",
                "Lutte",
                "Blomee",
                "Neena",
                "Memdi",
                "Urimu",
                "Takala",
                "Tikke",
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

            },
            new()
            {
                "Luh'daihen",
                "Jin'ta",
                "Jir'sike",
                "Jo'do",
                "Wal'talak",
                "Mul'nakal",
                "Zuxujen",
                "Zas'niaz",
                "Sum'kur",
                "Lo'ghohn",
                "Ikjuz",
                "Tze'sha",
                "Moz'kil",
                "Koshu",
                "Xas'na",
            },
            new()
            {
                "Shazi",
                "Kha'ma",
                "Az'math",
                "Fu'me",
                "Jooz'dilo",
                "Seajeti",
                "Lilmene",
                "Jishin",
                "Eahde",
                "Xinwu",
                "Shalra",
                "Yol'ku",
                "Vuzundah",
                "Sharla",
                "Eho",
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

            },
            new()
            {
                "Martius",
                "Keagan",
                "Trever",
                "Terrence",
                "Merrick",
                "Tyree",
                "Elton",
                "Raghnall",
                "Hadley",
                "Gere",
                "Neville",
                "Armond",
                "Cornelius",
                "Arney",
                "Martin",
            },
            new()
            {
                "Fontanne",
                "Linda",
                "Allesha",
                "Madisen",
                "Loraina",
                "Syndony",
                "Courtney",
                "Salva",
                "Ivona",
                "Bryana",
                "Clarice",
                "Genevie",
                "Steffi",
                "Fleta",
                "Florette",
            }
        ),
        new Race("Dumb Kobold", "Common", "Kobold",
            1.00,
            new()
            {
                "Shadowbolt"
            },
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
        ),
        new Race("Golemagg the Incinerator", "Elite", "MoltenGiant",
            3.00,
            new()
            {
                "Magma Splash",
                "Earthquake",
            },
            new()
            {

            }
        ),
        new Race("Taerar", "Elite", "Taerar",
            3.00,
            new()
            {
                "Tail Sweep",
                "Noxious Breath",
                "Dream Fog",
                "Arcane Blast",
                "Bellowing Roar",
            },
            new()
            {

            }
        ),
        new Race("Emeriss", "Elite", "Emeriss",
            3.00,
            new()
            {
                "Tail Sweep",
                "Noxious Breath",
                "Dream Fog",
                "Spore Cloud",
                "Volatile Infection",
                "Corruption of the Earth",
            },
            new()
            {

            }
        ),
        new Race("Lethon", "Elite", "Lethon",
            3.00,
            new()
            {
                "Tail Sweep",
                "Noxious Breath",
                "Dream Fog",
                "Shadow Bolt Whirl",
                "Draw Spirit",
            },
            new()
            {

            }
        ),
        new Race("Ysondre", "Elite", "Ysondre",
            3.00,
            new()
            {
                "Tail Sweep",
                "Noxious Breath",
                "Dream Fog",
                "Lightning Wave",
                "Moonfire",
                "Curse of Thorns",
            },
            new()
            {

            }
        )
    };
}
