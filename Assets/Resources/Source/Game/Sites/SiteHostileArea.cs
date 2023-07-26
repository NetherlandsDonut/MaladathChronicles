using System.Collections.Generic;

public class SiteHostileArea
{
    public SiteHostileArea(string name, int recommendedLevel, List<(string, string)> possibleEncounters)
    {
        this.name = name;
        this.recommendedLevel = recommendedLevel;
        this.possibleEncounters = possibleEncounters;
    }

    public SiteHostileArea(string name, int recommendedLevel, List<(string, string)> possibleEncounters, int length, string bossEncounter)
    {
        this.name = name;
        this.recommendedLevel = recommendedLevel;
        this.possibleEncounters = possibleEncounters;
        this.length = length;
        this.bossEncounter = bossEncounter;
    }

    public string name, bossEncounter;
    public int recommendedLevel, length;
    public List<(string, string)> possibleEncounters;

    public static List<SiteHostileArea> hostileAreas = new()
    {
        new SiteHostileArea("Corin's Crossing", 54, new()
        {
            ("Scourge Warder", "Common"),
            ("Dark Summoner", "Common"),
        }),
        new SiteHostileArea("Blackwood Lake", 54, new()
        {
            ("Plaguehound", "Common"),
            ("Noxious Plaguebat", "Common"),
        }),
        new SiteHostileArea("Lake Mereldar", 54, new()
        {
            ("Blighted Surge", "Common"),
            ("Plague Ravager", "Common"),
        }),
        new SiteHostileArea("Pestilent Scar", 54, new()
        {
            ("Living Decay", "Common"),
            ("Plaguehound", "Common"),
            ("Noxious Plaguebat", "Common"),
            ("Rotting Sludge", "Common"),
        }),
        new SiteHostileArea("Plaguewood", 54, new()
        {
            ("Scourge Warder", "Common"),
            ("Putrid Gargoyle", "Common"),
            ("Necromancer", "Common"),
            ("Cursed Mage", "Common"),
            ("Cannibal Ghoul", "Common"),
            ("Death Cultist", "Common"),
        }),
        new SiteHostileArea("Terrordale", 53, new()
        {
            ("Cursed Mage", "Common"),
            ("Cannibal Ghoul", "Common"),
            ("Scourge Soldier", "Common"),
            ("Crypt Fiend", "Common"),
            ("Torn Screamer", "Common"),
        }),
        new SiteHostileArea("Terrorweb Tunnel", 55, new()
        {
            ("Crypt Fiend", "Common"),
            ("Crypt Walker", "Common"),
        }),
        new SiteHostileArea("Darrowshire", 53, new()
        {
            ("Plaguehound Runt", "Common"),
            ("Scourge Soldier", "Common"),
        }),
        new SiteHostileArea("Thondroril River", 53, new()
        {
            ("Plaguehound Runt", "Common"),
            ("Plaguebat", "Common"),
        }),
        new SiteHostileArea("Tyr's Hand", 53, new()
        {
            ("Scarlet Curate", "Common"),
            ("Scarlet Warder", "Common"),
            ("Scarlet Enchanter", "Common"),
            ("Scarlet Cleric", "Common"),
        }),
        new SiteHostileArea("The Marris Stead", 53, new()
        {
            ("Putrid Gargoyle", "Common"),
            ("Plaguebat", "Common"),
            ("Plaguehound Runt", "Common"),
        }),








        new SiteHostileArea("Stonetalon Peak", 23, new()
        {
            ("Nefarian", "Common"),
        }),
        new SiteHostileArea("Scarab Terrace", 60, new()
        {
            ("Qiraji Gladiator", "Common"),
            ("Qiraji Swarmguard", "Common"),
            ("Hive'Zara Stinger", "Common"),
            ("Hive'Zara Wasp", "Common"),
        }, 4, "Kurinnaxx"),
        new SiteHostileArea("General's Terrace", 60, new()
        {
            ("Qiraji Gladiator", "Common"),
            ("Qiraji Warrior", "Common"),
            ("Swarmguard Needler", "Common"),
        }, 4, "General Rajaxx"),
        new SiteHostileArea("Reservoir", 60, new()
        {
            ("Flesh Hunter", "Common"),
            ("Obsidian Destroyer", "Common"),
        }, 3, "Moam"),
        new SiteHostileArea("Hatchery", 60, new()
        {
            ("Flesh Hunter", "Common"),
            ("Hive'Zara Sandstalker", "Common"),
            ("Hive'Zara Soldier", "Common"),
        }, 3, "Buru The Gorger"),
        new SiteHostileArea("Comb", 60, new()
        {
            ("Hive'Zara Collector", "Common"),
            ("Hive'Zara Drone", "Common"),
            ("Hive'Zara Swarmer", "Common"),
            ("Hive'Zara Tail Lasher", "Common"),
            ("Silicate Feeder", "Common"),
        }, 6, "Ayamiss The Hunter"),
        new SiteHostileArea("Watchers' Terrace", 60, new()
        {
            ("Anubisath Guardian", "Common")
        }, 2, "Ossirian The Unscarred"),
    };

    //Common
    //Uncommon
    //Rare
    //Boss
}
