using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Defines;
using static InputLine;

using static Root.Anchor;
using static Root.RegionBackgroundType;

public class Window : MonoBehaviour
{
    public Desktop desktop;
    public List<RegionGroup> regionGroups;
    public RegionGroup headerGroup;
    public int xOffset, yOffset;
    public string title, layer;
    public WindowAnchor anchor;
    public GameObject[] shadows;
    public bool disabledShadows, disabledGeneralSprites, disabledCollisions, masked;

    public void Initialise(Desktop desktop, string title, bool upperUI)
    {
        this.title = title;
        if (title == "Board") Board.board.window = this;
        else if (title == "BufferBoard") BufferBoard.bufferBoard.window = this;
        this.desktop = desktop;
        anchor = new WindowAnchor(Center);
        regionGroups = new();
        shadows = new GameObject[8];
        if (upperUI) layer = "Upper";
        else layer = "Default";
        desktop.windows.Add(this);
    }

    public void FadeIn()
    {
        var rs = GetComponentsInChildren<SpriteRenderer>().ToList();
        foreach (var r in rs) r.gameObject.AddComponent<FadeIn>().random = false;
    }

    public RegionGroup LBRegionGroup() => regionGroups.SafeLast();

    public int PlannedHeight(bool includeHeader = false)
    {
        return (regionGroups.Count > 0 ? regionGroups.Max(x => x.PlannedHeight()) : 0) + (includeHeader && headerGroup != null ? (headerGroup.setHeight != 0 ? headerGroup.setHeight : headerGroup.PlannedHeight()) : 0);
    }

    public void ResetPosition()
    {
        if (anchor.anchor != None) transform.parent = desktop.screen.transform;
        transform.localPosition = Vector3.zero;
        transform.localPosition = Anchor();
        transform.localPosition += (anchor.anchor != None ? new Vector3(screenX / -2, screenY / 2) : Vector3.zero) + (Vector3)anchor.offset;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -desktop.windows.Count + (layer == "Default" ? 1024 : 0));

        Vector2 Anchor() => anchor.anchor switch
        {
            Top => new Vector2(screenX / 2 - Width() / 2 - 1, 0),
            TopRight => new Vector2(screenX - 2 - Width(), 0),
            Bottom => new Vector2(screenX / 2 - Width() / 2 - 1, 2 - screenY + yOffset),
            BottomRight => new Vector2(screenX - 2 - Width(), 2 - screenY + yOffset),
            BottomLeft => new Vector2(0, 2 - screenY + yOffset),
            Center => new Vector2(screenX / 2 - Width() / 2 - 1, screenY / -2 + yOffset / 2),
            _ => new Vector2(0, 0),
        };
    }

    public int Width()
    {
        var head = headerGroup != null ? headerGroup.AutoWidth() : 0;
        return head > xOffset ? head : xOffset;
    }

    public void Respawn(bool onlyWhenActive = false)
    {
        if (CDesktop != desktop || onlyWhenActive && !desktop.windows.Contains(this)) return;
        CDesktop.windows.FindAll(x => x.title == "Tooltip").ForEach(x => CloseWindow(x));
        CloseWindow(this, false);
        SpawnWindowBlueprint(title, false);
    }

    public void Rebuild()
    {
        CDesktop.windows.Remove(this);
        CDesktop.windows.Add(this);
        xOffset = 0;
        for (int i = 0; i < regionGroups.Count; i++)
        {
            var regionGroup = regionGroups[0];
            regionGroups.Remove(regionGroup);
            regionGroups.Add(regionGroup);
            regionGroup.transform.parent = transform;
            regionGroup.transform.localPosition = new Vector3(xOffset, regionGroup != headerGroup && headerGroup != null ? -headerGroup.currentHeight : 0, 0);
            BuildRegionGroup(regionGroup);
        }
        yOffset = headerGroup != null ? (regionGroups.Count > 1 ? regionGroups.Where(x => x != headerGroup).Max(x => x.currentHeight) : 0) + headerGroup.currentHeight : (regionGroups.Count > 0 ? regionGroups.Max(x => x.currentHeight) : 0);
        if (masked) GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(x => x.maskInteraction = SpriteMaskInteraction.VisibleInsideMask);

        void BuildRegionGroup(RegionGroup regionGroup)
        {
            int extendOffset = 0;

            #region CREATING REGIONS

            //Draw all the regions
            for (int i = 0; i < regionGroup.regions.Count; i++)
            {
                var region = regionGroup.regions[0];
                regionGroup.regions.Remove(region);
                regionGroup.regions.Add(region);
                region.draw();
                if (regionGroup == headerGroup)
                {
                    var temp = xOffset - headerGroup.AutoWidth();
                    if (region.xExtend < temp) region.xExtend = temp;
                }
            }

            #endregion

            #region DRAWING REGION CONTENTS

            //Draws region lines and text
            foreach (var region in regionGroup.regions)
                foreach (var line in region.lines)
                {
                    var objectOffset = (region.checkbox != null ? 15 : 0) + (region.reverseButtons ? region.smallButtons.Count * 19 : region.bigButtons.Count * 38);
                    int length = 0;
                    if (regionGroup.setWidth == 0)
                        foreach (var text in line.texts)
                        {
                            text.Erase();
                            foreach (var character in text.text)
                                length = text.SpawnCharacter(character, length);
                        }
                    else
                    {
                        var emptySpace = regionGroup.setWidth - (defines.textPaddingLeft + defines.textPaddingRight + objectOffset + (region.reverseButtons ? region.bigButtons.Count * 38 : region.smallButtons.Count * 19));
                        var fullText = string.Join("", line.texts.Select(x => x.text));
                        var toPrint = 0;
                        var useWrapper = false;
                        var currentLength = 0;
                        var wrapperLength = Font.fonts["Tahoma Bold"].Length(defines.textWrapEnding);
                        for (int i = 0; i < fullText.Length; i++)
                        {
                            var newCharLength = Font.fonts["Tahoma Bold"].Length(fullText[i]) + 1;
                            if (fullText.Length > i + 1 && fullText[i + 1] != ' ' && currentLength + newCharLength + wrapperLength + 1 > emptySpace)
                            {
                                useWrapper = fullText[i] != ' ';
                                break;
                            }
                            else if (currentLength + newCharLength + wrapperLength <= emptySpace)
                            {
                                currentLength += newCharLength;
                                toPrint++;
                            }
                        }
                        var printed = 0;
                        foreach (var text in line.texts)
                        {
                            foreach (var character in text.text)
                                if (++printed <= toPrint)
                                    length = text.SpawnCharacter(character, length);
                            if (printed > toPrint && useWrapper)
                            {
                                for (int i = 0; i < defines.textWrapEnding.Length; i++)
                                    length = text.SpawnCharacter(defines.textWrapEnding[i], length);
                                length = text.SpawnCharacter(' ', length);
                                break;
                            }
                        }
                    }
                    if (line.align == "Left")
                        line.transform.localPosition = new Vector3(2 + defines.textPaddingLeft + objectOffset, -region.currentHeight - 3, 0);
                    else if (line.align == "Center")
                        line.transform.localPosition = new Vector3(2 + (region.regionGroup.AutoWidth() / 2) - (length / 2), -region.currentHeight - 3, 0);
                    else if (line.align == "Right")
                        line.transform.localPosition = new Vector3(-defines.textPaddingLeft + region.regionGroup.AutoWidth() - (region.reverseButtons ? region.bigButtons.Count * 38 : region.smallButtons.Count * 19) - length, -region.currentHeight - 3, 0);
                    region.currentHeight += 15;
                }

            //Draws small buttons for single lined regions
            foreach (var region in regionGroup.regions)
                foreach (var smallButton in region.smallButtons)
                {
                    if (region.currentHeight < 15) region.currentHeight = 15;
                    if (smallButton.texture == null) continue;
                    var load = Resources.Load<Sprite>("Sprites/Buttons/" + smallButton.texture);
                    smallButton.GetComponent<SpriteRenderer>().sprite = load == null ? Resources.Load<Sprite>("Sprites/Buttons/OtherEmpty") : load;
                    smallButton.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    if (title.StartsWith("Site: ")) smallButton.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    smallButton.transform.localPosition = region.reverseButtons ? new Vector3(10.5f + 19 * region.smallButtons.IndexOf(smallButton), -10.5f, 0.1f) : new Vector3(regionGroup.AutoWidth() - 10 + region.xExtend + 1.5f - 19 * region.smallButtons.IndexOf(smallButton), -10.5f, 0.1f);
                    if (smallButton.gameObject.GetComponent<BoxCollider2D>() == null)
                        smallButton.gameObject.AddComponent<BoxCollider2D>();
                    if (smallButton.gameObject.GetComponent<Highlightable>() == null)
                        smallButton.gameObject.AddComponent<Highlightable>().Initialise(region, null, null, null, null);
                    if (smallButton.frame == null)
                        smallButton.frame = new GameObject("ButtonFrame", typeof(SpriteRenderer));
                    smallButton.frame.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(title.StartsWith("Site: ") ? "Sprites/Other/PremadeCircularSite" : "Sprites/PremadeBorders/ButtonFrame" + (smallButton.GetComponent<SpriteRenderer>().sprite.texture.name.StartsWith("Other") ? "" : "Shadowed"));
                    if (title.StartsWith("Site: ")) smallButton.frame.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    smallButton.frame.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    if (title.StartsWith("Site: ")) smallButton.frame.AddComponent<SpriteMask>().sprite = Resources.Load<Sprite>("Sprites/PremadeBorders/ButtonCircleFrameMask");
                    smallButton.frame.transform.parent = smallButton.transform;
                    smallButton.frame.transform.localPosition = new Vector3(0, 0, -0.05f);
                    var h = smallButton.gameObject.GetComponent<Highlightable>();
                    if (disabledCollisions || h.pressEvent == null && h.middlePressEvent == null && h.rightPressEvent == null && h.tooltip == null)
                        Destroy(smallButton.GetComponent<BoxCollider2D>());
                }

            //Draws big buttons for single lined regions
            foreach (var region in regionGroup.regions)
            {
                if (region.bigButtons.Count > 0 && region.currentHeight < 34) region.currentHeight = 34;
                foreach (var bigButton in region.bigButtons)
                {
                    if (bigButton.texture == null) continue;
                    var load = Resources.Load<Sprite>("Sprites/ButtonsBig/" + bigButton.texture);
                    bigButton.GetComponent<SpriteRenderer>().sprite = load == null ? Resources.Load<Sprite>("Sprites/ButtonsBig/OtherEmpty") : load;
                    bigButton.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    bigButton.transform.localPosition = region.reverseButtons ? new Vector3(regionGroup.AutoWidth() - 20 + region.xExtend + 2f - 38 * region.bigButtons.IndexOf(bigButton), -20f, 0.1f) : new Vector3(20 + 38 * region.bigButtons.IndexOf(bigButton), -20f, 0.1f);
                    if (bigButton.gameObject.GetComponent<BoxCollider2D>() == null)
                        bigButton.gameObject.AddComponent<BoxCollider2D>();
                    if (bigButton.gameObject.GetComponent<Highlightable>() == null)
                        bigButton.gameObject.AddComponent<Highlightable>().Initialise(region, null, null, null, null);
                    if (bigButton.frame == null)
                        bigButton.frame = new GameObject("BigButtonFrame", typeof(SpriteRenderer));
                    bigButton.frame.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(title.StartsWith("TalentButton") ? "Sprites/PremadeBorders/PremadeTalent" : "Sprites/PremadeBorders/BigButtonFrame");
                    bigButton.frame.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    if (title.StartsWith("TalentButton")) bigButton.frame.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    bigButton.frame.transform.parent = bigButton.transform;
                    bigButton.frame.transform.localPosition = new Vector3(0, 0, -0.05f);
                    var h = bigButton.gameObject.GetComponent<Highlightable>();
                    if (disabledCollisions || h.pressEvent == null && h.middlePressEvent == null && h.rightPressEvent == null && h.tooltip == null)
                        Destroy(bigButton.GetComponent<BoxCollider2D>());
                }
            }

            //Draws checkbox for the region
            foreach (var region in regionGroup.regions)
                if (region.checkbox != null)
                {
                    region.checkbox.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Checkbox/" + (region.backgroundType == ButtonRed || region.backgroundType == Button ? "Dark" : "Bright") + (region.checkbox.value.Value() ? "On" : "Off"));
                    region.checkbox.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    region.checkbox.transform.localPosition = new Vector3(10.5f, -10.5f, 0.1f);
                    if (region.checkbox.gameObject.GetComponent<BoxCollider2D>() == null)
                        region.checkbox.gameObject.AddComponent<BoxCollider2D>();
                    if (region.checkbox.gameObject.GetComponent<Highlightable>() == null && region.backgroundType != ButtonRed && region.backgroundType != Button)
                        region.checkbox.gameObject.AddComponent<Highlightable>().Initialise(region, null, null, null, null);
                    if (region.checkbox.frame == null)
                        region.checkbox.frame = new GameObject("CheckboxFrame", typeof(SpriteRenderer));
                    region.checkbox.frame.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PremadeBorders/CheckboxFrame");
                    region.checkbox.frame.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    region.checkbox.frame.transform.parent = region.checkbox.transform;
                    region.checkbox.frame.transform.localPosition = new Vector3();
                    if (disabledCollisions) Destroy(region.checkbox.GetComponent<BoxCollider2D>());
                }

            //Draws inputLines for the regions
            foreach (var region in regionGroup.regions)
                if (region.inputLine != null)
                {
                    var objectOffset = (region.checkbox != null ? 15 : 0) + region.bigButtons.Count * 38;
                    if (region.lines.Count > 0)
                        region.inputLine.transform.localPosition = new Vector3(11 + region.lines[0].Length(), -region.currentHeight + 12, 0);
                    else
                    {
                        if (region.currentHeight < 15) region.currentHeight = 15;
                        region.inputLine.transform.localPosition = new Vector3(2 + defines.textPaddingLeft, -region.currentHeight + 12, 0);
                    }
                    int length = 0;
                    region.inputLine.text.Erase();
                    var print = region.inputLine.text.text.Value();
                    if (inputDestination == region.inputLine.text.text && inputLineWindow == title)
                        print = print.Insert(inputLineMarker > print.Length ? print.Length : inputLineMarker, defines.markerCharacter);
                    else
                    {
                        if (region.inputLine.align != "Right") print += " ";
                        if (region.inputLine.align != "Left") print = " " + print;
                    }
                    foreach (var character in print)
                        length = region.inputLine.text.SpawnCharacter(character, length, region.inputLine.color);
                    if (region.inputLine.align == "Center") region.inputLine.transform.localPosition = new Vector3(2 + (region.regionGroup.AutoWidth() / 2) - (length / 2), -region.currentHeight + 12, 0);
                    else if (region.inputLine.align == "Right") region.inputLine.transform.localPosition = new Vector3(-defines.textPaddingLeft + region.regionGroup.AutoWidth() - (region.smallButtons.Count * 19) - length, -region.currentHeight + 12, 0);
                }

            #endregion

            #region POSITIONING & EXPANDING

            //Position all regions and marks which ones need to be extended
            foreach (var region in regionGroup.regions)
            {
                region.transform.localPosition = new Vector3(0, -regionGroup.currentHeight - extendOffset, 0);
                if (regionGroup == headerGroup)
                {
                    if (regionGroup.stretchRegion == region && regionGroup.setHeight != 0)
                        region.yExtend = regionGroup.setHeight - regionGroup.PlannedHeight() + 10 + (region.lines.Count > 0 ? 4 : 0);
                }
                else if (regionGroup.PlannedHeight() < regionGroup.setHeight)
                    if (regionGroup.stretchRegion == region || regionGroup.stretchRegion == null && region == regionGroup.regions.Last())
                        region.yExtend = regionGroup.setHeight - regionGroup.PlannedHeight() + (region.lines.Count > 0 ? 4 : 0);
                if (region.yExtend > 0) extendOffset += region.yExtend;
                regionGroup.currentHeight += 4 + region.currentHeight;
            }

            #endregion

            #region BORDERS & BACKGROUNDS

            //Draws region backgrounds
            if (!disabledGeneralSprites)
                foreach (var region in regionGroup.regions)
                    if (region.backgroundType != Empty)
                    {
                        region.background.transform.parent = region.transform;
                        region.background.GetComponent<RegionBackground>().Initialise(region);
                        region.background.GetComponent<SpriteRenderer>().sprite = region.backgroundType == Image ? region.backgroundImage : Resources.Load<Sprite>("Sprites/Fills/" + region.backgroundType);
                        region.background.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                        if (region.backgroundType == Image) region.background.transform.localScale = new Vector3(1, 1, 1);
                        else region.background.transform.localScale = new Vector3(regionGroup.AutoWidth() - 2 + region.xExtend, region.AutoHeight() + 2 + region.yExtend, 1);
                        region.background.transform.localPosition = new Vector3(2, -2, 0.8f);
                        if (region.backgroundType == Button || region.backgroundType == ButtonRed || region.backgroundType == Image)
                        {
                            if (region.background.GetComponent<BoxCollider2D>() == null)
                                region.background.AddComponent<BoxCollider2D>();
                            region.background.GetComponent<BoxCollider2D>().enabled = !disabledCollisions;
                        }
                    }

            //Draws region borders
            if (!disabledGeneralSprites)
                foreach (var region in regionGroup.regions)
                    if (region.backgroundType != Empty)
                    {
                        for (int i = 0; i < 4; i++)
                            if (region.borders[i] == null)
                            {
                                region.borders[i] = new GameObject("Border", typeof(SpriteRenderer));
                                region.borders[i].transform.parent = region.transform;
                                region.borders[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PremadeBorders/RegionBorder");
                                region.borders[i].GetComponent<SpriteRenderer>().sortingLayerName = layer;
                            }
                        for (int i = 0; i < 4; i++)
                            if (region.borders[i + 4] == null)
                            {
                                region.borders[i + 4] = new GameObject("BorderCorner", typeof(SpriteRenderer));
                                region.borders[i + 4].transform.parent = region.transform;
                                region.borders[i + 4].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PremadeBorders/RegionBorderCorner");
                                region.borders[i + 4].GetComponent<SpriteRenderer>().sortingLayerName = "Upper";
                                if (i == 1 || i == 3) region.borders[i + 4].GetComponent<SpriteRenderer>().flipX = true;
                                if (i == 2 || i == 3) region.borders[i + 4].GetComponent<SpriteRenderer>().flipY = true;
                            }
                        region.borders[0].transform.localScale = region.borders[3].transform.localScale = new Vector3(regionGroup.AutoWidth() + 2 + region.xExtend, 2, 2);
                        region.borders[1].transform.localScale = region.borders[2].transform.localScale = new Vector3(2, region.AutoHeight() + 4 + region.yExtend, 2);
                        region.borders[0].transform.localPosition = region.borders[1].transform.localPosition = new Vector3(0, 0, 0.5f);
                        region.borders[2].transform.localPosition = new Vector3(regionGroup.AutoWidth() + region.xExtend, 0, 0.5f);
                        region.borders[3].transform.localPosition = new Vector3(0, -region.AutoHeight() - 4 - region.yExtend, 0.5f);
                        if (!defines.windowBorders)
                        {
                            region.borders[0].transform.localScale -= new Vector3(6, 0, 0);
                            region.borders[3].transform.localScale -= new Vector3(6, 0, 0);
                            region.borders[1].transform.localScale -= new Vector3(0, 4, 0);
                            region.borders[2].transform.localScale -= new Vector3(0, 4, 0);
                            region.borders[0].transform.localPosition += new Vector3(3, 0, 0);
                            region.borders[3].transform.localPosition += new Vector3(3, 0, 0);
                            region.borders[1].transform.localPosition -= new Vector3(0, 3, 0);
                            region.borders[2].transform.localPosition -= new Vector3(0, 3, 0);
                        }
                        if (title == "HostileAreaProgress")
                            if (regionGroup != headerGroup)
                            {
                                Destroy(region.borders[4]);
                                Destroy(region.borders[5]);
                                if (regionGroup.transform != transform.GetChild(1))
                                    Destroy(region.borders[6]);
                                if (regionGroup.transform != transform.GetChild(transform.childCount - 1))
                                {
                                    Destroy(region.borders[7]);
                                    region.borders[3].transform.localScale += new Vector3(4, 0, 0);
                                    region.borders[2].transform.localScale += new Vector3(0, 2, 0);
                                    region.borders[2].transform.localPosition += new Vector3(0, 1, 0);
                                }
                            }
                            else
                            {
                                region.borders[1].transform.localScale += new Vector3(0, 4, 0);
                                region.borders[2].transform.localScale += new Vector3(0, 4, 0);
                                region.borders[3].transform.localScale += new Vector3(2, 0, 0);
                                region.borders[3].transform.localPosition -= new Vector3(1, 0, 0);
                                Destroy(region.borders[6]);
                                Destroy(region.borders[7]);
                            }
                        if (regionGroup.AutoWidth() + region.xExtend - 1.5f - 19 * region.smallButtons.Count < 0 || 3.5f + 38 * region.bigButtons.Count >= regionGroup.AutoWidth() + region.xExtend)
                            for (int i = 0; i < 4; i++)
                                region.borders[i + 4].GetComponent<SpriteRenderer>().sprite = null;
                        else
                        {
                            region.borders[4].transform.localPosition = new Vector3(3.5f + (region.reverseButtons ? 19 * region.smallButtons.Count : 38 * region.bigButtons.Count), -3.5f, 0.05f);
                            region.borders[5].transform.localPosition = new Vector3(regionGroup.AutoWidth() + region.xExtend - 1.5f - (region.reverseButtons ? region.bigButtons.Count * 38 : 19 * region.smallButtons.Count), -3.5f, 0.05f);
                            region.borders[6].transform.localPosition = new Vector3(3.5f + (region.reverseButtons ? 19 * region.smallButtons.Count : 38 * region.bigButtons.Count), -region.AutoHeight() - 2.5f - region.yExtend, 0.05f);
                            region.borders[7].transform.localPosition = new Vector3(regionGroup.AutoWidth() + region.xExtend - 1.5f - (region.reverseButtons ? region.bigButtons.Count * 38 : (region.bigButtons.Count > 0 || region.lines.Count > 1 ? 0 : 19 * region.smallButtons.Count)), -region.AutoHeight() - 2.5f - region.yExtend, 0.05f);
                        }
                    }

            //Draws region shadows
            if (!defines.windowBorders && !disabledGeneralSprites && !disabledShadows)
                if (defines.shadowSystem == 1)
                    foreach (var region in regionGroup.regions)
                        if (region.backgroundType != Empty)
                        {
                            var shadowSprites = Resources.LoadAll<Sprite>("Sprites/Other/Second");
                            for (int i = 0; i < 5; i++)
                                if (region.shadows[i] == null)
                                {
                                    region.shadows[i] = new GameObject("Shadow", typeof(SpriteRenderer));
                                    region.shadows[i].transform.parent = region.transform;
                                    region.shadows[i].GetComponent<SpriteRenderer>().sprite = shadowSprites[i];
                                    region.shadows[i].GetComponent<SpriteRenderer>().sortingLayerName = layer + "Shadows";
                                }
                            region.shadows[1].transform.localScale = new Vector3(1, region.borders[2].transform.localScale.y - 5, 1);
                            region.shadows[3].transform.localScale = new Vector3(region.borders[3].transform.localScale.x - 5, 1, 1);
                            region.shadows[0].transform.localPosition = new Vector3(region.borders[2].transform.localPosition.x + 2, -4, 0.9f);
                            region.shadows[1].transform.localPosition = new Vector3(region.borders[2].transform.localPosition.x + 2, -8, 0.9f);
                            region.shadows[2].transform.localPosition = new Vector3(region.borders[2].transform.localPosition.x - 1, region.borders[3].transform.localPosition.y + 1, 0.9f);
                            region.shadows[3].transform.localPosition = new Vector3(8, region.borders[3].transform.localPosition.y - 2, 0.9f);
                            region.shadows[4].transform.localPosition = new Vector3(4, region.borders[3].transform.localPosition.y - 2, 0.9f);
                            region.shadows[2].GetComponent<SpriteRenderer>().sprite = shadowSprites[5];
                            if (title == "HostileAreaProgress")
                                if (regionGroup != headerGroup)
                                {
                                    Destroy(region.shadows[0]);
                                    if (regionGroup.transform != transform.GetChild(1))
                                        Destroy(region.shadows[4]);
                                    if (regionGroup.transform != transform.GetChild(transform.childCount - 1))
                                    {
                                        region.shadows[3].transform.localScale += new Vector3(5, 0, 0);
                                        Destroy(region.shadows[2]);
                                    }
                                }
                                else
                                {
                                    region.shadows[1].transform.localScale += new Vector3(0, 5, 0);
                                    Destroy(region.shadows[2]);
                                }
                        }

            #endregion

            regionGroup.currentHeight += extendOffset;
            if (headerGroup != regionGroup)
                xOffset += regionGroup.AutoWidth();
        }
    }
}