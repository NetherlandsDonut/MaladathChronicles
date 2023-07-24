using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FutureEntity
{
    public FutureEntity(Entity entity)
    {
        health = entity.health;
        name = entity.name;
        inventory = entity.inventory;
        equipment = entity.equipment;
        stats = entity.stats;
        actionBars = entity.actionBars;
        buffs = new();
        foreach (var buff in entity.buffs)
            buffs.Add((buff.Item1, buff.Item2));
        resources = new();
        foreach (var pair in entity.resources)
            resources.Add(pair.Key, pair.Value);
    }

    public int health;
    public string name;
    public Dictionary<string, int> resources;
    public List<string> actionBars;
    public Stats stats;
    public Inventory inventory;
    public Dictionary<string, string> equipment;
    public List<(string, int)> buffs;

    public Dictionary<string, double> ElementImportance(double healthPerc, double otherHealthPerc)
    {
        var abilities = Ability.abilities.FindAll(x => actionBars.Contains(x.name));
        double elementCosts = abilities.Sum(x => x.cost.Sum(y => y.Value));
        var sheet = new Dictionary<string, double>();
        foreach (var resource in resources)
            sheet.Add(resource.Key, abilities.FindAll(x => x.cost.ContainsKey(resource.Key)).Sum(x => x.cost[resource.Key] * AbilityTagModifier(x.tags)) / elementCosts + 0.025);
        return sheet;
    }

    public void DetractResources(Dictionary<string, int> resources)
    {
        foreach (var resource in resources)
            this.resources[resource.Key] -= resource.Value;
    }

    public double AbilityTagModifier(List<string> tags)
    {
        if (tags.Contains("Damage")) return 2.00;
        if (tags.Contains("Defensive")) return 1.50;
        if (tags.Contains("Healing")) return 0.75;
        if (tags.Contains("Stun")) return 1.20;
        return 1.00;
    }

    public double AmountModifier(int n)
    {
        if (n == 0) return 0;
        if (n == 1) return 0.10;
        return 0.30 * Mathf.Abs(n - 1);
    }

    public int MaxHealth()
    {
        return stats.stats["Stamina"] * 20;
    }

    public void FlareBuffs(FutureBoard board)
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            var index = i;
            board.actions.Add(Buff.buffs.Find(y => y.name == buffs[index].Item1).futureEffects(board.enemy == this, board));
            board.actions.Add(() =>
            {
                buffs[index] = (buffs[index].Item1, buffs[index].Item2 - 1);
                if (buffs[index].Item2 <= 0)
                {
                    Buff.buffs.Find(y => y.name == buffs[index].Item1).futureKillEffects(board.enemy == this, board);
                    RemoveBuff(buffs[index]);
                }
            });
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
