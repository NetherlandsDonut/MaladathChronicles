using System.Linq;

using static SaveGame;

public class QuestCondition
{
    //Type of the condition this is
    //[Kill, Item, Visit]
    public string type;

    //Name of the thing that needs to be done
    public string name;

    //Amount of progress required for completion
    public int amount;

    //Amount that was already done when the quest was accepted
    public int amountDone;

    //Checks whether player already done the quest
    public string status;

    //Checks whether this condition is already fulfilled
    public bool IsDone()
    {
        if (type == "Item")
            return currentSave.player.inventory.items.Sum(x => x.name == name ? x.amount : 0) >= amount;
        return status == "Done";
    }

    //Checks whether this condition is already fulfilled
    public string Print()
    {
        if (type == "Item") return name + ": " + amountDone + " / " + amount;
        else if (type == "Kill") return name + ": " + amountDone + " / " + amount;
        else if (type == "Visit") return name + " visited: " + (status == "Done" ? 1 : 0) + " / 1";
        return "";
    }
}
