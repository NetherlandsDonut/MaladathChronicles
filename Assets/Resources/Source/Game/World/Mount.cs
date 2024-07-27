using System.Collections.Generic;

using static Root;
using static Defines;

public class Mount
{
   //Initialisation method to fill automatic values
   //and remove empty collections to avoid serialising them later
   public void Initialise()
   {
      if (abilities != null)
         foreach (var ability in abilities)
            if (!Ability.abilities.Exists(x => x.name == ability))
               Ability.abilities.Insert(0, new Ability()
               {
                  name = ability,
                  icon = "Ability" + ability,
                  events = new(),
                  tags = new()
               });
   }

   #region Description

   //Tooltip window when hovered over a mount button
   public static void PrintMountTooltip(Entity rider, Mount mount)
   {
      SetAnchor(-92, CDesktop.windows.Exists(x => x.title == "CurrentMount") ? 47 : 142);
      AddHeaderGroup();
      SetRegionGroupWidth(182);
      AddHeaderRegion(() =>
      {
         AddLine(mount.name, mount.speed == 7 ? "Rare" : "Epic");
      });
      AddPaddingRegion(() =>
      {
         AddBigButton(mount.icon);
         if (rider.level < (mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts)) { SetBigButtonToRed(); AddBigButtonOverlay("OtherGridBlurred"); }
         AddLine("Required level: ", "DarkGray");
         AddText((mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts) + "", Coloring.ColorRequiredLevel(mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts));
         AddLine("Speed: ", "DarkGray");
         AddText(mount.speed == 7 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"), "HalfGray");
      });
      PrintPriceRegion(mount.price);
   }

   #endregion

   //Name of the mount
   public string name;

   //Icon of the mount
   public string icon;

   //Speed of the mount, while mount is equipped,
   //this speed replaces the player speed
   public int speed;

   //Price of the mount
   public int price;

   //List of abilities provided by the mount
   public List<string> abilities;

   //List of factions that offer these mounts
   public List<string> factions;

   //Currently opened mount
   public static Mount mount;

   //EXTERNAL FILE: List containing all mounts in-game
   public static List<Mount> mounts;

   //List of all filtered mounts by input search
   public static List<Mount> mountsSearch;
}
