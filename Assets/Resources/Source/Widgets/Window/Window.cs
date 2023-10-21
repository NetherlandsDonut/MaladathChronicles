using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;
using static Root.RegionBackgroundType;

using static GameSettings;

using static InputLine;

public class Window : MonoBehaviour
{
    public Desktop desktop;
    public GameObject groupGrouping;
    public List<RegionGroup> regionGroups;
    public RegionGroup LBRegionGroup, headerGroup;
    public int xOffset, yOffset;
    public string title, layer;
    public WindowAnchor anchor;
    public GameObject background;
    public GameObject[] shadows;
    public bool disabledShadows, disabledGeneralSprites, disabledCollisions, masked;

    public void Initialise(Desktop desktop, string title, bool upperUI)
    {
        this.title = title;
        if (title == "BattleBoard") Board.board.window = this;
        else if (title == "BufferBoard") BufferBoard.bufferBoard.window = this;
        this.desktop = desktop;
        anchor = new WindowAnchor(Center);
        regionGroups = new();
        shadows = new GameObject[8];
        if (upperUI) layer = "Upper";
        else layer = "Default";

        desktop.LBWindow = this;
        desktop.windows.Add(this);
    }

    public int PlannedHeight(bool includeHeader = false)
    {
        return (regionGroups.Count > 0 ? regionGroups.Max(x => x.PlannedHeight()) : 0) + (includeHeader && headerGroup != null ? (headerGroup.setHeight != 0 ? headerGroup.setHeight : headerGroup.PlannedHeight()) : 0);
    }

    public void ResetPosition()
    {
        if (anchor.anchor != None && anchor.magnet == null) transform.parent = desktop.screen.transform;
        transform.localPosition = Vector3.zero;
        transform.localPosition = anchor.magnet != null ? MagnetAnchor(anchor.magnet.transform.localPosition, new Vector2(anchor.magnet.Width(), anchor.magnet.PlannedHeight())) : Anchor();
        transform.localPosition += (anchor.anchor != None && anchor.magnet == null ? new Vector3(screenX / -2, screenY / 2) : Vector3.zero) + (Vector3)anchor.offset;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -desktop.windows.Count + (layer == "Default" ? 1024 : 0));

        Vector2 Anchor()
        {
            switch (anchor.anchor)
            {
                case Bottom: return new Vector2(screenX / 2 - Width() / 2 - 1, 2 - screenY + yOffset);
                case BottomRight: return new Vector2(screenX - 2 - Width(), 2 - screenY + yOffset);
                case BottomLeft: return new Vector2(0, 2 - screenY + yOffset);
                case Top: return new Vector2(screenX / 2 - Width() / 2 - 1, 0);
                case TopRight: return new Vector2(screenX - 2 - Width(), 0);
                case TopLeft: return new Vector2(0, 0);
                case Center: return new Vector2(screenX / 2 - Width() / 2 - 1, screenY / -2 + yOffset / 2);
                default: return new Vector2(0, 0);
            }
        }

        Vector2 MagnetAnchor(Vector2 position, Vector2 size)
        {
            switch (anchor.anchor)
            {
                case Bottom: return new Vector2(position.x - (Width() - size.x) / 2, position.y - size.y);
                case BottomLeft: return new Vector2(position.x + size.x - Width(), position.y - size.y);
                case BottomRight: return new Vector2(position.x, position.y - size.y);
                case Top: return new Vector2(position.x - (Width() - size.x) / 2, position.y + yOffset);
                case TopRight: return new Vector2(position.x + size.x, position.y);
                case TopLeft: return new Vector2(position.x - Width(), position.y);
                case Center: return new Vector2(screenX / 2 - Width() / 2, screenY / -2 + yOffset / 2);
                case RightTop: return new Vector2(position.x + size.x, position.y);
                case RightBottom: return new Vector2(position.x + size.x, position.y - size.y + yOffset);
                default: return new Vector2(0, 0);
            }
        }
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
        var paginations = regionGroups.Select(x => x.pagination).ToList();
        CloseWindow(this);
        SpawnWindowBlueprint(title, false, paginations);
    }

    public void Rebuild(List<int> paginations)
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
        yOffset = (regionGroups.Count > 0 ? regionGroups.Max(x => x.currentHeight) : 0) + (headerGroup != null ? headerGroup.currentHeight : 0);
        groupGrouping.transform.localPosition = new Vector3(0, headerGroup != null ? -headerGroup.currentHeight : 0, 0);

        //Draws window background
        if (!disabledGeneralSprites)
        {
            if (background == null)
                background = new GameObject("Window Background", typeof(SpriteRenderer));
            background.transform.parent = transform;
            background.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Backgrounds/Window");
            background.GetComponent<SpriteRenderer>().sortingLayerName = layer;
            background.GetComponent<SpriteRenderer>().sortingOrder = -10;
            background.transform.localScale = new Vector3(xOffset + 2, PlannedHeight() + (headerGroup != null ? headerGroup.PlannedHeight() : 0) + 2, 1);
            background.transform.localPosition = new Vector3(0, 0, 0.9f);
            if (background.GetComponent<BoxCollider2D>() == null)
                background.AddComponent<BoxCollider2D>();
            if (disabledCollisions)
                Destroy(background.GetComponent<BoxCollider2D>());
        }

        //Draws window shadows
        if (!disabledGeneralSprites && !disabledShadows && settings.shadows.Value())
            if (shadowSystem == 0)
            {
                var shadowSprites = Resources.LoadAll<Sprite>("Sprites/Building/Shadows/First");
                for (int i = 0; i < 8; i++)
                    if (shadows[i] == null)
                    {
                        shadows[i] = new GameObject("Shadow", typeof(SpriteRenderer));
                        shadows[i].transform.parent = transform;
                        shadows[i].GetComponent<SpriteRenderer>().sprite = shadowSprites[i];
                        shadows[i].GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    }
                shadows[1].transform.localScale = shadows[6].transform.localScale = new Vector3(Width() + 2, 1, 1);
                shadows[3].transform.localScale = shadows[4].transform.localScale = new Vector3(1, yOffset + 2, 1);
                shadows[0].transform.localPosition = new Vector3(-18, 18, 0.9f);
                shadows[1].transform.localPosition = new Vector3(0, 18, 0.9f);
                shadows[2].transform.localPosition = new Vector3(Width() + 2, 18, 0.9f);
                shadows[3].transform.localPosition = new Vector3(-18, 0, 0.9f);
                shadows[4].transform.localPosition = new Vector3(Width() + 2, 0, 0.9f);
                shadows[5].transform.localPosition = new Vector3(-18, -yOffset - 2, 0.9f);
                shadows[6].transform.localPosition = new Vector3(0, -yOffset - 2, 0.9f);
                shadows[7].transform.localPosition = new Vector3(Width() + 2, -yOffset - 2, 0.9f);
            }
            else if (shadowSystem == 1)
            {
                var shadowSprites = Resources.LoadAll<Sprite>("Sprites/Building/Shadows/Second");
                for (int i = 0; i < 5; i++)
                    if (shadows[i] == null)
                    {
                        shadows[i] = new GameObject("Shadow", typeof(SpriteRenderer));
                        shadows[i].transform.parent = transform;
                        shadows[i].GetComponent<SpriteRenderer>().sprite = shadowSprites[i];
                        shadows[i].GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    }
                shadows[1].transform.localScale = new Vector3(1, yOffset - 2, 1);
                shadows[3].transform.localScale = new Vector3(Width() - 2, 1, 1);
                shadows[0].transform.localPosition = new Vector3(Width() + 2, 0, 0.9f);
                shadows[1].transform.localPosition = new Vector3(Width() + 2, -4, 0.9f);
                shadows[2].transform.localPosition = new Vector3(Width() + 2, -yOffset - 2, 0.9f);
                shadows[3].transform.localPosition = new Vector3(4, -yOffset - 2, 0.9f);
                shadows[4].transform.localPosition = new Vector3(0, -yOffset - 2, 0.9f);
            }
            else
            {
                var shadowSprites = Resources.LoadAll<Sprite>("Sprites/Building/Shadows/Third");
                for (int i = 0; i < 8; i++)
                    if (shadows[i] == null)
                    {
                        shadows[i] = new GameObject("Shadow", typeof(SpriteRenderer));
                        shadows[i].transform.parent = transform;
                        shadows[i].GetComponent<SpriteRenderer>().sprite = shadowSprites[i];
                        shadows[i].GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    }
                shadows[1].transform.localScale = shadows[6].transform.localScale = new Vector3(Width() + 2, 1, 1);
                shadows[3].transform.localScale = shadows[4].transform.localScale = new Vector3(1, yOffset + 2, 1);
                shadows[0].transform.localPosition = new Vector3(-5, 5, 0.9f);
                shadows[1].transform.localPosition = new Vector3(0, 5, 0.9f);
                shadows[2].transform.localPosition = new Vector3(Width() + 2, 5, 0.9f);
                shadows[3].transform.localPosition = new Vector3(-5, 0, 0.9f);
                shadows[4].transform.localPosition = new Vector3(Width() + 2, 0, 0.9f);
                shadows[5].transform.localPosition = new Vector3(-5, -yOffset - 2, 0.9f);
                shadows[6].transform.localPosition = new Vector3(0, -yOffset - 2, 0.9f);
                shadows[7].transform.localPosition = new Vector3(Width() + 2, -yOffset - 2, 0.9f);
            }
        if (masked) GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(x => x.maskInteraction = SpriteMaskInteraction.VisibleInsideMask);

        void BuildRegionGroup(RegionGroup regionGroup)
        {
            int extendOffset = 0;
            CDesktop.LBWindow.LBRegionGroup = regionGroup;
            int index = CDesktop.LBWindow.regionGroups.IndexOf(regionGroup);
            if (index != -1 && paginations?.Count > index)
                if (regionGroup.maxPagination() < paginations[index]) regionGroup.pagination = regionGroup.maxPagination() - 1;
                else regionGroup.pagination = paginations[index];

            #region CREATING REGIONS

            //Draw all the regions
            foreach (var region in regionGroup.regions)
            {
                regionGroup.LBRegion = region;
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
                    var objectOffset = (region.checkbox != null ? 15 : 0) + region.bigButtons.Count * 38;
                    int length = 0;
                    foreach (var text in line.texts)
                    {
                        text.Erase();
                        var split = new List<string>();
                        foreach (var character in text.text)
                            if (split.Count > 0 && split[split.Count - 1] == " ")
                                split[split.Count - 1] += character;
                            else split.Add(character + "");
                        foreach (var part in split)
                            if (regionGroup.setWidth == 0)
                                foreach (var character in part)
                                    length = text.SpawnCharacter(character, length);
                            else if (textPaddingLeft + 6 + (line.align == "Right" ? 2 : 0) + length + Font.fonts["Tahoma Bold"].Length(part) + (split.Last() == part ? 0 : Font.fonts["Tahoma Bold"].Length(textWrapEnding)) + objectOffset < regionGroup.setWidth - region.smallButtons.Count * 19)
                                foreach (var character in part)
                                    length = text.SpawnCharacter(character, length);
                            else
                            {
                                for (int i = 0; i < 3; i++)
                                    length = text.SpawnCharacter(textWrapEnding[i], length);
                                break;
                            }
                    }
                    if (line.align == "Left")
                        line.transform.localPosition = new Vector3(2 + textPaddingLeft + objectOffset, -region.currentHeight - 3, 0);
                    else if (line.align == "Center")
                        line.transform.localPosition = new Vector3(2 + (region.regionGroup.AutoWidth() / 2) - (length / 2), -region.currentHeight - 3, 0);
                    else if (line.align == "Right")
                        line.transform.localPosition = new Vector3(-textPaddingLeft + region.regionGroup.AutoWidth() - (region.smallButtons.Count * 19) - length, -region.currentHeight - 3, 0);
                    region.currentHeight += 15;
                }

            //Draws small buttons for single lined regions
            foreach (var region in regionGroup.regions)
                foreach (var smallButton in region.smallButtons)
                {
                    if (region.currentHeight < 15) region.currentHeight = 15;
                    if (smallButton.buttonType == null) continue;
                    var load = Resources.Load<Sprite>("Sprites/Building/Buttons/" + smallButton.buttonType);
                    smallButton.GetComponent<SpriteRenderer>().sprite = load == null ? Resources.Load<Sprite>("Sprites/Building/Buttons/OtherEmpty") : load;
                    smallButton.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    smallButton.transform.localPosition = new Vector3(regionGroup.AutoWidth() - 10 + region.xExtend + 1.5f - 19 * region.smallButtons.IndexOf(smallButton), -10.5f, 0.1f);
                    if (smallButton.gameObject.GetComponent<BoxCollider2D>() == null)
                        smallButton.gameObject.AddComponent<BoxCollider2D>();
                    if (smallButton.gameObject.GetComponent<Highlightable>() == null)
                        smallButton.gameObject.AddComponent<Highlightable>().Initialise(region, null, null, null, null);
                    if (smallButton.frame == null)
                        smallButton.frame = new GameObject("ButtonFrame", typeof(SpriteRenderer));
                    smallButton.frame.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Borders/ButtonFrame");
                    smallButton.frame.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    smallButton.frame.transform.parent = smallButton.transform;
                    smallButton.frame.transform.localPosition = new Vector3();
                    if (disabledCollisions) Destroy(smallButton.GetComponent<BoxCollider2D>());
                }

            //Draws big buttons for single lined regions
            foreach (var region in regionGroup.regions)
            {
                if (region.bigButtons.Count > 0 && region.currentHeight < 34) region.currentHeight = 34;
                foreach (var bigButton in region.bigButtons)
                {
                    if (bigButton.buttonType == null) continue;
                    var load = Resources.Load<Sprite>("Sprites/Building/BigButtons/" + bigButton.buttonType);
                    bigButton.GetComponent<SpriteRenderer>().sprite = load == null ? Resources.Load<Sprite>("Sprites/Building/BigButtons/OtherEmpty") : load;
                    bigButton.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    bigButton.transform.localPosition = new Vector3(20 + 38 * region.bigButtons.IndexOf(bigButton), -20f, 0.1f);
                    if (bigButton.gameObject.GetComponent<BoxCollider2D>() == null)
                        bigButton.gameObject.AddComponent<BoxCollider2D>();
                    if (bigButton.gameObject.GetComponent<Highlightable>() == null)
                        bigButton.gameObject.AddComponent<Highlightable>().Initialise(region, null, null, null, null);
                    if (bigButton.frame == null)
                        bigButton.frame = new GameObject("BigButtonFrame", typeof(SpriteRenderer));
                    bigButton.frame.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Borders/BigButtonFrame");
                    bigButton.frame.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    bigButton.frame.transform.parent = bigButton.transform;
                    bigButton.frame.transform.localPosition = new Vector3();
                    if (disabledCollisions) Destroy(bigButton.GetComponent<BoxCollider2D>());
                }
            }

            //Draws checkbox for the region
            foreach (var region in regionGroup.regions)
                if (region.checkbox != null)
                {
                    region.checkbox.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Checkboxes/" + (region.backgroundType == RedButton || region.backgroundType == Button ? "Dark" : "Bright") + (region.checkbox.value.Value() ? "On" : "Off"));
                    region.checkbox.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    region.checkbox.transform.localPosition = new Vector3(10.5f, -10.5f, 0.1f);
                    if (region.checkbox.gameObject.GetComponent<BoxCollider2D>() == null)
                        region.checkbox.gameObject.AddComponent<BoxCollider2D>();
                    if (region.checkbox.gameObject.GetComponent<Highlightable>() == null && region.backgroundType != RedButton && region.backgroundType != Button)
                        region.checkbox.gameObject.AddComponent<Highlightable>().Initialise(region, null, null, null, null);
                    if (region.checkbox.frame == null)
                        region.checkbox.frame = new GameObject("CheckboxFrame", typeof(SpriteRenderer));
                    region.checkbox.frame.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Borders/CheckboxFrame");
                    region.checkbox.frame.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    region.checkbox.frame.transform.parent = region.checkbox.transform;
                    region.checkbox.frame.transform.localPosition = new Vector3();
                    if (disabledCollisions) Destroy(region.checkbox.GetComponent<BoxCollider2D>());
                }

            //Draws inputLines for the regions
            foreach (var region in regionGroup.regions)
                if (region.inputLine != null)
                {
                    if (region.lines.Count > 0)
                        region.inputLine.transform.localPosition = new Vector3(11 + region.lines[0].Length(), -region.currentHeight + 12, 0);
                    else
                    {
                        if (region.currentHeight < 15) region.currentHeight = 15;
                        region.inputLine.transform.localPosition = new Vector3(2 + textPaddingLeft, -region.currentHeight + 12, 0);
                    }
                    int length = 0;
                    region.inputLine.text.Erase();
                    var print = region.inputLine.text.text.Value();
                    if (inputDestination == region.inputLine.text.text && inputLineName == region.inputLine.name)
                        print = print.Insert(inputLineMarker > print.Length ? print.Length : inputLineMarker, markerCharacter);
                    else print += " ";
                    foreach (var character in print)
                        length = region.inputLine.text.SpawnCharacter(character, length, region.inputLine.color);
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
                        region.yExtend = regionGroup.setHeight - regionGroup.PlannedHeight() + 10;
                }
                else if (regionGroup.PlannedHeight() < regionGroup.setHeight)
                    if (regionGroup.stretchRegion == region || regionGroup.stretchRegion == null && region == regionGroup.regions.Last())
                        region.yExtend = regionGroup.setHeight - regionGroup.PlannedHeight();
                if (region.yExtend > 0) extendOffset += region.yExtend;
                regionGroup.currentHeight += 4 + region.currentHeight;
            }

            #endregion

            #region BORDERS & BACKGROUNDS

            //Draws region backgrounds
            if (!disabledGeneralSprites)
                foreach (var region in regionGroup.regions)
                {
                    region.background.transform.parent = region.transform;
                    region.background.GetComponent<RegionBackground>().Initialise(region);
                    region.background.GetComponent<SpriteRenderer>().sprite = region.backgroundType == Image ? region.backgroundImage : Resources.Load<Sprite>("Sprites/Building/Backgrounds/" + region.backgroundType);
                    region.background.GetComponent<SpriteRenderer>().sortingLayerName = layer;
                    if (region.backgroundType == Image) region.background.transform.localScale = new Vector3(1, 1, 1);
                    else region.background.transform.localScale = new Vector3(regionGroup.AutoWidth() - 2 + region.xExtend, region.AutoHeight() + 2 + region.yExtend, 1);
                    region.background.transform.localPosition = new Vector3(2, -2, 0.8f);
                    if (region.backgroundType == Button || region.backgroundType == RedButton)
                    {
                        if (region.background.GetComponent<BoxCollider2D>() == null)
                            region.background.AddComponent<BoxCollider2D>();
                        region.background.GetComponent<BoxCollider2D>().enabled = !disabledCollisions;
                    }
                }

            //Draws region borders
            if (!disabledGeneralSprites)
                foreach (var region in regionGroup.regions)
                {
                    for (int i = 0; i < 4; i++)
                        if (region.borders[i] == null)
                        {
                            region.borders[i] = new GameObject("Border", typeof(SpriteRenderer));
                            region.borders[i].transform.parent = region.transform;
                            region.borders[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Building/Borders/RegionBorder");
                            region.borders[i].GetComponent<SpriteRenderer>().sortingLayerName = layer;
                        }
                    region.borders[0].transform.localScale = region.borders[3].transform.localScale = new Vector3(regionGroup.AutoWidth() + 2 + region.xExtend, 2, 2);
                    region.borders[1].transform.localScale = region.borders[2].transform.localScale = new Vector3(2, region.AutoHeight() + 4 + region.yExtend, 2);
                    region.borders[0].transform.localPosition = region.borders[1].transform.localPosition = new Vector3(0, 0, 0.5f);
                    region.borders[2].transform.localPosition = new Vector3(regionGroup.AutoWidth() + region.xExtend, 0, 0.5f);
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

            //Asigning tooltips for big buttons
            foreach (var region in regionGroup.regions)
                if (region.bigButtons.Count > 0)
                    foreach (var bigButtonHandle in region.bigButtons.Select(x => x.GetComponent<TooltipHandle>()).ToList().FindAll(x => x != null))
                        bigButtonHandle.ApplyTooltip();

            //Asigning tooltips for checkboxes
            foreach (var region in regionGroup.regions)
                if (region.checkbox != null && region.checkbox.GetComponent<TooltipHandle>() != null)
                    region.checkbox.GetComponent<TooltipHandle>().ApplyTooltip();

            #endregion

            regionGroup.currentHeight += extendOffset;
            if (headerGroup != regionGroup)
                xOffset += regionGroup.AutoWidth();
        }
    }
}