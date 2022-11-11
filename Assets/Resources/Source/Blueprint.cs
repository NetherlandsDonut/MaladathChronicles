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
            CloseWindowOnLostFocus();
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
        new("UnitCardsExplorer", () => {
            SetAnchor(Center);

            AddHeaderGroup();
            AddHandleRegion(() =>
            {
                AddLine("Unit Cards Explorer", Black);
                AddSmallButton(Close,
                    (h) =>
                    {
                        CloseWindow("UnitCardsExplorer");
                    }
                );
            });
            AddRegionGroup();
            SetRegionGroupWidth(150);
            AddButtonRegion(
                () =>
                {
                    AddLine("Leader cards:", Gray);
                    AddDropdown((s) => s + ":", () => new List<string> { "Leader cards", "Unit cards", "Spell cards" });
                    AddSmallButton(NextPage,
                        (h) =>
                        {
                            Data.data.leaderCards.Add(new LeaderCard() { title = random.Next(10000, 100000).ToString() });
                        }
                    );
                },
                (h) =>
                {

                }
            );
            AddLineList(12,
                () => Data.data.leaderCards.Count,
                (i) =>
                {
                    SetRegionBackground(Button);
                    AddLine(Data.data.leaderCards[i].title, Black);
                    SetTooltipForRegion(
                        () =>
                        {
                            SetAnchor(BottomRight);
                            AddRegionGroup();
                            AddHeaderRegion(() =>
                            {
                                AddLine(Data.data.leaderCards[i].title, LightGray);
                            });
                        }
                    );
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
            AddPaddingRegion(() =>
            {
                AddLine("Max. consumption: ", DarkGray);
                AddText("217 kW", Gray);
                AddLine("Min. consumption: ", DarkGray);
                AddText("7 kW", Gray);
            });
        }),
    };

    public static List<Blueprint> desktopBlueprints = new()
    {
        new("Menu", () => {
            AddHotkey(Q, () => { CDesktop.LBWindow.Rebuild(); });
            AddHotkey(R, () => { SpawnWindowBlueprint("Testing"); });
            AddHotkey(L, () => { SpawnWindowBlueprint("LayoutTest"); });
            AddHotkey(U, () => { SpawnWindowBlueprint("UnitCardsExplorer"); });
            AddHotkey(Tab, () => { SpawnDesktopBlueprint("Game"); });
        }),
        new("Game", () => {
            AddHotkey(A, () => { SpawnWindowBlueprint("Testing"); });
            AddHotkey(Tab, () => { SwitchDesktop("Menu"); CloseDesktop("Game"); });
        }),
    };
}
