using System.Collections.Generic;

using UnityEngine;

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
