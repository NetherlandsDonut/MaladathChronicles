using System;

using static Root;

public class Tooltip
{
    public Tooltip(Func<Highlightable> caller, Func<Highlightable, Action> tooltip)
    {
        this.caller = caller;
        this.tooltip = tooltip;
    }

    public Window window;
    public Func<Highlightable, Action> tooltip;
    public Func<Highlightable> caller;

    public void SpawnTooltip()
    {
        SpawnWindowBlueprint(new Blueprint("Tooltip", tooltip(caller())));
        window = caller().window.desktop.LBWindow;
        CloseWindowOnLostFocus();
    }
}
