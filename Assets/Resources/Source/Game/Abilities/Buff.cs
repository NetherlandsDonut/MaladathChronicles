using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

public class Buff
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

    //Name of the buff
    public string name;

    //Icon of the buff in status bar
    public string icon;

    //Dispel type that let's an entity dispel a buff with proper ability
    public string dispelType;

    //Indicates whether the buff is removed after death
    public bool deathResistant;

    //Tags to help AI in calculating priorities in combat
    public List<string> tags;

    //Indicates whether this buff can be stacked on a target
    public bool stackable;

    //Rank variables to scale buffs with their level
    public List<Dictionary<string, string>> ranks;

    //Stats provided by this buff
    public Dictionary<string, int> gains;

    //List of events this buff has
    //This is essentially all the buff's effects with it's triggerers that make them happen
    public List<Event> events;

    //Description of the buff to show on hover in the status bar
    public Description description;

    #region Execution

    public void ExecuteEvents(Board board, FutureBoard futureBoard, Dictionary<string, string> trigger, (Buff, int, GameObject, int) entityBuff)
    {
        //In case of this buff having no events just return
        if (events == null) return;
        foreach (var eve in events)
            foreach (var triggerData in eve.triggers)
                if (triggerData.ContainsKey("Trigger") && triggerData["Trigger"] == trigger["Trigger"])
                {
                    bool execute = false;
                    if (trigger["Trigger"] == "BuffAdd" || trigger["Trigger"] == "BuffRemove" || trigger["Trigger"] == "BuffFlare")
                    {
                        string buffName = trigger.ContainsKey("BuffName") ? trigger["BuffName"] : "None";
                        string buffNameData = triggerData.ContainsKey("BuffName") ? (triggerData["BuffName"] == "This" ? name : triggerData["BuffName"]) : name;
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "Effector";
                        execute = (buffName == buffNameData || buffNameData == "Any") && (triggerer == triggererData || triggerer == "Any");
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
                        execute = (resourceType == resourceTypeData || resourceTypeData == "Any") && CompareValues(resourceAmount, resourceAmountData, compareData) && (triggerer == triggererData || triggerer == "Any");
                    }
                    else if (trigger["Trigger"] == "ResourceMaxed" || trigger["Trigger"] == "ResourceDeplated")
                    {
                        string resourceType = trigger.ContainsKey("ResourceType") ? trigger["ResourceType"] : "None";
                        string resourceTypeData = triggerData.ContainsKey("ResourceType") ? triggerData["ResourceType"] : "None";
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = (resourceType == resourceTypeData || resourceTypeData == "Any") && (triggerer == triggererData || triggerer == "Any");
                    }
                    else if (trigger["Trigger"] == "AbilityCast" || trigger["Trigger"] == "CooldownEnd")
                    {
                        string abilityName = trigger.ContainsKey("AbilityName") ? trigger["AbilityName"] : "None";
                        string abilityNameData = triggerData.ContainsKey("AbilityName") ? triggerData["AbilityName"] : "None";
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = (abilityName == abilityNameData || abilityNameData == "Any") && (triggerer == triggererData || triggerer == "Any");
                    }
                    else if (trigger["Trigger"] == "Damage")
                    {
                        int damageAmount = trigger.ContainsKey("DamageAmount") ? int.Parse(trigger["DamageAmount"]) : 1;
                        int damageAmountData = triggerData.ContainsKey("DamageAmount") ? int.Parse(triggerData["DamageAmount"]) : 1;
                        string compareData = triggerData.ContainsKey("Compare") ? triggerData["Compare"] : ">=";
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = CompareValues(damageAmount, damageAmountData, compareData) && (triggerer == triggererData || triggerer == "Any");
                    }
                    else if (trigger["Trigger"] == "Heal")
                    {
                        int healAmount = trigger.ContainsKey("HealAmount") ? int.Parse(trigger["HealAmount"]) : 1;
                        int healAmountData = triggerData.ContainsKey("HealAmount") ? int.Parse(triggerData["HealAmount"]) : 1;
                        string compareData = triggerData.ContainsKey("Compare") ? triggerData["Compare"] : ">=";
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = CompareValues(healAmount, healAmountData, compareData) && (triggerer == triggererData || triggerer == "Any");
                    }
                    else if (trigger["Trigger"] == "HealthMaxed" || trigger["Trigger"] == "HealthDeplated")
                    {
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = triggerer == triggererData || triggerer == "Any";
                    }
                    else if (trigger["Trigger"] == "CombatBegin") execute = true;
                    else if (trigger["Trigger"] == "TurnBegin") execute = true;
                    else if (trigger["Trigger"] == "TurnEnd") execute = true;
                    if (execute)
                        if (eve.conditions == null || eve.conditions.Count == 0 || eve.conditions.All(x => x.IsMet(SaveGame.currentSave, board, futureBoard)))
                        {
                            if (entityBuff.Item3 != null) board.actions.Add(() => { AddSmallButtonOverlay(entityBuff.Item3, "OtherBlack", 1, 5); });
                            eve.ExecuteEffects(board, futureBoard, icon, trigger, RankVariables(entityBuff.Item4), name, entityBuff.Item4);
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

    public static void PrintWorldBuffTooltip(WorldBuff worldBuff)
    {
        SetAnchor(Top, 0, -34);
        AddHeaderGroup();
        SetRegionGroupWidth(228);
        SetRegionGroupHeight(195);
        AddHeaderRegion(() => { AddLine(worldBuff.buff.OnlyNameCategory()); });
        AddPaddingRegion(() =>
        {
            AddBigButton(worldBuff.Buff.icon);
            AddLine("Minutes left: ", "DarkGray");
            AddText(worldBuff.minutesLeft + "");
        });
        worldBuff.Buff.PrintDescription(SaveGame.currentSave.player, null, worldBuff.rank);
        AddRegionGroup();
        SetRegionGroupWidth(228);
        AddPaddingRegion(() => { AddLine(); });
    }

    public static void PrintBuffTooltip(Entity Effector, Entity other, (Buff, int, GameObject, int) buff)
    {
        SetAnchor(Top, 0, -34);
        AddHeaderGroup();
        if (CDesktop.title == "Game")
            DisableShadows();
        SetRegionGroupWidth(228);
        SetRegionGroupHeight(195);
        AddHeaderRegion(() => { AddLine(buff.Item1.name); });
        AddPaddingRegion(() =>
        {
            AddBigButton(buff.Item1.icon);
            AddLine("Dispellable: ", "DarkGray");
            AddText(buff.Item1.dispelType != "None" ? "Yes" : "No");
            if (buff.Item3 != null && buff.Item2 > 0)
            {
                AddLine("Turns left: ", "DarkGray");
                AddText(buff.Item2 + "");
            }
        });
        buff.Item1.PrintDescription(Effector, other, buff.Item4);
        AddRegionGroup();
        SetRegionGroupWidth(228);
        AddPaddingRegion(() => { AddLine(); });
    }

    public void PrintDescription(Entity effector, Entity other, int rank)
    {
        int width = CDesktop.LBWindow().LBRegionGroup().setWidth;
        if (description != null) description.Print(effector, other, width, RankVariables(rank));
        else AddHeaderRegion(() =>
        {
            SetRegionAsGroupExtender();
            AddLine("No description", "DarkGray");
        });
    }

    #endregion

    //Currently opened buff
    public static Buff buff;

    //EXTERNAL FILE: List containing all buffs in-game
    public static List<Buff> buffs;

    //List of all filtered buffs by input search
    public static List<Buff> buffsSearch;
}