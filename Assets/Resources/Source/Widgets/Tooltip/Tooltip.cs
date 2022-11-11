using System;

using static Root;

public class Tooltip
{
    public Tooltip(Func<Highlightable> caller, Action tooltip)
    {
        this.caller = caller;
        this.tooltip = tooltip;
    }

    public Window window;
    public Action tooltip;
    public Func<Highlightable> caller;

    public void SpawnTooltip()
    {
        SpawnWindowBlueprint(new Blueprint("Tooltip", tooltip));
        window = caller().window.desktop.LBWindow;
        CloseWindowOnLostFocus();
    }
}
