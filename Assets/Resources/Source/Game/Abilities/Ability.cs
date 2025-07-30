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

    public bool PossibleToHaveResources(Entity entity) => cost.All(x => x.Value <= entity.MaxResource(x.Key));
    public bool EnoughResources(Entity entity) => EnoughResources(entity.resources);
    public bool EnoughResources(Dictionary<string, int> resources) => !cost.Any(x => x.Value > resources[x.Key]);

    //Checks if this ability can be casted on any valid target by the participant
    public bool HasValidTarget(CombatParticipant participant)
    {
        if (possibleTargets == "Friendly")
            return Board.board.participants.Any(x => x.who.CanBeTargetted(false) && x.team == participant.team && participant != x);
        else if (possibleTargets == "Enemies")
            return Board.board.participants.Any(x => x.who.CanBeTargetted(false) && x.team != participant.team);
        else if (possibleTargets == "Entity")
            return Board.board.participants.Any(x => x.who.CanBeTargetted(false));
        else if (possibleTargets == "Self") return true;
        else if (possibleTargets == "Tile") return true;
        else return false;
    }

    #endregion

    #region Execution

    public List<Condition> ConditionsNotMet(Dictionary<string, string> trigger, Ability ability, SaveGame save, Board board) => events.FindAll(x => x.triggers.Any(y => y["Trigger"] == trigger["Trigger"])).SelectMany(x => x.conditions == null ? new() : x.conditions.Where(y => !y.IsMet(ability, trigger, save, board)).ToList()).ToList();

    public bool AreConditionsMet(Dictionary<string, string> trigger, Event eve, SaveGame save, Board board) => eve.conditions == null || eve.conditions.Count == 0 || eve.conditions.All(x => x.IsMet(this, trigger, save, board));

    public void ExecuteEvents(SaveGame save, Dictionary<string, string> trigger, Item item)
    {
        //In case of this ability having no events just return
        if (events == null) return;
        foreach (var eve in events)
        {
            bool execute = false;
            foreach (var triggerData in eve.triggers)
                if (triggerData.ContainsKey("Trigger") && triggerData["Trigger"] == trigger["Trigger"])
                    if (trigger["Trigger"] == "ItemUsed")
                    {
                        string itemHash = trigger.ContainsKey("ItemHash") ? trigger["ItemHash"] : "";
                        string sitePresence = triggerData.ContainsKey("SitePresence") ? triggerData["SitePresence"] : "";
                        execute = item != null && item.GetHashCode() + "" == itemHash && (sitePresence == "" || save.currentSite == sitePresence);
                    }
            if (execute && (trigger.ContainsKey("IgnoreConditions") && trigger["IgnoreConditions"] == "Yes" || AreConditionsMet(trigger, eve, save, null)))
                eve.ExecuteEffects(save, item, trigger, RankVariables(trigger.ContainsKey("AbilityRank") && int.TryParse(trigger["AbilityRank"], out int parse) ? parse : 0), this);
        }
    }

    public void ExecuteEvents(Board board, Dictionary<string, string> trigger, Item item, int entitySource)
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
                        string abilityNameData = triggerData.ContainsKey("AbilityName") ? (triggerData["AbilityName"] == "This" ? name : triggerData["AbilityName"]) : "Any";
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
            if (execute && board.CooldownOn(entitySource, name) <= 0)
                if (trigger.ContainsKey("IgnoreConditions") && trigger["IgnoreConditions"] == "Yes" || AreConditionsMet(trigger, eve, null, board))
                {
                    board.PutOnCooldown(entitySource, this);
                    var rank = trigger.ContainsKey("AbilityRank") && int.TryParse(trigger["AbilityRank"], out int parse) ? parse : 0;
                    eve.ExecuteEffects(board, icon, trigger, RankVariables(rank), name, rank);
                }
        }
    }

    public Dictionary<string, string> RankVariables(int abilityRank)
    {
        var variables = new Dictionary<string, string>();
        if (ranks != null)
            foreach (var rank in ranks)
                if (ranks.IndexOf(rank) > abilityRank) break;
                else foreach (var variable in rank)
                        if (variables.ContainsKey(variable.Key)) variables[variable.Key] = variable.Value;
                        else variables.Add(variable.Key, variable.Value);
        return variables;
    }

    #endregion

    #region Description

    public static void PrintAbilityTooltip(Entity effector, Ability ability, int rank, Item item = null)
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
            AddPaddingRegion(() => AddLine());
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
                AddBigButton(item != null ? (item.icon ?? "OtherUnknown") : (ability.icon ?? "OtherUnknown"));
                AddLine("Cooldown: ", "DarkGray");
                AddText(ability.cooldown == 0 ? "None" : ability.cooldown + (ability.cooldown == 1 ? " turn" : " turns"), "Gray");
                if (CDesktop.title == "Game" || CDesktop.title == "GameSimulation")
                {
                    var c = Board.board.CooldownOn(Board.board.participants.FindIndex(x => x.who == effector), ability.name);
                    if (c > 0)
                    {
                        AddLine("Cooldown left: ", "DarkGray");
                        AddText(c + (c == 1 ? " turn" : " turns"), "Gray");
                    }
                }
            });
            ability.PrintDescription(effector, width, rank);
            if (ability.cost != null)
                foreach (var cost in ability.cost)
                    if (cost.Value > 0)
                    {
                        AddRegionGroup();
                        AddHeaderRegion(() => AddSmallButton("Element" + cost.Key + "Rousing"));
                        AddRegionGroup();
                        SetRegionGroupWidth(33);
                        AddHeaderRegion(() => AddLine(cost.Value + "", effector != null && effector.resources != null ? cost.Value > effector.resources[cost.Key] ? "Red" : "Green" : "Gray"));
                    }
            AddRegionGroup();
            SetRegionGroupWidth(width - (ability.cost == null ? 0 : ability.cost.Count(x => x.Value > 0)) * 52);
            AddPaddingRegion(() => AddLine());
        }
    }

    public void PrintDescription(Entity effector, int width, int rank)
    {
        if (description != null) description.Print(effector, width, RankVariables(rank));
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

    //Sound that is played when the player begins to target someone usign this ability
    public string targettingSound;

    //Provides information on how many turns will the ability be disabled after casting
    public int cooldown;

    //Indicates whether this ability should be always put on the bottom of the action bars in combat
    public bool putOnEnd;

    //Hides this ability in spellbook
    public bool hide;

    //Information on who can this spell be casted
    public string possibleTargets;

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
