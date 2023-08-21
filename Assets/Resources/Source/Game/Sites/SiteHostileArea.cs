using System.Linq;
using System.Collections.Generic;

using static Root;

public class SiteHostileArea
{
    public SiteHostileArea(string name, string zone, string subType, List<(string, string)> possibleEncounters)
    {
        this.name = name;
        this.zone = zone;
        this.subType = subType;
        this.possibleEncounters = new();
        foreach (var encounter in possibleEncounters)
        {
            var split = encounter.Item1.Split("-");
            this.possibleEncounters.Add((int.Parse(split[0]), int.Parse(split[split.Length == 1 ? 0 : 1]), encounter.Item2));
        }
        if (this.possibleEncounters.Count > 0)
            recommendedLevel = (int)this.possibleEncounters.Average(x => (x.Item1 + x.Item2) / 2.0);
    }

    public SiteHostileArea(string name, string zone, string subType, List<(string, string)> possibleEncounters, List<(int, string, string)> bossEncounters)
    {
        this.name = name;
        this.zone = zone;
        this.subType = subType;
        this.possibleEncounters = new();
        foreach (var encounter in possibleEncounters)
        {
            var split = encounter.Item1.Split("-");
            this.possibleEncounters.Add((int.Parse(split[0]), int.Parse(split[split.Length == 1 ? 0 : 1]), encounter.Item2));
        }
        this.bossEncounters = bossEncounters.Select(x => (x.Item1, int.Parse(x.Item2), x.Item3)).ToList();
        if (this.bossEncounters.Count > 0)
            recommendedLevel = (int)this.bossEncounters.Average(x => x.Item2);
    }

    public Entity RollEncounter()
    {
        var encounters = possibleEncounters.Select(x => (random.Next(x.Item1, x.Item2 + 1), Race.races.Find(y => y.name == x.Item3))).ToList();
        if (random.Next(0, 100) < 1)
        {
            var rares = encounters.FindAll(x => x.Item2.rarity == "Rare");
            rares = rares.FindAll(x => !currentSave.raresKilled.Contains(x.Item2.name));
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

    public string name, zone, subType;
    public int recommendedLevel;
    public bool instancePart, complexPart;
    public List<(int, int, string)> possibleEncounters;
    public List<(int, int, string)> bossEncounters;

    public static SiteHostileArea area;

    public static List<SiteHostileArea> hostileAreas = new()
    {
        #region Stratholme

        new SiteHostileArea("King's Square", "Stratholme", "HostileArea", new()
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
        }),
        new SiteHostileArea("Market Row", "Stratholme", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "The Unforgiven")
        }),
        new SiteHostileArea("Crusaders' Square", "Stratholme", "HostileArea", new()
        {
            ("56", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "58", "Timmy the Cruel")
        }),
        new SiteHostileArea("The Hoard", "Stratholme", "HostileArea", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Malor the Zealous")
        }),
        new SiteHostileArea("The Hall of Lights", "Stratholme", "HostileArea", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Malor the Zealous")
        }),
        new SiteHostileArea("The Scarlet Bastion", "Stratholme", "HostileArea", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Malor the Zealous")
        }),
        new SiteHostileArea("The Crimson Throne", "Stratholme", "HostileArea", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "62", "Balnazzar")
        }),
        new SiteHostileArea("Elder's Square", "Stratholme", "HostileArea", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Magistrate Barthilas")
        }),
        new SiteHostileArea("The Gauntlet", "Stratholme", "HostileArea", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Nerub'enkan")
        }),
        new SiteHostileArea("Slaughter Square", "Stratholme", "HostileArea", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "61", "Ramstein the Gorger")
        }),
        new SiteHostileArea("The Slaughter House", "Stratholme", "HostileArea", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "62", "Baron Rivendare")
        }),

        #endregion

        #region Razorfen Downs

        new SiteHostileArea("The Caller's Chamber", "Razorfen Downs", "HostileArea", new()
        {
            ("40", "Ragglesnout"),
        },
        new()
        {
            (2, "40", "Plaguemaw"),
            (2, "40", "Tuten'kash")
        }),
        new SiteHostileArea("The Bone Pile", "Razorfen Downs", "HostileArea", new()
        {
            ("40", "Ragglesnout"),
        },
        new()
        {
            (2, "39", "Mordresh Fire Eye"),
        }),
        new SiteHostileArea("Spiral of Thorns", "Razorfen Downs", "HostileArea", new()
        {
            ("40", "Ragglesnout"),
        },
        new()
        {
            (2, "40", "Glutton"),
            (2, "41", "Amnennar the Coldbringer"),
        }),

        #endregion
        
        #region Razorfen Kraul

        new SiteHostileArea("Kraul Commons", "Razorfen Kraul", "HostileArea", new()
        {
            ("40", "Geomancer Acolyte"),
            ("40", "Razorfen Beast Stalker"),
            ("40", "Razorfen Geomagus"),
        },
        new()
        {
            (2, "40", "Hunter Bonetusk"),
            (2, "28", "Roogug")
        }),
        new SiteHostileArea("Kraul Drain", "Razorfen Kraul", "HostileArea", new()
        {

        },
        new()
        {

        }),
        new SiteHostileArea("Central Grounds", "Razorfen Kraul", "HostileArea", new()
        {
            ("40", "Razorfen Kraulshaper"),
            ("40", "Razorfen Scarbalde"),
            ("40", "Razorfen Huntmaster"),
        },
        new()
        {
            (2, "40", "Warlord Ramtusk")
        }),
        new SiteHostileArea("Bat Caves", "Razorfen Kraul", "HostileArea", new()
        {
            ("40", "Kraulshaped Monstrosity"),
            ("40", "Razorfen Kraulshaper"),
            ("40", "Vile Bat")
        },
        new()
        {
            (2, "40", "Groyat, the Blind Hunter")
        }),
        new SiteHostileArea("Charlga's Seat", "Razorfen Kraul", "HostileArea", new()
        {
            ("40", "Enormous Bullfrog"),
            ("40", "Razorfen Hidecrusher"),
            ("40", "Razorfen Stonechanter"),
            ("40", "Razorfen Thornbolt")
        },
        new()
        {
            (2, "33", "Charlga Razorflank")
        }),

        #endregion

        #region The Deadmines

        new SiteHostileArea("Defias Hideout", "The Deadmines", "HostileArea", new()
        {
            ("19", "Miner Johnson"),
            ("17-18", "Defias Evoker"),
            ("16-17", "Defias Watchman"),
            ("17-18", "Defias Overseer"),
            ("17-18", "Defias Miner")
        },
        new()
        {
            (3, "19", "Rhahk'Zor"),
        }),
        new SiteHostileArea("Mast Room", "The Deadmines", "HostileArea", new()
        {
            ("18-19", "Defias Taskmaster"),
            ("18-19", "Defias Wizard"),
            ("18-19", "Defias Strip Miner")
        },
        new()
        {
            (3, "19", "Sneed"),
        }),
        new SiteHostileArea("Goblin Foundry", "The Deadmines", "HostileArea", new()
        {
            ("18-20", "Remote Controlled Golem"),
            ("18-19", "Goblin Engineer")
        },
        new()
        {
            (3, "20", "Gilnid"),
        }),
        new SiteHostileArea("Ironclad Cove", "The Deadmines", "HostileArea", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "20", "Mr. Smite"),
        }),
        new SiteHostileArea("The Juggernaut", "The Deadmines", "HostileArea", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "20", "Captain Greenskin"),
            (2, "21", "Edwin VanCleef"),
        }),

        #endregion
        
        #region Scholomance
        
        new SiteHostileArea("Reliquary", "Scholomance", "HostileArea", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "59", "Blood Steward of Kirtonos")
        }),
        new SiteHostileArea("Chamber of Summoning", "Scholomance", "HostileArea", new()
        {
            ("18-19", "Defias Taskmaster"),
            ("18-19", "Defias Wizard"),
            ("18-19", "Defias Strip Miner")
        },
        new()
        {
            (3, "59", "Kirtonos the Herald"),
        }),
        new SiteHostileArea("Great Ossuary", "Scholomance", "HostileArea", new()
        {
            ("18-19", "Defias Taskmaster"),
            ("18-19", "Defias Wizard"),
            ("18-19", "Defias Strip Miner")
        },
        new()
        {
            (3, "61", "Rattlegore"),
        }),
        new SiteHostileArea("Hall of Secrets", "Scholomance", "HostileArea", new()
        {
            ("18-20", "Remote Controlled Golem"),
            ("18-19", "Goblin Engineer")
        },
        new()
        {
            (3, "60", "Lorekeeper Polkelt"),
        }),
        new SiteHostileArea("Hall of the Damned", "Scholomance", "HostileArea", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "60", "Doctor Theolen Krastinov"),
        }),
        new SiteHostileArea("Laboratory", "Scholomance", "HostileArea", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "62", "Ras Frostwhisper")
        }),
        new SiteHostileArea("Vault of the Ravenian", "Scholomance", "HostileArea", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "60", "The Ravenian")
        }),
        new SiteHostileArea("The Coven", "Scholomance", "HostileArea", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "60", "Instructor Malicia")
        }),
        new SiteHostileArea("The Shadow Vault", "Scholomance", "HostileArea", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "60", "Instructor Malicia")
        }),
        new SiteHostileArea("Viewing Room", "Scholomance", "HostileArea", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "60", "Vectus"),
        }),
        new SiteHostileArea("Barov Family Vault", "Scholomance", "HostileArea", new()
        {
            ("19", "Miner Johnson"),
            ("17-18", "Defias Evoker"),
            ("16-17", "Defias Watchman"),
            ("17-18", "Defias Overseer"),
            ("17-18", "Defias Miner")
        },
        new()
        {
            (3, "60", "Lord Alexei Barov"),
        }),
        new SiteHostileArea("Headmaster's Study", "Scholomance", "HostileArea", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "61", "Darkmaster Gandling"),
        }),

        #endregion

        #region Maraudon

        new SiteHostileArea("The Noxious Hollow", "Maraudon", "HostileArea", new()
        {
            ("45-46", "Creeping Sludge"),
            ("45-46", "Spewed Larva"),
            ("45-46", "Constrictor Vine")
        },
        new()
        {
            (3, "47", "Noxxion"),
        }),
        new SiteHostileArea("Foulspore Cavern", "Maraudon", "HostileArea", new()
        {
            ("46-47", "Barbed Lasher"),
            ("46-48", "Celebrian Dryad"),
            ("46-47", "Deeprot Stomper")
        },
        new()
        {
            (3, "48", "Razorlash"),
        }),
        new SiteHostileArea("Wicked Grotto", "Maraudon", "HostileArea", new()
        {
            ("47-48", "Deeprot Stomper"),
            ("46-47", "Deeprot Tangler"),
            ("46-47", "Poison Sprite")
        },
        new()
        {
            (3, "48", "Tinkerer Gizlock"),
        }),
        new SiteHostileArea("Vyletongue Seat", "Maraudon", "HostileArea", new()
        {
            ("46-47", "Putridus Satyr"),
            ("47-48", "Putridus Shadowstalker"),
            ("46-47", "Putridus Trickster")
        },
        new()
        {
            (3, "48", "Lord Vyletongue"),
        }),
        new SiteHostileArea("Poison Falls", "Maraudon", "HostileArea", new()
        {
            ("48", "Meshlock the Harvester"),
            ("47-48", "Barbed Lasher"),
            ("48-49", "Celebrian Dryad"),
            ("47-48", "Cavern Lurker")
        },
        new()
        {
            (3, "49", "Celebras the Cursed"),
        }),
        new SiteHostileArea("Earth Song Falls", "Maraudon", "HostileArea", new()
        {
            ("50", "Rotgrip"),
            ("48-49", "Primordial Behemoth"),
            ("48-49", "Theradrim Guardian")
        },
        new()
        {
            (3, "50", "Landslide"),
        }),
        new SiteHostileArea("Zaetar's Grave", "Maraudon", "HostileArea", new()
        {
            ("48-50", "Primordial Behemoth"),
            ("49-50", "Thessala Hydra"),
            ("49-50", "Subterranean Diemetradon"),
            ("48-50", "Deep Borer")
        },
        new()
        {
            (3, "51", "Princess Theradras"),
        }),

        #endregion
        
        #region Gnomeregan

        new SiteHostileArea("Trogg Caves", "Gnomeregan", "HostileArea", new()
        {
            ("27-28", "Caverndeep Burrower"),
            ("28-29", "Irradiated Pillager"),
            ("28-29", "Caverndeep Ambusher")
        },
        new()
        {
            (3, "30", "Grubbis"),
        }),
        new SiteHostileArea("Hall of Gears", "Gnomeregan", "HostileArea", new()
        {
            ("29-30", "Corrosive Lurker"),
            ("29-30", "Irradiated Slime"),
            ("29-31", "Irradiated Horror")
        },
        new()
        {
            (3, "31", "Viscous Fallout"),
        }),
        new SiteHostileArea("Launch Bay", "Gnomeregan", "HostileArea", new()
        {
            ("30-31", "Leprous Technician"),
            ("31-32", "Mobile Alert System"),
            ("31-32", "Mechanized Sentry")
        },
        new()
        {
            (3, "32", "Electrocutioner 6000"),
        }),
        new SiteHostileArea("Engineering Labs", "Gnomeregan", "HostileArea", new()
        {
            ("32-33", "Mechano Tank"),
            ("31-33", "Mobile Alert System")
        },
        new()
        {
            (3, "33", "Crowd Pummeler 9-60"),
        }),
        new SiteHostileArea("Tinkers' Court", "Gnomeregan", "HostileArea", new()
        {

        },
        new()
        {
            (0, "34", "Mekgineer Thermaplugg"),
        }),

        #endregion

        #region Wailing Caverns

        new SiteHostileArea("Screaming Gully", "Wailing Caverns", "HostileArea", new()
        {
            ("18-19", "Deviate Guardian"),
            ("18-19", "Deviate Ravager"),
            ("19-20", "Druid of the Fang"),
            ("18-19", "Evolving Ectoplasm")
        },
        new()
        {
            (3, "20", "Lady Anacondra"),
        }),
        new SiteHostileArea("Pit of Fangs", "Wailing Caverns", "HostileArea", new()
        {
            ("18-19", "Deviate Adder"),
            ("18-20", "Deviate Python"),
            ("19-20", "Deviate Viper"),
            ("20", "Kresh")
        },
        new()
        {
            (3, "20", "Lord Cobrahn"),
            (2, "21", "Lord Pythas"),
        }),
        new SiteHostileArea("Winding Chasm", "Wailing Caverns", "HostileArea", new()
        {
            ("20-21", "Deviate Dreadfang"),
            ("19-20", "Deviate Lasher"),
            ("19-20", "Deviate Shambler"),
            ("20-21", "Deviate Venomwing"),
        },
        new()
        {
            (2, "21", "Skum"),
        }),
        new SiteHostileArea("Crag of the Everliving", "Wailing Caverns", "HostileArea", new()
        {
            ("20-21", "Deviate Dreadfang"),
            ("19-20", "Deviate Lasher"),
            ("19-20", "Deviate Shambler"),
            ("20-21", "Deviate Venomwing"),
            ("19-20", "Druid of the Fang"),
            ("20", "Deviate Faerie Dragon")
        },
        new()
        {
            (2, "21", "Lord Serpentis"),
            (1, "21", "Verdan the Everliving")
        }),
        new SiteHostileArea("Dreamer's Rock", "Wailing Caverns", "HostileArea", new()
        {
            ("20-21", "Deviate Moccasin"),
            ("20-21", "Evolving Ectoplasm"),
            ("20-21", "Nightmare Ectoplasm"),
        },
        new()
        {
            (3, "22", "Mutantus The Devourer"),
        }),

        #endregion

        #region Blackfathom Deeps
        
        new SiteHostileArea("Pool of Ask'ar", "Blackfathom Deeps", "HostileArea", new()
        {
            ("23-24", "Blackfathom Myrmidon"),
            ("23-24", "Blackfathom Sea Witch"),
            ("23-24", "Mudrock Snapjaw")
        },
        new()
        {
            (2, "25", "Ghamoo Ra"),
            (1, "25", "Lady Sarevess")
        }),
        new SiteHostileArea("Shrine of Gelihast", "Blackfathom Deeps", "HostileArea", new()
        {
            ("25-26", "Blindlight Oracle"),
            ("25-26", "Blindlight Muckdweller")
        },
        new()
        {
            (2, "26", "Gelihast"),
        }),
        new SiteHostileArea("Moonshrine Ruins", "Blackfathom Deeps", "HostileArea", new()
        {
            ("26", "Aqua Guardian"),
            ("25-26", "Twilight Acolyte"),
            ("25-27", "Twilight Aquamancer")
        },
        new()
        {
            (3, "27", "Baron Aquanis"),
        }),
        new SiteHostileArea("Forgotten Pool", "Blackfathom Deeps", "HostileArea", new()
        {
            ("26-27", "Deep Pool Threshkin"),
            ("26-27", "Skittering Crustacean")
        },
        new()
        {
            (2, "27", "Old Serra'kis"),
        }),
        new SiteHostileArea("Moonshrine Sanctum", "Blackfathom Deeps", "HostileArea", new()
        {
            ("27-28", "Twilight Elementalist"),
            ("27-28", "Twilight Shadowmage")
        },
        new()
        {
            (2, "28", "Twilight Lord Kelris"),
        }),
        new SiteHostileArea("Aku'mai's Lair", "Blackfathom Deeps", "HostileArea", new() { },
        new()
        {
            (0, "28", "Aku'mai"),
        }),
        
        #endregion
        
        #region Ruins of Stromgarde
        
        new SiteHostileArea("North Town", "Ruins of Stromgarde", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("The Sanctum", "Ruins of Stromgarde", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Western Town", "Ruins of Stromgarde", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Tower of Arathor", "Ruins of Stromgarde", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Stromgarde Keep", "Ruins of Stromgarde", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),

        #endregion

        #region Ruins of Alterac
        
        new SiteHostileArea("Town Center", "Ruins of Alterac", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Alterac Chapel", "Ruins of Alterac", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Alterac Keep", "Ruins of Alterac", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),

        #endregion

        #region Purgation Isle
        
        new SiteHostileArea("Isle Landing", "Purgation Isle", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Mountain Peak", "Purgation Isle", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),

        #endregion

        #region Scarlet Monastery Grounds

        new SiteHostileArea("Whispering Gardens", "Scarlet Monastery Grounds", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Terrace of Repose", "Scarlet Monastery Grounds", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("The Grand Vestibule", "Scarlet Monastery Grounds", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),

        new SiteHostileArea("Chamber of Atonement", "Scarlet Monastery", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Interrogator Vishas")
        }),
        new SiteHostileArea("Forlorn Cloister", "Scarlet Monastery", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Bloodmage Thalnos"),
        }),
        new SiteHostileArea("Honor's Tomb", "Scarlet Monastery", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Bloodmage Thalnos"),
        }),
        new SiteHostileArea("Huntsman's Cloister", "Scarlet Monastery", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Houndmaster Loksey"),
        }),
        new SiteHostileArea("Gallery of Treasures", "Scarlet Monastery", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {

        }),
        new SiteHostileArea("Athenaeum", "Scarlet Monastery", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Arcanist Doan"),
        }),
        new SiteHostileArea("Training Grounds", "Scarlet Monastery", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Arcanist Doan"),
        }),
        new SiteHostileArea("Crusader's Armory", "Scarlet Monastery", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Arcanist Doan"),
        }),
        new SiteHostileArea("Hall of Champions", "Scarlet Monastery", "HostileArea", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "40", "Herod"),
        }),
        new SiteHostileArea("Chapel Gardens", "Scarlet Monastery", "HostileArea", new()
        {
            ("39-40", "Scarlet Myrmidon"),
            ("39-40", "Scarlet Defender"),
            ("39-40", "Scarlet Wizard"),
        },
        new()
        {

        }),
        new SiteHostileArea("Crusader's Chapel", "Scarlet Monastery", "HostileArea", new()
        {
            ("39-40", "Scarlet Monk"),
            ("39-40", "Scarlet Champion"),
            ("39-40", "Scarlet Centurion"),
        },
        new()
        {
            (04, "42", "Scarlet Commander Mograine"),
            (04, "42", "High Inquisitor Whitemane"),
        }),

        #endregion
        
        #region Blackrock Depths

        new SiteHostileArea("Detention Block", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "52", "High Interrogator Gerstahn")
        }),
        new SiteHostileArea("Halls of the Law", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "53", "Houndmaster Grebmar"),
            (04, "53", "Lord Roccor")
        }),
        new SiteHostileArea("Ring of Law", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "54", "Anub'shiah"),
            (04, "54", "Eviscerator"),
            (04, "54", "Gorosh the Dervish"),
            (04, "54", "Grizzle"),
            (04, "54", "Hedrum the Creeper"),
            (04, "54", "Ok'thor the Breaker"),
        }),
        new SiteHostileArea("Shrine of Thaurissan", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Pyromancer Loregrain")
        }),
        new SiteHostileArea("The Black Vault", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Warder Stilgiss")
        }),
        new SiteHostileArea("Dark Iron Highway", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "55", "Bael'Gar")
        }),
        new SiteHostileArea("The Black Anvil", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "56", "Lord Incendius")
        }),
        new SiteHostileArea("Hall of Crafting", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "56", "Fineous Darkvire")
        }),
        new SiteHostileArea("West Garrison", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "General Angerforge")
        }),
        new SiteHostileArea("The Manufactory", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Golem Lord Argelmach")
        }),
        new SiteHostileArea("The Grim Guzzler", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Golem Lord Argelmach")
        }),
        new SiteHostileArea("Chamber of Enchantment", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Ambassador Flamelash")
        }),
        new SiteHostileArea("Mold Foundry", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Panzor the Invincible")
        }),
        new SiteHostileArea("Summoners' Tomb", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "58", "The Seven")
        }),
        new SiteHostileArea("The Lyceum", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("The Iron Hall", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "58", "Magmus")
        }),
        new SiteHostileArea("The Imperial Seat", "Blackrock Depths", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "59", "Emperor Dagran Thaurissan")
        }),

        #endregion
        
        #region Blackwing Lair

        new SiteHostileArea("Dragonmaw Garrison", "Blackwing Lair", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Razorgore the Untamed")
        }),
        new SiteHostileArea("Shadow Wing Lair", "Blackwing Lair", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Vaelastrasz the Corrupt"),
        }),
        new SiteHostileArea("Halls of Strife", "Blackwing Lair", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Broodlord Lashlayer"),
        }),
        new SiteHostileArea("Crimson Laboratories", "Blackwing Lair", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Firemaw"),
            (04, "60", "Ebonroc"),
            (04, "60", "Flamegor")
        }),
        new SiteHostileArea("Chromaggus' Lair", "Blackwing Lair", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Chromaggus")
        }),
        new SiteHostileArea("Nefarian's Lair", "Blackwing Lair", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Nefarian")
        }),

        #endregion

        #region Molten Core

        new SiteHostileArea("Magmadar Cavern", "Molten Core", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Lucifron"),
            (04, "60", "Magmadar")
        }),
        new SiteHostileArea("Western Lavafalls", "Molten Core", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Gehennas"),
            (04, "60", "Garr"),
            (04, "60", "Baron Geddon"),
            (04, "60", "Shazzrah"),
        }),
        new SiteHostileArea("Eastern Lavafalls", "Molten Core", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Sulfuron Harbringer"),
            (04, "60", "Golemagg The Incinerator"),
            (04, "60", "Majordomo Executus"),
        }),
        new SiteHostileArea("Ragnaros' Lair", "Molten Core", "HostileArea", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Ragnaros"),
        }),

        #endregion
        
        #region Feralas
        
        new SiteHostileArea("Dream Bough", "Feralas", "EmeraldBough", new() { },
        new()
        {
            (0, "60", "Lethon"),
        }),
        new SiteHostileArea("High Wilderness", "Feralas", "HostileArea", new()
        {
            ("43-44", "Longtooth Howler"),
            ("44-45", "Grizzled Ironfur Bear"),
            ("45-46", "Rogue Vale Screecher"),
        }),
        new SiteHostileArea("The Writhing Deep", "Feralas", "HostileArea", new()
        {
            ("44-45", "Zukk'ash Wasp"),
            ("44-45", "Zukk'ash Worker"),
            ("45-46", "Zukk'ash Stinger"),
            ("45-46", "Zukk'ash Tunneler"),
        }),
        new SiteHostileArea("Woodpaw Hills", "Feralas", "HostileArea", new()
        {
            ("40-41", "Woodpaw Mongrel"),
            ("41-42", "Woodpaw Trapper"),
            ("42-43", "Woodpaw Mystic"),
            ("42-43", "Woodpaw Alpha"),
            ("42-43", "Woodpaw Reaver"),
        }),
        new SiteHostileArea("Gordunni Outpost", "Feralas", "HostileArea", new()
        {
            ("40-41", "Gordunni Ogre"),
            ("41-42", "Gordunni Ogre Mage"),
            ("42-43", "Gordunni Brute"),
        }),
        new SiteHostileArea("Lower Wilds", "Feralas", "HostileArea", new()
        {
            ("40-41", "Longtooth Runner"),
            ("41-42", "Ironfur Bear"),
            ("43-44", "Grimtotem Shaman"),
        }),
        new SiteHostileArea("Grimtotem Compound", "Feralas", "HostileArea", new()
        {
            ("41-42", "Grimtotem Raider"),
            ("41-42", "Grimtotem Naturalist"),
            ("43-44", "Grimtotem Shaman"),
        }),
        new SiteHostileArea("Frayfeather Highlands", "Feralas", "HostileArea", new()
        {
            ("44-45", "Frayfeather Stagwing"),
            ("45-46", "Frayfeather Skystormer"),
            ("46-47", "Frayfeather Patriarch"),
            ("48", "Antillus the Soarer"),
        }),
        new SiteHostileArea("Ruins of Isildien", "Feralas", "HostileArea", new()
        {
            ("43-44", "Gordunni Mauler"),
            ("43-44", "Gordunni Warlock"),
            ("44-46", "Gordunni Battlemaster"),
            ("44-46", "Gordunni Shaman"),
            ("46-47", "Gordunni Warlord"),
        }),
        new SiteHostileArea("Ruins of Solarsal", "Feralas", "HostileArea", new()
        {
            ("41-43", "Hatecrest Wave Rider"),
            ("43-45", "Hatecrest Siren"),
            ("42-43", "Hatecrest Screamer"),
        }),
        new SiteHostileArea("Shalzaru's Lair", "Feralas", "HostileArea", new()
        {
            ("44-45", "Hatecrest Serpent Guard"),
            ("43-45", "Hatecrest Sorceress"),
            ("43-44", "Hatecrest Myrmidon"),
            ("47", "Lord Shalzaru"),
        }),
        new SiteHostileArea("The Twin Colossals", "Feralas", "HostileArea", new()
        {
            ("47-48", "Sea Elemental"),
            ("48-49", "Shore Strider"),
        }),
        new SiteHostileArea("Ruins of Ravenwind", "Feralas", "HostileArea", new()
        {
            ("48-49", "Northspring Roguefeather"),
            ("48-49", "Northspring Slayer"),
            ("49-50", "Northspring Windcaller"),
        }),
        new SiteHostileArea("Oneiros", "Feralas", "HostileArea", new()
        {
            ("60-61", "Jademir Oracle"),
            ("60-61", "Jademir Tree Watcher"),
            ("60-61", "Jademir Boughguard"),
        }),
        new SiteHostileArea("Rage Scar Hold", "Feralas", "HostileArea", new()
        {
            ("60-61", "Jademir Oracle"),
        }),
        new SiteHostileArea("The Forgotten Coast", "Feralas", "HostileArea", new()
        {
            ("47-48", "Sea Elemental"),
            ("48-49", "Shore Strider"),
        }),
        new SiteHostileArea("Lariss Pavillion", "Feralas", "HostileArea", new()
        {

        }),
        new SiteHostileArea("Wildwind Lake", "Feralas", "HostileArea", new()
        {

        }),
        new SiteHostileArea("Feral Scar Vale", "Feralas", "HostileArea", new()
        {

        }),
        new SiteHostileArea("Verdantis River", "Feralas", "HostileArea", new()
        {

        }),
        new SiteHostileArea("Isle of Dread", "Feralas", "HostileArea", new()
        {
            ("61-62", "Arcane Chimaerok"),
            ("60-61", "Chimaerok Devourer"),
            ("62", "Lord Lakmaeran"),
        }),

        #endregion

        #region Dire Maul
        
        new SiteHostileArea("Warpwood Quarter", "Dire Maul", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "57", "Lethtendris")
        }),
        new SiteHostileArea("The Conservatory", "Dire Maul", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "58", "Hydrospawn")
        }),
        new SiteHostileArea("The Shrine of Eldre'tharr", "Dire Maul", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "58", "Alzzin the Wildshaper")
        }),
        new SiteHostileArea("Capital Gardens", "Dire Maul", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "60", "Tendris Warpwood")
        }),
        new SiteHostileArea("Court of the Highborne", "Dire Maul", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "60", "Magister Kalendris"),
            (04, "60", "Illyanna Ravenoak")
        }),
        new SiteHostileArea("Prison of Immol'Thar", "Dire Maul", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "61", "Immol'thar")
        }),
        new SiteHostileArea("The Athenaeum", "Dire Maul", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "61", "Prince Tortheldrin")
        }),
        new SiteHostileArea("Gordok Commons", "Dire Maul", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "60", "Guard Mol'dar"),
            (04, "60", "Guard Slip'kik"),
            (04, "61", "Captain Kromcrush")
        }),
        new SiteHostileArea("Halls of Destruction", "Dire Maul", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "61", "Cho'Rush the Observer")
        }),
        new SiteHostileArea("Gordok's Seat", "Dire Maul", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "62", "King Gordok")
        }),

        #endregion

        #region Tirisfal Glades

        new SiteHostileArea("Deathknell", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        }),
        new SiteHostileArea("Venomweb Vale", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Balnir Farmstead", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Brightwater Lake", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Crusader's Outpost", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Cold Hearth Manor", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Agamand Mills", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Agamand Family Crypt", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Whispering Shore", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Solliden Farmstead", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Night Web's Hollow", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Nightmare Vale", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The North Coast", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Garren's Haunt", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Ruins of Lordaeron", "Tirisfal Glades", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Moonglade
        
        new SiteHostileArea("Shrine of Remulos", "Moonglade", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stormrage Barrow Dens", "Moonglade", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Azshara

        new SiteHostileArea("Rethress Sanctum", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Ruined Reaches", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lake Mennar", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hetaera's Clutch", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ruins of Eldarath", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shadowsong Shrine", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ursolan", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bitter Reaches", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Temple of Arkkoran", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Jagged Reef", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tower of Eldara", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bay of Storms", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Southridge Beach", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ravencrest Monument", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thalassian Base Camp", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Shattered Strand", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Legash Encampment", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Haldarr Encampment", "Azshara", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Forlorn Ridge", "Azshara", "HostileArea", new()
        {

        }),

        #endregion
        
        #region Dustwallow Marsh

        new SiteHostileArea("Witch Hill", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tidefury Cove", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Quagmire", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Den of Flame", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stonemaul Ruins", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shady Rest Inn", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sentry Point", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lost Point", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Northpoint Tower", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Emberstrife's Den", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dreadmurk Shore", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Darkmist Cavern", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bluefen", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bloodfen Burrow", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Beezil's Wreck", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Alcaz Island", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Alcaz Dungeon", "Dustwallow Marsh", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Onyxia's Lair
        
        new SiteHostileArea("Onyxia's Lair", "Onyxia's Lair", "HostileArea", new()
        {
            ("60", "Onyxian Lair Guard"),
            ("60", "Onyxian Warder")
        },
        new()
        {
            (00, "60", "Onyxia")
        }),

        #endregion
        
        #region Thousand Needles

        new SiteHostileArea("Mirage Raceway", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Weazel's Crater", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Windbreak Canyon", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Great Lift", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tahonda Ruins", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Rustmaul Dig Site", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Splithoof Crag", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Camp Ethok", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Darkcloud Pinnacle", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shimmering Flats", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Roguefeather Den", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Highperch", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tahonda Ruins", "Thousand Needles", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Tanaris

        new SiteHostileArea("Waterspring Field", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Southbreak Shore", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Steamwheedle Port", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Wavestrider Beach", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Broken Pillar", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Noxious Lair", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Abyssal Sands", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lost Rigger Cove", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Gaping Chasm", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dunemaul Compound", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Southmoon Ruins", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Eastmoon Ruins", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sandsorrow Watch", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thistleshrub Valley", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Noonshade Ruins", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Land's End Beach", "Tanaris", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Un'Goro Crater

        new SiteHostileArea("Lakkari Tar Pits", "Un'Goro Crater", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fire Plume Ridge", "Un'Goro Crater", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Marshlands", "Un'Goro Crater", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fungal Rock", "Un'Goro Crater", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Terror Run", "Un'Goro Crater", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Slithering Scar", "Un'Goro Crater", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Golakka Hot Springs", "Un'Goro Crater", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Darkshore

        new SiteHostileArea("The Long Wash", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Cliffspring River", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Twilight Vale", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Twilight Shore", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mist's Edge", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bashal'Aran", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ameth'Aran", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ruins of Mathystra", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tower of Althalaxx", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Master's Glaive", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Moonkin Caves", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Remtravel's Excavation", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Blackwood Den", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Grove of the Ancients", "Darkshore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Teldrassil

        new SiteHostileArea("Shadowglen", "Teldrassil", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Wellspring Lake", "Teldrassil", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Wellspring River", "Teldrassil", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shadowthread Cave", "Teldrassil", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Oracle Glade", "Teldrassil", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shadowglen", "Teldrassil", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ban'ethil Hollow", "Teldrassil", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fel Rock", "Teldrassil", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Gnarlpine Hold", "Teldrassil", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lake Al'Ameth", "Teldrassil", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Pools of Arlithrien", "Teldrassil", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Searing Gorge

        new SiteHostileArea("The Slag Pit", "Searing Gorge", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Cauldron", "Searing Gorge", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Firewatch Ridge", "Searing Gorge", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Grimesilt Digsite", "Searing Gorge", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dustfire Valley", "Searing Gorge", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stonewrought Pass", "Searing Gorge", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Blackchar Cave", "Searing Gorge", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Wetlands
        
        new SiteHostileArea("Dun Modr", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dun Algaz", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dragonmaw Gates", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Grim Batol", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sundown Marsh", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Whelgar's Excavation Site", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ironbeard's Tomb", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thelgen Rock", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mosshide Fen", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Green Belt", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Saltspray Glen", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sundown Marsh", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bluegill Marsh", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Black Channel Marsh", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Angerfang Encampment", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Raptor Ridge", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Direforge Hill", "Wetlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Alterac Mountains
        
        new SiteHostileArea("Ruins of Strahnbrad", "Alterac Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Headland", "Alterac Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Slaughter Hollow", "Alterac Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dalaran", "Alterac Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lordamere Internment Camp", "Alterac Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Crushridge Hold", "Alterac Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Growless Cave", "Alterac Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Hillsbrad Foothills
        
        new SiteHostileArea("Durnholde Keep", "Hillsbrad Foothills", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ravenholdt Manor", "Hillsbrad Foothills", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Darrow Hill", "Hillsbrad Foothills", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Eastern Strand", "Hillsbrad Foothills", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Western Strand", "Hillsbrad Foothills", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Nethander Stead", "Hillsbrad Foothills", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Southpoint Tower", "Hillsbrad Foothills", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Chillwind Point", "Hillsbrad Foothills", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Azurelode Mine", "Hillsbrad Foothills", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hillsbrad", "Hillsbrad Foothills", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Purgation Isle", "Hillsbrad Foothills", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Arathi Highlands
        
        new SiteHostileArea("Thandol Span", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Go'Shek Farm", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Circle of West Binding", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Circle of Inner Binding", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Circle of East Binding", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Circle of Outer Binding", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dabyrie's Farmstead", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Drywhisker Gorge", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Northfold Manor", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Boulder'gor", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Boulderfist Hall", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Witherbark Village", "Arathi Highlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Duskwood
        
        new SiteHostileArea("Twilight Grove", "Duskwood", "EmeraldBough", new() { },
        new()
        {
            (0, "60", "Emeriss"),
        }),
        new SiteHostileArea("Tranquil Gardens Cemetery", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Raven Hill Cemetery", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Raven Hill", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Addle's Stead", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Yorgen's Farmstead", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Brightwood Grove", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Vul'Gol Ogre Mound", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dawning Wood Catacombs", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Forlorn Rowe", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Manor Mistmantle", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Darkened Bank", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Rotting Orchard", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Roland's Doom", "Duskwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Hinterlands
        
        new SiteHostileArea("Seradane", "Hinterlands", "EmeraldBough", new() { },
        new()
        {
            (0, "60", "Ysondre"),
        }),
        new SiteHostileArea("Jintha'Alor", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Skulk Rock", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shadra'Alor", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Plaguemist Ravine", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Valorwind Lake", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Creeping Ruin", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Overlook Cliffs", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Agol'watha", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shaol'watha", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hiri'watha", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Altar of Zul", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bogen's Ledge", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Quel'Danil Lodge", "Hinterlands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Redridge Mountains
        
        new SiteHostileArea("Redridge Canyons", "Redridge Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Render's Valley", "Redridge Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Render's Camp", "Redridge Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stonewatch", "Redridge Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Alther's Mill", "Redridge Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Rethban Caverns", "Redridge Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lakeridge Highway", "Redridge Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lake Everstill", "Redridge Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tower of Ilgalar", "Redridge Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Galardell Valley", "Redridge Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Three Corners", "Redridge Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Felwood
        
        new SiteHostileArea("Ruins of Constellas", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Deadmaw Village", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Irontree Woods", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Irontree Cavern", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bloodvenom River", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bloodvenom Falls", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Jaedenar", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shrine of the Deceiver", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Deadwood Village", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Felpaw Village", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Morlos'Aran", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shatter Scar Vale", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Jadefire Run", "Felwood", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Shadow Hold
        
        new SiteHostileArea("Upper Tunnels", "Shadow Hold", "HostileArea", new()
        {
            ("52-53", "Jaedenar Enforcer"),
            ("52-53", "Jaedenar Warlock"),
            ("52-53", "Jaedenar Darkweaver"),
        },
        new()
        {
            (02, "54", "Ulathek")
        }),
        new SiteHostileArea("Council Camp", "Shadow Hold", "HostileArea", new()
        {
            ("53-54", "Jadefire Trickster"),
            ("53-54", "Jadefire Hellcaller"),
            ("53-54", "Vile Ooze"),
        },
        new()
        {
            (03, "55", "Prince Xavalis")
        }),
        new SiteHostileArea("Altar Room", "Shadow Hold", "HostileArea", new()
        {
            ("53-54", "Jaedenar Warlock"),
            ("54-55", "Jaedenar Hunter"),
            ("55-56", "Jaedenar Sayaad"),
        },
        new()
        {
            (03, "56", "Rakaiah")
        }),
        new SiteHostileArea("Inner Sanctum", "Shadow Hold", "HostileArea", new()
        {
            ("53-54", "Jaedenar Enforcer"),
            ("53-54", "Jaedenar Warlock"),
            ("54-55", "Jaedenar Darkweaver"),
            ("55", "Jaedenar Legionnaire"),
        },
        new()
        {
            (03, "56", "Shadow Lord Fel'Dan")
        }),
        new SiteHostileArea("Banehollow Shrine", "Shadow Hold", "HostileArea", new()
        {
            ("54-55", "Jaedenar Hunter"),
            ("55-56", "Jaedenar Legionnaire"),
        },
        new()
        {
            (02, "57", "Lord Banehollow")
        }),

        #endregion
        
        #region Winterspring

        new SiteHostileArea("Ruins of Kel'Theril", "Winterspring", "HostileArea", new()
        {
            ("54-56", "Suffering Highborne"),
            ("56-57", "Anguished Highborne"),
            ("56-57", "Watery Invader"),
        }),
        new SiteHostileArea("Hidden Grove", "Winterspring", "HostileArea", new()
        {
            ("56-58", "Crazed Owlbeast"),
            ("58-59", "Berserk Owlbeast"),
        }),
        new SiteHostileArea("Frostsaber Rock", "Winterspring", "HostileArea", new()
        {
            ("58-59", "Frostsaber Huntress"),
            ("58-60", "Frostsaber Stalker"),
            ("59", "Rak'shiri"),
        }),
        new SiteHostileArea("Northern Vale", "Winterspring", "HostileArea", new()
        {
            ("57-58", "Elder Shardtooth"),
            ("57-58", "Winterspring Schreecher"),
            ("58-59", "Chillwind Ravager"),
        }),
        new SiteHostileArea("Moon Horror Den", "Winterspring", "HostileArea", new()
        {
            ("54-55", "Winterspring Owl"),
            ("54-56", "Raging Owlbeast"),
        }),
        new SiteHostileArea("Yeti Caverns", "Winterspring", "HostileArea", new()
        {
            ("56-57", "Ice Thistle Yeti"),
            ("56-57", "Ice Thistle Matriarch"),
            ("57-58", "Ice Thistle Patriarch"),
        }),
        new SiteHostileArea("Darkwhisper Gorge", "Winterspring", "HostileArea", new()
        {
            ("59-60", "Hederine Initiate"),
            ("59-60", "Hederine Slayer"),
            ("59-60", "Hederine Manastalker"),
        }),
        new SiteHostileArea("Frostwhisper Gorge", "Winterspring", "HostileArea", new()
        {
            ("59-60", "Frostmaul Giant"),
            ("59-60", "Frostmaul Preserver"),
            ("60", "Kashoch the Reaver"),
        }),
        new SiteHostileArea("Mazthoril", "Winterspring", "HostileArea", new()
        {
            ("56-57", "Cobalt Scalebane"),
            ("56-57", "Cobalt Wyrmkin"),
            ("58", "Manaclaw"),
            ("60", "Scryer"),
        }),
        new SiteHostileArea("Ice Thistle Hills", "Winterspring", "HostileArea", new()
        {
            ("55-56", "Ice Thistle Yeti"),
            ("56-57", "Ice Thistle Matriarch"),
            ("56-57", "Chillwind Chimaera"),
        }),
        new SiteHostileArea("Frostfire Hot Springs", "Winterspring", "HostileArea", new()
        {
            ("53-54", "Shardtooth Bear"),
            ("54-55", "Ragged Owlbeast"),
            ("54-55", "Fledgling Chillwind"),
        }),
        new SiteHostileArea("Winterfall Village", "Winterspring", "HostileArea", new()
        {
            ("55-57", "Winterfall Den Watcher"),
            ("57-58", "Winterfall Shaman"),
            ("58", "Winterfall Ursa"),
            ("59", "High Chief Winterfall"),
        }),
        new SiteHostileArea("Timbermaw Post", "Winterspring", "HostileArea", new()
        {
            ("53-54", "Winterfall Pathfinder"),
            ("54-55", "Winterfall Totemic"),
            ("55-56", "Winterfall Den Watcher"),
        }),
        new SiteHostileArea("Owl Wing Thicket", "Winterspring", "HostileArea", new()
        {
            ("56-58", "Crazed Owlbeast"),
            ("58-59", "Berserk Owlbeast"),
            ("58-59", "Moontouched Owlbeast"),
        }),
        new SiteHostileArea("Dun Mandarr", "Winterspring", "HostileArea", new()
        {
            ("56-57", "Winterspring Screecher"),
            ("56-58", "Crazed Owlbeast"),
            ("58-59", "Berserk Owlbeast"),
            ("58-59", "Moontouched Owlbeast"),
        }),

        #endregion
        
        #region Deadwind Pass

        new SiteHostileArea("The Vice", "Deadwind Pass", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Deadman's Crossing", "Deadwind Pass", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Karazhan", "Deadwind Pass", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sleeping Gorge", "Deadwind Pass", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ariden's Camp", "Deadwind Pass", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Blasted Lands

        new SiteHostileArea("The Dark Portal", "Blasted Lands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Serpent's Coil", "Blasted Lands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dreadmaul Hold", "Blasted Lands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dreadmaul Post", "Blasted Lands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Tainted Scar", "Blasted Lands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Altar of Storms", "Blasted Lands", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Stonetalon Mountains

        new SiteHostileArea("Greatwood Vale", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Talondeep Path", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Talon Den", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Webwinder Path", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Charred Vale", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Windshear Mine", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Windshear Crag", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sishir Canyon", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mirkfallon Lake", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Camp Aparaje", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Grimtotem Post", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Cragpool Lake", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Boulderslide Ravine", "Stonetalon Mountains", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region The Barrens

        new SiteHostileArea("Lushwater Oasis", "The Barrens", "HostileArea", new()
        {
            ("12-13", "Kolkar Wrangler"),
            ("14-15", "Kolkar Stormer"),
            ("15-16", "Oasis Snapjaw"),
            ("20", "Gesharahan"),
        }),
        new SiteHostileArea("Raptor Grounds", "The Barrens", "HostileArea", new()
        {
            ("16-17", "Sunscale Scytheclaw"),
        }),
        new SiteHostileArea("The Stagnant Oasis", "The Barrens", "HostileArea", new()
        {
            ("14-15", "Kolkar Pack Runner"),
            ("15-16", "Oasis Snapjaw"),
            ("15-16", "Kolkar Bloodcharger"),
        }),
        new SiteHostileArea("Field of Giants", "The Barrens", "HostileArea", new()
        {
            ("21-22", "Silithid Swarmer"),
            ("24", "Silithid Harvester"),
        }),
        new SiteHostileArea("Thorn Hill", "The Barrens", "HostileArea", new()
        {
            ("11-12", "Razormane Hunter"),
            ("12-13", "Razormane Geomancer"),
            ("12-13", "Razormane Defender"),
            ("13-14", "Razormane Mystic"),
            ("15", "Kreenig Snarlsnout"),
        }),
        new SiteHostileArea("Agama'gor", "The Barrens", "HostileArea", new()
        {
            ("16-17", "Bristleback Water Seeker"),
            ("17-18", "Bristleback Hunter"),
            ("18-19", "Bristleback Geomancer"),
            ("22", "Swinegart Spearhide"),
        }),
        new SiteHostileArea("Southfury River", "The Barrens", "HostileArea", new()
        {
            ("11-12", "Dreadmaw Crocolisk"),
        }),
        new SiteHostileArea("Bramblescar", "The Barrens", "HostileArea", new()
        {
            ("16-17", "Bristleback Water Seeker"),
            ("17-18", "Bristleback Hunter"),
            ("18-19", "Bristleback Thornweaver"),
            ("19-20", "Bristleback Geomancer"),
        }),
        new SiteHostileArea("Bael Modan", "The Barrens", "HostileArea", new()
        {
            ("21-22", "Bael'dun Excavator"),
            ("21-22", "Bael'dun Foreman"),
            ("24", "Digger Flameforge"),
        }),
        new SiteHostileArea("The Dry Hills", "The Barrens", "HostileArea", new()
        {
            ("16-17", "Witchwing Slayer"),
            ("17-18", "Witchwing Windcaller"),
            ("20", "Serena Bloodfeather"),
        }),
        new SiteHostileArea("The Sludge Fen", "The Barrens", "HostileArea", new()
        {
            ("14-15", "Venture Co. Drudger"),
            ("15-16", "Venture Co. Mercenary"),
            ("18", "Supervisor Lugwizzle"),
            ("19", "Engineer Whirleygig"),
        }),
        new SiteHostileArea("The Merchant Coast", "The Barrens", "HostileArea", new()
        {
            ("12-13", "Southsea Brigand"),
            ("13-14", "Southsea Canonneer"),
        }),
        new SiteHostileArea("Northwatch Hold", "The Barrens", "HostileArea", new()
        {
            ("15-16", "Theramore Marine"),
            ("16-17", "Theramore Preserver"),
            ("19", "Cannoneer Smythe"),
            ("20", "Captain Fairmount"),
        }),
        new SiteHostileArea("The Forgotten Pools", "The Barrens", "HostileArea", new()
        {
            ("12-13", "Kolkar Wrangler"),
            ("14-15", "Kolkar Stormer"),
            ("16", "Barak Kodobane"),
        }),
        new SiteHostileArea("Boulder Lode Mine", "The Barrens", "HostileArea", new()
        {
            ("16-17", "Venture Co. Enforcer"),
            ("17-18", "Venture Co. Overseer"),
            ("19", "Boss Copperplug"),
        }),
        new SiteHostileArea("Gold Road", "The Barrens", "HostileArea", new()
        {
            ("12-13", "Fleeting Plainstrider"),
            ("12-13", "Savannah Huntress"),
            ("13-14", "Zhevra Runner"),
        }),
        new SiteHostileArea("Southern Gold Road", "The Barrens", "HostileArea", new()
        {
            ("19-20", "Zhevra Courser"),
            ("20-21", "Thunderhead"),
            ("21-22", "Thunderhawk Cloudscraper"),
        }),
        new SiteHostileArea("Dreadmist Peak", "The Barrens", "HostileArea", new()
        {
            ("10-11", "Burning Blade Bruiser"),
            ("11-12", "Burning Blade Acolyte"),
            ("15", "Rathorian"),
        }),
        new SiteHostileArea("Honor's Stand", "The Barrens", "HostileArea", new()
        {
            ("14-15", "Barrens Giraffe"),
            ("14-15", "Sunscale Screecher"),
            ("15-16", "Hacklefang Hyena"),
        }),
        new SiteHostileArea("Blackthorn Ridge", "The Barrens", "HostileArea", new()
        {
            ("21-22", "Razormane Pathfinder"),
            ("21-22", "Razormane Stalker"),
            ("23-24", "Razormane Seer"),
            ("23-25", "Razormane Warfrenzy"),
            ("26", "Hagg Taurenbane"),
        }),
        new SiteHostileArea("Baeldun Keep", "The Barrens", "HostileArea", new()
        {
            ("24-25", "Bael'dun Soldier"),
            ("24-25", "Bael'dun Rifleman"),
            ("26", "Bael'dun Officer"),
            ("30", "General Twinbraid"),
        }),
        new SiteHostileArea("Fray Island", "The Barrens", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Mulgore

        new SiteHostileArea("The Ravaged Caravan", "Mulgore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Palemane Rock", "Mulgore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Baeldun Digsite", "Mulgore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Brambleblade Ravine", "Mulgore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Red Cloud Mesa", "Mulgore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Golden Plains", "Mulgore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Red Rocks", "Mulgore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Windfury Ridge", "Mulgore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Venture Co. Mine", "Mulgore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Rolling Plains", "Mulgore", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Silverpine Forest
        
        new SiteHostileArea("Deep Elem Mine", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Graymane Wall", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fenris Keep", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Dawning Isles", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Dead Field", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Pyrewood Village", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ambermill", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Shining Strand", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Skittering Dark", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Olsen's Farthing", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ivar Patch", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Malden's Orchard", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Beren's Peril", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("North Tide's Run", "Silverpine Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Loch Modan

        new SiteHostileArea("Valley of Kings", "Loch Modan", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ironband's Excavation Site", "Loch Modan", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stonesplinter Valley", "Loch Modan", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mo'grosh Stronghold", "Loch Modan", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Silver Stream Mine", "Loch Modan", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Durotar

        new SiteHostileArea("Valley of Trials", "Durotar", "HostileArea", new()
        {
            ("01-02", "Mottled Boar"),
            ("02-03", "Scorpid Worker"),
        }),
        new SiteHostileArea("Skull Rock", "Durotar", "HostileArea", new()
        {
            ("09-10", "Burning Blade Fanatic"),
            ("10-11", "Burning Blade Apprentice"),
            ("14", "Gazz'uz"),
        }),
        new SiteHostileArea("Scuttle Coast", "Durotar", "HostileArea", new()
        {
            ("05-06", "Pygmy Surf Crawler"),
            ("06-07", "Makrura Shellhide"),
            ("06-07", "Makrura Clacker"),
        }),
        new SiteHostileArea("Tiragarde Keep", "Durotar", "HostileArea", new()
        {
            ("05-06", "Kul'Tiras Sailor"),
            ("06-07", "Kul'Tiras Marine"),
            ("08", "Lieutenant Benedict"),
        }),
        new SiteHostileArea("Thunder Ridge", "Durotar", "HostileArea", new()
        {
            ("09-10", "Thunder Lizard"),
            ("10-11", "Lightning Hide"),
            ("12", "Fizzle Darkstorm"),
        }),
        new SiteHostileArea("Bladefist Bay", "Durotar", "HostileArea", new()
        {
            ("08-09", "Elder Mottled Boar"),
            ("08-10", "Bloodtalon Scythemaw"),
            ("09-10", "Venomtail Scorpid"),
        }),
        new SiteHostileArea("Darkspear Strand", "Durotar", "HostileArea", new()
        {
            ("05-06", "Pygmy Surf Crawler"),
            ("05-06", "Clattering Scorpid"),
            ("06-07", "Makrura Clacker"),
        }),
        new SiteHostileArea("Echo Isles", "Durotar", "HostileArea", new()
        {
            ("08-09", "Bloodtalon Taillasher"),
            ("08-09", "Voodoo Troll"),
            ("08-09", "Hexed Troll"),
            ("10", "Zalazane"),
        }),
        new SiteHostileArea("Burning Blade Coven", "Durotar", "HostileArea", new()
        {
            ("03-04", "Vile Familiar"),
            ("04-05", "Burning Blade Novice"),
        }),
        new SiteHostileArea("Kolkar Crag", "Durotar", "HostileArea", new()
        {
            ("06-07", "Kolkar Drudge"),
            ("07-08", "Kolkar Outrunner"),
            ("09", "Warlord Kolkanis"),
        }),
        new SiteHostileArea("Razormane Grounds", "Durotar", "HostileArea", new()
        {
            ("07-08", "Razormane Scout"),
            ("08-09", "Razormane Dustrunner"),
            ("09-10", "Razormane Battleguard"),
            ("09", "Geolord Mottle"),
        }),
        new SiteHostileArea("Razorwind Canyon", "Durotar", "HostileArea", new()
        {
            ("07-08", "Dustwind Pillager"),
            ("07-08", "Dustwind Harpy"),
        }),
        new SiteHostileArea("Drygulch Ravine", "Durotar", "HostileArea", new()
        {
            ("09", "Dustwind Savage"),
            ("10-11", "Dustwind Storm Witch"),
        }),

        #endregion
        
        #region Ashenvale
        
        new SiteHostileArea("Bough Shadow", "Ashenvale", "EmeraldBough", new() { },
        new()
        {
            (0, "60", "Taerar"),
        }),
        new SiteHostileArea("Falfarren River", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Night Run", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Warsong Labor Camp", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Warsong Lumber Camp", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Raynewood Retreat", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Greenpaw Village", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Howling Vale", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Demon Fall Canyon", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Satyrnaar", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Felfire Hill", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Iris Lake", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fallen Sky Lake", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bloodtooth Camp", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ruins of Stardust", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fire Scar Shrine", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Xavian", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Zoram Strand", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lake Falathim", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Shrine of Aessina", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thistlefur Village", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Nightsong Woods", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ruins of Ordil'Aran", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mystral Lake", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Dor'Danil Barrow Den", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bathran's Haunt", "Ashenvale", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Burning Steppes

        new SiteHostileArea("The Pillar of Ash", "Burning Steppes", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Slither Rock", "Burning Steppes", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Terror Wing Path", "Burning Steppes", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dreadmaul Rock", "Burning Steppes", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Draco'dar", "Burning Steppes", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Blackrock Stronghold", "Burning Steppes", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ruins of Thaurissan", "Burning Steppes", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Blackrock Pass", "Burning Steppes", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Altar of Storms", "Burning Steppes", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Desolace

        new SiteHostileArea("Bolgan's Hole", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Gelkis Village", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ethel Rethor", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sargeron", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Valley of Bones", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Valley of Spears", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sar'theris Strand", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Magram Village", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thunder Axe Fortress", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Kodo Graveyard", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shadowbreak Ravine", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ranazjar Isle", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Kolkar Village", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mannoroc Coven", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tethris Aran", "Desolace", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Elwynn Froest

        new SiteHostileArea("Jasperlode Mine", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Forest's Edge", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Echo Ridge Mine", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mirror Lake", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mirror Lake Orchard", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Heroes Vigil", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Northshire Vineyards", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stonefield Farm", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fargodeep Mine", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Maclure Vineyards", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tower of Azora", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ridgepoint Tower", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Brackwell Pumpkin Patch", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Eastvale Logging Camp", "Elwynn Forest", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Uldaman

        new SiteHostileArea("Hall of the Keepers", "Uldaman", "HostileArea", new()
        {
            ("36-37", "Stonevault Ambusher"),
            ("37-38", "Stonevault Pillager"),
            ("37-38", "Stonevault Oracle")
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Map Chamber", "Uldaman", "HostileArea", new() { },
        new()
        {
            (0, "40", "Ironaya"),
        }),
        new SiteHostileArea("Temple Hall", "Uldaman", "HostileArea", new()
        {
            ("44", "Stone Steward"),
            ("41-43", "Venomlash Scorpid"),
            ("42-43", "Earthen Sculptor"),
        },
        new()
        {
            (3, "44", "Ancient Stone Keeper"),
        }),
        new SiteHostileArea("Dig Three", "Uldaman", "HostileArea", new()
        {
            ("42-44", "Shadowforge Darkcaster"),
            ("42-44", "Shadowforge Sharpshooter")
        },
        new()
        {
            (3, "45", "Galgann Firehammer"),
        }),
        new SiteHostileArea("The Stone Vault", "Uldaman", "HostileArea", new()
        {
            ("42-44", "Jadespine Basilisk"),
            ("41-43", "Stonevault Geomancer"),
            ("42-43", "Stonevault Brawler")
        },
        new()
        {
            (3, "45", "Grimlok"),
        }),
        new SiteHostileArea("Khaz'Goroth's Seat", "Uldaman", "HostileArea", new()
        {
            ("44-45", "Vault Warder"),
            ("45", "Stone Steward")
        },
        new()
        {
            (2, "47", "Archaedas"),
        }),

        #endregion
        
        #region Shadowfang Keep

        new SiteHostileArea("Jail", "Shadowfang Keep", "HostileArea", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("The Courtyard", "Shadowfang Keep", "HostileArea", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Kitchen", "Shadowfang Keep", "HostileArea", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Dining Hall", "Shadowfang Keep", "HostileArea", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Perimeter Wall", "Shadowfang Keep", "HostileArea", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("The Laboratory", "Shadowfang Keep", "HostileArea", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Tower Summit", "Shadowfang Keep", "HostileArea", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Arugal Chamber", "Shadowfang Keep", "HostileArea", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),

        #endregion

        #region Badlands
        
        new SiteHostileArea("Camp Kosh", "Badlands", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
            ("36-37", "Dustbelcher Mystic"),
        }),
        new SiteHostileArea("Camp Boff", "Badlands", "HostileArea", new()
        {
            ("38-39", "Dustbelcher Ogre"),
            ("39-40", "Dustbelcher Brute"),
        }),
        new SiteHostileArea("Valley of Fangs", "Badlands", "HostileArea", new()
        {
            ("35-36", "Crag Coyote"),
            ("38-39", "Ridge Huntress"),
        }),
        new SiteHostileArea("The Dustbowl", "Badlands", "HostileArea", new()
        {
            ("38-39", "Ridge Huntress"),
            ("39-40", "Elder Crag Coyote"),
            ("39-40", "Giant Buzzard"),
            ("40-41", "Ridge Stalker Patriarch"),
        }),
        new SiteHostileArea("Dustwing Gulch", "Badlands", "HostileArea", new()
        {
            ("37-38", "Ridge Stalker"),
            ("40-42", "Rabid Crag Coyote"),
        }),
        new SiteHostileArea("Apocryphan's Rest", "Badlands", "HostileArea", new()
        {
            ("39-41", "Giant Buzzard"),
            ("40-41", "Ridge Stalker Patriarch"),
            ("45", "Anathemus"),
        }),
        new SiteHostileArea("Mirage Flats", "Badlands", "HostileArea", new()
        {
            ("38-39", "Ridge Huntress"),
            ("39-40", "Elder Crag Coyote"),
            ("39-40", "Giant Buzzard"),
            ("40-41", "Ridge Stalker Patriarch"),
        }),
        new SiteHostileArea("Dustbelch Grotto", "Badlands", "HostileArea", new()
        {
            ("41-42", "Dustbelcher Wyrmhunter"),
            ("41-42", "Dustbelcher Shaman"),
            ("41-42", "Dustbelcher Mauler"),
            ("43-44", "Dustbelcher Ogre Mage"),
            ("44-45", "Dustbelcher Lord"),
        }),
        new SiteHostileArea("The Maker's Terrace", "Badlands", "HostileArea", new()
        {
            ("35-36", "Shadowforge Surveyor"),
            ("35-36", "Shadowforge Digger"),
            ("36-37", "Shadowforge Ruffian"),
            ("42", "Ambassador Infernus"),
        }),
        new SiteHostileArea("Hammertoe's Digsite", "Badlands", "HostileArea", new()
        {
            ("35-36", "Shadowforge Tunneler"),
            ("36-37", "Shadowforge Darkweaver"),
            ("39-40", "Stone Golem"),
            ("42", "Ambassador Infernus"),
        }),
        new SiteHostileArea("Angor Fortress", "Badlands", "HostileArea", new()
        {
            ("38-39", "Shadowforge Warrior"),
            ("38-39", "Shadowforge Chanter"),
            ("39-40", "Stone Golem"),
            ("42", "Ambassador Infernus"),
        }),
        new SiteHostileArea("Agmond's End", "Badlands", "HostileArea", new()
        {
            ("39-40", "Stonevault Bonesnapper"),
            ("40-41", "Stonevault Shaman"),
            ("42", "Murdaloc"),
        }),
        new SiteHostileArea("Lethlor Ravine", "Badlands", "HostileArea", new()
        {
            ("41-43", "Scalding Whelp"),
            ("43-46", "Scorched Guardian"),
        }),

        #endregion

        #region Dun Morogh

        new SiteHostileArea("Coldridge Valley", "Dun Morogh", "HostileArea", new()
        {
            ("01-02", "Rockjaw Trogg"),
            ("01-02", "Ragged Young Wolf"),
            ("01-03", "Small Crag Boar"),
            ("02-03", "Burly Rockjaw Trogg"),
        }),
        new SiteHostileArea("Coldridge Pass", "Dun Morogh", "HostileArea", new()
        {
            ("02-03", "Burly Rockjaw Trogg"),
            ("03-04", "Rockjaw Raider"),
        }),
        new SiteHostileArea("The Grizzled Den", "Dun Morogh", "HostileArea", new()
        {
            ("05-06", "Young Wendigo"),
            ("06-07", "Wendigo"),
        }),
        new SiteHostileArea("Chill Breeze Valley", "Dun Morogh", "HostileArea", new()
        {
            ("06-07", "Large Crag Boar"),
            ("06-07", "Wendigo"),
            ("11", "Old Icebeard"),
        }),
        new SiteHostileArea("Ice Flow Lake", "Dun Morogh", "HostileArea", new()
        {
            ("07-08", "Winter Wolf"),
            ("10", "Timber"),
        }),
        new SiteHostileArea("Frostmane Hold", "Dun Morogh", "HostileArea", new()
        {
            ("08-09", "Frostmane Headhunter"),
            ("08-09", "Frostmane Snowstrider"),
            ("08-10", "Frostmane Shadowcaster"),
            ("09-10", "Frostmane Hideskinner"),
        }),
        new SiteHostileArea("Shimmer Ridge", "Dun Morogh", "HostileArea", new()
        {
            ("08-09", "Frostmane Headhunter"),
            ("08-09", "Frostmane Snowstrider"),
            ("08-09", "Frostmane Seer"),
        }),
        new SiteHostileArea("The Tundrid Hills", "Dun Morogh", "HostileArea", new()
        {
            ("06-07", "Elder Crag Boar"),
            ("07-08", "Snow Leopard"),
            ("07-08", "Ice Claw Bear"),
        }),
        new SiteHostileArea("Gol'Bolar Quarry", "Dun Morogh", "HostileArea", new()
        {
            ("08-09", "Rockjaw Skullthumper"),
            ("08-10", "Rockjaw Bonesnapper"),
        }),
        new SiteHostileArea("Helm's Bed Lake", "Dun Morogh", "HostileArea", new()
        {
            ("09-10", "Scarred Crag Boar"),
            ("09-11", "Rockjaw Bonesnapper"),
            ("11-12", "Rockjaw Backbreaker"),
        }),
        new SiteHostileArea("Misty Pine Refuge", "Dun Morogh", "HostileArea", new()
        {

        }),

        #endregion
        
        #region Western Plaguelands

        new SiteHostileArea("Uther's Tomb", "Western Plaguelands", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Dalson's Tears", "Western Plaguelands", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Ruins of Andorhal", "Western Plaguelands", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Caer Darrow", "Western Plaguelands", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Felstone Field", "Western Plaguelands", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("The Writhing Haunt", "Western Plaguelands", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Northridge Lumber Camp", "Western Plaguelands", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Gahrron's Withering", "Western Plaguelands", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("The Weeping Cave", "Western Plaguelands", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Central Town", "Hearthglen", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Hearthglen Mine", "Hearthglen", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Mardenholde Keep", "Hearthglen", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
        }),

        #endregion

        #region Eastern Plaguelands

        new SiteHostileArea("Corin's Crossing", "Eastern Plaguelands", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
            ("53-54", "Dark Summoner"),
        }),
        new SiteHostileArea("Blackwood Lake", "Eastern Plaguelands", "HostileArea", new()
        {
            ("53-54", "Plaguehound"),
            ("53-54", "Noxious Plaguebat"),
        }),
        new SiteHostileArea("Lake Mereldar", "Eastern Plaguelands", "HostileArea", new()
        {
            ("53-54", "Blighted Surge"),
            ("53-54", "Plague Ravager"),
        }),
        new SiteHostileArea("Pestilent Scar", "Eastern Plaguelands", "HostileArea", new()
        {
            ("53-54", "Living Decay"),
            ("53-54", "Plaguehound"),
            ("53-54", "Noxious Plaguebat"),
            ("53-54", "Rotting Sludge"),
        }),
        new SiteHostileArea("Fungal Vale", "Eastern Plaguelands", "HostileArea", new()
        {

        }),
        new SiteHostileArea("Zul'Mashar", "Eastern Plaguelands", "HostileArea", new()
        {

        }),
        new SiteHostileArea("The Noxious Glade", "Eastern Plaguelands", "HostileArea", new()
        {

        }),
        new SiteHostileArea("Quel'Lithien Lodge", "Eastern Plaguelands", "HostileArea", new()
        {

        }),
        new SiteHostileArea("Plaguewood", "Eastern Plaguelands", "HostileArea", new()
        {
            ("53-54", "Scourge Warder"),
            ("53-54", "Putrid Gargoyle"),
            ("53-54", "Necromancer"),
            ("53-54", "Cursed Mage"),
            ("53-54", "Cannibal Ghoul"),
            ("53-54", "Death Cultist"),
        }),
        new SiteHostileArea("Terrordale", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Cursed Mage"),
            ("52-53", "Cannibal Ghoul"),
            ("52-53", "Scourge Soldier"),
            ("52-53", "Crypt Fiend"),
            ("52-53", "Torn Screamer"),
        }),
        new SiteHostileArea("Terrorweb Tunnel", "Eastern Plaguelands", "HostileArea", new()
        {
            ("55-56", "Crypt Fiend"),
            ("55-56", "Crypt Walker"),
        }),
        new SiteHostileArea("Darrowshire", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Plaguehound Runt"),
            ("52-53", "Scourge Soldier"),
        }),
        new SiteHostileArea("Thondroril River", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Plaguehound Runt"),
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("The Marris Stead", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Putrid Gargoyle"),
            ("52-53", "Plaguebat"),
            ("52-53", "Plaguehound Runt"),
        }),
        new SiteHostileArea("Crown Guard Tower", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Eastwall Tower", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Eastwall Gate", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Northdale", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("The Infectis Scar", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Quel'Lithien Lodge", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Plaguewood Tower", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Northpass Tower", "Eastern Plaguelands", "HostileArea", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Town Square", "Tyr's Hand", "HostileArea", new()
        {
            ("52-53", "Scarlet Curate"),
            ("52-53", "Scarlet Warder"),
        }),
        new SiteHostileArea("Tyr's Hand Abbey", "Tyr's Hand", "HostileArea", new()
        {
            ("52-53", "Scarlet Warder"),
            ("52-53", "Scarlet Cleric"),
        }),
        new SiteHostileArea("Tyr's Hand Keep", "Tyr's Hand", "HostileArea", new()
        {
            ("52-53", "Scarlet Curate"),
            ("52-53", "Scarlet Warder"),
        }),
        new SiteHostileArea("Scarlet Basilica", "Tyr's Hand", "HostileArea", new()
        {
            ("52-53", "Scarlet Curate"),
            ("52-53", "Scarlet Enchanter"),
            ("52-53", "Scarlet Cleric"),
        }),

        #endregion

        #region Silithus
        
        new SiteHostileArea("Pincer Sands", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bones of Grakkarond", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Swarming Pillar", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Scarab Wall", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hive'Zora", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hive'Ashi", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hive'Regal", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Southwind Village", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Crystal Vale", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Twilight Outpost", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Twilight Post", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Twilight Base Camp", "Silithus", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion

        #region Ruins of Ahn'Qiraj

        new SiteHostileArea("Scarab Terrace", "Ruins of Ahn'Qiraj", "HostileArea", new()
        {
            ("60", "Qiraji Gladiator"),
            ("60", "Qiraji Swarmguard"),
            ("60", "Hive'Zara Stinger"),
            ("60", "Hive'Zara Wasp"),
        },
        new()
        {
            (04, "60", "Kurinnaxx")
        }),
        new SiteHostileArea("General's Terrace", "Ruins of Ahn'Qiraj", "HostileArea", new()
        {
            ("60", "Qiraji Gladiator"),
            ("60", "Qiraji Warrior"),
            ("60", "Swarmguard Needler"),
        },
        new()
        {
            (04, "60", "General Rajaxx")
        }),
        new SiteHostileArea("The Reservoir", "Ruins of Ahn'Qiraj", "HostileArea", new()
        {
            ("60", "Flesh Hunter"),
            ("60", "Obsidian Destroyer"),
        },
        new()
        {
            (03, "60", "Moam")
        }),
        new SiteHostileArea("The Hatchery", "Ruins of Ahn'Qiraj", "HostileArea", new()
        {
            ("60", "Flesh Hunter"),
            ("60", "Hive'Zara Sandstalker"),
            ("60", "Hive'Zara Soldier"),
        },
        new()
        {
            (04, "60", "Buru The Gorger")
        }),
        new SiteHostileArea("The Comb", "Ruins of Ahn'Qiraj", "HostileArea", new()
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
        }),
        new SiteHostileArea("Watcher's Terrace", "Ruins of Ahn'Qiraj", "HostileArea", new()
        {
            ("60", "Anubisath Guardian")
        },
        new()
        {
            (02, "60", "Ossirian The Unscarred")
        }),

        #endregion
        
        #region Temple of Ahn'Qiraj

        new SiteHostileArea("The Temple Gates", "Temple of Ahn'Qiraj", "HostileArea", new()
        {
            ("60", "Anubisath Sentinel"),
            ("60", "Obsidian Eradicator")
        },
        new()
        {
            (02, "60", "The Prophet Skeram")
        }),
        new SiteHostileArea("The Hive Undergrounds", "Temple of Ahn'Qiraj", "HostileArea", new()
        {
            ("60", "Qiraji Champion"),
            ("60", "Qiraji Lasher"),
            ("60", "Qiraji Mindslayer"),
            ("60", "Qiraji Brainwasher"),
        },
        new()
        {
            (02, "60", "Battleguard Sartura"),
            (02, "60", "Fankriss the Unyielding"),
        }),
        new SiteHostileArea("Abandoned Tunnel", "Temple of Ahn'Qiraj", "HostileArea", new()
        {

        },
        new()
        {
            (00, "60", "Viscidus")
        }),
        new SiteHostileArea("Princess Chambers", "Temple of Ahn'Qiraj", "HostileArea", new()
        {
            ("60", "Vekniss Borer"),
            ("60", "Vekniss Guardian"),
            ("60", "Vekniss Hive Crawler"),
        },
        new()
        {
            (02, "60", "Princess Huhuran")
        }),
        new SiteHostileArea("Qiraji Imperial Seat", "Temple of Ahn'Qiraj", "HostileArea", new()
        {
            ("60", "Anubisath Sentinel"),
            ("60", "Obsidian Eradicator")
        },
        new()
        {
            (02, "60", "Twin Emperors")
        }),
        new SiteHostileArea("Ouro's Lair", "Temple of Ahn'Qiraj", "HostileArea", new()
        {
            ("60", "Vekniss Borer"),
            ("60", "Vekniss Guardian"),
            ("60", "Vekniss Hive Crawler"),
        },
        new()
        {
            (02, "60", "Ouro")
        }),
        new SiteHostileArea("Vault of C'Thun", "Temple of Ahn'Qiraj", "HostileArea", new()
        {
            ("60", "Anubisath Sentinel"),
            ("60", "Obsidian Eradicator")
        },
        new()
        {
            (02, "60", "C'Thun")
        }),

        #endregion
        
        #region Swamp of Sorrows
        
        new SiteHostileArea("Stagalbog", "Swamp of Sorrows", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sorrowmurk", "Swamp of Sorrows", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Pool of Tears", "Swamp of Sorrows", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Misty Valley", "Swamp of Sorrows", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Shifting Marsh", "Swamp of Sorrows", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Splinterspear Junction", "Swamp of Sorrows", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Misty Reed Strand", "Swamp of Sorrows", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Itharius's Cave", "Swamp of Sorrows", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Temple of Atal'Hakkar
        
        new SiteHostileArea("The Pit of Sacrafice", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Butchery", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Broken Hall", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hall of Serpents", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hall of the Cursed", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Pit of Refuse", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lair of the Chosen", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hall of Masks", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hall of Ritual", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hall of Bones", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Den of the Caller", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Chamber of Blood", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Chamber of the Dreamer", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sanctum of the Fallen God", "Temple of Atal'Hakkar", "HostileArea", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Westfall
        
        new SiteHostileArea("Westfall Lighthouse", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("The Dagger Hills", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("The Dead Acre", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Moonbrook", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Saldean's Farm", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Longshore", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Gold Coast Quarry", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("The Dust Plains", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Furlbrow's Pumpkin Farm", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Molsen Farm", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Jangolode Mine", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Jansen Stead", "Westfall", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),

        #endregion

        #region Stranglethorn Valley

        new SiteHostileArea("Lake Nazferiti", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Kal'ai Ruins", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Ruins of Jubuwal", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Ziata'jai Ruins", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Gurubashi Arena", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Crystal Shore", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Zuuldaia Ruins", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Ruins of Zul'Kunda", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Wild Shore", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Nek'mani Wellspring", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Bloodsail Compound", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Tkashi Ruins", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Venture Co. Base Camp", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("The Savage Coast", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Mizjah Ruins", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Balia'mah Ruins", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Mosh'Ogg Ogre Mound", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Jaguero Isle", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Ruins of Aboraz", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Mistvale Valley", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Ruins of Zul'Mamwe", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Kurzen's Compound", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Crystalvein Mine", "Stranglethorn Valley", "HostileArea", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),

        #endregion

        #region Zul'Gurub
        
        new SiteHostileArea("Altar of Hir'eek", "Zul'Gurub", "HostileArea", new()
        {
            ("60", "Bloodseeker Bat"),
            ("60", "Gurubashi Bat Rider"),
            ("60", "Gurubashi Headhunter"),
        },
        new()
        {
            (03, "60", "High Priestess Jeklik")
        }),
        new SiteHostileArea("The Coil", "Zul'Gurub", "HostileArea", new()
        {
            ("60", "Gurubashi Axe Thrower"),
            ("60", "Razzashi Cobra"),
            ("60", "Razzashi Serpent"),
        },
        new()
        {
            (03, "60", "High Priest Venoxis")
        }),
        new SiteHostileArea("Shadra'zaar", "Zul'Gurub", "HostileArea", new()
        {
            ("60", "Hakkari Shadowcaster"),
            ("60", "Razzashi Broodwidow"),
            ("60", "Razzashi Skitterer"),
            ("60", "Razzashi Venombrood"),
        },
        new()
        {
            (03, "60", "High Priestess Mar'li")
        }),
        new SiteHostileArea("Hakkari Grounds", "Zul'Gurub", "HostileArea", new()
        {
            ("60", "Gurubashi Champion"),
            ("60", "Hakkari Priest"),
            ("60", "Razzashi Raptor"),
            ("60", "Gurubashi Blood Drinker"),
        },
        new()
        {
            (03, "60", "Bloodlord Mandokir")
        }),
        new SiteHostileArea("Edge of Madness", "Zul'Gurub", "HostileArea", new()
        {
            ("60", "Gurubashi Berserker"),
            ("60", "Hakkari Blood Priest"),
            ("60", "Mad Servant"),
        },
        new()
        {
            (00, "60", "Gri'lek"),
            (00, "60", "Hazza'rah"),
            (00, "60", "Renataki"),
            (00, "60", "Wushoolay")
        }),
        new SiteHostileArea("Naze of Shirvallah", "Zul'Gurub", "HostileArea", new()
        {
            ("60", "Gurubashi Axe Thrower"),
            ("60", "Zulian Tiger"),
            ("60", "Gurubashi Berserker"),
        },
        new()
        {
            (03, "60", "High Priest Thekal")
        }),
        new SiteHostileArea("Pagle's Pointe", "Zul'Gurub", "HostileArea", new()
        {
            ("60", "Hooktooth Frenzy"),
        },
        new()
        {
            (02, "60", "Gahz'ranka")
        }),
        new SiteHostileArea("Temple of Bethekk", "Zul'Gurub", "HostileArea", new()
        {
            ("60", "Zulian Panther"),
            ("60", "Gurubashi Blood Drinker"),
            ("60", "Hakkari Shadow Hunter"),
        },
        new()
        {
            (00, "60", "High Priestess Arlokk")
        }),
        new SiteHostileArea("The Bloodfire Pit", "Zul'Gurub", "HostileArea", new()
        {
            ("60", "Voodoo Slave"),
            ("60", "Withered Mistress"),
            ("60", "Atal'ai Mistress"),
        },
        new()
        {
            (00, "60", "Jin'do the Hexxer")
        }),
        new SiteHostileArea("Altar of the Blood God", "Zul'Gurub", "HostileArea", new()
        {
            ("60", "Gurubashi Champion"),
            ("60", "Son of Hakkar"),
            ("60", "Hakkari Shadow Hunter"),
            ("60", "Hakkari Blood Priest"),
            ("60", "Gurubashi Berserker"),
        },
        new()
        {
            (05, "60", "Hakkar the Soulflayer")
        }),

        #endregion
    };
}
