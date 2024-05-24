using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using static Root;
using static Race;
using static Spec;
using static Sound;
using static Defines;

public class Entity
{
    public Entity() { }

    public Entity(string name, string creationGender, Race race, Spec spec, List<string> items)
    {
        level = 1;
        gender = creationGender;
        unspentTalentPoints = level - 1;
        this.race = race.name;
        if (name != "") this.name = name;
        else this.name = gender == "Female" ? race.femaleNames[random.Next(race.femaleNames.Count)] : race.maleNames[random.Next(race.maleNames.Count)];
        this.spec = spec.name;
        faction = race.faction;
        abilities = race.abilities.Merge(spec.abilities).Merge(spec.talentTrees.SelectMany(x => x.talents.FindAll(y => y.defaultTaken)).ToDictionary(x => x.ability, x => 0));
        stats = new Stats(race.stats.stats.ToDictionary(x => x.Key, x => x.Value));
        mounts = new();
        inventory = new Inventory(items);
        learnedRecipes = new();
        professionSkills = new();
        reputation = new();
        var side = Side();
        foreach (var faction in Faction.factions)
            if (faction.name == this.faction)
                reputation.Add(faction.name, 4600);
            else if (faction.side == side)
                reputation.Add(faction.name, 4250);
            else if (faction.side != side && faction.side != "Neutral")
                reputation.Add(faction.name, 0);
        inventory.items.RemoveAll(x => x == null);
        for (int i = 0; i < 0; i++)
        {
            Item item;
            do item = Item.items[random.Next(Item.items.Count)].CopyItem();
            while (!item.CanEquip(this) || item.lvl - level < -5 || item.lvl > level);
            inventory.items.Add(item);
        }
        equipment = new Dictionary<string, Item>();
        EquipAllItems();
        actionBars = Ability.abilities.FindAll(x => abilities.ContainsKey(x.name) && x.cost != null).OrderBy(x => x.cost.Sum(y => y.Value)).OrderBy(x => x.putOnEnd).Select(x => x.name).Take(ActionBarsAmount()).ToList();
        InitialiseCombat();
    }

    public Entity(int level, Race race)
    {
        this.level = level;
        race ??= races.Find(x => x.name == "Dumb Kobold");
        kind = race.kind;
        this.race = name = race.name;
        abilities = race.abilities.ToDictionary(x => x.Key, x => x.Value);
        actionBars = Ability.abilities.FindAll(x => abilities.ContainsKey(x.name) && x.cost != null).OrderBy(x => x.cost.Sum(y => y.Value)).OrderBy(x => x.putOnEnd).Select(x => x.name).ToList();
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
        InitialiseCombat();
    }

    #region Experience & Leveling

    //Level of this entity
    public int level;

    //Current amount of experience this unit has
    public int experience;

    //Tells whether this entity will get experience from
    //killing an enemy that was at given level
    public bool WillGetExperience(int level)
    {
        return this.level - 5 <= level;
    }

    //Tells whether this entity will get experience from
    //killing an enemy that was at given level
    public int ActionBarsAmount()
    {
        return level >= 30 ? 7 : (level >= 20 ? 6 : (level >= 10 ? 5 : 4));
    }

    //Grants experience to this entity
    public void ReceiveExperience(int exp)
    {
        experience += exp;
        while (ExperienceNeeded() <= experience)
        {
            experience -= ExperienceNeeded();
            level++;
            unspentTalentPoints++;
            PlaySound("DesktopLevelUp", 0.3f);
        }
        Respawn("ExperienceBarBorder", true);
        Respawn("ExperienceBar", true);
    }

    //Provides amount of experience needed to level up
    public int ExperienceNeeded() => level switch
    {
        01 => 400,
        02 => 900,
        03 => 1400,
        04 => 2100,
        05 => 2800,
        06 => 3600,
        07 => 4500,
        08 => 5400,
        09 => 6500,
        10 => 7600,
        11 => 8800,
        12 => 10100,
        13 => 11400,
        14 => 12900,
        15 => 14400,
        16 => 16000,
        17 => 17700,
        18 => 19400,
        19 => 21300,
        20 => 23200,
        21 => 25200,
        22 => 27300,
        23 => 29400,
        24 => 31700,
        25 => 34000,
        26 => 36400,
        27 => 38900,
        28 => 41400,
        29 => 44300,
        30 => 47400,
        31 => 50800,
        32 => 54500,
        33 => 58600,
        34 => 62800,
        35 => 67100,
        36 => 71600,
        37 => 76100,
        38 => 80800,
        39 => 85700,
        40 => 90700,
        41 => 95800,
        42 => 101000,
        43 => 106300,
        44 => 111800,
        45 => 117500,
        46 => 123200,
        47 => 129100,
        48 => 135100,
        49 => 141200,
        50 => 147500,
        51 => 153900,
        52 => 160400,
        53 => 167100,
        54 => 173900,
        55 => 180800,
        56 => 187900,
        57 => 195000,
        58 => 202300,
        59 => 209800,
        _ => 0,
    };

    //Provides amount of experience needed to level up
    public int ExperienceForEqualEnemy() => level switch
    {
        01 => 133,
        02 => 300,
        03 => 466,
        04 => 700,
        05 => 933,
        06 => 1200,
        07 => 1500,
        08 => 1800,
        09 => 2166,
        10 => 1900,
        11 => 2200,
        12 => 2525,
        13 => 2850,
        14 => 3225,
        15 => 3600,
        16 => 4000,
        17 => 4425,
        18 => 4850,
        19 => 5325,
        20 => 4640,
        21 => 5040,
        22 => 5460,
        23 => 5880,
        24 => 6340,
        25 => 6800,
        26 => 7280,
        27 => 7780,
        28 => 8280,
        29 => 8860,
        30 => 7900,
        31 => 8466,
        32 => 9083,
        33 => 9766,
        34 => 10466,
        35 => 11183,
        36 => 11933,
        37 => 12683,
        38 => 13466,
        39 => 14283,
        40 => 12957,
        41 => 13685,
        42 => 14428,
        43 => 15185,
        44 => 15971,
        45 => 16785,
        46 => 17600,
        47 => 18442,
        48 => 19300,
        49 => 20171,
        50 => 18437,
        51 => 19237,
        52 => 20171,
        53 => 20887,
        54 => 21737,
        55 => 22600,
        56 => 23375,
        57 => 24375,
        58 => 25287,
        59 => 26225,
        _ => 0,
    };

    #endregion

    #region Resources

    //Tells max amount of given resource entity can hold at once
    public int MaxResource(string resource)
    {
        return Stats()[resource + " Mastery"] + 5;
    }

    //Gives specific resource in given amount to the entity
    public void AddResource(string resource, int amount)
    {
        AddResources(new() { { resource, amount } });
    }

    //Gives many resources in given amounts to the entity
    public void AddResources(Dictionary<string, int> resources)
    {
        foreach (var resource in this.resources.Keys.ToList())
        {
            if (resources.ContainsKey(resource))
            {
                this.resources[resource] += resources[resource];
                Board.board.CallEvents(this, new() { { "Trigger", "ResourceCollected" }, { "Triggerer", "Effector" }, { "ResourceType", resource }, { "ResourceAmount", resources[resource] + "" } });
                Board.board.CallEvents(this == Board.board.player ? Board.board.enemy : Board.board.player, new() { { "Trigger", "ResourceCollected" }, { "Triggerer", "Other" }, { "ResourceType", resource }, { "ResourceAmount", resources[resource] + "" } });
            }
            if (this.resources[resource] > MaxResource(resource))
                this.resources[resource] = MaxResource(resource);
            else if (this.resources[resource] < 0)
                this.resources[resource] = 0;
        }
    }

    //Detracts specific resource in given amount from the entity
    public void DetractResource(string resource, int amount)
    {
        DetractResources(new() { { resource, amount } });
    }

    //Detracts resources in given amounts from the entity
    public void DetractResources(Dictionary<string, int> resources)
    {
        foreach (var resource in this.resources.Keys.ToList())
        {
            if (resources.ContainsKey(resource))
            {
                this.resources[resource] -= resources[resource];
                Board.board.CallEvents(this, new() { { "Trigger", "ResourceLost" }, { "Triggerer", "Effector" }, { "ResourceType", resource }, { "ResourceAmount", resources[resource] + "" } });
                Board.board.CallEvents(this == Board.board.player ? Board.board.enemy : Board.board.player, new() { { "Trigger", "ResourceLost" }, { "Triggerer", "Other" }, { "ResourceType", resource }, { "ResourceAmount", resources[resource] + "" } });
            }
            if (this.resources[resource] > MaxResource(resource))
                this.resources[resource] = MaxResource(resource);
            else if (this.resources[resource] < 0)
                this.resources[resource] = 0;
        }
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
    public Dictionary<string, int> ItemAbilities()
    {
        var list = equipment.SelectMany(x => x.Value.abilities).ToDictionary(x => x.Key, x => x.Value);
        var sets = equipment.ToList().FindAll(x => x.Value.set != null).Select(x => ItemSet.itemSets.Find(y => y.name == x.Value.set)).Distinct();
        foreach (var set in sets)
        {
            var temp = set.EquippedPieces(this);
            foreach (var bonus in set.setBonuses)
                if (temp >= bonus.requiredPieces)
                    list = list.Merge(bonus.abilitiesProvided);
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
        else if (item.type == "One Handed") return abilities.ContainsKey("Dual Wielding Proficiency") && (!equipment.ContainsKey("Main Hand") || equipment["Main Hand"].type != "Two Handed") && !equipment.ContainsKey("Off Hand") || !equipment.ContainsKey("Main Hand");
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
            else if (abilities.ContainsKey("Dual Wielding Proficiency") && item1.ilvl > item2.ilvl)
                return item.ilvl > item2.ilvl;
            else
                return item.ilvl > item1.ilvl;
        }
        else if (item.type == "Off Hand")
        {
            var item1 = GetItemInSlot("Main Hand");
            var item2 = GetItemInSlot("Off Hand");
            var temp = inventory.items.OrderByDescending(x => x.ilvl);
            var item3 = temp.Count() > 0 ? temp.First(x => x.CanEquip(this) && x.type == "One Handed") : null;
            if (item1 != null && item2 == null)
                return item1.type == "Two Handed" && item3 != null ? item.ilvl / 2 + item3.ilvl / 2 > item1.ilvl : false;
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
                            abilities.Remove(ability.Key);
                    if (index != -1) inventory.items.Insert(index, equipment[slot]);
                    else inventory.items.Add(equipment[slot]);
                    equipment.Remove(slot);
                }
    }

    //Unequips a bag in given index
    public void UnequipBag(int index = 0)
    {
        if (inventory.bags.Count <= index) return;
        var itemAbilities = inventory.bags[index].abilities;
        if (itemAbilities != null)
            foreach (var ability in itemAbilities)
                abilities.Remove(ability.Key);
        PlaySound(inventory.bags[index].ItemSound("PutDown"), 0.6f);
        inventory.items.Add(inventory.bags[index]);
        inventory.bags.RemoveAt(index);
    }

    #endregion

    #region Reputation

    public string ReputationRank(string faction)
    {
        int amount = Reputation(faction);
        if (amount >= 8400) return "Exalted";
        else if (amount >= 6300) return "Revered";
        else if (amount >= 5100) return "Honored";
        else if (amount >= 4500) return "Friendly";
        else if (amount >= 4200) return "Neutral";
        else if (amount >= 3900) return "Unfriendly";
        else if (amount >= 3600) return "Hostile";
        else return "Hated";
    }

    public int Reputation(string faction) => reputation.ContainsKey(faction) ? reputation[faction] : defines.defaultStanding;

    public Dictionary<string, int> reputation;

    #endregion

    #region Professions

    //List of all professions skill
    public Dictionary<string, (int, List<string>)> professionSkills;

    //List of all learned recipes grouped by profession
    public Dictionary<string, List<string>> learnedRecipes;

    //Learns a recipe
    public void LearnRecipe(Recipe recipe)
    {
        if (!learnedRecipes.ContainsKey(recipe.profession))
            learnedRecipes.Add(recipe.profession, new());
        if (!learnedRecipes[recipe.profession].Contains(recipe.name))
            learnedRecipes[recipe.profession].Add(recipe.name);
    }

    //Learns a recipe
    public void LearnRecipe(string profession, string recipe)
    {
        if (!learnedRecipes.ContainsKey(profession))
            learnedRecipes.Add(profession, new());
        if (!learnedRecipes[profession].Contains(recipe))
            learnedRecipes[profession].Add(recipe);
    }

    //Checks if the player can craft a recipe taking into
    //consideration empty space in the player inventory and reagents required
    public bool CanCraft(Recipe recipe)
    {
        var can = true;
        foreach (var reagent in recipe.reagents)
            if (inventory.items.Sum(x => x.name == reagent.Key ? x.amount : 0) < reagent.Value)
                can = false;
        if (!inventory.CanAddItems(recipe.results.Select(x => Item.items.Find(y => x.Key == y.name).CopyItem(x.Value)).ToList()))
            can = false;
        return can;
    }

    //Crafts a recipe and gives player all the resulting benefits
    public List<Item> Craft(Recipe recipe)
    {
        foreach (var reagent in recipe.reagents)
        {
            int left = reagent.Value;
            var items = inventory.items.FindAll(x => x.name == reagent.Key);
            for (int i = items.Count - 1; i >= 0 && left > 0; i--)
            {
                var temp = items[i].amount;
                items[i].amount -= items[i].amount >= left ? left : items[i].amount;
                if (items[i].amount <= 0)
                    inventory.items.Remove(items[i]);
                left -= temp;
            }
        }
        var crafted = recipe.results.Select(x => Item.items.Find(y => x.Key == y.name).CopyItem(x.Value)).ToList();
        return crafted;
    }

    #endregion

    #region Talents & Spec

    //Amount of currently unspent talent points for this entity
    public int unspentTalentPoints;

    //Checks whether entity can pick specific talent
    public bool CanPickTalent(int spec, Talent talent)
    {
        if (abilities.ContainsKey(talent.ability) && abilities[talent.ability] >= Ability.abilities.Find(x => x.name == talent.ability).ranks.Count - 1) return false;
        if (talent.tree == 1 && TreeCompletion(spec, 0) < defines.adeptTreeRequirement) return false;
        var talentTree = Spec().talentTrees[spec];
        var temp = talentTree.talents.FindAll(x => x.tree == talent.tree && abilities.ContainsKey(x.ability));
        if (talent.row > (temp.Count > 0 ? temp.Max(x => x.row) + 1 : 0)) return false;
        if (talent.inherited)
        {
            var foo = Ability.abilities.Find(x => x.name == PreviousTalent(spec, talent).ability).ranks.Count;
            if (!abilities.ContainsKey(PreviousTalent(spec, talent).ability) || abilities[PreviousTalent(spec, talent).ability] + 1 < (foo < 1 ? 1 : foo)) return false;
        }
        return true;
    }

    //Provides a talent that preceeds given talent
    public Talent PreviousTalent(int spec, Talent talent)
    {
        var temp = Spec().talentTrees[spec].talents.OrderByDescending(x => x.row).ToList().FindAll(x => x.col == talent.col && x.tree == talent.tree);
        return temp.Find(x => x.row < talent.row);
    }

    //Provides a talent that preceeds given talent
    public int TreeSize(int spec, int tree)
    {
        var temp = Spec().talentTrees[spec].talents.FindAll(x => x.tree == tree);
        return temp.Sum(x => Ability.abilities.Find(y => y.name == x.ability).ranks.Count);
    }

    //Provides a talent that preceeds given talent
    public int TreeCompletion(int spec, int tree)
    {
        var temp = Spec().talentTrees[spec].talents.FindAll(x => x.tree == tree).Select(x => x.ability);
        return abilities.Sum(x => temp.Contains(x.Key) ? x.Value + 1 : 0);
    }

    #endregion

    #region Stats

    //02:42 - rutheran to auberdine by ship
    //02:10 - stormwind to goldshire on land

    public int TravelPassTime() => Speed() switch
    {
        01 => 70,
        02 => 56,
        03 => 42,
        04 => 29,
        05 => 14,
        06 => 12,
        07 => 10,
        08 => 09,
        09 => 08,
        10 => 07,
        11 => 06,
        12 => 06,
        13 => 06,
        14 => 05,
         _ => 04
    };

    public int Speed()
    {
        var mount = Mount.mounts.Find(x => x.name == this.mount);
        return mount == null ? 5 : mount.speed;
    }

    public int MaxHealth()
    {
        return Stats()["Stamina"] * 5;
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
                if (offHand.maxDamage > 0)
                {
                    min /= 1.5;
                    min /= 1.5;
                    min += offHand.minDamage / offHand.speed / 1.5;
                    max += offHand.maxDamage / offHand.speed / 1.5;
                }
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
                if (itemPair.Value.stats != null)
                    foreach (var stat in itemPair.Value.stats.stats)
                        stats[stat.Key] += stat.Value;
        return stats;
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

    #endregion

    #region Combat

    //Prepares this entity for combat
    public void InitialiseCombat(bool fullReset = true)
    {
        if (fullReset)
            health = MaxHealth();
        buffs = new();
        ResetResources();
    }

    //Prepares this entity for fishing
    public void InitialiseFishing()
    {
        buffs = new();
        ResetResources();
    }

    //Deals given amount of damage to this entity
    public void Damage(int damage, bool dontCall)
    {
        var before = health;
        health -= damage;
        if (!dontCall)
        {
            Board.board.CallEvents(this, new() { { "Trigger", "Damage" }, { "Triggerer", "Effector" }, { "DamageAmount", damage + "" } });
            Board.board.CallEvents(this == Board.board.player ? Board.board.enemy : Board.board.player, new() { { "Trigger", "Damage" }, { "Triggerer", "Other" }, { "DamageAmount", damage + "" } });
        }
        if (health <= 0 && before > 0)
        {
            Board.board.CallEvents(this, new() { { "Trigger", "HealthDeplated" }, { "Triggerer", "Effector" } });
            Board.board.CallEvents(this == Board.board.player ? Board.board.enemy : Board.board.player, new() { { "Trigger", "HealthDeplated" }, { "Triggerer", "Other" } });
        }
    }

    //Heals this entity by given amount
    public void Heal(int heal, bool dontCall)
    {
        var before = health;
        health += heal;
        if (health > MaxHealth())
            health = MaxHealth();
        if (!dontCall)
        {
            Board.board.CallEvents(this, new() { { "Trigger", "Heal" }, { "Triggerer", "Effector" }, { "HealAmount", heal + "" } });
            Board.board.CallEvents(this == Board.board.player ? Board.board.enemy : Board.board.player, new() { { "Trigger", "Heal" }, { "Triggerer", "Other" }, { "HealAmount", heal + "" } });
        }
        if (health == MaxHealth() && before != health)
        {
            Board.board.CallEvents(this, new() { { "Trigger", "HealthMaxed" }, { "Triggerer", "Effector" } });
            Board.board.CallEvents(this == Board.board.player ? Board.board.enemy : Board.board.player, new() { { "Trigger", "HealthMaxed" }, { "Triggerer", "Other" } });
        }
    }

    public Dictionary<Ability, int> AbilitiesInCombat()
    {
        var temp = Ability.abilities.FindAll(x => abilities.ContainsKey(x.name) && (x.cost == null || actionBars.Contains(x.name)));
        return temp.ToDictionary(x => x, x => abilities[x.name]);
    }

    //Pops all buffs on this entity activating
    //their effects and reducing duration by 1 turn
    //If duration reaches 0 it removes the buff
    public void FlareBuffs()
    {
        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            var index = i;
            if (buffs[index].Item2 == 1)
            {
                Board.board.CallEvents(this, new() { { "Trigger", "BuffRemove" }, {"Triggerer", "Effector" }, { "BuffName", buffs[index].Item1.name } });
                Board.board.CallEvents(Board.board.player == this ? Board.board.enemy : Board.board.player, new() { { "Trigger", "BuffRemove" }, {"Triggerer", "Other" }, { "BuffName", buffs[index].Item1.name } });
            }
            Board.board.actions.Add(() =>
            {
                buffs[index] = (buffs[index].Item1, buffs[index].Item2 - 1, buffs[index].Item3, buffs[index].Item4);
                if (buffs[index].Item2 <= 0) RemoveBuff(buffs[index]);
            });
        }
    }

    //Adds a buff to this entity
    public void AddBuff(Buff buff, int duration, GameObject buffObject, int rank)
    {
        if (!buff.stackable)
        {
            var list = buffs.FindAll(x => x.Item1 == buff).ToList();
            for (int i = list.Count - 1; i >= 0; i--) RemoveBuff(list[i]);
        }
        buffs.Add((buff, duration, buffObject, rank));
    }

    //Removes a buff from this entity
    public void RemoveBuff((Buff, int, GameObject, int) buff)
    {
        var temp = buff.Item3.GetComponent<FlyingBuff>();
        temp.dyingIndex = temp.Index();
        (this == Board.board.player ? Board.board.temporaryBuffsPlayer : Board.board.temporaryBuffsEnemy).Remove(buff.Item3);
        buffs.Remove(buff);
    }

    #endregion

    #region Faction

    //Faction of the entity
    public string faction;

    //Which side of the conflict is this entity on
    public string Side() => Faction.factions.Find(x => x.name == faction).side;

    #endregion

    #region World Events

    //public List<WorldBuff> worldBuffs;

    public Dictionary<string, int> worldCooldowns;

    #endregion

    #region PVP

    //Amount of honor this character has
    public int currentHonor;

    //Amount of honor this character has
    public int honor;

    //Current PVP rank of this entity
    public PVPRank Rank() => PVPRank.pvpRanks.OrderBy(x => x.rank).Last(x => x.honorRequired <= honor);

    #endregion

    #region Enemy Exclusives

    //This variable is only for enemies
    //and can have three different values:
    //Common, Rare and Elite
    public string kind;

    #endregion

    //Name of the entity
    public string name;

    //Race of the entity
    public string race;
    public Race Race()
    {
        if (race == null) return null;
        return races.Find(x => x.name == race);
    }

    //The class that this entity is representing
    public string spec;
    public Spec Spec()
    {
        if (spec == null) return null;
        return specs.Find(x => x.name == spec);
    }

    //Gender of this entity
    public string gender;

    //Set hearthstone home location
    public string homeLocation;

    //List of abilities that this entity has access to
    //This can be abilities from items, class or race
    public Dictionary<string, int> abilities;

    //Set action bars in spellbook
    public List<string> actionBars;

    //"Naked" stats of the entity
    public Stats stats;

    //Inventory of the entity storing currency and items
    public Inventory inventory;

    //Current mount equipped
    public string mount;

    //All of the mounts that this entity possesses
    public List<string> mounts;

    //Currently equipped items
    //Equipped items are not present in the inventory!
    public Dictionary<string, Item> equipment;

    //Current health of the entity
    [NonSerialized] public int health;

    //Stores information about resources of the entity in combat
    [NonSerialized] public Dictionary<string, int> resources;

    //List of active buffs and debuffs on this entity
    [NonSerialized] public List<(Buff, int, GameObject, int)> buffs;
}
