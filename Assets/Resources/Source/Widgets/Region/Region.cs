using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;

public class Region : MonoBehaviour
{
    //Parent
    public RegionGroup regionGroup;

    //Children
    public Line LBLine;
    public List<Line> lines;
    public InputLine inputLine;
    public LineDropdown dropdown;
    public LineCheckbox checkbox;
    public LineSmallButton LBSmallButton;
    public List<LineSmallButton> smallButtons;
    public LineBigButton LBBigButton;
    public List<LineBigButton> bigButtons;

    //Fields
    public Action draw;
    public Tooltip tooltip;
    public Action<Highlightable> pressEvent;
    public int currentHeight, xExtend, yExtend;
    public RegionBackgroundType backgroundType;
    public GameObject background;
    public GameObject[] borders;

    public void Initialise(RegionGroup regionGroup, RegionBackgroundType backgroundType, Action draw, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip, int insert)
    {
        lines = new();
        smallButtons = new();
        bigButtons = new();
        borders = new GameObject[4];
        this.draw = draw;
        if (tooltip != null)
            this.tooltip = new Tooltip(() => background.GetComponent<Highlightable>(), tooltip);
        this.pressEvent = pressEvent;
        this.regionGroup = regionGroup;
        this.backgroundType = backgroundType;

        regionGroup.LBRegion = this;
        if (insert == -1) regionGroup.regions.Add(this);
        else regionGroup.regions.Insert(insert, this);
    }

    public int PlannedHeight()
    {
        var content = lines.Count * 15 + (inputLine != null ? 15 : 0);
        if (content < 34 && bigButtons.Count > 0) content = 34;
        return content + (content > 0 ? 4 : 0);
    }

    public int AutoHeight()
    {
        return currentHeight;
    }

    public int AutoWidth()
    {
        //If a region has any big buttons we assume there is nothing
        //else in it and we set the width to 38px times the amount of buttons
        if (bigButtons.Count > 0) return 38 * bigButtons.Count - 10;
        //Number "12" is simply additional free space to not clamp the
        //window borders tightly on region content, the "19" is the button sprite width
        //and the "15" is the checkbox width that fills the line
        return (lines.Count > 0 ? lines.Max(x => x.Length()) + 12 : 0) + 19 * smallButtons.Count + (checkbox != null ? 15 : 0) + (inputLine != null ? (inputLine.Length() > 0 ? inputLine.Length() : 4) + (inputLine.FindID() == currentInputLine ? font.Length(markerCharacter) : 0) : 0);
    }

    public void ResetContent()
    {
        for (int i = 0; i < lines.Count; i++)
            Destroy(lines[i].gameObject);
        lines = new();
        if (inputLine != null)
            Destroy(inputLine.gameObject);
        inputLine = null;
        for (int i = 0; i < smallButtons.Count; i++)
            Destroy(smallButtons[i].gameObject);
        smallButtons = new();
        for (int i = 0; i < bigButtons.Count; i++)
            Destroy(bigButtons[i].gameObject);
        bigButtons = new();
        if (checkbox != null)
            Destroy(checkbox.gameObject);
        checkbox = null;
        if (background != null)
            Destroy(background);
        background = null;
        for (int i = 0; i < 4; i++)
            Destroy(borders[i]);
        borders = new GameObject[4];
        currentHeight = xExtend = yExtend = 0;
    }

    public void OnMouseUp()
    {
        if (checkbox != null)
        {
            checkbox.value.Invert();
            regionGroup.window.Rebuild();
        }
        else if (dropdown != null)
        {
            dropdown.Unwind();
        }
        if (pressEvent != null)
        {
            var a = background.GetComponent<Highlightable>();
            pressEvent(a);
            regionGroup.window.Rebuild();
        }
    }
}
