using System.Collections.Generic;

using static Assets;

public class Faction
{
   //Name of the faction
   public string name;

   //Icon of the faction
   public string icon;

   //Side of the conflict this faction is on
   //Can be: Alliance, Horde and Neutral
   public string side;

   //Gets the icon of this faction
   //If it's not set then it uses the default icon for it's side
   public string Icon()
   {
      if (assets.factionIcons.Contains(icon + ".png")) return icon;
      else return "Faction" + side;
   }

   //Currently opened faction
   public static Faction faction;

   //EXTERNAL FILE: List containing all factions in-game
   public static List<Faction> factions;

   //List of all filtered factions by input search
   public static List<Faction> factionsSearch;
}
