using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static SiteComplex;
using static SiteInstance;

using static Root.Color;
using static Root.Anchor;

using static UnityEngine.KeyCode;

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
                AddLine("Menu", Gray);
            });
            AddRegionGroup();
            AddButtonRegion(() =>
            {
                AddLine("Singleplayer", Black);
            },
            (h) =>
            {
                SpawnWindowBlueprint("TitleScreenSingleplayer");
                CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Settings", Black);
            },
            (h) =>
            {
                SpawnWindowBlueprint("TitleScreenSettings");
                CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Achievments", Black);
            },
            (h) =>
            {
                //SpawnWindowBlueprint("TitleScreenGraveyard");
                //CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Graveyard", Black);
            },
            (h) =>
            {
                //SpawnWindowBlueprint("TitleScreenGraveyard");
                //CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Credits", Black);
            },
            (h) =>
            {
                //SpawnWindowBlueprint("TitleScreenGraveyard");
                //CloseWindow(h.window);
            },
            (h) => () =>
            {
                SetAnchor(BottomRight);
                AddRegionGroup();
                AddHeaderRegion(
                    () =>
                    {
                        AddLine("Exits the game.", LightGray);
                        AddLine("This action does not", LightGray);
                        AddLine("save your game progress!", LightGray);
                    }
                );
            });
            AddButtonRegion(() =>
            {
                AddLine("Exit", Black);
            },
            (h) =>
            {
                Application.Quit();
            },
            (h) => () =>
            {
                SetAnchor(BottomRight);
                AddRegionGroup();
                AddHeaderRegion(
                    () =>
                    {
                        AddLine("Exits the game.", LightGray);
                        AddLine("This action does not", LightGray);
                        AddLine("save your game progress!", LightGray);
                    }
                );
            });
        }),
        new("TitleScreenSingleplayer", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Singleplayer", Gray);
            });
            AddRegionGroup();
            AddButtonRegion(() =>
            {
                AddLine("Continue last game", Black);
            },
            (h) =>
            {
                saveGames.Add(new SaveGame());
                currentSave = saveGames[0];
                SpawnDesktopBlueprint("Map");
                SwitchDesktop("Map");
            });
            AddButtonRegion(() =>
            {
                AddLine("Create a new character", Black);
            },
            (h) =>
            {

            });
            AddButtonRegion(() =>
            {
                AddLine("Load saved game", Black);
            },
            (h) =>
            {

            });
            AddButtonRegion(() =>
            {
                AddLine("Back", Black);
            },
            (h) =>
            {
                SpawnWindowBlueprint("TitleScreenMenu");
                CloseWindow(h.window);
            },
            (h) => () =>
            {
                SetAnchor(BottomRight);
                AddRegionGroup();
                AddHeaderRegion(
                    () =>
                    {
                        AddLine("Returns you to the main menu", LightGray);
                    }
                );
            });
        }),
        new("TitleScreenSettings", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Settings", Gray);
            });
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddCheckbox(settings.shadows);
                AddLine("Shadows", Gray);
            });
            AddButtonRegion(() =>
                {
                    AddLine("Back", Black);
                },
                (h) =>
                {
                    SpawnWindowBlueprint("TitleScreenMenu");
                    CloseWindow(h.window);
                }
            );
        }),
        new("PlayerBattleInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            AddButtonRegion(
                () =>
                {
                    AddLine(Board.board.player.name, Black);
                    AddSmallButton("MenuFlee", (h) =>
                    {
                        CloseDesktop("Game");
                        if (Board.board.area != null && Board.board.area.instancePart)
                        {
                            SwitchDesktop("DungeonEntrance");
                            CDesktop.Rebuild();
                        }
                        else SwitchDesktop("Map");
                    });
                    AddSmallButton("MenuLog", (h) => { });
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton("Class" + Board.board.player.spec,
                    (h) => { }
                );
                AddLine("Level: " + Board.board.player.level, Gray);
                AddLine("Health: " + Board.board.player.health + "/" + Board.board.player.MaxHealth(), Gray);
            });
            foreach (var actionBar in Board.board.player.actionBars)
            {
                var abilityObj = Ability.abilities.Find(x => x.name == actionBar.ability);
                if (abilityObj == null || abilityObj.cost == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(actionBar.ability, Black);
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
                            abilityObj.effects(true);
                            Board.board.player.DetractResources(abilityObj.cost);
                            Board.board.temporaryElementsPlayer = new();
                            h.window.desktop.Rebuild();
                        }
                    },
                    (h) => () =>
                    {
                        SetAnchor(Top, 0, -23);
                        AddHeaderGroup();
                        SetRegionGroupWidth(236);
                        SetRegionGroupHeight(217);
                        AddHeaderRegion(() =>
                        {
                            AddLine(actionBar.ability, Gray);
                        });
                        AddPaddingRegion(() =>
                        {
                            AddBigButton(abilityObj.icon, (h) => { });
                            AddLine("Cooldown: ", DarkGray);
                            AddText(abilityObj.cooldown == 0 ? "None" : abilityObj.cooldown + (abilityObj.cooldown == 1 ? " turn"  : " turns"), Gray);
                            if (actionBar.cooldown > 0)
                            {
                                AddLine("Cooldown left: ", DarkGray);
                                AddText(actionBar.cooldown + (actionBar.cooldown == 1 ? " turn"  : " turns"), Gray);
                            }
                        });
                        abilityObj.description(true);
                        foreach (var cost in abilityObj.cost)
                        {
                            AddRegionGroup();
                            AddHeaderRegion(() =>
                            {
                                AddSmallButton("Element" + cost.Key + "Rousing", (h) => { });
                            });
                            AddRegionGroup();
                            SetRegionGroupWidth(20);
                            AddHeaderRegion(() =>
                            {
                                AddLine(cost.Value + "", cost.Value > Board.board.player.resources[cost.Key] ? Red : Green);
                            });
                        }
                        AddRegionGroup();
                        SetRegionGroupWidth(236 - abilityObj.cost.Count * 49);
                        AddPaddingRegion(() =>
                        {
                            AddLine("", LightGray);
                        });
                    }
                );
            }
        }),
        new("SpellbookAbilityList", () => {
            SetAnchor(147, 0);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            AddHeaderRegion(
                () =>
                {
                    AddLine("Active abilities:", Gray);
                    AddSmallButton("OtherClose", (h) =>
                    {
                        CloseDesktop("SpellbookScreen");
                        SwitchDesktop("Map");
                        PlaySound("DesktopSpellbookScreenClose");
                    });
                }
            );
            var activeAbilities = Ability.abilities.FindAll(x => x.cost != null && currentSave.player.abilities.Contains(x.name)).ToList();
            var passiveAbilities = Ability.abilities.FindAll(x => x.cost == null && currentSave.player.abilities.Contains(x.name)).ToList();
            for (int i = 0; i < activeAbilities.Count; i++)
            {
                var abilityObj = activeAbilities[i];
                AddButtonRegion(
                    () =>
                    {
                        AddLine(abilityObj.name, Black);
                        AddSmallButton(abilityObj.icon,
                        (h) =>
                        {

                        });
                        if (currentSave.player.actionBars.Exists(x => x.ability == abilityObj.name))
                        {
                        SetSmallButtonToGrayscale();
                        AddSmallButtonOverlay("OtherGridBlurred");
                        }
                    },
                    (h) =>
                    {
                        if (!currentSave.player.actionBars.Exists(x => x.ability == abilityObj.name) && currentSave.player.actionBars.Count < currentSave.player.actionBarsUnlocked)
                        {
                            currentSave.player.actionBars.Add(new ActionBar(abilityObj.name));
                            CloseWindow("PlayerSpellbookInfo");
                            CloseWindow(h.window);
                            SpawnWindowBlueprint("PlayerSpellbookInfo");
                            SpawnWindowBlueprint("SpellbookAbilityList");
                        }
                    },
                    (h) => () =>
                    {
                        SetAnchor(Top, 0, -23);
                        AddHeaderGroup();
                        SetRegionGroupWidth(236);
                        SetRegionGroupHeight(217);
                        AddHeaderRegion(() =>
                        {
                            AddLine(abilityObj.name, Gray);
                        });
                        AddPaddingRegion(() =>
                        {
                            AddBigButton(abilityObj.icon, (h) => { });
                            AddLine("Cooldown: ", DarkGray);
                            AddText(abilityObj.cooldown == 0 ? "None" : abilityObj.cooldown + (abilityObj.cooldown == 1 ? " turn"  : " turns"), Gray);
                        });
                        abilityObj.description(true);
                        foreach (var cost in abilityObj.cost)
                        {
                            AddRegionGroup();
                            AddHeaderRegion(() =>
                            {
                                AddSmallButton("Element" + cost.Key + "Rousing", (h) => { });
                            });
                            AddRegionGroup();
                            SetRegionGroupWidth(20);
                            AddHeaderRegion(() =>
                            {
                                AddLine(cost.Value + "", cost.Value > currentSave.player.MaxResource(cost.Key) ? Red : Gray);
                            });
                        }
                        AddRegionGroup();
                        SetRegionGroupWidth(236 - abilityObj.cost.Count * 49);
                        AddPaddingRegion(() =>
                        {
                            AddLine("", LightGray);
                        });
                    }
                );
            }
            if (passiveAbilities.Count(x => x.description != null) > 0)
                AddHeaderRegion(
                    () =>
                    {
                        AddLine("Passive abilities:", Gray);
                    }
                );
            for (int i = 0; i < passiveAbilities.Count; i++)
            {
                var abilityObj = passiveAbilities[i];
                if (abilityObj.description == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(abilityObj.name, Black);
                        AddSmallButton(abilityObj.icon,
                        (h) =>
                        {

                        });
                    },
                    (h) => { },
                    (h) => () =>
                    {
                        SetAnchor(Top, 0, -23);
                        AddHeaderGroup();
                        SetRegionGroupWidth(236);
                        SetRegionGroupHeight(217);
                        AddHeaderRegion(() =>
                        {
                            AddLine(abilityObj.name, Gray);
                        });
                        AddPaddingRegion(() =>
                        {
                            AddBigButton(abilityObj.icon, (h) => { });
                            AddLine("Cooldown: ", DarkGray);
                            AddText(abilityObj.cooldown == 0 ? "None" : abilityObj.cooldown + (abilityObj.cooldown == 1 ? " turn"  : " turns"), Gray);
                        });
                        abilityObj.description(true);
                        if (abilityObj.cost != null)
                            foreach (var cost in abilityObj.cost)
                            {
                                AddRegionGroup();
                                AddHeaderRegion(() =>
                                {
                                    AddSmallButton("Element" + cost.Key + "Rousing", (h) => { });
                                });
                                AddRegionGroup();
                                SetRegionGroupWidth(20);
                                AddHeaderRegion(() =>
                                {
                                    AddLine(cost.Value + "", Gray);
                                });
                            }
                        AddRegionGroup();
                        SetRegionGroupWidth(236 - (abilityObj.cost == null ? 0 : abilityObj.cost.Count) * 49);
                        AddPaddingRegion(() =>
                        {
                            AddLine("", LightGray);
                        });
                    }
                );
            }
        }),
        new("PlayerSpellbookInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            AddButtonRegion(
                () =>
                {
                    AddLine(currentSave.player.name, Black);
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton("Class" + currentSave.player.spec,
                    (h) => { }
                );
                AddLine("Level: " + currentSave.player.level, Gray);
                AddLine("Health: " + currentSave.player.health + "/" + currentSave.player.MaxHealth(), Gray);
            });
            for (int i = 0; i < currentSave.player.actionBarsUnlocked; i++)
            {
                var index = i;
                var abilityObj = currentSave.player.actionBars.Count <= index ? null : Ability.abilities.Find(x => x.name == currentSave.player.actionBars[index].ability);
                if (abilityObj != null)
                    AddButtonRegion(
                        () =>
                        {
                            AddLine(abilityObj.name, Black);
                            AddSmallButton(abilityObj.icon, (h) => { });
                        },
                        (h) =>
                        {
                            currentSave.player.actionBars.RemoveAt(index);
                            CloseWindow("SpellbookAbilityList");
                            CloseWindow("PlayerSpellbookInfo");
                            SpawnWindowBlueprint("SpellbookAbilityList");
                            SpawnWindowBlueprint("PlayerSpellbookInfo");
                        },
                        (h) => () =>
                        {
                            SetAnchor(Top, 0, -23);
                            AddHeaderGroup();
                            SetRegionGroupWidth(236);
                            SetRegionGroupHeight(217);
                            AddHeaderRegion(() =>
                            {
                                AddLine(abilityObj.name, Gray);
                            });
                            AddPaddingRegion(() =>
                            {
                                AddBigButton(abilityObj.icon, (h) => { });
                                AddLine("Cooldown: ", DarkGray);
                                AddText(abilityObj.cooldown == 0 ? "None" : abilityObj.cooldown + (abilityObj.cooldown == 1 ? " turn"  : " turns"), Gray);
                            });
                            abilityObj.description(true);
                            foreach (var cost in abilityObj.cost)
                            {
                                AddRegionGroup();
                                AddHeaderRegion(() =>
                                {
                                    AddSmallButton("Element" + cost.Key + "Rousing", (h) => { });
                                });
                                AddRegionGroup();
                                SetRegionGroupWidth(20);
                                AddHeaderRegion(() =>
                                {
                                    AddLine(cost.Value + "", cost.Value > currentSave.player.MaxResource(cost.Key) ? Red : Gray);
                                });
                            }
                            AddRegionGroup();
                            SetRegionGroupWidth(236 - abilityObj.cost.Count * 49);
                            AddPaddingRegion(() =>
                            {
                                AddLine("", LightGray);
                            });
                        }
                    );
                else
                    AddHeaderRegion(
                        () =>
                        {
                            AddLine("", Black);
                            AddSmallButton("OtherEmpty", (h) => { });
                        }
                    );
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
            });
            //AddPaddingRegion(() =>
            //{
            //    AddLine("Show activated abilities", Gray);
            //    AddCheckbox(showActive);
            //});
            //AddPaddingRegion(() =>
            //{
            //    AddLine("Show passive abilities", Gray);
            //    AddCheckbox(showPassive);
            //});
        }),
        new("SpellbookResources", () => {
            SetAnchor(BottomLeft);
            AddHeaderGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Starting resources:");
            });
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
                            SetRegionGroupWidth(83);
                            AddHeaderRegion(() =>
                            {
                                AddLine(element + ":", Gray);
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(currentSave.player.resources.ToList().Find(x => x.Key == element).Value + " / " + currentSave.player.MaxResource(element), Gray);
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(76);
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    var value = currentSave.player.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? DarkGray : (value > currentSave.player.MaxResource(element) ? Red : Green));
                    AddText(" / " + currentSave.player.MaxResource(element), DarkGray);
                    AddSmallButton("Element" + elements2[elements1.IndexOf(element)] + "Rousing",
                        (h) => { },
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(83);
                            AddHeaderRegion(() =>
                            {
                                AddLine(elements2[elements1.IndexOf(element)] + ":", Gray);
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(currentSave.player.resources.ToList().Find(x => x.Key == elements2[elements1.IndexOf(element)]).Value + " / " + currentSave.player.MaxResource(elements2[elements1.IndexOf(element)]), Gray);
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(56);
            foreach (var element in elements2)
                AddHeaderRegion(() =>
                {
                    var value = currentSave.player.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? DarkGray : (value > currentSave.player.MaxResource(element) ? Red : Green));
                    AddText(" / " + currentSave.player.MaxResource(element), DarkGray);
                });
        }, true),
        new("EnemyBattleInfo", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            AddButtonRegion(
                () =>
                {
                    AddLine(Board.board.enemy.name, Black);
                    AddSmallButton("MenuMenu", (h) => { });
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton("Portrait" + Race.races.Find(x => x.name == Board.board.enemy.race).portrait, (h) => { });
                AddLine("Level: ", Gray);
                AddText(Board.board.enemy.level - 10 > Board.board.player.level ? "??" : "" + Board.board.enemy.level, EntityColoredLevel(Board.board.enemy.level));
                AddLine("Health: " + Board.board.enemy.health + "/" + Board.board.enemy.MaxHealth(), Gray);
            });
            foreach (var actionBar in Board.board.enemy.actionBars)
            {
                var abilityObj = Ability.abilities.Find(x => x.name == actionBar.ability);
                if (abilityObj == null || abilityObj.cost == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(actionBar.ability, Black);
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
                        SetAnchor(Top, 0, -23);
                        AddHeaderGroup();
                        SetRegionGroupWidth(236);
                        SetRegionGroupHeight(217);
                        AddHeaderRegion(() =>
                        {
                            AddLine(actionBar.ability, Gray);
                        });
                        AddPaddingRegion(() =>
                        {
                            AddBigButton(abilityObj.icon, (h) => { });
                            AddLine("Cooldown: ", DarkGray);
                            AddText(abilityObj.cooldown == 0 ? "None" : abilityObj.cooldown + (abilityObj.cooldown == 1 ? " turn"  : " turns"), Gray);
                        });
                        abilityObj.description(false);
                        foreach (var cost in abilityObj.cost)
                        {
                            AddRegionGroup();
                            AddHeaderRegion(() =>
                            {
                                AddSmallButton("Element" + cost.Key + "Rousing", (h) => { });
                            });
                            AddRegionGroup();
                            SetRegionGroupWidth(20);
                            AddHeaderRegion(() =>
                            {
                                AddLine(cost.Value + "", LightGray);
                            });
                        }
                        AddRegionGroup();
                        SetRegionGroupWidth(236 - abilityObj.cost.Count * 49);
                        AddPaddingRegion(() =>
                        {
                            AddLine("", LightGray);
                        });
                    }
                , -1, "EnemyActionBar" + Board.board.enemy.actionBars.IndexOf(actionBar));
            }
        }),
        new("LocationInfo", () => {
            SetAnchor(Top);
            AddRegionGroup();
            AddHeaderRegion(
                () =>
                {
                    AddLine("Blackwing Lair", Gray);
                }
            );
        }),
        new("BattleBoard", () => {
            SetAnchor(Top, 0, -15 + 19 * (Board.board.field.GetLength(1) - 7));
            var boardBackground = new GameObject("BoardBackground", typeof(SpriteRenderer));
            boardBackground.transform.parent = CDesktop.LBWindow.transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/BoardBackground" + Board.board.field.GetLength(0) + "x" + Board.board.field.GetLength(1));
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
                        //    );
                        //});
                    }
                });
            }
        }),
        new("MapToolbar", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            AddButtonRegion(
                () =>
                {
                    AddLine(currentSave.player.name, Black);
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton("Class" + currentSave.player.spec,
                    (h) => { }
                );
                AddLine("Level: " + currentSave.player.level, Gray);
                AddLine("Health: " + currentSave.player.health + "/" + currentSave.player.MaxHealth(), Gray);
            });
            AddButtonRegion(
                () =>
                {
                    AddLine("Character Sheet", Black);
                    AddSmallButton("MenuCharacterSheet", (h) => { });
                },
                (h) =>
                {
                    SpawnDesktopBlueprint("CharacterSheet");
                    SwitchDesktop("CharacterSheet");
                    PlaySound("DesktopCharacterSheetOpen");
                }
            );
            if (CDesktop.windows.Exists(x => x.title == "Inventory"))
                AddHeaderRegion(
                    () =>
                    {
                        AddLine("Inventory", DarkGray);
                        AddSmallButton("MenuInventory", (h) => { });
                        AddSmallButtonOverlay("OtherGrid");
                    }
                );
            else
                AddButtonRegion(
                    () =>
                    {
                        AddLine("Inventory", Black);
                        AddSmallButton("MenuInventory", (h) => { });
                    },
                    (h) =>
                    {
                        SpawnWindowBlueprint("Inventory");
                        CloseWindow("MapToolbar");
                        SpawnWindowBlueprint("MapToolbar");
                        PlaySound("DesktopInventoryOpen");
                    }
                );
            AddButtonRegion(
                () =>
                {
                    AddLine("Spellbook", Black);
                    AddSmallButton("MenuSpellbook", (h) => { });
                },
                (h) =>
                {
                    SpawnDesktopBlueprint("SpellbookScreen");
                    SwitchDesktop("SpellbookScreen");
                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("Talents", Black);
                    AddSmallButton("MenuTalents", (h) => { });
                },
                (h) =>
                {
                    SpawnDesktopBlueprint("TalentScreen");
                    SwitchDesktop("TalentScreen");
                }
            );
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
            //AddRegionGroup();
            //SetRegionGroupWidth(123);
            //AddHeaderRegion(() =>
            //{
            //    foreach (var foo in currentSave.player.Stats())
            //        if (!foo.Key.Contains("Mastery"))
            //            AddLine(foo.Key + ":", Gray);
            //});
            //AddRegionGroup();
            //SetRegionGroupWidth(40);
            //AddHeaderRegion(() =>
            //{
            //    foreach (var foo in currentSave.player.Stats())
            //        if (!foo.Key.Contains("Mastery"))
            //            AddLine(foo.Value + "", foo.Value > currentSave.player.stats.stats[foo.Key] ? Uncommon : (foo.Value < currentSave.player.stats.stats[foo.Key] ? DangerousRed : Gray));
            //});
        }, true),
        new("InstanceLeftSide", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            AddPaddingRegion(() =>
            {
                foreach (var line in instance.description)
                    AddLine(line, DarkGray);
                //AddLine("Select area on the right.", DarkGray);
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
                AddLine("", Gray);
                AddLine("", Gray);
            });
        }),
        new("ComplexLeftSide", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            AddPaddingRegion(() =>
            {
                foreach (var line in complex.description)
                    AddLine(line, DarkGray);
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
                AddLine("", Gray);
                AddLine("", Gray);
            });
        }),
        new("Inventory", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            //SetRegionGroupHeight(358);
            var items = currentSave.player.inventory.items;
            AddHeaderRegion(() =>
            {
                AddLine("Inventory:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("Inventory");
                    CloseWindow("MapToolbar");
                    SpawnWindowBlueprint("MapToolbar");
                    PlaySound("DesktopInventoryClose");
                });
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
            AddHeaderRegion(() =>
            {
                AddLine("Money: ");
                AddLine("Honor: ");
                AddLine("");
            });
        }, true),
        new("ItemDrop", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            SetRegionGroupWidth(256);
            AddHeaderRegion(() =>
            {
                AddLine("Loot from ", Gray);
                AddText("Chief Ukorz Sandscalp", Gray);
            });
            var item = Item.items[random.Next(Item.items.Count)];
            AddHeaderRegion(() =>
            {
                AddBigButton(item.icon, (h) => { });
                var split = item.name.Split(", ");
                AddLine(split[0], Item.rarityColors[item.rarity]);
                if (split.Length > 1)
                    AddLine("\"" + split[1] + "\"", Item.rarityColors[item.rarity]);
            });
            AddPaddingRegion(() =>
            {
                if (item.armorClass != null)
                {
                    AddLine(item.armorClass + " " + item.type, Gray);
                    AddLine(item.armor + " Armor", Gray);
                }
                else if (item.maxDamage != 0)
                {
                    AddLine(item.type + " " + item.detailedType, Gray);
                    AddLine(item.minDamage + " - " + item.maxDamage + " Damage", Gray);
                }
                else
                    AddLine(item.type, Gray);
            });
            if (item.stats.stats.Count > 0)
                AddPaddingRegion(() =>
                {
                    foreach (var stat in item.stats.stats)
                        AddLine("+" + stat.Value + " " + stat.Key, Gray);
                });
            AddHeaderRegion(() =>
            {
                AddLine("Required level: ", DarkGray);
                AddText("" + item.lvl, ItemColoredLevel(item.lvl));
            });
            AddPaddingRegion(
                () =>
                {
                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("Accept the item", Black);
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
            SetRegionGroupWidth(100);
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
                    AddLine((int)item.price + "", Gold);
                    AddSmallButton("ItemCoinsGold", (h) => { });
                }
            );
            AddRegionGroup();
            AddPaddingRegion(
                () =>
                {
                    AddLine((int)(item.price * 100 % 100) + ""  + "", Silver);
                    AddSmallButton("ItemCoinsSilver", (h) => { });
                }
            );
            AddRegionGroup();
            AddPaddingRegion(
                () =>
                {
                    AddLine((int)(item.price * 10000 % 100) + "", Copper);
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
                        AddLine("Envenom", Gray);
                    });
                    AddHeaderRegion(() =>
                    {
                        AddLine("Cast cost:", Gray);
                    });
                    AddPaddingRegion(() =>
                    {
                        AddLine("x2 Decay", Gray);
                        AddLine("x3 Shadow", Gray);
                        AddLine("x1 Air", Gray);
                    });
                    AddHeaderRegion(() =>
                    {
                        AddLine("Effects:", Gray);
                    });
                    AddPaddingRegion(() =>
                    {
                        AddLine("Strike the target for 24* damage.", Gray);
                        AddLine("Additionaly poison the target for 4* damage", Gray);
                        AddLine("every time they make move for next 3 turns.", Gray);
                    });
                    AddPaddingRegion(() =>
                    {
                        AddLine("* Scaled with Agility and Decay Mastery.", Gray);
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
            PrintItem(currentSave.player.GetSlot("Neck"));
        }),
        new("CharacterBackSlot", () => {
            SetAnchor(-98, 22);
            PrintItem(currentSave.player.GetSlot("Back"));
        }),
        new("CharacterRingSlot", () => {
            SetAnchor(-98, -30);
            PrintItem(currentSave.player.GetSlot("Finger"));
        }),
        new("CharacterHeadSlot", () => {
            SetAnchor(-46, 100);
            PrintItem(currentSave.player.GetSlot("Head"));
        }),
        new("CharacterChestSlot", () => {
            SetAnchor(-46, 48);
            PrintItem(currentSave.player.GetSlot("Chest"));
        }),
        new("CharacterLegsSlot", () => {
            SetAnchor(-46, -4);
            PrintItem(currentSave.player.GetSlot("Legs"));
        }),
        new("CharacterFeetSlot", () => {
            SetAnchor(-46, -56);
            PrintItem(currentSave.player.GetSlot("Feet"));
        }),
        new("CharacterShouldersSlot", () => {
            SetAnchor(6, 100);
            PrintItem(currentSave.player.GetSlot("Shoulders"));
        }),
        new("CharacterHandsSlot", () => {
            SetAnchor(6, 48);
            PrintItem(currentSave.player.GetSlot("Hands"));
        }),
        new("CharacterWaistSlot", () => {
            SetAnchor(6, -4);
            PrintItem(currentSave.player.GetSlot("Waist"));
        }),
        new("CharacterSpecialSlot", () => {
            SetAnchor(6, -56);
            PrintItem(currentSave.player.GetSlot("Special"));
        }),
        new("CharacterMainHandSlot", () => {
            SetAnchor(58, 74);
            PrintItem(currentSave.player.GetSlot("Main Hand"));
        }),
        new("CharacterOffHandSlot", () => {
            SetAnchor(58, 22);
            PrintItem(currentSave.player.GetSlot("Off Hand"));
        }),
        new("CharacterTrinketSlot", () => {
            SetAnchor(58, -30);
            PrintItem(currentSave.player.GetSlot("Trinket"));
        }),
        new("CharacterStats", () => {
            SetAnchor(BottomLeft);
            AddRegionGroup();
            var stats = currentSave.player.Stats();
            AddHeaderRegion(() =>
            {
                foreach (var foo in stats)
                    if (!foo.Key.Contains("Mastery"))
                        AddLine(foo.Key + ":", Gray);
            });
            AddHeaderRegion(() =>
            {
                foreach (var foo in stats)
                    if (foo.Key.Contains("Mastery"))
                        AddLine(foo.Key + ":", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Melee Attack Power:", Gray);
                AddLine("Ranged Attack Power:", Gray);
                AddLine("Spell Power:", Gray);
                AddLine("Critical Strike:", Gray);
                AddLine("Spell Critical:", Gray);
            });
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                foreach (var foo in stats)
                    if (!foo.Key.Contains("Mastery"))
                        AddLine(foo.Value + "", foo.Value > currentSave.player.stats.stats[foo.Key] ? Uncommon : (foo.Value < currentSave.player.stats.stats[foo.Key] ? DangerousRed : Gray));
            });
            AddHeaderRegion(() =>
            {
                foreach (var foo in stats)
                    if (foo.Key.Contains("Mastery"))
                        AddLine(foo.Value + "", foo.Value > currentSave.player.stats.stats[foo.Key] ? Uncommon : (foo.Value < currentSave.player.stats.stats[foo.Key] ? DangerousRed : Gray));
            });
            AddHeaderRegion(() =>
            {
                AddLine(currentSave.player.MeleeAttackPower() + "", Gray);
                AddLine(currentSave.player.RangedAttackPower() + "", Gray);
                AddLine(currentSave.player.SpellPower() + "", Gray);
                AddLine(currentSave.player.CriticalStrike().ToString("0.00") + "%", Gray);
                AddLine(currentSave.player.SpellCritical().ToString("0.00") + "%", Gray);
            });
        }),
        new("CharacterCreation", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(218);
            AddHeaderRegion(() =>
            {
                AddLine("Faction: " + creationFaction);
                AddSmallButton("ActionReroll", (h) =>
                {
                    creationFaction = random.Next(2) == 1 ? "Horde" : "Alliance";
                    creationRace = null;
                    creationClass = null;
                });
            });
            AddHeaderRegion(() =>
            {
                AddBigButton("HonorAlliance", (h) => { creationFaction = "Alliance"; creationRace = null; creationClass = null; });
                if (creationFaction != "Alliance") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                AddBigButton("HonorHorde", (h) => { creationFaction = "Horde"; creationRace = null; creationClass = null; });
                if (creationFaction != "Horde") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
            });
            AddHeaderRegion(() =>
            {
                AddLine("Gender: " + creationGender);
                AddSmallButton("ActionReroll", (h) =>
                {
                    creationGender = random.Next(2) == 1 ? "Female" : "Male";
                });
            });
            AddHeaderRegion(() =>
            {
                if (creationFaction == null) return;
                AddBigButton("OtherGenderMale", (h) => { creationGender = "Male"; });
                if (creationGender != "Male") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                AddBigButton("OtherGenderFemale", (h) => { creationGender = "Female"; });
                if (creationGender != "Female") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
            });
            AddHeaderRegion(() =>
            {
                var races = Race.races.FindAll(x => x.faction == creationFaction);
                AddLine("Race: " + creationRace);
                AddSmallButton("ActionReroll", (h) =>
                {
                    creationRace = races[random.Next(races.Count)].name;
                    creationClass = null;
                });
            });
            AddHeaderRegion(() =>
            {
                if (creationGender == null) return;
                if (creationFaction == "Alliance")
                {
                    AddBigButton("PortraitDwarf" + creationGender, (h) => { creationRace = "Dwarf"; creationClass = null; });
                    if (creationRace != "Dwarf") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    AddBigButton("PortraitGnome" + creationGender, (h) => { creationRace = "Gnome"; creationClass = null; });
                    if (creationRace != "Gnome") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    AddBigButton("PortraitHuman" + creationGender, (h) => { creationRace = "Human"; creationClass = null; });
                    if (creationRace != "Human") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    AddBigButton("PortraitNightElf" + creationGender, (h) => { creationRace = "Night Elf"; creationClass = null; });
                    if (creationRace != "Night Elf") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                }
                else if (creationFaction == "Horde")
                {
                    AddBigButton("PortraitOrc" + creationGender, (h) => { creationRace = "Orc"; creationClass = null; });
                    if (creationRace != "Orc") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    AddBigButton("PortraitTauren" + creationGender, (h) => { creationRace = "Tauren"; creationClass = null; });
                    if (creationRace != "Tauren") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    AddBigButton("PortraitTroll" + creationGender, (h) => { creationRace = "Troll"; creationClass = null; });
                    if (creationRace != "Troll") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    AddBigButton("PortraitForsaken" + creationGender, (h) => { creationRace = "Forsaken"; creationClass = null; });
                    if (creationRace != "Forsaken") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                }
            });
            AddHeaderRegion(() =>
            {
                var classes = Class.classes.FindAll(x => x.possibleRaces.Contains(creationRace));
                AddLine("Class: " + creationClass);
                AddSmallButton("ActionReroll", (h) =>
                {
                    creationClass = classes[random.Next(classes.Count)].name;
                });
            });
            AddHeaderRegion(() =>
            {
                var classes = Class.classes.FindAll(x => x.possibleRaces.Contains(creationRace));
                if (creationRace != null)
                    foreach (var foo in classes)
                    {
                        AddBigButton("Class" + foo.name, (h) => { creationClass = foo.name; });
                        if (creationClass != foo.name) { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    }
            });
        }),
        new("CharacterBaseStats", () => {
            SetAnchor(BottomLeft);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                foreach (var foo in currentSave.player.Stats())
                    if (!foo.Key.Contains("Mastery"))
                        AddLine(foo.Key + ":", Gray);
            });
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                foreach (var foo in currentSave.player.Stats())
                    if (!foo.Key.Contains("Mastery"))
                        AddLine(foo.Value + "", foo.Value > currentSave.player.stats.stats[foo.Key] ? Uncommon : (foo.Value < currentSave.player.stats.stats[foo.Key] ? DangerousRed : Gray));
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
                            SetRegionGroupWidth(83);
                            AddHeaderRegion(() =>
                            {
                                AddLine(element + ":", Gray);
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(Board.board.player.resources.ToList().Find(x => x.Key == element).Value + " / " + Board.board.player.MaxResource(element), Gray);
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(39);
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    var value = Board.board.player.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? DarkGray : (value < Board.board.player.MaxResource(element) ? Gray : Green));
                    AddSmallButton("Element" + elements2[elements1.IndexOf(element)] + "Rousing",
                        (h) => { },
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(83);
                            AddHeaderRegion(() =>
                            {
                                AddLine(elements2[elements1.IndexOf(element)] + ":", Gray);
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(Board.board.player.resources.ToList().Find(x => x.Key == elements2[elements1.IndexOf(element)]).Value + " / " + Board.board.player.MaxResource(elements2[elements1.IndexOf(element)]), Gray);
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(20);
            foreach (var element in elements2)
                AddHeaderRegion(() =>
                {
                    var value = Board.board.player.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? DarkGray : (value < Board.board.player.MaxResource(element) ? Gray : Green));
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
                            SetRegionGroupWidth(83);
                            AddHeaderRegion(() =>
                            {
                                AddLine(element + ":", Gray);
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(Board.board.enemy.resources.ToList().Find(x => x.Key == element).Value + " / " + Board.board.enemy.MaxResource(element), Gray);
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(39);
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    var value = Board.board.enemy.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? DarkGray : (value < Board.board.enemy.MaxResource(element) ? Gray : Green));
                    AddSmallButton("Element" + elements2[elements1.IndexOf(element)] + "Rousing",
                        (h) => { },
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(83);
                            AddHeaderRegion(() =>
                            {
                                AddLine(elements2[elements1.IndexOf(element)] + ":", Gray);
                            });
                            AddPaddingRegion(() =>
                            {
                                AddLine(Board.board.enemy.resources.ToList().Find(x => x.Key == elements2[elements1.IndexOf(element)]).Value + " / " + Board.board.enemy.MaxResource(elements2[elements1.IndexOf(element)]), Gray);
                            });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(20);
            foreach (var element in elements2)
                AddHeaderRegion(() =>
                {
                    var value = Board.board.enemy.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? DarkGray : (value < Board.board.enemy.MaxResource(element) ? Gray : Green));
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
                    AddText(currentSave.player.unspentTalentPoints + "", Green);
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
            SetRegionGroupWidth(203);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddLine(a.talentTrees[1].name + ": " + a.talentTrees[1].talents.Count(x => currentSave.player.abilities.Contains(x.ability)));
            });
            SetRegionGroupWidth(202);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddLine(a.talentTrees[2].name + ": " + a.talentTrees[2].talents.Count(x => currentSave.player.abilities.Contains(x.ability)));
            });
            SetRegionGroupWidth(203);
        }, true),
        new("InstanceHeader", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(628);
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
            SetAnchor(TopLeft);
            AddHeaderGroup();
            SetRegionGroupWidth(628);
            AddHeaderRegion(() =>
            {
                AddLine("Console");
                AddSmallButton("OtherClose", (h) => { CloseWindow(h.window); });
            });
            AddRegionGroup();
            SetRegionGroupWidth(628);
            AddInputRegion(String.consoleInput, InputType.Everything, "Console");
        },  true),
    };

    public static List<Blueprint> desktopBlueprints = new()
    {
        new("Map", () =>
        {
            SpawnWindowBlueprint("MapToolbar");
            var findSites = windowBlueprints.FindAll(x => x.title.StartsWith("Site: "));
            foreach (var site in findSites) SpawnWindowBlueprint(site);
            AddHotkey(W, () => { var amount = new Vector3(0, (float)Math.Round(EuelerGrowth())); CDesktop.screen.transform.position += amount; cursor.transform.position += amount; }, false);
            AddHotkey(A, () => { var amount = new Vector3(-(float)Math.Round(EuelerGrowth()), 0); CDesktop.screen.transform.position += amount; cursor.transform.position += amount; }, false);
            AddHotkey(S, () => { var amount = new Vector3(0, -(float)Math.Round(EuelerGrowth())); CDesktop.screen.transform.position += amount; cursor.transform.position += amount; }, false);
            AddHotkey(D, () => { var amount = new Vector3((float)Math.Round(EuelerGrowth()), 0); CDesktop.screen.transform.position += amount; cursor.transform.position += amount; }, false);
            AddHotkey(C, () =>
            {
                SpawnDesktopBlueprint("CharacterSheet");
                SwitchDesktop("CharacterSheet");
                PlaySound("DesktopCharacterSheetOpen");
            });
            AddHotkey(N, () => { SpawnDesktopBlueprint("TalentScreen"); SwitchDesktop("TalentScreen"); });
            AddHotkey(P, () => { SpawnDesktopBlueprint("SpellbookScreen"); SwitchDesktop("SpellbookScreen"); });
            AddHotkey(B, () =>
            {
                if (CDesktop.windows.Exists(x => x.title == "Inventory"))
                {
                    CloseWindow("Inventory");
                    CloseWindow("MapToolbar");
                    SpawnWindowBlueprint("MapToolbar");
                    PlaySound("DesktopInventoryClose");
                }
                else
                {
                    SpawnWindowBlueprint("Inventory");
                    CloseWindow("MapToolbar");
                    SpawnWindowBlueprint("MapToolbar");
                    PlaySound("DesktopInventoryOpen");
                }
            });
            AddHotkey(Escape, () =>
            {
                if (CDesktop.windows.Exists(x => x.title == "Inventory"))
                {
                    CloseWindow("Inventory");
                    CloseWindow("MapToolbar");
                    SpawnWindowBlueprint("MapToolbar");
                    PlaySound("DesktopInventoryClose");
                }
            });
            AddHotkey(L, () => { SpawnWindowBlueprint("ItemDrop"); });
            AddHotkey(BackQuote, () => { SpawnWindowBlueprint("Console"); });
        }),
        new("DungeonEntrance", () =>
        {
            SetDesktopBackground("Areas/Area" + instance.name.Replace("'", "").Replace(" ", ""));
            SpawnWindowBlueprint("Dungeon: " + instance.name);
            SpawnWindowBlueprint("InstanceLeftSide");
            AddHotkey(Escape, () =>
            {
                var window = CDesktop.windows.Find(x => x.title.StartsWith("Area: "));
                if (window != null)
                {
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground("Areas/Area" + instance.name.Replace("'", "").Replace(" ", ""));
                    CloseWindow(window);
                }
                else if (instance.complexPart)
                {
                    CloseDesktop("DungeonEntrance");
                    SpawnDesktopBlueprint("ComplexEntrance");
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("DungeonEntrance");
                }
            });
        }),
        new("RaidEntrance", () =>
        {
            SetDesktopBackground("Areas/Area" + instance.name.Replace("'", "").Replace(" ", ""));
            SpawnWindowBlueprint("Raid: " + instance.name);
            SpawnWindowBlueprint("InstanceLeftSide");
            AddHotkey(Escape, () =>
            {
                var window = CDesktop.windows.Find(x => x.title.StartsWith("Area: "));
                if (window != null)
                {
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground("Areas/Area" + instance.name.Replace("'", "").Replace(" ", ""));
                    CloseWindow(window);
                }
                else if (instance.complexPart)
                {
                    CloseDesktop("RaidEntrance");
                    SpawnDesktopBlueprint("ComplexEntrance");
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("RaidEntrance");
                }
            });
        }),
        new("ComplexEntrance", () =>
        {
            SetDesktopBackground("Areas/Complex" + complex.name.Replace("'", "").Replace(" ", ""));
            SpawnWindowBlueprint("Complex: " + complex.name);
            SpawnWindowBlueprint("ComplexLeftSide");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInstanceClose");
                CloseDesktop("ComplexEntrance");
            });
        }),
        new("Game", () =>
        {
            SetDesktopBackground("Areas/Area" + (Board.board.area.instancePart ? instance.name.Replace("'", "").Replace(" ", "") : "") + Board.board.area.name.Replace("'", "").Replace(" ", ""));
            SpawnWindowBlueprint("BattleBoard");
            SpawnWindowBlueprint("PlayerBattleInfo");
            SpawnWindowBlueprint("EnemyBattleInfo");
            SpawnWindowBlueprint("PlayerResources");
            SpawnWindowBlueprint("EnemyResources");
            Board.board.Reset();
            AddHotkey(Escape, () => { SwitchDesktop("Map"); CloseDesktop("Game"); });
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
            });
            AddHotkey(BackQuote, () => { SpawnWindowBlueprint("Console"); });
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
                SwitchDesktop("Map");
                CloseDesktop("CharacterSheet");
                PlaySound("DesktopCharacterSheetClose");
            });
            AddHotkey(Escape, () =>
            {
                SwitchDesktop("Map");
                CloseDesktop("CharacterSheet");
                PlaySound("DesktopCharacterSheetClose");
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
                CDesktop.screen.transform.position += amount; cursor.transform.position += amount;
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
                CDesktop.screen.transform.position += amount; cursor.transform.position += amount;
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
            SetDesktopBackground("SkinLong", false);
            SpawnWindowBlueprint("SpellbookAbilityList");
            SpawnWindowBlueprint("PlayerSpellbookInfo");
            SpawnWindowBlueprint("SpellbookResources");
            AddHotkey(P, () => { SwitchDesktop("Map"); CloseDesktop("SpellbookScreen"); PlaySound("DesktopSpellbookScreenClose"); });
            AddHotkey(Escape, () => { SwitchDesktop("Map"); CloseDesktop("SpellbookScreen"); PlaySound("DesktopSpellbookScreenClose"); });
            AddHotkey(W, () =>
            {
                var amount = new Vector3(0, (float)Math.Round(EuelerGrowth())) / 2;
                CDesktop.screen.transform.position += amount; cursor.transform.position += amount;
                if (CDesktop.screen.transform.position.y > -180)
                {
                    var off = CDesktop.screen.transform.position.y + 180f;
                    CDesktop.screen.transform.position -= new Vector3(0, off);
                    cursor.transform.position -= new Vector3(0, off);
                }
            },  false);
            AddHotkey(S, () =>
            {
                var amount = new Vector3(0, -(float)Math.Round(EuelerGrowth())) / 2;
                CDesktop.screen.transform.position += amount; cursor.transform.position += amount;
                if (CDesktop.screen.transform.position.y < -1282)
                {
                    var off = CDesktop.screen.transform.position.y + 1282f;
                    CDesktop.screen.transform.position -= new Vector3(0, off);
                    cursor.transform.position -= new Vector3(0, off);
                }
            },  false);
        }),
        new("TitleScreen", () =>
        {
            SpawnWindowBlueprint("TitleScreenMenu");
            SpawnWindowBlueprint("CharacterCreation");
        }),
    };
}
