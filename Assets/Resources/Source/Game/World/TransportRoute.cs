using System.Collections.Generic;

using static Root;
using static Sound;
using static SitePath;
using static SaveGame;

public class TransportRoute
{
    //Initialisation method to fill automatic values
    public void Initialise()
    {
        foreach (var site in sites)
            if (!transportationConnectedToSite.ContainsKey(site))
                transportationConnectedToSite.Add(site, new() { this });
            else if (!transportationConnectedToSite[site].Contains(this))
                transportationConnectedToSite[site].Add(this);
    }

    //Type of traveling that the entity takes while on this path
    public string means;

    //Price to pay for transport
    public int price;

    //Amount of time that passes after taking this transport route
    public int time;

    //Sites that are connected together
    public List<string> sites;

    //Play the sound at the end of the travel
    public void PlayPathEndSound()
    {
        if (means == "Tram") PlaySound("TramStop", 0.6f);
        else if (means == "Zeppelin") PlaySound("ZeppelinStop", 0.45f);
        else if (means == "Ship") PlaySound("ShipStop", 0.45f);
        else if (means == "DarnassusTeleport") PlaySound("TeleportStop", 0.5f);
    }

    //Transport mouseover information
    public void PrintTooltip()
    {
        var destination = Site.FindSite(x => x.name == sites.Find(x => x != currentSave.currentSite && (SiteArea.area == null || x != SiteArea.area.name)));
        SetAnchor(Anchor.Center);
        AddHeaderGroup();
        SetRegionGroupWidth(182);
        AddPaddingRegion(() =>
        {
            AddLine("Take a " + (means.ToLower().Contains("teleport") ? "teleporter" : means.ToLower()) + " to:", "DarkGray");
        });
        AddHeaderRegion(() =>
        {
            AddLine(destination.capitalRedirect ?? destination.convertDestinationTo ?? destination.name);
            var zone = Zone.zones.Find(x => x.name == destination.zone);
            AddSmallButton("Zone" + zone.icon.Clean());
        });
        if (price > 0)
        {
            AddPaddingRegion(() =>
            {
                AddLine("For the price of:", "DarkGray");
            });
            PrintPriceRegion(price, 38, 38, 49);
        }
    }

    //EXTERNAL FILE: List containing all paths in-game
    public static List<TransportRoute> transportRoutes;
}
