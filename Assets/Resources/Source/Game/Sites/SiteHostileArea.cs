using System.Linq;
using System.Collections.Generic;

using static Root;
using System.Diagnostics;

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
            //UnityEngine.Debug.Log(encounter + " " + name);
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

    public string name;
    public int recommendedLevel;
    public bool instancePart;
    public List<(int, int, string)> possibleEncounters;
    public List<(int, int, string)> bossEncounters;

    public static List<SiteHostileArea> hostileAreas = new()
    {
        #region Stratholme

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
        }),
        new SiteHostileArea("Market Row", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "The Unforgiven")
        }),
        new SiteHostileArea("Crusaders' Square", new()
        {
            ("56", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "58", "Timmy the Cruel")
        }),
        new SiteHostileArea("The Scarlet Bastion", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Malor the Zealous")
        }),
        new SiteHostileArea("The Crimson Throne", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "62", "Balnazzar")
        }),
        new SiteHostileArea("Elder's Square", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Magistrate Barthilas")
        }),
        new SiteHostileArea("The Gauntlet", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Nerub'enkan")
        }),
        new SiteHostileArea("Slaughter Square", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "61", "Ramstein the Gorger")
        }),
        new SiteHostileArea("The Slaughter House", new()
        {
            ("59-60", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "62", "Baron Rivendare")
        }),

        #endregion

        #region Razorfen Downs

        new SiteHostileArea("The Caller's Chamber", new()
        {
            ("40", "Ragglesnout"),
        },
        new()
        {
            (2, "40", "Plaguemaw"),
            (2, "40", "Tuten'kash")
        }),
        new SiteHostileArea("The Bone Pile", new()
        {
            ("40", "Ragglesnout"),
        },
        new()
        {
            (2, "39", "Mordresh Fire Eye"),
        }),
        new SiteHostileArea("Spiral of Thorns", new()
        {
            ("40", "Ragglesnout"),
        },
        new()
        {
            (2, "40", "Glutton"),
            (2, "41", "Amnennar the Coldbringer"),
        }),

        #endregion

        #region Deadmines

        new SiteHostileArea("Defias Hideout", new()
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
        new SiteHostileArea("Mast Room", new()
        {
            ("18-19", "Defias Taskmaster"),
            ("18-19", "Defias Wizard"),
            ("18-19", "Defias Strip Miner")
        },
        new()
        {
            (3, "19", "Sneed"),
        }),
        new SiteHostileArea("Goblin Foundry", new()
        {
            ("18-20", "Remote Controlled Golem"),
            ("18-19", "Goblin Engineer")
        },
        new()
        {
            (3, "20", "Gilnid"),
        }),
        new SiteHostileArea("Ironclad Cove", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "20", "Mr. Smite"),
        }),
        new SiteHostileArea("The Juggernaut", new()
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
        
        new SiteHostileArea("Reliquary", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "59", "Blood Steward of Kirtonos")
        }),
        new SiteHostileArea("Chamber of Summoning", new()
        {
            ("18-19", "Defias Taskmaster"),
            ("18-19", "Defias Wizard"),
            ("18-19", "Defias Strip Miner")
        },
        new()
        {
            (3, "59", "Kirtonos the Herald"),
        }),
        new SiteHostileArea("Great Ossuary", new()
        {
            ("18-19", "Defias Taskmaster"),
            ("18-19", "Defias Wizard"),
            ("18-19", "Defias Strip Miner")
        },
        new()
        {
            (3, "61", "Rattlegore"),
        }),
        new SiteHostileArea("Hall of Secrets", new()
        {
            ("18-20", "Remote Controlled Golem"),
            ("18-19", "Goblin Engineer")
        },
        new()
        {
            (3, "60", "Lorekeeper Polkelt"),
        }),
        new SiteHostileArea("Hall of the Damned", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "60", "Doctor Theolen Krastinov"),
        }),
        new SiteHostileArea("Laboratory", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "62", "Ras Frostwhisper")
        }),
        new SiteHostileArea("Vault of the Ravenian", new()
        {
            ("19-20", "Defias Blackguard"),
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (2, "60", "The Ravenian")
        }),
        new SiteHostileArea("The Coven", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "60", "Instructor Malicia")
        }),
        new SiteHostileArea("The Shadow Vault", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "60", "Instructor Malicia")
        }),
        new SiteHostileArea("Viewing Room", new()
        {
            ("19-20", "Defias Squallshaper"),
            ("19-20", "Defias Pirate")
        },
        new()
        {
            (3, "60", "Vectus"),
        }),
        new SiteHostileArea("Barov Family Vault", new()
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
        new SiteHostileArea("Headmaster's Study", new()
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

        new SiteHostileArea("The Noxious Hollow", new()
        {
            ("45-46", "Creeping Sludge"),
            ("45-46", "Spewed Larva"),
            ("45-46", "Constrictor Vine")
        },
        new()
        {
            (3, "47", "Noxxion"),
        }),
        new SiteHostileArea("Foulspore Cavern", new()
        {
            ("46-47", "Barbed Lasher"),
            ("46-48", "Celebrian Dryad"),
            ("46-47", "Deeprot Stomper")
        },
        new()
        {
            (3, "48", "Razorlash"),
        }),
        new SiteHostileArea("Wicked Grotto", new()
        {
            ("47-48", "Deeprot Stomper"),
            ("46-47", "Deeprot Tangler"),
            ("46-47", "Poison Sprite")
        },
        new()
        {
            (3, "48", "Tinkerer Gizlock"),
        }),
        new SiteHostileArea("Vyletongue Seat", new()
        {
            ("46-47", "Putridus Satyr"),
            ("47-48", "Putridus Shadowstalker"),
            ("46-47", "Putridus Trickster")
        },
        new()
        {
            (3, "48", "Lord Vyletongue"),
        }),
        new SiteHostileArea("Poison Falls", new()
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
        new SiteHostileArea("Earth Song Falls", new()
        {
            ("50", "Rotgrip"),
            ("48-49", "Primordial Behemoth"),
            ("48-49", "Theradrim Guardian")
        },
        new()
        {
            (3, "50", "Landslide"),
        }),
        new SiteHostileArea("Zaetar's Grave", new()
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

        new SiteHostileArea("Trogg Caves", new()
        {
            ("27-28", "Caverndeep Burrower"),
            ("28-29", "Irradiated Pillager"),
            ("28-29", "Caverndeep Ambusher")
        },
        new()
        {
            (3, "30", "Grubbis"),
        }),
        new SiteHostileArea("Hall of Gears", new()
        {
            ("29-30", "Corrosive Lurker"),
            ("29-30", "Irradiated Slime"),
            ("29-31", "Irradiated Horror")
        },
        new()
        {
            (3, "31", "Viscous Fallout"),
        }),
        new SiteHostileArea("Launch Bay", new()
        {
            ("30-31", "Leprous Technician"),
            ("31-32", "Mobile Alert System"),
            ("31-32", "Mechanized Sentry")
        },
        new()
        {
            (3, "32", "Electrocutioner 6000"),
        }),
        new SiteHostileArea("Engineering Labs", new()
        {
            ("32-33", "Mechano Tank"),
            ("31-33", "Mobile Alert System")
        },
        new()
        {
            (3, "33", "Crowd Pummeler 9-60"),
        }),
        new SiteHostileArea("Tinkers' Court", new()
        {

        },
        new()
        {
            (0, "34", "Mekgineer Thermaplugg"),
        }),

        #endregion

        #region Wailing Caverns

        new SiteHostileArea("Screaming Gully", new()
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
        new SiteHostileArea("Pit of Fangs", new()
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
        new SiteHostileArea("Winding Chasm", new()
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
        new SiteHostileArea("Crag of the Everliving", new()
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
        new SiteHostileArea("Dreamer's Rock", new()
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
        
        new SiteHostileArea("Pool of Ask'ar", new()
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
        new SiteHostileArea("Shrine of Gelihast", new()
        {
            ("25-26", "Blindlight Oracle"),
            ("25-26", "Blindlight Muckdweller")
        },
        new()
        {
            (2, "26", "Gelihast"),
        }),
        new SiteHostileArea("Moonshrine Ruins", new()
        {
            ("26", "Aqua Guardian"),
            ("25-26", "Twilight Acolyte"),
            ("25-27", "Twilight Aquamancer")
        },
        new()
        {
            (3, "27", "Baron Aquanis"),
        }),
        new SiteHostileArea("Forgotten Pool", new()
        {
            ("26-27", "Deep Pool Threshkin"),
            ("26-27", "Skittering Crustacean")
        },
        new()
        {
            (2, "27", "Old Serra'kis"),
        }),
        new SiteHostileArea("Moonshrine Sanctum", new()
        {
            ("27-28", "Twilight Elementalist"),
            ("27-28", "Twilight Shadowmage")
        },
        new()
        {
            (2, "28", "Twilight Lord Kelris"),
        }),
        new SiteHostileArea("Aku'mai's Lair",
        new() { },
        new()
        {
            (0, "28", "Aku'mai"),
        }),
        
        #endregion

        #region Scarlet Monastery

        new SiteHostileArea("Chamber of Atonement", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Interrogator Vishas")
        }),
        new SiteHostileArea("Forlorn Cloister", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Bloodmage Thalnos"),
        }),
        new SiteHostileArea("Honor's Tomb", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Bloodmage Thalnos"),
        }),
        new SiteHostileArea("Huntsman's Cloister", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Houndmaster Loksey"),
        }),
        new SiteHostileArea("Athenaeum", new()
        {
            ("32-32", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "32", "Arcanist Doan"),
        }),

        #endregion
        
        #region Blackrock Depths

        new SiteHostileArea("Detention Block", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "52", "High Interrogator Gerstahn")
        }),
        new SiteHostileArea("Halls of the Law", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "53", "Houndmaster Grebmar"),
            (04, "53", "Lord Roccor")
        }),
        new SiteHostileArea("Ring of Law", new()
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
        new SiteHostileArea("Shrine of Thaurissan", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Pyromancer Loregrain")
        }),
        new SiteHostileArea("The Black Vault", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Warder Stilgiss")
        }),
        new SiteHostileArea("Dark Iron Highway", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "55", "Bael'Gar")
        }),
        new SiteHostileArea("The Black Anvil", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "56", "Lord Incendius")
        }),
        new SiteHostileArea("Hall of Crafting", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "56", "Fineous Darkvire")
        }),
        new SiteHostileArea("West Garrison", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "General Angerforge")
        }),
        new SiteHostileArea("The Manufactory", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Golem Lord Argelmach")
        }),
        new SiteHostileArea("The Grim Guzzler", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Golem Lord Argelmach")
        }),
        new SiteHostileArea("Chamber of Enchantment", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Ambassador Flamelash")
        }),
        new SiteHostileArea("Mold Foundry", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "57", "Panzor the Invincible")
        }),
        new SiteHostileArea("Summoners' Tomb", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "58", "The Seven")
        }),
        new SiteHostileArea("The Lyceum", new()
        {
            ("56-57", "Cannibal Ghoul"),
        }),
        new SiteHostileArea("The Iron Hall", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "58", "Magmus")
        }),
        new SiteHostileArea("The Imperial Seat", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "59", "Emperor Dagran Thaurissan")
        }),

        #endregion
        
        #region Blackwing Lair

        new SiteHostileArea("Dragonmaw Garrison", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Razorgore the Untamed")
        }),
        new SiteHostileArea("Shadow Wing Lair", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Vaelastrasz the Corrupt"),
        }),
        new SiteHostileArea("Halls of Strife", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Broodlord Lashlayer"),
        }),
        new SiteHostileArea("Crimson Laboratories", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Firemaw"),
            (04, "60", "Ebonroc"),
            (04, "60", "Flamegor")
        }),
        new SiteHostileArea("Chromaggus' Lair", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Chromaggus")
        }),
        new SiteHostileArea("Nefarian's Lair", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Nefarian")
        }),

        #endregion

        #region Uldaman

        new SiteHostileArea("Hall of the Keepers", new()
        {
            ("36-37", "Stonevault Ambusher"),
            ("37-38", "Stonevault Pillager"),
            ("37-38", "Stonevault Oracle")
        },
        new()
        {
            (3, "40", "Revelosh"),
        }),
        new SiteHostileArea("Map Chamber", new() { },
        new()
        {
            (0, "40", "Ironaya"),
        }),
        new SiteHostileArea("Temple Hall", new()
        {
            ("44", "Stone Steward"),
            ("41-43", "Venomlash Scorpid"),
            ("42-43", "Earthen Sculptor"),
        },
        new()
        {
            (3, "44", "Ancient Stone Keeper"),
        }),
        new SiteHostileArea("Dig Three", new()
        {
            ("42-44", "Shadowforge Darkcaster"),
            ("42-44", "Shadowforge Sharpshooter")
        },
        new()
        {
            (3, "45", "Galgann Firehammer"),
        }),
        new SiteHostileArea("The Stone Vault", new()
        {
            ("42-44", "Jadespine Basilisk"),
            ("41-43", "Stonevault Geomancer"),
            ("42-43", "Stonevault Brawler")
        },
        new()
        {
            (3, "45", "Grimlok"),
        }),
        new SiteHostileArea("Khaz'Goroth's Seat", new()
        {
            ("44-45", "Vault Warder"),
            ("45", "Stone Steward")
        },
        new()
        {
            (2, "47", "Archaedas"),
        }),

        #endregion

        #region Molten Core

        new SiteHostileArea("Magmadar Cavern", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Lucifron"),
            (04, "60", "Magmadar")
        }),
        new SiteHostileArea("The Lavafalls", new()
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
        new SiteHostileArea("Ragnaros' Lair", new()
        {
            ("56-57", "Cannibal Ghoul"),
        },
        new()
        {
            (04, "60", "Ragnaros"),
        }),

        #endregion

        #region Dire Maul

        new SiteHostileArea("Prison Of Immol'Thar", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        }),

        #endregion

        #region Tirisfal Glades

        new SiteHostileArea("Deathknell", new()
        {
            ("01-02", "Duskbat"),
            ("01-02", "Wretched Zombie"),
            ("01-02", "Rattlecage Skeleton"),
        }),

        #endregion

        #region Eastern Plaguelands

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

        #endregion

        #region Ruins of Ahn'Qiraj

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
        }),
        new SiteHostileArea("General's Terrace", new()
        {
            ("60", "Qiraji Gladiator"),
            ("60", "Qiraji Warrior"),
            ("60", "Swarmguard Needler"),
        },
        new()
        {
            (04, "60", "General Rajaxx")
        }),
        new SiteHostileArea("Reservoir", new()
        {
            ("60", "Flesh Hunter"),
            ("60", "Obsidian Destroyer"),
        },
        new()
        {
            (03, "60", "Moam")
        }),
        new SiteHostileArea("Hatchery", new()
        {
            ("60", "Flesh Hunter"),
            ("60", "Hive'Zara Sandstalker"),
            ("60", "Hive'Zara Soldier"),
        },
        new()
        {
            (04, "60", "Buru The Gorger")
        }),
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
        }),
        new SiteHostileArea("Watcher's Terrace", new()
        {
            ("60", "Anubisath Guardian")
        },
        new()
        {
            (02, "60", "Ossirian The Unscarred")
        }),

        #endregion
    };
}
