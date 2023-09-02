using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Font;
using static Sound;

public class Region : MonoBehaviour
{
    public RegionGroup regionGroup;
    public Line LBLine;
    public List<Line> lines;
    public InputLine inputLine;
    public LineCheckbox checkbox;
    public LineSmallButton LBSmallButton;
    public List<LineSmallButton> smallButtons;
    public LineBigButton LBBigButton;
    public List<LineBigButton> bigButtons;
    public Action draw;
    public Tooltip tooltip;
    public bool resetInputFieldSet;
    public Action<Highlightable> pressEvent;
    public int currentHeight, xExtend, yExtend;
    public RegionBackgroundType backgroundType;
    public GameObject background;
    public GameObject[] borders;

    public void Initialise(RegionGroup regionGroup, RegionBackgroundType backgroundType, Action draw, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip)
    {
        lines = new();
        smallButtons = new();
        bigButtons = new();
        borders = new GameObject[4];
        this.draw = draw;
        if (tooltip != null && regionGroup.window != null)
            this.tooltip = new Tooltip(() => background.GetComponent<Highlightable>(), tooltip);
        this.pressEvent = pressEvent;
        this.regionGroup = regionGroup;
        this.backgroundType = backgroundType;

        regionGroup.LBRegion = this;
        regionGroup.regions.Add(this);
    }

    public int PlannedHeight()
    {
        var content = (lines.Count > 0 ? 2 : 0) + lines.Count * 15 + (inputLine != null && lines.Count == 0 ? 17 : 0);
        if (content < 36 && bigButtons.Count > 0) content = 36;
        else if (content < 17 && smallButtons.Count > 0) content = 17;
        return content + (content > 0 ? 2 : 0);
    }

    public int AutoHeight()
    {
        return currentHeight;
    }

    public int AutoWidth()
    {
        //Number "12" is simply additional free space to not clamp the
        //window borders tightly on region content, the "19" is the button sprite width
        //and the "15" is the checkbox width that fills the line
        var lineX = lines.Count > 0 && lines.Max(x => x.Length()) > 0 ? lines.Max(x => x.Length()) + 2 + textPaddingLeft + textPaddingRight : 0;
        var bigButtonX = 38 * bigButtons.Count;
        var smallButtonX = 19 * smallButtons.Count;
        var checkboxX = checkbox != null ? 15 : 0;
        var inputLineX = inputLine != null ? (inputLine.Length() > 0 ? inputLine.Length() : 0) + (inputLine == InputLine.inputLine ? font.Length(markerCharacter) : 0) : 0;
        return lineX + bigButtonX + smallButtonX + checkboxX + inputLineX;
    }

    public void ResetContent()
    {
        for (int i = 0; i < lines.Count; i++)
            Destroy(lines[i].gameObject);
        lines = new();
        if (inputLine != null && InputLine.inputLine == inputLine)
            resetInputFieldSet = true;
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
        if (pressEvent != null && background.GetComponent<Highlightable>().over)
        {
            PlaySound("DesktopButtonPress", 0.6f);
            var a = background.GetComponent<Highlightable>();
            pressEvent(a);
            regionGroup.window.Rebuild();
        }
    }
}
