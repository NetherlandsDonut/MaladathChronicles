using System.Linq;
using System.Collections.Generic;

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
        if (this.possibleEncounters.Count > 0)
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

    public string name, zone;
    public int recommendedLevel;
    public bool instancePart, complexPart;
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
        new SiteHostileArea("The Hoard", "Stratholme", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Malor the Zealous")
        }),
        new SiteHostileArea("The Hall of Lights", "Stratholme", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Malor the Zealous")
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

        new SiteHostileArea("Kraul Commons", "Razorfen Kraul", new()
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
        new SiteHostileArea("Kraul Drain", "Razorfen Kraul", new()
        {

        },
        new()
        {

        }),
        new SiteHostileArea("Central Grounds", "Razorfen Kraul", new()
        {
            ("40", "Razorfen Kraulshaper"),
            ("40", "Razorfen Scarbalde"),
            ("40", "Razorfen Huntmaster"),
        },
        new()
        {
            (2, "40", "Warlord Ramtusk")
        }),
        new SiteHostileArea("Bat Caves", "Razorfen Kraul", new()
        {
            ("40", "Kraulshaped Monstrosity"),
            ("40", "Razorfen Kraulshaper"),
            ("40", "Vile Bat")
        },
        new()
        {
            (2, "40", "Groyat, the Blind Hunter")
        }),
        new SiteHostileArea("Charlga's Seat", "Razorfen Kraul", new()
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
        
        #region Ruins of Stromgarde
        
        new SiteHostileArea("North Town", "Ruins of Stromgarde", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("The Sanctum", "Ruins of Stromgarde", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Western Town", "Ruins of Stromgarde", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Tower of Arathor", "Ruins of Stromgarde", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Stromgarde Keep", "Ruins of Stromgarde", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),

        #endregion

        #region Ruins of Alterac
        
        new SiteHostileArea("Town Center", "Ruins of Alterac", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Alterac Chapel", "Ruins of Alterac", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Alterac Keep", "Ruins of Alterac", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),

        #endregion

        #region Purgation Isle
        
        new SiteHostileArea("Isle Landing", "Purgation Isle", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Mountain Peak", "Purgation Isle", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),

        #endregion

        #region Scarlet Monastery Grounds

        new SiteHostileArea("Whispering Gardens", "Scarlet Monastery Grounds", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("Terrace of Repose", "Scarlet Monastery Grounds", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("The Grand Vestibule", "Scarlet Monastery Grounds", new()
        {
            ("32-32", "Cannibal Ghoul"),
        }),

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
        new SiteHostileArea("Gallery of Treasures", "Scarlet Monastery", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {

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
            (04, "40", "Herod"),
        }),
        new SiteHostileArea("Chapel Gardens", "Scarlet Monastery", new()
        {
            ("39-40", "Scarlet Myrmidon"),
            ("39-40", "Scarlet Defender"),
            ("39-40", "Scarlet Wizard"),
        },
        new()
        {

        }),
        new SiteHostileArea("Crusader's Chapel", "Scarlet Monastery", new()
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
        new SiteHostileArea("Western Lavafalls", "Molten Core", new()
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
        new SiteHostileArea("Eastern Lavafalls", "Molten Core", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
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
        new SiteHostileArea("Lariss Pavillion", "Feralas", new()
        {

        }),
        new SiteHostileArea("Wildwind Lake", "Feralas", new()
        {

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
        new SiteHostileArea("Venomweb Vale", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Balnir Farmstead", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Brightwater Lake", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Crusader's Outpost", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Cold Hearth Manor", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Agamand Mills", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Agamand Family Crypt", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Whispering Shore", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Solliden Farmstead", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Night Web's Hollow", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Nightmare Vale", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The North Coast", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Garren's Haunt", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Ruins of Lordaeron", "Tirisfal Glades", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Moonglade
        
        new SiteHostileArea("Shrine of Remulos", "Moonglade", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stormrage Barrow Dens", "Moonglade", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Azshara

        new SiteHostileArea("The Ruined Reaches", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lake Mennar", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hetaera's Clutch", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ruins of Eldarath", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shadowsong Shrine", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ursolan", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bitter Reaches", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Temple of Arkkoran", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Jagged Reef", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tower of Eldara", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bay of Storms", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Southridge Beach", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ravencrest Monument", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thalassian Base Camp", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Shattered Strand", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Legash Encampment", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Haldarr Encampment", "Azshara", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Forlorn Ridge", "Azshara", new()
        {

        }),

        #endregion
        
        #region Dustwallow Marsh

        new SiteHostileArea("Witch Hill", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tidefury Cove", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Quagmire", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Den of Flame", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stonemaul Ruins", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shady Rest Inn", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sentry Point", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lost Point", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Northpoint Tower", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Emberstrife's Den", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dreadmurk Shore", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Darkmist Cavern", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bluefen", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bloodfen Burrow", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Beezil's Wreck", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Alcaz Island", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Alcaz Dungeon", "Dustwallow Marsh", new()
        {
            ("01-02", "Duskbat"),
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
        
        #region Thousand Needles

        new SiteHostileArea("Tahonda Ruins", "Thousand Needles", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Rustmaul Digsite", "Thousand Needles", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Splithoof Crag", "Thousand Needles", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Camp Ethok", "Thousand Needles", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Darkcloud Pinnacle", "Thousand Needles", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shimmering Flats", "Thousand Needles", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Roguefeather Den", "Thousand Needles", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Highperch", "Thousand Needles", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tahonda Ruins", "Thousand Needles", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Tanaris

        new SiteHostileArea("The Noxious Lair", "Tanaris", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Abyssal Sands", "Tanaris", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lost Rigger Cove", "Tanaris", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Gaping Chasm", "Tanaris", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dunemaul Compound", "Tanaris", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Southmoon Ruins", "Tanaris", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Eastmoon Ruins", "Tanaris", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sandsorrow Watch", "Tanaris", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thistleshrub Valley", "Tanaris", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Noonshade Ruins", "Tanaris", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Land's End Beach", "Tanaris", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Un'Goro Crater

        new SiteHostileArea("Lakkari Tar Pits", "Un'Goro Crater", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fire Plume Ridge", "Un'Goro Crater", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Marshlands", "Un'Goro Crater", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fungal Rock", "Un'Goro Crater", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Terror Run", "Un'Goro Crater", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Slithering Scar", "Un'Goro Crater", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Golakka Hot Springs", "Un'Goro Crater", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Darkshore

        new SiteHostileArea("Cliffspring River", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Twilight Vale", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Twilight Shore", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mist's Edge", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bashal'Aran", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ameth'Aran", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ruins of Mathystra", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tower of Althalaxx", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Master's Glaive", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Moonkin Caves", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Remtravel's Excavation", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Blackwood Den", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Grove of the Ancients", "Darkshore", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Teldrassil

        new SiteHostileArea("Shadowglen", "Teldrassil", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Wellspring Lake", "Teldrassil", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Wellspring River", "Teldrassil", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shadowthread Cave", "Teldrassil", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Oracle Glade", "Teldrassil", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shadowglen", "Teldrassil", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ban'ethil Hollow", "Teldrassil", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fel Rock", "Teldrassil", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Gnarlpine Hold", "Teldrassil", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lake Al'Ameth", "Teldrassil", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Pools of Arlithrien", "Teldrassil", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Searing Gorge

        new SiteHostileArea("The Slag Pit", "Searing Gorge", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Cauldron", "Searing Gorge", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Firewatch Ridge", "Searing Gorge", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Grimesilt Digsite", "Searing Gorge", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dustfire Valley", "Searing Gorge", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stonewrought Pass", "Searing Gorge", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Blackchar Cave", "Searing Gorge", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Wetlands
        
        new SiteHostileArea("Dun Modr", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dun Algaz", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dragonmaw Gates", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Grim Batol", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sundown Marsh", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Whelgar's Excavation Site", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ironbeard's Tomb", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thelgen Rock", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mosshide Fen", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Green Belt", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Saltspray Glen", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sundown Marsh", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bluegill Marsh", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Black Channel Marsh", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Angerfang Encampment", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Raptor Ridge", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Direforge Hill", "Wetlands", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Alterac Mountains
        
        new SiteHostileArea("Ruins of Strahnbrad", "Alterac Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Headland", "Alterac Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Slaughter Hollow", "Alterac Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dalaran", "Alterac Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lordamere Internment Camp", "Alterac Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Crushridge Hold", "Alterac Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Growless Cave", "Alterac Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Hillsbrad Foothills
        
        new SiteHostileArea("Durnholde Keep", "Hillsbrad Foothills", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ravenholdt Manor", "Hillsbrad Foothills", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Darrow Hill", "Hillsbrad Foothills", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Eastern Strand", "Hillsbrad Foothills", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Western Strand", "Hillsbrad Foothills", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Nethander Stead", "Hillsbrad Foothills", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Southpoint Tower", "Hillsbrad Foothills", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Chillwind Point", "Hillsbrad Foothills", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Azurelode Mine", "Hillsbrad Foothills", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hillsbrad", "Hillsbrad Foothills", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Purgation Isle", "Hillsbrad Foothills", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Arathi Highlands
        
        new SiteHostileArea("Thandol Span", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Go'Shek Farm", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Circle of West Binding", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Circle of Inner Binding", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Circle of East Binding", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Circle of Outer Binding", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dabyrie's Farmstead", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Drywhisker Gorge", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Northfold Manor", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Boulder'gor", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Boulderfist Hall", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Witherbark Village", "Arathi Highlands", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Hinterlands
        
        new SiteHostileArea("Jintha'Alor", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Skulk Rock", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Seradane", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shandra'Alor", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Plaguemist Ravine", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Valorwind Lake", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Creeping Ruin", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Overlook Cliffs", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Agol'watha", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shaol'watha", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hiri'watha", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Altar of Zul", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bogen's Ledge", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Quel'Danil Lodge", "Hinterlands", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Redridge Mountains
        
        new SiteHostileArea("Redridge Canyons", "Redridge Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Render's Valley", "Redridge Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Render's Camp", "Redridge Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stonewatch", "Redridge Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Alther's Mill", "Redridge Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Rethban Caverns", "Redridge Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lakeridge Highway", "Redridge Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lake Everstill", "Redridge Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tower of Ilgalar", "Redridge Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Galardell Valley", "Redridge Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Three Corners", "Redridge Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Felwood
        
        new SiteHostileArea("Ruins of Constellas", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Deadmaw Village", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Irontree Woods", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Irontree Cavern", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bloodvenom River", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bloodvenom Falls", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Jaedenar", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shrine of the Deceiver", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Deadwood Village", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Felpaw Village", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Morlos'Aran", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shatter Scar Vale", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Jadefire Run", "Felwood", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Shadow Hold
        
        new SiteHostileArea("Upper Tunnels", "Shadow Hold", new()
        {
            ("52-53", "Jaedenar Enforcer"),
            ("52-53", "Jaedenar Warlock"),
            ("52-53", "Jaedenar Darkweaver"),
        },
        new()
        {
            (02, "54", "Ulathek")
        }),
        new SiteHostileArea("Council Camp", "Shadow Hold", new()
        {
            ("53-54", "Jadefire Trickster"),
            ("53-54", "Jadefire Hellcaller"),
            ("53-54", "Vile Ooze"),
        },
        new()
        {
            (03, "55", "Prince Xavalis")
        }),
        new SiteHostileArea("Altar Room", "Shadow Hold", new()
        {
            ("53-54", "Jaedenar Warlock"),
            ("54-55", "Jaedenar Hunter"),
            ("55-56", "Jaedenar Sayaad"),
        },
        new()
        {
            (03, "56", "Rakaiah")
        }),
        new SiteHostileArea("Inner Sanctum", "Shadow Hold", new()
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
        new SiteHostileArea("Banehollow Shrine", "Shadow Hold", new()
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

        new SiteHostileArea("Ruins of Kel'Theril", "Winterspring", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hidden Grove", "Winterspring", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Frostsaber Rock", "Winterspring", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Darkwhisper Gorge", "Winterspring", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Frostwhisper Gorge", "Winterspring", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mazthoril", "Winterspring", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ice Thistle Hills", "Winterspring", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Frostfire Hot Springs", "Winterspring", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Winterfall Village", "Winterspring", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Timbermaw Post", "Winterspring", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Owl Wing Thicket", "Winterspring", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Deadwind Pass

        new SiteHostileArea("The Vice", "Deadwind Pass", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Deadman's Crossing", "Deadwind Pass", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Karazhan", "Deadwind Pass", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sleeping Gorge", "Deadwind Pass", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ariden's Camp", "Deadwind Pass", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Blasted Lands

        new SiteHostileArea("The Dark Portal", "Blasted Lands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Serpent's Coil", "Blasted Lands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dreadmaul Hold", "Blasted Lands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dreadmaul Post", "Blasted Lands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Tainted Scar", "Blasted Lands", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Altar of Storms", "Blasted Lands", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Stonetalon Mountains

        new SiteHostileArea("Talondeep Path", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Talon Den", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Webwinder Path", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Charred Vale", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Windshear Mine", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Windshear Crag", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sishir Canyon", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mirkfallon Lake", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Camp Aparaje", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Grimtotem Post", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Cragpool Lake", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Boulderslide Ravine", "Stonetalon Mountains", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region The Barrens

        new SiteHostileArea("Lushwater Oasis", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Stagnant Oasis", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Field of Giants", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thorn Hill", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Agama'gor", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Southfury River", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bramblescar", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bael Modan", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Dry Hills", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Sludge Fen", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Raptor Grounds", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Northwatch Hold", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Forgotten Pools", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Boulder Lode Mine", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Forgotten Pools", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fray Island", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Gold Road", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Southern Gold Road", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dreadmist Peak", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Honor's Stand", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Blackthorn Ridge", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Baeldun Keep", "The Barrens", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Mulgore

        new SiteHostileArea("The Ravaged Caravan", "Mulgore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Palemane Rock", "Mulgore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Baeldun Digsite", "Mulgore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Brambleblade Ravine", "Mulgore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Red Cloud Mesa", "Mulgore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Golden Plains", "Mulgore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Red Rocks", "Mulgore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Windfury Ridge", "Mulgore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Venture Co. Mine", "Mulgore", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Rolling Plains", "Mulgore", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Silverpine Forest
        
        new SiteHostileArea("Deep Elem Mine", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Graymane Wall", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fenris Keep", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Dawning Isles", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Dead Field", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Pyrewood Village", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ambermill", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Shining Strand", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Skittering Dark", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Olsen's Farthing", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ivar Patch", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Malden's Orchard", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Beren's Peril", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("North Tide's Run", "Silverpine Forest", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Loch Modan

        new SiteHostileArea("Valley of Kings", "Loch Modan", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ironband's Excavation Site", "Loch Modan", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stonesplinter Valley", "Loch Modan", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mo'grosh Stronghold", "Loch Modan", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Silver Stream Mine", "Loch Modan", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Durotar

        new SiteHostileArea("Skull Rock", "Durotar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Scuttle Coast", "Durotar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tiragarde Keep", "Durotar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thunder Ridge", "Durotar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bladefist Bay", "Durotar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Echo Isles", "Durotar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Drygulch Ravine", "Durotar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Burning Blade Coven", "Durotar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Kolkar Crag", "Durotar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Razormane Grounds", "Durotar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Razorwind Canyon", "Durotar", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Ashenvale
        
        new SiteHostileArea("The Howling Vale", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Demon Fall Canyon", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Satyrnaar", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Felfire Hill", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Iris Lake", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fallen Sky Lake", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bloodtooth Camp", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ruins of Stardust", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fire Scar Shrine", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Xavian", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Zoram Strand", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lake Falathim", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Shrine of Aessina", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thistlefur Village", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Nightsong Woods", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ruins of Ordil'Aran", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mystral Lake", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Dor'Danil Barrow Den", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bough Shadow", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Bathran's Haunt", "Ashenvale", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Burning Steppes

        new SiteHostileArea("The Pillar of Ash", "Burning Steppes", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Slither Rock", "Burning Steppes", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Terror Wing Path", "Burning Steppes", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Dreadmaul Rock", "Burning Steppes", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Draco'dar", "Burning Steppes", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Blackrock Stronghold", "Burning Steppes", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ruins of Thaurissan", "Burning Steppes", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Blackrock Pass", "Burning Steppes", new()
        {
            ("01-02", "Duskbat"),
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
        new SiteHostileArea("Ethel Rethor", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sargeron", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Valley of Bones", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Valley of Spears", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sar'theris Strand", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Magram Village", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Thunder Axe Fortress", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Kodo Graveyard", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Shadowbreak Ravine", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Ranazjar Isle", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Kolkar Village", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mannoroc Coven", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Tethris Aran", "Desolace", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Elwynn Froest

        new SiteHostileArea("Jasperlode Mine", "Elwynn Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Forest's Edge", "Elwynn Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Echo Ridge Mine", "Elwynn Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mirror Lake", "Elwynn Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Mirror Lake Orchard", "Elwynn Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hero's Vigil", "Elwynn Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Northshire Vineyards", "Elwynn Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Stonefield Farm", "Elwynn Forest", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Fargodeep Mine", "Elwynn Forest", new()
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
        
        #region Shadowfang Keep

        new SiteHostileArea("Jail", "Shadowfang Keep", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("The Courtyard", "Shadowfang Keep", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Kitchen", "Shadowfang Keep", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Dining Hall", "Shadowfang Keep", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Perimeter Wall", "Shadowfang Keep", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("The Laboratory", "Shadowfang Keep", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Tower Summit", "Shadowfang Keep", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Arugal Chamber", "Shadowfang Keep", new()
        {
            ("36-37", "Stonevault Ambusher"),
        },
        new()
        {
            (3, "40", "Revelosh"),
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

        new SiteHostileArea("Coldridge Valley", "Dun Morogh", new()
        {
            ("01-02", "Rockjaw Trogg"),
            ("01-02", "Ragged Young Wolf"),
            ("01-03", "Small Crag Boar"),
            ("02-03", "Burly Rockjaw Trogg"),
        }),
        new SiteHostileArea("Coldridge Pass", "Dun Morogh", new()
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
        new SiteHostileArea("Misty Pine Refuge", "Dun Morogh", new()
        {

        }),

        #endregion
        
        #region Western Plaguelands

        new SiteHostileArea("Uther's Tomb", "Western Plaguelands", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Dalson's Tears", "Western Plaguelands", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Ruins of Andorhal", "Western Plaguelands", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Caer Darrow", "Western Plaguelands", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Felstone Field", "Western Plaguelands", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("The Writhing Haunt", "Western Plaguelands", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Northridge Lumber Camp", "Western Plaguelands", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Gahrron's Withering", "Western Plaguelands", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("The Weeping Cave", "Western Plaguelands", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Central Town", "Hearthglen", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Hearthglen Mine", "Hearthglen", new()
        {
            ("53-54", "Scourge Warder"),
        }),
        new SiteHostileArea("Mardenholde Keep", "Hearthglen", new()
        {
            ("53-54", "Scourge Warder"),
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
        new SiteHostileArea("The Marris Stead", "Eastern Plaguelands", new()
        {
            ("52-53", "Putrid Gargoyle"),
            ("52-53", "Plaguebat"),
            ("52-53", "Plaguehound Runt"),
        }),
        new SiteHostileArea("Crown Guard Tower", "Eastern Plaguelands", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Eastwall Tower", "Eastern Plaguelands", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Eastwall Gate", "Eastern Plaguelands", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Northdale", "Eastern Plaguelands", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("The Infectis Scar", "Eastern Plaguelands", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Quel'Lithien Lodge", "Eastern Plaguelands", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Plaguewood Tower", "Eastern Plaguelands", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Northpass Tower", "Eastern Plaguelands", new()
        {
            ("52-53", "Plaguebat"),
        }),
        new SiteHostileArea("Town Square", "Tyr's Hand", new()
        {
            ("52-53", "Scarlet Curate"),
            ("52-53", "Scarlet Warder"),
        }),
        new SiteHostileArea("Tyr's Hand Abbey", "Tyr's Hand", new()
        {
            ("52-53", "Scarlet Warder"),
            ("52-53", "Scarlet Cleric"),
        }),
        new SiteHostileArea("Tyr's Hand Keep", "Tyr's Hand", new()
        {
            ("52-53", "Scarlet Curate"),
            ("52-53", "Scarlet Warder"),
        }),
        new SiteHostileArea("Scarlet Basilica", "Tyr's Hand", new()
        {
            ("52-53", "Scarlet Curate"),
            ("52-53", "Scarlet Enchanter"),
            ("52-53", "Scarlet Cleric"),
        }),

        #endregion

        #region Silithus
        
        new SiteHostileArea("Bones of Grakkarond", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Swarming Pillar", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Scarab Wall", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hive'Zora", "Silithus", new()
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
        new SiteHostileArea("Twilight Outpost", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Twilight Post", "Silithus", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Twilight Base Camp", "Silithus", new()
        {
            ("01-02", "Duskbat"),
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
        new SiteHostileArea("The Reservoir", "Ruins of Ahn'Qiraj", new()
        {
            ("60", "Flesh Hunter"),
            ("60", "Obsidian Destroyer"),
        },
        new()
        {
            (03, "60", "Moam")
        }),
        new SiteHostileArea("The Hatchery", "Ruins of Ahn'Qiraj", new()
        {
            ("60", "Flesh Hunter"),
            ("60", "Hive'Zara Sandstalker"),
            ("60", "Hive'Zara Soldier"),
        },
        new()
        {
            (04, "60", "Buru The Gorger")
        }),
        new SiteHostileArea("The Comb", "Ruins of Ahn'Qiraj", new()
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
        
        #region Temple of Ahn'Qiraj

        new SiteHostileArea("The Temple Gates", "Temple of Ahn'Qiraj", new()
        {
            ("60", "Anubisath Sentinel"),
            ("60", "Obsidian Eradicator")
        },
        new()
        {
            (02, "60", "The Prophet Skeram")
        }),
        new SiteHostileArea("The Hive Undergrounds", "Temple of Ahn'Qiraj", new()
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
        new SiteHostileArea("Abandoned Tunnel", "Temple of Ahn'Qiraj", new()
        {

        },
        new()
        {
            (00, "60", "Viscidus")
        }),
        new SiteHostileArea("Princess Chambers", "Temple of Ahn'Qiraj", new()
        {
            ("60", "Vekniss Borer"),
            ("60", "Vekniss Guardian"),
            ("60", "Vekniss Hive Crawler"),
        },
        new()
        {
            (02, "60", "Princess Huhuran")
        }),
        new SiteHostileArea("Qiraji Imperial Seat", "Temple of Ahn'Qiraj", new()
        {
            ("60", "Anubisath Sentinel"),
            ("60", "Obsidian Eradicator")
        },
        new()
        {
            (02, "60", "Twin Emperors")
        }),
        new SiteHostileArea("Ouro's Lair", "Temple of Ahn'Qiraj", new()
        {
            ("60", "Vekniss Borer"),
            ("60", "Vekniss Guardian"),
            ("60", "Vekniss Hive Crawler"),
        },
        new()
        {
            (02, "60", "Ouro")
        }),
        new SiteHostileArea("Vault of C'Thun", "Temple of Ahn'Qiraj", new()
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
        
        new SiteHostileArea("Stagalbog", "Swamp of Sorrows", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sorrowmurk", "Swamp of Sorrows", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Pool of Tears", "Swamp of Sorrows", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Misty Valley", "Swamp of Sorrows", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Shifting Marsh", "Swamp of Sorrows", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Splinterspear Junction", "Swamp of Sorrows", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Misty Reed Strand", "Swamp of Sorrows", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Itharius's Cave", "Swamp of Sorrows", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion
        
        #region Temple of Atal'Hakkar
        
        new SiteHostileArea("The Pit of Sacrafice", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Butchery", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("The Broken Hall", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hall of Serpents", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hall of the Cursed", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Pit of Refuse", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Lair of the Chosen", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hall of Masks", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hall of Ritual", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Hall of Bones", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Den of the Caller", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Chamber of Blood", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Chamber of the Dreamer", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),
        new SiteHostileArea("Sanctum of the Fallen God", "Temple of Atal'Hakkar", new()
        {
            ("01-02", "Duskbat"),
        }),

        #endregion

        #region Stranglethorn Valley
        
        new SiteHostileArea("Gurubashi Arena", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Crystal Shore", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Zuuldaia Ruins", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Ruins of Zul'Kunda", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Wild Shore", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Nek'mani Wellspring", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Bloodsail Compound", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Tkashi Ruins", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Venture Co. Base Camp", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("The Savage Coast", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Mizjah Ruins", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Balia'mah Ruins", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Mosh'Ogg Ogre Mound", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Jaguero Isle", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Ruins of Aboraz", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Mistvale Valley", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Ruins of Zul'Mamwe", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Kurzen's Compound", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
        }),
        new SiteHostileArea("Crystalvein Mine", "Stranglethorn Valley", new()
        {
            ("35-37", "Dustbelcher Warrior"),
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
