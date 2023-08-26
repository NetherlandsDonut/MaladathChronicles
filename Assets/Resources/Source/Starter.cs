using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using static Root;
using static Root.Anchor;

using static Site;
using static Font;
using static Sound;
using static Cursor;
using static Talent;
using static Coloring;
using static Transport;
using static CursorRemote;
using static Serialization;
using static SiteInstance;
using static SiteComplex;

public class Starter : MonoBehaviour
{
    void Start()
    {
        if (!File.Exists("UnityPlayer.dll"))
            Application.Quit();
        random = new System.Random();
        font = new Font("Tahoma Bold");
        desktops = new();
        settings = new GameSettings();
        saveGames = new List<SaveGame>();
        fallingElements = new List<FallingElement>();
        Deserialize(ref SiteHostileArea.areas, "areas");
        Deserialize(ref instances, "instances");
        Deserialize(ref complexes, "complexes");
        Deserialize(ref SiteTown.towns, "towns");
        Deserialize(ref Class.specs, "classes");
        Deserialize(ref Race.races, "races");
        Deserialize(ref ItemSet.itemSets, "sets");
        Deserialize(ref Item.items, "items");
        Deserialize(ref Ability.abilities, "abilities");
        Deserialize(ref Buff.buffs, "buffs");
        SiteHostileArea.areas.ForEach(x => x.Initialise());
        instances.ForEach(x => x.Initialise());
        complexes.ForEach(x => x.Initialise());
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
        for (int i = 0; i < instances.Count; i++)
        {
            var index = i;
            var instance = instances[index];
            Blueprint.windowBlueprints.Add(
                new Blueprint(instance.type + ": " + instance.name,
                    () =>
                    {
                        PlayAmbience(instance.ambience);
                        SetAnchor(TopRight);
                        AddRegionGroup();
                        SetRegionGroupWidth(161);
                        SetRegionGroupHeight(354);
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
                            AddLine("Level range: ", "Gray");
                            var range = instance.LevelRange();
                            AddText(range.Item1 + "", ColorEntityLevel(range.Item1));
                            AddText(" - ", "Gray");
                            AddText(range.Item2 + "", ColorEntityLevel(range.Item2));
                        });
                        foreach (var wing in instance.wings)
                            PrintInstanceWing(instance, wing);
                        AddPaddingRegion(() => { });
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
                        PlayAmbience(town.ambience);
                        SetAnchor(TopRight);
                        AddRegionGroup();
                        SetRegionGroupWidth(161);
                        SetRegionGroupHeight(354);
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
                            AddHeaderRegion(() => { AddLine("Transportation:"); });
                            foreach (var transport in town.transport)
                            {
                                AddButtonRegion(() =>
                                {
                                    AddLine(transport.destination, "Black");
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
                                (h) => () => { PrintTransportTooltip(transport); });
                            }
                        }
                        AddPaddingRegion(() => { });
                    }
                )
            );
        }
        for (int i = 0; i < complexes.Count; i++)
        {
            var index = i;
            var complex = complexes[index];
            Blueprint.windowBlueprints.Add(
                new Blueprint("Complex: " + complex.name,
                    () =>
                    {
                        PlayAmbience(complex.ambience);
                        SetAnchor(TopRight);
                        AddRegionGroup();
                        SetRegionGroupWidth(161);
                        SetRegionGroupHeight(354);
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
                        AddPaddingRegion(() => { AddLine("Sites: "); });
                        foreach (var site in complex.sites)
                            PrintComplexSite(site);
                        AddPaddingRegion(() => { });
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
                        PlayAmbience(area.ambience);
                        SetAnchor(TopLeft);
                        AddRegionGroup();
                        SetRegionGroupWidth(161);
                        SetRegionGroupHeight(354);
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
                                    SetDesktopBackground("Areas/Area" + (area.instancePart ? instance.name : complex.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
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
                            AddLine("Recommended level: ");
                            AddText(area.recommendedLevel + "", ColorEntityLevel(area.recommendedLevel));
                        });
                        if (area.bossEncounters != null && area.bossEncounters.Count > 0 && area.bossEncounters.Sum(x => x.requiredProgress) > 0)
                            AddPaddingRegion(() =>
                            {
                                AddLine("Exploration progress: ", "DarkGray");
                                var temp = currentSave.siteProgress;
                                int progress = (int)(currentSave.siteProgress.ContainsKey(area.name) ? (double)currentSave.siteProgress[area.name] / area.bossEncounters.Sum(x => x.requiredProgress) * 100 : 0);
                                AddText((progress > 100 ? 100 : progress) + "%", ColorProgress(progress));
                            });
                        AddPaddingRegion(() =>
                        {
                            SetRegionAsGroupExtender();
                        });
                        if (area.possibleEncounters != null && area.possibleEncounters.Count > 0)
                        {
                            AddHeaderRegion(() => { AddLine("Possible encounters:", "DarkGray"); });
                            foreach (var encounter in area.possibleEncounters)
                                AddPaddingRegion(() =>
                                {
                                    AddLine(encounter.who, "DarkGray", "Right");
                                    var race = Race.races.Find(x => x.name == encounter.who);
                                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                });
                            AddButtonRegion(() => { AddLine("Explore", "Black"); },
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
                                AddLine("Bosses: ", "Gray");
                                //AddSmallButton("OtherBoss", (h) => { });
                            });
                            foreach (var boss in area.bossEncounters)
                            {
                                AddButtonRegion(() =>
                                {
                                    SetRegionBackground(RegionBackgroundType.RedButton);
                                    AddLine(boss.who, "", "Right");
                                    var race = Race.races.Find(x => x.name == boss.who);
                                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                },
                                (h) =>
                                {
                                    Board.NewBoard(area.RollBoss(boss), area);
                                    SpawnDesktopBlueprint("Game");
                                    SwitchDesktop("Game");
                                });
                            }
                        }
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
        #if (UNITY_EDITOR)
        var ambienceList = AssetDatabase.FindAssets("t:AudioClip Ambience", new[] { "Assets/Resources/Ambience/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Ambience/", "")).ToList();
        var soundList = AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets/Resources/Sounds/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sounds", "")).ToList();
        var itemIconList = AssetDatabase.FindAssets("t:Texture Item", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var abilityIconList = AssetDatabase.FindAssets("t:Texture Ability", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var portraitList = AssetDatabase.FindAssets("t:Texture Portrait", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        ambienceList.RemoveAll(x => !x.StartsWith("Ambience"));
        itemIconList.RemoveAll(x => !x.StartsWith("Item"));
        abilityIconList.RemoveAll(x => !x.StartsWith("Ability"));
        portraitList.RemoveAll(x => !x.StartsWith("Portrait"));
        Assets.assets = new Assets(ambienceList, soundList, itemIconList, abilityIconList, portraitList);
        Serialize(Assets.assets, "assets");
        #else
        Deserialize(ref Assets.assets, "assets");
        #endif
        cursor = FindObjectOfType<Cursor>();
        cursorEnemy = FindObjectOfType<CursorRemote>();
        ambience = FindObjectsOfType<AudioSource>().First(x => x.name == "Ambience");
        SpawnDesktopBlueprint("TitleScreen");
        Destroy(gameObject);
    }
}
