using System.Collections.Generic;
using UnityEngine;
using static Root;

public class SiteSpiritHealer : Site
{
   //Initialisation method to fill automatic values
   //and remove empty collections to avoid serialising them later
   public override void Initialise()
   {
      SitePath.pathsConnectedToSite.Remove(name);
      if (x != 0 && y != 0) Blueprint.windowBlueprints.Add(new Blueprint("Site: SpiritHealer: " + name, () => PrintSite()));
   }

   //Currently opened spirit healer
   public static SiteSpiritHealer spiritHealer;

   //EXTERNAL FILE: List containing all spirit healers in-game
   public static List<SiteSpiritHealer> spiritHealers;

   //List of all filtered spirit healers by input search
   public static List<SiteSpiritHealer> spiritHealersSearch;

   public override void PrintSite()
   {
      SetAnchor(x, y);
      DisableGeneralSprites();
      AddRegionGroup();
      AddPaddingRegion(() =>
      {
         AddSmallButton("MapSpiritHealer");
      });
   }

   //Returns path to a texture that is the background visual of this site
   public override string Background() => "Areas/Area" + (zone + name).Clean() + "SpiritHealer";
}
