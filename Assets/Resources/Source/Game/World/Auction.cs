using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

using static Root;

public class Auction
{
    public Auction() { }

    public Auction(Auctionable auctionable)
    {
        price = random.Next(auctionable.minPrice, auctionable.maxPrice);
        var foo = Item.items.Find(x => x.name == auctionable.item);
        item = foo.CopyItem(random.Next(1, foo.maxStack + 1));
        hoursLeft = 48;
    }

    //Auctioned items
    public Item item;

    //Unit price for the auctioned item
    public int price;

    //Amount of hours left for this auction to be active
    public int hoursLeft;
}
