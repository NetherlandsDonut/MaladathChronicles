using System.Collections.Generic;

public class CombatParticipant
{
    //Who is this combat participant
    public Entity who;

    //On which team this entity is
    public int team;

    //Is this participant human controlled
    public bool human;

    //Abilities (Active and passive) that this participant has in the combat
    public Dictionary<Ability, int> combatAbilities;
}

public class FutureCombatParticipant
{
    //Who is this combat participant
    public FutureEntity who;

    //On which team this entity is
    public int team;

    //Is this participant human controlled
    public bool human;

    //Abilities (Active and passive) that this participant has in the combat
    public Dictionary<Ability, int> combatAbilities;

    Dictionary<string, double> elementImportance;
}

