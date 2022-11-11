using System;
using UnityEngine;
using System.Collections.Generic;

using static Root;

using static Root.Color;

public class LineDropdown : MonoBehaviour
{
    //Fields
    public string currentChoice;
    public Func<List<string>> choices;
    public Func<string, string> headerChange;

    public void Initialise(Func<string, string> headerChange, Func<List<string>> choices)
    {
        this.headerChange = headerChange;
        this.choices = choices;

        GetComponent<Region>().dropdown = this;
    }

    public void Unwind()
    {
        SpawnWindowBlueprint(new Blueprint("DropdownMenu",
            () =>
            {
                var region = GetComponent<Region>();
                SetAnchor(region.regionGroup.window.transform.position.x, region.regionGroup.window.transform.position.y + region.transform.localPosition.y + region.PlannedHeight() + (region.regionGroup.window.headerGroup == null ? 0 : region.regionGroup.window.headerGroup.PlannedHeight()));
                AddRegionGroup();
                SetRegionGroupWidth(region.AutoWidth());
                var choicesListed = choices();
                AddLineList(choicesListed.Count,
                    (i) =>
                    {
                        AddLine(choicesListed[i], Black);
                    },
                    (h) =>
                    {
                        currentChoice = h.region.lines[0].texts[0].text;
                        GetComponent<Region>().regionGroup.window.desktop.Rebuild();
                        CloseWindow(h.window);
                    }
                );
            }
        ));
        CloseWindowOnLostFocus();
    }
}
