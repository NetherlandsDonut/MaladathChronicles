using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static UnityEngine.KeyCode;

using static Root;
using static Root.Anchor;
using static Root.RegionBackgroundType;

using static Item;
using static Buff;
using static Race;
using static Zone;
using static Site;
using static Spec;
using static Sound;
using static Event;
using static Mount;
using static Board;
using static Recipe;
using static Person;
using static Defines;
using static Faction;
using static ItemSet;
using static Ability;
using static PVPRank;
using static SitePath;
using static SaveGame;
using static Coloring;
using static GeneralDrop;
using static PersonType;
using static Profession;
using static GameSettings;
using static FishingBoard;
using static WorldAbility;
using static Serialization;
using static PersonCategory;
using static PermanentEnchant;
using static SiteSpiritHealer;
using static SiteHostileArea;
using static SiteInstance;
using static SiteComplex;
using static SiteTown;

public class Blueprint
{
    public Blueprint(string title, Action actions, bool upperUI = false)
    {
        this.title = title;
        this.actions = actions;
        this.upperUI = upperUI;
    }

    public string title;
    public Action actions;
    public bool upperUI;

    public static List<Blueprint> windowBlueprints = new()
    {
        //Bestiary
        new("BestiaryKalimdor", () => {
            SetAnchor(-210, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(200);
            SetRegionGroupHeight(176);
            AddHeaderRegion(() =>
            {
                AddLine("Kalimdor", "", "Center");
            });
            AddPaddingRegion(() =>
            {
                SetRegionBackgroundAsImage("Sprites/Textures/Kalimdor");
                SetRegionAsGroupExtender();
            });
            AddRegionGroup();
            SetRegionGroupWidth(200);
            SetRegionGroupHeight(91);
            AddPaddingRegion(() =>
            {
                var allSites = new List<Site>();
                for (int i = 0; i < towns.Count; i++)
                    allSites.Add(towns[i]);
                for (int i = 0; i < areas.Count; i++)
                    allSites.Add(areas[i]);
                for (int i = 0; i < complexes.Count; i++)
                    allSites.Add(complexes[i]);
                for (int i = 0; i < instances.Count; i++)
                    allSites.Add(instances[i]);
                var zonesExcluded = zones.FindAll(x => x.continent != "Kalimdor").Select(x => x.name);
                allSites.RemoveAll(x => x.x == 0 && x.y == 0 || zonesExcluded.Contains(x.zone));
                AddLine("Explored areas: " + allSites.Count(x => currentSave.siteVisits.ContainsKey(x.name)) + " / " + allSites.Count, "DarkGray", "Center");
                var commons = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.commonEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Common entries: " + commons.Count(x => currentSave.commonsKilled.ContainsKey(x)) + " / " + commons.Count, "DarkGray", "Center");
                var rares = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.rareEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Rares killed: " + rares.Count(x => currentSave.raresKilled.ContainsKey(x)) + " / " + rares.Count, "DarkGray", "Center");
                var elites = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.eliteEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Elites killed: " + elites.Count(x => currentSave.elitesKilled.ContainsKey(x)) + " / " + elites.Count, "DarkGray", "Center");
                SetRegionAsGroupExtender();
            });
            AddButtonRegion(() =>
            {
                AddLine("Explore", "", "Center");
            },
            (h) =>
            {
                //PlaySound("DesktopInstanceOpen");
                //SpawnDesktopBlueprint("Bestiary");
            });
        }),
        new("BestiaryEasternKingdoms", () => {
            SetAnchor(9, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(200);
            SetRegionGroupHeight(176);
            AddHeaderRegion(() =>
            {
                AddLine("Eastern Kingdoms", "", "Center");
            });
            AddPaddingRegion(() =>
            {
                SetRegionBackgroundAsImage("Sprites/Textures/EasternKingdoms");
                SetRegionAsGroupExtender();
            });
            AddRegionGroup();
            SetRegionGroupWidth(200);
            SetRegionGroupHeight(91);
            AddPaddingRegion(() =>
            {
                var allSites = new List<Site>();
                for (int i = 0; i < towns.Count; i++)
                    allSites.Add(towns[i]);
                for (int i = 0; i < areas.Count; i++)
                    allSites.Add(areas[i]);
                for (int i = 0; i < complexes.Count; i++)
                    allSites.Add(complexes[i]);
                for (int i = 0; i < instances.Count; i++)
                    allSites.Add(instances[i]);
                var zonesExcluded = zones.FindAll(x => x.continent != "Eastern Kingdoms").Select(x => x.name);
                allSites.RemoveAll(x => x.x == 0 && x.y == 0 || zonesExcluded.Contains(x.zone));
                AddLine("Explored areas: " + allSites.Count(x => currentSave.siteVisits.ContainsKey(x.name)) + " / " + allSites.Count, "DarkGray", "Center");
                var commons = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.commonEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Common entries: " + commons.Count(x => currentSave.commonsKilled.ContainsKey(x)) + " / " + commons.Count, "DarkGray", "Center");
                var rares = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.rareEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Rares killed: " + rares.Count(x => currentSave.raresKilled.ContainsKey(x)) + " / " + rares.Count, "DarkGray", "Center");
                var elites = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.eliteEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Elites killed: " + elites.Count(x => currentSave.elitesKilled.ContainsKey(x)) + " / " + elites.Count, "DarkGray", "Center");
                SetRegionAsGroupExtender();
            });
            AddButtonRegion(() =>
            {
                AddLine("Explore", "", "Center");
            },
            (h) =>
            {
                //PlaySound("DesktopInstanceOpen");
                //SpawnDesktopBlueprint("Bestiary");
            });
        }),

        //Game
        new("Board", () => {
            SetAnchor(Top, 0, -34 + 19 * (board.field.GetLength(1) - 7));
            var boardBackground = new GameObject("BoardBackground", typeof(SpriteRenderer), typeof(SpriteMask));
            boardBackground.transform.parent = CDesktop.LBWindow.transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/BoardBackground" + board.field.GetLength(0) + "x" + board.field.GetLength(1));
            var mask = boardBackground.GetComponent<SpriteMask>();
            mask.sprite = Resources.Load<Sprite>("Sprites/Textures/BoardMask" + board.field.GetLength(0) + "x" + board.field.GetLength(1));
            mask.isCustomRangeActive = true;
            mask.frontSortingLayerID = SortingLayer.NameToID("Missile");
            mask.backSortingLayerID = SortingLayer.NameToID("Default");
            boardBackground = new GameObject("BoardInShadow", typeof(SpriteRenderer));
            boardBackground.transform.parent = CDesktop.LBWindow.transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/BoardShadow" + board.field.GetLength(0) + "x" + board.field.GetLength(1));
            boardBackground.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
            DisableGeneralSprites();
            AddRegionGroup();
            for (int i = 0; i < board.field.GetLength(1); i++)
                AddPaddingRegion(() =>
                {
                    for (int j = 0; j < board.field.GetLength(0); j++)
                        AddBigButton(board.GetFieldButton(),
                        (h) =>
                        {
                            var list = board.FloodCount(h.region.bigButtons.FindIndex(x => x.GetComponent<Highlightable>() == h), h.region.regionGroup.regions.IndexOf(h.region));
                            board.temporaryElementsPlayer = new();
                            board.playerFinishedMoving = true;
                            board.FloodDestroy(list);
                        });
                });
        }),
        new("BufferBoard", () => {
            SetAnchor(Top, 0, 194 + 19 * (BufferBoard.bufferBoard.field.GetLength(1) - 7));
            MaskWindow();
            DisableGeneralSprites();
            DisableCollisions();
            AddRegionGroup();
            for (int i = 0; i < BufferBoard.bufferBoard.field.GetLength(1); i++)
            {
                AddPaddingRegion(() =>
                {
                    for (int j = 0; j < BufferBoard.bufferBoard.field.GetLength(0); j++)
                    {
                        AddBigButton(BufferBoard.bufferBoard.GetFieldButton(),
                        (h) =>
                        {

                        });
                    }
                });
            }
        }, true),
        new("EnemyResources", () => {
            SetAnchor(BottomRight);
            AddRegionGroup();
            var elements1 = new List<string> { "Fire", "Water", "Earth", "Air", "Frost" };
            var elements2 = new List<string> { "Lightning", "Arcane", "Decay", "Order", "Shadow" };
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    AddSmallButton("Element" + element + "Rousing",
                        null,
                        null,
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(98);
                            AddHeaderRegion(() =>
                            {
                                AddLine(element + ":", "Gray");
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(board.enemy.resources.ToList().Find(x => x.Key == element).Value + " / " + board.enemy.MaxResource(element), "Gray");
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(49);
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    var value = board.enemy.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value < board.enemy.MaxResource(element) ? "Gray" : "Green"));
                    AddSmallButton("Element" + elements2[elements1.IndexOf(element)] + "Rousing",
                        null,
                        null,
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(98);
                            AddHeaderRegion(() =>
                            {
                                AddLine(elements2[elements1.IndexOf(element)] + ":", "Gray");
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(board.enemy.resources.ToList().Find(x => x.Key == elements2[elements1.IndexOf(element)]).Value + " / " + board.enemy.MaxResource(elements2[elements1.IndexOf(element)]), "Gray");
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(30);
            foreach (var element in elements2)
                AddHeaderRegion(() =>
                {
                    var value = board.enemy.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value < board.enemy.MaxResource(element) ? "Gray" : "Green"));
                });
        }),
        new("EnemyBattleInfo", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            AddButtonRegion(
                () =>
                {
                    AddLine(board.enemy.name, "Black");
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        PlaySound("DesktopMenuOpen");
                        SpawnDesktopBlueprint("GameMenu");
                    });
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                var race = races.Find(x => x.name == board.enemy.race);
                AddBigButton(race.portrait == "" ? "OtherUnknown" : race.portrait + (race.genderedPortrait ? board.enemy.gender : ""), (h) => { });
                AddLine("Level: ", "Gray");
                AddText(board.enemy.level - 10 > board.player.level ? "??" : "" + board.enemy.level, ColorEntityLevel(board.enemy.level));
            });
            AddHealthBar(40, -38, "Enemy", board.enemy);
            foreach (var actionBar in board.enemy.actionBars)
            {
                var abilityObj = abilities.Find(x => x.name == actionBar);
                if (abilityObj == null || abilityObj.cost == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(actionBar, "", "Right");
                        AddSmallButton(abilityObj.icon, (h) => { });
                        if (!abilityObj.EnoughResources(board.enemy))
                        {
                            SetSmallButtonToGrayscale();
                            AddSmallButtonOverlay("OtherGridBlurred");
                        }
                        if (board.CooldownOn(false, actionBar) > 0)
                            AddSmallButtonOverlay("OtherGrid");
                    },
                    (h) =>
                    {

                    },
                    null,
                    (h) => () =>
                    {
                        PrintAbilityTooltip(board.enemy, board.player, abilityObj, board.enemy.abilities[abilityObj.name]);
                    }
                );
            }
        }),
        new("PlayerResources", () => {
            SetAnchor(BottomLeft);
            AddRegionGroup();
            var elements1 = new List<string> { "Fire", "Water", "Earth", "Air", "Frost" };
            var elements2 = new List<string> { "Lightning", "Arcane", "Decay", "Order", "Shadow" };
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    AddSmallButton("Element" + element + "Rousing",
                        null,
                        null,
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(98);
                            AddHeaderRegion(() =>
                            {
                                AddLine(element + ":", "Gray");
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(board.player.resources.ToList().Find(x => x.Key == element).Value + " / " + board.player.MaxResource(element), "Gray");
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(49);
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    var value = board.player.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value < board.player.MaxResource(element) ? "Gray" : "Green"));
                    AddSmallButton("Element" + elements2[elements1.IndexOf(element)] + "Rousing",
                        null,
                        null,
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(98);
                            AddHeaderRegion(() =>
                            {
                                AddLine(elements2[elements1.IndexOf(element)] + ":", "Gray");
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(board.player.resources.ToList().Find(x => x.Key == elements2[elements1.IndexOf(element)]).Value + " / " + board.player.MaxResource(elements2[elements1.IndexOf(element)]), "Gray");
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(30);
            foreach (var element in elements2)
                AddHeaderRegion(() =>
                {
                    var value = board.player.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value < board.player.MaxResource(element) ? "Gray" : "Green"));
                });
        }),
        new("PlayerBattleInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            AddButtonRegion(
                () =>
                {
                    AddLine(board.player.name, "Black");
                    AddSmallButton("MenuFlee", (h) =>
                    {
                        board.EndCombat("Fled");
                    });
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                if (board.player.spec != null)
                {
                    AddBigButton(board.player.Spec().icon,
                        (h) => { }
                    );
                }
                else
                {
                    var race = races.Find(x => x.name == board.enemy.race);
                    AddBigButton(race.portrait == "" ? "OtherUnknown" : race.portrait + (race.genderedPortrait ? board.enemy.gender : ""), (h) => { });
                }
                AddLine("Level: " + board.player.level, "Gray");
            });
            AddHealthBar(40, -38, "Player", board.player);
            foreach (var actionBar in board.player.actionBars)
            {
                var abilityObj = abilities.Find(x => x.name == actionBar);
                if (abilityObj == null || abilityObj.cost == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(actionBar, "", "Right");
                        AddSmallButton(abilityObj.icon, (h) => { });
                        if (!abilityObj.EnoughResources(board.player))
                        {
                            SetSmallButtonToGrayscale();
                            AddSmallButtonOverlay("OtherGridBlurred");
                        }
                        if (board.CooldownOn(true, actionBar) > 0)
                            AddSmallButtonOverlay("OtherGrid");
                    },
                    (h) =>
                    {
                        if (abilityObj.EnoughResources(board.player) && board.CooldownOn(true, actionBar) <= 0)
                        {
                            board.CallEvents(board.player, new() { { "Trigger", "AbilityCast" }, {"Triggerer", "Effector" }, { "AbilityName", abilityObj.name } });
                            board.CallEvents(board.enemy, new() { { "Trigger", "AbilityCast" }, {"Triggerer", "Other" }, { "AbilityName", abilityObj.name } });
                            board.player.DetractResources(abilityObj.cost);
                            foreach (var element in abilityObj.cost)
                                board.log.elementsUsed.Inc(element.Key, element.Value);
                            board.temporaryElementsPlayer = new();
                            h.window.desktop.RebuildAll();
                        }
                    },
                    null,
                    (h) => () =>
                    {
                        PrintAbilityTooltip(board.player, board.enemy, abilityObj, board.player.abilities[abilityObj.name]);
                    }
                );
            }
        }),

        //Character
        new("CharacterBaseStats", () => {
            SetAnchor(BottomLeft);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                foreach (var foo in currentSave.player.Stats())
                    if (!foo.Key.Contains("Mastery"))
                        AddLine(foo.Key + ":", "Gray");
            });
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                foreach (var foo in currentSave.player.Stats())
                    if (!foo.Key.Contains("Mastery"))
                        AddLine(foo.Value + "", foo.Value > currentSave.player.stats.stats[foo.Key] ? "Uncommon" : (foo.Value < currentSave.player.stats.stats[foo.Key] ? "DangerousRed" : "Gray"));
            });
        }),
        new("CharacterCreation", () => {
            SetAnchor(TopLeft);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(228);
            SetRegionGroupHeight(354);
            AddHeaderRegion(() =>
            {
                AddLine("Side: " + creationSide);
                AddSmallButton("ActionReroll", (h) =>
                {
                    creationSide = random.Next(2) == 1 ? "Horde" : "Alliance";
                    creationRace = "";
                    creationSpec = "";
                    h.window.Respawn();
                });
            });
            AddHeaderRegion(() =>
            {
                AddBigButton("HonorAlliance", (h) => { creationSide = "Alliance"; creationRace = ""; creationSpec = ""; h.window.Respawn(); });
                if (creationSide != "Alliance") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                AddBigButton("HonorHorde", (h) => { creationSide = "Horde"; creationRace = ""; creationSpec = ""; h.window.Respawn(); });
                if (creationSide != "Horde") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
            });
            AddHeaderRegion(() =>
            {
                AddLine("Gender: " + creationGender);
                AddSmallButton("ActionReroll", (h) =>
                {
                    creationGender = random.Next(2) == 1 ? "Female" : "Male";
                    h.window.Respawn();
                });
            });
            AddHeaderRegion(() =>
            {
                AddBigButton("OtherGenderMale", (h) => { creationGender = "Male"; h.window.Respawn(); });
                if (creationGender != "Male") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                AddBigButton("OtherGenderFemale", (h) => { creationGender = "Female"; h.window.Respawn(); });
                if (creationGender != "Female") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
            });
            if (creationSide != "" && creationGender != "")
            {
                AddHeaderRegion(() =>
                {
                    var races = Race.races.FindAll(x => x.Faction() != null && x.Faction().side == creationSide);
                    AddLine("Race: " + creationRace);
                    AddSmallButton("ActionReroll", (h) =>
                    {
                        creationRace = races[random.Next(races.Count)].name;
                        creationSpec = "";
                        h.window.Respawn();
                    });
                });
                AddHeaderRegion(() =>
                {
                    if (creationSide == "Alliance")
                    {
                        AddBigButton("PortraitDwarf" + creationGender, (h) => { creationRace = "Dwarf"; creationSpec = ""; h.window.Respawn(); });
                        if (creationRace != "Dwarf") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitGnome" + creationGender, (h) => { creationRace = "Gnome"; creationSpec = ""; h.window.Respawn(); });
                        if (creationRace != "Gnome") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitHuman" + creationGender, (h) => { creationRace = "Human"; creationSpec = ""; h.window.Respawn(); });
                        if (creationRace != "Human") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitNightElf" + creationGender, (h) => { creationRace = "Night Elf"; creationSpec = ""; h.window.Respawn(); });
                        if (creationRace != "Night Elf") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitPandaren" + creationGender, (h) => { creationRace = "Pandaren"; creationSpec = ""; h.window.Respawn(); });
                        if (creationRace != "Pandaren") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    }
                    else if (creationSide == "Horde")
                    {
                        AddBigButton("PortraitOrc" + creationGender, (h) => { creationRace = "Orc"; creationSpec = ""; h.window.Respawn(); });
                        if (creationRace != "Orc") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitTauren" + creationGender, (h) => { creationRace = "Tauren"; creationSpec = ""; h.window.Respawn(); });
                        if (creationRace != "Tauren") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitTroll" + creationGender, (h) => { creationRace = "Troll"; creationSpec = ""; h.window.Respawn(); });
                        if (creationRace != "Troll") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitForsaken" + creationGender, (h) => { creationRace = "Forsaken"; creationSpec = ""; h.window.Respawn(); });
                        if (creationRace != "Forsaken") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitPandaren" + creationGender, (h) => { creationRace = "Pandaren"; creationSpec = ""; h.window.Respawn(); });
                        if (creationRace != "Pandaren") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    }
                });
            }
            if (creationRace != "")
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Class: " + creationSpec);
                    AddSmallButton("ActionReroll", (h) =>
                    {
                        var temp = specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace));
                        creationSpec = temp[random.Next(temp.Count)].name;
                        h.window.Respawn();
                    });
                });
                AddHeaderRegion(() =>
                {
                    foreach (var foo in specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace)))
                    {
                        AddBigButton(foo.icon, (h) => { creationSpec = foo.name; });
                        if (creationSpec != foo.name) { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    }
                });
            }
            AddPaddingRegion(() => { });
        }),
        new("CharacterCreationRightSide", () => {
            SetAnchor(TopRight);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(410);
            SetRegionGroupHeight(354);
            AddHeaderRegion(() =>
            {
                AddLine("Character creation: " + creationSide);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("CharacterCreation");
                    CloseWindow("CharacterCreationRightSide");
                    SpawnWindowBlueprint("CharacterRoster");
                    SpawnWindowBlueprint("CharacterInfo");
                    SpawnWindowBlueprint("TitleScreenSingleplayer");
                });
                AddSmallButton("ActionReroll", (h) =>
                {
                    creationSide = random.Next(2) == 1 ? "Horde" : "Alliance";
                    creationGender = random.Next(2) == 1 ? "Male" : "Female";
                    var races = Race.races.FindAll(x => x.faction == creationSide || x.faction == "Both");
                    var race = races[random.Next(races.Count)];
                    creationRace = race.name;
                    var temp = specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace));
                    creationSpec = temp[random.Next(temp.Count)].name;
                    do creationName = creationGender == "Male" ? race.maleNames[random.Next(race.maleNames.Count)] : race.femaleNames[random.Next(race.femaleNames.Count)];
                    while (saves[settings.selectedRealm].Exists(x => x.player.name == creationName));
                    CDesktop.windows.Find(x => x.title == "CharacterCreation").Respawn();
                });
            });
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddButtonRegion(() =>
            {
                AddLine("Finish creation");
            },
            (h) =>
            {
                PlaySound("DesktopCreateCharacter");
                AddNewSave();
                CloseWindow("CharacterCreation");
                CloseWindow("CharacterCreationRightSide");
                SpawnWindowBlueprint("CharacterRoster");
                SpawnWindowBlueprint("CharacterInfo");
                SpawnWindowBlueprint("TitleScreenSingleplayer");
                SaveGames();
            });
        }),
        new("CharacterInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (settings.selectedCharacter != "")
            {
                var slot = saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter);
                var spec = slot.player.Spec();
                AddHeaderRegion(() => { AddLine(slot.player.name); });
                AddHeaderRegion(() =>
                {
                    AddBigButton("Portrait" + slot.player.race.Clean() + (slot.player.Race().genderedPortrait ? slot.player.gender : ""), (h) => { });
                    AddBigButton(spec.icon, (h) => { });
                    AddLine("Level: " + slot.player.level, "Gray");
                    AddLine(spec.name, "Gray");
                });
                AddHeaderRegion(() => { AddLine("Stats:"); });
                var stats = slot.player.Stats();
                AddPaddingRegion(() =>
                {
                    AddLine("Stamina: ", "DarkGray");
                    AddText(stats["Stamina"] + "");
                    AddLine("Strength: ", "DarkGray");
                    AddText(stats["Strength"] + "");
                    AddLine("Agility: ", "DarkGray");
                    AddText(stats["Agility"] + "");
                    AddLine("Intellect: ", "DarkGray");
                    AddText(stats["Intellect"] + "");
                    AddLine("Spirit: ", "DarkGray");
                    AddText(stats["Spirit"] + "");
                });
                AddHeaderRegion(() => { AddLine("Talents:"); });
                AddPaddingRegion(() =>
                {
                    AddLine(spec.talentTrees[0].name + ": ", "DarkGray");
                    AddText(spec.talentTrees[0].talents.Count(x => slot.player.abilities.ContainsKey(x.ability)) + "");
                    AddLine(spec.talentTrees[1].name + ": ", "DarkGray");
                    AddText(spec.talentTrees[1].talents.Count(x => slot.player.abilities.ContainsKey(x.ability)) + "");
                    AddLine(spec.talentTrees[2].name + ": ", "DarkGray");
                    AddText(spec.talentTrees[2].talents.Count(x => slot.player.abilities.ContainsKey(x.ability)) + "");
                });
                AddHeaderRegion(() => { AddLine("Enemies defeated:"); });
                AddPaddingRegion(() =>
                {
                    AddLine("Common: ", "DarkGray");
                    AddText("" + (slot.commonsKilled.Count > 0 ? slot.commonsKilled.Sum(x => x.Value) : 0));
                    AddLine("Rares: ", "DarkGray");
                    AddText("" + (slot.raresKilled.Count > 0 ? slot.raresKilled.Sum(x => x.Value) : 0));
                    AddLine("Elites: ", "DarkGray");
                    AddText("" + (slot.elitesKilled.Count > 0 ? slot.elitesKilled.Sum(x => x.Value) : 0));
                });
                AddHeaderRegion(() => { AddLine("Total time played:"); });
                AddPaddingRegion(() =>
                {
                    SetRegionAsGroupExtender();
                    AddLine(slot.timePlayed.Hours + "h "  + slot.timePlayed.Minutes + "m", "DarkGray");
                });
                AddButtonRegion(() =>
                {
                    AddLine("Delete character", "Black");
                },
                (h) =>
                {
                    String.promptConfirm.Set("");
                    CDesktop.RespawnAll();
                    SpawnWindowBlueprint("ConfirmDeleteCharacter");
                    CDesktop.LBWindow.LBRegionGroup.LBRegion.inputLine.Activate();
                });
            }
            else
            {
                AddHeaderRegion(() => { AddLine("Character:"); });
                AddPaddingRegion(() => { AddLine("No character selected", "DarkGray"); SetRegionAsGroupExtender(); });
                AddPaddingRegion(() => AddLine("Delete a character", "DarkGray"));
            }
        }, true),
        new("CharacterInfoStats", () => {
            SetAnchor(-92, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(182);
            SetRegionGroupHeight(271);
            var stats = currentSave.player.Stats();
            AddHeaderRegion(() =>
            {
                AddLine("Character stats:");
            });
            AddHeaderRegion(() =>
            {
                foreach (var foo in stats)
                    if (!foo.Key.Contains("Mastery"))
                    {
                        AddLine(foo.Key + ": ", "Gray", "Right");
                        AddText(foo.Value + "", "Uncommon");
                    }
            });
            AddHeaderRegion(() =>
            {
                AddLine("Max health: ", "Gray", "Right");
                AddText(currentSave.player.MaxHealth() + "", "Uncommon");
                AddLine("Melee attack power: ", "Gray", "Right");
                AddText(currentSave.player.MeleeAttackPower() + "", "Uncommon");
                AddLine("Ranged attack power: ", "Gray", "Right");
                AddText(currentSave.player.RangedAttackPower() + "", "Uncommon");
                AddLine("Spell power: ", "Gray", "Right");
                AddText(currentSave.player.SpellPower() + "", "Uncommon");
                AddLine("Critical strike: ", "Gray", "Right");
                AddText(currentSave.player.CriticalStrike().ToString("0.00") + "%", "Uncommon");
                AddLine("Spell critical: ", "Gray", "Right");
                AddText(currentSave.player.SpellCritical().ToString("0.00") + "%", "Uncommon");
            });
            AddPaddingRegion(() => SetRegionAsGroupExtender());
        }, true),
        new("CharacterInfoStatsRight", () => {
            SetAnchor(TopRight, -19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(271);
            var stats = currentSave.player.Stats();
            AddHeaderRegion(() =>
            {
                AddLine("Character mastery:");
            });
            AddHeaderRegion(() =>
            {
                var ordered = stats.ToList().FindAll(x => x.Key.Contains("Mastery")).OrderBy(x => x.Key).OrderByDescending(x => x.Value).ToList();
                foreach (var foo in ordered)
                {
                    AddLine(foo.Key + ": ", "Gray", "Right");
                    AddText(foo.Value + "", "Uncommon");
                }
            });
            AddPaddingRegion(() => SetRegionAsGroupExtender());
        }, true),
        new("CharacterRankingTop", () =>
        {
            SetAnchor(-293, 153);
            DisableShadows();
            AddHeaderGroup();
            SetRegionGroupWidth(588);
            AddHeaderRegion(() =>
            {
                AddLine("Ranking", "Gray", "Center");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseDesktop("RankingScreen");
                });
            });
            if (settings.selectedRealmRanking == "")
                settings.selectedRealmRanking = Realm.realms[0].name;
            foreach (var realmRef in Realm.realms)
            {
                var realm = realmRef;
                AddRegionGroup();
                SetRegionGroupWidth(147);
                if (settings.selectedRealmRanking == realm.name)
                    AddPaddingRegion(() =>
                    {
                        AddLine(realm.name, "", "Center");
                    });
                else
                    AddButtonRegion(() =>
                    {
                        AddLine(realm.name, "", "Center");
                    },
                    (h) =>
                    {
                        settings.selectedRealmRanking = realm.name;
                        Respawn("CharacterRankingList");
                        Respawn("CharacterRankingListRight");
                    });
            }
        }),
        new("CharacterRankingList", () =>
        {
            SetAnchor(-293, 115);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(550);
            SetRegionGroupHeight(262);
            var slots = saves[settings.selectedRealmRanking].OrderByDescending(x => x.Score()).ToList();
            for (int i = 0; i < 7; i++)
                if (i < slots.Count)
                {
                    var slot = slots[i];
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("Portrait" + slot.player.race.Clean() + (slot.player.Race().genderedPortrait ? slot.player.gender : ""), (h) => { });
                        AddBigButton("Class" + slot.player.spec, (h) => { });
                        AddLine(slot.player.name + ", a level " + slot.player.level + " ");
                        AddText(slot.player.spec, slot.player.spec);
                        AddLine("Score: " + slot.Score());
                        if (slot.playerDead) AddText(", died while fighting " + (slot.deathInfo.commonSource ? "a " : "") + slot.deathInfo.source + " in " + slot.deathInfo.area);
                    });
                }
                else
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("OtherBlank", (h) => { });
                        AddBigButton("OtherBlank", (h) => { });
                    });
        }),
        new("CharacterRankingListRight", () =>
        {
            SetAnchor(257, 115);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(38);
            SetRegionGroupHeight(262);
            var slots = saves[settings.selectedRealmRanking].OrderByDescending(x => x.Score()).ToList();
            for (int i = 0; i < 7; i++)
                if (i < slots.Count)
                {
                    var slot = slots[i];
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("PVP" + (slot.player.Side() == "Alliance" ? "A" : "H") + slot.player.Rank().rank, (h) => { });
                    });
                }
                else
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("OtherBlank", (h) => { });
                    });
        }),
        new("CharacterRankingShadow", () =>
        {
            SetAnchor(-293, 153);
            AddRegionGroup();
            SetRegionGroupWidth(588);
            SetRegionGroupHeight(300);
            AddPaddingRegion(() => { });
        }),
        new("CharacterStats", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            var stats = currentSave.player.Stats();
            AddHeaderRegion(() =>
            {
                foreach (var foo in stats)
                    if (!foo.Key.Contains("Mastery"))
                        AddLine(foo.Key + ":", "Gray", "Right");
            });
            AddHeaderRegion(() =>
            {
                foreach (var foo in stats)
                    if (foo.Key.Contains("Mastery"))
                        AddLine(foo.Key + ":", "Gray", "Right");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Melee Attack Power:", "Gray", "Right");
                AddLine("Ranged Attack Power:", "Gray", "Right");
                AddLine("Spell Power:", "Gray", "Right");
                AddLine("Critical Strike:", "Gray", "Right");
                AddLine("Spell Critical:", "Gray", "Right");
                AddLine("Health From Stamina:", "Gray", "Right");
            });
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                foreach (var foo in stats)
                    if (!foo.Key.Contains("Mastery"))
                        AddLine(foo.Value + "", foo.Value > currentSave.player.stats.stats[foo.Key] ? "Uncommon" : (foo.Value < currentSave.player.stats.stats[foo.Key] ? "DangerousRed" : "Gray"));
            });
            AddHeaderRegion(() =>
            {
                foreach (var foo in stats)
                    if (foo.Key.Contains("Mastery"))
                        AddLine(foo.Value + "", foo.Value > currentSave.player.stats.stats[foo.Key] ? "Uncommon" : (foo.Value < currentSave.player.stats.stats[foo.Key] ? "DangerousRed" : "Gray"));
            });
            AddHeaderRegion(() =>
            {
                AddLine(currentSave.player.MeleeAttackPower() + "", "Gray");
                AddLine(currentSave.player.RangedAttackPower() + "", "Gray");
                AddLine(currentSave.player.SpellPower() + "", "Gray");
                AddLine(currentSave.player.CriticalStrike().ToString("0.00") + "%", "Gray");
                AddLine(currentSave.player.SpellCritical().ToString("0.00") + "%", "Gray");
                AddLine(currentSave.player.MaxHealth() + "", "Gray");
            });
        }),
        new("ExperienceBar", () => {
            SetAnchor(Bottom);
            var experience = currentSave == null ? 0 : (int)(319 * (currentSave.player.experience / (double)currentSave.player.ExperienceNeeded()));
            AddRegionGroup();
            SetRegionGroupWidth(experience * 2);
            SetRegionGroupHeight(12);
            AddPaddingRegion(() => { SetRegionBackground(Experience); });
            AddRegionGroup();
            SetRegionGroupWidth((319 - experience) * 2);
            SetRegionGroupHeight(12);
            AddPaddingRegion(() => { SetRegionBackground(NoExperience); });
        },  true),
        new("ExperienceBarBorder", () => {
            SetAnchor(Bottom);
            AddRegionGroup();
            SetRegionGroupWidth(638);
            SetRegionGroupHeight(12);
            AddPaddingRegion(() => { });
        }),

        //Chest
        new("Chest", () => {
            SetAnchor(259, -111);
            Chest.SpawnChestObject(new Vector2(0, 0), "Chest");
        }),
        new("ChestInfo", () => {
            SetAnchor(-92, -86);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(
                () =>
                {
                    AddLine(area.name + " spoils:");
                    AddSmallButton("OtherClose", (h) =>
                    {
                        PlaySound("DesktopCloseChest");
                        CloseDesktop("ChestLoot");
                    });
                }
            );
        }),
        new("ChestLoot", () => {
            SetAnchor(-92, -105);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddPaddingRegion(
                () =>
                {
                    for (int j = 0; j < 4 && j < currentSave.openedChests[area.name].inventory.items.Count; j++)
                        PrintLootItem(currentSave.openedChests[area.name].inventory.items[j]);
                }
            );
        }),

        //Console
        new("Console", () => {
            SetAnchor(Top);
            if (CDesktop.windows.Exists(x => x.title == "MapToolbar"))
                DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(638);
            AddPaddingRegion(() =>
            {
                AddInputLine(String.consoleInput);
            });
        },  true),

        //Login Screen
        new("CharacterRoster", () => {
            if (settings.selectedCharacter != "")
                SetDesktopBackground(saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter).LoginBackground(), true);
            else SetDesktopBackground("Sky", true);
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddHeaderRegion(() =>
            {
                AddLine("Realm:", "Gray");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    CloseWindow("RealmRoster");
                    CloseWindow("CharacterInfo");
                    CloseWindow("TitleScreenSingleplayer");
                    RemoveDesktopBackground();
                    Respawn("TitleScreenMenu");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine(settings.selectedRealm == "" ? "None" : settings.selectedRealm);
            },
            (h) =>
            {
                Respawn("RealmRoster");
            });
            if (settings.selectedRealm != "")
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Characters:", "Gray");
                });
                if (saves.ContainsKey(settings.selectedRealm))
                {
                    var aliveSlots = saves[settings.selectedRealm].FindAll(x => !x.playerDead);
                    foreach (var slot in aliveSlots)
                    {
                        AddPaddingRegion(() =>
                        {
                            AddBigButton("Portrait" + slot.player.race.Clean() + (slot.player.Race().genderedPortrait ? slot.player.gender : ""), (h) =>
                            {
                                CloseWindow("RealmRoster");
                                if (settings.selectedCharacter != slot.player.name)
                                {
                                    settings.selectedCharacter = slot.player.name;
                                    SetDesktopBackground(slot.LoginBackground(), true);
                                    Respawn("CharacterInfo");
                                }
                            });
                            if (settings.selectedCharacter != slot.player.name)
                            {
                                SetBigButtonToGrayscale();
                                AddBigButtonOverlay("OtherGridBlurred");
                            }
                            AddLine(slot.player.name);
                            AddLine("Level: " + slot.player.level + " ");
                            AddText(slot.player.spec, slot.player.spec);
                        });
                    }
                    AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
                    if (aliveSlots.Count < 7)
                        AddButtonRegion(() =>
                        {
                            AddLine("Create a new character", "Black");
                        },
                        (h) =>
                        {
                            CloseWindow(h.window);
                            CloseWindow("RealmRoster");
                            CloseWindow("CharacterInfo");
                            CloseWindow("TitleScreenSingleplayer");
                            creationName = "";
                            creationSide = "";
                            creationGender = "";
                            creationRace = "";
                            creationSpec = "";
                            SpawnWindowBlueprint("CharacterCreation");
                            SpawnWindowBlueprint("CharacterCreationRightSide");
                        });
                    else AddPaddingRegion(() => AddLine("Create a new character", "DarkGray"));
                }
                else AddPaddingRegion(() => AddLine("Create a new character", "DarkGray"));
            }
            else AddPaddingRegion(() => { });
        }, true),
        new("RealmRoster", () =>
        {
            SetAnchor(Center, 0, 117);
            AddHeaderGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Realms:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("RealmRoster");
                });
            });
            AddRegionGroup();
            foreach (var realm in Realm.realms)
            {
                if (realm.name == settings.selectedRealm)
                    AddHeaderRegion(() =>
                    {
                        AddLine(realm.name);
                    });
                else
                    AddButtonRegion(() =>
                    {
                        AddLine(realm.name);
                    },
                    (h) =>
                    {
                        settings.selectedRealm = realm.name;
                        if (saves[settings.selectedRealm].Count > 0)
                        {
                            settings.selectedCharacter = saves[settings.selectedRealm][0].player.name;
                            SpawnTransition();
                        }
                        else if (settings.selectedCharacter != "")
                        {
                            settings.selectedCharacter = "";
                            SpawnTransition();
                        }
                        CloseWindow(h.window);
                        Respawn("TitleScreenSingleplayer");
                        Respawn("CharacterRoster");
                        Respawn("CharacterInfo");
                    });
            }
            AddRegionGroup();
            foreach (var realm in Realm.realms)
            {
                AddPaddingRegion(() =>
                {
                    AddLine(realm.pvp ? "PVP" : "PVE", realm.pvp ? "DangerousRed" : "Gray");
                });
            }
            AddRegionGroup();
            foreach (var realm in Realm.realms)
            {
                AddPaddingRegion(() =>
                {
                    AddLine(realm.hardcore ? "Hardcore" : "Softcore", realm.hardcore ? "DangerousRed" : "Gray");
                });
            }
            AddRegionGroup();
            foreach (var realm in Realm.realms)
            {
                AddPaddingRegion(() =>
                {
                    AddLine(saves[realm.name].Count + "", saves[realm.name].Count == 7 ? "DangerousRed" : "Gray");
                    AddText(" / ", "DarkGray");
                    AddText("7", saves[realm.name].Count == 7 ? "DangerousRed" : "Gray");
                    AddText(" chars", "DarkGray");
                });
            }
        }, true),
        new("ConfirmDeleteCharacter", () => {
            SetAnchor(Center);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Confirm deletion:");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Type");
                AddText(" DELETE ", "DangerousRed");
                AddText("to confirm deletion");
            });
            AddPaddingRegion(() =>
            {
                AddInputLine(String.promptConfirm, "DangerousRed");
            });
        }, true),

        //Inventory
        new("PlayerEquipmentInfo", () => {
            if (CDesktop.title == "Map") return;
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(252);
            AddHeaderRegion(() => AddLine("Equipment:"));
            Foo("Head", currentSave.player.GetItemInSlot("Head"));
            Foo("Shoulders", currentSave.player.GetItemInSlot("Shoulders"));
            Foo("Back", currentSave.player.GetItemInSlot("Back"));
            Foo("Chest", currentSave.player.GetItemInSlot("Chest"));
            Foo("Wrists", currentSave.player.GetItemInSlot("Wrists"));
            Foo("Hands", currentSave.player.GetItemInSlot("Hands"));
            Foo("Waist", currentSave.player.GetItemInSlot("Waist"));
            Foo("Legs", currentSave.player.GetItemInSlot("Legs"));
            Foo("Feet", currentSave.player.GetItemInSlot("Feet"));
            Foo("Main Hand", currentSave.player.GetItemInSlot("Main Hand"));
            bool showOff = currentSave.player.GetItemInSlot("Main Hand") == null || currentSave.player.GetItemInSlot("Main Hand") != null && currentSave.player.GetItemInSlot("Main Hand").type != "Two Handed";
            if (showOff) Foo("Off Hand", currentSave.player.GetItemInSlot("Off Hand"));
            Foo("Neck", currentSave.player.GetItemInSlot("Neck"));
            Foo("Finger", currentSave.player.GetItemInSlot("Finger"));
            Foo("Trinket", currentSave.player.GetItemInSlot("Trinket"));
            if (!showOff) AddPaddingRegion(() => { AddLine(""); });
            //if (currentSave.player.spec == "Druid")
            //    Foo("Idol", currentSave.player.GetItemInSlot("Special"));
            //if (currentSave.player.spec == "Paladin")
            //    Foo("Libram", currentSave.player.GetItemInSlot("Special"));
            //if (currentSave.player.spec == "Shaman")
            //    Foo("Totem", currentSave.player.GetItemInSlot("Special"));
            //AddPaddingRegion(() => { AddLine(); AddLine(); AddLine(); AddLine(); });

            void Foo(string slot, Item item)
            {
                if (item != null)
                    AddHeaderRegion(
                        () =>
                        {
                            AddLine(item.name, item.rarity, "Right");
                            AddSmallButton(item.icon,
                            (h) =>
                            {
                                if (CDesktop.windows.Exists(x => x.title == "Inventory"))
                                {
                                    PlaySound(item.ItemSound("PutDown"), 0.6f);
                                    currentSave.player.Unequip(new() { slot });
                                    Respawn("PlayerEquipmentInfo");
                                    Respawn("Inventory");
                                }
                            },
                            null,
                            (h) => () =>
                            {
                                if (CDesktop.windows.Exists(x => x.title == "Inventory"))
                                    PrintItemTooltip(item);
                            });
                            if (settings.rarityIndicators.Value())
                                AddSmallButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 2);
                        }
                    );
                else
                    AddPaddingRegion(() =>
                    {
                        AddLine(slot, "DarkGray", "Right");
                        AddSmallButton("OtherEmpty", (h) => { });
                    });
            }
        }),
        new("Inventory", () => {
            if (CDesktop.title == "Map") return;
            SetAnchor(TopRight, -19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var items = currentSave.player.inventory.items;
            AddHeaderRegion(() =>
            {
                AddLine("Inventory:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.player.inventory.items.Reverse();
                    CloseWindow("Inventory");
                    SpawnWindowBlueprint("Inventory");
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "InventorySettings") && !CDesktop.windows.Exists(x => x.title == "BankSort") && !CDesktop.windows.Exists(x => x.title == "VendorSort") && !CDesktop.windows.Exists(x => x.title == "InventorySort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("InventorySort");
                        Respawn("Inventory");
                        Respawn("Bank", true);
                        Respawn("ExperienceBarBorder", true);
                        Respawn("ExperienceBar", true);
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
                if (!CDesktop.windows.Exists(x => x.title == "InventorySettings") && !CDesktop.windows.Exists(x => x.title == "BankSort") && !CDesktop.windows.Exists(x => x.title == "VendorSort") && !CDesktop.windows.Exists(x => x.title == "InventorySort"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("InventorySettings");
                        Respawn("Inventory");
                        Respawn("Bank", true);
                        Respawn("ExperienceBarBorder", true);
                        Respawn("ExperienceBar", true);
                    });
                else
                    AddSmallButton("OtherSettingsOff", (h) => { });
            });
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (index * 5 + j >= currentSave.player.inventory.BagSpace()) AddBigButton("OtherDisabled", (h) => { });
                            else if (items.Count > index * 5 + j) PrintInventoryItem(items[index * 5 + j]);
                            else AddBigButton("OtherEmpty", (h) => { });
                    }
                );
            }
            AddHeaderRegion(() =>
            {
                AddLine("Bags:");
                for (int i = 0; i < defines.maxBagsEquipped; i++)
                {
                    var index = i;
                    AddSmallButton(currentSave.player.inventory.bags.Count > index ? currentSave.player.inventory.bags[index].icon : "OtherEmpty",
                        (h) =>
                        {
                            if (currentSave.player.inventory.bags.Count > index && currentSave.player.inventory.items.Count < currentSave.player.inventory.BagSpace() - currentSave.player.inventory.bags[index].bagSpace)
                                currentSave.player.UnequipBag(index);
                        },
                        null,
                        (h) => () =>
                        {
                            if (currentSave.player.inventory.bags.Count > index)
                                PrintItemTooltip(currentSave.player.inventory.bags[index]);
                        }
                    );
                }
            });
            PrintPriceRegion(currentSave.player.inventory.money);
        }, true),
        new("InventorySort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort inventory:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("InventorySort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderBy(x => x.name).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By amount", "Black");
            },
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderBy(x => x.amount).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By rarity", "Black");
            },
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.rarity == "Poor" ? 0 : (x.rarity == "Common" ? 1 : (x.rarity == "Uncommon" ? 2 : (x.rarity == "Rare" ? 3 : (x.rarity == "Epic" ? 4 : 5))))).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By item power", "Black");
            },
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.ilvl).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By price", "Black");
            },
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.price).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By type", "Black");
            },
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.type).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
        }),
        new("InventorySettings", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Inventory settings:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("InventorySettings");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Rarity indicators", "Black");
                AddCheckbox(settings.rarityIndicators);
            },
            (h) =>
            {
                settings.rarityIndicators.Invert();
                CDesktop.RespawnAll();
            });
            if (settings.rarityIndicators.Value())
                AddButtonRegion(() =>
                {
                    AddLine("Big Rarity indicators", "Black");
                    AddCheckbox(settings.bigRarityIndicators);
                },
                (h) =>
                {
                    settings.bigRarityIndicators.Invert();
                    CDesktop.RespawnAll();
                });
            AddButtonRegion(() =>
            {
                AddLine("Upgrade indicators", "Black");
                AddCheckbox(settings.upgradeIndicators);
            },
            (h) =>
            {
                settings.upgradeIndicators.Invert();
                CDesktop.RespawnAll();
            });
            AddButtonRegion(() =>
            {
                AddLine("New slot indicators", "Black");
                AddCheckbox(settings.newSlotIndicators);
            },
            (h) =>
            {
                settings.newSlotIndicators.Invert();
                CDesktop.RespawnAll();
            });
        }),
        new("ConfirmItemDestroy", () => {
            SetAnchor(-92, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(182);
            AddPaddingRegion(() =>
            {
                AddLine("You are about to destroy", "", "Center");
                AddLine(item.name, item.rarity, "Center");
            });
            AddRegionGroup();
            SetRegionGroupWidth(91);
            AddButtonRegion(() =>
            {
                SetRegionBackground(RedButton);
                AddLine("Proceed", "", "Center");
            },
            (h) =>
            {
                PlaySound("DesktopMenuClose");
                currentSave.player.inventory.items.Remove(item);
                CloseWindow("ConfirmItemDestroy");
                CDesktop.RespawnAll();
            });
            AddRegionGroup();
            SetRegionGroupWidth(91);
            AddButtonRegion(() =>
            {
                AddLine("Cancel", "", "Center");
            },
            (h) =>
            {
                PlaySound("DesktopMenuClose");
                CloseWindow("ConfirmItemDestroy");
            });
        }, true),
        new("CurrentMount", () => {
            SetAnchor(-92, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine("Current mount:");
            });
            AddPaddingRegion(() =>
            {
                var mount = mounts.Find(x => x.name == currentSave.player.mount);
                if (mount != null)
                {
                    AddLine(mount.name);
                    AddLine("Speed: ", "DarkGray");
                    AddText(mount.speed == 6 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"));
                    AddBigButton(mount.icon, (h) => { });
                }
                else AddBigButton("OtherDisabled", (h) => { });
            });
            var mount = mounts.Find(x => x.name == currentSave.player.mount);
            AddButtonRegion(() =>
            {
                AddLine("Dismount");
            },
            (h) =>
            {
                currentSave.player.mount = "";
                Respawn("MountList");
                CloseWindow(h.window);
            });
        }),
        new("SplitItem", () => {
            SetAnchor(-115, 146);
            AddRegionGroup();
            SetRegionGroupWidth(228);
            AddHeaderRegion(() =>
            {
                AddLine("Enter amount to pick up:");
            });
            AddPaddingRegion(() =>
            {
                AddInputLine(String.splitAmount);
            });
        }, true),

        //Combat Results
        new("CombatResults", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(200);
            SetRegionGroupHeight(94);
            AddHeaderRegion(() =>
            {
                AddLine("Combat Results", "", "Center");
                AddSmallButton("OtherChart", (h) => { PlaySound("DesktopInstanceOpen"); SpawnDesktopBlueprint("CombatLog"); });
            });
            AddPaddingRegion(() =>
            {
                if (board.results.experience > 0)
                    AddLine("You earned " + board.results.experience + " experience", "", "Center");
                else AddLine("You earned no experience", "", "Center");
                SetRegionAsGroupExtender();
            });
            AddButtonRegion(() =>
            {
                if (board.results.result == "Won")
                {
                    if (board.results.inventory.items.Count > 0)
                        AddLine("Show Loot", "", "Center");
                    else AddLine("OK", "", "Center");
                }
                else if (Realm.realms.Find(x => x.name == settings.selectedRealm).hardcore)
                {
                    SetRegionBackground(RedButton);
                    AddLine("Game Over", "", "Center");
                }
                else
                    AddLine("Release Spirit", "", "Center");
            },
            (h) =>
            {
                var hard = Realm.realms.Find(x => x.name == settings.selectedRealm).hardcore;
                if (hard && board.results.result == "Lost")
                {
                    CloseSave();
                    SaveGames();
                    CloseDesktop("CombatResults");
                    CloseDesktop("Map");
                    CloseDesktop("TitleScreen");
                    SpawnDesktopBlueprint("TitleScreen");
                }
                else
                {
                    if (area.instancePart)
                    {
                        CloseDesktop("Instance");
                        SpawnDesktopBlueprint("Instance");
                        SpawnWindowBlueprint("HostileArea: " + area.name);
                        SetDesktopBackground(area.Background());
                        Respawn("BossQueue");
                    }
                    else
                    {
                        CloseDesktop("HostileArea");
                        SpawnDesktopBlueprint("HostileArea");
                    }
                    CloseDesktop("CombatResults");
                    if (board.results.inventory.items.Count > 0)
                    {
                        PlaySound("DesktopInventoryOpen");
                        SpawnDesktopBlueprint("CombatResultsLoot");
                    }
                }
            });
        }),
        new("CombatResultsChart", () => {
            SetAnchor(-301, 161);
            AddHeaderGroup();
            SetRegionGroupWidth(600);
            AddHeaderRegion(() =>
            {
                AddLine("Combat Log", "", "Center");
                AddSmallButton("OtherClose", (h) => { PlaySound("DesktopInstanceClose"); CloseDesktop("CombatLog"); });
                AddSmallButton(settings.chartBigIcons.Value() ? "OtherSmaller" : "OtherBigger",
                    (h) =>
                    {
                        settings.chartBigIcons.Invert();
                        PlaySound("DesktopChartSwitch");
                        CloseDesktop("CombatLog");
                        SpawnDesktopBlueprint("CombatLog");
                    });
            });
            AddPaddingRegion(() => { AddLine(chartPage, "", "Center"); });
            AddChart();
        }),
        new("CombatResultsChartLeftArrow", () => {
            DisableShadows();
            SetAnchor(-301, 142);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddSmallButton("OtherPreviousPage", (h) =>
                {
                    PlaySound("DesktopChartSwitch");
                    if (chartPage == "Damage Taken") chartPage = "Damage Dealt";
                    else if (chartPage == "Healing Received") chartPage = "Damage Taken";
                    else if (chartPage == "Elements Used") chartPage = "Healing Received";
                    else if (chartPage == "Damage Dealt") chartPage = "Elements Used";
                    CloseDesktop("CombatLog");
                    SpawnDesktopBlueprint("CombatLog");
                });
            });
        }, true),
        new("CombatResultsChartRightArrow", () => {
            DisableShadows();
            SetAnchor(280, 142);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddSmallButton("OtherNextPage", (h) =>
                {
                    PlaySound("DesktopChartSwitch");
                    if (chartPage == "Damage Dealt") chartPage = "Damage Taken";
                    else if (chartPage == "Damage Taken") chartPage = "Healing Received";
                    else if (chartPage == "Healing Received") chartPage = "Elements Used";
                    else if (chartPage == "Elements Used") chartPage = "Damage Dealt";
                    CloseDesktop("CombatLog");
                    SpawnDesktopBlueprint("CombatLog");
                });
            });
        }, true),
        new("CombatResultsLoot", () => {
            SetAnchor(-92, -105);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddPaddingRegion(
                () =>
                {
                    for (int j = 0; j < 4 && j < board.results.inventory.items.Count; j++)
                        PrintLootItem(board.results.inventory.items[j]);
                }
            );
        }),
        new("LootInfo", () => {
            SetAnchor(-92, -86);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(
                () =>
                {
                    AddLine(board.enemy.name + ":");
                    AddSmallButton("OtherClose", (h) =>
                    {
                        PlaySound("DesktopInventoryClose");
                        CloseDesktop("CombatResultsLoot");
                    });
                }
            );
        }),

        //Town
        new("Person", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var type = personTypes.Find(x => x.type == person.type);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""), (h) => { });
            });
            if (type.category == "Trainer")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I seek training.");
                },
                (h) => { });
                AddButtonRegion(() =>
                {
                    AddLine("I want to reset my talents.");
                },
                (h) => { });
            }
            else if (type.category == "Profession Trainer")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to learn the profession.");
                },
                (h) =>
                {
                    PlaySound("DesktopInstanceOpen", 0.2f);
                    CloseWindow(h.window);
                    CloseWindow("Town: " + town.name);
                    SpawnWindowBlueprint("ProfessionLevelTrainer");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
                var rt = professions.Find(x => x.name == type.profession).recipeType;
                if (rt != null)
                    AddButtonRegion(() =>
                    {
                        AddLine("I would like to learn " + rt.ToLower() + "s.");
                    },
                    (h) =>
                    {
                        PlaySound("DesktopInstanceOpen", 0.2f);
                        CloseWindow(h.window);
                        CloseWindow("Town: " + town.name);
                        SpawnWindowBlueprint("ProfessionRecipeTrainer");
                        Respawn("ExperienceBarBorder");
                        Respawn("ExperienceBar");
                    });
            }
            else if (type.category == "Banker")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to open my vault.");
                },
                (h) =>
                {
                    currentSave.banks ??= new();
                    if (!currentSave.banks.ContainsKey(town.name))
                        currentSave.banks.Add(town.name, new() { items = new() });
                    PlaySound("DesktopBankOpen", 0.2f);
                    CloseWindow(h.window);
                    CloseWindow("Town: " + town.name);
                    SpawnWindowBlueprint("Bank");
                    SpawnWindowBlueprint("Inventory");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
                AddButtonRegion(() =>
                {
                    AddLine("I want to buy more vault space.");
                },
                (h) => { });
            }
            else if (type.category == "Innkeeper")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to rest in this inn.");
                },
                (h) => { });
                AddButtonRegion(() =>
                {
                    AddLine("I want to browse your goods.");
                },
                (h) =>
                {
                    PlaySound("DesktopInventoryOpen");
                    CloseWindow(h.window);
                    SpawnWindowBlueprint("Vendor");
                    SpawnWindowBlueprint("Inventory");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
                if (currentSave.player.homeLocation != town.name)
                    AddButtonRegion(() =>
                    {
                        AddLine("I want this inn to be my home.");
                    },
                    (h) =>
                    {
                        PlaySound("DesktopInstanceOpen");
                        CloseWindow(h.window);
                        SpawnWindowBlueprint("MakeInnHome");
                        Respawn("ExperienceBarBorder");
                        Respawn("ExperienceBar");
                    });
                if (!currentSave.player.inventory.items.Exists(x => x.name == "Hearthstone"))
                    AddButtonRegion(() =>
                    {
                        AddLine("I lost my hearthstone.");
                    },
                    (h) =>
                    {
                        var item = items.Find(x => x.name == "Hearthstone");
                        if (currentSave.player.inventory.CanAddItem(item))
                        {
                            PlaySound(item.ItemSound("PickUp"));
                            currentSave.player.inventory.AddItem(item.CopyItem(1));
                        }
                        h.window.Respawn();
                        Respawn("ExperienceBarBorder");
                        Respawn("ExperienceBar");
                    });
            }
            else if (type.category == "Vendor")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to browse your goods.");
                },
                (h) =>
                {
                    PlaySound("DesktopInventoryOpen");
                    CloseWindow(h.window);
                    CloseWindow("Town: " + town.name);
                    SpawnWindowBlueprint("Vendor");
                    SpawnWindowBlueprint("Inventory");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
            }
            else if (type.category == "Battlemaster")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to sign up for an arena match.");
                },
                (h) => { });
                AddButtonRegion(() =>
                {
                    AddLine("I want to buy gladiator equipment.");
                },
                (h) => { });
            }
            else if (type.category == "Stable Master")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to swap my mount.");
                },
                (h) =>
                {
                    PlaySound("DesktopInventoryOpen");
                    CloseWindow(h.window);
                    CloseWindow("Town: " + town.name);
                    SpawnWindowBlueprint("MountList");
                    if (mounts.Find(x => x.name == currentSave.player.mount) != null)
                        SpawnWindowBlueprint("CurrentMount");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
                AddButtonRegion(() =>
                {
                    AddLine("I want to buy a new mount.");
                },
                (h) => {  });
            }
            else if (type.category == "Flight Master")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to take a flight path.");
                },
                (h) =>
                {
                    PlaySound("DesktopInventoryOpen");
                    CloseWindow(h.window);
                    CloseWindow("Town: " + town.name);
                    SpawnWindowBlueprint("FlightMaster");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
            }
            AddButtonRegion(() =>
            {
                AddLine("Goodbye.");
            },
            (h) =>
            {
                PlaySound("DesktopInstanceClose");
                person = null;
                CloseWindow(h.window);
                if (personCategory != null) Respawn("Persons");
                Respawn("Town: " + town.name);
                Respawn("Persons", true);
            });
        }, true),
        new("Persons", () => {
            SetAnchor(TopRight, -19, -57);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            AddPaddingRegion(() =>
            {
                AddLine(personCategory.category + "s:", "Gray");
                AddSmallButton("OtherReverse",
                (h) =>
                {
                    personCategory = null;
                    CloseWindow(h.window.title);
                    Respawn("Town: " + town.name);
                    PlaySound("DesktopInstanceClose");
                });
            });
            var people = town.people.FindAll(x => x.category == personCategory);
            foreach (var person in people)
            {
                var personType = personTypes.Find(x => x.type == person.type);
                AddButtonRegion(() =>
                {
                    AddLine(person.name, "Black");
                    AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? factions.Find(x => x.name == town.faction).side : "") : "OtherUnknown", (h) => { });
                },
                (h) =>
                {
                    Person.person = person;
                    Respawn("Person");
                    CloseWindow("Persons");
                    CloseWindow("Town: " + town.name);
                    PlaySound("DesktopInstanceOpen");
                });
            }
        }, true),
        new("Vendor", () => {
            currentSave.buyback ??= new(true);
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var items = currentSave.vendorStock[town.name + ":" + person.name];
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("Vendor");
                    CloseWindow("Inventory");
                    Respawn("Person");
                    PlaySound("DesktopInventoryClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Vendor goods:");
            });
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (index * 5 + j >= 999) AddBigButton("OtherNoSlot", (h) => { });
                            else if (items.Count > index * 5 + j) PrintVendorItem(items[index * 5 + j], null);
                            else AddBigButton("OtherEmpty", (h) => { });
                    }
                );
            }
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Merchant", "", "Center"));
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Buyback", "", "Center"), (h) => { CloseWindow("Vendor"); SpawnWindowBlueprint("VendorBuyback"); PlaySound("VendorSwitchTab"); });
        }, true),
        new("VendorBuyback", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var items = new List<Item>();
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("VendorBuyback");
                    CloseWindow("Inventory");
                    Respawn("Person");
                    PlaySound("DesktopInventoryClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Buyback stock:");
            });
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (currentSave.buyback.items.Count > index * 5 + j) PrintVendorItem(null, currentSave.buyback.items[index * 5 + j]);
                            else AddBigButton("OtherEmpty", (h) => { });
                    }
                );
            }
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Merchant", "", "Center"), (h) => { CloseWindow("VendorBuyback"); SpawnWindowBlueprint("Vendor"); PlaySound("VendorSwitchTab"); });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Buyback", "", "Center"));
        }, true),
        new("MakeInnHome", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var type = personTypes.Find(x => x.type == person.type);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""), (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Do you want to change your", "DarkGray");
                AddLine("home from ", "DarkGray");
                AddText(currentSave.player.homeLocation, "LightGray");
                AddLine("to ", "DarkGray");
                AddText(town.name, "LightGray");
                AddLine("");
            });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() =>
            {
                AddLine("Cancel", "", "Center");
            },
            (h) =>
            {
                PlaySound("DesktopInstanceClose");
                CloseWindow("MakeInnHome");
                Respawn("Person");
            });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() =>
            {
                AddLine("Yes", "", "Center");
            },
            (h) =>
            {
                PlaySound("DesktopHomeLocation");
                currentSave.player.homeLocation = town.name;
                CloseWindow("MakeInnHome");
                Respawn("Person");
            });
        }),
        new("MountList", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup(() => currentSave.player.mounts.Count, 6);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(288);
            var type = personTypes.Find(x => x.type == person.type);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("MountList");
                    CloseWindow("CurrentMount");
                    Respawn("Person");
                    PlaySound("DesktopInventoryClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Stabled mounts:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.player.mounts.Reverse();
                    Respawn("MountList");
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "MountsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("MountsSort");
                        Respawn("MountList");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            var mounts = currentSave.player.mounts.Select(x => Mount.mounts.Find(y => y.name == x)).ToList();
            mounts.RemoveAll(x => x.name == currentSave.player.mount);
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (mounts.Count > index + 6 * regionGroup.pagination)
                    {
                        var mount = mounts[index + 6 * regionGroup.pagination];
                        AddLine(mount.name);
                        AddLine("Speed: ", "DarkGray");
                        AddText(mount.speed == 6 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"));
                        AddBigButton(mount.icon,
                            (h) =>
                            {
                                var mount = mounts[index + 6 * regionGroup.pagination];
                                if (currentSave.player.mount != mount.name)
                                {
                                    currentSave.player.mount = mount.name;
                                    Respawn("MountList");
                                    Respawn("CurrentMount");
                                    PlaySound("DesktopActionbarAdd", 0.7f);
                                }
                            },
                            null,
                            (h) => () =>
                            {
                                SetAnchor(Center);
                                var mount = mounts[index + 6 * regionGroup.pagination];
                                if (mount.abilities != null && mount.abilities.Count > 0)
                                    PrintAbilityTooltip(currentSave.player, null, abilities.Find(x => x.name == mount.abilities[0]), 0);
                            }
                        );
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled", (h) => { });
                    }
                });
            }
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
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                currentSave.player.mounts = currentSave.player.mounts.OrderBy(x => x).ToList();
                CloseWindow("MountsSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By speed", "Black");
            },
            (h) =>
            {
                currentSave.player.mounts = currentSave.player.mounts.OrderByDescending(x => Mount.mounts.Find(y => y.name == x).speed).ToList();
                CloseWindow("MountsSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
        }),
        new("Bank", () => {
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupHeight(281);
            var items = currentSave.banks[town.name].items;
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("Bank");
                    CloseWindow("Inventory");
                    Respawn("Person");
                    PlaySound("DesktopInventoryClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Bank:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.banks[town.name].items.Reverse();
                    Respawn("Bank");
                    Respawn("Inventory");
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "InventorySettings") && !CDesktop.windows.Exists(x => x.title == "InventorySort") && !CDesktop.windows.Exists(x => x.title == "BankSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("BankSort");
                        Respawn("Bank");
                        Respawn("Inventory");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
            });
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (index * 5 + j >= currentSave.banks[town.name].BagSpace()) AddBigButton("OtherNoSlot", (h) => { });
                            else if (items.Count > index * 5 + j) PrintBankItem(items[index * 5 + j]);
                            else AddBigButton("OtherEmpty", (h) => { });
                    }
                );
            }
        }, true),
        new("BankSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort bank inventory:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("BankSort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                currentSave.banks[town.name].items = currentSave.banks[town.name].items.OrderBy(x => x.name).ToList();
                CloseWindow("BankSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                currentSave.banks[town.name].items = currentSave.banks[town.name].items.OrderBy(x => x.amount).ToList();
                CloseWindow("BankSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By item power", "Black");
            },
            (h) =>
            {
                currentSave.banks[town.name].items = currentSave.banks[town.name].items.OrderByDescending(x => x.ilvl).ToList();
                CloseWindow("BankSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By price", "Black");
            },
            (h) =>
            {
                currentSave.banks[town.name].items = currentSave.banks[town.name].items.OrderByDescending(x => x.price).ToList();
                CloseWindow("BankSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By type", "Black");
            },
            (h) =>
            {
                currentSave.banks[town.name].items = currentSave.banks[town.name].items.OrderByDescending(x => x.type).ToList();
                CloseWindow("BankSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
        }),
        new("FlightMaster", () => {
            SetAnchor(TopLeft, 19, -38);
            var side = currentSave.player.Side();
            AddRegionGroup(() => town.flightPaths[side].Count - 1, 12);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(285);
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("FlightMaster");
                    Respawn("Person");
                    PlaySound("DesktopInstanceClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Possible destinations:");
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup, "FlightMaster");
            var destinations = town.flightPaths[side].FindAll(x => x != town).OrderBy(x => x.name).ThenBy(x => currentSave.siteVisits.ContainsKey(x.name)).ToList();
            for (int i = 0; i < 12; i++)
            {
                var index = i;
                if (destinations.Count >= index + 12 * RegionGroup.SavedStaticPagination(CDesktop.LBWindow.regionGroups.IndexOf(regionGroup)))
                    AddButtonRegion(() =>
                    {
                        if (destinations.Count > index + 12 * regionGroup.pagination)
                        {
                            var destination = destinations[index + 12 * regionGroup.pagination];
                            if (currentSave.siteVisits.ContainsKey(destination.name))
                            {
                                AddLine(destination.name);
                                AddSmallButton("Zone" + destination.zone.Clean()/*faction.Icon()*/, (h) => { });
                            }
                            else
                            {
                                SetRegionBackground(Header);
                                AddLine("Unknown", "DarkGray");
                                AddSmallButton("OtherDisabled", (h) => { });
                            }
                        }
                        else if (destinations.Count == index + 12 * regionGroup.pagination)
                        {
                            SetRegionBackground(Padding);
                            AddLine("");
                        }
                    },
                    (h) =>
                    {
                        if (h.region.backgroundType != Button) return;
                        CloseDesktop("Town");
                        SwitchDesktop("Map");
                        CDesktop.LockScreen();
                        PlaySound("DesktopTransportPay");
                        var temp = currentSave.currentSite;
                        currentSave.currentSite = town.name;
                        Respawn("Site: " + temp);
                        Respawn("Site: " + currentSave.currentSite);
                        CDesktop.cameraDestination = new Vector2(town.x, town.y);
                    });
            }
        }),

        //Fishing
        new("FishingAnchor", () => {
            SetAnchor(BottomLeft, 19, 35);
            AddHeaderGroup();
            AddPaddingRegion(() =>
            {
                AddBigButton("TradeFishing",
                (h) =>
                {
                    NewFishingBoard(FindSite(x => x.name == currentSave.currentSite));
                    SpawnDesktopBlueprint("FishingGame");
                });
            });
        }),
        new("FishingBoard", () => {
            SetAnchor(Top, 0, -34 + 19 * (fishingBoard.field.GetLength(1) - 7));
            var boardBackground = new GameObject("BoardBackground", typeof(SpriteRenderer), typeof(SpriteMask));
            boardBackground.transform.parent = CDesktop.LBWindow.transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/BoardBackground" + fishingBoard.field.GetLength(0) + "x" + fishingBoard.field.GetLength(1));
            var mask = boardBackground.GetComponent<SpriteMask>();
            mask.sprite = Resources.Load<Sprite>("Sprites/Textures/BoardMask" + fishingBoard.field.GetLength(0) + "x" + fishingBoard.field.GetLength(1));
            mask.isCustomRangeActive = true;
            mask.frontSortingLayerID = SortingLayer.NameToID("Missile");
            mask.backSortingLayerID = SortingLayer.NameToID("Default");
            boardBackground = new GameObject("BoardInShadow", typeof(SpriteRenderer));
            boardBackground.transform.parent = CDesktop.LBWindow.transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/BoardShadow" + fishingBoard.field.GetLength(0) + "x" + fishingBoard.field.GetLength(1));
            boardBackground.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
            DisableGeneralSprites();
            AddRegionGroup();
            for (int i = 0; i < fishingBoard.field.GetLength(1); i++)
            {
                AddPaddingRegion(() =>
                {
                    for (int j = 0; j < fishingBoard.field.GetLength(0); j++)
                    {
                        AddBigButton(fishingBoard.GetFieldButton(),
                        (h) =>
                        {
                            var list = fishingBoard.FloodCount(h.region.bigButtons.FindIndex(x => x.GetComponent<Highlightable>() == h), h.region.regionGroup.regions.IndexOf(h.region));
                            //fishingBoard.FloodDestroy(list);
                        });
                    }
                });
            }
        }),
        new("FishingBufferBoard", () => {
            SetAnchor(Top, 0, 194 + 19 * (FishingBufferBoard.fishingBufferBoard.field.GetLength(1) - 7));
            MaskWindow();
            DisableGeneralSprites();
            DisableCollisions();
            AddRegionGroup();
            for (int i = 0; i < FishingBufferBoard.fishingBufferBoard.field.GetLength(1); i++)
            {
                AddPaddingRegion(() =>
                {
                    for (int j = 0; j < FishingBufferBoard.fishingBufferBoard.field.GetLength(0); j++)
                    {
                        AddBigButton(FishingBufferBoard.fishingBufferBoard.GetFieldButton(),
                        (h) =>
                        {

                        });
                    }
                });
            }
        }, true),

        //Map
        new("MapToolbarShadow", () => {
            SetAnchor(Top);
            AddRegionGroup();
            SetRegionGroupWidth(638);
            SetRegionGroupHeight(15);
            AddPaddingRegion(() => { });
        }, true),
        new("MapToolbar", () => {
            AddHotkey(N, () =>
            {
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                if (CDesktop.title != "TalentScreen")
                {
                    PlaySound("DesktopTalentScreenOpen");
                    SpawnDesktopBlueprint("TalentScreen");
                }
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopTalentScreenClose");
                }
            });
            AddHotkey(P, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                if (CDesktop.title != "SpellbookScreen")
                    SpawnDesktopBlueprint("SpellbookScreen");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopSpellbookScreenClose");
                }
            });
            AddHotkey(B, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                if (CDesktop.title != "EquipmentScreen")
                    SpawnDesktopBlueprint("EquipmentScreen");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopInventoryClose");
                }
            });
            AddHotkey(T, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                if (CDesktop.title != "BestiaryScreen")
                    SpawnDesktopBlueprint("BestiaryScreen");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopInstanceClose");
                }
            });
            AddHotkey(R, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CharacterSheet");
                if (CDesktop.title != "CraftingScreen")
                    SpawnDesktopBlueprint("CraftingScreen");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopInstanceClose");
                }
            });
            AddHotkey(C, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                if (CDesktop.title != "CharacterSheet")
                    SpawnDesktopBlueprint("CharacterSheet");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopCharacterSheetClose");
                }
            });
            SetAnchor(Top);
            DisableShadows();
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton(CDesktop.title == "CharacterSheet" ? "OtherClose" : "MenuCharacterSheet", (h) =>
                {
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("TalentScreen");
                    CloseDesktop("CraftingScreen");
                    if (CDesktop.title != "CharacterSheet")
                        SpawnDesktopBlueprint("CharacterSheet");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopCharacterSheetClose");
                    }
                });
                AddSmallButton(CDesktop.title == "EquipmentScreen" ? "OtherClose" : "MenuInventory", (h) =>
                {
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("TalentScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("CharacterSheet");
                    if (CDesktop.title != "EquipmentScreen")
                        SpawnDesktopBlueprint("EquipmentScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopInventoryClose");
                    }
                });
                AddSmallButton(CDesktop.title == "SpellbookScreen" ? "OtherClose" : "MenuSpellbook", (h) =>
                {
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("TalentScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("CharacterSheet");
                    if (CDesktop.title != "SpellbookScreen")
                        SpawnDesktopBlueprint("SpellbookScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopSpellbookScreenClose");
                    }
                });
                AddSmallButton(CDesktop.title == "TalentScreen" ? "OtherClose" : "MenuClasses", (h) =>
                {
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("CharacterSheet");
                    if (CDesktop.title != "TalentScreen")
                    {
                        PlaySound("DesktopTalentScreenOpen");
                        SpawnDesktopBlueprint("TalentScreen");
                    }
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopTalentScreenClose");
                    }
                });
                AddSmallButton(CDesktop.title == "BestiaryScreen" ? "OtherClose" : "MenuCompletion", (h) =>
                {
                    CloseDesktop("TalentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("CharacterSheet");
                    if (CDesktop.title != "BestiaryScreen")
                        SpawnDesktopBlueprint("BestiaryScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopInstanceClose");
                    }
                });
                AddSmallButton(CDesktop.title == "CraftingScreen" ? "OtherClose" : "MenuCrafting", (h) =>
                {
                    CloseDesktop("TalentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("CharacterSheet");
                    if (CDesktop.title != "CraftingScreen")
                        SpawnDesktopBlueprint("CraftingScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopInstanceClose");
                    }
                });
            });
        }, true),
        new("MapToolbarClockLeft", () => {
            SetAnchor(TopLeft);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(262);
            AddPaddingRegion(() =>
            {
                AddLine("Day " + (currentSave.day + 1), "", "Right");
            });
        }, true),
        new("MapToolbarStatusLeft", () => {
            SetAnchor(TopLeft);
            DisableGeneralSprites();
            AddRegionGroup();
            SetRegionGroupWidth(262);
            AddPaddingRegion(() =>
            {
                if (currentSave.player.unspentTalentPoints > 0)
                {
                    AddLine("You have ", "", "Left");
                    AddText(currentSave.player.unspentTalentPoints + "", "Uncommon");
                    AddText(" talent point" + (currentSave.player.unspentTalentPoints == 1 ? "!" : "s!"));
                }
            });
        }, true),
        new("MapToolbarStatusRight", () => {
            SetAnchor(TopRight);
            DisableGeneralSprites();
            AddRegionGroup();
            SetRegionGroupWidth(262);
            AddPaddingRegion(() =>
            {
                AddSmallButton("OtherSettings", (h) =>
                {
                    PlaySound("DesktopMenuOpen");
                    SpawnDesktopBlueprint("GameMenu");
                });
            });
        }, true),
        new("MapToolbarClockRight", () => {
            SetAnchor(TopRight, -19);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(243);
            AddPaddingRegion(() =>
            {
                AddLine(currentSave.hour + (currentSave.minute < 10 ? ":0" : ":") + currentSave.minute, "", "Left");
            });
        }, true),

        //Menu
        new("TitleScreenMenu", () => {
            SetAnchor(Top);
            AddRegionGroup();
            SetRegionGroupWidth(130);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() =>
            {
                AddLine("", "Gray");
                AddLine("", "Gray");
                AddLine("Maladath", "Epic", "Center");
                AddLine("Chronicles", "Epic", "Center");
                AddLine("0.5.9", "DimGray", "Center");
                AddLine("", "Gray");
                AddLine("", "Gray");
                AddLine("", "Gray");
                AddLine("", "Gray");
                AddLine("", "Gray");
                AddLine("", "Gray");
                AddLine("", "Gray");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Menu", "Gray", "Center");
            });
            AddButtonRegion(() =>
            {
                AddLine("Singleplayer", "", "Center");
            },
            (h) =>
            {
                if (settings.selectedRealm == "")
                    SpawnWindowBlueprint("RealmRoster");
                SpawnWindowBlueprint("CharacterRoster");
                SpawnWindowBlueprint("CharacterInfo");
                SpawnWindowBlueprint("TitleScreenSingleplayer");
                CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Settings", "", "Center");
            },
            (h) =>
            {
                SpawnWindowBlueprint("GameSettings");
                CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Rankings", "", "Center");
            },
            (h) =>
            {
                SpawnDesktopBlueprint("RankingScreen");
            });
            AddButtonRegion(() =>
            {
                AddLine("Credits", "", "Center");
            },
            (h) =>
            {
                //BLIZZARD: MUSIC, SOUNDS AND TEXTURES
                //POOH: PROGRAMMING, DESIGN, 
                //RYVED & LEKRIS: CONSULTATION AND ADVICE
                //SPECIAL THANKS: ALL OF DISCO ADVANCE FOR BEING THE BEST TEAM EVER
            });
            AddButtonRegion(() =>
            {
                AddLine("Exit", "", "Center");
            },
            (h) =>
            {
                Application.Quit();
            });
            AddPaddingRegion(() => { });
            var maladathIcon = new GameObject("MaladathIcon", typeof(SpriteRenderer));
            maladathIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/MaladathIcon");
            maladathIcon.GetComponent<SpriteRenderer>().sortingLayerName = "Upper";
            maladathIcon.GetComponent<SpriteRenderer>().sortingOrder = 1;
            maladathIcon.transform.parent = CDesktop.LBWindow.transform;
            maladathIcon.transform.localPosition = new Vector3(69, -184);
        }, true),
        new("TitleScreenSingleplayer", () => {
            SetAnchor(Bottom);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(296);
            if (settings.selectedRealm != "" && settings.selectedCharacter != "")
            {
                AddButtonRegion(() =>
                {
                    AddLine("Enter World", "", "Center");
                },
                (h) =>
                {
                    Login();
                    SpawnDesktopBlueprint("Map");
                    var find = FindSite(x => x.name == currentSave.currentSite);
                    if (find != null) CDesktop.cameraDestination = new Vector2(find.x, find.y);
                });
            }
            else
                AddPaddingRegion(() => { AddLine("Enter World", "DarkGray", "Center"); });
        }, true),
        new("GameMenu", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Menu", "Gray");
                AddSmallButton("OtherClose", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                    CloseDesktop("GameMenu");
                });
            });
            AddRegionGroup();
            AddButtonRegion(() =>
            {
                AddLine("Settings", "Black");
            },
            (h) =>
            {
                SpawnWindowBlueprint("GameSettings");
                CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Save and return to main menu", "Black");
            },
            (h) =>
            {
                CloseSave();
                SaveGames();
                CloseDesktop("GameMenu");
                CloseDesktop("TitleScreen");
                SpawnDesktopBlueprint("TitleScreen");
                CloseDesktop("Map");
            });
            AddButtonRegion(() =>
            {
                AddLine("Save and exit", "Black");
            },
            (h) =>
            {
                CloseSave();
                SaveGames();
                Application.Quit();
            });
        }, true),
        new("GameSettings", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Settings:", "Gray");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    SpawnWindowBlueprint(CDesktop.title == "GameMenu" ? "GameMenu" : "TitleScreenMenu");
                });
            });
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddLine("Visuals", "", "Center");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(settings.pixelPerfectVision);
                AddLine("Pixel perfect vision");
            },
            (h) =>
            {
                settings.pixelPerfectVision.Invert();
                CDesktop.RespawnAll();
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(settings.fastCascading);
                AddLine("Fast cascading");
            },
            (h) =>
            {
                settings.fastCascading.Invert();
                CDesktop.RespawnAll();
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(new Bool(defines.windowBorders));
                AddLine("Window borders");
            },
            (h) =>
            {
                defines.windowBorders ^= true;
                CDesktop.RespawnAll();
            });
            AddPaddingRegion(() =>
            {
                AddLine("Sound", "", "Center");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(settings.music);
                AddLine("Music");
            },
            (h) =>
            {
                settings.music.Invert();
                CDesktop.RespawnAll();
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(settings.soundEffects);
                AddLine("Sound effects");
            },
            (h) =>
            {
                settings.soundEffects.Invert();
                CDesktop.RespawnAll();
            });
        }, true),
        new("LocationInfo", () => {
            SetAnchor(Top);
            AddRegionGroup();
            SetRegionGroupWidth(296);
            AddHeaderRegion(
                () =>
                {
                    AddLine(locationName, "", "Center");
                }
            );
        }),
        new("ProfessionLevelTrainer", () => {
            SetAnchor(TopLeft, 19, -38);
            var type = personTypes.Find(x => x.type == person.type);
            var profession = professions.Find(x => x.name == type.profession);
            var levels = profession.levels.OrderBy(x => x.requiredSkill).ToList();
            if (currentSave.player.professionSkills.ContainsKey(profession.name))
                levels = levels.FindAll(x => !currentSave.player.professionSkills[profession.name].Item2.Contains(x.levelName));
            AddHeaderGroup(() => levels.Count, 6);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(288);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""), (h) => { });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Learnable levels:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window.title);
                    Respawn("Person");
                    PlaySound("DesktopInstanceClose");
                });
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (levels.Count > index + 6 * regionGroup.pagination)
                    {
                        var key = levels[index + 6 * regionGroup.pagination];
                        AddLine(key.levelName);
                        AddLine("", "DarkGray");
                        if (key.requiredLevel > 0)
                        {
                            AddText("Level: ", "DarkGray");
                            AddText(key.requiredLevel + "", ColorRequiredLevel(key.requiredLevel));
                        }
                        if (key.requiredSkill > 0)
                        {
                            AddText(", Skill: ", "DarkGray");
                            AddText(key.requiredSkill + "", ColorProfessionRequiredSkill(profession.name, key.requiredSkill));
                        }
                        AddBigButton(profession.icon,
                            (h) =>
                            {
                                var key = levels[index + 6 * regionGroup.pagination];

                                //If player is high enough level..
                                if (currentSave.player.level >= key.requiredLevel)
                                {
                                    //If has the profession and at a proper level..
                                    if (key.requiredSkill == 0 || currentSave.player.professionSkills.ContainsKey(type.profession) && currentSave.player.professionSkills[type.profession].Item1 >= key.requiredSkill)
                                    {
                                        //If doesnt have the level yet..
                                        if (!currentSave.player.professionSkills.ContainsKey(type.profession) || currentSave.player.professionSkills.ContainsKey(type.profession) && !currentSave.player.professionSkills[type.profession].Item2.Contains(key.levelName))
                                        {
                                            //Learn the level
                                            if (!currentSave.player.professionSkills.ContainsKey(type.profession))
                                                currentSave.player.professionSkills.Add(type.profession, (1, new()));
                                            currentSave.player.professionSkills[type.profession].Item2.Add(key.levelName);
                                            Respawn(h.window.title);
                                            PlaySound("DesktopSkillLearned");
                                        }
                                    }
                                }
                            }
                        );
                        var can = false;
                        if (currentSave.player.level >= key.requiredLevel)
                            if (key.requiredSkill == 0 || currentSave.player.professionSkills.ContainsKey(type.profession) && currentSave.player.professionSkills[type.profession].Item1 >= key.requiredSkill)
                                if (!currentSave.player.professionSkills.ContainsKey(type.profession) || currentSave.player.learnedRecipes.ContainsKey(type.profession) && !currentSave.player.professionSkills[type.profession].Item2.Contains(key.levelName))
                                    can = true;
                        if (!can)
                        {
                            SetBigButtonToGrayscale();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                        else
                            AddBigButtonOverlay("OtherGlowLearnable");
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled", (h) => { });
                    }
                });
            }
        }),
        new("ProfessionRecipeTrainer", () => {
            SetAnchor(TopLeft, 19, -38);
            var type = personTypes.Find(x => x.type == person.type);
            var recipes = Recipe.recipes.FindAll(x => x.profession == type.profession && x.trainingCost > 0 && (x.learnedAt <= type.skillCap || type.skillCap == 0));
            if (currentSave.player.learnedRecipes.ContainsKey(type.profession))
                recipes = recipes.FindAll(x => !currentSave.player.learnedRecipes[type.profession].Contains(x.name));
            AddHeaderGroup(() => recipes.Count, 6);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(288);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""), (h) => { });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Learnable recipes:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window.title);
                    Respawn("Person");
                    PlaySound("DesktopInstanceClose");
                });
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (recipes.Count > index + 6 * regionGroup.pagination)
                    {
                        var key = recipes[index + 6 * regionGroup.pagination];
                        AddLine(key.name);
                        AddLine("", "DarkGray");
                        if (key.learnedAt > 0)
                        {
                            AddText("Required skill: ", "DarkGray");
                            AddText(key.learnedAt + " ", ColorProfessionRequiredSkill(key.profession, key.learnedAt));
                        }
                        AddBigButton(key.Icon(),
                            (h) =>
                            {
                                var key = recipes[index + 6 * regionGroup.pagination];

                                //If has the profession and at a proper level..
                                if (currentSave.player.professionSkills.ContainsKey(key.profession) && currentSave.player.professionSkills[key.profession].Item1 >= key.learnedAt)
                                {
                                    //If doesnt have the recipe..
                                    if (!currentSave.player.learnedRecipes.ContainsKey(type.profession) || currentSave.player.learnedRecipes.ContainsKey(type.profession) && !currentSave.player.learnedRecipes[type.profession].Contains(key.name))
                                    {
                                        //Add the recipe
                                        currentSave.player.LearnRecipe(key);
                                        Respawn(h.window.title);
                                        PlaySound("DesktopSkillLearned");
                                    }
                                }
                            },
                            null,
                            (h) => () =>
                            {
                                SetAnchor(Center);
                                var key = recipes[index + 6 * regionGroup.pagination];
                                PrintItemTooltip(items.Find(x => x.name == key.results.First().Key), Input.GetKey(KeyCode.LeftShift));
                            }
                        );
                        var can = false;
                        if (currentSave.player.professionSkills.ContainsKey(key.profession) && currentSave.player.professionSkills[key.profession].Item1 >= key.learnedAt)
                            if (!currentSave.player.learnedRecipes.ContainsKey(type.profession) || currentSave.player.learnedRecipes.ContainsKey(type.profession) && !currentSave.player.learnedRecipes[type.profession].Contains(key.name))
                                can = true;
                        if (!can)
                        {
                            SetBigButtonToGrayscale();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                        else
                            AddBigButtonOverlay("OtherGlowLearnable");
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled", (h) => { });
                    }
                });
            }
        }),

        //Spellbook
        new("SpellbookAbilityListActivated", () => {
            SetAnchor(TopRight, -19, -38);
            var activeAbilities = abilities.FindAll(x => !x.hide && x.cost != null && currentSave.player.abilities.ContainsKey(x.name)).ToDictionary(x => x, x => currentSave.player.abilities[x.name]);
            AddHeaderGroup(() => abilities.Count(x => !x.hide && x.cost != null && currentSave.player.abilities.ContainsKey(x.name)), 7);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(288);
            AddHeaderRegion(() =>
            {
                AddLine("Active abilities:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    abilities.Reverse();
                    Respawn("SpellbookAbilityListActivated");
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "AbilitiesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("AbilitiesSort");
                        Respawn("SpellbookAbilityListActivated");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (activeAbilities.Count > index + 6 * regionGroup.pagination)
                    {
                        var key = activeAbilities.ToList()[index + 6 * regionGroup.pagination];
                        AddLine(key.Key.name);
                        AddLine("Rank: ", "DarkGray");
                        AddText("" + ToRoman(key.Value + 1));
                        AddBigButton(key.Key.icon,
                            (h) =>
                            {
                                var key = activeAbilities.ToList()[index + 6 * regionGroup.pagination];
                                if (!currentSave.player.actionBars.Contains(key.Key.name) && currentSave.player.actionBars.Count < currentSave.player.ActionBarsAmount())
                                {
                                    currentSave.player.actionBars.Add(key.Key.name);
                                    Respawn("PlayerSpellbookInfo");
                                    Respawn("SpellbookAbilityListActivated", true);
                                    PlaySound("DesktopActionbarAdd", 0.7f);
                                }
                            },
                            null,
                            (h) => () =>
                            {
                                SetAnchor(Center);
                                var key = activeAbilities.ToList()[index + 6 * regionGroup.pagination].Key;
                                PrintAbilityTooltip(currentSave.player, null, key, activeAbilities[key]);
                            }
                        );
                        if (currentSave.player.actionBars.Contains(key.Key.name))
                        {
                            SetBigButtonToGrayscale();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                        else if (currentSave.player.actionBars.Count < currentSave.player.ActionBarsAmount())
                            AddBigButtonOverlay("OtherGlowLearnable");
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled", (h) => { });
                    }
                });
            }
            AddRegionGroup();
            SetRegionGroupWidth(86);
            AddPaddingRegion(() => AddLine("Activated", "", "Center"));
            AddRegionGroup();
            SetRegionGroupWidth(85);
            AddButtonRegion(() => AddLine("Passive", "", "Center"), (h) => { CloseWindow("SpellbookAbilityListActivated"); SpawnWindowBlueprint("SpellbookAbilityListPassive"); });
        }),
        new("SpellbookAbilityListPassive", () => {
            SetAnchor(TopRight, -19, -38);
            var passiveAbilities = abilities.FindAll(x => !x.hide && x.cost == null && currentSave.player.abilities.ContainsKey(x.name)).ToDictionary(x => x, x => currentSave.player.abilities[x.name]);
            AddHeaderGroup(() => abilities.Count(x => !x.hide && x.cost == null && currentSave.player.abilities.ContainsKey(x.name)), 7);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(288);
            AddHeaderRegion(() =>
            {
                AddLine("Passive abilities:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    abilities.Reverse();
                    Respawn("SpellbookAbilityListPassive");
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "AbilitiesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("AbilitiesSort");
                        Respawn("SpellbookAbilityListPassive");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (passiveAbilities.Count > index + 6 * regionGroup.pagination)
                    {
                        var key = passiveAbilities.ToList()[index + 6 * regionGroup.pagination];
                        AddLine(key.Key.name);
                        AddLine("Rank: ", "DarkGray");
                        AddText("" + ToRoman(key.Value + 1));
                        AddBigButton(key.Key.icon,
                            null,
                            null,
                            (h) => () =>
                            {
                                SetAnchor(Center);
                                var key = passiveAbilities.ToList()[index + 6 * regionGroup.pagination].Key;
                                PrintAbilityTooltip(currentSave.player, null, key, passiveAbilities[key]);
                            }
                        );
                        if (currentSave.player.actionBars.Contains(key.Key.name))
                        {
                            SetBigButtonToGrayscale();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled", (h) => { });
                    }
                });
            }
            AddRegionGroup();
            SetRegionGroupWidth(86);
            AddButtonRegion(() => AddLine("Activated", "", "Center"), (h) => { CloseWindow("SpellbookAbilityListPassive"); SpawnWindowBlueprint("SpellbookAbilityListActivated"); });
            AddRegionGroup();
            SetRegionGroupWidth(85);
            AddPaddingRegion(() => AddLine("Passive", "", "Center"));
        }),
        new("SpellbookResources", () => {
            SetAnchor(-301, -29);
            DisableShadows();
            AddHeaderGroup();
            AddHeaderRegion(() => { AddLine("Starting elements:"); });
            AddRegionGroup();
            var elements1 = new List<string> { "Fire", "Water", "Earth", "Air", "Frost" };
            var elements2 = new List<string> { "Lightning", "Arcane", "Decay", "Order", "Shadow" };
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    AddSmallButton("Element" + element + "Rousing",
                        null,
                        null,
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(93);
                            AddHeaderRegion(() => { AddLine(element + ":", "Gray"); });
                            AddPaddingRegion(() => { AddLine(currentSave.player.resources.ToList().Find(x => x.Key == element).Value + "/" + currentSave.player.MaxResource(element), "Gray"); });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(86);
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    var value = 0;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value > currentSave.player.MaxResource(element) ? "Red" : "Green"));
                    AddText("/" + currentSave.player.MaxResource(element), "DarkGray");
                    AddSmallButton("Element" + elements2[elements1.IndexOf(element)] + "Rousing",
                        null,
                        null,
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(93);
                            AddHeaderRegion(() =>
                            {
                                AddLine(elements2[elements1.IndexOf(element)] + ":", "Gray");
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(currentSave.player.resources.ToList().Find(x => x.Key == elements2[elements1.IndexOf(element)]).Value + " / " + currentSave.player.MaxResource(elements2[elements1.IndexOf(element)]), "Gray");
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(66);
            foreach (var element in elements2)
                AddHeaderRegion(() =>
                {
                    var value = 0;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value > currentSave.player.MaxResource(element) ? "Red" : "Green"));
                    AddText(" / " + currentSave.player.MaxResource(element), "DarkGray");
                });
        }, true),
        new("PlayerSpellbookInfo", () => {
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                AddLine("Action bars:");
            });
            for (int i = 0; i < currentSave.player.ActionBarsAmount(); i++)
            {
                var index = i;
                var abilityObj = currentSave.player.actionBars.Count <= index ? null : abilities.Find(x => x.name == currentSave.player.actionBars[index]);
                if (abilityObj != null)
                    AddButtonRegion(
                        () =>
                        {
                            AddLine(abilityObj.name, "", "Right");
                            AddSmallButton(abilityObj.icon, (h) => { });
                        },
                        (h) =>
                        {
                            currentSave.player.actionBars.RemoveAt(index);
                            Respawn("SpellbookAbilityListActivated", true);
                            Respawn("SpellbookAbilityListPassive", true);
                            Respawn("PlayerSpellbookInfo");
                            PlaySound("DesktopActionbarRemove", 0.7f);
                        },
                        null,
                        (h) => () =>
                        {
                            PrintAbilityTooltip(currentSave.player, null, abilityObj, currentSave.player.abilities[abilityObj.name]);
                        }
                    );
                else
                    AddHeaderRegion(
                        () =>
                        {
                            AddLine("", "Black");
                            AddSmallButton("OtherEmpty", (h) => { });
                        }
                    );
            }
            AddPaddingRegion(() => SetRegionAsGroupExtender());
        }),

        //Talents
        new("TalentScreenHeader", () => {
            SetAnchor(Top, 0, -19);
            AddRegionGroup();
            AddHeaderRegion(
                () =>
                {
                    AddSmallButton("OtherPreviousPage", (h) =>
                    {
                        PlaySound("DesktopSwitchPage");
                        currentSave.lastVisitedTalents--;
                        if (currentSave.lastVisitedTalents < 0)
                            currentSave.lastVisitedTalents = currentSave.player.Spec().talentTrees.Count - 1;
                        CloseDesktop("TalentScreen");
                        SpawnDesktopBlueprint("TalentScreen");
                    });
                }
            );
            AddRegionGroup();
            SetRegionGroupWidth(220);
            AddHeaderRegion(
                () =>
                {
                    AddLine(currentSave.player.Spec().talentTrees[currentSave.lastVisitedTalents].name, "", "Center");
                }
            );
            AddRegionGroup();
            AddHeaderRegion(
                () =>
                {
                    AddSmallButton("OtherNextPage", (h) =>
                    {
                        PlaySound("DesktopSwitchPage");
                        currentSave.lastVisitedTalents++;
                        if (currentSave.lastVisitedTalents >= currentSave.player.Spec().talentTrees.Count)
                            currentSave.lastVisitedTalents = 0;
                        CloseDesktop("TalentScreen");
                        SpawnDesktopBlueprint("TalentScreen");
                    });
                }
            );
        }),
        new("TalentScreenFooter", () => {
            SetAnchor(-130, -143);
            AddRegionGroup();
            SetRegionGroupWidth(258);
            AddPaddingRegion(
                () =>
                {
                    AddLine(">> " + (currentSave.player.TreeCompletion(currentSave.lastVisitedTalents, 1) + currentSave.player.TreeCompletion(currentSave.lastVisitedTalents, 0)) + " / " + (currentSave.player.TreeSize(currentSave.lastVisitedTalents, 1) + currentSave.player.TreeSize(currentSave.lastVisitedTalents, 0)) + " <<", "", "Center");
                }
            );
        }),
        new("TalentTreeRight", () => {
            SetAnchor(TopRight, 0, -19);
            var specShadow = new GameObject("SpecShadow", typeof(SpriteRenderer));
            specShadow.transform.parent = CDesktop.LBWindow.transform;
            specShadow.transform.localPosition = new Vector3(2, -2, 0.1f);
            specShadow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/Specs/SpecShadow");
            DisableShadows();
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(309);
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                SetRegionBackgroundAsImage("Sprites/Textures/Specs/" + currentSave.player.spec + currentSave.player.Spec().talentTrees[currentSave.lastVisitedTalents].name + "Right");
                if (currentSave.player.TreeCompletion(currentSave.lastVisitedTalents, 0) < defines.adeptTreeRequirement)
                    SetRegionBackgroundToGrayscale();
            });
            AddHeaderRegion(() =>
            {
                AddLine(currentSave.player.TreeCompletion(currentSave.lastVisitedTalents, 1) + "", "", "Center");
                AddText(" / ", "DarkGray");
                AddText(currentSave.player.TreeSize(currentSave.lastVisitedTalents, 1) + "");
            });
        }),
        new("TalentTreeLeft", () => {
            SetAnchor(TopLeft, 0, -19);
            var specShadow = new GameObject("SpecShadow", typeof(SpriteRenderer));
            specShadow.transform.parent = CDesktop.LBWindow.transform;
            specShadow.transform.localPosition = new Vector3(2, -2, 0.1f);
            specShadow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/Specs/SpecShadow");
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(309);
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                SetRegionBackgroundAsImage("Sprites/Textures/Specs/" + currentSave.player.spec + currentSave.player.Spec().talentTrees[currentSave.lastVisitedTalents].name + "Left");
            });
            AddHeaderRegion(() =>
            {
                AddLine(currentSave.player.TreeCompletion(currentSave.lastVisitedTalents, 0) + "", "", "Center");
                AddText(" / ", "DarkGray");
                AddText(currentSave.player.TreeSize(currentSave.lastVisitedTalents, 0) + "");
            });
        }),

        #region Dev Panel

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
                Serialize(pvpRanks, "pvpranks", false, false, prefix);
                Serialize(zones, "zones", false, false, prefix);
                Serialize(paths, "paths", false, false, prefix);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                    CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "HostileAreasSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("HostileAreasSort");
                        Respawn("ObjectManagerHostileAreas");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (areasSearch.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = areasSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton("Site" + foo.type, (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    area = areasSearch[index + 10 * regionGroup.pagination];
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
                AddSmallButton("Site" + area.type, (h) => { });
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "HostileAreasSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("HostileAreasSort");
                        Respawn("ObjectManagerHostileAreas");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (townsSearch.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = townsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(factions.Find(x => x.name == foo.faction).Icon(), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    town = townsSearch[index + 10 * regionGroup.pagination];
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
                AddSmallButton(factions.Find(x => x.name == town.faction).Icon(), (h) => { });
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                    CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "InstancesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("InstancesSort");
                        Respawn("ObjectManagerInstances");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (instancesSearch.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = instancesSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton("Site" + foo.type, (h) => { });
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
                    instance = instancesSearch[index + 10 * regionGroup.pagination];
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                    CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "ComplexesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("ComplexesSort");
                        CloseWindow("ObjectManagerComplexes");
                        SpawnWindowBlueprint("ObjectManagerComplexes");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (complexesSearch.Count > index + regionGroup.perPage * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = complexesSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton("SiteComplex", (h) => { });
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
                    complex = complexesSearch[index + regionGroup.perPage * regionGroup.pagination];
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
            }
            else if (instance != null)
            {
                var index = Assets.assets.ambienceSearch.IndexOf(instance.ambience + ".ogg");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
            }
            else if (complex != null)
            {
                var index = Assets.assets.ambienceSearch.IndexOf(complex.ambience + ".ogg");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
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
                    if (Assets.assets.ambienceSearch.Count > index + regionGroup.perPage * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.ambienceSearch[index + 10 * regionGroup.pagination];
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
                    var foo = Assets.assets.ambience[index + 10 * regionGroup.pagination];
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
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
                    if (Assets.assets.soundsSearch.Count > index + regionGroup.perPage * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.soundsSearch[index + regionGroup.perPage * regionGroup.pagination];
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
                    var foo = Assets.assets.soundsSearch[index + regionGroup.perPage * regionGroup.pagination];
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
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
                    if (Assets.assets.itemIconsSearch.Count > index + regionGroup.perPage * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.itemIconsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.Substring(4));
                        AddSmallButton(Assets.assets.itemIconsSearch[index + 10 * regionGroup.pagination].Replace(".png", ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.itemIconsSearch[index + 10 * regionGroup.pagination];
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
            }
            else if (buff != null)
            {
                var index = Assets.assets.abilityIconsSearch.IndexOf(buff.icon + ".png");
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
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
                    if (Assets.assets.abilityIconsSearch.Count > index + regionGroup.perPage * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.abilityIconsSearch[index + regionGroup.perPage * regionGroup.pagination];
                        AddLine(foo.Substring(7));
                        AddSmallButton(Assets.assets.abilityIconsSearch[index + regionGroup.perPage * regionGroup.pagination].Replace(".png", ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.abilityIconsSearch[index + regionGroup.perPage * regionGroup.pagination];
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
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
                    if (Assets.assets.factionIconsSearch.Count > index + regionGroup.perPage * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.factionIconsSearch[index + regionGroup.perPage * regionGroup.pagination];
                        AddLine(foo.Substring(7));
                        AddSmallButton(Assets.assets.factionIconsSearch[index + regionGroup.perPage * regionGroup.pagination].Replace(".png", ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.factionIconsSearch[index + regionGroup.perPage * regionGroup.pagination];
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
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
                    if (Assets.assets.mountIconsSearch.Count > index + regionGroup.perPage * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.mountIconsSearch[index + regionGroup.perPage * regionGroup.pagination];
                        AddLine(foo.Substring(5));
                        AddSmallButton(Assets.assets.mountIconsSearch[index + regionGroup.perPage * regionGroup.pagination].Replace(".png", ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.mountIconsSearch[index + regionGroup.perPage * regionGroup.pagination];
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
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
                    if (Assets.assets.portraitsSearch.Count > index + regionGroup.perPage * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = Assets.assets.portraitsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.Replace("Portrait", ""));
                        AddSmallButton(Assets.assets.portraitsSearch[index + 10 * regionGroup.pagination].Replace(".png", ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.portraitsSearch[index + regionGroup.perPage * regionGroup.pagination];
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
                    PlaySound("DesktopInventorySort", 0.2f);
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
                    if (possibleEffects.Count > index + regionGroup.perPage * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = possibleEffects[index + 10 * regionGroup.pagination];
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
                    var foo = possibleEffects[index + 10 * regionGroup.pagination];
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
                    PlaySound("DesktopInventorySort", 0.2f);
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
                    if (possibleTriggers.Count > index + regionGroup.perPage * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = possibleTriggers[index + 10 * regionGroup.pagination];
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
                    var foo = possibleTriggers[index + 10 * regionGroup.pagination];
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
                    PlaySound("DesktopChangePage", 0.4f);
                    SpawnWindowBlueprint("ObjectManagerEventEffects");
                    CloseWindow("ObjectManagerEventTrigger");
                    CloseWindow(h.window);
                });
                AddSmallButton("OtherSave", (h) =>
                {
                    PlaySound("DesktopTooltipHide", 0.4f);
                    triggersCopy = eventEdit.triggers.Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList();
                    h.window.Respawn();
                });
                AddSmallButton("OtherPaste", (h) =>
                {
                    if (effectsCopy != null)
                    {
                        PlaySound("DesktopWeirdClick3", 0.4f);
                        eventEdit.triggers.AddRange(triggersCopy.Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList());
                        h.window.Respawn();
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
                    h.window.Respawn();
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
                h.window.Respawn();
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
                    h.window.Respawn();
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
                    h.window.Respawn();
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
                        h.window.Respawn();
                    });
                });
                AddButtonRegion(() =>
                {
                    AddLine(trigger.ContainsKey("AbilityName") ? trigger["AbilityName"] : (eventParentType == "Ability" ? "This" : "Any"));
                    if (trigger.ContainsKey("AbilityName") && trigger["AbilityName"] != (eventParentType == "Ability" ? "This" : "Any"))
                        AddSmallButton(abilities.Find(x => x.name == trigger["AbilityName"]).icon, (h) => { });
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
                        h.window.Respawn();
                    });
                });
                AddButtonRegion(() =>
                {
                    AddLine(trigger.ContainsKey("BuffName") ? trigger["BuffName"] : (eventParentType == "Buff" ? "This" : "Any"));
                    if (trigger.ContainsKey("BuffName") && trigger["BuffName"] != (eventParentType == "Buff" ? "This" : "Any"))
                        AddSmallButton(buffs.Find(x => x.name == trigger["BuffName"]).icon, (h) => { });
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
                        h.window.Respawn();
                    });
                });
                AddButtonRegion(() =>
                {
                    AddLine(trigger.ContainsKey("ResourceType") ? trigger["ResourceType"] : "Any");
                    if (trigger.ContainsKey("ResourceType") && trigger["ResourceType"] != "Any")
                        AddSmallButton("Element" + trigger["ResourceType"] + "Rousing", (h) => { });
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
                    h.window.Respawn();
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
                            h.window.Respawn();
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
                    PlaySound("DesktopChangePage", 0.4f);
                    SpawnWindowBlueprint("ObjectManagerEventTriggers");
                    CloseWindow("ObjectManagerEventEffect");
                    CloseWindow(h.window);
                });
                AddSmallButton("OtherSave", (h) =>
                {
                    PlaySound("DesktopTooltipHide", 0.4f);
                    effectsCopy = eventEdit.effects.Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList();
                    h.window.Respawn();
                });
                AddSmallButton("OtherPaste", (h) =>
                {
                    if (effectsCopy != null)
                    {
                        PlaySound("DesktopWeirdClick3", 0.4f);
                        eventEdit.effects.AddRange(effectsCopy.Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList());
                        h.window.Respawn();
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
                        h.window.Respawn();
                    });
                    if (eventEdit.effects[0] != effect) 
                        AddSmallButton("OtherMoveUp", (h) =>
                        {
                            var index = eventEdit.effects.IndexOf(effect);
                            eventEdit.effects.RemoveAt(index);
                            eventEdit.effects.Insert(index - 1, effect);
                            h.window.Respawn();
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
                    h.window.Respawn();
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
                h.window.Respawn();
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
                        h.window.Respawn();
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
                    h.window.Respawn();
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
                        h.window.Respawn();
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
                    h.window.Respawn();
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Power type:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("PowerType"))
                            effect["PowerType"] = "None";
                        h.window.Respawn();
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
                    h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
                    });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("BuffName") ? effect["BuffName"] : "None");
                    if (effect.ContainsKey("BuffName") && effect["BuffName"] != "None")
                        AddSmallButton(buffs.Find(x => x.name == effect["BuffName"]).icon, (h) => { });
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
                        h.window.Respawn();
                    });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("BuffName") ? effect["BuffName"] : "None");
                    if (effect.ContainsKey("BuffName") && effect["BuffName"] != "None")
                        AddSmallButton(buffs.Find(x => x.name == effect["BuffName"]).icon, (h) => { });
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
                        h.window.Respawn();
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
                        h.window.Respawn();
                    });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None");
                    if (effect.ContainsKey("ResourceType") && effect["ResourceType"] != "None")
                        AddSmallButton("Element" + effect["ResourceType"] + "Rousing", (h) => { });
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
                    h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
                    });
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Element from:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ElementFrom"))
                            effect.Remove("ElementFrom");
                        h.window.Respawn();
                    });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("ElementFrom") ? effect["ElementFrom"] : "Random");
                    if (effect.ContainsKey("ElementFrom") && effect["ElementFrom"] != "Random")
                        AddSmallButton("Element" + effect["ElementFrom"] + "Rousing", (h) => { });
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
                    h.window.Respawn();
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Element to:", "DarkGray");
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ElementTo"))
                            effect.Remove("ElementTo");
                        h.window.Respawn();
                    });
                });
                AddButtonRegion(() =>
                {
                    AddLine(effect.ContainsKey("ElementTo") ? effect["ElementTo"] : "Random");
                    if (effect.ContainsKey("ElementTo") && effect["ElementTo"] != "Random")
                        AddSmallButton("Element" + effect["ElementTo"] + "Rousing", (h) => { });
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
                    h.window.Respawn();
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
                    h.window.Respawn();
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
                    h.window.Respawn();
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
                    h.window.Respawn();
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Chance scale:", "DarkGray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    if (effect.ContainsKey("ChanceScale"))
                        effect["ChanceScale"] = "None";
                    h.window.Respawn();
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
                h.window.Respawn();
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
                    h.window.Respawn();
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
                    h.window.Respawn();
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
                h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                    h.window.Respawn();
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
                h.window.Respawn();
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
                        h.window.Respawn();
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
                    h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                },
                (h) =>
                {
                    h.window.Respawn();
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
                h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                },
                (h) =>
                {
                    h.window.Respawn();
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
                h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait, (h) => { });
                },
                (h) =>
                {
                    h.window.Respawn();
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
                h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                        h.window.Respawn();
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
                AddSmallButton("SiteHostileArea", (h) => { });
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
                AddSmallButton("SiteEmeraldBough", (h) => { });
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                    Respawn("ObjectManagerItems");
                });
                if (!CDesktop.windows.Exists(x => x.title == "ItemsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("ItemsSort");
                        Respawn("ObjectManagerItems");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (itemsSearch.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = itemsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon, (h) => { });
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
                    item = itemsSearch[index + 10 * regionGroup.pagination];
                    String.objectName.Set(item.name);
                    String.price.Set(item.price + "");
                    String.itemPower.Set(item.ilvl + "");
                    String.requiredLevel.Set(item.lvl + "");
                    Respawn("ObjectManagerItem");
                },
                null,
                (h) => () =>
                {
                    PrintItemTooltip(items[index + 10 * regionGroup.pagination]);
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
                h.window.Respawn();
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
                AddSmallButton(item.icon, (h) => { });
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                if (index >= 5) CDesktop.LBWindow.LBRegionGroup.pagination = index / 5;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "ItemSetsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("ItemSetsSort");
                        Respawn("ObjectManagerItemSets");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (itemSetsSearch.Count > index + 5 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = itemSetsSearch[index + 5 * regionGroup.pagination];
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
                    itemSet = itemSetsSearch[index + 5 * regionGroup.pagination];
                    String.objectName.Set(itemSet.name);
                    Respawn("ObjectManagerItemSet");
                });
                AddPaddingRegion(() =>
                {
                    AddLine();
                    if (itemSetsSearch.Count > index + 5 * regionGroup.pagination)
                    {
                        var foo = itemSetsSearch[index + 5 * regionGroup.pagination];
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
                h.window.Respawn();
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                AddSmallButton("ElementFireRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Earth: ", "DarkGray");
                AddInputLine(String.earth, String.earth.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementEarthRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Water: ", "DarkGray");
                AddInputLine(String.water, String.water.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementWaterRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Air: ", "DarkGray");
                AddInputLine(String.air, String.air.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementAirRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Frost: ", "DarkGray");
                AddInputLine(String.frost, String.frost.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementFrostRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Decay: ", "DarkGray");
                AddInputLine(String.decay, String.decay.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementDecayRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Shadow: ", "DarkGray");
                AddInputLine(String.shadow, String.shadow.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementShadowRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Order: ", "DarkGray");
                AddInputLine(String.order, String.order.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementOrderRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Arcane: ", "DarkGray");
                AddInputLine(String.arcane, String.arcane.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementArcaneRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Lightning: ", "DarkGray");
                AddInputLine(String.lightning, String.lightning.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementLightningRousing", (h) => { });
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
            }
            if (eventEdit != null)
            {
                var editingEffects = CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect");
                if (editingEffects)
                {
                    var index = abilitiesSearch.IndexOf(eventEdit.effects[selectedEffect].ContainsKey("AbilityName") ? abilities.Find(x => x.name == eventEdit.effects[selectedEffect]["AbilityName"]) : null);
                    if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
                }
                else
                {
                    var index = abilitiesSearch.IndexOf(eventEdit.triggers[selectedTrigger].ContainsKey("AbilityName") ? abilities.Find(x => x.name == eventEdit.triggers[selectedTrigger]["AbilityName"]) : null);
                    if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "AbilitiesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("AbilitiesSort");
                        Respawn("ObjectManagerAbilities");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (abilitiesSearch.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = abilitiesSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon, (h) => { });
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
                                eventEdit.effects[selectedEffect]["AbilityName"] = abilitiesSearch[index + 10 * regionGroup.pagination].name;
                            else eventEdit.effects[selectedEffect].Add("AbilityName", abilitiesSearch[index + 10 * regionGroup.pagination].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventEffect");
                            Respawn("ObjectManagerEventEffects");
                        }
                        else
                        {
                            if (eventEdit.triggers[selectedTrigger].ContainsKey("AbilityName"))
                                eventEdit.triggers[selectedTrigger]["AbilityName"] = abilitiesSearch[index + 10 * regionGroup.pagination].name;
                            else eventEdit.triggers[selectedTrigger].Add("AbilityName", abilitiesSearch[index + 10 * regionGroup.pagination].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventTrigger");
                            Respawn("ObjectManagerEventTriggers");
                        }
                    }
                    else
                    {
                        ability = abilitiesSearch[index + 10 * regionGroup.pagination];
                        String.objectName.Set(ability.name);
                        String.cooldown.Set(ability.cooldown + "");
                        Respawn("ObjectManagerAbility");
                    }
                },
                null,
                (h) => () =>
                {
                    SetAnchor(Center);
                    var key = abilitiesSearch.ToList()[index + 10 * regionGroup.pagination];
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
                h.window.Respawn();
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
                AddSmallButton(ability.icon, (h) => { });
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
                        h.window.Respawn();
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
            }
            if (eventEdit != null)
            {
                var editingEffects = CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect");
                if (editingEffects)
                {
                    var index = buffsSearch.IndexOf(eventEdit.effects[selectedEffect].ContainsKey("BuffName") ? buffs.Find(x => x.name == eventEdit.effects[selectedEffect]["BuffName"]) : null);
                    if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
                }
                else
                {
                    var index = buffsSearch.IndexOf(eventEdit.triggers[selectedTrigger].ContainsKey("BuffName") ? buffs.Find(x => x.name == eventEdit.triggers[selectedTrigger]["BuffName"]) : null);
                    if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "BuffsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("BuffsSort");
                        Respawn("ObjectManagerBuffs");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (buffsSearch.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = buffsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon, (h) => { });
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
                                eventEdit.effects[selectedEffect]["BuffName"] = buffsSearch[index + 10 * regionGroup.pagination].name;
                            else eventEdit.effects[selectedEffect].Add("BuffName", buffsSearch[index + 10 * regionGroup.pagination].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventEffects");
                            Respawn("ObjectManagerEventEffect");
                        }
                        else
                        {
                            if (eventEdit.triggers[selectedTrigger].ContainsKey("BuffName"))
                                eventEdit.triggers[selectedTrigger]["BuffName"] = buffsSearch[index + 10 * regionGroup.pagination].name;
                            else eventEdit.triggers[selectedTrigger].Add("BuffName", buffsSearch[index + 10 * regionGroup.pagination].name);
                            CloseWindow(h.window);
                            Respawn("ObjectManagerEventTriggers");
                            Respawn("ObjectManagerEventTrigger");
                        }
                    }
                    else
                    {
                        buff = buffsSearch[index + 10 * regionGroup.pagination];
                        String.objectName.Set(buff.name);
                        Respawn("ObjectManagerBuff");
                    }
                },
                null,
                (h) => () =>
                {
                    SetAnchor(Center);
                    PrintBuffTooltip(null, null, (buffsSearch[index + 10 * regionGroup.pagination], 0, null, 0));
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
                    h.window.Respawn();
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
                    h.window.Respawn();
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
                AddSmallButton(buff.icon, (h) => { });
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
            });
        }),
        new("ObjectManagerRaces", () => {
            if (racesSearch == null) racesSearch = races;
            SetAnchor(TopLeft);
            AddRegionGroup(() => racesSearch.Count);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (race != null)
            {
                var index = racesSearch.IndexOf(race);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "RacesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("RacesSort");
                        Respawn("ObjectManagerRaces");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (racesSearch.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = racesSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.portrait + (foo.genderedPortrait ? "Female" : ""), (h) => { });
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
                        if (Encounter.encounter != null) Encounter.encounter.who = racesSearch[index + 10 * regionGroup.pagination].name;
                        else
                        {
                            var enc = new Encounter() { who = racesSearch[index + 10 * regionGroup.pagination].name, levelMin = 1, levelMax = 0 };
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
                        race = racesSearch[index + 10 * regionGroup.pagination];
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
                h.window.Respawn();
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
                h.window.Respawn();
            });
            if (race.genderedPortrait)
            {
                AddPaddingRegion(() => { AddLine("Portraits:", "DarkGray"); });
                AddHeaderRegion(() =>
                {
                    AddLine(race.portrait.Substring(8) + ".png");
                    AddSmallButton(race.portrait + "Female", (h) => { });
                    AddSmallButton(race.portrait + "Male", (h) => { });
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
                        AddSmallButton(race.Faction().Icon(), (h) => { });
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
                    AddSmallButton(race.portrait, (h) => { });
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
                        AddSmallButton(race.Faction().Icon(), (h) => { });
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "MountsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("MountsSort");
                        Respawn("ObjectManagerMounts");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (mountsSearch.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = mountsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon, (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    mount = mountsSearch[index + 10 * regionGroup.pagination];
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
                h.window.Respawn();
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
                AddSmallButton(mount.icon, (h) => { });
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By training cost", "Black");
            },
            (h) =>
            {
                recipes = recipes.OrderByDescending(x => x.trainingCost).ToList();
                recipesSearch = recipesSearch.OrderByDescending(x => x.trainingCost).ToList();
                CloseWindow("RecipesSort");
                Respawn("ObjectManagerRecipes");
                PlaySound("DesktopInventorySort", 0.2f);
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "RecipesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("RecipesSort");
                        Respawn("ObjectManagerRecipes");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (recipesSearch.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = recipesSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.Icon(), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    recipe = recipesSearch[index + 10 * regionGroup.pagination];
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
                h.window.Respawn();
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                PlaySound("DesktopInventorySort", 0.2f);
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
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
            }
            else if (town != null)
            {
                var index = factionsSearch.FindIndex(x => x.name == town.faction);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
            }
            else if (race != null)
            {
                var index = factionsSearch.FindIndex(x => x.name == race.faction);
                if (index >= 10) CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "FactionsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("FactionsSort");
                        Respawn("ObjectManagerFactions");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
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
                    if (factionsSearch.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = factionsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.Icon(), (h) => { });
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
                        town.faction = factionsSearch[index + 10 * regionGroup.pagination].name;
                        CloseWindow(h.window);
                        Respawn("ObjectManagerTown");
                        Respawn("ObjectManagerTowns");
                    }
                    else if (race != null)
                    {
                        race.faction = factionsSearch[index + 10 * regionGroup.pagination].name;
                        CloseWindow(h.window);
                        Respawn("ObjectManagerRace");
                        Respawn("ObjectManagerRaces");
                    }
                    else
                    {
                        faction = factionsSearch[index + 10 * regionGroup.pagination];
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
                h.window.Respawn();
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
                AddSmallButton(faction.Icon(), (h) => { });
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
                    CDesktop.LBWindow.LBRegionGroup.pagination = index / 10;
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
                    PlaySound("DesktopInventorySort", 0.2f);
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
                    if (specsSearch.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(Button);
                        var foo = specsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon, (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    spec = specsSearch[index + 10 * regionGroup.pagination];
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
                AddSmallButton(spec.icon, (h) => { });
            });
            //AddButtonRegion(() =>
            //{
            //    AddLine(spec.icon + ".png");
            //    AddSmallButton(spec.icon, (h) => { });
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

        #endregion
    };

    public static List<Blueprint> desktopBlueprints = new()
    {
        #region Game
        
        new("TitleScreen", () =>
        {
            PlayAmbience("AmbienceMainScreen", 0.5f, true);
            SpawnWindowBlueprint("TitleScreenMenu");
            AddHotkey(BackQuote, () =>
            {
                if (SpawnWindowBlueprint("Console") != null)
                {
                    PlaySound("DesktopTooltipShow", 0.2f);
                    CDesktop.LBWindow.LBRegionGroup.LBRegion.inputLine.Activate();
                }
            });
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("GameSettings"))
                {
                    PlaySound("DesktopButtonClose");
                    SpawnWindowBlueprint("TitleScreenMenu");
                }
                else if (CloseWindow("CharacterCreation"))
                {
                    PlaySound("DesktopButtonClose");
                    CloseWindow("CharacterCreationRightSide");
                    SpawnWindowBlueprint("CharacterRoster");
                    SpawnWindowBlueprint("CharacterInfo");
                    SpawnWindowBlueprint("TitleScreenSingleplayer");
                }
                else if (CloseWindow("TitleScreenSingleplayer"))
                {
                    PlaySound("DesktopButtonClose");
                    CloseWindow("CharacterRoster");
                    CloseWindow("CharacterInfo");
                    CloseWindow("RealmRoster");
                    RemoveDesktopBackground();
                    SpawnWindowBlueprint("TitleScreenMenu");
                }
            });
        }),
        new("Map", () =>
        {
            PlaySound("DesktopOpenSave", 0.2f);
            SetDesktopBackground("LoadingScreens/LoadingScreen" + (CDesktop.cameraDestination.x < 2470 ? "Kalimdor" : "EasternKingdoms"));
            loadingBar = new GameObject[2];
            loadingBar[0] = new GameObject("LoadingBarBegin", typeof(SpriteRenderer));
            loadingBar[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/LoadingBarEnd");
            loadingBar[0].transform.position = new Vector3(-1171, 854);
            loadingBar[1] = new GameObject("LoadingBar", typeof(SpriteRenderer));
            loadingBar[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/LoadingBarStretch");
            loadingBar[1].transform.position = new Vector3(-1168, 854);
            OrderLoadingMap();
            AddHotkey(W, () => { MoveCamera(new Vector2(0, EuelerGrowth())); }, false);
            AddHotkey(A, () => { MoveCamera(new Vector2(-EuelerGrowth(), 0)); }, false);
            AddHotkey(S, () => { MoveCamera(new Vector2(0, -EuelerGrowth())); }, false);
            AddHotkey(D, () => { MoveCamera(new Vector2(EuelerGrowth(), 0)); }, false);
            AddHotkey(UpArrow, () =>
            {
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.y += (int)Math.Sqrt(EuelerGrowth());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Remove(find);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey(LeftArrow, () =>
            {
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.x -= (int)Math.Sqrt(EuelerGrowth());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey(DownArrow, () =>
            {
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.y -= (int)Math.Sqrt(EuelerGrowth());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey(RightArrow, () =>
            {
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.x += (int)Math.Sqrt(EuelerGrowth());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopMenuOpen");
                SpawnDesktopBlueprint("GameMenu");
            });
            AddHotkey(Delete, () =>
            {
                if (sitePathBuilder != null)
                {
                    UnityEngine.Object.Destroy(pathTest.Item2);
                    sitePathBuilder = null;
                }
            });
            AddHotkey(BackQuote, () =>
            {
                if (SpawnWindowBlueprint("Console") != null)
                {
                    PlaySound("DesktopTooltipShow", 0.2f);
                    CDesktop.LBWindow.LBRegionGroup.LBRegion.inputLine.Activate();
                }
            });
            AddHotkey(KeyCode.Space, () =>
            {
                var whereTo = FindSite(x => x.name == currentSave.currentSite);
                CDesktop.cameraDestination = new Vector2(whereTo.x, whereTo.y);
            });

            void MoveCamera(Vector2 amount)
            {
                var temp = CDesktop.cameraDestination + amount * 2;
                //temp = new Vector2Int((int)temp.x, (int)temp.y) / mapGridSize;
                CDesktop.cameraDestination = new Vector2(temp.x, temp.y);
            }
        }),
        new("HostileArea", () =>
        {
            SetDesktopBackground(area.Background());
            SpawnWindowBlueprint("HostileArea: " + area.name);
            if (area.fishing) SpawnWindowBlueprint("FishingAnchor");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            if (currentSave.siteProgress.ContainsKey(area.name) && area.progression.First(x => x.type == "Treasure").point == currentSave.siteProgress[area.name] && (!currentSave.openedChests.ContainsKey(area.name) || currentSave.openedChests[area.name].inventory.items.Count > 0))
                SpawnWindowBlueprint("Chest");
            AddHotkey(Escape, () =>
            {
                if (area.complexPart)
                {
                    CloseDesktop("HostileArea");
                    SpawnDesktopBlueprint("Complex");
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("HostileArea");
                }
            });
        }),
        new("CombatResults", () =>
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("CombatResults");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
        }),
        new("CombatLog", () =>
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("CombatResultsChart");
            SpawnWindowBlueprint("CombatResultsChartLeftArrow");
            SpawnWindowBlueprint("CombatResultsChartRightArrow");
            FillChart();
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(A, () =>
            {
                PlaySound("DesktopChartSwitch");
                if (chartPage == "Damage Taken") chartPage = "Damage Dealt";
                else if (chartPage == "Healing Received") chartPage = "Damage Taken";
                else if (chartPage == "Elements Used") chartPage = "Healing Received";
                else if (chartPage == "Damage Dealt") chartPage = "Elements Used";
                CloseDesktop("CombatLog");
                SpawnDesktopBlueprint("CombatLog");
            });
            AddHotkey(D, () =>
            {
                PlaySound("DesktopChartSwitch");
                if (chartPage == "Damage Dealt") chartPage = "Damage Taken";
                else if (chartPage == "Damage Taken") chartPage = "Healing Received";
                else if (chartPage == "Healing Received") chartPage = "Elements Used";
                else if (chartPage == "Elements Used") chartPage = "Damage Dealt";
                CloseDesktop("CombatLog");
                SpawnDesktopBlueprint("CombatLog");
            });
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInstanceClose");
                CloseDesktop("CombatLog");
            });
        }),
        new("CombatResultsLoot", () =>
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("CombatResultsLoot");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInventoryClose");
                CloseDesktop("CombatResultsLoot");
            });
        }),
        new("ChestLoot", () =>
        {
            SetDesktopBackground(area.Background());
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("ChestInfo");
            SpawnWindowBlueprint("ChestLoot");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopCloseChest");
                CloseDesktop("ChestLoot");
            });
        }),
        new("Town", () =>
        {
            SetDesktopBackground(town.Background());
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            SpawnWindowBlueprint("Town: " + town.name);
            AddHotkey(Tab, () =>
            {
                if (CloseWindow("Vendor"))
                {
                    PlaySound("VendorSwitchTab");
                    Respawn("VendorBuyback");
                }
                else if (CloseWindow("VendorBuyback"))
                {
                    PlaySound("VendorSwitchTab");
                    Respawn("Vendor");
                }
            });
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("MountList"))
                {
                    PlaySound("DesktopInstanceClose");
                    CloseWindow("CurrentMount");
                    Respawn("Person");
                }
                else if (CloseWindow("Inventory"))
                {
                    PlaySound("DesktopInventoryClose");
                    CloseWindow("Bank");
                    CloseWindow("Vendor");
                    CloseWindow("VendorBuyback");
                    Respawn("Person");
                }
                else if (CloseWindow("MakeInnHome"))
                {
                    PlaySound("DesktopInstanceClose");
                    CloseWindow("MakeInnHome");
                    Respawn("Person");
                }
                else if (CloseWindow("FlightMaster"))
                {
                    PlaySound("DesktopInstanceClose");
                    Respawn("Person");
                }
                else if (CloseWindow("ProfessionRecipeTrainer"))
                {
                    PlaySound("DesktopInstanceClose");
                    CloseWindow("ProfessionRecipeTrainer");
                    Respawn("Person");
                }
                else if (CloseWindow("ProfessionLevelTrainer"))
                {
                    PlaySound("DesktopInstanceClose");
                    CloseWindow("ProfessionLevelTrainer");
                    Respawn("Person");
                }
                else if (CloseWindow("Person"))
                {
                    PlaySound("DesktopInstanceClose");
                    person = null;
                    if (personCategory != null) Respawn("Persons");
                    else Respawn("Town: " + town.name);
                }
                else if (CloseWindow("Persons"))
                {
                    PlaySound("DesktopInstanceClose");
                    personCategory = null;
                    Respawn("Town: " + town.name);
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    town = null;
                    CloseDesktop("Town");
                }
            });
        }),
        new("Instance", () =>
        {
            SetDesktopBackground(instance.Background());
            SpawnWindowBlueprint(instance.type + ": " + instance.name);
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            //if (area != null && currentSave.siteProgress.ContainsKey(area.name) && area.progression.First(x => x.type == "Treasure").point == currentSave.siteProgress[area.name] && (!currentSave.openedChests.ContainsKey(area.name) || currentSave.openedChests[area.name].inventory.items.Count > 0))
            //    SpawnWindowBlueprint("Chest");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("HostileArea: " + area?.name))
                {
                    area = null;
                    CloseWindow("BossQueue");
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground(instance.Background());
                }
                else if (instance.complexPart)
                {
                    CloseDesktop("Instance");
                    SpawnDesktopBlueprint("Complex");
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("Instance");
                }
            });
        }),
        new("Complex", () =>
        {
            SetDesktopBackground(complex.Background());
            SpawnWindowBlueprint("Complex: " + complex.name);
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("HostileArea: " + area?.name))
                {
                    area = null;
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground(complex.Background());
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("Complex");
                }
            });
        }),
        new("Game", () =>
        {
            locationName = board.area.name;
            PlaySound("DesktopEnterCombat");
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("Board");
            SpawnWindowBlueprint("BufferBoard");
            SpawnWindowBlueprint("PlayerBattleInfo");
            SpawnWindowBlueprint("LocationInfo");
            SpawnWindowBlueprint("EnemyBattleInfo");
            SpawnWindowBlueprint("PlayerResources");
            SpawnWindowBlueprint("EnemyResources");
            board.Reset();
            AddHotkey(PageUp, () => {
                board.player.resources = new Dictionary<string, int>
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
                CDesktop.RebuildAll();
            });
            AddHotkey(PageDown, () => {
                board.enemy.resources = new Dictionary<string, int>
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
                CDesktop.RebuildAll();
            });
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopMenuOpen");
                SpawnDesktopBlueprint("GameMenu");
            });
            AddHotkey(BackQuote, () => { SpawnDesktopBlueprint("DevPanel"); });
            AddHotkey(KeypadMultiply, () => { board.EndCombat("Won"); });
        }),
        new("FishingGame", () =>
        {
            locationName = fishingBoard.site.name;
            PlaySound("DesktopEnterCombat");
            SetDesktopBackground(fishingBoard.site.Background());
            SpawnWindowBlueprint("FishingBoard");
            SpawnWindowBlueprint("FishingBufferBoard");
            //SpawnWindowBlueprint("PlayerBattleInfo");
            SpawnWindowBlueprint("LocationInfo");
            //SpawnWindowBlueprint("EnemyBattleInfo");
            //SpawnWindowBlueprint("PlayerResources");
            //SpawnWindowBlueprint("EnemyResources");
            fishingBoard.Reset();
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopMenuOpen");
                CloseDesktop("FishingGame");
            });
            //AddHotkey(KeypadMultiply, () => { board.EndCombat("Won"); });
        }),
        new("CharacterSheet", () =>
        {
            PlaySound("DesktopCharacterSheetOpen");
            SetDesktopBackground("Stone");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("CharacterInfoStats");
            SpawnWindowBlueprint("CharacterInfoStatsRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopCharacterSheetClose");
                CloseDesktop("CharacterSheet");
            });
        }),
        new("TalentScreen", () =>
        {
            SetDesktopBackground("Stone");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("TalentTreeLeft");
            SpawnWindowBlueprint("TalentScreenHeader");
            SpawnWindowBlueprint("TalentTreeRight");
            var playerSpec = currentSave.player.Spec();
            for (int tree = 0; tree < 2; tree++)
                for (int row = 0; row < 5; row++)
                    for (int col = 0; col < 3; col++)
                        if (windowBlueprints.Exists(x => x.title == "TalentButton" + tree + row + col))
                            if (playerSpec.talentTrees[currentSave.lastVisitedTalents].talents.Exists(x => x.row == row && x.col == col && x.tree == tree))
                                SpawnWindowBlueprint("TalentButton" + tree + row + col);
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(A, () =>
            {
                PlaySound("DesktopSwitchPage");
                currentSave.lastVisitedTalents--;
                if (currentSave.lastVisitedTalents < 0)
                    currentSave.lastVisitedTalents = currentSave.player.Spec().talentTrees.Count - 1;
                CloseDesktop("TalentScreen");
                SpawnDesktopBlueprint("TalentScreen");
            });
            AddHotkey(D, () =>
            {
                PlaySound("DesktopSwitchPage");
                currentSave.lastVisitedTalents++;
                if (currentSave.lastVisitedTalents >= currentSave.player.Spec().talentTrees.Count)
                    currentSave.lastVisitedTalents = 0;
                CloseDesktop("TalentScreen");
                SpawnDesktopBlueprint("TalentScreen");
            });
            AddHotkey(Escape, () => { CloseDesktop("TalentScreen"); PlaySound("DesktopTalentScreenClose"); });
        }),
        new("SpellbookScreen", () =>
        {
            PlaySound("DesktopSpellbookScreenOpen");
            SetDesktopBackground("Skin");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("SpellbookAbilityListActivated");
            SpawnWindowBlueprint("PlayerSpellbookInfo");
            SpawnWindowBlueprint("SpellbookResources");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () => { SwitchDesktop("Map"); CloseDesktop("SpellbookScreen"); PlaySound("DesktopSpellbookScreenClose"); });
            AddPaginationHotkeys();
        }),
        new("EquipmentScreen", () =>
        {
            PlaySound("DesktopInventoryOpen");
            SetDesktopBackground("Leather");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInventoryClose");
                CloseDesktop("EquipmentScreen");
            });
        }),
        new("BestiaryScreen", () =>
        {
            PlaySound("DesktopInstanceOpen");
            SetDesktopBackground("Stone");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("BestiaryKalimdor");
            SpawnWindowBlueprint("BestiaryEasternKingdoms");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInstanceClose");
                CloseDesktop("BestiaryScreen");
            });
        }),
        new("CraftingScreen", () =>
        {
            PlaySound("DesktopInstanceOpen");
            SetDesktopBackground("Stone");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInstanceClose");
                CloseDesktop("CraftingScreen");
            });
        }),
        new("GameMenu", () =>
        {
            SetDesktopBackground("Leather");
            SpawnWindowBlueprint("GameMenu");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("Settings"))
                {
                    PlaySound("DesktopButtonClose");
                    SpawnWindowBlueprint("GameMenu");
                }
                else
                {
                    PlaySound("DesktopMenuClose");
                    CloseDesktop("GameMenu");
                }
            });
        }),
        new("RankingScreen", () =>
        {
            SetDesktopBackground("SkyRed");
            SpawnWindowBlueprint("CharacterRankingShadow");
            SpawnWindowBlueprint("CharacterRankingTop");
            SpawnWindowBlueprint("CharacterRankingList");
            SpawnWindowBlueprint("CharacterRankingListRight");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopButtonClose");
                CloseDesktop("RankingScreen");
            });
        }),

        #endregion

        #region Dev Panel

        new("DevPanel", () =>
        {
            #if (!UNITY_EDITOR)

            Serialize(races, "races", true, false, prefix);
            Serialize(specs, "Specs", true, false, prefix);
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
            Serialize(spiritHealers, "spirithealers", true, false, prefix);
            Serialize(personTypes, "personTypes", true, false, prefix);

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
            SpawnWindowBlueprint("Board");
            SpawnWindowBlueprint("BufferBoard");
            SpawnWindowBlueprint("PlayerBattleInfo");
            SpawnWindowBlueprint("LocationInfo");
            SpawnWindowBlueprint("EnemyBattleInfo");
            SpawnWindowBlueprint("PlayerResources");
            SpawnWindowBlueprint("EnemyResources");
            board.Reset();
            AddHotkey(PageUp, () => {
                board.player.resources = new Dictionary<string, int>
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
                CDesktop.RebuildAll();
            });
            AddHotkey(PageDown, () => {
                board.enemy.resources = new Dictionary<string, int>
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
                CDesktop.RebuildAll();
            });
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

        #endregion
    };

    public static void AddPaginationHotkeys()
    {
        AddHotkey(D, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            group.pagination += 1;
            var max = group.maxPagination();
            if (group.pagination >= max)
                group.pagination = max - 1;
            window.Respawn();
        });
        AddHotkey(D, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            group.pagination += (int)Math.Round(EuelerGrowth()) / 2;
            var max = group.maxPagination();
            if (group.pagination >= max)
                group.pagination = max - 1;
            window.Respawn();
        }, false);
        AddHotkey(A, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            group.pagination -= 1;
            if (group.pagination < 0)
                group.pagination = 0;
            window.Respawn();
        });
        AddHotkey(A, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            group.pagination -= (int)Math.Round(EuelerGrowth()) / 2;
            if (group.pagination < 0)
                group.pagination = 0;
            window.Respawn();
        }, false);
        AddHotkey(Alpha1, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            group.pagination = 0;
            window.Respawn();
        });
        AddHotkey(Alpha2, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 1.1);
            window.Respawn();
        });
        AddHotkey(Alpha3, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 2.2);
            window.Respawn();
        });
        AddHotkey(Alpha4, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 3.3);
            window.Respawn();
        });
        AddHotkey(Alpha5, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 4.4);
            window.Respawn();
        });
        AddHotkey(Alpha6, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 5.5);
            window.Respawn();
        });
        AddHotkey(Alpha7, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 6.6);
            window.Respawn();
        });
        AddHotkey(Alpha8, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPagination != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 7.7);
            window.Respawn();
        });
        AddHotkey(Alpha9, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 8.8);
            window.Respawn();
        });
        AddHotkey(Alpha0, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = max - 1;
            window.Respawn();
        });
    }
}
