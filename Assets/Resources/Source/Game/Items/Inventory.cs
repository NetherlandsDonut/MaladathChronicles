using System.Linq;
using System.Collections.Generic;

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

    //Tells whether the player can fit the item in the inventory
    public bool CanAddItem(Item item)
    {
        if (ignoreSpaceChecks) return true;
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
        var find = items.FindAll(x => x.name == item.name);
        foreach (var stack in find)
        {
            var free = stack.maxStack - stack.amount;
            if (free > 0)
                if (item.amount > free) (item.amount, stack.amount) = (item.amount - free, stack.maxStack);
                else { (item.amount, stack.amount) = (0, stack.amount + item.amount); break; }
        }
        if (item.amount > 0 && (ignoreSpaceChecks || items.Count < BagSpace()))
            items.Add(item.CopyItem(item.amount));
    }

    //Decays items that have duration left of their existance
    //This is used mainly for buyback items from vendors
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
        return bags.Sum(x => x.bagSpace) + defines.backpackSpace;
    }

    //If true, functions don't look at empty space in bags
    public bool ignoreSpaceChecks;

    //Relinks references to static lists for items loaded from saved games
    public void RelinkReferences()
    {
        foreach (var item in items)
            item.RelinkReferences();
    }

    //Relinks references to static lists for items loaded from saved games
    public void DelinkReferences()
    {
        foreach (var item in items)
            item.RelinkReferences();
    }

    #endregion
}
