using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Defines;

public class Region : MonoBehaviour
{
    public Action draw;
    public List<Line> lines;
    public InputLine inputLine;
    public LineCheckbox checkbox;
    public RegionGroup regionGroup;
    public List<LineSmallButton> smallButtons;
    public List<LineBigButton> bigButtons;
    public int currentHeight, xExtend, yExtend;
    public RegionBackgroundType backgroundType;
    public Sprite backgroundImage;
    public GameObject background;
    public GameObject[] borders;
    public GameObject[] shadows;

    //Set height of the region
    public int setHeight;

    //Set custom offset for text in this region
    public int setOffsetLeft, setOffsetRight;

    //If true small buttons appear on the left side and big ones on the right
    public bool reverseButtons;

    public void Initialise(RegionGroup regionGroup, RegionBackgroundType backgroundType, Action draw)
    {
        lines = new();
        smallButtons = new();
        bigButtons = new();
        borders = new GameObject[8];
        shadows = new GameObject[8];
        this.draw = draw;
        this.regionGroup = regionGroup;
        this.backgroundType = backgroundType;
        regionGroup.regions.Add(this);
    }

    public int PlannedHeight()
    {
        if (setHeight > 0) return setHeight;
        var content = (lines.Count > 0 ? 2 : 0) + (lines.Count == 0 ? 0 : lines.GroupBy(x => x.align).Max(x => x.Count())) * 15 + (inputLine != null && lines.Count == 0 ? 17 : 0);
        if (content < 36 && bigButtons.Count > 0) content = 36;
        else if (content < 17 && smallButtons.Count > 0) content = 17;
        return content + (content > 0 ? 2 : 0);
    }

    public Line LBLine() => lines.SafeLast();

    public LineBigButton LBBigButton() => bigButtons.SafeLast();

    public LineSmallButton LBSmallButton() => smallButtons.SafeLast();

    public int AutoHeight() => setHeight > 0 ? setHeight - 4 : currentHeight;

    public int AutoWidth()
    {
        var lineX = lines.Count > 0 && lines.Max(x => x.Length()) > 0 ? lines.Max(x => x.Length()) + 2 + defines.textPaddingLeft + defines.textPaddingRight : 0;
        var bigButtonX = 38 * bigButtons.Count;
        var smallButtonX = 19 * smallButtons.Count;
        var checkboxX = checkbox != null ? 15 : 0;
        var inputLineX = inputLine != null ? (inputLine.Length() > 0 ? inputLine.Length() : 0)/* + (inputLine == InputLine.inputLine ? font.Length(markerCharacter) : 0)*/ : 0;
        return lineX + bigButtonX + smallButtonX + checkboxX + inputLineX;
    }
}
