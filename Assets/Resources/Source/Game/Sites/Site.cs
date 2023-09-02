using UnityEngine;

using static Root;
using static Root.Anchor;

using static Sound;
using static Coloring;
using static SiteTown;
using static SiteHostileArea;
using static SiteInstance;
using static SiteComplex;

using System;
using System.Linq;
using System.Collections.Generic;

public static class Site
{
    public static void PrintSite(string name, string type, Vector2 anchor)
    {
        SetAnchor(anchor.x, anchor.y);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            if (type == "Town")
            {
                var find = towns.Find(x => x.name == name);
                if (find == null)
                {
                    Debug.LogError("ERROR 001: No town named \"" + name + "\" has been found.");
                    return;
                }
                AddSmallButton("Faction" + find.faction,
                (h) =>
                {
                    town = find;
                    PlaySound("DesktopInstanceOpen");
                    SpawnDesktopBlueprint("TownEntrance");
                    SwitchDesktop("TownEntrance");
                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine(name, "Gray");
                    });
                });
            }
            else if (type == "HostileArea")
            {
                var find = areas.Find(x => x.name == name);
                AddSmallButton(find == null ? "OtherUnknown" : "Site" + find.type,
                (h) =>
                {
                    area = areas.Find(x => x.name == name);
                    if (area == null) return;
                    PlaySound("DesktopInstanceOpen");
                    SpawnDesktopBlueprint("HostileAreaEntrance");
                    SwitchDesktop("HostileAreaEntrance");
                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine(name, "Gray");
                    });
                    if (find != null)
                    {
                        AddHeaderRegion(() =>
                        {
                            AddLine("Recommended level: ", "Gray");
                            AddText(find.recommendedLevel + "", ColorEntityLevel(find.recommendedLevel));
                        });
                        if (find.commonEncounters != null && find.commonEncounters.Count > 0)
                            AddPaddingRegion(() =>
                            {
                                AddLine("Common: ", "Gray");
                                foreach (var enemy in find.commonEncounters)
                                {
                                    var race = Race.races.Find(x => x.name == enemy.who);
                                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                }
                            });
                        if (find.eliteEncounters != null && find.eliteEncounters.Count > 0)
                            AddPaddingRegion(() =>
                            {
                                AddLine("Elite: ", "Gray");
                                foreach (var enemy in find.eliteEncounters)
                                {
                                    var race = Race.races.Find(x => x.name == enemy.who);
                                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                }
                            });
                        if (find.rareEncounters != null && find.rareEncounters.Count > 0)
                            AddPaddingRegion(() =>
                            {
                                AddLine("Rare: ", "Gray");
                                foreach (var enemy in find.rareEncounters)
                                    AddSmallButton("OtherUnknown", (h) => { });
                            });
                    }
                });
            }
            else if (type == "Dungeon" || type == "Raid")
                AddSmallButton("Site" + type,
                (h) =>
                {
                    instance = instances.Find(x => x.name == name);
                    if (instance != null)
                    {
                        PlaySound("DesktopInstanceOpen");
                        SpawnDesktopBlueprint("InstanceEntrance");
                        SwitchDesktop("InstanceEntrance");
                    }
                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    SetRegionGroupWidth(152);
                    AddHeaderRegion(() => { AddLine(name, "Gray"); });
                    var instance = instances.Find(x => x.name == name);
                    AddPaddingRegion(() =>
                    {
                        AddLine("Level range: ", "Gray");
                        if (instance == null) AddText("??", "DarkGray");
                        else
                        {
                            var range = instance.LevelRange();
                            AddText(range.Item1 + "", ColorEntityLevel(range.Item1));
                            AddText(" - ", "Gray");
                            AddText(range.Item2 + "", ColorEntityLevel(range.Item2));
                        }
                    });
                    var areas = instance.wings.SelectMany(x => x.areas.Select(y => SiteHostileArea.areas.Find(z => z.name == y["AreaName"])));
                    var total = areas.SelectMany(x => x.commonEncounters ?? new()).Distinct().ToList();
                    total.AddRange(areas.SelectMany(x => x.eliteEncounters ?? new()).Distinct().ToList());
                    total.AddRange(areas.SelectMany(x => x.rareEncounters ?? new()).Distinct().ToList());
                    var races = total.Select(x => Race.races.Find(y => y.name == x.who).portrait).Distinct().ToList();
                    if (races.Count > 0)
                        for (int i = 0; i < Math.Ceiling(races.Count / 8.0); i++)
                        {
                            var ind = i;
                            AddPaddingRegion(() =>
                            {
                                for (int j = 0; j < 8 && j < races.Count - ind * 8; j++)
                                {
                                    var jnd = j;
                                    AddSmallButton(races[jnd + ind * 8], (h) => { });
                                }
                            });
                        }
                });
            else if (type == "Complex")
                AddSmallButton("Site" + type,
                (h) =>
                {
                    complex = complexes.Find(x => x.name == name);
                    if (complex != null)
                    {
                        PlaySound("DesktopInstanceOpen");
                        SpawnDesktopBlueprint("ComplexEntrance");
                        SwitchDesktop("ComplexEntrance");
                    }
                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() => { AddLine(name, "Gray"); });
                    AddPaddingRegion(() => { AddLine("Contains sites:", "DarkGray"); });
                    complex = complexes.Find(x => x.name == name);
                    foreach (var site in complex.sites)
                        AddHeaderRegion(() =>
                        {
                            AddLine(site["SiteName"], "DarkGray");
                            AddSmallButton("Site" + site["SiteType"], (h) => { });
                        });
                });
        });
    }
}
