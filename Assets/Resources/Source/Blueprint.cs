using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Item;
using static Root;
using static Buff;
using static Race;
using static Class;
using static Sound;
using static Cursor;
using static ItemSet;
using static Ability;
using static Coloring;
using static Serialization;
using static SiteHostileArea;
using static SiteInstance;
using static SiteComplex;
using static SiteTown;

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
                AddLine("Menu", "Gray");
            });
            AddRegionGroup();
            AddButtonRegion(() =>
            {
                AddLine("Singleplayer", "Black");
            },
            (h) =>
            {
                SpawnWindowBlueprint("TitleScreenSingleplayer");
                CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Settings", "Black");
            },
            (h) =>
            {
                SpawnWindowBlueprint("TitleScreenSettings");
                CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Achievments", "Black");
            },
            (h) =>
            {
                //SpawnWindowBlueprint("TitleScreenGraveyard");
                //CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Graveyard", "Black");
            },
            (h) =>
            {
                //SpawnWindowBlueprint("TitleScreenGraveyard");
                //CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Credits", "Black");
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
                        AddLine("Exits the game.");
                        AddLine("This action does not");
                        AddLine("save your game progress!");
                    }
                );
            });
            AddButtonRegion(() =>
            {
                AddLine("Exit", "Black");
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
                        AddLine("Exits the game.");
                        AddLine("This action does not");
                        AddLine("save your game progress!");
                    }
                );
            });
        }, true),
        new("TitleScreenSingleplayer", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Singleplayer", "Gray");
            });
            AddRegionGroup();
            AddButtonRegion(() =>
            {
                AddLine("Continue last game", "Black");
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
                AddLine("Create a new character", "Black");
            },
            (h) =>
            {

            });
            AddButtonRegion(() =>
            {
                AddLine("Load saved game", "Black");
            },
            (h) =>
            {

            });
            AddButtonRegion(() =>
            {
                AddLine("Back", "Black");
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
                        AddLine("Returns you to the main menu");
                    }
                );
            });
        }, true),
        new("TitleScreenSettings", () =>
        {
            SetAnchor(Center);
            AddHeaderGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Settings", "Gray");
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
            AddButtonRegion(() =>
                {
                    AddLine("Back", "Black");
                },
                (h) =>
                {
                    SpawnWindowBlueprint("TitleScreenMenu");
                    CloseWindow(h.window);
                }
            );
        }, true),
        new("PlayerBattleInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            AddButtonRegion(
                () =>
                {
                    AddLine(Board.board.player.name, "Black");
                    AddSmallButton("MenuFlee", (h) =>
                    {
                        CloseDesktop("Game");
                        if (Board.board.area != null)
                        {
                            if (Board.board.area.instancePart)
                            SwitchDesktop("InstanceEntrance");
                            CDesktop.Rebuild();
                        }
                        else SwitchDesktop("Map");
                    });
                    //AddSmallButton("MenuLog", (h) => { });
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton(Board.board.player.GetClass().icon,
                    (h) => { }
                );
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
                            h.window.desktop.Rebuild();
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
            SetRegionGroupWidth(161);
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
                AddBigButton("Portrait" + currentSave.player.race + currentSave.player.gender,
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
                    AddButtonRegion(
                        () =>
                        {
                            AddLine(item.name, "", "Right");
                            AddSmallButton(item.icon, (h) => { });
                        },
                        (h) =>
                        {
                            PlaySound(item.ItemSound("PutDown"));
                            currentSave.player.Unequip(new() { slot });
                            CloseWindow(h.window);
                            SpawnWindowBlueprint("PlayerEquipmentInfo");
                            CloseWindow("Inventory");
                            SpawnWindowBlueprint("Inventory");
                        },
                        (h) => () =>
                        {
                            SetAnchor(Center);
                            PrintItemTooltip(item);
                        }
                    );
                else
                    AddHeaderRegion(
                        () =>
                        {
                            AddLine(slot, "DarkGray", "Right");
                            AddSmallButton("OtherEmpty", (h) => { });
                        }
                    );
            }
        }),
        new("SpellbookAbilityListHeader", () => {
            SetAnchor(TopRight);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(161);
            AddHeaderRegion(
                () =>
                {
                    AddLine("Spellbook:");
                    AddSmallButton("OtherClose", (h) =>
                    {
                        CloseDesktop("SpellbookScreen");
                        SwitchDesktop("Map");
                        PlaySound("DesktopSpellbookScreenClose");
                    });
                }
            );
        }, true),
        new("SpellbookAbilityList", () => {
            SetAnchor(147, 0);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(1460);
            AddHeaderRegion(() => { AddLine(); });
            AddPaddingRegion(() => { AddLine("Active abilities:", "DarkGray"); });
            var activeAbilities = Ability.abilities.FindAll(x => x.cost != null && currentSave.player.abilities.Contains(x.name)).ToList();
            var passiveAbilities = Ability.abilities.FindAll(x => x.cost == null && currentSave.player.abilities.Contains(x.name)).ToList();
            for (int i = 0; i < activeAbilities.Count; i++)
            {
                var abilityObj = activeAbilities[i];
                AddButtonRegion(
                    () =>
                    {
                        AddLine(abilityObj.name, "", "Right");
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
                            PlaySound("DesktopActionbarAdd", 0.7f);
                        }
                    },
                    (h) => () =>
                    {
                        SetAnchor(Top, 0, -42);
                        AddHeaderGroup();
                        SetRegionGroupWidth(236);
                        SetRegionGroupHeight(217);
                        AddHeaderRegion(() =>
                        {
                            AddLine(abilityObj.name, "Gray");
                        });
                        AddPaddingRegion(() =>
                        {
                            AddBigButton(abilityObj.icon, (h) => { });
                            AddLine("Cooldown: ", "DarkGray");
                            AddText(abilityObj.cooldown == 0 ? "None" : abilityObj.cooldown + (abilityObj.cooldown == 1 ? " turn"  : " turns"), "Gray");
                        });
                        abilityObj.PrintDescription(currentSave.player, null, 236);
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
                                AddLine(cost.Value + "", cost.Value > currentSave.player.MaxResource(cost.Key) ? "Red" : "Gray");
                            });
                        }
                        AddRegionGroup();
                        SetRegionGroupWidth(236 - abilityObj.cost.Count * 49);
                        AddPaddingRegion(() => { AddLine(); });
                    }
                );
            }
            if (passiveAbilities.Count(x => x.description != null) > 0)
                AddPaddingRegion(() => { AddLine("Passive abilities:", "DarkGray"); });
            for (int i = 0; i < passiveAbilities.Count; i++)
            {
                var abilityObj = passiveAbilities[i];
                if (abilityObj.description == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(abilityObj.name, "Black");
                        AddSmallButton(abilityObj.icon,
                        (h) =>
                        {

                        });
                    },
                    (h) => { },
                    (h) => () =>
                    {
                        PrintAbilityTooltip(currentSave.player, null, abilityObj);
                    }
                );
            }
            AddPaddingRegion(() => { });
        }),
        new("PlayerSpellbookInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
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
                            SetRegionGroupWidth(83);
                            AddHeaderRegion(() => { AddLine(element + ":", "Gray"); });
                            AddPaddingRegion(() => { AddLine(currentSave.player.resources.ToList().Find(x => x.Key == element).Value + "/" + currentSave.player.MaxResource(element), "Gray"); });
                        }
                    );
                });
            AddRegionGroup();
            SetRegionGroupWidth(76);
            foreach (var element in elements1)
                AddHeaderRegion(() =>
                {
                    var value = currentSave.player.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value > currentSave.player.MaxResource(element) ? "Red" : "Green"));
                    AddText("/" + currentSave.player.MaxResource(element), "DarkGray");
                    AddSmallButton("Element" + elements2[elements1.IndexOf(element)] + "Rousing",
                        (h) => { },
                        (h) => () =>
                        {
                            SetAnchor(Top, h.window);
                            AddRegionGroup();
                            SetRegionGroupWidth(83);
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
            SetRegionGroupWidth(56);
            foreach (var element in elements2)
                AddHeaderRegion(() =>
                {
                    var value = currentSave.player.resources.ToList().Find(x => x.Key == element).Value;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value > currentSave.player.MaxResource(element) ? "Red" : "Green"));
                    AddText("/" + currentSave.player.MaxResource(element), "DarkGray");
                });
        }, true),
        new("EnemyBattleInfo", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            AddButtonRegion(
                () =>
                {
                    AddLine(Board.board.enemy.name, "Black");
                    AddSmallButton("OtherSettings", (h) => { SpawnWindowBlueprint("TitleScreenMenu"); });
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                var race = races.Find(x => x.name == Board.board.enemy.race);
                AddBigButton(race.portrait == "" ? "OtherUnknown" : race.portrait, (h) => { });
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
            SetRegionGroupWidth(286);
            AddHeaderRegion(
                () =>
                {
                    AddLine(area.name, "", "Center");
                }
            );
        }),
        new("BattleBoard", () => {
            SetAnchor(Top, 0, -34 + 19 * (Board.board.field.GetLength(1) - 7));
            var boardBackground = new GameObject("BoardBackground", typeof(SpriteRenderer), typeof(SpriteMask));
            boardBackground.transform.parent = CDesktop.LBWindow.transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/BoardBackground" + Board.board.field.GetLength(0) + "x" + Board.board.field.GetLength(1));
            boardBackground.GetComponent<SpriteMask>().sprite = Resources.Load<Sprite>("Sprites/Textures/BoardMask" + Board.board.field.GetLength(0) + "x" + Board.board.field.GetLength(1));
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
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
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
            AddButtonRegion(
                () =>
                {
                    AddLine("Character Sheet", "", "Right");
                    AddSmallButton("MenuCharacterSheet", (h) => { });
                },
                (h) =>
                {
                    SpawnDesktopBlueprint("CharacterSheet");
                    SwitchDesktop("CharacterSheet");
                    PlaySound("DesktopCharacterSheetOpen");
                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("Inventory", "", "Right");
                    AddSmallButton("MenuInventory", (h) => { });
                },
                (h) =>
                {
                    SpawnDesktopBlueprint("EquipmentScreen");
                    SwitchDesktop("EquipmentScreen");
                }
            );
            AddButtonRegion(
                () =>
                {
                    AddLine("Spellbook", "", "Right");
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
                    AddLine("Talents", "", "Right");
                    AddSmallButton("MenuTalents", (h) => { });
                },
                (h) =>
                {
                    SpawnDesktopBlueprint("TalentScreen");
                    SwitchDesktop("TalentScreen");
                }
            );
            AddPaddingRegion(() => { });
        }, true),
        new("InstanceLeftSide", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
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
            SetRegionGroupWidth(161);
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
            SetRegionGroupWidth(161);
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
            SetRegionGroupWidth(161);
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
            SetRegionGroupHeight(354);
            var items = currentSave.player.inventory.items;
            AddHeaderRegion(() =>
            {
                AddLine("Inventory:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseDesktop("EquipmentScreen");
                    SwitchDesktop("Map");
                    PlaySound("DesktopInventoryClose");
                });
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.player.inventory.items.Reverse();
                    CloseWindow("Inventory");
                    SpawnWindowBlueprint("Inventory");
                    PlaySound("DesktopInventorySort");
                });
                if (!CDesktop.windows.Exists(x => x.title == "InventorySettings") && !CDesktop.windows.Exists(x => x.title == "InventorySort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("InventorySort");
                        CloseWindow("Inventory");
                        SpawnWindowBlueprint("Inventory");
                    });
                else
                    AddSmallButton("OtherSortOff", (h) => { });
                if (!CDesktop.windows.Exists(x => x.title == "InventorySettings") && !CDesktop.windows.Exists(x => x.title == "InventorySort"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("InventorySettings");
                        CloseWindow("Inventory");
                        SpawnWindowBlueprint("Inventory");
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
        new("InventorySort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddLine("Sort inventory:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("InventorySort");
                    CloseWindow("Inventory");
                    SpawnWindowBlueprint("Inventory");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderBy(x => x.name).ToList();
                CloseWindow("Inventory");
                CloseWindow("InventorySort");
                SpawnWindowBlueprint("Inventory");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By item power", "Black");
            },
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.ilvl).ToList();
                CloseWindow("Inventory");
                CloseWindow("InventorySort");
                SpawnWindowBlueprint("Inventory");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By price", "Black");
            },
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.price).ToList();
                CloseWindow("Inventory");
                CloseWindow("InventorySort");
                SpawnWindowBlueprint("Inventory");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By type", "Black");
            },
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.type).ToList();
                CloseWindow("Inventory");
                CloseWindow("InventorySort");
                SpawnWindowBlueprint("Inventory");
                PlaySound("DesktopInventorySort");
            });
        }),
        new("InventorySettings", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddLine("Inventory settings:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("InventorySettings");
                    CloseWindow("Inventory");
                    SpawnWindowBlueprint("Inventory");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Rarity indicators", "Black");
                AddCheckbox(settings.rarityIndicators);
            },
            (h) =>
            {
                CloseWindow("Inventory");
                SpawnWindowBlueprint("Inventory");
                CloseWindow("InventorySettings");
                SpawnWindowBlueprint("InventorySettings");
            });
            if (settings.rarityIndicators.Value())
                AddButtonRegion(() =>
                {
                    AddLine("Big Rarity indicators", "Black");
                    AddCheckbox(settings.bigRarityIndicators);
                },
                (h) =>
                {
                    CloseWindow("Inventory");
                    SpawnWindowBlueprint("Inventory");
                });
            AddButtonRegion(() =>
            {
                AddLine("Upgrade indicators", "Black");
                AddCheckbox(settings.upgradeIndicators);
            },
            (h) =>
            {
                CloseWindow("Inventory");
                SpawnWindowBlueprint("Inventory");
            });
            AddButtonRegion(() =>
            {
                AddLine("New slot indicators", "Black");
                AddCheckbox(settings.newSlotIndicators);
            },
            (h) =>
            {
                CloseWindow("Inventory");
                SpawnWindowBlueprint("Inventory");
            });
        }),
        new("ItemDrop", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            SetRegionGroupWidth(256);
            AddHeaderRegion(() =>
            {
                AddLine("Loot from ", "Gray");
                AddText("Chief Ukorz Sandscalp", "Gray");
            });
            var item = Item.items[random.Next(Item.items.Count)];
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
                var races = Race.races.FindAll(x => x.faction == creationFaction || x.faction == "Both");
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
                    AddBigButton("PortraitPandaren" + creationGender, (h) => { creationRace = "Pandaren"; creationClass = null; });
                    if (creationRace != "Pandaren") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
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
                    AddBigButton("PortraitPandaren" + creationGender, (h) => { creationRace = "Pandaren"; creationClass = null; });
                    if (creationRace != "Pandaren") { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                }
            });
            AddHeaderRegion(() =>
            {
                AddLine("Class: " + creationClass);
                AddSmallButton("ActionReroll", (h) =>
                {
                    var classes = specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace));
                    creationClass = classes[random.Next(classes.Count)].name;
                });
            });
            AddHeaderRegion(() =>
            {
                if (creationRace != null)
                {
                    var classes = specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace));
                    foreach (var foo in classes)
                    {
                        AddBigButton(foo.icon, (h) => { creationClass = foo.name; });
                        if (creationClass != foo.name) { AddBigButtonOverlay("OtherGridBlurred"); SetBigButtonToGrayscale(); }
                    }
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
                            SetRegionGroupWidth(83);
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
            SetRegionGroupWidth(39);
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
                            SetRegionGroupWidth(83);
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
            SetRegionGroupWidth(20);
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
                            SetRegionGroupWidth(83);
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
            SetRegionGroupWidth(39);
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
                            SetRegionGroupWidth(83);
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
            SetRegionGroupWidth(20);
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
            SetAnchor(Top);
            AddRegionGroup();
            SetRegionGroupWidth(628);
            AddInputRegion(String.consoleInput, InputType.Everything);
            AddSmallButton("OtherClose", (h) => { CloseWindow(h.window); });
        },  true),

        #region Dev Panel

        new("ObjectManagerLobby", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(354);
            AddHeaderRegion(() => { AddLine("Object types:"); });
            AddButtonRegion(() => { AddLine("Hostile areas"); }, (h) => { SpawnDesktopBlueprint("ObjectManagerHostileAreas"); });
            AddButtonRegion(() => { AddLine("Instances"); }, (h) => { SpawnDesktopBlueprint("ObjectManagerInstances"); });
            AddButtonRegion(() => { AddLine("Complexes"); }, (h) => { SpawnDesktopBlueprint("ObjectManagerComplexes"); });
            AddButtonRegion(() => { AddLine("Races"); }, (h) => { SpawnDesktopBlueprint("ObjectManagerRaces"); });
            AddButtonRegion(() => { AddLine("Classes"); }, (h) => { SpawnDesktopBlueprint("ObjectManagerClasses"); });
            AddButtonRegion(() => { AddLine("Abilities"); }, (h) => { SpawnDesktopBlueprint("ObjectManagerAbilities"); });
            AddButtonRegion(() => { AddLine("Buffs"); }, (h) => { SpawnDesktopBlueprint("ObjectManagerBuffs"); });
            AddButtonRegion(() => { AddLine("Items"); }, (h) => { SpawnDesktopBlueprint("ObjectManagerItems"); });
            AddButtonRegion(() => { AddLine("Item sets"); }, (h) => { SpawnDesktopBlueprint("ObjectManagerItemSets"); });
            AddPaddingRegion(() => { AddLine("Actions:"); });
            AddButtonRegion(() => { AddLine("Save data"); }, (h) =>
            {
                Serialize(races, "races");
                Serialize(specs, "classes");
                Serialize(abilities, "abilities");
                Serialize(buffs, "buffs");
                Serialize(areas, "areas");
                Serialize(instances, "instances");
                Serialize(complexes, "complexes");
                Serialize(towns, "towns");
                Serialize(items, "items");
                Serialize(itemSets, "sets");
            });
            AddPaddingRegion(() => { });
        }),
        new("HostileAreasSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddLine("Sort hostile areas:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("HostileAreasSort");
                    CloseWindow("ObjectManagerHostileAreas");
                    SpawnWindowBlueprint("ObjectManagerHostileAreas");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                areas = areas.OrderBy(x => x.name).ToList();
                CloseWindow("HostileAreasSort");
                CloseWindow("ObjectManagerHostileAreas");
                SpawnWindowBlueprint("ObjectManagerHostileAreas");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By zone", "Black");
            },
            (h) =>
            {
                areas = areas.OrderBy(x => x.zone).ToList();
                CloseWindow("HostileAreasSort");
                CloseWindow("ObjectManagerHostileAreas");
                SpawnWindowBlueprint("ObjectManagerHostileAreas");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By type", "Black");
            },
            (h) =>
            {
                areas = areas.OrderBy(x => x.type).ToList();
                CloseWindow("HostileAreasSort");
                CloseWindow("ObjectManagerHostileAreas");
                SpawnWindowBlueprint("ObjectManagerHostileAreas");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By encounter amount", "Black");
            },
            (h) =>
            {
                areas = areas.OrderByDescending(x => x.possibleEncounters == null ? -1 : x.possibleEncounters.Count).ToList();
                CloseWindow("HostileAreasSort");
                CloseWindow("ObjectManagerHostileAreas");
                SpawnWindowBlueprint("ObjectManagerHostileAreas");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By boss amount", "Black");
            },
            (h) =>
            {
                areas = areas.OrderByDescending(x => x.bossEncounters == null ? -1 : x.bossEncounters.Count).ToList();
                CloseWindow("HostileAreasSort");
                CloseWindow("ObjectManagerHostileAreas");
                SpawnWindowBlueprint("ObjectManagerHostileAreas");
                PlaySound("DesktopInventorySort");
            });
        }),
        new("ObjectManagerHostileAreas", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Areas:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerHostileAreas"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    areas.Reverse();
                    CloseWindow("ObjectManagerHostileAreas");
                    SpawnWindowBlueprint("ObjectManagerHostileAreas");
                    PlaySound("DesktopInventorySort");
                });
                if (!CDesktop.windows.Exists(x => x.title == "HostileAreasSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("HostileAreasSort");
                        CloseWindow("ObjectManagerHostileAreas");
                        SpawnWindowBlueprint("ObjectManagerHostileAreas");
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
            var max = Math.Ceiling(areas.Count / 10.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (areas.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = areas[index + 10 * regionGroup.pagination];
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
                    area = areas[index + 10 * regionGroup.pagination];
                    instance = null;
                    complex = null;
                    SetDesktopBackground("Areas/Area" + area.zone + area.name.Replace(".", "").Replace(" ", "").Replace("\'", ""));
                    CloseWindow("ObjectManagerHostileArea");
                    SpawnWindowBlueprint("ObjectManagerHostileArea");
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(areas.Count + " hostile areas", "DarkGray");
            });
        }),
        new("ObjectManagerHostileArea", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(161);
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
                AddLine(area.ambience == null ? "None" : area.ambience.Replace("Ambience", "") + ".ogg");
                if (area.ambience != "None")
                    AddSmallButton("OtherSound", (h) => { PlayAmbience(area.ambience); });
            },
            (h) =>
            {
                CloseWindow("ObjectManagerHostileAreaTypeList");
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerAmbienceList"))
                {
                    CloseWindow("ObjectManagerHostileAreas");
                    SpawnWindowBlueprint("ObjectManagerAmbienceList");
                }
            });
            AddPaddingRegion(() => { });
        }),
        new("InstancesSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddLine("Sort instances:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("InstancesSort");
                    CloseWindow("ObjectManagerInstances");
                    SpawnWindowBlueprint("ObjectManagerInstances");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                instances = instances.OrderBy(x => x.name).ToList();
                CloseWindow("InstancesSort");
                CloseWindow("ObjectManagerInstances");
                SpawnWindowBlueprint("ObjectManagerInstances");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By zone", "Black");
            },
            (h) =>
            {
                instances = instances.OrderBy(x => x.zone).ToList();
                CloseWindow("InstancesSort");
                CloseWindow("ObjectManagerInstances");
                SpawnWindowBlueprint("ObjectManagerInstances");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By type", "Black");
            },
            (h) =>
            {
                instances = instances.OrderBy(x => x.type).ToList();
                CloseWindow("InstancesSort");
                CloseWindow("ObjectManagerInstances");
                SpawnWindowBlueprint("ObjectManagerInstances");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By area amount", "Black");
            },
            (h) =>
            {
                instances = instances.OrderByDescending(x => x.wings.Sum(y => y.areas.Count)).ToList();
                CloseWindow("InstancesSort");
                CloseWindow("ObjectManagerInstances");
                SpawnWindowBlueprint("ObjectManagerInstances");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By wing amount", "Black");
            },
            (h) =>
            {
                instances = instances.OrderByDescending(x => x.wings.Count).ToList();
                CloseWindow("InstancesSort");
                CloseWindow("ObjectManagerInstances");
                SpawnWindowBlueprint("ObjectManagerInstances");
                PlaySound("DesktopInventorySort");
            });
        }),
        new("ObjectManagerInstances", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Instances:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerInstances"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    instances.Reverse();
                    CloseWindow("ObjectManagerInstances");
                    SpawnWindowBlueprint("ObjectManagerInstances");
                    PlaySound("DesktopInventorySort");
                });
                if (!CDesktop.windows.Exists(x => x.title == "InstancesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("InstancesSort");
                        CloseWindow("ObjectManagerInstances");
                        SpawnWindowBlueprint("ObjectManagerInstances");
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
            var max = Math.Ceiling(instances.Count / 10.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (instances.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = instances[index + 10 * regionGroup.pagination];
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
                    instance = instances[index + 10 * regionGroup.pagination];
                    complex = null;
                    SetDesktopBackground("Areas/Area" + instance.name.Replace(".", "").Replace(" ", "").Replace("\'", ""));
                    CloseWindow("ObjectManagerInstance");
                    SpawnWindowBlueprint("ObjectManagerInstance");
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(instances.Count + " instances", "DarkGray");
            });
        }),
        new("ObjectManagerInstance", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Instance:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(instance.name); });
            AddPaddingRegion(() => { AddLine("Zone:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(instance.zone); });
            AddPaddingRegion(() => { AddLine("Ambience:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(instance.ambience == null ? "None" : instance.ambience.Replace("Ambience", "") + ".ogg");
                if (instance.ambience != "None")
                    AddSmallButton("OtherSound", (h) => { PlayAmbience(instance.ambience); });
            },
            (h) =>
            {
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerAmbienceList"))
                {
                    CloseWindow("ObjectManagerInstances");
                    SpawnWindowBlueprint("ObjectManagerAmbienceList");
                }
            });
            AddPaddingRegion(() => { });
        }),
        new("ComplexesSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(152);
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
                CloseWindow("ComplexesSort");
                CloseWindow("ObjectManagerComplexes");
                SpawnWindowBlueprint("ObjectManagerComplexes");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By zone", "Black");
            },
            (h) =>
            {
                complexes = complexes.OrderBy(x => x.zone).ToList();
                CloseWindow("ComplexesSort");
                CloseWindow("ObjectManagerComplexes");
                SpawnWindowBlueprint("ObjectManagerComplexes");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By site amount", "Black");
            },
            (h) =>
            {
                complexes = complexes.OrderByDescending(x => x.sites.Count).ToList();
                CloseWindow("ComplexesSort");
                CloseWindow("ObjectManagerComplexes");
                SpawnWindowBlueprint("ObjectManagerComplexes");
                PlaySound("DesktopInventorySort");
            });
        }),
        new("ObjectManagerComplexes", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Complexes:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerComplexes"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    complexes.Reverse();
                    CloseWindow("ObjectManagerComplexes");
                    SpawnWindowBlueprint("ObjectManagerComplexes");
                    PlaySound("DesktopInventorySort");
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
            var max = Math.Ceiling(complexes.Count / 10.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (complexes.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = complexes[index + 10 * regionGroup.pagination];
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
                    complex = complexes[index + 10 * regionGroup.pagination];
                    SetDesktopBackground("Areas/Complex" + complex.name.Replace(".", "").Replace(" ", "").Replace("\'", ""));
                    CloseWindow("ObjectManagerComplex");
                    SpawnWindowBlueprint("ObjectManagerComplex");
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(complexes.Count + " complexes", "DarkGray");
            });
        }),
        new("ObjectManagerComplex", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Complex:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(complex.name); });
            AddPaddingRegion(() => { AddLine("Zone:", "DarkGray"); });
            AddHeaderRegion(() => { AddLine(complex.zone); });
            AddPaddingRegion(() => { AddLine("Ambience:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(complex.ambience == null ? "None" : complex.ambience.Replace("Ambience", "") + ".ogg");
                if (complex.ambience != "None")
                    AddSmallButton("OtherSound", (h) => { PlayAmbience(complex.ambience); });
            },
            (h) =>
            {
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerAmbienceList"))
                {
                    CloseWindow("ObjectManagerComplexes");
                    SpawnWindowBlueprint("ObjectManagerAmbienceList");
                }
            });
            AddPaddingRegion(() => { });
        }),
        new("ObjectManagerAmbienceList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
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
                    Assets.assets.ambience.Reverse();
                    CloseWindow("ObjectManagerAmbienceList");
                    SpawnWindowBlueprint("ObjectManagerAmbienceList");
                    PlaySound("DesktopInventorySort");
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search, InputType.Everything);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            var max = Math.Ceiling(Assets.assets.ambience.Count / 10.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.ambience.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = Assets.assets.ambience[index + 10 * regionGroup.pagination];
                        AddLine(foo);
                        AddSmallButton("OtherSound", (h) => { PlayAmbience("Ambience" + foo.Replace(".ogg", "")); });
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
                        area.ambience = "Ambience" + foo.Replace(".ogg", "");
                        CloseWindow("ObjectManagerHostileArea");
                        SpawnWindowBlueprint("ObjectManagerHostileArea");
                        SpawnWindowBlueprint("ObjectManagerHostileAreas");
                    }
                    else if (instance != null)
                    {
                        instance.ambience = "Ambience" + foo.Replace(".ogg", "");
                        CloseWindow("ObjectManagerInstance");
                        SpawnWindowBlueprint("ObjectManagerInstance");
                        SpawnWindowBlueprint("ObjectManagerInstances");
                    }
                    else if (complex != null)
                    {
                        complex.ambience = "Ambience" + foo.Replace(".ogg", "");
                        CloseWindow("ObjectManagerComplex");
                        SpawnWindowBlueprint("ObjectManagerComplex");
                        SpawnWindowBlueprint("ObjectManagerComplexes");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(Assets.assets.ambience.Count + " ambience tracks", "DarkGray");
            });
        }),
        new("ObjectManagerIconList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Icons:");
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
                    Assets.assets.icons.Reverse();
                    CloseWindow("ObjectManagerIconList");
                    SpawnWindowBlueprint("ObjectManagerIconList");
                    PlaySound("DesktopInventorySort");
                });
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            var max = Math.Ceiling(Assets.assets.icons.Count / 10.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.icons.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = Assets.assets.icons[index + 10 * regionGroup.pagination];
                        AddLine(foo);
                        AddSmallButton(Assets.assets.icons[index + 10 * regionGroup.pagination].Replace(".png", ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.icons[index + 10 * regionGroup.pagination];
                    CloseWindow("ObjectManagerIconList");
                    if (item != null)
                    {
                        item.icon = foo.Replace(".png", "");
                        CloseWindow("ObjectManagerItem");
                        SpawnWindowBlueprint("ObjectManagerItem");
                        SpawnWindowBlueprint("ObjectManagerItems");
                    }
                    else if (ability != null)
                    {
                        ability.icon = foo.Replace(".png", "");
                        CloseWindow("ObjectManagerAbility");
                        SpawnWindowBlueprint("ObjectManagerAbility");
                        SpawnWindowBlueprint("ObjectManagerAbilities");
                    }
                    else if (buff != null)
                    {
                        buff.icon = foo.Replace(".png", "");
                        CloseWindow("ObjectManagerBuff");
                        SpawnWindowBlueprint("ObjectManagerBuff");
                        SpawnWindowBlueprint("ObjectManagerBuffs");
                    }
                    else if (spec != null)
                    {
                        spec.icon = foo.Replace(".png", "");
                        CloseWindow("ObjectManagerClass");
                        SpawnWindowBlueprint("ObjectManagerClass");
                        SpawnWindowBlueprint("ObjectManagerClasses");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(Assets.assets.icons.Count + " icons", "DarkGray");
            });
        }),
        new("ObjectManagerPortraitList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
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
                    Assets.assets.portraits.Reverse();
                    CloseWindow("ObjectManagerPortraitList");
                    SpawnWindowBlueprint("ObjectManagerPortraitList");
                    PlaySound("DesktopInventorySort");
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search, InputType.Everything);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            var max = Math.Ceiling(Assets.assets.portraits.Count / 10.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (Assets.assets.portraits.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = Assets.assets.portraits[index + 10 * regionGroup.pagination];
                        AddLine(foo);
                        AddSmallButton(Assets.assets.portraits[index + 10 * regionGroup.pagination].Replace(".png", ""), (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    var foo = Assets.assets.portraits[index + 10 * regionGroup.pagination];
                    CloseWindow("ObjectManagerPortraitList");
                    if (race != null)
                    {
                        race.portrait = foo.Replace(".png", "");
                        CloseWindow("ObjectManagerRace");
                        SpawnWindowBlueprint("ObjectManagerRace");
                        SpawnWindowBlueprint("ObjectManagerRaces");
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(Assets.assets.portraits.Count + " portraits", "DarkGray");
            });
        }),
        new("ObjectManagerHostileAreaTypeList", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
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
            var max = 1;
            AddPaginationLine(regionGroup, max);
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
                    CloseWindow("ObjectManagerHostileArea");
                    SpawnWindowBlueprint("ObjectManagerHostileArea");
                    CloseWindow("ObjectManagerHostileAreas");
                    SpawnWindowBlueprint("ObjectManagerHostileAreas");
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
                    CloseWindow("ObjectManagerHostileArea");
                    SpawnWindowBlueprint("ObjectManagerHostileArea");
                    CloseWindow("ObjectManagerHostileAreas");
                    SpawnWindowBlueprint("ObjectManagerHostileAreas");
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
            SetRegionGroupWidth(161);
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
            var max = 1;
            AddPaginationLine(regionGroup, max);
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
                    CloseWindow("ObjectManagerItem");
                    SpawnWindowBlueprint("ObjectManagerItem");
                    CloseWindow("ObjectManagerItems");
                    SpawnWindowBlueprint("ObjectManagerItems");
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
                    CloseWindow("ObjectManagerItem");
                    SpawnWindowBlueprint("ObjectManagerItem");
                    CloseWindow("ObjectManagerItems");
                    SpawnWindowBlueprint("ObjectManagerItems");
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
                    CloseWindow("ObjectManagerItem");
                    SpawnWindowBlueprint("ObjectManagerItem");
                    CloseWindow("ObjectManagerItems");
                    SpawnWindowBlueprint("ObjectManagerItems");
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
                    CloseWindow("ObjectManagerItem");
                    SpawnWindowBlueprint("ObjectManagerItem");
                    CloseWindow("ObjectManagerItems");
                    SpawnWindowBlueprint("ObjectManagerItems");
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
                    CloseWindow("ObjectManagerItem");
                    SpawnWindowBlueprint("ObjectManagerItem");
                    CloseWindow("ObjectManagerItems");
                    SpawnWindowBlueprint("ObjectManagerItems");
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
                    CloseWindow("ObjectManagerItem");
                    SpawnWindowBlueprint("ObjectManagerItem");
                    CloseWindow("ObjectManagerItems");
                    SpawnWindowBlueprint("ObjectManagerItems");
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
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddLine("Sort items:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("ItemsSort");
                    CloseWindow("ObjectManagerItems");
                    SpawnWindowBlueprint("ObjectManagerItems");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                items = items.OrderBy(x => x.name).ToList();
                CloseWindow("ItemsSort");
                CloseWindow("ObjectManagerItems");
                SpawnWindowBlueprint("ObjectManagerItems");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By item power", "Black");
            },
            (h) =>
            {
                items = items.OrderByDescending(x => x.ilvl).ToList();
                CloseWindow("ItemsSort");
                CloseWindow("ObjectManagerItems");
                SpawnWindowBlueprint("ObjectManagerItems");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By price", "Black");
            },
            (h) =>
            {
                items = items.OrderByDescending(x => x.price).ToList();
                CloseWindow("ItemsSort");
                CloseWindow("ObjectManagerItems");
                SpawnWindowBlueprint("ObjectManagerItems");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By type", "Black");
            },
            (h) =>
            {
                items = items.OrderByDescending(x => x.type).ToList();
                CloseWindow("ItemsSort");
                CloseWindow("ObjectManagerItems");
                SpawnWindowBlueprint("ObjectManagerItems");
                PlaySound("DesktopInventorySort");
            });
        }),
        new("ObjectManagerItems", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Items:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerItems"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    items.Reverse();
                    CloseWindow("ObjectManagerItems");
                    SpawnWindowBlueprint("ObjectManagerItems");
                    PlaySound("DesktopInventorySort");
                });
                if (!CDesktop.windows.Exists(x => x.title == "ItemsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("ItemsSort");
                        CloseWindow("ObjectManagerItems");
                        SpawnWindowBlueprint("ObjectManagerItems");
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
            var max = Math.Ceiling(items.Count / 10.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (items.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = items[index + 10 * regionGroup.pagination];
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
                    item = items[index + 10 * regionGroup.pagination];
                    String.objectName.Set(item.name);
                    String.price.Set(item.price + "");
                    String.itemPower.Set(item.ilvl + "");
                    String.requiredLevel.Set(item.lvl + "");
                    CloseWindow("ObjectManagerItem");
                    SpawnWindowBlueprint("ObjectManagerItem");
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
                String.objectName.Set(item.name);
                String.price.Set(item.price + "");
                String.itemPower.Set(item.ilvl + "");
                String.requiredLevel.Set(item.lvl + "");
                CloseWindow("ObjectManagerItem");
                SpawnWindowBlueprint("ObjectManagerItem");
                h.window.Rebuild();
            });
        }),
        new("ObjectManagerItem", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Item:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything, item.rarity);
            AddPaddingRegion(() => { AddLine("Icon:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(item.icon + ".png");
                AddSmallButton(item.icon, (h) => { });
            },
            (h) =>
            {
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerIconList"))
                {
                    CloseWindow("ObjectManagerItems");
                    SpawnWindowBlueprint("ObjectManagerIconList");
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
            AddPaddingRegion(() => { AddLine("Price:", "DarkGray"); });
            AddInputRegion(String.price, InputType.Decimal);
            AddPaddingRegion(() => { AddLine("Item power:", "DarkGray"); });
            AddInputRegion(String.itemPower, InputType.Numbers);
            AddPaddingRegion(() => { AddLine("Required level:", "DarkGray"); });
            AddInputRegion(String.requiredLevel, InputType.Numbers);
            AddPaddingRegion(() => { });
        }),
        new("ItemSetsSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddLine("Sort item sets:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("ItemsSort");
                    CloseWindow("ObjectManagerItemSets");
                    SpawnWindowBlueprint("ObjectManagerItemSets");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                ItemSet.itemSets = ItemSet.itemSets.OrderBy(x => x.name).ToList();
                CloseWindow("ItemSetsSort");
                CloseWindow("ObjectManagerItemSets");
                SpawnWindowBlueprint("ObjectManagerItemSets");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By amount of tiers", "Black");
            },
            (h) =>
            {
                ItemSet.itemSets = ItemSet.itemSets.OrderByDescending(x => x.setBonuses.Count).ToList();
                CloseWindow("ItemSetsSort");
                CloseWindow("ObjectManagerItemSets");
                SpawnWindowBlueprint("ObjectManagerItemSets");
                PlaySound("DesktopInventorySort");
            });
        }),
        new("ObjectManagerItemSets", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Item sets:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerItemSets"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    itemSets.Reverse();
                    CloseWindow("ObjectManagerItemSets");
                    SpawnWindowBlueprint("ObjectManagerItemSets");
                    PlaySound("DesktopInventorySort");
                });
                if (!CDesktop.windows.Exists(x => x.title == "ItemSetsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("ItemSetsSort");
                        CloseWindow("ObjectManagerItemSets");
                        SpawnWindowBlueprint("ObjectManagerItemSets");
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
            var max = Math.Ceiling(itemSets.Count / 5.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 5; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (itemSets.Count > index + 5 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = itemSets[index + 5 * regionGroup.pagination];
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
                    itemSet = itemSets[index + 5 * regionGroup.pagination];
                    String.objectName.Set(itemSet.name);
                    CloseWindow("ObjectManagerItemSet");
                    SpawnWindowBlueprint("ObjectManagerItemSet");
                });
                AddPaddingRegion(() =>
                {
                    AddLine();
                    if (itemSets.Count > index + 5 * regionGroup.pagination)
                    {
                        var foo = itemSets[index + 5 * regionGroup.pagination];
                        var setItems = items.FindAll(x => x.set == foo.name);
                        for (var j = 0; j < setItems.Count && j < 9; j++)
                        {
                            var J = j;
                            AddSmallButton(setItems[J].icon, (h) => { });
                            AddSmallButtonOverlay("OtherRarity" + setItems[J].rarity + "Big");
                        }
                    }
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(itemSets.Count + " item sets", "DarkGray");
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
                String.objectName.Set(itemSet.name);
                CloseWindow("ObjectManagerItemSet");
                SpawnWindowBlueprint("ObjectManagerItemSet");
                h.window.Rebuild();
            });
        }),
        new("ObjectManagerItemSet", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Item set:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything);
            AddPaddingRegion(() => { });
        }),
        new("AbilitiesSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddLine("Sort abilities:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("AbilitiesSort");
                    CloseWindow("ObjectManagerAbilities");
                    SpawnWindowBlueprint("ObjectManagerAbilities");
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
                CloseWindow("ObjectManagerAbilities");
                SpawnWindowBlueprint("ObjectManagerAbilities");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By cost", "Black");
            },
            (h) =>
            {
                abilities = abilities.OrderByDescending(x => x.cost.Sum(y => y.Value)).ToList();
                CloseWindow("AbilitiesSort");
                CloseWindow("ObjectManagerAbilities");
                SpawnWindowBlueprint("ObjectManagerAbilities");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By cooldown", "Black");
            },
            (h) =>
            {
                abilities = abilities.OrderByDescending(x => x.cooldown).ToList();
                CloseWindow("AbilitiesSort");
                CloseWindow("ObjectManagerAbilities");
                SpawnWindowBlueprint("ObjectManagerAbilities");
                PlaySound("DesktopInventorySort");
            });
        }),
        new("ObjectManagerAbilities", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Abilities:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerAbilities"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    abilities.Reverse();
                    CloseWindow("ObjectManagerAbilities");
                    SpawnWindowBlueprint("ObjectManagerAbilities");
                    PlaySound("DesktopInventorySort");
                });
                if (!CDesktop.windows.Exists(x => x.title == "AbilitiesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("AbilitiesSort");
                        CloseWindow("ObjectManagerAbilities");
                        SpawnWindowBlueprint("ObjectManagerAbilities");
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
            var max = Math.Ceiling(abilities.Count / 10.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (abilities.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = abilities[index + 10 * regionGroup.pagination];
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
                    ability = abilities[index + 10 * regionGroup.pagination];
                    String.objectName.Set(ability.name);
                    String.cooldown.Set(ability.cooldown + "");
                    CloseWindow("ObjectManagerAbility");
                    SpawnWindowBlueprint("ObjectManagerAbility");
                },
                (h) => () =>
                {
                    SetAnchor(Center);
                    PrintAbilityTooltip(null, null, abilities[index + 10 * regionGroup.pagination]);
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(abilities.Count + " abilities", "DarkGray");
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
                    events = new(),
                    tags = new()
                };
                abilities.Add(ability);
                String.objectName.Set(ability.name);
                String.cooldown.Set(ability.cooldown + "");
                CloseWindow("ObjectManagerAbility");
                SpawnWindowBlueprint("ObjectManagerAbility");
                h.window.Rebuild();
            });
        }),
        new("ObjectManagerAbility", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Ability:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything, ability.name);
            AddPaddingRegion(() => { AddLine("Icon:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(ability.icon + ".png");
                AddSmallButton(ability.icon, (h) => { });
            },
            (h) =>
            {
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerIconList"))
                {
                    CloseWindow("ObjectManagerAbilities");
                    SpawnWindowBlueprint("ObjectManagerIconList");
                }
            });
            AddPaddingRegion(() => { });
        }),
        new("BuffsSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddLine("Sort buffs:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("BuffsSort");
                    CloseWindow("ObjectManagerBuffs");
                    SpawnWindowBlueprint("ObjectManagerBuffs");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                buffs = buffs.OrderBy(x => x.name).ToList();
                CloseWindow("BuffsSort");
                CloseWindow("ObjectManagerBuffs");
                SpawnWindowBlueprint("ObjectManagerBuffs");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By dispel type", "Black");
            },
            (h) =>
            {
                buffs = buffs.OrderByDescending(x => x.dispelType).ToList();
                CloseWindow("BuffsSort");
                CloseWindow("ObjectManagerBuffs");
                SpawnWindowBlueprint("ObjectManagerBuffs");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By stackable property", "Black");
            },
            (h) =>
            {
                buffs = buffs.OrderByDescending(x => x.stackable).ToList();
                CloseWindow("BuffsSort");
                CloseWindow("ObjectManagerBuffs");
                SpawnWindowBlueprint("ObjectManagerBuffs");
                PlaySound("DesktopInventorySort");
            });
        }),
        new("ObjectManagerBuffs", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Buffs:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerBuffs"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    buffs.Reverse();
                    CloseWindow("ObjectManagerBuffs");
                    SpawnWindowBlueprint("ObjectManagerBuffs");
                    PlaySound("DesktopInventorySort");
                });
                if (!CDesktop.windows.Exists(x => x.title == "BuffsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("BuffsSort");
                        CloseWindow("ObjectManagerBuffs");
                        SpawnWindowBlueprint("ObjectManagerBuffs");
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
            var max = Math.Ceiling(buffs.Count / 10.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (buffs.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = buffs[index + 10 * regionGroup.pagination];
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
                    buff = buffs[index + 10 * regionGroup.pagination];
                    String.objectName.Set(buff.name);
                    CloseWindow("ObjectManagerBuff");
                    SpawnWindowBlueprint("ObjectManagerBuff");
                },
                (h) => () =>
                {
                    SetAnchor(Center);
                    PrintBuffTooltip(null, null, (buffs[index + 10 * regionGroup.pagination], 0, null));
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(buffs.Count + " buffs", "DarkGray");
            });
            AddButtonRegion(() =>
            {
                AddLine("Create a new buff");
            },
            (h) =>
            {
                buff = new Buff()
                {
                    name = "Buff #" + buffs.Count,
                    events = new(),
                    tags = new()
                };
                buffs.Add(buff);
                String.objectName.Set(buff.name);
                CloseWindow("ObjectManagerBuff");
                SpawnWindowBlueprint("ObjectManagerBuff");
                h.window.Rebuild();
            });
        }),
        new("ObjectManagerBuff", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Buff:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything);
            AddPaddingRegion(() => { AddLine("Icon:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(buff.icon + ".png");
                AddSmallButton(buff.icon, (h) => { });
            },
            (h) =>
            {
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerIconList"))
                {
                    CloseWindow("ObjectManagerBuffs");
                    SpawnWindowBlueprint("ObjectManagerIconList");
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
            AddPaddingRegion(() => { });
        }),
        new("RacesSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddLine("Sort races:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("RacesSort");
                    CloseWindow("ObjectManagerRaces");
                    SpawnWindowBlueprint("ObjectManagerRaces");
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                races = races.OrderBy(x => x.name).ToList();
                CloseWindow("RacesSort");
                CloseWindow("ObjectManagerRaces");
                SpawnWindowBlueprint("ObjectManagerRaces");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By portrait", "Black");
            },
            (h) =>
            {
                races = races.OrderBy(x => x.portrait).ToList();
                CloseWindow("RacesSort");
                CloseWindow("ObjectManagerRaces");
                SpawnWindowBlueprint("ObjectManagerRaces");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By faction", "Black");
            },
            (h) =>
            {
                races = races.OrderByDescending(x => x.faction).ToList();
                CloseWindow("RacesSort");
                CloseWindow("ObjectManagerRaces");
                SpawnWindowBlueprint("ObjectManagerRaces");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By kind", "Black");
            },
            (h) =>
            {
                races = races.OrderBy(x => x.kind == "Elite" ? 0 : (x.kind == "Rare" ? 1 : 2)).ToList();
                CloseWindow("RacesSort");
                CloseWindow("ObjectManagerRaces");
                SpawnWindowBlueprint("ObjectManagerRaces");
                PlaySound("DesktopInventorySort");
            });
            AddButtonRegion(() =>
            {
                AddLine("By vitality", "Black");
            },
            (h) =>
            {
                races = races.OrderByDescending(x => x.vitality).ToList();
                CloseWindow("RacesSort");
                CloseWindow("ObjectManagerRaces");
                SpawnWindowBlueprint("ObjectManagerRaces");
                PlaySound("DesktopInventorySort");
            });
        }),
        new("ObjectManagerRaces", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Races:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerRaces"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    races.Reverse();
                    CloseWindow("ObjectManagerRaces");
                    SpawnWindowBlueprint("ObjectManagerRaces");
                    PlaySound("DesktopInventorySort");
                });
                if (!CDesktop.windows.Exists(x => x.title == "RacesSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("RacesSort");
                        CloseWindow("ObjectManagerRaces");
                        SpawnWindowBlueprint("ObjectManagerRaces");
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
            var max = Math.Ceiling(races.Count / 10.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (races.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = races[index + 10 * regionGroup.pagination];
                        AddLine(foo.name);
                        AddSmallButton(foo.faction != null ? foo.portrait + "Female" : foo.portrait, (h) => { });
                    }
                    else
                    {
                        SetRegionBackground(RegionBackgroundType.Padding);
                        AddLine();
                    }
                },
                (h) =>
                {
                    race = races[index + 10 * regionGroup.pagination];
                    String.objectName.Set(race.name);
                    String.vitality.Set(race.vitality + "");
                    CloseWindow("ObjectManagerRace");
                    SpawnWindowBlueprint("ObjectManagerRace");
                });
            }
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine(races.Count + " races", "DarkGray");
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
                String.objectName.Set(race.name);
                String.vitality.Set(race.vitality + "");
                CloseWindow("ObjectManagerRace");
                SpawnWindowBlueprint("ObjectManagerRace");
                h.window.Rebuild();
            });
        }),
        new("ObjectManagerRace", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Race:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything);
            if (race.faction != null)
            {
                AddPaddingRegion(() => { AddLine("Portraits:", "DarkGray"); });
                AddHeaderRegion(() =>
                {
                    AddLine(race.portrait + ".png");
                    AddSmallButton(race.portrait + "Female", (h) => { });
                    AddSmallButton(race.portrait + "Male", (h) => { });
                });
                AddPaddingRegion(() => { AddLine("Faction:", "DarkGray"); });
                AddHeaderRegion(() => { AddLine(race.faction); });
            }
            else if (race.faction == null)
            {
                AddPaddingRegion(() => { AddLine("Portrait:", "DarkGray"); });
                AddButtonRegion(() =>
                {
                    AddLine(race.portrait + ".png");
                    AddSmallButton(race.portrait, (h) => { });
                },
                (h) =>
                {
                    if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerPortraitList"))
                    {
                        CloseWindow("ObjectManagerRaces");
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
                AddPaddingRegion(() => { AddLine("Vitality:", "DarkGray"); });
                AddInputRegion(String.vitality, InputType.Decimal);
            }
            AddPaddingRegion(() => { });
        }),
        new("ObjectManagerClasses", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(358);
            AddHeaderRegion(() =>
            {
                AddLine("Classes:");
                AddSmallButton("OtherClose", (h) => { CloseDesktop("ObjectManagerClasses"); });
                AddSmallButton("OtherReverse", (h) =>
                {
                    specs.Reverse();
                    CloseWindow("ObjectManagerClasses");
                    SpawnWindowBlueprint("ObjectManagerClasses");
                    PlaySound("DesktopInventorySort");
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Search:", "DarkGray");
                AddInputLine(String.search, InputType.Everything);
            });
            var regionGroup = CDesktop.LBWindow.LBRegionGroup;
            var max = Math.Ceiling(specs.Count / 10.0);
            AddPaginationLine(regionGroup, max);
            for (int i = 0; i < 10; i++)
            {
                var index = i;
                AddButtonRegion(() =>
                {
                    if (specs.Count > index + 10 * regionGroup.pagination)
                    {
                        SetRegionBackground(RegionBackgroundType.Button);
                        var foo = specs[index + 10 * regionGroup.pagination];
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
                    spec = specs[index + 10 * regionGroup.pagination];
                    String.objectName.Set(spec.name);
                    CloseWindow("ObjectManagerClass");
                    SpawnWindowBlueprint("ObjectManagerClass");
                });
            }
            AddPaddingRegion(() =>
            {
                AddLine(specs.Count + " classes", "DarkGray");
            });
        }),
        new("ObjectManagerClass", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(161);
            SetRegionGroupHeight(354);
            AddPaddingRegion(() => { AddLine("Class:", "DarkGray"); });
            AddInputRegion(String.objectName, InputType.Everything, spec.name);
            AddPaddingRegion(() => { AddLine("Icon:", "DarkGray"); });
            AddButtonRegion(() =>
            {
                AddLine(spec.icon + ".png");
                AddSmallButton(spec.icon, (h) => { });
            },
            (h) =>
            {
                if (!CDesktop.windows.Exists(x => x.title == "ObjectManagerIconList"))
                {
                    CloseWindow("ObjectManagerClasses");
                    SpawnWindowBlueprint("ObjectManagerIconList");
                }
            });
            AddPaddingRegion(() => { });
        }),

        #endregion
    };

    public static List<Blueprint> desktopBlueprints = new()
    {
        #region Game

        new("Map", () =>
        {
            PlaySound("DesktopOpenSave", 0.2f);
            if (!mapLoaded)
            {
                loadingBar = new GameObject[2];
                loadingBar[0] = new GameObject("LoadingBarBegin", typeof(SpriteRenderer));
                loadingBar[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/LoadingBarEnd");
                loadingBar[0].transform.position = new Vector3(-1181, 863);
                loadingBar[1] = new GameObject("LoadingBar", typeof(SpriteRenderer));
                loadingBar[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/LoadingBarStretch");
                loadingBar[1].transform.position = new Vector3(-1178, 863);
            }
            SetDesktopBackground("LoadingScreens/LoadingScreenKalimdor");
            OrderLoadingMap();
            AddHotkey(W, () => { CheckPosition(new Vector3(0, (float)Math.Round(EuelerGrowth()))); }, false);
            AddHotkey(A, () => { CheckPosition(new Vector3(-(float)Math.Round(EuelerGrowth()), 0)); }, false);
            AddHotkey(S, () => { CheckPosition(new Vector3(0, -(float)Math.Round(EuelerGrowth()))); }, false);
            AddHotkey(D, () => { CheckPosition(new Vector3((float)Math.Round(EuelerGrowth()), 0)); }, false);
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
                //if (CDesktop.windows.Exists(x => x.title == "Inventory"))
                //{
                //    CloseWindow("Inventory");
                //    CloseWindow("MapToolbar");
                //    SpawnWindowBlueprint("MapToolbar");
                //    PlaySound("DesktopInventoryClose");
                //}
            AddHotkey(L, () => { SpawnWindowBlueprint("ItemDrop"); });
            });
            AddHotkey(BackQuote, () => { SpawnDesktopBlueprint("DevPanel"); });
            AddHotkey(L, () => { SpawnWindowBlueprint("ItemDrop"); });

            void CheckPosition(Vector3 amount)
            {
                var temp = CDesktop.screen.transform.position;
                var continent = "";
                if (temp.x <= 3000) continent = "Kalimdor";
                else if (temp.x >= 3000) continent = "Eastern Kingdoms";
                if (continent == "Kalimdor" && (temp + amount).x > 2867)
                {
                    CDesktop.screen.transform.position = new Vector3(2867, temp.y + amount.y, temp.z);
                    cursor.transform.position += temp - CDesktop.screen.transform.position;
                }
                else if (continent == "Eastern Kingdoms" && (temp + amount).x < 4572)
                {
                    CDesktop.screen.transform.position = new Vector3(4572, temp.y + amount.y, temp.z);
                    cursor.transform.position += temp - CDesktop.screen.transform.position;
                }
                else
                {
                    CDesktop.screen.transform.position += amount;
                    cursor.transform.position += amount;
                }
                if (temp.x < 750)
                    CDesktop.screen.transform.position = new Vector3(750, temp.y);
                else if (temp.x > 6295)
                    CDesktop.screen.transform.position = new Vector3(6295, temp.y);
                if (temp.y < -4686)
                    CDesktop.screen.transform.position = new Vector3(temp.x, -4686);
                else if (temp.y > -288)
                    CDesktop.screen.transform.position = new Vector3(temp.x, -288);
            }
        }),
        new("HostileAreaEntrance", () =>
        {
            SetDesktopBackground("Areas/Area" + (area.zone + area.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
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
                var window = CDesktop.windows.Find(x => x.title.StartsWith("HostileArea: "));
                if (window != null)
                {
                    SpawnTransition();
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground("Areas/Area" + instance.name.Replace("'", "").Replace(".", "").Replace(" ", ""));
                    CloseWindow(window);
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
        }),
        new("ComplexEntrance", () =>
        {
            SetDesktopBackground("Areas/Complex" + complex.name.Replace("'", "").Replace(".", "").Replace(" ", ""));
            SpawnWindowBlueprint("Complex: " + complex.name);
            SpawnWindowBlueprint("ComplexLeftSide");
            AddHotkey(Escape, () =>
            {
                var window = CDesktop.windows.Find(x => x.title.StartsWith("HostileArea: "));
                if (window != null)
                {
                    SpawnTransition();
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground("Areas/Complex" + complex.name.Replace("'", "").Replace(".", "").Replace(" ", ""));
                    CloseWindow(window);
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
                CDesktop.Rebuild();
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
                CDesktop.Rebuild();
            });
            AddHotkey(BackQuote, () => { SpawnDesktopBlueprint("DevPanel"); });
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
            SetDesktopBackground("Skin");
            SpawnWindowBlueprint("SpellbookAbilityList");
            SpawnWindowBlueprint("SpellbookAbilityListHeader");
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
        new("EquipmentScreen", () =>
        {
            PlaySound("DesktopInventoryOpen");
            SetDesktopBackground("Leather");
            SpawnWindowBlueprint("SpellbookAbilityList");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("Inventory");
            AddHotkey(B, () =>
            {
                SwitchDesktop("Map");
                CloseDesktop("EquipmentScreen");
                PlaySound("DesktopInventoryClose");
            });
            AddHotkey(Escape, () =>
            {
                SwitchDesktop("Map");
                CloseDesktop("EquipmentScreen");
                PlaySound("DesktopInventoryClose");
            });
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
            AddHotkey(BackQuote, () => { SpawnDesktopBlueprint("DevPanel"); });
        }),

        #endregion

        #region Dev Panel

        new("DevPanel", () =>
        {
            Serialize(races, "races", true);
            Serialize(specs, "classes", true);
            Serialize(abilities, "abilities", true);
            Serialize(buffs, "buffs", true);
            Serialize(areas, "areas", true);
            Serialize(instances, "instances", true);
            Serialize(complexes, "complexes", true);
            Serialize(towns, "towns", true);
            Serialize(items, "items", true);
            Serialize(itemSets, "sets", true);
            PlayAmbience("AmbienceSholazarBasin", 0.5f, true);
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerLobby");
            AddHotkey(Escape, () => { CloseDesktop("DevPanel"); });
        }),
        new("ObjectManagerHostileAreas", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerHostileAreas");
            AddHotkey(Escape, () => { area = null; CloseDesktop("ObjectManagerHostileAreas"); });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerHostileAreas");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination += 1;
                var max = Math.Ceiling((window.title == "ObjectManagerAmbienceList" ? Assets.assets.ambience.Count : areas.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerHostileAreas");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination += (int)Math.Round(EuelerGrowth()) / 2;
                var max = Math.Ceiling((window.title == "ObjectManagerAmbienceList" ? Assets.assets.ambience.Count : areas.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            }, false);
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerHostileAreas");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination -= 1;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            });
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerHostileAreas");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination -= (int)Math.Round(EuelerGrowth()) / 2;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            }, false);
        }),
        new("ObjectManagerInstances", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerInstances");
            AddHotkey(Escape, () => { instance = null; CloseDesktop("ObjectManagerInstances"); });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerInstances");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination += 1;
                var max = Math.Ceiling((window.title == "ObjectManagerAmbienceList" ? Assets.assets.ambience.Count : instances.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerInstances");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination += (int)Math.Round(EuelerGrowth()) / 2;
                var max = Math.Ceiling((window.title == "ObjectManagerAmbienceList" ? Assets.assets.ambience.Count : instances.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            }, false);
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerInstances");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination -= 1;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            });
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerInstances");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination -= (int)Math.Round(EuelerGrowth()) / 2;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            }, false);
        }),
        new("ObjectManagerComplexes", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerComplexes");
            AddHotkey(Escape, () => { complex = null; CloseDesktop("ObjectManagerComplexes"); });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerComplexes");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination += 1;
                var max = Math.Ceiling((window.title == "ObjectManagerAmbienceList" ? Assets.assets.ambience.Count : complexes.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerComplexes");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination += (int)Math.Round(EuelerGrowth()) / 2;
                var max = Math.Ceiling((window.title == "ObjectManagerAmbienceList" ? Assets.assets.ambience.Count : complexes.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            }, false);
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerComplexes");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination -= 1;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            });
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerComplexes");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerAmbienceList");
                if (window == null) return;
                window.regionGroups[0].pagination -= (int)Math.Round(EuelerGrowth()) / 2;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            }, false);
        }),
        new("ObjectManagerRaces", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerRaces");
            AddHotkey(Escape, () => { race = null; CloseDesktop("ObjectManagerRaces"); });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerRaces");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerPortraitList");
                if (window == null) return;
                window.regionGroups[0].pagination += 1;
                var max = Math.Ceiling((window.title == "ObjectManagerPortraitList" ? Assets.assets.portraits.Count : races.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerRaces");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerPortraitList");
                if (window == null) return;
                window.regionGroups[0].pagination += (int)Math.Round(EuelerGrowth()) / 2;
                var max = Math.Ceiling((window.title == "ObjectManagerPortraitList" ? Assets.assets.portraits.Count : races.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            }, false);
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerRaces");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerPortraitList");
                if (window == null) return;
                window.regionGroups[0].pagination -= 1;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            });
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerRaces");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerPortraitList");
                if (window == null) return;
                window.regionGroups[0].pagination -= (int)Math.Round(EuelerGrowth()) / 2;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            }, false);
        }),
        new("ObjectManagerClasses", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerClasses");
            AddHotkey(Escape, () => { spec = null; CloseDesktop("ObjectManagerClasses"); });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerClasses");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination += 1;
                var max = Math.Ceiling((window.title == "ObjectManagerIconList" ? Assets.assets.icons.Count : specs.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerClasses");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination += (int)Math.Round(EuelerGrowth()) / 2;
                var max = Math.Ceiling((window.title == "ObjectManagerIconList" ? Assets.assets.icons.Count : specs.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            }, false);
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerClasses");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination -= 1;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            });
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerClasses");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination -= (int)Math.Round(EuelerGrowth()) / 2;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            }, false);
        }),
        new("ObjectManagerAbilities", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerAbilities");
            AddHotkey(Escape, () => { ability = null; CloseDesktop("ObjectManagerAbilities"); });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerAbilities");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination += 1;
                var max = Math.Ceiling((window.title == "ObjectManagerIconList" ? Assets.assets.icons.Count : abilities.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerAbilities");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination += (int)Math.Round(EuelerGrowth()) / 2;
                var max = Math.Ceiling((window.title == "ObjectManagerIconList" ? Assets.assets.icons.Count : abilities.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            }, false);
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerAbilities");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination -= 1;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            });
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerAbilities");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination -= (int)Math.Round(EuelerGrowth()) / 2;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            }, false);
        }),
        new("ObjectManagerBuffs", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerBuffs");
            AddHotkey(Escape, () => { buff = null; CloseDesktop("ObjectManagerBuffs"); });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerBuffs");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination += 1;
                var max = Math.Ceiling((window.title == "ObjectManagerIconList" ? Assets.assets.icons.Count : buffs.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerBuffs");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination += (int)Math.Round(EuelerGrowth()) / 2;
                var max = Math.Ceiling((window.title == "ObjectManagerIconList" ? Assets.assets.icons.Count : buffs.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            }, false);
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerBuffs");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination -= 1;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            });
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerBuffs");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination -= (int)Math.Round(EuelerGrowth()) / 2;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            }, false);
        }),
        new("ObjectManagerItems", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerItems");
            AddHotkey(Escape, () => { item = null; CloseDesktop("ObjectManagerItems"); });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerItems");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination += 1;
                var max = Math.Ceiling((window.title == "ObjectManagerIconList" ? Assets.assets.icons.Count : items.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerItems");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination += (int)Math.Round(EuelerGrowth()) / 2;
                var max = Math.Ceiling((window.title == "ObjectManagerIconList" ? Assets.assets.icons.Count : items.Count) / 10.0);
                if (window.regionGroups[0].pagination >= max)
                    window.regionGroups[0].pagination = (int)max - 1;
                window.Rebuild();
            }, false);
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerItems");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination -= 1;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            });
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerItems");
                if (window == null) window = CDesktop.windows.Find(x => x.title == "ObjectManagerIconList");
                if (window == null) return;
                window.regionGroups[0].pagination -= (int)Math.Round(EuelerGrowth()) / 2;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            }, false);
        }),
        new("ObjectManagerItemSets", () =>
        {
            SetDesktopBackground("Areas/AreaTheCelestialPlanetarium");
            SpawnWindowBlueprint("ObjectManagerItemSets");
            AddHotkey(Escape, () => { itemSet = null; CloseDesktop("ObjectManagerItemSets"); });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerItemSets");
                if (window == null) return;
                window.regionGroups[0].pagination += 1;
                if (window.regionGroups[0].pagination >= Math.Ceiling(itemSets.Count / 5.0))
                    window.regionGroups[0].pagination = (int)Math.Ceiling(itemSets.Count / 5.0) - 1;
                window.Rebuild();
            });
            AddHotkey(D, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerItemSets");
                if (window == null) return;
                window.regionGroups[0].pagination += (int)Math.Round(EuelerGrowth()) / 2;
                if (window.regionGroups[0].pagination >= Math.Ceiling(itemSets.Count / 5.0))
                    window.regionGroups[0].pagination = (int)Math.Ceiling(itemSets.Count / 5.0) - 1;
                window.Rebuild();
            }, false);
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerItemSets");
                if (window == null) return;
                window.regionGroups[0].pagination -= 1;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            });
            AddHotkey(A, () =>
            {
                var window = CDesktop.windows.Find(x => x.title == "ObjectManagerItemSets");
                if (window == null) return;
                window.regionGroups[0].pagination -= (int)Math.Round(EuelerGrowth()) / 2;
                if (window.regionGroups[0].pagination < 0)
                    window.regionGroups[0].pagination = 0;
                window.Rebuild();
            }, false);
        }),

        #endregion
    };
}
