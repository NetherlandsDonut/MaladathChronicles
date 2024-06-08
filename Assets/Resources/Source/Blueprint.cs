using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static UnityEngine.KeyCode;

using static Root;
using static Root.Anchor;
using static Root.RegionBackgroundType;

using static Item;
using static Race;
using static Zone;
using static Site;
using static Spec;
using static Quest;
using static Sound;
using static Mount;
using static Board;
using static Recipe;
using static Person;
using static Defines;
using static Faction;
using static Ability;
using static Enchant;
using static SitePath;
using static SaveGame;
using static Coloring;
using static PersonType;
using static Profession;
using static GameSettings;
using static FishingBoard;
using static PersonCategory;
using static SiteHostileArea;
using static SiteInstance;
using static SiteComplex;
using static SiteTown;
using System.Xml.Linq;
using System.Net.NetworkInformation;

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
            SetRegionGroupHeight(95);
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
            SetRegionGroupHeight(95);
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
        new("BoardFrame", () => {
            SetAnchor(-115, 146);
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
        }, true),
        new("Board", () => {
            SetAnchor(Top, 0, -15 + 19 * (board.field.GetLength(1) - 7));
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
            SetAnchor(Top, 0, 213 + 19 * (BufferBoard.bufferBoard.field.GetLength(1) - 7));
            MaskWindow();
            DisableGeneralSprites();
            DisableCollisions();
            AddRegionGroup();
            for (int i = 0; i < BufferBoard.bufferBoard.field.GetLength(1); i++)
                AddPaddingRegion(() =>
                {
                    for (int j = 0; j < BufferBoard.bufferBoard.field.GetLength(0); j++)
                        AddBigButton(BufferBoard.bufferBoard.GetFieldButton());
                });
        }, true),
        new("EnemyBattleInfo", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(190);
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
                AddBigButton(race.portrait == "" ? "OtherUnknown" : race.portrait + (race.genderedPortrait ? board.enemy.gender : ""));
                AddLine("Level: ", "DarkGray");
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
                        if (board.enemyCooldowns.ContainsKey(actionBar))
                        {
                            AddLine("" + board.enemyCooldowns[actionBar] + " / ", "DimGray", "Right");
                            AddText(actionBar, "Black");
                        }
                        else AddLine(actionBar, "", "Right");
                        AddSmallButton(abilityObj.icon);
                        if (!abilityObj.EnoughResources(board.enemy))
                        {
                            SetSmallButtonToGrayscale();
                            AddSmallButtonOverlay("OtherGridBlurred");
                        }
                        if (board.CooldownOn(false, actionBar) > 0)
                            AddSmallButtonOverlay("AutoCast");
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
        new("PlayerBattleInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddButtonRegion(
                () =>
                {
                    AddLine(board.player.name, "Black");
                    AddSmallButton("MenuFlee", (h) =>
                    {
                        board.EndCombat("Fled");
                    });
                }
            );
            AddHeaderRegion(() =>
            {
                if (board.player.spec != null)
                    AddBigButton(board.player.Spec().icon);
                else
                {
                    var race = races.Find(x => x.name == board.enemy.race);
                    AddBigButton(race.portrait == "" ? "OtherUnknown" : race.portrait + (race.genderedPortrait ? board.enemy.gender : ""));
                }
                AddLine("Level: " , "DarkGray");
                AddText("" + board.player.level, "Gray");
            });
            AddHealthBar(40, -38, "Player", board.player);
            foreach (var actionBar in board.player.actionBars)
            {
                var abilityObj = abilities.Find(x => x.name == actionBar);
                if (abilityObj == null || abilityObj.cost == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        if (board.playerCooldowns.ContainsKey(actionBar))
                        {
                            AddLine("" + board.playerCooldowns[actionBar] + " / ", "DimGray", "Right");
                            AddText(actionBar, "Black");
                        }
                        else AddLine(actionBar, "", "Right");
                        AddSmallButton(abilityObj.icon);
                        if (!abilityObj.EnoughResources(board.player))
                        {
                            SetSmallButtonToGrayscale();
                            AddSmallButtonOverlay("OtherGridBlurred");
                        }
                        if (board.CooldownOn(true, actionBar) > 0)
                            AddSmallButtonOverlay("AutoCast");
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
                        AddBigButton("Portrait" + slot.player.race.Clean() + (slot.player.Race().genderedPortrait ? slot.player.gender : ""));
                        AddBigButton("Class" + slot.player.spec);
                        AddLine(slot.player.name + ", a level " + slot.player.level + " ");
                        AddText(slot.player.spec, slot.player.spec);
                        AddLine("Score: " + slot.Score());
                        if (slot.playerDead) AddText(", died while fighting " + (slot.deathInfo.commonSource ? "a " : "") + slot.deathInfo.source + " in " + slot.deathInfo.area);
                    });
                }
                else
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("OtherBlank");
                        AddBigButton("OtherBlank");
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
                        AddBigButton("PVP" + (slot.player.Side() == "Alliance" ? "A" : "H") + slot.player.Rank().rank);
                    });
                }
                else
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("OtherBlank");
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
        }, true),
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
                            creationRace = "";
                            creationSpec = "";
                            creationGender = "";
                            String.creationName.Set("");
                            CloseWindow(h.window);
                            CloseWindow("RealmRoster");
                            CloseWindow("CharacterInfo");
                            CloseWindow("TitleScreenSingleplayer");
                            SpawnWindowBlueprint("CharacterCreationWho");
                            SpawnWindowBlueprint("CharacterCreationFinish");
                            SpawnWindowBlueprint("CharacterCreationFactionHorde");
                            SpawnWindowBlueprint("CharacterCreationFactionAlliance");
                            SetDesktopBackground("Leather");
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
                        if (saves[settings.selectedRealm].Count(x => !x.playerDead) > 0)
                        {
                            settings.selectedCharacter = saves[settings.selectedRealm].First(x => !x.playerDead).player.name;
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
        new("CharacterInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            if (settings.selectedCharacter != "")
            {
                var slot = saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter);
                var spec = slot.player.Spec();
                AddHeaderRegion(() => { AddLine("Character:"); });
                AddHeaderRegion(() =>
                {
                    AddBigButton("Portrait" + slot.player.race.Clean() + (slot.player.Race().genderedPortrait ? slot.player.gender : ""));
                    AddLine(slot.player.name, "Gray");
                    AddLine("Level: " + slot.player.level + " ", "Gray");
                    AddText(spec.name, spec.name);
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
        new("CharacterCreationFactionHorde", () => {
            SetAnchor(BottomLeft, 19, 19);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddBigButton("TabardOrgrimmar",
                (h) =>
                {
                    if (creationRace == "Orc") return;
                    creationRace = "Orc";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Orc").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Orcs of Orgrimmar", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Orcs are a proud and powerful race with warrior culture deeply rooted in honor and combat. Originally from the shattered world of Draenor, they now inhabit the harsh landscapes of Durotar, with their capital at Orgrimmar. They strive to overcome their dark past and build a new future for their people." }
                        }
                    } } } }.Print(null, null, 258, null);
                });
                if (creationRace != "Orc")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardDarkspearTribe",
                (h) =>
                {
                    if (creationRace == "Troll") return;
                    creationRace = "Troll";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Troll").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Trolls of the Darkspear Tribe", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Trolls are a fierce and agile race with a long history of mysticism and shamanism. The Darkspear Tribe, having allied with the Horde, has established itself in the Echo Isles and the coastal regions of Durotar. Known for their cunning and resourcefulness, they are formidable warriors and mystics." }
                        }
                    } } } }.Print(null, null, 258, null);
                });
                if (creationRace != "Troll")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardThunderBluff",
                (h) =>
                {
                    if (creationRace == "Tauren") return;
                    creationRace = "Tauren";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Tauren").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Tauren of Thunder Bluff", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Tauren are massive, bovine-like beings with a deep spiritual connection to nature and the Earth Mother. They dwell in the grassy plains of Mulgore, with their capital in Thunder Bluff. Renowned for their strength and wisdom, they serve as staunch protectors of the natural world." }
                        }
                    } } } }.Print(null, null, 258, null);
                });
                if (creationRace != "Tauren")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardUndercity",
                (h) =>
                {
                    if (creationRace == "Forsaken") return;
                    creationRace = "Forsaken";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Forsaken").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Forsaken of the Undercity", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "The Forsaken are former humans who have broken free from the Lich King's control, now seeking vengeance and a place in the world. They inhabit the eerie and decaying ruins of the Undercity, beneath the fallen kingdom of Lordaeron. Driven by a desire for autonomy and revenge, they are both feared and misunderstood by the living." }
                        }
                    } } } }.Print(null, null, 258, null);
                });
                if (creationRace != "Forsaken")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
            });
        }),
        new("CharacterCreationFactionAlliance", () => {
            SetAnchor(BottomRight, -19, 19);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddBigButton("TabardStormwind",
                (h) =>
                {
                    if (creationRace == "Human") return;
                    creationRace = "Human";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Human").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");

                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Humans of Stormwind", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Humans are a resilient and versatile race known for their unyielding spirit and strong sense of justice. They have a rich history of surviving numerous wars and catastrophes, making them natural leaders in the Alliance. Their capital city is Stormwind, a bustling hub of trade and governance." }
                        }
                    } } } }.Print(null, null, 258, null);
                });
                if (creationRace != "Human")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardIronforge",
                (h) =>
                {
                    if (creationRace == "Dwarf") return;
                    creationRace = "Dwarf";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Dwarf").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Dwarfs of Ironforge", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Dwarves are hardy and stout creatures famed for their skills in mining and blacksmithing. They hail from the snowy peaks of Dun Morogh and are deeply connected to their ancestral homeland of Ironforge. Their adventurous nature drives them to uncover ancient relics and forgotten lore." }
                        }
                    } } } }.Print(null, null, 258, null);
                });
                if (creationRace != "Dwarf")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardGnomeregan",
                (h) =>
                {
                    if (creationRace == "Gnome") return;
                    creationRace = "Gnome";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Gnome").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Gnomes of Gnomeregan", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Gnomes are brilliant inventors and tinkerers, known for their technological prowess and innovative gadgets. Originally from the subterranean city of Gnomeregan, many now reside with their Dwarven allies in Ironforge. Despite their small stature, they possess an insatiable curiosity and boundless energy." }
                        }
                    } } } }.Print(null, null, 258, null);
                });
                if (creationRace != "Gnome")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardDarnassus",
                (h) =>
                {
                    if (creationRace == "Night Elf") return;
                    creationRace = "Night Elf";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Night Elf").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Night Elfs of Darnassus", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Night Elves are an ancient and mystical race with a profound connection to nature and druidic magic. They once lived in isolation in the lush forests of Kalimdor with their majestic city of Darnassus. Renowned for their agility and wisdom, they strive to protect the world of nature from harm." }
                        }
                    } } } }.Print(null, null, 258, null);
                });
                if (creationRace != "Night Elf")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
            });
        }),
        new("CharacterCreationSpec", () => {
            SetAnchor(Top, 0, -19);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                var availableSpecs = specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace));
                foreach (var foo in availableSpecs)
                {
                    var spec = foo;
                    AddBigButton(spec.icon,
                    (h) =>
                    {
                        if (creationSpec == spec.name) return;
                        creationSpec = spec.name;
                        Respawn("CharacterCreationWho");
                        Respawn("CharacterCreationFactionRaceChoice");
                        CloseWindow("CharacterCreationFinish");
                        SpawnWindowBlueprint("CharacterCreationFinish");
                    },
                    null, (h) => () =>
                    {
                        SetAnchor(Top, 0, -76);
                        AddRegionGroup();
                        SetRegionGroupWidth(296);
                        AddHeaderRegion(() => AddLine(spec.name, "", "Center"));
                        Description desc = null;
                        if (spec.name == "Warlock")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Warlocks are feared practitioners of dark magic, summoning demons and wielding fel energies that corrupt and destroy. Often shunned by mainstream society, they are driven by a desire for power and knowledge forbidden to others. Warlocks walk a perilous path, balancing the destructive forces they command with the ever-present risk of their own corruption." }
                                }
                            } } } };
                        else if (spec.name == "Mage")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Mages are scholarly spellcasters who delve into the arcane arts, harnessing the raw energies of magic to alter reality. They are often members of esteemed magical institutions like the Kirin Tor, dedicating their lives to the pursuit of knowledge and mastery of the arcane. Mages wield immense power, capable of both creating wonders and unleashing devastating destruction." }
                                }
                            } } } };
                        else if (spec.name == "Priest")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Priests are devout servants of the divine, channeling the powers of the Light or the Void to heal and guide their followers. They serve as spiritual leaders within their communities, offering solace and wisdom in times of need. Whether upholding the Light's purity or delving into the shadows of the Void, priests are driven by their faith and commitment to their spiritual path." }
                                }
                            } } } };
                        else if (spec.name == "Rogue")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Rogues are shadowy figures who thrive in the underbelly of society, mastering the arts of stealth, subterfuge, and assassination. They are often found as spies, thieves, and mercenaries, using their cunning and agility to outmaneuver their foes. Rogues are the unseen enforcers of their factions, executing their tasks with lethal precision and leaving no trace behind." }
                                }
                            } } } };
                        else if (spec.name == "Warrior")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Warriors are the embodiment of physical strength and martial prowess, drawing on centuries-old traditions of combat and honor. They are often seen as the backbone of their respective societies, respected for their bravery and skill in battle. From the disciplined ranks of Stormwind's knights to the fierce clans of orcish berserkers, warriors are found in every culture, upholding their people's martial heritage." }
                                }
                            } } } };
                        else if (spec.name == "Druid")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Druids are guardians of nature who draw their strength from the natural world and the primal forces that govern it. They follow the teachings of ancient demigods like Cenarius, learning to shapeshift and harness the power of the wild. Druids are deeply connected to the balance of nature, serving as its protectors and stewards in a world often threatened by chaos and destruction." }
                                }
                            } } } };
                        else if (spec.name == "Shaman")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Shamans are spiritual mediators who commune with the elemental forces of nature, invoking the power of earth, fire, water, and air. They are deeply respected within their societies as guides and visionaries, capable of bridging the physical and spiritual worlds. Shamans draw upon ancient traditions and rituals, channeling the elements to maintain balance and harmony within the world." }
                                }
                            } } } };
                        else if (spec.name == "Paladin")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Paladins are holy warriors dedicated to the Light, wielding divine power to vanquish evil and protect the innocent. Originating from the ancient orders such as the Knights of the Silver Hand, they are bound by a sacred oath to uphold justice and righteousness. Paladins are revered as champions of their faith, standing as beacons of hope in the darkest times." }
                                }
                            } } } };
                        else if (spec.name == "Hunter")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Hunters are expert survivalists and marksmen, deeply attuned to the wilderness and the creatures that inhabit it. Often raised in the wilds, they form profound bonds with their animal companions and learn to navigate and master their environment. From the forests of Ashenvale to the savannas of the Barrens, hunters are the quintessential rangers and protectors of the natural world." }
                                }
                            } } } };
                        desc?.Print(null, null, 296, null);
                    });
                    if (creationSpec != spec.name)
                    {
                        AddBigButtonOverlay("OtherGridBlurred");
                        SetBigButtonToGrayscale();
                    }
                }
            });
        }),
        new("CharacterCreationFinish", () => {
            SetAnchor(Bottom, 0, 76);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("ActionReroll",
                (h) =>
                {
                    var temp = random.Next(8);
                    creationRace = temp == 0 ? "Orc" : (temp == 1 ? "Troll" : (temp == 2 ? "Tauren" : (temp == 3 ? "Forsaken" : (temp == 4 ? "Human" : (temp == 5 ? "Dwarf" : (temp == 6 ? "Gnome" : "Night Elf"))))));
                    creationGender = random.Next(2) == 0 ? "Male" : "Female";
                    var availableSpecs = specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace));
                    creationSpec = availableSpecs[random.Next(availableSpecs.Count)].name;
                    var name = "";
                    var race = races.Find(x => x.name == creationRace);
                    do name = creationGender == "Female" ? race.femaleNames[random.Next(race.femaleNames.Count)] : race.maleNames[random.Next(race.maleNames.Count)];
                    while (saves[settings.selectedRealm].Any(x => x.player.name == name));
                    String.creationName.Set(name);
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == creationRace).previewSite).Background());
                    CloseWindow("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFactionRaceChoice");
                    CloseWindow("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    CloseWindow("CharacterCreationWho");
                    SpawnWindowBlueprint("CharacterCreationFactionHorde");
                    SpawnWindowBlueprint("CharacterCreationFactionAlliance");
                    SpawnWindowBlueprint("CharacterCreationFactionRaceChoice");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationWho");
                    PlaySound("DesktopReroll" + random.Next(1, 3), 0.4f);
                });
            });
            AddRegionGroup();
            SetRegionGroupWidth(114);
            if (creationSpec != "" && creationGender != "" && creationRace != "" && String.creationName.Value().Length < 3) AddButtonRegion(() => { SetRegionBackground(RedButton); AddLine("Finish Creation", "Black", "Center"); });
            else if (creationSpec != "" && creationGender != "" && creationRace != "")
            {
                AddButtonRegion(() =>
                {
                    AddLine("Finish Creation", "", "Center");
                },
                (h) =>
                {
                    PlaySound("DesktopCreateCharacter");
                    AddNewSave();
                    CloseWindow("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFactionRaceChoice");
                    CloseWindow("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    CloseWindow("CharacterCreationWho");
                    SpawnWindowBlueprint("CharacterRoster");
                    SpawnWindowBlueprint("CharacterInfo");
                    SpawnWindowBlueprint("TitleScreenSingleplayer");
                    SaveGames();
                });
            }
            else AddPaddingRegion(() => AddLine("Finish Creation", "DarkGray", "Center"));
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("OtherClose",
                (h) =>
                {
                    CloseWindow("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFactionRaceChoice");
                    CloseWindow("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    CloseWindow("CharacterCreationWho");
                    SpawnWindowBlueprint("CharacterRoster");
                    SpawnWindowBlueprint("CharacterInfo");
                    SpawnWindowBlueprint("TitleScreenSingleplayer");
                });
            });
        }),
        new("CharacterCreationWho", () => {
            SetAnchor(Bottom, 0, 19);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddBigButton("Portrait" + creationRace.Clean() + "Male",
                (h) =>
                {
                    if (creationRace != "")
                    {
                        creationGender = "Male";
                        var oldName = String.creationName.Value();
                        var name = "";
                        var race = races.Find(x => x.name == creationRace);
                        do name = race.maleNames[random.Next(race.maleNames.Count)];
                        while (name == oldName || saves[settings.selectedRealm].Any(x => x.player.name == name));
                        String.creationName.Set(name);
                        CloseWindow("CharacterCreationWho");
                        Respawn("CharacterCreationWho");
                        CloseWindow("CharacterCreationFinish");
                        Respawn("CharacterCreationFinish");
                    }
                });
                if (creationRace != "" && creationGender != "Male")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
            });
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                if (creationRace == "") AddLine("Choose Race", "DarkGray", "Center");
                else if (creationGender == "" && creationSpec == "") AddLine("Choose Portrait and Class", "DarkGray", "Center");
                else if (creationSpec == "") AddLine("Choose Class", "DarkGray", "Center");
                else if (creationGender == "") AddLine("Choose Portrait", "DarkGray", "Center");
                else AddInputLine(String.creationName, "White", "Center");
            });
            AddPaddingRegion(() =>
            {
                AddLine(creationRace != "" ? creationRace + (creationSpec != "" ? " " + creationSpec : "") : "?", "", "Center");
            });
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddBigButton("Portrait" + creationRace.Clean() + "Female",
                (h) =>
                {
                    if (creationRace != "")
                    {
                        creationGender = "Female";
                        var oldName = String.creationName.Value();
                        var name = "";
                        var race = races.Find(x => x.name == creationRace);
                        do name = race.femaleNames[random.Next(race.femaleNames.Count)];
                        while (name == oldName || saves[settings.selectedRealm].Any(x => x.player.name == name));
                        String.creationName.Set(name);
                        CloseWindow("CharacterCreationWho");
                        Respawn("CharacterCreationWho");
                        CloseWindow("CharacterCreationFinish");
                        Respawn("CharacterCreationFinish");
                    }
                });
                if (creationRace != "" && creationGender != "Female")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
            });
        }),

        //Crafting Screen
        new("ProfessionListPrimary", () => {
            SetAnchor(-301, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var professions = Profession.professions.FindAll(x => currentSave.player.professionSkills.ContainsKey(x.name));
            AddHeaderRegion(() =>
            {
                AddLine("Primary professions:");
            });
            var primary = professions.Where(x => x.primary).ToList();
            for (int i = 0; i < defines.maxPrimaryProfessions; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (primary.Count() > index)
                    {
                        AddLine(primary[index].name);
                        AddLine("Skill: ", "DarkGray");
                        AddText(currentSave.player.professionSkills[primary[index].name].Item1 + "", "Gray");
                        AddText(" / ", "DarkGray");
                        AddText(primary[index].levels.FindAll(x => currentSave.player.professionSkills[primary[index].name].Item2.Contains(x.levelName)).Max(x => x.maxSkill) + "", "Gray");
                        AddBigButton(primary[index].icon,
                        (h) =>
                        {
                            profession = primary[index];
                            if (profession.recipeType == null) return;
                            CloseWindow("ProfessionListPrimary");
                            CloseWindow("ProfessionListSecondary");
                            Respawn("CraftingList");
                            PlaySound("DesktopInstanceOpen");
                        });
                    }
                    else AddBigButton("OtherDisabled");
                });
            }
        }),
        new("ProfessionListSecondary", () => {
            SetAnchor(-301, 28);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine("Secondary professions:");
            });
            var secondary = professions.Where(x => !x.primary).ToList();
            for (int i = 0; i < secondary.Count(); i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (currentSave.player.professionSkills.ContainsKey(secondary[index].name))
                    {
                        AddLine(secondary[index].name);
                        AddLine("Skill: ", "DarkGray");
                        AddText(currentSave.player.professionSkills[secondary[index].name].Item1 + "", "Gray");
                        AddText(" / ", "DarkGray");
                        AddText(secondary[index].levels.FindAll(x => currentSave.player.professionSkills[secondary[index].name].Item2.Contains(x.levelName)).Max(x => x.maxSkill) + "", "Gray");
                        AddBigButton(secondary[index].icon,
                        (h) =>
                        {
                            profession = secondary[index];
                            if (profession.recipeType == null) return;
                            CloseWindow("ProfessionListPrimary");
                            CloseWindow("ProfessionListSecondary");
                            Respawn("CraftingList");
                            PlaySound("DesktopInstanceOpen");
                        });
                    }
                    else AddBigButton("OtherDisabled");
                });
            }
        }),
        new("CraftingList", () => {
            SetAnchor(TopLeft, 19, -38);
            var recipes = currentSave.player.learnedRecipes[profession.name].Select(x => Recipe.recipes.Find(y => y.name == x)).Where(x => (!settings.onlyHavingMaterials.Value() || currentSave.player.CanCraft(x, true, true) > 0) && (!settings.onlySkillUp.Value() || x.skillUpGray > currentSave.player.professionSkills[profession.name].Item1)).ToList();
            AddRegionGroup(() => recipes.Count, 11);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                AddLine(profession.name);
                //AddLine("Skill: ", "DarkGray");
                //AddText(currentSave.player.professionSkills[profession.name].Item1 + "", "Gray");
                //AddText(" / ", "DarkGray");
                //AddText(profession.levels.FindAll(x => currentSave.player.professionSkills[profession.name].Item2.Contains(x.levelName)).Max(x => x.maxSkill) + "", "Gray");
                AddBigButton(profession.icon);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("CraftingRecipe");
                    CloseWindow("CraftingList");
                    Respawn("ProfessionListPrimary");
                    Respawn("ProfessionListSecondary");
                    PlaySound("DesktopInstanceClose");
                });
                AddSkillBar(40, -19, profession, currentSave.player);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Known " + profession.recipeType.ToLower() + (profession.recipeType.Last() == 's' ? ":" : "s:"), "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.player.learnedRecipes[profession.name].Reverse();
                    CloseWindow("CraftingList");
                    Respawn("CraftingList");
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "CraftingSettings") && !CDesktop.windows.Exists(x => x.title == "CraftingSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("CraftingSort");
                        CloseWindow("CraftingList");
                        Respawn("CraftingList");
                    });
                else
                    AddSmallButton("OtherSortOff");
                if (!CDesktop.windows.Exists(x => x.title == "CraftingSettings") && !CDesktop.windows.Exists(x => x.title == "CraftingSort"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("CraftingSettings");
                        CloseWindow("CraftingList");
                        Respawn("CraftingList");
                    });
                else
                    AddSmallButton("OtherSettingsOff");
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            for (int i = 0; i < 11; i++)
            {
                var index = i;
                if (recipes.Count > index + 11 * regionGroup.pagination())
                {
                    AddButtonRegion(() =>
                    {
                        var recipe = recipes[index + 11 * regionGroup.pagination()];
                        AddLine(recipe.name, "Black");
                        var amountPossible = currentSave.player.CanCraft(recipe, false, true);
                        AddText(amountPossible > 0 ? " [" + amountPossible + "]" : "", "Black");
                        AddSmallButton(recipe.Icon());
                        if (settings.rarityIndicators.Value() && recipe.results.Count > 0)
                            AddSmallButtonOverlay("OtherRarity" + items.Find(x => x.name == recipe.results.ToList()[0].Key) + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 2);
                    },
                    (h) =>
                    {
                        recipe = recipes[index + 11 * regionGroup.pagination()];
                        enchant = recipe.enchantment ? enchants.Find(x => x.name == recipe.name) : null;
                        if (enchantmentTarget != null && (enchant == null || enchant.type != enchantmentTarget.type))
                            enchantmentTarget = null;
                        Respawn("CraftingRecipe");
                        PlaySound("DesktopInstanceOpen");
                    });
                    var skill = currentSave.player.professionSkills[profession.name].Item1;
                    if (recipes[index + 11 * regionGroup.pagination()].skillUpYellow > skill)
                        SetRegionBackgroundAsImage("Sprites/Textures/SkillUpOrange");
                    else if (recipes[index + 11 * regionGroup.pagination()].skillUpGreen > skill)
                        SetRegionBackgroundAsImage("Sprites/Textures/SkillUpYellow");
                    else if (recipes[index + 11 * regionGroup.pagination()].skillUpGray > skill)
                        SetRegionBackgroundAsImage("Sprites/Textures/SkillUpGreen");
                }
                else if (recipes.Count == index + 11 * regionGroup.pagination())
                {
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
                }
            }
            AddPaginationLine(regionGroup);
        }),
        new("CraftingRecipe", () => {
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                AddLine(recipe.name + ":", "Gray");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("CraftingRecipe");
                    PlaySound("DesktopInstanceClose");
                });
            });
            if (recipe.results.Count > 0)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Results:", "Gray");
                });
                AddPaddingRegion(() =>
                {
                    var results = recipe.results.Select(x => items.Find(y => y.name == x.Key).CopyItem(x.Value)).ToList();
                    foreach (var result in results)
                    {
                        AddBigButton(result.icon, null, null, (h) => () =>
                        {
                            if (CDesktop.windows.Exists(x => x.title == "CraftingSort")) return;
                            if (CDesktop.windows.Exists(x => x.title == "CraftingSettings")) return;
                            PrintItemTooltip(result, Input.GetKey(LeftShift));
                        });
                        SpawnFloatingText(CDesktop.LBWindow.LBRegionGroup.LBRegion.transform.position + new Vector3(32, -27) + new Vector3(38, 0) * (results.IndexOf(result) % 5), result.amount + "", "", "Right");
                    }
                });
            }
            if (recipe.enchantment)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Enchantment:", "Gray");
                });
                AddPaddingRegion(() =>
                {
                    AddBigButton("AbilityGreaterHeal");
                });
            }
            if (recipe.reagents.Count > 0)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Reagents:", "Gray");
                });
                AddPaddingRegion(() =>
                {
                    var reagents = recipe.reagents.Select(x => items.Find(y => y.name == x.Key).CopyItem(x.Value)).ToList();
                    foreach (var reagent in reagents)
                    {
                        AddBigButton(reagent.icon, null, null, (h) => () =>
                        {
                            if (CDesktop.windows.Exists(x => x.title == "CraftingSort")) return;
                            if (CDesktop.windows.Exists(x => x.title == "CraftingSettings")) return;
                            PrintItemTooltip(reagent, Input.GetKey(LeftShift));
                        });
                        SpawnFloatingText(CDesktop.LBWindow.LBRegionGroup.LBRegion.transform.position + new Vector3(32, -27) + new Vector3(38, 0) * (reagents.IndexOf(reagent) % 5), currentSave.player.inventory.items.Sum(x => x.name == reagent.name ? x.amount : 0) + "/" + reagent.amount, "", "Right");
                    }
                });
            }
            if (recipe.enchantment)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Enchantment target:", "Gray");
                });
                AddPaddingRegion(() =>
                {
                    AddBigButton(enchantmentTarget != null ? enchantmentTarget.icon : "OtherUnknown",
                    (h) =>
                    {
                        CloseWindow("CraftingSort");
                        CloseWindow("CraftingSettings");
                        CloseWindow("CraftingList");
                        Respawn("EnchantingList");
                    },
                    null,
                    (h) => () =>
                    {
                        if (enchantmentTarget == null) return;
                        if (CDesktop.windows.Exists(x => x.title == "CraftingSort")) return;
                        if (CDesktop.windows.Exists(x => x.title == "CraftingSettings")) return;
                        PrintItemTooltip(enchantmentTarget, Input.GetKey(LeftShift));
                    });
                });
            }
            AddPaddingRegion(() =>
            {
                if (recipe.enchantment && enchantmentTarget?.enchant != null)
                {
                    AddLine("This process will override", "DangerousRed");
                    AddLine("the previous enchantment!", "DangerousRed");
                }
                SetRegionAsGroupExtender();
            });
            if (currentSave.player.CanCraft(recipe) > 0 && (!recipe.enchantment || enchantmentTarget != null))
                AddButtonRegion(() =>
                {
                    AddLine(recipe.enchantment ? "Enchant" : "Craft");
                },
                (h) =>
                {
                    var crafted = currentSave.player.Craft(recipe);
                    var skill = currentSave.player.professionSkills;
                    if (recipe.skillUpYellow > skill[recipe.profession].Item1)
                        skill[recipe.profession] = (skill[recipe.profession].Item1 + 1, skill[recipe.profession].Item2);
                    else if (recipe.skillUpGreen > skill[recipe.profession].Item1 && Roll(75))
                        skill[recipe.profession] = (skill[recipe.profession].Item1 + 1, skill[recipe.profession].Item2);
                    else if (recipe.skillUpGray > skill[recipe.profession].Item1 && Roll(25))
                        skill[recipe.profession] = (skill[recipe.profession].Item1 + 1, skill[recipe.profession].Item2);
                    foreach (var item in crafted)
                    {
                        currentSave.player.inventory.AddItem(item);
                        PlaySound(item.ItemSound("PutDown"), 0.6f);
                    }
                    if (recipe.enchantment)
                    {
                        enchantmentTarget.enchant = enchant;
                        enchantmentTarget = null;
                        PlaySound("PutDownGems", 0.6f);
                    }
                    Respawn("CraftingList");
                    CloseWindow("CraftingRecipe");
                    SpawnWindowBlueprint("CraftingRecipe");
                });
            else
                AddPaddingRegion(() =>
                {
                    AddLine(recipe.enchantment ? "Enchant" : "Craft", "DarkGray");
                });
        }),
        new("CraftingSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort " + profession.recipeType.ToLower() + (profession.recipeType.Last() == 's' ? ":" : "s:"), "Gray");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("CraftingSort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                currentSave.player.learnedRecipes[profession.name] = currentSave.player.learnedRecipes[profession.name].OrderBy(x => recipes.Find(y => y.name == x).name).ToList();
                CloseWindow("CraftingSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By possible crafts", "Black");
            },
            (h) =>
            {
                currentSave.player.learnedRecipes[profession.name] = currentSave.player.learnedRecipes[profession.name].OrderByDescending(x => currentSave.player.CanCraft(recipes.Find(y => y.name == x), false, true)).ToList();
                CloseWindow("CraftingSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By skill up", "Black");
            },
            (h) =>
            {
                currentSave.player.learnedRecipes[profession.name] = currentSave.player.learnedRecipes[profession.name].OrderByDescending(x => recipes.Find(y => y.name == x).skillUpYellow).ToList();
                CloseWindow("CraftingSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
        }),
        new("CraftingSettings", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Recipe list settings:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("CraftingSettings");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Has materials", "Black");
                AddCheckbox(settings.onlyHavingMaterials);
            },
            (h) =>
            {
                settings.onlyHavingMaterials.Invert();
                CDesktop.RespawnAll();
            });
            AddButtonRegion(() =>
            {
                AddLine("Has skill up", "Black");
                AddCheckbox(settings.onlySkillUp);
            },
            (h) =>
            {
                settings.onlySkillUp.Invert();
                CDesktop.RespawnAll();
            });
        }),
        new("EnchantingList", () => {
            SetAnchor(TopLeft, 19, -38);
            var possibleItems = currentSave.player.inventory.items.Concat(currentSave.player.equipment.Select(x => x.Value)).Where(x => x.type == enchant.type).OrderBy(x => x.name).ToList();
            AddRegionGroup(() => possibleItems.Count, 12);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                AddLine("Possible items:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("EnchantingList");
                    Respawn("CraftingList");
                });
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            for (int i = 0; i < 12; i++)
            {
                var index = i;
                if (possibleItems.Count > index + 12 * regionGroup.pagination())
                    AddButtonRegion(() =>
                    {
                        var item = possibleItems[index + 12 * regionGroup.pagination()];
                        AddLine(item.name);
                        AddSmallButton(item.icon, null, null,
                        (h) => () =>
                        {
                            if (CDesktop.windows.Exists(x => x.title == "CraftingSort")) return;
                            if (CDesktop.windows.Exists(x => x.title == "CraftingSettings")) return;
                            PrintItemTooltip(item, Input.GetKey(LeftShift));
                        });
                    },
                    (h) =>
                    {
                        var item = possibleItems[index + 12 * regionGroup.pagination()];
                        enchantmentTarget = item;
                        PlaySound("DesktopEnchantingTarget");
                        CloseWindow("EnchantingList");
                        Respawn("CraftingRecipe");
                        Respawn("CraftingList");
                    });
                else if (possibleItems.Count == index + 12 * regionGroup.pagination())
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
            }
            AddPaginationLine(regionGroup);
        }),

        //Quest Log
        new("QuestList", () => {
            SetAnchor(TopLeft, 19, -38);
            var quests = currentSave.player.currentQuests;
            AddRegionGroup(() => quests.Count, 11);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                AddLine("Quest Log");
                AddLine("Current quests: ", "DarkGray");
                AddText("" + currentSave.player.currentQuests.Count, "Gray");
                AddBigButton("MenuQuestLog");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Current quests:", "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.player.currentQuests.Reverse();
                    CloseWindow("QuestList");
                    Respawn("QuestList");
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "QuestSettings") && !CDesktop.windows.Exists(x => x.title == "QuestSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("QuestSort");
                        CloseWindow("QuestList");
                        Respawn("QuestList");
                    });
                else
                    AddSmallButton("OtherSortOff");
                if (!CDesktop.windows.Exists(x => x.title == "QuestSettings") && !CDesktop.windows.Exists(x => x.title == "QuestSort"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("QuestSettings");
                        CloseWindow("QuestList");
                        Respawn("QuestList");
                    });
                else
                    AddSmallButton("OtherSettingsOff");
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            for (int i = 0; i < 11; i++)
            {
                var index = i;
                if (quests.Count > index + 11 * regionGroup.pagination())
                {
                    AddButtonRegion(() =>
                    {
                        var quest = quests[index + 11 * regionGroup.pagination()];
                        AddLine((settings.questLevel.Value() ? "[" + quest.questLevel + "] " : "") + quest.name, "Black");
                        AddSmallButton(quest.Icon());
                    },
                    (h) =>
                    {
                        quest = quests[index + 11 * regionGroup.pagination()];
                        Respawn("Quest");
                        PlaySound("DesktopInstanceOpen");
                    });
                    var color = ColorQuestLevel(quests[index + 11 * regionGroup.pagination()].questLevel);
                    if (color != null) SetRegionBackgroundAsImage("Sprites/Textures/SkillUp" + color);
                }
                else if (quests.Count == index + 11 * regionGroup.pagination())
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
            }
            AddPaginationLine(regionGroup);
        }),
        new("Quest", () => {
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                AddLine(quest.name, "Black");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("Quest");
                    PlaySound("DesktopInstanceClose");
                });
            });
            var color = ColorQuestLevel(quest.questLevel);
            if (color != null) SetRegionBackgroundAsImage("Sprites/Textures/SkillUp" + color);
            if (quest.description != null)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Description:");
                });
                new Description()
                { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                    {
                        { "Color", "DarkGray" },
                        { "Text", quest.description }
                    }
                } } } }.Print(null, null, 190, null);
            }
            AddHeaderRegion(() =>
            {
                AddLine("Details:");
            });
            foreach (var condition in quest.conditions)
                condition.Print();
        }),
        new("QuestSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort quests:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("QuestSort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                currentSave.player.currentQuests = currentSave.player.currentQuests.OrderBy(x => x.name).ToList();
                CloseWindow("QuestSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By quest level", "Black");
            },
            (h) =>
            {
                currentSave.player.currentQuests = currentSave.player.currentQuests.OrderBy(x => x.questLevel).ToList();
                CloseWindow("QuestSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By zone", "Black");
            },
            (h) =>
            {
                currentSave.player.currentQuests = currentSave.player.currentQuests.OrderBy(x => x.zone).ToList();
                CloseWindow("QuestSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.2f);
            });
        }),
        new("QuestSettings", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Quest Log settings:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("QuestSettings");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Show quest level", "Black");
                AddCheckbox(settings.questLevel);
            },
            (h) =>
            {
                settings.questLevel.Invert();
                CDesktop.RespawnAll();
            });
        }),

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
                        AddSmallButton("OtherEmpty");
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
                    AddSmallButton("OtherSortOff");
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
                    AddSmallButton("OtherSettingsOff");
            });
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (index * 5 + j >= currentSave.player.inventory.BagSpace()) AddBigButton("OtherDisabled");
                            else if (items.Count > index * 5 + j) PrintInventoryItem(items[index * 5 + j]);
                            else AddBigButton("OtherEmpty");
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
                        Respawn("HostileArea");
                        Respawn("HostileAreaProgress");
                        Respawn("HostileAreaDenizens");
                        Respawn("HostileAreaElites");
                        SetDesktopBackground(area.Background());
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
        
        //Complex
        new("Complex", () => 
        {
            PlayAmbience(complex.ambience);
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            AddHeaderRegion(() =>
            {
                AddLine(complex.name, "Gray");
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
        }),

        //Instance
        new("Instance", () => 
        {
            PlayAmbience(instance.ambience);
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            AddHeaderRegion(() =>
            {
                AddLine(instance.name, "Gray");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    var title = CDesktop.title;
                    CloseDesktop(title);
                    if (instance.complexPart)
                        SpawnDesktopBlueprint("Complex");
                    else
                    {
                        PlaySound("DesktopInstanceClose");
                        SwitchDesktop("Map");
                    }
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Level range: ", "DarkGray");
                var range = instance.LevelRange();
                AddText(range.Item1 + "", ColorEntityLevel(range.Item1));
                AddText(" - ", "DarkGray");
                AddText(range.Item2 + "", ColorEntityLevel(range.Item2));
            });
            foreach (var wing in instance.wings)
                PrintInstanceWing(instance, wing);
        }),

        //Hostile Area
        new("HostileArea", () => 
        {
            if (area.ambience == null)
            {
                var zone = zones.Find(x => x.name == area.zone);
                if (zone != null) PlayAmbience(currentSave.IsNight() ? zone.ambienceNight : zone.ambienceDay);
            }
            else PlayAmbience(area.ambience);
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine(area.name, "Gray");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    PlaySound("DesktopInstanceClose");
                    if (area.instancePart)
                    {
                        SetDesktopBackground(instance.Background());
                        CloseWindow(h.window);
                        CloseWindow("BossQueue");
                        CloseWindow("HostileAreaProgress");
                        CloseWindow("HostileAreaDenizens");
                        CloseWindow("HostileAreaElites");
                    }
                    else if (area.complexPart)
                    {
                        SetDesktopBackground(complex.Background());
                        CloseWindow(h.window);
                        CloseWindow("BossQueue");
                        CloseWindow("HostileAreaProgress");
                        CloseWindow("HostileAreaDenizens");
                        CloseWindow("HostileAreaElites");
                    }
                    else CloseDesktop("HostileArea");
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Recommended level: ", "DarkGray");
                AddText(area.recommendedLevel + "", ColorEntityLevel(area.recommendedLevel));
            });
            AddButtonRegion(() => { AddLine("Explore", "Black"); },
            (h) =>
            {
                NewBoard(area.RollEncounter(), area);
                SpawnDesktopBlueprint("Game");
                SwitchDesktop("Game");
            });
        }),
        new("HostileAreaProgress", () => 
        {
            SetAnchor(BottomLeft, 19, 35);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine("Exploration progress:", "Gray");
            });
            var thickness = 5;
            if (area.progression != null && area.progression.Count > 0)
                for (int i = 0; i <= area.areaSize; i++)
                {
                    var index = i;
                    if (index > 0)
                    {
                        var progressions = area.progression.FindAll(x => x.point == index);
                        var printType = "";
                        if (progressions.Exists(x => x.type == "Boss") && progressions.Exists(x => x.type == "Area")) printType = "BossArea";
                        else if (progressions.Exists(x => x.type == "Treasure") && progressions.Exists(x => x.type == "Area")) printType = "TreasureArea";
                        else if (progressions.Exists(x => x.type == "Boss")) printType = "Boss";
                        else if (progressions.Exists(x => x.type == "Treasure")) printType = "Treasure";
                        else if (progressions.Exists(x => x.type == "Area")) printType = "Area";
                        if (printType != "")
                        {
                            var marker = new GameObject("ProgressionMarker", typeof(SpriteRenderer));
                            marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/Progress" + printType);
                            marker.transform.parent = CDesktop.LBWindow.LBRegionGroup.LBRegion.transform;
                            marker.transform.localPosition = new Vector3(1 + CDesktop.LBWindow.LBRegionGroup.setWidth, -3 - thickness);
                        }
                    }
                    if (i < area.areaSize)
                    {
                        AddRegionGroup();
                        SetRegionGroupWidth((i == area.areaSize - 1 ? 190 % area.areaSize : 0) + 190 / area.areaSize);
                        SetRegionGroupHeight(thickness);
                        AddPaddingRegion(() =>
                        {
                            var temp = currentSave.siteProgress.ContainsKey(area.name) ? currentSave.siteProgress[area.name] : 0;
                            if (temp > index) SetRegionBackground(ProgressDone);
                            else SetRegionBackground(ProgressEmpty);
                        });
                    }
                }
        }),
        new("HostileAreaDenizens", () => 
        {
            SetAnchor(TopLeft, 19, -95);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            if (area.commonEncounters != null && area.commonEncounters.Count > 0)
                foreach (var encounter in area.commonEncounters)
                    AddPaddingRegion(() =>
                    {
                        AddLine(encounter.who, "DarkGray", "Right");
                        var race = races.Find(x => x.name == encounter.who);
                        AddSmallButton(race == null ? "OtherUnknown" : race.portrait);
                    });
        }),
        new("HostileAreaElites", () =>
        {
            if (area.eliteEncounters == null || area.eliteEncounters.Count == 0) return;
            SetAnchor(BottomLeft, 19, 82);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            foreach (var encounter in area.eliteEncounters)
                AddPaddingRegion(() =>
                {
                    AddLine(encounter.who);
                    AddLine("Level: ", "DarkGray");
                    AddText("" + encounter.levelMin, ColorEntityLevel(encounter.levelMin));
                    var race = races.Find(x => x.name == encounter.who);
                    AddBigButton(race == null ? "OtherUnknown" : race.portrait,
                        (h) =>
                        {
                            NewBoard(area.RollEncounter(encounter), area);
                            SpawnDesktopBlueprint("Game");
                            SwitchDesktop("Game");
                        }
                    );
                });
        }),

        //Town
        new("Town", () => 
        {
            PlayAmbience(town.ambience);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine(town.name, "Gray");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    var title = CDesktop.title;
                    CloseDesktop(title);
                    PlaySound("DesktopInstanceClose");
                    SwitchDesktop("Map");
                });
            });
            if (CDesktop.windows.Exists(x => x.title == "Persons")) return;
            if (transportationConnectedToSite.ContainsKey(town.name))
            {
                var transportOptions = transportationConnectedToSite[town.name];
                AddPaddingRegion(() => { AddLine("Transportation:"); });
                foreach (var transport in transportOptions)
                {
                    var desitnationName = transport.sites.Find(x => x != town.name);
                    var destination = towns.Find(x => x.name == desitnationName);
                    if (destination == null) continue;
                    AddButtonRegion(() =>
                    {
                        AddLine(desitnationName, "Black");
                        AddSmallButton("Transport" + transport.means);
                    },
                    (h) =>
                    {
                        if (transport.price > 0)
                        {
                            if (transport.price > currentSave.player.inventory.money) return;
                            PlaySound("DesktopTransportPay");
                            currentSave.player.inventory.money -= transport.price;
                        }

                        //Close town screen as we're beginning to travel on map
                        CloseDesktop("Town");

                        //Switch desktop to map
                        SwitchDesktop("Map");

                        //Lead path to the destination
                        LeadPath(transport, true);

                        //Queue moving player to the destination
                        destination.ExecutePath("Town");
                    },
                    null,
                    (h) => () => { transport.PrintTooltip(); });
                }
            }
            if (town.people != null)
            {
                var groups = town.people.Where(x => !x.hidden).OrderBy(x => x.type).GroupBy(x => x.category).OrderBy(x => x.Count()).ThenBy(x => x.Key != null ? x.Key.priority : 0);
                AddPaddingRegion(() => { AddLine("Points of interest:", "Gray"); });
                foreach (var group in groups)
                    if (group.Key == null) continue;
                    else if (group.Key.category == "Flight Master")
                        foreach (var person in group)
                        {
                            var faction = factions.Find(x => x.name == person.faction);
                            faction ??= factions.Find(x => x.name == races.Find(y => y.name == person.race).faction);
                            faction ??= factions.Find(x => x.name == currentSave.player.faction);
                            if (faction.side == currentSave.player.Side())
                            {
                                var personType = personTypes.Find(x => x.type == person.type);
                                AddButtonRegion(() =>
                                {
                                    AddLine(person.name, "Black");
                                    AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? faction.side : "") : "OtherUnknown");
                                },
                                (h) =>
                                {
                                    Person.person = person;
                                    CloseWindow(h.window.title);
                                    Respawn("Person");
                                    PlaySound("DesktopInstanceOpen");
                                });
                            }
                        }
                    else if (group.Count() == 1)
                        foreach (var person in group)
                        {
                            var personType = personTypes.Find(x => x.type == person.type);
                            AddButtonRegion(() =>
                            {
                                AddLine(person.name, "Black");
                                AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? factions.Find(x => x.name == town.faction).side : "") : "OtherUnknown");
                            },
                            (h) =>
                            {
                                Person.person = person;
                                CloseWindow(h.window.title);
                                Respawn("Person");
                                PlaySound("DesktopInstanceOpen");
                            });
                        }
                    else
                    {
                        var person = group.First();
                        AddButtonRegion(() =>
                        {
                            AddLine(group.Key.category + "s (" + group.Count() + ")", "Black");
                            AddSmallButton(person.category != null ? person.category.icon + (person.category.factionVariant ? factions.Find(x => x.name == town.faction).side : "") : "OtherUnknown");
                        },
                        (h) =>
                        {
                            personCategory = group.Key;
                            CloseWindow("Person");
                            Respawn("Persons");
                            PlaySound("DesktopInstanceOpen");
                        });
                    }
            }
        }),
        new("TownHostile", () => 
        {
            PlayAmbience(town.ambience);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
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
                var rank = currentSave.player.ReputationRank(town.faction);
                if (rank == "Hated")
                {
                    AddLine("This town's folk consider you");
                    AddLine("to be their enemy.");
                }
                else if (rank == "Hostile")
                {
                    AddLine("This town's folk consider you");
                    AddLine("to be an enemy.");
                }
                else if (rank == "Unfriendly")
                {
                    AddLine("This town's folk are reluctant");
                    AddLine("towards you.");
                    AddLine("Consider improving your reputation");
                    AddLine("with " + town.faction + " in order");
                    AddLine("to be welcomed here.");
                }
            });
        }),
        new("Person", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var type = personTypes.Find(x => x.type == person.type);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""));
            });
            if (type.category == "Trainer")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I seek training.");
                });
                AddButtonRegion(() =>
                {
                    AddLine("I want to reset my talents.");
                });
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
                    CloseWindow("Town");
                    SpawnWindowBlueprint("ProfessionLevelTrainer");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
                var pr = professions.Find(x => x.name == type.profession);
                if (pr == null) Debug.Log("ERROR 013: Profession was not found: \"" + type.profession + "\"");
                else
                {
                    var rt = professions.Find(x => x.name == type.profession).recipeType;
                    if (rt != null)
                        AddButtonRegion(() =>
                        {
                            AddLine("I would like to learn " + rt.ToLower() + (rt.Last() == 's' ? "." : "s."));
                        },
                        (h) =>
                        {
                            PlaySound("DesktopInstanceOpen", 0.2f);
                            CloseWindow(h.window);
                            CloseWindow("Town");
                            SpawnWindowBlueprint("ProfessionRecipeTrainer");
                            Respawn("ExperienceBarBorder");
                            Respawn("ExperienceBar");
                        });
                }
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
                    CloseWindow("Town");
                    SpawnWindowBlueprint("Bank");
                    SpawnWindowBlueprint("Inventory");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
            }
            else if (type.category == "Innkeeper")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to rest in this inn.");
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
                        Respawn("ExperienceBarBorder");
                        Respawn("ExperienceBar");
                    });
            }
            else if (type.category == "Battlemaster")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to enter the arena.");
                });
                AddButtonRegion(() =>
                {
                    AddLine("I want to buy equipment.");
                });
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
                    CloseWindow("Town");
                    SpawnWindowBlueprint("MountCollection");
                    if (mounts.Find(x => x.name == currentSave.player.mount) != null)
                        SpawnWindowBlueprint("CurrentMount");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
                if (mounts.Count(x => !currentSave.player.mounts.Contains(x.name) && x.factions != null && x.factions.Contains(person.faction == null ? town.faction : person.faction)) > 0)
                    AddButtonRegion(() =>
                    {
                        AddLine("I want to buy a new mount.");
                    },
                    (h) =>
                    {
                        PlaySound("DesktopInventoryOpen");
                        CloseWindow(h.window);
                        CloseWindow("Town");
                        SpawnWindowBlueprint("MountVendor");
                        Respawn("ExperienceBarBorder");
                        Respawn("ExperienceBar");
                    });
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
                    CloseWindow("Town");
                    SpawnWindowBlueprint("FlightMaster");
                    if (mounts.Find(x => x.name == currentSave.player.mount) != null)
                        SpawnWindowBlueprint("CurrentMount");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
            }
            if (person.itemsSold != null && person.itemsSold.Count > 0)
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to browse your goods.");
                },
                (h) =>
                {
                    if (!currentSave.vendorStock.ContainsKey(town.name + ":" + person.name) && person.itemsSold != null && person.itemsSold.Count > 0)
                        currentSave.vendorStock.Add(town.name + ":" + person.name, person.ExportStock());
                    PlaySound("DesktopInventoryOpen");
                    CloseWindow(h.window);
                    CloseWindow("Town");
                    SpawnWindowBlueprint("Vendor");
                    SpawnWindowBlueprint("Inventory");
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
                Respawn("Town");
                Respawn("Persons", true);
            });
        }, true),
        new("Persons", () => {
            SetAnchor(TopLeft, 19, -57);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddPaddingRegion(() =>
            {
                AddLine(personCategory.category + "s:", "Gray");
                AddSmallButton("OtherReverse",
                (h) =>
                {
                    personCategory = null;
                    CloseWindow(h.window.title);
                    Respawn("Town");
                    PlaySound("DesktopInstanceClose");
                });
            });
            var people = town.people.FindAll(x => x.category == personCategory && !x.hidden);
            foreach (var person in people)
            {
                var personType = personTypes.Find(x => x.type == person.type);
                AddButtonRegion(() =>
                {
                    AddLine(person.name, "Black");
                    AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? factions.Find(x => x.name == town.faction).side : "") : "OtherUnknown");
                },
                (h) =>
                {
                    Person.person = person;
                    Respawn("Person");
                    CloseWindow("Persons");
                    CloseWindow("Town");
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
                            if (index * 5 + j >= 999) AddBigButton("OtherNoSlot");
                            else if (items.Count > index * 5 + j) PrintVendorItem(items[index * 5 + j], null);
                            else AddBigButton("OtherEmpty");
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
                            else AddBigButton("OtherEmpty");
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
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""));
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
        new("MountCollection", () => {
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
                    CloseWindow("MountCollection");
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
                    Respawn("MountCollection");
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "MountsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("MountsSort");
                        Respawn("MountCollection");
                    });
                else
                    AddSmallButton("OtherSortOff");
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
                    if (mounts.Count > index + 6 * regionGroup.pagination())
                    {
                        var mount = mounts[index + 6 * regionGroup.pagination()];
                        AddLine(mount.name);
                        AddLine("Speed: ", "DarkGray");
                        AddText(mount.speed == 7 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"));
                        AddBigButton(mount.icon,
                            (h) =>
                            {
                                var mount = mounts[index + 6 * regionGroup.pagination()];
                                if (currentSave.player.mount != mount.name && currentSave.player.level >= (mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts))
                                {
                                    currentSave.player.mount = mount.name;
                                    Respawn("MountCollection");
                                    Respawn("CurrentMount");
                                    PlaySound("DesktopActionbarAdd", 0.7f);
                                }
                            },
                            null,
                            (h) => () =>
                            {
                                var mount = mounts[index + 6 * regionGroup.pagination()];
                                PrintMountTooltip(currentSave.player, mount);
                            }
                        );
                        if (currentSave.player.level < (mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts)) SetBigButtonToRed();
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled");
                    }
                });
            }
        }),
        new("MountVendor", () => {
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup(() => Mount.mounts.Count(x => !currentSave.player.mounts.Contains(x.name) && x.factions != null && x.factions.Contains(person.faction == null ? town.faction : person.faction)), 6);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(285);
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("MountVendor");
                    Respawn("Person");
                    PlaySound("DesktopInstanceClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Available mounts:");
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            var mounts = Mount.mounts.FindAll(x => !currentSave.player.mounts.Contains(x.name) && x.factions != null && x.factions.Contains(person.faction == null ? town.faction : person.faction)).OrderBy(x => x.speed).ThenBy(x => x.price).ThenBy(x => x.name).ToList();
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                if (mounts.Count >= index + 6 * regionGroup.pagination())
                    AddPaddingRegion(() =>
                    {
                        if (mounts.Count > index + 6 * regionGroup.pagination())
                        {
                            var mount = mounts[index + 6 * regionGroup.pagination()];
                            AddLine(mount.name);
                            AddLine("Speed: ", "DarkGray");
                            AddText(mount.speed == 7 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"));
                            AddBigButton(mount.icon,
                                (h) =>
                                {
                                    var mount = mounts[index + 6 * regionGroup.pagination()];
                                    if (currentSave.player.inventory.money >= mount.price)
                                    {
                                        currentSave.player.inventory.money -= mount.price;
                                        currentSave.player.mounts.Add(mount.name);
                                        Respawn("MountVendor");
                                        PlaySound("DesktopTransportPay");
                                    }
                                },
                                null,
                                (h) => () =>
                                {
                                    var mount = mounts[index + 6 * regionGroup.pagination()];
                                    PrintMountTooltip(currentSave.player, mount);
                                }
                            );
                            if (currentSave.player.level < (mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts))
                                SetBigButtonToRed();
                        }
                        else if (mounts.Count == index + 6 * regionGroup.pagination())
                        {
                            SetRegionBackground(Padding);
                            AddLine("");
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
                    AddText(mount.speed == 7 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"));
                    AddBigButton(mount.icon);
                }
                else AddBigButton("OtherDisabled");
            });
            var mount = mounts.Find(x => x.name == currentSave.player.mount);
            if (CDesktop.windows.Exists(x => x.title == "MountCollection"))
                AddButtonRegion(() =>
                {
                    AddLine("Dismount");
                },
                (h) =>
                {
                    currentSave.player.mount = "";
                    Respawn("MountCollection");
                    CloseWindow(h.window);
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
                    AddSmallButton("OtherSortOff");
            });
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (index * 5 + j >= currentSave.banks[town.name].BagSpace()) AddBigButton("OtherNoSlot");
                            else if (items.Count > index * 5 + j) PrintBankItem(items[index * 5 + j]);
                            else AddBigButton("OtherEmpty");
                    }
                );
            }
            AddHeaderRegion(() =>
            {
                AddLine("");
            });
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
            var destinations = town.flightPaths[side].FindAll(x => x != town).OrderBy(x => !currentSave.siteVisits.ContainsKey(x.name)).ThenBy(x => x.zone).ThenBy(x => x.name).ToList();
            AddRegionGroup(() => destinations.Count, 12);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
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
            for (int i = 0; i < 12; i++)
            {
                var index = i;
                if (destinations.Count > index + 12 * regionGroup.pagination())
                    AddButtonRegion(() =>
                    {
                        var destination = destinations[index + 12 * regionGroup.pagination()];
                        if (currentSave.siteVisits.ContainsKey(destination.name))
                        {
                            AddLine(destination.name);
                            AddSmallButton("Zone" + destination.zone.Clean());
                        }
                        else
                        {
                            SetRegionBackground(Header);
                            AddLine("?", "DarkGray");
                            AddSmallButton("OtherDisabled");
                        }
                    },
                    (h) =>
                    {
                        var destination = destinations[index + 12 * regionGroup.pagination()];
                        currentSave.currentSite = destination.name;
                        Respawn("Site: " + town.name);
                        Respawn("Site: " + currentSave.currentSite);
                        town = destination;

                        //if (transport.price > 0)
                        //{
                        //    if (transport.price > currentSave.player.inventory.money) return;
                        //    PlaySound("DesktopTransportPay");
                        //    currentSave.player.inventory.money -= transport.price;
                        //}

                        //Close town screen as we're beginning to travel on map
                        CloseDesktop("Town");

                        //Switch desktop to map
                        SwitchDesktop("Map");

                        //Move camera to the newly visited town
                        CDesktop.cameraDestination = new Vector2(town.x, town.y);

                        ////Find current site
                        //var current = FindSite(x => x.name == currentSave.currentSite);

                        ////Lead path to the destination
                        //LeadPath(new SitePath() { means = "Flight", sites = new() { current.name, town.name }, points = new() { (town.x, town.y), (current.x, current.y) }, spacing = 9999 }, true);

                        ////Queue moving player to the destination
                        //town.ExecutePath("Town");
                    });
                else if (destinations.Count == index + 12 * regionGroup.pagination())
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
            }
            AddPaginationLine(regionGroup);
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
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""));
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
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (levels.Count > index + 6 * regionGroup.pagination())
                    {
                        var key = levels[index + 6 * regionGroup.pagination()];
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
                                var key = levels[index + 6 * regionGroup.pagination()];

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
                                            {
                                                currentSave.player.professionSkills.Add(type.profession, (1, new()));
                                                if (!currentSave.player.learnedRecipes.ContainsKey(type.profession))
                                                    currentSave.player.learnedRecipes.Add(type.profession, new());
                                                foreach (var recipe in professions.Find(x => x.name == type.profession).defaultRecipes)
                                                    currentSave.player.LearnRecipe(type.profession, recipe);
                                            }
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
                        AddBigButton("OtherDisabled");
                    }
                });
            }
            AddPaginationLine(regionGroup);
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
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""));
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
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (recipes.Count > index + 6 * regionGroup.pagination())
                    {
                        var key = recipes[index + 6 * regionGroup.pagination()];
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
                                var key = recipes[index + 6 * regionGroup.pagination()];

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
                                var key = recipes[index + 6 * regionGroup.pagination()];
                                if (key.results.Count > 0)
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
                        AddBigButton("OtherDisabled");
                    }
                });
            }
            AddPaginationLine(regionGroup);
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
        new("BoardFrame", () => {
            SetAnchor(-115, 146);
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
        }),
        new("FishingBoard", () => {
            SetAnchor(Top, 0, -15 + 19 * (fishingBoard.field.GetLength(1) - 7));
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
            SetAnchor(Top, 0, 213 + 19 * (FishingBufferBoard.fishingBufferBoard.field.GetLength(1) - 7));
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
                CloseDesktop("QuestLog");
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
                CloseDesktop("QuestLog");
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
                CloseDesktop("QuestLog");
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
                CloseDesktop("QuestLog");
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
                CloseDesktop("QuestLog");
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
                CloseDesktop("QuestLog");
                if (CDesktop.title != "CharacterSheet")
                    SpawnDesktopBlueprint("CharacterSheet");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopCharacterSheetClose");
                }
            });
            AddHotkey(L, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                if (CDesktop.title != "QuestLog")
                    SpawnDesktopBlueprint("QuestLog");
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
                    CloseDesktop("QuestLog");
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
                    CloseDesktop("QuestLog");
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
                    CloseDesktop("QuestLog");
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
                    CloseDesktop("QuestLog");
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
                AddSmallButton(CDesktop.title == "QuestLog" ? "OtherClose" : "MenuQuestLog", (h) =>
                {
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("TalentScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("CharacterSheet");
                    if (CDesktop.title != "QuestLog")
                        SpawnDesktopBlueprint("QuestLog");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopInventoryClose");
                    }
                });
                AddSmallButton(CDesktop.title == "CraftingScreen" ? "OtherClose" : "MenuCrafting", (h) =>
                {
                    CloseDesktop("TalentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("CharacterSheet");
                    CloseDesktop("QuestLog");
                    if (CDesktop.title != "CraftingScreen")
                        SpawnDesktopBlueprint("CraftingScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopInstanceClose");
                    }
                });
                AddSmallButton(CDesktop.title == "BestiaryScreen" ? "OtherClose" : "MenuCompletion", (h) =>
                {
                    CloseDesktop("TalentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("CharacterSheet");
                    CloseDesktop("QuestLog");
                    if (CDesktop.title != "BestiaryScreen")
                        SpawnDesktopBlueprint("BestiaryScreen");
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
            SetRegionGroupWidth(253);
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
                AddLine("Level: ", "DarkGray", "Left");
                AddText(currentSave.player.level + "", "Gray");
            });
        }, true),
        new("MapToolbarStatusRight", () => {
            SetAnchor(TopRight);
            DisableGeneralSprites();
            AddRegionGroup();
            SetRegionGroupWidth(262);
            AddPaddingRegion(() =>
            {
                if (currentSave.player.unspentTalentPoints > 0)
                {
                    AddLine("You have ", "", "Right");
                    AddText(currentSave.player.unspentTalentPoints + "", "Uncommon");
                    AddText(" talent point" + (currentSave.player.unspentTalentPoints == 1 ? "!" : "s!"));
                }
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
            SetRegionGroupWidth(233);
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
            SetRegionGroupWidth(242);
            AddPaddingRegion(
                () =>
                {
                    AddLine(locationName, "Gray", "Center");
                }
            );
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
                    AddSmallButton("OtherSortOff");
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (activeAbilities.Count > index + 6 * regionGroup.pagination())
                    {
                        var key = activeAbilities.ToList()[index + 6 * regionGroup.pagination()];
                        AddLine(key.Key.name);
                        AddLine("Rank: ", "DarkGray");
                        AddText("" + ToRoman(key.Value + 1));
                        AddBigButton(key.Key.icon,
                            (h) =>
                            {
                                var key = activeAbilities.ToList()[index + 6 * regionGroup.pagination()];
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
                                var key = activeAbilities.ToList()[index + 6 * regionGroup.pagination()].Key;
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
                        AddBigButton("OtherDisabled");
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
                    AddSmallButton("OtherSortOff");
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (passiveAbilities.Count > index + 6 * regionGroup.pagination())
                    {
                        var key = passiveAbilities.ToList()[index + 6 * regionGroup.pagination()];
                        AddLine(key.Key.name);
                        AddLine("Rank: ", "DarkGray");
                        AddText("" + ToRoman(key.Value + 1));
                        AddBigButton(key.Key.icon,
                            null,
                            null,
                            (h) => () =>
                            {
                                SetAnchor(Center);
                                var key = passiveAbilities.ToList()[index + 6 * regionGroup.pagination()].Key;
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
                        AddBigButton("OtherDisabled");
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
                    AddText(" / " + currentSave.player.MaxResource(element), "DarkGray");
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
                            AddSmallButton(abilityObj.icon);
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
                            AddSmallButton("OtherEmpty");
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
        }, true),
    };

    public static List<Blueprint> desktopBlueprints = new()
    {
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
                else if (CloseWindow("CharacterCreationFinish"))
                {
                    PlaySound("DesktopButtonClose");
                    CloseWindow("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFactionRaceChoice");
                    CloseWindow("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    CloseWindow("CharacterCreationWho");
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
            SpawnWindowBlueprint("HostileArea");
            SpawnWindowBlueprint("HostileAreaProgress");
            SpawnWindowBlueprint("HostileAreaDenizens");
            SpawnWindowBlueprint("HostileAreaElites");
            //if (area.fishing) SpawnWindowBlueprint("FishingAnchor");
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
            if (currentSave.player.Reputation(town.faction) >= 4200)
            {
                SpawnWindowBlueprint("Town");
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
                    if (CloseWindow("MountCollection"))
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
                        Respawn("Town");
                        if (personCategory != null) Respawn("Persons");
                    }
                    else if (CloseWindow("Persons"))
                    {
                        PlaySound("DesktopInstanceClose");
                        personCategory = null;
                        Respawn("Town");
                    }
                    else
                    {
                        PlaySound("DesktopInstanceClose");
                        town = null;
                        CloseDesktop("Town");
                    }
                });
            }
            else
            {

                SpawnWindowBlueprint("TownHostile");
                AddHotkey(Escape, () =>
                {
                    PlaySound("DesktopInstanceClose");
                    town = null;
                    CloseDesktop("Town");
                });
            }
        }),
        new("Instance", () => 
        {
            SetDesktopBackground(instance.Background());
            SpawnWindowBlueprint("Instance");
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
                if (CloseWindow("HostileArea"))
                {
                    area = null;
                    CloseWindow("BossQueue");
                    CloseWindow("HostileAreaProgress");
                    CloseWindow("HostileAreaDenizens");
                    CloseWindow("HostileAreaElites");
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
            SpawnWindowBlueprint("Complex");
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
                if (CloseWindow("HostileArea"))
                {
                    area = null;
                    PlaySound("DesktopButtonClose");
                    CloseWindow("HostileAreaProgress");
                    CloseWindow("HostileAreaDenizens");
                    CloseWindow("HostileAreaElites");
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
            SpawnTransition();
            locationName = board.area.name;
            PlaySound("DesktopEnterCombat");
            SetDesktopBackground(board.area.Background());
            board.Reset();
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
            SpawnWindowBlueprint("FishingBoardFrame");
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
        new("QuestLog", () => 
        {
            PlaySound("DesktopCharacterSheetOpen");
            SetDesktopBackground("Skin");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            SpawnWindowBlueprint("QuestList");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopCharacterSheetClose");
                CloseDesktop("QuestLog");
            });
            AddPaginationHotkeys();
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
            SetDesktopBackground("Skin");
            SpawnWindowBlueprint("ProfessionListPrimary");
            SpawnWindowBlueprint("ProfessionListSecondary");
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
            AddPaginationHotkeys();
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
    };

    public static void AddPaginationHotkeys()
    {
        AddHotkey(D, () => 
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup != null && x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            if (group == null && window.headerGroup != null && window.headerGroup.maxPaginationReq != null)
                group = window.headerGroup;
            if (group == null) return;
            var temp = group.pagination();
            group.IncrementPagination();
            if (temp != group.pagination())
                PlaySound("DesktopChangePage", 0.4f);
            window.Respawn();
        });
        AddHotkey(D, () => 
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup != null && x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            if (group == null && window.headerGroup != null && window.headerGroup.maxPaginationReq != null)
                group = window.headerGroup;
            if (group == null) return;
            var temp = group.pagination();
            group.IncrementPaginationEuler();
            if (temp != group.pagination())
                PlaySound("DesktopChangePage", 0.4f);
            window.Respawn();
        }, false);
        AddHotkey(A, () => 
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup != null && x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            if (group == null && window.headerGroup != null && window.headerGroup.maxPaginationReq != null)
                group = window.headerGroup;
            if (group == null) return;
            var temp = group.pagination();
            group.DecrementPagination();
            if (temp != group.pagination())
                PlaySound("DesktopChangePage", 0.4f);
            window.Respawn();
        });
        AddHotkey(A, () => 
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup != null && x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            if (group == null && window.headerGroup != null && window.headerGroup.maxPaginationReq != null)
                group = window.headerGroup;
            if (group == null) return;
            var temp = group.pagination();
            group.DecrementPaginationEuler();
            if (temp != group.pagination())
                PlaySound("DesktopChangePage", 0.4f);
            window.Respawn();
        }, false);
    }
}
