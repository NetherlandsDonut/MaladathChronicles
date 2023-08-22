using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Font;
using static Blueprint;
using static SiteComplex;
using static SiteInstance;
using static SiteArea;
using static SiteTown;

using static Root.Color;
using static Root.Anchor;
using static Root.RegionBackgroundType;

public static class Root
{
    public static int screenX = 640;
    public static int screenY = 360;
    public static int shadowSystem = 1;
    public static int aiDepth = 5;

    public static GameObject fastTravelCamera;
    public static List<FallingElement> fallingElements;
    public static bool canUnlockScreen;
    public static bool mapLoaded;

    public static string creationName = "Lisette";
    public static string creationFaction = "Alliance";
    public static string creationGender = "Female";
    public static string creationRace = "Human";
    public static string creationClass = "Warrior";
    public static int maxPlayerLevel;

    public static GameObject[] loadingBar;
    public static int loadingScreenObjectLoad;
    public static int loadingScreenObjectLoadAim;
    public static List<Blueprint> loadSites;

    public static Cursor cursor;
    public static CursorRemote cursorEnemy;
    public static int inputLineMarker;
    public static System.Random random;
    public static int keyStack;
    public static int titleScreenCameraDirection;
    public static float heldKeyTime;
    public static float animationTime;
    public static float frameTime = 0.08f;
    public static List<Desktop> desktops;
    public static Desktop CDesktop, LBDesktop;
    public static string markerCharacter = "_", currentInputLine = "";
    public static List<(string, Vector2)> windowRemoteAnchors;
    public static int fallingSoundsPlayedThisFrame;

    public static SaveGame currentSave;
    public static List<SaveGame> saveGames;
    public static GameSettings settings;

    public static void PlaySound(string path, float volume = 0.5f)
    {
        var temp = Resources.Load<AudioClip>("Sounds/" + path);
        if (temp == null) return;
        cursor.GetComponent<AudioSource>().PlayOneShot(temp, volume);
    }

    #region Fonts

    public static Font font;

    public static Sprite GetGlyph(char character)
    {
        var index = charset.IndexOf(character);
        if (index == -1)
        { Debug.LogError("This character was not found in the font character data: " + character); return null; }
        else if (font.glyphs.Length < index)
        { Debug.LogError("This character was not found in the font glyph set: " + character); return null; }
        return font.glyphs[index];
    }

    #endregion

    #region Shatter

    public static GameObject SpawnBuff(Vector3 position, string icon, Entity target)
    {
        var buff = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabBuff"));
        buff.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Buttons/" + icon);
        buff.transform.parent = Board.board.window.desktop.transform;
        buff.transform.position = position;
        buff.GetComponent<FlyingBuff>().Initiate(currentSave.player == target, (h) => { },
            (h) => () =>
            {
                var fb = h.GetComponent<FlyingBuff>();
                var buff = (fb.onPlayer ? Board.board.player.buffs : Board.board.enemy.buffs).Find(x => x.Item3 == h.gameObject);
                var buffObj = Buff.buffs.Find(x => x.name == buff.Item1);
                SetAnchor(Top, 0, -23);
                AddHeaderGroup();
                SetRegionGroupWidth(226);
                SetRegionGroupHeight(217);
                AddHeaderRegion(() =>
                {
                    AddLine(buff.Item1, Gray);
                });
                AddPaddingRegion(() =>
                {
                    AddBigButton(buffObj.icon, (h) => { });
                    AddLine("Dispellable: ", DarkGray);
                    AddText(buffObj.dispelType != "None" ? "Yes" : "No", Gray);
                    AddLine("Turns left: ", DarkGray);
                    AddText(buff.Item2 + "", Gray);
                });
                buffObj.PrintDescription(target, Board.board.player == target ? Board.board.enemy : Board.board.player);
                AddRegionGroup();
                SetRegionGroupWidth(236);
                AddPaddingRegion(() => { AddLine(""); });
            }
        );
        return buff;
    }

    public static GameObject SpawnShatterElement(double speed, double amount, Vector3 position, string sprite, bool oneDirection = false, string block = "0000")
    {
        var element = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabElement"));
        element.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Buttons/" + sprite);
        element.transform.parent = Board.board.window.desktop.transform;
        element.transform.position = position;
        element.GetComponent<FlyingElement>().Initiate();
        SpawnShatter(speed, amount, position, sprite, oneDirection, block);
        return element;
    }

    public static void SpawnShatterSmall(double speed, double amount, Vector3 position, string sprite, bool oneDirection = false, string block = "0000")
    {
        var shatter = new GameObject("Shatter", typeof(Shatter));
        shatter.GetComponent<Shatter>().Initiate(3);
        shatter.transform.parent = Board.board.window.desktop.transform;
        shatter.transform.position = position - new Vector3(7, 9);
        shatter.layer = 2;
        var foo = Resources.Load<Sprite>("Sprites/Building/Buttons/" + sprite);
        int x = (int)foo.textureRect.width, y = (int)foo.textureRect.height;
        var dot = Resources.Load<GameObject>("Prefabs/PrefabDot");
        var direction = RollDirection();
        if (amount > 1) amount = 1;
        else if (amount < 0) amount = 0;
        for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
                if (random.Next(0, (int)Math.Abs(amount * 10 - 10)) == 0)
                    SpawnDot(i, j, foo.texture.GetPixel(i, j));

        void SpawnDot(int c, int v, Color32 color)
        {
            var newObject = UnityEngine.Object.Instantiate(dot);
            newObject.GetComponent<Shatter>().Initiate(random.Next(1, 8) / 50.0f);
            newObject.transform.parent = shatter.transform;
            newObject.transform.localPosition = new Vector3(c, v);
            newObject.GetComponent<SpriteRenderer>().color = color;
            newObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
            newObject.GetComponent<Rigidbody2D>().AddRelativeForce((direction / 2 + UnityEngine.Random.insideUnitCircle / 2) * (int)(100 * speed));
            if (!oneDirection) direction = RollDirection();
        }

        Vector2 RollDirection()
        {
            var direction = UnityEngine.Random.insideUnitCircle;
            if (block[0] == '1' && block[2] == '1') direction = new Vector2(direction.x, 0);
            else if (block[0] == '1' && direction.y > 0 || block[2] == '1' && direction.y < 0) direction = new Vector2(direction.x, -direction.y);
            if (block[1] == '1' && block[3] == '1') direction = new Vector2(0, direction.y);
            else if (block[1] == '1' && direction.x > 0 || block[3] == '1' && direction.x < 0) direction = new Vector2(-direction.x, direction.y);
            return direction;
        }
    }

    public static void SpawnShatter(double speed, double amount, Vector3 position, string sprite, bool oneDirection = false, string block = "0000")
    {
        var shatter = new GameObject("Shatter", typeof(Shatter));
        shatter.GetComponent<Shatter>().Initiate(3);
        shatter.transform.parent = Board.board.window.desktop.transform;
        shatter.transform.position = position;
        shatter.layer = 1;
        var foo = Resources.Load<Sprite>("Sprites/Building/BigButtons/" + sprite);
        int x = (int)foo.textureRect.width, y = (int)foo.textureRect.height;
        var dot = Resources.Load<GameObject>("Prefabs/PrefabDot");
        var direction = RollDirection();
        if (amount > 1) amount = 1;
        else if (amount < 0) amount = 0;
        for (int i = 3; i < x - 2; i++)
            for (int j = 3; j < y - 2; j++)
                if (random.Next(0, (int)Math.Abs(amount * 10 - 10)) == 0)
                    SpawnDot(i, j, foo.texture.GetPixel(i, j));

        void SpawnDot(int c, int v, Color32 color)
        {
            var newObject = UnityEngine.Object.Instantiate(dot);
            newObject.GetComponent<Shatter>().Initiate(random.Next(1, 8) / 50.0f);
            newObject.transform.parent = shatter.transform;
            newObject.transform.localPosition = new Vector3(c, v);
            newObject.GetComponent<SpriteRenderer>().color = color;
            newObject.GetComponent<Rigidbody2D>().AddRelativeForce((direction / 2 + UnityEngine.Random.insideUnitCircle / 2) * (int)(100 * speed));
            if (!oneDirection) direction = RollDirection();
        }

        Vector2 RollDirection()
        {
            var direction = UnityEngine.Random.insideUnitCircle;
            if (block[0] == '1' && block[2] == '1') direction = new Vector2(direction.x, 0);
            else if (block[0] == '1' && direction.y > 0 || block[2] == '1' && direction.y < 0) direction = new Vector2(direction.x, -direction.y);
            if (block[1] == '1' && block[3] == '1') direction = new Vector2(0, direction.y);
            else if (block[1] == '1' && direction.x > 0 || block[3] == '1' && direction.x < 0) direction = new Vector2(-direction.x, direction.y);
            return direction;
        }
    }

    #endregion

    #region Talent

    public static void PrintEquipmentItem(Item item)
    {
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddBigButton(item == null ? "OtherEmpty" : item.icon,
            (h) =>
            {

            },
            (h) => () =>
            {
                if (item == null) return;
                SetAnchor(BottomRight);
                PrintItemTooltip(item);
            });
            if (item != null) AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""));
        });
    }

    public static void PrintItemTooltip(Item item)
    {
        AddHeaderGroup();
        SetRegionGroupWidth(188);
        var split = item.name.Split(", ");
        AddHeaderRegion(() =>
        {
            AddLine(split[0], Item.rarityColors[item.rarity]);
        });
        if (split.Length > 1)
            AddHeaderRegion(() =>
            {
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
        if (item.classes != null)
            AddHeaderRegion(() =>
            {
                AddLine("Classes: ", DarkGray);
                foreach (var spec in item.classes)
                {
                    AddText(spec, ColorFromText(spec));
                    if (spec != item.classes.Last())
                        AddText(", ", DarkGray);
                }
            });
        if (item.set != null)
        {
            AddHeaderRegion(() =>
            {
                AddLine("Part of ", DarkGray);
                AddText(item.set, Gray);
            });
            var set = ItemSet.itemSets.Find(x => x.name == item.set);
            if (set == null)
            {
                Debug.Log("ERROR 002: Set not found \"" + item.set + "\"");
                return;
            }
            AddPaddingRegion(() =>
            {
                foreach (var bonus in set.setBonuses)
                {
                    var howMuch = set.EquippedPieces(currentSave.player);
                    bool has = howMuch >= bonus.requiredPieces;
                    AddLine((has ? bonus.requiredPieces : howMuch) + "/" + bonus.requiredPieces + " Set: ", has ? Uncommon : DarkGray);
                    if (bonus.description.Count > 0)
                        AddText(bonus.description[0], has ? Uncommon : DarkGray);
                    for (int i = 0; i < bonus.description.Count - 1; i++)
                        AddLine(bonus.description[0], has ? Uncommon : DarkGray);
                }
            });
        }
        AddHeaderRegion(() =>
        {
            AddLine("Required level: ", DarkGray);
            AddText("" + item.lvl, ItemColoredLevel(item.lvl));
        });
        var lacking = 0;
        if ((int)item.price > 0)
        {
            AddRegionGroup();
            AddPaddingRegion(
                () =>
                {
                    AddSmallButton("ItemCoinsGold", (h) => { });
                }

            );
            AddRegionGroup();
            SetRegionGroupWidth(20);
            AddPaddingRegion(
                () =>
                {
                    AddLine((int)item.price + "", Gold);
                }

            );
        }
        else lacking++;
        if ((int)(item.price * 100 % 100) > 0)
        {
            AddRegionGroup();
            AddPaddingRegion(
                () =>
                {
                    AddSmallButton("ItemCoinsSilver", (h) => { });
                }
            );
            AddRegionGroup();
            SetRegionGroupWidth(20);
            AddPaddingRegion(
                () =>
                {
                    AddLine((int)(item.price * 100 % 100) + "" + "", Silver);
                }
            );
        }
        else lacking++;
        if ((int)(item.price * 10000 % 100) > 0)
        {
            AddRegionGroup();
            AddPaddingRegion(
                () =>
                {
                    AddSmallButton("ItemCoinsCopper", (h) => { });
                }
            );
            AddRegionGroup();
            SetRegionGroupWidth(20);
            AddPaddingRegion(
                () =>
                {
                    AddLine((int)(item.price * 10000 % 100) + "" + "", Copper);
                }
            );
        }
        else lacking++;
        AddRegionGroup();
        SetRegionGroupWidth(188 - (3 - lacking) * 49);
        AddPaddingRegion(() => { AddLine("", Black); });
    }

    public static void PrintTransportTooltip(Transport transport)
    {
        AddHeaderGroup();
        SetRegionGroupWidth(188);
        AddHeaderRegion(() =>
        {
            AddLine(transport.means, Gray);
        });
        AddPaddingRegion(() =>
        {
            AddLine("To " + transport.destination, Gray);
        });
        if (transport.price > 0)
        {
            var lacking = 0;
            if ((int)transport.price > 0)
            {
                AddRegionGroup();
                AddPaddingRegion(
                    () =>
                    {
                        AddSmallButton("ItemCoinsGold", (h) => { });
                    }

                );
                AddRegionGroup();
                SetRegionGroupWidth(20);
                AddPaddingRegion(
                    () =>
                    {
                        AddLine((int)transport.price + "", Gold);
                    }

                );
            }
            else lacking++;
            if ((int)(transport.price * 100 % 100) > 0)
            {
                AddRegionGroup();
                AddPaddingRegion(
                    () =>
                    {
                        AddSmallButton("ItemCoinsSilver", (h) => { });
                    }
                );
                AddRegionGroup();
                SetRegionGroupWidth(20);
                AddPaddingRegion(
                    () =>
                    {
                        AddLine((int)(transport.price * 100 % 100) + "" + "", Silver);
                    }
                );
            }
            else lacking++;
            if ((int)(transport.price * 10000 % 100) > 0)
            {
                AddRegionGroup();
                AddPaddingRegion(
                    () =>
                    {
                        AddSmallButton("ItemCoinsCopper", (h) => { });
                    }
                );
                AddRegionGroup();
                SetRegionGroupWidth(20);
                AddPaddingRegion(
                    () =>
                    {
                        AddLine((int)(transport.price * 10000 % 100) + "" + "", Copper);
                    }
                );
            }
            else lacking++;
            AddRegionGroup();
            SetRegionGroupWidth(188 - (3 - lacking) * 49);
            AddPaddingRegion(() => { AddLine("", Black); });
        }
        else
            AddHeaderRegion(() =>
            {
                AddLine("Travel free of charge", Gray);
            });
    }


    public static void PrintInventoryItem(Item item)
    {
        AddBigButton(item.icon,
            (h) =>
            {
                if (item.CanEquip(currentSave.player))
                {
                    PlaySound(item.ItemSound("PickUp"));
                    item.Equip(currentSave.player);
                    CloseWindow(h.window);
                    SpawnWindowBlueprint("Inventory");
                    CloseWindow("PlayerEquipmentInfo");
                    SpawnWindowBlueprint("PlayerEquipmentInfo");
                }
            },
            (h) => () =>
            {
                if (item == null) return;
                SetAnchor(Center);
                PrintItemTooltip(item);
            }
        );
        if (settings.rarityIndicators.Value())
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 2);
        if (currentSave.player.HasItemEquipped(item.name))
        {
            SetBigButtonToGrayscale();
            AddBigButtonOverlay("OtherGridBlurred", 0, 2);
        }
        if (item.CanEquip(currentSave.player) && currentSave.player.IsItemNewSlot(item) && (settings.upgradeIndicators.Value() || settings.newSlotIndicators.Value()))
            AddBigButtonOverlay(settings.newSlotIndicators.Value() ? "OtherItemNewSlot" : "OtherItemUpgrade", 0, 2);
        else if (settings.upgradeIndicators.Value() && item.CanEquip(currentSave.player) && currentSave.player.IsItemAnUpgrade(item))
            AddBigButtonOverlay("OtherItemUpgrade", 0, 2);
    }

    public static void PrintSite(string name, string type, Vector2 anchor)
    {
        SetAnchor(anchor.x, anchor.y);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            if (type == "Town")
            {
                var find = towns.Find(x => x.name == name);
                if (find == null)
                {
                    Debug.LogError("ERROR 001: No town named \"" + name + "\" has been found.");
                    return;
                }
                AddSmallButton("Faction" + find.faction,
                (h) =>
                {
                    town = find;
                    PlaySound("DesktopInstanceOpen");
                    SpawnDesktopBlueprint("TownEntrance");
                    SwitchDesktop("TownEntrance");
                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine(name, Gray);
                    });
                });
            }
            else if (type == "HostileArea")
            {
                var find = areas.Find(x => x.name == name);
                AddSmallButton(find == null ? "OtherUnknown" : "Site" + find.type,
                (h) =>
                {
                    area = areas.Find(x => x.name == name);
                    if (area == null) return;
                    PlaySound("DesktopInstanceOpen");
                    SpawnDesktopBlueprint("HostileAreaEntrance");
                    SwitchDesktop("HostileAreaEntrance");
                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine(name, Gray);
                    });
                    if (find != null)
                    {
                        AddHeaderRegion(() =>
                        {
                            AddLine("Recommended level: ", Gray);
                            AddText(find.recommendedLevel + "", EntityColoredLevel(find.recommendedLevel));
                        });
                        AddPaddingRegion(() =>
                        {
                            AddLine("Possible encounters:", DarkGray);
                            foreach (var encounter in find.possibleEncounters)
                                AddLine("- " + encounter.who, DarkGray);
                        });
                    }
                });
            }
            else if (type == "Dungeon")
                AddSmallButton("Site" + type,
                (h) =>
                {
                    instance = instances.Find(x => x.name == name);
                    if (instance != null)
                    {
                        PlaySound("DesktopInstanceOpen");
                        SpawnDesktopBlueprint("InstanceEntrance");
                        SwitchDesktop("InstanceEntrance");
                    }
                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine(name, Gray);
                    });
                    AddPaddingRegion(() =>
                    {
                        AddLine("Level range: ", Gray);
                        instance = instances.Find(x => x.name == name);
                        if (instance == null)
                            AddText("??", DarkGray);
                        else
                        {
                            var range = instance.LevelRange();
                            AddText(range.Item1 + "", EntityColoredLevel(range.Item1));
                            AddText(" - ", Gray);
                            AddText(range.Item2 + "", EntityColoredLevel(range.Item2));
                        }
                    });
                });
            else if (type == "Raid")
                AddSmallButton("Site" + type,
                (h) =>
                {
                    instance = instances.Find(x => x.name == name);
                    if (instance != null)
                    {
                        PlaySound("DesktopInstanceOpen");
                        SpawnDesktopBlueprint("InstanceEntrance");
                        SwitchDesktop("InstanceEntrance");
                    }
                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine(name, Gray);
                    });
                    AddPaddingRegion(() =>
                    {
                        AddLine("Level range: ", Gray);
                        instance = instances.Find(x => x.name == name);
                        if (instance == null)
                            AddText("??", DarkGray);
                        else
                        {
                            var range = instance.LevelRange();
                            AddText(range.Item1 + "", EntityColoredLevel(range.Item1));
                            AddText(" - ", Gray);
                            AddText(range.Item2 + "", EntityColoredLevel(range.Item2));
                        }
                    });
                });
            else if (type == "Complex")
                AddSmallButton("Site" + type,
                (h) =>
                {
                    complex = complexes.Find(x => x.name == name);
                    if (complex != null)
                    {
                        PlaySound("DesktopInstanceOpen");
                        SpawnDesktopBlueprint("ComplexEntrance");
                        SwitchDesktop("ComplexEntrance");
                    }
                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine(name, Gray);
                    });
                    AddPaddingRegion(() =>
                    {
                        AddLine("Contains sites:", DarkGray);
                    });
                    complex = complexes.Find(x => x.name == name);
                    foreach (var site in complex.sites)
                    AddHeaderRegion(() =>
                    {
                        AddLine(site.Item2, DarkGray);
                        AddSmallButton("Site" + site.Item1, (h) => { });
                    });
                });
        });
    }

    public static void PrintRaidWing(SiteInstance instance, InstanceWing wing)
    {
        if (instance.wings.Count > 1)
            AddHeaderRegion(() =>
            {
                AddLine(wing.name, Gray);
            });
        var temp = wing.areas.Select(x => areas.Find(y => y.name == x.Item2)).ToList();
        foreach (var area in temp)
        AddButtonRegion(() =>
        {
            var name = area != null ? area.name : "AREA NOT FOUND";
            AddLine(name, Black);
        },
        (h) =>
        {
            if (area == null) return;
            var window = CDesktop.windows.Find(x => x.title.StartsWith("Area: "));
            if (window != null)
                if (window.title == "Area: " + area.name) return;
                else CloseWindow(window);
            SpawnWindowBlueprint("Area: " + area.name);
            SetDesktopBackground("Areas/Area" + (instance.name + area.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
            SpawnTransition();
        });
    }

    public static void PrintComplexSite(SiteComplex complex, (string, string) site)
    {
        AddButtonRegion(() =>
        {
            AddLine(site.Item2, Black);
            AddSmallButton("Site" + site.Item1, (h) => { });
        },
        (h) =>
        {
            if (site.Item1 == "HostileArea")
            {
                area = areas.Find(x => x.name == site.Item2);
                var window = CDesktop.windows.Find(x => x.title.StartsWith("Area: "));
                if (window != null)
                    if (window.title == "Area: " + area.name) return;
                    else CloseWindow(window);
                SpawnWindowBlueprint("Area: " + area.name);
                SetDesktopBackground("Areas/Area" + (area.zone + area.name).Replace("'", "").Replace(".", "").Replace(" ", ""));
                SpawnTransition();
            }
            else
            {
                CloseDesktop("ComplexEntrance");
                instance = instances.Find(x => x.name == site.Item2);
                SpawnDesktopBlueprint("InstanceEntrance");
            }
        });
    }

    public static Color ColorFromText(string color)
    {
        if (color == "Black") return Black;
        else if (color == "DarkGray") return DarkGray;
        else if (color == "Gray") return Gray;
        else if (color == "LightGray") return LightGray;
        else if (color == "White") return White;
        else if (color == "Red") return Red;
        else if (color == "DangerousRed") return DangerousRed;
        else if(color == "Poor") return Poor;
        else if(color == "Common") return Common;
        else if(color == "Uncommon") return Uncommon;
        else if(color == "Rare") return Rare;
        else if(color == "Epic") return Epic;
        else if(color == "Legendary") return Legendary;
        else if (color == "Paladin") return Paladin;
        else if (color == "Warrior") return Warrior;
        else if (color == "Rogue") return Rogue;
        else if (color == "Priest") return Priest;
        else if (color == "Mage") return Mage;
        else if (color == "Druid") return Druid;
        else if (color == "Warlock") return Warlock;
        else if (color == "Hunter") return Hunter;
        else if (color == "Shaman") return Shaman;
        else return Gray;
    }

    public static Color ItemColoredLevel(int level)
    {
        if (level > currentSave.player.level) return Red;
        else return Gray;
    }

    public static Color ProgressColored(int progress)
    {
        if (progress == 0) return DarkGray;
        else if (progress <= 20) return Red;
        else if (progress <= 40) return Orange;
        else if (progress <= 60) return Yellow;
        else return Green;
    }

    public static Color EntityColoredLevel(int level)
    {
        if (level - 4 > currentSave.player.level) return DangerousRed;
        else if (level - 2 > currentSave.player.level) return Orange;
        else if (level + 2 < currentSave.player.level && currentSave.player.WillGetExperience(level)) return Green;
        else if (!currentSave.player.WillGetExperience(level)) return DarkGray;
        else return Yellow;
    }

    public static void PrintTalent(int spec, int row, int col)
    {
        SetAnchor(25 + (spec == 1 ? 213 : (spec == 2 ? 425 : 0)) + 62 * col, -62 * row - 23);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            var playerClass = currentSave.player.GetClass();
            var talent = playerClass.talentTrees[spec].talents.Find(x => x.row == row && x.col == col);
            var previousTalent = currentSave.player.PreviousTalent(spec, talent);
            var previousTalentDistance = previousTalent == null ? 0 : talent.row - previousTalent.row;
            var abilityObj = Ability.abilities.Find(x => x.name == talent.ability);
            AddBigButton("Ability" + talent.ability.Replace(" ", "").Replace(":", ""),
                (h) =>
                {
                    var canPick = currentSave.player.CanPickTalent(spec, talent);
                    if (!currentSave.player.abilities.Contains(talent.ability) && currentSave.player.CanPickTalent(spec, talent))
                    {
                        currentSave.player.unspentTalentPoints--;
                        PlaySound("DesktopTalentAcquired", 0.2f);
                        currentSave.player.abilities.Add(talent.ability);
                        CDesktop.Rebuild();
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
                        AddLine(talent.ability, Gray);
                    });
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("Ability" + talent.ability.Replace(" ", "").Replace(":", ""), (h) => { });
                        if (abilityObj != null)
                        {
                            AddLine("Required level: ", DarkGray);
                            AddText(1 + "", Gray);
                            AddLine("Cooldown: ", DarkGray);
                            AddText(abilityObj.cooldown == 0 ? "None" : abilityObj.cooldown + (abilityObj.cooldown == 1 ? " turn" : " turns"), Gray);
                        }
                    });
                    if (abilityObj != null)
                    {
                        abilityObj.PrintDescription(currentSave.player, null);
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
                                    AddLine(cost.Value + "", Board.board != null ? (cost.Value > Board.board.player.resources[cost.Key] ? Red : Green) : Gray);
                                });
                            }
                    }
                    else AddPaddingRegion(() =>
                    {
                        AddLine("Ability wasn't found!", Red);
                        SetRegionAsGroupExtender();
                    });
                    AddRegionGroup();
                    SetRegionGroupWidth(256 - (abilityObj == null || abilityObj.cost == null ? 0 : abilityObj.cost.Count) * 49);
                    AddPaddingRegion(() =>
                    {
                        AddLine("", LightGray);
                    });
                }
            );
            if (currentSave.player.abilities.Contains(talent.ability))
                AddBigButtonOverlay("OtherGlowLearned");
            else
            {
                var canPick = currentSave.player.CanPickTalent(spec, talent);
                if (currentSave.player.CanPickTalent(spec, talent)) AddBigButtonOverlay("OtherGlowLearnable");
                else
                {
                    SetBigButtonToGrayscale();
                    AddBigButtonOverlay("OtherGridBlurred");
                }
            }
            if (talent.inherited)
            {
                GameObject body = null;
                if (currentSave.player.abilities.Contains(previousTalent.ability))
                {
                    body = AddBigButtonOverlay("OtherTalentArrowFillBody");
                    body.transform.localPosition = new Vector3(0, 26, 0);
                    body.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    body.transform.localScale = new Vector3(1, previousTalentDistance * 18 + (previousTalentDistance > 1 ? (previousTalentDistance - 1) * 44 : 0), 1);
                    AddBigButtonOverlay("OtherTalentArrowFillHead").GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
                body = AddBigButtonOverlay("OtherTalentArrowBody");
                body.transform.localPosition = new Vector3(0, 28, 0);
                body.transform.localScale = new Vector3(1, previousTalentDistance * 14 + (previousTalentDistance > 1 ? (previousTalentDistance - 1) * 48 : 0), 1);
                AddBigButtonOverlay("OtherTalentArrowHead");
            }
        });
    }

    #endregion

    #region Desktop

    public static void SpawnDesktopBlueprint(string blueprintTitle, bool autoSwitch = true)
    {
        var blueprint = desktopBlueprints.Find(x => x.title == blueprintTitle);
        if (blueprint == null) return;
        AddDesktop(blueprint.title);
        if (autoSwitch)
            SwitchDesktop(blueprintTitle);
        blueprint.actions();
        //currentDesktop.Rebuild();
    }

    public static void CloseDesktop(string desktopName)
    {
        var find = desktops.Find(x => x.title == desktopName);
        desktops.Remove(find);
        if (find == CDesktop)
            SwitchDesktop(desktops[0].title);
        UnityEngine.Object.Destroy(find.gameObject);
    }

    private static void AddDesktop(string title)
    {
        var newObject = new GameObject("Desktop: " + title, typeof(Desktop), typeof(SpriteRenderer));
        newObject.transform.localPosition = new Vector3();
        var newDesktop = newObject.GetComponent<Desktop>();
        LBDesktop = newDesktop;
        newDesktop.Initialise(title);
        desktops.Add(newDesktop);
        newDesktop.screen = new GameObject("Camera", typeof(Camera)/*, typeof(PixelPerfectCamera)*/, typeof(SpriteRenderer)).GetComponent<Camera>();
        newDesktop.screen.transform.parent = newDesktop.transform;
        newDesktop.GetComponent<SpriteRenderer>().sortingLayerName = "DesktopBackground";
        newDesktop.screen.GetComponent<SpriteRenderer>().sortingLayerName = "DesktopBackground";
        newDesktop.screen.orthographicSize = 180;
        newDesktop.screen.nearClipPlane = -100;
        newDesktop.screen.farClipPlane = 1000;
        newDesktop.screen.clearFlags = CameraClearFlags.SolidColor;
        newDesktop.screen.backgroundColor = UnityEngine.Color.black;
        newDesktop.screen.orthographic = true;
        //var ppc = newDesktop.screen.GetComponent<PixelPerfectCamera>();
        //ppc.assetsPPU = 1;
        //ppc.refResolutionX = 640;
        //ppc.refResolutionY = 360;
        //ppc.pixelSnapping = true;
        var cameraBorder = new GameObject("CameraBorder", typeof(SpriteRenderer));
        var cameraShadow = new GameObject("CameraShadow", typeof(SpriteRenderer));
        cameraShadow.transform.parent = cameraBorder.transform.parent = newDesktop.screen.transform;
        cameraBorder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/CameraBorder");
        cameraShadow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/CameraShadow");
        cameraBorder.GetComponent<SpriteRenderer>().sortingLayerName = "CameraBorder";
        cameraShadow.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
        newDesktop.screenlock = new GameObject("Screenlock", typeof(BoxCollider2D), typeof(SpriteRenderer));
        newDesktop.screenlock.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/Screenlock");
        newDesktop.screenlock.GetComponent<SpriteRenderer>().sortingLayerName = "DesktopBackground";
        newDesktop.screenlock.GetComponent<SpriteRenderer>().sortingOrder = 1;
        newDesktop.screenlock.GetComponent<BoxCollider2D>().size = new Vector2(640, 360);
        newDesktop.screenlock.transform.parent = newDesktop.screen.transform;
        newDesktop.UnlockScreen();
        newObject.SetActive(false);
    }

    public static void SwitchDesktop(string name)
    {
        if (CDesktop != null)
            CDesktop.gameObject.SetActive(false);
        var find = desktops.Find(x => x.title == name);
        if (find != null) CDesktop = find;
        if (CDesktop != null)
        {
            CDesktop.gameObject.SetActive(true);
            desktops.Remove(CDesktop);
            desktops.Insert(0, CDesktop);
            SpawnTransition();
        }
    }

    public static void OrderLoadingMap()
    {
        loadSites = windowBlueprints.FindAll(x => x.title.StartsWith("Site: "));
        loadingScreenObjectLoad = 0;
        loadingScreenObjectLoadAim = loadSites.Count;
    }

    public static void SpawnTransition(float time = 0.1f)
    {
        var transition = new GameObject("CameraTransition", typeof(SpriteRenderer), typeof(Shatter));
        transition.transform.parent = CDesktop.screen.transform;
        transition.transform.localPosition = Vector3.zero;
        transition.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/CameraTransition");
        transition.GetComponent<Shatter>().Initiate(time, transition.GetComponent<SpriteRenderer>());
    }

    public static void RemoveDesktopBackground(bool followCamera = true)
    {
        if (followCamera) CDesktop.screen.GetComponent<SpriteRenderer>().sprite = null;
        else CDesktop.GetComponent<SpriteRenderer>().sprite = null;
    }

    public static void SetDesktopBackground(string texture, bool followCamera = true)
    {
        if (followCamera) CDesktop.screen.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/" + texture);
        else CDesktop.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/" + texture);
    }

    //Hotkeys can be added only on desktop creation!
    public static void AddHotkey(KeyCode key, Action action, bool keyDown = true)
    {
        LBDesktop.hotkeys.Add(new Hotkey(key, action, keyDown));
    }

    public static float EuelerGrowth()
    {
        return (float)Math.Pow(keyStack / 100.0 + 1.0, Math.E);
    }

    #endregion

    #region Windows

    public static void SpawnWindowBlueprint(string blueprintTitle) => SpawnWindowBlueprint(windowBlueprints.Find(x => x.title == blueprintTitle));
    public static void SpawnWindowBlueprint(Blueprint blueprint)
    {
        if (blueprint == null) return;
        AddWindow(blueprint.title, blueprint.upperUI);
        blueprint.actions();
        CDesktop.LBWindow.Rebuild();
        CDesktop.LBWindow.ResetPosition();
    }

    public static void AddWindow(string title, bool upperUI)
    {
        var newObject = new GameObject("Window: " + title, typeof(Window));
        newObject.transform.parent = CDesktop.transform;
        //I'm spawning it at z = 9999 because I don't want it to
        //cover anything on the screen when it's being built
        newObject.transform.localPosition = new Vector3(0, 0, 9999);
        newObject.GetComponent<Window>().Initialise(CDesktop, title, upperUI);
    }

    public static void CloseWindow(string windowName) => CloseWindow(CDesktop.windows.Find(x => x.title == windowName));
    public static void CloseWindow(Window window)
    {
        CDesktop.windows.Remove(window);
        UnityEngine.Object.Destroy(window.gameObject);
        CDesktop.Reindex();
    }

    public static void CloseWindowOnLostFocus() => CDesktop.LBWindow.closeOnLostFocus = true;

    public static void MarkWindowGlobal(string id)
    {
        if (CDesktop.globalWindows.ContainsKey(id))
            CDesktop.globalWindows.Remove(id);
        CDesktop.globalWindows.Add(id, CDesktop.LBWindow);
    }

    public static void SetAnchor(string anchor)
    {
        var find = windowRemoteAnchors.Find(x => x.Item1 == anchor);
        SetAnchor(find.Item2.x, find.Item2.y);
    }

    public static void SetAnchor(Anchor anchor, Window magnet)
    {
        CDesktop.LBWindow.anchor = new WindowAnchor(anchor, 0, 0, magnet);
    }

    public static void SetAnchor(float x = 0, float y = 0)
    {
        CDesktop.LBWindow.anchor = new WindowAnchor(None, x, y, null);
    }

    public static void SetAnchor(Anchor anchor, float x = 0, float y = 0)
    {
        CDesktop.LBWindow.anchor = new WindowAnchor(anchor, x, y, null);
    }

    public static void DisableGeneralSprites()
    {
        CDesktop.LBWindow.disabledGeneralSprites = true;
    }

    public static void DisableShadows()
    {
        CDesktop.LBWindow.disabledShadows = true;
    }

    public static void MaskWindow()
    {
        CDesktop.LBWindow.masked = true;
    }

    #endregion

    #region RegionGroups

    public static void AddHeaderGroup(string id = "")
    {
        var newObject = new GameObject("RegionGroup", typeof(RegionGroup));
        newObject.transform.parent = CDesktop.LBWindow.transform;
        newObject.GetComponent<RegionGroup>().Initialise(CDesktop.LBWindow);
        if (id != "") MarkRegionGroupGlobal(id);
    }

    public static void AddRegionGroup(int insert = -1, string id = "")
    {
        var newObject = new GameObject("RegionGroup", typeof(RegionGroup));
        newObject.transform.parent = CDesktop.LBWindow.transform;
        newObject.GetComponent<RegionGroup>().Initialise(CDesktop.LBWindow, insert);
        if (id != "") MarkRegionGroupGlobal(id);
    }

    public static void CloseRegionGroup(string id) => CloseRegionGroup(CDesktop.globalRegionGroups[id]);
    public static void CloseRegionGroup(RegionGroup regionGroup)
    {
        if (regionGroup != null)
        {
            regionGroup.window.regionGroups.Remove(regionGroup);
            UnityEngine.Object.Destroy(regionGroup.gameObject);
        }
    }

    public static void MarkRegionGroupGlobal(string id)
    {
        if (CDesktop.globalRegionGroups.ContainsKey(id))
            CDesktop.globalRegionGroups.Remove(id);
        CDesktop.globalRegionGroups.Add(id, CDesktop.LBWindow.LBRegionGroup);
    }

    public static void SetRegionGroupWidth(int width)
    {
        CDesktop.LBWindow.LBRegionGroup.setWidth = width;
    }

    public static void SetRegionGroupHeight(int height)
    {
        CDesktop.LBWindow.LBRegionGroup.setHeight = height;
    }

    #endregion

    #region RegionLists

    public static void AddLineList(int count, Action<int> inDraw, Action<Highlightable> press) => AddLineList(count, () => count, inDraw, null, press);
    public static void AddLineList(int regionAmount, Func<int> count, Action<int> inDraw, Action<int> outDraw, Action<Highlightable> press)
    {
        var regionGroup = CDesktop.LBWindow.LBRegionGroup;
        if (regionGroup.regionList != null) return;
        var newObject = new GameObject("RegionList", typeof(RegionList));
        newObject.transform.parent = regionGroup.transform;
        var regionList = newObject.GetComponent<RegionList>();
        regionList.Initialise(regionGroup, count, inDraw, outDraw);
        for (int i = 0; i < regionAmount; i++)
        {
            AddButtonRegion(() => { }, press);
            regionList.regions.Add(regionGroup.LBRegion);
            regionList.transform.parent = newObject.transform;
        }
    }

    public static void MarkLineListGlobal(string id)
    {
        if (CDesktop.globalLines.ContainsKey(id))
            CDesktop.globalLines.Remove(id);
        CDesktop.globalLines.Add(id, CDesktop.LBWindow.LBRegionGroup.LBRegion.LBLine);
    }

    #endregion

    #region Regions

    private static void AddRegion(RegionBackgroundType backgroundType, Action draw, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip, int insert = -1, string id = "")
    {
        var newObject = new GameObject("Region", typeof(Region));
        var regionGroup = CDesktop.LBWindow.LBRegionGroup;
        newObject.transform.parent = regionGroup.transform;
        newObject.GetComponent<Region>().Initialise(regionGroup, backgroundType, draw, pressEvent, tooltip, insert);
        if (id != "") MarkRegionGlobal(id);
    }

    public static void AddRegionOverlay(Region onWhat, string overlay, float time = 0)
    {
        var newObject = new GameObject("RegionOverlay", typeof(SpriteRenderer));
        newObject.transform.parent = onWhat.transform;
        newObject.transform.localPosition = onWhat.background.transform.localPosition - new Vector3(0, 0, 0.1f);
        newObject.transform.localScale = onWhat.background.transform.localScale - new Vector3(19 * onWhat.smallButtons.Count, 0);
        newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Backgrounds/" + overlay);
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(time);
        }
    }

    public static void AddHandleRegion(Action draw, int insert = -1, string id = "")
    {
        AddRegion(Handle, draw, (h) => { }, null, insert, id);
    }

    public static void AddButtonRegion(Action draw, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip = null, int insert = -1, string id = "")
    {
        AddRegion(Button, draw, pressEvent, tooltip, insert, id);
    }

    public static void AddHeaderRegion(Action draw, int insert = -1, string id = "")
    {
        AddRegion(Header, draw, (h) => { }, null, insert, id);
    }

    public static void AddPaddingRegion(Action draw, int insert = -1, string id = "")
    {
        AddRegion(Padding, draw, (h) => { }, null, insert, id);
    }

    public static void AddInputRegion(String refText, InputType inputType, string id, int insert = -1)
    {
        AddRegion(Padding, () =>
        {
            AddInputLine(refText, inputType, id);
        }, 
        (h) =>
        {
            //CAN BE ACCESSED THROUGH H?
            inputLineMarker = CDesktop.globalInputLines[id].text.text.Value().Length;
        },
        null, insert, id);
    }

    public static void MarkRegionGlobal(string id)
    {
        if (CDesktop.globalRegions.ContainsKey(id))
            CDesktop.globalRegions.Remove(id);
        CDesktop.globalRegions.Add(id, CDesktop.LBWindow.LBRegionGroup.LBRegion);
    }

    //When other region groups are lenghier than the
    //one this region is in then the unique extender will
    //be extended to match length of the other group regions
    public static void SetRegionAsGroupExtender()
    {
        var temp = CDesktop.LBWindow.LBRegionGroup;
        temp.EXTRegion = temp.LBRegion;
    }

    public static void SetRegionBackground(RegionBackgroundType backgroundType)
    {
        CDesktop.LBWindow.LBRegionGroup.LBRegion.backgroundType = backgroundType;
    }

    #endregion

    #region Lines

    public static void AddLine(string text, Color color = Gray, bool wordWrap = false, string id = "")
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.lines.Count > 0 && region.smallButtons.Count > 0) return;
        var newObject = new GameObject("Line", typeof(Line));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<Line>().Initialise(region);
        AddText(text, color);
        if (id != "") MarkLineGlobal(id);
    }

    public static void AddPaginationLine()
    {
        var regionGroup = CDesktop.LBWindow.LBRegionGroup;
        AddLine("Page: " + (regionGroup.pagination + 1) + " / " + (int)Math.Ceiling((double)regionGroup.regionList.count() / regionGroup.regionList.regions.Count), DarkGray);
    }

    public static void MarkLineGlobal(string id)
    {
        if (CDesktop.globalLines.ContainsKey(id))
            CDesktop.globalLines.Remove(id);
        CDesktop.globalLines.Add(id, CDesktop.LBWindow.LBRegionGroup.LBRegion.LBLine);
    }

    #endregion

    #region SmallButtons

    public static void SetSmallButtonToGrayscale()
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBSmallButton.gameObject;
        button.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Grayscale");
    }

    public static void AddSmallButtonOverlay(string overlay, float time = 0, int sortingOrder = 1)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBSmallButton.gameObject;
        AddSmallButtonOverlay(button, overlay, time, sortingOrder);
    }

    public static void AddSmallButtonOverlay(GameObject onWhat, string overlay, float time = 0, int sortingOrder = 1)
    {
        var newObject = new GameObject("SmallButtonOverlay", typeof(SpriteRenderer));
        newObject.transform.parent = onWhat.transform;
        newObject.transform.localPosition = Vector3.zero;
        if (overlay == "Cooldown") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Building/Shadows/Cooldown");
        else newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Buttons/" + overlay);
        newObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = "Upper";
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(0.1f);
        }
    }

    public static void AddSmallButton(string type, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip = null)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        //if (region.lines.Count > 1) return;
        var newObject = new GameObject("SmallButton: " + type.ToString(), typeof(LineSmallButton), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineSmallButton>().Initialise(region, type, pressEvent, tooltip);
    }

    public static void AddNextPageButton()
    {
        var regionGroup = CDesktop.LBWindow.LBRegionGroup;
        AddSmallButton("OtherNextPage",
            (h) =>
            {
                if ((int)Math.Ceiling((double)regionGroup.regionList.count() / regionGroup.regionList.regions.Count) - 1 > regionGroup.pagination)
                {   
                    regionGroup.pagination++;
                    regionGroup.window.Rebuild();
                }
            }
        );
    }

    public static void AddPreviousPageButton()
    {
        var regionGroup = CDesktop.LBWindow.LBRegionGroup;
        AddSmallButton("OtherPreviousPage",
            (h) =>
            {
                if (regionGroup.pagination > 0)
                {
                    regionGroup.pagination--;
                    regionGroup.window.Rebuild();
                }
            }
        );
    }

    //public static void MarkLineGlobal(string id)
    //{
    //    if (CDesktop.globalLines.ContainsKey(id))
    //        CDesktop.globalLines.Remove(id);
    //    CDesktop.globalLines.Add(id, CDesktop.LBWindow.LBRegionGroup.LBRegion.LBLine);
    //}

    #endregion

    #region BigButtons

    public static void SetBigButtonToGrayscale()
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBBigButton.gameObject;
        button.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Grayscale");
    }

    public static GameObject AddBigButtonOverlay(string overlay, float time = 0, int sortingOrder = 1)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBBigButton.gameObject;
        return AddBigButtonOverlay(button, overlay, time, sortingOrder);
    }

    public static GameObject AddBigButtonOverlay(GameObject onWhat, string overlay, float time = 0, int sortingOrder = 1)
    {
        var newObject = new GameObject("BigButtonGrid", typeof(SpriteRenderer));
        newObject.transform.parent = onWhat.transform;
        newObject.transform.localPosition = Vector3.zero;
        newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/BigButtons/" + overlay);
        newObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = "Upper";
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(time);
        }
        return newObject;
    }

    public static void AddBigButton(string type, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip = null)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var newObject = new GameObject("BigButton: " + (type == null ? "Empty" : type.ToString()), typeof(LineBigButton), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineBigButton>().Initialise(region, type, pressEvent, tooltip);
    }

    #endregion

    #region Dropdowns

    public static void AddDropdown(Func<string, string> headerChange, Func<List<string>> choices)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.dropdown != null || region.checkbox != null) return;
        region.gameObject.AddComponent<LineDropdown>().Initialise(headerChange, choices);
    }

    //public static void MarkLineGlobal(string id)
    //{
    //    if (CDesktop.globalLines.ContainsKey(id))
    //        CDesktop.globalLines.Remove(id);
    //    CDesktop.globalLines.Add(id, CDesktop.LBWindow.LBRegionGroup.LBRegion.LBLine);
    //}

    #endregion

    #region Checkboxes

    public static void AddCheckbox(Bool value)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.checkbox != null || region.dropdown != null) return;
        var newObject = new GameObject("Checkbox", typeof(LineCheckbox), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineCheckbox>().Initialise(region, value);
    }

    //public static void MarkLineGlobal(string id)
    //{
    //    if (CDesktop.globalLines.ContainsKey(id))
    //        CDesktop.globalLines.Remove(id);
    //    CDesktop.globalLines.Add(id, CDesktop.LBWindow.LBRegionGroup.LBRegion.LBLine);
    //}

    #endregion

    #region Text

    public static void AddText(string text = "", Color color = Gray)
    {
        var newObject = new GameObject("Text", typeof(LineText));
        var line = CDesktop.LBWindow.LBRegionGroup.LBRegion.LBLine;
        newObject.transform.parent = line.transform;
        newObject.GetComponent<LineText>().Initialise(line, text, color);
    }

    public static void MarkTextGlobal(string id)
    {
        if (CDesktop.globalTexts.ContainsKey(id))
            CDesktop.globalTexts.Remove(id);
        CDesktop.globalTexts.Add(id, CDesktop.LBWindow.LBRegionGroup.LBRegion.LBLine.LBText);
    }

    #endregion

    #region InputLines

    public static void AddInputLine(String refText, InputType inputType, string id)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.lines.Count > 0 && region.checkbox != null) return;
        var newObject = new GameObject("InputLine", typeof(InputLine));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<InputLine>().Initialise(region, refText, inputType);
        MarkInputLineGlobal(id);
    }

    //public static void AddInputLine(InputType inputType = InputType.Everything, string defaultInput = "")
    //{
    //    var newObject = new GameObject("InputLine", typeof(InputLine));
    //    newObject.transform.parent = CDesktop.LBRegion.transform;
    //    newObject.transform.localPosition = new Vector3(0, CDesktop.LBRegion.yOffset, 0);
    //    CDesktop.LBRegion.yOffset -= 15;
    //    CDesktop.currentInputLine = newObject.GetComponent<InputLine>();
    //    CDesktop.currentInputLine.inputType = inputType;
    //    newObject = new GameObject("Text", typeof(Text));
    //    newObject.transform.parent = CDesktop.currentInputLine.transform;
    //    newObject.transform.localPosition = new Vector3();
    //    var newText = newObject.GetComponent<Text>();
    //    newText.Initialise(defaultInput, LightGray);
    //    CDesktop.currentText = newText;
    //    CDesktop.currentInputLine.text = newText;
    //    CDesktop.LBRegion.inputLines.Add(CDesktop.currentInputLine);
    //}

    public static void MarkInputLineGlobal(string id)
    {
        if (CDesktop.globalInputLines.ContainsKey(id))
            CDesktop.globalInputLines.Remove(id);
        CDesktop.globalInputLines.Add(id, CDesktop.LBWindow.LBRegionGroup.LBRegion.inputLine);
    }

    #endregion

    #region Enumerations

    public enum Speed
    {
        UltraFast,
        VeryFast,
        Fast,
        Average,
        Slow,
        VerySlow
    }

    public enum Race
    {
        Lizardmen,
        TheEmpire,
        VampireCounts,
        VampireCoast,
        Greenskins,
        Dwarfs,
        DarkElves,
        HighElves,
        Skaven,
        Norsca
    }

    public enum WindofMagic
    {
        Ghyran,
        Azyr,
        Ulgu,
        Shyish,
        Aqshy,
        Ghur,
        Hysh,
        Chamon
    }

    public enum InputType
    {
        Everything,
        Letters,
        Numbers
    }

    public enum RegionBackgroundType
    {
        Handle,
        Button,
        Header,
        Padding,
        RedButton
    }

    public enum Anchor
    {
        None,
        Center,
        Bottom,
        BottomRight,
        BottomLeft,
        Top,
        TopRight,
        TopLeft,
        LeftBottom,
        Left,
        LeftTop,
        RightBottom,
        Right,
        RightTop
    }

    //public static Color32 GetColor(Color color) => colors[color];
    //public static Dictionary<Color, Color32> colors = new()
    //{
    //    { White, new Color32(234, 234, 234, 255) },
    //    { LightGray, new Color32(202, 202, 202, 255) },
    //    { Gray, new Color32(183, 183, 183, 255) },
    //    { DarkGray, new Color32(114, 114, 114, 255) },
    //    { Black, new Color32(31, 31, 31, 255) },
    //    { Red, new Color32(181, 77, 77, 255) },
    //    { Green, new Color32(73, 178, 86, 255) }
    //};

    public static Color32 GetColor(Color color) => colors[0][color];
    public static Dictionary<Color, Color32>[] colors = new Dictionary<Color, Color32>[]
    {
        new ()
        {
            { White, new Color32(234, 234, 234, 255) },
            { LightGray, new Color32(202, 202, 202, 255) },
            { Gray, new Color32(183, 183, 183, 255) },
            { DarkGray, new Color32(114, 114, 114, 255) },
            { Black, new Color32(31, 31, 31, 255) },
            { Red, new Color32(181, 77, 77, 255) },
            { DangerousRed, new Color32(219, 48, 48, 255) },
            { Yellow, new Color32(181, 159, 77, 255) },
            { Orange, new Color32(185, 104, 57, 255) },
            { Green, new Color32(81, 181, 77, 255) },
            { Druid, new Color32(184, 90, 7, 255) },
            { Warrior, new Color32(144, 113, 79, 255) },
            { Rogue, new Color32(184, 177, 76, 255) },
            { Hunter, new Color32(124, 153, 83, 255) },
            { Mage, new Color32(45, 144, 170, 255) },
            { Shaman, new Color32(0, 81, 160, 255) },
            { Warlock, new Color32(97, 98, 172, 255) },
            { Paladin, new Color32(177, 101, 134, 255) },
            { Priest, new Color32(191, 175, 164, 255) },
            { Copper, new Color32(184, 80, 41, 255) },
            { Silver, new Color32(170, 188, 210, 255) },
            { Gold, new Color32(255, 210, 11, 255) },
            { Poor, new Color32(114, 114, 114, 255) },
            { Common, new Color32(183, 183, 183, 255) },
            { Uncommon, new Color32(26, 201, 0, 255) },
            { Rare, new Color32(0, 117, 226, 255) },
            { Epic, new Color32(163, 53, 238, 255) },
            { Legendary, new Color32(221, 110, 0, 255) }
        },
        new ()
        {
            { White, new Color32(234, 234, 234, 255) },
            { LightGray, new Color32(202, 202, 202, 255) },
            { Gray, new Color32(183, 183, 183, 255) },
            { DarkGray, new Color32(114, 114, 114, 255) },
            { Black, new Color32(31, 31, 31, 255) },
            { TrueBlack, new Color32(0, 0, 0, 255) },
            { Red, new Color32(181, 77, 77, 255) },
            { Green, new Color32(73, 178, 86, 255) }
        }
    };

    public enum Color
    {
        White,
        LightGray,
        Gray,
        DarkGray,
        Black,
        TrueBlack,
        Red,
        DangerousRed,
        DarkRed,
        Green,
        DarkGreen,
        Blue,
        DarkBlue,
        Yellow,
        Brown,
        Pink,
        Purple,
        Orange,

        Druid,
        Warrior,
        Rogue,
        Hunter,
        Mage,
        Shaman,
        Warlock,
        Paladin,
        Priest,

        Copper,
        Silver,
        Gold,

        Poor,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum CursorType
    {
        None,
        Default,
        Click,
        Grab,
        Await,
        Write
    }

    #endregion
}
