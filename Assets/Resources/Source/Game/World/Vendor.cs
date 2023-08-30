using System.Collections.Generic;

public class Vendor
{
    public string name, race, type, welcomeSound, goodbyeSound;

    public string VendorSound(bool welcome)
    {
        //BASE IT ON RACE AND BOOL
        return "nosound";
    }

    public List<string> itemsSold;
}

public class VendorType
{
    public string name, icon;

    public static List<VendorType> vendorTypes;
}