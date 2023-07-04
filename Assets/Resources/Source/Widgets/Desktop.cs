using System;
using UnityEngine;
using System.Collections.Generic;

using static Root;
using static Blueprint;

public class Desktop : MonoBehaviour
{

    //Children
    public Window LBWindow;
    public List<Window> windows;

    //Fields
    public string title;
    public Tooltip tooltip;
    public float tooltipChanneling;
    public Dictionary<KeyCode, Action> hotkeys;

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

    public void Update()
    {
        if (tooltip != null && tooltipChanneling > 0)
        {
            tooltipChanneling -= Time.deltaTime;
            if (tooltipChanneling <= 0) tooltip.SpawnTooltip();
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
            foreach (var hotkey in hotkeys)
                if (Input.GetKeyDown(hotkey.Key))
                    hotkey.Value();
    }
}
