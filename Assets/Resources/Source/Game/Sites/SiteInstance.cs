using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Quest;
using static Faction;
using static SaveGame;
using static SiteArea;
using static Coloring;

public class SiteInstance : Site
{
    //Instance wings that store all the instance's areas
    public List<InstanceWing> wings;

    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
        type ??= "Dungeon";
        if (faction != null)
            if (!factions.Exists(x => x.name == faction))
                factions.Insert(0, new Faction()
                {
                    name = faction,
                    icon = "Faction" + faction,
                    side = "Neutral"
                });
        if (wings != null)
            foreach (var wing in wings)
                if (wing.areas != null)
                    foreach (var area in wing.areas)
                        if (area.ContainsKey("AreaName"))
                            if (!areas.Exists(x => x.name == area["AreaName"]))
                                areas.Insert(0, new SiteArea()
                                {
                                    name = area["AreaName"],
                                    commonEncounters = new(),
                                    rareEncounters = new(),
                                    eliteEncounters = new(),
                                    type = "HostileArea",
                                    zone = name
                                });
        var localAreas = wings.SelectMany(x => x.areas.Select(y => y.ContainsKey("AreaName") ? y["AreaName"] : ""));
        var temp = areas.FindAll(x => localAreas.Contains(x.name));
        temp.ForEach(x => x.instancePart = true);
        SitePath.pathsConnectedToSite.Remove(name);
        if (x != 0 && y != 0) Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite()));
    }

    //Suggested level range for the player to enter this instance
    private (int, int) LevelRange(List<string> localAreas)
    {
        var temp = areas.FindAll(x => localAreas.Contains(x.name)).Where(x => x.recommendedLevel[currentSave.playerSide] > 0);
        if (temp.Count() == 0) return (0, 0);
        return (temp.Min(x => x.recommendedLevel[currentSave.playerSide]), temp.Max(x => x.recommendedLevel[currentSave.playerSide]));
    }

    //Suggested level range for the player to enter this instance
    public (int, int) LevelRange(int wing = -1)
    {
        if (wing > -1 && wing < wings.Count) return LevelRange(wings[wing].areas.Select(x => x["AreaName"]).ToList());
        return LevelRange(wings.SelectMany(x => x.areas.Select(y => y["AreaName"])).ToList());
    }

    //Currently opened instance
    public static SiteInstance instance;

    //Currently opened instance wing
    public static InstanceWing wing;

    //EXTERNAL FILE: List containing all instances in-game
    public static List<SiteInstance> instances;

    //List of all filtered instances by input search
    public static List<SiteInstance> instancesSearch;

    //Function to print the site onto the map
    public override void PrintSite()
    {
        SetAnchor(x, y);
        DisableGeneralSprites();
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton(currentSave.siteVisits.ContainsKey(name) ? "Map" + type : "MapUnknown",
            (h) =>
            {
                CDesktop.cameraDestination = new Vector2(x, y);
            },
            (h) =>
            {
                if (!canDrawPaths) return;
                if (sitePathBuilder != null) return;
                if (h == null) LeadPath();
                else ExecutePath("Instance");
            },
            !currentSave.siteVisits.ContainsKey(name) ? null :
            (h) => () =>
            {
                if (!currentSave.siteVisits.ContainsKey(name)) return;
                SetAnchor(TopLeft, 19, -38);
                AddRegionGroup();
                SetRegionGroupWidth(171);
                AddHeaderRegion(() => AddLine(name));
                AddPaddingRegion(() =>
                {
                    AddLine("Level range: ", "DarkGray");
                    var range = LevelRange();
                    AddText(range.Item1 + "", ColorEntityLevel(currentSave.player, range.Item1));
                    AddText(" - ", "DarkGray");
                    AddText(range.Item2 + "", ColorEntityLevel(currentSave.player, range.Item2));
                });
                var areas = wings.SelectMany(x => x.areas.Select(y => SiteArea.areas.Find(z => z.name == y["AreaName"])));
                var side = currentSave.playerSide;
                var total = areas.SelectMany(x => x.CommonEncounters(side) ?? new()).Distinct().ToList();
                //total.AddRange(areas.SelectMany(x => x.eliteEncounters ?? new()).Distinct().ToList());
                //total.AddRange(areas.SelectMany(x => x.rareEncounters ?? new()).Distinct().ToList());
                var races = total.Select(x => Race.races.Find(y => y.name == x.who).portrait).Distinct().ToList();
                var currentRow = 0;
                var currentAmount = 0;
                while (races.Count > 0)
                {
                    if (currentAmount == 0 && currentRow == 0)
                        AddPaddingRegion(() => AddLine("Hostiles:", "HalfGray"));
                    if (currentAmount == 0)
                        AddPaddingRegion(() => ReverseButtons());
                    AddSmallButton(races[0]);
                    currentAmount++;
                    if (currentRow == 0 && currentAmount == 9 || currentRow > 0 && currentAmount == 9)
                    {
                        currentRow++;
                        currentAmount = 0;
                    }
                    races.RemoveAt(0);
                }
                var q = currentSave.player.QuestsAt(this);
                if (q.Count > 0)
                {
                    AddEmptyRegion();
                    foreach (var quest in q)
                    {
                        var ogQuest = Quest.quests.Find(x => x.questID == quest.questID);
                        var con = quest.conditions.FindAll(x => !x.IsDone() && x.Where(false).Contains(this));
                        AddPaddingRegion(() =>
                        {
                            AddLine(ogQuest.name, "Black");
                            AddSmallButton(ogQuest.ZoneIcon());
                        });
                        var color = ColorQuestLevel(ogQuest.questLevel);
                        if (color != null) SetRegionBackgroundAsImage("SkillUp" + color);
                        if (con.Count > 0)
                            foreach (var condition in con)
                                condition.Print(false);
                    }
                }
            },
            (h) =>
            {
                if (debug)
                    BuildPath();
            });
            var news = currentSave.player.AvailableQuestsAt(this);
            sitesWithQuestMarkers.Remove(this);
            if (currentSave.currentSite == name)
                AddSmallButtonOverlay("PlayerLocationFromBelow", 0, 3);
            var todos = currentSave.player.QuestsAt(this, true);
            var normals = news.Count(x => currentSave.player.WillGetExperience(x.questLevel)) > 0;
            var lows = news.Count(x => !currentSave.player.WillGetExperience(x.questLevel)) > 0;
            var returns = currentSave.player.QuestsDoneAt(this, true).Count > 0;
            if (todos.Count > 0 || normals || (lows && GameSettings.settings.showLowLevelQuests.Value()) || returns) sitesWithQuestMarkers.Add(this);
            if (returns && normals) AddSmallButtonOverlay("AvailableQuestReturnNormal", 0, 3);
            else if (returns && lows && GameSettings.settings.showLowLevelQuests.Value()) AddSmallButtonOverlay("AvailableQuestReturnLow", 0, 3);
            else if (returns) AddSmallButtonOverlay("AvailableQuestReturn", 0, 3);
            else if (normals) AddSmallButtonOverlay("AvailableQuestNormal", 0, 3);
            else if (lows && GameSettings.settings.showLowLevelQuests.Value()) AddSmallButtonOverlay("AvailableQuestLow", 0, 3);
            if (todos.Count > 0) AddSmallButtonOverlay("QuestMarker", 0, 3);
        });
    }

    //Returns path to a texture that is the background visual of this site
    public override string Background()
    {
        var save = currentSave ?? selectedSave;
        return "Areas/Area" + name.Clean() + (save != null && save.IsNight() && !noNightVariant ? "Night" : "");
    }

    public static void PrintInstanceWing(SiteInstance instance, InstanceWing wing)
    {
        //if (instance.wings.Count > 1)
        //    AddHeaderRegion(() => { AddLine(wing.name); });
        var temp = wing.areas.Select(x => (areas.Find(y => x.ContainsKey("AreaName") && y.name == x["AreaName"]), x)).ToList();
        foreach (var area in temp)
            if (showAreasUnconditional || area.x.ContainsKey("OpenByDefault") && area.x["OpenByDefault"] == "True" || currentSave.unlockedAreas.Contains(area.Item1.name))
                AddButtonRegion(() =>
                {
                    var name = area.Item1 != null ? area.Item1.name : "AREA NOT FOUND";
                    AddLine(name);
                    if (currentSave.siteProgress.ContainsKey(area.Item1.name) && area.Item1.areaSize == currentSave.siteProgress[area.Item1.name])
                        SetRegionBackgroundAsImage("ClearedArea");
                },
                (h) =>
                {
                    if (area.Item1 == null) return;
                    SiteArea.area = area.Item1;
                    CloseWindow("AreaQuestTracker");
                    Respawn("Area");
                    Respawn("AreaProgress");
                    Respawn("AreaElites");
                    Respawn("Chest");
                    SetDesktopBackground("Areas/Area" + (instance.name + area.Item1.name).Clean() + (area.Item1.specialClearBackground && area.Item1.eliteEncounters.All(x => currentSave.elitesKilled.ContainsKey(x.who)) ? "Cleared" : ""));
                });
            else
                AddPaddingRegion(() => AddLine("?", "DimGray"));
    }
}

public class InstanceWing
{
    //Name of the instance wing
    public string name;

    //Areas during nighttime usually change visuals.
    //Sites marked with this boolean as true keep it always the same.
    public bool noNightVariant;

    //List of all areas in this instance wing
    public List<Dictionary<string, string>> areas;

    //Returns path to a texture that is the background visual of this site
    public string Background()
    {
        return "Areas/Area" + SiteInstance.instance.name.Clean() + (SiteInstance.instance.wings.IndexOf(this) + 1) + (currentSave != null && currentSave.IsNight() && !noNightVariant ? "Night" : "");
    }
}
