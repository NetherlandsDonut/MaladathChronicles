using System;
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
        new("ESCMenu", () => {
            CloseWindowOnLostFocus();
            SetAnchor(Center);
            AddRegionGroup();
            AddHandleRegion(() =>
            {
                AddLine("Menu", Black);
                AddSmallButton("OtherClose",
                    (h) =>
                    {
                        CloseWindow(h.window);
                    }
                );
            });
            AddButtonRegion(() =>
            {
                AddLine("Settings", Black);
            },
            (h) =>
            {

            });
            AddButtonRegion(() =>
            {
                AddLine("Exit", Black);
            },
            (h) =>
            {
                UnityEngine.Application.Quit();
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
        new("City", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHandleRegion(() =>
            {
                AddLine("The Synagogue", Black);
                AddSmallButton("OtherClose",
                    (h) =>
                    {
                        CloseWindow(h.window);
                    }
                );
            });
            var list = new List<string> { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight" };
            AddPaddingRegion(() =>
            {
                AddLine("Encounters", Gray);
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
        new("Piracy", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHandleRegion(() =>
            {
                AddLine("The Synagogue", Black);
                AddSmallButton("OtherClose",
                    (h) =>
                    {
                        CloseWindow(h.window);
                    }
                );
            });
            var list = new List<string> { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight" };
            AddPaddingRegion(() =>
            {
                AddLine("Encounters", Gray);
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
        new("PlayerBattleInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(138);
            AddButtonRegion(
                () =>
                {
                    AddLine("Roowr", Black);
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton("ClassRogue",
                    (h) => { },
                    (h) => () =>
                    {
                        SetAnchor(BottomRight);
                        AddRegionGroup();
                        AddHeaderRegion(() =>
                        {
                            AddBigButton("ClassRogue", (h) => { });
                            AddLine("Rogue", Gray);
                        });
                        AddHeaderRegion(() =>
                        {
                            AddLine("Main elements:", Gray);
                        });
                        AddPaddingRegion(() =>
                        {
                            AddBigButton("ElementDecayAwakened", (h) => { });
                            AddBigButton("ElementShadowAwakened", (h) => { });
                            AddBigButton("ElementAirAwakened", (h) => { });
                        });
                        AddHeaderRegion(() =>
                        {
                            AddLine("Class description:", Gray);
                        });
                        AddPaddingRegion(() =>
                        {
                            AddLine("Rogues often initiate combat with a surprise attack", DarkGray);
                            AddLine("from the shadows, leading with vicious melee strikes. ", DarkGray);
                            AddLine("When in protracted battles, they utilize a successive ", DarkGray);
                            AddLine("combination of carefully chosen attacks to soften", DarkGray);
                            AddLine("the enemy up for a killing blow.", DarkGray);
                        });
                    }
                );
                AddLine("Level 21", Gray);
            });
        }),
        new("EnemyBattleInfo", () => {
            SetAnchor(BottomRight);
            AddRegionGroup();
            SetRegionGroupWidth(138);
            AddButtonRegion(
                () =>
                {
                    AddLine("Chromaggus", Black);
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton("wtf", (h) => { });
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
            SetAnchor(Top/*, 0, -39*/);
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
                            Board.board.FloodDestroy(h.window, list);
                        },
                        (h) => () =>
                        {
                            var coords = (h.region.bigButtons.FindIndex(x => x.GetComponent<Highlightable>() == h), h.region.regionGroup.regions.IndexOf(h.region));
                            var count = Board.board.FloodCount(coords.Item1, coords.Item2).Count;
                            SetAnchor(TopRight);
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
            SetAnchor(Center);
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
                        AddBigButton("FactionHorde", (h) => { });
                        AddLine("Crossroads", Gray);
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
            });
        })
    };

    public static List<Blueprint> desktopBlueprints = new()
    {
        new("Menu", () => {
            AddHotkey(B, () => { SpawnWindowBlueprint("BlackTemple"); });
            AddHotkey(C, () => { SpawnWindowBlueprint("City"); });
            AddHotkey(R, () => { SpawnWindowBlueprint("Testing"); });
            AddHotkey(L, () => { SpawnWindowBlueprint("LayoutTest"); });
            AddHotkey(U, () => { SpawnWindowBlueprint("UnitCardsExplorer"); });
            AddHotkey(P, () => { SpawnWindowBlueprint("Piracy"); });
            AddHotkey(A, () => { /*SpawnWindowBlueprint("BattleBoard"); SpawnWindowBlueprint("PlayerBattleInfo"); SpawnWindowBlueprint("EnemyBattleInfo"); SpawnWindowBlueprint("BattleActionBar"); */SpawnWindowBlueprint("Crossroads"); Board.board.Reset(); });
            AddHotkey(Escape, () => { SpawnWindowBlueprint("ESCMenu"); });
            AddHotkey(Tab, () => { SpawnDesktopBlueprint("Game"); });
        }),
        new("Game", () => {
            AddHotkey(A, () => { SpawnWindowBlueprint("Testing"); });
            AddHotkey(Tab, () => { SwitchDesktop("Menu"); CloseDesktop("Game"); });
        }),
    };
}
