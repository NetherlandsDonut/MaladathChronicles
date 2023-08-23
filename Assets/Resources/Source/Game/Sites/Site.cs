using UnityEngine;

using static Root;
using static Root.Anchor;

using static Sound;
using static Coloring;
using static SiteTown;
using static SiteHostileArea;
using static SiteInstance;
using static SiteComplex;

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
                        AddPaddingRegion(() =>
                        {
                            AddLine("Possible encounters:", "DarkGray");
                            foreach (var encounter in find.possibleEncounters)
                                AddLine("- " + encounter.who, "DarkGray");
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
                    AddHeaderRegion(() => { AddLine(name, "Gray"); });
                    AddPaddingRegion(() =>
                    {
                        AddLine("Level range: ", "Gray");
                        instance = instances.Find(x => x.name == name);
                        if (instance == null) AddText("??", "DarkGray");
                        else
                        {
                            var range = instance.LevelRange();
                            AddText(range.Item1 + "", ColorEntityLevel(range.Item1));
                            AddText(" - ", "Gray");
                            AddText(range.Item2 + "", ColorEntityLevel(range.Item2));
                        }
                    });
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
