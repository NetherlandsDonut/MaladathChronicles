using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Sound;
using static ItemSet;
using static SaveGame;
using static SiteTown;
using static Coloring;
using static GameSettings;

public class Item
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        if (set != null)
            if (!itemSets.Exists(x => x.name == set))
                itemSets.Insert(0, new ItemSet()
                {
                    name = set,
                    setBonuses = new()
                });
        if (abilities != null)
            foreach (var ability in abilities)
                if (!Ability.abilities.Exists(x => x.name == ability))
                    Ability.abilities.Insert(0, new Ability()
                    {
                        name = ability,
                        icon = "Ability" + ability,
                        events = new(),
                        tags = new()
                    });
    }

    //Rarity of this item which can range from Poor to Legendary
    public string rarity;

    //Name of the item
    public string name;

    //Icon of the item in the inventory
    public string icon;

    //Detailed type of the item
    //EXAMPLE: "Axe" for item of "Two Handed" type
    public string detailedType;

    //Type of the item
    public string type;

    //Armor class of the armor piece
    //Can range from Cloth to Plate
    public string armorSpec;

    //Set that this item is part of
    public string set;

    //Faction that this item belongs to
    public string faction;

    //Reputation standing required from the player to use this item
    public string reputationRequired;
    
    //Item power / level of this item, helps in calculating which item is better than other
    public int ilvl;

    //Minimum required level of the character for it to be able to equip or use this item
    public int lvl;

    //Minimum damage this weapon can do
    public int minDamage;
    
    //Maximum damage this weapon can do
    public int maxDamage;

    //Amount of armor provided to the wearer of this item
    public int armor;

    //Amount of block power provided to the wearer
    public int block;

    //List of abilities provided to the wearer of this item
    public List<string> abilities;

    //Spec restrictions for this item
    //Specs listed in it are the specs that exclusively can use this item
    public List<string> specs;
    
    //Price of the item for it to be bought, the sell price is 1/4 of that
    public double price;

    //Weapon attack speed
    public double speed;
    
    //Stats provided to the wearer like Stamina or Intellect
    public Stats stats;

    //This is a list of races that are eligible to drop this item
    public List<string> droppedBy;

    //Those two are unused right now but will serve the role of loot
    public List<string> possibleItems;
    public List<string> alternateItems;

    //This function returns the type of sound that this item makes when it is being manipulated
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
        else if (armorSpec == "Cloth") result = "ClothLeather";
        else if (armorSpec == "Leather") result = "ClothLeather";
        else if (armorSpec == "Mail") result = "ChainLarge";
        else if (armorSpec == "Plate") result = "MetalLarge";
        else result = "ClothLeather";
        return soundType + result;
    }

    public bool CanBuy(Entity entity)
    {
        return entity.inventory.money >= price && (faction == null || true); //true = rep
    }

    public bool CanEquip(Entity entity)
    {
        if (type == "Miscellaneous")
            return false;
        if (specs != null && !specs.Contains(entity.spec))
            return false;
        if (armorSpec != null)
            return entity.abilities.Contains(armorSpec + " Proficiency");
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
            null,
            (h) => () =>
            {
                if (item == null) return;
                SetAnchor(BottomRight);
                PrintItemTooltip(item);
            });
            if (item != null) AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""));
        });
    }

    public static void PrintBankItem(Item item)
    {
        AddBigButton(item.icon,
            null,
            (h) =>
            {
                if (currentSave.player.inventory.items.Count < 40)
                {
                    PlaySound(item.ItemSound("PickUp"), 0.6f);
                    currentSave.player.inventory.items.Add(item);
                    currentSave.banks[town.name].items.Remove(item);
                    Respawn("Inventory");
                    Respawn("Bank");
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
    }

    public static void PrintVendorItem(Item item)
    {
        AddBigButton(item.icon,
            null,
            (h) =>
            {
                if (currentSave.player.inventory.items.Count < 40 && currentSave.player.inventory.money >= item.price)
                {
                    PlaySound("DesktopTransportPay");
                    currentSave.player.inventory.items.Add(item);
                    currentSave.player.inventory.money -= item.price;
                    Respawn("Inventory");
                    Respawn("Vendor");
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
    }

    public static void PrintInventoryItem(Item item)
    {
        AddBigButton(item.icon,
            null,
            (h) =>
            {
                if (CDesktop.title == "Vendor")
                {
                    if (currentSave.banks[town.name].items.Count < 40)
                    {
                        PlaySound(item.ItemSound("DesktopTransportPay"), 0.6f);
                        currentSave.banks[town.name].items.Add(item);
                        currentSave.player.inventory.items.Remove(item);
                        Respawn("Inventory");
                        Respawn("Bank");
                    }
                }
                else if (CDesktop.title == "Bank")
                {
                    if (currentSave.banks[town.name].items.Count < 40)
                    {
                        PlaySound(item.ItemSound("PutDown"), 0.6f);
                        currentSave.banks[town.name].items.Add(item);
                        currentSave.player.inventory.items.Remove(item);
                        Respawn("Inventory");
                        Respawn("Bank");
                    }
                }
                else
                {
                    if (item.CanEquip(currentSave.player))
                    {
                        PlaySound(item.ItemSound("PickUp"), 0.6f);
                        item.Equip(currentSave.player);
                        Respawn("Inventory");
                        Respawn("PlayerEquipmentInfo");
                    }
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

    public static void PrintLootItem(Item item)
    {
        AddBigButton(item.icon,
            (h) =>
            {
                if (currentSave.player.inventory.items.Count < 40)
                {
                    PlaySound(item.ItemSound("PutDown"), 0.6f);
                    currentSave.player.inventory.items.Add(item);
                    Board.board.results.items.Remove(item);
                    if (Board.board.results.exclusiveItems.Contains(item.name))
                        Board.board.results.items.RemoveAll(x => Board.board.results.exclusiveItems.Contains(x.name));
                    if (Board.board.results.items.Count == 0)
                    {
                        CloseDesktop(CDesktop.title);
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("CombatResultsLoot");
                    }
                }
            },
            null,
            (h) => () =>
            {
                if (item == null) return;
                SetAnchor(Center);
                PrintItemTooltip(item);
            }
        );
        if (Board.board != null && Board.board.results.exclusiveItems.Count > 1 && Board.board.results.exclusiveItems.Contains(item.name))
            AddBigButtonOverlay("OtherItemExclusive", 0, 2);
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
            if (item.armorSpec != null)
            {
                AddLine(item.armorSpec + " " + item.type);
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
        if (item.specs != null)
            AddHeaderRegion(() =>
            {
                AddLine("Specs: ", "DarkGray");
                foreach (var spec in item.specs)
                {
                    AddText(spec, spec);
                    if (spec != item.specs.Last())
                        AddText(", ", "DarkGray");
                }
            });
        if (item.set != null)
        {
            var set = itemSets.Find(x => x.name == item.set);
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
        if (item.price > 0)
            PrintPriceRegion(item.price);
    }

    public static Item GetItem(string name) => items.Find(x => x.name == name);

    //Currently opened item
    public static Item item;

    //EXTERNAL FILE: List containing all buffs in-game
    public static List<Item> items;

    //List of all filtered items by input search
    public static List<Item> itemsSearch;
}
