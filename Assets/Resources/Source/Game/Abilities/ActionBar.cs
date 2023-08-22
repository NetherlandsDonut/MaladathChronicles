using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar
{
    public ActionBar(string ability)
    {
        this.ability = ability;
        cooldown = 0;
    }

    public ActionBar(string ability, int cooldown)
    {
        this.ability = ability;
        this.cooldown = cooldown;
    }

    public string ability;
    public int cooldown;
}
