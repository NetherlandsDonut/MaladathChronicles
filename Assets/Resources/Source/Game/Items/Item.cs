using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Sound;
using static ItemSet;
using static Defines;
using static SaveGame;
using static SiteTown;
using static Coloring;
using static GameSettings;
using static PermanentEnchant;

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
                if (!Ability.abilities.Exists(x => x.name == ability.Key))
                    Ability.abilities.Insert(0, new Ability()
                    {
                        name = ability.Key,
                        icon = "Ability" + ability,
                        events = new(),
                        tags = new()
                    });
    }

    //Sets a random permanent enchant for the item
    public void SetRandomEnchantment()
    {
        if (!randomEnchantment) return;
        var enchantment = GenerateEnchantment();
        if (enchantment == null) return;
        name += " " + enchantment.suffix;
        foreach (var stat in enchantment.stats)
            if (stats.stats.ContainsKey(stat.Key)) stats.stats[stat.Key] += EnchantmentStatGrowth(ilvl, stat.Value.Length);
            else stats.stats.Add(stat.Key, EnchantmentStatGrowth(ilvl, stat.Value.Length));
            
        PermanentEnchant GenerateEnchantment()
        {
            var containing = new List<PermanentEnchant>();
            var key = "";
            if (type == "One Handed" || type == "Two Handed") key = type + " " + detailedType;
            else if (armorClass != null) key = armorClass + " Armor";
            else if (detailedType != null) key = detailedType;
            else key = type;
            containing = pEnchants.FindAll(x => x.commonlyOn != null && x.commonlyOn.Contains(key) || x.rarelyOn != null && x.rarelyOn.Contains(key));
            if (Roll(10))
            {
                var rare = containing.FindAll(x => x.rarelyOn != null && x.rarelyOn.Contains(key));
                if (rare.Count > 0) return rare[random.Next(0, rare.Count)];
            }
            else
            {
                var common = containing.FindAll(x => x.commonlyOn != null && x.commonlyOn.Contains(key));
                if (common.Count > 0) return common[random.Next(0, common.Count)];
            }
            if (containing.Count == 0) return null;
            return containing[random.Next(0, containing.Count)];
        }
    }

    //Rarity of this item which can range from Poor to Legendary
    public string rarity;

    //Name of the item
    public string name;

    //Icon of the item in the inventory
    public string icon;

    //Determines wether instances of this item get a random enchant
    public bool randomEnchantment;

    //Detailed type of the item
    //EXAMPLE: "Axe" for item of "Two Handed" type
    public string detailedType;

    //Type of the item
    public string type;

    //Armor class of the armor piece
    //Can range from Cloth to Plate
    public string armorClass;

    //Set that this item is part of
    public string set;

    //Faction that this item belongs to
    public string faction;

    //Source where this item can be gotten from
    public string source;

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

    //Amount of this item
    public int amount;

    //Max amount of this item per stack
    public int maxStack;

    //List of abilities provided to the wearer of this item
    public Dictionary<string, int> abilities;

    //Spec restrictions for this item
    //Specs listed in it are the specs that exclusively can use this item
    public List<string> specs;
    
    //Price of the item for it to be bought, the sell price is 1/4 of that
    public double price;

    //Weapon attack speed
    public double speed;

    //Amount of bag space provided
    public int bagSpace;
    
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
        else if (type == "Bag") result = "Bag";
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
        return entity.inventory.money >= price && (faction == null || true); //true = rep
    }

    public bool CanEquip(Entity entity)
    {
        if (type == "Miscellaneous")
            return false;
        if (specs != null && !specs.Contains(entity.spec))
            return false;
        if (type == "Bag")
            return entity.inventory.bags.Count < defines.maxBagsEquipped;
        else if (armorClass != null)
            return entity.abilities.ContainsKey(armorClass + " Proficiency");
        else if (type == "Pouch")
            return entity.abilities.ContainsKey("Pouch Proficiency");
        else if (type == "Quiver")
            return entity.abilities.ContainsKey("Quiver Proficiency");
        else if (type == "Libram")
            return entity.abilities.ContainsKey("Libram Proficiency");
        else if (type == "Totem")
            return entity.abilities.ContainsKey("Totem Proficiency");
        else if (type == "Idol")
            return entity.abilities.ContainsKey("Idol Proficiency");
        else if (type == "Two Handed")
        {
            if (detailedType == "Sword")
                return entity.abilities.ContainsKey("Two Handed Sword Proficiency");
            else if (detailedType == "Axe")
                return entity.abilities.ContainsKey("Two Handed Axe Proficiency");
            else if (detailedType == "Mace")
                return entity.abilities.ContainsKey("Two Handed Mace Proficiency");
            else if (detailedType == "Polearm")
                return entity.abilities.ContainsKey("Polearm Proficiency");
            else if (detailedType == "Staff")
                return entity.abilities.ContainsKey("Staff Proficiency");
            else if (detailedType == "Bow")
                return entity.abilities.ContainsKey("Bow");
            else if (detailedType == "Crossbow")
                return entity.abilities.ContainsKey("Crossbow");
            else if (detailedType == "Gun")
                return entity.abilities.ContainsKey("Gun");
            else
                return true;
        }
        else if (type == "Off Hand")
        {
            if (detailedType == "Shield")
                return entity.abilities.ContainsKey("Shield Proficiency");
            else
                return entity.abilities.ContainsKey("Off Hand Proficiency");
        }
        else if (type == "One Handed")
        {
            if (detailedType == "Dagger")
                return entity.abilities.ContainsKey("Dagger Proficiency");
            else if (detailedType == "Sword")
                return entity.abilities.ContainsKey("One Handed Sword Proficiency");
            else if (detailedType == "Axe")
                return entity.abilities.ContainsKey("One Handed Axe Proficiency");
            else if (detailedType == "Mace")
                return entity.abilities.ContainsKey("One Handed Mace Proficiency");
            else if (detailedType == "Wand")
                return entity.abilities.ContainsKey("Wand Proficiency");
            else if (detailedType == "Fist Weapon")
                return entity.abilities.ContainsKey("Fist Weapon Proficiency");
            else
                return true;
        }
        else
            return true;
    }

    public bool CanUse(Entity entity)
    {
        if (type == "Miscellaneous")
            return abilities != null;
        else
            return false;
    }

    private void Equip(Entity entity, string slot)
    {
        if (slot == "Bag") entity.inventory.bags.Add(this);
        else entity.equipment[slot] = this;
        if (entity.inventory.items.Contains(this))
            entity.inventory.items.Remove(this);
        if (abilities == null) return;
        entity.abilities = entity.abilities.Concat(abilities).ToDictionary(x => x.Key, x => x.Value);
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
        else if (type == "Bag")
        {
            Equip(entity, "Bag");
        }
        else
        {
            if (type == null) Debug.Log(name);
            entity.Unequip(new() { type }, index);
            Equip(entity, type);
        }
    }

    public void Use(Entity entity)
    {

    }

    public static void PrintBankItem(Item item)
    {
        AddBigButton(item.icon,
            null,
            (h) =>
            {
                if (currentSave.player.inventory.CanAddItem(item))
                {
                    if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                    {
                        String.splitAmount.Set("");
                        SpawnWindowBlueprint("SplitItem");
                        CDesktop.LBWindow.LBRegionGroup.LBRegion.inputLine.Activate();
                        splitDelegate = () =>
                        {
                            var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                            if (amount <= 0) return;
                            PlaySound(item.ItemSound("PickUp"), 0.6f);
                            if (amount > item.amount) amount = item.amount;
                            if (amount == item.amount)
                            {
                                currentSave.player.inventory.AddItem(item);
                                currentSave.banks[town.name].items.Remove(item);
                            }
                            else
                            {
                                currentSave.player.inventory.AddItem(item.CopyItem(amount));
                                item.amount -= amount;
                            }
                            Respawn("Inventory");
                            Respawn("Bank");
                        };
                    }
                    else
                    {
                        PlaySound(item.ItemSound("PickUp"), 0.6f);
                        currentSave.player.inventory.AddItem(item);
                        currentSave.banks[town.name].items.Remove(item);
                        Respawn("Inventory");
                        Respawn("Bank");
                    }
                }
            },
            (h) => () =>
            {
                if (item == null) return;
                PrintItemTooltip(item);
            }
        );
        if (settings.rarityIndicators.Value())
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 2);
        if (item.amount > 1)
            SpawnFloatingText(CDesktop.LBWindow.LBRegionGroup.LBRegion.transform.position + new Vector3(32, -27) + new Vector3(38, 0) * (currentSave.banks[town.name].items.IndexOf(item) % 5), item.amount + "", "", "Right");
    }

    public static void PrintVendorItem(Item item)
    {
        AddBigButton(item.icon,
            null,
            (h) =>
            {
                if (currentSave.player.inventory.CanAddItem(item) && currentSave.player.inventory.money >= item.price * 4)
                {
                    PlaySound("DesktopTransportPay");
                    var buyback = CDesktop.windows.Exists(x => x.title == "VendorBuyback");
                    if (buyback) currentSave.buyback.items.Remove(item);
                    currentSave.player.inventory.items.Add(item);
                    currentSave.player.inventory.money -= item.price * 4;
                    Respawn("Inventory");
                    Respawn(buyback ? "VendorBuyback" : "Vendor");
                }
            },
            (h) => () =>
            {
                if (item == null) return;
                PrintItemTooltip(item, false, 4);
            }
        );
        if (settings.rarityIndicators.Value())
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 2);
        if (item.amount > 1)
            SpawnFloatingText(CDesktop.LBWindow.LBRegionGroup.LBRegion.transform.position + new Vector3(32, -27) + new Vector3(38, 0) * (currentSave.buyback.items.IndexOf(item) % 5), item.amount + "", "", "Right");
    }

    public static void PrintInventoryItem(Item item)
    {
        AddBigButton(item.icon,
            null,
            (h) =>
            {
                if (CDesktop.title == "Vendor")
                {
                    if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                    {
                        String.splitAmount.Set("");
                        SpawnWindowBlueprint("SplitItem");
                        CDesktop.LBWindow.LBRegionGroup.LBRegion.inputLine.Activate();
                        splitDelegate = () =>
                        {
                            var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                            if (amount <= 0) return;
                            PlaySound("DesktopTransportPay");
                            if (amount > item.amount) amount = item.amount;
                            currentSave.buyback ??= new(true);
                            if (amount == item.amount)
                            {
                                currentSave.buyback.AddItem(item);
                                currentSave.player.inventory.items.Remove(item);
                            }
                            else
                            {
                                currentSave.buyback.AddItem(item.CopyItem(amount));
                                item.amount -= amount;
                            }
                            currentSave.player.inventory.money += item.price * amount;
                            Respawn("Inventory");
                            CloseWindow("Vendor");
                            Respawn("VendorBuyback");
                        };
                    }
                    else
                    {
                        PlaySound("DesktopTransportPay");
                        currentSave.buyback ??= new(true);
                        currentSave.buyback.AddItem(item);
                        currentSave.player.inventory.items.Remove(item);
                        currentSave.player.inventory.money += item.price * item.amount;
                        Respawn("Inventory");
                        CloseWindow("Vendor");
                        Respawn("VendorBuyback");
                    }
                }
                else if (CDesktop.windows.Exists(x => x.title == "Bank"))
                {
                    if (currentSave.banks[town.name].CanAddItem(item))
                    {
                        if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                        {
                            String.splitAmount.Set("");
                            SpawnWindowBlueprint("SplitItem");
                            CDesktop.LBWindow.LBRegionGroup.LBRegion.inputLine.Activate();
                            splitDelegate = () =>
                            {
                                var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                                if (amount <= 0) return;
                                PlaySound(item.ItemSound("PutDown"), 0.6f);
                                if (amount > item.amount) amount = item.amount;
                                if (amount == item.amount)
                                {
                                    currentSave.banks[town.name].AddItem(item);
                                    currentSave.player.inventory.items.Remove(item);
                                }
                                else
                                {
                                    currentSave.banks[town.name].AddItem(item.CopyItem(amount));
                                    item.amount -= amount;
                                }
                                Respawn("Inventory");
                                Respawn("Bank");
                            };
                        }
                        else
                        {
                            PlaySound(item.ItemSound("PutDown"), 0.6f);
                            currentSave.banks[town.name].AddItem(item);
                            currentSave.player.inventory.items.Remove(item);
                            Respawn("Inventory");
                            Respawn("Bank");
                        }
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
                    else if (item.CanUse(currentSave.player))
                    {
                        PlaySound(item.ItemSound("Use"), 0.6f);
                        item.Use(currentSave.player);
                        Respawn("Inventory");
                        Respawn("PlayerEquipmentInfo");
                    }
                }
            },
            (h) => () =>
            {
                if (item == null) return;
                if (CDesktop.windows.Exists(x => x.title == "ConfirmItemDestroy")) return;
                PrintItemTooltip(item, Input.GetKey(KeyCode.LeftShift));
            },
            (h) =>
            {
                Item.item = item;
                PlaySound("DesktopMenuOpen");
                SpawnWindowBlueprint("ConfirmItemDestroy");
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
        if (item.amount > 1)
            SpawnFloatingText(CDesktop.LBWindow.LBRegionGroup.LBRegion.transform.position + new Vector3(32, -27) + new Vector3(38, 0) * (currentSave.player.inventory.items.IndexOf(item) % 5), item.amount + "", "", "Right");
    }

    public static void PrintLootItem(Item item)
    {
        AddBigButton(item.icon,
            (h) =>
            {
                if (currentSave.player.inventory.CanAddItem(item))
                {
                    PlaySound(item.ItemSound("PutDown"), 0.6f);
                    if (currentSave.player.inventory.AddItem(item))
                        Board.board.results.inventory.items.Remove(item);
                    if (Board.board.results.exclusiveItems.Contains(item.name))
                        Board.board.results.inventory.items.RemoveAll(x => Board.board.results.exclusiveItems.Contains(x.name));
                    if (Board.board.results.inventory.items.Count == 0) CloseDesktop(CDesktop.title);
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
                if (CDesktop.windows.Exists(x => x.title == "ConfirmItemDestroy")) return;
                PrintItemTooltip(item, Input.GetKey(KeyCode.LeftShift));
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
        if (item.amount > 1)
            SpawnFloatingText(CDesktop.LBWindow.LBRegionGroup.LBRegion.transform.position + new Vector3(32, -27) + new Vector3(38, 0) * Board.board.results.inventory.items.IndexOf(item), item.amount + "", "", "Right");
    }

    public static void PrintItemTooltip(Item item, bool compare = false, double priceMultiplier = 1)
    {
        SetAnchor(-115, 146);
        AddHeaderGroup();
        SetRegionGroupWidth(228);
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
            else if (item.bagSpace != 0) AddLine(item.bagSpace + " Slot Bag");
            else AddLine(item.type == null ? "" : item.type);
        });
        if (item.stats != null && item.stats.stats.Count > 0)
            AddPaddingRegion(() =>
            {
                foreach (var stat in item.stats.stats)
                    AddLine("+" + stat.Value + " " + stat.Key);
            });
        var current = currentSave != null && currentSave.player.equipment.ContainsKey(item.type) ? currentSave.player.equipment[item.type] : null;
        if (compare)
        {
            AddHeaderRegion(() =>
            {
                AddLine("Stat changes on equip:", "DarkGray");
            });
            AddPaddingRegion(() =>
            {
                var statsRecorded = new List<string>();
                foreach (var stat in item.stats.stats)
                {
                    statsRecorded.Add(stat.Key);
                    var balance = current != null && current.stats != null && current.stats.stats.ContainsKey(stat.Key) ? stat.Value - current.stats.stats[stat.Key] : stat.Value;
                    AddLine((balance > 0 ? "+" : "") + balance, balance > 0 ? "Uncommon" : "DangerousRed");
                    AddText(" " + stat.Key);
                }
                if (current != null && current.stats != null)
                    foreach (var stat in current.stats.stats)
                        if (!statsRecorded.Contains(stat.Key))
                        {
                            AddLine("-" + stat.Value, "DangerousRed");
                            AddText(" " + stat.Key);
                        }
                if (CDesktop.LBWindow.LBRegionGroup.LBRegion.lines.Count == 0)
                    AddLine("No changes", "Gray");
            });
        }
        if (item.specs != null)
            AddHeaderRegion(() =>
            {
                AddLine("Classes: ", "DarkGray");
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
        if (item.lvl > 1)
            AddHeaderRegion(() =>
            {
                AddLine("Required level: ", "DarkGray");
                AddText(item.lvl + "", ColorItemRequiredLevel(item.lvl));
            });
        if (item.price > 0)
            PrintPriceRegion(item.price * priceMultiplier);
    }

    public Item CopyItem(int amount = 1)
    {
        var newItem = this.Copy<Item>();
        newItem.amount = amount;
        return newItem;
    }

    public static Item GetItem(string name) => items.Find(x => x.name == name);

    //Currently opened item
    public static Item item;

    //EXTERNAL FILE: List containing all buffs in-game
    public static List<Item> items;

    //List of all filtered items by input search
    public static List<Item> itemsSearch;
}
