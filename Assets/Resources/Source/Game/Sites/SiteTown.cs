using System;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Anchor;

using static Sound;
using static Faction;
using static Transport;
using static FlightPathGroup;

public class SiteTown : Site
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
        if (people != null && people.Exists(x => x.type == "Flight Master"))
        {
            var sites = flightPathGroups.FindAll(x => x.sitesConnected.Contains(name)).SelectMany(x => x.sitesConnected).Distinct().ToList().FindAll(x => x != name).Select(x => new Transport() { means = "Flight", destination = x }).ToList();
            flightPaths = sites.Count == 0 ? new() : sites.OrderBy(x => factions.Find(z => z.name == towns.Find(y => y.name == x.destination).faction).Icon()).ThenBy(x => x.destination).ToList();
        }
        if (faction != null)
            if (!factions.Exists(x => x.name == faction))
                factions.Insert(0, new Faction()
                {
                    name = faction,
                    icon = "Faction" + faction,
                    side = "Neutral"
                });
        if (!Blueprint.windowBlueprints.Exists(x => x.title == "Town: " + name))
            Blueprint.windowBlueprints.Add(
                new Blueprint("Town: " + name,
                    () =>
                    {
                        PlayAmbience(ambience);
                        SetAnchor(TopRight);
                        AddRegionGroup();
                        SetRegionGroupWidth(171);
                        SetRegionGroupHeight(338);
                        AddHeaderRegion(() =>
                        {
                            AddLine(name);
                            AddSmallButton("OtherClose",
                            (h) =>
                            {
                                var title = CDesktop.title;
                                CloseDesktop(title);
                                PlaySound("DesktopInstanceClose");
                                SwitchDesktop("Map");
                            });
                        });
                        if (transport != null)
                        {
                            AddHeaderRegion(() => { AddLine("Transportation:"); });
                            foreach (var transport in transport)
                            {
                                var desitnation = towns.Find(x => x.name == transport.destination);
                                AddButtonRegion(() =>
                                {
                                    AddLine(transport.destination, "Black");
                                    AddSmallButton("Transport" + transport.means, (h) => { });
                                },
                                (h) =>
                                {
                                    CloseDesktop("Town");
                                    SwitchDesktop("Map");
                                    CDesktop.LockScreen();
                                    if (transport.price > 0)
                                        PlaySound("DesktopTransportPay");
                                    desitnation.QueueSiteOpen("Town");
                                },
                                null,
                                (h) => () => { PrintTransportTooltip(transport); });
                            }
                        }
                        if (people != null)
                        {
                            AddHeaderRegion(() => { AddLine("Points of interest:"); });
                            foreach (var person in people)
                            {
                                var personType = PersonType.personTypes.Find(x => x.name == person.type);
                                if (personType.type == "Battlemaster")
                                    AddButtonRegion(() =>
                                    {
                                        AddLine(person.name, "Black");
                                        AddSmallButton(personType.icon + factions.Find(x => x.name == faction).side, (h) => { });
                                    },
                                    (h) =>
                                    {

                                    });
                                else if (personType.type != "Other")
                                    AddButtonRegion(() =>
                                    {
                                        AddLine(person.name, "Black");
                                        AddSmallButton(personType != null ? personType.icon : "OtherUnknown", (h) => { });
                                    },
                                    (h) =>
                                    {
                                        Person.person = person;
                                        if (personType.type == "Banker")
                                            SpawnDesktopBlueprint("Bank");
                                        if (personType.type == "Vendor")
                                            SpawnDesktopBlueprint("Vendor");
                                    });
                            }
                        }
                        AddPaddingRegion(() => SetRegionAsGroupExtender());
                    }
                )
            );
        if (x != 0 && y != 0)
            Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite()));
    }

    //List of all transport paths provided in this town
    public List<Transport> transport;

    //List of NPC's that are inside of this town
    public List<Person> people;

    //List of town flight paths, these are generated automatically
    [NonSerialized] public List<Transport> flightPaths;

    //Currently opened town
    public static SiteTown town;

    //EXTERNAL FILE: List containing all towns in-game
    public static List<SiteTown> towns;

    //List of all filtered towns by input search
    public static List<SiteTown> townsSearch;

    //Prints the site on the world map
    public override void PrintSite()
    {
        SetAnchor(x * 19, y * 19);
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton(factions.Find(x => x.name == faction).Icon(),
            (h) => { QueueSiteOpen("Town"); },
            null,
            (h) => () =>
            {
                SetAnchor(TopRight, h.window);
                AddRegionGroup();
                AddHeaderRegion(() =>
                {
                    AddLine(name);
                });
            });
        });
    }

    //Returns path to a texture that is the background visual of this site
    public override string Background() => "Areas/Area" + (zone + name).Clean();
}
