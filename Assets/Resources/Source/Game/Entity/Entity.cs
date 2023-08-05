using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

using static Root;

public class Entity
{
    public Entity(string name, Race race, Class spec, List<string> items)
    {
        ResetResources();
        level = 56;
        this.name = name;
        unspentTalentPoints = 20;
        this.race = race.name;
        this.spec = spec.name;
        abilities = race.abilities.Select(x => x).Concat(spec.abilities.FindAll(x => x.Item2 <= level).Select(x => x.Item1)).Concat(spec.talentTrees.SelectMany(x => x.talents.FindAll(y => y.defaultTaken)).Select(x => x.ability)).Distinct().ToList();
        actionBarsUnlocked = 7;
        stats = new Stats(race.stats.stats.ToDictionary(x => x.Key, x => x.Value));
        inventory = new Inventory(items);
        equipment = new Dictionary<string, string>();
        AutoEquip();
        Initialise();
    }

    public Entity(int level, Race race)
    {
        ResetResources();
        this.level = level;
        this.race = name = race.name;
        abilities = race.abilities.Select(x => x).Distinct().ToList();
        actionBarsUnlocked = 7;
        actionBars = Ability.abilities.FindAll(x => abilities.Contains(x.name) && x.cost != null).OrderBy(x => x.cost.Sum(y => y.Value)).OrderBy(x => x.putOnEnd).Select(x => new ActionBar(x.name)).ToList();
        var importance = ElementImportance(race.rarity == "Common");
        stats = new Stats(
            new()
            {
                { "Stamina", (int)(5 * this.level * race.vitality) + 5 },
                { "Strength", 3 * this.level },
                { "Agility", 3 * this.level },
                { "Intellect", 3 * this.level },

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

    public List<string> ItemAbilities() => equipment.SelectMany(x => Item.items.Find(y => y.name == x.Value).abilities).ToList();

    public bool HasItemEquipped(string item) => equipment.Any(x => x.Value == item);

    public int MaxResource(string resource) => Stats()[resource + " Mastery"] + 3;

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
                {
                    var itemAbilities = Item.items.Find(x => x.name == equipment[slot]).abilities;
                    if (itemAbilities != null)
                        foreach (var ability in itemAbilities)
                            abilities.Remove(ability);
                    equipment.Remove(slot);
                }
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
        return Stats()["Stamina"] * 10;
    }

    public (double, double) WeaponDamage()
    {
        if (equipment == null) return (1, 2);
        else if (equipment.ContainsKey("Two Handed"))
        {
            var twohanded = inventory.items.Find(x => x.name == equipment["Two Handed"]);
            return (twohanded.minDamage / twohanded.speed, twohanded.maxDamage / twohanded.speed);
        }
        else
        {
            double min = 0, max = 0;
            if (equipment.ContainsKey("Main Hand"))
            {
                var mainHand = inventory.items.Find(x => x.name == equipment["Main Hand"]);
                min += mainHand.minDamage / mainHand.speed;
                max += mainHand.maxDamage / mainHand.speed;
            }
            if (equipment.ContainsKey("Off Hand"))
            {
                var offHand = inventory.items.Find(x => x.name == equipment["Off Hand"]);
                min /= 1.5;
                min /= 1.5;
                min += offHand.minDamage / offHand.speed / 1.5;
                max += offHand.maxDamage / offHand.speed / 1.5;
            }
            return (min, max);
        }
    }

    public double RollWeaponDamage()
    {
        var damage = WeaponDamage();
        if (damage.Item2 == 0) return random.Next(2, 5);
        return random.Next((int)(damage.Item1 * 100), (int)(damage.Item2 * 100) + 1) / 100.0;
    }

    public Dictionary<string, int> Stats()
    {
        var stats = new Dictionary<string, int>();
        foreach (var stat in this.stats.stats)
            stats.Add(stat.Key, stat.Value);
        var temp = GetClass();
        if (temp != null)
        {
            stats["Stamina"] += (int)(temp.rules["Stamina per Level"] * level);
            stats["Strength"] += (int)(temp.rules["Strength per Level"] * level);
            stats["Agility"] += (int)(temp.rules["Agility per Level"] * level);
            stats["Intellect"] += (int)(temp.rules["Intellect per Level"] * level);
        }
        if (equipment != null)
        {
            var itemsEquipped = new List<Item>();
            foreach (var item in equipment)
                itemsEquipped.Add(inventory.items.Find(x => x.name == item.Value));
            foreach (var item in itemsEquipped)
                foreach (var stat in item.stats.stats)
                    stats[stat.Key] += stat.Value;
        }
        return stats;
    }

    public double MeleeAttackPower()
    {
        var temp = GetClass();
        if (temp == null) return Stats()["Strength"] * 2 + Stats()["Agility"] * 2;
        var sum = temp.rules["Melee Attack Power per Strength"] * Stats()["Strength"];
        sum += temp.rules["Melee Attack Power per Agility"] * Stats()["Agility"];
        return sum;
    }

    public double RangedAttackPower()
    {
        var temp = GetClass();
        if (temp == null) return Stats()["Agility"] * 3;
        var sum = temp.rules["Ranged Attack Power per Agility"] * Stats()["Agility"];
        return sum;
    }

    public double SpellPower()
    {
        var temp = GetClass();
        if (temp == null) return Stats()["Intellect"] * 3;
        var sum = temp.rules["Spell Power per Intellect"] * Stats()["Intellect"];
        return sum;
    }

    public double CriticalStrike()
    {
        var temp = GetClass();
        if (temp == null) return Stats()["Agility"] * 0.03;
        var sum = temp.rules["Critical Strike per Strength"] * Stats()["Strength"];
        sum += temp.rules["Critical Strike per Agility"] * Stats()["Agility"];
        return sum;
    }

    public double SpellCritical()
    {
        var temp = GetClass();
        if (temp == null) return Stats()["Intellect"] * 0.03;
        var sum = temp.rules["Spell Critical per Intellect"] * Stats()["Intellect"];
        return sum;
    }

    public void Cooldown() => actionBars.ForEach(x => x.cooldown -= x.cooldown == 0 ? 0 : 1);

    public void Damage(double damage)
    {
        health -= (int)System.Math.Round(damage);
    }

    public void Heal(double heal)
    {
        health += (int)System.Math.Round(heal);
        if (health > MaxHealth()) health = MaxHealth();
    }

    public void FlareBuffs()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            var index = i;
            Board.board.actions.Add(() => { AddSmallButtonOverlay(buffs[index].Item3, "OtherGlowFull", 1); });
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
