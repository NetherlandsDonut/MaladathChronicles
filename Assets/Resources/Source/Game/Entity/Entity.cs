using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class Entity
{
    public Entity(string name, Race race, Class spec, List<string> items)
    {
        ResetResources();
        level = 1;
        this.name = name;
        inventory = new Inventory(items);
        equipment = new Dictionary<string, string>();
        AutoEquip();
        unspentTalentPoints = 20;
        this.race = race.name;
        this.spec = spec.name;
        abilities = race.abilities.Select(x => x).Concat(spec.abilities.FindAll(x => x.Item2 <= level).Select(x => x.Item1)).Concat(spec.talentTrees.SelectMany(x => x.talents.FindAll(y => y.defaultTaken)).Select(x => x.ability)).Distinct().ToList();
        actionBarsUnlocked = 7;
        actionBars = Ability.abilities.FindAll(x => abilities.Contains(x.name) && x.cost != null).Select(x => new ActionBar(x.name)).ToList();
        stats = new Stats(race.stats.stats.ToDictionary(x => x.Key, x => x.Value));
        stats.stats["Stamina"] = 3 * level + 5;
        Initialise();
    }

    public Entity(int level, Race race)
    {
        ResetResources();
        this.level = level;
        this.race = name = race.name;
        abilities = race.abilities.Select(x => x).Distinct().ToList();
        actionBarsUnlocked = 7;
        actionBars = Ability.abilities.FindAll(x => abilities.Contains(x.name) && x.cost != null).Select(x => new ActionBar(x.name)).ToList();
        var importance = ElementImportance(race.rarity == "Common");
        stats = new Stats(
            new()
            {
                { "Stamina", (int)(3 * this.level * race.vitality) + 5 },

                { "Strength", 1 },
                { "Agility", 1 },
                { "Intellect", 1 },

                { "Earth Mastery", 10 },
                { "Fire Mastery", 10 },
                { "Air Mastery", 10 },
                { "Water Mastery", 10 },
                { "Frost Mastery", 10 },
                { "Lightning Mastery", 10 },
                { "Arcane Mastery", 10 },
                { "Decay Mastery", 10 },
                { "Shadow Mastery", 10 },
                { "Order Mastery", 10 },
            }
        );
        Initialise();
    }

    public Dictionary<string, double> ElementImportance(bool randomised)
    {
        var abilities = Ability.abilities.FindAll(x => actionBars.Exists(y => y.ability == x.name));
        double elementCosts = abilities.Sum(x => x.cost.Sum(y => y.Value));
        var sheet = new Dictionary<string, double>();
        foreach (var resource in resources)
        {
            var amount = abilities.FindAll(x => x.cost.ContainsKey(resource.Key)).Sum(x => x.cost[resource.Key]) / elementCosts;
            sheet.Add(resource.Key, (randomised ? Root.random.Next(5, 13) / 10.0 : 1) * amount);
        }
        return sheet;
    }

    public void CapResources()
    {
        var temp = resources.ToArray();
        for (int i = 0; i < temp.Length; i++)
        {
            var resource = temp[i];
            if (resource.Value > MaxResource(resource.Key))
                resources[resource.Key] = MaxResource(resource.Key);
            else if (resource.Value < 0) resources[resource.Key] = 0;
        }
    }

    public int MaxResource(string resource) => stats.stats[resource + " Mastery"] + 3;

    public void ResetResources()
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

    public bool CanPickTalent(int spec, Talent talent)
    {
        if (unspentTalentPoints == 0) return false;
        var talentTree = GetClass().talentTrees[spec];
        if (talent.row > talentTree.talents.FindAll(x => abilities.Contains(x.ability)).Max(x => x.row) + 1) return false;
        if (talent.inherited) if (!abilities.Contains(PreviousTalent(spec, talent).ability)) return false;
        return true;
    }

    public Talent PreviousTalent(int spec, Talent talent)
    {
        var temp = GetClass().talentTrees[spec].talents.OrderByDescending(x => x.row).ToList().FindAll(x => x.col == talent.col);
        return temp.Find(x => x.row < talent.row);
    }

    public Item GetSlot(string slot)
    {
        if (equipment.ContainsKey(slot))
            return Item.GetItem(equipment[slot]);
        else return null;
    }

    public Class GetClass() => Class.classes.Find(x => x.name == spec);

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

    public void AddResource(string resource, int amount) => AddResources(new() { { resource, amount } });

    public void AddResources(Dictionary<string, int> resources)
    {
        foreach (var resource in resources)
            this.resources[resource.Key] += resource.Value;
        CapResources();
    }

    public void DetractResources(Dictionary<string, int> resources)
    {
        foreach (var resource in resources)
            this.resources[resource.Key] -= resource.Value;
        CapResources();
    }

    public void Initialise(bool fullReset = true)
    {
        if (fullReset) { health = MaxHealth(); }
        buffs = new();
        ResetResources();
    }

    public int MaxHealth()
    {
        return stats.stats["Stamina"] * 10;
    }

    public void Cooldown() => actionBars.ForEach(x => x.cooldown -= x.cooldown == 0 ? 0 : 1);

    public void FlareBuffs()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            var index = i;
            Board.board.actions.Add(() => { Root.AddSmallButtonOverlay(buffs[index].Item3, "OtherGlowFull", 1); });
            Board.board.actions.Add(Buff.buffs.Find(y => y.name == buffs[index].Item1).effects(Board.board.enemy == this));
            Board.board.actions.Add(() =>
            {
                buffs[index] = (buffs[index].Item1, buffs[index].Item2 - 1, buffs[index].Item3);
                if (buffs[index].Item2 <= 0)
                {
                    Buff.buffs.Find(y => y.name == buffs[index].Item1).killEffects(Board.board.enemy == this);
                    RemoveBuff(buffs[index]);
                }
            });
        }
    }

    public void AddBuff(string buff, int duration, GameObject buffObject)
    {
        var buffObj = Buff.buffs.Find(x => x.name == buff);
        if (buffs.Exists(x => x.Item1 == buff) && !buffObj.stackable)
        {
            var list = buffs.FindAll(x => x.Item1 == buff).ToList();
            for (int i = list.Count - 1; i >= 0; i--)
                RemoveBuff(list[i]);
        }
        buffs.Add((buff, duration, buffObject));
    }

    public void RemoveBuff((string, int, GameObject) buff)
    {
        var temp = buff.Item3.GetComponent<FlyingBuff>();
        temp.dyingIndex = temp.Index();
        (this == Board.board.player ? Board.board.temporaryBuffsPlayer : Board.board.temporaryBuffsEnemy).Remove(buff.Item3);
        buffs.Remove(buff);
    }

    public int ExperienceNeeded() => (int)(System.Math.Pow(1.04, level + 1) * 100 * (level + 1));
    public int ExperienceNeededOverall()
    {
        var sum = 0;
        for (int i = 1; i < 60; i++)
            sum += (int)(System.Math.Pow(1.04, i + 1) * 100 * (i + 1));
        return sum;
    }

    public int health, level, unspentTalentPoints, actionBarsUnlocked, experience;
    public string name, race, spec;
    public Dictionary<string, int> resources;
    public List<string> abilities;
    public List<ActionBar> actionBars;
    public Stats stats;
    public Inventory inventory;
    public Dictionary<string, string> equipment;
    public List<(string, int, GameObject)> buffs;
}
