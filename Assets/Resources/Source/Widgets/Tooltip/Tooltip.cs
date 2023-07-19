using System;
using System.Diagnostics;
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
        if (CDesktop.windows.Contains(window))
            UnityEngine.Debug.Log(window.name + " is no longer accessible for tooltip");
        SpawnWindowBlueprint(new Blueprint("Tooltip", tooltip(caller()), true));
        window = caller().window.desktop.LBWindow;
        CloseWindowOnLostFocus();
    }
}
