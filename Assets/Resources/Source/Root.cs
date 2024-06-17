using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Sound;
using static Coloring;
using static Blueprint;
using static GameSettings;

using static Root.Anchor;
using static Root.RegionBackgroundType;

public static class Root
{
    //Program constants
    public static int screenX = 640;
    public static int screenY = 360;

    public static MapGrid grid;
    public static List<Vector2> cameraBoundaryPoints;
    public static List<FallingElement> fallingElements;
    public static List<FlyingMissile> flyingMissiles;
    public static bool canUnlockScreen;
    public static bool useUnityData = true;
    public static bool showSitesUnconditional;
    public static bool showAreasUnconditional;
    public static bool disableCameraBounds;
    public static int builderSpacing;
    public static string prefix = "";

    public static string chartPage;
    public static Region iconRow;

    public static string locationName;
    public static string creationRace;
    public static string creationSpec;
    public static string creationGender;

    public static Highlightable mouseOver;
    public static GameObject[] loadingBar;
    public static int loadingScreenObjectLoad;
    public static int loadingScreenObjectLoadAim;
    public static List<Blueprint> loadSites;

    public static Color32 dayColor = new(255, 255, 255, 255);
    public static Color32 nightColor = new(185, 185, 202, 255);

    public static Action splitDelegate;
    public static string[,] groundData;
    public static System.Random random;
    public static int inputLineMarker;
    public static int keyStack;
    public static float lastFunnyEffectTime;
    public static Vector3 lastFunnyEffectPosition;
    public static List<(int, int)> titleScreenFunnyEffect = new();
    public static float heldKeyTime;
    public static float animationTime;
    public static float animatedSpriteTime;
    public static List<Desktop> desktops;
    public static Desktop CDesktop, LBDesktop;
    public static List<Dictionary<string, string>> triggersCopy, effectsCopy;

    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count; i > 1;)
        {
            int rnd = random.Next(i--);
            T value = list[rnd];
            list[rnd] = list[i];
            list[i] = value;
        }
    }

    public static float Grayscale(this Color32 color) => (0.299f * color.r) + (0.587f * color.g) + (0.114f * color.b);

    public static List<Item> Multilate(this List<Item> list, int times)
    {
        var output = list.ToList();
        for (int i = 0; i < times - 1; i++)
            output.AddRange(list.Select(x => x.CopyItem(x.amount)));
        return output;
    }

    public static Dictionary<T, U> Merge<T, U>(this Dictionary<T, U> A, Dictionary<T, U> B)
    {
        var temp = A.ToDictionary(x => x.Key, x => x.Value);
        foreach (var pair in B)
            if (temp.ContainsKey(pair.Key)) continue;
            else temp.Add(pair.Key, pair.Value);
        return temp;
    }

    public static void Inc<TKey>(this Dictionary<TKey, int> dic, TKey source, int amount = 1)
    {
        if (dic.ContainsKey(source)) dic[source] += amount;
        else dic.Add(source, amount);
    }

    public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey source)
    {
        if (dic.ContainsKey(source)) return dic[source];
        else return default;
    }

    public static string ToRoman(int number)
    {
        if (number < 0 || number > 3999) return "";
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        return "";
    }

    public static Blueprint FindWindowBlueprint(string name)
    {
        var find = windowBlueprints.Find(x => x.title == name);
        find ??= BlueprintDev.windowBlueprints.Find(x => x.title == name);
        return find;
    }

    public static Blueprint FindDesktopBlueprint(string name)
    {
        var find = desktopBlueprints.Find(x => x.title == name);
        find ??= BlueprintDev.desktopBlueprints.Find(x => x.title == name);
        return find;
    }

    public static T Copy<T>(this object obj) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(obj));

    public static List<(string, string, string)> TrimLast(this List<(string, string, string)> list, bool should)
    {
        if (should == false) return list;
        list[^1] = (list[^1].Item1.TrimEnd(), list[^1].Item2, list[^1].Item3);
        return list;
    }

    public static bool Roll(double chance) => random.Next(0, 100000) < chance * 1000;

    public static string Clean(this string text) => text?.Replace("'", "").Replace(".", "").Replace(" ", "");

    public static void SetMouseOver(Highlightable highlightable) => mouseOver = highlightable;

    #region Desktop

    public static void SpawnDesktopBlueprint(string blueprintTitle, bool autoSwitch = true)
    {
        var blueprint = FindDesktopBlueprint(blueprintTitle);
        if (blueprint == null) return;
        var spawnedNew = false;
        if (!desktops.Exists(x => x.title == blueprintTitle))
            { AddDesktop(blueprint.title); spawnedNew = true; }
        if (autoSwitch) SwitchDesktop(blueprintTitle);
        if (spawnedNew) blueprint.actions();
    }

    public static bool CloseDesktop(string desktopName)
    {
        var find = desktops.Find(x => x.title == desktopName);
        if (find == null) return false;
        desktops.Remove(find);
        if (find == CDesktop)
            SwitchDesktop(desktops[0].title);
        UnityEngine.Object.Destroy(find.gameObject);
        return true;
    }

    private static void AddDesktop(string title)
    {
        var newObject = new GameObject("Desktop: " + title, typeof(Desktop), typeof(SpriteRenderer));
        newObject.transform.localPosition = new Vector3();
        var newDesktop = newObject.GetComponent<Desktop>();
        LBDesktop = newDesktop;
        newDesktop.Initialise(title);
        desktops.Add(newDesktop);
        newDesktop.screen = new GameObject("Camera", typeof(Camera), typeof(SpriteRenderer)).GetComponent<Camera>();
        newDesktop.screen.transform.parent = newDesktop.transform;
        var screenOffsetter = new GameObject("CameraOffset");
        screenOffsetter.transform.parent = newDesktop.transform;
        screenOffsetter.transform.localPosition = new Vector2(10, -9);
        newDesktop.screen.transform.parent = screenOffsetter.transform;
        newDesktop.GetComponent<SpriteRenderer>().sortingLayerName = "DesktopBackground";
        newDesktop.screen.GetComponent<SpriteRenderer>().sortingLayerName = "DesktopBackground";
        newDesktop.screen.orthographicSize = 180;
        newDesktop.screen.nearClipPlane = -1024;
        newDesktop.screen.farClipPlane = 4096;
        newDesktop.screen.clearFlags = CameraClearFlags.SolidColor;
        newDesktop.screen.backgroundColor = new Color32(0, 29, 41, 255);
        newDesktop.screen.orthographic = true;
        if (settings.pixelPerfectVision.Value()) newDesktop.screen.gameObject.AddComponent<PixelCamera>();
        var cameraBorder = new GameObject("CameraBorder", typeof(SpriteRenderer));
        var cameraShadow = new GameObject("CameraShadow", typeof(SpriteRenderer));
        cameraShadow.transform.parent = cameraBorder.transform.parent = newDesktop.screen.transform;
        cameraBorder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Fullscreen/Camera/CameraBorder");
        cameraShadow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Fullscreen/Camera/CameraShadow");
        cameraBorder.GetComponent<SpriteRenderer>().sortingLayerName = "CameraBorder";
        cameraShadow.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
        newDesktop.screenlock = new GameObject("Screenlock", typeof(BoxCollider2D), typeof(SpriteRenderer));
        newDesktop.screenlock.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Camera/CameraScreenlock");
        newDesktop.screenlock.GetComponent<SpriteRenderer>().sortingLayerName = "DesktopBackground";
        newDesktop.screenlock.GetComponent<SpriteRenderer>().sortingOrder = 1;
        newDesktop.screenlock.GetComponent<BoxCollider2D>().size = new Vector2(640, 360);
        newDesktop.screenlock.transform.parent = newDesktop.screen.transform;
        newDesktop.UnlockScreen();
        newObject.SetActive(false);
    }

    public static void SwitchDesktop(string name)
    {
        if (CDesktop != null && CDesktop.title == name) return;
        var windows = CDesktop != null ? CDesktop.windows.Select(x => x.title).ToList() : null;
        if (mouseOver != null)
            mouseOver.OnMouseExit();
        if (CDesktop != null)
            CDesktop.gameObject.SetActive(false);
        var find = desktops.Find(x => x.title == name);
        if (find != null) CDesktop = find;
        if (CDesktop != null)
        {
            if (name == "Map") grid.UpdateTextureColors(true);
            CDesktop.gameObject.SetActive(true);
            desktops.Remove(CDesktop);
            desktops.Insert(0, CDesktop);
            SpawnTransition();
        }
        if (CDesktop.cameraDestination != Vector2.zero)
        {
            Cursor.cursor.transform.position += (Vector3)CDesktop.cameraDestination - CDesktop.screen.transform.localPosition;
            CDesktop.screen.transform.localPosition = (Vector3)CDesktop.cameraDestination;
        }
        if (windows != null)
            foreach (var window in windows)
                Respawn(window, true);
        if (!settings.pixelPerfectVision.Value() && CDesktop.screen.GetComponent<PixelCamera>() != null) UnityEngine.Object.Destroy(CDesktop.screen.GetComponent<PixelCamera>());
        else if (settings.pixelPerfectVision.Value() && CDesktop.screen.GetComponent<PixelCamera>() == null) CDesktop.screen.gameObject.AddComponent<PixelCamera>();
    }

    public static void OrderLoadingMap()
    {
        Quest.sitesToRespawn = new();
        Quest.sitesWithQuestMarkers = new();
        cameraBoundaryPoints = new();
        loadSites = windowBlueprints.FindAll(x => x.title.StartsWith("Site: "));
        if (!showSitesUnconditional)
            for (int i = loadSites.Count - 1; i >= 0; i--)
            {
                var site = Site.FindSite(x => "Site: " + x.name == loadSites[i].title);
                if (site != null && !site.CanBeSeen())
                {
                    cameraBoundaryPoints.Add(new Vector2(site.x, site.y));
                    loadSites.RemoveAt(i);
                }
            }
        loadingScreenObjectLoad = 0;
        loadingScreenObjectLoadAim = loadSites.Count;
    }

    public static void SpawnTransition(bool single = true, float speed = 2f)
    {
        if (CDesktop.transition != null && single) return;
        var transition = new GameObject("CameraTransition", typeof(SpriteRenderer), typeof(Shatter));
        transition.transform.parent = CDesktop.screen.transform;
        transition.transform.localPosition = new Vector3(0, 0, -0.01f);
        transition.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Fullscreen/Camera/CameraTransition");
        transition.GetComponent<SpriteRenderer>().sortingLayerName = "CameraBorder";
        transition.GetComponent<Shatter>().Initiate(speed, 0, transition.GetComponent<SpriteRenderer>());
        CDesktop.transition = transition;
    }

    public static void RemoveDesktopBackground(bool followCamera = true)
    {
        if (followCamera) CDesktop.screen.GetComponent<SpriteRenderer>().sprite = null;
        else CDesktop.GetComponent<SpriteRenderer>().sprite = null;
    }

    public static void SetDesktopBackground(string texture, bool followCamera = true)
    {
        var sprite = Resources.Load<Sprite>("Sprites/Fullscreen/" + texture);
        var temp = (followCamera ? CDesktop.screen.gameObject : CDesktop.gameObject).GetComponent<SpriteRenderer>();
        if (sprite == null) Debug.Log("ERROR 004: Desktop background not found: \"Sprites/Fullscreen/" + texture + "\"");
        if (temp.sprite != sprite)
        {
            SpawnTransition();
            temp.sprite = sprite;
        }
    }

    //Hotkeys can be added only on desktop creation!
    public static void AddHotkey(KeyCode key, Action action, bool keyDown = true)
    {
        if (LBDesktop.hotkeys.Exists(x => x.key == key && x.keyDown == keyDown)) return;
        LBDesktop.hotkeys.Add(new Hotkey(key, action, keyDown));
    }

    public static float EuelerGrowth()
    {
        return (float)Math.Pow(keyStack / 150.0 + 1.0, Math.E);
    }

    #endregion

    #region Windows

    public static Window SpawnWindowBlueprint(string blueprintTitle, bool resetSearch = true)
    {
        return SpawnWindowBlueprint(FindWindowBlueprint(blueprintTitle), resetSearch);
    }

    public static Window SpawnWindowBlueprint(Blueprint blueprint, bool resetSearch = true)
    {
        if (blueprint == null) return null;
        if (CDesktop.windows.Exists(x => x.title == blueprint.title)) return null;
        AddWindow(blueprint.title, blueprint.upperUI);
        blueprint.actions();
        if (resetSearch && CDesktop.LBWindow.regionGroups.Any(x => x.maxPaginationReq != null)) String.search.Set("");
        CDesktop.LBWindow.Rebuild();
        CDesktop.LBWindow.ResetPosition();
        return CDesktop.LBWindow;
    }

    public static void AddWindow(string title, bool upperUI)
    {
        var newObject = new GameObject("Window: " + title, typeof(Window));
        newObject.transform.parent = CDesktop.transform;
        newObject.GetComponent<Window>().Initialise(CDesktop, title, upperUI);
    }

    public static bool Respawn(string windowName, bool onlyWhenActive = false)
    {
        var window = CDesktop.windows.Find(x => x.title == windowName);
        bool wasThere = window != null;
        if (wasThere) window.Respawn(onlyWhenActive);
        else if (!onlyWhenActive) SpawnWindowBlueprint(windowName, true);
        return wasThere;
    }

    public static bool CloseWindow(string windowName, bool resetPagination = true)
    {
        return CloseWindow(CDesktop.windows.Find(x => x.title == windowName), resetPagination);
    }

    public static bool CloseWindow(Window window, bool resetPagination = true)
    {
        if (window == null) return false;
        if (resetPagination && staticPagination.ContainsKey(window.title))
            staticPagination.Remove(window.title);
        CDesktop.windows.Remove(window);
        UnityEngine.Object.Destroy(window.gameObject);
        return true;
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

    public static void DisableCollisions()
    {
        CDesktop.LBWindow.disabledCollisions = true;
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

    public static void AddHeaderGroup(Func<double> maxPagination = null, int perPage = 10)
    {
        var newObject = new GameObject("HeaderGroup", typeof(RegionGroup));
        newObject.transform.parent = CDesktop.LBWindow.transform;
        newObject.GetComponent<RegionGroup>().Initialise(CDesktop.LBWindow, true, maxPagination, perPage);
    }

    public static void AddRegionGroup(Func<double> maxPagination = null, int perPage = 10)
    {
        var newObject = new GameObject("RegionGroup", typeof(RegionGroup));
        newObject.transform.parent = CDesktop.LBWindow.transform;
        newObject.GetComponent<RegionGroup>().Initialise(CDesktop.LBWindow, false, maxPagination, perPage);
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

    #region Regions

    private static void AddRegion(RegionBackgroundType backgroundType, Action draw, Action<Highlightable> pressEvent, Action<Highlightable> rightPressEvent, Func<Highlightable, Action> tooltip, Action<Highlightable> middlePressEvent)
    {
        var region = new GameObject("Region", typeof(Region)).GetComponent<Region>();
        var regionGroup = CDesktop.LBWindow.LBRegionGroup;
        region.transform.parent = regionGroup.transform;
        region.background = new GameObject("Background", typeof(SpriteRenderer), typeof(RegionBackground));
        region.background.transform.parent = region.transform;
        region.Initialise(regionGroup, backgroundType, draw);
        if (pressEvent != null || rightPressEvent != null || middlePressEvent != null || tooltip != null)
            region.background.AddComponent<Highlightable>().Initialise(region, pressEvent, rightPressEvent, tooltip, middlePressEvent);
    }

    public static void AddRegionOverlay(Region onWhat, string overlay, float time = 0)
    {
        var newObject = new GameObject("RegionOverlay", typeof(SpriteRenderer));
        newObject.transform.parent = onWhat.transform;
        newObject.transform.localPosition = onWhat.background.transform.localPosition - new Vector3(0, 0, 0.1f);
        newObject.transform.localScale = onWhat.background.transform.localScale - new Vector3(19 * onWhat.smallButtons.Count, 0);
        newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Fills/" + overlay);
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(time);
        }
    }

    public static void AddButtonRegion(Action draw, Action<Highlightable> pressEvent = null, Action<Highlightable> rightPressEvent = null, Func<Highlightable, Action> tooltip = null, Action<Highlightable> middlePressEvent = null)
    {
        AddRegion(Button, draw, pressEvent, rightPressEvent, tooltip, middlePressEvent);
    }

    public static void AddEmptyRegion()
    {
        AddRegion(Empty, () => { AddLine(""); }, null, null, null, null);
    }

    public static void AddHeaderRegion(Action draw)
    {
        AddRegion(Header, draw, null, null, null, null);
    }

    public static void AddPaddingRegion(Action draw)
    {
        AddRegion(Padding, draw, null, null, null, null);
    }

    public static void AddPaginationLine(RegionGroup group)
    {
        AddPaddingRegion(() =>
        {
            AddLine("Page: ", "DarkGray");
            AddText(group.pagination() + 1 + "");
            AddText(" / ", "DarkGray");
            AddText(group.maxPagination() + "");
            AddSmallButton("OtherNextPage", (h) =>
            {
                if (group.pagination() < group.maxPagination() - 1)
                {
                    PlaySound("DesktopChangePage", 0.4f);
                    group.IncrementPagination();
                }
            });
            AddSmallButton("OtherPreviousPage", (h) =>
            {
                if (group.pagination() > 0)
                {
                    PlaySound("DesktopChangePage", 0.4f);
                    group.DecrementPagination();
                }
            });
        });
    }

    public static void ReverseButtons()
    {
        CDesktop.LBWindow.LBRegionGroup.LBRegion.reverseButtons ^= true;
    }

    public static void SetRegionBackgroundToGrayscale()
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        region.background.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Grayscale");
    }

    //When other region groups are lenghier than the
    //one this region is in then the unique extender will
    //be extended to match length of the other group regions
    public static void SetRegionAsGroupExtender()
    {
        var temp = CDesktop.LBWindow.LBRegionGroup;
        temp.stretchRegion = temp.LBRegion;
    }

    public static void SetRegionBackground(RegionBackgroundType backgroundType)
    {
        CDesktop.LBWindow.LBRegionGroup.LBRegion.backgroundType = backgroundType;
    }

    public static void SetRegionBackgroundAsImage(string replacement)
    {
        SetRegionBackground(Image);
        CDesktop.LBWindow.LBRegionGroup.LBRegion.backgroundImage = Resources.Load<Sprite>("Sprites/RegionReplacements/" + replacement);
    }

    public static void PrintPriceRegion(int price)
    {
        int width = CDesktop.LBWindow.LBRegionGroup.setWidth;
        var lacking = 0;
        if (price > 9999) Foo("ItemCoinsGold", price / 10000 + "", "Gold"); else lacking++;
        if (price / 100 % 100 > 0) Foo("ItemCoinsSilver", price / 100 % 100 + "", "Silver"); else lacking++;
        if (price % 100 > 0 || price == 0) Foo("ItemCoinsCopper", price % 100 + "", "Copper"); else lacking++;
        AddRegionGroup();
        SetRegionGroupWidth(width - (3 - lacking) * 54);
        AddPaddingRegion(() => { AddLine(""); });

        void Foo(string icon, string text, string color)
        {
            AddRegionGroup();
            AddPaddingRegion(() => { AddSmallButton(icon); });
            AddRegionGroup();
            SetRegionGroupWidth(35);
            AddPaddingRegion(() => { AddLine(text, color); });
        }
    }

    #endregion

    #region Lines

    public static void AddLine(string text = "", string color = "", string align = "Left")
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.lines.Count > 0 && region.smallButtons.Count > 0) return;
        var newObject = new GameObject("Line", typeof(Line));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<Line>().Initialise(region, align);
        AddText(text, color);
    }

    public static string DefaultTextColorForRegion(RegionBackgroundType type)
    {
        if (type == Header) return "Gray";
        if (type == Padding) return "Gray";
        if (type == Button) return "Black";
        if (type == ButtonRed) return "Black";
        else return "Gray";
    }

    #endregion

    #region SmallButtons

    public static void SetSmallButtonToGrayscale()
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBSmallButton.gameObject;
        button.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Grayscale");
    }

    public static void AddSmallButtonOverlay(string overlay, float time = 0, int sortingOrder = 0)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBSmallButton.gameObject;
        AddSmallButtonOverlay(button, overlay, time, sortingOrder);
    }

    public static void SmallButtonFlipX()
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBSmallButton.gameObject;
        button.GetComponent<SpriteRenderer>().flipX ^= true;
    }

    public static void SmallButtonFlipY()
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBSmallButton.gameObject;
        button.GetComponent<SpriteRenderer>().flipY ^= true;
    }

    public static void AddSmallButtonOverlay(GameObject onWhat, string overlay, float time = 0, int sortingOrder = 0)
    {
        var newObject = new GameObject("SmallButtonOverlay", typeof(SpriteRenderer));
        newObject.transform.parent = onWhat.transform;
        newObject.transform.localPosition = new Vector3(overlay == "PlayerLocationFromBelow" || overlay == "PlayerLocationSmall" || overlay == "AvailableQuest" ? 1 : (overlay == "YellowGlowBig" ? 0.5f : 0), overlay == "AvailableQuest" ? -8.5f : (overlay == "PlayerLocationSmall" || overlay == "PlayerLocationFromBelow" ? -6f : (overlay == "YellowGlowBig" ? -0.5f : 0)), -0.01f);
        if (overlay == "Cooldown") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/Cooldown", true);
        else if (overlay == "YellowGlow") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/YellowGlow", true);
        else if (overlay == "YellowGlowBig") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/YellowGlowBig", false, 0.07f);
        else if (overlay == "AutoCast") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/AutoCastFull", true);
        else if (overlay == "PlayerLocationFromBelow") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/PlayerLocationFromBelow", false, 0.07f);
        else if (overlay == "PlayerLocationSmall") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/PlayerLocationSmall", false, 0.07f);
        else if (overlay == "AvailableQuest") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/AvailableQuest", false, 0.07f);
        else newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buttons/" + overlay);
        newObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = CDesktop.LBWindow.layer;
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(time);
        }
    }

    public static void AddSmallButton(string type, Action<Highlightable> pressEvent = null, Action<Highlightable> rightPressEvent = null, Func<Highlightable, Action> tooltip = null, Action<Highlightable> middlePressEvent = null)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var newObject = new GameObject("SmallButton: " + type.ToString(), typeof(LineSmallButton), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineSmallButton>().Initialise(region, type);
        if (pressEvent != null || rightPressEvent != null || tooltip != null)
            newObject.AddComponent<Highlightable>().Initialise(region, pressEvent, rightPressEvent, tooltip, middlePressEvent);
    }

    #endregion

    #region BigButtons

    public static void SetBigButtonToRed()
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBBigButton.gameObject;
        button.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Red");
    }

    public static void SetBigButtonToGrayscale()
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBBigButton.gameObject;
        button.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Grayscale");
    }

    public static GameObject AddBigButtonOverlay(string overlay, float time = 0, int sortingOrder = 0)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBBigButton.gameObject;
        return AddBigButtonOverlay(button, "Sprites/ButtonsBig/" + overlay, time, sortingOrder);
    }

    public static void BigButtonFlipX()
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBBigButton.gameObject;
        button.GetComponent<SpriteRenderer>().flipX ^= true;
    }

    public static void BigButtonFlipY()
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBBigButton.gameObject;
        button.GetComponent<SpriteRenderer>().flipY ^= true;
    }

    public static GameObject AddBigButtonCooldownOverlay(double percentage)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var button = region.LBBigButton.gameObject;
        var newObject = new GameObject("BigButtonGrid", typeof(SpriteRenderer));
        newObject.transform.parent = button.transform;
        newObject.transform.localPosition = new Vector3(0, 0, -0.01f);
        var sprites = Resources.LoadAll<Sprite>("Sprites/Other/CooldownBig");
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = CDesktop.LBWindow.layer;
        var value = 1.0 / sprites.Length;
        var first = 0;
        for (int i = 0; i < sprites.Length - 1; i++)
            if (percentage > value)
            {
                percentage -= value;
                first++;
            }
        newObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 164);
        newObject.GetComponent<SpriteRenderer>().sprite = sprites[Math.Abs(sprites.Length - 1 - first)];
        return newObject;
    }

    public static GameObject AddBigButtonOverlay(GameObject onWhat, string overlay, float time = 0, int sortingOrder = 0)
    {
        var newObject = new GameObject("BigButtonGrid", typeof(SpriteRenderer));
        newObject.transform.parent = onWhat.transform;
        newObject.transform.localPosition = new Vector3(0, 0, -0.01f);
        newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(overlay);
        newObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = CDesktop.LBWindow.layer;
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(time);
        }
        return newObject;
    }

    public static GameObject AddBigButtonOverlay(Vector2 position, string overlay, float time = 0, int sortingOrder = 0)
    {
        var newObject = new GameObject("BigButtonGrid", typeof(SpriteRenderer));
        newObject.transform.parent = CDesktop.transform;
        newObject.transform.localPosition = new Vector3(position.x, position.y, -0.01f);
        newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ButtonsBig/" + overlay);
        newObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = CDesktop.LBWindow.layer;
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(time);
        }
        return newObject;
    }

    public static void AddBigButton(string type, Action<Highlightable> pressEvent = null, Action<Highlightable> rightPressEvent = null, Func<Highlightable, Action> tooltip = null, Action<Highlightable> middlePressEvent = null)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var newObject = new GameObject("BigButton: " + (type == null ? "Empty" : type.ToString()), typeof(LineBigButton), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineBigButton>().Initialise(region, type);
        if (pressEvent != null || rightPressEvent != null || tooltip != null)
            newObject.AddComponent<Highlightable>().Initialise(region, pressEvent, rightPressEvent, tooltip, middlePressEvent);
    }

    #endregion

    #region Checkboxes

    public static void AddCheckbox(Bool value)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.checkbox != null) return;
        var newObject = new GameObject("Checkbox", typeof(LineCheckbox), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineCheckbox>().Initialise(region, value);
    }

    #endregion

    #region Text

    public static void AddText(string text = "", string color = "")
    {
        text ??= "";
        var newObject = new GameObject("Text", typeof(LineText));
        var line = CDesktop.LBWindow.LBRegionGroup.LBRegion.LBLine;
        newObject.transform.parent = line.transform;
        newObject.GetComponent<LineText>().Initialise(line, text, color == "" ? DefaultTextColorForRegion(line.region.backgroundType) : color);
    }

    public static void SpawnFloatingText(Vector2 position, string text = "", string color = "", string align = "Center")
    {
        text ??= "";
        var newObject = new GameObject("FloatingText", typeof(FloatingText));
        newObject.transform.parent = CDesktop.LBWindow.LBRegionGroup.LBRegion.transform;
        newObject.transform.localPosition = position;
        var temp = newObject.GetComponent<FloatingText>();
        temp.Initialise(text, color == "" ? "Gray" : color, align, false);
    }

    public static void SpawnFallingText(Vector2 position, string text = "", string color = "", string align = "Center")
    {
        text ??= "";
        var newObject = new GameObject("FallingText", typeof(FloatingText));
        newObject.transform.parent = CDesktop.transform;
        newObject.transform.localPosition = position;
        newObject.AddComponent<Rigidbody2D>().gravityScale = 2.5f;
        newObject.AddComponent<Shatter>().Initiate(5);
        var temp = newObject.GetComponent<FloatingText>();
        temp.Initialise(text, color == "" ? "Gray" : color, align);
    }

    #endregion

    #region InputLines

    public static void AddInputLine(String refText, string color = "", string align = "Left")
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.lines.Count > 0 && region.checkbox != null) return;
        var newObject = new GameObject("InputLine", typeof(InputLine));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<InputLine>().Initialise(region, refText, color, align);
    }

    #endregion

    #region Charts

    public static void AddChart()
    {
        Dictionary<string, int> dic = new();
        var rising = false;
        if (chartPage == "Damage Dealt") dic = Board.board.log.damageDealt;
        else if (chartPage == "Damage Taken") dic = Board.board.log.damageTaken;
        else if (chartPage == "Healing Received") dic = Board.board.log.healingReceived;
        else if (chartPage == "Elements Used") dic = Board.board.log.elementsUsed;
        var temp = dic.Count * (settings.chartBigIcons.Value() ? 38 : 19);
        if (temp == 0)
        {
            AddRegionGroup();
            SetRegionGroupWidth(600);
            SetRegionGroupHeight(242);
            AddPaddingRegion(() =>
            {
                SetRegionBackground(ChartBackground);
                SetRegionAsGroupExtender();
            });
        }
        else
        {
            var list = dic.OrderBy(x => x.Value * (rising ? 1 : -1)).Select(x => (x.Key, x.Value)).ToList();
            AddRegionGroup();
            var left = 600 - temp;
            SetRegionGroupWidth((int)Math.Floor(left / 2.0));
            SetRegionGroupHeight(242);
            AddPaddingRegion(() =>
            {
                SetRegionBackground(ChartBackground);
                SetRegionAsGroupExtender();
            });
            AddRegionGroup();
            SetRegionGroupWidth(temp);
            SetRegionGroupHeight(242);
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
            });
            AddHeaderRegion(() =>
            {
                if (settings.chartBigIcons.Value()) for (int i = list.Count - 1; i >= 0 && i - list.Count > -15; i--)
                    if (chartPage == "Elements Used") AddBigButton("Element" + list[i].Key + "Rousing");
                    else AddBigButton(Ability.abilities.Find(y => y.name == list[i].Key).icon);
                else for (int i = 0; i < list.Count && i < 30; i++)
                    if (chartPage == "Elements Used") AddSmallButton("Element" + list[i].Key + "Rousing");
                    else AddSmallButton(Ability.abilities.Find(y => y.name == list[i].Key).icon);
            });
            iconRow = CDesktop.LBWindow.LBRegionGroup.LBRegion;
            AddRegionGroup();
            SetRegionGroupWidth((int)Math.Ceiling(left / 2.0));
            SetRegionGroupHeight(261);
            AddPaddingRegion(() =>
            {
                SetRegionBackground(ChartBackground);
                SetRegionAsGroupExtender();
            });
        }
    }

    public static void FillChart()
    {
        Dictionary<string, int> dic = new();
        var rising = false;
        if (chartPage == "Damage Dealt") dic = Board.board.log.damageDealt;
        else if (chartPage == "Damage Taken") dic = Board.board.log.damageTaken;
        else if (chartPage == "Healing Received") dic = Board.board.log.healingReceived;
        else if (chartPage == "Elements Used") dic = Board.board.log.elementsUsed;
        var temp = dic.Count * (settings.chartBigIcons.Value() ? 38 : 19);
        if (temp > 0)
        {
            var list = dic.OrderBy(x => x.Value * (rising ? 1 : -1)).Select(x => (x.Key, x.Value)).ToList();
            var highestValue = list.Max(x => x.Value);
            if (settings.chartBigIcons.Value())
                for (int i = list.Count - 1; i >= 0 && i > -15; i--)
                    AddChartColumn(list.Count, Math.Abs(list.Count - 1 - i), list.Sum(x => x.Value), highestValue, list[i].Value);
            else for (int i = 0; i < list.Count && i < 30; i++)
                AddChartColumn(list.Count, i, list.Sum(x => x.Value), highestValue, list[i].Value);
        }
    }

    public static void AddChartColumn(int amount, int index, int total, int highestValue, int value)
    {
        var height = (int)((223.0 - (settings.chartBigIcons.Value() ? 19 : 0)) / highestValue * value);
        SpawnWindowBlueprint(new Blueprint("ChartColumn" + index, () =>
        {
            var foo = (settings.chartBigIcons.Value() ? iconRow.bigButtons.Select(x => x.transform) : iconRow.smallButtons.Select(x => x.transform)).Last().position;
            SetAnchor(foo.x + (settings.chartBigIcons.Value() ? -38 : 19) * (amount - 1 - index) - (settings.chartBigIcons.Value() ? 20f : 10.5f), foo.y + (settings.chartBigIcons.Value() ? 24 : 14.5f) + height);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(settings.chartBigIcons.Value() ? 38 : 19);
            SetRegionGroupHeight(height);
            AddPaddingRegion(() =>
            {
                SetRegionBackground(Button);
                if (settings.chartBigIcons.Value())
                    AddLine(Math.Round(100.0 / total * value) + "%");
                SetRegionAsGroupExtender();
            });
        },
        true));
    }

    #endregion

    public static void AddQuestList(List<Quest> quests, string f = "Add")
    {
        AddHeaderGroup();
        SetRegionGroupWidth(182);
        AddHeaderRegion(() =>
        {
            if (f == "Add")
                AddLine("Available quests:");
            else if (f == "Turn")
                AddLine("Quests ready to turn in:");
        });
        foreach (var quest in quests)
        {
            AddButtonRegion(() =>
            {
                AddLine((settings.questLevel.Value() ? "[" + quest.questLevel + "] " : "") + quest.name, "Black");
                AddSmallButton(quest.ZoneIcon());
            },
            (h) =>
            {
                Quest.quest = quest;
                CloseWindow("Instance");
                CloseWindow("Complex");
                if (f == "Add") Respawn("QuestAdd");
                else if (f == "Turn") Respawn("QuestTurn");
            });
            var color = ColorQuestLevel(quest.questLevel);
            if (color != null) SetRegionBackgroundAsImage("SkillUp" + color);
        }
    }

    #region FluidBar

    public static void AddSkillBar(int x, int y, Profession profession, Entity entity)
    {
        var skillBar = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabSkillBar"));
        skillBar.transform.parent = CDesktop.LBWindow.transform;
        skillBar.transform.localPosition = new Vector3(x, y, 0);
        var thisBar = skillBar.GetComponent<FluidBar>();
        thisBar.Initialise(150, () => profession.levels.Where(x => entity.professionSkills[profession.name].Item2.Contains(x.levelName)).Max(x => x.maxSkill), () => entity.professionSkills[profession.name].Item1, false);
        thisBar.UpdateFluidBar();
    }

    public static void AddResourceBar(int x, int y, string resource, string forWho, Entity entity)
    {
        var resourceBar = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabResourceBar"));
        resourceBar.transform.parent = CDesktop.LBWindow.transform;
        resourceBar.transform.localPosition = new Vector3(x, y, 0);
        var thisBar = resourceBar.GetComponent<FluidBar>();
        resourceBar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/FluidBars/ResourceBar/Resource" + resource + "Bar/FilledBar");
        thisBar.Initialise(entity.MaxResource(resource) * 8, () => entity.MaxResource(resource), () => entity.resources[resource], true);
        thisBar.split.sprite = Resources.Load<Sprite>("Sprites/FluidBars/ResourceBar/Resource" + resource + "Bar/Splitter");
        thisBar.GetComponentsInChildren<SpriteRenderer>().First(x => x.name == "Capstone").sprite = Resources.Load<Sprite>("Sprites/FluidBars/ResourceBar/Resource" + resource + "Bar/Capstone");
        if (Board.board.resourceBars.ContainsKey(forWho)) Board.board.resourceBars[forWho].Add(resource, thisBar);
        else Board.board.resourceBars.Add(forWho, new() { { resource, thisBar } });
        thisBar.UpdateFluidBar();
    }

    public static void AddHealthBar(int x, int y, string forWho, Entity entity)
    {
        var healthBar = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHealthBar"));
        healthBar.transform.parent = CDesktop.LBWindow.transform;
        healthBar.transform.localPosition = new Vector3(x, y, 0);
        var thisBar = healthBar.GetComponent<FluidBar>();
        thisBar.Initialise(150, () => entity.MaxHealth(), () => entity.health, false);
        if (Board.board.healthBars.ContainsKey(forWho)) Board.board.healthBars[forWho] = thisBar;
        else Board.board.healthBars.Add(forWho, thisBar);
        thisBar.UpdateFluidBar();
    }

    #endregion

    #region Static Pagination

    //Saved static pagination
    public static Dictionary<string, int[]> staticPagination;

    public static void PreparePagination(this RegionGroup rg)
    {
        if (!staticPagination.ContainsKey(rg.window.title))
            staticPagination.Add(rg.window.title, new int[rg.window.regionGroups.Count + (rg.window.headerGroup != null ? 1 : 0)]);
        else if (staticPagination[rg.window.title].Length <= rg.window.regionGroups.IndexOf(rg))
        {
            var temp = staticPagination[rg.window.title];
            Array.Resize(ref temp, rg.window.regionGroups.Count);
        }
    }

    public static void CorrectPagination(this RegionGroup rg)
    {
        var dx = rg.window.regionGroups.IndexOf(rg);
        if (dx == -1) dx = staticPagination[rg.window.title].Length - 1;
        var pg = rg.pagination();
        var mpg = rg.maxPagination();
        if (pg >= mpg) staticPagination[rg.window.title][dx] = mpg - 1;
        else if (pg < 0) staticPagination[rg.window.title][dx] = 0;
    }

    public static void SetPagination(this RegionGroup rg, int to)
    {
        var dx = rg.window.regionGroups.IndexOf(rg);
        if (dx == -1) dx = staticPagination[rg.window.title].Length - 1;
        rg.PreparePagination();
        staticPagination[rg.window.title][dx] = to;
        rg.CorrectPagination();
    }

    public static void IncrementPagination(this RegionGroup rg)
    {
        rg.PreparePagination();
        var dx = rg.window.regionGroups.IndexOf(rg);
        if (dx == -1) dx = staticPagination[rg.window.title].Length - 1;
        staticPagination[rg.window.title][dx]++;
        rg.CorrectPagination();
    }

    public static void IncrementPaginationEuler(this RegionGroup rg)
    {
        rg.PreparePagination();
        var dx = rg.window.regionGroups.IndexOf(rg);
        if (dx == -1) dx = staticPagination[rg.window.title].Length - 1;
        staticPagination[rg.window.title][dx] += (int)Math.Round(EuelerGrowth()) / 2;
        rg.CorrectPagination();
    }

    public static void DecrementPagination(this RegionGroup rg)
    {
        rg.PreparePagination();
        var dx = rg.window.regionGroups.IndexOf(rg);
        if (dx == -1) dx = staticPagination[rg.window.title].Length - 1;
        staticPagination[rg.window.title][dx]--;
        rg.CorrectPagination();
    }

    public static void DecrementPaginationEuler(this RegionGroup rg)
    {
        rg.PreparePagination();
        var dx = rg.window.regionGroups.IndexOf(rg);
        if (dx == -1) dx = staticPagination[rg.window.title].Length - 1;
        staticPagination[rg.window.title][dx] -= (int)Math.Round(EuelerGrowth()) / 2;
        rg.CorrectPagination();
    }

    #endregion

    #region Enumerations

    #region General

    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return (((-p0 + 3 * (p1 - p2) + p3) * t + (3 * (p0 + p2) - 6 * p1)) * t + 3 * (p1 - p0)) * t + p0;
    }

    public static bool CompareValues(double x, double y, string compare)
    {
        if (compare == ">=") return x >= y;
        if (compare == ">") return x > y;
        if (compare == "<=") return x <= y;
        if (compare == "<") return x < y;
        if (compare == "==") return x == y;
        if (compare == "!=") return x != y;
        if (compare == "<>") return x != y;
        return false;
    }

    #endregion

    public enum InputType
    {
        Everything,
        Letters,
        StrictLetters,
        Capitals,
        Numbers,
        Decimal
    }

    public enum RegionBackgroundType
    {
        Empty,
        Image,
        Padding,
        Header,
        Button,
        ButtonRed,
        Experience,
        ExperienceNone,
        ExperienceRested,
        ProgressDone,
        ProgressEmpty,
        ChartBackground
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
