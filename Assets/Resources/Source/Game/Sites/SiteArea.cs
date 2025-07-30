using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using static Coloring;
using static Faction;
using static Quest;
using static Race;
using static Root;
using static Root.Anchor;
using static SaveGame;
using static SitePath;

public class SiteArea : Site
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
        people?.ForEach(x => x.Initialise());
        if (faction != null)
            if (!factions.Exists(x => x.name == faction))
                factions.Insert(0, new Faction()
                {
                    name = faction,
                    icon = "Faction" + faction,
                    side = "Neutral"
                });
        flightPaths = new();
        foreach (var foo in FlightPathGroup.flightPathGroups.FindAll(x => x.sites.Contains(name)))
        {
            if (!flightPaths.ContainsKey(foo.side))
                flightPaths.Add(foo.side, new());
            flightPaths[foo.side].AddRange(foo.sites.Select(x => areas.Find(y => y.name == x)));
        }
        foreach (var foo in flightPaths) foo.Value?.Remove(this);
        if (commonEncounters != null)
            foreach (var encounter in commonEncounters)
                if (!races.Exists(x => x.name == encounter.who))
                    races.Insert(0, new Race()
                    {
                        name = encounter.who,
                        abilities = new(),
                        kind = "Common",
                        portrait = "PortraitChicken",
                        vitality = 1.0,
                    });
                else if (races.Find(x => x.name == encounter.who).kind != "Common")
                    Debug.Log("ERROR 010: " + encounter.who + " isn't a common enemy");
        if (rareEncounters != null)
            foreach (var encounter in rareEncounters)
                if (!races.Exists(x => x.name == encounter.who))
                    races.Insert(0, new Race()
                    {
                        name = encounter.who,
                        abilities = new(),
                        kind = "Rare",
                        portrait = "PortraitParrot",
                        vitality = 2.0,
                    });
                else if (races.Find(x => x.name == encounter.who).kind != "Rare")
                    Debug.Log("ERROR 011: " + encounter.who + " isn't a rare enemy");
        if (eliteEncounters != null)
            foreach (var encounter in eliteEncounters)
                if (!races.Exists(x => x.name == encounter.who))
                    races.Insert(0, new Race()
                    {
                        name = encounter.who,
                        abilities = new(),
                        kind = "Elite",
                        portrait = "PortraitCow",
                        vitality = 3.0,
                    });
                else if (races.Find(x => x.name == encounter.who).kind != "Elite")
                    Debug.Log("ERROR 012: " + encounter.who + " isn't an elite enemy");
        var all = new List<Encounter>();
        if (commonEncounters != null)
            if (commonEncounters.Count > 0)
                all.AddRange(commonEncounters);
            else commonEncounters = null;
        if (rareEncounters != null)
            if (rareEncounters.Count > 0)
                all.AddRange(rareEncounters);
            else rareEncounters = null;
        if (eliteEncounters != null)
            if (eliteEncounters.Count > 0)
                all.AddRange(eliteEncounters);
            else eliteEncounters = null;
        recommendedLevel = new();
        if (all.Count > 0)
        {
            var allianceOnes = all.Where(x => (x.side == null || x.side == "Alliance") && x.levelMin > 0).ToList();
            recommendedLevel.Add("Alliance", allianceOnes.Count == 0 ? 0 : (int)allianceOnes.Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin));
            var hordeOnes = all.Where(x => (x.side == null || x.side == "Horde") && x.levelMin > 0).ToList();
            recommendedLevel.Add("Horde", hordeOnes.Count == 0 ? 0 : (int)hordeOnes.Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin));
        }
        else
        {
            recommendedLevel.Add("Alliance", new());
            recommendedLevel.Add("Horde", new());
        }
        pathsConnectedToSite.Remove(name);
        transportationConnectedToSite.Remove(name);
        Blueprint.windowBlueprints.RemoveAll(x => x.title == "Site: " + name);
        if (x != 0 && y != 0) Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite()));
    }

    public string Icon() 
    {
        var f = factions.Find(x => x.name == faction);
        if (f != null) return f.Icon();
        else return currentSave.siteVisits.ContainsKey(name) ? type + (recommendedLevel["Horde"] < recommendedLevel["Alliance"] ? "HordeAligned" : (recommendedLevel["Horde"] > recommendedLevel["Alliance"] ? "AllianceAligned" : "")) : "Unknown";
    }

    //List of NPC's that are inside of this area
    public List<Person> people;

    //List of area flight paths, these are generated automatically
    [NonSerialized] public Dictionary<string, List<SiteArea>> flightPaths;

    //Currently opened area
    public static SiteArea area;

    //EXTERNAL FILE: List containing all areas in-game
    public static List<SiteArea> areas;

    //List of all filtered areas by input search
    public static List<SiteArea> areasSearch;

    //Prints the site on the world map
    public override void PrintSite()
    {
        SetAnchor(x, y);
        DisableGeneralSprites();
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("Map" + (currentSave.siteVisits.ContainsKey(name) ? Icon() : "Unknown"),
            (h) =>
            {
                CDesktop.cameraDestination = new Vector2(x, y);
            },
            (h) =>
            {
                if (sitePathBuilder != null) return;
                if (h == null) LeadPath();
                else ExecutePath("Area");
            },
            (h) => () =>
            {
                if (!currentSave.siteVisits.ContainsKey(name)) return;
                SetAnchor(TopLeft, 19, -38);
                AddRegionGroup();
                SetRegionGroupWidth(171);
                var f = factions.Find(x => x.name == faction);
                AddHeaderRegion(() => AddLine(name, faction == null ? "" : ColorReputation(currentSave.player.Reputation(faction))));
                if (recommendedLevel[currentSave.playerSide] > 0)
                    AddHeaderRegion(() =>
                    {
                        AddLine("Recommended level: ", "DarkGray");
                        AddText(recommendedLevel[currentSave.playerSide] + "", ColorEntityLevel(currentSave.player, recommendedLevel[currentSave.playerSide]));
                    });
                var common = CommonEncounters(currentSave.playerSide);
                if (common != null && common.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Hostiles: ", "HalfGray");
                        foreach (var enemy in common)
                        {
                            var race = races.Find(x => x.name == enemy.who);
                            AddSmallButton(race == null ? "OtherUnknown" : race.portrait);
                        }
                    });
                var peopleTogether = capitalRedirect != null ? areas.Where(x => x.capitalRedirect == capitalRedirect).SelectMany(x => x.people ?? new()).ToList() : people;
                if (peopleTogether != null && peopleTogether.Count > 0)
                {
                    var legit = peopleTogether.Where(x => !x.hidden && PersonType.personTypes.Exists(y => y.type == x.type)).OrderBy(x => x.category.priority).ThenBy(x => x.type).ToList();
                    var types = legit.Select(x => PersonType.personTypes.Find(y => y.type == x.type)).Where(x => x != null).ToList();
                    var icons = types.Distinct().Select(x => x.icon + (x.factionVariant ? factions.Find(x => x.name == faction)?.side : "")).ToList();
                    var currentRow = 0;
                    var currentAmount = 0;
                    while (icons.Count > 0)
                    {
                        if (currentAmount == 0 && currentRow == 0)
                            AddPaddingRegion(() => AddLine("NPC's:", "HalfGray"));
                        else if (currentAmount == 0)
                            AddPaddingRegion(() => { });
                        AddSmallButton(icons[0]);
                        currentAmount++;
                        if (currentRow == 0 && currentAmount == 6 || currentRow > 0 && currentAmount == 9)
                        {
                            currentRow++;
                            currentAmount = 0;
                        }
                        icons.RemoveAt(0);
                    }
                }
                var q = currentSave.player.QuestsDoneAt(this);
                if (q.Count > 0)
                {
                    AddEmptyRegion();
                    foreach (var quest in q)
                    {
                        AddPaddingRegion(() =>
                        {
                            AddLine(quest.name, "Black");
                            AddSmallButton(quest.ZoneIcon());
                        });
                        var color = ColorQuestLevel(quest.questLevel);
                        if (color != null) SetRegionBackgroundAsImage("SkillUp" + color);
                        AddPaddingRegion(() =>
                        {
                            AddLine("Completed", "Uncommon");
                        });
                    }
                }
                q = currentSave.player.QuestsAt(this);
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
            (h) =>
            {
                if (debug)
                    BuildPath();
            });
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
        return "Areas/Area" + (zone + name).Clean() + (save != null && save.IsNight() && !noNightVariant ? "Night" : "");
    }
}
