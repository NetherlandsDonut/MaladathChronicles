using System.Collections.Generic;

public class Person
{
    public string name, gender, race, faction, type;

    //Play a sound by this vendor
    public string VendorSound(bool welcome)
    {
        //BASE IT ON RACE AND BOOL
        return "nosound";
    }
    
    //Based on this variable vendor's stock will be resupplied
    public List<string> itemsSold;

    //Currently opened person
    public static Person person;
}

public class PersonType
{
    public string name, icon, type, classRestriction;

    public static List<PersonType> personTypes;
}