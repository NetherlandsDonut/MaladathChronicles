using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using static Root;
using System.Security.Cryptography;

public class FutureEntity
{
    public FutureEntity(FutureEntity entity)
    {
        health = entity.health;
        name = entity.name;
        spec = entity.spec;
        inventory = entity.inventory;
        equipment = entity.equipment;
        stats = entity.stats;
        actionBars = new();
        foreach (var actionBar in entity.actionBars)
            actionBars.Add(new ActionBar(actionBar.ability, actionBar.cooldown));
        buffs = new();
        foreach (var buff in entity.buffs)
            buffs.Add((buff.Item1, buff.Item2));
        resources = new();
        foreach (var pair in entity.resources)
            resources.Add(pair.Key, pair.Value);
    }

    public FutureEntity(Entity entity)
    {
        health = entity.health;
        name = entity.name;
        spec = entity.spec;
        inventory = entity.inventory;
        equipment = entity.equipment;
        stats = entity.stats;
        actionBars = new();
        foreach (var actionBar in entity.actionBars)
            actionBars.Add(new ActionBar(actionBar.ability, actionBar.cooldown));
        buffs = new();
        foreach (var buff in entity.buffs)
            buffs.Add((buff.Item1, buff.Item2));
        resources = new();
        foreach (var pair in entity.resources)
            resources.Add(pair.Key, pair.Value);
    }

    public int health;
    public string name, spec;
    public Dictionary<string, int> resources;
    public List<ActionBar> actionBars;
    public Stats stats;
    public Inventory inventory;
    public Dictionary<string, string> equipment;
    public List<(string, int)> buffs;

    public Dictionary<string, double> ElementImportance(double healthPerc, double otherHealthPerc)
    {
        var abilities = Ability.abilities.FindAll(x => actionBars.Exists(y => y.ability == x.name));
        double elementCosts = abilities.Sum(x => x.cost.Sum(y => y.Value));
        var sheet = new Dictionary<string, double>();
        foreach (var resource in resources)
        {
            var amount = abilities.FindAll(x => x.cost.ContainsKey(resource.Key)).Sum(x => x.cost[resource.Key] * AbilityTagModifier(x.tags)) / elementCosts + 0.025;
            sheet.Add(resource.Key, Root.random.Next(5, 13) / 10.0 * amount);
        }
        return sheet;
    }

    public Class GetClass() => Class.classes.Find(x => x.name == spec);

    public int MaxHealth()
    {
        return Stats()["Stamina"] * 10;
    }

    public (int, int) WeaponDamage()
    {
        if (equipment.ContainsKey("Two Handed"))
        {
            var twohanded = inventory.items.Find(x => x.name == equipment["Two Handed"]);
            return ((int)(twohanded.minDamage / twohanded.speed), (int)(twohanded.maxDamage / twohanded.speed));
        }
        else
        {
            double min = 0, max = 0;
            if (equipment.ContainsKey("Main Hand"))
            {
                var mainHand = inventory.items.Find(x => x.name == equipment["Main Hand"]);
                min += (int)(mainHand.minDamage / mainHand.speed);
                max += (int)(mainHand.maxDamage / mainHand.speed);
            }
            if (equipment.ContainsKey("Off Hand"))
            {
                var offHand = inventory.items.Find(x => x.name == equipment["Off Hand"]);
                min /= 1.5;
                min /= 1.5;
                min += offHand.minDamage / offHand.speed / 1.5;
                max += offHand.maxDamage / offHand.speed / 1.5;
            }
            return ((int)min, (int)max);
        }
    }

    public Dictionary<string, int> Stats()
    {
        var stats = new Dictionary<string, int>();
        foreach (var stat in this.stats.stats)
            stats.Add(stat.Key, stat.Value);
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

    public int RollWeaponDamage()
    {
        var damage = WeaponDamage();
        if (damage.Item2 == 0) return random.Next(2, 5);
        return random.Next(damage.Item1, damage.Item2 + 1);
    }

    public double MeleeAttackPower()
    {
        var temp = GetClass();
        var sum = temp.rules["Melee Attack Power per Strength"] * Stats()["Strength"];
        sum += temp.rules["Melee Attack Power per Agility"] * Stats()["Agility"];
        return sum;
    }

    public double RangedAttackPower()
    {
        var temp = GetClass();
        var sum = temp.rules["Ranged Attack Power per Agility"] * Stats()["Agility"];
        return sum;
    }

    public double SpellPower()
    {
        var temp = GetClass();
        var sum = temp.rules["Spell Power per Intellect"] * Stats()["Intellect"];
        return sum;
    }

    public double CriticalStrike()
    {
        var temp = GetClass();
        var sum = temp.rules["Critical Strike per Strength"] * Stats()["Strength"];
        sum += temp.rules["Critical Strike per Agility"] * Stats()["Agility"];
        return sum;
    }

    public double SpellCritical()
    {
        var temp = GetClass();
        var sum = temp.rules["Spell Critical per Intellect"] * Stats()["Intellect"];
        return sum;
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

    public double AbilityTagModifier(List<string> tags)
    {
        if (tags.Contains("Damage"))    return 2.00;
        if (tags.Contains("Defensive")) return 1.50;
        if (tags.Contains("Stun"))      return 1.20;
        if (tags.Contains("Healing"))   return 0.85;
        if (tags.Contains("Gathering")) return 0.60;
        return 1.00;
    }

    public double AmountModifier(int n)
    {
        if (n == 0) return 0;
        if (n == 1) return 0.10;
        return 0.30 * Mathf.Abs(n - 1) * (n > 3 ? 3 : 1);
    }

    public void Cooldown()
    {
        foreach (var actionBar in actionBars)
            if (actionBar.cooldown > 0)
                actionBar.cooldown -= 1;
    }

    public void Damage(double damage)
    {
        health -= (int)System.Math.Round(damage);
    }

    public void FlareBuffs(FutureBoard board)
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            var index = i;
            var find = Buff.buffs.Find(y => y.name == buffs[index].Item1);
            find.futureEffects(board.enemy == this, board)();
            buffs[index] = (buffs[index].Item1, buffs[index].Item2 - 1);
            if (buffs[index].Item2 <= 0)
            {
                Buff.buffs.Find(y => y.name == buffs[index].Item1).futureKillEffects(board.enemy == this, board);
                RemoveBuff(buffs[index]);
            }
        }
    }

    public void AddBuff(string buff, int duration)
    {
        var buffObj = Buff.buffs.Find(x => x.name == buff);
        if (buffs.Exists(x => x.Item1 == buff) && !buffObj.stackable)
        {
            var list = buffs.FindAll(x => x.Item1 == buff).ToList();
            for (int i = list.Count - 1; i >= 0; i--)
                RemoveBuff(list[i]);
        }
        buffs.Add((buff, duration));
    }

    public void RemoveBuff((string, int) buff)
    {
        buffs.Remove(buff);
    }
}
