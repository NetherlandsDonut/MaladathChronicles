using System.Collections.Generic;
using System.Linq;

public class Item
{
    public Item() { }

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
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, string armorClass, int armor, Stats stats = null, List<string> abilities = null)
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
        this.abilities = abilities;
    }

    //Shields
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, int armor, int block, Stats stats = null, List<string> abilities = null)
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
        this.abilities = abilities;
    }

    //Jewelry, Relics, Quivers, Pouches and Totems
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, Stats stats = null, List<string> abilities = null)
    {
        this.ilvl = ilvl;
        this.lvl = lvl;
        this.rarity = rarity;
        this.price = price;
        this.name = name;
        this.icon = icon;
        this.type = type;
        this.stats = stats;
        this.abilities = abilities;
    }

    //Capes
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, int armor, Stats stats = null, List<string> abilities = null)
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
        this.abilities = abilities;
    }

    //OffHands
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, string detailedType, Stats stats = null, List<string> abilities = null)
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
        this.abilities = abilities;
    }

    //Weapons
    public Item(int ilvl, int lvl, string rarity, double price, string name, string icon, string type, string detailedType, int minDamage, int maxDamage, double speed, Stats stats = null, List<string> abilities = null)
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
        this.abilities = abilities;
    }

    public string rarity, name, icon, detailedType, type, armorClass;
    public int ilvl, lvl, minDamage, maxDamage, armor, block;
    public List<string> possibleItems, alternateItems, abilities;
    public double price, speed;
    public Stats stats;

    public static Dictionary<string, Root.Color> rarityColors = new()
    {
        { "Poor", Root.Color.Poor },
        { "Common", Root.Color.Common },
        { "Uncommon", Root.Color.Uncommon },
        { "Rare", Root.Color.Rare },
        { "Epic", Root.Color.Epic },
        { "Legendary", Root.Color.Legendary },
    };
    
    public bool CanEquip(Entity entity)
    {
        if (armorClass != null)
            return entity.abilities.Contains(armorClass + " Proficiency");
        else
            return true;
    }

    private void Equip(Entity entity, string slot)
    {
        entity.equipment[slot] = name;
        if (abilities == null) return;
        entity.abilities.AddRange(abilities);
        entity.abilities = entity.abilities.Distinct().ToList();
    }

    public void Equip(Entity entity, bool secondSlot = false)
    {
        if (type == "Two Handed")
        {
            entity.Unequip(new() { "Off Hand", "Main Hand" });
            Equip(entity, "Main Hand");
        }
        else if (type == "Off Hand")
        {
            var mainHand = entity.GetSlot("Main Hand");
            if (mainHand != null && mainHand.type == "Two Handed")
                entity.Unequip(new() { "Main Hand" });
            entity.Unequip(new() { "Off Hand" });
            Equip(entity, "Off Hand");
        }
        else if (type == "One Handed")
        {
            if (secondSlot)
            {
                var mainHand = entity.GetSlot("Main Hand");
                if (mainHand != null && mainHand.type == "Two Handed")
                    entity.Unequip(new() { "Main Hand" });
                entity.Unequip(new() { "Off Hand" });
                Equip(entity, "Off Hand");
            }
            else
            {
                entity.Unequip(new() { "Main Hand" });
                Equip(entity, "Main Hand");
            }
        }
        else
        {
            entity.Unequip(new() { type });
            Equip(entity, type);
        }
    }

    public static Item GetItem(string name) => items.Find(x => x.name == name);

    public static List<Item> items;

    //public static List<Item> items = new()
    //{
    //    new Item("Rare",
    //        "Ukorz Sandscalp Loot",
    //        "ItemBag15",
    //        "LootBox",
    //        new List<string>
    //        {
    //            "Big Bad Pauldrons",
    //            "Ripsaw",
    //            "The Chief\'s Enforcer",
    //            "Embrace of The Lycan",
    //            "Jang\'Thraze",
    //        },
    //        new List<string>
    //        {
    //            "Tracker's Headband",
    //            "Tracker's Leggins",
    //            "Tracker's Shoulderpads",
    //            "Brigade Breastplate",
    //            "Brigade Leggins",
    //            "Warmonger's Belt",
    //            "Warmonger's Cloak",
    //            "Warmonger's Gauntlets",
    //            "Cabalist Helm",
    //            "Cabalist Spaulders",
    //            "Cabalist Belt",
    //            "Cabalist Gloves",
    //            "Cabalist Boots",
    //            "Royal Headband",
    //            "Royal Trousers",
    //            "Royal Amice",
    //            "Regal Robe",
    //            "Regal Armor",
    //            "Chieftain's Cloak",
    //            "Embossed Plate Armor",
    //            "Gossamer Headpiece",
    //            "Gossamer Shoulderpads",
    //            "Gossamer Pants",
    //            "Gossamer Belt",
    //            "Gossamer Gloves",
    //            "Gossamer Boots",
    //            "Shriveled Heart",
    //            "Champion's Helmet",
    //            "Champion's Pauldrons",
    //            "Champion's Greaves",
    //            "Champion's Girdle",
    //            "Champion's Gauntlets",
    //            "Gothic Plate Helmet",
    //            "Gothic Plate Spaulders",
    //            "Gothic Plate Gauntlets",
    //            "Gothic Plate Girdle",
    //            "Gothic Plate Leggins",
    //            "Gothic Sabatons",
    //            "Heraldic Cloak",
    //        }
    //    ),
    //    new Item("Rare",
    //        "Witch Doctor Zum'rah Loot",
    //        "ItemBag15",
    //        "LootBox",
    //        new List<string>
    //        {
    //            "Jumanza Grips",
    //            "Zum'Rah's Vexing Cane",
    //        },
    //        new List<string>
    //        {
    //            "Tracker's Headband",
    //            "Tracker's Leggins",
    //            "Tracker's Shoulderpads",
    //            "Brigade Breastplate",
    //            "Brigade Leggins",
    //            "Warmonger's Belt",
    //            "Warmonger's Cloak",
    //            "Warmonger's Gauntlets",
    //            "Cabalist Helm",
    //            "Cabalist Spaulders",
    //            "Cabalist Belt",
    //            "Cabalist Gloves",
    //            "Cabalist Boots",
    //            "Royal Headband",
    //            "Royal Trousers",
    //            "Royal Amice",
    //            "Regal Robe",
    //            "Regal Armor",
    //            "Chieftain's Cloak",
    //            "Embossed Plate Armor",
    //            "Gossamer Headpiece",
    //            "Gossamer Shoulderpads",
    //            "Gossamer Pants",
    //            "Gossamer Belt",
    //            "Gossamer Gloves",
    //            "Gossamer Boots",
    //            "Shriveled Heart",
    //            "Champion's Helmet",
    //            "Champion's Pauldrons",
    //            "Champion's Greaves",
    //            "Champion's Girdle",
    //            "Champion's Gauntlets",
    //            "Gothic Plate Helmet",
    //            "Gothic Plate Spaulders",
    //            "Gothic Plate Gauntlets",
    //            "Gothic Plate Girdle",
    //            "Gothic Plate Leggins",
    //            "Gothic Sabatons",
    //            "Heraldic Cloak",
    //        }
    //    )
    //};
}
