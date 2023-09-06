using System.Collections.Generic;

public class Person
{
    public string name, gender, race, faction, type;

    public string VendorSound(bool welcome)
    {
        //BASE IT ON RACE AND BOOL
        return "nosound";
    }
    
    public List<string> itemsSold;
}

public class PersonType
{
    public string name, icon, classRestriction;

    public static List<PersonType> personTypes;
}