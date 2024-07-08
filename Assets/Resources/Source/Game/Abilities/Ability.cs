using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Anchor;

public class Ability
{
    #region Initialisation

    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        ranks ??= new();
        if (ranks.Count == 0)
            ranks.Add(new());
        events ??= new();
        tags ??= new();
    }

    #endregion

    #region Resource Check

    public bool EnoughResources(Entity entity) => EnoughResources(entity.resources);
    public bool EnoughResources(FutureEntity entity) => EnoughResources(entity.resources);
    public bool EnoughResources(Dictionary<string, int> resources) => !cost.Any(x => x.Value > resources[x.Key]);

    #endregion

    #region Execution

    public void ExecuteEvents(SaveGame save, Dictionary<string, string> trigger, Item item, int abilityRank)
    {
        //In case of this ability having no events just return
        if (events == null) return;
        foreach (var eve in events)
        {
            bool execute = false;
            foreach (var triggerData in eve.triggers)
                if (triggerData.ContainsKey("Trigger") && triggerData["Trigger"] == trigger["Trigger"])
                {
                    if (trigger["Trigger"] == "ItemUsed")
                    {
                        string itemHash = trigger.ContainsKey("ItemHash") ? trigger["ItemHash"] : "";
                        execute = item != null && item.GetHashCode() + "" == itemHash;
                    }
                }
            if (execute) eve.ExecuteEffects(save, item, trigger, RankVariables(abilityRank), this, abilityRank);
        }
    }

    public void ExecuteEvents(Board board, FutureBoard futureBoard, Dictionary<string, string> trigger, Item item, int abilityRank, bool sourcedFromPlayer)
    {
        //In case of this ability having no events just return
        if (events == null) return;
        foreach (var eve in events)
        {
            bool execute = false;
            foreach (var triggerData in eve.triggers)
                if (triggerData.ContainsKey("Trigger") && triggerData["Trigger"] == trigger["Trigger"])
                {
                    if (trigger["Trigger"] == "BuffAdd" || trigger["Trigger"] == "BuffRemove" || trigger["Trigger"] == "BuffFlare")
                    {
                        string buffName = trigger.ContainsKey("BuffName") ? trigger["BuffName"] : "None";
                        string buffNameData = triggerData.ContainsKey("BuffName") ? triggerData["BuffName"] : "None";
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = (buffName == buffNameData || buffNameData == "Any") && triggerer == triggererData;
                    }
                    else if (trigger["Trigger"] == "ResourceCollected" || trigger["Trigger"] == "ResourceDetracted")
                    {
                        string resourceType = trigger.ContainsKey("ResourceType") ? trigger["ResourceType"] : "None";
                        string resourceTypeData = triggerData.ContainsKey("ResourceType") ? triggerData["ResourceType"] : "None";
                        string compareData = triggerData.ContainsKey("Compare") ? triggerData["Compare"] : ">=";
                        int resourceAmount = trigger.ContainsKey("ResourceAmount") ? int.Parse(trigger["ResourceAmount"]) : 1;
                        int resourceAmountData = triggerData.ContainsKey("ResourceAmount") ? int.Parse(triggerData["ResourceAmount"]) : 1;
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = (resourceType == resourceTypeData || resourceTypeData == "Any") && CompareValues(resourceAmount, resourceAmountData, compareData) && triggerer == triggererData;
                    }
                    else if (trigger["Trigger"] == "ResourceMaxed" || trigger["Trigger"] == "ResourceDeplated")
                    {
                        string resourceType = trigger.ContainsKey("ResourceType") ? trigger["ResourceType"] : "None";
                        string resourceTypeData = triggerData.ContainsKey("ResourceType") ? triggerData["ResourceType"] : "None";
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = (resourceType == resourceTypeData || resourceTypeData == "Any") && triggerer == triggererData;
                    }
                    else if (trigger["Trigger"] == "AbilityCast" || trigger["Trigger"] == "Cooldown")
                    {
                        string abilityName = trigger.ContainsKey("AbilityName") ? trigger["AbilityName"] : "None";
                        string abilityNameData = triggerData.ContainsKey("AbilityName") ? (triggerData["AbilityName"] == "This" ? name : triggerData["AbilityName"]) : name;
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "Effector";
                        execute = (abilityName == abilityNameData || abilityNameData == "Any") && triggerer == triggererData;
                    }
                    else if (trigger["Trigger"] == "Damage")
                    {
                        int damageAmount = trigger.ContainsKey("DamageAmount") ? int.Parse(trigger["DamageAmount"]) : 1;
                        int damageAmountData = triggerData.ContainsKey("DamageAmount") ? int.Parse(triggerData["DamageAmount"]) : 1;
                        string compareData = triggerData.ContainsKey("Compare") ? triggerData["Compare"] : ">=";
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = CompareValues(damageAmount, damageAmountData, compareData) && triggerer == triggererData;
                    }
                    else if (trigger["Trigger"] == "Heal")
                    {
                        int healAmount = trigger.ContainsKey("HealAmount") ? int.Parse(trigger["HealAmount"]) : 1;
                        int healAmountData = triggerData.ContainsKey("HealAmount") ? int.Parse(triggerData["HealAmount"]) : 1;
                        string compareData = triggerData.ContainsKey("Compare") ? triggerData["Compare"] : ">=";
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = CompareValues(healAmount, healAmountData, compareData) && triggerer == triggererData;
                    }
                    else if (trigger["Trigger"] == "HealthMaxed" || trigger["Trigger"] == "HealthDeplated")
                    {
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = triggerer == triggererData;
                    }
                    else if (trigger["Trigger"] == "CombatBegin") execute = true;
                    else if (trigger["Trigger"] == "TurnBegin") execute = true;
                    else if (trigger["Trigger"] == "TurnEnd") execute = true;
                    else if (trigger["Trigger"] == "ItemUsed")
                    {
                        string itemHash = trigger.ContainsKey("ItemHash") ? trigger["ItemHash"] : "";
                        execute = item != null && item.GetHashCode() + "" == itemHash;
                    }
                }
            if (execute && (board != null ? board.CooldownOn(sourcedFromPlayer, name) : futureBoard.CooldownOn(sourcedFromPlayer, name)) <= 0)
            {
                if (board != null) board.PutOnCooldown(sourcedFromPlayer, this);
                else futureBoard.PutOnCooldown(sourcedFromPlayer, this);
                eve.ExecuteEffects(board, futureBoard, icon, trigger, RankVariables(abilityRank), name, abilityRank);
            }
        }
    }

    public Dictionary<string, string> RankVariables(int abilityRank)
    {
        var variables = new Dictionary<string, string>();
        foreach (var rank in ranks)
            if (ranks.IndexOf(rank) > abilityRank) break;
            else foreach (var variable in rank)
                    if (variables.ContainsKey(variable.Key)) variables[variable.Key] = variable.Value;
                    else variables.Add(variable.Key, variable.Value);
        return variables;
    }

    #endregion

    #region Description

    public static void PrintAbilityTooltip(Entity effector, Entity other, Ability ability, int rank, Item item = null)
    {
        AddHeaderGroup();
        var width = 220;
        if (CDesktop.title == "Game")
        {
            SetAnchor(Top, 0, -34);
            DisableShadows();
            width = 228;
            SetRegionGroupHeight(195);
        }
        else if (CDesktop.title == "TalentScreen")
        {
            SetAnchor(Top, 0, -57);
            SetRegionGroupHeight(187);
        }
        else
        {
            SetAnchor(Top, 0, -38);
            SetRegionGroupHeight(187);
        }
        SetRegionGroupWidth(width);
        if (ability == null)
        {
            AddHeaderRegion(() => AddLine("Ability not found.", "Red"));
            AddRegionGroup();
            SetRegionGroupWidth(width);
            AddPaddingRegion(() => { AddLine(); });
        }
        else
        {
            AddHeaderRegion(() =>
            {
                AddLine(ability.name, "Gray");
                if (item == null) AddText(" " + ToRoman(rank + 1));
            });
            AddPaddingRegion(() =>
            {
                AddBigButton(item != null ? (item.icon == null ? "OtherUnknown" : item.icon) : (ability.icon == null ? "OtherUnknown" : ability.icon));
                AddLine("Cooldown: ", "DarkGray");
                AddText(ability.cooldown == 0 ? "None" : ability.cooldown + (ability.cooldown == 1 ? " turn" : " turns"), "Gray");
                if (CDesktop.title == "Game" || CDesktop.title == "GameSimulation")
                {
                    var c = Board.board.CooldownOn(effector == Board.board.player, ability.name);
                    if (c > 0)
                    {
                        AddLine("Cooldown left: ", "DarkGray");
                        AddText(c + (c == 1 ? " turn" : " turns"), "Gray");
                    }
                }
            });
            ability.PrintDescription(effector, other, width, rank);
            if (ability.cost != null)
                foreach (var cost in ability.cost)
                    if (cost.Value > 0)
                    {
                        AddRegionGroup();
                        AddHeaderRegion(() =>
                        {
                            AddSmallButton("Element" + cost.Key + "Rousing");
                        });
                        AddRegionGroup();
                        SetRegionGroupWidth(33);
                        AddHeaderRegion(() =>
                        {
                            AddLine(cost.Value + "", effector != null && effector.resources != null ? cost.Value > effector.resources[cost.Key] ? "Red" : "Green" : "Gray");
                        });
                    }
            AddRegionGroup();
            SetRegionGroupWidth(width - (ability.cost == null ? 0 : ability.cost.Count(x => x.Value > 0)) * 52);
            AddPaddingRegion(() => { AddLine(); });
        }
    }

    public void PrintDescription(Entity effector, Entity other, int width, int rank)
    {

        if (description != null) description.Print(effector, other, width, RankVariables(rank));
        else AddHeaderRegion(() =>
        {
            SetRegionAsGroupExtender();
            AddLine("No description", "DarkGray");
        });
    }

    #endregion

    //Name of the ability
    public string name;

    //Icon of the ability in action bars and spellbook
    public string icon;

    //Provides information on how many turns will the ability be disabled after casting
    public int cooldown;

    //Indicates whether this ability should be always put on the bottom of the action bars in combat
    public bool putOnEnd;

    //Hides this ability in spellbook
    public bool hide;

    //Tags to help AI in calculating priorities in combat
    public List<string> tags;

    //Cost of the ability to cast
    //Keys provide information what type of resource is required
    //Values provide information how much of that resource entity needs
    //EXAMPLE: { "Frost": 4, "Decay": 2  } 
    public Dictionary<string, int> cost;

    //Rank variables to scale abilities with their level
    public List<Dictionary<string, string>> ranks;

    //List of events this ability has
    //This is essentially all the ability's effects with it's triggerers that make them happen
    public List<Event> events;

    //Description of the buff to show on hover in the status bar
    public Description description;

    //Currently opened ability
    public static Ability ability;

    //EXTERNAL FILE: List containing all abilities in-game
    public static List<Ability> abilities;

    //List of all filtered abilities by input search
    public static List<Ability> abilitiesSearch;
}
