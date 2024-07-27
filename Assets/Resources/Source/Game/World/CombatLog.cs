using System.Collections.Generic;

public class CombatLog
{
   public CombatLog()
   {
      damageDealt = new();
      damageTaken = new();
      healingReceived = new();
      elementsUsed = new();
   }

   //Damage dealt by player to the enemy
   public Dictionary<string, int> damageDealt;

   //Damage dealt to player
   public Dictionary<string, int> damageTaken;

   //Healing received by the player
   public Dictionary<string, int> healingReceived;

   //Elements used by the player
   public Dictionary<string, int> elementsUsed;
}
