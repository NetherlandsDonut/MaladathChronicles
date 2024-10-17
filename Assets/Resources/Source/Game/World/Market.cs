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
        hoursSinceUpdate = 48;
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
        //As long as there are leftover hours of update..
        while (hoursSinceUpdate > 0)
        {
            //Decrease hours left for all active auctions by one
            //and remove all of which time is up
            for (int i = auctions.Count - 1; i >= 0; i--)
                if (--auctions[i].hoursLeft <= 0)
                    auctions.RemoveAt(i);

            //Go through all possible items that can be auctioned
            //and check which were lucky to be have someone auction them
            foreach (var foo in auctionables)
                if (Roll(foo.frequency))
                    auctions.Add(new Auction(name, foo));

            //Decrease timer by one
            hoursSinceUpdate -= 1;
        }
    }

    public static List<Auction> exploredAuctions;

    public static Dictionary<string, List<Auction>> exploredAuctionsGroups;
}
