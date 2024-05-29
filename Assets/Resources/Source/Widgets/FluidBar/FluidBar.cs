using System;
using System.Linq;

using UnityEngine;

public class FluidBar : MonoBehaviour
{
    //Width in pixels of the fluid bar
    public int barWidth;

    //Max value for the fluid bar
    public Func<int> max;

    //Function to see current status
    public Func<int> current;

    //Reference to the splitter
    public SpriteRenderer split;

    //Initialises the fluid bars parameters
    public void Initialise(int barWidth, Func<int> max, Func<int> current)
    {
        this.barWidth = barWidth;
        this.max = max;
        this.current = current;
        split = GetComponentsInChildren<SpriteRenderer>().First(x => x.name == "BarSplit");
    }

    //Updates the length of the fluid bar
    public void UpdateFluidBar()
    {
        var aim = (int)Math.Ceiling((double)barWidth / max() * current());
        if (aim <= 0) aim = -2;
        else if (aim < 2) aim = 2;
        else if (aim > barWidth - 4) aim = barWidth;
        split.transform.localPosition = new Vector2(aim - 2, -2);
    }
}
