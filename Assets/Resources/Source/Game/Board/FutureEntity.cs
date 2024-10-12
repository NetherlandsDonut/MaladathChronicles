using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;

public class FutureEntity
{
    public FutureEntity(FutureEntity entity)
    {
        level = entity.level;
        health = entity.health;
        name = entity.name;
        spec = entity.spec;
        inventory = entity.inventory;
        equipment = entity.equipment;
        stats = entity.stats;
        dead = entity.dead;
        currentActionSet = entity.currentActionSet;
        actionBars = entity.actionBars.ToDictionary(x => x.Key, x => x.Value.ToList());
        worldBuffs = entity.worldBuffs?.ToList();
        buffs = entity.buffs.Select(x => new CombatBuff(x)).ToList();
        resources = new();
        foreach (var pair in entity.resources)
            resources.Add(pair.Key, pair.Value);
    }

    public FutureEntity(Entity entity)
    {
        level = entity.level;
        health = entity.health;
        name = entity.name;
        spec = entity.spec;
        inventory = entity.inventory;
        equipment = entity.equipment;
        stats = entity.stats;
        dead = entity.dead;
        currentActionSet = entity.currentActionSet;
        actionBars = entity.actionBars.ToDictionary(x => x.Key, x => x.Value.ToList());
        worldBuffs = entity.worldBuffs?.ToList();
        buffs = entity.buffs.Select(x => new CombatBuff(x)).ToList();
        resources = new();
        foreach (var pair in entity.resources)
            resources.Add(pair.Key, pair.Value);
    }

    public int level;

    public string name;

    public string spec;

    public Spec Spec() => global::Spec.specs.Find(x => x.name == spec);

    public string currentActionSet;

    public Dictionary<string, List<string>> actionBars;

    public Stats stats;

    public Inventory inventory;

    public Dictionary<string, Item> equipment;

    public int health;

    public Dictionary<string, int> resources;

    public List<CombatBuff> buffs;

    //List of active world buffs and world debuffs on this entity
    public List<WorldBuff> worldBuffs;

    //Is this entity dead
    public bool dead;

    public Dictionary<string, double> ElementImportance()
    {
        var abilities = Ability.abilities.FindAll(x => actionBars[currentActionSet].Exists(y => y == x.name));
        var elementCostsSeparate = abilities.SelectMany(x => x.cost.ToList()).GroupBy(x => x.Key, x => x.Value).ToDictionary(x => x.Key, x => x.Sum(x => x));
        double elementCosts = elementCostsSeparate.Sum(x => x.Value);
        var sheet = new Dictionary<string, double>();
        foreach (var resource in resources)
        {
            var amount = (abilities.FindAll(x => x.cost.ContainsKey(resource.Key)).Sum(x => x.cost[resource.Key] * AbilityTagModifier(x.tags)) / elementCosts + 0.025) / (elementCostsSeparate.ContainsKey(resource.Key) && elementCostsSeparate[resource.Key] > 0 ? (resource.Value / elementCostsSeparate[resource.Key] + 0.1) : 1.1);
            sheet.Add(resource.Key, random.Next(5, 13) / 10.0 * amount);
        }
        return sheet;
    }

    //Calculates max health for the entity
    public int MaxHealth()
    {
        return Stats()["Stamina"] * 10;
    }

    //Returns range of weapon damage entity has
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
            else             return (01, 02);
        }
        if (equipment.ContainsKey("Two Handed"))
        {
            var twohanded = equipment["Two Handed"];
            return (Math.Round(twohanded.minDamage / twohanded.speed), Math.Round(twohanded.maxDamage / twohanded.speed));
        }
        else
        {
            double min = 0, max = 0;
            if (equipment.ContainsKey("Main Hand"))
            {
                var mainHand = equipment["Main Hand"];
                min += Math.Round(mainHand.minDamage / mainHand.speed);
                max += Math.Round(mainHand.maxDamage / mainHand.speed);
            }
            if (equipment.ContainsKey("Off Hand"))
            {
                var offHand = equipment["Off Hand"];
                min /= 1.5;
                min /= 1.5;
                min += Math.Round(offHand.minDamage / offHand.speed) / 1.5;
                max += Math.Round(offHand.maxDamage / offHand.speed) / 1.5;
            }
            return (Math.Round(min), Math.Round(max));
        }
    }

    public Dictionary<string, int> Stats()
    {
        var stats = new Dictionary<string, int>();
        foreach (var stat in this.stats.stats)
            stats.Add(stat.Key, stat.Value);
        var temp = Spec();
        if (temp != null)
        {
            stats["Stamina"] += (int)(temp.rules["Stamina per Level"] * level);
            stats["Strength"] += (int)(temp.rules["Strength per Level"] * level);
            stats["Agility"] += (int)(temp.rules["Agility per Level"] * level);
            stats["Intellect"] += (int)(temp.rules["Intellect per Level"] * level);
            stats["Spirit"] += (int)(temp.rules["Spirit per Level"] * level);
        }
        if (equipment != null)
            foreach (var itemPair in equipment)
            {
                if (itemPair.Value.stats != null)
                    foreach (var stat in itemPair.Value.stats.stats)
                        stats.Inc(stat.Key, stat.Value);
                if (itemPair.Value.armor > 0)
                    stats.Inc("Armor", itemPair.Value.armor);
            }
        if (worldBuffs != null)
            foreach (var worldBuff in worldBuffs)
                if (worldBuff.Buff.gains != null)
                    foreach (var stat in worldBuff.Buff.gains)
                        stats.Inc(stat.Key, stat.Value);
        if (buffs != null)
            foreach (var buff in buffs)
                if (buff.buff != null && buff.buff.gains != null)
                    foreach (var stat in buff.buff.gains)
                        stats.Inc(stat.Key, stat.Value);
        return stats;
    }

    public double RollWeaponDamage()
    {
        var damage = WeaponDamage();
        return random.Next((int)(damage.Item1 * 100), (int)(damage.Item2 * 100) + 1) / 100.0;
    }
    
    public double MeleeAttackPower()
    {
        var temp = Spec();
        if (temp == null) return Stats()["Strength"] * 2 + Stats()["Agility"] * 2;
        var sum = temp.rules["Melee Attack Power per Strength"] * Stats()["Strength"];
        sum += temp.rules["Melee Attack Power per Agility"] * Stats()["Agility"];
        return sum;
    }

    public double RangedAttackPower()
    {
        var temp = Spec();
        if (temp == null) return Stats()["Agility"] * 3;
        var sum = temp.rules["Ranged Attack Power per Agility"] * Stats()["Agility"];
        return sum;
    }

    public double SpellPower()
    {
        var temp = Spec();
        if (temp == null) return Stats()["Intellect"] * 3;
        var sum = temp.rules["Spell Power per Intellect"] * Stats()["Intellect"];
        return sum;
    }

    public double CriticalStrike()
    {
        var temp = Spec();
        if (temp == null) return Stats()["Agility"] * 0.03;
        var sum = temp.rules["Critical Strike per Strength"] * Stats()["Strength"];
        sum += temp.rules["Critical Strike per Agility"] * Stats()["Agility"];
        return sum;
    }

    public double SpellCritical()
    {
        var temp = Spec();
        if (temp == null) return Stats()["Intellect"] * 0.03;
        var sum = temp.rules["Spell Critical per Intellect"] * Stats()["Intellect"];
        return sum;
    }

    public int MaxResource(string resource) => stats.stats[resource + " Mastery"] + 5;

    public void AddResource(FutureBoard futureBoard, string resource, int amount) => AddResources(futureBoard, new() { { resource, amount } });

    public void AddResources(FutureBoard futureBoard, Dictionary<string, int> resources)
    {
        foreach (var resource in this.resources.Keys.ToList())
            if (resources.ContainsKey(resource) && resources[resource] > 0)
            {
                var before = this.resources[resource];
                this.resources[resource] += resources[resource];
                if (this.resources[resource] > MaxResource(resource))
                    this.resources[resource] = MaxResource(resource);
                foreach (var participant in futureBoard.participants)
                    if (participant.who == this) futureBoard.CallEvents(this, new() { { "Trigger", "ResourceCollected" }, { "Triggerer", "Effector" }, { "ResourceType", resource }, { "ResourceAmount", resources[resource] + "" } });
                    else futureBoard.CallEvents(this, new() { { "Trigger", "ResourceCollected" }, { "Triggerer", "Other" }, { "ResourceType", resource }, { "ResourceAmount", resources[resource] + "" } });
                foreach (var participant in futureBoard.participants)
                    if (participant.who == this) futureBoard.CallEvents(this, new() { { "Trigger", "ResourceMaxed" }, { "Triggerer", "Effector" }, { "ResourceType", resource } });
                    else futureBoard.CallEvents(this, new() { { "Trigger", "ResourceMaxed" }, { "Triggerer", "Other" }, { "ResourceType", resource } });
            }
    }

    public void DetractResource(FutureBoard futureBoard, string resource, int amount) => DetractResources(futureBoard, new() { { resource, amount } });

    public void DetractResources(FutureBoard futureBoard, Dictionary<string, int> resources)
    {
        foreach (var resource in this.resources.Keys.ToList())
            if (resources.ContainsKey(resource) && resources[resource] > 0)
            {
                var before = this.resources[resource];
                this.resources[resource] -= resources[resource];
                if (this.resources[resource] < 0)
                    this.resources[resource] = 0;
                foreach (var participant in futureBoard.participants)
                    if (participant.who == this) futureBoard.CallEvents(this, new() { { "Trigger", "ResourceLost" }, { "Triggerer", "Effector" }, { "ResourceType", resource }, { "ResourceAmount", resources[resource] + "" } });
                    else futureBoard.CallEvents(this, new() { { "Trigger", "ResourceLost" }, { "Triggerer", "Other" }, { "ResourceType", resource }, { "ResourceAmount", resources[resource] + "" } });
                foreach (var participant in futureBoard.participants)
                    if (participant.who == this) futureBoard.CallEvents(this, new() { { "Trigger", "ResourceDeplated" }, { "Triggerer", "Effector" }, { "ResourceType", resource } });
                    else futureBoard.CallEvents(this, new() { { "Trigger", "ResourceDeplated" }, { "Triggerer", "Other" }, { "ResourceType", resource } });
            }
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

    public void Heal(FutureBoard futureBoard, double heal, bool dontCall)
    {
        var before = health;
        health += (int)Math.Round(heal);
        if (health > MaxHealth())
            health = MaxHealth();
        if (!dontCall)
            foreach (var participant in futureBoard.participants)
                if (participant.who == this) futureBoard.CallEvents(participant.who, new() { { "Trigger", "Heal" }, { "Triggerer", "Effector" }, { "HealAmount", heal + "" } });
                else futureBoard.CallEvents(participant.who, new() { { "Trigger", "Heal" }, { "Triggerer", "Other" }, { "HealAmount", heal + "" } });
        if (health == MaxHealth() && before != health)
            foreach (var participant in futureBoard.participants)
                if (participant.who == this) futureBoard.CallEvents(participant.who, new() { { "Trigger", "HealthMaxed" }, { "Triggerer", "Effector" } });
                else futureBoard.CallEvents(participant.who, new() { { "Trigger", "HealthMaxed" }, { "Triggerer", "Other" } });
    }

    public void Damage(FutureBoard futureBoard, double damage, bool dontCall)
    {
        var before = health;
        health -= (int)Math.Ceiling(damage);
        if (!dontCall)
            foreach (var participant in futureBoard.participants)
                if (participant.who == this) futureBoard.CallEvents(participant.who, new() { { "Trigger", "Damage" }, { "Triggerer", "Effector" }, { "DamageAmount", damage + "" } });
                else futureBoard.CallEvents(participant.who, new() { { "Trigger", "Damage" }, { "Triggerer", "Other" }, { "DamageAmount", damage + "" } });
        if (health <= 0 && before > 0)
        {
            foreach (var participant in futureBoard.participants)
                if (participant.who == this) futureBoard.CallEvents(participant.who, new() { { "Trigger", "HealthDeplated" }, { "Triggerer", "Effector" } });
                else futureBoard.CallEvents(participant.who, new() { { "Trigger", "HealthDeplated" }, { "Triggerer", "Other" } });
            if (health <= 0) Die();
        }
    }

    public void Die()
    {
        //Mark this entity as dead
        dead = true;

        //Find all world buffs that player has that aren't death persistant
        var toRemove1 = buffs.Where(x => !x.buff.deathResistant).ToList();

        //Remove not death resistant world buffs from player that just died
        foreach (var buff in toRemove1) RemoveBuff(buff);

        //Find all world buffs that player has that aren't death persistant
        var toRemove2 = worldBuffs.Where(x => !x.Buff.deathResistant);

        //Remove not death resistant world buffs from player that just died
        foreach (var buff in toRemove2) RemoveWorldBuff(buff);
    }

    public void FlareBuffs(FutureBoard futureBoard)
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            var index = i;
            if (buffs[index].durationLeft == 1)
            {
                foreach (var participant in futureBoard.participants)
                    if (participant.who == this) futureBoard.CallEvents(participant.who, new() { { "Trigger", "BuffRemove" }, { "Triggerer", "Effector" }, { "BuffName", buffs[index].buff.name } });
                    else futureBoard.CallEvents(participant.who, new() { { "Trigger", "BuffRemove" }, { "Triggerer", "Other" }, { "BuffName", buffs[index].buff.name } });
            }
            if (--buffs[index].durationLeft <= 0) RemoveBuff(buffs[index]);
        }
    }

    //Adds a buff to this future entity
    public void AddBuff(Buff buff, int duration, int rank)
    {
        if (!buff.stackable)
        {
            var list = buffs.FindAll(x => x.buff == buff).ToList();
            for (int i = list.Count - 1; i >= 0; i--)
                RemoveBuff(list[i]);
        }
        buffs.Add(new CombatBuff(buff, duration, null, rank));
    }

    //Removes a world buff from this future entity
    public void RemoveWorldBuff(WorldBuff worldBuff) => worldBuffs.Remove(worldBuff);

    //Removes a buff from this future entity
    public void RemoveBuff(CombatBuff buff) => buffs.Remove(buff);
}
