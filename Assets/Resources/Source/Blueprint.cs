using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static UnityEngine.KeyCode;

using static Root;
using static Root.Anchor;

using static Item;
using static Buff;
using static Race;
using static Zone;
using static Site;
using static Spec;
using static Sound;
using static Event;
using static Person;
using static Defines;
using static MapGrid;
using static Faction;
using static ItemSet;
using static Ability;
using static SitePath;
using static SaveGame;
using static Coloring;
using static PersonType;
using static GameSettings;
using static Serialization;
using static PermanentEnchant;
using static SiteSpiritHealer;
using static SiteHostileArea;
using static SiteInstance;
using static SiteComplex;
using static SiteTown;
using static UnityEngine.GraphicsBuffer;

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
                AddLine("0.4.0", "Black", "Center");
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
                AddLine("Graveyard", "", "Center");
            },
            (h) =>
            {

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
                    if (find != null) CDesktop.cameraDestination = new Vector2(find.x, find.y) * mapGridSize;
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
            AddPaddingRegion(() =>
            {
                AddInputLine(String.promptConfirm, "DangerousRed");
            });
        }, true),
        new("ConfirmItemDestroy", () => {
            SetAnchor(-115, 146);
            AddHeaderGroup();
            SetRegionGroupWidth(228);
            AddPaddingRegion(() =>
            {
                AddLine("You are about to destroy", "", "Center");
                AddLine(item.name, item.rarity, "Center");
            });
            AddRegionGroup();
            SetRegionGroupWidth(114);
            AddButtonRegion(() =>
            {
                SetRegionBackground(RegionBackgroundType.RedButton);
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
            SetRegionGroupWidth(114);
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
        new("CharacterRoster", () =>
        {
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
                    foreach (var slot in saves[settings.selectedRealm])
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
            AddPaddingRegion(() =>
            {
                AddCheckbox(settings.snapCamera);
                AddLine("Snap camera to sites", "Gray");
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
                        Board.board.EndCombat("Fled");
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
                    AddBigButton(Board.board.player.Spec().icon,
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
                            foreach (var element in abilityObj.cost)
                                Board.board.log.elementsUsed.Inc(element.Key, element.Value);
                            Board.board.temporaryElementsPlayer = new();
                            h.window.desktop.RebuildAll();
                        }
                    },
                    null,
                    (h) => () =>
                    {
                        PrintAbilityTooltip(Board.board.player, Board.board.enemy, abilityObj, Board.board.player.abilities[abilityObj.name]);
                    }
                );
            }
        }),
        new("PlayerEquipmentInfo", () => {
            SetAnchor(TopLeft, 0, -19);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(290);
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
            AddPaddingRegion(() => { AddLine(); AddLine(); AddLine(); AddLine(); });

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
                            null,
                            (h) => () =>
                            {
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
        new("BossQueue", () => {
            if (area.eliteEncounters == null) return;
            var bosses = area.eliteEncounters.FindAll(x => !currentSave.elitesKilled.ContainsKey(x.who));
            if (bosses.Count == 0) return;
            var boss = bosses[0];
            if (boss == null || !currentSave.siteProgress.ContainsKey(area.name)) return;
            var temp = area.progression.Find(x => x.bossName == boss.who);
            if (temp == null || currentSave.siteProgress[area.name] < temp.point) return;
            var bossBackground = new GameObject("BossBackground", typeof(SpriteRenderer));
            bossBackground.transform.parent = CDesktop.LBWindow.transform;
            bossBackground.transform.localPosition = new Vector2(20, -20);
            bossBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/BossBackground");
            SetAnchor(BottomLeft, 23, 42);
            AddHeaderGroup();
            AddPaddingRegion(() =>
            {
                var race = races.Find(x => x.name == boss.who);
                AddBigButton(race == null ? "OtherUnknown" : race.portrait,
                (h) =>
                {
                    Board.NewBoard(area.RollEncounter(boss), area);
                    SpawnDesktopBlueprint("Game");
                    SwitchDesktop("Game");
                });
            });
        }),
        new("SpellbookAbilityListActivated", () => {
            SetAnchor(TopRight, 0, -19);
            var activeAbilities = abilities.FindAll(x => !x.hide && x.cost != null && currentSave.player.abilities.ContainsKey(x.name)).ToDictionary(x => x, x => currentSave.player.abilities[x.name]);
            AddHeaderGroup(() => abilities.Count(x => !x.hide && x.cost != null && currentSave.player.abilities.ContainsKey(x.name)), 7);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(342);
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
            for (int i = 0; i < 7; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (activeAbilities.Count > index + 7 * regionGroup.pagination)
                    {
                        var key = activeAbilities.ToList()[index + 7 * regionGroup.pagination];
                        AddLine(key.Key.name);
                        AddLine("Rank: ", "DarkGray");
                        AddText("" + ToRoman(key.Value + 1));
                        AddBigButton(key.Key.icon,
                            (h) =>
                            {
                                var key = activeAbilities.ToList()[index + 7 * regionGroup.pagination];
                                if (!currentSave.player.actionBars.Exists(x => x.ability == key.Key.name) && currentSave.player.actionBars.Count < currentSave.player.ActionBarsAmount())
                                {
                                    currentSave.player.actionBars.Add(new ActionBar(key.Key.name));
                                    Respawn("PlayerSpellbookInfo");
                                    Respawn("SpellbookAbilityListActivated", true);
                                    PlaySound("DesktopActionbarAdd", 0.7f);
                                }
                            },
                            null,
                            (h) => () =>
                            {
                                SetAnchor(Center);
                                var key = activeAbilities.ToList()[index + 7 * regionGroup.pagination].Key;
                                PrintAbilityTooltip(currentSave.player, null, key, activeAbilities[key]);
                            }
                        );
                        if (currentSave.player.actionBars.Exists(x => x.ability == key.Key.name))
                        {
                            SetBigButtonToGrayscale();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                        else if (currentSave.player.actionBars.Count < currentSave.player.ActionBarsAmount())
                            AddBigButtonOverlay("OtherGlowLearnable");
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
            SetAnchor(TopRight, 0, -19);
            var passiveAbilities = abilities.FindAll(x => !x.hide && x.cost == null && currentSave.player.abilities.ContainsKey(x.name)).ToDictionary(x => x, x => currentSave.player.abilities[x.name]);
            AddHeaderGroup(() => abilities.Count(x => !x.hide && x.cost == null && currentSave.player.abilities.ContainsKey(x.name)), 7);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(342);
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
            for (int i = 0; i < 7; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (passiveAbilities.Count > index + 7 * regionGroup.pagination)
                    {
                        var key = passiveAbilities.ToList()[index + 7 * regionGroup.pagination];
                        AddLine(key.Key.name);
                        AddLine("Rank: ", "DarkGray");
                        AddText("" + ToRoman(key.Value + 1));
                        AddBigButton(key.Key.icon,
                            null,
                            null,
                            (h) => () =>
                            {
                                SetAnchor(Center);
                                var key = passiveAbilities.ToList()[index + 7 * regionGroup.pagination].Key;
                                PrintAbilityTooltip(currentSave.player, null, key, passiveAbilities[key]);
                            }
                        );
                        if (currentSave.player.actionBars.Exists(x => x.ability == key.Key.name))
                        {
                            SetBigButtonToGrayscale();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
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
                AddBigButton(currentSave.player.Spec().icon,
                    (h) => { }
                );
                AddLine("Level: " + currentSave.player.level, "Gray");
                AddLine("Health: " + currentSave.player.health + "/" + currentSave.player.MaxHealth(), "Gray");
            });
            for (int i = 0; i < currentSave.player.ActionBarsAmount(); i++)
            {
                var index = i;
                var abilityObj = currentSave.player.actionBars.Count <= index ? null : abilities.Find(x => x.name == currentSave.player.actionBars[index].ability);
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
        new("SpellbookResources", () => {
            SetAnchor(-320, -48);
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
                    null,
                    (h) => () =>
                    {
                        PrintAbilityTooltip(Board.board.enemy, Board.board.player, abilityObj, Board.board.enemy.abilities[abilityObj.name]);
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
        new("LootInfo", () => {
            SetAnchor(-115, -90);
            AddRegionGroup();
            SetRegionGroupWidth(228);
            AddHeaderRegion(
                () =>
                {
                    AddLine(Board.board.enemy.name + "'s Loot");
                    AddSmallButton("OtherClose", (h) =>
                    {
                        PlaySound("DesktopInventoryClose");
                        CloseDesktop("CombatResultsLoot");
                    });
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
        new("MapToolbarShadow", () => {
            SetAnchor(Top);
            AddRegionGroup();
            SetRegionGroupWidth(640);
            SetRegionGroupHeight(15);
            AddPaddingRegion(() => { });
        }),
        new("MapToolbar", () => {
            AddHotkey(N, () =>
            {
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
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
                if (CDesktop.title != "BestiaryScreen")
                    SpawnDesktopBlueprint("BestiaryScreen");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopInstanceClose");
                }
            });
            SetAnchor(Top);
            DisableShadows();
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("MenuCharacterSheet", (h) =>
                {
                    SpawnDesktopBlueprint("CharacterSheet");
                });
                AddSmallButton(CDesktop.title == "EquipmentScreen" ? "OtherClose" : "MenuInventory", (h) =>
                {
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("TalentScreen");
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
                    if (CDesktop.title != "SpellbookScreen")
                        SpawnDesktopBlueprint("SpellbookScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopSpellbookScreenClose");
                    }
                });
                //AddSmallButton(CDesktop.title == "PetScreen" ? "OtherClose" : "MenuSpellbook", (h) =>
                //{
                //    var temp = CDesktop.title;
                //    if (CDesktop.title != "Map" && CDesktop.title != "CombatResults")
                //        CloseDesktop(CDesktop.title);
                //    if (temp != "SpellbookScreen")
                //        SpawnDesktopBlueprint("SpellbookScreen");
                //});
                AddSmallButton(CDesktop.title == "TalentScreen" ? "OtherClose" : "MenuClasses", (h) =>
                {
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
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
            SetAnchor(Top, -183, 0);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(272);
            AddPaddingRegion(() =>
            {
                AddLine("Day " + (currentSave.day + 1), "", "Right");
            });
        }, true),
        new("MapToolbarStatusLeft", () => {
            SetAnchor(Top, -183, 0);
            DisableGeneralSprites();
            AddRegionGroup();
            SetRegionGroupWidth(272);
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
            SetAnchor(Top, 183, 0);
            DisableGeneralSprites();
            AddRegionGroup();
            SetRegionGroupWidth(272);
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
            SetAnchor(Top, 183, 0);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(272);
            AddPaddingRegion(() =>
            {
                AddLine(currentSave.hour + (currentSave.minute < 10 ? ":0" : ":") + currentSave.minute, "", "Left");
            });
        }, true),
        new("InstanceLeftSide", () => {
            //SetAnchor(TopLeft);
            //AddRegionGroup();
            //SetRegionGroupWidth(171);
            //SetRegionGroupHeight(342);
            //AddPaddingRegion(() =>
            //{
            //    //foreach (var line in instance.description)
            //    //    AddLine(line, "DarkGray");
            //});
        }),
        new("HostileAreaRightSide", () => {
            //SetAnchor(TopRight);
            //AddRegionGroup();
            //SetRegionGroupWidth(171);
            //SetRegionGroupHeight(342);
            //AddPaddingRegion(() =>
            //{
            //    //foreach (var line in instance.description)
            //    //    AddLine(line, "DarkGray");
            //    //AddLine("Select area on the right.", "DarkGray");
            //});
        }),
        new("ComplexLeftSide", () => {
            //SetAnchor(TopLeft);
            //AddRegionGroup();
            //SetRegionGroupWidth(171);
            //SetRegionGroupHeight(342);
            //AddPaddingRegion(() =>
            //{
            //    //foreach (var line in complex.description)
            //    //    AddLine(line, "DarkGray");
            //});
        }),
        new("TownLeftSide", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(342);
            if (town.flightPaths == null || town.flightPaths.Count == 0)
            {

            }
            else
            {
                AddHeaderRegion(() => { AddLine("Flight paths:"); });
                int unknownLocations = 0;
                foreach (var flightPath in town.flightPaths)
                {
                    var desitnationName = flightPath.sites.Find(x => x != currentSave.currentSite);
                    var destination = towns.Find(x => x.name == desitnationName);
                    var faction = factions.Find(x => x.name == destination.faction);
                    if (faction.side == "Neutral" || faction.side == races.Find(x => x.name == currentSave.player.race).Faction().side)
                        if (currentSave.siteVisits.ContainsKey(destination.name))
                        {
                                AddButtonRegion(() =>
                                {
                                    AddLine(desitnationName);
                                    AddSmallButton(faction.Icon(), (h) => { });
                                },
                                (h) =>
                                {
                                    CloseDesktop("Town");
                                    SwitchDesktop("Map");
                                    CDesktop.LockScreen();
                                    if (flightPath.price > 0)
                                        PlaySound("DesktopTransportPay");
                                    destination.QueueSiteOpen("Town");
                                },
                                null,
                                (h) => () => { flightPath.PrintTooltip(); });
                        }
                        else unknownLocations++;
                }
                for (int i = 0; i < unknownLocations; i++)
                    AddPaddingRegion(() =>
                    {
                        AddLine("?", "DarkGray");
                    });
            }
            AddPaddingRegion(() => { });
        }),
        new("Inventory", () => {
            SetAnchor(TopRight, 0, -19);
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
                if (!CDesktop.windows.Exists(x => x.title == "InventorySettings") && !CDesktop.windows.Exists(x => x.title == "BankSort") && !CDesktop.windows.Exists(x => x.title == "VendorSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("InventorySort");
                        Respawn("Inventory");
                        Respawn("Bank", true);
                        Respawn("ExperienceBar", true);
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
                if (!CDesktop.windows.Exists(x => x.title == "InventorySettings") && !CDesktop.windows.Exists(x => x.title == "BankSort") && !CDesktop.windows.Exists(x => x.title == "VendorSort"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("InventorySettings");
                        Respawn("Inventory");
                        Respawn("Bank", true);
                        Respawn("ExperienceBar", true);
                    });
                else
                    AddSmallButton("OtherSettingsOff", (h) => { });
            });
            for (int i = 0; i < 7; i++)
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
            SetRegionGroupWidth(239);
            AddHeaderRegion(
                () =>
                {
                    AddLine(currentSave.player.Spec().talentTrees[currentSave.lastVisitedTalents].name + "    ", "", "Center");
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
        new("Bank", () => {
            SetAnchor(TopLeft, 0, -19);
            AddRegionGroup();
            SetRegionGroupHeight(319);
            var items = currentSave.banks[town.name].items;
            AddHeaderRegion(() =>
            {
                AddLine("Bank:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("Bank");
                    Respawn("Person");
                    Respawn("Town: " + town.name);
                    CloseWindow("Inventory");
                    PlaySound("DesktopBankClose");
                });
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
            for (int i = 0; i < 7; i++)
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
            AddPaddingRegion(() => SetRegionAsGroupExtender());
        }, true),
        new("Vendor", () => {
            currentSave.buyback ??= new();
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            var items = new List<Item>();
            AddHeaderRegion(() =>
            {
                AddLine("Vendor:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseDesktop("Vendor");
                    SwitchDesktop("Town");
                    //PlaySound("DesktopBankClose");
                });
                if (!CDesktop.windows.Exists(x => x.title == "InventorySettings") && !CDesktop.windows.Exists(x => x.title == "InventorySort") && !CDesktop.windows.Exists(x => x.title == "VendorSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("VendorSort");
                        Respawn("Vendor");
                        Respawn("Inventory");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
            });
            for (int i = 0; i < 7; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (index * 5 + j >= 999) AddBigButton("OtherNoSlot", (h) => { });
                            else if (items.Count > index * 5 + j) PrintVendorItem(items[index * 5 + j]);
                            else AddBigButton("OtherEmpty", (h) => { });
                    }
                );
            }
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Merchant", "", "Center"));
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Buyback", "", "Center"), (h) => { CloseWindow("Vendor"); CloseWindow("VendorSort"); SpawnWindowBlueprint("VendorBuyback"); });
        }, true),
        new("VendorBuyback", () => {
            SetAnchor(TopLeft);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var items = new List<Item>();
            AddHeaderRegion(() =>
            {
                AddLine("Vendor:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseDesktop("Vendor");
                    SwitchDesktop("Town");
                    //PlaySound("DesktopBankClose");
                });
            });
            for (int i = 0; i < 7; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (currentSave.buyback.Count > index * 5 + j) PrintVendorItem(currentSave.buyback[index * 5 + j]);
                            else AddBigButton("OtherEmpty", (h) => { });
                    }
                );
            }
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Merchant", "", "Center"), (h) => { CloseWindow("VendorBuyback"); SpawnWindowBlueprint("Vendor"); });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Buyback", "", "Center"));
        }, true),
        new("CharacterInfoEquipment", () => {
            currentSave.buyback ??= new();
            SetAnchor(TopLeft, 0, -19);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(290);
            //AddHeaderRegion(() =>
            //{
            //    AddLine("Character equipment:");
            //});
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
            AddPaddingRegion(() => SetRegionAsGroupExtender());

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
                            null,
                            (h) => () =>
                            {
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
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Equipment", "", "Center"));
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Stats", "", "Center"), (h) => { CloseWindow(h.window); SpawnWindowBlueprint("CharacterInfoStats"); Respawn("ExperienceBar"); });
        }, true),
        new("PersonBattlemaster", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(250);
            var personType = personTypes.Find(x => x.name == person.type);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " " + person.name);
                AddSmallButton(personType.icon, (h) => { });
            });
            AddButtonRegion(() =>
            {
                AddLine("Goodbye.");
            },
            (h) =>
            {
                person = null;
                CloseWindow(h.window);
            });
        }, true),
        new("Person", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(250);
            var type = personTypes.Find(x => x.name == person.type);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.type == "Battlemaster" ? factions.Find(x => x.name == town.faction).side : ""), (h) => { });
            });
            if (type.type == "Trainer")
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
            else if (type.type == "Banker")
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
                    Respawn("ExperienceBar");
                });
                AddButtonRegion(() =>
                {
                    AddLine("I want to buy additional vault space.");
                },
                (h) => { });
            }
            else if (type.type == "Innkeeper")
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
                (h) => { });
                AddButtonRegion(() =>
                {
                    AddLine("I want to make this inn my home.");
                },
                (h) => { currentSave.player.homeLocation = town.name; });
            }
            else if (type.type == "Battlemaster")
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
            else if (type.type == "Stable Master")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to swap my current mount.");
                },
                (h) => { });
                AddButtonRegion(() =>
                {
                    AddLine("I want to buy a new mount.");
                },
                (h) => { });
            }
            AddButtonRegion(() =>
            {
                AddLine("Goodbye.");
            },
            (h) =>
            {
                person = null;
                CloseWindow(h.window);
            });
        }, true),
        new("CharacterInfoStats", () => {
            SetAnchor(TopLeft, 0, -19);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(290);
            var stats = currentSave.player.Stats();
            //AddHeaderRegion(() =>
            //{
            //    AddLine("Character stats:");
            //});
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
                AddLine("Melee Attack Power: ", "Gray", "Right");
                AddText(currentSave.player.MeleeAttackPower() + "", "Uncommon");
                AddLine("Ranged Attack Power: ", "Gray", "Right");
                AddText(currentSave.player.RangedAttackPower() + "", "Uncommon");
                AddLine("Spell Power: ", "Gray", "Right");
                AddText(currentSave.player.SpellPower() + "", "Uncommon");
                AddLine("Critical Strike: ", "Gray", "Right");
                AddText(currentSave.player.CriticalStrike().ToString("0.00") + "%", "Uncommon");
                AddLine("Spell Critical: ", "Gray", "Right");
                AddText(currentSave.player.SpellCritical().ToString("0.00") + "%", "Uncommon");
                AddLine("Health From Stamina: ", "Gray", "Right");
                AddText(currentSave.player.MaxHealth() + "", "Uncommon");
            });
            AddPaddingRegion(() => SetRegionAsGroupExtender());
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Equipment", "", "Center"), (h) => { CloseWindow(h.window); SpawnWindowBlueprint("CharacterInfoEquipment"); Respawn("ExperienceBar"); });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Stats", "", "Center"));
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
        new("VendorSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(162);
            AddHeaderRegion(() =>
            {
                AddLine("Sort vendor inventory:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("VendorSort");
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
            SetRegionGroupWidth(162);
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
                if (Board.board.results.experience > 0)
                    AddLine("You earned " + Board.board.results.experience + " experience", "", "Center");
                else AddLine("You earned no experience", "", "Center");
                SetRegionAsGroupExtender();
            });
            AddButtonRegion(() =>
            {
                if (Board.board.results.result == "Won")
                {
                    if (Board.board.results.items.Count > 0)
                        AddLine("Show Loot", "", "Center");
                    else AddLine("OK", "", "Center");
                }
                else if (Realm.realms.Find(x => x.name == settings.selectedRealm).hardcore)
                {
                    SetRegionBackground(RegionBackgroundType.RedButton);
                    AddLine("Game Over", "", "Center");
                }
                else
                    AddLine("Release Spirit", "", "Center");
            },
            (h) =>
            {
                CloseDesktop("CombatResults");
                if (Board.board.results.items.Count > 0)
                {
                    PlaySound("DesktopInventoryOpen");
                    SpawnDesktopBlueprint("CombatResultsLoot");
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
            SetAnchor(-115, -109);
            AddRegionGroup();
            SetRegionGroupWidth(228);
            AddPaddingRegion(
                () =>
                {
                    for (int j = 0; j < 6; j++)
                        if (j < Board.board.results.items.Count)
                            PrintLootItem(Board.board.results.items[j]);
                        else AddBigButton("OtherEmpty", (h) => { });
                }
            );
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
        new("ExperienceBar", () => {
            SetAnchor(Bottom);
            var experience = (int)(319 * (currentSave.player.experience / (double)currentSave.player.ExperienceNeeded()));
            AddRegionGroup();
            SetRegionGroupWidth(experience * 2);
            SetRegionGroupHeight(12);
            AddPaddingRegion(() => { SetRegionBackground(RegionBackgroundType.Experience); });
            AddRegionGroup();
            SetRegionGroupWidth((319 - experience) * 2);
            SetRegionGroupHeight(12);
            AddPaddingRegion(() => { SetRegionBackground(RegionBackgroundType.NoExperience); });
        },  true),
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
                Serialize(buffs, "buffs", false, false, prefix);
                Serialize(areas, "areas", false, false, prefix);
                Serialize(instances, "instances", false, false, prefix);
                Serialize(complexes, "complexes", false, false, prefix);
                Serialize(towns, "towns", false, false, prefix);
                Serialize(items, "items", false, false, prefix);
                Serialize(itemSets, "sets", false, false, prefix);
                Serialize(factions, "factions", false, false, prefix);
                Serialize(personTypes, "persontypes", false, false, prefix);
                Serialize(spiritHealers, "spirithealers", false, false, prefix);
                Serialize(pEnchants, "permanentenchants", false, false, prefix);
                Serialize(zones, "zones", false, false, prefix);
                Serialize(paths, "paths", false, false, prefix);
                Serialize(defines, "defines", false, false, prefix);
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
                        SpawnWindowBlueprint("ObjectManagerSpecs");
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
                    price = 0.0001
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
            SetRegionGroupWidth(162);
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
                abilities = abilities.OrderBy(x => currentSave.player.actionBars.Exists(y => y.ability == x.name)).ToList();
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
                    Board.NewBoard(ability);
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
            AddHotkey(C, () =>
            {
                if (!CloseWindow("CharacterInfoEquipment") && !CloseWindow("CharacterInfoStats"))
                {
                    SpawnWindowBlueprint("CharacterInfoEquipment");
                    PlaySound("DesktopCharacterSheetOpen");
                    Respawn("ExperienceBar");
                }
                else PlaySound("DesktopCharacterSheetClose");
            });
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
            AddHotkey(L, () => { SpawnWindowBlueprint("ItemDrop"); });

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
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBar");
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
            SetDesktopBackground(Board.board.area.Background());
            SpawnWindowBlueprint("CombatResults");
            SpawnWindowBlueprint("ExperienceBar");
        }),
        new("CombatLog", () =>
        {
            SetDesktopBackground(Board.board.area.Background());
            SpawnWindowBlueprint("CombatResultsChart");
            SpawnWindowBlueprint("CombatResultsChartLeftArrow");
            SpawnWindowBlueprint("CombatResultsChartRightArrow");
            FillChart();
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
            SetDesktopBackground(Board.board.area.Background());
            locationName = Board.board.enemy.name + "'s Loot";
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
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInventoryClose");
                CloseDesktop("CombatResultsLoot");
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
            SpawnWindowBlueprint("ExperienceBar");
            SpawnWindowBlueprint("Town: " + town.name);
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInstanceClose");
                CloseDesktop("Town");
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
            SpawnWindowBlueprint("ExperienceBar");
            SpawnWindowBlueprint("InstanceLeftSide");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("HostileArea: " + area?.name))
                {
                    area = null;
                    CloseWindow("BossQueue");
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground(instance.Background());
                    Respawn("InstanceLeftSide");
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
            SpawnWindowBlueprint("ExperienceBar");
            SpawnWindowBlueprint("ComplexLeftSide");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("HostileArea: " + area?.name))
                {
                    area = null;
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground(complex.Background());
                    Respawn("ComplexLeftSide");
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
            locationName = Board.board.area.name;
            PlaySound("DesktopEnterCombat");
            SetDesktopBackground(Board.board.area.Background());
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
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopMenuOpen");
                SpawnDesktopBlueprint("GameMenu");
            });
            AddHotkey(BackQuote, () => { SpawnDesktopBlueprint("DevPanel"); });
            AddHotkey(KeypadMultiply, () => { Board.board.EndCombat("Won"); });
        }),
        new("GameSimulation", () =>
        {
            locationName = Board.board.area.name;
            PlaySound("DesktopEnterCombat");
            SetDesktopBackground(Board.board.area.Background());
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
            SpawnWindowBlueprint("CharacterStats");
            SpawnWindowBlueprint("ExperienceBar");
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
            SetDesktopBackground("Stone");
            SpawnWindowBlueprint("MapToolbarShadow");
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
                        if (windowBlueprints.Exists(x => x.title == "Talent" + tree + row + col))
                            if (playerSpec.talentTrees[currentSave.lastVisitedTalents].talents.Exists(x => x.row == row && x.col == col && x.tree == tree))
                                SpawnWindowBlueprint("Talent" + tree + row + col);
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
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInstanceClose");
                CloseDesktop("BestiaryScreen");
            });
        }),
        new("Bank", () =>
        {
            SetDesktopBackground(town.Background() + "Bank");
            SpawnWindowBlueprint("Bank");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopBankClose");
                CloseDesktop("Bank");
                SwitchDesktop("Town");
            });
        }),
        new("Vendor", () =>
        {
            //PlaySound("DesktopBankOpen", 0.2f);
            SetDesktopBackground(town.Background());
            SpawnWindowBlueprint("Vendor");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                //PlaySound("DesktopBankClose");
                CloseDesktop("Vendor");
                SwitchDesktop("Town");
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
            Serialize(factions, "factions", true, false, prefix);
            Serialize(spiritHealers, "spirithealers", true, false, prefix);
            Serialize(personTypes, "personTypes", true, false, prefix);

            #endif

            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerLobby");
            AddHotkey(Escape, () => { CloseDesktop("DevPanel"); });
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
