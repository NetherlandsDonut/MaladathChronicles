using System.Linq;
using System.Collections.Generic;

public class Item
{
    public string rarity, name, icon, detailedType, type, armorClass, set;
    public int ilvl, lvl, minDamage, maxDamage, armor, block;
    public List<string> possibleItems, alternateItems, abilities, classes;
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

    public string ItemSound(string soundType)
    {
        string result;
        if (detailedType == "Staff") result = "WoodLarge";
        else if (detailedType == "Wand") result = "Wand";
        else if (detailedType == "Totem") result = "WoodLarge";
        else if (detailedType == "Libram") result = "Ring";
        else if (detailedType == "Idol") result = "Ring";
        else if (detailedType == "Quiver") result = "ClothLeather";
        else if (detailedType == "Shield") result = "MetalLarge";
        else if (type == "Back") result = "ClothLeather";
        else if (type == "Neck") result = "Ring";
        else if (type == "Finger") result = "Ring";
        else if (type == "Trinket") result = "Ring";
        else if (type == "Off Hand") result = "Book";
        else if (type == "One Handed") result = "MetalSmall";
        else if (type == "Two Handed") result = "MetalLarge";
        else if (armorClass == "Cloth") result = "ClothLeather";
        else if (armorClass == "Leather") result = "ClothLeather";
        else if (armorClass == "Mail") result = "ChainLarge";
        else if (armorClass == "Plate") result = "MetalLarge";
        else result = "ClothLeather";
        return soundType + result;
    }

    public bool CanEquip(Entity entity)
    {
        if (type == "Miscellaneous")
            return false;
        if (classes != null && !classes.Contains(entity.spec))
            return false;
        if (armorClass != null)
            return entity.abilities.Contains(armorClass + " Proficiency");
        else if (type == "Pouch")
            return entity.abilities.Contains("Pouch Proficiency");
        else if (type == "Quiver")
            return entity.abilities.Contains("Quiver Proficiency");
        else if (type == "Libram")
            return entity.abilities.Contains("Libram Proficiency");
        else if (type == "Totem")
            return entity.abilities.Contains("Totem Proficiency");
        else if (type == "Idol")
            return entity.abilities.Contains("Idol Proficiency");
        else if (type == "Two Handed")
        {
            if (detailedType == "Sword")
                return entity.abilities.Contains("Two Handed Sword Proficiency");
            else if (detailedType == "Axe")
                return entity.abilities.Contains("Two Handed Axe Proficiency");
            else if (detailedType == "Mace")
                return entity.abilities.Contains("Two Handed Mace Proficiency");
            else if (detailedType == "Polearm")
                return entity.abilities.Contains("Polearm Proficiency");
            else if (detailedType == "Staff")
                return entity.abilities.Contains("Staff Proficiency");
            else if (detailedType == "Bow")
                return entity.abilities.Contains("Bow");
            else if (detailedType == "Crossbow")
                return entity.abilities.Contains("Crossbow");
            else if (detailedType == "Gun")
                return entity.abilities.Contains("Gun");
            else
                return true;
        }
        else if (type == "Off Hand")
        {
            if (detailedType == "Shield")
                return entity.abilities.Contains("Shield Proficiency");
            else
                return entity.abilities.Contains("Off Hand Proficiency");
        }
        else if (type == "One Handed")
        {
            if (detailedType == "Dagger")
                return entity.abilities.Contains("Dagger Proficiency");
            else if (detailedType == "Sword")
                return entity.abilities.Contains("One Handed Sword Proficiency");
            else if (detailedType == "Axe")
                return entity.abilities.Contains("One Handed Axe Proficiency");
            else if (detailedType == "Mace")
                return entity.abilities.Contains("One Handed Mace Proficiency");
            else if (detailedType == "Wand")
                return entity.abilities.Contains("Wand Proficiency");
            else if (detailedType == "Fist Weapon")
                return entity.abilities.Contains("Fist Weapon Proficiency");
            else
                return true;
        }
        else
            return true;
    }

    private void Equip(Entity entity, string slot)
    {
        entity.equipment[slot] = this;
        if (entity.inventory.items.Contains(this))
            entity.inventory.items.Remove(this);
        if (abilities == null) return;
        entity.abilities.AddRange(abilities);
        entity.abilities = entity.abilities.Distinct().ToList();
    }

    public List<string> PossibleSlots()
    {
        if (type == "Two Handed") return new() { "Main Hand" };
        else if (type == "Off Hand") return new() { "Off Hand" };
        else if (type == "One Handed") return new() { "Main Hand", "Off Hand" };
        else return new() { type };
    }

    public void Equip(Entity entity, bool secondSlot = false)
    {
        var index = entity.inventory.items.IndexOf(this);
        if (type == "Two Handed")
        {
            entity.Unequip(new() { "Off Hand", "Main Hand" }, index);
            Equip(entity, "Main Hand");
        }
        else if (type == "Off Hand")
        {
            var mainHand = entity.GetItemInSlot("Main Hand");
            if (mainHand != null && mainHand.type == "Two Handed")
                entity.Unequip(new() { "Main Hand" }, index);
            entity.Unequip(new() { "Off Hand" }, index);
            Equip(entity, "Off Hand");
        }
        else if (type == "One Handed")
        {
            if (secondSlot)
            {
                var mainHand = entity.GetItemInSlot("Main Hand");
                if (mainHand != null && mainHand.type == "Two Handed")
                    entity.Unequip(new() { "Main Hand" }, index);
                entity.Unequip(new() { "Off Hand" }, index);
                Equip(entity, "Off Hand");
            }
            else
            {
                entity.Unequip(new() { "Main Hand" }, index);
                Equip(entity, "Main Hand");
            }
        }
        else
        {
            if (type == null) UnityEngine.Debug.Log(name);
            entity.Unequip(new() { type }, index);
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
