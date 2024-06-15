using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Quest;
using static Faction;
using static Coloring;
using static SaveGame;
using static SiteHostileArea;

public class SiteInstance : Site
{
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
                                areas.Insert(0, new SiteHostileArea()
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
    public (int, int) LevelRange()
    {
        var localAreas = wings.SelectMany(x => x.areas.Select(y => y.ContainsKey("AreaName") ? y["AreaName"] : ""));
        var temp = areas.FindAll(x => localAreas.Contains(x.name));
        if (temp.Count() == 0) return (0, 0);
        return (temp.Min(x => x.recommendedLevel), temp.Max(x => x.recommendedLevel));
    }

    //Currently opened instance
    public static SiteInstance instance;

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
            (h) => { CDesktop.cameraDestination = new Vector2(x, y); },
            (h) =>
            {
                if (h == null) LeadPath();
                else ExecutePath("Instance");
            },
            (h) => () =>
            {
                if (!currentSave.siteVisits.ContainsKey(name)) return;
                SetAnchor(TopLeft, 19, -38);
                AddRegionGroup();
                SetRegionGroupWidth(171);
                AddHeaderRegion(() => { AddLine(name); });
                AddPaddingRegion(() =>
                {
                    AddLine("Level range: ", "DarkGray");
                    var range = LevelRange();
                    AddText(range.Item1 + "", ColorEntityLevel(range.Item1));
                    AddText(" - ", "DarkGray");
                    AddText(range.Item2 + "", ColorEntityLevel(range.Item2));
                });
                var areas = wings.SelectMany(x => x.areas.Select(y => SiteHostileArea.areas.Find(z => z.name == y["AreaName"])));
                var total = areas.SelectMany(x => x.commonEncounters ?? new()).Distinct().ToList();
                total.AddRange(areas.SelectMany(x => x.eliteEncounters ?? new()).Distinct().ToList());
                total.AddRange(areas.SelectMany(x => x.rareEncounters ?? new()).Distinct().ToList());
                var races = total.Select(x => Race.races.Find(y => y.name == x.who).portrait).Distinct().ToList();
                var perRow = 9;
                if (races.Count > 0)
                    for (int i = 0; i < Math.Ceiling(races.Count / (double)perRow); i++)
                    {
                        var ind = i;
                        AddPaddingRegion(() =>
                        {
                            for (int j = 0; j < perRow && j < races.Count - ind * perRow; j++)
                            {
                                var jnd = j;
                                AddSmallButton(races[jnd + ind * perRow]);
                            }
                        });
                    }
                var q = currentSave.player.QuestsAt(this);
                if (q.Count > 0)
                {
                    AddEmptyRegion();
                    foreach (var quest in q)
                    {
                        var con = quest.conditions.FindAll(x => !x.IsDone() && x.Where().Contains(this));
                        AddPaddingRegion(() =>
                        {
                            AddLine(quest.name, "Black");
                            AddSmallButton(quest.ZoneIcon());
                        });
                        var color = ColorQuestLevel(quest.questLevel);
                        if (color != null) SetRegionBackgroundAsImage("SkillUp" + color);
                        if (con.Count > 0)
                            foreach (var condition in con)
                                condition.Print(false);
                    }
                }
            },
            (h) => { BuildPath(); });
            var q = currentSave.player.AvailableQuestsAt(this, true).Count;
            sitesWithQuestMarkers.Remove(this);
            if (currentSave.currentSite == name)
                AddSmallButtonOverlay("PlayerLocationFromBelow", 0, 3);
            var a = q > 0;
            var b = currentSave.player.QuestsAt(this, true).Count > 0;
            if (a || b) sitesWithQuestMarkers.Add(this);
            if (a) AddSmallButtonOverlay("AvailableQuest", 0, 3);
            if (b) AddSmallButtonOverlay("QuestMarker", 0, 3);
            if (currentSave.player.QuestsDoneAt(this, true).Count > 0)
                AddSmallButtonOverlay("YellowGlowBig", 0, 2);
        });
    }

    //Returns path to a texture that is the background visual of this site
    public override string Background()
    {
        var save = currentSave ?? saves[GameSettings.settings.selectedRealm].Find(x => x.player.name == GameSettings.settings.selectedCharacter);
        return "Areas/Area" + name.Clean() + (save != null && save.IsNight() && !noNightVariant ? "Night" : "");
    }

    public static void PrintInstanceSite(Dictionary<string, string> site)
    {
        var find = areas.Find(x => x.name == site["AreaName"]);
        if (find != null && (showAreasUnconditional || site.ContainsKey("OpenByDefault") && site["OpenByDefault"] == "True" || currentSave.unlockedAreas.Contains(find.name)))
            AddButtonRegion(() =>
            {
                AddLine(find.name);
                if (currentSave.siteProgress.ContainsKey(find.name) && find.areaSize == currentSave.siteProgress[find.name])
                    SetRegionBackgroundAsImage("ClearedArea");
            },
            (h) =>
            {
                area = find;
                Respawn("HostileArea");
                Respawn("HostileAreaProgress");
                Respawn("HostileAreaDenizens");
                Respawn("HostileAreaElites");
                Respawn("Chest");
                SetDesktopBackground(area.Background());
            });
        else
            AddPaddingRegion(() =>
            {
                AddLine("?", "DimGray");
            });
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
                    SiteHostileArea.area = area.Item1;
                    Respawn("HostileArea");
                    Respawn("HostileAreaProgress");
                    Respawn("HostileAreaDenizens");
                    Respawn("HostileAreaElites");
                    Respawn("Chest");
                    SetDesktopBackground("Areas/Area" + (instance.name + area.Item1.name).Clean() + (area.Item1.specialClearBackground && area.Item1.eliteEncounters.All(x => currentSave.elitesKilled.ContainsKey(x.who)) ? "Cleared" : ""));
                });
            else
                AddPaddingRegion(() =>
                {
                    AddLine("?", "DimGray");
                });
    }
}

public class InstanceWing
{
    public string name;
    public List<Dictionary<string, string>> areas;
}
