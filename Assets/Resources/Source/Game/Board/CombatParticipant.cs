using System.Collections.Generic;

public class CombatParticipant
{
    //Who is this combat participant
    public Entity who;

    //On which team this entity is
    public int team;

    //Is this participant human controlled
    public bool human;

    //Amount of threat generated
    public int generatedThreat;

    //Abilities (Active and passive) that this participant has in the combat
    public Dictionary<Ability, int> combatAbilities;

    //Last targetted participant
    public CombatParticipant lastTarget;

    //The base threat equilibrium point for this participant
    public int BaseThreat() => who.MaxHealth();

    //Swaps this combatant's team in combat
    public void SwapTeam(int newTeam)
    {
        var ind = Board.board.participants.IndexOf(this);
        if (team == 1) Board.board.team1.Remove(ind);
        else Board.board.team2.Remove(ind);
        if (newTeam == 1) Board.board.team1.Add(ind);
        else Board.board.team2.Add(ind);
        team = newTeam;
        Root.Respawn("EnemyBattleInfo");
        Root.Respawn("FriendlyBattleInfo");
        foreach (var res in who.resources)
        {
            Root.CloseWindow("Friendly" + res.Key + "Resource");
            Root.SpawnWindowBlueprint("Friendly" + res.Key + "Resource");
            Root.CloseWindow("Enemy" + res.Key + "Resource");
            Root.SpawnWindowBlueprint("Enemy" + res.Key + "Resource");
        }
    }
}
