using System.Linq;
using System.Collections.Generic;

using static Defines;

//Iventory is a space for storing money and items
//It's used by entities and banks
public class Inventory
{
    public Inventory() { bags ??= new(); items ??= new(); }
    public Inventory(List<string> items)
    {
        bags = new();
        this.items = items.Select(x => Item.GetItem(x)).ToList();
    } 

    //Amount of money in the bags
    public double money;

    //List of all items contained in the bags
    public List<Item> items;

    //Bags equipped in this inventory
    public List<Item> bags;

    public int BagSpace()
    {
        return bags.Sum(x => x.bagSpace) + defines.backpackSpace;
    }
}
