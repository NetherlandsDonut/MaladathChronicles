using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Sound;
using static SaveGame;
using static Coloring;
using static GameSettings;

public class Item
{
    public string rarity, name, icon, detailedType, type, armorClass, set, faction, reputationRequired;
    public int ilvl, lvl, minDamage, maxDamage, armor, block;
    public List<string> possibleItems, alternateItems, abilities, classes;
    public double price, speed;
    public Stats stats;

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

    public bool CanBuy(Entity entity)
    {
        return entity.money >= price && (faction == null || REPENOUGH);
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
            var mainHand = entity.GetItemInSlot("Main Hand");
            var offHand = entity.GetItemInSlot("Off Hand");
            secondSlot = mainHand != null && mainHand.type != "Two Handed" && (offHand == null || offHand.ilvl <= mainHand.ilvl);
            if (secondSlot)
            {
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
            if (type == null) Debug.Log(name);
            entity.Unequip(new() { type }, index);
            Equip(entity, type);
        }
    }

    public static void PrintEquipmentItem(Item item)
    {
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddBigButton(item == null ? "OtherEmpty" : item.icon,
            (h) =>
            {

            },
            (h) => () =>
            {
                if (item == null) return;
                SetAnchor(BottomRight);
                PrintItemTooltip(item);
            });
            if (item != null) AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""));
        });
    }

    public static void PrintInventoryItem(Item item)
    {
        AddBigButton(item.icon,
            (h) =>
            {
                if (item.CanEquip(currentSave.player))
                {
                    PlaySound(item.ItemSound("PickUp"), 0.6f);
                    item.Equip(currentSave.player);
                    CloseWindow(h.window);
                    SpawnWindowBlueprint("Inventory");
                    CloseWindow("PlayerEquipmentInfo");
                    SpawnWindowBlueprint("PlayerEquipmentInfo");
                }
            },
            (h) => () =>
            {
                if (item == null) return;
                SetAnchor(Center);
                PrintItemTooltip(item);
            }
        );
        if (settings.rarityIndicators.Value())
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 2);
        if (currentSave.player.HasItemEquipped(item.name))
        {
            SetBigButtonToGrayscale();
            AddBigButtonOverlay("OtherGridBlurred", 0, 2);
        }
        if (item.CanEquip(currentSave.player) && currentSave.player.IsItemNewSlot(item) && (settings.upgradeIndicators.Value() || settings.newSlotIndicators.Value()))
            AddBigButtonOverlay(settings.newSlotIndicators.Value() ? "OtherItemNewSlot" : "OtherItemUpgrade", 0, 2);
        else if (settings.upgradeIndicators.Value() && item.CanEquip(currentSave.player) && currentSave.player.IsItemAnUpgrade(item))
            AddBigButtonOverlay("OtherItemUpgrade", 0, 2);
    }

    public static void PrintItemTooltip(Item item)
    {
        AddHeaderGroup();
        SetRegionGroupWidth(190);
        var split = item.name.Split(", ");
        AddHeaderRegion(() => { AddLine(split[0], item.rarity); });
        if (split.Length > 1) AddHeaderRegion(() => { AddLine("\"" + split[1] + "\"", item.rarity); });
        AddPaddingRegion(() =>
        {
            if (item.armorClass != null)
            {
                AddLine(item.armorClass + " " + item.type);
                AddLine(item.armor + " Armor");
            }
            else if (item.maxDamage != 0)
            {
                AddLine(item.type + " " + item.detailedType);
                AddLine(item.minDamage + " - " + item.maxDamage + " Damage");
            }
            else
                AddLine(item.type == null ? "" : item.type);
        });
        if (item.stats != null && item.stats.stats.Count > 0)
            AddPaddingRegion(() =>
            {
                foreach (var stat in item.stats.stats)
                    AddLine("+" + stat.Value + " " + stat.Key);
            });
        if (item.classes != null)
            AddHeaderRegion(() =>
            {
                AddLine("Classes: ", "DarkGray");
                foreach (var spec in item.classes)
                {
                    AddText(spec, spec);
                    if (spec != item.classes.Last())
                        AddText(", ", "DarkGray");
                }
            });
        if (item.set != null)
        {
            var set = ItemSet.itemSets.Find(x => x.name == item.set);
            if (set != null)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Part of ", "DarkGray");
                    AddText(item.set, "Gray");
                });
                AddPaddingRegion(() =>
                {
                    foreach (var bonus in set.setBonuses)
                    {
                        var howMuch = currentSave != null && currentSave.player != null ? set.EquippedPieces(currentSave.player) : 0;
                        bool has = howMuch >= bonus.requiredPieces;
                        AddLine((has ? bonus.requiredPieces : howMuch) + "/" + bonus.requiredPieces + " Set: ", has ? "Uncommon" : "DarkGray");
                        if (bonus.description.Count > 0)
                            AddText(bonus.description[0], has ? "Uncommon" : "DarkGray");
                        for (int i = 0; i < bonus.description.Count - 1; i++)
                            AddLine(bonus.description[0], has ? "Uncommon" : "DarkGray");
                    }
                });
            }
        }
        if (item.lvl > 0)
            AddHeaderRegion(() =>
            {
                AddLine("Required level: ", "DarkGray");
                AddText(item.lvl + "", ColorItemRequiredLevel(item.lvl));
            });
        PrintPriceRegion(item.price);
    }

    public static Item GetItem(string name) => items.Find(x => x.name == name);

    public static Item item;
    public static List<Item> items, itemsSearch;
}
