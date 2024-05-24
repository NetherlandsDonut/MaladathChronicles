using System.Collections.Generic;

using static Root;
using static Defines;

using static SaveGame;

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

    public static void PrintMountTooltip(Entity rider, Mount mount)
    {
        SetAnchor(-92, CDesktop.windows.Exists(x => x.title == "CurrentMount") ? 47 : 142);
        AddHeaderGroup();
        SetRegionGroupWidth(190);
        AddHeaderRegion(() =>
        {
            AddLine(mount.name, "Gray");
        });
        AddPaddingRegion(() =>
        {
            AddBigButton(mount.icon, (h) => { });
            if (currentSave.player.level < (mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts)) SetBigButtonToRed();
            AddLine("Required level: ", "DarkGray");
            AddText((mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts) + "", Coloring.ColorRequiredLevel(mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts));
            AddLine("Speed: ", "DarkGray");
            AddText(mount.speed == 7 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"));
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
