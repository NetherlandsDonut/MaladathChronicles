using System;
using UnityEngine;
using System.Collections.Generic;

using static Root;
using System.Linq;

public class LineBigButton : MonoBehaviour
{
    //Parent
    public Region region;

    //Children
    public GameObject frame;

    //Fields
    public Tooltip tooltip;
    public Action<Highlightable> pressEvent;
    public string buttonType;
    //public List<GameObject> overlays;

    public void Initialise(Region region, string buttonType, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip)
    {
        this.region = region;
        this.buttonType = buttonType;
        this.pressEvent = pressEvent;
        //overlays = new List<GameObject>();
        if (tooltip != null && gameObject != null)
            this.tooltip = new Tooltip(() => GetComponent<Highlightable>(), tooltip);

        region.LBBigButton = this;
        region.bigButtons.Add(this);
    }

    public void OnMouseUp()
    {
        if (cursor.render.sprite == null) return;
        if (pressEvent != null)
        {
            PlaySound("DesktopButtonPress", 0.6f);
            pressEvent(GetComponent<Highlightable>());
            region.regionGroup.window.Rebuild();
        }
    }

    //public void OnMouseEnter()
    //{
    //    if (name.Contains("Element") && region.regionGroup.window.title == "BattleBoard")
    //    {
    //        var x = region.bigButtons.IndexOf(this);
    //        var y = region.regionGroup.regions.IndexOf(region);
    //        var list = Board.board.FloodCount(x, y);
    //        foreach (var button in list.Select(x => region.regionGroup.regions[x.Item2].bigButtons[x.Item1]))
    //            overlays.Add(AddBigButtonOverlay(button.gameObject, "OtherCross"));
    //    }
    //}

    //public void OnMouseExit()
    //{
    //    if (name.Contains("Element") && region.regionGroup.window.title == "BattleBoard")
    //    {
    //        overlays.ForEach(x => x.AddComponent<Shatter>().Initiate(0));
    //        overlays = new List<GameObject>();
    //    }
    //}
}
