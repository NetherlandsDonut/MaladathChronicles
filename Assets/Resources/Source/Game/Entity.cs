using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class Entity
{
    public Entity(string name, Race race, Class spec, List<string> items)
    {
        this.name = name;
        inventory = new Inventory(items);
        equipment = new Dictionary<string, string>();
        AutoEquip();

        unspentTalentPoints = 20;
        this.race = race.name;
        this.spec = spec.name;
        stats = new Stats(race.stats.stats.ToDictionary(x => x.Key, x => x.Value));
        level = race.level;
        abilities = race.abilities.Select(x => x).Concat(spec.abilities.FindAll(x => x.Item2 <= level).Select(x => x.Item1)).Concat(spec.talentTrees.SelectMany(x => x.talents.FindAll(y => y.defaultTaken)).Select(x => x.ability)).Distinct().ToList();
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

    public void DetractResources(Dictionary<string, int> resources)
    {
        foreach (var resource in resources)
            this.resources[resource.Key] -= resource.Value;
    }

    public void Initialise(bool fullReset = true)
    {
        if (fullReset)
        {
            health = MaxHealth();
        }
        actionBars = Ability.abilities.FindAll(x => abilities.Contains(x.name) && x.cost != null).Select(x => x.name).ToList();
        buffs = new();
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
                    var temp = buffs[index].Item3.GetComponent<FlyingBuff>();
                    temp.dyingIndex = temp.Index();
                    (this == Board.board.player ? Board.board.temporaryBuffsPlayer : Board.board.temporaryBuffsEnemy).Remove(buffs[index].Item3);
                    buffs.RemoveAt(index);
                }
            });
        }
    }

    public int health, level, unspentTalentPoints;
    public string name, race, spec;
    public Dictionary<string, int> resources;
    public List<string> abilities, actionBars;
    public Stats stats;
    public Inventory inventory;
    public Dictionary<string, string> equipment;
    public List<(string, int, GameObject)> buffs;
}
