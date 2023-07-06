using System;
using UnityEngine;
using System.Collections.Generic;

using static Font;
using static Blueprint;

using static Root.Color;
using static Root.RegionBackgroundType;

public static class Root
{
    public static int screenX = 640;
    public static int screenY = 360;

    public static String testText = new String();
    public static String testText2 = new String();

    public static Cursor cursor;
    public static int inputLineMarker;
    public static System.Random random;
    public static float heldKeyTime;
    public static float animationTime;
    public static float frameTime = 0.07f;
    public static List<Desktop> desktops;
    public static Desktop CDesktop, LBDesktop;
    public static string markerCharacter = "_", currentInputLine = "";

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

    #region Desktop

    public static void SpawnDesktopBlueprint(string blueprintTitle, bool autoSwitch = true)
    {
        var blueprint = desktopBlueprints.Find(x => x.title == blueprintTitle);
        if (blueprint == null) return;
        AddDesktop(blueprint.title);
        blueprint.actions();
        if (autoSwitch)
            SwitchDesktop(blueprintTitle);
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
        newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/Desktop Stone");
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = "Desktop Background";
        LBDesktop = newDesktop;
        newDesktop.Initialise(title);
        desktops.Add(newDesktop);
        //if (desktops.Count == 1) CDesktop = newDesktop;
        newDesktop.screenlock = new GameObject("Screenlock", typeof(BoxCollider2D));
        newDesktop.screenlock.GetComponent<BoxCollider2D>().offset = new Vector2(320, -180);
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

    //Hotkeys can be added only on desktop creation!
    public static void AddHotkey(KeyCode key, Action action)
    {
        LBDesktop.hotkeys.Add(key, action);
    }

    #endregion

    #region Windows

    public static void SpawnWindowBlueprint(string blueprintTitle) => SpawnWindowBlueprint(windowBlueprints.Find(x => x.title == blueprintTitle));
    public static void SpawnWindowBlueprint(Blueprint blueprint)
    {
        if (blueprint == null) return;
        AddWindow(blueprint.title);
        blueprint.actions();
        CDesktop.LBWindow.Rebuild();
        CDesktop.LBWindow.ResetPosition();
    }

    public static void AddWindow(string title)
    {
        var newObject = new GameObject("Window: " + title, typeof(Window), typeof(AudioSource));
        newObject.transform.parent = CDesktop.transform;
        newObject.transform.localPosition = new Vector3();
        newObject.GetComponent<Window>().Initialise(CDesktop, title);
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

    public static void SetAnchor(float x = 0, float y = 0)
    {
        CDesktop.LBWindow.anchor = new WindowAnchor(Anchor.None, x, y);
    }

    public static void SetAnchor(Anchor anchor, float x = 0, float y = 0)
    {
        CDesktop.LBWindow.anchor = new WindowAnchor(anchor, x, y);
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

    private static void AddRegion(RegionBackgroundType backgroundType, Action draw, Action<Highlightable> pressEvent, int insert = -1, string id = "")
    {
        var newObject = new GameObject("Region", typeof(Region));
        var regionGroup = CDesktop.LBWindow.LBRegionGroup;
        newObject.transform.parent = regionGroup.transform;
        newObject.GetComponent<Region>().Initialise(regionGroup, backgroundType, draw, pressEvent, insert);
        if (id != "") MarkRegionGlobal(id);
    }

    public static void AddHandleRegion(Action draw, int insert = -1, string id = "")
    {
        AddRegion(Handle, draw, (h) => { }, insert, id);
    }

    public static void AddButtonRegion(Action draw, Action<Highlightable> pressEvent, int insert = -1, string id = "")
    {
        AddRegion(Button, draw, pressEvent, insert, id);
    }

    public static void AddHeaderRegion(Action draw, int insert = -1, string id = "")
    {
        AddRegion(Header, draw, (h) => { }, insert, id);
    }

    public static void AddPaddingRegion(Action draw, int insert = -1, string id = "")
    {
        AddRegion(Padding, draw, (h) => { }, insert, id);
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
        insert, id);
    }

    public static void SetTooltipForRegion(Func<Highlightable, Action> tooltip)
    {
        var target = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        target.gameObject.AddComponent<TooltipHandle>().tooltip = new Tooltip(() => target.background.GetComponent<Highlightable>(), tooltip);
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

    public static void AddSmallButton(SmallButtonTypes type, Action<Highlightable> pressEvent)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.lines.Count > 1) return;
        var newObject = new GameObject("SmallButton: " + type.ToString(), typeof(LineSmallButton), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineSmallButton>().Initialise(region, type, pressEvent);
    }

    public static void AddNextPageButton()
    {
        var regionGroup = CDesktop.LBWindow.LBRegionGroup;
        AddSmallButton(SmallButtonTypes.NextPage,
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
        AddSmallButton(SmallButtonTypes.PreviousPage,
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

    public static void SetTooltipForSmallButton(Func<Highlightable, Action> tooltip)
    {
        var target = CDesktop.LBWindow.LBRegionGroup.LBRegion.LBSmallButton;
        if (target.gameObject == null) return; //THIS LINE IS HAVOC, DELETE IT WHEN POSSIBLE
        target.gameObject.AddComponent<TooltipHandle>().tooltip = new Tooltip(() => target.GetComponent<Highlightable>(), tooltip);
    }

    //public static void MarkLineGlobal(string id)
    //{
    //    if (CDesktop.globalLines.ContainsKey(id))
    //        CDesktop.globalLines.Remove(id);
    //    CDesktop.globalLines.Add(id, CDesktop.LBWindow.LBRegionGroup.LBRegion.LBLine);
    //}

    #endregion

    #region BigButtons

    public static void AddBigButton(BigButtonTypes type, Action<Highlightable> pressEvent)
    {
        var region = CDesktop.LBWindow.LBRegionGroup.LBRegion;
        if (region.lines.Count > 1) return;
        var newObject = new GameObject("BigButton: " + type.ToString(), typeof(LineBigButton), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineBigButton>().Initialise(region, type, pressEvent);
    }

    public static void SetTooltipForBigButton(Func<Highlightable, Action> tooltip)
    {
        var target = CDesktop.LBWindow.LBRegionGroup.LBRegion.LBBigButton;
        if (target.gameObject == null) return; //THIS LINE IS HAVOC, DELETE IT WHEN POSSIBLE
        target.gameObject.AddComponent<TooltipHandle>().tooltip = new Tooltip(() => target.GetComponent<Highlightable>(), tooltip);
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

    public static void SetTooltipForCheckbox(Func<Highlightable, Action> tooltip)
    {
        var target = CDesktop.LBWindow.LBRegionGroup.LBRegion.checkbox;
        target.gameObject.AddComponent<TooltipHandle>().tooltip = new Tooltip(() => target.GetComponent<Highlightable>(), tooltip);
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

    public enum SmallButtonTypes
    {
        Close,
        Unwind,
        NextPage,
        PreviousPage
    }

    public enum BigButtonTypes
    {
        Empty,
        AwakenedEarth,
        AwakenedFire,
        AwakenedWater,
        AwakenedAir,
        AwakenedLightning,
        AwakenedFrost,
        AwakenedDecay,
        AwakenedArcane,
        AwakenedOrder,
        AwakenedShadow,
        RousingEarth,
        RousingFire,
        RousingWater,
        RousingAir,
        RousingLightning,
        RousingFrost,
        RousingDecay,
        RousingArcane,
        RousingOrder,
        RousingShadow,
        SoulOfEarth,
        SoulOfFire,
        SoulOfWater,
        SoulOfAir,
        SoulOfLightning,
        SoulOfFrost,
        SoulOfDecay,
        SoulOfArcane,
        SoulOfOrder,
        SoulOfShadow,
    }

    public enum SoundEffects
    {
        None,
        Coins,
        PutDownSmallWood,
        PutDownSmallMetal,
        PickUpRocks,
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
        TopLeft
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
            { Green, new Color32(73, 178, 86, 255) },
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
