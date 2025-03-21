using static Root;

public class Auction
{
    public Auction() { }

    public Auction(string targetMarket, Auctionable auctionable)
    {
        market = targetMarket;
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

    //Market on which the auction is placed
    public string market;
}
