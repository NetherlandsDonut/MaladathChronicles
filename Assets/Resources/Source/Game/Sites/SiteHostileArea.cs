using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;
using static Root.RegionBackgroundType;

using static Race;
using static Sound;
using static Faction;
using static MapGrid;
using static SaveGame;
using static Coloring;
using static SiteComplex;
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
        if (all.Count > 0) recommendedLevel = (int)all.Average(x => x.levelMax != 0 ? (x.levelMin + x.levelMax) / 2.0 : x.levelMin);
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
                        {
                            progression.Add(new AreaProgression() { point = eliteEncounters.IndexOf(boss) * 2 + 2, areaName = areasIn[areasIn.IndexOf(name) + 1], type = "Area" });
                        }
                    }
                }
            progression.Add(new AreaProgression() { point = length, type = "Treasure" });
        }
        if (!Blueprint.windowBlueprints.Exists(x => x.title == "HostileArea: " + name))
            Blueprint.windowBlueprints.Add(
                new Blueprint("HostileArea: " + name,
                    () =>
                    {
                        if (ambience == null)
                        {
                            var zone = Zone.zones.Find(x => x.name == this.zone);
                            if (zone != null) PlayAmbience(currentSave.IsNight() ? zone.ambienceNight : zone.ambienceDay);
                        }
                        else PlayAmbience(ambience);
                        SetAnchor(TopLeft, 19, -38);
                        AddHeaderGroup();
                        SetRegionGroupWidth(200);
                        AddHeaderRegion(() =>
                        {
                            AddLine(name);
                            AddSmallButton("OtherClose",
                            (h) =>
                            {
                                PlaySound("DesktopInstanceClose");
                                if (instancePart)
                                {
                                    SetDesktopBackground(instance.Background());
                                    CloseWindow(h.window);
                                    CloseWindow("BossQueue");
                                    Respawn("InstanceLeftSide");
                                }
                                else if (complexPart)
                                {
                                    SetDesktopBackground(complex.Background());
                                    CloseWindow(h.window);
                                    CloseWindow("BossQueue");
                                    Respawn("ComplexLeftSide");
                                }
                                else CloseDesktop("HostileArea");
                            });
                        });
                        AddPaddingRegion(() =>
                        {
                            AddLine("Recommended level: ", "DarkGray");
                            AddText(recommendedLevel + "", ColorEntityLevel(recommendedLevel));
                        });
                        if (commonEncounters != null && commonEncounters.Count > 0)
                        {
                            //if (currentSave.siteProgress.ContainsKey(name) && eliteEncounters != null && eliteEncounters.Count > 0 /*&& eliteEncounters.Sum(x => x.requiredProgress) <= currentSave.siteProgress[name]*/)
                            //    foreach (var encounter in commonEncounters)
                            //        AddButtonRegion(() =>
                            //        {
                            //            AddLine(encounter.who, "", "Right");
                            //            var race = races.Find(x => x.name == encounter.who);
                            //            AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                            //        },
                            //        (h) =>
                            //        {
                            //            Board.NewBoard(RollEncounter(encounter), this);
                            //            SpawnDesktopBlueprint("Game");
                            //            SwitchDesktop("Game");
                            //        });
                            //else
                            foreach (var encounter in commonEncounters)
                                AddPaddingRegion(() =>
                                {
                                    AddLine(encounter.who, "DarkGray", "Right");
                                    var race = races.Find(x => x.name == encounter.who);
                                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                });
                        }
                        AddButtonRegion(() => { AddLine("Explore", "Black"); },
                        (h) =>
                        {
                            Board.NewBoard(RollEncounter(), this);
                            SpawnDesktopBlueprint("Game");
                            SwitchDesktop("Game");
                        });
                        if (progression != null && progression.Count > 0)
                            for (int i = 0; i <= areaSize; i++)
                            {
                                var index = i;
                                if (index > 0)
                                {
                                    var progressions = progression.FindAll(x => x.point == index);
                                    var printType = "";
                                    if (progressions.Exists(x => x.type == "Boss") && progressions.Exists(x => x.type == "Area")) printType = "BossArea";
                                    else if (progressions.Exists(x => x.type == "Treasure") && progressions.Exists(x => x.type == "Area")) printType = "TreasureArea";
                                    else if (progressions.Exists(x => x.type == "Boss")) printType = "Boss";
                                    else if (progressions.Exists(x => x.type == "Treasure")) printType = "Treasure";
                                    else if (progressions.Exists(x => x.type == "Area")) printType = "Area";
                                    if (printType != "")
                                    {
                                        var marker = new GameObject("ProgressionMarker", typeof(SpriteRenderer));
                                        marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/Progress" + printType);
                                        marker.transform.parent = CDesktop.LBWindow.LBRegionGroup.LBRegion.transform;
                                        marker.transform.localPosition = new Vector3(1 + CDesktop.LBWindow.LBRegionGroup.setWidth, -6);
                                    }
                                }
                                if (i < areaSize)
                                {
                                    AddRegionGroup();
                                    SetRegionGroupWidth((i == areaSize - 1 ? 200 % areaSize : 0) + 200 / areaSize);
                                    SetRegionGroupHeight(2);
                                    AddPaddingRegion(() =>
                                    {
                                        var temp = currentSave.siteProgress.ContainsKey(name) ? currentSave.siteProgress[name] : 0;
                                        if (temp > index) SetRegionBackground(ProgressDone);
                                        else SetRegionBackground(ProgressEmpty);
                                    });
                                }
                            }
                    }
                )
            );
        SitePath.pathsConnectedToSite.Remove(name);
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
                if (h == null) LeadPath();
                else ExecutePath("HostileArea");
            },
            (h) => () =>
            {
                if (!currentSave.siteVisits.ContainsKey(name)) return;
                SetAnchor(TopRight, -19, -38);
                AddRegionGroup();
                AddHeaderRegion(() =>
                {
                    AddLine(name);
                });
                AddHeaderRegion(() =>
                {
                    AddLine("Recommended level: ");
                    AddText(recommendedLevel + "", ColorEntityLevel(recommendedLevel));
                });
                if (commonEncounters != null && commonEncounters.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Common: ");
                        foreach (var enemy in commonEncounters)
                        {
                            var race = races.Find(x => x.name == enemy.who);
                            AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                        }
                    });
                if (eliteEncounters != null && eliteEncounters.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Elite: ");
                        foreach (var enemy in eliteEncounters)
                        {
                            var race = races.Find(x => x.name == enemy.who);
                            AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                        }
                    });
                if (rareEncounters != null && rareEncounters.Count > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Rare: ");
                        foreach (var enemy in rareEncounters)
                            AddSmallButton("OtherUnknown", (h) => { });
                    });
            },
            (h) => { BuildPath(); });
            if (currentSave.currentSite == name)
                AddSmallButtonOverlay("PlayerLocation", 0, 2);
        });
    }

    //Returns path to a texture that is the background visual of this site
    public override string Background()
    {
        var save = currentSave ?? saves[GameSettings.settings.selectedRealm].Find(x => x.player.name == GameSettings.settings.selectedCharacter);
        return "Areas/Area" + (zone + name).Clean() + (save.IsNight() && !noNightVariant ? "Night" : "") + (specialClearBackground && eliteEncounters.All(x => save.elitesKilled.ContainsKey(x.who)) ? "Cleared" : "");
    }

    public Entity RollEncounter()
    {
        var encounters = commonEncounters.Select(x => (x.levelMax != 0 ? random.Next(x.levelMin, x.levelMax + 1) : x.levelMin, races.Find(y => y.name == x.who))).ToList();
        if (Roll(5) && rareEncounters != null)
        {
            var rares = rareEncounters.FindAll(x => !currentSave.raresKilled.ContainsKey(x.who));
            if (rares.Count > 0) encounters = rares.Select(x => (x.levelMax != 0 ? random.Next(x.levelMin, x.levelMax + 1) : x.levelMin, races.Find(y => y.name == x.who))).ToList();
        }
        var find = encounters[random.Next(0, encounters.Count)];
        return new Entity(find.Item1, find.Item2);
    }

    public Entity RollEncounter(Encounter boss)
    {
        return new Entity(boss.levelMax != 0 ? random.Next(boss.levelMin, boss.levelMax + 1) : boss.levelMin, races.Find(x => x.name == boss.who));
    }

    //Tells program whether this area has a special
    //clear background that is shown only after clearing the area
    public bool specialClearBackground;

    //List of possible common encounters in this area
    public List<Encounter> commonEncounters;

    //List of possible rare encounters in this area
    public List<Encounter> rareEncounters;

    //List of special elite encounters in this area
    public List<Encounter> eliteEncounters;

    //Size of the area
    public int areaSize;

    //Names for area sizes
    public static Dictionary<int, string> areaSizeNames = new()
    {
        { 1, "Tiny" },
        { 2, "Tiny" },
        { 3, "Small" },
        { 4, "Medium" },
        { 5, "Large" },
        { 6, "Huge" },
    };

    //List of of progression points in the area
    public List<AreaProgression> progression;

    //Automatically calculated number that suggests
    //at which level player should enter this area
    [NonSerialized] public int recommendedLevel;

    //Determines whether this area is part of an instance
    [NonSerialized] public bool instancePart;

    //Determines whether this area is part of a complex
    [NonSerialized] public bool complexPart;

    //Currently opened hostile area
    public static SiteHostileArea area;

    //EXTERNAL FILE: List containing all hostile areas in-game
    public static List<SiteHostileArea> areas;

    //List of all filtered hostile areas by input search
    public static List<SiteHostileArea> areasSearch;
}

public class Encounter
{
    public string who;
    public int levelMin, levelMax;

    public static Encounter encounter;
}
