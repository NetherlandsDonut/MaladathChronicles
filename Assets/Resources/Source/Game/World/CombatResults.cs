using System.Collections.Generic;

public class CombatResults
{
    public CombatResults(string result)
    {
        this.result = result;
        inventory = new(true);
        exclusiveItems = new();
    }

    public string result;

    public int experience;

    public Inventory inventory;

    public List<string> exclusiveItems;
}
