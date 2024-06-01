using UnityEngine;

public class QuestCondition
{
    //Type of the condition this is
    //[Kill, Item]
    public string type;

    //Name of the thing that needs to be done
    public string name;

    //Amount of progress required for completion
    public int amount;

    //Amount that was already done when the quest was accepted
    public int storedAmount;
}
