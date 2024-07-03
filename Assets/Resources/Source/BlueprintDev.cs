using System.Linq;
using System.Collections.Generic;

using static UnityEngine.KeyCode;

using static Root;
using static Root.Anchor;
using static Root.RegionBackgroundType;

using static Item;
using static Buff;
using static Race;
using static Zone;
using static Spec;
using static Sound;
using static Event;
using static Quest;
using static Mount;
using static Board;
using static Recipe;
using static Defines;
using static Faction;
using static ItemSet;
using static Ability;
using static Enchant;
using static PVPRank;
using static SitePath;
using static SaveGame;
using static Blueprint;
using static PersonType;
using static Profession;
using static GeneralDrop;
using static WorldAbility;
using static Serialization;
using static PersonCategory;
using static PermanentEnchant;
using static SiteSpiritHealer;
using static SiteHostileArea;
using static SiteInstance;
using static SiteComplex;
using static SiteTown;

public static class BlueprintDev
{
    public static List<Blueprint> windowBlueprints = new()
    {
        new("ObjectManagerLobby", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddHeaderRegion(() => { AddLine("Object types:"); AddSmallButton("OtherClose", (h) => { CloseDesktop("DevPanel"); }); });
            AddButtonRegion(() => { AddLine("Hostile areas"); }, (h) =>
            {
                areasSearch = areas;
                SpawnDesktopBlueprint("ObjectManagerHostileAreas");
            });
            AddButtonRegion(() => { AddLine("Instances"); }, (h) =>
            {
                instancesSearch = instances;
                SpawnDesktopBlueprint("ObjectManagerInstances");
            });
            AddButtonRegion(() => { AddLine("Complexes"); }, (h) =>
            {
                complexesSearch = complexes;
                SpawnDesktopBlueprint("ObjectManagerComplexes");
            });
            AddButtonRegion(() => { AddLine("Towns"); }, (h) =>
            {
                townsSearch = towns;
                SpawnDesktopBlueprint("ObjectManagerTowns");
            });
            AddButtonRegion(() => { AddLine("Races"); }, (h) => { SpawnDesktopBlueprint("ObjectManagerRaces"); });
            AddButtonRegion(() => { AddLine("Specs"); }, (h) =>
            {
                specsSearch = specs;
                SpawnDesktopBlueprint("ObjectManagerSpecs");
            });
            AddButtonRegion(() => { AddLine("Abilities"); }, (h) =>
            {
                abilitiesSearch = abilities;
                SpawnDesktopBlueprint("ObjectManagerAbilities");
            });
            AddButtonRegion(() => { AddLine("Buffs"); }, (h) =>
            {
                buffsSearch = buffs;
                SpawnDesktopBlueprint("ObjectManagerBuffs");
            });
            AddButtonRegion(() => { AddLine("Items"); }, (h) =>
            {
                itemsSearch = items;
                SpawnDesktopBlueprint("ObjectManagerItems");
            });
            AddButtonRegion(() => { AddLine("Item sets"); }, (h) =>
            {
                itemSetsSearch = itemSets;
                SpawnDesktopBlueprint("ObjectManagerItemSets");
            });
            AddButtonRegion(() => { AddLine("Mounts"); }, (h) =>
            {
                mountsSearch = mounts;
                SpawnDesktopBlueprint("ObjectManagerMounts");
            });
            AddButtonRegion(() => { AddLine("Recipes"); }, (h) =>
            {
                recipesSearch = recipes;
                SpawnDesktopBlueprint("ObjectManagerRecipes");
            });
            AddButtonRegion(() => { AddLine("Cloth types"); }, (h) =>
            {
                generalDropsSearch = generalDrops;
                SpawnDesktopBlueprint("ObjectManagerClothTypes");
            });
            AddButtonRegion(() => { AddLine("Factions"); }, (h) =>
            {
                factionsSearch = factions;
                SpawnDesktopBlueprint("ObjectManagerFactions");
            });
            AddPaddingRegion(() => { AddLine("Actions:"); });
            AddButtonRegion(() => { AddLine("Save data"); }, (h) =>
            {
                Serialize(races, "races", false, false, prefix);
                Serialize(specs, "Specs", false, false, prefix);
                Serialize(abilities, "abilities", false, false, prefix);
                Serialize(worldAbilities, "worldabilities", false, false, prefix);
                Serialize(buffs, "buffs", false, false, prefix);
                Serialize(areas, "areas", false, false, prefix);
                Serialize(instances, "instances", false, false, prefix);
                Serialize(complexes, "complexes", false, false, prefix);
                Serialize(towns, "towns", false, false, prefix);
                Serialize(items, "items", false, false, prefix);
                Serialize(itemSets, "sets", false, false, prefix);
                Serialize(mounts, "mounts", false, false, prefix);
                Serialize(generalDrops, "generaldrops", false, false, prefix);
                Serialize(recipes, "recipes", false, false, prefix);
                Serialize(professions, "professions", false, false, prefix);
                Serialize(factions, "factions", false, false, prefix);
                Serialize(personTypes, "persontypes", false, false, prefix);
                Serialize(personCategories, "personcategories", false, false, prefix);
                Serialize(spiritHealers, "spirithealers", false, false, prefix);
                Serialize(pEnchants, "permanentenchants", false, false, prefix);
                Serialize(enchants, "enchants", false, false, prefix);
                Serialize(pvpRanks, "pvpranks", false, false, prefix);
                Serialize(zones, "zones", false, false, prefix);
                Serialize(quests, "quests", false, false, prefix);
                Serialize(paths, "paths", false, false, prefix);
                Serialize(FishingSpot.fishingSpots, "fishingspots", false, false, prefix);
                Serialize(defines, "defines", false, false, prefix);
            });
            AddPaddingRegion(() => { });
        }),
        new("HostileAreasSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort hostile areas:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("HostileAreasSort");
                    Respawn("ObjectManagerHostileAreas");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                areas = areas.OrderBy(x => x.name).ToList();
                areasSearch = areasSearch.OrderBy(x => x.name).ToList();
                CloseWindow("HostileAreasSort");
                Respawn("ObjectManagerHostileAreas");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By zone", "Black");
            },
            (h) =>
            {
                areas = areas.OrderBy(x => x.zone).ToList();
                areasSearch = areasSearch.OrderBy(x => x.zone).ToList();
                CloseWindow("HostileAreasSort");
                Respawn("ObjectManagerHostileAreas");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By type", "Black");
            },
            (h) =>
            {
                areas = areas.OrderBy(x => x.type).ToList();
                areasSearch = areasSearch.OrderBy(x => x.type).ToList();
                CloseWindow("HostileAreasSort");
                Respawn("ObjectManagerHostileAreas");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By encounter amount", "Black");
            },
            (h) =>
            {
                areas = areas.OrderByDescending(x => x.commonEncounters == null ? -1 : x.commonEncounters.Count).ToList();
                areasSearch = areasSearch.OrderByDescending(x => x.commonEncounters == null ? -1 : x.commonEncounters.Count).ToList();
                CloseWindow("HostileAreasSort");
                Respawn("ObjectManagerHostileAreas");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By boss amount", "Black");
            },
            (h) =>
            {
                areas = areas.OrderByDescending(x => x.eliteEncounters == null ? -1 : x.eliteEncounters.Count).ToList();
                areasSearch = areasSearch.OrderByDescending(x => x.eliteEncounters == null ? -1 : x.eliteEncounters.Count).ToList();
                CloseWindow("HostileAreasSort");
                Respawn("ObjectManagerHostileAreas");
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("ObjectManagerHostileAreas", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => areasSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (area != null)
            {
                var index = areas.IndexOf(area);
                if (index >= 10)
                    CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Areas:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerHostileAreas"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    areas.Reverse();
                    areasSearch.Reverse();
                    Respawn("ObjectManagerHostileAreas");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "HostileAreasSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("HostileAreasSort");
                        Respawn("ObjectManagerHostileAreas");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (areasSearch.Count > index + 10 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = areasSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name);
                        AddSmallButton("Site" + foo.type);
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    area = areasSearch[index + 10 * regionGroup.pagination()];
                    instance = null;
                    complex = null;
                    SetDesktopBackground(area.Background());
                    CloseWindow("ObjectManagerHostileAreaCommonEncounters");
                    CloseWindow("ObjectManagerHostileAreaRareEncounters");
                    CloseWindow("ObjectManagerHostileAreaEliteEncounters");
                    Respawn("ObjectManagerHostileArea");
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(areas.Count + " hostile areas", "DarkGray");
                if (areas.Count != areasSearch.Count)
                    AddLine(areasSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerHostileArea", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Hostile area:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(area.name); });
            AddPaddingRegion(() => { AddLine("Zone:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(area.zone); });
            AddPaddingRegion(() => { AddLine("Type:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(area.type);
                AddSmallButton("Site" + area.type);
            },
            (h) =>
            {
                CloseWindow("ObjectManagerAmbienceList");
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerHostileAreaTypeList"))
                {
                    CloseWindow("ObjectManagerHostileAreas");
                    SpawnWindowBlueprint("ObjectManagerHostileAreaTypeList");
                }
            });
            AddPaddingRegion(() => { AddLine("Ambience:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(area.ambience == null ? "None" : area.ambience.Substring(8) + ".ogg");
                if (area.ambience != "None")
                    AddSmallButton("OtherSound", (h) =>
                    {
                        PlayAmbience(area.ambience);
                    });
            },
            (h) =>
            {
                CloseWindow("ObjectManagerHostileAreaTypeList");
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerAmbienceList"))
                {
                    CloseWindow("ObjectManagerHostileAreas");
                    Assets.assets.ambienceSearch = Assets.assets.ambience;
                    SpawnWindowBlueprint("ObjectManagerAmbienceList");
                }
            });
            AddPaddingRegion(() => { AddLine("Encounters:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine("Common encounters");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerHostileAreaRareEncounters");
                CloseWindow("ObjectManagerHostileAreaEliteEncounters");
                Respawn("ObjectManagerHostileAreaCommonEncounters");
            });
            AddButtonRegion(() =>
            {
                AddLine("Rare encounters");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerHostileAreaCommonEncounters");
                CloseWindow("ObjectManagerHostileAreaEliteEncounters");
                Respawn("ObjectManagerHostileAreaRareEncounters");
            });
            AddButtonRegion(() =>
            {
                AddLine("Elite encounters");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerHostileAreaCommonEncounters");
                CloseWindow("ObjectManagerHostileAreaRareEncounters");
                Respawn("ObjectManagerHostileAreaEliteEncounters");
            });
            AddPaddingRegion(() => { });
        }),
        new("ObjectManagerTowns", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => towns.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (town != null)
            {
                var index = towns.IndexOf(town);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Towns:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerTowns"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    towns.Reverse();
                    townsSearch.Reverse();
                    Respawn("ObjectManagerTowns");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "HostileAreasSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("HostileAreasSort");
                        Respawn("ObjectManagerHostileAreas");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (townsSearch.Count > index + 10 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = townsSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name);
                        AddSmallButton(factions.Find(x => x.name == foo.faction).Icon());
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    town = townsSearch[index + 10 * regionGroup.pagination()];
                    SetDesktopBackground(town.Background());
                    Respawn("ObjectManagerTown");
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(towns.Count + " towns", "DarkGray");
                if (towns.Count != townsSearch.Count)
                    AddLine(townsSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerTown", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Town:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(town.name); });
            AddPaddingRegion(() => { AddLine("Zone:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(town.zone); });
            AddPaddingRegion(() => { AddLine("Faction:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(town.faction);
                AddSmallButton(factions.Find(x => x.name == town.faction).Icon());
            },
            (h) =>
            {
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerFactions"))
                {
                    factionsSearch = factions;
                    CloseWindow("ObjectManagerTowns");
                    SpawnWindowBlueprint("ObjectManagerFactions");
                }
            });
            AddPaddingRegion(() => { AddLine("Ambience:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(town.ambience == null ? "None" : town.ambience.Substring(8) + ".ogg");
                if (town.ambience != "None")
                    AddSmallButton("OtherSound", (h) =>
                    {
                        PlayAmbience(town.ambience);
                    });
            },
            (h) =>
            {
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerAmbienceList"))
                {
                    CloseWindow("ObjectManagerTowns");
                    Assets.assets.ambienceSearch = Assets.assets.ambience;
                    SpawnWindowBlueprint("ObjectManagerAmbienceList");
                }
            });
            AddPaddingRegion(() => { });
        }),
        new("InstancesSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort instances:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("InstancesSort");
                    Respawn("ObjectManagerInstances");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                instances = instances.OrderBy(x => x.name).ToList();
                instancesSearch = instancesSearch.OrderBy(x => x.name).ToList();
                CloseWindow("InstancesSort");
                Respawn("ObjectManagerInstances");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By zone", "Black");
            },
            (h) =>
            {
                instances = instances.OrderBy(x => x.zone).ToList();
                instancesSearch = instancesSearch.OrderBy(x => x.zone).ToList();
                CloseWindow("InstancesSort");
                Respawn("ObjectManagerInstances");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By type", "Black");
            },
            (h) =>
            {
                instances = instances.OrderBy(x => x.type).ToList();
                instancesSearch = instancesSearch.OrderBy(x => x.type).ToList();
                CloseWindow("InstancesSort");
                Respawn("ObjectManagerInstances");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By area amount", "Black");
            },
            (h) =>
            {
                instances = instances.OrderByDescending(x => x.wings.Sum(y => y.areas.Count)).ToList();
                instancesSearch = instancesSearch.OrderByDescending(x => x.wings.Sum(y => y.areas.Count)).ToList();
                CloseWindow("InstancesSort");
                Respawn("ObjectManagerInstances");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By wing amount", "Black");
            },
            (h) =>
            {
                instances = instances.OrderByDescending(x => x.wings.Count).ToList();
                instancesSearch = instancesSearch.OrderByDescending(x => x.wings.Count).ToList();
                CloseWindow("InstancesSort");
                Respawn("ObjectManagerInstances");
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("ObjectManagerInstances", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => instancesSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (instance != null)
            {
                var index = instances.IndexOf(instance);
                if (index >= 10)
                    CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Instances:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerInstances"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    instances.Reverse();
                    instancesSearch.Reverse();
                    Respawn("ObjectManagerInstances");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "InstancesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("InstancesSort");
                        Respawn("ObjectManagerInstances");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (instancesSearch.Count > index + 10 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = instancesSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name);
                        AddSmallButton("Site" + foo.type);
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    area = null;
                    instance = instancesSearch[index + 10 * regionGroup.pagination()];
                    complex = null;
                    SetDesktopBackground(instance.Background());
                    Respawn("ObjectManagerInstance");
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(instances.Count + " instances", "DarkGray");
                if (instances.Count != instancesSearch.Count)
                    AddLine(instancesSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerInstance", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Instance:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(instance.name); });
            AddPaddingRegion(() => { AddLine("Zone:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(instance.zone); });
            AddPaddingRegion(() => { AddLine("Ambience:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(instance.ambience == null ? "None" : instance.ambience.Substring(8) + ".ogg");
                if (instance.ambience != "None")
                    AddSmallButton("OtherSound", (h) =>
                    {
                        PlayAmbience(instance.ambience);
                    });
            },
            (h) =>
            {
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerAmbienceList"))
                {
                    CloseWindow("ObjectManagerInstances");
                    Assets.assets.ambienceSearch = Assets.assets.ambience;
                    SpawnWindowBlueprint("ObjectManagerAmbienceList");
                }
            });
            AddPaddingRegion(() => { });
        }),
        new("ComplexesSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort complexes:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("ComplexesSort");
                    CloseWindow("ObjectManagerComplexes");
                    SpawnWindowBlueprint("ObjectManagerComplexes");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                complexes = complexes.OrderBy(x => x.name).ToList();
                complexesSearch = complexesSearch.OrderBy(x => x.name).ToList();
                CloseWindow("ComplexesSort");
                CloseWindow("ObjectManagerComplexes");
                SpawnWindowBlueprint("ObjectManagerComplexes");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By zone", "Black");
            },
            (h) =>
            {
                complexes = complexes.OrderBy(x => x.zone).ToList();
                complexesSearch = complexesSearch.OrderBy(x => x.zone).ToList();
                CloseWindow("ComplexesSort");
                CloseWindow("ObjectManagerComplexes");
                SpawnWindowBlueprint("ObjectManagerComplexes");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By site amount", "Black");
            },
            (h) =>
            {
                complexes = complexes.OrderByDescending(x => x.sites.Count).ToList();
                complexesSearch = complexesSearch.OrderByDescending(x => x.sites.Count).ToList();
                CloseWindow("ComplexesSort");
                CloseWindow("ObjectManagerComplexes");
                SpawnWindowBlueprint("ObjectManagerComplexes");
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("ObjectManagerComplexes", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => complexesSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (complex != null)
            {
                var index = complexes.IndexOf(complex);
                if (index >= 10)
                    CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Complexes:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerComplexes"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    complexes.Reverse();
                    complexesSearch.Reverse();
                    CloseWindow("ObjectManagerComplexes");
                    SpawnWindowBlueprint("ObjectManagerComplexes");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "ComplexesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("ComplexesSort");
                        CloseWindow("ObjectManagerComplexes");
                        SpawnWindowBlueprint("ObjectManagerComplexes");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < regionGroup.perPage; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (complexesSearch.Count > index + regionGroup.perPage * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = complexesSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name);
                        AddSmallButton("SiteComplex");
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    area = null;
                    instance = null;
                    complex = complexesSearch[index + regionGroup.perPage * regionGroup.pagination()];
                    SetDesktopBackground(complex.Background());
                    CloseWindow("ObjectManagerComplex");
                    SpawnWindowBlueprint("ObjectManagerComplex");
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(complexes.Count + " complexes", "DarkGray");
                if (complexes.Count != complexesSearch.Count)
                    AddLine(complexesSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerComplex", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Complex:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(complex.name); });
            AddPaddingRegion(() => { AddLine("Zone:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(complex.zone); });
            AddPaddingRegion(() => { AddLine("Ambience:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(complex.ambience == null ? "None" : complex.ambience.Substring(8) + ".ogg");
                if (complex.ambience != "None")
                    AddSmallButton("OtherSound", (h) =>
                    {
                        PlayAmbience(complex.ambience);
                    });
            },
            (h) =>
            {
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerAmbienceList"))
                {
                    CloseWindow("ObjectManagerComplexes");
                    Assets.assets.ambienceSearch = Assets.assets.ambience;
                    SpawnWindowBlueprint("ObjectManagerAmbienceList");
                }
            });
            AddPaddingRegion(() => { });
        }),
        new("ObjectManagerAmbienceList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => Assets.assets.ambienceSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (area != null)
            {
                var index = Assets.assets.ambienceSearch.IndexOf(area.ambience + ".ogg");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            else if (instance != null)
            {
                var index = Assets.assets.ambienceSearch.IndexOf(instance.ambience + ".ogg");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            else if (complex != null)
            {
                var index = Assets.assets.ambienceSearch.IndexOf(complex.ambience + ".ogg");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Ambience tracks:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    if (area != null)
                        SpawnWindowBlueprint("ObjectManagerHostileAreas");
                    else if (instance != null)
                        SpawnWindowBlueprint("ObjectManagerInstances");
                    else if (complex != null)
                        SpawnWindowBlueprint("ObjectManagerComplexes");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    Assets.assets.ambienceSearch.Reverse();
                    Respawn("ObjectManagerAmbienceList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < regionGroup.perPage; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.ambienceSearch.Count > index + regionGroup.perPage * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.ambienceSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.Substring(8));
                        AddSmallButton("OtherSound", (h) =>
                        {
                            PlayAmbience(foo.Replace(".ogg", ""));
                        });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.ambience[index + 10 * regionGroup.pagination()];
                    CloseWindow("ObjectManagerAmbienceList");
                    if (area != null)
                    {
                        area.ambience = foo.Replace(".ogg", "");
                        Respawn("ObjectManagerHostileArea");
                        SpawnWindowBlueprint("ObjectManagerHostileAreas");
                    }
                    else if (instance != null)
                    {
                        instance.ambience = foo.Replace(".ogg", "");
                        Respawn("ObjectManagerInstance");
                        SpawnWindowBlueprint("ObjectManagerInstances");
                    }
                    else if (complex != null)
                    {
                        complex.ambience = foo.Replace(".ogg", "");
                        Respawn("ObjectManagerComplex");
                        SpawnWindowBlueprint("ObjectManagerComplexes");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(Assets.assets.ambience.Count + " ambience tracks", "DarkGray");
                if (Assets.assets.ambience.Count != Assets.assets.ambienceSearch.Count)
                    AddLine(Assets.assets.ambienceSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerSoundsList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => Assets.assets.soundsSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (eventEdit != null)
            {
                var temp = eventEdit.effects[selectedEffect];
                var index = temp.ContainsKey("SoundEffect") && temp["SoundEffect"] != "None" ? Assets.assets.soundsSearch.IndexOf(temp["SoundEffect"] + ".ogg") : 0;
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Sound effects:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    if (eventEdit != null)
                        SpawnWindowBlueprint("ObjectManagerEventEffects");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    Assets.assets.soundsSearch.Reverse();
                    Respawn("ObjectManagerSoundsList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < regionGroup.perPage; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.soundsSearch.Count > index + regionGroup.perPage * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.soundsSearch[index + regionGroup.perPage * regionGroup.pagination()];
                        AddLine(foo);
                        AddSmallButton("OtherSound", (h) =>
                        {
                            PlaySound(foo.Replace(".ogg", ""));
                        });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.soundsSearch[index + regionGroup.perPage * regionGroup.pagination()];
                    CloseWindow("ObjectManagerSoundsList");
                    if (eventEdit != null)
                    {
                        var temp = eventEdit.effects[selectedEffect];
                        if (temp.ContainsKey("SoundEffect"))
                            temp["SoundEffect"] = foo.Replace(".ogg", "");
                        else temp.Add("SoundEffect", foo.Replace(".ogg", ""));
                        Respawn("ObjectManagerEventEffect");
                        Respawn("ObjectManagerEventEffects");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(Assets.assets.sounds.Count + " sound effects", "DarkGray");
                if (Assets.assets.sounds.Count != Assets.assets.soundsSearch.Count)
                    AddLine(Assets.assets.soundsSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerItemIconList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => Assets.assets.itemIconsSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (item != null)
            {
                var index = Assets.assets.itemIconsSearch.IndexOf(item.icon + ".png");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Item icons:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    if (item != null) Respawn("ObjectManagerItems");
                    else if (ability != null) Respawn("ObjectManagerAbilities");
                    else if (buff != null) Respawn("ObjectManagerBuffs");
                    else if (spec != null) Respawn("ObjectManagerSpecs");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    Assets.assets.itemIconsSearch.Reverse();
                    Respawn("ObjectManagerItemIconList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < regionGroup.perPage; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.itemIconsSearch.Count > index + regionGroup.perPage * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.itemIconsSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.Substring(4));
                        AddSmallButton(Assets.assets.itemIconsSearch[index + 10 * regionGroup.pagination()].Replace(".png", ""));
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.itemIconsSearch[index + 10 * regionGroup.pagination()];
                    CloseWindow("ObjectManagerItemIconList");
                    if (item != null)
                    {
                        item.icon = foo.Replace(".png", "");
                        Respawn("ObjectManagerItem");
                        Respawn("ObjectManagerItems");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(Assets.assets.itemIcons.Count + " item icons", "DarkGray");
                if (Assets.assets.itemIcons.Count != Assets.assets.itemIconsSearch.Count)
                    AddLine(Assets.assets.itemIconsSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerAbilityIconList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => Assets.assets.abilityIconsSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (ability != null)
            {
                var index = Assets.assets.abilityIconsSearch.IndexOf(ability.icon + ".png");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            else if (buff != null)
            {
                var index = Assets.assets.abilityIconsSearch.IndexOf(buff.icon + ".png");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Ability icons:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    if (ability != null) Respawn("ObjectManagerAbilities");
                    else if (buff != null) Respawn("ObjectManagerBuffs");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    Assets.assets.abilityIcons.Reverse();
                    Respawn("ObjectManagerAbilityIconList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < regionGroup.perPage; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.abilityIconsSearch.Count > index + regionGroup.perPage * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.abilityIconsSearch[index + regionGroup.perPage * regionGroup.pagination()];
                        AddLine(foo.Substring(7));
                        AddSmallButton(Assets.assets.abilityIconsSearch[index + regionGroup.perPage * regionGroup.pagination()].Replace(".png", ""));
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.abilityIconsSearch[index + regionGroup.perPage * regionGroup.pagination()];
                    CloseWindow("ObjectManagerAbilityIconList");
                    if (ability != null)
                    {
                        ability.icon = foo.Replace(".png", "");
                        Respawn("ObjectManagerAbility");
                        Respawn("ObjectManagerAbilities");
                    }
                    else if (buff != null)
                    {
                        buff.icon = foo.Replace(".png", "");
                        Respawn("ObjectManagerBuff");
                        Respawn("ObjectManagerBuffs");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(Assets.assets.abilityIcons.Count + " ability icons", "DarkGray");
                if (Assets.assets.abilityIcons.Count != Assets.assets.abilityIconsSearch.Count)
                    AddLine(Assets.assets.abilityIconsSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerFactionIconList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => Assets.assets.factionIconsSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (faction != null)
            {
                var index = Assets.assets.factionIconsSearch.IndexOf(faction.icon + ".png");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Faction icons:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    if (faction != null) Respawn("ObjectManagerFactions");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    Assets.assets.factionIcons.Reverse();
                    Respawn("ObjectManagerFactionIconList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < regionGroup.perPage; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.factionIconsSearch.Count > index + regionGroup.perPage * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.factionIconsSearch[index + regionGroup.perPage * regionGroup.pagination()];
                        AddLine(foo.Substring(7));
                        AddSmallButton(Assets.assets.factionIconsSearch[index + regionGroup.perPage * regionGroup.pagination()].Replace(".png", ""));
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.factionIconsSearch[index + regionGroup.perPage * regionGroup.pagination()];
                    CloseWindow("ObjectManagerFactionIconList");
                    if (faction != null)
                    {
                        faction.icon = foo.Replace(".png", "");
                        Respawn("ObjectManagerFaction");
                        Respawn("ObjectManagerFactions");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(Assets.assets.factionIcons.Count + " faction icons", "DarkGray");
                if (Assets.assets.factionIcons.Count != Assets.assets.factionIconsSearch.Count)
                    AddLine(Assets.assets.factionIconsSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerMountIconList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => Assets.assets.mountIconsSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (mount != null)
            {
                var index = Assets.assets.mountIconsSearch.IndexOf(mount.icon + ".png");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Mount icons:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    if (mount != null) Respawn("ObjectManagerMounts");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    Assets.assets.mountIcons.Reverse();
                    Respawn("ObjectManagerMountIconList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < regionGroup.perPage; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.mountIconsSearch.Count > index + regionGroup.perPage * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.mountIconsSearch[index + regionGroup.perPage * regionGroup.pagination()];
                        AddLine(foo.Substring(5));
                        AddSmallButton(Assets.assets.mountIconsSearch[index + regionGroup.perPage * regionGroup.pagination()].Replace(".png", ""));
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.mountIconsSearch[index + regionGroup.perPage * regionGroup.pagination()];
                    CloseWindow("ObjectManagerMountIconList");
                    if (mount != null)
                    {
                        mount.icon = foo.Replace(".png", "");
                        Respawn("ObjectManagerMount");
                        Respawn("ObjectManagerMounts");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(Assets.assets.mountIcons.Count + " mount icons", "DarkGray");
                if (Assets.assets.mountIcons.Count != Assets.assets.mountIconsSearch.Count)
                    AddLine(Assets.assets.mountIconsSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerPortraitList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => Assets.assets.portraitsSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (race != null)
            {
                var index = Assets.assets.portraitsSearch.IndexOf(race.portrait + ".png");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Portraits:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    SpawnWindowBlueprint("ObjectManagerRaces");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    Assets.assets.portraitsSearch.Reverse();
                    Respawn("ObjectManagerPortraitList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < regionGroup.perPage; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.portraitsSearch.Count > index + regionGroup.perPage * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.portraitsSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.Replace("Portrait", ""));
                        AddSmallButton(Assets.assets.portraitsSearch[index + 10 * regionGroup.pagination()].Replace(".png", ""));
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.portraitsSearch[index + regionGroup.perPage * regionGroup.pagination()];
                    CloseWindow("ObjectManagerPortraitList");
                    if (race != null)
                    {
                        race.portrait = foo.Replace(".png", "");
                        Respawn("ObjectManagerRace");
                        SpawnWindowBlueprint("ObjectManagerRaces");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(Assets.assets.portraits.Count + " portraits", "DarkGray");
                if (Assets.assets.portraits.Count != Assets.assets.portraitsSearch.Count)
                    AddLine(Assets.assets.portraitsSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerEffectList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => possibleEffects.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Effects:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    SpawnWindowBlueprint("ObjectManagerEventEffects");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    possibleEffects.Reverse();
                    Respawn("ObjectManagerEffectList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < regionGroup.perPage; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (possibleEffects.Count > index + regionGroup.perPage * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = possibleEffects[index + 10 * regionGroup.pagination()];
                        AddLine(foo);
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = possibleEffects[index + 10 * regionGroup.pagination()];
                    CloseWindow("ObjectManagerEffectList");
                    if (eventEdit.effects[selectedEffect].ContainsKey("Effect"))
                        eventEdit.effects[selectedEffect]["Effect"] = foo;
                    else eventEdit.effects[selectedEffect].Add("Effect", foo);
                    SpawnWindowBlueprint("ObjectManagerEventEffects");
                    Respawn("ObjectManagerEventEffect");
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(possibleEffects.Count + " different effects", "DarkGray");
            });
        }),
        new("ObjectManagerTriggerList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => possibleTriggers.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Triggers:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    SpawnWindowBlueprint("ObjectManagerEventTriggers");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    possibleEffects.Reverse();
                    Respawn("ObjectManagerTriggerList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < regionGroup.perPage; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (possibleTriggers.Count > index + regionGroup.perPage * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = possibleTriggers[index + 10 * regionGroup.pagination()];
                        AddLine(foo);
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = possibleTriggers[index + 10 * regionGroup.pagination()];
                    CloseWindow("ObjectManagerTriggerList");
                    if (eventEdit.triggers[selectedTrigger].ContainsKey("Trigger"))
                        eventEdit.triggers[selectedTrigger]["Trigger"] = foo;
                    else eventEdit.triggers[selectedTrigger].Add("Trigger", foo);
                    SpawnWindowBlueprint("ObjectManagerEventTriggers");
                    Respawn("ObjectManagerEventTrigger");
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(possibleTriggers.Count + " different triggers", "DarkGray");
            });
        }),
        new("ObjectManagerEventTriggers", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddHeaderRegion(() =>
            {
                AddLine("Event:");
                AddSmallButton("OtherClose", (h) =>
                {
                    eventEdit = null;
                    CloseWindow(h.window);
                    CloseWindow("ObjectManagerEventTrigger");
                    if (eventParentType == "Ability") SpawnWindowBlueprint("ObjectManagerAbilities");
                    else SpawnWindowBlueprint("ObjectManagerBuffs");
                });
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaddingRegion(() =>
            {
                AddLine("Triggers: ", "DarkGray");
                AddText(eventEdit.triggers.Count + "", "Gray");
                AddText(" / ", "DarkGray");
                AddText("9", "Gray");
                AddSmallButton("OtherNextPage", (h) =>
                {
                    PlaySound("DesktopChangePage", 0.6f);
                    SpawnWindowBlueprint("ObjectManagerEventEffects");
                    CloseWindow("ObjectManagerEventTrigger");
                    CloseWindow(h.window);
                });
                AddSmallButton("OtherSave", (h) =>
                {
                    PlaySound("DesktopTooltipHide", 0.6f);
                    triggersCopy = eventEdit.triggers.Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList();
                    });
                AddSmallButton("OtherPaste", (h) =>
                {
                    if (effectsCopy != null)
                    {
                        PlaySound("DesktopWeirdClick3", 0.6f);
                        eventEdit.triggers.AddRange(triggersCopy.Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList());
                        }
                });
            });
            foreach (var trigger in eventEdit.triggers)
            {
                AddButtonRegion(() =>
                {
                    var type = trigger.ContainsKey("Trigger") ? trigger["Trigger"] : "";
                    AddLine("Trigger #" + (eventEdit.triggers.IndexOf(trigger) + 1) + (type == "" ? "" : ": " + type));
                },
                (h) =>
                {
                    selectedTrigger = eventEdit.triggers.IndexOf(trigger);
                    String.resourceAmount.Set(trigger.ContainsKey("ResourceAmount") ? trigger["ResourceAmount"] : "1");
                    Respawn("ObjectManagerEventTriggers");
                    Respawn("ObjectManagerEventTrigger");
                });
            }
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            if (eventEdit.triggers.Count < 9)
                AddButtonRegion(() =>
                {
                    AddLine("Add a new trigger");
                },
                (h) =>
                {
                    eventEdit.triggers.Add(new Dictionary<string, string>());
                    });
            else
                AddPaddingRegion(() =>
                {
                    AddLine("Add a new trigger", "DarkGray");
                });
        }),
        new("ObjectManagerEventTrigger", () => {
            var trigger = eventEdit.triggers[selectedTrigger];
            DisableShadows();
            SetAnchor(Top);
            AddHeaderGroup();
            SetRegionGroupWidth(296);
            AddHeaderRegion(() =>
            {
                AddLine("Trigger:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                });
                AddSmallButton("OtherTrash", (h) =>
                {
                    eventEdit.triggers.RemoveAt(selectedTrigger);
                    CloseWindow(h.window);
                    Respawn("ObjectManagerEventTriggers");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine(trigger.ContainsKey("Trigger") ? trigger["Trigger"] : "None");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerEventTriggers");
                Respawn("ObjectManagerTriggerList");
            });
            AddRegionGroup();
            SetRegionGroupWidth(148);
            SetRegionGroupHeight(316);
            AddPaddingRegion(() =>
            {
                AddLine("Triggerer:", "DarkGray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    if (trigger.ContainsKey("Triggerer"))
                        trigger["Triggerer"] = "Any";
                    });
            });
            if (trigger.ContainsKey("Trigger") && trigger["Trigger"] != "CombatBegin")
                AddButtonRegion(() =>
                {
                    AddLine(trigger.ContainsKey("Triggerer") ? trigger["Triggerer"] : "Any");
                },
                (h) =>
                {
                    if (!trigger.ContainsKey("Triggerer"))
                        trigger.Add("Triggerer", "Effector");
                    else if (trigger["Triggerer"] == "Effector")
                        trigger["Triggerer"] = "Other";
                    else if (trigger["Triggerer"] == "Other")
                        trigger["Triggerer"] = "Any";
                    else if (trigger["Triggerer"] == "Any")
                        trigger["Triggerer"] = "Effector";
                    });
            if (trigger.ContainsKey("Trigger") && (trigger["Trigger"] == "AbilityCast" || trigger["Trigger"] == "Cooldown"))
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Ability:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (trigger.ContainsKey("AbilityName"))
                            trigger["AbilityName"] = eventParentType == "Ability" ? "This" : "Any";
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(trigger.ContainsKey("AbilityName") ? trigger["AbilityName"] : (eventParentType == "Ability" ? "This" : "Any"));
                    if (trigger.ContainsKey("AbilityName") && trigger["AbilityName"] != (eventParentType == "Ability" ? "This" : "Any"))
                        AddSmallButton(abilities.Find(x => x.name == trigger["AbilityName"]).icon);
                },
                (h) =>
                {
                    if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerAbilities"))
                    {
                        CloseWindow("ObjectManagerEventTriggers");
                        String.search.Set("");
                        abilitiesSearch = abilities;
                        Respawn("ObjectManagerAbilities");
                    }
                });
            }
            else if (trigger.ContainsKey("Trigger") && (trigger["Trigger"] == "BuffRemove" || trigger["Trigger"] == "BuffAdd" || trigger["Trigger"] == "BuffFlare"))
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Buff:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (trigger.ContainsKey("BuffName"))
                            trigger["BuffName"] = eventParentType == "Buff" ? "This" : "Any";
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(trigger.ContainsKey("BuffName") ? trigger["BuffName"] : (eventParentType == "Buff" ? "This" : "Any"));
                    if (trigger.ContainsKey("BuffName") && trigger["BuffName"] != (eventParentType == "Buff" ? "This" : "Any"))
                        AddSmallButton(buffs.Find(x => x.name == trigger["BuffName"]).icon);
                },
                (h) =>
                {
                    if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerBuffs"))
                    {
                        CloseWindow("ObjectManagerEventTriggers");
                        CloseWindow("ObjectManagerSoundsList");
                        String.search.Set("");
                        buffsSearch = buffs;
                        Respawn("ObjectManagerBuffs");
                    }
                });
            }
            else if (trigger.ContainsKey("Trigger") && (trigger["Trigger"] == "ResourceCollected" || trigger["Trigger"] == "ResourceDetracted" || trigger["Trigger"] == "ResourceMaxed" || trigger["Trigger"] == "ResourceDeplated"))
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Resource type:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (trigger.ContainsKey("ResourceType"))
                            trigger["ResourceType"] = "Any";
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(trigger.ContainsKey("ResourceType") ? trigger["ResourceType"] : "Any");
                    if (trigger.ContainsKey("ResourceType") && trigger["ResourceType"] != "Any")
                        AddSmallButton("Element" + trigger["ResourceType"] + "Rousing");
                },
                (h) =>
                {
                    if (!trigger.ContainsKey("ResourceType"))
                        trigger.Add("ResourceType", "Fire");
                    else if (trigger["ResourceType"] == "Fire")
                        trigger["ResourceType"] = "Earth";
                    else if (trigger["ResourceType"] == "Earth")
                        trigger["ResourceType"] = "Water";
                    else if (trigger["ResourceType"] == "Water")
                        trigger["ResourceType"] = "Air";
                    else if (trigger["ResourceType"] == "Air")
                        trigger["ResourceType"] = "Frost";
                    else if (trigger["ResourceType"] == "Frost")
                        trigger["ResourceType"] = "Decay";
                    else if (trigger["ResourceType"] == "Decay")
                        trigger["ResourceType"] = "Shadow";
                    else if (trigger["ResourceType"] == "Shadow")
                        trigger["ResourceType"] = "Order";
                    else if (trigger["ResourceType"] == "Order")
                        trigger["ResourceType"] = "Arcane";
                    else if (trigger["ResourceType"] == "Arcane")
                        trigger["ResourceType"] = "Lightning";
                    else if (trigger["ResourceType"] == "Lightning")
                        trigger["ResourceType"] = "Any";
                    else if (trigger["ResourceType"] == "Any")
                        trigger["ResourceType"] = "Fire";
                    });
                if (trigger["Trigger"] == "ResourceCollected" || trigger["Trigger"] == "ResourceDetracted")
                    AddPaddingRegion(() =>
                    {
                        AddLine("Resource amount:", "DarkGray");
                        AddInputLine(String.resourceAmount);
                        AddSmallButton("OtherReverse", (h) =>
                        {
                            if (trigger.ContainsKey("ResourceAmount"))
                                trigger["ResourceAmount"] = "1";
                            String.resourceAmount.Set("1");
                            });
                    });
            }
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddRegionGroup();
            SetRegionGroupWidth(148);
            SetRegionGroupHeight(316);
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
        }),
        new("ObjectManagerEventEffects", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddHeaderRegion(() =>
            {
                AddLine("Event:");
                AddSmallButton("OtherClose", (h) =>
                {
                    eventEdit = null;
                    CloseWindow(h.window);
                    CloseWindow("ObjectManagerEventEffect");
                    if (eventParentType == "Ability") SpawnWindowBlueprint("ObjectManagerAbilities");
                    else SpawnWindowBlueprint("ObjectManagerBuffs");
                });
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaddingRegion(() =>
            {
                AddLine("Effects: ", "DarkGray");
                AddText(eventEdit.effects.Count + "", "Gray");
                AddText(" / ", "DarkGray");
                AddText("15", "Gray");
                AddSmallButton("OtherPreviousPage", (h) =>
                {
                    PlaySound("DesktopChangePage", 0.6f);
                    SpawnWindowBlueprint("ObjectManagerEventTriggers");
                    CloseWindow("ObjectManagerEventEffect");
                    CloseWindow(h.window);
                });
                AddSmallButton("OtherSave", (h) =>
                {
                    PlaySound("DesktopTooltipHide", 0.6f);
                    effectsCopy = eventEdit.effects.Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList();
                    });
                AddSmallButton("OtherPaste", (h) =>
                {
                    if (effectsCopy != null)
                    {
                        PlaySound("DesktopWeirdClick3", 0.6f);
                        eventEdit.effects.AddRange(effectsCopy.Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList());
                        }
                });
            });
            foreach (var effect in eventEdit.effects)
            {
                AddButtonRegion(() =>
                {
                    var type = effect.ContainsKey("Effect") ? effect["Effect"] : "";
                    AddLine("Effect #" + (eventEdit.effects.IndexOf(effect) + 1) + (type == "" ? (effect.ContainsKey("SoundEffect") && effect["SoundEffect"] != "None" ? ": SoundEffect" : "") : ": " + type));
                    AddSmallButton("OtherCopy", (h) =>
                    {
                        eventEdit.effects.Insert(eventEdit.effects.IndexOf(effect) + 1, effect.ToDictionary(x => x.Key, x => x.Value));
                        });
                    if (eventEdit.effects[0] != effect)
                        AddSmallButton("OtherMoveUp", (h) =>
                        {
                            var index = eventEdit.effects.IndexOf(effect);
                            eventEdit.effects.RemoveAt(index);
                            eventEdit.effects.Insert(index - 1, effect);
                            });
                },
                (h) =>
                {
                    selectedEffect = eventEdit.effects.IndexOf(effect);
                    String.chance.Set(effect.ContainsKey("Chance") ? effect["Chance"] : "0");
                    String.chanceBase.Set(effect.ContainsKey("ChanceBase") ? effect["ChanceBase"] : "100");
                    String.chanceScale.Set(effect.ContainsKey("ChanceScale") ? effect["ChanceScale"] : "None");
                    String.animationArc.Set(effect.ContainsKey("AnimationArc") ? effect["AnimationArc"] : "20");
                    String.trailStrength.Set(effect.ContainsKey("TrailStrength") ? effect["TrailStrength"] : "5");
                    String.animationSpeed.Set(effect.ContainsKey("AnimationSpeed") ? effect["AnimationSpeed"] : "1,5");
                    String.shatterDensity.Set(effect.ContainsKey("ShatterDensity") ? effect["ShatterDensity"] : "1");
                    String.shatterDegree.Set(effect.ContainsKey("ShatterDegree") ? effect["ShatterDegree"] : "20");
                    String.shatterSpeed.Set(effect.ContainsKey("ShatterSpeed") ? effect["ShatterSpeed"] : "6");
                    String.elementShatterDensity.Set(effect.ContainsKey("ElementShatterDensity") ? effect["ElementShatterDensity"] : "1");
                    String.elementShatterDegree.Set(effect.ContainsKey("ElementShatterDegree") ? effect["ElementShatterDegree"] : "8");
                    String.elementShatterSpeed.Set(effect.ContainsKey("ElementShatterSpeed") ? effect["ElementShatterSpeed"] : "5");
                    String.await.Set(effect.ContainsKey("Await") ? effect["Await"] : "0");
                    String.powerScale.Set(effect.ContainsKey("PowerScale") ? effect["PowerScale"] : "1");
                    String.resourceAmount.Set(effect.ContainsKey("ResourceAmount") ? effect["ResourceAmount"] : "1");
                    String.changeAmount.Set(effect.ContainsKey("ChangeAmount") ? effect["ChangeAmount"] : "1");
                    String.buffDuration.Set(effect.ContainsKey("BuffDuration") ? effect["BuffDuration"] : "3");
                    Respawn("ObjectManagerEventEffects");
                    Respawn("ObjectManagerEventEffect");
                });
            }
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            if (eventEdit.effects.Count < 15)
                AddButtonRegion(() =>
                {
                    AddLine("Add a new effect");
                },
                (h) =>
                {
                    eventEdit.effects.Add(new Dictionary<string, string>());
                    });
            else
                AddPaddingRegion(() =>
                {
                    AddLine("Add a new effect", "DarkGray");
                });
        }),
        new("ObjectManagerEventEffect", () => {
            var effect = eventEdit.effects[selectedEffect];
            DisableShadows();
            SetAnchor(Top);
            AddHeaderGroup();
            SetRegionGroupWidth(296);
            AddHeaderRegion(() =>
            {
                AddLine("Effect:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                });
                AddSmallButton("OtherTrash", (h) =>
                {
                    eventEdit.effects.RemoveAt(selectedEffect);
                    CloseWindow(h.window);
                    Respawn("ObjectManagerEventEffects");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine(effect.ContainsKey("Effect") ? effect["Effect"] : "None");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerEventEffects");
                Respawn("ObjectManagerEffectList");
            });
            AddRegionGroup();
            SetRegionGroupWidth(148);
            SetRegionGroupHeight(316);
            if (effect.ContainsKey("Effect") && effect["Effect"] != "ChangeElements")
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Affect:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("Affect"))
                            effect["Affect"] = "None";
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("Affect") ? effect["Affect"] : "None");
                },
                (h) =>
                {
                    if (!effect.ContainsKey("Affect"))
                        effect.Add("Affect", "Effector");
                    else if (effect["Affect"] == "Effector")
                        effect["Affect"] = "Other";
                    else if (effect["Affect"] == "Other")
                        effect["Affect"] = "None";
                    else if (effect["Affect"] == "None")
                        effect["Affect"] = "Effector";
                    });
            }
            if (effect.ContainsKey("Effect") && (effect["Effect"] == "Damage" || effect["Effect"] == "Heal"))
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Power source:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("PowerSource"))
                            effect["PowerSource"] = "None";
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("PowerSource") ? effect["PowerSource"] : "None");
                },
                (h) =>
                {
                    if (!effect.ContainsKey("PowerSource"))
                        effect.Add("PowerSource", "Effector");
                    else if (effect["PowerSource"] == "Effector")
                        effect["PowerSource"] = "Other";
                    else if (effect["PowerSource"] == "Other")
                        effect["PowerSource"] = "Effector";
                    });
                AddPaddingRegion(() =>
                {
                    AddLine("Power type:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("PowerType"))
                            effect["PowerType"] = "None";
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("PowerType") ? effect["PowerType"] : "None");
                },
                (h) =>
                {
                    if (!effect.ContainsKey("PowerType"))
                        effect.Add("PowerType", "Spell");
                    else if (effect["PowerType"] == "Spell")
                        effect["PowerType"] = "Melee";
                    else if (effect["PowerType"] == "Melee")
                        effect["PowerType"] = "Ranged";
                    else if (effect["PowerType"] == "Ranged")
                        effect["PowerType"] = "Spell";
                    });
                AddPaddingRegion(() =>
                {
                    AddLine("Power scale:", "DarkGray");
                    AddInputLine(String.powerScale);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("PowerScale"))
                            effect["PowerScale"] = "1,0";
                        String.powerScale.Set("1,0");
                        });
                });
            }
            else if (effect.ContainsKey("Effect") && effect["Effect"] == "RemoveBuff")
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Buff:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("BuffName"))
                            effect["BuffName"] = "None";
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("BuffName") ? effect["BuffName"] : "None");
                    if (effect.ContainsKey("BuffName") && effect["BuffName"] != "None")
                        AddSmallButton(buffs.Find(x => x.name == effect["BuffName"]).icon);
                },
                (h) =>
                {
                    if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerBuffs"))
                    {
                        CloseWindow("ObjectManagerEventEffects");
                        CloseWindow("ObjectManagerSoundsList");
                        String.search.Set("");
                        buffsSearch = buffs;
                        SpawnWindowBlueprint("ObjectManagerBuffs");
                    }
                });
            }
            else if (effect.ContainsKey("Effect") && effect["Effect"] == "AddBuff")
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Buff:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("BuffName"))
                            effect["BuffName"] = "None";
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("BuffName") ? effect["BuffName"] : "None");
                    if (effect.ContainsKey("BuffName") && effect["BuffName"] != "None")
                        AddSmallButton(buffs.Find(x => x.name == effect["BuffName"]).icon);
                },
                (h) =>
                {
                    if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerBuffs"))
                    {
                        CloseWindow("ObjectManagerEventEffects");
                        CloseWindow("ObjectManagerSoundsList");
                        buffsSearch = buffs;
                        Respawn("ObjectManagerBuffs");
                    }
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Buff duration:", "DarkGray");
                    AddInputLine(String.buffDuration);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("PowerScale"))
                            effect["PowerScale"] = "3";
                        String.buffDuration.Set("3");
                        });
                });
            }
            else if (effect.ContainsKey("Effect") && (effect["Effect"] == "GiveResource" || effect["Effect"] == "DetractResource"))
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Resource type:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ResourceType"))
                            effect.Remove("ResourceType");
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None");
                    if (effect.ContainsKey("ResourceType") && effect["ResourceType"] != "None")
                        AddSmallButton("Element" + effect["ResourceType"] + "Rousing");
                },
                (h) =>
                {
                    if (!effect.ContainsKey("ResourceType"))
                        effect.Add("ResourceType", "Fire");
                    else if (effect["ResourceType"] == "Fire")
                        effect["ResourceType"] = "Earth";
                    else if (effect["ResourceType"] == "Earth")
                        effect["ResourceType"] = "Water";
                    else if (effect["ResourceType"] == "Water")
                        effect["ResourceType"] = "Air";
                    else if (effect["ResourceType"] == "Air")
                        effect["ResourceType"] = "Frost";
                    else if (effect["ResourceType"] == "Frost")
                        effect["ResourceType"] = "Decay";
                    else if (effect["ResourceType"] == "Decay")
                        effect["ResourceType"] = "Shadow";
                    else if (effect["ResourceType"] == "Shadow")
                        effect["ResourceType"] = "Order";
                    else if (effect["ResourceType"] == "Order")
                        effect["ResourceType"] = "Arcane";
                    else if (effect["ResourceType"] == "Arcane")
                        effect["ResourceType"] = "Lightning";
                    else if (effect["ResourceType"] == "Lightning")
                        effect["ResourceType"] = "Fire";
                    });
                AddPaddingRegion(() =>
                {
                    AddLine("Resource amount:", "DarkGray");
                    AddInputLine(String.resourceAmount);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ResourceAmount"))
                            effect["ResourceAmount"] = "1";
                        String.resourceAmount.Set("1");
                        });
                });
            }
            else if (effect.ContainsKey("Effect") && (effect["Effect"] == "ChangeElements"))
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Change amount:", "DarkGray");
                    AddInputLine(String.changeAmount);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ChangeAmount"))
                            effect["ChangeAmount"] = "1";
                        String.changeAmount.Set("1");
                        });
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Element from:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ElementFrom"))
                            effect.Remove("ElementFrom");
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("ElementFrom") ? effect["ElementFrom"] : "Random");
                    if (effect.ContainsKey("ElementFrom") && effect["ElementFrom"] != "Random")
                        AddSmallButton("Element" + effect["ElementFrom"] + "Rousing");
                },
                (h) =>
                {
                    if (!effect.ContainsKey("ElementFrom"))
                        effect.Add("ElementFrom", "Fire");
                    else if (effect["ElementFrom"] == "Fire")
                        effect["ElementFrom"] = "Earth";
                    else if (effect["ElementFrom"] == "Earth")
                        effect["ElementFrom"] = "Water";
                    else if (effect["ElementFrom"] == "Water")
                        effect["ElementFrom"] = "Air";
                    else if (effect["ElementFrom"] == "Air")
                        effect["ElementFrom"] = "Frost";
                    else if (effect["ElementFrom"] == "Frost")
                        effect["ElementFrom"] = "Decay";
                    else if (effect["ElementFrom"] == "Decay")
                        effect["ElementFrom"] = "Shadow";
                    else if (effect["ElementFrom"] == "Shadow")
                        effect["ElementFrom"] = "Order";
                    else if (effect["ElementFrom"] == "Order")
                        effect["ElementFrom"] = "Arcane";
                    else if (effect["ElementFrom"] == "Arcane")
                        effect["ElementFrom"] = "Lightning";
                    else if (effect["ElementFrom"] == "Lightning")
                        effect["ElementFrom"] = "Fire";
                    });
                AddPaddingRegion(() =>
                {
                    AddLine("Element to:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ElementTo"))
                            effect.Remove("ElementTo");
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("ElementTo") ? effect["ElementTo"] : "Random");
                    if (effect.ContainsKey("ElementTo") && effect["ElementTo"] != "Random")
                        AddSmallButton("Element" + effect["ElementTo"] + "Rousing");
                },
                (h) =>
                {
                    if (!effect.ContainsKey("ElementTo"))
                        effect.Add("ElementTo", "Fire");
                    else if (effect["ElementTo"] == "Fire")
                        effect["ElementTo"] = "Earth";
                    else if (effect["ElementTo"] == "Earth")
                        effect["ElementTo"] = "Water";
                    else if (effect["ElementTo"] == "Water")
                        effect["ElementTo"] = "Air";
                    else if (effect["ElementTo"] == "Air")
                        effect["ElementTo"] = "Frost";
                    else if (effect["ElementTo"] == "Frost")
                        effect["ElementTo"] = "Decay";
                    else if (effect["ElementTo"] == "Decay")
                        effect["ElementTo"] = "Shadow";
                    else if (effect["ElementTo"] == "Shadow")
                        effect["ElementTo"] = "Order";
                    else if (effect["ElementTo"] == "Order")
                        effect["ElementTo"] = "Arcane";
                    else if (effect["ElementTo"] == "Arcane")
                        effect["ElementTo"] = "Lightning";
                    else if (effect["ElementTo"] == "Lightning")
                        effect["ElementTo"] = "Fire";
                    });
            }
            AddPaddingRegion(() =>
            {
                AddLine("Await:", "DarkGray");
                AddInputLine(String.await);
                AddSmallButton("OtherReverse", (h) =>
                {
                    if (effect.ContainsKey("Await"))
                        effect["Await"] = "0";
                    String.await.Set("0");
                    });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Chance base:", "DarkGray");
                AddInputLine(String.chanceBase);
                AddSmallButton("OtherReverse", (h) =>
                {
                    if (effect.ContainsKey("ChanceBase"))
                        effect["ChanceBase"] = "100";
                    String.chanceBase.Set("100");
                    });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Chance:", "DarkGray");
                AddInputLine(String.chance);
                AddSmallButton("OtherReverse", (h) =>
                {
                    if (effect.ContainsKey("Chance"))
                        effect["Chance"] = "0";
                    String.chance.Set("0");
                    });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Chance scale:", "DarkGray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    if (effect.ContainsKey("ChanceScale"))
                        effect["ChanceScale"] = "None";
                    });
            });
            AddButtonRegion(() =>
            {
                AddLine(effect.ContainsKey("ChanceScale") ? effect["ChanceScale"] : "None");
            },
            (h) =>
            {
                if (!effect.ContainsKey("ChanceScale"))
                    effect.Add("ChanceScale", "Fire Mastery");
                else if (effect["ChanceScale"] == "Fire Mastery")
                    effect["ChanceScale"] = "Earth Mastery";
                else if (effect["ChanceScale"] == "Earth Mastery")
                    effect["ChanceScale"] = "Water Mastery";
                else if (effect["ChanceScale"] == "Water Mastery")
                    effect["ChanceScale"] = "Air Mastery";
                else if (effect["ChanceScale"] == "Air Mastery")
                    effect["ChanceScale"] = "Frost Mastery";
                else if (effect["ChanceScale"] == "Frost Mastery")
                    effect["ChanceScale"] = "Decay Mastery";
                else if (effect["ChanceScale"] == "Decay Mastery")
                    effect["ChanceScale"] = "Shadow Mastery";
                else if (effect["ChanceScale"] == "Shadow Mastery")
                    effect["ChanceScale"] = "Order Mastery";
                else if (effect["ChanceScale"] == "Order Mastery")
                    effect["ChanceScale"] = "Arcane Mastery";
                else if (effect["ChanceScale"] == "Arcane Mastery")
                    effect["ChanceScale"] = "Lightning Mastery";
                else if (effect["ChanceScale"] == "Lightning Mastery")
                    effect["ChanceScale"] = "Strength";
                else if (effect["ChanceScale"] == "Strength")
                    effect["ChanceScale"] = "Agility";
                else if (effect["ChanceScale"] == "Agility")
                    effect["ChanceScale"] = "Intellect";
                else if (effect["ChanceScale"] == "Intellect")
                    effect["ChanceScale"] = "None";
                else if (effect["ChanceScale"] == "None")
                    effect["ChanceScale"] = "Fire Mastery";
                });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddRegionGroup();
            SetRegionGroupWidth(148);
            SetRegionGroupHeight(316);
            AddPaddingRegion(() =>
            {
                AddLine("Sound effect:", "DarkGray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    if (effect.ContainsKey("SoundEffect"))
                        effect["SoundEffect"] = "None";
                    });
            });
            AddButtonRegion(() =>
            {
                AddLine(!effect.ContainsKey("SoundEffect") ? "None" : effect["SoundEffect"] + ".ogg");
                if (effect.ContainsKey("SoundEffect"))
                    AddSmallButton("OtherSound", (h) =>
                    {
                        PlaySound(effect["SoundEffect"]);
                    });
            },
            (h) =>
            {
                Assets.assets.soundsSearch = Assets.assets.sounds;
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerSoundsList"))
                {
                    CloseWindow("ObjectManagerEventEffects");
                    CloseWindow("ObjectManagerBuffs");
                    Respawn("ObjectManagerSoundsList");
                }
            });
            AddPaddingRegion(() =>
            {
                AddLine("Animation type:", "DarkGray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    if (effect.ContainsKey("AnimationType"))
                        effect["AnimationType"] = "None";
                    });
            });
            AddButtonRegion(() =>
            {
                AddLine(effect.ContainsKey("AnimationType") ? effect["AnimationType"] : "None");
            },
            (h) =>
            {
                if (!effect.ContainsKey("AnimationType"))
                    effect.Add("AnimationType", "Missile");
                else if (effect["AnimationType"] == "Missile")
                    effect["AnimationType"] = "None";
                else if (effect["AnimationType"] == "None")
                    effect["AnimationType"] = "Missile";
                });
            if (effect.ContainsKey("AnimationType") && effect["AnimationType"] != "None")
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Animation speed:", "DarkGray");
                    AddInputLine(String.animationSpeed);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("AnimationSpeed"))
                            effect["AnimationSpeed"] = "1,5";
                        String.animationSpeed.Set("1,5");
                        });
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Animation arc:", "DarkGray");
                    AddInputLine(String.animationArc);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("AnimationArc"))
                            effect["AnimationArc"] = "20";
                        String.animationArc.Set("20");
                        });
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Trail strength:", "DarkGray");
                    AddInputLine(String.trailStrength);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("TrailStrength"))
                            effect["TrailStrength"] = "5";
                        String.trailStrength.Set("5");
                        });
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine("Shatter type:", "DarkGray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    if (effect.ContainsKey("ShatterType"))
                        effect["ShatterType"] = "None";
                    });
            });
            AddButtonRegion(() =>
            {
                AddLine(effect.ContainsKey("ShatterType") ? effect["ShatterType"] : "None");
            },
            (h) =>
            {
                if (!effect.ContainsKey("ShatterType"))
                    effect.Add("ShatterType", "Central");
                else if (effect["ShatterType"] == "Central")
                    effect["ShatterType"] = "Directional";
                else if (effect["ShatterType"] == "Directional")
                    effect["ShatterType"] = "None";
                else if (effect["ShatterType"] == "None")
                    effect["ShatterType"] = "Central";
                });
            if (effect.ContainsKey("ShatterType") && effect["ShatterType"] != "None")
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Shatter target:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ShatterTarget"))
                            effect["ShatterTarget"] = "Effector";
                        });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("ShatterTarget") ? effect["ShatterTarget"] : "None");
                },
                (h) =>
                {
                    if (!effect.ContainsKey("ShatterTarget"))
                        effect.Add("ShatterTarget", "Effector");
                    else if (effect["ShatterTarget"] == "Effector")
                        effect["ShatterTarget"] = "Other";
                    else if (effect["ShatterTarget"] == "Other")
                        effect["ShatterTarget"] = "Effector";
                    });
                AddPaddingRegion(() =>
                {
                    AddLine("Shatter degree:", "DarkGray");
                    AddInputLine(String.shatterDegree);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ShatterDegree"))
                            effect["ShatterDegree"] = "20";
                        String.shatterDegree.Set("20");
                        });
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Shatter density:", "DarkGray");
                    AddInputLine(String.shatterDensity);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ShatterDensity"))
                            effect["ShatterDensity"] = "1";
                        String.shatterDensity.Set("1");
                        });
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Shatter speed:", "DarkGray");
                    AddInputLine(String.shatterSpeed);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ShatterSpeed"))
                            effect["ShatterSpeed"] = "6";
                        String.shatterSpeed.Set("6");
                        });
                });
            }
            if (effect.ContainsKey("Effect") && (effect["Effect"] == "ChangeElements"))
            {
                AddPaddingRegion(() =>
                {
                    AddLine("E.Shatter degree:", "DarkGray");
                    AddInputLine(String.elementShatterDegree);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ElementShatterDegree"))
                            effect["ElementShatterDegree"] = "8";
                        String.elementShatterDegree.Set("8");
                        });
                });
                AddPaddingRegion(() =>
                {
                    AddLine("E.Shatter density:", "DarkGray");
                    AddInputLine(String.elementShatterDensity);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ElementShatterDensity"))
                            effect["ElementShatterDensity"] = "1";
                        String.elementShatterDensity.Set("1");
                        });
                });
                AddPaddingRegion(() =>
                {
                    AddLine("E.Shatter speed:", "DarkGray");
                    AddInputLine(String.elementShatterSpeed);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ElementShatterSpeed"))
                            effect["ElementShatterSpeed"] = "5";
                        String.elementShatterSpeed.Set("5");
                        });
                });
            }
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
        }),
        new("ObjectManagerHostileAreaCommonEncounters", () => {
            DisableShadows();
            SetAnchor(Top);
            AddHeaderGroup();
            SetRegionGroupWidth(296);
            AddHeaderRegion(() =>
            {
                AddLine("Common encounters:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                });
            });
            AddRegionGroup();
            SetRegionGroupWidth(167);
            SetRegionGroupHeight(335);
            if (area.commonEncounters == null) area.commonEncounters = new List<Encounter>();
            foreach (var ce in area.commonEncounters)
            {
                if (!String.encounterLevels.ContainsKey(ce))
                    String.encounterLevels.Add(ce, (new String() { value = ce.levelMin + "", inputType = InputType.Numbers }, new String() { value = ce.levelMax + "", inputType = InputType.Numbers }));
                AddButtonRegion(() =>
                {
                    AddLine(ce.who);
                    var race = races.Find(x => x.name == ce.who);
                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait);
                },
                (h) =>
                {
                    Encounter.encounter = ce;
                    CloseWindow("ObjectManagerHostileAreas");
                    Respawn("ObjectManagerRaces");
                });
            }
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddButtonRegion(() =>
            {
                AddLine("Add new encounter");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerHostileAreas");
                Respawn("ObjectManagerRaces");
            });
            AddRegionGroup();
            SetRegionGroupWidth(55);
            SetRegionGroupHeight(335);
            foreach (var ce in area.commonEncounters)
                AddPaddingRegion(() =>
                {
                    AddInputLine(String.encounterLevels[ce].Item1);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        ce.levelMin = 1;
                        String.encounterLevels[ce].Item1.Set("1");
                        });
                });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddRegionGroup();
            SetRegionGroupWidth(55);
            SetRegionGroupHeight(335);
            foreach (var ce in area.commonEncounters)
                AddPaddingRegion(() =>
                {
                    AddInputLine(String.encounterLevels[ce].Item2);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        ce.levelMax = 0;
                        String.encounterLevels[ce].Item2.Set("0");
                        });
                });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddRegionGroup();
            SetRegionGroupWidth(19);
            SetRegionGroupHeight(335);
            foreach (var ce in area.commonEncounters)
                AddPaddingRegion(() =>
                {
                    AddSmallButton("OtherTrash", (h) =>
                    {
                        area.commonEncounters.Remove(ce);
                        });
                });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
        }),
        new("ObjectManagerHostileAreaRareEncounters", () => {
            DisableShadows();
            SetAnchor(Top);
            AddHeaderGroup();
            SetRegionGroupWidth(296);
            AddHeaderRegion(() =>
            {
                AddLine("Rare encounters:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                });
            });
            AddRegionGroup();
            SetRegionGroupWidth(167);
            SetRegionGroupHeight(335);
            if (area.rareEncounters == null) area.rareEncounters = new List<Encounter>();
            foreach (var ce in area.rareEncounters)
            {
                if (!String.encounterLevels.ContainsKey(ce))
                    String.encounterLevels.Add(ce, (new String() { value = ce.levelMin + "", inputType = InputType.Numbers }, new String() { value = ce.levelMax + "", inputType = InputType.Numbers }));
                AddButtonRegion(() =>
                {
                    AddLine(ce.who);
                    var race = races.Find(x => x.name == ce.who);
                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait);
                },
                (h) =>
                {
                    Encounter.encounter = ce;
                    CloseWindow("ObjectManagerHostileAreas");
                    Respawn("ObjectManagerRaces");
                });
            }
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddButtonRegion(() =>
            {
                AddLine("Add new encounter");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerHostileAreas");
                Respawn("ObjectManagerRaces");
            });
            AddRegionGroup();
            SetRegionGroupWidth(55);
            SetRegionGroupHeight(335);
            foreach (var ce in area.rareEncounters)
                AddPaddingRegion(() =>
                {
                    AddInputLine(String.encounterLevels[ce].Item1);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        ce.levelMin = 1;
                        String.encounterLevels[ce].Item1.Set("1");
                        });
                });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddRegionGroup();
            SetRegionGroupWidth(55);
            SetRegionGroupHeight(335);
            foreach (var ce in area.rareEncounters)
                AddPaddingRegion(() =>
                {
                    AddInputLine(String.encounterLevels[ce].Item2);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        ce.levelMax = 0;
                        String.encounterLevels[ce].Item2.Set("0");
                        });
                });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddRegionGroup();
            SetRegionGroupWidth(19);
            SetRegionGroupHeight(335);
            foreach (var ce in area.rareEncounters)
                AddPaddingRegion(() =>
                {
                    AddSmallButton("OtherTrash", (h) =>
                    {
                        area.rareEncounters.Remove(ce);
                        });
                });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
        }),
        new("ObjectManagerHostileAreaEliteEncounters", () => {
            DisableShadows();
            SetAnchor(Top);
            AddHeaderGroup();
            SetRegionGroupWidth(296);
            AddHeaderRegion(() =>
            {
                AddLine("Elite encounters:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                });
            });
            AddRegionGroup();
            SetRegionGroupWidth(167);
            SetRegionGroupHeight(335);
            if (area.eliteEncounters == null) area.eliteEncounters = new List<Encounter>();
            foreach (var ce in area.eliteEncounters)
            {
                if (!String.encounterLevels.ContainsKey(ce))
                    String.encounterLevels.Add(ce, (new String() { value = ce.levelMin + "", inputType = InputType.Numbers }, new String() { value = ce.levelMax + "", inputType = InputType.Numbers }));
                AddButtonRegion(() =>
                {
                    AddLine(ce.who);
                    var race = races.Find(x => x.name == ce.who);
                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait);
                },
                (h) =>
                {
                    Encounter.encounter = ce;
                    CloseWindow("ObjectManagerHostileAreas");
                    Respawn("ObjectManagerRaces");
                });
            }
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddButtonRegion(() =>
            {
                AddLine("Add new encounter");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerHostileAreas");
                Respawn("ObjectManagerRaces");
            });
            AddRegionGroup();
            SetRegionGroupWidth(55);
            SetRegionGroupHeight(335);
            foreach (var ce in area.eliteEncounters)
                AddPaddingRegion(() =>
                {
                    AddInputLine(String.encounterLevels[ce].Item1);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        ce.levelMin = 1;
                        String.encounterLevels[ce].Item1.Set("1");
                        });
                });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddRegionGroup();
            SetRegionGroupWidth(55);
            SetRegionGroupHeight(335);
            foreach (var ce in area.eliteEncounters)
                AddPaddingRegion(() =>
                {
                    AddInputLine(String.encounterLevels[ce].Item2);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        ce.levelMax = 0;
                        String.encounterLevels[ce].Item2.Set("0");
                        });
                });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddRegionGroup();
            SetRegionGroupWidth(19);
            SetRegionGroupHeight(335);
            foreach (var ce in area.eliteEncounters)
                AddPaddingRegion(() =>
                {
                    AddSmallButton("OtherTrash", (h) =>
                    {
                        area.eliteEncounters.Remove(ce);
                        });
                });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
        }),
        new("ObjectManagerHostileAreaTypeList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Area types:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    SpawnWindowBlueprint("ObjectManagerHostileAreas");
                });
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            AddButtonRegion(() =>
            {
                AddLine("HostileArea");
                AddSmallButton("SiteHostileArea");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerHostileAreaTypeList");
                if (area != null)
                {
                    area.type = "HostileArea";
                    Respawn("ObjectManagerHostileArea");
                    Respawn("ObjectManagerHostileAreas");
                }
            });
            AddButtonRegion(() =>
            {
                AddLine("EmeraldBough");
                AddSmallButton("SiteEmeraldBough");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerHostileAreaTypeList");
                if (area != null)
                {
                    area.type = "EmeraldBough";
                    Respawn("ObjectManagerHostileArea");
                    Respawn("ObjectManagerHostileAreas");
                }
            });
            for (int i = 2; i < 10; i++)
                AddPaddingRegion(() => { AddLine(); });
            AddPaddingRegion(() =>
            {
                AddLine("2 hostile area types", "DarkGray");
            });
        }),
        new("ObjectManagerRarityList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Rarities:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    SpawnWindowBlueprint("ObjectManagerItems");
                });
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            AddButtonRegion(() =>
            {
                AddLine("Poor");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerRarityList");
                if (item != null)
                {
                    item.rarity = "Poor";
                    Respawn("ObjectManagerItem");
                    Respawn("ObjectManagerItems");
                }
            });
            AddButtonRegion(() =>
            {
                AddLine("Common");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerRarityList");
                if (item != null)
                {
                    item.rarity = "Common";
                    Respawn("ObjectManagerItem");
                    Respawn("ObjectManagerItems");
                }
            });
            AddButtonRegion(() =>
            {
                AddLine("Uncommon");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerRarityList");
                if (item != null)
                {
                    item.rarity = "Uncommon";
                    Respawn("ObjectManagerItem");
                    Respawn("ObjectManagerItems");
                }
            });
            AddButtonRegion(() =>
            {
                AddLine("Rare");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerRarityList");
                if (item != null)
                {
                    item.rarity = "Rare";
                    Respawn("ObjectManagerItem");
                    Respawn("ObjectManagerItems");
                }
            });
            AddButtonRegion(() =>
            {
                AddLine("Epic");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerRarityList");
                if (item != null)
                {
                    item.rarity = "Epic";
                    Respawn("ObjectManagerItem");
                    Respawn("ObjectManagerItems");
                }
            });
            AddButtonRegion(() =>
            {
                AddLine("Legendary");
            },
            (h) =>
            {
                CloseWindow("ObjectManagerRarityList");
                if (item != null)
                {
                    item.rarity = "Legendary";
                    Respawn("ObjectManagerItem");
                    Respawn("ObjectManagerItems");
                }
            });
            for (int i = 6; i < 10; i++)
                AddPaddingRegion(() => { AddLine(); });
            AddPaddingRegion(() =>
            {
                AddLine("6 rarities", "DarkGray");
            });
        }),
        new("ItemsSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort items:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("ItemsSort");
                    Respawn("ObjectManagerItems");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                items = items.OrderBy(x => x.name).ToList();
                itemsSearch = itemsSearch.OrderBy(x => x.name).ToList();
                PlaySound("DesktopInventorySort", 0.4f);
                CloseWindow("ItemsSort");
                Respawn("ObjectManagerItems");
            });
            AddButtonRegion(() =>
            {
                AddLine("By item power", "Black");
            },
            (h) =>
            {
                items = items.OrderByDescending(x => x.ilvl).ToList();
                itemsSearch = itemsSearch.OrderByDescending(x => x.ilvl).ToList();
                PlaySound("DesktopInventorySort", 0.4f);
                CloseWindow("ItemsSort");
                Respawn("ObjectManagerItems");
            });
            AddButtonRegion(() =>
            {
                AddLine("By price", "Black");
            },
            (h) =>
            {
                items = items.OrderByDescending(x => x.price).ToList();
                itemsSearch = itemsSearch.OrderByDescending(x => x.price).ToList();
                PlaySound("DesktopInventorySort", 0.4f);
                CloseWindow("ItemsSort");
                Respawn("ObjectManagerItems");
            });
            AddButtonRegion(() =>
            {
                AddLine("By type", "Black");
            },
            (h) =>
            {
                items = items.OrderByDescending(x => x.type).ToList();
                itemsSearch = itemsSearch.OrderByDescending(x => x.type).ToList();
                PlaySound("DesktopInventorySort", 0.4f);
                CloseWindow("ItemsSort");
                Respawn("ObjectManagerItems");
            });
        }),
        new("ObjectManagerItems", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => itemsSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (item != null)
            {
                var index = itemsSearch.IndexOf(item);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Items:");
                AddSmallButton("OtherClose", (h) =>
                {
                    item = null; itemsSearch = null;
                    CloseDesktop("ObjectManagerItems");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    items.Reverse();
                    itemsSearch.Reverse();
                    PlaySound("DesktopInventorySort", 0.4f);
                    Respawn("ObjectManagerItems");
                });
                if (!CDesktop.windows.Exists(x => x.title == "ItemsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("ItemsSort");
                        Respawn("ObjectManagerItems");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (itemsSearch.Count > index + 10 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = itemsSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon);
                        AddSmallButtonOverlay("OtherRarity" + foo.rarity + "Big");
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    item = itemsSearch[index + 10 * regionGroup.pagination()];
                    String.objectName.Set(item.name);
                    String.price.Set(item.price + "");
                    String.itemPower.Set(item.ilvl + "");
                    String.requiredLevel.Set(item.lvl + "");
                    Respawn("ObjectManagerItem");
                },
                null,
                (h) => () =>
                {
                    PrintItemTooltip(items[index + 10 * regionGroup.pagination()]);
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(items.Count + " items", "DarkGray");
                if (items.Count != itemsSearch.Count)
                    AddLine(itemsSearch.Count + " found in search", "DarkGray");
            });
            AddButtonRegion(() =>
            {
                AddLine("Create a new item");
            },
            (h) =>
            {
                item = new Item()
                {
                    name = "Item #" + items.Count,
                    abilities = new(),
                    specs = new(),
                    icon = "ItemEgg03",
                    type = "Miscellaneous",
                    rarity = "Common",
                    price = 1
                };
                items.Add(item);
                itemsSearch = items.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
                String.objectName.Set(item.name);
                String.price.Set(item.price + "");
                String.itemPower.Set(item.ilvl + "");
                String.requiredLevel.Set(item.lvl + "");
                Respawn("ObjectManagerItem");
                });
        }),
        new("ObjectManagerItem", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Item:", "DarkGray"); });
            AddPaddingRegion(() => { AddInputLine(String.objectName, item.rarity); });
            AddPaddingRegion(() => { AddLine("Icon:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(item.icon.Substring(4) + ".png");
                AddSmallButton(item.icon);
            },
            (h) =>
            {
                if (CDesktop.windows.Find(x => x.title == "ObjectManagerItemIconList") == null)
                {
                    CloseWindow("ObjectManagerItems");
                    Assets.assets.itemIconsSearch = Assets.assets.itemIcons;
                    SpawnWindowBlueprint("ObjectManagerItemIconList");
                }
            });
            AddPaddingRegion(() => { AddLine("Rarity:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(item.rarity);
            },
            (h) =>
            {
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerRarityList"))
                {
                    CloseWindow("ObjectManagerItems");
                    SpawnWindowBlueprint("ObjectManagerRarityList");
                }
            });
            AddPaddingRegion(() =>
            {
                AddLine("Price:", "DarkGray");
                AddInputLine(String.price);
            });
            AddPaddingRegion(() =>
            {
                AddLine("Item power:", "DarkGray");
                AddInputLine(String.itemPower);
            });
            AddPaddingRegion(() =>
            {
                AddLine("Required level:", "DarkGray");
                AddInputLine(String.requiredLevel);
            });
            AddButtonRegion(() =>
            {
                AddLine("Manage stats");
            },
            (h) =>
            {
                item.stats ??= new(new());
                if (!item.stats.stats.ContainsKey("Stamina")) item.stats.stats.Add("Stamina", 0);
                if (!item.stats.stats.ContainsKey("Strength")) item.stats.stats.Add("Strength", 0);
                if (!item.stats.stats.ContainsKey("Agility")) item.stats.stats.Add("Agility", 0);
                if (!item.stats.stats.ContainsKey("Intellect")) item.stats.stats.Add("Intellect", 0);
                if (!item.stats.stats.ContainsKey("Spirit")) item.stats.stats.Add("Spirit", 0);
                String.stamina.Set(item.stats.stats["Stamina"] + "");
                String.strength.Set(item.stats.stats["Strength"] + "");
                String.agility.Set(item.stats.stats["Agility"] + "");
                String.intellect.Set(item.stats.stats["Intellect"] + "");
                String.spirit.Set(item.stats.stats["Spirit"] + "");
                Respawn("ObjectManagerItemStatManager");
                CloseWindow("ObjectManagerItems");
            });
            AddPaddingRegion(() => { });
        }),
        new("ItemSetsSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort item sets:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("ItemSetsSort");
                    Respawn("ObjectManagerItemSets");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                itemSets = itemSets.OrderBy(x => x.name).ToList();
                itemSetsSearch = itemSetsSearch.OrderBy(x => x.name).ToList();
                CloseWindow("ItemSetsSort");
                Respawn("ObjectManagerItemSets");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By average item power", "Black");
            },
            (h) =>
            {
                itemSets = itemSets.OrderByDescending(x => items.FindAll(y => y.set == x.name).Average(y => y.ilvl)).ToList();
                itemSetsSearch = itemSetsSearch.OrderByDescending(x => items.FindAll(y => y.set == x.name).Average(y => y.ilvl)).ToList();
                CloseWindow("ItemSetsSort");
                Respawn("ObjectManagerItemSets");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By amount of tiers", "Black");
            },
            (h) =>
            {
                itemSets = itemSets.OrderByDescending(x => x.setBonuses.Count).ToList();
                itemSetsSearch = itemSetsSearch.OrderByDescending(x => x.setBonuses.Count).ToList();
                CloseWindow("ItemSetsSort");
                Respawn("ObjectManagerItemSets");
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("ObjectManagerItemSets", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => itemSetsSearch.Count, 5);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (itemSet != null)
            {
                var index = itemSetsSearch.IndexOf(itemSet);
                if (index >= 5) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 5);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Item sets:");
                AddSmallButton("OtherClose", (h) =>
                {
                    itemSet = null; itemSetsSearch = null;
                    CloseDesktop("ObjectManagerItemSets");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    itemSets.Reverse();
                    itemSetsSearch.Reverse();
                    Respawn("ObjectManagerItemSets");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "ItemSetsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("ItemSetsSort");
                        Respawn("ObjectManagerItemSets");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 5; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (itemSetsSearch.Count > index + 5 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = itemSetsSearch[index + 5 * regionGroup.pagination()];
                        AddLine(foo.name);
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    itemSet = itemSetsSearch[index + 5 * regionGroup.pagination()];
                    String.objectName.Set(itemSet.name);
                    Respawn("ObjectManagerItemSet");
                });
                AddPaddingRegion(() =>
                {
                    AddLine();
                    if (itemSetsSearch.Count > index + 5 * regionGroup.pagination())
                    {
                        var foo = itemSetsSearch[index + 5 * regionGroup.pagination()];
                        var setItems = items.FindAll(x => x.set == foo.name);
                        for (var j = 0; j < setItems.Count && j < 9; j++)
                        {
                            var J = j;
                            AddSmallButton(setItems[J].icon, null, null, (h) => () =>
                            {
                                PrintItemTooltip(setItems[J]);
                            });
                            AddSmallButtonOverlay("OtherRarity" + setItems[J].rarity + "Big");
                        }
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(itemSets.Count + " item sets", "DarkGray");
                if (itemSets.Count != itemSetsSearch.Count)
                    AddLine(itemSetsSearch.Count + " found in search", "DarkGray");
            });
            AddButtonRegion(() =>
            {
                AddLine("Create a new item set");
            },
            (h) =>
            {
                itemSet = new ItemSet()
                {
                    name = "Item set #" + itemSets.Count,
                    setBonuses = new List<SetBonus>()
                };
                itemSets.Add(itemSet);
                itemSetsSearch = itemSets.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
                String.objectName.Set(itemSet.name);
                Respawn("ObjectManagerItemSet");
                });
        }),
        new("ObjectManagerItemSet", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Item set:", "DarkGray"); });
            AddPaddingRegion(() => { AddInputLine(String.objectName); });
            AddPaddingRegion(() => { });
        }),
        new("AbilitiesSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort abilities:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("AbilitiesSort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                abilities = abilities.OrderBy(x => x.name).ToList();
                CloseWindow("AbilitiesSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By status", "Black");
            },
            (h) =>
            {
                abilities = abilities.OrderBy(x => currentSave.player.actionBars.Contains(x.name)).ToList();
                CloseWindow("AbilitiesSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By rank", "Black");
            },
            (h) =>
            {
                abilities = abilities.OrderByDescending(x => currentSave.player.abilities.ContainsKey(x.name) ? currentSave.player.abilities[x.name] : 0).ToList();
                CloseWindow("AbilitiesSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By cost", "Black");
            },
            (h) =>
            {
                abilities = abilities.OrderByDescending(x => x.cost == null ? -1 : x.cost.Sum(y => y.Value)).ToList();
                CloseWindow("AbilitiesSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By cooldown", "Black");
            },
            (h) =>
            {
                abilities = abilities.OrderByDescending(x => x.cooldown).ToList();
                CloseWindow("AbilitiesSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("ObjectManagerCostManager", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Ability cost:", "Gray");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    if (ability.cost["Fire"] == 0) ability.cost.Remove("Fire");
                    if (ability.cost["Earth"] == 0) ability.cost.Remove("Earth");
                    if (ability.cost["Water"] == 0) ability.cost.Remove("Water");
                    if (ability.cost["Air"] == 0) ability.cost.Remove("Air");
                    if (ability.cost["Frost"] == 0) ability.cost.Remove("Frost");
                    if (ability.cost["Decay"] == 0) ability.cost.Remove("Decay");
                    if (ability.cost["Shadow"] == 0) ability.cost.Remove("Shadow");
                    if (ability.cost["Order"] == 0) ability.cost.Remove("Order");
                    if (ability.cost["Arcane"] == 0) ability.cost.Remove("Arcane");
                    if (ability.cost["Lightning"] == 0) ability.cost.Remove("Lightning");
                    if (ability.cost.Count == 0) ability.cost = null;
                    CloseWindow(h.window);
                    Respawn("ObjectManagerAbilities");
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Fire: ", "DarkGray");
                AddInputLine(String.fire, String.fire.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementFireRousing");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Earth: ", "DarkGray");
                AddInputLine(String.earth, String.earth.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementEarthRousing");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Water: ", "DarkGray");
                AddInputLine(String.water, String.water.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementWaterRousing");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Air: ", "DarkGray");
                AddInputLine(String.air, String.air.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementAirRousing");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Frost: ", "DarkGray");
                AddInputLine(String.frost, String.frost.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementFrostRousing");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Decay: ", "DarkGray");
                AddInputLine(String.decay, String.decay.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementDecayRousing");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Shadow: ", "DarkGray");
                AddInputLine(String.shadow, String.shadow.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementShadowRousing");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Order: ", "DarkGray");
                AddInputLine(String.order, String.order.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementOrderRousing");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Arcane: ", "DarkGray");
                AddInputLine(String.arcane, String.arcane.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementArcaneRousing");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Lightning: ", "DarkGray");
                AddInputLine(String.lightning, String.lightning.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementLightningRousing");
            });
            AddPaddingRegion(() => { });
        }),
        new("ObjectManagerItemStatManager", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Stats:", "Gray");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    if (item.stats.stats["Stamina"] == 0) item.stats.stats.Remove("Stamina");
                    if (item.stats.stats["Strength"] == 0) item.stats.stats.Remove("Strength");
                    if (item.stats.stats["Agility"] == 0) item.stats.stats.Remove("Agility");
                    if (item.stats.stats["Intellect"] == 0) item.stats.stats.Remove("Intellect");
                    if (item.stats.stats["Spirit"] == 0) item.stats.stats.Remove("Spirit");
                    //if (item.stats.stats.Count == 0) item.stats = null;
                    CloseWindow(h.window);
                    Respawn("ObjectManagerItems");
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Stamina: ", "DarkGray");
                AddInputLine(String.stamina, String.stamina.Value() == "0" ? "DarkGray" : "Gray");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Strength: ", "DarkGray");
                AddInputLine(String.strength, String.strength.Value() == "0" ? "DarkGray" : "Gray");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Agility: ", "DarkGray");
                AddInputLine(String.agility, String.agility.Value() == "0" ? "DarkGray" : "Gray");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Intellect: ", "DarkGray");
                AddInputLine(String.intellect, String.intellect.Value() == "0" ? "DarkGray" : "Gray");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Spirit: ", "DarkGray");
                AddInputLine(String.spirit, String.spirit.Value() == "0" ? "DarkGray" : "Gray");
            });
            AddPaddingRegion(() => { });
        }),
        new("ObjectManagerAbilities", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => abilitiesSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (ability != null)
            {
                var index = abilitiesSearch.IndexOf(ability);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            if (eventEdit != null)
            {
                var editingEffects = CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect");
                if (editingEffects)
                {
                    var index = abilitiesSearch.IndexOf(eventEdit.effects[selectedEffect].ContainsKey("AbilityName") ? abilities.Find(x => x.name == eventEdit.effects[selectedEffect]["AbilityName"]) : null);
                    if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
                }
                else
                {
                    var index = abilitiesSearch.IndexOf(eventEdit.triggers[selectedTrigger].ContainsKey("AbilityName") ? abilities.Find(x => x.name == eventEdit.triggers[selectedTrigger]["AbilityName"]) : null);
                    if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
                }
            }
            AddHeaderRegion(() =>
            {
                AddLine("Abilities:");
                AddSmallButton("OtherClose", (h) =>
                {
                    if (eventEdit != null)
                    {
                        CloseWindow(h.window);
                        var editingEffects = CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect");
                        if (editingEffects) Respawn("ObjectManagerEventEffects");
                        else Respawn("ObjectManagerEventTriggers");
                    }
                    else
                    {
                        ability = null; eventEdit = null; abilitiesSearch = null;
                        CloseDesktop("ObjectManagerAbilities");
                    }
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    abilities.Reverse();
                    abilitiesSearch.Reverse();
                    Respawn("ObjectManagerAbilities");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "AbilitiesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("AbilitiesSort");
                        Respawn("ObjectManagerAbilities");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (abilitiesSearch.Count > index + 10 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = abilitiesSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon);
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    if (eventEdit != null)
                    {
                        var editingEffects = CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect");
                        if (editingEffects)
                        {
                            if (eventEdit.effects[selectedEffect].ContainsKey("AbilityName"))
                                eventEdit.effects[selectedEffect]["AbilityName"] = abilitiesSearch[index + 10 * regionGroup.pagination()].name;
                            else eventEdit.effects[selectedEffect].Add("AbilityName", abilitiesSearch[index + 10 * regionGroup.pagination()].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventEffect");
                            Respawn("ObjectManagerEventEffects");
                        }
                        else
                        {
                            if (eventEdit.triggers[selectedTrigger].ContainsKey("AbilityName"))
                                eventEdit.triggers[selectedTrigger]["AbilityName"] = abilitiesSearch[index + 10 * regionGroup.pagination()].name;
                            else eventEdit.triggers[selectedTrigger].Add("AbilityName", abilitiesSearch[index + 10 * regionGroup.pagination()].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventTrigger");
                            Respawn("ObjectManagerEventTriggers");
                        }
                    }
                    else
                    {
                        ability = abilitiesSearch[index + 10 * regionGroup.pagination()];
                        String.objectName.Set(ability.name);
                        String.cooldown.Set(ability.cooldown + "");
                        Respawn("ObjectManagerAbility");
                    }
                },
                null,
                (h) => () =>
                {
                    SetAnchor(Center);
                    var key = abilitiesSearch.ToList()[index + 10 * regionGroup.pagination()];
                    PrintAbilityTooltip(null, null, key, 0);
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(abilities.Count + " abilities", "DarkGray");
                if (abilities.Count != abilitiesSearch.Count)
                    AddLine(abilitiesSearch.Count + " found in search", "DarkGray");
            });
            AddButtonRegion(() =>
            {
                AddLine("Create a new ability");
            },
            (h) =>
            {
                ability = new Ability()
                {
                    name = "Ability #" + abilities.Count,
                    icon = "AbilityAbolishMagic",
                    events = new(),
                    tags = new()
                };
                abilities.Add(ability);
                abilitiesSearch = abilities.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
                String.objectName.Set(ability.name);
                String.cooldown.Set(ability.cooldown + "");
                Respawn("ObjectManagerAbility");
                });
        }),
        new("ObjectManagerAbility", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() =>
            {
                AddLine("Ability:", "DarkGray");
                AddSmallButton("OtherSound",
                (h) =>
                {
                    NewBoard(ability);
                    SpawnDesktopBlueprint("GameSimulation");
                });
            });
            AddPaddingRegion(() => { AddInputLine(String.objectName, ability.name); });
            AddPaddingRegion(() => { AddLine("Icon:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(ability.icon.Substring(7) + ".png");
                AddSmallButton(ability.icon);
            },
            (h) =>
            {
                if (CDesktop.windows.Find(x => x.title == "ObjectManagerAbilityIconList") == null)
                {
                    CloseWindow("ObjectManagerAbilities");
                    CloseWindow("ObjectManagerEventTriggers");
                    CloseWindow("ObjectManagerEventTrigger");
                    CloseWindow("ObjectManagerEventTriggerList");
                    CloseWindow("ObjectManagerEventEffects");
                    CloseWindow("ObjectManagerEventEffect");
                    CloseWindow("ObjectManagerEventEffectList");
                    Assets.assets.abilityIconsSearch = Assets.assets.abilityIcons;
                    SpawnWindowBlueprint("ObjectManagerAbilityIconList");
                }
            });
            AddPaddingRegion(() => { AddLine("Events:", "DarkGray"); });
            foreach (var foo in ability.events)
            {
                AddButtonRegion(() =>
                {
                    AddLine("Event #" + (ability.events.IndexOf(foo) + 1));
                    AddSmallButton("OtherTrash",
                    (h) =>
                    {
                        eventEdit = null;
                        ability.events.Remove(foo);
                        CloseWindow("ObjectManagerEventTriggers");
                        CloseWindow("ObjectManagerEventEffects");
                        Respawn("ObjectManagerAbilities");
                    });
                },
                (h) =>
                {
                    eventEdit = foo;
                    eventParentType = "Ability";
                    var window = CDesktop.windows.Find(x => x.title.Contains("ObjectManagerEvent"));
                    if (window != null)
                    {
                        window.Respawn();
                    }
                    else if (window == null)
                    {
                        SpawnWindowBlueprint("ObjectManagerEventTriggers");
                        CloseWindow("ObjectManagerAbilities");
                        CloseWindow("ObjectManagerCostManager");
                    }
                });
            }
            AddButtonRegion(() =>
            {
                AddLine("Manage casting cost");
            },
            (h) =>
            {
                ability.cost ??= new();
                if (!ability.cost.ContainsKey("Fire")) ability.cost.Add("Fire", 0);
                if (!ability.cost.ContainsKey("Earth")) ability.cost.Add("Earth", 0);
                if (!ability.cost.ContainsKey("Water")) ability.cost.Add("Water", 0);
                if (!ability.cost.ContainsKey("Air")) ability.cost.Add("Air", 0);
                if (!ability.cost.ContainsKey("Frost")) ability.cost.Add("Frost", 0);
                if (!ability.cost.ContainsKey("Decay")) ability.cost.Add("Decay", 0);
                if (!ability.cost.ContainsKey("Shadow")) ability.cost.Add("Shadow", 0);
                if (!ability.cost.ContainsKey("Order")) ability.cost.Add("Order", 0);
                if (!ability.cost.ContainsKey("Arcane")) ability.cost.Add("Arcane", 0);
                if (!ability.cost.ContainsKey("Lightning")) ability.cost.Add("Lightning", 0);
                String.fire.Set(ability.cost["Fire"] + "");
                String.earth.Set(ability.cost["Earth"] + "");
                String.water.Set(ability.cost["Water"] + "");
                String.air.Set(ability.cost["Air"] + "");
                String.frost.Set(ability.cost["Frost"] + "");
                String.decay.Set(ability.cost["Decay"] + "");
                String.shadow.Set(ability.cost["Shadow"] + "");
                String.order.Set(ability.cost["Order"] + "");
                String.arcane.Set(ability.cost["Arcane"] + "");
                String.lightning.Set(ability.cost["Lightning"] + "");
                Respawn("ObjectManagerCostManager");
                CloseWindow("ObjectManagerAbilities");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Ability cooldown: ", "DarkGray");
                AddInputLine(String.cooldown);
            });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            if (ability.events.Count < 5)
                AddButtonRegion(() =>
                {
                    AddLine("Add new event");
                },
                (h) =>
                {
                    ability.events.Add(new Event()
                    {
                        triggers = new(),
                        effects = new()
                    });
                    Respawn(h.window.title);
                });
            else
                AddPaddingRegion(() =>
                {
                    AddLine("Add new event", "DarkGray");
                });

        }),
        new("BuffsSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort buffs:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("BuffsSort");
                    Respawn("ObjectManagerBuffs");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                buffs = buffs.OrderBy(x => x.name).ToList();
                buffsSearch = buffsSearch.OrderBy(x => x.name).ToList();
                CloseWindow("BuffsSort");
                Respawn("ObjectManagerBuffs");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By dispel type", "Black");
            },
            (h) =>
            {
                buffs = buffs.OrderByDescending(x => x.dispelType).ToList();
                buffsSearch = buffsSearch.OrderByDescending(x => x.dispelType).ToList();
                CloseWindow("BuffsSort");
                Respawn("ObjectManagerBuffs");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By stackable property", "Black");
            },
            (h) =>
            {
                buffs = buffs.OrderByDescending(x => x.stackable).ToList();
                buffsSearch = buffsSearch.OrderByDescending(x => x.stackable).ToList();
                CloseWindow("BuffsSort");
                Respawn("ObjectManagerBuffs");
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("ObjectManagerBuffs", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => buffsSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (buff != null)
            {
                var index = buffsSearch.IndexOf(buff);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            if (eventEdit != null)
            {
                var editingEffects = CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect");
                if (editingEffects)
                {
                    var index = buffsSearch.IndexOf(eventEdit.effects[selectedEffect].ContainsKey("BuffName") ? buffs.Find(x => x.name == eventEdit.effects[selectedEffect]["BuffName"]) : null);
                    if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
                }
                else
                {
                    var index = buffsSearch.IndexOf(eventEdit.triggers[selectedTrigger].ContainsKey("BuffName") ? buffs.Find(x => x.name == eventEdit.triggers[selectedTrigger]["BuffName"]) : null);
                    if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
                }
            }
            AddHeaderRegion(() =>
            {
                AddLine("Buffs:");
                AddSmallButton("OtherClose", (h) =>
                {
                    if (eventEdit != null)
                    {
                        CloseWindow(h.window);
                        var editingEffects = CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect");
                        if (editingEffects) Respawn("ObjectManagerEventEffects");
                        else Respawn("ObjectManagerEventTriggers");
                    }
                    else
                    {
                        buff = null; buffsSearch = null;
                        CloseDesktop("ObjectManagerBuffs");
                    }
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    buffs.Reverse();
                    buffsSearch.Reverse();
                    Respawn("ObjectManagerBuffs");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "BuffsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("BuffsSort");
                        Respawn("ObjectManagerBuffs");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (buffsSearch.Count > index + 10 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = buffsSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon);
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    if (eventEdit != null)
                    {
                        var editingEffects = CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect");
                        if (editingEffects)
                        {
                            if (eventEdit.effects[selectedEffect].ContainsKey("BuffName"))
                                eventEdit.effects[selectedEffect]["BuffName"] = buffsSearch[index + 10 * regionGroup.pagination()].name;
                            else eventEdit.effects[selectedEffect].Add("BuffName", buffsSearch[index + 10 * regionGroup.pagination()].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventEffects");
                            Respawn("ObjectManagerEventEffect");
                        }
                        else
                        {
                            if (eventEdit.triggers[selectedTrigger].ContainsKey("BuffName"))
                                eventEdit.triggers[selectedTrigger]["BuffName"] = buffsSearch[index + 10 * regionGroup.pagination()].name;
                            else eventEdit.triggers[selectedTrigger].Add("BuffName", buffsSearch[index + 10 * regionGroup.pagination()].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventTriggers");
                            Respawn("ObjectManagerEventTrigger");
                        }
                    }
                    else
                    {
                        buff = buffsSearch[index + 10 * regionGroup.pagination()];
                        String.objectName.Set(buff.name);
                        Respawn("ObjectManagerBuff");
                    }
                },
                null,
                (h) => () =>
                {
                    SetAnchor(Center);
                    PrintBuffTooltip(null, null, (buffsSearch[index + 10 * regionGroup.pagination()], 0, null, 0));
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(buffs.Count + " buffs", "DarkGray");
                if (buffs.Count != buffsSearch.Count)
                    AddLine(buffsSearch.Count + " found in search", "DarkGray");
            });
            if (eventEdit != null && ability != null)
                AddButtonRegion(() =>
                {
                    AddLine("Copy ability into a new buff");
                },
                (h) =>
                {
                    var buff = new Buff()
                    {
                        name = ability.name,
                        icon = ability.icon,
                        events = new(),
                        tags = new()
                    };
                    buffs.Add(buff);
                    buffsSearch = buffs.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
                    CloseWindow(h.window);
                    var editingEffects = CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect");
                    if (editingEffects)
                    {
                        if (eventEdit.effects[selectedEffect].ContainsKey("BuffName"))
                            eventEdit.effects[selectedEffect]["BuffName"] = buff.name;
                        else eventEdit.effects[selectedEffect].Add("BuffName", buff.name);
                        Respawn("ObjectManagerEventEffects");
                        Respawn("ObjectManagerEventEffect");
                    }
                    else
                    {
                        if (eventEdit.triggers[selectedTrigger].ContainsKey("BuffName"))
                            eventEdit.triggers[selectedTrigger]["BuffName"] = buff.name;
                        else eventEdit.triggers[selectedTrigger].Add("BuffName", buff.name);
                        Respawn("ObjectManagerEventTriggers");
                        Respawn("ObjectManagerEventTrigger");
                    }
                });
            AddButtonRegion(() =>
            {
                AddLine("Create a new buff");
            },
            (h) =>
            {
                if (eventEdit != null)
                {
                    var buff = new Buff()
                    {
                        name = "Buff #" + buffs.Count,
                        icon = "AbilityAbolishMagic",
                        events = new(),
                        tags = new()
                    };
                    buffs.Add(buff);
                    buffsSearch = buffs.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
                    }
                else
                {
                    buff = new Buff()
                    {
                        name = "Buff #" + buffs.Count,
                        icon = "AbilityAbolishMagic",
                        events = new(),
                        tags = new()
                    };
                    buffs.Add(buff);
                    buffsSearch = buffs.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
                    String.objectName.Set(buff.name);
                    Respawn("ObjectManagerBuff");
                    }
            });
        }),
        new("ObjectManagerBuff", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() =>
            {
                AddLine("Buff:", "DarkGray");
                AddSmallButton("OtherTrash", (h) =>
                {
                    buffs.Remove(buff);
                    CloseWindow(h.window);
                    Respawn("ObjectManagerBuffs");
                });
            });
            AddPaddingRegion(() => { AddInputLine(String.objectName); });
            AddPaddingRegion(() => { AddLine("Icon:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(buff.icon.Substring(7) + ".png");
                AddSmallButton(buff.icon);
            },
            (h) =>
            {
                if (CloseWindow("ObjectManagerBuffs"))
                {
                    Assets.assets.abilityIconsSearch = Assets.assets.abilityIcons;
                    SpawnWindowBlueprint("ObjectManagerAbilityIconList");
                }
            });
            AddPaddingRegion(() => { AddLine("Stackable:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(buff.stackable ? "Yes" : "No");
            },
            (h) =>
            {
                buff.stackable ^= true;
            });
            AddPaddingRegion(() => { AddLine("Events:", "DarkGray"); });
            foreach (var foo in buff.events)
            {
                AddButtonRegion(() =>
                {
                    AddLine("Event #" + (buff.events.IndexOf(foo) + 1));
                },
                (h) =>
                {
                    eventEdit = foo;
                    eventParentType = "Buff";
                    var window = CDesktop.windows.Find(x => x.title.Contains("ObjectManagerEvent"));
                    if (window != null)
                    {
                        window.Respawn();
                    }
                    else if (window == null)
                    {
                        SpawnWindowBlueprint("ObjectManagerEventTriggers");
                        CloseWindow("ObjectManagerBuffs");
                    }
                });
            }
            AddPaddingRegion(() => SetRegionAsGroupExtender());
            if (buff.events.Count < 5)
                AddButtonRegion(() =>
                {
                    AddLine("Add new event");
                },
                (h) =>
                {
                    buff.events.Add(new Event()
                    {
                        triggers = new(),
                        effects = new()
                    });
                    Respawn(h.window.title);
                });
            else
                AddPaddingRegion(() =>
                {
                    AddLine("Add new event", "DarkGray");
                });
        }),
        new("RacesSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort races:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("RacesSort");
                    Respawn("ObjectManagerRaces");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                races = races.OrderBy(x => x.name).ToList();
                racesSearch = racesSearch.OrderBy(x => x.name).ToList();
                CloseWindow("RacesSort");
                Respawn("ObjectManagerRaces");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By portrait", "Black");
            },
            (h) =>
            {
                races = races.OrderBy(x => x.portrait).ToList();
                racesSearch = racesSearch.OrderBy(x => x.portrait).ToList();
                CloseWindow("RacesSort");
                Respawn("ObjectManagerRaces");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By faction", "Black");
            },
            (h) =>
            {
                races = races.OrderByDescending(x => x.faction).ToList();
                racesSearch = racesSearch.OrderByDescending(x => x.faction).ToList();
                CloseWindow("RacesSort");
                Respawn("ObjectManagerRaces");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By kind", "Black");
            },
            (h) =>
            {
                races = races.OrderBy(x => x.kind == "Elite" ? 0 : (x.kind == "Rare" ? 1 : 2)).ToList();
                racesSearch = racesSearch.OrderBy(x => x.kind == "Elite" ? 0 : (x.kind == "Rare" ? 1 : 2)).ToList();
                CloseWindow("RacesSort");
                Respawn("ObjectManagerRaces");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By vitality", "Black");
            },
            (h) =>
            {
                races = races.OrderByDescending(x => x.vitality).ToList();
                racesSearch = racesSearch.OrderByDescending(x => x.vitality).ToList();
                CloseWindow("RacesSort");
                Respawn("ObjectManagerRaces");
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("ObjectManagerRaces", () => {
            racesSearch ??= races;
            SetAnchor(TopLeft);
            AddRegionGroup(() => racesSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (race != null)
            {
                var index = racesSearch.IndexOf(race);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Races:");
                AddSmallButton("OtherClose", (h) =>
                {
                    if (CDesktop.title == "ObjectManagerRaces")
                    {
                        race = null; racesSearch = null;
                        CloseDesktop("ObjectManagerRaces");
                    }
                    else
                    {
                        Encounter.encounter = null;
                        CloseWindow(h.window);
                        SpawnWindowBlueprint("ObjectManagerHostileAreas");
                    }
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    races.Reverse();
                    racesSearch.Reverse();
                    Respawn("ObjectManagerRaces");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "RacesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("RacesSort");
                        Respawn("ObjectManagerRaces");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (racesSearch.Count > index + 10 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = racesSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name);
                        AddSmallButton(foo.portrait + (foo.genderedPortrait ? "Female" : ""));
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    if (CDesktop.title == "ObjectManagerHostileAreas")
                    {
                        if (Encounter.encounter != null) Encounter.encounter.who = racesSearch[index + 10 * regionGroup.pagination()].name;
                        else
                        {
                            var enc = new Encounter() { who = racesSearch[index + 10 * regionGroup.pagination()].name, levelMin = 1, levelMax = 0 };
                            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerHostileAreaCommonEncounters"))
                                area.commonEncounters.Add(enc);
                            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerHostileAreaRareEncounters"))
                                area.rareEncounters.Add(enc);
                            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerHostileAreaEliteEncounters"))
                                area.eliteEncounters.Add(enc);
                        }
                        CloseWindow(h.window);
                        CDesktop.RespawnAll();
                        SpawnWindowBlueprint("ObjectManagerHostileAreas");
                    }
                    else
                    {
                        race = racesSearch[index + 10 * regionGroup.pagination()];
                        String.objectName.Set(race.name);
                        String.vitality.Set(race.vitality + "");
                        Respawn("ObjectManagerRace");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(races.Count + " races", "DarkGray");
                if (races.Count != racesSearch.Count)
                    AddLine(racesSearch.Count + " found in search", "DarkGray");
            });
            AddButtonRegion(() =>
            {
                AddLine("Create a new race");
            },
            (h) =>
            {
                race = new Race()
                {
                    name = "Race #" + races.Count,
                    abilities = new(),
                    kind = "Common",
                    portrait = "PortraitChicken",
                    vitality = 1.0,
                };
                races.Add(race);
                racesSearch = races.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
                String.objectName.Set(race.name);
                String.vitality.Set(race.vitality + "");
                Respawn("ObjectManagerRace");
                });
        }),
        new("ObjectManagerRace", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() =>
            {
                AddLine("Race:", "DarkGray");
                AddSmallButton("OtherTrash", (h) =>
                {
                    races.Remove(race);
                    CloseWindow(h.window);
                    Respawn("ObjectManagerRaces");
                });
            });
            AddPaddingRegion(() => { AddInputLine(String.objectName); });
            AddPaddingRegion(() => { AddLine("Gendered portraits:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(race.genderedPortrait ? "True" : "False");
            },
            (h) =>
            {
                race.genderedPortrait ^= true;
                });
            if (race.genderedPortrait)
            {
                AddPaddingRegion(() => { AddLine("Portraits:", "DarkGray"); });
                AddHeaderRegion(() =>
                {
                    AddLine(race.portrait.Substring(8) + ".png");
                    AddSmallButton(race.portrait + "Female");
                    AddSmallButton(race.portrait + "Male");
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Faction:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        race.faction = null;
                        Respawn("ObjectManagerRace");
                    });
                });
                AddButtonRegion(() =>
                {
                    AddLine(race.faction ?? "None");
                    if (race.faction != null)
                        AddSmallButton(race.Faction().Icon());
                },
                (h) =>
                {
                    if (CloseWindow("ObjectManagerRaces") || CloseWindow("ObjectManagerPortraitList"))
                    {
                        factionsSearch = factions;
                        SpawnWindowBlueprint("ObjectManagerFactions");
                    }
                });
            }
            else
            {
                AddPaddingRegion(() => { AddLine("Portrait:", "DarkGray"); });
                AddButtonRegion(() =>
                {
                    AddLine(race.portrait.Replace("Portrait", "") + ".png");
                    AddSmallButton(race.portrait);
                },
                (h) =>
                {
                    if (CloseWindow("ObjectManagerRaces") || CloseWindow("ObjectManagerFactions"))
                    {
                        Assets.assets.portraitsSearch = Assets.assets.portraits;
                        SpawnWindowBlueprint("ObjectManagerPortraitList");
                    }
                });
                AddPaddingRegion(() => { AddLine("Kind:", "DarkGray"); });
                AddButtonRegion(() =>
                {
                    AddLine(race.kind);
                },
                (h) =>
                {
                    if (race.kind == "Common")
                        race.kind = "Rare";
                    else if (race.kind == "Rare")
                        race.kind = "Elite";
                    else if (race.kind == "Elite")
                        race.kind = "Common";
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Faction:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        race.faction = null;
                        Respawn("ObjectManagerRace");
                    });
                });
                AddButtonRegion(() =>
                {
                    AddLine(race.faction ?? "None");
                    if (race.faction != null)
                        AddSmallButton(race.Faction().Icon());
                },
                (h) =>
                {
                    if (CloseWindow("ObjectManagerRaces") || CloseWindow("ObjectManagerPortraitList"))
                    {
                        factionsSearch = factions;
                        SpawnWindowBlueprint("ObjectManagerFactions");
                    }
                });
                AddPaddingRegion(() => { AddLine("Vitality:", "DarkGray"); });
                AddPaddingRegion(() =>
                {
                    AddInputLine(String.vitality);
                });
            }
            AddPaddingRegion(() => { });
        }),
        new("MountsSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort mounts:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("MountsSort");
                    Respawn("ObjectManagerMounts");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                mounts = mounts.OrderBy(x => x.name).ToList();
                mountsSearch = mountsSearch.OrderBy(x => x.name).ToList();
                CloseWindow("MountsSort");
                Respawn("ObjectManagerMounts");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By speed", "Black");
            },
            (h) =>
            {
                mounts = mounts.OrderByDescending(x => x.speed).ToList();
                mountsSearch = mountsSearch.OrderByDescending(x => x.speed).ToList();
                CloseWindow("MountsSort");
                Respawn("ObjectManagerMounts");
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("ObjectManagerMounts", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => mountsSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (mount != null)
            {
                var index = mountsSearch.IndexOf(mount);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Mounts:");
                AddSmallButton("OtherClose", (h) =>
                {
                    mount = null; mountsSearch = null;
                    CloseDesktop("ObjectManagerMounts");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    mounts.Reverse();
                    mountsSearch.Reverse();
                    Respawn("ObjectManagerMounts");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "MountsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("MountsSort");
                        Respawn("ObjectManagerMounts");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (mountsSearch.Count > index + 10 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = mountsSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name, foo.speed == 7 ? "Rare" : "Epic");
                        AddSmallButton(foo.icon);
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    mount = mountsSearch[index + 10 * regionGroup.pagination()];
                    String.objectName.Set(mount.name);
                    Respawn("ObjectManagerMount");
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(mounts.Count + " mounts", "DarkGray");
                if (mounts.Count != mountsSearch.Count)
                    AddLine(mountsSearch.Count + " found in search", "DarkGray");
            });
            AddButtonRegion(() =>
            {
                AddLine("Create a new mount");
            },
            (h) =>
            {
                mount = new Mount()
                {
                    name = "Mount #" + mounts.Count,
                    icon = "None",
                    speed = 7
                };
                mounts.Add(mount);
                mountsSearch = mounts.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
                String.objectName.Set(mount.name);
                String.price.Set(mount.price + "");
                String.mountSpeed.Set(mount.speed + "");
                Respawn("ObjectManagerMount");
                });
        }),
        new("ObjectManagerMount", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Mount:", "DarkGray"); });
            AddPaddingRegion(() => { AddInputLine(String.objectName); });
            AddPaddingRegion(() => { AddLine("Icon:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(mount.icon.Replace("Mount", "") + ".png");
                AddSmallButton(mount.icon);
            },
            (h) =>
            {
                var list = CDesktop.windows.Find(x => x.title == "ObjectManagerMountIconList");
                if (list == null)
                {
                    CloseWindow("ObjectManagerMounts");
                    Assets.assets.mountIconsSearch = Assets.assets.mountIcons;
                    list = SpawnWindowBlueprint("ObjectManagerMountIconList");
                }
            });
            AddPaddingRegion(() =>
            {
                AddLine("Price:", "DarkGray");
                AddInputLine(String.price);
            });
            AddPaddingRegion(() => { AddLine("Speed:", "DarkGray"); });
            AddPaddingRegion(() => { AddInputLine(String.mountSpeed); });
            AddPaddingRegion(() => { });
        }),
        new("RecipesSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort recipes:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("RecipesSort");
                    Respawn("ObjectManagerRecipes");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                recipes = recipes.OrderBy(x => x.name).ToList();
                recipesSearch = recipesSearch.OrderBy(x => x.name).ToList();
                CloseWindow("RecipesSort");
                Respawn("ObjectManagerRecipes");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By profession", "Black");
            },
            (h) =>
            {
                recipes = recipes.OrderBy(x => x.profession).ToList();
                recipesSearch = recipesSearch.OrderBy(x => x.profession).ToList();
                CloseWindow("RecipesSort");
                Respawn("ObjectManagerRecipes");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By skill required", "Black");
            },
            (h) =>
            {
                recipes = recipes.OrderByDescending(x => x.learnedAt).ToList();
                recipesSearch = recipesSearch.OrderByDescending(x => x.learnedAt).ToList();
                CloseWindow("RecipesSort");
                Respawn("ObjectManagerRecipes");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By reagents required", "Black");
            },
            (h) =>
            {
                recipes = recipes.OrderByDescending(x => x.reagents.Count == 0 ? 0 : x.reagents.Sum(y => y.Value)).ToList();
                recipesSearch = recipesSearch.OrderByDescending(x => x.reagents.Count == 0 ? 0 : x.reagents.Sum(y => y.Value)).ToList();
                CloseWindow("RecipesSort");
                Respawn("ObjectManagerRecipes");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By training cost", "Black");
            },
            (h) =>
            {
                recipes = recipes.OrderByDescending(x => x.price).ToList();
                recipesSearch = recipesSearch.OrderByDescending(x => x.price).ToList();
                CloseWindow("RecipesSort");
                Respawn("ObjectManagerRecipes");
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("ObjectManagerRecipes", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => recipesSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (recipe != null)
            {
                var index = recipesSearch.IndexOf(recipe);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Recipes:");
                AddSmallButton("OtherClose", (h) =>
                {
                    recipe = null; recipesSearch = null;
                    CloseDesktop("ObjectManagerRecipes");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    recipes.Reverse();
                    recipesSearch.Reverse();
                    Respawn("ObjectManagerRecipes");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "RecipesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("RecipesSort");
                        Respawn("ObjectManagerRecipes");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (recipesSearch.Count > index + 10 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = recipesSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name);
                        AddSmallButton(foo.Icon());
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    recipe = recipesSearch[index + 10 * regionGroup.pagination()];
                    String.objectName.Set(recipe.name);
                    Respawn("ObjectManagerRecipe");
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(recipes.Count + " recipes", "DarkGray");
                if (recipes.Count != recipesSearch.Count)
                    AddLine(recipesSearch.Count + " found in search", "DarkGray");
            });
            AddButtonRegion(() =>
            {
                AddLine("Create a new recipe");
            },
            (h) =>
            {
                recipe = new Recipe()
                {
                    name = "Recipe #" + recipes.Count
                };
                recipes.Add(recipe);
                recipesSearch = recipes.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
                String.objectName.Set(recipe.name);
                Respawn("ObjectManagerRecipe");
                });
        }),
        new("ObjectManagerRecipe", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Recipe:", "DarkGray"); });
            AddPaddingRegion(() => { AddInputLine(String.objectName); });
            AddPaddingRegion(() => { });
        }),
        new("FactionsSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort factions:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("FactionsSort");
                    Respawn("ObjectManagerFactions");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                factions = factions.OrderBy(x => x.name).ToList();
                factionsSearch = factionsSearch.OrderBy(x => x.name).ToList();
                CloseWindow("FactionsSort");
                Respawn("ObjectManagerFactions");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By side", "Black");
            },
            (h) =>
            {
                factions = factions.OrderByDescending(x => x.side).ToList();
                factionsSearch = factionsSearch.OrderByDescending(x => x.side).ToList();
                CloseWindow("FactionsSort");
                Respawn("ObjectManagerFactions");
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("ObjectManagerFactions", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => factionsSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (faction != null)
            {
                var index = factionsSearch.IndexOf(faction);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            else if (town != null)
            {
                var index = factionsSearch.FindIndex(x => x.name == town.faction);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            else if (race != null)
            {
                var index = factionsSearch.FindIndex(x => x.name == race.faction);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Factions:");
                AddSmallButton("OtherClose", (h) =>
                {
                    if (town != null)
                    {
                        CloseWindow(h.window);
                        SpawnWindowBlueprint("ObjectManagerTowns");
                    }
                    else if (race != null)
                    {
                        CloseWindow(h.window);
                        SpawnWindowBlueprint("ObjectManagerRaces");
                    }
                    else
                    {
                        faction = null; factionsSearch = null;
                        CloseDesktop("ObjectManagerFactions");
                    }
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    factions.Reverse();
                    factionsSearch.Reverse();
                    Respawn("ObjectManagerFactions");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "FactionsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("FactionsSort");
                        Respawn("ObjectManagerFactions");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (factionsSearch.Count > index + 10 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = factionsSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name);
                        AddSmallButton(foo.Icon());
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    if (town != null)
                    {
                        town.faction = factionsSearch[index + 10 * regionGroup.pagination()].name;
                        CloseWindow(h.window);
                        Respawn("ObjectManagerTown");
                        Respawn("ObjectManagerTowns");
                    }
                    else if (race != null)
                    {
                        race.faction = factionsSearch[index + 10 * regionGroup.pagination()].name;
                        CloseWindow(h.window);
                        Respawn("ObjectManagerRace");
                        Respawn("ObjectManagerRaces");
                    }
                    else
                    {
                        faction = factionsSearch[index + 10 * regionGroup.pagination()];
                        String.objectName.Set(faction.name);
                        Respawn("ObjectManagerFaction");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(factions.Count + " factions", "DarkGray");
                if (factions.Count != factionsSearch.Count)
                    AddLine(factionsSearch.Count + " found in search", "DarkGray");
            });
            AddButtonRegion(() =>
            {
                AddLine("Create a new faction");
            },
            (h) =>
            {
                faction = new Faction()
                {
                    name = "Faction #" + factions.Count,
                    icon = "None",
                    side = "Neutral"
                };
                factions.Add(faction);
                factionsSearch = factions.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
                String.objectName.Set(faction.name);
                Respawn("ObjectManagerFaction");
                });
        }),
        new("ObjectManagerFaction", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Faction:", "DarkGray"); });
            AddPaddingRegion(() => { AddInputLine(String.objectName); });
            AddPaddingRegion(() => { AddLine("Icon:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(faction.icon.Replace("Faction", "") + ".png");
                AddSmallButton(faction.Icon());
            },
            (h) =>
            {
                var list = CDesktop.windows.Find(x => x.title == "ObjectManagerFactionIconList");
                if (list == null)
                {
                    CloseWindow("ObjectManagerFactions");
                    Assets.assets.factionIconsSearch = Assets.assets.factionIcons;
                    list = SpawnWindowBlueprint("ObjectManagerFactionIconList");
                }
            });
            AddPaddingRegion(() => { AddLine("Side:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(faction.side);
            },
            (h) =>
            {
                if (faction.side == "Hostile")
                    faction.side = "Neutral";
                else if (faction.side == "Neutral")
                    faction.side = "Alliance";
                else if (faction.side == "Alliance")
                    faction.side = "Horde";
                else if (faction.side == "Horde")
                    faction.side = "Hostile";
            });
            AddPaddingRegion(() => { });
        }),
        new("ObjectManagerSpecs", () => {
            SetAnchor(TopLeft);
            AddRegionGroup(() => specsSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (spec != null)
            {
                var index = specs.IndexOf(spec);
                if (index >= 10)
                    CDesktop.LBWindow.LBRegionGroup.SetPagination(index / 10);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Specs:");
                AddSmallButton("OtherClose", (h) =>
                {
                    spec = null; specsSearch = null;
                    CloseDesktop("ObjectManagerSpecs");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    specs.Reverse();
                    specsSearch.Reverse();
                    Respawn("ObjectManagerSpecs");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (specsSearch.Count > index + 10 * regionGroup.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = specsSearch[index + 10 * regionGroup.pagination()];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon);
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    spec = specsSearch[index + 10 * regionGroup.pagination()];
                    String.objectName.Set(spec.name);
                    Respawn("ObjectManagerSpec");
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(specs.Count + " specs", "DarkGray");
                if (specs.Count != specsSearch.Count)
                    AddLine(specsSearch.Count + " found in search", "DarkGray");
            });
            //AddButtonRegion(() =>
            //{
            //    AddLine("Create a new class");
            //},
            //(h) =>
            //{
            //    spec = new Spec()
            //    {
            //        name = "Spec #" + specs.Count,
            //        icon = "PortraitChicken",
            //        startingEquipment = new(),
            //        abilities = new(),
            //        rules = new()
            //        {
            //            { "Melee Attack Power per Strength", 2.0 },
            //            { "Ranged Attack Power per Strength", 0.0 },
            //            { "Critical Strike per Strength", 0.0 },
            //            { "Melee Attack Power per Agility", 2.0 },
            //            { "Ranged Attack Power per Agility", 0.0 },
            //            { "Critical Strike per Agility", 0.03 },
            //            { "Spell Power per Intellect", 1.0 },
            //            { "Spell Critical per Intellect", 0.03 },
            //            { "Stamina per Level", 3.0 },
            //            { "Strength per Level", 3.0 },
            //            { "Agility per Level", 3.0 },
            //            { "Intellect per Level", 0.0 }
            //        },
            //        talentTrees = new()
            //    };
            //    specs.Add(spec);
            //    specsSearch = specs.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
            //    String.objectName.Set(spec.name);
            //    CloseWindow("ObjectManagerSpec");
            //    SpawnWindowBlueprint("ObjectManagerSpec");
            //    h.window.Rebuild();
            //});
        }),
        new("ObjectManagerSpec", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Spec:", "DarkGray"); });
            AddPaddingRegion(() =>
            {
                AddInputLine(String.objectName, spec.name);
            });
            AddPaddingRegion(() => { AddLine("Icon:", "DarkGray"); });
            AddHeaderRegion(() =>
            {
                AddLine(spec.icon + ".png");
                AddSmallButton(spec.icon);
            });
            //AddButtonRegion(() =>
            //{
            //    AddLine(spec.icon + ".png");
            //    AddSmallButton(spec.icon);
            //},
            //(h) =>
            //{
            //    if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerIconList"))
            //    {
            //        CloseWindow("ObjectManagerSpecs");
            //        SpawnWindowBlueprint("ObjectManagerIconList");
            //    }
            //});
            AddPaddingRegion(() => { });
        }),
    };

    public static List<Blueprint> desktopBlueprints = new()
    {
        new("DevPanel", () =>
        {
            #if (!UNITY_EDITOR)

            Serialize(races, "races", true, false, prefix);
            Serialize(specs, "specs", true, false, prefix);
            Serialize(abilities, "abilities", true, false, prefix);
            Serialize(buffs, "buffs", true, false, prefix);
            Serialize(areas, "areas", true, false, prefix);
            Serialize(instances, "instances", true, false, prefix);
            Serialize(complexes, "complexes", true, false, prefix);
            Serialize(towns, "towns", true, false, prefix);
            Serialize(items, "items", true, false, prefix);
            Serialize(itemSets, "sets", true, false, prefix);
            Serialize(mounts, "mounts", true, false, prefix);
            Serialize(generalDrops, "generaldrops", true, false, prefix);
            Serialize(recipes, "recipes", true, false, prefix);
            Serialize(factions, "factions", true, false, prefix);
            Serialize(quests, "quests", true, false, prefix);
            Serialize(spiritHealers, "spirithealers", true, false, prefix);
            Serialize(personTypes, "personTypes", true, false, prefix);
            Serialize(enchants, "enchants", true, false, prefix);
            Serialize(zones, "zones", true, false, prefix);
            Serialize(professions, "professions", true, false, prefix);
            Serialize(worldAbilities, "worldabilities", true, false, prefix);
            Serialize(pEnchants, "permanentenchants", true, false, prefix);
            Serialize(pvpRanks, "pvpranks", true, false, prefix);
            Serialize(personCategories, "personcategories", true, false, prefix);
            Serialize(FlightPathGroup.flightPathGroups, "flightpaths", true, false, prefix);

            #endif

            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerLobby");
            AddHotkey(Escape, () => { CloseDesktop("DevPanel"); });
        }),
        new("GameSimulation", () =>
        {
            locationName = board.area.name;
            PlaySound("DesktopEnterCombat");
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("BoardFrame");
            SpawnWindowBlueprint("Board");
            SpawnWindowBlueprint("BufferBoard");
            SpawnWindowBlueprint("PlayerBattleInfo");
            SpawnWindowBlueprint("LocationInfo");
            SpawnWindowBlueprint("EnemyBattleInfo");
            var elements = new List<string> { "Fire", "Water", "Earth", "Air", "Frost", "Lightning", "Arcane", "Decay", "Order", "Shadow" };
            foreach (var element in elements)
            {
                SpawnWindowBlueprint("Player" + element + "Resource");
                SpawnWindowBlueprint("Enemy" + element + "Resource");
            }
            board.Reset();
        }),
        new("ObjectManagerHostileAreas", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerHostileAreas");
            AddHotkey(Escape, () => { area = null; areasSearch = null; Encounter.encounter = null; CloseDesktop("ObjectManagerHostileAreas"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerInstances", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerInstances");
            AddHotkey(Escape, () => { instance = null; instancesSearch = null; CloseDesktop("ObjectManagerInstances"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerComplexes", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerComplexes");
            AddHotkey(Escape, () => { complex = null; complexesSearch = null; CloseDesktop("ObjectManagerComplexes"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerTowns", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerTowns");
            AddHotkey(Escape, () => { town = null; townsSearch = null; CloseDesktop("ObjectManagerTowns"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerRaces", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerRaces");
            AddHotkey(Escape, () => { race = null; racesSearch = null; CloseDesktop("ObjectManagerRaces"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerSpecs", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerSpecs");
            AddHotkey(Escape, () => { spec = null; specsSearch = null; CloseDesktop("ObjectManagerSpecs"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerAbilities", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerAbilities");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("ObjectManagerCostManager"))
                {
                    PlaySound("DesktopButtonClose");
                    if (ability.cost["Fire"] == 0) ability.cost.Remove("Fire");
                    if (ability.cost["Earth"] == 0) ability.cost.Remove("Earth");
                    if (ability.cost["Water"] == 0) ability.cost.Remove("Water");
                    if (ability.cost["Air"] == 0) ability.cost.Remove("Air");
                    if (ability.cost["Frost"] == 0) ability.cost.Remove("Frost");
                    if (ability.cost["Decay"] == 0) ability.cost.Remove("Decay");
                    if (ability.cost["Shadow"] == 0) ability.cost.Remove("Shadow");
                    if (ability.cost["Order"] == 0) ability.cost.Remove("Order");
                    if (ability.cost["Arcane"] == 0) ability.cost.Remove("Arcane");
                    if (ability.cost["Lightning"] == 0) ability.cost.Remove("Lightning");
                    if (ability.cost.Count == 0) ability.cost = null;
                    SpawnWindowBlueprint("ObjectManagerAbilities");
                }
                else
                {
                    ability = null;
                    abilitiesSearch = null;
                    eventEdit = null;
                    CloseDesktop("ObjectManagerAbilities");
                }
            });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerBuffs", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerBuffs");
            AddHotkey(Escape, () => { buff = null; buffsSearch = null; CloseDesktop("ObjectManagerBuffs"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerItems", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerItems");
            AddHotkey(Escape, () => { item = null; itemsSearch = null; CloseDesktop("ObjectManagerItems"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerItemSets", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerItemSets");
            AddHotkey(Escape, () => { itemSet = null; itemSetsSearch = null; CloseDesktop("ObjectManagerItemSets"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerMounts", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerMounts");
            AddHotkey(Escape, () => { mount = null; mountsSearch = null; CloseDesktop("ObjectManagerMounts"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerRecipes", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerRecipes");
            AddHotkey(Escape, () => { recipe = null; recipesSearch = null; CloseDesktop("ObjectManagerRecipes"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerClothTypes", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerClothTypes");
            AddHotkey(Escape, () => { generalDrop = null; generalDropsSearch = null; CloseDesktop("ObjectManagerClothTypes"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerFactions", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerFactions");
            AddHotkey(Escape, () => { faction = null; factionsSearch = null; CloseDesktop("ObjectManagerFactions"); });
            AddPaginationHotkeys();
        }),
    };
}
