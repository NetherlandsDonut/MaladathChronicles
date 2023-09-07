using System;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static SaveGame;

public class Buff
{
    public string name, icon, dispelType;
    public List<string> tags;
    public bool stackable;
    public List<Event> events;
    public Description description;

    #region Execution

    public void ExecuteEvents(Board board, FutureBoard futureBoard, Dictionary<string, string> trigger, (Buff, int, GameObject) entityBuff)
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
                    {
                        if (entityBuff.Item3 != null) board.actions.Add(() => { AddSmallButtonOverlay(entityBuff.Item3, "OtherGlowFull", 1, 5); });
                        eve.ExecuteEffects(board, futureBoard, icon, trigger);
                    }
                }
    }

    #endregion

    #region Description

    public static void PrintBuffTooltip(Entity Effector, Entity other, (Buff, int, GameObject) buff)
    {
        SetAnchor(Top, 0, -39);
        AddHeaderGroup();
        SetRegionGroupWidth(246);
        SetRegionGroupHeight(227);
        AddHeaderRegion(() => { AddLine(buff.Item1.name); });
        AddPaddingRegion(() =>
        {
            AddBigButton(buff.Item1.icon, (h) => { });
            AddLine("Dispellable: ", "DarkGray");
            AddText(buff.Item1.dispelType != "None" ? "Yes" : "No");
            if (buff.Item3 != null)
            {
                AddLine("Turns left: ", "DarkGray");
                AddText(buff.Item2 + "");
            }
        });
        buff.Item1.PrintDescription(Effector, other);
        AddRegionGroup();
        SetRegionGroupWidth(246);
        AddPaddingRegion(() => { AddLine(); });
    }

    public void PrintDescription(Entity effector, Entity other)
    {
        int width = CDesktop.LBWindow.LBRegionGroup.setWidth;
        if (description != null) description.Print(effector, other, width);
        else AddHeaderRegion(() =>
        {
            SetRegionAsGroupExtender();
            AddLine("No description", "DarkGray");
        });
    }

    #endregion

    public static Buff buff;
    public static List<Buff> buffs, buffsSearch;
}