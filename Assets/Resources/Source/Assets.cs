using System;
using System.Collections.Generic;

//Assets class is responsible for storing information
//about usable assets for in-game content such as icons or sounds
public class Assets
{
   //List of all ambience tracks available in-game
   public List<string> ambience;

   //List of filtered ambience tracks by input search
   [NonSerialized] public List<string> ambienceSearch;

   //List of all sound effects available in-game
   public List<string> sounds;

   //List of filtered sound effects by input search
   [NonSerialized] public List<string> soundsSearch;

   //List of all item icons available in-game
   public List<string> itemIcons;

   //List of filtered item icons by input search
   [NonSerialized] public List<string> itemIconsSearch;

   //List of all ability icons available in-game
   public List<string> abilityIcons;

   //List of filtered ability icons by input search
   [NonSerialized] public List<string> abilityIconsSearch;

   //List of all faction icons available in-game
   public List<string> factionIcons;

   //List of filtered faction icons by input search
   [NonSerialized] public List<string> factionIconsSearch;

   //List of all mount icons available in-game
   public List<string> mountIcons;

   //List of filtered mount icons by input search
   [NonSerialized] public List<string> mountIconsSearch;

   //List of all portraits available in-game
   public List<string> portraits;

   //List of filtered portraits by input search
   [NonSerialized] public List<string> portraitsSearch;

   //EXTERNAL FILE: Assets object that keeps all asset data
   public static Assets assets;
}
