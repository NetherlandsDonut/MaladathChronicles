using System.Collections.Generic;

public class SiteTown
{
    public string name, zone, faction, ambience;
    public List<Transport> transport;
    public List<Vendor> vendors;

    public static SiteTown town;
    public static List<SiteTown> towns;
}
