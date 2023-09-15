using UnityEngine;

public class TooltipHandle : MonoBehaviour
{
    public Tooltip tooltip;

    public void ApplyTooltip()
    {
        tooltip.caller().tooltip = tooltip;
        Destroy(this);
    }
}
