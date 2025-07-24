using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using static Root;
using static SaveGame;

public class QuestCondition
{
    //Type of the condition this is
    //[Kill, Item, Visit]
    public string type;

    //Name of the thing that needs to be done
    public string name;

    //Amount of progress required for completion
    public int amount;

    //Amount that was already done when the quest was accepted
    public int amountDone;

    //Checks whether player already done the quest
    public string status;

    //Does the quest refrain from taking away this item
    public bool isItemNotTaken;

    //Checks whether this condition is already fulfilled
    public bool IsDone()
    {
        if (type == "Item")
            return currentSave.player.inventory.items.Sum(x => x.name == name ? x.amount : 0) >= amount;
        return status == "Done";
    }

    //Tell the player where this condition can be fulfilled
    public List<Site> Where()
    {
        var list = new List<Site>();
        if (type == "Kill") list = SiteHostileArea.areas.FindAll(x => x.commonEncounters != null && x.commonEncounters.Exists(x => x.who == name) || x.rareEncounters != null && x.rareEncounters.Exists(x => x.who == name) || x.eliteEncounters != null && x.eliteEncounters.Exists(x => x.who == name)).Select(x => (Site)x).ToList();
        else if (type == "Item")
        {
            var races = Item.items.Find(x => x.name == name).droppedBy;
            if (races != null) list = SiteHostileArea.areas.FindAll(x => x.commonEncounters != null && x.commonEncounters.Any(x => races.Contains(x.who)) || x.rareEncounters != null && x.rareEncounters.Any(x => races.Contains(x.who)) || x.eliteEncounters != null && x.eliteEncounters.Any(x => races.Contains(x.who))).Select(x => (Site)x).ToList();
        }
        else if (type == "Visit")
        {
            var find = Site.FindSite(x => x.name == name);
            if (find != null) list = new() { find };
        }
        if (list.Count > 0)
        {
            var com = list.FindAll(x => x.complexPart);
            var ins = list.FindAll(x => x.instancePart);
            var instances = ins.Select(x => SiteInstance.instances.Find(y => y.wings.Any(z => z.areas.Any(c => c["AreaName"] == x.name)))).ToList();
            var complexes = com.Concat(instances.Where(x => x.complexPart).Select(x => SiteComplex.complexes.Find(y => y.sites.Any(z => z["SiteName"] == x.name))));
            instances.RemoveAll(x => x.complexPart);
            list.RemoveAll(x => com.Contains(x) || ins.Contains(x));
            list = list.Concat(complexes).Concat(instances).ToList();
            return list.FindAll(x => x != null);
        }
        else return list;
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
        AddPaddingRegion(() =>
        {
            AddLine(line, "DarkGray");
            AddText(then, "Gray");
            if (markerButton && where.Count > 0)
                AddSmallButton("ItemMiscMap01", (h) =>
                {
                    CloseDesktop("HostileArea");
                    CloseDesktop("Instance");
                    CloseDesktop("Complex");
                    SwitchDesktop("Map");
                    CDesktop.cameraDestination = new Vector2(where[0].x, where[0].y);
                });
        });
    }
}
