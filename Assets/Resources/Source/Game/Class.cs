using System.Collections.Generic;

public class Class
{
    public Class(string name, List<(string, int)> abilities)
    {
        this.name = name;
        this.abilities = abilities;
    }

    public string name;
    public List<(string, int)> abilities;

    public static List<Class> classes = new()
    {
        new Class("Rogue",
            new List<(string, int)>
            {
                ("Envenom", 1),
                ("Kidney Shot", 1),
                ("Mutilate", 1),
                ("Evasion", 1),
                ("Garrote", 1),
                ("Rupture", 1)
            }
        ),
    };
}
