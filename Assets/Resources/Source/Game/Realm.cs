using System.Collections.Generic;

using static SaveGame;

public class Realm
{
   //Initialisation method to fill automatic values
   //and remove empty collections to avoid serialising them later
   public void Initialise()
   {
      if (!saves.ContainsKey(name))
         saves.Add(name, new());
   }

   //Name of the realm
   public string name;

   //Indicates whether the realm has permadeath rules on
   public bool hardcore;

   //Indicates whether the realm has pvp content
   public bool pvp;

   //EXTERNAL FILE: List containing all realms in-game
   public static List<Realm> realms;
}
