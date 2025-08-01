using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Quest;
using static Faction;
using static SiteArea;
using static SaveGame;
using static Coloring;
using static SiteInstance;
using static GameSettings;

public class SiteComplex : Site
{
    //List of all sites that this complex contains
    //Keys provide information what type of site it is
    //Values provide information what is the name of the site
    //EXAMPLE: { "SiteType": "Raid", "SiteName": "Molten Core" }
    public List<Dictionary<string, string>> sites;

    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
        type ??= "Complex";
        if (faction != null)
            if (!factions.Exists(x => x.name == faction))
                factions.Insert(0, new Faction()
                {
                    name = faction,
                    icon = "Faction" + faction,
                    side = "Neutral"
                });
        if (sites != null)
            foreach (var site in sites)
                if (site != null && site.ContainsKey("SiteType") && site.ContainsKey("SiteName"))
                    if (site["SiteType"] == "HostileArea")
                    {
                        if (!areas.Exists(x => x.name == site["SiteName"]))
                            areas.Insert(0, new SiteArea()
                            {
                                name = site["SiteName"],
                                commonEncounters = new(),
                                rareEncounters = new(),
                                eliteEncounters = new(),
                                type = "HostileArea",
                                zone = name
                            });
                    }
                    else if (site["SiteType"] == "Dungeon")
                    {
                        if (!instances.Exists(x => x.name == site["SiteName"]))
                            instances.Insert(0, new SiteInstance()
                            {
                                name = site["SiteName"],
                                wings = new(),
                                type = "Dungeon"
                            });
                    }
                    else if (site["SiteType"] == "Raid")
                    {
                        if (!instances.Exists(x => x.name == site["SiteName"]))
                            instances.Insert(0, new SiteInstance()
                            {
                                name = site["SiteName"],
                                wings = new(),
                                type = "Raid"
                            });
                    }
        instances.FindAll(x => sites.Exists(y => (y["SiteType"] == "Raid" || y["SiteType"] == "Dungeon") && y["SiteName"] == x.name)).ForEach(x => x.complexPart = true);
        areas.FindAll(x => sites.Exists(y => y["SiteType"] == "HostileArea" && y["SiteName"] == x.name)).ForEach(x => x.complexPart = true);
        SitePath.pathsConnectedToSite.Remove(name);
        if (x != 0 && y != 0)
            Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite()));
    }

    //Currently opened complex
    public static SiteComplex complex;

    //EXTERNAL FILE: List containing all complexes in-game
    public static List<SiteComplex> complexes;

    //List of all filtered complexes by input search
    public static List<SiteComplex> complexesSearch;

    //Returns path to a texture that is the background visual of this site
    public override string Background()
    {
        var save = currentSave ?? saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter);
        return "Areas/Complex" + name.Clean() + (save != null && save.IsNight() && !noNightVariant ? "Night" : "");
    }

    //Function to print the site onto the map
    public override void PrintSite()
    {
        SetAnchor(x, y);
        DisableGeneralSprites();
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton(currentSave.siteVisits.ContainsKey(name) ? (type == "Capital" ? "MapFaction" + factions.Find(x => x.name == faction).side : "MapComplex") : "MapUnknown",
            (h) =>
            {
                CDesktop.cameraDestination = new Vector2(x, y);
            },
            (h) =>
            {
                if (sitePathBuilder != null) return;
                if (h == null) LeadPath();
                else ExecutePath("Complex");
            },
            //!currentSave.siteVisits.ContainsKey(name) ? null :
            (h) => () =>
            {
                if (!currentSave.siteVisits.ContainsKey(name)) return;
                SetAnchor(TopLeft, 19, -38);
                AddRegionGroup();
                SetRegionGroupWidth(171);
                AddHeaderRegion(() => { AddLine(name, faction == null ? "" : ColorReputation(currentSave.player.Reputation(faction))); });
                complex = this;
                var range = (99, 0);
                var areas1 = complex.sites.Where(x => x["SiteType"] == "HostileArea").Select(x => areas.Find(y => y.name == x["SiteName"]).recommendedLevel).Where(x => x[currentSave.playerSide] > 0).ToList();
                if (areas1.Count > 0)
                {
                    var min = areas1.Min(x => x[currentSave.playerSide]);
                    var max = areas1.Max(x => x[currentSave.playerSide]);
                    if (range.Item1 > min) range = (min, range.Item2);
                    if (range.Item2 < max) range = (range.Item1, max);
                }
                var ranges = complex.sites.Where(x => x["SiteType"] == "Dungeon" || x["SiteType"] == "Raid").Select(x => instances.Find(y => y.name == x["SiteName"]).LevelRange()).ToList();
                if (ranges.Count > 0)
                {
                    var min = ranges.Min(x => x.Item1);
                    var max = ranges.Max(x => x.Item2);
                    if (range.Item1 > min) range = (min, range.Item2);
                    if (range.Item2 < max) range = (range.Item1, max);
                }
                if (range.Item2 > 0)
                    AddPaddingRegion(() =>
                    {
                        AddLine("Level range: ", "DarkGray");
                        AddText(range.Item1 + "", ColorEntityLevel(currentSave.player, range.Item1));
                        AddText(" - ", "DarkGray");
                        AddText(range.Item2 + "", ColorEntityLevel(currentSave.player, range.Item2));
                    });
                var areas2 = complex.sites.Where(x => x["SiteType"] != "Dungeon" && x["SiteType"] != "Raid").Select(x => areas.Find(y => y.name == x["SiteName"])).ToList();
                var instances2 = complex.sites.Where(x => x["SiteType"] == "Dungeon" || x["SiteType"] == "Raid").Select(x => instances.Find(y => y.name == x["SiteName"])).ToList();
                foreach (var instance in instances2)
                    areas2.AddRange(instance.wings.SelectMany(x => x.areas).Select(x => areas.Find(y => y.name == x["AreaName"])));
                var common = areas2.SelectMany(x => x.CommonEncounters(currentSave.playerSide)).Select(x => Race.races.Find(y => y.name == x.who)).ToList();
                var portraits = common.Where(x => x != null).Select(x => x.portrait).Distinct().ToList();
                var currentRow = 0;
                var currentAmount = 0;
                while (portraits != null && portraits.Count > 0)
                {
                    if (currentAmount == 0 && currentRow == 0)
                        AddPaddingRegion(() => AddLine("Hostiles:", "HalfGray"));
                    if (currentAmount == 0)
                        AddPaddingRegion(() => ReverseButtons());
                    AddSmallButton(portraits[0]);
                    currentAmount++;
                    if (currentRow == 0 && currentAmount == 9 || currentRow > 0 && currentAmount == 9)
                    {
                        currentRow++;
                        currentAmount = 0;
                    }
                    portraits.RemoveAt(0);
                }
                var people = areas2.SelectMany(x => x.people ?? new()).ToList();
                if (people != null && people.Count > 0)
                {
                    var legit = people.Where(x => !x.hidden && PersonType.personTypes.Exists(y => y.type == x.type)).OrderBy(x => x.category.priority).ThenBy(x => x.type).ToList();
                    var types = legit.Select(x => PersonType.personTypes.Find(y => y.type == x.type)).Where(x => x != null).ToList();
                    var icons = types.Distinct().Select(x => x.icon + (x.factionVariant ? factions.Find(x => x.name == faction)?.side : "")).ToList();
                    currentRow = 0;
                    currentAmount = 0;
                    while (icons.Count > 0)
                    {
                        if (currentAmount == 0 && currentRow == 0)
                            AddPaddingRegion(() => AddLine("NPC's:", "HalfGray"));
                        if (currentAmount == 0)
                            AddPaddingRegion(() => ReverseButtons());
                        AddSmallButton(icons[0]);
                        currentAmount++;
                        if (currentRow == 0 && currentAmount == 9 || currentRow > 0 && currentAmount == 9)
                        {
                            currentRow++;
                            currentAmount = 0;
                        }
                        icons.RemoveAt(0);
                    }
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

    public static void PrintComplexSite(Dictionary<string, string> site)
    {
        if (showAreasUnconditional || !site.ContainsKey("OpenByDefault") || site.ContainsKey("OpenByDefault") && site["OpenByDefault"] == "True" || currentSave.unlockedAreas.Contains(site["SiteName"]))
            AddButtonRegion(() =>
            {
                AddLine(site["SiteName"]);
                AddSmallButton(site.ContainsKey("SiteIcon") ? site["SiteIcon"] : "Site" + site["SiteType"]);
            },
            (h) =>
            {
                if (site["SiteType"] == "HostileArea")
                {
                    area = areas.Find(x => x.name == site["SiteName"]);
                    if (currentSave.player.QuestsAt(area).Count == 0)
                        CloseWindow("AreaQuestTracker");
                    else Respawn("AreaQuestTracker", true);
                    Respawn("Area");
                    Respawn("AreaQuestAvailable");
                    Respawn("AreaQuestDone");
                    Respawn("AreaProgress");
                    Respawn("AreaElites");
                    Respawn("Chest");
                    CloseWindow("Person");
                    CloseWindow("Persons");
                    SetDesktopBackground(area.Background());
                }
                else
                {
                    CloseDesktop("Complex");
                    instance = instances.Find(x => x.name == site["SiteName"]);
                    if (staticPagination.ContainsKey("Instance"))
                        staticPagination.Remove("Instance");
                    SpawnDesktopBlueprint("Instance");
                }
            });
        else AddHeaderRegion(() => AddLine("?", "DimGray"));
    }
}