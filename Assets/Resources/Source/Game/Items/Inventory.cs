using System.Linq;
using System.Collections.Generic;

using static Quest;
using static Defines;

//Iventory is a space for storing money and items
//It's used by entities and banks
public class Inventory
{
    #region Creation

    public Inventory()
    {
        bags ??= new();
        items ??= new();
    }

    public Inventory(bool ignoreSpaceChecks)
    {
        this.ignoreSpaceChecks = ignoreSpaceChecks;
        bags = new();
        items = new();
    }

    public Inventory(List<string> items)
    {
        bags = new();
        this.items = items.Select(x => Item.items.Find(y => y.name == x).CopyItem(1)).ToList();
    }

    #endregion

    #region Manipulation

    //Removes a specific amount of an item from the inventory
    public void RemoveItem(string name, int amount)
    {
        int left = amount;
        var matching = items.FindAll(x => x.name == name);
        var sumBefore = matching.Sum(x => x.amount);
        var sumAfter = sumBefore - left < 0 ? 0 : sumBefore - left;
        for (int i = matching.Count - 1; i >= 0 && left > 0; i--)
        {
            var temp = matching[i].amount;
            matching[i].amount -= matching[i].amount >= left ? left : matching[i].amount;
            if (matching[i].amount == 0)
            {
                items.Remove(matching[i]);
                matching.Remove(matching[i]);
            }
            left -= temp;
        }
        if (SaveGame.currentSave != null && SaveGame.currentSave.player.inventory == this)
        {
            var changed = new List<ActiveQuest>();
            var output = name + ": ";
            foreach (var quest in SaveGame.currentSave.player.currentQuests)
                foreach (var con in quest.conditions)
                    if (con.type == "Item" && con.name == name)
                    {
                        if (sumBefore != sumAfter && (sumBefore < con.amount || sumAfter < con.amount))
                        {
                            if (output.EndsWith(" ")) output += sumAfter + "/" + con.amount;
                            else output += ", " + sumAfter + "/" + con.amount;
                            if (sumBefore >= con.amount && sumAfter < con.amount)
                                changed.Add(quest);
                        }
                    }
            foreach (var quest in changed.Select((x => quests.Find(y => y.questID == x.questID))))
                quest.UpdateRelatedSites();
        }
    }

    //Removes a specific amount of an item from the inventory
    public void RemoveItem(Item item)
    {
        var sumBefore = items.Where(x => x.name == item.name).Sum(x => x.amount);
        var sumAfter = sumBefore - item.amount;
        items.Remove(item);
        if (SaveGame.currentSave != null && SaveGame.currentSave.player.inventory == this)
        {
            var changed = new List<ActiveQuest>();
            var output = item.name + ": ";
            foreach (var quest in SaveGame.currentSave.player.currentQuests)
                foreach (var con in quest.conditions)
                    if (con.type == "Item" && con.name == item.name)
                    {
                        if (sumBefore != sumAfter && (sumBefore < con.amount || sumAfter < con.amount))
                        {
                            if (output.EndsWith(" ")) output += sumAfter + "/" + con.amount;
                            else output += ", " + sumAfter + "/" + con.amount;
                            if (sumBefore >= con.amount && sumAfter < con.amount)
                                changed.Add(quest);
                        }
                    }
            foreach (var quest in changed.Select((x => quests.Find(y => y.questID == x.questID))))
                quest.UpdateRelatedSites();
        }
    }

    //Tells whether the player can fit the item in the inventory
    public bool CanAddItem(Item item)
    {
        if (ignoreSpaceChecks) return true;
        if (item.type == "Currency") return true;
        if (items.Count < BagSpace()) return true;
        var find = items.FindAll(x => x.name == item.name);
        if (find.Count > 0) return find.Sum(x => x.maxStack - x.amount) > 0;
        else return false;
    }

    //Tells whether the player can fit specific items in the inventory
    public bool CanAddItems(List<Item> list)
    {
        if (ignoreSpaceChecks) return true;
        int emptySlots = BagSpace() - items.Count;
        if (items.Count + list.Count < emptySlots) return true;
        var copyList = list.Select(x => x.CopyItem(x.amount)).ToList();
        var copyInventory = items.Select(x => x.CopyItem(x.amount)).ToList();
        foreach (var item in copyList)
        {
            if (item.type == "Currency") continue;
            var slots = copyInventory.FindAll(x => x.name == item.name);
            foreach (var depositSlot in slots)
                if (depositSlot.amount < depositSlot.maxStack)
                {
                    var howMuch = depositSlot.maxStack - depositSlot.amount;
                    if (howMuch <= item.amount)
                    {
                        depositSlot.amount += howMuch;
                        item.amount -= howMuch;
                    }
                    else
                    {
                        depositSlot.amount += item.amount;
                        item.amount = 0;
                        break;
                    }
                }
            if (item.amount > 0)
            {
                copyInventory.Add(item);
                if (--emptySlots < 0)
                    return false;
            }
        }
        return true;
    }

    //Adds item to the inventory and automatically fills stacks
    public void AddItem(Item item)
    {
        if (item.type == "Currency")
            if (item.name == "Gold" || item.name == "Silver" || item.name == "Copper")
            {
                Sound.PlaySound("DesktopTransportPay");
                money += item.price * item.amount;
                return;
            }
        var sumBefore = items.Sum(x => x.name == item.name ? x.amount : 0);
        var added = item.amount;
        var find = items.FindAll(x => x.name == item.name);
        foreach (var stack in find)
        {
            var free = stack.maxStack - stack.amount;
            if (free > 0)
                if (item.amount > free) (item.amount, stack.amount) = (item.amount - free, stack.maxStack);
                else { (item.amount, stack.amount) = (0, stack.amount + item.amount); break; }
        }
        if (item.amount > 0 && (ignoreSpaceChecks || items.Count < BagSpace()))
            AddNewItem(item.CopyItem(item.amount));
        if (SaveGame.currentSave != null && SaveGame.currentSave.player.inventory == this)
        {
            var changed = new List<ActiveQuest>();
            var output = item.name + ": ";
            foreach (var quest in SaveGame.currentSave.player.currentQuests)
                foreach (var con in quest.conditions)
                    if (con.type == "Item" && con.name == item.name)
                    {
                        if (sumBefore < con.amount)
                        {
                            if (output.EndsWith(" ")) output += sumBefore + added + "/" + con.amount;
                            else output += ", " + sumBefore + added + "/" + con.amount;
                            if (sumBefore + added >= con.amount)
                                changed.Add(quest);
                        }
                    }
            if (!output.EndsWith(" "))
                Root.SpawnFallingText(new UnityEngine.Vector2(0, 34), output, "Yellow");
            foreach (var quest in changed.Select((x => quests.Find(y => y.questID == x.questID))))
                quest.UpdateRelatedSites();
        }
    }

    //Extension method for adding new items to the list of items
    //This extension automatically asigns a spot in the inventory for the item
    public void AddNewItem(Item item)
    {
        item.x = -1;
        item.y = -1;
        items.Add(item);
        for (int j = 0; j < 6; j++)
            for (int i = 0; i < 5; i++)
                if (items.All(x => x.x != i || x.y != j))
                {
                    (item.x, item.y) = (i, j);
                    return;
                }
    }

    //After sorting the items in the inventory
    //this method applies the order of them and saves it
    public void ApplySortOrder() => ApplySortOrder(items);

    //After sorting the items in the inventory
    //this method applies the order of them and saves it
    public static void ApplySortOrder(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
            (items[i].x, items[i].y) = (i % 5, i / 5);
    }

    //Decays items that have duration left of their existance
    //This is used mainly for buyback items from vendors
    //BEWARE NOT TO DECAY PLAYER INVENTORY, because of quest items
    //and issues regarding split items resetting the timer
    public void DecayItems(int minutes)
    {
        for (int i = items.Count - 1; i >= 0; i--)
            if (items[i].minutesLeft > 0)
            {
                items[i].minutesLeft -= minutes;
                if (items[i].minutesLeft <= 0)
                    items.RemoveAt(i);
            }
    }

    #endregion

    #region Storage

    //Amount of money in the bags
    public int money;

    //List of all items contained in the bags
    public List<Item> items;

    //Bags equipped in this inventory
    public List<Item> bags;

    #endregion

    #region Management

    //Returns the amount of bag space inventory has
    public int BagSpace()
    {
        var theorySpace = bags.Sum(x => x.bagSpace) + defines.backpackSpace;
        return theorySpace > 30 ? 30 : theorySpace;
    }

    //If true, functions don't look at empty space in bags
    public bool ignoreSpaceChecks;

    #endregion

    //Loot generated by disenchanting
    public static Inventory disenchantLoot;

    //Did player already receive change in skill after disenchanting
    public static bool enchantingSkillChange;
}
