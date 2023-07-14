using System.Linq;
using System.Collections.Generic;
using UnityEditor;

public class Entity
{
    public Entity(string name, Race race, Class spec, List<string> items)
    {
        this.name = name;
        this.spec = spec;
        inventory = new Inventory(items);
        equipment = new Dictionary<string, string>();
        AutoEquip();

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

    public Item GetSlot(string slot)
    {
        if (equipment.ContainsKey(slot))
            return Item.GetItem(equipment[slot]);
        else return null;
    }

    public void AutoEquip()
    {
        foreach (var item in inventory.items)
            item.Equip(this);
    }

    public void Unequip(List<string> slots = null)
    {
        if (slots == null) equipment = new();
        else foreach (var slot in slots)
            if (equipment.ContainsKey(slot))
                equipment.Remove(slot);
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

    public int health, level, inventorySpace;
    public string name, race;
    public Dictionary<string, int> resources;
    public List<string> abilities;
    public Stats stats;
    public Inventory inventory;
    public Dictionary<string, string> equipment;
    public Class spec;
}
