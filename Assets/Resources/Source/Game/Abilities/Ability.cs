using System.Collections.Generic;

using static Root;
using static Root.Anchor;

public class Ability
{
    #region Resource Check

    public bool EnoughResources(Entity entity) => EnoughResources(entity.resources);
    public bool EnoughResources(FutureEntity entity) => EnoughResources(entity.resources);

    private bool EnoughResources(Dictionary<string, int> resources)
    {
        foreach (var resource in cost)
            if (resources[resource.Key] < resource.Value)
                return false;
        return true;
    }

    #endregion

    #region Execution

    public void ExecuteEvents(Board board, FutureBoard futureBoard, Dictionary<string, string> trigger)
    {
        if (events == null) return;
        foreach (var eve in events)
            foreach (var triggerData in eve.triggers)
                if (triggerData.ContainsKey("Trigger") && triggerData["Trigger"] == trigger["Trigger"])
                {
                    bool execute = false;
                    if (trigger["Trigger"] == "BuffAdd" || trigger["Trigger"] == "BuffRemove" || trigger["Trigger"] == "BuffFlare")
                    {
                        string buffName = trigger.ContainsKey("BuffName") ? trigger["BuffName"] : "None";
                        string buffNameData = triggerData.ContainsKey("BuffName") ? triggerData["BuffName"] : "None";
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = (buffName == buffNameData || buffNameData == "Any") && triggerer == triggererData;
                    }
                    else if (trigger["Trigger"] == "ResourceCollected")
                    {
                        string resourceType = trigger.ContainsKey("ResourceType") ? trigger["ResourceType"] : "None";
                        string resourceTypeData = triggerData.ContainsKey("ResourceType") ? triggerData["ResourceType"] : "None";
                        string compareData = triggerData.ContainsKey("Compare") ? triggerData["Compare"] : ">=";
                        int resourceAmount = trigger.ContainsKey("ResourceAmount") ? int.Parse(trigger["ResourceAmount"]) : 1;
                        int resourceAmountData = triggerData.ContainsKey("ResourceAmount") ? int.Parse(triggerData["ResourceAmount"]) : 1;
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = resourceType == resourceTypeData && CompareValues(resourceAmount, resourceAmountData, compareData) && triggerer == triggererData;
                    }
                    else if (trigger["Trigger"] == "ResourceMaxed" || trigger["Trigger"] == "ResourceDeplated")
                    {
                        string resourceType = trigger.ContainsKey("ResourceType") ? trigger["ResourceType"] : "None";
                        string resourceTypeData = triggerData.ContainsKey("ResourceType") ? triggerData["ResourceType"] : "None";
                        string triggerer = trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "None";
                        string triggererData = triggerData.ContainsKey("Triggerer") ? triggerData["Triggerer"] : "None";
                        execute = resourceType == resourceTypeData && triggerer == triggererData;
                    }
                    else if (trigger["Trigger"] == "AbilityCast" || trigger["Trigger"] == "Cooldown")
                    {
                        string abilityName = trigger.ContainsKey("AbilityName") ? trigger["AbilityName"] : "None";
                        string abilityNameData = triggerData.ContainsKey("AbilityName") ? triggerData["AbilityName"] : name;
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
                    if (execute) eve.ExecuteEffects(board, futureBoard, icon, trigger);
                }
    }

    public bool CompareValues(double x, double y, string compare)
    {
        if (compare == ">=") return x >= y;
        if (compare == ">") return x > y;
        if (compare == "<=") return x <= y;
        if (compare == "<") return x < y;
        if (compare == "==") return x == y;
        return false;
    }

    #endregion

    #region Description

    public static void PrintAbilityTooltip(Entity effector, Entity other, Ability ability)
    {
        SetAnchor(Top, 0, -39);
        AddHeaderGroup();
        SetRegionGroupWidth(246);
        SetRegionGroupHeight(227);
        if (ability == null)
        {
            AddHeaderRegion(() =>
            {
                AddLine("Ability not found.", "Red");
            });
            AddRegionGroup();
            SetRegionGroupWidth(246);
            AddPaddingRegion(() => { AddLine(); });
        }
        else
        {
            AddHeaderRegion(() =>
            {
                AddLine(ability.name, "Gray");
            });
            AddPaddingRegion(() =>
            {
                AddBigButton(ability.icon, (h) => { });
                AddLine("Cooldown: ", "DarkGray");
                AddText(ability.cooldown == 0 ? "None" : ability.cooldown + (ability.cooldown == 1 ? " turn" : " turns"), "Gray");
                if (effector != null && effector.actionBars != null)
                {
                    var find = effector.actionBars.Find(x => x.ability == ability.name);
                    if (find != null && find.cooldown > 0)
                    {
                        AddLine("Cooldown left: ", "DarkGray");
                        AddText(find.cooldown + (find.cooldown == 1 ? " turn" : " turns"), "Gray");
                    }
                }
            });
            ability.PrintDescription(effector, other, 246);
            if (ability.cost != null)
                foreach (var cost in ability.cost)
                {
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddSmallButton("Element" + cost.Key + "Rousing", (h) => { });
                    });
                    AddRegionGroup();
                    SetRegionGroupWidth(20);
                    AddHeaderRegion(() =>
                    {
                        AddLine(cost.Value + "", effector != null ? cost.Value > effector.resources[cost.Key] ? "Red" : "Green" : "Gray");
                    });
                }
            AddRegionGroup();
            SetRegionGroupWidth(246 - (ability.cost == null ? 0 : ability.cost.Count) * 49);
            AddPaddingRegion(() => { AddLine(); });
        }
    }

    public void PrintDescription(Entity effector, Entity other, int width)
    {
        if (description != null) description.Print(effector, other, width);
        else AddHeaderRegion(() =>
        {
            SetRegionAsGroupExtender();
            AddLine("No description", "DarkGray");
        });
    }

    #endregion

    public string name, icon;
    public int cooldown;
    public bool putOnEnd;
    public List<string> tags;
    public Dictionary<string, int> cost;
    public List<Event> events;
    public Description description;

    public static Ability ability;
    public static List<Ability> abilities;
}
