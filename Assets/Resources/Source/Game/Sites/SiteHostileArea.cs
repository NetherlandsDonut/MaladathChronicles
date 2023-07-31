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

    public SiteHostileArea(string name, List<(string, string)> possibleEncounters, int length, (string, string) bossEncounter)
    {
        this.name = name;
        this.possibleEncounters = new();
        foreach (var encounter in possibleEncounters)
        {
            var split = encounter.Item1.Split("-");
            this.possibleEncounters.Add((int.Parse(split[0]), int.Parse(split[split.Length == 1 ? 0 : 1]), encounter.Item2));
        }
        this.length = length;
        this.bossEncounter = (int.Parse(bossEncounter.Item1), bossEncounter.Item2);
        recommendedLevel = this.bossEncounter.Item1;
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
        return new Entity(encounters[0].Item1, encounters[0].Item2);
    }
    
    public Entity RollBoss()
    {
        return new Entity(bossEncounter.Item1, Race.races.Find(x => x.name == bossEncounter.Item2));
    }

    public string name;
    public int recommendedLevel, length;
    public List<(int, int, string)> possibleEncounters;
    public (int, string) bossEncounter;

    public static List<SiteHostileArea> hostileAreas = new()
    {
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
            04, ("60", "Kurinnaxx")
        ),
        new SiteHostileArea("General's Terrace", new()
            {
                ("60", "Qiraji Gladiator"),
                ("60", "Qiraji Warrior"),
                ("60", "Swarmguard Needler"),
            },
            04, ("60", "General Rajaxx")
        ),
        new SiteHostileArea("Reservoir", new()
            {
                ("60", "Flesh Hunter"),
                ("60", "Obsidian Destroyer"),
            },
            03, ("60", "Moam")
        ),
        new SiteHostileArea("Hatchery", new()
            {
                ("60", "Flesh Hunter"),
                ("60", "Hive'Zara Sandstalker"),
                ("60", "Hive'Zara Soldier"),
            },
            04, ("60", "Buru The Gorger")
        ),
        new SiteHostileArea("Comb", new()
            {
                ("60", "Hive'Zara Collector"),
                ("60", "Hive'Zara Drone"),
                ("60", "Hive'Zara Swarmer"),
                ("60", "Hive'Zara Tail Lasher"),
                ("60", "Silicate Feeder"),
            },
            06, ("60", "Ayamiss The Hunter")
        ),
        new SiteHostileArea("Watchers' Terrace", new()
            {
                ("60", "Anubisath Guardian")
            },
            02, ("60", "Ossirian The Unscarred")
        ),
    };
}
