using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Color;
using static Root.Anchor;
using static Serialization;

public class Starter : MonoBehaviour
{
    void Start()
    {
        random = new System.Random();
        font = new Font("Tahoma Bold");
        desktops = new();
        settings = new GameSettings();
        saveGames = new List<SaveGame>();
        fallingElements = new List<FallingElement>();
        Deserialize(ref SiteHostileArea.areas, "areas");
        Deserialize(ref SiteInstance.instances, "instances");
        Deserialize(ref SiteComplex.complexes, "complexes");
        Deserialize(ref SiteTown.towns, "towns");
        Deserialize(ref Class.classes, "classes");
        Deserialize(ref Race.races, "races");
        Deserialize(ref ItemSet.itemSets, "sets");
        Deserialize(ref Item.items, "items");
        Deserialize(ref Ability.abilities, "abilities");
        Deserialize(ref Buff.buffs, "buffs");
        SiteHostileArea.areas.ForEach(x => x.Initialise());
        SiteInstance.instances.ForEach(x => x.Initialise());
        SiteComplex.complexes.ForEach(x => x.Initialise());
        var temp = FindObjectsByType<WindowAnchorRemote>(FindObjectsSortMode.None);
        windowRemoteAnchors = temp.Select(x => (x.name, new Vector2(x.transform.position.x, x.transform.position.y))).ToList();
        for (int i = temp.Length - 1; i >= 0; i--) Destroy(temp[i].gameObject);
        for (int i = 0; i < windowRemoteAnchors.Count; i++)
        {
            var index = i;
            if (!windowRemoteAnchors[index].Item1.Contains(": ")) continue;
            var split = windowRemoteAnchors[index].Item1.Split(": ");
            var name = split[1];
            var type = split[0].Substring(4);
            Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite(name, type, windowRemoteAnchors[index].Item2)));
        }
        for (int i = 0; i < SiteInstance.instances.Count; i++)
        {
            var index = i;
            var instance = SiteInstance.instances[index];
            Blueprint.windowBlueprints.Add(
                new Blueprint(instance.type + ": " + instance.name,
                    () =>
                    {
                        SetAnchor(TopRight);
                        AddRegionGroup();
                        SetRegionGroupWidth(161);
                        SetRegionGroupHeight(344);
                        AddHeaderRegion(() =>
                        {
                            AddLine(instance.name);
                            AddSmallButton("OtherClose",
                            (h) =>
                            {
                                var title = CDesktop.title;
                                CloseDesktop(title);
                                if (instance.complexPart)
                                    SpawnDesktopBlueprint("ComplexEntrance");
                                else
                                {
                                    PlaySound("DesktopInstanceClose");
                                    SwitchDesktop("Map");
                                }
                            });
                        });
                        AddPaddingRegion(() =>
                        {
                            SetRegionAsGroupExtender();
                            AddLine("Level range: ", Gray);
                            var range = instance.LevelRange();
                            AddText(range.Item1 + "", EntityColoredLevel(range.Item1));
                            AddText(" - ", Gray);
                            AddText(range.Item2 + "", EntityColoredLevel(range.Item2));
                        });
                        foreach (var wing in instance.wings)
                            PrintRaidWing(instance, wing);
                        AddPaddingRegion(() =>
                        {
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                        });
                    }
                )
            );
        }
        for (int i = 0; i < SiteTown.towns.Count; i++)
        {
            var index = i;
            var town = SiteTown.towns[index];
            Blueprint.windowBlueprints.Add(
                new Blueprint("Town: " + town.name,
                    () =>
                    {
                        SetAnchor(TopRight);
                        AddRegionGroup();
                        SetRegionGroupWidth(161);
                        SetRegionGroupHeight(344);
                        AddHeaderRegion(() =>
                        {
                            AddLine(town.name);
                            AddSmallButton("OtherClose",
                            (h) =>
                            {
                                var title = CDesktop.title;
                                CloseDesktop(title);
                                PlaySound("DesktopInstanceClose");
                                SwitchDesktop("Map");
                            });
                        });
                        if (town.transport != null)
                        {
                            AddHeaderRegion(() =>
                            {
                                AddLine("Transportation:", Gray);
                            });
                            foreach (var transport in town.transport)
                            {
                                AddButtonRegion(() =>
                                {
                                    AddLine(transport.destination, Black);
                                    AddSmallButton("Transport" + transport.means, (h) => { });
                                },
                                (h) =>
                                {
                                    CloseDesktop("TownEntrance");
                                    SwitchDesktop("Map");
                                    CDesktop.LockScreen();
                                    if (transport.price > 0)
                                        PlaySound("DesktopTransportPay");
                                    fastTravelCamera = CDesktop.windows.Find(x => x.title == "Site: " + transport.destination).gameObject;
                                },
                                (h) => () =>
                                {
                                    SetAnchor(Center);
                                    PrintTransportTooltip(transport);
                                });
                            }
                        }
                        AddPaddingRegion(() =>
                        {
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                        });
                    }
                )
            );
        }
        for (int i = 0; i < SiteComplex.complexes.Count; i++)
        {
            var index = i;
            var complex = SiteComplex.complexes[index];
            Blueprint.windowBlueprints.Add(
                new Blueprint("Complex: " + complex.name,
                    () =>
                    {
                        SetAnchor(TopRight);
                        AddRegionGroup();
                        SetRegionGroupWidth(161);
                        SetRegionGroupHeight(344);
                        AddHeaderRegion(() =>
                        {
                            AddLine(complex.name);
                            AddSmallButton("OtherClose",
                            (h) =>
                            {
                                var title = CDesktop.title;
                                PlaySound("DesktopInstanceClose");
                                CloseDesktop(title);
                                SwitchDesktop("Map");
                            });
                        });
                        AddPaddingRegion(() =>
                        {
                            AddLine("Sites: ", Gray);
                        });
                        foreach (var site in complex.sites)
                            PrintComplexSite(complex, site);
                        AddPaddingRegion(() =>
                        {
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                        });
                    }
                )
            );
        }
        for (int i = 0; i < SiteHostileArea.areas.Count; i++)
        {
            var index = i;
            var area = SiteHostileArea.areas[index];
            Blueprint.windowBlueprints.Add(
                new Blueprint("HostileArea: " + area.name,
                    () =>
                    {
                        SetAnchor(TopLeft);
                        AddRegionGroup();
                        SetRegionGroupWidth(161);
                        SetRegionGroupHeight(344);
                        AddHeaderRegion(() =>
                        {
                            AddLine(area.name);
                            AddSmallButton("OtherClose",
                            (h) =>
                            {
                                if (area.instancePart || area.complexPart)
                                {
                                    SpawnTransition();
                                    PlaySound("DesktopInstanceClose");
                                    SetDesktopBackground("Areas/Area" + (area.instancePart ? SiteInstance.instance.name : SiteComplex.complex.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
                                    CloseWindow(h.window);
                                }
                                else
                                {
                                    PlaySound("DesktopInstanceClose");
                                    CloseDesktop("HostileAreaEntrance");
                                }
                            });
                        });
                        AddPaddingRegion(() =>
                        {
                            AddLine("Recommended level: ", Gray);
                            AddText(area.recommendedLevel + "", EntityColoredLevel(area.recommendedLevel));
                        });
                        if (area.bossEncounters != null && area.bossEncounters.Count > 0 && area.bossEncounters.Sum(x => x.requiredProgress) > 0)
                            AddPaddingRegion(() =>
                            {
                                AddLine("Exploration progress: ", DarkGray);
                                var temp = currentSave.siteProgress;
                                int progress = (int)(currentSave.siteProgress.ContainsKey(area.name) ? (double)currentSave.siteProgress[area.name] / area.bossEncounters.Sum(x => x.requiredProgress) * 100 : 0);
                                AddText((progress > 100 ? 100 : progress) + "%", ProgressColored(progress));
                            });
                        if (area.possibleEncounters != null && area.possibleEncounters.Count > 0)
                        {
                            AddPaddingRegion(() =>
                            {
                                AddLine("Possible encounters:", DarkGray);
                                foreach (var encounter in area.possibleEncounters)
                                    AddLine("- " + encounter.who, DarkGray);
                            });
                            AddButtonRegion(() =>
                            {
                                AddLine("Explore", Black);
                            },
                            (h) =>
                            {
                                Board.NewBoard(area.RollEncounter(), area);
                                SpawnDesktopBlueprint("Game");
                                SwitchDesktop("Game");
                            });
                        }
                        if (area.bossEncounters != null && area.bossEncounters.Count > 0)
                        {
                            AddHeaderRegion(() =>
                            {
                                AddLine("Bosses: ", Gray);
                                AddSmallButton("OtherBoss", (h) => { });
                            });
                            foreach (var boss in area.bossEncounters)
                            {
                                AddButtonRegion(() =>
                                {
                                    SetRegionBackground(RegionBackgroundType.RedButton);
                                    AddLine(boss.who, Black);
                                },
                                (h) =>
                                {
                                    Board.NewBoard(area.RollBoss(boss), area);
                                    SpawnDesktopBlueprint("Game");
                                    SwitchDesktop("Game");
                                });
                            }
                        }
                        AddPaddingRegion(() =>
                        {
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                            AddLine("", Gray);
                        });
                    }
                )
            );
        }
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 12; j++)
                for (int k = 0; k < 3; k++)
                {
                    var spec = i; var row = j; var col = k;
                    Blueprint.windowBlueprints.Add(new Blueprint("Talent" + spec + row + col, () => PrintTalent(spec, row, col)));
                }
        //Serialize(Race.races, "races 2");
        //Serialize(Class.classes, "classes 2");
        //Serialize(Ability.abilities, "abilities 2");
        //Serialize(Buff.buffs, "buffs 2");
        //Serialize(Race.races, "races 2");
        //Serialize(SiteTown.towns, "towns 2");
        //Serialize(SiteInstance.instances, "instances 2");
        //Serialize(SiteComplex.complexes, "complexes 2");
        //Serialize(ItemSet.itemSets, "sets 2");
        cursor = FindObjectOfType<Cursor>();
        cursorEnemy = FindObjectOfType<CursorRemote>();
        SpawnDesktopBlueprint("TitleScreen");
        Destroy(gameObject);
    }
}
