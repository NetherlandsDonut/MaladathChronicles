using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;

using static Root.RegionBackgroundType;

public class Window : MonoBehaviour
{
    //Parent
    public Desktop desktop;

    //Children
    public GameObject groupGrouping;
    public List<RegionGroup> regionGroups;
    public RegionGroup LBRegionGroup, headerGroup;

    //Fields
    public int xOffset;
    public string title;
    public Vector2 dragOffset;
    public WindowAnchor anchor;
    public bool closeOnLostFocus;
    public GameObject background;
    public GameObject[] shadows;

    public void Initialise(Desktop desktop, string title)
    {
        this.title = title;
        this.desktop = desktop;
        anchor = new WindowAnchor(Anchor.Center);
        regionGroups = new();
        shadows = new GameObject[8];

        desktop.LBWindow = this;
        desktop.windows.Insert(0, this);
        desktop.Reindex();
    }

    //public void MaxRowWidth()
    //{
    //    var rows = new List<List<RegionGroup>>() { new List<RegionGroup> { highestGroup } };
    //    for (int i = 1; rows[i - 1].Sum(x => x.attachedGroup.Count) > 0; i++)
    //        rows.Add(rows[i - 1].SelectMany(x => x.attachedGroup).ToList());
    //    var allofthem = rows.Select(x => x.Sum(y => y.regions.Max(z => z.AutoWidth()))).ToList();
    //    highestGroup.SetWidths(rows.Max(x => x.Sum(y => y.regions.Max(z => z.AutoWidth()))));
    //}

    public int PlannedHeight()
    {
        return regionGroups.Max(x => x.PlannedHeight());
    }

    public void ResetPosition()
    {
        transform.localPosition = Anchorise();
        transform.localPosition += (Vector3)anchor.offset;

        Vector2 Anchorise()
        {
            switch (anchor.anchor)
            {
                case Anchor.Bottom: return new Vector2(475 - xOffset / 2, -536 + PlannedHeight());
                case Anchor.BottomRight: return new Vector2(956 - xOffset, -536 + PlannedHeight());
                case Anchor.BottomLeft: return new Vector2(2, -536 + PlannedHeight());
                case Anchor.Top: return new Vector2(475 - xOffset / 2, -2);
                case Anchor.TopRight: return new Vector2(956 - xOffset, -2);
                case Anchor.TopLeft: return new Vector2(2, -2);
                case Anchor.Center: return new Vector2(480 - xOffset / 2, -270 + PlannedHeight() / 2);
                default: return new Vector2(0, 0);
            }
        }
    }

    public void Rebuild()
    {
        CDesktop.LBWindow = this;
        xOffset = 0;
        if (groupGrouping == null)
        {
            groupGrouping = new GameObject("Groups");
            groupGrouping.transform.parent = transform;
        }
        foreach (var regionGroup in regionGroups)
        {
            regionGroup.transform.parent = groupGrouping.transform;
            regionGroup.transform.localPosition = new Vector3(xOffset, 0, 0);
            BuildRegionGroup(regionGroup);
        }
        if (headerGroup != null) BuildRegionGroup(headerGroup);
        groupGrouping.transform.localPosition = new Vector3(0, headerGroup != null ? -headerGroup.currentHeight : 0, 0);

        //Draws window background
        if (background == null)
            background = new GameObject("Window Background", typeof(SpriteRenderer), typeof(WindowBackground));
        background.transform.parent = transform;
        background.GetComponent<WindowBackground>().Initialise(this);
        background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Backgrounds/Window");
        background.transform.localScale = new Vector3(xOffset + 2, PlannedHeight() + (headerGroup != null ? headerGroup.PlannedHeight() : 0) + 2, 1);
        background.transform.localPosition = new Vector3(0, 0, 0.9f);
        if (background.GetComponent<BoxCollider2D>() == null)
            background.AddComponent<BoxCollider2D>();

        //Draws window shadows
        var shadowSprites = Resources.LoadAll<Sprite>("Sprites/Building/Shadows/Window");
        for (int i = 0; i < 8; i++)
            if (shadows[i] == null)
            {
                shadows[i] = new GameObject("Shadow", typeof(SpriteRenderer));
                shadows[i].transform.parent = transform;
                shadows[i].GetComponent<SpriteRenderer>().sprite = shadowSprites[i];
            }
        shadows[1].transform.localScale = shadows[6].transform.localScale = new Vector3(xOffset + 2, 1, 1);
        shadows[3].transform.localScale = shadows[4].transform.localScale = new Vector3(1, PlannedHeight() + 2 + (headerGroup != null ? headerGroup.PlannedHeight() : 0), 1);
        shadows[0].transform.localPosition = new Vector3(-18, 18, 0.9f);
        shadows[1].transform.localPosition = new Vector3(0, 18, 0.9f);
        shadows[2].transform.localPosition = new Vector3(xOffset + 2, 18, 0.9f);
        shadows[3].transform.localPosition = new Vector3(-18, 0, 0.9f);
        shadows[4].transform.localPosition = new Vector3(xOffset + 2, 0, 0.9f);
        shadows[5].transform.localPosition = new Vector3(-18, -PlannedHeight() - 2 - (headerGroup != null ? headerGroup.PlannedHeight() : 0), 0.9f);
        shadows[6].transform.localPosition = new Vector3(0, -PlannedHeight() - 2 - (headerGroup != null ? headerGroup.PlannedHeight() : 0), 0.9f);
        shadows[7].transform.localPosition = new Vector3(xOffset + 2, -PlannedHeight() - 2 - (headerGroup != null ? headerGroup.PlannedHeight() : 0), 0.9f);
    }

    public void BuildRegionGroup(RegionGroup regionGroup)
    {
        //Reset all regions
        int extendOffset = 0;
        regionGroup.ResetContent();
        CDesktop.LBWindow.LBRegionGroup = regionGroup;

        #region CREATING REGIONS

        //Draw all regions in the regionlist
        if (regionGroup.regionList != null)
            for (int i = 0; i < regionGroup.regionList.regions.Count; i++)
            {
                regionGroup.LBRegion = regionGroup.regionList.regions[i];
                regionGroup.LBRegion.ResetContent();
                var index = i + regionGroup.pagination * regionGroup.regionList.regions.Count;
                if (index < regionGroup.regionList.count()) regionGroup.regionList.inDraw(index);
                else regionGroup.regionList.outDraw(index);
            }

        //Draw all the regions
        foreach (var region in regionGroup.regions)
        {
            regionGroup.LBRegion = region;
            region.draw();
            if (regionGroup == headerGroup)
                region.xExtend = xOffset - headerGroup.AutoWidth() - 10;
        }

        #endregion

        #region DRAWING REGION CONTENTS

        //Update dropdown contents
        foreach (var region in regionGroup.regions)
            if (region.dropdown != null)
                region.lines[0].texts[0].SetText(region.dropdown.headerChange(region.dropdown.currentChoice));

        //Draws region lines and text
        foreach (var region in regionGroup.regions)
            foreach (var line in region.lines)
            {
                var checkboxOffset = region.checkbox != null ? 15 : 0;
                line.transform.localPosition = new Vector3(6 + checkboxOffset, -region.currentHeight - 3, 0);
                region.currentHeight += 15;
                int length = 0;
                foreach (var text in line.texts)
                {
                    text.Erase();
                    foreach (var character in text.text)
                        length = text.SpawnCharacter(character, length);
                }
            }

        //Draws small buttons for single lined regions
        foreach (var region in regionGroup.regions)
            foreach (var smallButton in region.smallButtons)
            {
                smallButton.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Buttons/" + smallButton.buttonType);
                smallButton.transform.localPosition = new Vector3(regionGroup.AutoWidth() + region.xExtend + 1.5f - 19 * region.smallButtons.IndexOf(smallButton), -10.5f, 0.1f);
                if (smallButton.gameObject.GetComponent<BoxCollider2D>() == null)
                    smallButton.gameObject.AddComponent<BoxCollider2D>();
                if (smallButton.gameObject.GetComponent<Highlightable>() == null)
                    smallButton.gameObject.AddComponent<Highlightable>().Initialise(this, region);
                if (smallButton.frame == null)
                    smallButton.frame = new GameObject("ButtonFrame", typeof(SpriteRenderer));
                smallButton.frame.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Borders/ButtonFrame");
                smallButton.frame.transform.parent = smallButton.transform;
                smallButton.frame.transform.localPosition = new Vector3();
            }

        //Draws checkbox for the region
        foreach (var region in regionGroup.regions)
            if (region.checkbox != null)
            {
                region.checkbox.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Checkboxes/" + (region.backgroundType == Handle || region.backgroundType == Button ? "Dark" : "Bright") + (region.checkbox.value.Value() ? "On" : "Off"));
                region.checkbox.transform.localPosition = new Vector3(10.5f, -10.5f, 0.1f);
                if (region.checkbox.gameObject.GetComponent<BoxCollider2D>() == null)
                    region.checkbox.gameObject.AddComponent<BoxCollider2D>();
                if (region.checkbox.gameObject.GetComponent<Highlightable>() == null && region.backgroundType != Handle && region.backgroundType != Button)
                    region.checkbox.gameObject.AddComponent<Highlightable>().Initialise(this, region);
                if (region.checkbox.frame == null)
                    region.checkbox.frame = new GameObject("CheckboxFrame", typeof(SpriteRenderer));
                region.checkbox.frame.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Borders/CheckboxFrame");
                region.checkbox.frame.transform.parent = region.checkbox.transform;
                region.checkbox.frame.transform.localPosition = new Vector3();
            }

        //Draws ipnutLines for the regions
        foreach (var region in regionGroup.regions)
            if (region.inputLine != null)
            {
                region.inputLine.transform.localPosition = new Vector3(6, -region.currentHeight - 3, 0);
                region.currentHeight += 15;
                int length = 0;
                region.inputLine.text.Erase();
                var print = region.inputLine.text.text.Value();
                if (currentInputLine == region.inputLine.FindID())
                    print = print.Insert(inputLineMarker > print.Length ? print.Length : inputLineMarker, markerCharacter);
                else if (print.Length == 0) print += " ";
                foreach (var character in print)
                    length = region.inputLine.text.SpawnCharacter(character, length);
            }

        #endregion

        #region POSITIONING & EXPANDING

        //Position all regions and marks which ones need to be extended
        foreach (var region in regionGroup.regions)
        {
            region.transform.localPosition = new Vector3(0, -regionGroup.currentHeight, 0);
            if (extendOffset != 0) region.transform.localPosition = new Vector3(region.transform.localPosition.x, -regionGroup.currentHeight - extendOffset, region.transform.localPosition.z);
            if (regionGroup != headerGroup && regionGroup.PlannedHeight() < PlannedHeight())
                if (regionGroup.EXTRegion == region || regionGroup.EXTRegion == null && region == regionGroup.regions.Last())
                    region.yExtend = PlannedHeight() - regionGroup.PlannedHeight();
            if (region.yExtend > 0) extendOffset += region.yExtend;
            regionGroup.currentHeight += 4 + region.currentHeight;
        }

        #endregion

        #region BORDERS & BACKGROUNDS

        //Draws region backgrounds
        foreach (var region in regionGroup.regions)
        {
            if (region.background == null)
                region.background = new GameObject("Background", typeof(SpriteRenderer), typeof(RegionBackground));
            region.background.transform.parent = region.transform;
            region.background.GetComponent<RegionBackground>().Initialise(region);
            region.background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Backgrounds/" + region.backgroundType);
            region.background.transform.localScale = new Vector3(regionGroup.AutoWidth() + 8 + region.xExtend, region.AutoHeight() + 2 + region.yExtend, 1);
            region.background.transform.localPosition = new Vector3(2, -2, 0.8f);
            if (region.backgroundType == Button || region.backgroundType == Handle)
            {
                if (region.background.GetComponent<BoxCollider2D>() == null)
                    region.background.AddComponent<BoxCollider2D>();
                if (region.background.GetComponent<Highlightable>() == null)
                    region.background.AddComponent<Highlightable>().Initialise(this, region);
            }
            if (region.background.GetComponent<Highlightable>() != null)
                region.background.GetComponent<Highlightable>().windowHandle = region.backgroundType == Handle;
        }

        //Draws region borders
        foreach (var region in regionGroup.regions)
        {
            for (int i = 0; i < 4; i++)
                if (region.borders[i] == null)
                {
                    region.borders[i] = new GameObject("Border", typeof(SpriteRenderer));
                    region.borders[i].transform.parent = region.transform;
                    region.borders[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Borders/RegionBorder");
                }
            region.borders[0].transform.localScale = region.borders[3].transform.localScale = new Vector3(regionGroup.AutoWidth() + 12 + region.xExtend, 2, 2);
            region.borders[1].transform.localScale = region.borders[2].transform.localScale = new Vector3(2, region.AutoHeight() + 4 + region.yExtend, 2);
            region.borders[0].transform.localPosition = region.borders[1].transform.localPosition = new Vector3(0, 0, 0.5f);
            region.borders[2].transform.localPosition = new Vector3(regionGroup.AutoWidth() + 10 + region.xExtend, 0, 0.5f);
            region.borders[3].transform.localPosition = new Vector3(0, -region.AutoHeight() - 4 - region.yExtend, 0.5f);
        }

        #endregion

        #region TOOLTIPS

        //Asigning tooltips for regions
        foreach (var regionHandle in regionGroup.regions.Select(x => x.GetComponent<TooltipHandle>()).ToList().FindAll(x => x != null))
            regionHandle.ApplyTooltip();

        //Asigning tooltips for small buttons
        foreach (var region in regionGroup.regions)
            if (region.smallButtons.Count > 0)
                foreach (var smallButtonHandle in region.smallButtons.Select(x => x.GetComponent<TooltipHandle>()).ToList().FindAll(x => x != null))
                    smallButtonHandle.ApplyTooltip();

        //Asigning tooltips for checkboxes
        foreach (var region in regionGroup.regions)
            if (region.checkbox != null && region.checkbox.GetComponent<TooltipHandle>() != null)
                region.checkbox.GetComponent<TooltipHandle>().ApplyTooltip();

        #endregion

        //Finish, add width to total window width
        if (headerGroup != regionGroup)
            xOffset += regionGroup.AutoWidth() + 10;
    }
}