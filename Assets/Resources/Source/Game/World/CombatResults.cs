using System.Collections.Generic;

public class CombatResults
{
    public CombatResults(string result)
    {
        this.result = result;
        items = new();
        exclusiveItems = new();
    }

    public string result;

    public int experience;

    public double money;

    public List<Item> items;

    public List<string> exclusiveItems;
}
