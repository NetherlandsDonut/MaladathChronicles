using System;
using System.Collections.Generic;

using static Root;
using static Race;

public class SiteRaid
{
    public SiteRaid(string name, List<RaidStage> stages)
    {
        this.name = name;
        this.stages = stages;
    }

    public string name;
    public List<RaidStage> stages;

    public List<SiteRaid> raids = new()
    {
        new SiteRaid("Ruins of Ahn'Qiraj", new()
        {
            new RaidStage(3, "Scarab Terrace", new()
            {
                "Qiraji Gladiator",
                "Qiraji Swarmguard",
                "Hive'Zara Stinger",
                "Hive'Zara Wasp"
            }),
            new RaidStage(1, "Scarab Terrace BOSS", new()
            {
                "Kurinnaxx"
            }),
            new RaidStage(4, "General's Terrace", new()
            {
                "Qiraji Gladiator",
                "Qiraji Warrior",
                "Swarmguard Needler"
            }),
            new RaidStage(1, "General's Terrace BOSS", new()
            {
                "General Rajaxx"
            }),
            new RaidStage(3, "Reservoir", new()
            {
                "Flesh Hunter",
                "Obsidian Destroyer"
            }),
            new RaidStage(1, "Reservoir BOSS", new()
            {
                "Moam"
            }),
            new RaidStage(4, "Hatchery", new()
            {
                "Flesh Hunter",
                "Hive'Zara Sandstalker",
                "Hive'Zara Soldier"
            }),
            new RaidStage(1, "Hatchery BOSS", new()
            {
                "Buru The Gorger"
            }),
            new RaidStage(4, "Comb", new()
            {
                "Hive'Zara Collector",
                "Hive'Zara Drone",
                "Hive'Zara Swarmer",
                "Hive'Zara Tail Lasher",
                "Silicate Feeder"
            }),
            new RaidStage(1, "Comb BOSS", new()
            {
                "Ayamiss The Hunter"
            }),
            new RaidStage(2, "Watchers' Terrace", new()
            {
                "Anubisath Guardian"
            }),
            new RaidStage(1, "Watchers' Terrace BOSS", new()
            {
                "Ossirian The Unscarred"
            }),
        })
    };
}

public class RaidStage
{
    public RaidStage(int length, string name, List<string> possibleEncounters)
    {
        this.length = length;
        this.name = name;
        this.possibleEncounters = possibleEncounters;
    }

    public int length;
    public string name;
    public List<string> possibleEncounters;

    public Race RollEncounter() => races.Find(x => x.name == possibleEncounters[random.Next(0, possibleEncounters.Count)]);
}
