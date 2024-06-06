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

    //Where to move the splitter slowly
    public Vector3 newAim;

    //Time the splitter is moving
    public float time;

    //Time the splitter is moving
    public bool vertical;

    //Initialises the fluid bars parameters
    public void Initialise(int barWidth, Func<int> max, Func<int> current, bool vertical)
    {
        this.barWidth = barWidth;
        this.max = max;
        this.current = current;
        this.vertical = vertical;
        if (vertical)
        {
            GetComponentsInChildren<SpriteMask>().First(x => x.name == "EmptyBar").transform.localScale = new Vector3(1, this.max() * 8 + 2, 1);
            GetComponentsInChildren<SpriteRenderer>().First(x => x.name == "Capstone").transform.localPosition = new Vector3(0, -(this.max() * 8 - 2), 0);
        }
        split = GetComponentsInChildren<SpriteRenderer>().First(x => x.name == "BarSplit");
        UpdateFluidBar();
        split.transform.localPosition = newAim;
    }

    //Updates the length of the fluid bar
    public void UpdateFluidBar()
    {
        var aim = (int)Math.Ceiling((double)barWidth / max() * current());
        if (vertical)
        {
            if (aim < 2) aim = 0;
            else if (aim <= 8) aim = 8;
            else if (aim > barWidth - 4) aim = barWidth - 2;
            newAim = new Vector3(0, -10 - aim);
        }
        else
        {
            if (aim <= 0) aim = -4;
            else if (aim < 2) aim = 0;
            else if (aim > barWidth - 4) aim = barWidth - 2;
            newAim = new Vector3(aim, -2);
        }
        time = 0; 
    }

    public void Update()
    {
        if (Vector3.Distance(split.transform.localPosition, newAim) > 0.01f)
        {
            time += Time.deltaTime;
            split.transform.localPosition = Vector3.Lerp(split.transform.localPosition, newAim, time / 6);
        }
    }
}
