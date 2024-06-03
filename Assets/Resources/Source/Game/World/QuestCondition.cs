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
        else if (type == "Visit") list = new() { Site.FindSite(x => x.name == name) };
        var com = list.FindAll(x => ((SiteHostileArea)x).complexPart);
        var ins = list.FindAll(x => ((SiteHostileArea)x).instancePart);
        var instances = ins.Select(x => SiteInstance.instances.Find(y => y.wings.Any(z => z.areas.Any(c => c["AreaName"] == x.name)))).ToList();
        var complexes = com.Concat(instances.Where(x => x.complexPart).Select(x => SiteComplex.complexes.Find(y => y.sites.Any(z => z["SiteName"] == x.name))));
        instances.RemoveAll(x => x.complexPart);
        list.RemoveAll(x => com.Contains(x) || ins.Contains(x));
        list = list.Concat(complexes).Concat(instances).ToList();
        return list.FindAll(x => x != null && currentSave.siteVisits.ContainsKey(x.name));
    }

    //Checks whether this condition is already fulfilled
    public void Print(bool markerButton = true)
    {
        var line = "";
        var where = markerButton ? Where() : new();
        if (type == "Item") line = name + ": " + amountDone + " / " + amount;
        else if (type == "Kill") line = name + ": " + amountDone + " / " + amount;
        else if (type == "Visit") line = name + " visited: " + (status == "Done" ? 1 : 0) + " / 1";
        AddPaddingRegion(() =>
        {
            AddLine(line);
            if (markerButton && where.Count > 0)
                AddSmallButton("ItemMiscMap01", (h) =>
                {
                    SwitchDesktop("Map");
                    CloseDesktop("QuestLog");
                    CDesktop.cameraDestination = new Vector2(where[0].x, where[0].y);
                });
        });
    }
}
