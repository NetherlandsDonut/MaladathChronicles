using System.Collections.Generic;
using System.Linq;
using static Root;

public class SiteHostileArea
{
    public SiteHostileArea(string name, string zone, List<(string, string)> possibleEncounters)
    {
        this.name = name;
        this.zone = zone;
        this.possibleEncounters = new();
        foreach (var encounter in possibleEncounters)
        {
            var split = encounter.Item1.Split("-");
            this.possibleEncounters.Add((int.Parse(split[0]), int.Parse(split[split.Length == 1 ? 0 : 1]), encounter.Item2));
        }
        recommendedLevel = (int)this.possibleEncounters.Average(x => (x.Item1 + x.Item2) / 2.0);
    }

    public SiteHostileArea(string name, string zone, List<(string, string)> possibleEncounters, List<(int, string, string)> bossEncounters)
    {
        this.name = name;
        this.zone = zone;
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

    public string name, zone;
    public int recommendedLevel;
    public bool instancePart;
    public List<(int, int, string)> possibleEncounters;
    public List<(int, int, string)> bossEncounters;

    public static SiteHostileArea area;

    public static List<SiteHostileArea> hostileAreas = new()
    {
        #region Stratholme

        new SiteHostileArea("King's Square", "Stratholme", new()
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
        new SiteHostileArea("Market Row", "Stratholme", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "The Unforgiven")
        }),
        new SiteHostileArea("Crusaders' Square", "Stratholme", new()
        {
            ("56", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "58", "Timmy the Cruel")
        }),
        new SiteHostileArea("The Scarlet Bastion", "Stratholme", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Malor the Zealous")
        }),
        new SiteHostileArea("The Crimson Throne", "Stratholme", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "62", "Balnazzar")
        }),
        new SiteHostileArea("Elder's Square", "Stratholme", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Magistrate Barthilas")
        }),
        new SiteHostileArea("The Gauntlet", "Stratholme", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Nerub'enkan")
        }),
        new SiteHostileArea("Slaughter Square", "Stratholme", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "61", "Ramstein the Gorger")
        }),
        new SiteHostileArea("The Slaughter House", "Stratholme", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "62", "Baron Rivendare")
        }),

        #endregion

        #region Razorfen Downs

        new SiteHostileArea("The Caller's Chamber", "Razorfen Downs", new()
        {
            ("40", "Ragglesnout"),
        },
        new()
        {
            (2, "40", "Plaguemaw"),
            (2, "40", "Tuten'kash")
        }),
        new SiteHostileArea("The Bone Pile", "Razorfen Downs", new()
        {
            ("40", "Ragglesnout"),
        },
        new()
        {
            (2, "39", "Mordresh Fire Eye"),
        }),
        new SiteHostileArea("Spiral of Thorns", "Razorfen Downs", new()
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

        new SiteHostileArea("Razorfen Kraul1", "Razorfen Kraul", new()
        {
            ("40", "Battle Boar"),
            ("40", "Razorfen Beast Stalker"),
            ("40", "Razorfen Huntmaster")
        },
        new()
        {
            (2, "40", "Kraulshaper Tukaar")
        }),
        new SiteHostileArea("Razorfen Kraul2", "Razorfen Kraul", new()
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
        new SiteHostileArea("Razorfen Kraul3", "Razorfen Kraul", new()
        {
            ("40", "Razorfen Kraulshaper"),
            ("40", "Razorfen Scarbalde"),
            ("40", "Razorfen Huntmaster"),
        },
        new()
        {
            (2, "40", "Warlord Ramtusk")
        }),
        new SiteHostileArea("Razorfen Kraul4", "Razorfen Kraul", new()
        {
            ("40", "Kraulshaped Monstrosity"),
            ("40", "Razorfen Kraulshaper"),
            ("40", "Vile Bat")
        },
        new()
        {
            (2, "40", "Groyat, the Blind Hunter")
        }),
        new SiteHostileArea("Razorfen Kraul5", "Razorfen Kraul", new()
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

        new SiteHostileArea("Defias Hideout", "The Deadmines", new()
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
        new SiteHostileArea("Mast Room", "The Deadmines", new()
        {
            ("18-19", "Defias Taskmaster"),
            ("18-19", "Defias Wizard"),
            ("18-19", "Defias Strip Miner")
        },
        new()
        {
            (3, "19", "Sneed"),
        }),
        new SiteHostileArea("Goblin Foundry", "The Deadmines", new()
        {
            ("18-20", "Remote Controlled Golem"),
            ("18-19", "Goblin Engineer")
        },
        new()
        {
            (3, "20", "Gilnid"),
        }),
        new SiteHostileArea("Ironclad Cove", "The Deadmines", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "20", "Mr. Smite"),
        }),
        new SiteHostileArea("The Juggernaut", "The Deadmines", new()
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
        
        new SiteHostileArea("Reliquary", "Scholomance", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "59", "Blood Steward of Kirtonos")
        }),
        new SiteHostileArea("Chamber of Summoning", "Scholomance", new()
        {
            ("18-19", "Defias Taskmaster"),
            ("18-19", "Defias Wizard"),
            ("18-19", "Defias Strip Miner")
        },
        new()
        {
            (3, "59", "Kirtonos the Herald"),
        }),
        new SiteHostileArea("Great Ossuary", "Scholomance", new()
        {
            ("18-19", "Defias Taskmaster"),
            ("18-19", "Defias Wizard"),
            ("18-19", "Defias Strip Miner")
        },
        new()
        {
            (3, "61", "Rattlegore"),
        }),
        new SiteHostileArea("Hall of Secrets", "Scholomance", new()
        {
            ("18-20", "Remote Controlled Golem"),
            ("18-19", "Goblin Engineer")
        },
        new()
        {
            (3, "60", "Lorekeeper Polkelt"),
        }),
        new SiteHostileArea("Hall of the Damned", "Scholomance", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "60", "Doctor Theolen Krastinov"),
        }),
        new SiteHostileArea("Laboratory", "Scholomance", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "62", "Ras Frostwhisper")
        }),
        new SiteHostileArea("Vault of the Ravenian", "Scholomance", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "60", "The Ravenian")
        }),
        new SiteHostileArea("The Coven", "Scholomance", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "60", "Instructor Malicia")
        }),
        new SiteHostileArea("The Shadow Vault", "Scholomance", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "60", "Instructor Malicia")
        }),
        new SiteHostileArea("Viewing Room", "Scholomance", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "60", "Vectus"),
        }),
        new SiteHostileArea("Barov Family Vault", "Scholomance", new()
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
        new SiteHostileArea("Headmaster's Study", "Scholomance", new()
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

        new SiteHostileArea("The Noxious Hollow", "Maraudon", new()
        {
            ("45-46", "Creeping Sludge"),
            ("45-46", "Spewed Larva"),
            ("45-46", "Constrictor Vine")
        },
        new()
        {
            (3, "47", "Noxxion"),
        }),
        new SiteHostileArea("Foulspore Cavern", "Maraudon", new()
        {
            ("46-47", "Barbed Lasher"),
            ("46-48", "Celebrian Dryad"),
            ("46-47", "Deeprot Stomper")
        },
        new()
        {
            (3, "48", "Razorlash"),
        }),
        new SiteHostileArea("Wicked Grotto", "Maraudon", new()
        {
            ("47-48", "Deeprot Stomper"),
            ("46-47", "Deeprot Tangler"),
            ("46-47", "Poison Sprite")
        },
        new()
        {
            (3, "48", "Tinkerer Gizlock"),
        }),
        new SiteHostileArea("Vyletongue Seat", "Maraudon", new()
        {
            ("46-47", "Putridus Satyr"),
            ("47-48", "Putridus Shadowstalker"),
            ("46-47", "Putridus Trickster")
        },
        new()
        {
            (3, "48", "Lord Vyletongue"),
        }),
        new SiteHostileArea("Poison Falls", "Maraudon", new()
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
        new SiteHostileArea("Earth Song Falls", "Maraudon", new()
        {
            ("50", "Rotgrip"),
            ("48-49", "Primordial Behemoth"),
            ("48-49", "Theradrim Guardian")
        },
        new()
        {
            (3, "50", "Landslide"),
        }),
        new SiteHostileArea("Zaetar's Grave", "Maraudon", new()
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

        new SiteHostileArea("Trogg Caves", "Gnomeregan", new()
        {
            ("27-28", "Caverndeep Burrower"),
            ("28-29", "Irradiated Pillager"),
            ("28-29", "Caverndeep Ambusher")
        },
        new()
        {
            (3, "30", "Grubbis"),
        }),
        new SiteHostileArea("Hall of Gears", "Gnomeregan", new()
        {
            ("29-30", "Corrosive Lurker"),
            ("29-30", "Irradiated Slime"),
            ("29-31", "Irradiated Horror")
        },
        new()
        {
            (3, "31", "Viscous Fallout"),
        }),
        new SiteHostileArea("Launch Bay", "Gnomeregan", new()
        {
            ("30-31", "Leprous Technician"),
            ("31-32", "Mobile Alert System"),
            ("31-32", "Mechanized Sentry")
        },
        new()
        {
            (3, "32", "Electrocutioner 6000"),
        }),
        new SiteHostileArea("Engineering Labs", "Gnomeregan", new()
        {
            ("32-33", "Mechano Tank"),
            ("31-33", "Mobile Alert System")
        },
        new()
        {
            (3, "33", "Crowd Pummeler 9-60"),
        }),
        new SiteHostileArea("Tinkers' Court", "Gnomeregan", new()
        {

        },
        new()
        {
            (0, "34", "Mekgineer Thermaplugg"),
        }),

        #endregion

        #region Wailing Caverns

        new SiteHostileArea("Screaming Gully", "Wailing Caverns", new()
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
        new SiteHostileArea("Pit of Fangs", "Wailing Caverns", new()
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
        new SiteHostileArea("Winding Chasm", "Wailing Caverns", new()
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
        new SiteHostileArea("Crag of the Everliving", "Wailing Caverns", new()
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
        new SiteHostileArea("Dreamer's Rock", "Wailing Caverns", new()
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
        
        new SiteHostileArea("Pool of Ask'ar", "Blackfathom Deeps", new()
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
        new SiteHostileArea("Shrine of Gelihast", "Blackfathom Deeps", new()
        {
            ("25-26", "Blindlight Oracle"),
            ("25-26", "Blindlight Muckdweller")
        },
        new()
        {
            (2, "26", "Gelihast"),
        }),
        new SiteHostileArea("Moonshrine Ruins", "Blackfathom Deeps", new()
        {
            ("26", "Aqua Guardian"),
            ("25-26", "Twilight Acolyte"),
            ("25-27", "Twilight Aquamancer")
        },
        new()
        {
            (3, "27", "Baron Aquanis"),
        }),
        new SiteHostileArea("Forgotten Pool", "Blackfathom Deeps", new()
        {
            ("26-27", "Deep Pool Threshkin"),
            ("26-27", "Skittering Crustacean")
        },
        new()
        {
            (2, "27", "Old Serra'kis"),
        }),
        new SiteHostileArea("Moonshrine Sanctum", "Blackfathom Deeps", new()
        {
            ("27-28", "Twilight Elementalist"),
            ("27-28", "Twilight Shadowmage")
        },
        new()
        {
            (2, "28", "Twilight Lord Kelris"),
        }),
        new SiteHostileArea("Aku'mai's Lair", "Blackfathom Deeps",
        new() { },
        new()
        {
            (0, "28", "Aku'mai"),
        }),
        
        #endregion

        #region Scarlet Monastery

        new SiteHostileArea("Chamber of Atonement", "Scarlet Monastery", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Interrogator Vishas")
        }),
        new SiteHostileArea("Forlorn Cloister", "Scarlet Monastery", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Bloodmage Thalnos"),
        }),
        new SiteHostileArea("Honor's Tomb", "Scarlet Monastery", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Bloodmage Thalnos"),
        }),
        new SiteHostileArea("Huntsman's Cloister", "Scarlet Monastery", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Houndmaster Loksey"),
        }),
        new SiteHostileArea("Athenaeum", "Scarlet Monastery", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Arcanist Doan"),
        }),
        new SiteHostileArea("Training Grounds", "Scarlet Monastery", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Arcanist Doan"),
        }),
        new SiteHostileArea("Crusader's Armory", "Scarlet Monastery", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Arcanist Doan"),
        }),
        new SiteHostileArea("Hall of Champions", "Scarlet Monastery", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Arcanist Doan"),
        }),
        new SiteHostileArea("Chapel Gardens", "Scarlet Monastery", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Arcanist Doan"),
        }),
        new SiteHostileArea("Cathedral", "Scarlet Monastery", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Arcanist Doan"),
        }),

        #endregion
        
        #region Blackrock Depths

        new SiteHostileArea("Detention Block", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "52", "High Interrogator Gerstahn")
        }),
        new SiteHostileArea("Halls of the Law", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "53", "Houndmaster Grebmar"),
            (04, "53", "Lord Roccor")
        }),
        new SiteHostileArea("Ring of Law", "Blackrock Depths", new()
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
        new SiteHostileArea("Shrine of Thaurissan", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Pyromancer Loregrain")
        }),
        new SiteHostileArea("The Black Vault", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Warder Stilgiss")
        }),
        new SiteHostileArea("Dark Iron Highway", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "55", "Bael'Gar")
        }),
        new SiteHostileArea("The Black Anvil", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "56", "Lord Incendius")
        }),
        new SiteHostileArea("Hall of Crafting", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "56", "Fineous Darkvire")
        }),
        new SiteHostileArea("West Garrison", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "General Angerforge")
        }),
        new SiteHostileArea("The Manufactory", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Golem Lord Argelmach")
        }),
        new SiteHostileArea("The Grim Guzzler", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Golem Lord Argelmach")
        }),
        new SiteHostileArea("Chamber of Enchantment", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Ambassador Flamelash")
        }),
        new SiteHostileArea("Mold Foundry", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Panzor the Invincible")
        }),
        new SiteHostileArea("Summoners' Tomb", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "58", "The Seven")
        }),
        new SiteHostileArea("The Lyceum", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("The Iron Hall", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "58", "Magmus")
        }),
        new SiteHostileArea("The Imperial Seat", "Blackrock Depths", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "59", "Emperor Dagran Thaurissan")
        }),

        #endregion
        
        #region Blackwing Lair

        new SiteHostileArea("Dragonmaw Garrison", "Blackwing Lair", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Razorgore the Untamed")
        }),
        new SiteHostileArea("Shadow Wing Lair", "Blackwing Lair", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Vaelastrasz the Corrupt"),
        }),
        new SiteHostileArea("Halls of Strife", "Blackwing Lair", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Broodlord Lashlayer"),
        }),
        new SiteHostileArea("Crimson Laboratories", "Blackwing Lair", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Firemaw"),
            (04, "60", "Ebonroc"),
            (04, "60", "Flamegor")
        }),
        new SiteHostileArea("Chromaggus' Lair", "Blackwing Lair", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Chromaggus")
        }),
        new SiteHostileArea("Nefarian's Lair", "Blackwing Lair", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Nefarian")
        }),

        #endregion

        #region Molten Core

        new SiteHostileArea("Magmadar Cavern", "Molten Core", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Lucifron"),
            (04, "60", "Magmadar")
        }),
        new SiteHostileArea("The Lavafalls", "Molten Core", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Gehennas"),
            (04, "60", "Garr"),
            (04, "60", "Baron Geddon"),
            (04, "60", "Shazzrah"),
            (04, "60", "Sulfuron Harbringer"),
            (04, "60", "Golemagg The Incinerator"),
            (04, "60", "Majordomo Executus"),
        }),
        new SiteHostileArea("Ragnaros' Lair", "Molten Core", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Ragnaros"),
        }),

        #endregion
        
        #region Feralas
        
        new SiteHostileArea("High Wilderness", "Feralas", new()
        {
            ("43-44", "Longtooth Howler"),
            ("44-45", "Grizzled Ironfur Bear"),
            ("45-46", "Rogue Vale Screecher"),
        }),
        new SiteHostileArea("The Writhing Deep", "Feralas", new()
        {
            ("44-45", "Zukk'ash Wasp"),
            ("44-45", "Zukk'ash Worker"),
            ("45-46", "Zukk'ash Stinger"),
            ("45-46", "Zukk'ash Tunneler"),
        }),
        new SiteHostileArea("Woodpaw Hills", "Feralas", new()
        {
            ("40-41", "Woodpaw Mongrel"),
            ("41-42", "Woodpaw Trapper"),
            ("42-43", "Woodpaw Mystic"),
            ("42-43", "Woodpaw Alpha"),
            ("42-43", "Woodpaw Reaver"),
        }),
        new SiteHostileArea("Gordunni Outpost", "Feralas", new()
        {
            ("40-41", "Gordunni Ogre"),
            ("41-42", "Gordunni Ogre Mage"),
            ("42-43", "Gordunni Brute"),
        }),
        new SiteHostileArea("Lower Wilds", "Feralas", new()
        {
            ("40-41", "Longtooth Runner"),
            ("41-42", "Ironfur Bear"),
            ("43-44", "Grimtotem Shaman"),
        }),
        new SiteHostileArea("Grimtotem Compound", "Feralas", new()
        {
            ("41-42", "Grimtotem Raider"),
            ("41-42", "Grimtotem Naturalist"),
            ("43-44", "Grimtotem Shaman"),
        }),
        new SiteHostileArea("Frayfeather Highlands", "Feralas", new()
        {
            ("44-45", "Frayfeather Stagwing"),
            ("45-46", "Frayfeather Skystormer"),
            ("46-47", "Frayfeather Patriarch"),
            ("48", "Antillus the Soarer"),
        }),
        new SiteHostileArea("Ruins of Isildien", "Feralas", new()
        {
            ("43-44", "Gordunni Mauler"),
            ("43-44", "Gordunni Warlock"),
            ("44-46", "Gordunni Battlemaster"),
            ("44-46", "Gordunni Shaman"),
            ("46-47", "Gordunni Warlord"),
        }),
        new SiteHostileArea("Ruins of Solarsal", "Feralas", new()
        {
            ("41-43", "Hatecrest Wave Rider"),
            ("43-45", "Hatecrest Siren"),
            ("42-43", "Hatecrest Screamer"),
        }),
        new SiteHostileArea("Shalzaru's Lair", "Feralas", new()
        {
            ("44-45", "Hatecrest Serpent Guard"),
            ("43-45", "Hatecrest Sorceress"),
            ("43-44", "Hatecrest Myrmidon"),
            ("47", "Lord Shalzaru"),
        }),
        new SiteHostileArea("The Twin Colossals", "Feralas", new()
        {
            ("47-48", "Sea Elemental"),
            ("48-49", "Shore Strider"),
        }),
        new SiteHostileArea("Ruins of Ravenwind", "Feralas", new()
        {
            ("48-49", "Northspring Roguefeather"),
            ("48-49", "Northspring Slayer"),
            ("49-50", "Northspring Windcaller"),
        }),
        new SiteHostileArea("Oneiros", "Feralas", new()
        {
            ("60-61", "Jademir Oracle"),
            ("60-61", "Jademir Tree Watcher"),
            ("60-61", "Jademir Boughguard"),
        }),
        new SiteHostileArea("The Forgotten Coast", "Feralas", new()
        {
            ("47-48", "Sea Elemental"),
            ("48-49", "Shore Strider"),
        }),
        new SiteHostileArea("Isle of Dread", "Feralas", new()
        {
            ("61-62", "Arcane Chimaerok"),
            ("60-61", "Chimaerok Devourer"),
            ("62", "Lord Lakmaeran"),
        }),

        #endregion

        #region Dire Maul
        
        new SiteHostileArea("Warpwood Quarter", "Dire Maul", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "57", "Lethtendris")
        }),
        new SiteHostileArea("The Conservatory", "Dire Maul", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "58", "Hydrospawn")
        }),
        new SiteHostileArea("The Shrine of Eldre'tharr", "Dire Maul", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "58", "Alzzin the Wildshaper")
        }),
        new SiteHostileArea("Capital Gardens", "Dire Maul", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "60", "Tendris Warpwood")
        }),
        new SiteHostileArea("Court of the Highborne", "Dire Maul", new()
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
        new SiteHostileArea("Prison of Immol'Thar", "Dire Maul", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "61", "Immol'thar")
        }),
        new SiteHostileArea("The Athenaeum", "Dire Maul", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "61", "Prince Tortheldrin")
        }),
        new SiteHostileArea("Gordok Commons", "Dire Maul", new()
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
        new SiteHostileArea("Halls of Destruction", "Dire Maul", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        },
        new()
        {
            (04, "61", "Cho'Rush the Observer")
        }),
        new SiteHostileArea("Gordok's Seat", "Dire Maul", new()
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

        new SiteHostileArea("Deathknell", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        }),

        #endregion
        
        #region Desolace

        new SiteHostileArea("Bolgan's Hole", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Gelkis Village", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Silithus

        new SiteHostileArea("The Swarming Pillar", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Scarab Wall", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hive'Zara", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hive'Ashi", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hive'Regal", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Southwind Village", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Crystal Vale", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion

        #region Uldaman

        new SiteHostileArea("Hall of the Keepers", "Uldaman", new()
        {
            ("36-37", "Stonevault Ambusher"),
            ("37-38", "Stonevault Pillager"),
            ("37-38", "Stonevault Oracle")
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Map Chamber", "Uldaman", new() { },
        new()
        {
            (0, "40", "Ironaya"),
        }),
        new SiteHostileArea("Temple Hall", "Uldaman", new()
        {
            ("44", "Stone Steward"),
            ("41-43", "Venomlash Scorpid"),
            ("42-43", "Earthen Sculptor"),
        },
        new()
        {
            (3, "44", "Ancient Stone Keeper"),
        }),
        new SiteHostileArea("Dig Three", "Uldaman", new()
        {
            ("42-44", "Shadowforge Darkcaster"),
            ("42-44", "Shadowforge Sharpshooter")
        },
        new()
        {
            (3, "45", "Galgann Firehammer"),
        }),
        new SiteHostileArea("The Stone Vault", "Uldaman", new()
        {
            ("42-44", "Jadespine Basilisk"),
            ("41-43", "Stonevault Geomancer"),
            ("42-43", "Stonevault Brawler")
        },
        new()
        {
            (3, "45", "Grimlok"),
        }),
        new SiteHostileArea("Khaz'Goroth's Seat", "Uldaman", new()
        {
            ("44-45", "Vault Warder"),
            ("45", "Stone Steward")
        },
        new()
        {
            (2, "47", "Archaedas"),
        }),

        #endregion

        #region Badlands
        
        new SiteHostileArea("Camp Kosh", "Badlands", new()
        {
            ("35-37", "Dustbelcher Warrior"),
            ("36-37", "Dustbelcher Mystic"),
        }),
        new SiteHostileArea("Camp Boff", "Badlands", new()
        {
            ("38-39", "Dustbelcher Ogre"),
            ("39-40", "Dustbelcher Brute"),
        }),
        new SiteHostileArea("Valley of Fangs", "Badlands", new()
        {
            ("35-36", "Crag Coyote"),
            ("38-39", "Ridge Huntress"),
        }),
        new SiteHostileArea("The Dustbowl", "Badlands", new()
        {
            ("38-39", "Ridge Huntress"),
            ("39-40", "Elder Crag Coyote"),
            ("39-40", "Giant Buzzard"),
            ("40-41", "Ridge Stalker Patriarch"),
        }),
        new SiteHostileArea("Dustwing Gulch", "Badlands", new()
        {
            ("37-38", "Ridge Stalker"),
            ("40-42", "Rabid Crag Coyote"),
        }),
        new SiteHostileArea("Apocryphan's Rest", "Badlands", new()
        {
            ("39-41", "Giant Buzzard"),
            ("40-41", "Ridge Stalker Patriarch"),
            ("45", "Anathemus"),
        }),
        new SiteHostileArea("Mirage Flats", "Badlands", new()
        {
            ("38-39", "Ridge Huntress"),
            ("39-40", "Elder Crag Coyote"),
            ("39-40", "Giant Buzzard"),
            ("40-41", "Ridge Stalker Patriarch"),
        }),
        new SiteHostileArea("Dustbelch Grotto", "Badlands", new()
        {
            ("41-42", "Dustbelcher Wyrmhunter"),
            ("41-42", "Dustbelcher Shaman"),
            ("41-42", "Dustbelcher Mauler"),
            ("43-44", "Dustbelcher Ogre Mage"),
            ("44-45", "Dustbelcher Lord"),
        }),
        new SiteHostileArea("The Maker's Terrace", "Badlands", new()
        {
            ("35-36", "Shadowforge Surveyor"),
            ("35-36", "Shadowforge Digger"),
            ("36-37", "Shadowforge Ruffian"),
            ("42", "Ambassador Infernus"),
        }),
        new SiteHostileArea("Hammertoe's Digsite", "Badlands", new()
        {
            ("35-36", "Shadowforge Tunneler"),
            ("36-37", "Shadowforge Darkweaver"),
            ("39-40", "Stone Golem"),
            ("42", "Ambassador Infernus"),
        }),
        new SiteHostileArea("Angor Fortress", "Badlands", new()
        {
            ("38-39", "Shadowforge Warrior"),
            ("38-39", "Shadowforge Chanter"),
            ("39-40", "Stone Golem"),
            ("42", "Ambassador Infernus"),
        }),
        new SiteHostileArea("Agmond's End", "Badlands", new()
        {
            ("39-40", "Stonevault Bonesnapper"),
            ("40-41", "Stonevault Shaman"),
            ("42", "Murdaloc"),
        }),
        new SiteHostileArea("Lethlor Ravine", "Badlands", new()
        {
            ("41-43", "Scalding Whelp"),
            ("43-46", "Scorched Guardian"),
        }),

        #endregion

        #region Dun Morogh

        new SiteHostileArea("Coldrigde Valley", "Dun Morogh", new()
        {
            ("01-02", "Rockjaw Trogg"),
            ("01-02", "Ragged Young Wolf"),
            ("01-03", "Small Crag Boar"),
            ("02-03", "Burly Rockjaw Trogg"),
        }),
        new SiteHostileArea("Coldrigde Pass", "Dun Morogh", new()
        {
            ("02-03", "Burly Rockjaw Trogg"),
            ("03-04", "Rockjaw Raider"),
        }),
        new SiteHostileArea("The Grizzled Den", "Dun Morogh", new()
        {
            ("05-06", "Young Wendigo"),
            ("06-07", "Wendigo"),
        }),
        new SiteHostileArea("Chill Breeze Valley", "Dun Morogh", new()
        {
            ("06-07", "Large Crag Boar"),
            ("06-07", "Wendigo"),
            ("11", "Old Icebeard"),
        }),
        new SiteHostileArea("Ice Flow Lake", "Dun Morogh", new()
        {
            ("07-08", "Winter Wolf"),
            ("10", "Timber"),
        }),
        new SiteHostileArea("Frostmane Hold", "Dun Morogh", new()
        {
            ("08-09", "Frostmane Headhunter"),
            ("08-09", "Frostmane Snowstrider"),
            ("08-10", "Frostmane Shadowcaster"),
            ("09-10", "Frostmane Hideskinner"),
        }),
        new SiteHostileArea("Shimmer Ridge", "Dun Morogh", new()
        {
            ("08-09", "Frostmane Headhunter"),
            ("08-09", "Frostmane Snowstrider"),
            ("08-09", "Frostmane Seer"),
        }),
        new SiteHostileArea("The Tundrid Hills", "Dun Morogh", new()
        {
            ("06-07", "Elder Crag Boar"),
            ("07-08", "Snow Leopard"),
            ("07-08", "Ice Claw Bear"),
        }),
        new SiteHostileArea("Gol'Bolar Quarry", "Dun Morogh", new()
        {
            ("08-09", "Rockjaw Skullthumper"),
            ("08-10", "Rockjaw Bonesnapper"),
        }),
        new SiteHostileArea("Helm's Bed Lake", "Dun Morogh", new()
        {
            ("09-10", "Scarred Crag Boar"),
            ("09-11", "Rockjaw Bonesnapper"),
            ("11-12", "Rockjaw Backbreaker"),
        }),

        #endregion

        #region Eastern Plaguelands

        new SiteHostileArea("Corin's Crossing", "Eastern Plaguelands", new()
        {
            ("53-54", "Scourge Warder"),
            ("53-54", "Dark Summoner"),
        }),
        new SiteHostileArea("Blackwood Lake", "Eastern Plaguelands", new()
        {
            ("53-54", "Plaguehound"),
            ("53-54", "Noxious Plaguebat"),
        }),
        new SiteHostileArea("Lake Mereldar", "Eastern Plaguelands", new()
        {
            ("53-54", "Blighted Surge"),
            ("53-54", "Plague Ravager"),
        }),
        new SiteHostileArea("Pestilent Scar", "Eastern Plaguelands",  new()
        {
            ("53-54", "Living Decay"),
            ("53-54", "Plaguehound"),
            ("53-54", "Noxious Plaguebat"),
            ("53-54", "Rotting Sludge"),
        }),
        new SiteHostileArea("Plaguewood", "Eastern Plaguelands", new()
        {
            ("53-54", "Scourge Warder"),
            ("53-54", "Putrid Gargoyle"),
            ("53-54", "Necromancer"),
            ("53-54", "Cursed Mage"),
            ("53-54", "Cannibal Ghoul"),
            ("53-54", "Death Cultist"),
        }),
        new SiteHostileArea("Terrordale", "Eastern Plaguelands", new()
        {
            ("52-53", "Cursed Mage"),
            ("52-53", "Cannibal Ghoul"),
            ("52-53", "Scourge Soldier"),
            ("52-53", "Crypt Fiend"),
            ("52-53", "Torn Screamer"),
        }),
        new SiteHostileArea("Terrorweb Tunnel", "Eastern Plaguelands", new()
        {
            ("55-56", "Crypt Fiend"),
            ("55-56", "Crypt Walker"),
        }),
        new SiteHostileArea("Darrowshire", "Eastern Plaguelands", new()
        {
            ("52-53", "Plaguehound Runt"),
            ("52-53", "Scourge Soldier"),
        }),
        new SiteHostileArea("Thondroril River", "Eastern Plaguelands", new()
        {
            ("52-53", "Plaguehound Runt"),
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Tyr's Hand", "Eastern Plaguelands", new()
        {
            ("52-53", "Scarlet Curate"),
            ("52-53", "Scarlet Warder"),
            ("52-53", "Scarlet Enchanter"),
            ("52-53", "Scarlet Cleric"),
        }),
        new SiteHostileArea("The Marris Stead", "Eastern Plaguelands", new()
        {
            ("52-53", "Putrid Gargoyle"),
            ("52-53", "Plaguebat"),
            ("52-53", "Plaguehound Runt"),
        }),

        #endregion

        #region Ruins of Ahn'Qiraj

        new SiteHostileArea("Scarab Terrace", "Ruins of Ahn'Qiraj", new()
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
        new SiteHostileArea("General's Terrace", "Ruins of Ahn'Qiraj", new()
        {
            ("60", "Qiraji Gladiator"),
            ("60", "Qiraji Warrior"),
            ("60", "Swarmguard Needler"),
        },
        new()
        {
            (04, "60", "General Rajaxx")
        }),
        new SiteHostileArea("Reservoir", "Ruins of Ahn'Qiraj", new()
        {
            ("60", "Flesh Hunter"),
            ("60", "Obsidian Destroyer"),
        },
        new()
        {
            (03, "60", "Moam")
        }),
        new SiteHostileArea("Hatchery", "Ruins of Ahn'Qiraj", new()
        {
            ("60", "Flesh Hunter"),
            ("60", "Hive'Zara Sandstalker"),
            ("60", "Hive'Zara Soldier"),
        },
        new()
        {
            (04, "60", "Buru The Gorger")
        }),
        new SiteHostileArea("Comb", "Ruins of Ahn'Qiraj", new()
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
        new SiteHostileArea("Watcher's Terrace", "Ruins of Ahn'Qiraj", new()
        {
            ("60", "Anubisath Guardian")
        },
        new()
        {
            (02, "60", "Ossirian The Unscarred")
        }),

        #endregion

        #region Onyxia's Lair
        
        new SiteHostileArea("Onyxia's Lair", "Onyxia's Lair", new()
        {
            ("60", "Onyxian Lair Guard"),
            ("60", "Onyxian Warder")
        },
        new()
        {
            (00, "60", "Onyxia")
        }),

        #endregion

        #region Zul'Gurub
        
        new SiteHostileArea("Altar of Hir'eek", "Zul'Gurub", new()
        {
            ("60", "Bloodseeker Bat"),
            ("60", "Gurubashi Bat Rider"),
            ("60", "Gurubashi Headhunter"),
        },
        new()
        {
            (03, "60", "High Priestess Jeklik")
        }),
        new SiteHostileArea("The Coil", "Zul'Gurub", new()
        {
            ("60", "Gurubashi Axe Thrower"),
            ("60", "Razzashi Cobra"),
            ("60", "Razzashi Serpent"),
        },
        new()
        {
            (03, "60", "High Priest Venoxis")
        }),
        new SiteHostileArea("Shadra'zaar", "Zul'Gurub", new()
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
        new SiteHostileArea("Hakkari Grounds", "Zul'Gurub", new()
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
        new SiteHostileArea("Edge of Madness", "Zul'Gurub", new()
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
        new SiteHostileArea("Naze of Shirvallah", "Zul'Gurub", new()
        {
            ("60", "Gurubashi Axe Thrower"),
            ("60", "Zulian Tiger"),
            ("60", "Gurubashi Berserker"),
        },
        new()
        {
            (03, "60", "High Priest Thekal")
        }),
        new SiteHostileArea("Pagle's Pointe", "Zul'Gurub", new()
        {
            ("60", "Hooktooth Frenzy"),
        },
        new()
        {
            (02, "60", "Gahz'ranka")
        }),
        new SiteHostileArea("Temple of Bethekk", "Zul'Gurub", new()
        {
            ("60", "Zulian Panther"),
            ("60", "Gurubashi Blood Drinker"),
            ("60", "Hakkari Shadow Hunter"),
        },
        new()
        {
            (00, "60", "High Priestess Arlokk")
        }),
        new SiteHostileArea("The Bloodfire Pit", "Zul'Gurub", new()
        {
            ("60", "Voodoo Slave"),
            ("60", "Withered Mistress"),
            ("60", "Atal'ai Mistress"),
        },
        new()
        {
            (00, "60", "Jin'do the Hexxer")
        }),
        new SiteHostileArea("Altar of the Blood God", "Zul'Gurub", new()
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
