using System.Linq;
using System.Collections.Generic;

public class Inventory
{
    public Inventory() { }
    public Inventory(List<string> items)
    {
        this.items = items.Select(x => Item.GetItem(x)).ToList();
    }

    public double money;
    public List<Item> items;
}
