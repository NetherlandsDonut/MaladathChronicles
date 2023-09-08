using System.Linq;
using System.Collections.Generic;

//Iventory is a space for storing money and items
//It's used by entities and banks
public class Inventory
{
    public Inventory() { }
    public Inventory(List<string> items)
    {
        this.items = items.Select(x => Item.GetItem(x)).ToList();
    }

    //Amount of money in the bags
    public double money;

    //List of all items contained in the bags
    public List<Item> items;
}
