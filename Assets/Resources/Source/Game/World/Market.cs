using System.Collections.Generic;

using static Root;
using static Auctionable;

public class Market
{
    public Market() { }

    public Market(string name)
    {
        this.name = name;
        auctions = new();
        hoursSinceUpdate = 24;
    }

    //Name of the market
    public string name;

    //List of all auctions on this market
    public List<Auction> auctions;

    //Hours since this market was last updated
    public int hoursSinceUpdate;

    //Updates the market
    public void UpdateMarket()
    {
        if (hoursSinceUpdate >= 24)
        {
            hoursSinceUpdate = 24;
            auctions = new();
        }

        //As long as there are leftover hours of update..
        while (hoursSinceUpdate > 0)
        {
            //Decrease hours left for all active auctions by one
            //and remove all of which time is up
            for (int i = auctions.Count - 1; i >= 0; i--)
                if (--auctions[i].hoursLeft <= 0 || auctions[i].item.amount <= 0)
                    auctions.RemoveAt(i);

            //Go through all possible items that can be auctioned
            //and check which were lucky to be auctioned by someone
            foreach (var foo in auctionables)
                if (Roll(foo.frequency))
                    auctions.Add(new Auction(foo));

            //Decrease timer by one
            hoursSinceUpdate--;
        }
    }

    public static string auctionGroup;

    public static List<Auction> exploredAuctions;

    public static Dictionary<string, List<Auction>> exploredAuctionsGroups;
}
