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

    public void SpawnTooltip(bool playSound = true)
    {
        if (WindowUp("QuestList")) SetDesktopBackground("Backgrounds/RuggedLeatherFull", true, true);
        SpawnWindowBlueprint(new Blueprint("Tooltip", () => { DisableCollisions(); tooltip(caller())(); }, true));
        if (playSound) return;
        var newWindow = CDesktop.LBWindow();
        if (newWindow.title == "Tooltip" && newWindow.LBRegionGroup() != null)
            Sound.PlaySound("DesktopTooltipShow", 0.4f);
    }
}
