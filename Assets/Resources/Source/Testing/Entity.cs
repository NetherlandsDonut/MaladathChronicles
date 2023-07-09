using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Entity
{
    public static Entity player;

    public Entity(Race race, Class spec, string customName)
    {
        this.race = race.name;
        stats = new Stats(race.stats.ToDictionary(x => x.Key, x => x.Value));
        this.spec = spec;
        this.customName = customName;
        SetStartingResources();
    }

    public Entity(Race race)
    {
        this.race = race.name;
        stats = new Stats(race.stats.ToDictionary(x => x.Key, x => x.Value));
        customName = null;
        SetStartingResources();
    }

    public string customName, race;
    public Dictionary<string, int> resources;
    public Stats stats;
    public Class spec;

    public void SetStartingResources()
    {
        resources = new() 
        {
            { "Earth", 0 },
            { "Fire", 0 },
            { "Air", 0 },
            { "Water", 0 },
            { "Frost", 0 },
            { "Lightning", 0 },
            { "Arcane", 0 },
            { "Decay", 0 },
            { "Order", 0 },
            { "Shadow", 0 },
        };
    }
}
