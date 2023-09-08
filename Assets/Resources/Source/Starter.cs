using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using static Root;
using static Root.Anchor;

using static Font;
using static Sound;
using static Cursor;
using static Talent;
using static SaveGame;
using static Coloring;
using static Transport;
using static GameSettings;
using static CursorRemote;
using static SiteSpiritHealer;
using static Serialization;
using static SiteInstance;
using static SiteComplex;

public class Starter : MonoBehaviour
{
    void Start()
    {
        random = new System.Random();
        font = new Font("Tahoma Bold");
        desktops = new();
        settings = new GameSettings();
        fallingElements = new List<FallingElement>();
        grid = FindObjectOfType<MapGrid>();
        #if (UNITY_EDITOR)
        prefix = "D:\\Games\\Warcraft Elements\\";
        #endif
        Deserialize(ref saves, "characters", false, prefix);
        if (saves == null) saves = new();
        Deserialize(ref settings, "settings", false, prefix);
        if (settings == null) settings = new();
        else settings.FillNulls();
        if (!saves.Any(x => x.Value.Exists(y => y.player.name == settings.selectedCharacter)))
        {
            settings.selectedCharacter = "";
            settings.selectedRealm = "";
        }
        LoadData();
        cursor = FindObjectOfType<Cursor>();
        cursorEnemy = FindObjectOfType<CursorRemote>();
        ambience = FindObjectsOfType<AudioSource>().First(x => x.name == "Ambience");
        soundEffects = cursor.GetComponent<AudioSource>();
        SpawnDesktopBlueprint("TitleScreen");
        Destroy(gameObject);
    }

    public static void LoadData()
    {
        Deserialize(ref SiteHostileArea.areas, "areas", false, prefix);
        SiteHostileArea.areas ??= new();
        Deserialize(ref instances, "instances", false, prefix);
        instances ??= new();
        Deserialize(ref complexes, "complexes", false, prefix);
        complexes ??= new();
        Deserialize(ref SiteTown.towns, "towns", false, prefix);
        SiteTown.towns ??= new();
        Deserialize(ref Realm.realms, "realms", false, prefix);
        Realm.realms ??= new();
        Deserialize(ref PersonType.personTypes, "persontypes", false, prefix);
        PersonType.personTypes ??= new();
        Deserialize(ref Class.specs, "classes", false, prefix);
        Class.specs ??= new();
        Deserialize(ref Race.races, "races", false, prefix);
        Race.races ??= new();
        Deserialize(ref ItemSet.itemSets, "sets", false, prefix);
        ItemSet.itemSets ??= new();
        Deserialize(ref Item.items, "items", false, prefix);
        Item.items ??= new();
        Deserialize(ref Ability.abilities, "abilities", false, prefix);
        Ability.abilities ??= new();
        Deserialize(ref Buff.buffs, "buffs", false, prefix);
        Buff.buffs ??= new();
        Deserialize(ref Faction.factions, "factions", false, prefix);
        Faction.factions ??= new();
        Deserialize(ref spiritHealers, "spirithealers", false, prefix);
        spiritHealers ??= new();
#if (UNITY_EDITOR)
        var ambienceList = AssetDatabase.FindAssets("t:AudioClip Ambience", new[] { "Assets/Resources/Ambience/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Ambience/", "")).ToList();
        var soundList = AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets/Resources/Sounds/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sounds/", "")).ToList();
        var itemIconList = AssetDatabase.FindAssets("t:Texture Item", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var abilityIconList = AssetDatabase.FindAssets("t:Texture Ability", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var factionIconList = AssetDatabase.FindAssets("t:Texture Faction", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var portraitList = AssetDatabase.FindAssets("t:Texture Portrait", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        ambienceList.RemoveAll(x => !x.StartsWith("Ambience"));
        itemIconList.RemoveAll(x => !x.StartsWith("Item"));
        abilityIconList.RemoveAll(x => !x.StartsWith("Ability"));
        factionIconList.RemoveAll(x => !x.StartsWith("Faction"));
        portraitList.RemoveAll(x => !x.StartsWith("Portrait"));
        Assets.assets = new Assets()
        {
            ambience = ambienceList,
            sounds = soundList,
            itemIcons = itemIconList,
            abilityIcons = abilityIconList,
            factionIcons = factionIconList,
            portraits = portraitList
        };
        Serialize(Assets.assets, "assets", false, false, prefix);
        #else
        Deserialize(ref Assets.assets, "assets", false, prefix);
        Assets.assets ??= new()
        {
            ambience = new(),
            sounds = new(),
            itemIcons = new(),
            abilityIcons = new(),
            portraits = new()
        };
        #endif
        var countHA = SiteHostileArea.areas.Count;
        var countI = instances.Count;
        var countR = Race.races.Count;
        var countA = Ability.abilities.Count;
        var countIS = ItemSet.itemSets.Count;
        var countF = Faction.factions.Count;
        for (int i = 0; i < SiteTown.towns.Count; i++)
        {
            var town = SiteTown.towns[i];
            if (town.faction != null)
                if (!Faction.factions.Exists(x => x.name == town.faction))
                    Faction.factions.Insert(0, new Faction()
                    {
                        name = town.faction,
                        icon = "Faction" + town.faction,
                        side = "Neutral"
                    });
        }
        for (int i = 0; i < complexes.Count; i++)
        {
            var complex = complexes[i];
            if (complex.faction != null)
                if (!Faction.factions.Exists(x => x.name == complex.faction))
                    Faction.factions.Insert(0, new Faction()
                    {
                        name = complex.faction,
                        icon = "Faction" + complex.faction,
                        side = "Neutral"
                    });
            if (complex.sites != null)
                foreach (var site in complex.sites)
                    if (site != null && site.ContainsKey("SiteType") && site.ContainsKey("SiteName"))
                        if (site["SiteType"] == "HostileArea")
                        {
                            if (!SiteHostileArea.areas.Exists(x => x.name == site["SiteName"]))
                                SiteHostileArea.areas.Insert(0, new SiteHostileArea()
                                {
                                    name = site["SiteName"],
                                    commonEncounters = new(),
                                    rareEncounters = new(),
                                    eliteEncounters = new(),
                                    type = "HostileArea",
                                    zone = complex.name
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
        }
        for (int i = 0; i < instances.Count; i++)
        {
            var instance = instances[i];
            if (instance.faction != null)
                if (!Faction.factions.Exists(x => x.name == instance.faction))
                    Faction.factions.Insert(0, new Faction()
                    {
                        name = instance.faction,
                        icon = "Faction" + instance.faction,
                        side = "Neutral"
                    });
            if (instance.wings != null)
                foreach (var wing in instance.wings)
                    if (wing.areas != null)
                        foreach (var area in wing.areas)
                            if (area.ContainsKey("AreaName"))
                                if (!SiteHostileArea.areas.Exists(x => x.name == area["AreaName"]))
                                    SiteHostileArea.areas.Insert(0, new SiteHostileArea()
                                    {
                                        name = area["AreaName"],
                                        commonEncounters = new(),
                                        rareEncounters = new(),
                                        eliteEncounters = new(),
                                        type = "HostileArea",
                                        zone = instance.name
                                    });
        }
        for (int i = 0; i < SiteHostileArea.areas.Count; i++)
        {
            var area = SiteHostileArea.areas[i];
            if (area.faction != null)
                if (!Faction.factions.Exists(x => x.name == area.faction))
                    Faction.factions.Insert(0, new Faction()
                    {
                        name = area.faction,
                        icon = "Faction" + area.faction,
                        side = "Neutral"
                    });
            if (area.commonEncounters != null)
                foreach (var encounter in area.commonEncounters)
                    if (!Race.races.Exists(x => x.name == encounter.who))
                        Race.races.Insert(0, new Race()
                        {
                            name = encounter.who,
                            abilities = new(),
                            kind = "Common",
                            portrait = "PortraitChicken",
                            vitality = 1.0,
                        });
            if (area.rareEncounters != null)
                foreach (var encounter in area.rareEncounters)
                    if (!Race.races.Exists(x => x.name == encounter.who))
                        Race.races.Insert(0, new Race()
                        {
                            name = encounter.who,
                            abilities = new(),
                            kind = "Rare",
                            portrait = "PortraitParrot",
                            vitality = 2.0,
                        });
            if (area.eliteEncounters != null)
                foreach (var encounter in area.eliteEncounters)
                    if (!Race.races.Exists(x => x.name == encounter.who))
                        Race.races.Insert(0, new Race()
                        {
                            name = encounter.who,
                            abilities = new(),
                            kind = "Elite",
                            portrait = "PortraitCow",
                            vitality = 3.0,
                        });
        }
        for (int i = 0; i < Class.specs.Count; i++)
        {
            var spec = Class.specs[i];
            if (spec.abilities != null)
                foreach (var ability in spec.abilities)
                    if (!Ability.abilities.Exists(x => x.name == ability.Item1))
                        Ability.abilities.Insert(0, new Ability()
                        {
                            name = ability.Item1,
                            icon = "Ability" + ability.Item1.Replace(" ", ""),
                            events = new(),
                            tags = new()
                        });
            if (spec.talentTrees != null)
                foreach (var tree in spec.talentTrees)
                    foreach (var talent in tree.talents)
                        if (!Ability.abilities.Exists(x => x.name == talent.ability))
                            Ability.abilities.Insert(0, new Ability()
                            {
                                name = talent.ability,
                                icon = "Ability" + talent.ability.Replace(" ", ""),
                                events = new(),
                                tags = new()
                            });
        }
        for (int i = 0; i < Item.items.Count; i++)
        {
            var item = Item.items[i];
            if (item.set != null)
                if (!ItemSet.itemSets.Exists(x => x.name == item.set))
                    ItemSet.itemSets.Insert(0, new ItemSet()
                    {
                        name = item.set,
                        setBonuses = new()
                    });
            if (item.abilities != null)
                foreach (var ability in item.abilities)
                    if (!Ability.abilities.Exists(x => x.name == ability))
                        Ability.abilities.Insert(0, new Ability()
                        {
                            name = ability,
                            icon = "Ability" + ability,
                            events = new(),
                            tags = new()
                        });
        }
        for (int i = 0; i < Race.races.Count; i++)
        {
            var race = Race.races[i];
            if (race.abilities != null)
                foreach (var ability in race.abilities)
                    if (!Ability.abilities.Exists(x => x.name == ability))
                        Ability.abilities.Insert(0, new Ability()
                        {
                            name = ability,
                            icon = "Ability" + ability.Replace(" ", ""),
                            events = new(),
                            tags = new()
                        });
            if (race.faction != null && race.background == null || race.background == "")
                race.background = "AreaElwynnForestNorthshireAbbey";
        }
        for (int i = 0; i < Realm.realms.Count; i++)
        {
            var realm = Realm.realms[i];
            if (!saves.ContainsKey(realm.name))
                saves.Add(realm.name, new());
        }
        for (int i = 0; i < Ability.abilities.Count; i++)
        {
            var ability = Ability.abilities[i];
            if (ability.events == null)
                ability.events = new();
            if (ability.tags == null)
                ability.tags = new();
        }
        SiteHostileArea.areas.ForEach(x => x.Initialise());
        instances.ForEach(x => x.Initialise());
        complexes.ForEach(x => x.Initialise());
        for (int i = 0; i < instances.Count; i++)
        {
            var index = i;
            var instance = instances[index];
            if (Blueprint.windowBlueprints.Exists(x => x.title == instance.type + ": " + instance.name)) continue;
            Blueprint.windowBlueprints.Add(
                new Blueprint(instance.type + ": " + instance.name,
                    () =>
                    {
                        PlayAmbience(instance.ambience);
                        SetAnchor(TopRight);
                        AddRegionGroup();
                        SetRegionGroupWidth(171);
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
            if (Blueprint.windowBlueprints.Exists(x => x.title == "Town: " + town.name)) continue;
            Blueprint.windowBlueprints.Add(
                new Blueprint("Town: " + town.name,
                    () =>
                    {
                        PlayAmbience(town.ambience);
                        SetAnchor(TopRight);
                        AddRegionGroup();
                        SetRegionGroupWidth(171);
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
                                    var town = SiteTown.towns.Find(x => x.name == transport.destination);
                                    CDesktop.cameraDestination = new Vector2(town.x - 17, town.y + 9);
                                },
                                (h) => () => { PrintTransportTooltip(transport); });
                            }
                        }
                        if (town.people != null)
                        {
                            AddHeaderRegion(() => { AddLine("Points of interest:"); });
                            foreach (var person in town.people)
                            {
                                AddButtonRegion(() =>
                                {
                                    AddLine(person.name, "Black");
                                    var personType = PersonType.personTypes.Find(x => x.name == person.type);
                                    AddSmallButton(personType != null ? personType.icon : "OtherUnknown", (h) => { });
                                },
                                (h) =>
                                {
                                    if (person.type == "Banker")
                                        SpawnDesktopBlueprint("BankScreen");
                                });
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
            if (Blueprint.windowBlueprints.Exists(x => x.title == "Complex: " + complex.name)) continue;
            Blueprint.windowBlueprints.Add(
                new Blueprint("Complex: " + complex.name,
                    () =>
                    {
                        PlayAmbience(complex.ambience);
                        SetAnchor(TopRight);
                        AddRegionGroup();
                        SetRegionGroupWidth(171);
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
            if (Blueprint.windowBlueprints.Exists(x => x.title == "HostileArea: " + area.name)) continue;
            Blueprint.windowBlueprints.Add(
                new Blueprint("HostileArea: " + area.name,
                    () =>
                    {
                        PlayAmbience(area.ambience);
                        SetAnchor(TopLeft);
                        AddRegionGroup();
                        SetRegionGroupWidth(171);
                        SetRegionGroupHeight(354);
                        AddHeaderRegion(() =>
                        {
                            AddLine(area.name);
                            AddSmallButton("OtherClose",
                            (h) =>
                            {
                                if (area.instancePart || area.complexPart)
                                {
                                    PlaySound("DesktopInstanceClose");
                                    SetDesktopBackground("Areas/Area" + (area.instancePart ? instance.name : complex.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
                                    CloseWindow(h.window);
                                    Respawn(area.instancePart ? "InstanceLeftSide" : "ComplexLeftSide");
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
                        if (area.eliteEncounters != null && area.eliteEncounters.Count > 0 && area.eliteEncounters.Sum(x => x.requiredProgress) > 0)
                            AddPaddingRegion(() =>
                            {
                                AddLine("Exploration progress: ", "DarkGray");
                                var temp = currentSave.siteProgress;
                                int progress = (int)(currentSave.siteProgress.ContainsKey(area.name) ? (double)currentSave.siteProgress[area.name] / area.eliteEncounters.Sum(x => x.requiredProgress) * 100 : 0);
                                AddText((progress > 100 ? 100 : progress) + "%", ColorProgress(progress));
                            });
                        AddButtonRegion(() => { AddLine("Explore", "Black"); },
                        (h) =>
                        {
                            Board.NewBoard(area.RollEncounter(), area);
                            SpawnDesktopBlueprint("Game");
                            SwitchDesktop("Game");
                        });
                        AddPaddingRegion(() =>
                        {
                            SetRegionAsGroupExtender();
                        });
                        if (area.commonEncounters != null && area.commonEncounters.Count > 0)
                        {
                            if (currentSave.siteProgress.ContainsKey(area.name) && area.eliteEncounters != null && area.eliteEncounters.Count > 0 && area.eliteEncounters.Sum(x => x.requiredProgress) <= currentSave.siteProgress[area.name])
                                foreach (var encounter in area.commonEncounters)
                                    AddButtonRegion(() =>
                                    {
                                        AddLine(encounter.who, "", "Right");
                                        var race = Race.races.Find(x => x.name == encounter.who);
                                        AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                    },
                                    (h) =>
                                    {
                                        Board.NewBoard(area.RollEncounter(encounter), area);
                                        SpawnDesktopBlueprint("Game");
                                        SwitchDesktop("Game");
                                    });
                            else
                                foreach (var encounter in area.commonEncounters)
                                    AddPaddingRegion(() =>
                                    {
                                        AddLine(encounter.who, "DarkGray", "Right");
                                        var race = Race.races.Find(x => x.name == encounter.who);
                                        AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                    });
                        }
                        if (area.eliteEncounters != null && area.eliteEncounters.Count > 0)
                        {
                            //AddHeaderRegion(() =>
                            //{
                            //    AddLine("Bosses: ", "Gray");
                            //});
                            foreach (var boss in area.eliteEncounters)
                            {
                                if (currentSave.siteProgress.ContainsKey(area.name) && boss.requiredProgress <= currentSave.siteProgress[area.name])
                                    AddButtonRegion(() =>
                                    {
                                        SetRegionBackground(RegionBackgroundType.RedButton);
                                        AddLine(boss.who, "", "Right");
                                        var race = Race.races.Find(x => x.name == boss.who);
                                        AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                    },
                                    (h) =>
                                    {
                                        Board.NewBoard(area.RollEncounter(boss), area);
                                        SpawnDesktopBlueprint("Game");
                                        SwitchDesktop("Game");
                                    });
                                else
                                    AddPaddingRegion(() =>
                                    {
                                        AddLine(boss.who, "DangerousRed", "Right");
                                        var race = Race.races.Find(x => x.name == boss.who);
                                        AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                                    });
                            }
                        }
                    }
                )
            );
        }
        for (int i = 0; i < SiteHostileArea.areas.Count; i++)
        {
            var index = i;
            if (SiteHostileArea.areas[index].x != 0)
                Blueprint.windowBlueprints.Add(new Blueprint("Site: " + SiteHostileArea.areas[index].name, () => SiteHostileArea.areas[index].PrintSite()));
        }
        for (int i = 0; i < spiritHealers.Count; i++)
        {
            var index = i;
            if (spiritHealers[index].x != 0)
                Blueprint.windowBlueprints.Add(new Blueprint("Site: SpiritHealer " + spiritHealers[index].name, () => spiritHealers[index].PrintSite()));
        }
        for (int i = 0; i < instances.Count; i++)
        {
            var index = i;
            if (instances[index].x != 0)
                Blueprint.windowBlueprints.Add(new Blueprint("Site: " + instances[index].name, () => instances[index].PrintSite()));
        }
        for (int i = 0; i < complexes.Count; i++)
        {
            var index = i;
            if (complexes[index].x != 0)
                Blueprint.windowBlueprints.Add(new Blueprint("Site: " + complexes[index].name, () => complexes[index].PrintSite()));
        }
        for (int i = 0; i < SiteTown.towns.Count; i++)
        {
            var index = i;
            if (SiteTown.towns[index].x != 0)
                Blueprint.windowBlueprints.Add(new Blueprint("Site: " + SiteTown.towns[index].name, () => SiteTown.towns[index].PrintSite()));
        }
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 12; j++)
                for (int k = 0; k < 3; k++)
                {
                    var spec = i; var row = j; var col = k;
                    if (Blueprint.windowBlueprints.Exists(x => x.title == "Talent" + spec + row + col)) continue;
                    Blueprint.windowBlueprints.Add(new Blueprint("Talent" + spec + row + col, () => PrintTalent(spec, row, col)));
                }
        if (countHA != SiteHostileArea.areas.Count) Debug.Log("Added " + (SiteHostileArea.areas.Count - countHA) + " lacking areas.");
        if (countI != instances.Count) Debug.Log("Added " + (instances.Count - countI) + " lacking instances.");
        if (countR != Race.races.Count) Debug.Log("Added " + (Race.races.Count - countR) + " lacking races.");
        if (countA != Ability.abilities.Count) Debug.Log("Added " + (Ability.abilities.Count - countA) + " lacking abilities.");
        if (countIS != ItemSet.itemSets.Count) Debug.Log("Added " + (ItemSet.itemSets.Count - countIS) + " lacking item sets.");
        if (countF != Faction.factions.Count) Debug.Log("Added " + (Faction.factions.Count - countF) + " lacking factions.");
    }
}
