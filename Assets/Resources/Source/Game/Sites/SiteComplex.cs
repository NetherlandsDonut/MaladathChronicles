using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Sound;
using static Faction;
using static SaveGame;
using static SiteInstance;
using static GameSettings;
using static SiteHostileArea;

public class SiteComplex : Site
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
        type ??= "Complex";
        if (faction != null)
            if (!factions.Exists(x => x.name == faction))
                factions.Insert(0, new Faction()
                {
                    name = faction,
                    icon = "Faction" + faction,
                    side = "Neutral"
                });
        if (sites != null)
            foreach (var site in sites)
                if (site != null && site.ContainsKey("SiteType") && site.ContainsKey("SiteName"))
                    if (site["SiteType"] == "HostileArea")
                    {
                        if (!areas.Exists(x => x.name == site["SiteName"]))
                            areas.Insert(0, new SiteHostileArea()
                            {
                                name = site["SiteName"],
                                commonEncounters = new(),
                                rareEncounters = new(),
                                eliteEncounters = new(),
                                type = "HostileArea",
                                zone = name
                            });
                    }
                    else if (site["SiteType"] == "Dungeon")
                    {
                        if (!instances.Exists(x => x.name == site["SiteName"]))
                            instances.Insert(0, new SiteInstance()
                            {
                                name = site["SiteName"],
                                wings = new(),
                                type = "Dungeon"
                            });
                    }
                    else if (site["SiteType"] == "Raid")
                    {
                        if (!instances.Exists(x => x.name == site["SiteName"]))
                            instances.Insert(0, new SiteInstance()
                            {
                                name = site["SiteName"],
                                wings = new(),
                                type = "Raid"
                            });
                    }
        instances.FindAll(x => sites.Exists(y => (y["SiteType"] == "Raid" || y["SiteType"] == "Dungeon") && y["SiteName"] == x.name)).ForEach(x => x.complexPart = true);
        areas.FindAll(x => sites.Exists(y => y["SiteType"] == "HostileArea" && y["SiteName"] == x.name)).ForEach(x => x.complexPart = true);
        if (!Blueprint.windowBlueprints.Exists(x => x.title == "Complex: " + name))
            Blueprint.windowBlueprints.Add(
                new Blueprint("Complex: " + name,
                    () =>
                    {
                        PlayAmbience(ambience);
                        SetAnchor(TopRight, -19, -38);
                        AddRegionGroup();
                        SetRegionGroupWidth(171);
                        AddHeaderRegion(() =>
                        {
                            AddLine(name);
                            AddSmallButton("OtherClose",
                            (h) =>
                            {
                                var title = CDesktop.title;
                                PlaySound("DesktopInstanceClose");
                                CloseDesktop(title);
                                SwitchDesktop("Map");
                            });
                        });
                        AddPaddingRegion(() => { AddLine("Sites: "); });
                        foreach (var site in sites)
                            PrintComplexSite(site);
                    }
                )
            );
        SitePath.pathsConnectedToSite.Remove(name);
        if (x != 0 && y != 0)
            Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite()));
    }

    //Complex description showed in the left side of the screen
    public List<string> description;

    //List of all sites that this complex contains
    //Keys provide information what type of site it is
    //Values provide information what is the name of the site
    //EXAMPLE: { "SiteType": "Raid", "SiteName": "Molten Core" } 
    public List<Dictionary<string, string>> sites;

    //Currently opened complex
    public static SiteComplex complex;

    //EXTERNAL FILE: List containing all complexes in-game
    public static List<SiteComplex> complexes;

    //List of all filtered complexes by input search
    public static List<SiteComplex> complexesSearch;

    //Returns path to a texture that is the background visual of this site
    public override string Background()
    {
        var save = currentSave ?? saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter);
        return "Areas/Complex" + name.Clean() + (save != null && save.IsNight() && !noNightVariant ? "Night" : "");
    }

    //Function to print the site onto the map
    public override void PrintSite()
    {
        SetAnchor(x, y);
        DisableGeneralSprites();
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton(currentSave.siteVisits.ContainsKey(name) ? "MapComplex" : "MapUnknown",
            (h) => { CDesktop.cameraDestination = new Vector2(x, y); },
            (h) =>
            {
                if (h == null) LeadPath();
                else ExecutePath("Complex");
            },
            (h) => () =>
            {
                if (!currentSave.siteVisits.ContainsKey(name)) return;
                SetAnchor(TopLeft, 19, -38);
                AddRegionGroup();
                AddHeaderRegion(() => { AddLine(name, "Gray"); });
                complex = this;
                foreach (var site in complex.sites)
                    AddHeaderRegion(() =>
                    {
                        AddLine(site["SiteName"], "DarkGray");
                        AddSmallButton("Site" + site["SiteType"], (h) => { });
                    });
            },
            (h) => { BuildPath(); });
            if (currentSave.currentSite == name)
                    AddSmallButtonOverlay("PlayerLocation", 0, 2);
        });
    }

    public static void PrintComplexSite(Dictionary<string, string> site)
    {
        AddButtonRegion(() =>
        {
            AddLine(site["SiteName"], "", "Right");
            AddSmallButton("Site" + site["SiteType"], (h) => { });
        },
        (h) =>
        {
            if (site["SiteType"] == "HostileArea")
            {
                area = areas.Find(x => x.name == site["SiteName"]);
                var window = CDesktop.windows.Find(x => x.title.StartsWith("HostileArea: "));
                if (window != null)
                    if (window.title == "HostileArea: " + area.name) return;
                    else CloseWindow(window);
                SpawnWindowBlueprint("HostileArea: " + area.name);
                Respawn("BossQueue");
                SetDesktopBackground(area.Background());
            }
            else
            {
                CloseDesktop("Complex");
                instance = instances.Find(x => x.name == site["SiteName"]);
                SpawnDesktopBlueprint("Instance");
            }
        });
    }
}