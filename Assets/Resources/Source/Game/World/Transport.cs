using static Root;
using static Root.Anchor;

public class Transport
{
    public string means, destination;
    public double price;

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
