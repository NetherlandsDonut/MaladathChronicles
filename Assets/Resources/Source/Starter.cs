using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Color;

public class Starter : MonoBehaviour
{
    void Start()
    {
        random = new System.Random();
        font = new Font("Tahoma Bold");
        desktops = new();
        String.consoleInput.Set(">");
        cursor = FindObjectOfType<Cursor>();
        cursorEnemy = FindObjectOfType<CursorRemote>();
        settings = new GameSettings();
        Serialization.Deserialize(ref Item.items, "items");
        saveGames = new List<SaveGame>();
        var temp = FindObjectsByType<WindowAnchorRemote>(FindObjectsSortMode.None);
        windowRemoteAnchors = temp.Select(x => (x.name, new Vector2(x.transform.position.x, x.transform.position.y))).ToList();
        for (int i = temp.Length - 1; i >= 0; i--)
            Destroy(temp[i].gameObject);
        SpawnDesktopBlueprint("TitleScreen");
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
                        SetAnchor(Anchor.TopRight);
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
                        SetAnchor(Anchor.TopRight);
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
                        SetAnchor(Anchor.TopRight);
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
        for (int i = 0; i < SiteHostileArea.hostileAreas.Count; i++)
            if (SiteHostileArea.hostileAreas[i].instancePart)
            {
                var index = i;
                var area = SiteHostileArea.hostileAreas[index];
                Blueprint.windowBlueprints.Add(
                    new Blueprint("Area: " + area.name,
                        () =>
                        {
                            SetAnchor(Anchor.TopLeft);
                            AddRegionGroup();
                            SetRegionGroupWidth(161);
                            SetRegionGroupHeight(344);
                            AddHeaderRegion(() =>
                            {
                                AddLine(area.name);
                                AddSmallButton("OtherClose",
                                (h) =>
                                {
                                    SpawnTransition();
                                    SetDesktopBackground("Areas/Area" + SiteInstance.instance.name.Replace("'", "").Replace(".", "").Replace(" ", ""));
                                    CloseWindow(h.window);
                                });
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine("Recommended level: ", Gray);
                                AddText(area.recommendedLevel + "", EntityColoredLevel(area.recommendedLevel));
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine("Exploration progress: ", DarkGray);
                                var temp = currentSave.siteProgress;
                                int progress = (int)(currentSave.siteProgress.ContainsKey(area.name) ? (double)currentSave.siteProgress[area.name] / area.bossEncounters.Sum(x => x.Item1) * 100 : 0);
                                AddText((progress > 100 ? 100 : progress) + "%", ProgressColored(progress));
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine("Possible encounters:", DarkGray);
                                foreach (var encounter in area.possibleEncounters)
                                    AddLine("- " + encounter.Item3, DarkGray);
                            });
                            AddButtonRegion(() =>
                            {
                                AddLine("Explore", Black);
                            },
                            (h) =>
                            {
                                Board.board = new Board(6, 6, area.RollEncounter(), area);
                                SpawnDesktopBlueprint("Game");
                                SwitchDesktop("Game");
                            });
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
                                    AddLine(boss.Item3, Black);
                                },
                                (h) =>
                                {
                                    Board.board = new Board(6, 6, area.RollBoss(boss), area);
                                    SpawnDesktopBlueprint("Game");
                                    SwitchDesktop("Game");
                                });
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
            else
            {
                var index = i;
                var area = SiteHostileArea.hostileAreas[index];
                Blueprint.windowBlueprints.Add(
                    new Blueprint("Area: " + area.name,
                        () =>
                        {
                            SetAnchor(Anchor.TopLeft);
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
                            AddPaddingRegion(() =>
                            {
                                AddLine("Possible encounters:", DarkGray);
                                foreach (var encounter in area.possibleEncounters)
                                    AddLine("- " + encounter.Item3, DarkGray);
                            });
                            AddButtonRegion(() =>
                            {
                                AddLine("Explore", Black);
                            },
                            (h) =>
                            {
                                Board.board = new Board(6, 6, area.RollEncounter(), area);
                                SpawnDesktopBlueprint("Game");
                                SwitchDesktop("Game");
                            });
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
        //Serialize(Data.data, "Data");
        Destroy(gameObject);
    }
}
