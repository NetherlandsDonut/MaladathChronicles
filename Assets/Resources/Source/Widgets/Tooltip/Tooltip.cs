using System;

using static Root;

public class Tooltip
{
    public Tooltip(Func<Highlightable> caller, Func<Highlightable, Action> tooltip)
    {
        this.caller = caller;
        this.tooltip = tooltip;
    }

    public Func<Highlightable, Action> tooltip;
    public Func<Highlightable> caller;

    public void SpawnTooltip()
    {
        SpawnWindowBlueprint(new Blueprint("Tooltip", () => { DisableCollisions(); tooltip(caller())(); }, true));
        if (CDesktop.LBWindow().LBRegionGroup() != null) Sound.PlaySound("DesktopTooltipShow", 0.4f);
        else CloseWindow(CDesktop.LBWindow());
    }
}
