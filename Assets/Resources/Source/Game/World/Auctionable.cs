using System.Collections.Generic;

public class Auctionable
{
    //Name of the item
    public string item;

    //Minimum price this item will be auctioned at
    public int minPrice;

    //Maximum price this item will be auctioned at
    public int maxPrice;

    //Chance to see this item enter market each hour
    public float frequency;

    //Currently opened auctionable
    public static Auctionable auctionable;

    //EXTERNAL FILE: List containing all auctionables in-game
    public static List<Auctionable> auctionables;

    //List of all filtered auctionables by input search
    public static List<Auctionable> auctionablesSearch;
}
