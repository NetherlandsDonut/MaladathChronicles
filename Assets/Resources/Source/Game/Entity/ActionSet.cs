using System.Collections.Generic;

public class ActionSet
{
    //Name of the action set
    public string name;

    //Icon for this action set
    public string icon;

    //The ability that makes this action set appear for the player in the spellbook
    public string abilityForVisibility;

    //Whether this action set is visible regardless of everything
    public bool alwaysVisible;

    //Abilities forced onto the user of this set, these cannot be removed
    //However they don't have to be the only ones used if player can edit it
    public List<string> forcedAbilities;

    //Currently opened action set
    public static ActionSet actionSet;

    //EXTERNAL FILE: List containing all action sets in-game
    public static List<ActionSet> actionSets;
}
