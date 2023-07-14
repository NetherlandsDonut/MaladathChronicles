using System;
using UnityEngine;
using System.Collections.Generic;

using static Root;

using static Root.Color;
using static Root.Anchor;
using static Root.RegionBackgroundType;

using static UnityEngine.KeyCode;

public class Blueprint
{
    public Blueprint(string title, Action actions)
    {
        this.title = title;
        this.actions = actions;
    }

    public string title;
    public Action actions;

    public static List<Blueprint> windowBlueprints = new()
    {
        new("Testing", () => {
            SetAnchor(BottomRight);
            AddRegionGroup();
            AddHandleRegion(() =>
            {
                AddLine("Chemical plant", Black);
                AddSmallButton("OtherNextPage",
                    (h) =>
                    {
                        AddRegionGroup(1);
                        AddHandleRegion(() =>
                        {
                            AddLine("Chemical plant", Black);
                            AddSmallButton("OtherClose",
                                (h) =>
                                {
                                    CloseRegionGroup(h.region.regionGroup);
                                }
                            );
                        });
                        AddHeaderRegion(() =>
                        {
                            AddLine("General", LightGray);
                        });
                        AddPaddingRegion(() =>
                        {
                            SetRegionAsGroupExtender();
                            AddLine("Crafting speed: ", DarkGray);
                            AddText("1.00", Gray);
                            AddLine("Products finished: ", DarkGray);
                            AddText("7406", Gray);
                            AddLine("Pollution: ", DarkGray);
                            AddText("4/m", Gray);
                            AddLine("Health: ", DarkGray);
                            AddText("300/300", Gray);
                        });
                        AddButtonRegion(() =>
                        {
                            AddLine("Add to list of abilities", Black);
                        },
                        (h) =>
                        {
                            h.window.Rebuild();
                        });
                    }
                );
            });
            var list = new List<string> { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight" };
            AddPaddingRegion(() =>
            {
                AddLine("List:", Gray);
            });
            AddLineList(6,
                () => list.Count,
                (i) =>
                {
                    SetRegionBackground(Button);
                    AddLine(list[i], Black);
                },
                (i) =>
                {
                    SetRegionBackground(Padding);
                    AddLine("", Gray);
                },
                null
            );
            AddPaddingRegion(() =>
            {
                AddPaginationLine();
                AddNextPageButton();
                AddPreviousPageButton();
            });

            AddRegionGroup();
            AddHandleRegion(() =>
            {
                AddLine("Chemical plant", Black);
            });
            AddButtonRegion(() =>
            {
                AddLine("Select", Black);
            },
            (h) =>
            {

            });
            AddHeaderRegion(() =>
            {
                AddLine("General", LightGray);
            });
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Crafting speed: ", DarkGray);
                AddText("1.00", Gray);
                AddLine("Products finished: ", DarkGray);
                AddText("7406", Gray);
                AddLine("Pollution: ", DarkGray);
                AddText("4/m", Gray);
                AddLine("Health: ", DarkGray);
                AddText("300/300", Gray);
            });
            AddInputRegion(testText, InputType.Everything, "TestingText");
            AddInputRegion(testText2, InputType.Everything, "TestingText2");
        }),
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
            SetRegionGroupWidth(138);
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
                AddBigButton("Class" + Board.board.player.spec.name,
                    (h) => { }
                    //(h) => () =>
                    //{
                    //    SetAnchor(BottomRight);
                    //    AddRegionGroup();
                    //    AddHeaderRegion(() =>
                    //    {
                    //        AddBigButton("ClassRogue", (h) => { });
                    //        AddLine("Rogue", Gray);
                    //    });
                    //    AddHeaderRegion(() =>
                    //    {
                    //        AddLine("Main elements:", Gray);
                    //    });
                    //    AddPaddingRegion(() =>
                    //    {
                    //        AddBigButton("ElementDecayAwakened", (h) => { });
                    //        AddBigButton("ElementShadowAwakened", (h) => { });
                    //        AddBigButton("ElementAirAwakened", (h) => { });
                    //    });
                    //    AddHeaderRegion(() =>
                    //    {
                    //        AddLine("Class description:", Gray);
                    //    });
                    //    AddPaddingRegion(() =>
                    //    {
                    //        AddLine("Rogues often initiate combat with a surprise attack", DarkGray);
                    //        AddLine("from the shadows, leading with vicious melee strikes. ", DarkGray);
                    //        AddLine("When in protracted battles, they utilize a successive ", DarkGray);
                    //        AddLine("combination of carefully chosen attacks to soften", DarkGray);
                    //        AddLine("the enemy up for a killing blow.", DarkGray);
                    //    });
                    //}
                );
                AddLine("Level " + Board.board.player.level, Gray);
                AddLine("Health: " + Board.board.player.health + "/" + Board.board.player.MaxHealth(), Gray);
            });
            foreach (var ability in Board.board.player.abilities)
            {
                var abilityObj = Ability.abilities.Find(x => x.name == ability);
                if (abilityObj == null || abilityObj.cost == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(ability, Black);
                        AddSmallButton("Ability" + ability.Replace(" ", ""), (h) => { });
                    },
                    (h) =>
                    {
                        SetAnchor(BottomRight);
                        AddRegionGroup();
                        AddHeaderRegion(() =>
                        {
                            AddBigButton("Ability" + ability.Replace(" ", ""), (h) => { });
                            AddLine(ability, Gray);
                        });
                        AddHeaderRegion(() =>
                        {
                            AddLine("Resource cost:", Gray);
                        });
                        AddPaddingRegion(() =>
                        {
                            foreach (var resource in abilityObj.cost)
                            {
                                AddLine(resource.Key, Gray);
                                AddText(" x" + resource.Value, White);
                            }
                        });
                    }
                );
            }
        }),
        new("EnemyBattleInfo", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(138);
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
                AddBigButton("Portrait" + Board.board.enemy.name.Replace(", ", ""), (h) => { });
                AddLine("Level " + Board.board.enemy.level, Gray);
                AddLine("Health: " + Board.board.enemy.health + "/" + Board.board.enemy.MaxHealth(), Gray);
            });
            foreach (var ability in Board.board.enemy.abilities)
            {
                var abilityObj = Ability.abilities.Find(x => x.name == ability);
                if (abilityObj == null || abilityObj.cost == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(ability, Black);
                        AddSmallButton("Ability" + ability, (h) => { });
                    },
                    (h) =>
                    {
                        SetAnchor(BottomRight);
                        AddRegionGroup();
                        AddHeaderRegion(() =>
                        {
                            AddBigButton("Ability" + ability.Replace(" ", ""), (h) => { });
                            AddLine(ability, Gray);
                        });
                        AddHeaderRegion(() =>
                        {
                            AddLine("Resource cost:", Gray);
                        });
                        AddPaddingRegion(() =>
                        {
                            foreach (var resource in abilityObj.cost)
                            {
                                AddLine(resource.Key, Gray);
                                AddText(" x" + resource.Value, White);
                            }
                        });
                    }
                );
            }
        }),
        new("EnemyBattleInfo Old", () => {
            SetAnchor(BottomRight);
            AddRegionGroup();
            SetRegionGroupWidth(138);
            AddButtonRegion(
                () =>
                {
                    AddLine("Bone Construct", Black);
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton("PortraitBoneConstruct", (h) => { });
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
        new("JourneyInfoOld", () => {
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
        new("BattleBoard", () => {
            SetAnchor(Top, 0, -19);
            AddRegionGroup();
            for (int i = 0; i < Board.board.field.GetLength(0); i++)
            {
                AddPaddingRegion(() =>
                {
                    for (int j = 0; j < Board.board.field.GetLength(1); j++)
                    {
                        AddBigButton(Board.board.GetFieldButton(),
                        (h) =>
                        {
                            var list = Board.board.FloodCount(h.region.bigButtons.FindIndex(x => x.GetComponent<Highlightable>() == h), h.region.regionGroup.regions.IndexOf(h.region));
                            Board.board.FloodDestroy(list);
                        },
                        (h) => () =>
                        {
                            var coords = (h.region.bigButtons.FindIndex(x => x.GetComponent<Highlightable>() == h), h.region.regionGroup.regions.IndexOf(h.region));
                            var count = Board.board.FloodCount(coords.Item1, coords.Item2).Count;
                            SetAnchor(BottomRight);
                            AddRegionGroup();
                            AddHeaderRegion(
                                () =>
                                {
                                    AddLine("x" + count + " ", LightGray);
                                    AddText(Board.board.GetFieldName(coords.Item1, coords.Item2), Board.board.GetFieldColor(coords.Item1, coords.Item2));
                                }
                            );
                        });
                    }
                });
            }
        }),
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
                    h.window.PlaySound("SpellEnvenomCast");
                    if (random.Next(0, 2) == 1)
                        h.window.PlaySound("SpellEnvenomImpact");
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
        new("Crossroads", () => {
            SetAnchor(1774, -2571);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Crossroads", Gray);
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
                        AddLine("I hope so", Gray);
                        AddLine("There is nothing here yet", Gray);
                        AddLine("But there will be something soon", Gray);
                    });
                });
            });
        }),
        new("Ratchet", () => {
            SetAnchor(1984, -2677);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionNeutral",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Ratchet", Gray);
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
                        AddLine("I hope so", Gray);
                        AddLine("There is nothing here yet", Gray);
                        AddLine("But there will be something soon", Gray);
                    });
                });
            });
        }),
        new("WailingCaverns", () => {
            SetAnchor(1647, -2664);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("SiteDungeon",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Wailing Caverns", Gray);
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
                });
            });
        }),
        new("CampTaurajo", () => {
            SetAnchor(1644, -2968);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(Bottom, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Camp Taurajo", Gray);
                    });
                    //AddHeaderRegion(() =>
                    //{
                    //    AddLine("Nothing:", Gray);
                    //});
                    //AddPaddingRegion(() =>
                    //{
                    //    AddLine("There is nothing here yet", Gray);
                    //    AddLine("But there will be something soon", Gray);
                    //    AddLine("I hope so", Gray);
                    //});
                    //AddHeaderRegion(() =>
                    //{
                    //    AddLine("Very nothing:", Gray);
                    //});
                    //AddPaddingRegion(() =>
                    //{
                    //    AddLine("There is nothing here yet", Gray);
                    //    AddLine("But there will be something soon", Gray);
                    //});
                });
            });
        }),
        new("Maraudon", () => {
            SetAnchor(645, -2779);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("SiteDungeon",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Maraudon", Gray);
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
                });
            });
        }),
        new("TheramoreIsle", () => {
            SetAnchor(2141, -3256);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionAlliance",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Theramore Isle", Gray);
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
                });
            });
        }),
        new("Gadgetzan", () => {
            SetAnchor(2011, -3939);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionNeutral",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Gadgetzan", Gray);
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
                        AddLine("I hope so", Gray);
                        AddLine("There is nothing here yet", Gray);
                        AddLine("But there will be something soon", Gray);
                    });
                });
            });
        }),
        new("Everlook", () => {
            SetAnchor(2191, -1113);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionNeutral",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Everlook", Gray);
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
                        AddLine("I hope so", Gray);
                        AddLine("There is nothing here yet", Gray);
                        AddLine("But there will be something soon", Gray);
                    });
                });
            });
        }),
        new("TalonbranchGlade", () => {
            SetAnchor(1623, -1227);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionAlliance",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Talonbranch Glade", Gray);
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
                });
            });
        }),
        new("BloodvenomPost", () => {
            SetAnchor(1341, -1382);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {
                    SpawnWindowBlueprint("SelectedTown");
                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Bloodvenom Post", Gray);
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
                });
            });
        }),
        new("RazorHill", () => {
            SetAnchor(2202, -2421);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Razor Hill", Gray);
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
                });
            });
        }),
        new("ThunderBluff", () => {
            SetAnchor(1228, -2734);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Thunder Bluff", Gray);
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
                });
            });
        }),
        new("Orgrimmar", () => {
            SetAnchor(2093, -2160);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Orgrimmar", Gray);
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
                });
            });
        }),
        new("SplintertreePost", () => {
            SetAnchor(1744, -2009);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Splintertree Post", Gray);
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
                });
            });
        }),
        new("DireMaul", () => {
            SetAnchor(960, -3397);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("SiteDungeon",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Dire Maul", Gray);
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
                });
            });
        }),
        new("RuinsAhnQiraj", () => {
            SetAnchor(844, -4395);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("SiteRaid",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Ruins of Ahn\'Qiraj", Gray);
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
                });
            });
        }),
        new("TempleAhnQiraj", () => {
            SetAnchor(824, -4236);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("SiteRaid",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Temple of Ahn\'Qiraj", Gray);
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
                });
            });
        }),
        new("SunrockRetreat", () => {
            SetAnchor(1033, -2300);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Sunrock Retreat", Gray);
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
                });
            });
        }),
        new("ZoramgarOutpost", () => {
            SetAnchor(1030, -1804);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Zoram\'gar Outpost", Gray);
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
                });
            });
        }),
        new("StonetalonPeak", () => {
            SetAnchor(939, -1943);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionAlliance",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Stonetalon Peak", Gray);
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
                });
            });
        }),
        new("NijelsPoint", () => {
            SetAnchor(975, -2446);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionAlliance",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Nijel\'s Point", Gray);
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
                });
            });
        }),
        new("ShadowpreyVillage", () => {
            SetAnchor(592, -2818);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Shadowprey Village", Gray);
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
                });
            });
        }),
        new("CampMojache", () => {
            SetAnchor(1179, -3374);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Camp Mojache", Gray);
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
                });
            });
        }),
        new("FreewindPost", () => {
            SetAnchor(1734, -3592);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Freewind Post", Gray);
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
                });
            });
        }),
        new("RazorfenDowns", () => {
            SetAnchor(1762, -3467);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("SiteDungeon",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Razorfen Downs", Gray);
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
                });
            });
        }),
        new("BrackenwallVillage", () => {
            SetAnchor(1821, -3131);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Brackenwall Village", Gray);
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
                });
            });
        }),
        new("BloodhoofVillage", () => {
            SetAnchor(1319, -2956);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Bloodhoof Village", Gray);
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
                });
            });
        }),
        new("SenjinVillage", () => {
            SetAnchor(2233, -2656);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Sen\'jin Village", Gray);
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
                });
            });
        }),
        new("Thalaanar", () => {
            SetAnchor(1382, -3397);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionAlliance",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Thalaanar", Gray);
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
                });
            });
        }),
        new("FeathermoonStronghold", () => {
            SetAnchor(563, -3392);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionAlliance",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Feathermoon Stronghold", Gray);
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
                });
            });
        }),
        new("Auberdine", () => {
            SetAnchor(1142, -1176);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionAlliance",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Auberdine", Gray);
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
                });
            });
        }),
        new("BlackfathomDeeps", () => {
            SetAnchor(1058, -1644);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("SiteDungeon",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Blackfathom Deeps", Gray);
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
                });
            });
        }),
        new("Darnassus", () => {
            SetAnchor(720, -465);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionAlliance",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Darnassus", Gray);
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
                });
            });
        }),
        new("RutTheranVilage", () => {
            SetAnchor(1027, -711);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionAlliance",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("RutTheranVilage", Gray);
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
                });
            });
        }),
        new("TalrendisPoint", () => {
            SetAnchor(2183, -1958);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionAlliance",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Talrendis Point", Gray);
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
                });
            });
        }),
        new("Valormok", () => {
            SetAnchor(2120, -1745);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionHorde",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Valormok", Gray);
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
                });
            });
        }),
        new("ZulFarrak", () => {
            //SetAnchor(1781, -3768);
            SetAnchor(0, -0);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("SiteDungeon",
                (h) =>
                {
                    Board.board = new Board(8, 8, "Nefarian");
                    SpawnDesktopBlueprint("Game");
                    SwitchDesktop("Game");
                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Zul\'Farrak", Gray);
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
                });
            });
        }),
        new("CenarionHold", () => {
            SetAnchor(1076, -3875);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("FactionNeutral",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Cenarion Hold", Gray);
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
                        AddLine("I hope so", Gray);
                        AddLine("There is nothing here yet", Gray);
                        AddLine("But there will be something soon", Gray);
                    });
                });
            });
        }),
        new("WindshearMine", () => {
            SetAnchor(1317, -2281);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("SiteHostileArea",
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine("Windshear Mine", Gray);
                    });
                    AddPaddingRegion(() =>
                    {
                        AddLine("Enemy levels: ", DarkGray);
                        AddText("19-23", Gray);
                    });
                });
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
    };

    public static List<Blueprint> desktopBlueprints = new()
    {
        new("Map", () =>
        {
            SpawnWindowBlueprint("Crossroads");
            SpawnWindowBlueprint("Ratchet");
            SpawnWindowBlueprint("WailingCaverns");
            SpawnWindowBlueprint("CampTaurajo");
            SpawnWindowBlueprint("Maraudon");
            SpawnWindowBlueprint("TheramoreIsle");
            SpawnWindowBlueprint("Gadgetzan");
            SpawnWindowBlueprint("Everlook");
            SpawnWindowBlueprint("TalonbranchGlade");
            SpawnWindowBlueprint("BloodvenomPost");
            SpawnWindowBlueprint("RazorHill");
            SpawnWindowBlueprint("ThunderBluff");
            SpawnWindowBlueprint("Orgrimmar");
            SpawnWindowBlueprint("SplintertreePost");
            SpawnWindowBlueprint("DireMaul");
            SpawnWindowBlueprint("RuinsAhnQiraj");
            SpawnWindowBlueprint("TempleAhnQiraj");
            SpawnWindowBlueprint("SunrockRetreat");
            SpawnWindowBlueprint("ZoramgarOutpost");
            SpawnWindowBlueprint("StonetalonPeak");
            SpawnWindowBlueprint("NijelsPoint");
            SpawnWindowBlueprint("ShadowpreyVillage");
            SpawnWindowBlueprint("CampMojache");
            SpawnWindowBlueprint("FreewindPost");
            SpawnWindowBlueprint("RazorfenDowns");
            SpawnWindowBlueprint("BrackenwallVillage");
            SpawnWindowBlueprint("BloodhoofVillage");
            SpawnWindowBlueprint("SenjinVillage");
            SpawnWindowBlueprint("Thalaanar");
            SpawnWindowBlueprint("FeathermoonStronghold");
            SpawnWindowBlueprint("Auberdine");
            SpawnWindowBlueprint("BlackfathomDeeps");
            SpawnWindowBlueprint("Darnassus");
            SpawnWindowBlueprint("RutTheranVilage");
            SpawnWindowBlueprint("TalrendisPoint");
            SpawnWindowBlueprint("Valormok");
            SpawnWindowBlueprint("ZulFarrak");
            SpawnWindowBlueprint("CenarionHold");
            SpawnWindowBlueprint("WindshearMine");
            AddHotkey(W, () => { var amount = new Vector3(0, (float)Math.Round(EuelerGrowth())); CDesktop.screen.transform.position += amount; cursor.transform.position += amount; }, false);
            AddHotkey(A, () => { var amount = new Vector3(-(float)Math.Round(EuelerGrowth()), 0); CDesktop.screen.transform.position += amount; cursor.transform.position += amount; }, false);
            AddHotkey(S, () => { var amount = new Vector3(0, -(float)Math.Round(EuelerGrowth())); CDesktop.screen.transform.position += amount; cursor.transform.position += amount; }, false);
            AddHotkey(D, () => { var amount = new Vector3((float)Math.Round(EuelerGrowth()), 0); CDesktop.screen.transform.position += amount; cursor.transform.position += amount; }, false);
            AddHotkey(C, () => { SpawnDesktopBlueprint("CharacterScreen"); SwitchDesktop("CharacterScreen"); });
            AddHotkey(B, () => { SpawnWindowBlueprint("PlayerInventory"); });
        }),
        new("Game", () =>
        {
            SetDesktopBackground("ZoneStonetalonMountains");
            SpawnWindowBlueprint("BattleBoard");
            SpawnWindowBlueprint("PlayerBattleInfo");
            SpawnWindowBlueprint("EnemyBattleInfo");
            //SpawnWindowBlueprint("BattleActionBar");
            Board.board.Reset();
            AddHotkey(Escape, () => { SwitchDesktop("Map"); CloseDesktop("Game"); });
        }),
        new("CharacterScreen", () =>
        {
            SetDesktopBackground("Stone");
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
        new("TitleScreen", () =>
        {
            //SetDesktopBackground("ZoneStonetalonMountains");
            SpawnWindowBlueprint("TitleScreenMenu");
        }),
    };
}
