using System.Linq;
using System.Collections.Generic;

public class Entity
{
    public static Entity player;

    public Entity(Race race, Class spec, string name)
    {
        this.name = name;
        this.spec = spec;

        this.race = race.name;
        stats = new Stats(race.stats.stats.ToDictionary(x => x.Key, x => x.Value));
        level = race.level;
        abilities = race.abilities.Select(x => x).Concat(spec.abilities.FindAll(x => x.Item2 <= level).Select(x => x.Item1)).Distinct().ToList();
        Initialise();
    }

    public Entity(Race race)
    {
        this.race = name = race.name;
        stats = new Stats(race.stats.stats.ToDictionary(x => x.Key, x => x.Value));
        level = race.level;
        abilities = race.abilities.Select(x => x).Distinct().ToList();
        Initialise();
    }

    public void Initialise(bool fullReset = true)
    {
        if (fullReset)
        {
            health = MaxHealth();
        }
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

    public int MaxHealth() 
    {
        return stats.stats["Stamina"] * 20;
    }

    public int health, level;
    public string name, race;
    public Dictionary<string, int> resources;
    public List<string> abilities;
    public Stats stats;
    public Class spec;
}
