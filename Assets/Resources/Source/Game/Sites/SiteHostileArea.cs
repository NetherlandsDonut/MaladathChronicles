using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Race;
using static Quest;
using static Faction;
using static SaveGame;
using static Coloring;
using static SiteInstance;

public class SiteHostileArea : Site
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
        if (areaSize < 1) areaSize = 3;
        type ??= "HostileArea";
        if (faction != null)
            if (!factions.Exists(x => x.name == faction))
                factions.Insert(0, new Faction()
                {
                    name = faction,
                    icon = "Faction" + faction,
                    side = "Neutral"
                });
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
            var allianceOnes = all.Where(x => x.side == null || x.side == "Alliance").ToList();
            recommendedLevel.Add("Alliance", allianceOnes.Count == 0 ? 0 : (int)all.Where(x => x.side == null || x.side == "Alliance").Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin));
            var hordeOnes = all.Where(x => x.side == null || x.side == "Horde").ToList();
            recommendedLevel.Add("Horde", hordeOnes.Count == 0 ? 0 : (int)all.Where(x => x.side == null || x.side == "Horde").Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin));
        }
        if (progression == null)
        {
            var length = eliteEncounters != null ? eliteEncounters.Count * 2 + 1 : 3;
            if (areaSize < length) areaSize = length;
            progression = new();
            if (eliteEncounters != null)
                foreach (var boss in eliteEncounters)
                {
                    progression.Add(new AreaProgression() { point = eliteEncounters.IndexOf(boss) * 2 + 2, bossName = boss.who, type = "Boss" });
                    if (eliteEncounters.Last() == boss && instancePart)
                    {
                        var inst = instances.Find(x => x.wings.Any(y => y.areas.Any(z => z["AreaName"] == name)));
                        var wing = inst.wings.Find(x => x.areas.Any(z => z["AreaName"] == name));
                        var areasIn = wing.areas.Select(x => x["AreaName"]).ToList();
                        if (areasIn.Last() != name)
                            progression.Add(new AreaProgression() { point = eliteEncounters.IndexOf(boss) * 2 + 2, areaName = areasIn[areasIn.IndexOf(name) + 1], type = "Area" });
                    }
                }
            progression.Add(new AreaProgression() { point = length, type = "Treasure" });
        }
        SitePath.pathsConnectedToSite.Remove(name);
        SitePath.transportationConnectedToSite.Remove(name);
        Blueprint.windowBlueprints.RemoveAll(x => x.title == "Site: " + name);
        if (x != 0 && y != 0) Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite()));
    }

    //Function to print the site onto the map
    public override void PrintSite()
    {
        SetAnchor(x, y);
        DisableGeneralSprites();
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("Map" + (currentSave.siteVisits.ContainsKey(name) ? type : "Unknown"),
            (h) => { CDesktop.cameraDestination = new Vector2(x, y); },
            (h) =>
            {
                if (zone == "Teldrassil" && zone != FindSite(x => x.name == currentSave.currentSite).zone) return;
                if (h == null) LeadPath();
                else ExecutePath("HostileArea");
            },
            (h) => () =>
            {
                if (!currentSave.siteVisits.ContainsKey(name)) return;
                SetAnchor(TopLeft, 19, -38);
                AddRegionGroup();
                SetRegionGroupWidth(171);
                AddHeaderRegion(() => AddLine(name));
                if (recommendedLevel[currentSave.playerSide] > 0)
                    AddHeaderRegion(() =>
                    {
                        AddLine("Recommended level: ", "DarkGray");
                        AddText(recommendedLevel[currentSave.playerSide] + "", ColorEntityLevel(recommendedLevel[currentSave.playerSide]));
                    });
                var common = CommonEncounters(currentSave.playerSide);
                if (common != null && common.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Common: ", "HalfGray");
                        foreach (var enemy in common)
                        {
                            var race = races.Find(x => x.name == enemy.who);
                            AddSmallButton(race == null ? "OtherUnknown" : race.portrait);
                        }
                    });
                if (eliteEncounters != null && eliteEncounters.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Elite: ", "HalfGray");
                        foreach (var enemy in eliteEncounters)
                        {
                            var race = races.Find(x => x.name == enemy.who);
                            AddSmallButton(race == null ? "OtherUnknown" : race.portrait);
                        }
                    });
                //if (rareEncounters != null && rareEncounters.Count > 0)
                //    AddPaddingRegion(() =>
                //    {
                //        AddLine("Rare: ", "DarkGray");
                //        foreach (var enemy in rareEncounters)
                //            AddSmallButton("OtherUnknown");
                //    });
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
        if (currentSave.player.dead)
            CDesktop.LBWindow().gameObject.SetActive(false);
    }

    //Returns path to a texture that is the background visual of this site
    public override string Background()
    {
        var save = currentSave ?? saves[GameSettings.settings.selectedRealm].Find(x => x.player.name == GameSettings.settings.selectedCharacter);
        return "Areas/Area" + (zone + name).Clean() + (save != null && save.IsNight() && !noNightVariant ? "Night" : "") + (specialClearBackground && eliteEncounters.All(x => save.elitesKilled.ContainsKey(x.who)) ? "Cleared" : "");
    }

    public List<Encounter> CommonEncounters(string side) => commonEncounters?.Where(x => x.side == side || x.side == null).ToList();

    public List<Encounter> RareEncounters(string side) => rareEncounters?.Where(x => x.side == side || x.side == null).ToList();

    public List<Entity> RollEncounters(int amount)
    {
        var alreadyGotRare = false;
        var list = new List<Entity>();
        var common = CommonEncounters(currentSave.playerSide);
        var rare = RareEncounters(currentSave.playerSide);
        Encounter toAdd = null;
        for (int i = 0; i < amount; i++)
        {
            if (rare != null && !alreadyGotRare && Roll(5))
            {
                toAdd = rare[random.Next(0, rare.Count)];
                alreadyGotRare = true;
            }
            else do toAdd = common[random.Next(0, common.Count)];
                while (list.Exists(x => x.name == toAdd.who));
            list.Add(new Entity(random.Next(toAdd.levelMin, toAdd.levelMax == 0 ? toAdd.levelMin + 1 : toAdd.levelMax + 1), races.Find(y => y.name == toAdd.who)));
        }
        return list;
    }

    public Entity RollEncounter(Encounter boss)
    {
        return new Entity(boss.levelMax != 0 ? random.Next(boss.levelMin, boss.levelMax + 1) : boss.levelMin, races.Find(x => x.name == boss.who));
    }

    //Currently opened hostile area
    public static SiteHostileArea area;

    //EXTERNAL FILE: List containing all hostile areas in-game
    public static List<SiteHostileArea> areas;

    //List of all filtered hostile areas by input search
    public static List<SiteHostileArea> areasSearch;
}

public class Encounter
{
    //What race this encounter is
    public string who;

    //For which side this encounter is available,
    //leave this blank to be available to everyone
    public string side;

    //Level range of the encounter,
    //when a single level is desired just leave levelMax blank
    public int levelMin, levelMax;

    //Currently selected encounter
    public static Encounter encounter;
}
