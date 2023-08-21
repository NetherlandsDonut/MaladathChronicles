using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

using static Root;
using System;
using System.Reflection;

public class Entity
{
    //Player constructor
    public Entity(string name, string creationGender, Race race, Class spec, List<string> items)
    {
        level = 60;
        this.name = name;
        gender = creationGender;
        unspentTalentPoints = 20;
        this.race = race.name;
        this.spec = spec.name;
        abilities = race.abilities.Select(x => x).Concat(spec.abilities.FindAll(x => x.Item2 <= level).Select(x => x.Item1)).Concat(spec.talentTrees.SelectMany(x => x.talents.FindAll(y => y.defaultTaken)).Select(x => x.ability)).Distinct().ToList();
        actionBarsUnlocked = 7;
        stats = new Stats(race.stats.stats.ToDictionary(x => x.Key, x => x.Value));
        inventory = new Inventory(items);
        for (int i = 0; i < 40; i++)
        {
            Item item;
            do item = Item.items[random.Next(Item.items.Count)];
            while (!item.CanEquip(this) || item.lvl - level < -5 || item.lvl > level);
            inventory.items.Add(item);
        }
        equipment = new Dictionary<string, Item>();
        EquipAllItems();
        actionBars = Ability.abilities.FindAll(x => abilities.Contains(x.name) && x.cost != null).OrderBy(x => x.cost.Sum(y => y.Value)).OrderBy(x => x.putOnEnd).Select(x => new ActionBar(x.name)).Take(actionBarsUnlocked).ToList();
        Initialise();
    }

    //Enemy constructor
    public Entity(int level, Race race)
    {
        this.level = level;
        race ??= Race.races.Find(x => x.name == "Dumb Kobold");
        this.race = name = race.name;
        abilities = race.abilities.Select(x => x).Distinct().ToList();
        actionBarsUnlocked = 7;
        actionBars = Ability.abilities.FindAll(x => abilities.Contains(x.name) && x.cost != null).OrderBy(x => x.cost.Sum(y => y.Value)).OrderBy(x => x.putOnEnd).Select(x => new ActionBar(x.name)).ToList();
        stats = new Stats(
            new()
            {
                { "Stamina", (int)(5 * this.level * race.vitality) + 5 },
                { "Strength", 3 * this.level },
                { "Agility", 3 * this.level },
                { "Intellect", 3 * this.level },
                { "Spirit", 0 },

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
        var importance = ElementImportance(race.rarity == "Common");
    }

    #region Experience & Leveling

    //Tells whether this entity will get experience from
    //killing an enemy that was at given level
    public bool WillGetExperience(int level)
    {
        return this.level - 5 <= level;
    }

    //Grants experience to this entity
    public void ReceiveExperience(int exp)
    {
        experience += exp;
        if (ExperienceNeeded() <= experience)
        {
            experience -= ExperienceNeeded();
            level += 1;
            PlaySound("DesktopLevelUp");
        }
    }

    //Provides amount of experience needed to level up
    public int ExperienceNeeded()
    {
        return (int)(System.Math.Pow(1.04, level + 1) * 100 * (level + 1));
    }

    //Provides experience needed to reach max level
    public int ExperienceNeededOverall()
    {
        var sum = 0;
        for (int i = 1; i < maxPlayerLevel; i++)
            sum += (int)(System.Math.Pow(1.04, i + 1) * 100 * (i + 1));
        return sum;
    }

    #endregion

    #region Resources

    //Checks whether any resources exceed max or min
    //levels and rounds them to fit in the limit
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

    //Tells max amount of given resource entity can hold at once
    public int MaxResource(string resource)
    {
        return Stats()[resource + " Mastery"] + 3;
    }

    //Gives specific resource in given amount to the entity
    public void AddResource(string resource, int amount)
    {
        AddResources(new() { { resource, amount } });
    }

    //Gives many resources in given amounts to the entity
    public void AddResources(Dictionary<string, int> resources)
    {
        foreach (var resource in resources)
        {
            if (resource.Key == "Decay" && buffs.Exists(x => x.Item1 == "Spore Cloud"))
                Board.board.actions.Add(() => (Board.board.enemy == this ? Board.board.player : Board.board.enemy).Damage(RollWeaponDamage() * (SpellPower() / 10.0 + 1) * 1.1 * resource.Value));
            this.resources[resource.Key] += resource.Value;
        }
        CapResources();
    }

    //Detracts resources in given amounts from the entity
    public void DetractResources(Dictionary<string, int> resources)
    {
        foreach (var resource in resources)
            this.resources[resource.Key] -= resource.Value;
        CapResources();
    }

    //Resets entity's resources to their base amount
    //This value is 0 at default but grows with spirit stat
    //which provides starting resources for combat
    public void ResetResources()
    {
        var stats = Stats();
        resources = new()
        {
            { "Earth", stats["Earth Mastery"] * stats["Spirit"] * stats["Spirit"] / 2048 },
            { "Fire", stats["Fire Mastery"] * stats["Spirit"] * stats["Spirit"] / 2048 },
            { "Air", stats["Air Mastery"] * stats["Spirit"] * stats["Spirit"] / 2048 },
            { "Water", stats["Water Mastery"] * stats["Spirit"] * stats["Spirit"] / 2048 },
            { "Frost", stats["Frost Mastery"] * stats["Spirit"] * stats["Spirit"] / 2048 },
            { "Lightning", stats["Lightning Mastery"] * stats["Spirit"] * stats["Spirit"] / 2048 },
            { "Arcane", stats["Arcane Mastery"] * stats["Spirit"] * stats["Spirit"] / 2048 },
            { "Decay", stats["Decay Mastery"] * stats["Spirit"] * stats["Spirit"] / 2048 },
            { "Order", stats["Order Mastery"] * stats["Spirit"] * stats["Spirit"] / 2048 },
            { "Shadow", stats["Shadow Mastery"] * stats["Spirit"] * stats["Spirit"] / 2048 },
        };
    }

    #endregion

    #region Inventory & Equipment

    //Provides list of abilities gained from equipped items
    public List<string> ItemAbilities()
    {
        var list = equipment.SelectMany(x => x.Value.abilities).ToList();
        var sets = equipment.ToList().FindAll(x => x.Value.set != null).Select(x => ItemSet.itemSets.Find(y => y.name == x.Value.set)).Distinct();
        foreach (var set in sets)
        {
            var temp = set.EquippedPieces(this);
            foreach (var bonus in set.setBonuses)
                if (temp >= bonus.requiredPieces)
                    list.AddRange(bonus.abilitiesProvided);
        }
        return list;
    }

    //Checks whether entity has given item equipped in any slot
    public bool HasItemEquipped(string item)
    {
        return equipment.Any(x => x.Value.name == item);
    }

    //Checks whether given item is filling a currently free spot
    public bool IsItemNewSlot(Item item)
    {
        if (item.type == "Two Handed") return !equipment.ContainsKey("Main Hand") && !equipment.ContainsKey("Off Hand");
        else if (item.type == "One Handed") return abilities.Contains("Dual Wielding Proficiency") && (!equipment.ContainsKey("Main Hand") || equipment["Main Hand"].type != "Two Handed") && !equipment.ContainsKey("Off Hand") || !equipment.ContainsKey("Main Hand");
        else if (item.type == "Off Hand") return (!equipment.ContainsKey("Main Hand") || equipment["Main Hand"].type != "Two Handed") && !equipment.ContainsKey("Off Hand");
        else return !equipment.ContainsKey(item.type);
    }

    //Checks whether given item is an upgrade for the entity
    public bool IsItemAnUpgrade(Item item)
    {
        if (item.type == "Two Handed")
        {
            var item1 = GetItemInSlot("Main Hand");
            var item2 = GetItemInSlot("Off Hand");
            if (item1 != null && item2 != null)
                return item.ilvl > (item1.ilvl + item2.ilvl) / 2;
            else if (item1 != null)
                return item.ilvl > item1.ilvl;
            else
                return true;
        }
        else if (item.type == "One Handed")
        {
            var item1 = GetItemInSlot("Main Hand");
            var item2 = GetItemInSlot("Off Hand");
            if (item1 != null && item1.type == "Two Handed")
                return item.ilvl / 2 > item1.ilvl;
            else
                if (item1 == null || item2 == null)
                return true;
            else if (abilities.Contains("Dual Wielding Proficiency") && item1.ilvl > item2.ilvl)
                return item.ilvl > item2.ilvl;
            else
                return item.ilvl > item1.ilvl;
        }
        else if (item.type == "Off Hand")
        {
            var item1 = GetItemInSlot("Main Hand");
            var item2 = GetItemInSlot("Off Hand");
            if (item1 != null && item2 == null)
                return item1.type == "Two Handed"/* ? item.ilvl / 2 > item1.ilvl : true*/;
            else if (item2 != null)
                return item.ilvl > item2.ilvl;
            else
                return true;
        }
        else
        {
            if (GetItemInSlot(item.type) == null)
            {
                Debug.Log("ERROR 003: wtf " + item.type);
            }
            return item.ilvl > GetItemInSlot(item.type).ilvl;
        }
    }

    //Gets item that is currently equipped in given slot
    public Item GetItemInSlot(string slot)
    {
        if (equipment.ContainsKey(slot))
            return equipment[slot];
        else return null;
    }

    //Automatically equips all items in the
    //inventory that can be equipped by the entity
    public void EquipAllItems()
    {
        for (int i = inventory.items.Count - 1; i >= 0; i--)
            if (inventory.items[i].CanEquip(this))
                inventory.items[i].Equip(this);
    }

    //Unequips items in given list of slots
    public void Unequip(List<string> slots = null, int index = -1)
    {
        if (slots == null) equipment = new();
        else foreach (var slot in slots)
                if (equipment.ContainsKey(slot))
                {
                    var itemAbilities = equipment[slot].abilities;
                    if (itemAbilities != null)
                        foreach (var ability in itemAbilities)
                            abilities.Remove(ability);
                    if (index != -1) inventory.items.Insert(index, equipment[slot]);
                    else inventory.items.Add(equipment[slot]);
                    equipment.Remove(slot);
                }
    }

    #endregion

    #region Talents & Class

    //Checks whether entity can pick specific talent
    public bool CanPickTalent(int spec, Talent talent)
    {
        if (unspentTalentPoints == 0) return false;
        var talentTree = GetClass().talentTrees[spec];
        if (talent.row > talentTree.talents.FindAll(x => abilities.Contains(x.ability)).Max(x => x.row) + 1) return false;
        if (talent.inherited) if (!abilities.Contains(PreviousTalent(spec, talent).ability)) return false;
        return true;
    }

    //Provides a talent that preceeds given talent
    public Talent PreviousTalent(int spec, Talent talent)
    {
        var temp = GetClass().talentTrees[spec].talents.OrderByDescending(x => x.row).ToList().FindAll(x => x.col == talent.col);
        return temp.Find(x => x.row < talent.row);
    }

    //Provides entity's class
    public Class GetClass()
    {
        return Class.classes.Find(x => x.name == spec);
    }

    #endregion

    #region Stats

    public int MaxHealth()
    {
        return Stats()["Stamina"] * 10;
    }

    public (double, double) WeaponDamage()
    {
        if (equipment == null)
        {
            if (level == 60) return (43, 70);
            if (level == 59) return (42, 68);
            if (level == 58) return (41, 67);
            if (level == 57) return (40, 66);
            if (level == 56) return (40, 64);
            if (level == 55) return (39, 63);
            if (level == 54) return (38, 62);
            if (level == 53) return (37, 61);
            if (level == 52) return (36, 59);
            if (level == 51) return (35, 57);
            if (level == 50) return (34, 56);
            if (level == 49) return (33, 55);
            if (level == 48) return (33, 52);
            if (level == 47) return (32, 50);
            if (level == 46) return (31, 48);
            if (level == 45) return (30, 46);
            if (level == 44) return (29, 45);
            if (level == 43) return (29, 44);
            if (level == 42) return (28, 43);
            if (level == 41) return (28, 42);
            if (level == 40) return (27, 41);
            if (level == 39) return (26, 40);
            if (level == 38) return (25, 39);
            if (level == 37) return (24, 38);
            if (level == 36) return (23, 37);
            if (level == 35) return (22, 36);
            if (level == 34) return (22, 35);
            if (level == 33) return (11, 34);
            if (level == 32) return (21, 33);
            if (level == 31) return (20, 32);
            if (level == 30) return (20, 31);
            if (level == 29) return (19, 31);
            if (level == 28) return (18, 30);
            if (level == 27) return (17, 29);
            if (level == 26) return (17, 28);
            if (level == 25) return (16, 27);
            if (level == 24) return (16, 26);
            if (level == 23) return (15, 24);
            if (level == 22) return (15, 23);
            if (level == 21) return (14, 22);
            if (level == 20) return (14, 21);
            if (level == 19) return (13, 20);
            if (level == 18) return (13, 19);
            if (level == 17) return (12, 18);
            if (level == 16) return (12, 17);
            if (level == 15) return (11, 16);
            if (level == 14) return (11, 15);
            if (level == 13) return (10, 14);
            if (level == 12) return (10, 13);
            if (level == 11) return (09, 12);
            if (level == 10) return (09, 11);
            if (level == 09) return (08, 10);
            if (level == 08) return (07, 09);
            if (level == 07) return (06, 08);
            if (level == 06) return (05, 07);
            if (level == 05) return (04, 06);
            if (level == 04) return (03, 05);
            if (level == 03) return (02, 03);
            if (level == 02) return (01, 03);
            else return (01, 02);
        }
        else if (equipment.ContainsKey("Two Handed"))
        {
            var twohanded = equipment["Two Handed"];
            return (twohanded.minDamage / twohanded.speed, twohanded.maxDamage / twohanded.speed);
        }
        else
        {
            double min = 0, max = 0;
            if (equipment.ContainsKey("Main Hand"))
            {
                var mainHand = equipment["Main Hand"];
                min += mainHand.minDamage / mainHand.speed;
                max += mainHand.maxDamage / mainHand.speed;
            }
            if (equipment.ContainsKey("Off Hand"))
            {
                var offHand = equipment["Off Hand"];
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
            foreach (var itemPair in equipment)
                if (itemPair.Value.stats != null)
                    foreach (var stat in itemPair.Value.stats.stats)
                        stats[stat.Key] += stat.Value;
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

    #endregion

    #region Combat

    //Provides information that tells which elements
    //are of biggest importance for this entity in combat
    public Dictionary<string, double> ElementImportance(bool randomised)
    {
        var abilities = Ability.abilities.FindAll(x => actionBars.Exists(y => y.ability == x.name));
        double elementCosts = abilities.Sum(x => x.cost.Sum(y => y.Value));
        var sheet = new Dictionary<string, double>();
        foreach (var resource in resources)
        {
            var amount = abilities.FindAll(x => x.cost.ContainsKey(resource.Key)).Sum(x => x.cost[resource.Key]) / elementCosts;
            sheet.Add(resource.Key, (randomised ? random.Next(5, 13) / 10.0 : 1) * amount);
        }
        return sheet;
    }

    //Prepares this entity for combat
    public void Initialise(bool fullReset = true)
    {
        if (fullReset)
        {
            health = MaxHealth();
        }
        actionBars.ForEach(x => x.cooldown = 0);
        buffs = new();
        ResetResources();
    }

    //Cooldowns all action bar abilities by 1 turn
    public void Cooldown()
    {
        actionBars.ForEach(x => x.cooldown -= x.cooldown == 0 ? 0 : 1);
    }

    //Deals given amount of damage to this entity
    public void Damage(double damage, bool dontPrompt = false)
    {
        var p = Board.board.player == this;
        health -= (int)Math.Ceiling(damage);
        if (dontPrompt) return;
        foreach (var buff in buffs)
            if (buff.Item1 == "Volatile Infection")
            {
                Board.board.actions.Add(() => animationTime += frameTime * 3);
                Board.board.actions.Add(() =>
                {
                    AddSmallButtonOverlay(buff.Item3, "OtherGlowFull", 1);
                    Damage(damage / 10, true);
                    SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityDeathPact", true, !p ? "1000" : "1001");
                    SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityDeathPact", true, !p ? "1000" : "1001");
                    PlaySound("AbilityVenomousBiteFlare");
                    animationTime += frameTime * 3;
                });
            }
    }

    //Heals this entity by given amount
    public void Heal(double heal)
    {
        health += (int)Math.Round(heal);
        if (health > MaxHealth()) health = MaxHealth();
    }

    //Pops all buffs on this entity activating
    //their effects and reducing duration by 1 turn
    //If duration reaches 0 it removes the buff
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

    //Adds a buff to this entity
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

    //Removes a buff from this entity
    public void RemoveBuff((string, int, GameObject) buff)
    {
        var temp = buff.Item3.GetComponent<FlyingBuff>();
        temp.dyingIndex = temp.Index();
        (this == Board.board.player ? Board.board.temporaryBuffsPlayer : Board.board.temporaryBuffsEnemy).Remove(buff.Item3);
        buffs.Remove(buff);
    }

    #endregion

    public int health, level, unspentTalentPoints, actionBarsUnlocked, experience;
    public string name, race, spec, gender;
    public Dictionary<string, int> resources;
    public List<string> abilities;
    public List<ActionBar> actionBars;
    public Stats stats;
    public Inventory inventory;
    public Dictionary<string, Item> equipment;
    public List<(string, int, GameObject)> buffs;
}
