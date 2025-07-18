using System.Collections.Generic;

using static Root;
using static Sound;
using static SiteTown;

public class SiteCapital : Site
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
        type ??= "Capital";
        if (districts != null)
            foreach (var town in districts)
                if (!towns.Exists(x => x.name == town))
                    towns.Insert(0, new SiteTown()
                    {
                        name = town,
                        zone = this.zone,
                        faction = this.faction,
                        capitalRedirect = this.name
                    });
        SitePath.pathsConnectedToSite.Remove(name);
    }

    //List of towns
    public List<string> districts;

    //Instance inside of this capital
    //There are two examples of this in the game
    public string instance;

    //Currently opened capital
    public static SiteCapital capital;

    //Currently opened town from which we get to the capital
    public static SiteTown capitalThroughTown;

    //EXTERNAL FILE: List containing all complexes in-game
    public static List<SiteCapital> capitals;

    //List of all filtered complexes by input search
    public static List<SiteCapital> capitalsSearch;

    //Prints a single district out of all accessible
    public static void PrintCapitalTown(string town)
    {
        var find = towns.Find(x => x.name == town);
        AddButtonRegion(() =>
        {
            AddLine(town);
        },
        (h) =>
        {
            if (SiteTown.town == find) return;
            SiteTown.town = find;
            PlaySound("DesktopInstanceOpen");
            CloseDesktop("Town");
            SpawnDesktopBlueprint("Town");
            SpawnTransition();
        });
    }

    //Prints a single district out of all accessible
    public static void PrintCapitalInstance(string instance)
    {
        var find = SiteInstance.instances.Find(x => x.name == instance);
        AddButtonRegion(() =>
        {
            AddLine(find.name);
            AddSmallButton("Site" + find.type);
        },
        (h) =>
        {
            SiteInstance.instance = find;
            PlaySound("DesktopInstanceOpen");
            CloseDesktop("Capital");
            CloseDesktop("Town");
            SpawnDesktopBlueprint("Instance");
            SpawnTransition();
        });
    }
}