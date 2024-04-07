using System;

public class ActionBar
{
    #region Initialisation

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

    #endregion

    //Name of the ability in this action bar slot
    public string ability;

    //Duration of the cooldown that is left before the ability can be used again
    //It is not serialised because I don't plan to let anyone save the game during the board section
    [NonSerialized] public int cooldown;
}
