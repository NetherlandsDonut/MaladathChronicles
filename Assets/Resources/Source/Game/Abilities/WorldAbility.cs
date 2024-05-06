using System.Linq;
using System.Collections.Generic;

using static Root;

public class WorldAbility
{
    #region Initialisation

    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        if (item != null)
        {
            var foo = Item.items.Find(x => x.name == item);
            if (foo != null)
            {
                foo.worldAbilities ??= new();
                if (!foo.worldAbilities.Contains(this))
                    foo.worldAbilities.Add(this);
            }
        }
        worldEvents ??= new();
    }

    #endregion

    #region Execution

    public void ExecuteEvents(Item item, Dictionary<string, string> trigger)
    {
        //In case of this ability having no events just return
        if (worldEvents == null) return;
        foreach (var eve in worldEvents)
        {
            bool execute = false;
            foreach (var triggerData in eve.triggers)
                if (triggerData.ContainsKey("Trigger") && triggerData["Trigger"] == trigger["Trigger"])
                {
                    if (trigger["Trigger"] == "WorldBuffAdd" || trigger["Trigger"] == "WorldBuffRemove")
                    {
                        string buffName = trigger.ContainsKey("BuffName") ? trigger["BuffName"] : "None";
                        string buffNameData = triggerData.ContainsKey("BuffName") ? triggerData["BuffName"] : "None";
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = (buffName == buffNameData || buffNameData == "Any") && triggerer == triggererData;
                    }
                    else if (trigger["Trigger"] == "ItemUsed")
                    {
                        string itemName = trigger.ContainsKey("ItemName") ? trigger["ItemName"] : "None";
                        string itemNameData = triggerData.ContainsKey("ItemName") ? (triggerData["ItemName"] == "This" ? this.item : triggerData["ItemName"]) : this.item;
                        execute = itemName == itemNameData || itemNameData == "Any";
                    }
                }
            if (execute)
            {
                eve.ExecuteEffects(icon, trigger, item);
            }
        }
    }

    #endregion

    #region Description

    public void PrintDescription(Entity effector, int width, bool showEmpty = true)
    {
        if (description != null) description.Print(effector, null, width, null);
        else if (showEmpty) AddHeaderRegion(() =>
        {
            SetRegionAsGroupExtender();
            AddLine("No description", "DarkGray");
        });
    }

    #endregion

    //Item using this ability
    public string item;

    //Icon of the ability for shatter effects?
    public string icon;

    //Provides information on how many turns will the ability be disabled after casting
    public string cooldownGroup;

    //List of world events this world ability has
    //This is essentially all the ability's effects with it's triggerers that make them happen
    public List<WorldEvent> worldEvents;

    //Description of the buff to show on hover in the status bar
    public Description description;

    //Currently opened world ability
    public static WorldAbility worldAbility;

    //EXTERNAL FILE: List containing all world abilities in-game
    public static List<WorldAbility> worldAbilities;

    //List of all filtered world abilities by input search
    public static List<WorldAbility> worldAbilitiesSearch;
}
