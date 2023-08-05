using System.Linq;
using System.Collections.Generic;

using static Root;

public class SiteHostileArea
{
    public SiteHostileArea(string name, List<(string, string)> possibleEncounters)
    {
        this.name = name;
        this.possibleEncounters = new();
        foreach (var encounter in possibleEncounters)
        {
            var split = encounter.Item1.Split("-");
            this.possibleEncounters.Add((int.Parse(split[0]), int.Parse(split[split.Length == 1 ? 0 : 1]), encounter.Item2));
        }
        recommendedLevel = (int)this.possibleEncounters.Average(x => (x.Item1 + x.Item2) / 2.0);
    }

    public SiteHostileArea(string name, List<(string, string)> possibleEncounters, List<(int, string, string)> bossEncounters)
    {
        this.name = name;
        this.possibleEncounters = new();
        foreach (var encounter in possibleEncounters)
        {
            var split = encounter.Item1.Split("-");
            this.possibleEncounters.Add((int.Parse(split[0]), int.Parse(split[split.Length == 1 ? 0 : 1]), encounter.Item2));
        }
        this.bossEncounters = bossEncounters.Select(x => (x.Item1, int.Parse(x.Item2), x.Item3)).ToList();
        recommendedLevel = (int)this.bossEncounters.Average(x => x.Item2);
    }

    public Entity RollEncounter()
    {
        var encounters = possibleEncounters.Select(x => (random.Next(x.Item1, x.Item2 + 1), Race.races.Find(y => y.name == x.Item3))).ToList();
        if (random.Next(0, 100) < 1)
        {
            var rares = encounters.FindAll(x => x.Item2.rarity == "Rare");
            rares = rares.FindAll(x => !currentSave.rareKilled.Contains(x.Item2.name));
            if (rares.Count > 0)
                encounters = new() { rares[random.Next(0, rares.Count)] };
        }
        var find = encounters[random.Next(0, encounters.Count)];
        return new Entity(find.Item1, find.Item2);
    }
    
    public Entity RollBoss((int, int, string) data)
    {
        return new Entity(data.Item2, Race.races.Find(x => x.name == data.Item3));
    }

    public string name;
    public int recommendedLevel;
    public bool instancePart;
    public List<(int, int, string)> possibleEncounters;
    public List<(int, int, string)> bossEncounters;

    public static List<SiteHostileArea> hostileAreas = new()
    {
        new SiteHostileArea("King's Square", new()
            {
                ("55-56", "Scourge Warder"),
                ("55-56", "Putrid Gargoyle"),
                ("55-56", "Necromancer"),
                ("55-56", "Cursed Mage"),
                ("55-56", "Cannibal Ghoul"),
                ("55-56", "Death Cultist"),
            },
            new()
            {
                (04, "56", "Postmaster Malown")
            }
        ),
        new SiteHostileArea("Market Row", new()
            {
                ("56-57", "Cannibal Ghoul"),
            },
            new()
            {
                (04, "57", "The Unforgiven")
            }
        ),
        new SiteHostileArea("Crusaders' Square", new()
            {
                ("56", "Cannibal Ghoul"),
            },
            new()
            {
                (04, "58", "Timmy the Cruel")
            }
        ),
        new SiteHostileArea("The Scarlet Bastion", new()
            {
                ("59-60", "Cannibal Ghoul"),
            },
            new()
            {
                (04, "60", "Malor the Zealous")
            }
        ),
        new SiteHostileArea("The Crimson Throne", new()
            {
                ("59-60", "Cannibal Ghoul"),
            },
            new()
            {
                (04, "62", "Balnazzar")
            }
        ),
        new SiteHostileArea("Elder's Square", new()
            {
                ("59-60", "Cannibal Ghoul"),
            },
            new()
            {
                (04, "60", "Magistrate Barthilas")
            }
        ),
        new SiteHostileArea("The Gauntlet", new()
            {
                ("59-60", "Cannibal Ghoul"),
            },
            new()
            {
                (04, "60", "Nerub'enkan")
            }
        ),
        new SiteHostileArea("Slaughter Square", new()
            {
                ("59-60", "Cannibal Ghoul"),
            },
            new()
            {
                (04, "61", "Ramstein the Gorger")
            }
        ),
        new SiteHostileArea("The Slaughter House", new()
            {
                ("59-60", "Cannibal Ghoul"),
            },
            new()
            {
                (04, "62", "Baron Rivendare")
            }
        ),
        new SiteHostileArea("Prison Of Immol'Thar", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        }),
        new SiteHostileArea("Deathknell", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        }),
        new SiteHostileArea("Corin's Crossing", new()
        {
            ("53-54", "Scourge Warder"),
            ("53-54", "Dark Summoner"),
        }),
        new SiteHostileArea("Blackwood Lake", new()
        {
            ("53-54", "Plaguehound"),
            ("53-54", "Noxious Plaguebat"),
        }),
        new SiteHostileArea("Lake Mereldar", new()
        {
            ("53-54", "Blighted Surge"),
            ("53-54", "Plague Ravager"),
        }),
        new SiteHostileArea("Pestilent Scar",  new()
        {
            ("53-54", "Living Decay"),
            ("53-54", "Plaguehound"),
            ("53-54", "Noxious Plaguebat"),
            ("53-54", "Rotting Sludge"),
        }),
        new SiteHostileArea("Plaguewood", new()
        {
            ("53-54", "Scourge Warder"),
            ("53-54", "Putrid Gargoyle"),
            ("53-54", "Necromancer"),
            ("53-54", "Cursed Mage"),
            ("53-54", "Cannibal Ghoul"),
            ("53-54", "Death Cultist"),
        }),
        new SiteHostileArea("Terrordale", new()
        {
            ("52-53", "Cursed Mage"),
            ("52-53", "Cannibal Ghoul"),
            ("52-53", "Scourge Soldier"),
            ("52-53", "Crypt Fiend"),
            ("52-53", "Torn Screamer"),
        }),
        new SiteHostileArea("Terrorweb Tunnel", new()
        {
            ("55-56", "Crypt Fiend"),
            ("55-56", "Crypt Walker"),
        }),
        new SiteHostileArea("Darrowshire", new()
        {
            ("52-53", "Plaguehound Runt"),
            ("52-53", "Scourge Soldier"),
        }),
        new SiteHostileArea("Thondroril River", new()
        {
            ("52-53", "Plaguehound Runt"),
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Tyr's Hand", new()
        {
            ("52-53", "Scarlet Curate"),
            ("52-53", "Scarlet Warder"),
            ("52-53", "Scarlet Enchanter"),
            ("52-53", "Scarlet Cleric"),
        }),
        new SiteHostileArea("The Marris Stead", new()
        {
            ("52-53", "Putrid Gargoyle"),
            ("52-53", "Plaguebat"),
            ("52-53", "Plaguehound Runt"),
        }),
        new SiteHostileArea("Stonetalon Peak", new()
            {
                ("60", "Nefarian"),
            }
        ),
        new SiteHostileArea("Scarab Terrace", new()
            {
                ("60", "Qiraji Gladiator"),
                ("60", "Qiraji Swarmguard"),
                ("60", "Hive'Zara Stinger"),
                ("60", "Hive'Zara Wasp"),
            },
            new()
            {
                (04, "60", "Kurinnaxx")
            }
        ),
        new SiteHostileArea("General's Terrace", new()
            {
                ("60", "Qiraji Gladiator"),
                ("60", "Qiraji Warrior"),
                ("60", "Swarmguard Needler"),
            },
            new()
            {
                (04, "60", "General Rajaxx")
            }
        ),
        new SiteHostileArea("Reservoir", new()
            {
                ("60", "Flesh Hunter"),
                ("60", "Obsidian Destroyer"),
            },
            new()
            {
                (03, "60", "Moam")
            }
        ),
        new SiteHostileArea("Hatchery", new()
            {
                ("60", "Flesh Hunter"),
                ("60", "Hive'Zara Sandstalker"),
                ("60", "Hive'Zara Soldier"),
            },
            new()
            {
                (04, "60", "Buru The Gorger")
            }
        ),
        new SiteHostileArea("Comb", new()
            {
                ("60", "Hive'Zara Collector"),
                ("60", "Hive'Zara Drone"),
                ("60", "Hive'Zara Swarmer"),
                ("60", "Hive'Zara Tail Lasher"),
                ("60", "Silicate Feeder"),
            },
            new()
            {
                (06, "60", "Ayamiss The Hunter")
            }
        ),
        new SiteHostileArea("Watcher's Terrace", new()
            {
                ("60", "Anubisath Guardian")
            },
            new()
            {
                (02, "60", "Ossirian The Unscarred")
            }
        ),
    };
}
