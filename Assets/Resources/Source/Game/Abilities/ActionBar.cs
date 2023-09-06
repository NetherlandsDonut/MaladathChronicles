using System;

public class ActionBar
{
    public ActionBar() { }

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

    //Name of the ability in this action bar slot
    public string ability;

    //Duration of the cooldown that is left
    [NonSerialized] public int cooldown;
}
