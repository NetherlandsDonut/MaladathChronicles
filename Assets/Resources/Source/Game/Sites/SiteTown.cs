using System.Collections.Generic;

public class SiteTown
{
    public SiteTown(string name, string zone, string faction)
    {
        this.name = name;
        this.zone = zone;
        this.faction = faction;
    }

    public string name, zone, faction;

    public static SiteTown town;

    public static List<SiteTown> towns = new()
    {
        //HUMAN NATION OF STORMWIND
        new SiteTown("Stormwind",               "Elwynn Forest",        "Alliance"),
        new SiteTown("Goldshire",               "Elwynn Forest",        "Alliance"),
        new SiteTown("Lakeshire",               "Elwynn Forest",        "Alliance"),
        new SiteTown("Northshire Abbey",        "Elwynn Forest", 	    "Alliance"),
        new SiteTown("Southshore",              "Hillsbrad Foothills",  "Alliance"),
        new SiteTown("Darkshire",               "Duskwood",       	    "Alliance"),
        new SiteTown("Rebel Camp",              "Stranglethorn Valley", "Alliance"),
        new SiteTown("Sentinel Hill",           "Westfall",       	    "Alliance"),
        new SiteTown("Theramore Isle",          "Dustwallow Marsh",   	"Alliance"),
        new SiteTown("Menethil Harbor",         "Wetlands", 		    "Alliance"),
        new SiteTown("Nethergarde Keep",        "Blasted Lands", 	    "Alliance"),
        new SiteTown("Morgan's Vigil",          "Burning Steppes", 		"Alliance"),
        new SiteTown("Faldir's Cove",           "Arathi Highlands",     "Alliance"),

        //KALDOREI
        new SiteTown("Darnassus",               "Teldrassil",           "Alliance"),
        new SiteTown("Aldrassil",               "Teldrassil",           "Alliance"),
        new SiteTown("Star Breeze Village",     "Teldrassil",           "Alliance"),
        new SiteTown("Rut'theran Village",      "Teldrassil",           "Alliance"),
        new SiteTown("Dolanaar",                "Teldrassil",           "Alliance"),
        new SiteTown("Astranaar",               "Ashenvale",            "Alliance"),
        new SiteTown("Starfall Village",        "Winterspring",         "Alliance"),
        new SiteTown("Silverwind Refuge",       "Ashenvale",            "Alliance"),
        new SiteTown("Forest Song",             "Ashenvale",            "Alliance"),
        new SiteTown("Auberdine",               "Darkshore",            "Alliance"),
        new SiteTown("Talrendis Point",         "Azshara",              "Alliance"),
        new SiteTown("Talonbranch Glade",       "Felwood",              "Alliance"),
        new SiteTown("Feathermoon Stronghold",  "Feralas",              "Alliance"),
        new SiteTown("Nijel's Point",           "Desolace",             "Alliance"),
        new SiteTown("Thalaanar",               "Thousand Needles",     "Alliance"),
        new SiteTown("Stonetalon Peak",         "Stonetalon Mountains", "Alliance"),

        //WILDHAMMER DWARFS
        new SiteTown("Aerie Peak",              "Hinterlands",          "Alliance"),

        //IRONFORGE DWARFS
        new SiteTown("Ironforge",               "Dun Morogh",       	"Alliance"),
        new SiteTown("Thelsamar",               "Loch Modan", 			"Alliance"),
        new SiteTown("Dun Garok",               "Hillsbrad Foothills",  "Alliance"),
        new SiteTown("Dun Modr",                "Wetlands", 			"Alliance"),
        new SiteTown("Kharanos",                "Dun Morogh", 			"Alliance"),
        new SiteTown("Anvilmar",                "Dun Morogh",       	"Alliance"),
        new SiteTown("Algaz Station",           "Loch Modan", 			"Alliance"),

        //new SiteTown("North Gate Outpost",      "Dun Morogh",           "Alliance"),
        //new SiteTown("South Gate Outpost",      "Dun Morogh",           "Alliance"),
        //new SiteTown("Amberstill Ranch",        "Dun Morogh",           "Alliance"),

        //ORCS
        new SiteTown("Orgrimmar",               "Durotar", 			    "Horde"),
        new SiteTown("Razor Hill",              "Durotar", 			    "Horde"),
        new SiteTown("Zoram'gar Outpost",       "Ashenvale", 		    "Horde"),
        new SiteTown("Crossroads",              "The Barrens", 		    "Horde"),
        new SiteTown("Bloodvenom Post",         "Felwood",              "Horde"),
        new SiteTown("Valormok",                "Azshara",              "Horde"),
        new SiteTown("Grom'gol Base Camp",      "Stranglethorn Valley", "Horde"),
        new SiteTown("Splintertree Post",       "Ashenvale",            "Horde"),
        new SiteTown("Kargath",                 "Badlands",             "Horde"),
        new SiteTown("Stonard",                 "Swamp of Sorrows",     "Horde"),

        //OGRES
        new SiteTown("Brackenwall Village",     "Dustwallow Marsh",     "Horde"),

        //DARKSPEAR TROLLS
        new SiteTown("Sen'jin Village",         "Durotar", 			    "Horde"),
        new SiteTown("Shadowprey Village",      "Desolace",           "Horde"),

        //REVANTUSK TROLLS
        new SiteTown("Revantusk Village",       "Hinterlands",           "Horde"),

        //TAUREN
        new SiteTown("Thunder Bluff",           "Mulgore", 			    "Horde"),
        new SiteTown("Bloodhoof Village",       "Mulgore", 			    "Horde"),
        new SiteTown("Camp Narache",            "Mulgore",              "Horde"),
        new SiteTown("Camp Mojache",            "Feralas",              "Horde"),
        new SiteTown("Camp Taurajo",            "The Barrens",          "Horde"),
        new SiteTown("Freewind Post",           "Thousand Needles",     "Horde"),
        new SiteTown("Sun Rock Retreat",        "Stonetalon Mountains", "Horde"),
        new SiteTown("Ghost Walker Post",       "Desolace",             "Horde"),

        //FORSAKEN
        new SiteTown("Undercity",               "Tirisfal Glades", 		"Horde"),
        new SiteTown("The Bulwark",             "Tirisfal Glades", 		"Horde"),
        new SiteTown("Deathknell",              "Tirisfal Glades", 		"Horde"),
        new SiteTown("Brill",                   "Tirisfal Glades", 		"Horde"),
        new SiteTown("Hammerfall",              "Arathi Highlands", 	"Horde"),
        new SiteTown("Tarren Mill",             "Hillsbrad Foothills", 	"Horde"),
        new SiteTown("Sepulcher",               "Silverpine Forest", 	"Horde"),

        //CENARION CIRCLE
        new SiteTown("Cenarion Hold",           "Silithus",             "Neutral"),
        new SiteTown("Emerald Sanctuary",       "Felwood",              "Neutral"),
        new SiteTown("Nighthaven",              "Moonglade",            "Neutral"),

        //ARGENT DAWN
        new SiteTown("Light's Hope Chapel",     "Eastern Plaguelands",  "Neutral"),

        //THORIUM BROTHERHOOD
        new SiteTown("Thorium Point",           "Searing Gorge",        "Neutral"),

        //GOBLINS
        new SiteTown("Everlook",                "Winterspring",       	"Neutral"),
        new SiteTown("Gadgetzan",               "Tanaris",       	    "Neutral"),
        new SiteTown("Ratchet",                 "The Barrens",       	"Neutral"),
        new SiteTown("Booty Bay",               "Stranglethorn Valley", "Neutral"),

        //NEUTRAL
        new SiteTown("Marshal's Refuge",        "Un'Goro Crater",       "Neutral"),
        new SiteTown("Timbermaw Hold",          "Felwood",       	    "Neutral"),
    };
}
