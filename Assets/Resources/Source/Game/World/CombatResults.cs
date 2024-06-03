using System.Collections.Generic;

public class CombatResults
{
    public CombatResults(string result)
    {
        this.result = result;
        inventory = new(true);
        exclusiveItems = new();
    }

    //Result of the combat
    public string result;

    //Amount of experience awarded to the player
    public int experience;

    //Dropped items from the enemy
    public Inventory inventory;

    //?
    public List<string> exclusiveItems;
}
