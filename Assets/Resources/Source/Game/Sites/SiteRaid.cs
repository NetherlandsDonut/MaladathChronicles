using System;
using System.Collections.Generic;

using static Root;
using static Race;

public class SiteRaid
{
    public SiteRaid(string name, List<string> stages)
    {
        this.name = name;
        this.stages = stages;
    }

    public string name;
    public List<string> stages;

    public List<SiteRaid> raids = new()
    {
        new SiteRaid("Ruins of Ahn'Qiraj", new()
        {
            "Scarab Terrace",
            "General's Terrace",
            "Reservoir",
            "Hatchery",
            "Comb",
            "Watcher's Terrace"
        })
    };
}

//public class RaidStage
//{
//    public RaidStage(int length, string area)
//    {
//        this.length = length;
//        this.area = SiteHostileArea.hostileAreas.Find(x => x.name == area);
//    }

//    public int length;
//    public SiteHostileArea area;

//    public Race RollEncounter() => races.Find(x => x.name == area.possibleEncounters[random.Next(0, area.possibleEncounters.Count)].Item1);
//}
