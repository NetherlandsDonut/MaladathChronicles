using static Root;
using static Root.Anchor;

public class Transport
{
    //Means of transportation
    //Based on this value transport icon is chosen
    public string means;

    //Destination of the transportation
    public string destination;

    //Price to be payed by passengers to travel
    public double price;

    //Transport mouseover information
    public static void PrintTransportTooltip(Transport transport)
    {
        SetAnchor(Center);
        AddHeaderGroup();
        SetRegionGroupWidth(188);
        AddHeaderRegion(() => { AddLine(transport.means); });
        AddPaddingRegion(() => { AddLine("To " + transport.destination); });
        PrintPriceRegion(transport.price);
    }
}
