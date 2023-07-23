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
        new SiteHostileArea("Stonetalon Peak", 23, new()
        {
            ("Nefarian", "Common")
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
