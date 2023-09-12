using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Sound;
using static Faction;
using static Transport;

public class SiteTown : Site
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
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
                        SetRegionGroupHeight(354);
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
                                AddButtonRegion(() =>
                                {
                                    AddLine(transport.destination, "Black");
                                    AddSmallButton("Transport" + transport.means, (h) => { });
                                },
                                (h) =>
                                {
                                    CloseDesktop("TownEntrance");
                                    SwitchDesktop("Map");
                                    CDesktop.LockScreen();
                                    if (transport.price > 0)
                                        PlaySound("DesktopTransportPay");
                                    CDesktop.cameraDestination = new Vector2(x - 17, y + 9);
                                },
                                (h) => () => { PrintTransportTooltip(transport); });
                            }
                        }
                        if (people != null)
                        {
                            AddHeaderRegion(() => { AddLine("Points of interest:"); });
                            foreach (var person in people)
                            {
                                AddButtonRegion(() =>
                                {
                                    AddLine(person.name, "Black");
                                    var personType = PersonType.personTypes.Find(x => x.name == person.type);
                                    AddSmallButton(personType != null ? personType.icon : "OtherUnknown", (h) => { });
                                },
                                (h) =>
                                {
                                    if (person.type == "Banker")
                                        SpawnDesktopBlueprint("BankScreen");
                                });
                            }
                        }
                        AddPaddingRegion(() => { });
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
}
