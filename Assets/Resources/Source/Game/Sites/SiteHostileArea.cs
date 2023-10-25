using System;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Anchor;

using static Race;
using static Sound;
using static Faction;
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
        if (!Blueprint.windowBlueprints.Exists(x => x.title == "HostileArea: " + name))
            Blueprint.windowBlueprints.Add(
                new Blueprint("HostileArea: " + name,
                    () =>
                    {
                        if (ambience == null)
                        {
                            var zone = Zone.zones.Find(x => x.name == this.zone);
                            if (zone != null) PlayAmbience(zone.ambience);
                        }
                        else PlayAmbience(ambience);
                        SetAnchor(TopLeft);
                        AddRegionGroup();
                        SetRegionGroupWidth(171);
                        SetRegionGroupHeight(338);
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
                                    Respawn("InstanceLeftSide");
                                }
                                else if (complexPart)
                                {
                                    SetDesktopBackground(complex.Background());
                                    CloseWindow(h.window);
                                    Respawn("ComplexLeftSide");
                                }
                                else CloseDesktop("HostileArea");
                            });
                        });
                        AddPaddingRegion(() =>
                        {
                            AddLine("Recommended level: ");
                            AddText(recommendedLevel + "", ColorEntityLevel(recommendedLevel));
                        });
                        if (eliteEncounters != null && eliteEncounters.Count > 0 && eliteEncounters.Sum(x => x.requiredProgress) > 0)
                            AddPaddingRegion(() =>
                            {
                                AddLine("Exploration progress: ", "DarkGray");
                                var temp = currentSave.siteProgress;
                                int progress = (int)(currentSave.siteProgress.ContainsKey(name) ? (double)currentSave.siteProgress[name] / eliteEncounters.Sum(x => x.requiredProgress) * 100 : 0);
                                AddText((progress > 100 ? 100 : progress) + "%", ColorProgress(progress));
                            });
                        AddButtonRegion(() => { AddLine("Explore", "Black"); },
                        (h) =>
                        {
                            Board.NewBoard(RollEncounter(), this);
                            SpawnDesktopBlueprint("Game");
                            SwitchDesktop("Game");
                        });
                        AddPaddingRegion(() =>
                        {
                            SetRegionAsGroupExtender();
                        });
                        if (commonEncounters != null && commonEncounters.Count > 0)
                        {
                            if (currentSave.siteProgress.ContainsKey(name) && eliteEncounters != null && eliteEncounters.Count > 0 && eliteEncounters.Sum(x => x.requiredProgress) <= currentSave.siteProgress[name])
                                foreach (var encounter in commonEncounters)
                                    AddButtonRegion(() =>
                                    {
                                        AddLine(encounter.who, "", "Right");
                                        var race = races.Find(x => x.name == encounter.who);
                                        AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                    },
                                    (h) =>
                                    {
                                        Board.NewBoard(RollEncounter(encounter), this);
                                        SpawnDesktopBlueprint("Game");
                                        SwitchDesktop("Game");
                                    });
                            else
                                foreach (var encounter in commonEncounters)
                                    AddPaddingRegion(() =>
                                    {
                                        AddLine(encounter.who, "DarkGray", "Right");
                                        var race = races.Find(x => x.name == encounter.who);
                                        AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                    });
                        }
                        if (eliteEncounters != null && eliteEncounters.Count > 0)
                        {
                            foreach (var boss in eliteEncounters)
                            {
                                if (currentSave.siteProgress.ContainsKey(name) && boss.requiredProgress <= currentSave.siteProgress[name])
                                    AddButtonRegion(() =>
                                    {
                                        SetRegionBackground(RegionBackgroundType.RedButton);
                                        AddLine(boss.who, "", "Right");
                                        var race = races.Find(x => x.name == boss.who);
                                        AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                    },
                                    (h) =>
                                    {
                                        Board.NewBoard(RollEncounter(boss), this);
                                        SpawnDesktopBlueprint("Game");
                                        SwitchDesktop("Game");
                                    });
                                else
                                    AddPaddingRegion(() =>
                                    {
                                        AddLine(boss.who, "DangerousRed", "Right");
                                        var race = races.Find(x => x.name == boss.who);
                                        AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                    });
                            }
                        }
                    }
                )
            );
        LoadConnections();
        Blueprint.windowBlueprints.RemoveAll(x => x.title == "Site: " + name);
        if (x != 0 && y != 0)
            Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite()));
    }

    //Function to print the site onto the map
    public override void PrintSite()
    {
        SetAnchor(x * 19, y * 19);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("Site" + type,
            (h) =>
            {
                QueueSiteOpen("HostileArea");
            },
            (h) =>
            {
                if (siteConnect == null) siteConnect = this;
                else
                {
                    DrawPath(siteConnect);
                    if (connections == null)
                        connections = new();
                    connections.Add(siteConnect.name);
                    if (connectionsLoaded == null)
                        connectionsLoaded = new();
                    connectionsLoaded.Add(siteConnect);
                    if (siteConnect.connections == null)
                        siteConnect.connections = new();
                    siteConnect.connections.Add(name);
                    if (siteConnect.connectionsLoaded == null)
                        siteConnect.connectionsLoaded = new();
                    siteConnect.connectionsLoaded.Add(this);
                    siteConnect = null;
                }
            },
            (h) => () =>
            {
                SetAnchor(TopRight, h.window);
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
            });
        });
    }

    //Returns path to a texture that is the background visual of this site
    public override string Background() => "Areas/Area" + (zone + name).Clean() + (specialClearBackground && eliteEncounters.All(x => currentSave.elitesKilled.ContainsKey(x.who)) ? "Cleared" : "");

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
    public int levelMin, levelMax, requiredProgress;

    public static Encounter encounter;
}
