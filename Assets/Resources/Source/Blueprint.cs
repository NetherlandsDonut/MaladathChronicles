using System;
using System.Collections.Generic;

using static Root;

using static Root.Color;
using static Root.Anchor;
using static Root.RegionBackgroundType;
using static Root.SmallButtonTypes;

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
                AddSmallButton(NextPage,
                    (h) =>
                    {
                        AddRegionGroup(1);
                        AddHandleRegion(() =>
                        {
                            AddLine("Chemical plant", Black);
                            AddSmallButton(Close,
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
                AddSmallButton(Close,
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
                AddSmallButton(Close,
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
        new("BlackTemple", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHandleRegion(() =>
            {
                AddLine("Black Temple: Scouting The Canals", Black);
                AddSmallButton(Close,
                    (h) =>
                    {
                        CloseWindow(h.window);
                    }
                );
            });
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Ambrose", Paladin);
                AddSmallButton(Unwind,
                    (h) =>
                    {
                    },
                    (h) => () =>
                    {
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
                    }
                );
            }
            );
            AddPaddingRegion(() =>
            {
                AddLine("Class: ", DarkGray);
                AddText("Paladin", Paladin);
                AddLine("Spec: ", DarkGray);
                AddText("Holy", Paladin);
                AddLine("Health: ", DarkGray);
                AddText("67/74", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Taeilynn", Warrior);
                AddSmallButton(Unwind,
                    (h) =>
                    {
                        CloseWindow(h.window);
                    }
                );
            });
            AddPaddingRegion(() =>
            {
                AddLine("Class: ", DarkGray);
                AddText("Warrior", Warrior);
                AddLine("Spec: ", DarkGray);
                AddText("Protection", Warrior);
                AddLine("Health: ", DarkGray);
                AddText("108/121", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Otterley", Hunter);
                AddSmallButton(SmallButtonTypes.Unwind,
                    (h) =>
                    {
                        CloseWindow(h.window);
                    }
                );
            });
            AddPaddingRegion(() =>
            {
                AddLine("Class: ", DarkGray);
                AddText("Hunter", Hunter);
                AddLine("Spec: ", DarkGray);
                AddText("Marksmanship", Hunter);
                AddLine("Health: ", DarkGray);
                AddText("62/62", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Sagmund", Mage);
                AddSmallButton(Unwind,
                    (h) =>
                    {
                        CloseWindow(h.window);
                    }
                );
            });
            AddPaddingRegion(() =>
            {
                AddLine("Class: ", DarkGray);
                AddText("Mage", Mage);
                AddLine("Spec: ", DarkGray);
                AddText("Frost", Mage);
                AddLine("Health: ", DarkGray);
                AddText("51/51", Gray);
            });
        }),
        new("Piracy", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHandleRegion(() =>
            {
                AddLine("The Synagogue", Black);
                AddSmallButton(Close,
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
            SetAnchor(TopRight);
            AddRegionGroup();
            AddHandleRegion(() =>
            {
                AddLine("Roowr", Black);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Resources:", Gray);
            });
            AddPaddingRegion(() =>
            {
                AddLine("x2 Decay", Gray);
                AddLine("x3 Shadow", Gray);
                AddLine("x1 Air", Gray);
            });
            SetRegionAsGroupExtender();
            AddButtonRegion(
                () =>
                {
                    AddLine("Mutilate", Black);
                },
                (h) =>
                {

                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("Garrote", Black);
                },
                (h) =>
                {

                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("Rupture", Black);
                },
                (h) =>
                {

                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("Envenom", Black);
                },
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(Center);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
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
                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("Evasion", Black);
                },
                (h) =>
                {

                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("Kidney Shot", Black);
                },
                (h) =>
                {

                }
            );
        }),
        new("BattleBoard", () => {
            SetAnchor(Center);
            AddRegionGroup();
            for (int i = 0; i < Board.board.field.GetLength(0); i++)
            {
                AddPaddingRegion(() =>
                {
                    for (int j = 0; j < Board.board.field.GetLength(1); j++)
                    {
                        AddBigButton(Board.board.GetFieldButton(), (h) =>
                        {
                            var list = Board.board.FloodCount(h.region.bigButtons.FindIndex(x => x.GetComponent<Highlightable>() == h), h.region.regionGroup.regions.IndexOf(h.region));
                            Board.board.FloodDestroy(h.window, list);
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
            AddHotkey(A, () => { SpawnWindowBlueprint("BattleBoard"); SpawnWindowBlueprint("PlayerBattleInfo"); Board.board.Reset(); });
            AddHotkey(Escape, () => { SpawnWindowBlueprint("ESCMenu"); });
            AddHotkey(Tab, () => { SpawnDesktopBlueprint("Game"); });
        }),
        new("Game", () => {
            AddHotkey(A, () => { SpawnWindowBlueprint("Testing"); });
            AddHotkey(Tab, () => { SwitchDesktop("Menu"); CloseDesktop("Game"); });
        }),
    };
}
