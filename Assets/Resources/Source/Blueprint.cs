using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using static UnityEngine.KeyCode;

using static Root;
using static Root.Anchor;

using static Item;
using static Buff;
using static Race;
using static Class;
using static Sound;
using static Event;
using static Cursor;
using static Faction;
using static ItemSet;
using static Ability;
using static SaveGame;
using static Coloring;
using static GameSettings;
using static Serialization;
using static SiteHostileArea;
using static SiteInstance;
using static SiteComplex;
using static SiteTown;
using UnityEngine.Rendering.PostProcessing;

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
        new("TitleScreenMenu", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Menu", "Gray");
            });
            AddRegionGroup();
            AddButtonRegion(() =>
            {
                AddLine("Singleplayer", "Black");
            },
            (h) =>
            {
                SpawnWindowBlueprint("CharacterRoster");
                SpawnWindowBlueprint("CharacterInfo");
                SpawnWindowBlueprint("TitleScreenSingleplayer");
                CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Settings", "Black");
            },
            (h) =>
            {
                SpawnWindowBlueprint("GameSettings");
                CloseWindow(h.window);
            });
            //AddButtonRegion(() =>
            //{
            //    AddLine("Achievments", "Black");
            //},
            //(h) =>
            //{

            //});
            //AddButtonRegion(() =>
            //{
            //    AddLine("Graveyard", "Black");
            //},
            //(h) =>
            //{

            //});
            //AddButtonRegion(() =>
            //{
            //    AddLine("Dev panel", "Black");
            //},
            //(h) =>
            //{
            //    SpawnDesktopBlueprint("DevPanel");
            //});
            //AddButtonRegion(() =>
            //{
            //    AddLine("Credits", "Black");
            //},
            //(h) =>
            //{
            //    //BLIZZARD: MUSIC, SOUNDS AND TEXTURES
            //    //POOH: PROGRAMMING AND DESIGNING
            //    //RYVED: CONSULTATION AND BRAINSTORMING
            //    //LEKRIS: CONSULTATION AND BRAINSTORMING
            //});
            AddButtonRegion(() =>
            {
                AddLine("Exit", "Black");
            },
            (h) =>
            {
                Application.Quit();
            });
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
                    SpawnDesktopBlueprint("Map");
                    Login();
                    SetDesktopBackground("LoadingScreens/LoadingScreen" + (CDesktop.cameraDestination.x < 130 ? "Kalimdor" : "EasternKingdoms"));
                });
            }
            else
                AddPaddingRegion(() => { AddLine("Enter World", "DarkGray", "Center"); });
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
                        h.window.Respawn();
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
        new("GameOver", () =>
        {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Game over:");
            });
            AddPaddingRegion(() =>
            {
                AddLine(currentSave.player.name + " died ");
            });
            AddButtonRegion(() =>
            {
                AddLine("Return to title screen", "", "Center");
            },
            (h) =>
            {
                CloseSave();
                SaveGames();
                saves[settings.selectedRealm].Remove(currentSave);
                //graveyard.Add(currentSave);
                CloseDesktop("GameOver");
                CloseDesktop("Map");
                CloseDesktop("TitleScreen");
                SpawnDesktopBlueprint("TitleScreen");
            });
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
            AddInputRegion(String.promptConfirm, InputType.Capitals, "DangerousRed");
        }, true),
        new("CharacterInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            if (settings.selectedCharacter != "")
            {
                var slot = saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter);
                var spec = slot.player.GetClass();
                AddHeaderRegion(() => { AddLine(slot.player.name); });
                AddHeaderRegion(() =>
                {
                    AddBigButton("Portrait" + slot.player.race.Replace("'", "").Replace(".", "").Replace(" ", "") + (Race.races.Find(x => x.name == slot.player.race).genderedPortrait ? slot.player.gender : ""), (h) => { });
                    AddBigButton(slot.player.GetClass().icon, (h) => { });
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
                    AddText(spec.talentTrees[0].talents.Count(x => slot.player.abilities.Contains(x.ability)) + "");
                    AddLine(spec.talentTrees[1].name + ": ", "DarkGray");
                    AddText(spec.talentTrees[1].talents.Count(x => slot.player.abilities.Contains(x.ability)) + "");
                    AddLine(spec.talentTrees[2].name + ": ", "DarkGray");
                    AddText(spec.talentTrees[2].talents.Count(x => slot.player.abilities.Contains(x.ability)) + "");
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
        new("CharacterRoster", () =>
        {
            if (settings.selectedCharacter != "")
                SetDesktopBackground("Areas/" + races.Find(x => x.name == saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter).player.race).background, true);
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
            AddHeaderRegion(() =>
            {
                AddLine("Characters:", "Gray");
            });
            if (saves.ContainsKey(settings.selectedRealm))
            {
                foreach (var slot in saves[settings.selectedRealm])
                {
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("Portrait" + slot.player.race.Replace("'", "").Replace(".", "").Replace(" ", "") + (races.Find(x => x.name == slot.player.race).genderedPortrait ? slot.player.gender : ""), (h) =>
                        {
                            CloseWindow("RealmRoster");
                            if (settings.selectedCharacter != slot.player.name)
                            {
                                settings.selectedCharacter = slot.player.name;
                                SetDesktopBackground("Areas/" + races.Find(x => x.name == slot.player.race).background, true);
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
                if (saves[settings.selectedRealm].Count < 7)
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
                        creationClass = "";
                        SpawnWindowBlueprint("CharacterCreation");
                        SpawnWindowBlueprint("CharacterCreationRightSide");
                    });
                else
                    AddPaddingRegion(() => AddLine("Create a new character", "DarkGray"));
            }
            else
                AddPaddingRegion(() => AddLine("Create a new character", "DarkGray"));
        }, true),
        new("GameSettings", () =>
        {
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
                AddCheckbox(settings.shadows);
                AddLine("Shadows", "Gray");
            });
            AddPaddingRegion(() =>
            {
                AddCheckbox(settings.pixelPerfectVision);
                AddLine("Pixel perfect vision", "Gray");
            });
            AddPaddingRegion(() =>
            {
                AddCheckbox(settings.music);
                AddLine("Music", "Gray");
            });
            AddPaddingRegion(() =>
            {
                AddCheckbox(settings.soundEffects);
                AddLine("Sound effects", "Gray");
            });
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
                AddLine("Achievments", "Black");
            },
            (h) =>
            {

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
        new("PlayerBattleInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            AddButtonRegion(
                () =>
                {
                    AddLine(Board.board.player.name, "Black");
                    AddSmallButton("MenuFlee", (h) =>
                    {
                        Board.board.EndCombat("PlayerFled");
                    });
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                if (Board.board.player.spec != null)
                {
                    AddBigButton(Board.board.player.GetClass().icon,
                        (h) => { }
                    );
                }
                else
                {
                    var race = races.Find(x => x.name == Board.board.enemy.race);
                    AddBigButton(race.portrait == "" ? "OtherUnknown" : race.portrait + (race.genderedPortrait ? Board.board.enemy.gender : ""), (h) => { });
                }
                AddLine("Level: " + Board.board.player.level, "Gray");
                AddLine("Health: " + Board.board.player.health + "/" + Board.board.player.MaxHealth(), "Gray");
            });
            foreach (var actionBar in Board.board.player.actionBars)
            {
                var abilityObj = abilities.Find(x => x.name == actionBar.ability);
                if (abilityObj == null || abilityObj.cost == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(actionBar.ability, "", "Right");
                        AddSmallButton(abilityObj.icon, (h) => { });
                        if (!abilityObj.EnoughResources(Board.board.player))
                        {
                            SetSmallButtonToGrayscale();
                            AddSmallButtonOverlay("OtherGridBlurred");
                        }
                        if (actionBar.cooldown > 0)
                            AddSmallButtonOverlay("OtherGrid");
                    },
                    (h) =>
                    {
                        if (abilityObj.EnoughResources(Board.board.player) && actionBar.cooldown == 0)
                        {
                            actionBar.cooldown = abilityObj.cooldown;
                            Board.board.CallEvents(Board.board.player, new() { { "Trigger", "AbilityCast" }, {"Triggerer", "Effector" }, { "AbilityName", abilityObj.name } });
                            Board.board.CallEvents(Board.board.enemy, new() { { "Trigger", "AbilityCast" }, {"Triggerer", "Other" }, { "AbilityName", abilityObj.name } });
                            Board.board.player.DetractResources(abilityObj.cost);
                            Board.board.temporaryElementsPlayer = new();
                            h.window.desktop.RebuildAll();
                        }
                    },
                    (h) => () =>
                    {
                        PrintAbilityTooltip(Board.board.player, Board.board.enemy, abilityObj);
                    }
                );
            }
        }),
        new("PlayerEquipmentInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddButtonRegion(
                () =>
                {
                    AddLine(currentSave.player.name, "Black");
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton("Portrait" + currentSave.player.race.Replace("'", "").Replace(".", "").Replace(" ", "") + (Race.races.Find(x => x.name == currentSave.player.race).genderedPortrait ? currentSave.player.gender : ""),
                    (h) => { }
                );
                AddBigButton(currentSave.player.GetClass().icon,
                    (h) => { }
                );
                AddLine("Level: " + currentSave.player.level, "Gray");
            });
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
            if (currentSave.player.GetItemInSlot("Main Hand") == null || currentSave.player.GetItemInSlot("Main Hand") != null && currentSave.player.GetItemInSlot("Main Hand").type != "Two Handed")
                Foo("Off Hand", currentSave.player.GetItemInSlot("Off Hand"));
            Foo("Neck", currentSave.player.GetItemInSlot("Neck"));
            Foo("Finger", currentSave.player.GetItemInSlot("Finger"));
            Foo("Trinket", currentSave.player.GetItemInSlot("Trinket"));
            if (currentSave.player.spec == "Druid")
                Foo("Idol", currentSave.player.GetItemInSlot("Special"));
            if (currentSave.player.spec == "Paladin")
                Foo("Libram", currentSave.player.GetItemInSlot("Special"));
            if (currentSave.player.spec == "Shaman")
                Foo("Totem", currentSave.player.GetItemInSlot("Special"));
            AddPaddingRegion(() =>
            {
                AddLine();
                AddLine();
                AddLine();
                AddLine();
            });

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
                                PlaySound(item.ItemSound("PutDown"), 0.6f);
                                currentSave.player.Unequip(new() { slot });
                                Respawn("PlayerEquipmentInfo");
                                Respawn("Inventory");
                            },
                            (h) => () =>
                            {
                                SetAnchor(Center);
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
        new("SpellbookAbilityList", () => {
            SetAnchor(TopRight);
            var activeAbilities = abilities.FindAll(x => x.cost != null && currentSave.player.abilities.Contains(x.name)).ToList();
            AddHeaderGroup(() => activeAbilities.Count, 7);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Active abilities:");
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
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 7; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (activeAbilities.Count > index + 7 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = activeAbilities[index + 7 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon, (h) => { });
                        if (currentSave.player.actionBars.Exists(x => x.ability == foo.name))
                        {
                            SetSmallButtonToGrayscale();
                            AddSmallButtonOverlay("OtherGridBlurred");
                        }
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = activeAbilities[index + 7 * regionGroup.pagination];
                    if (!currentSave.player.actionBars.Exists(x => x.ability == foo.name) && currentSave.player.actionBars.Count < currentSave.player.actionBarsUnlocked)
                    {
                        currentSave.player.actionBars.Add(new ActionBar(foo.name));
                        Respawn("PlayerSpellbookInfo");
                        Respawn("SpellbookAbilityList");
                        PlaySound("DesktopActionbarAdd", 0.7f);
                    }
                },
                (h) => () =>
                {
                    SetAnchor(Center);
                    PrintAbilityTooltip(null, null, abilitiesSearch[index + 10 * regionGroup.pagination]);
                });
            }
            var passiveAbilities = abilities.FindAll(x => x.cost == null && currentSave.player.abilities.Contains(x.name)).ToList();
            AddRegionGroup(() => passiveAbilities.Count, 7);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(358);
            AddPaddingRegion(() => SetRegionAsGroupExtender());
            AddHeaderRegion(() =>
            {
                AddLine("Active abilities:");
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
            regionGroup = CDesktop.LBWindow.LBRegionGroup;
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 7; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (passiveAbilities.Count > index + 7 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = passiveAbilities[index + 7 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon, (h) => { });
                        if (currentSave.player.actionBars.Exists(x => x.ability == foo.name))
                        {
                            SetSmallButtonToGrayscale();
                            AddSmallButtonOverlay("OtherGridBlurred");
                        }
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
                        AddLine();
                    }
                },
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(Center);
                    PrintAbilityTooltip(null, null, abilitiesSearch[index + 10 * regionGroup.pagination]);
                });
            }
            //AddPaddingRegion(() =>
            //{
            //    SetRegionAsGroupExtender();
            //    AddLine(abilities.Count + " abilities", "DarkGray");
            //    if (abilities.Count != abilitiesSearch.Count)
            //        AddLine(abilitiesSearch.Count + " found in search", "DarkGray");
            //});
        }),
        new("PlayerSpellbookInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddButtonRegion(
                () =>
                {
                    AddLine(currentSave.player.name, "Black");
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton(currentSave.player.GetClass().icon,
                    (h) => { }
                );
                AddLine("Level: " + currentSave.player.level, "Gray");
                AddLine("Health: " + currentSave.player.health + "/" + currentSave.player.MaxHealth(), "Gray");
            });
            for (int i = 0; i < currentSave.player.actionBarsUnlocked; i++)
            {
                var index = i;
                var abilityObj = currentSave.player.actionBars.Count <= index ? null : Ability.abilities.Find(x => x.name == currentSave.player.actionBars[index].ability);
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
                            CloseWindow("SpellbookAbilityList");
                            CloseWindow("PlayerSpellbookInfo");
                            SpawnWindowBlueprint("SpellbookAbilityList");
                            SpawnWindowBlueprint("PlayerSpellbookInfo");
                            PlaySound("DesktopActionbarRemove", 0.7f);
                        },
                        (h) => () =>
                        {
                            PrintAbilityTooltip(currentSave.player, null, abilityObj);
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
        new("SpellbookResources", () => {
            SetAnchor(BottomLeft);
            DisableShadows();
            AddHeaderGroup();
            AddHeaderRegion(() => { AddLine("Starting resources:", "DarkGray"); });
            AddRegionGroup();
            var elements1 = new List<string> { "Fire", "Water", "Earth", "Air", "Frost" };
            var elements2 = new List<string> { "Lightning", "Arcane", "Decay", "Order", "Shadow" };
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    AddSmallButton("Element" + element + "Rousing",
                        (h) => { },
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
                        (h) => { },
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
                    AddText("/" + currentSave.player.MaxResource(element), "DarkGray");
                });
        }, true),
        new("EnemyBattleInfo", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            AddButtonRegion(
                () =>
                {
                    AddLine(Board.board.enemy.name, "Black");
                    AddSmallButton("OtherSettings", (h) => { SpawnDesktopBlueprint("GameMenu"); });
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                var race = races.Find(x => x.name == Board.board.enemy.race);
                AddBigButton(race.portrait == "" ? "OtherUnknown" : race.portrait + (race.genderedPortrait ? Board.board.enemy.gender : ""), (h) => { });
                AddLine("Level: ", "Gray");
                AddText(Board.board.enemy.level - 10 > Board.board.player.level ? "??" : "" + Board.board.enemy.level, ColorEntityLevel(Board.board.enemy.level));
                AddLine("Health: " + Board.board.enemy.health + "/" + Board.board.enemy.MaxHealth(), "Gray");
            });
            foreach (var actionBar in Board.board.enemy.actionBars)
            {
                var abilityObj = abilities.Find(x => x.name == actionBar.ability);
                if (abilityObj == null || abilityObj.cost == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(actionBar.ability, "", "Right");
                        AddSmallButton(abilityObj.icon, (h) => { });
                        if (!abilityObj.EnoughResources(Board.board.enemy))
                        {
                            SetSmallButtonToGrayscale();
                            AddSmallButtonOverlay("OtherGridBlurred");
                        }
                        if (actionBar.cooldown > 0)
                            AddSmallButtonOverlay("OtherGrid");
                    },
                    (h) =>
                    {

                    },
                    (h) => () =>
                    {
                        PrintAbilityTooltip(Board.board.enemy, Board.board.player, abilityObj);
                    }
                );
            }
        }),
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
        new("BattleBoard", () => {
            SetAnchor(Top, 0, -34 + 19 * (Board.board.field.GetLength(1) - 7));
            var boardBackground = new GameObject("BoardBackground", typeof(SpriteRenderer), typeof(SpriteMask));
            boardBackground.transform.parent = CDesktop.LBWindow.transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/BoardBackground" + Board.board.field.GetLength(0) + "x" + Board.board.field.GetLength(1));
            var mask = boardBackground.GetComponent<SpriteMask>();
            mask.sprite = Resources.Load<Sprite>("Sprites/Textures/BoardMask" + Board.board.field.GetLength(0) + "x" + Board.board.field.GetLength(1));
            mask.isCustomRangeActive = true;
            mask.frontSortingLayerID = SortingLayer.NameToID("Missile");
            mask.backSortingLayerID = SortingLayer.NameToID("Default");
            boardBackground = new GameObject("BoardInShadow", typeof(SpriteRenderer));
            boardBackground.transform.parent = CDesktop.LBWindow.transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/BoardShadow" + Board.board.field.GetLength(0) + "x" + Board.board.field.GetLength(1));
            boardBackground.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
            DisableGeneralSprites();
            AddRegionGroup();
            for (int i = 0; i < Board.board.field.GetLength(1); i++)
            {
                AddPaddingRegion(() =>
                {
                    for (int j = 0; j < Board.board.field.GetLength(0); j++)
                    {
                        AddBigButton(Board.board.GetFieldButton(),
                        (h) =>
                        {
                            var list = Board.board.FloodCount(h.region.bigButtons.FindIndex(x => x.GetComponent<Highlightable>() == h), h.region.regionGroup.regions.IndexOf(h.region));
                            Board.board.temporaryElementsPlayer = new();
                            Board.board.playerFinishedMoving = true;
                            Board.board.FloodDestroy(list);
                        });
                        //(h) => () =>
                        //{
                        //    var coords = (h.region.bigButtons.FindIndex(x => x.GetComponent<Highlightable>() == h), h.region.regionGroup.regions.IndexOf(h.region));
                        //    var count = Board.board.FloodCount(coords.Item1, coords.Item2).Count;
                        //    SetAnchor(Bottom);
                        //    AddRegionGroup();
                        //    AddHeaderRegion(
                        //        () =>
                        //        {
                        //            AddLine("x" + count + " ", LightGray);
                        //            AddText(Board.board.GetFieldName(coords.Item1, coords.Item2), Board.board.GetFieldColor(coords.Item1, coords.Item2));
                        //        }
                        //    );s
                        //});
                    }
                });
            }
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
        new("MapToolbar", () => {
            SetAnchor(Bottom);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Map", "DarkGray", "Center");
            });
            AddPaddingRegion(() =>
            {
                AddSmallButton("MenuCharacterSheet", (h) =>
                {
                    SpawnDesktopBlueprint("CharacterSheet");
                    SwitchDesktop("CharacterSheet");
                });
                AddSmallButton("MenuInventory", (h) =>
                {
                    SpawnDesktopBlueprint("EquipmentScreen");
                    SwitchDesktop("EquipmentScreen");
                });
                AddSmallButton("MenuSpellbook", (h) =>
                {
                    SpawnDesktopBlueprint("SpellbookScreen");
                    SwitchDesktop("SpellbookScreen");
                });
                AddSmallButton("MenuClasses", (h) =>
                {
                    SpawnDesktopBlueprint("TalentScreen");
                    SwitchDesktop("TalentScreen");
                });
                AddSmallButton("MenuCompletion", (h) =>
                {
                    SpawnDesktopBlueprint("TalentScreen");
                    SwitchDesktop("TalentScreen");
                });
            });
        }, true),
        new("InstanceLeftSide", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(175);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() =>
            {
                foreach (var line in instance.description)
                    AddLine(line, "DarkGray");
            });
        }),
        new("HostileAreaRightSide", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() =>
            {
                //foreach (var line in instance.description)
                //    AddLine(line, "DarkGray");
                //AddLine("Select area on the right.", "DarkGray");
            });
        }),
        new("ComplexLeftSide", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(175);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() =>
            {
                foreach (var line in complex.description)
                    AddLine(line, "DarkGray");
            });
        }),
        new("TownLeftSide", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(175);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() =>
            {
                //foreach (var line in complex.description)
                //    AddLine(line, "DarkGray");
            });
        }),
        new("Inventory", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupHeight(358);
            var items = currentSave.player.inventory.items;
            AddHeaderRegion(() =>
            {
                AddLine("Inventory:");
                AddSmallButton("OtherClose", (h) =>
                {
                    if (town != null)
                    {
                        CloseDesktop("BankScreen");
                        SwitchDesktop("TownEntrance");
                        PlaySound("DesktopBankClose");
                    }
                    else
                    {
                        CloseDesktop("EquipmentScreen");
                        SwitchDesktop("Map");
                        PlaySound("DesktopInventoryClose");
                    }
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.player.inventory.items.Reverse();
                    CloseWindow("Inventory");
                    SpawnWindowBlueprint("Inventory");
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "InventorySettings") && !CDesktop.windows.Exists(x => x.title == "BankSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("InventorySort");
                        Respawn("Inventory");
                        if (town != null)
                            Respawn("Bank");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
                if (!CDesktop.windows.Exists(x => x.title == "InventorySettings") && !CDesktop.windows.Exists(x => x.title == "BankSort"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("InventorySettings");
                        Respawn("Inventory");
                        if (town != null)
                            Respawn("Bank");
                    });
                else
                    AddSmallButton("OtherSettingsOff", (h) => { });
            });
            for (int i = 0; i < 8; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (items.Count > index * 5 + j) PrintInventoryItem(items[index * 5 + j]);
                            else AddBigButton("OtherEmpty", (h) => { });
                    }
                );
            }
            AddPaddingRegion(() => { AddLine(); });
        }, true),
        new("Bank", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupHeight(358);
            var items = currentSave.banks[town.name].items;
            AddHeaderRegion(() =>
            {
                AddLine("Bank:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseDesktop("BankScreen");
                    SwitchDesktop("TownEntrance");
                    PlaySound("DesktopBankClose");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.banks[town.name].items.Reverse();
                    Respawn("Bank");
                    Respawn("Inventory");
                    PlaySound("DesktopInventorySort", 0.2f);
                });
                if (!CDesktop.windows.Exists(x => x.title == "InventorySettings") && !CDesktop.windows.Exists(x => x.title == "InventorySort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("BankSort");
                        Respawn("Bank");
                        Respawn("Inventory");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
            });
            for (int i = 0; i < 8; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (items.Count > index * 5 + j) PrintBankItem(items[index * 5 + j]);
                            else AddBigButton("OtherEmpty", (h) => { });
                    }
                );
            }
            AddPaddingRegion(() => { AddLine(); });
        }, true),
        new("BankSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(162);
            AddHeaderRegion(() =>
            {
                AddLine("Sort bank inventory:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("BankSort");
                    Respawn("Bank");
                    Respawn("Inventory");
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
                Respawn("Bank");
                Respawn("Inventory");
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
                Respawn("Bank");
                Respawn("Inventory");
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
                Respawn("Bank");
                Respawn("Inventory");
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
                Respawn("Bank");
                Respawn("Inventory");
                PlaySound("DesktopInventorySort", 0.2f);
            });
        }),
        new("InventorySort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(162);
            AddHeaderRegion(() =>
            {
                AddLine("Sort inventory:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("InventorySort");
                    Respawn("Inventory");
                    Respawn("Bank");
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
                Respawn("Inventory");
                Respawn("Bank");
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
                Respawn("Inventory");
                Respawn("Bank");
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
                Respawn("Inventory");
                Respawn("Bank");
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
                Respawn("Inventory");
                Respawn("Bank");
                PlaySound("DesktopInventorySort", 0.2f);
            });
        }),
        new("InventorySettings", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(162);
            AddHeaderRegion(() =>
            {
                AddLine("Inventory settings:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("InventorySettings");
                    Respawn("Inventory");
                    Respawn("PlayerEquipmentInfo");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Rarity indicators", "Black");
                AddCheckbox(settings.rarityIndicators);
            },
            (h) =>
            {
                Respawn("Inventory");
                Respawn("PlayerEquipmentInfo");
                Respawn("InventorySettings");
            });
            if (settings.rarityIndicators.Value())
                AddButtonRegion(() =>
                {
                    AddLine("Big Rarity indicators", "Black");
                    AddCheckbox(settings.bigRarityIndicators);
                },
                (h) =>
                {
                    Respawn("Inventory");
                    Respawn("PlayerEquipmentInfo");
                });
            AddButtonRegion(() =>
            {
                AddLine("Upgrade indicators", "Black");
                AddCheckbox(settings.upgradeIndicators);
            },
            (h) =>
            {
                Respawn("Inventory");
                Respawn("PlayerEquipmentInfo");
            });
            AddButtonRegion(() =>
            {
                AddLine("New slot indicators", "Black");
                AddCheckbox(settings.newSlotIndicators);
            },
            (h) =>
            {
                Respawn("Inventory");
                Respawn("PlayerEquipmentInfo");
            });
        }),
        new("ItemDrop", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            SetRegionGroupWidth(266);
            AddHeaderRegion(() =>
            {
                AddLine("Loot from ", "Gray");
                AddText("Chief Ukorz Sandscalp", "Gray");
            });
            var item = items[random.Next(items.Count)];
            AddHeaderRegion(() =>
            {
                AddBigButton(item.icon, (h) => { });
                var split = item.name.Split(", ");
                AddLine(split[0], item.rarity);
                if (split.Length > 1)
                    AddLine("\"" + split[1] + "\"", item.rarity);
            });
            AddPaddingRegion(() =>
            {
                if (item.armorClass != null)
                {
                    AddLine(item.armorClass + " " + item.type, "Gray");
                    AddLine(item.armor + " Armor", "Gray");
                }
                else if (item.maxDamage != 0)
                {
                    AddLine(item.type + " " + item.detailedType, "Gray");
                    AddLine(item.minDamage + " - " + item.maxDamage + " Damage", "Gray");
                }
                else
                    AddLine(item.type, "Gray");
            });
            if (item.stats.stats.Count > 0)
                AddPaddingRegion(() =>
                {
                    foreach (var stat in item.stats.stats)
                        AddLine("+" + stat.Value + " " + stat.Key, "Gray");
                });
            AddHeaderRegion(() =>
            {
                AddLine("Required level: ", "DarkGray");
                AddText("" + item.lvl, ColorItemRequiredLevel(item.lvl));
            });
            AddPaddingRegion(
                () =>
                {
                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("Accept the item", "Black");
                },
                (h) =>
                {

                }
            );
            AddRegionGroup();
            AddPaddingRegion(
                () =>
                {
                    AddSmallButton("ActionReroll",
                        (h) =>
                        {

                        }
                    );
                }
            );
            AddRegionGroup();
            SetRegionGroupWidth(110);
            AddPaddingRegion(
                () =>
                {
                    AddLine("You can reroll!");
                }
            );
            AddRegionGroup();
            AddPaddingRegion(
                () =>
                {
                    AddLine((int)item.price + "", "Gold");
                    AddSmallButton("ItemCoinsGold", (h) => { });
                }
            );
            AddRegionGroup();
            AddPaddingRegion(
                () =>
                {
                    AddLine((int)(item.price * 100 % 100) + ""  + "", "Silver");
                    AddSmallButton("ItemCoinsSilver", (h) => { });
                }
            );
            AddRegionGroup();
            AddPaddingRegion(
                () =>
                {
                    AddLine((int)(item.price * 10000 % 100) + "", "Copper");
                    AddSmallButton("ItemCoinsCopper", (h) => { });
                }
            );
        }, true),
        new("BattleActionBar", () => {
            SetAnchor(Bottom);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddBigButton("ClassRogueSpellMutilate",
                (h) => { });
                AddBigButton("ClassRogueSpellGarrote",
                (h) => { });
                AddBigButton("ClassRogueSpellRupture",
                (h) => { });
                AddBigButton("ClassRogueSpellEnvenom",
                (h) =>
                {
                    PlaySound("SpellEnvenomCast");
                    if (random.Next(0, 2) == 1)
                        PlaySound("SpellEnvenomImpact");
                },
                (h) => () =>
                {
                    SetAnchor(Center);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddBigButton("ClassRogueSpellEnvenom", (h) => { });
                        AddLine("Envenom", "Gray");
                    });
                    AddHeaderRegion(() =>
                    {
                        AddLine("Cast cost:", "Gray");
                    });
                    AddPaddingRegion(() =>
                    {
                        AddLine("x2 Decay", "Gray");
                        AddLine("x3 Shadow", "Gray");
                        AddLine("x1 Air", "Gray");
                    });
                    AddHeaderRegion(() =>
                    {
                        AddLine("Effects:", "Gray");
                    });
                    AddPaddingRegion(() =>
                    {
                        AddLine("Strike the target for 24* damage.", "Gray");
                        AddLine("Additionaly poison the target for 4* damage", "Gray");
                        AddLine("every time they make move for next 3 turns.", "Gray");
                    });
                    AddPaddingRegion(() =>
                    {
                        AddLine("* Scaled with Agility and Decay Mastery.", "Gray");
                    });
                });
                AddBigButton("ClassRogueSpellEvasion",
                (h) => { });
                AddBigButton("ClassRogueSpellKidneyShot",
                (h) => { });
                AddBigButton("OtherEmpty",
                (h) => { });
            });
        }),
        new("CharacterNeckSlot", () => {
            SetAnchor(-98, 74);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Neck"));
        }),
        new("CharacterBackSlot", () => {
            SetAnchor(-98, 22);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Back"));
        }),
        new("CharacterRingSlot", () => {
            SetAnchor(-98, -30);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Finger"));
        }),
        new("CharacterHeadSlot", () => {
            SetAnchor(-46, 100);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Head"));
        }),
        new("CharacterChestSlot", () => {
            SetAnchor(-46, 48);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Chest"));
        }),
        new("CharacterLegsSlot", () => {
            SetAnchor(-46, -4);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Legs"));
        }),
        new("CharacterFeetSlot", () => {
            SetAnchor(-46, -56);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Feet"));
        }),
        new("CharacterShouldersSlot", () => {
            SetAnchor(6, 100);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Shoulders"));
        }),
        new("CharacterHandsSlot", () => {
            SetAnchor(6, 48);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Hands"));
        }),
        new("CharacterWaistSlot", () => {
            SetAnchor(6, -4);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Waist"));
        }),
        new("CharacterSpecialSlot", () => {
            SetAnchor(6, -56);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Special"));
        }),
        new("CharacterMainHandSlot", () => {
            SetAnchor(58, 74);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Main Hand"));
        }),
        new("CharacterOffHandSlot", () => {
            SetAnchor(58, 22);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Off Hand"));
        }),
        new("CharacterTrinketSlot", () => {
            SetAnchor(58, -30);
            PrintEquipmentItem(currentSave.player.GetItemInSlot("Trinket"));
        }),
        new("CharacterStats", () => {
            SetAnchor(BottomLeft);
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
            });
        }),
        new("CharacterCreation", () => {
            SetAnchor(TopLeft);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(228);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Side: " + creationSide);
                AddSmallButton("ActionReroll", (h) =>
                {
                    creationSide = random.Next(2) == 1 ? "Horde" : "Alliance";
                    creationRace = "";
                    creationClass = "";
                    h.window.Respawn();
                });
            });
            AddHeaderRegion(() =>
            {
                AddBigButton("HonorAlliance", (h) => { creationSide = "Alliance"; creationRace = ""; creationClass = ""; h.window.Respawn(); });
                if (creationSide != "Alliance") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                AddBigButton("HonorHorde", (h) => { creationSide = "Horde"; creationRace = ""; creationClass = ""; h.window.Respawn(); });
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
                        creationClass = "";
                        h.window.Respawn();
                    });
                });
                AddHeaderRegion(() =>
                {
                    if (creationSide == "Alliance")
                    {
                        AddBigButton("PortraitDwarf" + creationGender, (h) => { creationRace = "Dwarf"; creationClass = ""; h.window.Respawn(); });
                        if (creationRace != "Dwarf") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitGnome" + creationGender, (h) => { creationRace = "Gnome"; creationClass = ""; h.window.Respawn(); });
                        if (creationRace != "Gnome") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitHuman" + creationGender, (h) => { creationRace = "Human"; creationClass = ""; h.window.Respawn(); });
                        if (creationRace != "Human") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitNightElf" + creationGender, (h) => { creationRace = "Night Elf"; creationClass = ""; h.window.Respawn(); });
                        if (creationRace != "Night Elf") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitPandaren" + creationGender, (h) => { creationRace = "Pandaren"; creationClass = ""; h.window.Respawn(); });
                        if (creationRace != "Pandaren") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    }
                    else if (creationSide == "Horde")
                    {
                        AddBigButton("PortraitOrc" + creationGender, (h) => { creationRace = "Orc"; creationClass = ""; h.window.Respawn(); });
                        if (creationRace != "Orc") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitTauren" + creationGender, (h) => { creationRace = "Tauren"; creationClass = ""; h.window.Respawn(); });
                        if (creationRace != "Tauren") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitTroll" + creationGender, (h) => { creationRace = "Troll"; creationClass = ""; h.window.Respawn(); });
                        if (creationRace != "Troll") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitForsaken" + creationGender, (h) => { creationRace = "Forsaken"; creationClass = ""; h.window.Respawn(); });
                        if (creationRace != "Forsaken") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                        AddBigButton("PortraitPandaren" + creationGender, (h) => { creationRace = "Pandaren"; creationClass = ""; h.window.Respawn(); });
                        if (creationRace != "Pandaren") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    }
                });
            }
            if (creationRace != "")
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Class: " + creationClass);
                    AddSmallButton("ActionReroll", (h) =>
                    {
                        var classes = specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace));
                        creationClass = classes[random.Next(classes.Count)].name;
                        h.window.Respawn();
                    });
                });
                AddHeaderRegion(() =>
                {
                    var classes = specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace));
                    foreach (var foo in classes)
                    {
                        AddBigButton(foo.icon, (h) => { creationClass = foo.name; });
                        if (creationClass != foo.name) { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
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
                    var classes = specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace));
                    creationClass = classes[random.Next(classes.Count)].name;
                    creationName = creationGender == "Male" ? race.maleNames[random.Next(race.maleNames.Count)] : race.femaleNames[random.Next(race.femaleNames.Count)];
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
            });
        }),
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
        new("PlayerResources", () => {
            SetAnchor(BottomLeft);
            AddRegionGroup();
            var elements1 = new List<string> { "Fire", "Water", "Earth", "Air", "Frost" };
            var elements2 = new List<string> { "Lightning", "Arcane", "Decay", "Order", "Shadow" };
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    AddSmallButton("Element" + element + "Rousing",
                        (h) => { },
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(93);
                            AddHeaderRegion(() =>
                            {
                                AddLine(element + ":", "Gray");
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(Board.board.player.resources.ToList().Find(x => x.Key == element).Value + " / " + Board.board.player.MaxResource(element), "Gray");
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(49);
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    var value = Board.board.player.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value < Board.board.player.MaxResource(element) ? "Gray" : "Green"));
                    AddSmallButton("Element" + elements2[elements1.IndexOf(element)] + "Rousing",
                        (h) => { },
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
                                AddLine(Board.board.player.resources.ToList().Find(x => x.Key == elements2[elements1.IndexOf(element)]).Value + " / " + Board.board.player.MaxResource(elements2[elements1.IndexOf(element)]), "Gray");
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(30);
            foreach (var element in elements2)
                AddHeaderRegion(() =>
                {
                    var value = Board.board.player.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value < Board.board.player.MaxResource(element) ? "Gray" : "Green"));
                });
        }),
        new("EnemyResources", () => {
            SetAnchor(BottomRight);
            AddRegionGroup();
            var elements1 = new List<string> { "Fire", "Water", "Earth", "Air", "Frost" };
            var elements2 = new List<string> { "Lightning", "Arcane", "Decay", "Order", "Shadow" };
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    AddSmallButton("Element" + element + "Rousing",
                        (h) => { },
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(93);
                            AddHeaderRegion(() =>
                            {
                                AddLine(element + ":", "Gray");
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(Board.board.enemy.resources.ToList().Find(x => x.Key == element).Value + " / " + Board.board.enemy.MaxResource(element), "Gray");
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(49);
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    var value = Board.board.enemy.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value < Board.board.enemy.MaxResource(element) ? "Gray" : "Green"));
                    AddSmallButton("Element" + elements2[elements1.IndexOf(element)] + "Rousing",
                        (h) => { },
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
                                AddLine(Board.board.enemy.resources.ToList().Find(x => x.Key == elements2[elements1.IndexOf(element)]).Value + " / " + Board.board.enemy.MaxResource(elements2[elements1.IndexOf(element)]), "Gray");
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(30);
            foreach (var element in elements2)
                AddHeaderRegion(() =>
                {
                    var value = Board.board.enemy.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value < Board.board.enemy.MaxResource(element) ? "Gray" : "Green"));
                });
        }),
        new("ReturnToMap", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddSmallButton("OtherClose",
                (h) =>
                {
                    var title = CDesktop.title;
                    SwitchDesktop("Map");
                    if (title == "TalentScreen")
                        PlaySound("DesktopTalentScreenClose");
                    else if (title == "SpellbookScreen")
                        PlaySound("DesktopSpellbookScreenClose");
                    CloseDesktop(title);
                });
            });
        }, true),
        new("TalentHeader", () => {
            SetAnchor(TopLeft);
            var a = currentSave.player.GetClass();
            AddHeaderGroup();
            AddPaddingRegion(() =>
            {
                if (currentSave.player.unspentTalentPoints > 0)
                {
                    AddLine("You have ");
                    AddText(currentSave.player.unspentTalentPoints + "", "Green");
                    AddText(" unspent points!");
                }
                else if (currentSave.player.level < 60)
                    AddLine("Next talent point at level " + currentSave.player.level + (currentSave.player.level % 2 == 0 ? 2 : 1));
                else
                    AddLine("Look for orbs of power to gain additional talent points!");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    var title = CDesktop.title;
                    SwitchDesktop("Map");
                    if (title == "TalentScreen")
                        PlaySound("DesktopTalentScreenClose");
                    else if (title == "SpellbookScreen")
                        PlaySound("DesktopSpellbookScreenClose");
                    CloseDesktop(title);
                });
            });
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddLine(a.talentTrees[0].name + ": " + a.talentTrees[0].talents.Count(x => currentSave.player.abilities.Contains(x.ability)));
            });
            SetRegionGroupWidth(213);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddLine(a.talentTrees[1].name + ": " + a.talentTrees[1].talents.Count(x => currentSave.player.abilities.Contains(x.ability)));
            });
            SetRegionGroupWidth(212);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddLine(a.talentTrees[2].name + ": " + a.talentTrees[2].talents.Count(x => currentSave.player.abilities.Contains(x.ability)));
            });
            SetRegionGroupWidth(213);
        }, true),
        new("InstanceHeader", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(638);
            AddPaddingRegion(() =>
            {
                AddLine(instance.name);
                AddSmallButton("OtherClose",
                (h) =>
                {
                    var title = CDesktop.title;
                    CloseDesktop(title);
                    SwitchDesktop("Map");
                });
            });
        }, true),
        new("Console", () => {
            SetAnchor(Top);
            AddRegionGroup();
            SetRegionGroupWidth(638);
            AddInputRegion(String.consoleInput, InputType.Everything);
            AddSmallButton("OtherClose", (h) => { CloseWindow(h.window); });
        },  true),

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
            AddButtonRegion(() => { AddLine("Races"); }, (h) =>
            {
                racesSearch = races;
                SpawnDesktopBlueprint("ObjectManagerRaces");
            });
            AddButtonRegion(() => { AddLine("Classes"); }, (h) =>
            {
                specsSearch = specs;
                SpawnDesktopBlueprint("ObjectManagerClasses");
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
            AddButtonRegion(() => { AddLine("Factions"); }, (h) =>
            {
                factionsSearch = factions;
                SpawnDesktopBlueprint("ObjectManagerFactions");
            });
            AddPaddingRegion(() => { AddLine("Actions:"); });
            AddButtonRegion(() => { AddLine("Save data"); }, (h) =>
            {
                Serialize(races, "races", false, false, prefix);
                Serialize(specs, "classes", false, false, prefix);
                Serialize(abilities, "abilities", false, false, prefix);
                Serialize(buffs, "buffs", false, false, prefix);
                Serialize(areas, "areas", false, false, prefix);
                Serialize(instances, "instances", false, false, prefix);
                Serialize(complexes, "complexes", false, false, prefix);
                Serialize(towns, "towns", false, false, prefix);
                Serialize(items, "items", false, false, prefix);
                Serialize(itemSets, "sets", false, false, prefix);
                Serialize(factions, "factions", false, false, prefix);
            });
            AddPaddingRegion(() => { });
        }),
        new("HostileAreasSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(162);
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = areasSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton("Site" + foo.type, (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    area = areasSearch[index + 10 * regionGroup.pagination];
                    instance = null;
                    complex = null;
                    SetDesktopBackground("Areas/Area" + (area.zone + area.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = townsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(factions.Find(x => x.name == foo.faction).Icon(), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    town = townsSearch[index + 10 * regionGroup.pagination];
                    SetDesktopBackground("Areas/Area" + (town.zone + town.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
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
            SetRegionGroupWidth(162);
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = instancesSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton("Site" + foo.type, (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    area = null;
                    instance = instancesSearch[index + 10 * regionGroup.pagination];
                    complex = null;
                    SetDesktopBackground("Areas/Area" + instance.name.Replace("'", "").Replace(".", "").Replace(" ", ""));
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
            SetRegionGroupWidth(162);
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = complexesSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton("SiteComplex", (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    area = null;
                    instance = null;
                    complex = complexesSearch[index + regionGroup.perPage * regionGroup.pagination];
                    SetDesktopBackground("Areas/Complex" + complex.name.Replace("'", "").Replace(".", "").Replace(" ", ""));
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = Assets.assets.ambienceSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.Substring(8));
                        AddSmallButton("OtherSound", (h) =>
                        {
                            PlayAmbience(foo.Replace(".ogg", ""));
                        });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                AddInputLine(String.search, InputType.Everything);
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = Assets.assets.soundsSearch[index + regionGroup.perPage * regionGroup.pagination];
                        AddLine(foo);
                        AddSmallButton("OtherSound", (h) =>
                        {
                            PlaySound(foo.Replace(".ogg", ""));
                        });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                    if (item != null)
                        SpawnWindowBlueprint("ObjectManagerItems");
                    else if (ability != null)
                        SpawnWindowBlueprint("ObjectManagerAbilities");
                    else if (buff != null)
                        SpawnWindowBlueprint("ObjectManagerBuffs");
                    else if (spec != null)
                        SpawnWindowBlueprint("ObjectManagerClasses");
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = Assets.assets.itemIconsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.Substring(4));
                        AddSmallButton(Assets.assets.itemIconsSearch[index + 10 * regionGroup.pagination].Replace(".png", ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                        SpawnWindowBlueprint("ObjectManagerItems");
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
                    if (ability != null)
                        SpawnWindowBlueprint("ObjectManagerAbilities");
                    else if (buff != null)
                        SpawnWindowBlueprint("ObjectManagerBuffs");
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = Assets.assets.abilityIconsSearch[index + regionGroup.perPage * regionGroup.pagination];
                        AddLine(foo.Substring(7));
                        AddSmallButton(Assets.assets.abilityIconsSearch[index + regionGroup.perPage * regionGroup.pagination].Replace(".png", ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                        SpawnWindowBlueprint("ObjectManagerAbilities");
                    }
                    else if (buff != null)
                    {
                        buff.icon = foo.Replace(".png", "");
                        Respawn("ObjectManagerBuff");
                        SpawnWindowBlueprint("ObjectManagerBuffs");
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
                    if (faction != null)
                        SpawnWindowBlueprint("ObjectManagerFactions");
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = Assets.assets.factionIconsSearch[index + regionGroup.perPage * regionGroup.pagination];
                        AddLine(foo.Substring(7));
                        AddSmallButton(Assets.assets.factionIconsSearch[index + regionGroup.perPage * regionGroup.pagination].Replace(".png", ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                        SpawnWindowBlueprint("ObjectManagerFactions");
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = Assets.assets.portraitsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.Replace("Portrait", ""));
                        AddSmallButton(Assets.assets.portraitsSearch[index + 10 * regionGroup.pagination].Replace(".png", ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = possibleEffects[index + 10 * regionGroup.pagination];
                        AddLine(foo);
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = possibleTriggers[index + 10 * regionGroup.pagination];
                        AddLine(foo);
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                        AddInputLine(String.resourceAmount, InputType.Numbers);
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
            AddButtonRegion(() =>
            {
                AddLine("Remove this trigger");
            },
            (h) =>
            {
                eventEdit.triggers.RemoveAt(selectedTrigger);
                CloseWindow(h.window);
                Respawn("ObjectManagerEventTriggers");
            });
            AddRegionGroup();
            SetRegionGroupWidth(148);
            SetRegionGroupHeight(316);
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            AddButtonRegion(() =>
            {
                AddLine("Remove this trigger");
            },
            (h) =>
            {
                eventEdit.triggers.RemoveAt(selectedTrigger);
                CloseWindow(h.window);
                Respawn("ObjectManagerEventTriggers");
            });
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
                AddText("9", "Gray");
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
                    String.trailStrength.Set(effect.ContainsKey("TrailStrength") ? effect["TrailStrength"] : "0,85");
                    String.animationSpeed.Set(effect.ContainsKey("AnimationSpeed") ? effect["AnimationSpeed"] : "1,5");
                    String.shatterDensity.Set(effect.ContainsKey("ShatterDensity") ? effect["ShatterDensity"] : "2");
                    String.shatterDegree.Set(effect.ContainsKey("ShatterDegree") ? effect["ShatterDegree"] : "0,7");
                    String.shatterSpeed.Set(effect.ContainsKey("ShatterSpeed") ? effect["ShatterSpeed"] : "4");
                    String.await.Set(effect.ContainsKey("Await") ? effect["Await"] : "0");
                    String.powerScale.Set(effect.ContainsKey("PowerScale") ? effect["PowerScale"] : "1");
                    String.resourceAmount.Set(effect.ContainsKey("ResourceAmount") ? effect["ResourceAmount"] : "1");
                    String.buffDuration.Set(effect.ContainsKey("BuffDuration") ? effect["BuffDuration"] : "3");
                    Respawn("ObjectManagerEventEffects");
                    Respawn("ObjectManagerEventEffect");
                });
            }
            AddPaddingRegion(() => { SetRegionAsGroupExtender(); });
            if (eventEdit.effects.Count < 9)
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
                    AddInputLine(String.powerScale, InputType.Decimal);
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
                    AddInputLine(String.buffDuration, InputType.Numbers);
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
                            effect["ResourceType"] = "None";
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
                    AddInputLine(String.resourceAmount, InputType.Numbers);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ResourceAmount"))
                            effect["ResourceAmount"] = "1";
                        String.resourceAmount.Set("1");
                        h.window.Respawn();
                    });
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine("Await:", "DarkGray");
                AddInputLine(String.await, InputType.Numbers);
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
                AddInputLine(String.chanceBase, InputType.Numbers);
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
                AddInputLine(String.chance, InputType.Numbers);
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
                    AddInputLine(String.animationSpeed, InputType.Decimal);
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
                    AddInputLine(String.animationArc, InputType.Decimal);
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
                    AddInputLine(String.trailStrength, InputType.Decimal);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("TrailStrength"))
                            effect["TrailStrength"] = "0,85";
                        String.trailStrength.Set("0,85");
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
                    AddInputLine(String.shatterDegree, InputType.Decimal);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ShatterDegree"))
                            effect["ShatterDegree"] = "0,7";
                        String.shatterDegree.Set("0,7");
                        h.window.Respawn();
                    });
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Shatter density:", "DarkGray");
                    AddInputLine(String.shatterDensity, InputType.Numbers);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ShatterDensity"))
                            effect["ShatterDensity"] = "2";
                        String.shatterDensity.Set("2");
                        h.window.Respawn();
                    });
                });
                AddPaddingRegion(() =>
                {
                    AddLine("Shatter speed:", "DarkGray");
                    AddInputLine(String.shatterSpeed, InputType.Decimal);
                    AddSmallButton("OtherReverse", (h) =>
                    {
                        if (effect.ContainsKey("ShatterSpeed"))
                            effect["ShatterSpeed"] = "4";
                        String.shatterSpeed.Set("4");
                        h.window.Respawn();
                    });
                });
            }
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
            SetRegionGroupWidth(162);
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = itemsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon, (h) => { });
                        AddSmallButtonOverlay("OtherRarity" + foo.rarity + "Big");
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                (h) => () =>
                {
                    SetAnchor(Center);
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
                    classes = new(),
                    icon = "ItemEgg03",
                    type = "Miscellaneous",
                    rarity = "Common",
                    price = 0.0001
                };
                items.Add(item);
                itemsSearch = items.FindAll(x => x.name.ToLower().Contains(String.search.Value().ToLower()));
                String.objectName.Set(item.name);
                String.price.Set(item.price + "");
                String.itemPower.Set(item.ilvl + "");
                String.requiredLevel.Set(item.lvl + "");
                Respawn("ObjectManagerItem");
                h.window.Rebuild();
            });
        }),
        new("ObjectManagerItem", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Item:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything, item.rarity);
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
                AddInputLine(String.price, InputType.Decimal);
            });
            AddPaddingRegion(() =>
            {
                AddLine("Item power:", "DarkGray");
                AddInputLine(String.itemPower, InputType.Numbers);
            });
            AddPaddingRegion(() =>
            {
                AddLine("Required level:", "DarkGray");
                AddInputLine(String.requiredLevel, InputType.Numbers);
            });
            AddPaddingRegion(() => { });
        }),
        new("ItemSetsSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(162);
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = itemSetsSearch[index + 5 * regionGroup.pagination];
                        AddLine(foo.name);
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                            AddSmallButton(setItems[J].icon, (h) => { }, (h) => () =>
                            {
                                SetAnchor(Center);
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
                h.window.Rebuild();
            });
        }),
        new("ObjectManagerItemSet", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Item set:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything);
            AddPaddingRegion(() => { });
        }),
        new("AbilitiesSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(162);
            AddHeaderRegion(() =>
            {
                AddLine("Sort abilities:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("AbilitiesSort");
                    Respawn("ObjectManagerAbilities");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                abilities = abilities.OrderBy(x => x.name).ToList();
                abilitiesSearch = abilitiesSearch.OrderBy(x => x.name).ToList();
                CloseWindow("AbilitiesSort");
                Respawn("ObjectManagerAbilities");
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By cost", "Black");
            },
            (h) =>
            {
                abilities = abilities.OrderByDescending(x => x.cost == null ? -1 : x.cost.Sum(y => y.Value)).ToList();
                abilitiesSearch = abilitiesSearch.OrderByDescending(x => x.cost == null ? -1 : x.cost.Sum(y => y.Value)).ToList();
                CloseWindow("AbilitiesSort");
                Respawn("ObjectManagerAbilities");
                PlaySound("DesktopInventorySort", 0.2f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By cooldown", "Black");
            },
            (h) =>
            {
                abilities = abilities.OrderByDescending(x => x.cooldown).ToList();
                abilitiesSearch = abilitiesSearch.OrderByDescending(x => x.cooldown).ToList();
                CloseWindow("AbilitiesSort");
                Respawn("ObjectManagerAbilities");
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
                AddInputLine(String.fire, InputType.Numbers, String.fire.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementFireRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Earth: ", "DarkGray");
                AddInputLine(String.earth, InputType.Numbers, String.earth.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementEarthRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Water: ", "DarkGray");
                AddInputLine(String.water, InputType.Numbers, String.water.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementWaterRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Air: ", "DarkGray");
                AddInputLine(String.air, InputType.Numbers, String.air.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementAirRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Frost: ", "DarkGray");
                AddInputLine(String.frost, InputType.Numbers, String.frost.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementFrostRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Decay: ", "DarkGray");
                AddInputLine(String.decay, InputType.Numbers, String.decay.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementDecayRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Shadow: ", "DarkGray");
                AddInputLine(String.shadow, InputType.Numbers, String.shadow.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementShadowRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Order: ", "DarkGray");
                AddInputLine(String.order, InputType.Numbers, String.order.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementOrderRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Arcane: ", "DarkGray");
                AddInputLine(String.arcane, InputType.Numbers, String.arcane.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementArcaneRousing", (h) => { });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Lightning: ", "DarkGray");
                AddInputLine(String.lightning, InputType.Numbers, String.lightning.Value() == "0" ? "DarkGray" : "Gray");
                AddSmallButton("ElementLightningRousing", (h) => { });
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = abilitiesSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon, (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                (h) => () =>
                {
                    SetAnchor(Center);
                    PrintAbilityTooltip(null, null, abilitiesSearch[index + 10 * regionGroup.pagination]);
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
                h.window.Rebuild();
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
                    Board.NewBoard(ability);
                    SpawnDesktopBlueprint("GameSimulation");
                });
            });
            AddInputRegion(String.objectName, InputType.Everything, ability.name);
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
                AddInputLine(String.cooldown, InputType.Numbers);
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
            SetRegionGroupWidth(162);
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = buffsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon, (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                (h) => () =>
                {
                    SetAnchor(Center);
                    PrintBuffTooltip(null, null, (buffsSearch[index + 10 * regionGroup.pagination], 0, null));
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
                    h.window.Rebuild();
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
                    h.window.Rebuild();
                }
            });
        }),
        new("ObjectManagerBuff", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Buff:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything);
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
            SetRegionGroupWidth(162);
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
                    race = null; racesSearch = null;
                    CloseDesktop("ObjectManagerRaces");
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = racesSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.portrait + (foo.genderedPortrait ? "Female" : ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    race = racesSearch[index + 10 * regionGroup.pagination];
                    String.objectName.Set(race.name);
                    String.vitality.Set(race.vitality + "");
                    Respawn("ObjectManagerRace");
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
                h.window.Rebuild();
            });
        }),
        new("ObjectManagerRace", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Race:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything);
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
                AddPaddingRegion(() => { AddLine("Faction:", "DarkGray"); });
                AddHeaderRegion(() => { AddLine(race.faction); });
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
                AddInputRegion(String.vitality, InputType.Decimal);
                //AddPaddingRegion(() => { AddLine("Abilities:", "DarkGray"); });
                //for (int)
                //AddButtonRegion(() =>
                //{
                //    AddLine(race.kind);
                //},
                //(h) =>
                //{
                //    if (race.kind == "Common")
                //        race.kind = "Rare";
                //    else if (race.kind == "Rare")
                //        race.kind = "Elite";
                //    else if (race.kind == "Elite")
                //        race.kind = "Common";
                //});
            }
            AddPaddingRegion(() => { });
        }),
        new("FactionsSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(162);
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
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = factionsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.Icon(), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                h.window.Rebuild();
            });
        }),
        new("ObjectManagerFaction", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Faction:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything);
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
        new("ObjectManagerClasses", () => {
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
                AddLine("Classes:");
                AddSmallButton("OtherClose", (h) =>
                {
                    spec = null; specsSearch = null;
                    CloseDesktop("ObjectManagerClasses");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    specs.Reverse();
                    specsSearch.Reverse();
                    Respawn("ObjectManagerClasses");
                    PlaySound("DesktopInventorySort", 0.2f);
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search, InputType.Everything);
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
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = specsSearch[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.icon, (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    spec = specsSearch[index + 10 * regionGroup.pagination];
                    String.objectName.Set(spec.name);
                    Respawn("ObjectManagerClass");
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(specs.Count + " classes", "DarkGray");
                if (specs.Count != specsSearch.Count)
                    AddLine(specsSearch.Count + " found in search", "DarkGray");
            });
            //AddButtonRegion(() =>
            //{
            //    AddLine("Create a new class");
            //},
            //(h) =>
            //{
            //    spec = new Class()
            //    {
            //        name = "Class #" + specs.Count,
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
            //    CloseWindow("ObjectManagerClass");
            //    SpawnWindowBlueprint("ObjectManagerClass");
            //    h.window.Rebuild();
            //});
        }),
        new("ObjectManagerClass", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Class:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything, spec.name);
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
            //        CloseWindow("ObjectManagerClasses");
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
            AddHotkey(BackQuote, () => { SpawnDesktopBlueprint("DevPanel"); });
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("Settings"))
                {
                    PlaySound("DesktopButtonClose");
                    SpawnWindowBlueprint("TitleScreenMenu");
                }
                if (CloseWindow("CharacterCreation"))
                {
                    PlaySound("DesktopButtonClose");
                    CloseWindow("CharacterCreationRightSide");
                    SpawnWindowBlueprint("CharacterRoster");
                    SpawnWindowBlueprint("CharacterInfo");
                    SpawnWindowBlueprint("TitleScreenSingleplayer");
                }
                else
                {
                    PlaySound("DesktopButtonClose");
                    CloseWindow("TitleScreenSingleplayer");
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
            loadingBar = new GameObject[2];
            loadingBar[0] = new GameObject("LoadingBarBegin", typeof(SpriteRenderer));
            loadingBar[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/LoadingBarEnd");
            loadingBar[0].transform.position = new Vector3(-1181, 863);
            loadingBar[1] = new GameObject("LoadingBar", typeof(SpriteRenderer));
            loadingBar[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/LoadingBarStretch");
            loadingBar[1].transform.position = new Vector3(-1178, 863);
            OrderLoadingMap();
            AddHotkey(W, () => { MoveCamera(new Vector3(0, EuelerGrowth())); }, false);
            AddHotkey(A, () => { MoveCamera(new Vector3(-EuelerGrowth(), 0)); }, false);
            AddHotkey(S, () => { MoveCamera(new Vector3(0, -EuelerGrowth())); }, false);
            AddHotkey(D, () => { MoveCamera(new Vector3(EuelerGrowth(), 0)); }, false);
            AddHotkey(C, () =>
            {
                SpawnDesktopBlueprint("CharacterSheet");
                SwitchDesktop("CharacterSheet");
                PlaySound("DesktopCharacterSheetOpen");
            });
            AddHotkey(N, () => { SpawnDesktopBlueprint("TalentScreen"); SwitchDesktop("TalentScreen"); });
            AddHotkey(P, () => { SpawnDesktopBlueprint("SpellbookScreen"); SwitchDesktop("SpellbookScreen"); });
            AddHotkey(B, () => { SpawnDesktopBlueprint("EquipmentScreen"); SwitchDesktop("EquipmentScreen"); });
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopMenuOpen");
                SpawnDesktopBlueprint("GameMenu");
            });
            AddHotkey(BackQuote, () => { SpawnDesktopBlueprint("DevPanel"); });
            AddHotkey(L, () => { SpawnWindowBlueprint("ItemDrop"); });

            void MoveCamera(Vector3 amount)
            {
                CDesktop.cameraDestination += new Vector2(amount.x, amount.y) / 5;
            }
        }),
        new("HostileAreaEntrance", () =>
        {
            SetDesktopBackground("Areas/Area" + (area.zone + area.name).Replace("'", "").Replace(".", "").Replace(" ", "") + (area.specialClearBackground && area.eliteEncounters.All(x => currentSave.elitesKilled.ContainsKey(x.who)) ? "Cleared" : ""));
            SpawnWindowBlueprint("HostileArea: " + area.name);
            SpawnWindowBlueprint("HostileAreaRightSide");
            AddHotkey(Escape, () =>
            {
                if (area.complexPart)
                {
                    CloseDesktop("HostileAreaEntrance");
                    SpawnDesktopBlueprint("ComplexEntrance");
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("HostileAreaEntrance");
                }
            });
        }),
        new("TownEntrance", () =>
        {
            SetDesktopBackground("Areas/Area" + (town.zone + town.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
            SpawnWindowBlueprint("Town: " + town.name);
            SpawnWindowBlueprint("TownLeftSide");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInstanceClose");
                CloseDesktop("TownEntrance");
            });
        }),
        new("InstanceEntrance", () =>
        {
            SetDesktopBackground("Areas/Area" + instance.name.Replace("'", "").Replace(".", "").Replace(" ", ""));
            SpawnWindowBlueprint(instance.type + ": " + instance.name);
            SpawnWindowBlueprint("InstanceLeftSide");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("HostileArea: " + area?.name))
                {
                    area = null;
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground("Areas/Area" + instance.name.Replace("'", "").Replace(".", "").Replace(" ", ""));
                    Respawn("InstanceLeftSide");
                }
                else if (instance.complexPart)
                {
                    CloseDesktop("InstanceEntrance");
                    SpawnDesktopBlueprint("ComplexEntrance");
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("InstanceEntrance");
                }
            });
            AddHotkey(BackQuote, () =>
            {
                if (area == null) return;
                if (area.commonEncounters != null)
                    foreach (var encounter in area.commonEncounters)
                        if (!races.Exists(x => x.name == encounter.who))
                            races.Insert(0, new Race()
                            {
                                name = encounter.who,
                                abilities = new(),
                                kind = "Common",
                                portrait = "PortraitChicken",
                                vitality = 1.0,
                            });
                if (area.eliteEncounters != null)
                    foreach (var encounter in area.eliteEncounters)
                        if (!races.Exists(x => x.name == encounter.who))
                            races.Insert(0, new Race()
                            {
                                name = encounter.who,
                                abilities = new(),
                                kind = "Elite",
                                portrait = "PortraitCow",
                                vitality = 1.0,
                            });
                racesSearch = races;
                SpawnDesktopBlueprint("ObjectManagerRaces");
            });
        }),
        new("ComplexEntrance", () =>
        {
            //locationName = complex.name;
            //SpawnWindowBlueprint("LocationInfo");
            SetDesktopBackground("Areas/Complex" + complex.name.Replace("'", "").Replace(".", "").Replace(" ", ""));
            SpawnWindowBlueprint("Complex: " + complex.name);
            SpawnWindowBlueprint("ComplexLeftSide");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("HostileArea: " + area?.name))
                {
                    area = null;
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground("Areas/Complex" + complex.name.Replace("'", "").Replace(".", "").Replace(" ", ""));
                    Respawn("ComplexLeftSide");
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("ComplexEntrance");
                }
            });
        }),
        new("Game", () =>
        {
            locationName = Board.board.area.name;
            PlaySound("DesktopEnterCombat");
            SetDesktopBackground("Areas/Area" + (Board.board.area.zone + Board.board.area.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
            SpawnWindowBlueprint("BattleBoard");
            SpawnWindowBlueprint("BufferBoard");
            SpawnWindowBlueprint("PlayerBattleInfo");
            SpawnWindowBlueprint("LocationInfo");
            SpawnWindowBlueprint("EnemyBattleInfo");
            SpawnWindowBlueprint("PlayerResources");
            SpawnWindowBlueprint("EnemyResources");
            Board.board.Reset();
            AddHotkey(PageUp, () => {
                Board.board.player.resources = new Dictionary<string, int>
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
                Board.board.enemy.resources = new Dictionary<string, int>
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
            AddHotkey(BackQuote, () => { SpawnDesktopBlueprint("DevPanel"); });
            AddHotkey(KeypadMultiply, () => { Board.board.enemy.health = 0; });
        }),
        new("GameSimulation", () =>
        {
            locationName = Board.board.area.name;
            PlaySound("DesktopEnterCombat");
            SetDesktopBackground("Areas/Area" + (Board.board.area.zone + Board.board.area.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
            SpawnWindowBlueprint("BattleBoard");
            SpawnWindowBlueprint("BufferBoard");
            SpawnWindowBlueprint("PlayerBattleInfo");
            SpawnWindowBlueprint("LocationInfo");
            SpawnWindowBlueprint("EnemyBattleInfo");
            SpawnWindowBlueprint("PlayerResources");
            SpawnWindowBlueprint("EnemyResources");
            Board.board.Reset();
            AddHotkey(PageUp, () => {
                Board.board.player.resources = new Dictionary<string, int>
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
                Board.board.enemy.resources = new Dictionary<string, int>
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
        new("CharacterSheet", () =>
        {
            SetDesktopBackground("Stone");
            SpawnWindowBlueprint("ReturnToMap");
            SpawnWindowBlueprint("CharacterNeckSlot");
            SpawnWindowBlueprint("CharacterBackSlot");
            SpawnWindowBlueprint("CharacterRingSlot");
            SpawnWindowBlueprint("CharacterHeadSlot");
            SpawnWindowBlueprint("CharacterChestSlot");
            SpawnWindowBlueprint("CharacterLegsSlot");
            SpawnWindowBlueprint("CharacterFeetSlot");
            SpawnWindowBlueprint("CharacterShouldersSlot");
            SpawnWindowBlueprint("CharacterHandsSlot");
            SpawnWindowBlueprint("CharacterWaistSlot");
            SpawnWindowBlueprint("CharacterSpecialSlot");
            SpawnWindowBlueprint("CharacterMainHandSlot");
            SpawnWindowBlueprint("CharacterOffHandSlot");
            SpawnWindowBlueprint("CharacterTrinketSlot");
            SpawnWindowBlueprint("CharacterStats");
            AddHotkey(C, () =>
            {
                PlaySound("DesktopCharacterSheetClose");
                CloseDesktop("CharacterSheet");
            });
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopCharacterSheetClose");
                CloseDesktop("CharacterSheet");
            });
        }),
        new("TalentScreen", () =>
        {
            PlaySound("DesktopTalentScreenOpen");
            SetDesktopBackground("StoneSplitLong", false);
            SpawnWindowBlueprint("TalentHeader");
            var playerClass = currentSave.player.GetClass();
            for (int spec = 0; spec < 3; spec++)
                for (int row = 0; row <= playerClass.talentTrees[spec].talents.Max(x => x.row); row++)
                    for (int col = 0; col < 3; col++)
                        if (windowBlueprints.Exists(x => x.title == "Talent" + spec + row + col))
                            if (playerClass.talentTrees[spec].talents.Exists(x => x.row == row && x.col == col))
                                SpawnWindowBlueprint("Talent" + spec + row + col);
            AddHotkey(N, () => { SwitchDesktop("Map"); CloseDesktop("TalentScreen"); PlaySound("DesktopTalentScreenClose"); });
            AddHotkey(Escape, () => { SwitchDesktop("Map"); CloseDesktop("TalentScreen"); PlaySound("DesktopTalentScreenClose"); });
            AddHotkey(W, () =>
            {
                var amount = new Vector3(0, (float)Math.Round(EuelerGrowth())) / 2;
                CDesktop.screen.transform.position += amount;
                cursor.transform.position += amount;
                if (CDesktop.screen.transform.position.y > -140)
                {
                    var off = CDesktop.screen.transform.position.y + 140f;
                    CDesktop.screen.transform.position -= new Vector3(0, off);
                    cursor.transform.position -= new Vector3(0, off);
                }
            },  false);
            AddHotkey(S, () =>
            {
                var amount = new Vector3(0, -(float)Math.Round(EuelerGrowth())) / 2;
                CDesktop.screen.transform.position += amount;
                cursor.transform.position += amount;
                if (CDesktop.screen.transform.position.y < -528)
                {
                    var off = CDesktop.screen.transform.position.y + 528f;
                    CDesktop.screen.transform.position -= new Vector3(0, off);
                    cursor.transform.position -= new Vector3(0, off);
                }
            },  false);
        }),
        new("SpellbookScreen", () =>
        {
            PlaySound("DesktopSpellbookScreenOpen");
            locationName = "Spellbook";
            SpawnWindowBlueprint("LocationInfo");
            SetDesktopBackground("Skin");
            SpawnWindowBlueprint("SpellbookAbilityList");
            SpawnWindowBlueprint("PlayerSpellbookInfo");
            SpawnWindowBlueprint("SpellbookResources");
            AddHotkey(P, () => { SwitchDesktop("Map"); CloseDesktop("SpellbookScreen"); PlaySound("DesktopSpellbookScreenClose"); });
            AddHotkey(Escape, () => { SwitchDesktop("Map"); CloseDesktop("SpellbookScreen"); PlaySound("DesktopSpellbookScreenClose"); });
        }),
        new("EquipmentScreen", () =>
        {
            PlaySound("DesktopInventoryOpen");
            locationName = "Inventory";
            SpawnWindowBlueprint("LocationInfo");
            SetDesktopBackground("Leather");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("Inventory");
            AddHotkey(B, () =>
            {
                PlaySound("DesktopInventoryClose");
                CloseDesktop("EquipmentScreen");
            });
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInventoryClose");
                CloseDesktop("EquipmentScreen");
            });
        }),
        new("BankScreen", () =>
        {
            currentSave.banks ??= new();
            if (!currentSave.banks.ContainsKey(town.name))
                currentSave.banks.Add(town.name, new() { items = new() });
            PlaySound("DesktopBankOpen", 0.2f);
            SetDesktopBackground("Areas/Area" + (town.zone + town.name).Replace("'", "").Replace(".", "").Replace(" ", "") + "Bank");
            SpawnWindowBlueprint("Bank");
            SpawnWindowBlueprint("Inventory");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopBankClose");
                CloseDesktop("BankScreen");
                SwitchDesktop("TownEntrance");
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
        new("GameOver", () =>
        {
            SetDesktopBackground("Stone");
            SpawnWindowBlueprint("GameOver");
            AddHotkey(Escape, () =>
            {
                CloseSave();
                SaveGames();
                saves[settings.selectedRealm].Remove(currentSave);
                //graveyard.Add(currentSave);
                CloseDesktop("GameOver");
                CloseDesktop("Map");
                CloseDesktop("TitleScreen");
                SpawnDesktopBlueprint("TitleScreen");
            });
        }),

        #endregion

        #region Dev Panel

        new("DevPanel", () =>
        {
            Serialize(races, "races", true, false, prefix);
            Serialize(specs, "classes", true, false, prefix);
            Serialize(abilities, "abilities", true, false, prefix);
            Serialize(buffs, "buffs", true, false, prefix);
            Serialize(areas, "areas", true, false, prefix);
            Serialize(instances, "instances", true, false, prefix);
            Serialize(complexes, "complexes", true, false, prefix);
            Serialize(towns, "towns", true, false, prefix);
            Serialize(items, "items", true, false, prefix);
            Serialize(itemSets, "sets", true, false, prefix);
            Serialize(factions, "factions", true, false, prefix);
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerLobby");
            AddHotkey(Escape, () => { CloseDesktop("DevPanel"); });
        }),
        new("ObjectManagerHostileAreas", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerHostileAreas");
            AddHotkey(Escape, () => { area = null; areasSearch = null; CloseDesktop("ObjectManagerHostileAreas"); });
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
        new("ObjectManagerClasses", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerClasses");
            AddHotkey(Escape, () => { spec = null; specsSearch = null; CloseDesktop("ObjectManagerClasses"); });
            AddPaginationHotkeys();
        }),
        new("ObjectManagerAbilities", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerAbilities");
            AddHotkey(Escape, () => { ability = null; abilitiesSearch = null; eventEdit = null; CloseDesktop("ObjectManagerAbilities"); });
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
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            group.pagination += 1;
            var max = group.maxPagination();
            if (group.pagination >= max)
                group.pagination = max - 1;
            window.Rebuild();
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
            window.Rebuild();
        }, false);
        AddHotkey(A, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            group.pagination -= 1;
            if (group.pagination < 0)
                group.pagination = 0;
            window.Rebuild();
        });
        AddHotkey(A, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            group.pagination -= (int)Math.Round(EuelerGrowth()) / 2;
            if (group.pagination < 0)
                group.pagination = 0;
            window.Rebuild();
        }, false);
        AddHotkey(Alpha1, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            group.pagination = 0;
            window.Rebuild();
        });
        AddHotkey(Alpha2, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 1.1);
            window.Rebuild();
        });
        AddHotkey(Alpha3, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 2.2);
            window.Rebuild();
        });
        AddHotkey(Alpha4, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 3.3);
            window.Rebuild();
        });
        AddHotkey(Alpha5, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 4.4);
            window.Rebuild();
        });
        AddHotkey(Alpha6, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 5.5);
            window.Rebuild();
        });
        AddHotkey(Alpha7, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 6.6);
            window.Rebuild();
        });
        AddHotkey(Alpha8, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPagination != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 7.7);
            window.Rebuild();
        });
        AddHotkey(Alpha9, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max / 10 * 8.8);
            window.Rebuild();
        });
        AddHotkey(Alpha0, () =>
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null) return;
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            var max = group.maxPagination();
            group.pagination = (int)(max - 1);
            window.Rebuild();
        });
    }
}
