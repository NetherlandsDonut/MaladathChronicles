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
using static Auctionable;
using static FishingSpot;
using static GeneralDrop;
using static Serialization;
using static PersonCategory;
using static PermanentEnchant;
using static SiteSpiritHealer;
using static SiteInstance;
using static SiteComplex;
using static SiteArea;

public static class BlueprintDev
{
    public static List<Blueprint> windowBlueprints = new()
    {
        new("ObjectManagerLobby", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddHeaderRegion(() =>
            {
                AddLine("Object types:");
                AddSmallButton("OtherClose", (h) => CloseDesktop("DevPanel"));
            });
            AddButtonRegion(() => AddLine("Races"), (h) =>
            {
                racesSearch = races;
                SpawnDesktopBlueprint("ObjectManagerRaces");
            });
            AddButtonRegion(() => AddLine("Abilities"), (h) =>
            {
                abilitiesSearch = abilities;
                SpawnDesktopBlueprint("ObjectManagerAbilities");
            });
            AddButtonRegion(() => AddLine("Buffs"), (h) =>
            {
                buffsSearch = buffs;
                SpawnDesktopBlueprint("ObjectManagerBuffs");
            });
            AddButtonRegion(() => AddLine("Items"), (h) =>
            {
                itemsSearch = items;
                SpawnDesktopBlueprint("ObjectManagerItems");
            });
            AddButtonRegion(() => AddLine("Item sets"), (h) =>
            {
                itemSetsSearch = itemSets;
                SpawnDesktopBlueprint("ObjectManagerItemSets");
            });
            AddButtonRegion(() => AddLine("Mounts"), (h) =>
            {
                mountsSearch = mounts;
                SpawnDesktopBlueprint("ObjectManagerMounts");
            });
            AddButtonRegion(() => AddLine("Recipes"), (h) =>
            {
                recipesSearch = recipes;
                SpawnDesktopBlueprint("ObjectManagerRecipes");
            });
            AddButtonRegion(() => AddLine("General drops"), (h) =>
            {
                generalDropsSearch = generalDrops;
                SpawnDesktopBlueprint("ObjectManagerGeneralDrops");
            });
            AddButtonRegion(() => AddLine("Factions"), (h) =>
            {
                factionsSearch = factions;
                SpawnDesktopBlueprint("ObjectManagerFactions");
            });
            AddPaddingRegion(() => AddLine("Actions:"));
            AddButtonRegion(() => AddLine("Save data"), (h) =>
            {
                Serialize(races, "races", false, false, prefix);
                Serialize(specs, "Specs", false, false, prefix);
                Serialize(abilities, "abilities", false, false, prefix);
                Serialize(buffs, "buffs", false, false, prefix);
                Serialize(instances, "instances", false, false, prefix);
                Serialize(complexes, "complexes", false, false, prefix);
                Serialize(areas, "areas", false, false, prefix);
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
                Serialize(auctionables, "auctionables", false, false, prefix);
                Serialize(fishingSpots, "fishingspots", false, false, prefix);
                Serialize(defines, "defines", false, false, prefix);
            });
            AddPaddingRegion(() => { });
        }),
        new("ObjectManagerSoundsList", () => {
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = Assets.assets.soundsSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            //if (eventEdit != null)
            //{
            //    var temp = eventEdit.effects[selectedEffect];
            //    var index = temp.ContainsKey("SoundEffect") && temp["SoundEffect"] != "None" ? Assets.assets.soundsSearch.IndexOf(temp["SoundEffect"] + ".ogg") : 0;
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
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
            AddPaginationLine();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.soundsSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.soundsSearch[index + thisWindow.pagination()];
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
                    var foo = Assets.assets.soundsSearch[index + thisWindow.pagination()];
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = Assets.assets.itemIconsSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            //if (item != null)
            //{
            //    var index = Assets.assets.itemIconsSearch.IndexOf(item.icon + ".png");
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
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
            AddPaginationLine();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.itemIconsSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.itemIconsSearch[index + thisWindow.pagination()];
                        AddLine(foo.Substring(4));
                        AddSmallButton(Assets.assets.itemIconsSearch[index + thisWindow.pagination()].Replace(".png", ""));
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.itemIconsSearch[index + thisWindow.pagination()];
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = Assets.assets.abilityIconsSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            //if (ability != null)
            //{
            //    var index = Assets.assets.abilityIconsSearch.IndexOf(ability.icon + ".png");
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
            //else if (buff != null)
            //{
            //    var index = Assets.assets.abilityIconsSearch.IndexOf(buff.icon + ".png");
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
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
            AddPaginationLine();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.abilityIconsSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.abilityIconsSearch[index + thisWindow.pagination()];
                        AddLine(foo.Substring(7));
                        AddSmallButton(Assets.assets.abilityIconsSearch[index + thisWindow.pagination()].Replace(".png", ""));
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.abilityIconsSearch[index + thisWindow.pagination()];
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = Assets.assets.factionIconsSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            //if (faction != null)
            //{
            //    var index = Assets.assets.factionIconsSearch.IndexOf(faction.icon + ".png");
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
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
            AddPaginationLine();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.factionIconsSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.factionIconsSearch[index + thisWindow.pagination()];
                        AddLine(foo.Substring(7));
                        AddSmallButton(Assets.assets.factionIconsSearch[index + thisWindow.pagination()].Replace(".png", ""));
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.factionIconsSearch[index + thisWindow.pagination()];
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = Assets.assets.mountIconsSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            //if (mount != null)
            //{
            //    var index = Assets.assets.mountIconsSearch.IndexOf(mount.icon + ".png");
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
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
            AddPaginationLine();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.mountIconsSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.mountIconsSearch[index + thisWindow.pagination()];
                        AddLine(foo.Substring(5));
                        AddSmallButton(Assets.assets.mountIconsSearch[index + thisWindow.pagination()].Replace(".png", ""));
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.mountIconsSearch[index + thisWindow.pagination()];
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = Assets.assets.portraitsSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            //if (race != null)
            //{
            //    var index = Assets.assets.portraitsSearch.IndexOf(race.portrait + ".png");
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
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
            AddPaginationLine();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.portraitsSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.portraitsSearch[index + thisWindow.pagination()];
                        AddLine(foo.Replace("Portrait", ""));
                        AddSmallButton(Assets.assets.portraitsSearch[index + thisWindow.pagination()].Replace(".png", ""));
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.portraitsSearch[index + thisWindow.pagination()];
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = possibleEffects;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
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
            AddPaginationLine();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (possibleEffects.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = possibleEffects[index + thisWindow.pagination()];
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
                    var foo = possibleEffects[index + thisWindow.pagination()];
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = possibleTriggers;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
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
            AddPaginationLine();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (possibleTriggers.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = possibleTriggers[index + thisWindow.pagination()];
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
                    var foo = possibleTriggers[index + thisWindow.pagination()];
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
            var thisWindow = CDesktop.LBWindow();
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
            AddPaddingRegion(() => SetRegionAsGroupExtender());
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
                    if (!WindowUp("ObjectManagerAbilities"))
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
                    if (!WindowUp("ObjectManagerBuffs"))
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
            AddPaddingRegion(() => SetRegionAsGroupExtender());
            AddRegionGroup();
            SetRegionGroupWidth(148);
            SetRegionGroupHeight(316);
            AddPaddingRegion(() => SetRegionAsGroupExtender());
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
            var thisWindow = CDesktop.LBWindow();
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
                    String.soundVolume.Set(effect.ContainsKey("SoundEffectVolume") ? effect["SoundEffectVolume"] : "0,7");
                    Respawn("ObjectManagerEventEffects");
                    Respawn("ObjectManagerEventEffect");
                });
            }
            AddPaddingRegion(() => SetRegionAsGroupExtender());
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
                    if (!WindowUp("ObjectManagerBuffs"))
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
                        AddSmallButton(buffs.Find(x => x.name == effect["BuffName"])?.icon);
                },
                (h) =>
                {
                    if (!WindowUp("ObjectManagerBuffs"))
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
            AddPaddingRegion(() => SetRegionAsGroupExtender());
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
                        var volume = effect.ContainsKey("SoundEffectVolume") ? float.Parse(effect["SoundEffectVolume"]) : 0.7f;
                        PlaySound(effect["SoundEffect"], volume);
                    });
            },
            (h) =>
            {
                Assets.assets.soundsSearch = Assets.assets.sounds;
                if (!WindowUp("ObjectManagerSoundsList"))
                {
                    CloseWindow("ObjectManagerEventEffects");
                    CloseWindow("ObjectManagerBuffs");
                    Respawn("ObjectManagerSoundsList");
                }
            });
            AddPaddingRegion(() =>
            {
                AddLine("Sound volume:", "DarkGray");
                AddInputLine(String.soundVolume);
                AddSmallButton("OtherReverse", (h) =>
                {
                    if (effect.ContainsKey("SoundEffectVolume"))
                        effect["SoundEffectVolume"] = "0,7";
                    String.soundVolume.Set("0,7");
                    });
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
                AddLine("Shatter target:", "DarkGray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    if (effect.ContainsKey("ShatterTarget"))
                        effect.Remove("ShatterTarget");
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
            if (effect.ContainsKey("ShatterTarget"))
            {
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
            AddPaddingRegion(() => SetRegionAsGroupExtender());
        }),
        new("ObjectManagerRarityList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddHeaderRegion(() =>
            {
                AddLine("Rarities:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    SpawnWindowBlueprint("ObjectManagerItems");
                });
            });
            var thisWindow = CDesktop.LBWindow();
            AddPaginationLine();
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
                AddPaddingRegion(() => AddLine());
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = itemsSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
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
                if (!WindowUp("ItemsSort"))
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
            AddPaginationLine();
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (itemsSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = itemsSearch[index + thisWindow.pagination()];
                        AddLine(foo.name.CutTail());
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
                    item = itemsSearch[index + thisWindow.pagination()];
                    String.objectName.Set(item.name);
                    String.price.Set(item.price + "");
                    String.itemPower.Set(item.ilvl + "");
                    String.requiredLevel.Set(item.lvl + "");
                    Respawn("ObjectManagerItem");
                },
                null,
                (h) => () => PrintItemTooltip(items[index + thisWindow.pagination()]));
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(items.Count + " items", "DarkGray");
                if (items.Count != itemsSearch.Count)
                    AddLine(itemsSearch.Count + " found in search", "DarkGray");
            });
        }),
        new("ObjectManagerItem", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => AddLine("Item:", "DarkGray"));
            AddPaddingRegion(() => AddInputLine(String.objectName, item.rarity));
            AddPaddingRegion(() => AddLine("Icon:", "DarkGray"));
            AddButtonRegion(() =>
            {
                AddLine(item.icon[4..] + ".png");
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
            AddPaddingRegion(() => AddLine("Rarity:", "DarkGray"));
            AddButtonRegion(() => AddLine(item.rarity),
            (h) =>
            {
                if (!WindowUp("ObjectManagerRarityList"))
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
            AddButtonRegion(() => AddLine("Manage stats"),
            (h) =>
            {
                item.stats ??= new();
                if (!item.stats.ContainsKey("Stamina")) item.stats.Add("Stamina", 0);
                if (!item.stats.ContainsKey("Strength")) item.stats.Add("Strength", 0);
                if (!item.stats.ContainsKey("Agility")) item.stats.Add("Agility", 0);
                if (!item.stats.ContainsKey("Intellect")) item.stats.Add("Intellect", 0);
                if (!item.stats.ContainsKey("Spirit")) item.stats.Add("Spirit", 0);
                String.stamina.Set(item.stats["Stamina"] + "");
                String.strength.Set(item.stats["Strength"] + "");
                String.agility.Set(item.stats["Agility"] + "");
                String.intellect.Set(item.stats["Intellect"] + "");
                String.spirit.Set(item.stats["Spirit"] + "");
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
            AddButtonRegion(() => AddLine("By name", "Black"),
            (h) =>
            {
                itemSets = itemSets.OrderBy(x => x.name).ToList();
                itemSetsSearch = itemSetsSearch.OrderBy(x => x.name).ToList();
                CloseWindow("ItemSetsSort");
                Respawn("ObjectManagerItemSets");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By average item power", "Black"),
            (h) =>
            {
                itemSets = itemSets.OrderByDescending(x => items.FindAll(y => y.set == x.name).Average(y => y.ilvl)).ToList();
                itemSetsSearch = itemSetsSearch.OrderByDescending(x => items.FindAll(y => y.set == x.name).Average(y => y.ilvl)).ToList();
                CloseWindow("ItemSetsSort");
                Respawn("ObjectManagerItemSets");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By amount of tiers", "Black"),
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
            var rowAmount = 5;
            var thisWindow = CDesktop.LBWindow();
            var list = itemSetsSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
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
                if (!WindowUp("ItemSetsSort"))
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
            AddPaginationLine();
            for (int i = 0; i < 5; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (itemSetsSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = itemSetsSearch[index + thisWindow.pagination()];
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
                    itemSet = itemSetsSearch[index + thisWindow.pagination()];
                    String.objectName.Set(itemSet.name);
                    Respawn("ObjectManagerItemSet");
                });
                AddPaddingRegion(() =>
                {
                    AddLine();
                    if (itemSetsSearch.Count > index + thisWindow.pagination())
                    {
                        var foo = itemSetsSearch[index + thisWindow.pagination()];
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
        }),
        new("ObjectManagerItemSet", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => AddLine("Item set:", "DarkGray"));
            AddPaddingRegion(() => AddInputLine(String.objectName));
            AddPaddingRegion(() => { });
        }),
        new("ObjectManagerCostManager", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
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
            SetRegionGroupHeight(354);
            AddHeaderRegion(() =>
            {
                AddLine("Stats:", "Gray");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    if (item.stats["Stamina"] == 0) item.stats.Remove("Stamina");
                    if (item.stats["Strength"] == 0) item.stats.Remove("Strength");
                    if (item.stats["Agility"] == 0) item.stats.Remove("Agility");
                    if (item.stats["Intellect"] == 0) item.stats.Remove("Intellect");
                    if (item.stats["Spirit"] == 0) item.stats.Remove("Spirit");
                    //if (item.stats.Count == 0) item.stats = null;
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
            var rowAmount = 5;
            var thisWindow = CDesktop.LBWindow();
            var list = abilitiesSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            //if (ability != null)
            //{
            //    var index = abilitiesSearch.IndexOf(ability);
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
            //if (eventEdit != null)
            //{
            //    var editingEffects = WindowUp("ObjectManagerEventEffect");
            //    if (editingEffects)
            //    {
            //        var index = abilitiesSearch.IndexOf(eventEdit.effects[selectedEffect].ContainsKey("AbilityName") ? abilities.Find(x => x.name == eventEdit.effects[selectedEffect]["AbilityName"]) : null);
            //        if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //    }
            //    else
            //    {
            //        var index = abilitiesSearch.IndexOf(eventEdit.triggers[selectedTrigger].ContainsKey("AbilityName") ? abilities.Find(x => x.name == eventEdit.triggers[selectedTrigger]["AbilityName"]) : null);
            //        if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //    }
            //}
            AddHeaderRegion(() =>
            {
                AddLine("Abilities:");
                AddSmallButton("OtherClose", (h) =>
                {
                    if (eventEdit != null)
                    {
                        CloseWindow(h.window);
                        var editingEffects = WindowUp("ObjectManagerEventEffect");
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
                if (!WindowUp("AbilitiesSort"))
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
            AddPaginationLine();
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (abilitiesSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = abilitiesSearch[index + thisWindow.pagination()];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon == null ? "OtherUnknown" : foo.icon);
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
                        var editingEffects = WindowUp("ObjectManagerEventEffect");
                        if (editingEffects)
                        {
                            if (eventEdit.effects[selectedEffect].ContainsKey("AbilityName"))
                                eventEdit.effects[selectedEffect]["AbilityName"] = abilitiesSearch[index + thisWindow.pagination()].name;
                            else eventEdit.effects[selectedEffect].Add("AbilityName", abilitiesSearch[index + thisWindow.pagination()].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventEffect");
                            Respawn("ObjectManagerEventEffects");
                        }
                        else
                        {
                            if (eventEdit.triggers[selectedTrigger].ContainsKey("AbilityName"))
                                eventEdit.triggers[selectedTrigger]["AbilityName"] = abilitiesSearch[index + thisWindow.pagination()].name;
                            else eventEdit.triggers[selectedTrigger].Add("AbilityName", abilitiesSearch[index + thisWindow.pagination()].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventTrigger");
                            Respawn("ObjectManagerEventTriggers");
                        }
                    }
                    else
                    {
                        ability = abilitiesSearch[index + thisWindow.pagination()];
                        String.objectName.Set(ability.name);
                        String.cooldown.Set(ability.cooldown + "");
                        Respawn("ObjectManagerAbility");
                    }
                },
                null,
                (h) => () =>
                {
                    SetAnchor(Center);
                    var key = abilitiesSearch.ToList()[index + thisWindow.pagination()];
                    PrintAbilityTooltip(null, key, 0);
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
            AddPaddingRegion(() => AddInputLine(String.objectName, ability.name));
            if (ability.icon != null)
            {
                AddPaddingRegion(() => AddLine("Icon:", "DarkGray"));
                AddButtonRegion(() =>
                {
                    AddLine(ability.icon[7..] + ".png");
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
            }
            AddPaddingRegion(() => AddLine("Events:", "DarkGray"));
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
            AddButtonRegion(() => AddLine("Manage casting cost"),
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
            AddPaddingRegion(() => SetRegionAsGroupExtender());
            if (ability.events.Count < 5)
                AddButtonRegion(() => AddLine("Add new event"),
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
                AddPaddingRegion(() => AddLine("Add new event", "DarkGray"));

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
            AddButtonRegion(() => AddLine("By name", "Black"),
            (h) =>
            {
                buffs = buffs.OrderBy(x => x.name).ToList();
                buffsSearch = buffsSearch.OrderBy(x => x.name).ToList();
                CloseWindow("BuffsSort");
                Respawn("ObjectManagerBuffs");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By dispel type", "Black"),
            (h) =>
            {
                buffs = buffs.OrderByDescending(x => x.dispelType).ToList();
                buffsSearch = buffsSearch.OrderByDescending(x => x.dispelType).ToList();
                CloseWindow("BuffsSort");
                Respawn("ObjectManagerBuffs");
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By stackable property", "Black"),
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = buffsSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            if (eventEdit != null)
            {
                var editingEffects = WindowUp("ObjectManagerEventEffect");
                if (editingEffects)
                {
                    var index = buffsSearch.IndexOf(eventEdit.effects[selectedEffect].ContainsKey("BuffName") ? buffs.Find(x => x.name == eventEdit.effects[selectedEffect]["BuffName"]) : null);
                    if (index >= 10) CDesktop.LBWindow().SetPagination(index - 10);
                }
                else
                {
                    var index = buffsSearch.IndexOf(eventEdit.triggers[selectedTrigger].ContainsKey("BuffName") ? buffs.Find(x => x.name == eventEdit.triggers[selectedTrigger]["BuffName"]) : null);
                    if (index >= 10) CDesktop.LBWindow().SetPagination(index - 10);
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
                        var editingEffects = WindowUp("ObjectManagerEventEffect");
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
                if (!WindowUp("BuffsSort"))
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
            AddPaginationLine();
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (buffsSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = buffsSearch[index + thisWindow.pagination()];
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
                        var editingEffects = WindowUp("ObjectManagerEventEffect");
                        if (editingEffects)
                        {
                            if (eventEdit.effects[selectedEffect].ContainsKey("BuffName"))
                                eventEdit.effects[selectedEffect]["BuffName"] = buffsSearch[index + thisWindow.pagination()].name;
                            else eventEdit.effects[selectedEffect].Add("BuffName", buffsSearch[index + thisWindow.pagination()].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventEffects");
                            Respawn("ObjectManagerEventEffect");
                        }
                        else
                        {
                            if (eventEdit.triggers[selectedTrigger].ContainsKey("BuffName"))
                                eventEdit.triggers[selectedTrigger]["BuffName"] = buffsSearch[index + thisWindow.pagination()].name;
                            else eventEdit.triggers[selectedTrigger].Add("BuffName", buffsSearch[index + thisWindow.pagination()].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventTriggers");
                            Respawn("ObjectManagerEventTrigger");
                        }
                    }
                    else
                    {
                        buff = buffsSearch[index + thisWindow.pagination()];
                        String.objectName.Set(buff.name);
                        Respawn("ObjectManagerBuff");
                    }
                },
                null,
                (h) => () =>
                {
                    SetAnchor(Center);
                    PrintBuffTooltip(null, new CombatBuff(buffsSearch[index + thisWindow.pagination()], 0, null, 0));
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
                AddButtonRegion(() => AddLine("Copy ability into a new buff"),
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
                    var editingEffects = WindowUp("ObjectManagerEventEffect");
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
            AddButtonRegion(() => AddLine("Create a new buff"),
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
            AddPaddingRegion(() => AddInputLine(String.objectName));
            AddPaddingRegion(() => AddLine("Icon:", "DarkGray"));
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
            AddPaddingRegion(() => AddLine("Stackable:", "DarkGray"));
            AddButtonRegion(() =>
            {
                AddLine(buff.stackable ? "Yes" : "No");
            },
            (h) =>
            {
                buff.stackable ^= true;
            });
            AddPaddingRegion(() => AddLine("Events:", "DarkGray"));
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = racesSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            //if (race != null)
            //{
            //    var index = racesSearch.IndexOf(race);
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
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
                if (!WindowUp("RacesSort"))
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
            AddPaginationLine();
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (racesSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = racesSearch[index + thisWindow.pagination()];
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
                        if (Encounter.encounter != null) Encounter.encounter.who = racesSearch[index + thisWindow.pagination()].name;
                        else
                        {
                            var enc = new Encounter() { who = racesSearch[index + thisWindow.pagination()].name, levelMin = 1, levelMax = 0 };
                            if (WindowUp("ObjectManagerHostileAreaCommonEncounters"))
                                area.commonEncounters.Add(enc);
                            else if (WindowUp("ObjectManagerHostileAreaRareEncounters"))
                                area.rareEncounters.Add(enc);
                            else if (WindowUp("ObjectManagerHostileAreaEliteEncounters"))
                                area.eliteEncounters.Add(enc);
                        }
                        CloseWindow(h.window);
                        CDesktop.RespawnAll();
                        SpawnWindowBlueprint("ObjectManagerHostileAreas");
                    }
                    else
                    {
                        race = racesSearch[index + thisWindow.pagination()];
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
            AddPaddingRegion(() => AddInputLine(String.objectName));
            AddPaddingRegion(() => AddLine("Gendered portraits:", "DarkGray"));
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
                AddPaddingRegion(() => AddLine("Portraits:", "DarkGray"));
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
                AddPaddingRegion(() => AddLine("Portrait:", "DarkGray"));
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
                AddPaddingRegion(() => AddLine("Kind:", "DarkGray"));
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
                AddPaddingRegion(() => AddLine("Vitality:", "DarkGray"));
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = mountsSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            //if (mount != null)
            //{
            //    var index = mountsSearch.IndexOf(mount);
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
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
                if (!WindowUp("MountsSort"))
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
            AddPaginationLine();
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (mountsSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = mountsSearch[index + thisWindow.pagination()];
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
                    mount = mountsSearch[index + thisWindow.pagination()];
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
            AddPaddingRegion(() => AddLine("Mount:", "DarkGray"));
            AddPaddingRegion(() => AddInputLine(String.objectName));
            AddPaddingRegion(() => AddLine("Icon:", "DarkGray"));
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
            AddPaddingRegion(() => AddLine("Speed:", "DarkGray"));
            AddPaddingRegion(() => AddInputLine(String.mountSpeed));
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = recipesSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            //if (recipe != null)
            //{
            //    var index = recipesSearch.IndexOf(recipe);
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
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
                if (!WindowUp("RecipesSort"))
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
            AddPaginationLine();
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (recipesSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = recipesSearch[index + thisWindow.pagination()];
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
                    recipe = recipesSearch[index + thisWindow.pagination()];
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
            AddPaddingRegion(() => AddLine("Recipe:", "DarkGray"));
            AddPaddingRegion(() => AddInputLine(String.objectName));
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
            var rowAmount = 10;
            var thisWindow = CDesktop.LBWindow();
            var list = factionsSearch;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            //if (faction != null)
            //{
            //    var index = factionsSearch.IndexOf(faction);
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
            //else if (town != null)
            //{
            //    var index = factionsSearch.FindIndex(x => x.name == town.faction);
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
            //else if (race != null)
            //{
            //    var index = factionsSearch.FindIndex(x => x.name == race.faction);
            //    if (index >= 10) CDesktop.LBWindow().LBRegionGroup().SetPagination(index / 10);
            //}
            AddHeaderRegion(() =>
            {
                AddLine("Factions:");
                AddSmallButton("OtherClose", (h) =>
                {
                    if (area != null)
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
                if (!WindowUp("FactionsSort"))
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
            AddPaginationLine();
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (factionsSearch.Count > index + thisWindow.pagination())
                    {
                        SetRegionBackground(Button);
                        var foo = factionsSearch[index + thisWindow.pagination()];
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
                    if (area != null)
                    {
                        area.faction = factionsSearch[index + thisWindow.pagination()].name;
                        CloseWindow(h.window);
                        Respawn("ObjectManagerTown");
                        Respawn("ObjectManagerTowns");
                    }
                    else if (race != null)
                    {
                        race.faction = factionsSearch[index + thisWindow.pagination()].name;
                        CloseWindow(h.window);
                        Respawn("ObjectManagerRace");
                        Respawn("ObjectManagerRaces");
                    }
                    else
                    {
                        faction = factionsSearch[index + thisWindow.pagination()];
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
            AddPaddingRegion(() => AddLine("Faction:", "DarkGray"));
            AddPaddingRegion(() => AddInputLine(String.objectName));
            AddPaddingRegion(() => AddLine("Icon:", "DarkGray"));
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
            AddPaddingRegion(() => AddLine("Side:", "DarkGray"));
            AddButtonRegion(() =>
            {
                AddLine(faction.side);
            },
            (h) =>
            {
                if (faction.side == "Hostile")
                    faction.side = "Unfriendly";
                else if (faction.side == "Unfriendly")
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
            Serialize(pEnchants, "permanentenchants", true, false, prefix);
            Serialize(pvpRanks, "pvpranks", true, false, prefix);
            Serialize(personCategories, "personcategories", true, false, prefix);
            Serialize(FlightPathGroup.flightPathGroups, "flightpaths", true, false, prefix);

            #endif

            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerLobby");
            AddHotkey("Open menu / Back", () => CloseDesktop("DevPanel"));
        }),
        new("GameSimulation", () =>
        {
            PlaySound("DesktopEnterCombat");
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("BoardFrame");
            SpawnWindowBlueprint("Board");
            SpawnWindowBlueprint("BufferBoard");
            SpawnWindowBlueprint("FriendlyBattleInfo");
            SpawnWindowBlueprint("LocationInfo");
            SpawnWindowBlueprint("EnemyBattleInfo");
            var elements = new List<string> { "Fire", "Water", "Earth", "Air", "Frost", "Lightning", "Arcane", "Decay", "Order", "Shadow" };
            foreach (var element in elements)
            {
                SpawnWindowBlueprint("Friendly" + element + "Resource");
                SpawnWindowBlueprint("Enemy" + element + "Resource");
            }
            board.Reset();
            AddHotkey(PageUp, () => {
                board.participants[0].who.resources = new Dictionary<string, int>
                {
                    { "Earth", 99 },
                    { "Fire", 99 },
                    { "Air", 99 },
                    { "Water", 99 },
                    { "Frost", 99 },
                    { "Lightning", 99 },
                    { "Arcane", 99 },
                    { "Decay", 99 },
                    { "Order", 99 },
                    { "Shadow", 99 },
                };
                Respawn("FriendlyBattleInfo");
                board.UpdateResourceBars(0, elements);
            });
            AddHotkey(PageDown, () => {
                board.participants[1].who.resources = new Dictionary<string, int>
                {
                    { "Earth", 99 },
                    { "Fire", 99 },
                    { "Air", 99 },
                    { "Water", 99 },
                    { "Frost", 99 },
                    { "Lightning", 99 },
                    { "Arcane", 99 },
                    { "Decay", 99 },
                    { "Order", 99 },
                    { "Shadow", 99 },
                };
                Respawn("EnemyBattleInfo");
                board.UpdateResourceBars(1, elements);
            });
        }),
        new("ObjectManagerRaces", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerRaces");
            AddHotkey("Open menu / Back", () => { race = null; racesSearch = null; CloseDesktop("ObjectManagerRaces"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerAbilities", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerAbilities");
            AddHotkey("Open menu / Back", () =>
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
            AddHotkey("Open menu / Back", () => { buff = null; buffsSearch = null; CloseDesktop("ObjectManagerBuffs"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerItems", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerItems");
            AddHotkey("Open menu / Back", () => { item = null; itemsSearch = null; CloseDesktop("ObjectManagerItems"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerItemSets", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerItemSets");
            AddHotkey("Open menu / Back", () => { itemSet = null; itemSetsSearch = null; CloseDesktop("ObjectManagerItemSets"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerMounts", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerMounts");
            AddHotkey("Open menu / Back", () => { mount = null; mountsSearch = null; CloseDesktop("ObjectManagerMounts"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerRecipes", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerRecipes");
            AddHotkey("Open menu / Back", () => { recipe = null; recipesSearch = null; CloseDesktop("ObjectManagerRecipes"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerGeneralDrops", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerGeneralDrops");
            AddHotkey("Open menu / Back", () => { generalDrop = null; generalDropsSearch = null; CloseDesktop("ObjectManagerGeneralDrops"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerFactions", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerFactions");
            AddHotkey("Open menu / Back", () => { faction = null; factionsSearch = null; CloseDesktop("ObjectManagerFactions"); });
            AddPaginationHotkeys();
        }),
    };

    public static List<Dictionary<string, string>> triggersCopy, effectsCopy;
}
