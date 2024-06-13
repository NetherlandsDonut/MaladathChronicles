using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static Root;

using static Sound;
using static Coloring;

public class Quest
{
    public void Initialise()
    {
        if (siteStart != null)
        {
            var find = Site.FindSite(x => x.name == siteStart);
            if (find != null)
            {
                find.questsStarted ??= new();
                find.questsStarted.Add(this);
            }
            else Debug.Log("ERROR 013: Starting site for quest was not found: \"" + siteStart + "\"");
        }
    }

    //ID of the quest
    public int questID;

    //Prerequisite quest needed to be done before this becomes available
    public int previous;

    //Required level to have this quest
    public int requiredLevel;

    //Level of the quest
    public int questLevel;

    //Name of the quest
    public string name;

    //Description of the quest
    public string description;

    //Site where the quest is available for pickup
    public string siteStart;

    //Site where the quest can be handed in
    public string siteEnd;

    //Zone of the quest
    public string zone;

    //Reputation connected with the quest
    public string faction;

    //Required reputation rank from the faction
    //for the quest to be available
    public string requiredRank;

    //Amount of money awarded
    public int money;

    //Amount of experience awarded
    public int experience;

    //Eligble races for this quest
    public List<string> races;

    //Eligble classes for this quest
    public List<string> classes;

    //Item rewards for this quest
    public Dictionary<string, int> rewards;

    //Amount of reputation awarded to the quest faction
    public Dictionary<string, int> reputationGain;

    //Items provided by the quest when player accepts it
    public Dictionary<string, int> providedItems;

    //Conditions for completing the quest
    public List<QuestCondition> conditions;

    //Zone icon for the quest
    public string ZoneIcon() => "Zone" + zone.Clean();

    //Icon for the quest
    public string Icon()
    {
        var r = "Quest";
        if (races == null || races.Count == 0) r += "Neutral";
        else if (races.Contains("Orc") || races.Contains("Troll") || races.Contains("Tauren") || races.Contains("Forsaken")) r += "Red";
        else if (races.Contains("Human") || races.Contains("Dwarf") || races.Contains("Gnome") || races.Contains("Night Elf")) r += "Blue";
        else r += "Neutral";
        if (conditions == null) r += "Sealed";
        else if (conditions.Count == 1) r += "";
        else if (conditions.Count >= 2) r += "Big";
        else r += "Sealed";
        return r;
    }

    //Currently selected quest
    public static Quest quest;

    //EXTERNAL FILE: List containing all quests in-game
    public static List<Quest> quests;

    //List of all filtered quests by input search
    public static List<Quest> questsSearch;

    //All sites that currently have quest markers on them
    public static List<Site> sitesWithQuestMarkers;

    //All the sites that need to be respawned after entering the map again
    public static List<Site> sitesToRespawn;

    public void Print()
    {
        AddRegionGroup();
        SetRegionGroupWidth(190);
        SetRegionGroupHeight(281);
        var color = ColorQuestLevel(questLevel);
        AddHeaderRegion(() =>
        {
            AddLine(name, color != null ? "Black" : "Gray");
            if (CDesktop.title == "QuestLog")
                if (CDesktop.windows.Exists(x => x.title == "QuestConfirmAbandon"))
                {
                    AddSmallButton("OtherCloseOff");
                    AddSmallButton("OtherTrashOff");
                }
                else if (CDesktop.windows.Exists(x => x.title == "QuestConfirmAbandon") || CDesktop.windows.Exists(x => x.title == "QuestSort") || CDesktop.windows.Exists(x => x.title == "QuestSettings"))
                {
                    AddSmallButton("OtherClose", (h) =>
                    {
                        CloseWindow("Quest");
                        PlaySound("DesktopInstanceClose");
                    });
                    AddSmallButton("OtherTrashOff");
                }
                else
                {
                    AddSmallButton("OtherClose", (h) =>
                    {
                        CloseWindow("Quest");
                        PlaySound("DesktopInstanceClose");
                    });
                    AddSmallButton("OtherTrash", (h) =>
                    {
                        PlaySound("DesktopMenuOpen");
                        SpawnWindowBlueprint("QuestConfirmAbandon");
                        Respawn("QuestList");
                    });
                }
        });
        if (color != null) SetRegionBackgroundAsImage("SkillUpLong" + color);
        if (description != null)
        {
            AddHeaderRegion(() => AddLine("Description:"));
            new Description()
            {
                regions = new() { new() { regionType = "Padding", contents = new() { new ()
                    {
                        { "Color", "DarkGray" },
                        { "Text", description }
                    }
                } } }
            }.Print(null, null, 190, null);
        }
        if (conditions != null)
        {
            AddHeaderRegion(() => AddLine("Details:"));
            foreach (var condition in conditions)
                condition.Print(CDesktop.title == "QuestLog");
        }
        if (rewards != null)
        {
            AddHeaderRegion(() => AddLine("Rewards:"));
            AddPaddingRegion(() => { });
            foreach (var item in rewards)
            {
                var find = Item.items.Find(x => x.name == item.Key);
                AddBigButton(find.icon, null, null, (h) => () =>
                {
                    if (CDesktop.windows.Exists(x => x.title == "CraftingSort")) return;
                    if (CDesktop.windows.Exists(x => x.title == "CraftingSettings")) return;
                    Item.PrintItemTooltip(find, Input.GetKey(KeyCode.LeftShift));
                });
                if (find.maxStack > 1) SpawnFloatingText(CDesktop.LBWindow.LBRegionGroup.LBRegion.transform.position + new Vector3(32, -27) + new Vector3(38, 0) * (rewards.Keys.ToList().IndexOf(item.Key) % 5), item.Value + "", "", "Right");
            }
        }
    }

    //Copies a quest to a new one for the player to take
    public Quest CopyQuest()
    {
        var newQuest = new Quest();
        newQuest.name = name;
        if (classes != null)
            newQuest.classes = classes.ToList();
        if (races != null)
            newQuest.races = races.ToList();
        if (rewards != null)
            newQuest.rewards = rewards.ToDictionary(x => x.Key, x => x.Value);
        newQuest.previous = previous;
        newQuest.questLevel = questLevel;
        newQuest.requiredLevel = requiredLevel;
        if (reputationGain != null)
            newQuest.reputationGain = reputationGain.ToDictionary(x => x.Key, x => x.Value);
        newQuest.description = description;
        newQuest.conditions = new();
        if (conditions != null)
            foreach (var condition in conditions)
                newQuest.conditions.Add(new() { name = condition.name, amount = condition.amount, type = condition.type });
        newQuest.faction = faction;
        newQuest.requiredRank = requiredRank;
        newQuest.experience = experience;
        newQuest.money = money;
        newQuest.siteStart = siteStart;
        newQuest.siteEnd = siteEnd;
        if (providedItems != null)
            newQuest.providedItems = providedItems.ToDictionary(x => x.Key, x => x.Value);
        newQuest.questID = questID;
        newQuest.zone = zone;
        return newQuest;
    }
}
