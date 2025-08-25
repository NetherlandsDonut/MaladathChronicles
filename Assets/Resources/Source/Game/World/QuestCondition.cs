using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using static Root;
using static SaveGame;
using static SiteArea;

public class QuestCondition
{
    //Type of the condition this is
    //[Kill, Item, Visit, Explore, Flag]
    public string type;

    //Name of the thing that needs to be done
    public string name;

    //Required value on the flag
    public string value;

    //Amount of progress required for completion
    public int amount;

    //Amount that was already done when the quest was accepted
    public int amountDone;

    //Checks whether player already done the quest
    public string status;

    //Does the quest refrain from taking away this item
    public bool isItemNotTaken;

    //Custom list of the sites this condition can be done at
    //For now this is mostly utilised by conditions which can be only done
    //by using an item from the inventory at a specific location
    public List<string> sites;

    //Checks whether this condition is already fulfilled
    public bool IsDone()
    {
        if (type == "Item") return currentSave.player.inventory.items.Sum(x => x.name == name ? x.amount : 0) >= amount;
        return status == "Done";
    }

    //Tell the player where this condition can be fulfilled
    public List<Site> Where()
    {
        var list = new List<Site>();
        if (sites != null) list = sites.Select(x => Site.FindSite(y => y.name == x)).Distinct().ToList();
        else if (type == "Kill") list = areas.FindAll(x => x.commonEncounters != null && x.commonEncounters.Exists(x => x.who == name) || x.rareEncounters != null && x.rareEncounters.Exists(x => x.who == name) || x.eliteEncounters != null && x.eliteEncounters.Exists(x => x.who == name)).Select(x => (Site)x).ToList();
        else if (type == "Item")
        {
            var races = Item.items.Find(x => x.name == name).droppedBy;
            if (races != null) list = areas.FindAll(x => x.commonEncounters != null && x.commonEncounters.Any(x => races.Contains(x.who)) || x.rareEncounters != null && x.rareEncounters.Any(x => races.Contains(x.who)) || x.eliteEncounters != null && x.eliteEncounters.Any(x => races.Contains(x.who))).Select(x => (Site)x).ToList();
        }
        else if (type == "Visit" || type == "Explore")
        {
            var find = Site.FindSite(x => x.name == name);
            if (find != null) list = new() { find };
        }
        if (list.Count > 0)
        {
            var convert = list.FindAll(x => x.convertDestinationTo != null);
            foreach (var conv in convert)
                list.Add(Site.FindSite(x => x.name == conv.convertDestinationTo && x.x != 0 && x.y != 0));
            list.RemoveAll(x => x.convertDestinationTo != null);
            return list.FindAll(x => x != null);
        }
        else return list.Distinct().ToList();
    }

    //Checks whether this condition is already fulfilled
    public void Print(bool markerButton = true)
    {
        var line = "";
        var then = "";
        var where = markerButton ? Where() : new();
        if (type == "Item")
        {
            var sum = currentSave.player.inventory.items.Sum(x => x.name == name ? x.amount : 0);
            (line, then) = (name + ": ", (sum > amount ? amount : sum) + "/" + amount);
        }
        else if (type == "Kill") (line, then) = (name + ": ", amountDone + "/" + amount);
        else if (type == "Visit") (line, then) = (name + " visited: ", (status == "Done" ? 1 : 0) + "/1");
        else if (type == "Explore") (line, then) = (name + " explored: ", (status == "Done" ? 1 : 0) + "/1");
        else if (type == "Flag") (line, then) = (name.Replace("_", " ") + ": ", (status == "Done" ? 1 : 0) + "/1");
        AddPaddingRegion(() =>
        {
            AddLine(line, "DarkGray");
            AddText(then, "Gray");
            if (markerButton && where.Count > 0)
                AddSmallButton("ItemMiscMap01", (h) =>
                {
                    CloseDesktop("Area");
                    CloseDesktop("Instance");
                    CloseDesktop("Complex");
                    SwitchDesktop("Map");
                    var index = questMarkerOrder++ % where.Count;
                    CDesktop.cameraDestination = new Vector2(where[index].x, where[index].y);
                });
        });
    }
}
