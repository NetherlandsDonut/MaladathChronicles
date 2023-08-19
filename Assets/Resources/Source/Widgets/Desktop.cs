using System;
using UnityEngine;
using System.Collections.Generic;

using static Root;

public class Desktop : MonoBehaviour
{
    //Children
    public Window LBWindow;
    public List<Window> windows;
    public Camera screen;

    //Fields
    public string title;
    public Tooltip tooltip;
    public float tooltipChanneling;
    public List<Hotkey> hotkeys;
    public GameObject screenlock;
    public bool screenLocked;

    //Global markers
    public Dictionary<string, Window> globalWindows;
    public Dictionary<string, RegionGroup> globalRegionGroups;
    public Dictionary<string, Region> globalRegions;
    public Dictionary<string, Line> globalLines;
    public Dictionary<string, LineText> globalTexts;
    public Dictionary<string, InputLine> globalInputLines;

    public void Initialise(string title)
    {
        this.title = title;
        windows = new();
        hotkeys = new();
        globalWindows = new();
        globalRegionGroups = new();
        globalRegions = new();
        globalLines = new();
        globalTexts = new();
        globalInputLines = new();
    }

    public Window FocusedWindow() => windows.Count < 1 ? null : windows[0];

    public void Focus(Window window)
    {
        windows.Remove(window);
        windows.Insert(0, window);
        Reindex();
    }

    public void Reindex()
    {
        for (int i = 0; i < windows.Count; i++)
        {
            var savewindowsfromclosingbecauseoftooltip = i == 1 && windows[0].closeOnLostFocus;
            if (i > 0 && windows[i].closeOnLostFocus && !savewindowsfromclosingbecauseoftooltip) CloseWindow(windows[i--].title);
            else windows[i].transform.position = new Vector3(windows[i].transform.position.x, windows[i].transform.position.y, i);
        }
    }

    public void Rebuild()
    {
        windows.ForEach(x => x.Rebuild());
    }

    public void SetTooltip(Tooltip tooltip)
    {
        this.tooltip = tooltip;
        tooltipChanneling = 0.5f;
    }

    public void LockScreen()
    {
        screenLocked = true;
        screenlock.SetActive(true);
    }

    public void UnlockScreen()
    {
        screenLocked = false;
        screenlock.SetActive(false);
    }

    public void Start()
    {
        if (CDesktop.name == "Desktop: TalentScreen")
            screen.transform.localPosition = new Vector3(320,  -140);
        else if (CDesktop.name == "Desktop: SpellbookScreen")
            screen.transform.localPosition = new Vector3(0, -180);
        else if (CDesktop.name == "Desktop: Map")
            screen.transform.localPosition = new Vector3(-1000, 1000);
        //screen.transform.localPosition = new Vector3(5648, -1193);
    }

    public void Update()
    {
        if (loadSites != null && loadSites.Count > 0)
            for (int i = 0; i < 4; i++)
            {
                var site = loadSites[0];
                loadingScreenObjectLoad++;
                SpawnWindowBlueprint(site);
                loadSites.RemoveAt(0);
                loadingBar[1].transform.localScale = new Vector3((int)(357.0 / loadingScreenObjectLoadAim * loadingScreenObjectLoad), 1, 1);
                if (loadSites.Count == 0)
                {
                    RemoveDesktopBackground();
                    screen.transform.localPosition = new Vector3(2248, -2193);
                    SpawnWindowBlueprint("MapToolbar");
                    SpawnTransition(0.1f);
                    SpawnTransition(0.1f);
                    SpawnTransition(0.1f);
                    SpawnTransition(0.1f);
                    SpawnTransition(0.1f);
                    SpawnTransition(0.1f);
                    Destroy(loadingBar[0]);
                    Destroy(loadingBar[1]);
                    loadingBar = null;
                    PlaySound("DesktopLoadSuccess");
                    break;
                }
            }
        else
        {
            //if (1.0f / Time.smoothDeltaTime < 60)
            //    Debug.LogError("FPS: " + (int)(1.0f / Time.smoothDeltaTime));
            if (CDesktop.title == "TitleScreen")
            {
                var amount = new Vector3(titleScreenCameraDirection < 2 ? -1f : 1f, titleScreenCameraDirection > 2 ? -1f : (titleScreenCameraDirection < 1 ? -1f : 1f));
                screen.transform.localPosition += amount;
                cursor.transform.localPosition += amount;
                if (Math.Abs(screen.transform.localPosition.x - 1762) > 750 && screen.transform.localPosition.x < 3774 || Math.Abs(screen.transform.localPosition.x - 5374) > 750 && screen.transform.localPosition.x >= 3774)
                {
                    titleScreenCameraDirection = random.Next(0, 4);
                    screen.transform.localPosition = new Vector3(random.Next(0, 2) == 0 ? 1762 : 5374, random.Next(-3683, -1567));
                }
            }
            else if (CDesktop.title == "Map")
            {
            }
            if (screenLocked)
            {
                if (fastTravelCamera != null)
                {
                    var dest = fastTravelCamera.transform.position - new Vector3(66, 0);
                    var lerp = Vector3.zero;
                    if (Math.Abs(screen.transform.position.x - dest.x) + Math.Abs(screen.transform.position.y - dest.y) > 500)
                        lerp = Vector3.Lerp(screen.transform.position, dest, 0.02f);
                    else lerp = Vector3.LerpUnclamped(screen.transform.position, dest, 0.02f);
                    screen.transform.position = new Vector3(lerp.x, lerp.y, screen.transform.position.z);
                    if (Math.Abs(screen.transform.position.x - dest.x) + Math.Abs(screen.transform.position.y - dest.y) < 5)
                    {
                        fastTravelCamera = null;
                        UnlockScreen();
                    }
                }
                else if (title == "Game")
                {
                    if (animationTime > 0)
                        animationTime -= Time.deltaTime;
                    if (animationTime <= 0)
                    {
                        animationTime = frameTime;
                        Board.board.AnimateBoard();
                        Rebuild();
                    }
                }
            }
            else
            {
                if (tooltip != null && tooltipChanneling > 0)
                {
                    tooltipChanneling -= Time.deltaTime;
                    if (tooltipChanneling <= 0 && tooltip.caller != null && tooltip.caller() != null)
                        tooltip.SpawnTooltip();
                }
                if (heldKeyTime > 0) heldKeyTime -= Time.deltaTime;
                if (currentInputLine != "" && globalInputLines.ContainsKey(currentInputLine))
                {
                    var didSomething = false;
                    var CIL = globalInputLines[currentInputLine];
                    var length = CIL.text.text.Value().Length;
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
                    {
                        currentInputLine = "";
                        cursor.SetCursor(CursorType.Default);
                        UnityEngine.Cursor.lockState = CursorLockMode.None;
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            CIL.text.text.Confirm();
                            windows.ForEach(x => x.Rebuild());
                        }
                        else
                        {
                            CIL.text.text.Reset();
                            didSomething = true;
                        }
                    }
                    //else if (Input.GetKeyDown(KeyCode.Delete) && inputLineMarker < )
                    //{
                    //    heldArrowKeyTime = 0.4f;
                    //    Debug.Log(Input.inputString);
                    //    inputLineMarker--;
                    //    didSomething = true;
                    //}
                    else if (Input.GetKeyDown(KeyCode.Delete) && inputLineMarker < length)
                    {
                        heldKeyTime = 0.4f;
                        CIL.text.text.RemoveNextOne(inputLineMarker);
                        didSomething = true;
                    }
                    else if (Input.GetKey(KeyCode.Delete) && inputLineMarker < length && heldKeyTime <= 0)
                    {
                        heldKeyTime = 0.0245f;
                        CIL.text.text.RemoveNextOne(inputLineMarker);
                        didSomething = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow) && inputLineMarker > 0)
                    {
                        heldKeyTime = 0.4f;
                        inputLineMarker--;
                        didSomething = true;
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow) && inputLineMarker > 0 && heldKeyTime <= 0)
                    {
                        heldKeyTime = 0.0245f;
                        inputLineMarker--;
                        didSomething = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) && inputLineMarker < length)
                    {
                        heldKeyTime = 0.4f;
                        inputLineMarker++;
                        didSomething = true;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow) && inputLineMarker < length && heldKeyTime <= 0)
                    {
                        heldKeyTime = 0.0245f;
                        inputLineMarker++;
                        didSomething = true;
                    }
                    else foreach (char c in Input.inputString)
                        {
                            var a = inputLineMarker;
                            if (c == '\b')
                            {
                                if (inputLineMarker > 0 && length > 0)
                                    CIL.text.text.RemovePreviousOne(inputLineMarker--);
                            }
                            else if (c != '\n' && c != '\r' && CIL.CheckInput(c))
                            {
                                CIL.text.text.Insert(inputLineMarker, c);
                                inputLineMarker++;
                            }
                            if (length == CIL.text.text.Value().Length)
                                inputLineMarker = a;
                            didSomething = true;
                        }
                    if (didSomething)
                        CIL.region.regionGroup.window.Rebuild();
                }
                else
                {
                    int downs = 0, helds = 0;
                    foreach (var hotkey in hotkeys)
                        if (Input.GetKeyDown(hotkey.key) && hotkey.keyDown || Input.GetKey(hotkey.key) && !hotkey.keyDown)
                        {
                            if (Input.GetKeyDown(hotkey.key)) downs++;
                            else helds++;
                            hotkey.action();
                        }
                    if (downs > 0) keyStack = 0;
                    if (helds > 0 && keyStack < 100) keyStack++;
                }
            }
        }
    }
}
