using System.Collections.Generic;

public class Class
{
    public Class(string name, List<string> abilities)
    {
        this.name = name;
        this.abilities = abilities;
    }

    public string name;
    public List<string> abilities;

    public static List<Class> classes = new()
    {
        new Class("Rogue",
            new List<string>
            {
                "Envenom",
                "Kidney Shot",
                "Mutilate",
                "Evasion",
                "Garrote",
                "Rupture",
            }
        ),
    };
}
