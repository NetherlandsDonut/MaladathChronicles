using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Item
{
    //Armour
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, string armorClass, int armor, Stats stats)
    {
        this.ilvl = ilvl;
        this.lvl = lvl;
        this.rarity = rarity;
        this.price = price;
        this.name = name;
        this.icon = icon;
        this.type = type;
        this.armorClass = armorClass;
        this.armor = armor;
        this.stats = stats;
    }

    //Shields
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, int armor, int block, Stats stats)
    {
        this.ilvl = ilvl;
        this.lvl = lvl;
        this.rarity = rarity;
        this.price = price;
        this.name = name;
        this.icon = icon;
        this.type = type;
        this.armor = armor;
        this.block = block;
        this.stats = stats;
    }

    //Jewelry, Relics, Quivers, Pouches and Totems
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, Stats stats)
    {
        this.ilvl = ilvl;
        this.lvl = lvl;
        this.rarity = rarity;
        this.price = price;
        this.name = name;
        this.icon = icon;
        this.type = type;
        this.stats = stats;
    }

    //OffHands
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, string detailedType, Stats stats)
    {
        this.ilvl = ilvl;
        this.lvl = lvl;
        this.rarity = rarity;
        this.price = price;
        this.name = name;
        this.icon = icon;
        this.type = type;
        this.detailedType = detailedType;
        this.stats = stats;
    }

    //Weapons
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, string detailedType, int minDamage, int maxDamage, double speed, Stats stats)
    {
        this.ilvl = ilvl;
        this.lvl = lvl;
        this.rarity = rarity;
        this.price = price;
        this.name = name;
        this.icon = icon;
        this.type = type;
        this.detailedType = detailedType;
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
        this.speed = speed;
        this.stats = stats;
    }

    public string rarity, name, icon, detailedType, type, armorClass;
    public int ilvl, lvl, minDamage, maxDamage, armor, block;
    public double price, speed;
    public Stats stats;

    public bool CanEquip(Entity entity)
    {
        if (armorClass != null)
        {
            return entity.abilities.Contains(armorClass + " Proficiency");
        }
        else
            return true;
    }

    public void Equip(Entity entity, bool secondSlot = false)
    {
        if (type == "TwoHanded")
        {
            var offhand = entity.GetSlot("OffHand");
            if ((detailedType != "Bow" || detailedType != "Crossbow") && offhand != null && offhand.detailedType == "Quiver")
                entity.Unequip(new() { "OffHand" });
            else if (detailedType != "Gun" && offhand != null && offhand.detailedType == "Pouch")
                entity.Unequip(new() { "OffHand" });
            entity.Unequip(new() { "MainHand", "TwoHand" });
            entity.equipment["TwoHand"] = name;
        }
        else if (type == "OffHand")
        {
            entity.Unequip(new() { "OffHand", "TwoHand" });
            entity.equipment["OffHand"] = name;
        }
        else if (type == "OneHanded")
        {
            if (secondSlot)
            {
                entity.Unequip(new() { "OffHand", "TwoHand" });
                entity.equipment["OffHand"] = name;
            }
            else
            {
                entity.Unequip(new() { "MainHand", "TwoHand" });
                entity.equipment["MainHand"] = name;
            }
        }
        else if (type == "Ring")
        {
            if (secondSlot)
            {
                entity.Unequip(new() { "OffRing" });
                entity.equipment["OffRing"] = name;
            }
            else
            {
                entity.Unequip(new() { "MainRing" });
                entity.equipment["MainRing"] = name;
            }
        }
        else if (type == "Trinket")
        {
            if (secondSlot)
            {
                entity.Unequip(new() { "OffTrinket" });
                entity.equipment["OffTrinket"] = name;
            }
            else
            {
                entity.Unequip(new() { "MainTrinket" });
                entity.equipment["MainTrinket"] = name;
            }
        }
        else
        {
            entity.Unequip(new() { type });
            entity.equipment[type] = name;
        }
    }

    public static Item GetItem(string name) => items.Find(x => x.name == name);

    public static List<Item> items = new()
    {
        new Item(73, 60, "Rare", 2.9523,
            "Scaled Silithid Gauntlets",
            "ItemGauntlets10",
            "Hands",
            "Mail",
            266,
            new Stats(new()
            {
                { "Agility", 18 },
                { "Intellect", 8 },
                { "Spirit", 7 },
                { "Stamina", 18 },
            })
        ),
        new Item(61, 56, "Rare", 3.7593,
            "Carapace Spine Crossbow",
            "ItemCrossbow6",
            "Crossbow",
            "TwoHanded",
            82,
            124,
            3.30,
            new Stats(new()
            {
                { "Agility", 4 },
                { "Stamina", 9 },
            })
        ),
        new Item(61, 56, "Rare", 3.5031,
            "Rhombeard Protector",
            "ItemShield14",
            "Shield",
            2089,
            38,
            new Stats(new()
            {
                { "Intellect", 15 },
                { "Spirit", 10 },
            })
        ),
        new Item(60, 55, "Rare", 1.4846,
            "Band of Flesh",
            "ItemBone4",
            "Finger",
            new Stats(new()
            {
                { "Agility", 3 },
                { "Strength", 6 },
                { "Stamina", 16 },
            })
        ),
        new Item(59, 54, "Rare", 1.4368,
            "Ebonsteel Spaulders",
            "ItemShoulder25",
            "Shoulders",
            "Plate",
            553,
            new Stats(new()
            {
                { "Agility", 7 },
                { "Strength", 6 },
                { "Stamina", 16 },
            })
        ),
        new Item(58, 53, "Uncommon", 0.7530,
            "Band of Vigor",
            "ItemRing20",
            "Finger",
            new Stats(new()
            {
                { "Agility", 7 },
                { "Strength", 8 },
                { "Stamina", 7 },
            })
        ),
        new Item(57, 52, "Rare", 1.6581,
            "Haunting Specter Leggings",
            "ItemPants11",
            "Legs",
            "Cloth",
            71,
            new Stats(new()
            {
                { "Intellect", 12 },
                { "Spirit", 28 },
            })
        ),
        new Item(56, 51, "Rare", 4.8882,
            "Angerforge's Battle Axe",
            "ItemHalberd6",
            "TwoHanded",
            "Axe",
            100,
            150,
            2.60,
            new Stats(new()
            {
                { "Strength", 27 },
                { "Stamina", 11 },
            })
        ),
        new Item(55, 50, "Uncommon", 0.8750,
            "Ribbly's Quiver",
            "ItemQuiver6",
            "OffHand",
            "Quiver",
            new Stats(new()
            {
                { "Agility", 5 },
            })
        ),
        new Item(55, 50, "Rare", 1.1738,
            "Rubicund Armguards",
            "ItemBracer13",
            "Wrists",
            "Mail",
            143,
            new Stats(new()
            {
                { "Agility", 8 },
                { "Stamina", 14 },
            })
        ),
        new Item(55, 50, "Rare", 1.4464,
            "Shadefiend Boots",
            "ItemBoots1",
            "Feet",
            "Mail",
            109,
            new Stats(new()
            {
                { "Agility", 11 },
                { "Strength", 10 },
                { "Stamina", 13 },
            })
        ),
        new Item(55, 50, "Rare", 1.5596,
            "Carapace of Anub'shiah",
            "ItemChestPlate8",
            "Chest",
            "Plate",
            577,
            new Stats(new()
            {
                { "Agility", 11 },
                { "Strength", 11 },
                { "Stamina", 22 },
            })
        ),
        new Item(55, 50, "Rare", 3.6752,
            "Grizzle's Skinner\r\n",
            "ItemAxe12",
            "OneHanded",
            "Axe",
            61,
            114,
            2.40,
            new Stats(new()
            {
                { "Agility", 6 },
                { "Strength", 8 },
                { "Stamina", 5 },
            })
        ),
        new Item(53, 48, "Rare", 0.9698,
            "Blackveil Cape",
            "ItemCape18",
            "Shoulders",
            "Plate",
            38,
            new Stats(new()
            {
                { "Agility", 14 },
                { "Strength", 6 },
            })
        ),
        new Item(53, 48, "Rare", 1.0452,
            "Enthralled Sphere",
            "ItemOrb3",
            "OffHand",
            "Orb",
            new Stats(new()
            {
                { "Spirit", 5 },
                { "Stamina", 3 },
            })
        ),
    };
}
