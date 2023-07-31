using System;
using UnityEngine;
using System.Collections.Generic;

using static Font;
using static Blueprint;

using static Root.Color;
using static Root.Anchor;
using static Root.RegionBackgroundType;

public static class Root
{
    public static int screenX = 640;
    public static int screenY = 360;
    public static int shadowSystem = 1;
    public static int aiDepth = 6;

    public static Cursor cursor;
    public static CursorRemote cursorEnemy;
    public static int inputLineMarker;
    public static System.Random random;
    public static int keyStack;
    public static int titleScreenCameraDirection;
    public static float heldKeyTime;
    public static float animationTime;
    public static float frameTime = 0.07f;
    public static List<Desktop> desktops;
    public static Desktop CDesktop, LBDesktop;
    public static string markerCharacter = "_", currentInputLine = "";
    public static List<(string, Vector2)> windowRemoteAnchors;

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

    public static GameObject SpawnShatterBuff(double speed, double amount, Vector3 position, string sprite, Entity target, bool oneDirection = false, string block = "0000")
    {
        var buff = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabBuff"));
        buff.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Buttons/" + sprite);
        buff.transform.parent = Board.board.window.desktop.transform;
        buff.transform.position = position;
        buff.GetComponent<FlyingBuff>().Initiate(currentSave.player == target, (h) => { },
            (h) => () =>
            {
                var fb = h.GetComponent<FlyingBuff>();
                var buff = (fb.onPlayer ? Board.board.player.buffs : Board.board.enemy.buffs).Find(x => x.Item3 == h.gameObject);
                var buffObj = Buff.buffs.Find(x => x.name == buff.Item1);
                SetAnchor(Top, 0, -13);
                AddHeaderGroup();
                SetRegionGroupWidth(256);
                SetRegionGroupHeight(237);
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
                buffObj.description();
                AddRegionGroup();
                SetRegionGroupWidth(256);
                AddPaddingRegion(() => { AddLine(""); });
            }
        );
        SpawnShatter(speed, amount, position, sprite, oneDirection, block);
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
        for (int i = 6; i < x - 5; i++)
            for (int j = 6; j < y - 5; j++)
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

    public static void PrintSite(string name, string type, Vector2 anchor)
    {
        SetAnchor(anchor.x, anchor.y);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            if (type == "Town")
            {
                var town = SiteTown.towns.Find(x => x.name == name);
                if (town == null) { Debug.LogError("No town named \"" + name + "\" has been found."); return; }
                AddSmallButton("Faction" + town.faction,
                (h) =>
                {

                },
                (h) => () =>
                {
                    SetAnchor(TopRight, h.window);
                    AddRegionGroup();
                    AddHeaderRegion(() =>
                    {
                        AddLine(name, Gray);
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
            }
            else
                AddSmallButton("Site" + type,
                (h) =>
                {
                    var find = SiteHostileArea.hostileAreas.Find(x => x.name == name);
                    if (find != null)
                    {
                        Board.board = new Board(7, 7, find.RollEncounter());
                        SpawnDesktopBlueprint("Game");
                        SwitchDesktop("Game");
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
                    SetRegionGroupWidth(256 - (abilityObj == null ? 0 : abilityObj.cost.Count) * 44);
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
                    CDesktop.LBWindow.LBRegionGroup.LBRegion.LBBigButton.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Grayscale");
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
        newDesktop.screenlock.transform.parent = newObject.transform;
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
        }
    }

    public static void SetDesktopBackground(string texture, bool followCamera = true)
    {
        if (followCamera) LBDesktop.screen.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/" + texture);
        else LBDesktop.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/" + texture);
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

    public static void AddLine(string text, Color color = Gray, string id = "")
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

    public static void AddSmallButtonOverlay(string overlay, float time = 0)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBSmallButton.gameObject;
        AddSmallButtonOverlay(button, overlay, time);
    }

    public static void AddSmallButtonOverlay(GameObject onWhat, string overlay, float time = 0)
    {
        var newObject = new GameObject("SmallButtonOverlay", typeof(SpriteRenderer));
        newObject.transform.parent = onWhat.transform;
        newObject.transform.localPosition = Vector3.zero;
        newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Buttons/" + overlay);
        newObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(0.1f);
        }
    }

    public static void AddSmallButton(string type, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip = null)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.lines.Count > 1) return;
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

    public static GameObject AddBigButtonOverlay(string overlay)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBBigButton.gameObject;
        var newObject = new GameObject("BigButtonGrid", typeof(SpriteRenderer));
        newObject.transform.parent = button.transform;
        newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/BigButtons/" + overlay);
        newObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        return newObject;
    }

    public static void AddBigButton(string type, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip = null)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.lines.Count > 1) return;
        var newObject = new GameObject("BigButton: " + type.ToString(), typeof(LineBigButton), typeof(SpriteRenderer));
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
        Padding
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
            { Green, new Color32(74, 154, 26, 255) },
            { Druid, new Color32(184, 90, 7, 255) },
            { Warrior, new Color32(144, 113, 79, 255) },
            { Rogue, new Color32(184, 177, 76, 255) },
            { Hunter, new Color32(124, 153, 83, 255) },
            { Mage, new Color32(45, 144, 170, 255) },
            { Shaman, new Color32(0, 81, 160, 255) },
            { Warlock, new Color32(97, 98, 172, 255) },
            { Paladin, new Color32(177, 101, 134, 255) },
            { Priest, new Color32(184, 184, 184, 255) },
            { Copper, new Color32(184, 80, 41, 255) },
            { Silver, new Color32(170, 188, 210, 255) },
            { Gold, new Color32(255, 210, 11, 255) }
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
        Gold
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
