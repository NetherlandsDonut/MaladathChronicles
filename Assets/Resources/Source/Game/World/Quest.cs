using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Sound;
using static SaveGame;
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
            else Debug.Log("ERROR 014: Starting site for quest was not found: \"" + siteStart + "\"");
        }
        if (itemStart != null)
        {
            var find = Item.items.Find(x => x.name == itemStart);
            if (find != null)
            {
                find.questsStarted ??= new();
                if (!find.questsStarted.Contains(questID))
                    find.questsStarted.Add(questID);
            }
            else Debug.Log("ERROR 016: Quest starting item was not found: \"" + itemStart + "\"");
        }
    }

    //ID of the quest
    public int questID;

    //Prerequisite quests needed to be done before this becomes available
    public List<int> previous;

    //Required level to have this quest
    public int requiredLevel;

    //Level of the quest
    public int questLevel;

    //Name of the quest
    public string name;

    //Description of the quest
    public string description;

    //Objective of the quest
    public string objective;

    //Objective of the quest
    public string completion;

    //Item required to start quest
    public string itemStart;

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
    
    //Currently chosen quest reward by player
    public static string chosenReward;

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

    public void Print(string f = "Log")
    {
        var rowAmount = 1;
        var thisWindow = CDesktop.LBWindow();
        thisWindow.SetPaginationSingleStep(f == "Add" && description != null ? () => description.Split("$B$B").Length : (f == "Turn" && completion != null ? () => completion.Split("$B$B").Length : () => 1), rowAmount);
        AddRegionGroup();
        SetRegionGroupWidth(190);
        SetRegionGroupHeight(281);
        var color = ColorQuestLevel(questLevel);
        AddHeaderRegion(() =>
        {
            AddLine(name, color != null ? "Black" : "Gray");
            if (f == "Log")
                if (WindowUp("QuestConfirmAbandon"))
                {
                    AddSmallButton("OtherCloseOff");
                    AddSmallButton("OtherTrashOff");
                }
                else if (WindowUp("QuestConfirmAbandon") || WindowUp("QuestSort") || WindowUp("QuestSettings"))
                {
                    AddSmallButton("OtherClose", (h) =>
                    {
                        CloseWindow("Quest");
                        Respawn("Chest", true);
                        Respawn("Capital", true);
                        Respawn("PlayerMoney", true);
                        PlaySound("DesktopInstanceClose");
                    });
                    AddSmallButton("OtherTrashOff");
                }
                else
                {
                    AddSmallButton("OtherClose", (h) =>
                    {
                        CloseWindow("Quest");
                        Respawn("Chest", true);
                        Respawn("Capital", true);
                        Respawn("PlayerMoney", true);
                        PlaySound("DesktopInstanceClose");
                    });
                    AddSmallButton("OtherTrash", (h) =>
                    {
                        PlaySound("DesktopMenuOpen", 0.6f);
                        Respawn("QuestConfirmAbandon");
                        Respawn("QuestList");
                    });
                }
            else
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("Quest" + f);
                    PlaySound("DesktopInstanceClose");
                    Respawn("Chest", true);
                    Respawn("Capital", true);
                    Respawn("PlayerMoney", true);
                    Respawn("Complex", true);
                    Respawn("InstanceWing", true);
                    Respawn("Instance", true);
                });
        });
        if (color != null) SetRegionBackgroundAsImage("SkillUp" + color + "Long");
        var regionGroup = CDesktop.LBWindow().LBRegionGroup();
        if (f == "Add")
        {
            AddHeaderRegion(() =>
            {
                AddLine("Description: ", "Gray");
                AddText(thisWindow.pagination() + 1 + "", "HalfGray");
                AddText(" / ", "DarkGray");
                AddText(thisWindow.maxPagination() + 1 + "", "HalfGray");
                AddSmallButton("OtherNextPage", (h) =>
                {
                    if (thisWindow.pagination() < thisWindow.maxPagination())
                    {
                        PlaySound("DesktopChangePage", 0.6f);
                        thisWindow.IncrementPagination();
                    }
                });
                AddSmallButton("OtherPreviousPage", (h) =>
                {
                    if (thisWindow.pagination() > 0)
                    {
                        PlaySound("DesktopChangePage", 0.6f);
                        thisWindow.DecrementPagination();
                    }
                });
            });
            new Description()
            {
                regions = new() { new() { regionType = "Padding", contents = new() { new ()
                    {
                        { "Color", "DarkGray" },
                        { "Text", description != null ? description.Split("$B$B")[staticPagination.ContainsKey("QuestAdd") ? staticPagination["QuestAdd"] : 0] : "" }
                    }
                } } }
            }.Print(currentSave.player, 190, null);
            SetRegionAsGroupExtender();
        }
        if (f == "Turn")
        {
            AddHeaderRegion(() =>
            {
                AddLine("Description: ", "Gray");
                AddText(thisWindow.pagination() + 1 + "", "HalfGray");
                AddText(" / ", "DarkGray");
                AddText(thisWindow.maxPagination() + 1 + "", "HalfGray");
                AddSmallButton("OtherNextPage", (h) =>
                {
                    if (thisWindow.pagination() < thisWindow.maxPagination())
                    {
                        PlaySound("DesktopChangePage", 0.6f);
                        thisWindow.IncrementPagination();
                    }
                });
                AddSmallButton("OtherPreviousPage", (h) =>
                {
                    if (thisWindow.pagination() > 0)
                    {
                        PlaySound("DesktopChangePage", 0.6f);
                        thisWindow.DecrementPagination();
                    }
                });
            });
            new Description()
            {
                regions = new() { new() { regionType = "Padding", contents = new() { new ()
                    {
                        { "Color", "DarkGray" },
                        { "Text", completion != null ? completion.Split("$B$B")[staticPagination.ContainsKey("QuestTurn") ? staticPagination["QuestTurn"] : 0] : "" }
                    }
                } } }
            }.Print(currentSave.player, 190, null);
            SetRegionAsGroupExtender();
        }
        if (f != "Turn" && objective != null)
        {
            AddHeaderRegion(() => AddLine("Objective:"));
            new Description()
            {
                regions = new() { new() { regionType = "Padding", contents = new() { new()
                    {
                        { "Color", "DarkGray" },
                        { "Text", objective }
                    }
                } } }
            }.Print(currentSave.player, 190, null, false);
            if (f == "Log") SetRegionAsGroupExtender();
        }
        if (f == "Log" && conditions != null)
        {
            if (conditions.Count == 0 || currentSave.player.CanTurnQuest(this))
            {
                AddHeaderRegion(() => AddLine("Turn in at:"));
                AddPaddingRegion(() =>
                {
                    AddLine(siteEnd, "HalfGray");
                    AddSmallButton("ItemMiscMap01", (h) =>
                    {
                        CloseDesktop("HostileArea");
                        CloseDesktop("Instance");
                        CloseDesktop("Complex");
                        SwitchDesktop("Map");
                        CloseDesktop("QuestLog");
                        var where = Site.FindSite(x => x.name == siteEnd);
                        CDesktop.cameraDestination = new Vector2(where.x, where.y);
                    });
                });
            }
            else
            {
                AddHeaderRegion(() => AddLine("Details:"));
                foreach (var condition in currentSave.player.currentQuests.Find(x => x.questID == questID).conditions)
                    condition.Print(CDesktop.title == "QuestLog");
            }
        }
        if (rewards != null)
        {
            AddHeaderRegion(() => AddLine("Rewards:"));
            if (experience > 0)
                AddPaddingRegion(() =>
                {
                    if (experience > 0)
                    {
                        AddLine("XP: ", "HalfGray");
                        AddText(experience + "", "Gray");
                    }
                });
            AddPaddingRegion(() =>
            {
                foreach (var item in rewards)
                {
                    var find = Item.items.Find(x => x.name == item.Key);
                    AddBigButton(find.icon, f == "Turn" ? (h) => { chosenReward = find.name; } : null, null, (h) => () =>
                    {
                        if (WindowUp("CraftingSort")) return;
                        if (WindowUp("CraftingSettings")) return;
                        Item.PrintItemTooltip(find, Input.GetKey(KeyCode.LeftShift));
                    });
                    if (find.type != "Miscellaneous" && find.type != "Trade Good" && find.type != "Recipe" && !find.CanEquip(currentSave.player, true)) { SetBigButtonToRed(); AddBigButtonOverlay("OtherGridBlurred"); }
                    if (find.maxStack > 1) SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * (rewards.Keys.ToList().IndexOf(item.Key) % 5), item.Value + "", "", "", "Right");
                    if (find.name == chosenReward && rewards.Count > 1) AddBigButtonOverlay("OtherGlowChosen");
                }
            });
        }
        if (f == "Turn")
            AddButtonRegion(() => { AddLine("Complete the Quest"); }, (h) =>
            {
                if (rewards != null && rewards.Count == 1)
                {
                    var item = Item.items.Find(x => x.name == chosenReward).CopyItem(rewards[chosenReward]);
                    if (!currentSave.player.inventory.CanAddItem(item))
                    {
                        PlaySound("QuestFailed");
                        SpawnFallingText(new Vector2(0, 34), "Inventory full", "Red");
                    }
                    else
                    {
                        PlaySound(item.ItemSound("PutDown"), 0.8f);
                        currentSave.player.inventory.AddItem(item);
                        Foo();
                    }
                }
                else if (rewards != null && rewards.Count > 1)
                {
                    var item = chosenReward == null ? null : Item.items.Find(x => x.name == chosenReward).CopyItem(rewards[chosenReward]);
                    if (item == null)
                    {
                        PlaySound("QuestFailed");
                        SpawnFallingText(new Vector2(0, 34), "No reward chosen", "Red");
                    }
                    else if (!currentSave.player.inventory.CanAddItem(item))
                    {
                        PlaySound("QuestFailed");
                        SpawnFallingText(new Vector2(0, 34), "Inventory full", "Red");
                    }
                    else
                    {
                        PlaySound(item.ItemSound("PutDown"), 0.8f);
                        currentSave.player.inventory.AddItem(item);
                        Foo();
                    }
                }
                else Foo();

                void Foo()
                {
                    currentSave.player.TurnQuest(this);
                    PlaySound("QuestTurn");
                    CloseWindow(h.window);
                    Respawn("HostileAreaQuestAvailable", true);
                    Respawn("TownQuestAvailable", true);
                    Respawn("HostileAreaQuestDone", true);
                    Respawn("TownQuestDone", true);
                    Respawn("Chest", true);
                    Respawn("PlayerMoney", true);
                    Respawn("Capital", true);
                    Respawn("InstanceWing", true);
                    Respawn("Instance", true);
                    Respawn("Complex", true);
                }
            });
        if (f == "Add")
            AddButtonRegion(() => { AddLine("Accept Quest"); }, (h) =>
            {
                if (currentSave.player.CanAddQuest(this))
                {
                    PlaySound("QuestAdd", 0.4f);
                    currentSave.player.AddQuest(quest);
                    CloseWindow(h.window);
                    Respawn("HostileAreaQuestAvailable", true);
                    Respawn("TownQuestAvailable", true);
                    Respawn("HostileAreaQuestDone", true);
                    Respawn("TownQuestDone", true);
                    Respawn("Capital", true);
                    Respawn("HostileArea", true);
                    Respawn("QuestList", true);
                    Respawn("PlayerMoney", true);
                    Respawn("Chest", true);
                    Respawn("InstanceWing", true);
                    Respawn("Instance", true);
                    Respawn("Complex", true);
                }
                else PlaySound("QuestFailed", 0.4f);
            });
    }

    //Copies a quest to a new one for the player to take
    public ActiveQuest CopyQuest()
    {
        var newQuest = new ActiveQuest();
        newQuest.conditions = new();
        if (conditions != null)
            foreach (var condition in conditions)
                newQuest.conditions.Add(new() { name = condition.name, amount = condition.amount, type = condition.type });
        newQuest.questID = questID;
        return newQuest;
    }
}

public class ActiveQuest
{
    //ID of the quest
    public int questID;

    //Conditions for completing the quest
    public List<QuestCondition> conditions;
}
