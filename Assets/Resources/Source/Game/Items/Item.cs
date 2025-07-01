using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Sound;
using static ItemSet;
using static Defines;
using static SaveGame;
using static SiteTown;
using static Coloring;
using static Inventory;
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

    //Position of the item in the inventory
    public int x, y;

    //Rarity of this item which can range from Poor to Legendary
    public string rarity;

    //Name of the item
    public string name;

    //Icon of the item in the inventory
    public string icon;

    //Icon of the item in the inventory
    public bool combatUse;

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

    //Enchant applied by an enchanter
    public Enchant enchant;
    
    //Item power / level of this item, helps in calculating which item is better than other
    public int ilvl;

    //Minimum required level of the character for it to be able to equip or use this item
    public int lvl;

    //Minimum power modifier this weapon can roll
    public double minPower;

    //Maximum power modifier this weapon can roll
    public double maxPower;

    //Amount of armor provided to the wearer of this item
    public int armor;

    //Amount of block power provided to the wearer
    public int block;

    //Amount of this item
    public int amount;

    //Time left for this item to be removed from buyback
    public int minutesLeft;

    //Drop range, it's automatic if set to default
    public string dropRange;

    //Max amount of this item per stack
    public int maxStack;

    //Can player get rid of this item
    public bool indestructible;

    //Can player have more than one of these items
    public bool unique;

    //Items contained inside of the item
    public List<Item> itemsInside;

    //List of abilities provided to the wearer of this item
    public Dictionary<string, int> abilities;

    //Spec restrictions for this item
    //Specs listed in it are the specs that exclusively can use this item
    public List<string> specs;

    //Drop restrictions based on race
    public List<string> raceDropRestriction;

    //Drop restrictions based on character class
    public List<string> specDropRestriction;

    //List of quests that can be started by using this item
    public List<int> questsStarted;

    //Price of the item for it to be bought, the sell price is 1/4 of that
    public int price;

    //Amount of bag space provided
    public int bagSpace;
    
    //Stats provided to the wearer like Stamina or Intellect
    public Dictionary<string, int> stats;

    //This is a list of races that are eligible to drop this item
    public List<string> droppedBy;

    #region Equipping

    //Tells whether this item is generally wearable
    public bool IsWearable()
    {
        if (new List<string> { "Miscellaneous", "Trade Good", "Recipe", "Bag" }.Contains(type)) return false;
        return true;
    }

    //Equips this item on the chosen entity in a specific slot
    private void EquipInto(Entity entity, string slot)
    {
        if (slot == "Bag") entity.inventory.bags.Add(this);
        else entity.equipment[slot] = this;
        if (entity.inventory.items.Contains(this))
            entity.inventory.items.Remove(this);
        if (abilities != null)
            entity.abilities = entity.abilities.Concat(abilities).ToDictionary(x => x.Key, x => x.Value);
        if (enchant != null && enchant.abilities != null)
            entity.abilities = entity.abilities.Concat(enchant.abilities).ToDictionary(x => x.Key, x => x.Value);
    }

    //Equips this item on the chosen entity
    //Slot action determines how is the item equiped [Auto, ]
    public void Equip(Entity entity, bool autoSlotting, bool altSlot)
    {
        var unequiped = new List<Item>();
        if (type == "Two Handed")
        {
            if (entity.equipment.ContainsKey("Off Hand") && entity.equipment.ContainsKey("Main Hand") && entity.inventory.BagSpace() - entity.inventory.items.Count < 1)
                SpawnFallingText(new Vector2(0, 34), "Inventory full", "Red");
            else 
            {
                unequiped.AddRange(entity.Unequip(new() { "Off Hand", "Main Hand" }));
                EquipInto(entity, "Main Hand");
            }
        }
        else if (type == "Off Hand")
        {
            var mainHand = entity.GetItemInSlot("Main Hand");
            if (mainHand != null && mainHand.type == "Two Handed")
                unequiped.AddRange(entity.Unequip(new() { "Main Hand" }));
            else unequiped.AddRange(entity.Unequip(new() { "Off Hand" }));
            EquipInto(entity, "Off Hand");
        }
        else if (type == "One Handed")
        {
            var mainHand = entity.GetItemInSlot("Main Hand");
            var offHand = entity.GetItemInSlot("Off Hand");

            //If slot is chosen automatically..
            if (autoSlotting)

                //If there is a one handed weapon equipped and dual wielding is possible; equip the weapon in the off hand
                if (mainHand != null && mainHand.type != "Two Handed" && entity.abilities.ContainsKey("Dual Wielding Proficiency"))
                {
                    if (mainHand != null && mainHand.type == "Two Handed")
                        unequiped.AddRange(entity.Unequip(new() { "Main Hand" }));
                    else unequiped.AddRange(entity.Unequip(new() { "Off Hand" }));
                    EquipInto(entity, "Off Hand");
                }

                //Otherwise equip it into the main hand
                else
                {
                    unequiped.AddRange(entity.Unequip(new() { "Main Hand" }));
                    EquipInto(entity, "Main Hand");
                }

            //Otherwise equip it into the main slot or with LeftAlt into the offhand
            else
            {
                //If dual wielding is possible and LeftAlt is pressed; equip the weapon in the off hand
                if (altSlot && entity.abilities.ContainsKey("Dual Wielding Proficiency"))
                {
                    if (mainHand != null && mainHand.type == "Two Handed")
                        unequiped.AddRange(entity.Unequip(new() { "Main Hand" }));
                    else unequiped.AddRange(entity.Unequip(new() { "Off Hand" }));
                    EquipInto(entity, "Off Hand");
                }

                //Otherwise equip it into the main hand
                else
                {
                    unequiped.AddRange(entity.Unequip(new() { "Main Hand" }));
                    EquipInto(entity, "Main Hand");
                }
            }
        }
        else if (type == "Bag") EquipInto(entity, "Bag");
        else
        {
            if (type == null) Debug.Log(name);
            unequiped.AddRange(entity.Unequip(new() { type }));
            EquipInto(entity, type);
        }
        foreach (var item in unequiped)
            entity.inventory.AddItem(item);
    }

    //Checks whether a chosen entity can equip this item
    //While [downgradeArmor] is set to false this function does not allow
    //people downgrading their preferred armor class
    //For example it will say that a Paladin cannot equip a cloth item
    public bool CanEquip(Entity entity, bool downgradeArmor = false)
    {
        if (type == "Miscellaneous" || type == "Trade Good" || type == "Recipe")
            return false;
        if (specs != null && !specs.Contains(entity.spec))
            return false;
        if (type == "Bag")
            return entity.inventory.bags.Count < defines.maxBagsEquipped;
        else if (armorClass != null)
        {
            if (downgradeArmor)
            {
                if (armorClass == "Plate")
                {
                    if (entity.abilities.ContainsKey("Plate Proficiency")) return true;
                    else return false;
                }
                if (armorClass == "Mail")
                {
                    if (entity.abilities.ContainsKey("Plate Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Mail Proficiency")) return true;
                    else return false;
                }
                if (armorClass == "Leather")
                {
                    if (entity.abilities.ContainsKey("Plate Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Mail Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Leather Proficiency")) return true;
                    else return false;
                }
                if (armorClass == "Cloth")
                {
                    if (entity.abilities.ContainsKey("Plate Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Mail Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Leather Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Cloth Proficiency")) return true;
                    else return false;
                }
                return true;
            }
            else return entity.abilities.ContainsKey(armorClass + " Proficiency");
        }
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
        else if (type == "Ranged Weapon")
        {
            if (detailedType == "Bow")
                return entity.abilities.ContainsKey("Bow Proficiency");
            else if (detailedType == "Crossbow")
                return entity.abilities.ContainsKey("Crossbow Proficiency");
            else if (detailedType == "Gun")
                return entity.abilities.ContainsKey("Gun Proficiency");
            else
                return true;
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

    #endregion

    #region Using

    //Tells whether a chosen entity can use this item
    //This does not include equipping it
    public bool CanUse(Entity entity)
    {
        if (questsStarted != null && questsStarted.Count > 0)
            return true;
        else if (type == "Recipe")
        {
            var recipe = Recipe.recipes.Find(x => name.Contains(x.name));
            if (recipe != null) return entity.professionSkills.ContainsKey(recipe.profession) && entity.professionSkills[recipe.profession].Item1 >= recipe.learnedAt && !entity.learnedRecipes.ContainsKey(recipe.name);
            else Debug.Log("ERROR 007: Did not find a dedicated recipe to item: \"" + name + "\"");
        }
        else if (abilities != null)
            return CDesktop.title == "Game" == combatUse;
        return false;
    }

    //Uses this item on a chosen entity
    //This never equips any item, it uses it, but never equips it
    public void Use(Entity entity)
    {
        if (questsStarted != null)
        {
            var all = Quest.quests.FindAll(x => questsStarted.Contains(x.questID)).ToList();
            var quest = all.Find(x => currentSave.player.CanSeeItemQuest(x));
            if (quest == null)
            {
                PlaySound("QuestFailed");
                SpawnFallingText(new Vector2(0, 34), "Requirements not met", "Red");
            }
            else
            {
                var message = "";
                if (currentSave.player.completedQuests.Contains(quest.questID)) message = "Already completed";
                else if (quest.requiredLevel > currentSave.player.level) message = "Requires level " + quest.requiredLevel;
                else if (quest.faction != null && !currentSave.player.IsRankHighEnough(currentSave.player.ReputationRank(quest.faction), quest.requiredRank)) message = "Requires " + quest.requiredRank + " with " + quest.faction;
                if (message != "")
                {
                    PlaySound("QuestFailed");
                    SpawnFallingText(new Vector2(0, 34), message, "Red");
                }
                else
                {
                    SpawnDesktopBlueprint("QuestLog");
                    CloseDesktop("EquipmentScreen");
                    if (currentSave.player.currentQuests.Exists(x => x.questID == quest.questID))
                    {
                        Quest.quest = currentSave.player.currentQuests.Find(x => x.questID == quest.questID);
                        SpawnWindowBlueprint("Quest");
                    }
                    else
                    {
                        Quest.quest = quest;
                        SpawnWindowBlueprint("QuestAdd");
                    }
                }
            }
        }
        else if (type == "Recipe")
        {
            var recipe = Recipe.recipes.Find(x => name.Contains(x.name));
            entity.LearnRecipe(recipe);
            PlaySound("DesktopSkillLearned");
            if (amount > 1) amount--;
            else entity.inventory.items.Remove(this);
        }
        else currentSave.CallEvents(new() { { "Trigger", "ItemUsed" }, { "ItemHash", GetHashCode() + "" } });
    }

    //Check whether a chosen entity meets requirements to buy this item
    public bool CanBuy(Entity entity)
    {
        return entity.inventory.money >= price && (faction == null || currentSave.player.IsRankHighEnough(currentSave.player.ReputationRank(faction), reputationRequired));
    }

    #endregion

    #region Professions

    //Indicates whether the item is disenchantable
    public bool IsDisenchantable()
    {
        if (new List<string> { "Miscellaneous", "Trade Good", "Recipe", "Bag" }.Contains(type)) return false;
        if (rarity != "Uncommon" && rarity != "Rare" && rarity != "Epic") return false;
        return true;
    }

    //Generates disenchanting loot based on this item
    public Inventory GenerateDisenchantLoot()
    {
        var rarities = new List<string>() { "Uncommon" };
        if (rarity == "Rare" || rarity == "Epic") rarities.Add("Rare");
        if (rarity == "Epic") rarities.Add("Epic");
        var drops = GeneralDrop.generalDrops.FindAll(x => x.dropStart <= itemToDisenchant.ilvl && x.dropEnd >= itemToDisenchant.ilvl && rarities.Any(y => x.category == "Disenchant " + y));
        var inv = new Inventory(true);
        if (drops.Count > 0)
            foreach (var drop in drops)
                if (Roll(drop.rarity))
                {
                    int amount = 1;
                    for (int i = 1; i < drop.dropCount; i++) amount += Roll(50) ? 1 : 0;
                    inv.AddItem(items.Find(x => x.name == drop.item).CopyItem(amount));
                }
        return inv;
    }


    #endregion

    #region Print

    public static void PrintBankItem(Item item)
    {
        AddBigButton(item.icon,
            (h) =>
            {
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                if (movingItem == null)
                {
                    if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                    {
                        String.splitAmount.Set("");
                        SpawnWindowBlueprint("SplitItem");
                        CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                        movingItemX = h.region.bigButtons.IndexOf(h.GetComponent<LineBigButton>());
                        movingItemY = h.window.headerGroup.regions.IndexOf(h.region) - 2;
                        splitDelegate = () =>
                        {
                            var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                            if (amount >= item.amount) PickUpMovingItem("Bank", null);
                            else PickUpMovingItem("Bank", null, amount);
                            Respawn("Bank", true);
                            Respawn("Inventory", true);
                        };
                    }
                    else PickUpMovingItem("Bank", h);
                }
                else if (movingItem != null)
                    SwapMovingItem(h);
            },
            (h) =>
            {
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                if (movingItem == null && currentSave.player.inventory.CanAddItem(item))
                {
                    if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                    {
                        String.splitAmount.Set("");
                        SpawnWindowBlueprint("SplitItem");
                        CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                        splitDelegate = () =>
                        {
                            var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                            if (amount <= 0) return;
                            PlaySound(item.ItemSound("PickUp"), 0.8f);
                            if (amount >= item.amount)
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
                        PlaySound(item.ItemSound("PickUp"), 0.8f);
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
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                PrintItemTooltip(item);
            }
        );
        if (settings.rarityIndicators.Value())
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 3);
        if (item.maxStack > 1) SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * item.x, item.amount + "", "", "", "Right");
    }

    public static void PrintVendorItem(StockItem stockItem, Item buyback)
    {
        var item = stockItem != null ? items.Find(x => x.name == stockItem.item).CopyItem(stockItem.amount) : buyback;
        AddBigButton(item.icon,
            null,
            (h) =>
            {
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (movingItem == null && currentSave.player.inventory.CanAddItem(item))
                {
                    if (buyback != null)
                    {
                        if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                        {
                            String.splitAmount.Set("1");
                            SpawnWindowBlueprint("SplitItem");
                            CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                            splitDelegate = () =>
                            {
                                var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                                if (amount <= 0) return;
                                if (amount > item.amount) amount = item.amount;
                                if (currentSave.player.inventory.money >= item.price * amount)
                                {
                                    PlaySound("DesktopTransportPay");
                                    currentSave.player.inventory.money -= item.price * amount;
                                    if (amount == item.amount)
                                    {
                                        item.minutesLeft = 0;
                                        currentSave.buyback.items.Remove(item);
                                        currentSave.player.inventory.AddItem(item);
                                    }
                                    else
                                    {
                                        var newItem = item.CopyItem(amount);
                                        newItem.minutesLeft = 0;
                                        currentSave.player.inventory.AddItem(newItem);
                                        item.amount -= amount;
                                    }
                                    Respawn("Inventory");
                                    Respawn("VendorBuyback");
                                }
                            };
                        }
                        else if (currentSave.player.inventory.money >= item.price * item.amount)
                        {
                            PlaySound("DesktopTransportPay");
                            var price = item.price * item.amount;
                            currentSave.player.inventory.money -= price;
                            currentSave.buyback.items.Remove(item);
                            currentSave.player.inventory.AddItem(item);
                            Respawn("Inventory");
                        }
                    }
                    else if (stockItem != null && stockItem.amount > 0)
                    {
                        if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                        {
                            String.splitAmount.Set(item.amount + "");
                            SpawnWindowBlueprint("SplitItem");
                            CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                            splitDelegate = () =>
                            {
                                var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                                if (amount <= 0) return;
                                if (amount > item.amount) amount = item.amount;
                                if (currentSave.player.inventory.money >= item.price * amount * 4)
                                {
                                    PlaySound("DesktopTransportPay");
                                    stockItem.amount -= amount;
                                    if (stockItem.minutesLeft == 0) stockItem.minutesLeft = stockItem.restockSpeed;
                                    currentSave.player.inventory.AddItem(item.CopyItem(amount));
                                    currentSave.player.inventory.money -= item.price * amount * 4;
                                    Respawn("Inventory");
                                    Respawn("Vendor");
                                }
                            };
                        }
                        else if (currentSave.player.inventory.money >= item.price * 4)
                        {
                            PlaySound("DesktopTransportPay");
                            stockItem.amount -= 1;
                            if (stockItem.minutesLeft == 0) stockItem.minutesLeft = stockItem.restockSpeed;
                            currentSave.player.inventory.AddItem(item.CopyItem(1));
                            currentSave.player.inventory.money -= item.price * 4;
                            Respawn("Inventory");
                        }
                    }
                }
            },
            (h) => () =>
            {
                if (item == null) return;
                if (WindowUp("InventorySort")) return;
                PrintItemTooltip(item, Input.GetKey(KeyCode.LeftShift), buyback == null ? 4 : 1);
            }
        );
        if (settings.rarityIndicators.Value())
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 3);
        if (item.questsStarted != null)
        {
            var all = Quest.quests.FindAll(x => item.questsStarted.Contains(x.questID)).ToList();
            var status = "Cant";
            foreach (var quest in all)
            {
                if (currentSave.player.completedQuests.Contains(quest.questID)) continue;
                if (quest.requiredLevel > currentSave.player.level) continue;
                if (currentSave.player.currentQuests.Exists(x => x.questID == quest.questID))
                { status = "Active"; break; }
                status = "Can"; break;
            }
            AddBigButtonOverlay("QuestStarter" + (status == "Can" ? "" : (status == "Active" ? "Active" : "Off")), 0, 4);
        }
        if (item.amount == 0) SetBigButtonToGrayscale();
        else if (item.IsWearable() && !item.CanEquip(currentSave.player, true)) SetBigButtonToRed();
        if (stockItem != null || item.maxStack > 1) SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * ((buyback != null ? currentSave.buyback.items.IndexOf(buyback) : currentSave.vendorStock[town.name + ":" + Person.person.name].FindIndex(x => x.item == item.name)) % 5), item.amount + (false && buyback == null ?  "/" + currentSave.vendorStock[town.name + ":" + Person.person.name].Find(x => x.item == item.name).maxAmount : ""), "", "", "Right");
        if (stockItem != null && item.amount == 0 && stockItem.minutesLeft > 0) AddBigButtonCooldownOverlay(stockItem.minutesLeft / (double)stockItem.restockSpeed);
        else if (buyback != null && buyback.minutesLeft > 0) AddBigButtonBuybackOverlay(buyback.minutesLeft / (double)defines.buybackDecay);
    }

    public static void PrintInventoryItem(Item item)
    {
        AddBigButton(item.icon,
            (h) =>
            {
                if (WindowUp("ConfirmItemDisenchant")) return;
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                if (Cursor.cursor.color == "Pink")
                {
                    if (!h.GetComponent<SpriteRenderer>().material.name.Contains("Gray"))
                    {
                        itemToDisenchant = item;
                        Cursor.cursor.ResetColor();
                        PlaySound("DesktopMenuOpen", 0.6f);
                        Respawn("PlayerEquipmentInfo");
                        Respawn("PlayerWeaponsInfo");
                        SpawnWindowBlueprint("ConfirmItemDisenchant");
                    }
                }
                else if (movingItem == null)
                {
                    if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                    {
                        String.splitAmount.Set("");
                        SpawnWindowBlueprint("SplitItem");
                        CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                        movingItemX = h.region.bigButtons.IndexOf(h.GetComponent<LineBigButton>());
                        movingItemY = h.window.headerGroup.regions.IndexOf(h.region) - 1;
                        splitDelegate = () =>
                        {
                            var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                            if (amount >= item.amount) PickUpMovingItem("Inventory", null);
                            else PickUpMovingItem("Inventory", null, amount);
                            Respawn("Inventory", true);
                            Respawn("Bank", true);
                        };
                    }
                    else PickUpMovingItem("Inventory", h);
                }
                else if (movingItem != null)
                    SwapMovingItem(h);
            },
            (h) =>
            {
                if (item == null || itemToDisenchant == item) return;
                if (Cursor.cursor.color == "Pink") return;
                if (WindowUp("ConfirmItemDisenchant")) return;
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                if (CDesktop.windows.Exists(x => x.title.StartsWith("Vendor")))
                {
                    if (item.price > 0)
                        if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                        {
                            String.splitAmount.Set("1");
                            SpawnWindowBlueprint("SplitItem");
                            CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                            splitDelegate = () =>
                            {
                                var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                                if (amount <= 0) return;
                                PlaySound("DesktopTransportPay");
                                PlaySound(item.ItemSound("PutDown"));
                                if (amount > item.amount) amount = item.amount;
                                currentSave.buyback ??= new(true);
                                if (amount == item.amount)
                                {
                                    if (!item.indestructible)
                                        item.minutesLeft = defines.buybackDecay;
                                    currentSave.buyback.AddItem(item);
                                    currentSave.player.inventory.items.Remove(item);
                                }
                                else
                                {
                                    var newItem = item.CopyItem(amount);
                                    currentSave.buyback.AddItem(newItem);
                                    if (!newItem.indestructible)
                                        newItem.minutesLeft = defines.buybackDecay;
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
                            PlaySound(item.ItemSound("PutDown"));
                            currentSave.buyback ??= new(true);
                            currentSave.player.inventory.money += item.price * item.amount;
                            if (!item.indestructible)
                                item.minutesLeft = defines.buybackDecay;
                            currentSave.buyback.AddItem(item);
                            currentSave.player.inventory.items.Remove(item);
                            CloseWindow("Vendor");
                            Respawn("VendorBuyback");
                        }
                }
                else if (WindowUp("Bank"))
                {
                    if (currentSave.banks[town.name].CanAddItem(item))
                        if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                        {
                            String.splitAmount.Set("1");
                            SpawnWindowBlueprint("SplitItem");
                            CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                            splitDelegate = () =>
                            {
                                var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                                if (amount <= 0) return;
                                PlaySound(item.ItemSound("PutDown"), 0.8f);
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
                            PlaySound(item.ItemSound("PutDown"), 0.8f);
                            currentSave.banks[town.name].AddItem(item);
                            currentSave.player.inventory.items.Remove(item);
                            Respawn("Inventory");
                            Respawn("Bank");
                        }
                }
                else
                {
                    if (item.CanEquip(currentSave.player, true))
                    {
                        PlaySound(item.ItemSound("PickUp"), 0.8f);
                        item.Equip(currentSave.player, false, Input.GetKey(KeyCode.LeftAlt));
                        Respawn("Inventory", true);
                        Respawn("PlayerEquipmentInfo", true);
                        Respawn("PlayerWeaponsInfo", true);
                    }
                    else if (item.CanUse(currentSave.player))
                    {
                        PlaySound(item.ItemSound("Use"), 0.8f);
                        item.Use(currentSave.player);
                        Respawn("Inventory", true);
                        Respawn("PlayerEquipmentInfo", true);
                        Respawn("PlayerWeaponsInfo", true);
                    }
                }
            },
            (h) => () =>
            {
                if (item == null) return;
                if (WindowUp("ConfirmItemDisenchant")) return;
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                PrintItemTooltip(item, Input.GetKey(KeyCode.LeftShift));
            },
            (h) =>
            {
                if (openedItem == item) return;
                if (item.indestructible) return;
                if (movingItem != null) return;
                if (WindowUp("ConfirmItemDisenchant")) return;
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                itemToDestroy = item;
                PlaySound("DesktopMenuOpen", 0.6f);
                SpawnWindowBlueprint("ConfirmItemDestroy");
            }
        );
        if (settings.rarityIndicators.Value())
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 3);
        if (item.questsStarted != null)
        {
            var all = Quest.quests.FindAll(x => item.questsStarted.Contains(x.questID)).ToList();
            var status = "Cant";
            foreach (var quest in all)
            {
                if (currentSave.player.completedQuests.Contains(quest.questID)) continue;
                if (quest.requiredLevel > currentSave.player.level) continue;
                if (currentSave.player.currentQuests.Exists(x => x.questID == quest.questID))
                { status = "Active"; break; }
                status = "Can"; break;
            }
            AddBigButtonOverlay("QuestStarter" + (status == "Can" ? "" : (status == "Active" ? "Active" : "Off")), 0, 4);
        }
        if (item.CanEquip(currentSave.player) && currentSave.player.IsItemNewSlot(item) && (settings.upgradeIndicators.Value() || settings.newSlotIndicators.Value()))
            AddBigButtonOverlay(settings.newSlotIndicators.Value() ? "OtherItemNewSlot" : "OtherItemUpgrade", 0, 2);
        else if (settings.upgradeIndicators.Value() && item.CanEquip(currentSave.player) && currentSave.player.IsItemAnUpgrade(item))
            AddBigButtonOverlay("OtherItemUpgrade", 0, 2);
        if (Cursor.cursor.color == "Pink" && !item.IsDisenchantable()) SetBigButtonToGrayscale();
        else if (Cursor.cursor.color == "Pink") AddBigButtonOverlay("OtherGlowDisenchantable" + item.rarity, 0, 2);
        if (openedItem == item || itemToDisenchant == item || itemToDestroy == item) { AddBigButtonOverlay("OtherGridBlurred", 0, 3); SetBigButtonToGrayscale(); }
        if (item.maxStack > 1) SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * item.x, item.amount + "", "", "", "Right");
    }

    public static void PrintLootItem(Item item)
    {
        AddBigButton(item.icon,
            (h) => Click(),
            (h) => Click(),
            (h) => () =>
            {
                if (item == null) return;
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                PrintItemTooltip(item, Input.GetKey(KeyCode.LeftShift));
            }
        );
        if (Board.board != null && Board.board.results.exclusiveItems.Count > 1 && Board.board.results.exclusiveItems.Contains(item.name))
            AddBigButtonOverlay("OtherItemExclusive", 0, 2);
        if (settings.rarityIndicators.Value() && item.type != "Currency")
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 3);
        if (item.questsStarted != null)
        {
            var all = Quest.quests.FindAll(x => item.questsStarted.Contains(x.questID)).ToList();
            var status = "Cant";
            foreach (var quest in all)
            {
                if (currentSave.player.completedQuests.Contains(quest.questID)) continue;
                if (quest.requiredLevel > currentSave.player.level) continue;
                if (currentSave.player.currentQuests.Exists(x => x.questID == quest.questID))
                { status = "Active"; break; }
                status = "Can"; break;
            }
            AddBigButtonOverlay("QuestStarter" + (status == "Can" ? "" : (status == "Active" ? "Active" : "Off")), 0, 4);
        }
        if (item.CanEquip(currentSave.player) && currentSave.player.IsItemNewSlot(item) && (settings.upgradeIndicators.Value() || settings.newSlotIndicators.Value()))
            AddBigButtonOverlay(settings.newSlotIndicators.Value() ? "OtherItemNewSlot" : "OtherItemUpgrade", 0, 2);
        else if (settings.upgradeIndicators.Value() && item.CanEquip(currentSave.player) && currentSave.player.IsItemAnUpgrade(item))
            AddBigButtonOverlay("OtherItemUpgrade", 0, 2);
        if (item.maxStack > 1 && item.type != "Currency") SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * item.x, item.amount + "", "", "", "Right");

        void Click()
        {
            if (movingItem == null && currentSave.player.inventory.CanAddItem(item))
            {
                PlaySound(item.ItemSound("PutDown"), 0.8f);
                if (CDesktop.title == "CombatResultsLoot")
                {
                    currentSave.player.inventory.AddItem(item);
                    Board.board.results.inventory.items.Remove(item);
                    if (Board.board.results.exclusiveItems.Contains(item.name))
                        Board.board.results.inventory.items.RemoveAll(x => Board.board.results.exclusiveItems.Contains(x.name));
                    Board.board.results.inventory.ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && Board.board.results.inventory.items.Count == 0)
                    {
                        CloseDesktop("CombatResultsLoot");
                        SwitchDesktop("CombatResults");
                        Respawn("CombatResults");
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("CombatResultsLoot");
                    }
                }
                else if (CDesktop.title == "MiningLoot")
                {
                    PlaySound("Mining" + random.Next(1, 6));
                    currentSave.player.inventory.AddItem(item);
                    Board.board.results.miningLoot.items.Remove(item);
                    Board.board.results.miningLoot.ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && Board.board.results.miningLoot.items.Count == 0)
                    {
                        CloseDesktop("MiningLoot");
                        Respawn("CombatResultsMining");
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("MiningLoot");
                    }
                }
                else if (CDesktop.title == "HerbalismLoot")
                {
                    PlaySound("HerbGather" + random.Next(1, 5));
                    currentSave.player.inventory.AddItem(item);
                    Board.board.results.herbalismLoot.items.Remove(item);
                    Board.board.results.herbalismLoot.ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && Board.board.results.herbalismLoot.items.Count == 0)
                    {
                        CloseDesktop("HerbalismLoot");
                        Respawn("CombatResultsHerbalism");
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("HerbalismLoot");
                    }
                }
                else if (CDesktop.title == "SkinningLoot")
                {
                    PlaySound("SkinGather" + random.Next(1, 4));
                    currentSave.player.inventory.AddItem(item);
                    Board.board.results.skinningLoots[Board.board.results.selectedSkinningLoot].items.Remove(item);
                    Board.board.results.skinningLoots[Board.board.results.selectedSkinningLoot].ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && Board.board.results.skinningLoots[Board.board.results.selectedSkinningLoot].items.Count == 0)
                    {
                        CloseDesktop("SkinningLoot");
                        Respawn("CombatResultsSkinning1");
                        Respawn("CombatResultsSkinning2");
                        Respawn("CombatResultsSkinning3");
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("SkinningLoot");
                    }
                }
                else if (CDesktop.title == "DisenchantLoot")
                {
                    currentSave.player.inventory.AddItem(item);
                    disenchantLoot.items.Remove(item);
                    disenchantLoot.ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && disenchantLoot.items.Count == 0)
                    {
                        CloseDesktop("DisenchantLoot");
                        CDesktop.RespawnAll();
                    }
                    else Respawn("Inventory");
                }
                else if (CDesktop.title == "ChestLoot")
                {
                    currentSave.player.inventory.AddItem(item);
                    currentSave.openedChests[SiteHostileArea.area.name].inventory.items.Remove(item);
                    currentSave.openedChests[SiteHostileArea.area.name].inventory.ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && currentSave.openedChests[SiteHostileArea.area.name].inventory.items.Count == 0)
                    {
                        if (SiteHostileArea.area.instancePart)
                        {
                            CloseDesktop("Instance");
                            SpawnDesktopBlueprint("Instance");
                        }
                        else
                        {
                            CloseDesktop("HostileArea");
                            SpawnDesktopBlueprint("HostileArea");
                        }
                        CloseDesktop("ChestLoot");
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("ChestLoot");
                    }
                }
                else if (CDesktop.title == "ContainerLoot")
                {
                    currentSave.player.inventory.AddItem(item);
                    openedItem.itemsInside.Remove(item);
                    ApplySortOrder(openedItem.itemsInside);
                    if (settings.autoCloseLoot.Value() && openedItem.itemsInside.Count == 0)
                    {
                        currentSave.player.inventory.items.Remove(openedItem);
                        openedItem = null;
                        CloseDesktop("ContainerLoot");
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("ContainerLoot");
                    }
                }
            }
        }
    }

    //Prints an item's tooltip
    //Left Shift makes item compare appear in the tooltip if a compare is possible
    //Left Control makes the viewed item change to the recipe resulting item if possible
    public static void PrintItemTooltip(Item item, bool compare = false, double priceMultiplier = 1)
    {
        if (CDesktop.title == "Game") SetAnchor(Anchor.Bottom, 0, 37);
        else SetAnchor(-92, 142);
        AddHeaderGroup();
        SetRegionGroupWidth(182);
        if (Input.GetKey(KeyCode.LeftControl) && item.type == "Recipe")
        {
            var recipe = Recipe.recipes.Find(x => item.name.Contains(x.name));
            if (recipe != null) item = items.Find(x => x.name == recipe.results.First().Key).CopyItem(recipe.results.First().Value);
        }
        var split = item.name.Split(", ");
        AddHeaderRegion(() =>
        {
            AddLine(split[0], item.rarity);
            AddSmallButton(item.icon);
        });
        if (split.Length > 1) AddHeaderRegion(() => { AddLine("\"" + split[1] + "\"", item.rarity); });
        AddPaddingRegion(() =>
        {
            if (item.armorClass != null)
            {
                var copy = item.CopyItem(1);
                copy.specs = null;
                AddLine(item.armorClass + " ", currentSave != null && !copy.CanEquip(currentSave.player, true) ? "DangerousRed" : "Gray");
                AddText(item.type);
            }
            else if (item.minPower > 0)
            {
                AddLine(item.type + " " + item.detailedType, currentSave != null && !currentSave.player.abilities.ContainsKey((new List<string> { "Polearm", "Staff", "Bow", "Crossbow", "Gun", "Dagger", "Fist Weapon", "Wand" }.Contains(item.detailedType) ? item.detailedType : item.type + " " + item.detailedType) + " Proficiency") ? "DangerousRed" : "Gray");
                AddLine((item.minPower + "").Replace(",", "."), "Gray");
                AddText(" - ", "HalfGray");
                AddText((item.maxPower + "").Replace(",", "."), "Gray");
                AddText(" Power modifier", "HalfGray");
                AddLine("Average modifier: ", "HalfGray");
                AddText((Math.Round((item.minPower + item.maxPower) / 2, 2) + "").Replace(",", "."), "Gray");
            }
            else if (item.bagSpace != 0) AddLine(item.bagSpace + " Slot Bag");
            else if (item.type == "Recipe")
            {
                var recipe = Recipe.recipes.Find(x => item.name.Contains(x.name));
                AddLine(recipe.profession + " " + item.name.Split(':')[0].ToLower(), currentSave != null && currentSave.player.professionSkills.ContainsKey(recipe.profession) ? "Gray" : "DangerousRed");
            }
            else if (item.type == "Off Hand") AddLine(item.type + (item.detailedType != null ? " " + item.detailedType : ""), currentSave != null && !currentSave.player.abilities.ContainsKey(item.detailedType == "Shield" ? "Shield Proficiency" : "Off Hand Proficiency") ? "DangerousRed" : "Gray");
            else AddLine(item.type ?? "");
            if (item.armor > 0) AddLine(item.armor + " Armor", "", "Right");
        });
        if (item.stats != null && item.stats.Count > 0)
            AddPaddingRegion(() =>
            {
                foreach (var stat in item.stats)
                    AddLine("+" + stat.Value + " " + stat.Key);
            });
        if (compare && item.IsWearable())
        {
            Item current = null;
            Item currentSecond = null;
            if (currentSave != null)
                if (currentSave.player.equipment.ContainsKey(item.type))
                    current = currentSave.player.equipment[item.type];
                else if (item.type == "Two Handed" || item.type == "One Handed")
                {
                    current = currentSave.player.equipment.Get("Main Hand");
                    currentSecond = currentSave.player.equipment.Get("Off Hand");
                }
            AddHeaderRegion(() => AddLine("Stat changes on equip:", "DarkGray"));
            AddPaddingRegion(() =>
            {
                var statsRecorded = new List<string>();
                var a1 = item.armor;
                var a2 = current == null ? 0 : current.armor;
                var a3 = currentSecond == null ? 0 : currentSecond.armor;
                if (a1 - a2 - a3 != 0)
                {
                    var balance = a1 - a2 - a3;
                    AddLine((balance > 0 ? "+" : "") + balance, balance > 0 ? "Uncommon" : "DangerousRed");
                    AddText(" Armor");
                }
                if (item.type == "Ranged Weapon")
                {
                    var newPower = item.minPower <= 0 ? 1 : Math.Round((item.minPower + item.maxPower) / 2, 2);
                    var oldPower = current == null || current.minPower <= 0 ? 1 : Math.Round((current.minPower + current.maxPower) / 2, 2);
                    if (newPower - oldPower != 0)
                    {
                        var balance = Math.Round(newPower - oldPower, 2);
                        AddLine(((balance > 0 ? "+" : "") + balance).Replace(",", "."), balance > 0 ? "Uncommon" : "DangerousRed");
                        AddText(" Power modifier");
                    }
                }
                else if (item.type == "One Handed" || item.type == "Two Handed")
                {
                    var newPower = item.minPower <= 0 ? 1 : (item.minPower + item.maxPower) / 2;
                    var b1d = Math.Round(!Input.GetKey(KeyCode.LeftAlt) || item.type == "Two Handed" ? newPower : current == null || current.minPower <= 0 ? 0 : (Input.GetKey(KeyCode.LeftAlt) && current.type == "Two Handed" ? 0 : (current.minPower + current.maxPower) / 2), 2);
                    var b2d = Math.Round(item.type == "Two Handed" ? 0 : Input.GetKey(KeyCode.LeftAlt) ? newPower : currentSecond == null || currentSecond.minPower <= 0 ? 0 : (currentSecond.minPower + currentSecond.maxPower) / 2, 2);
                    if (b1d == 0 && b2d == 0) b1d = 1;
                    else if (b2d > 0)
                    {
                        b1d /= defines.dividerForDualWield;
                        b2d /= defines.dividerForDualWield;
                    }
                    var bd = Math.Round(b1d + b2d, 2);
                    var a1d = Math.Round(current == null || current.minPower <= 0 ? 0 : (current.minPower + current.maxPower) / 2, 2);
                    var a2d = Math.Round(currentSecond == null || currentSecond.minPower <= 0 ? 0 : (currentSecond.minPower + currentSecond.maxPower) / 2, 2);
                    if (a1d == 0 && a2d == 0) a1d = 1;
                    else if (a2d > 0)
                    {
                        a1d /= defines.dividerForDualWield;
                        a2d /= defines.dividerForDualWield;
                    }
                    var ad = Math.Round(a1d + a2d, 2);
                    if (bd - ad != 0)
                    {
                        var balance = Math.Round(bd - ad, 2);
                        AddLine(((balance > 0 ? "+" : "") + balance).Replace(",", "."), balance > 0 ? "Uncommon" : "DangerousRed");
                        AddText(" Power modifier");
                    }
                }
                a1 = item.block;
                a2 = current == null ? 0 : current.block;
                a3 = current == null ? 0 : current.block;
                if (a1 - a2 - a3 != 0)
                {
                    var balance = a1 - a2 - a3;
                    AddLine((balance > 0 ? "+" : "") + balance, balance > 0 ? "Uncommon" : "DangerousRed");
                    AddText(" Block");
                }
                if (item.stats != null)
                    foreach (var stat in item.stats)
                    {
                        statsRecorded.Add(stat.Key);
                        a2 = current != null && current.stats != null ? current.stats.Get(stat.Key) : 0;
                        a2 = currentSecond != null && currentSecond.stats != null ? current.stats.Get(stat.Key) : 0;
                        var balance = stat.Value - a2 - a3;
                        if (balance != 0)
                        {
                            AddLine((balance > 0 ? "+" : "") + balance, balance > 0 ? "Uncommon" : "DangerousRed");
                            AddText(" " + stat.Key);
                        }
                    }
                var fullLostStats = new Dictionary<string, int>();
                if (current != null && current.stats != null)
                    foreach (var stat in current.stats)
                        if (!statsRecorded.Contains(stat.Key))
                            fullLostStats.Inc(stat.Key, stat.Value);
                if (currentSecond != null && currentSecond.stats != null)
                    foreach (var stat in currentSecond.stats)
                        if (!statsRecorded.Contains(stat.Key))
                            fullLostStats.Inc(stat.Key, stat.Value);
                foreach (var stat in fullLostStats)
                {
                    AddLine("-" + stat.Value, "DangerousRed");
                    AddText(" " + stat.Key);
                }
                if (CDesktop.LBWindow().LBRegionGroup().LBRegion().lines.Count == 0)
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
        if (item.abilities != null)
            foreach (var ability in item.abilities)
            {
                var foo = Ability.abilities.Find(x => x.name == ability.Key);
                foo?.PrintDescription(currentSave.player, 182, ability.Value);
            }
        if (item.questsStarted != null) AddPaddingRegion(() => AddLine("Starts a quest", "HalfGray"));
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
        if (item.type == "Recipe")
        {
            var recipe = Recipe.recipes.Find(x => item.name.Contains(x.name));
            if (recipe.results.Count > 0)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Results:", "DarkGray");
                });
                AddPaddingRegion(() =>
                {
                    foreach (var result in recipe.results)
                        AddLine(result.Key + " x" + result.Value);
                });
            }
            else if (recipe.enchantment)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Enchantment:", "DarkGray");
                });
                var e = Enchant.enchants.Find(x => x.name == recipe.name);
                AddPaddingRegion(() =>
                {
                    AddLine(e.type);
                    AddLine(e.Name());
                });
            }
            AddHeaderRegion(() =>
            {
                AddLine("Reagents:", "DarkGray");
            });
            AddPaddingRegion(() =>
            {
                foreach (var reagent in recipe.reagents)
                    AddLine(reagent.Key + " x" + reagent.Value);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Required skill: ", "DarkGray");
                AddText(recipe.learnedAt + "", currentSave.player.professionSkills.ContainsKey(recipe.profession) && recipe.learnedAt <= currentSave.player.professionSkills[recipe.profession].Item1 ? "Uncommon" : "DangerousRed");
            });
        }
        if (item.enchant != null)
        {
            AddHeaderRegion(() =>
            {
                AddLine("Enchanted: " + item.enchant.Name(), "Uncommon");
            });
            //AddPaddingRegion(() =>
            //{
            //    foreach (var gain in item.enchant.gains)
            //        AddLine(gain.Key + " +" + gain.Value, "Uncommon");
            //});
        }
        if (item.lvl > 1)
            AddHeaderRegion(() =>
            {
                AddLine("Required level: ", "DarkGray");
                AddText(item.lvl + "", ColorRequiredLevel(item.lvl));
            });
        if (item.price > 0) PrintPriceRegion((int)(item.price * priceMultiplier) * (item.type != "Currency" ? 1 : item.amount), 38, 38, 49);
    }

    #endregion

    #region Other

    //This function returns the type of sound that this item makes when it is being manipulated
    public string ItemSound(string soundType)
    {
        string result;
        if (detailedType == "Staff") result = "WoodLarge";
        else if (detailedType == "Wand") result = "Wand";
        else if (detailedType == "Totem") result = "WoodLarge";
        else if (detailedType == "Bow") result = "WoodLarge";
        else if (detailedType == "Crossbow") result = "WoodLarge";
        else if (detailedType == "Gun") result = "MetalSmall";
        else if (detailedType == "Libram") result = "Ring";
        else if (detailedType == "Idol") result = "Ring";
        else if (detailedType == "Gem") result = "Gems";
        else if (detailedType == "Fish") result = "Meat";
        else if (detailedType == "Book") result = "Book";
        else if (detailedType == "Scepter") result = "Wand";
        else if (detailedType == "Lantern") result = "Wand";
        else if (detailedType == "Orb") result = "Wand";
        else if (detailedType == "Pouch") result = "Bag";
        else if (detailedType == "Potion") result = "Liquid";
        else if (detailedType == "Flowers") result = "Herb";
        else if (detailedType == "Torch") result = "WoodSmall";
        else if (detailedType == "Tool") result = "MetalSmall";
        else if (detailedType == "Quiver") result = "ClothLeather";
        else if (detailedType == "Shield") result = "MetalLarge";
        else if (detailedType == "Scroll") result = "ParchmentPaper";
        else if (detailedType == "Beak") result = "FoodGeneric";
        else if (detailedType == "Scale") result = "FoodGeneric";
        else if (detailedType == "Egg") result = "FoodGeneric";
        else if (detailedType == "Shell") result = "FoodGeneric";
        else if (detailedType == "Rune") result = "MetalSmall";
        else if (detailedType == "Dust") result = "Herb";
        else if (detailedType == "Rock") result = "RocksOre";
        else if (detailedType == "Ore") result = "RocksOre";
        else if (detailedType == "Bullet") result = "RocksOre";
        else if (detailedType == "Arrow") result = "WoodSmall";
        else if (detailedType == "Ingot") result = "MetalSmall";
        else if (detailedType == "Claw") result = "Meat";
        else if (detailedType == "Organ") result = "Meat";
        else if (detailedType == "Leather") result = "ClothLeather";
        else if (detailedType == "Essence") result = "Herb";
        else if (detailedType == "Box") result = "WoodSmall";
        else if (detailedType == "Cask") result = "WoodSmall";
        else if (detailedType == "Crate") result = "WoodSmall";
        else if (detailedType == "Crown") result = "MetalSmall";
        else if (detailedType == "Shard") result = "Gems";
        else if (detailedType == "Cloth") result = "ClothLeather";
        else if (detailedType == "Feather") result = "WoodSmall";
        else if (detailedType == "Letter") result = "ParchmentPaper";
        else if (detailedType == "Note") result = "ParchmentPaper";
        else if (detailedType == "Bandage") result = "ClothLeather";
        else if (detailedType == "Candle") result = "WoodSmall";
        else if (detailedType == "Drum") result = "WoodSmall";
        else if (detailedType == "Coin") result = "";
        else if (detailedType == "Key") result = "MetalSmall";
        else if (detailedType == "Horn") result = "MetalSmall";
        else if (detailedType == "Pick") result = "MetalLarge";
        else if (type == "Recipe") result = "ParchmentPaper";
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
        else if (armorClass == "Plate") result = "ChainLarge";
        else result = "ClothLeather";
        return soundType + result;
    }

    //Sets a random permanent enchant for the item
    public void SetRandomEnchantment()
    {
        if (!randomEnchantment) return;
        var enchantment = GenerateEnchantment();
        if (enchantment == null) return;
        name += " " + enchantment.suffix;
        if (enchantment.stats.Count > 0)
        {
            stats = new();
            foreach (var stat in enchantment.stats)
                stats.Inc(stat.Key, EnchantmentStatGrowth(ilvl, stat.Value.Length));
        }

        PermanentEnchant GenerateEnchantment()
        {
            var containing = new List<PermanentEnchant>();
            var key = "";
            if (type == "One Handed" || type == "Two Handed") key = type + " " + detailedType;
            else if (armorClass != null) key = armorClass + " Armor";
            else if (type == "Off Hand") key = type;
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

    //Copies this item with a specific amount
    public Item CopyItem(int amount = 1)
    {
        var newItem = new Item();
        newItem.abilities = abilities?.ToDictionary(x => x.Key, x => x.Value);
        newItem.amount = amount;
        newItem.armor = armor;
        newItem.armorClass = armorClass;
        newItem.bagSpace = bagSpace;
        newItem.block = block;
        newItem.detailedType = detailedType;
        newItem.droppedBy = droppedBy;
        newItem.dropRange = dropRange;
        newItem.faction = faction;
        newItem.combatUse = combatUse;
        newItem.icon = icon;
        newItem.ilvl = ilvl;
        newItem.lvl = lvl;
        newItem.minPower = minPower;
        newItem.maxPower = maxPower;
        newItem.maxStack = maxStack;
        newItem.minutesLeft = minutesLeft;
        newItem.name = name;
        newItem.price = price;
        newItem.rarity = rarity;
        newItem.randomEnchantment = randomEnchantment;
        newItem.reputationRequired = reputationRequired;
        newItem.set = set;
        newItem.source = source;
        newItem.indestructible = indestructible;
        newItem.unique = unique;
        newItem.specs = specs?.ToList();
        newItem.questsStarted = questsStarted?.ToList();
        newItem.stats = stats != null ? stats.ToDictionary(x => x.Key, x => x.Value) : null;
        newItem.type = type;
        return newItem;
    }

    #endregion

    //Currently opened item
    public static Item item;
    
    //Item selected to be disenchanted
    public static Item itemToDisenchant;

    //Item chosen to be destroyed
    public static Item itemToDestroy;

    //Currently opened container item
    public static Item openedItem;

    //EXTERNAL FILE: List containing all buffs in-game
    public static List<Item> items;

    //List of all filtered items by input search
    public static List<Item> itemsSearch;
}
