using System.Collections.Generic;

public class Item
{
    //Lootbox
    public Item(string rarity, string name, string icon, string type, List<string> possibleItems, List<string> alternateItems)
    {
        this.rarity = rarity;
        this.name = name;
        this.icon = icon;
        this.type = type;
        this.possibleItems = possibleItems;
        this.alternateItems = alternateItems;
    }

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

    //Capes
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, int armor, Stats stats)
    {
        this.ilvl = ilvl;
        this.lvl = lvl;
        this.rarity = rarity;
        this.price = price;
        this.name = name;
        this.icon = icon;
        this.type = type;
        this.armor = armor;
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
    public List<string> possibleItems, alternateItems;
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
            entity.Unequip(new() { "OffHand", "MainHand" });
            entity.equipment["TwoHand"] = name;
        }
        else if (type == "OffHand")
        {
            var mainHand = entity.GetSlot("MainHand");
            if (mainHand != null && mainHand.type == "TwoHanded")
                entity.Unequip(new() { "MainHand" });
            entity.Unequip(new() { "OffHand" });
            entity.equipment["OffHand"] = name;
        }
        else if (type == "OneHanded")
        {
            if (secondSlot)
            {
                var mainHand = entity.GetSlot("MainHand");
                if (mainHand != null && mainHand.type == "TwoHanded")
                    entity.Unequip(new() { "MainHand" });
                entity.Unequip(new() { "OffHand" });
                entity.equipment["OffHand"] = name;
            }
            else
            {
                entity.Unequip(new() { "MainHand" });
                entity.equipment["MainHand"] = name;
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
        new Item("Rare",
            "Ukorz Sandscalp Loot",
            "ItemBag15",
            "LootBox",
            new List<string>
            {
                "Big Bad Pauldrons",
                "Ripsaw",
                "The Chief\'s Enforcer",
                "Embrace of The Lycan",
                "Jang\'Thraze",
            },
            new List<string>
            {
                "Tracker's Headband",
                "Tracker's Leggins",
                "Tracker's Shoulderpads",
                "Brigade Breastplate",
                "Brigade Leggins",
                "Warmonger's Belt",
                "Warmonger's Cloak",
                "Warmonger's Gauntlets",
                "Cabalist Helm",
                "Cabalist Spaulders",
                "Cabalist Belt",
                "Cabalist Gloves",
                "Cabalist Boots",
                "Royal Headband",
                "Royal Trousers",
                "Royal Amice",
                "Regal Robe",
                "Regal Armor",
                "Chieftain's Cloak",
                "Embossed Plate Armor",
                "Gossamer Headpiece",
                "Gossamer Shoulderpads",
                "Gossamer Pants",
                "Gossamer Belt",
                "Gossamer Gloves",
                "Gossamer Boots",
                "Shriveled Heart",
                "Champion's Helmet",
                "Champion's Pauldrons",
                "Champion's Greaves",
                "Champion's Girdle",
                "Champion's Gauntlets",
                "Gothic Plate Helmet",
                "Gothic Plate Spaulders",
                "Gothic Plate Gauntlets",
                "Gothic Plate Girdle",
                "Gothic Plate Leggins",
                "Gothic Sabatons",
                "Heraldic Cloak",
            }
        ),
        new Item("Rare",
            "Witch Doctor Zum'rah Loot",
            "ItemBag15",
            "LootBox",
            new List<string>
            {
                "Jumanza Grips",
                "Zum'Rah's Vexing Cane",
            },
            new List<string>
            {
                "Tracker's Headband",
                "Tracker's Leggins",
                "Tracker's Shoulderpads",
                "Brigade Breastplate",
                "Brigade Leggins",
                "Warmonger's Belt",
                "Warmonger's Cloak",
                "Warmonger's Gauntlets",
                "Cabalist Helm",
                "Cabalist Spaulders",
                "Cabalist Belt",
                "Cabalist Gloves",
                "Cabalist Boots",
                "Royal Headband",
                "Royal Trousers",
                "Royal Amice",
                "Regal Robe",
                "Regal Armor",
                "Chieftain's Cloak",
                "Embossed Plate Armor",
                "Gossamer Headpiece",
                "Gossamer Shoulderpads",
                "Gossamer Pants",
                "Gossamer Belt",
                "Gossamer Gloves",
                "Gossamer Boots",
                "Shriveled Heart",
                "Champion's Helmet",
                "Champion's Pauldrons",
                "Champion's Greaves",
                "Champion's Girdle",
                "Champion's Gauntlets",
                "Gothic Plate Helmet",
                "Gothic Plate Spaulders",
                "Gothic Plate Gauntlets",
                "Gothic Plate Girdle",
                "Gothic Plate Leggins",
                "Gothic Sabatons",
                "Heraldic Cloak",
            }
        ),
        new Item(83, 60, "Epic", 5.8442,
            "Mish'undare, Circlet of the Mind Flayer",
            "ItemHelmet52",
            "Head",
            "Cloth",
            102,
            new Stats(new()
            {
                { "Stamina", 15 },
                { "Intellect", 24 },
                { "Spirit", 9 },
            })
            //Equip: Increases damage and healing done by magical spells and effects by up to 35.
            //Equip: Improves your chance to get a critical strike with spells by 2%.
        ),
        new Item(83, 60, "Epic", 5.9115,
            "Cloak of the Brood Lord",
            "ItemCape20",
            "Back",
            63,
            new Stats(new()
            {
                { "Stamina", 10 },
                { "Intellect", 14 },
            })
            //Equip: Increases damage and healing done by magical spells and effects by up to 28.
        ),
        new Item(83, 60, "Epic", 10.5328,
            "Prestor's Talisman of Connivery",
            "ItemNecklace17",
            "Neck",
            new Stats(new()
            {
                { "Agility", 30 },
            })
            //Equip: Improves your chance to hit by 1%.
        ),
        new Item(77, 60, "Epic", 14.8714,
            "Perdition's Blade",
            "ItemShortblade11",
            "OneHanded",
            "Dagger",
            73, 137,
            1.80,
            new Stats(new()
            {

            })
            //Chance on hit: Blasts a target for 40 to 56 Fire damage.
        ),
        new Item(76, 60, "Epic", 4.5577,
            "Mantle of Phrenic Power",
            "ItemShoulder2",
            "Shoulders",
            "Cloth",
            87,
            new Stats(new()
            {
                { "Stamina", 20 },
                { "Intellect", 20 },
            })
            //Equip: Increases damage done by Fire spells and effects by up to 33.
        ),
        new Item(73, 60, "Epic", 4.5577,
            "Mantle of the Blackwing Cabal",
            "ItemShoulder25",
            "Shoulders",
            "Cloth",
            84,
            new Stats(new()
            {
                { "Stamina", 12 },
                { "Intellect", 16 },
            })
            //Equip: Increases damage and healing done by magical spells and effects by up to 34.
        ),
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
        new Item(70, 60, "Epic", 13.8646,
            "Perdition's Blade",
            "ItemStaff13",
            "TwoHanded",
            "Staff",
            126, 205,
            2.90,
            new Stats(new()
            {
                { "Stamina", 16 },
                { "Intellect", 37 },
                { "Spirit", 14 },
            })
            //Equip: Improves your chance to get a critical strike with spells by 1%.
            //Equip: Increases damage and healing done by magical spells and effects by up to 40.
        ),
        new Item(71, 60, "Epic", 2.2280,
            "Mana Igniting Cord",
            "ItemBelt11",
            "Waist",
            "Cloth",
            61,
            new Stats(new()
            {
                { "Stamina", 12 },
                { "Intellect", 16 },
            })
            //Equip: Increases damage and healing done by magical spells and effects by up to 25.
            //Equip: Improves your chance to get a critical strike with spells by 1%.
        ),
        new Item(70, 60, "Epic", 13.5594,
            "Hyperthermically Insulated Lava Dredger",
            "ItemGizmo2",
            "TwoHanded",
            "Mace",
            155, 234,
            2.90,
            new Stats(new()
            {
                { "Stamina", 25 },
                { "Intellect", 24 },
                { "Fire Mastery", 2 },
            })
            //Equip: Restores 9 mana per 5 sec.
        ),
        new Item(65, 60, "Epic", 8.1895,
            "Fang of Venoxis",
            "ItemShortblade31",
            "OneHanded",
            "Dagger",
            35, 72,
            1.30,
            new Stats(new()
            {
                { "Intellect", 8 },
                { "Spirit", 6 },
            })
            //Equip: Increases damage and healing done by magical spells and effects by up to 24.
            //Equip: Restores 6 mana per 5 sec.
        ),
        new Item(61, 56, "Rare", 3.7593,
            "Carapace Spine Crossbow",
            "ItemCrossbow6",
            "Crossbow",
            "TwoHanded",
            82, 124,
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
            "Special",
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
