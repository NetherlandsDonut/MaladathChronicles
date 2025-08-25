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
        if (zone == null)
        {
            if (siteStart != null)
            {
                var find = Site.FindSite(x => x.name == siteStart);
                if (find != null) zone = find.zone;
            }
            else if (siteEnd != null)
            {
                var find = Site.FindSite(x => x.name == siteEnd);
                if (find != null) zone = find.zone;
            }
        }
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

    //Adds all related to this quest sites to the respawn queue
    public void UpdateRelatedSites()
    {
        //Prepare a list of all sites to respawn
        var sitesToRespawn = new List<Site>();

        //Add all sites where the quest's objectives can be done
        foreach (var site in currentSave.player.WhereCanQuestBeDone(this, true))
            if (!sitesToRespawn.Contains(site))
                if (site.convertDestinationTo != null) sitesToRespawn.Add(Site.FindSite(x => x.name == site.convertDestinationTo));
                else sitesToRespawn.Add(site);

        //If starting site exists, add it to the list
        var findSite = Site.FindSite(x => x.name == siteStart);
        if (findSite != null) sitesToRespawn.Add(findSite.convertDestinationTo == null ? findSite : Site.FindSite(x => x.name == findSite.convertDestinationTo));

        //If ending site exists, add it to the list
        findSite = Site.FindSite(x => x.name == siteEnd);
        if (findSite != null) sitesToRespawn.Add(findSite.convertDestinationTo == null ? findSite : Site.FindSite(x => x.name == findSite.convertDestinationTo));

        //Add all distinct direct relations to respawn queue
        foreach (var site in sitesToRespawn)
            if (!Quest.sitesToRespawn.Contains(site))
                Quest.sitesToRespawn.Add(site);
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

    //Name of the object/person that player is interacting with to get the quest. When itemStart is set instead of siteStart then the name used will be the item name instead
    public string startTalkName;

    //Portrait that appears on the screen when the player is trying to accept the quest. When itemStart is set instead of siteStart then the portrait used will be the item icon instead
    public string startTalkPortrait;

    //Site where the quest can be handed in
    public string siteEnd;

    //Name of the object/person player is handing the quest over to
    public string endTalkName;

    //Portrait that appears on the screen when the player is handing in the quest
    public string endTalkPortrait;

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

    //Unique items required to make this quest appear
    public List<string> requiredUniqueItems;

    //Amount of reputation awarded to the quest faction
    public Dictionary<string, int> reputationGain;

    //Items provided by the quest when player accepts it
    public Dictionary<string, int> providedItems;

    //Conditions for completing the quest
    public List<QuestCondition> conditions;

    //Zone icon for the quest
    public string ZoneIcon() => zone != null ? "Zone" + Zone.zones.Find(x => x.name == zone).icon.Clean() : "OtherUnknown";
    
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
                        if (CDesktop.title != "Map")
                        {
                            Respawn("PlayerMoney");
                            Respawn("InstanceWing");
                            Respawn("Instance");
                            Respawn("Complex");
                        }
                        PlaySound("DesktopInstanceClose");
                    });
                    AddSmallButton("OtherTrashOff");
                }
                else
                {
                    AddSmallButton("OtherClose", (h) =>
                    {
                        CloseWindow("Quest");
                        if (CDesktop.title != "Map")
                        {
                            Respawn("PlayerMoney");
                            Respawn("InstanceWing");
                            Respawn("Instance");
                            Respawn("Complex");
                        }
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
                    if (CDesktop.title != "Map")
                    {
                        Respawn("PlayerMoney");
                        Respawn("InstanceWing");
                        Respawn("Instance");
                        Respawn("Complex");
                    }
                });
        });
        if (color != null) SetRegionBackgroundAsImage("SkillUp" + color + "Long");
        var regionGroup = CDesktop.LBWindow().LBRegionGroup();
        if (f == "Add")
        {
            AddHeaderRegion(() =>
            {
                AddLine("Description: ", "Gray");
                if (thisWindow.maxPagination() > 0)
                {
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
                }
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
                if (thisWindow.maxPagination() > 0)
                {
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
                }
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
                        CloseDesktop("Area");
                        CloseDesktop("Instance");
                        CloseDesktop("Complex");
                        SwitchDesktop("Map");
                        var where = Site.FindSite(x => x.name == siteEnd);
                        CDesktop.cameraDestination = new Vector2(where.x, where.y);
                    });
                });
            }
            else
            {
                AddHeaderRegion(() => AddLine("Details:"));
                foreach (var condition in currentSave.player.currentQuests.Find(x => x.questID == questID).conditions)
                    condition.Print(CDesktop.title == "Map");
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
                    if (GameSettings.settings.rarityIndicators.Value())
                        AddBigButtonOverlay("OtherRarity" + find.rarity + (GameSettings.settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 3);
                    if (find.type != "Miscellaneous" && find.type != "Trade Good" && find.type != "Recipe" && !find.HasProficiency(currentSave.player, true)) { SetBigButtonToRedscale(); AddBigButtonOverlay("OtherGridBlurred"); }
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
                        SpawnFallingText(new Vector2(0, 34), "Inventory is full", "Red");
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
                        SpawnFallingText(new Vector2(0, 34), "Inventory is full", "Red");
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
                    if (CDesktop.title != "Map")
                    {
                        Respawn("Area");
                        Respawn("AreaQuestAvailable");
                        Respawn("AreaQuestDone");
                        Respawn("PlayerMoney");
                        Respawn("InstanceWing");
                        Respawn("Instance");
                        Respawn("Complex");
                    }
                    else Respawn("QuestList");
                }
            });
        if (f == "Add")
            AddButtonRegion(() => { AddLine("Accept Quest"); }, (h) =>
            {
                if (currentSave.player.CanAddQuest(this))
                {
                    PlaySound("QuestAdd", 0.4f);
                    currentSave.player.AddQuest(quest);
                    quest = null;
                    CloseWindow(h.window);
                    if (CDesktop.title != "Map")
                    {
                        Respawn("Area");
                        Respawn("AreaQuestAvailable");
                        Respawn("AreaQuestDone");
                        Respawn("PlayerMoney");
                        Respawn("InstanceWing");
                        Respawn("Instance");
                        Respawn("Complex");
                    }
                    else Respawn("QuestList");
                }
                else
                {
                    PlaySound("QuestFailed");
                    SpawnFallingText(new Vector2(0, 34), "Inventory is full", "Red");
                }
            });
    }

    //Copies a quest to a new one for the player to take
    public ActiveQuest CopyQuest()
    {
        var newQuest = new ActiveQuest { conditions = new() };
        if (conditions != null)
            foreach (var condition in conditions)
                newQuest.conditions.Add(new() { type = condition.type, name = condition.name, value = condition.value, amount = condition.amount, isItemNotTaken = condition.isItemNotTaken, sites = condition.sites?.ToList() });
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
