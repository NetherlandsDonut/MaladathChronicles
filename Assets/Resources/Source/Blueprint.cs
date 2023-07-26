using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;

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
            SetRegionGroupWidth(163);
            AddButtonRegion(
                () =>
                {
                    AddLine(Board.board.player.name, Black);
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
                AddLine("Level " + Board.board.player.level, Gray);
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
                        AddSmallButton("Ability" + actionBar.ability.Replace(" ", "").Replace(":", ""), (h) => { });
                        if (actionBar.cooldown > 0 || !abilityObj.EnoughResources(Board.board.player))
                            AddSmallButtonOverlay("OtherGrid");
                    },
                    (h) =>
                    {
                        if (abilityObj.EnoughResources(Board.board.player))
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
                        SetAnchor(Top, 0, -13);
                        AddHeaderGroup();
                        SetRegionGroupWidth(256);
                        SetRegionGroupHeight(237);
                        AddHeaderRegion(() =>
                        {
                            AddLine(actionBar.ability, Gray);
                        });
                        AddPaddingRegion(() =>
                        {
                            AddBigButton("Ability" + actionBar.ability.Replace(" ", "").Replace(":", ""), (h) => { });
                            AddLine("Required level: ", DarkGray);
                            AddText(Board.board.player.GetClass().abilities.Find(x => x.Item1 == actionBar.ability).Item2 + "", Gray);
                            AddLine("Cooldown: ", DarkGray);
                            AddText(abilityObj.cooldown == 0 ? "None" : abilityObj.cooldown + (abilityObj.cooldown == 1 ? " turn"  : " turns"), Gray);
                        });
                        abilityObj.description();
                        foreach (var cost in abilityObj.cost)
                        {
                            AddRegionGroup();
                            AddHeaderRegion(() =>
                            {
                                AddSmallButton("Element" + cost.Key + "Rousing", (h) => { });
                            });
                            AddRegionGroup();
                            SetRegionGroupWidth(15);
                            AddHeaderRegion(() =>
                            {
                                AddLine(cost.Value + "", cost.Value > Board.board.player.resources[cost.Key] ? Red : Green);
                            });
                        }
                        AddRegionGroup();
                        SetRegionGroupWidth(256 - abilityObj.cost.Count * 44);
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
            SetRegionGroupWidth(163);
            AddButtonRegion(
                () =>
                {
                    AddLine(Board.board.player.name, Black);
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
                AddLine("Level " + Board.board.player.level, Gray);
            });
            foreach (var actionBar in Board.board.player.actionBars)
            {
                var abilityObj = Ability.abilities.Find(x => x.name == actionBar.ability);
                if (abilityObj == null || abilityObj.cost == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(actionBar.ability, Black);
                        AddSmallButton("Ability" + actionBar.ability.Replace(" ", "").Replace(":", ""), (h) => { });
                    },
                    (h) =>
                    {

                    },
                    (h) => () =>
                    {
                        SetAnchor(Top, 0, -13);
                        AddHeaderGroup();
                        SetRegionGroupWidth(256);
                        SetRegionGroupHeight(237);
                        AddHeaderRegion(() =>
                        {
                            AddLine(actionBar.ability, Gray);
                        });
                        AddPaddingRegion(() =>
                        {
                            AddBigButton("Ability" + actionBar.ability.Replace(" ", "").Replace(":", ""), (h) => { });
                            AddLine("Required level: ", DarkGray);
                            AddText(Board.board.player.GetClass().abilities.Find(x => x.Item1 == actionBar.ability).Item2 + "", Gray);
                            AddLine("Cooldown: ", DarkGray);
                            AddText(abilityObj.cooldown == 0 ? "None" : abilityObj.cooldown + (abilityObj.cooldown == 1 ? " turn"  : " turns"), Gray);
                        });
                        abilityObj.description();
                        foreach (var cost in abilityObj.cost)
                        {
                            AddRegionGroup();
                            AddHeaderRegion(() =>
                            {
                                AddSmallButton("Element" + cost.Key + "Rousing", (h) => { });
                            });
                            AddRegionGroup();
                            SetRegionGroupWidth(15);
                            AddHeaderRegion(() =>
                            {
                                AddLine(cost.Value + "", cost.Value > Board.board.player.resources[cost.Key] ? Red : Green);
                            });
                        }
                        AddRegionGroup();
                        SetRegionGroupWidth(256 - abilityObj.cost.Count * 44);
                        AddPaddingRegion(() =>
                        {
                            AddLine("", LightGray);
                        });
                    }
                );
            }
        }),
        new("EnemyBattleInfo", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(163);
            AddButtonRegion(
                () =>
                {
                    AddLine(Board.board.enemy.name, Black);
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton("Portrait" + Race.races.Find(x => x.name == Board.board.enemy.race).portrait, (h) => { });
                AddLine("Level " + Board.board.enemy.level, Gray);
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
                        AddSmallButton("Ability" + actionBar.ability.Replace(" ", "").Replace(":", ""), (h) => { });
                        if (actionBar.cooldown > 0 || !abilityObj.EnoughResources(Board.board.enemy))
                            AddSmallButtonOverlay("OtherGrid");
                    },
                    (h) =>
                    {

                    },
                    (h) => () =>
                    {
                        SetAnchor(Top, 0, -13);
                        AddHeaderGroup();
                        SetRegionGroupWidth(256);
                        SetRegionGroupHeight(237);
                        AddHeaderRegion(() =>
                        {
                            AddLine(actionBar.ability, Gray);
                        });
                        AddPaddingRegion(() =>
                        {
                            AddBigButton("Ability" + actionBar.ability.Replace(" ", "").Replace(":", ""), (h) => { });
                            //AddLine("Required level: ", DarkGray);
                            //AddText(Board.board.enemy.spec.abilities.Find(x => x.Item1 == ability).Item2 + "", Gray);
                            AddLine("Cooldown: ", DarkGray);
                            AddText(abilityObj.cooldown == 0 ? "None" : abilityObj.cooldown + (abilityObj.cooldown == 1 ? " turn"  : " turns"), Gray);
                        });
                        abilityObj.description();
                        foreach (var cost in abilityObj.cost)
                        {
                            AddRegionGroup();
                            AddHeaderRegion(() =>
                            {
                                AddSmallButton("Element" + cost.Key + "Rousing", (h) => { });
                            });
                            AddRegionGroup();
                            SetRegionGroupWidth(15);
                            AddHeaderRegion(() =>
                            {
                                AddLine(cost.Value + "", LightGray);
                            });
                        }
                        AddRegionGroup();
                        SetRegionGroupWidth(256 - abilityObj.cost.Count * 44);
                        AddPaddingRegion(() =>
                        {
                            AddLine("", LightGray);
                        });
                    }
                );
            }
        }),
        new("JourneyInfo", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(138);
            AddButtonRegion(
                () =>
                {
                    AddLine("Nefarian", Black);
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton("PortraitNefarian", (h) => { });
                AddLine("Level 60", Gray);
            });
            AddButtonRegion(
                () =>
                {
                    AddLine("?", Black);
                    AddSmallButton("OtherUnknown", (h) => { });
                },
                (h) =>
                {

                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("?", Black);
                    AddSmallButton("OtherUnknown", (h) => { });
                },
                (h) =>
                {

                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("?", Black);
                    AddSmallButton("OtherUnknown", (h) => { });
                },
                (h) =>
                {

                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("?", Black);
                    AddSmallButton("OtherUnknown", (h) => { });
                },
                (h) =>
                {

                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("?", Black);
                    AddSmallButton("OtherUnknown", (h) => { });
                },
                (h) =>
                {

                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("?", Black);
                    AddSmallButton("OtherUnknown", (h) => { });
                },
                (h) =>
                {

                }
            );
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
            SetAnchor(Top, 0, -13);
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
            SetRegionGroupWidth(163);
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
                AddLine("Level " + currentSave.player.level, Gray);
                AddLine("Health: " + currentSave.player.health + "/" + currentSave.player.MaxHealth(), Gray);
            });
            AddButtonRegion(
                () =>
                {
                    AddLine("Inventory", Black);
                    AddSmallButton("MenuInventory", (h) => { });
                },
                (h) =>
                {
                    SpawnDesktopBlueprint("CharacterScreen");
                    SwitchDesktop("CharacterScreen");
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
            AddPaddingRegion(
                () =>
                {
                    AddBigButton("ItemCrossbow4", (h) => { });
                    AddLine("Required level: ", DarkGray);
                    AddText(34 + "", Gray);
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
                    AddLine("1", Gold);
                    AddSmallButton("ItemCoinsGold", (h) => { });
                }
            );
            AddRegionGroup();
            AddPaddingRegion(
                () =>
                {
                    AddLine("42", Silver);
                    AddSmallButton("ItemCoinsSilver", (h) => { });
                }
            );
            AddRegionGroup();
            AddPaddingRegion(
                () =>
                {
                    AddLine("11", Copper);
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
        new("SelectedTown", () => {
            SetAnchor(BottomLeft);
            AddRegionGroup();
            AddButtonRegion(() =>
            {
                AddLine("Bloodvenom Post", Black);
                AddSmallButton("OtherClose",
                (h) =>
                {
                    CloseWindow(h.window);
                },
                (h) => () =>
                {
                    SetAnchor(BottomRight);
                    AddRegionGroup();
                    AddHeaderRegion(
                        () =>
                        {
                            AddLine("Close this window", Gray);
                        }
                    );
                });
            },
            (h) =>
            {
            });
            AddHeaderRegion(() =>
            {
                AddBigButton("FactionHorde",
                (h) =>
                {
                },
                (h) => () =>
                {

                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Nothing:", Gray);
            });
            AddPaddingRegion(() =>
            {
                AddLine("There is nothing here yet", Gray);
                AddLine("But there will be something soon", Gray);
                AddLine("I hope so", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Very nothing:", Gray);
            });
            AddPaddingRegion(() =>
            {
                AddLine("There is nothing here yet", Gray);
                AddLine("But there will be something soon", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Very nothing:", Gray);
            });
            AddPaddingRegion(() =>
            {
                AddLine("There is nothing here yet", Gray);
                AddLine("But there will be something soon", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Very nothing:", Gray);
            });
            AddPaddingRegion(() =>
            {
                AddLine("There is nothing here yet", Gray);
                AddLine("But there will be something soon", Gray);
            });
        }),
        new("CharacterNeckSlot", () => {
            SetAnchor(-98, 74);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Neck");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterBackSlot", () => {
            SetAnchor(-98, 22);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Back");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterRingSlot", () => {
            SetAnchor(-98, -30);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Ring");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterHeadSlot", () => {
            SetAnchor(-46, 100);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Head");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterChestSlot", () => {
            SetAnchor(-46, 48);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Chest");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterLegsSlot", () => {
            SetAnchor(-46, -4);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Legs");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterFeetSlot", () => {
            SetAnchor(-46, -56);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Feet");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterShouldersSlot", () => {
            SetAnchor(6, 100);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Shoulders");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterHandsSlot", () => {
            SetAnchor(6, 48);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Hands");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterWaistSlot", () => {
            SetAnchor(6, -4);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Waist");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterSpecialSlot", () => {
            SetAnchor(6, -56);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Special");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterMainHandSlot", () => {
            SetAnchor(58, 74);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("MainHand");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterOffHandSlot", () => {
            SetAnchor(58, 22);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("OffHand");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterTrinketSlot", () => {
            SetAnchor(58, -30);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var item = currentSave.player.GetSlot("Trinket");
                AddBigButton(item == null ? "OtherEmpty" : item.icon,
                (h) =>
                {

                },
                (h) => () =>
                {

                });
            });
        }),
        new("CharacterStats", () => {
            SetAnchor(BottomLeft);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                foreach (var foo in currentSave.player.stats.stats)
                    AddLine(foo.Key + ": " + foo.Value, Gray);
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
                    AddSmallButton("Element" + element + "Rousing", (h) => { });
                });
            AddRegionGroup();
            SetRegionGroupWidth(34);
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    AddLine(Board.board.player.resources.ToList().Find(x => x.Key == element).Value + "", LightGray);
                    AddSmallButton("Element" + elements2[elements1.IndexOf(element)] + "Rousing", (h) => { });
                });
            AddRegionGroup();
            SetRegionGroupWidth(15);
            foreach (var element in elements2)
                AddHeaderRegion(() =>
                {
                    AddLine(Board.board.player.resources.ToList().Find(x => x.Key == element).Value + "", LightGray);
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
                    AddSmallButton("Element" + element + "Rousing", (h) => { });
                });
            AddRegionGroup();
            SetRegionGroupWidth(34);
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    AddLine(Board.board.enemy.resources.ToList().Find(x => x.Key == element).Value + "", LightGray);
                    AddSmallButton("Element" + elements2[elements1.IndexOf(element)] + "Rousing", (h) => { });
                });
            AddRegionGroup();
            SetRegionGroupWidth(15);
            foreach (var element in elements2)
                AddHeaderRegion(() =>
                {
                    AddLine(Board.board.enemy.resources.ToList().Find(x => x.Key == element).Value + "", LightGray);
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
        },  true),
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
            AddHotkey(C, () => { SpawnDesktopBlueprint("CharacterScreen"); SwitchDesktop("CharacterScreen"); });
            AddHotkey(N, () => { SpawnDesktopBlueprint("TalentScreen"); SwitchDesktop("TalentScreen"); });
            AddHotkey(P, () => { SpawnDesktopBlueprint("SpellbookScreen"); SwitchDesktop("SpellbookScreen"); });
            AddHotkey(B, () => { SpawnWindowBlueprint("PlayerInventory"); });
            AddHotkey(L, () => { SpawnWindowBlueprint("ItemDrop"); });
            AddHotkey(BackQuote, () => { SpawnWindowBlueprint("Console"); });
        }),
        new("Game", () =>
        {
            SetDesktopBackground("ZoneZulFarrak2");
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
        new("CharacterScreen", () =>
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
            //SpawnWindowBlueprint("PlayerBattleInfo");
            //SpawnWindowBlueprint("EnemyBattleInfo");
            //SpawnWindowBlueprint("BattleActionBar");
            AddHotkey(C, () => { SwitchDesktop("Map"); CloseDesktop("CharacterScreen"); });
            AddHotkey(Escape, () => { SwitchDesktop("Map"); CloseDesktop("CharacterScreen"); });
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
        new("TitleScreen", () =>
        {
            SpawnWindowBlueprint("TitleScreenMenu");
        }),
        new("SpellbookScreen", () =>
        {
            PlaySound("DesktopSpellbookScreenOpen");
            SetDesktopBackground("StoneSplitLong", false);
            SpawnWindowBlueprint("ReturnToMap");
            SpawnWindowBlueprint("PlayerSpellbookInfo");
            AddHotkey(P, () => { SwitchDesktop("Map"); CloseDesktop("SpellbookScreen"); PlaySound("DesktopSpellbookScreenClose"); });
            AddHotkey(Escape, () => { SwitchDesktop("Map"); CloseDesktop("SpellbookScreen"); PlaySound("DesktopSpellbookScreenClose"); });
        }),
        new("TitleScreen", () =>
        {
            SpawnWindowBlueprint("TitleScreenMenu");
        }),
    };
}
