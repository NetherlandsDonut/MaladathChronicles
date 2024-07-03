using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Quest;
using static Faction;
using static SaveGame;
using static SitePath;
using static Coloring;

public class SiteTown : Site
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
        if (fishing && !FishingSpot.fishingSpots.Exists(x => x.name == name))
        {
            var fs = new FishingSpot();
            fs.name = name;
            fs.waterType = "Normal";
            if (new List<string>() { "Durotar", "Mulgore", "Teldrassil", "Tirisfal Glades", "Elwynn Forest" }.Contains(zone))
                (fs.skillToFish, fs.difficulty) = (001, 025);
            else if (new List<string>() { "Shadowfang Keep", "Ragefire Chasm", "Wailing Caverns", "The Deadmines", "The Barrens", "Darkshore", "Silverpine Forest", "Westfall", "Loch Modan" }.Contains(zone))
                (fs.skillToFish, fs.difficulty) = (001, 075);
            else if (new List<string>() { "Blackfathom Deeps", "Razorfen Kraul", "Duskwood", "Ashenvale", "Stonetalon Mountains", "Hillsbrad Foothills", "Wetlands", "Redridge Mountains" }.Contains(zone))
                (fs.skillToFish, fs.difficulty) = (055, 150);
            else if (new List<string>() { "Temple of Atal'Hakkar", "Ruins of Stromgarde", "Ruins of Alterac", "Alcaz Island", "Razorfen Downs", "Maraudon", "Swamp of Sorrows", "Stranglethorn Vale", "Desolace", "Dustwallow Marsh", "Thousand Needles", "Alterac Mountains", "Arathi Highlands" }.Contains(zone))
                (fs.skillToFish, fs.difficulty) = (130, 225);
            else if (new List<string>() { "Hearthglen", "Jintha'Alor", "Dire Maul", "Shadow Hold", "Zul'Farrak", "Azshara", "Felwood", "Feralas", "Un'Goro Crater", "Tanaris", "Moonglade", "Western Plaguelands", "Hinterlands" }.Contains(zone))
                (fs.skillToFish, fs.difficulty) = (205, 300);
            else if (new List<string>() { "Upper Blackrock Spire", "Scholomance", "Stratholme", "Lower Blackrock Spire", "Tyr's Hand", "Blackrock Mountain", "Blackrock Depths", "Blackwing Lair", "Molten Core", "Temple of Ahn'Qiraj", "Ruins of Ahn'Qiraj", "Zul'Gurub", "Winterspring", "Silithus", "Eastern Plaguelands", "Deadwind Pass", "Burning Steppes" }.Contains(zone))
                (fs.skillToFish, fs.difficulty) = (330, 425);
            fs.possibleLoot = new() { };
            FishingSpot.fishingSpots.Add(fs);
        }
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
        foreach (var foo in FlightPathGroup.flightPathGroups.FindAll(x => x.sitesConnected.Contains(name)))
        {
            if (!flightPaths.ContainsKey(foo.side))
                flightPaths.Add(foo.side, new());
            flightPaths[foo.side].AddRange(foo.sitesConnected.Select(x => towns.Find(y => y.name == x)));
        }
        foreach (var foo in flightPaths)
            foo.Value?.Remove(this);
        pathsConnectedToSite.Remove(name);
        transportationConnectedToSite.Remove(name);
        if (x != 0 && y != 0)
            Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite()));
    }

    public string Icon() 
    {
        var f = factions.Find(x => x.name == faction);
        if (f != null) return f.Icon();
        else return "HostileArea";
    }

    //List of NPC's that are inside of this town
    public List<Person> people;

    //List of town flight paths, these are generated automatically
    [NonSerialized] public Dictionary<string, List<SiteTown>> flightPaths;

    //Currently opened town
    public static SiteTown town;

    //EXTERNAL FILE: List containing all towns in-game
    public static List<SiteTown> towns;

    //List of all filtered towns by input search
    public static List<SiteTown> townsSearch;

    //Prints the site on the world map
    public override void PrintSite()
    {
        SetAnchor(x, y);
        DisableGeneralSprites();
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("Map" + (currentSave.siteVisits.ContainsKey(name) ? Icon() : "Unknown"),
            (h) => { CDesktop.cameraDestination = new Vector2(x, y); },
            (h) =>
            {
                if (zone == "Teldrassil" && zone != FindSite(x => x.name == currentSave.currentSite).zone) return;
                if (h == null) LeadPath();
                else ExecutePath("Town");
            },
            (h) => () =>
            {
                if (!currentSave.siteVisits.ContainsKey(name)) return;
                SetAnchor(TopLeft, 19, -38);
                AddRegionGroup();
                SetRegionGroupWidth(171);
                var f = factions.Find(x => x.name == faction);
                var side = f == null ? "Neutral" : f.side;
                AddHeaderRegion(() =>
                {
                    AddLine(name, ColorReputation(currentSave.player.Reputation(faction)));
                });
                if (people != null)
                {
                    var legit = people.Where(x => !x.hidden && PersonType.personTypes.Exists(y => y.type == x.type)).OrderBy(x => x.category.priority).ThenBy(x => x.type).ToList();
                    var types = legit.Select(x => PersonType.personTypes.Find(y => y.type == x.type)).Where(x => x != null).ToList();
                    var icons = types.Distinct().Select(x => x.icon + (x.factionVariant ? factions.Find(x => x.name == faction)?.side : "")).ToList();
                    var perRow = 9;
                    if (icons.Count > 0)
                        for (int i = 0; i < Math.Ceiling(icons.Count / (double)perRow); i++)
                        {
                            var ind = i;
                            AddPaddingRegion(() =>
                            {
                                for (int j = perRow - 1; j >= 0; j--)
                                    if (j < icons.Count - ind * perRow)
                                    {
                                        var jnd = j;
                                        AddSmallButton(icons[jnd + ind * perRow]);
                                    }
                            });
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
        return "Areas/Area" + (zone + name).Clean() + (save != null && save.IsNight() && !noNightVariant ? "Night" : "");
    }
}
