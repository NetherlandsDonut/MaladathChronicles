using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.Anchor;

using static Sound;
using static Faction;
using static SitePath;
using static SaveGame;

public class SiteTown : Site
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public override void Initialise()
    {
        people?.ForEach(x => x.Initialise());
        if (faction != null)
            if (!factions.Exists(x => x.name == faction))
                factions.Insert(0, new Faction()
                {
                    name = faction,
                    icon = "Faction" + faction,
                    side = "Neutral"
                });
        flightPaths = new();
        foreach (var foo in FlightPathGroup.flightPathGroups.FindAll(x => x.sitesConnected.Contains(name)))
        {
            if (!flightPaths.ContainsKey(foo.side))
                flightPaths.Add(foo.side, new());
            flightPaths[foo.side].AddRange(foo.sitesConnected.Select(x => towns.Find(y => y.name == x)));
        }
        foreach (var foo in flightPaths)
            foo.Value?.Remove(this);
        if (!Blueprint.windowBlueprints.Exists(x => x.title == "Town: " + name))
            Blueprint.windowBlueprints.Add(
                new Blueprint("Town: " + name,
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
                                CloseDesktop(title);
                                PlaySound("DesktopInstanceClose");
                                SwitchDesktop("Map");
                            });
                        });
                        if (CDesktop.windows.Exists(x => x.title == "Persons")) return;
                        if (transportationConnectedToSite.ContainsKey(name))
                        {
                            var transportOptions = transportationConnectedToSite[name];
                            AddPaddingRegion(() => { AddLine("Transportation:"); });
                            foreach (var transport in transportOptions)
                            {
                                var desitnationName = transport.sites.Find(x => x != name);
                                var destination = towns.Find(x => x.name == desitnationName);
                                if (destination == null) continue;
                                AddButtonRegion(() =>
                                {
                                    AddLine(desitnationName, "Black");
                                    AddSmallButton("Transport" + transport.means, (h) => { });
                                },
                                (h) =>
                                {
                                    if (transport.price > 0)
                                    {
                                        if (transport.price > currentSave.player.inventory.money) return;
                                        PlaySound("DesktopTransportPay");
                                        currentSave.player.inventory.money -= transport.price;
                                    }

                                    //Close town screen as we're beginning to travel on map
                                    CloseDesktop("Town");

                                    //Switch desktop to map
                                    SwitchDesktop("Map");

                                    //Lead path to the destination
                                    LeadPath(transport, true);

                                    //Queue moving player to the destination
                                    destination.ExecutePath("Town");
                                },
                                null,
                                (h) => () => { transport.PrintTooltip(); });
                            }
                        }
                        if (people != null)
                        {
                            var groups = people.GroupBy(x => x.category).OrderBy(x => x.Count());
                            AddPaddingRegion(() => { AddLine("Points of interest:", "Gray"); });
                            foreach (var group in groups)
                                if (group.Key == null) continue;
                                else if (group.Key.category == "Flight Master")
                                    foreach (var person in group)
                                    {
                                        var faction = factions.Find(x => x.name == person.faction);
                                        faction ??= factions.Find(x => x.name == Race.races.Find(y => y.name == person.race).faction);
                                        faction ??= factions.Find(x => x.name == currentSave.player.faction);
                                        if (faction.side == currentSave.player.Side())
                                        {
                                            var personType = PersonType.personTypes.Find(x => x.type == person.type);
                                            AddButtonRegion(() =>
                                            {
                                                AddLine(person.name, "Black");
                                                AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? faction.side : "") : "OtherUnknown", (h) => { });
                                            },
                                            (h) =>
                                            {
                                                Person.person = person;
                                                CloseWindow(h.window.title);
                                                Respawn("Person");
                                                PlaySound("DesktopInstanceOpen");
                                            });
                                        }
                                    }
                                else if (group.Count() == 1)
                                    foreach (var person in group)
                                    {
                                        var personType = PersonType.personTypes.Find(x => x.type == person.type);
                                        AddButtonRegion(() =>
                                        {
                                            AddLine(person.name, "Black");
                                            AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? factions.Find(x => x.name == faction).side : "") : "OtherUnknown", (h) => { });
                                        },
                                        (h) =>
                                        {
                                            Person.person = person;
                                            CloseWindow(h.window.title);
                                            Respawn("Person");
                                            PlaySound("DesktopInstanceOpen");
                                        });
                                    }
                                else
                                {
                                    var person = group.First();
                                    AddButtonRegion(() =>
                                    {
                                        AddLine(group.Key.category + "s (" + group.Count() + ")", "Black");
                                        AddSmallButton(person.category != null ? person.category.icon + (person.category.factionVariant ? factions.Find(x => x.name == faction).side : "") : "OtherUnknown", (h) => { });
                                    },
                                    (h) =>
                                    {
                                        PersonCategory.personCategory = group.Key;
                                        CloseWindow("Person");
                                        Respawn("Persons");
                                        Respawn("Town: " + town.name);
                                        PlaySound("DesktopInstanceOpen");
                                    });
                                }
                        }
                    }
                )
            );
        pathsConnectedToSite.Remove(name);
        transportationConnectedToSite.Remove(name);
        if (x != 0 && y != 0)
            Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite()));
    }

    //List of NPC's that are inside of this town
    public List<Person> people;

    //List of town flight paths, these are generated automatically
    [NonSerialized] public Dictionary<string, List<SiteTown>> flightPaths;

    //Currently opened town
    public static SiteTown town;

    //EXTERNAL FILE: List containing all towns in-game
    public static List<SiteTown> towns;

    //List of all filtered towns by input search
    public static List<SiteTown> townsSearch;

    //Prints the site on the world map
    public override void PrintSite()
    {
        SetAnchor(x, y);
        DisableGeneralSprites();
        AddRegionGroup();
        AddPaddingRegion(() =>
        {
            AddSmallButton("Map" + (currentSave.siteVisits.ContainsKey(name) ? factions.Find(x => x.name == faction).Icon() : "Unknown"),
            (h) => { CDesktop.cameraDestination = new Vector2(x, y); },
            (h) =>
            {
                if (h == null) LeadPath();
                else ExecutePath("Town");
            },
            (h) => () =>
            {
                if (!currentSave.siteVisits.ContainsKey(name)) return;
                SetAnchor(TopLeft, 19, -38);
                AddRegionGroup();
                var side = factions.Find(x => x.name == faction).side;
                AddHeaderRegion(() =>
                {
                    AddLine(name, side == currentSave.player.Side() ? "Friendly" : (side == "Neutral" ? "Contested" : "Hostile"));
                });
                if (people != null)
                {
                    var types = people.Select(x => PersonType.personTypes.Find(y => y.type == x.type)).ToList();
                    types.Remove(null);
                    var icons = types.Distinct().Select(x => x.icon + (x.factionVariant ? factions.Find(x => x.name == faction).side : "")).ToList();
                    var amount = icons.Count % 9 == 0 ? 9 : (icons.Count % 8 == 0 ? 8 : 7);
                    if (icons.Count > 0)
                        for (int i = 0; i < Math.Ceiling(icons.Count / (double)amount); i++)
                        {
                            var ind = i;
                            AddPaddingRegion(() =>
                            {
                                for (int j = 0; j < amount && j < icons.Count - ind * amount; j++)
                                {
                                    var jnd = j;
                                    AddSmallButton(icons[jnd + ind * amount], (h) => { });
                                }
                            });
                        }
                }
            },
            (h) => { BuildPath(); });
            if (currentSave.currentSite == name)
                AddSmallButtonOverlay("PlayerLocation", 0, 2);
        });
    }

    //Returns path to a texture that is the background visual of this site
    public override string Background()
    {
        var save = currentSave ?? saves[GameSettings.settings.selectedRealm].Find(x => x.player.name == GameSettings.settings.selectedCharacter);
        return "Areas/Area" + (zone + name).Clean() + (save.IsNight() && !noNightVariant ? "Night" : "");
    }
}
