using System.Collections.Generic;

public class Ability
{
    public Ability(string name, int levelRequired, Dictionary<string, int> cost)
    {
        this.name = name;
        this.levelRequired = levelRequired;
        this.cost = cost;
    }

    public string name;
    public int levelRequired;
    public Dictionary<string, int> cost;

    public static List<Ability> abilities = new()
    {
        new Ability("Envenom", 1, new()
        {
            { "Order", 1 }
        }),
        new Ability("Rupture", 1, new()
        {
            { "Order", 1 }
        }),
        new Ability("Mutilate", 1, new()
        {
            { "Order", 1 }
        }),
        new Ability("Kidney Shot", 1, new()
        {
            { "Order", 1 }
        }),
        new Ability("Evasion", 1, new()
        {
            { "Order", 1 }
        }),
        new Ability("Garrote", 1, new()
        {
            { "Order", 1 }
        }),
    };
}
