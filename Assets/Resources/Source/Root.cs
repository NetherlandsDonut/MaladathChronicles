using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Blueprint;

using static Root.Anchor;
using static Root.RegionBackgroundType;

public static class Root
{
    //Program constants
    public static int screenX = 640;
    public static int screenY = 360;
    public static int textPaddingLeft = 4;
    public static int textPaddingRight = 12;
    public static int shadowSystem = 1;
    public static int aiDepth = 5;
    public static int aiManualBranches = 1;
    public static float frameTime = 0.08f;
    public static string markerCharacter = "_";
    public static string textWrapEnding = "...";
    public static string prefix = "";

    public static MapGrid grid;
    public static List<Transform> cameraBoundaryPoints;
    public static List<FallingElement> fallingElements;
    public static List<FlyingMissile> flyingMissiles;
    public static bool canUnlockScreen;

    public static string creationName;
    public static string creationSide;
    public static string creationGender;
    public static string creationRace;
    public static string creationClass;
    public static int maxPlayerLevel = 60;

    public static GameObject[] loadingBar;
    public static int loadingScreenObjectLoad;
    public static int loadingScreenObjectLoadAim;
    public static List<Blueprint> loadSites;

    public static int inputLineMarker;
    public static System.Random random;
    public static int keyStack;
    public static int titleScreenCameraDirection;
    public static float heldKeyTime;
    public static float animationTime;
    public static List<Desktop> desktops;
    public static Desktop CDesktop, LBDesktop;
    public static List<Dictionary<string, string>> triggersCopy, effectsCopy;

    #region Desktop

    public static void SpawnDesktopBlueprint(string blueprintTitle, bool autoSwitch = true)
    {
        var blueprint = desktopBlueprints.Find(x => x.title == blueprintTitle);
        if (blueprint == null) return;
        AddDesktop(blueprint.title);
        if (autoSwitch)
            SwitchDesktop(blueprintTitle);
        blueprint.actions();
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
        newDesktop.screen = new GameObject("Camera", typeof(Camera), typeof(SpriteRenderer)).GetComponent<Camera>();
        newDesktop.screen.transform.parent = newDesktop.transform;
        newDesktop.GetComponent<SpriteRenderer>().sortingLayerName = "DesktopBackground";
        newDesktop.screen.GetComponent<SpriteRenderer>().sortingLayerName = "DesktopBackground";
        newDesktop.screen.orthographicSize = 180;
        newDesktop.screen.nearClipPlane = -1024;
        newDesktop.screen.farClipPlane = 4096;
        newDesktop.screen.clearFlags = CameraClearFlags.SolidColor;
        newDesktop.screen.backgroundColor = Color.black;
        newDesktop.screen.orthographic = true;
        if (GameSettings.settings.pixelPerfectVision.Value()) newDesktop.screen.gameObject.AddComponent<PixelCamera>();
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
        if (CDesktop.cameraDestination != Vector2.zero)
        {
            var temp = CDesktop.screen.transform.position;
            var rounded = new Vector2((float)Math.Round(CDesktop.cameraDestination.x), (float)Math.Round(CDesktop.cameraDestination.y));
            var newPosition = rounded * 19;
            Cursor.cursor.transform.position += (Vector3)newPosition - temp;
            CDesktop.screen.transform.position = newPosition;
        }
    }

    public static void OrderLoadingMap()
    {
        cameraBoundaryPoints = new();
        loadSites = windowBlueprints.FindAll(x => x.title.StartsWith("Site: "));
        loadingScreenObjectLoad = 0;
        loadingScreenObjectLoadAim = loadSites.Count;
    }

    public static void OrderLoadingMapDead()
    {
        loadSites = windowBlueprints.FindAll(x => x.title.StartsWith("SiteDead: "));
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
        var sprite = Resources.Load<Sprite>("Sprites/Textures/" + texture);
        var temp = (followCamera ? CDesktop.screen.gameObject : CDesktop.gameObject).GetComponent<SpriteRenderer>();
        if (sprite == null) Debug.Log("ERROR 004: Desktop background not found: \"Sprites/Textures/" + texture + "\"");
        if (temp.sprite != sprite)
        {
            SpawnTransition();
            temp.sprite = sprite;
        }
    }

    //Hotkeys can be added only on desktop creation!
    public static void AddHotkey(KeyCode key, Action action, bool keyDown = true)
    {
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
        return SpawnWindowBlueprint(windowBlueprints.Find(x => x.title == blueprintTitle), resetSearch);
    }

    public static Window SpawnWindowBlueprint(Blueprint blueprint, bool resetSearch = true)
    {
        if (blueprint == null) return null;
        if (CDesktop.windows.Exists(x => x.title == blueprint.title)) return null;
        AddWindow(blueprint.title, blueprint.upperUI);
        blueprint.actions();
        if (resetSearch && CDesktop.LBWindow.regionGroups.Any(x => x.maxPaginationReq != null))
            String.search.Set("");
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

    public static void Respawn(string windowName)
    {
        var window = CDesktop.windows.Find(x => x.title == windowName);
        if (window != null) window.Respawn();
        else SpawnWindowBlueprint(windowName, true);
    }

    public static bool CloseWindow(string windowName)
    {
        return CloseWindow(CDesktop.windows.Find(x => x.title == windowName));
    }

    public static bool CloseWindow(Window window)
    {
        if (window == null) return false;
        CDesktop.windows.Remove(window);
        UnityEngine.Object.Destroy(window.gameObject);
        return false;
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

    public static void AddHeaderGroup()
    {
        var newObject = new GameObject("HeaderGroup", typeof(RegionGroup));
        newObject.transform.parent = CDesktop.LBWindow.transform;
        newObject.GetComponent<RegionGroup>().Initialise(CDesktop.LBWindow, true, null, 0);
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

    private static void AddRegion(RegionBackgroundType backgroundType, Action draw, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip)
    {
        var newObject = new GameObject("Region", typeof(Region));
        var regionGroup = CDesktop.LBWindow.LBRegionGroup;
        newObject.transform.parent = regionGroup.transform;
        newObject.GetComponent<Region>().Initialise(regionGroup, backgroundType, draw, pressEvent, tooltip);
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

    public static void AddButtonRegion(Action draw, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip = null)
    {
        AddRegion(Button, draw, pressEvent, tooltip);
    }

    public static void AddHeaderRegion(Action draw)
    {
        AddRegion(Header, draw, (h) => { }, null);
    }

    public static void AddPaddingRegion(Action draw)
    {
        AddRegion(Padding, draw, (h) => { }, null);
    }

    public static void AddInputRegion(String refText, InputType inputType, string color = "")
    {
        AddRegion(Padding, () =>
        {
            AddInputLine(refText, inputType, color);
        }, 
        (h) =>
        {
            inputLineMarker = h.region.inputLine.text.text.Value().Length;
        },
        null);
    }

    public static void AddPaginationLine(RegionGroup group)
    {
        AddPaddingRegion(() =>
        {
            AddLine("Page: ", "DarkGray");
            AddText(group.pagination + 1 + "");
            AddText(" / ", "DarkGray");
            AddText(group.maxPagination() + "");
            AddSmallButton("OtherNextPage", (h) =>
            {
                if (group.pagination < group.maxPagination() - 1)
                {
                    Sound.PlaySound("DesktopChangePage", 0.4f);
                    group.pagination++;
                    h.window.Rebuild();
                }
            });
            AddSmallButton("OtherPreviousPage", (h) =>
            {
                if (group.pagination > 0)
                {
                    Sound.PlaySound("DesktopChangePage", 0.4f);
                    group.pagination--;
                    h.window.Rebuild();
                }
            });
        });
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

    public static void PrintPriceRegion(double price)
    {
        int width = CDesktop.LBWindow.LBRegionGroup.setWidth;
        if (price <= 0) return;
        var lacking = 0;
        if ((int)price > 0)
            Foo("ItemCoinsGold", (int)price + "", "Gold");
        else lacking++;
        if ((int)(price * 100 % 100) > 0)
            Foo("ItemCoinsSilver", (int)(price * 100 % 100) + "", "Silver");
        else lacking++;
        if ((int)(price * 10000 % 100) > 0)
            Foo("ItemCoinsCopper", (int)(price * 10000 % 100) + "", "Copper");
        else lacking++;
        AddRegionGroup();
        SetRegionGroupWidth(width - (3 - lacking) * 52);
        AddPaddingRegion(() => { AddLine(""); });

        void Foo(string icon, string text, string color)
        {
            AddRegionGroup();
            AddPaddingRegion(() => { AddSmallButton(icon, (h) => { }); });
            AddRegionGroup();
            SetRegionGroupWidth(33);
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
        AddText(text, color == "" ? DefaultTextColorForRegion(region.backgroundType) : color);
    }

    public static string DefaultTextColorForRegion(RegionBackgroundType type)
    {
        if (type == Header) return "LightGray";
        if (type == Padding) return "Gray";
        if (type == Button) return "Black";
        if (type == RedButton) return "Black";
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
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = CDesktop.LBWindow.layer;
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(0.1f);
        }
    }

    public static void AddSmallButton(string type, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip = null)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        var newObject = new GameObject("SmallButton: " + type.ToString(), typeof(LineSmallButton), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineSmallButton>().Initialise(region, type, pressEvent, tooltip);
    }

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
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = CDesktop.LBWindow.layer;
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

    public static void AddText(string text = "", string color = "Gray")
    {
        var newObject = new GameObject("Text", typeof(LineText));
        var line = CDesktop.LBWindow.LBRegionGroup.LBRegion.LBLine;
        newObject.transform.parent = line.transform;
        newObject.GetComponent<LineText>().Initialise(line, text, color);
    }

    #endregion

    #region InputLines

    public static void AddInputLine(String refText, InputType inputType, string color = "")
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.lines.Count > 0 && region.checkbox != null) return;
        var newObject = new GameObject("InputLine", typeof(InputLine));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<InputLine>().Initialise(region, refText, inputType, color);
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
        Capitals,
        Numbers,
        Decimal
    }

    public enum RegionBackgroundType
    {
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
