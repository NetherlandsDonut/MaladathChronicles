using System;
using System.Collections.Generic;

using static Root;
using static Race;

public class SiteInstance
{
    public SiteInstance(string name, List<InstanceWing> wings)
    {
        this.name = name;
        this.wings = wings;
    }

    public string name;
    public List<InstanceWing> wings;

    public static SiteInstance instance;

    public static List<SiteInstance> dungeons = new()
    {
        new SiteInstance("Stratholme", new()
        {
            new InstanceWing("Undead Side", new()
            {
                "King's Square",
                "Market Row",
                "Crusaders' Square",
                "The Scarlet Bastion",
                "The Crimson Throne"
            }),
            new InstanceWing("Living Side", new()
            {
                "Elder's Square",
                "The Gauntlet",
                "Slaughter Square",
                "The Slaughter House"
            })
        }),
        new SiteInstance("Dire Maul", new()
        {
            new InstanceWing("Eastern Wing", new()
            {
                "Warpwood Quarter",
                "The Conservatory",
                "The Shrine of Eldre'Tharr"
            }),
            new InstanceWing("Northern Wing", new()
            {
                "Gordok Commons",
                "Halls Of Destruction",
                "Gordok's Seat"
            }),
            new InstanceWing("Western Wing", new()
            {
                "Capital Gardens",
                "Court Of The Highborne",
                "Prison Of Immol'Thar",
                "The Athenaeum",
            }),
        }),
        new SiteInstance("Zul'Farrak", new()
        {
            new InstanceWing("Zul'Farrak", new()
            {
                "Ass",
            })
        }),
    };

    public static List<SiteInstance> raids = new()
    {
        new SiteInstance("Ruins of Ahn'Qiraj", new()
        {
            new InstanceWing("Ruins of Ahn'Qiraj", new()
            {
                "Scarab Terrace",
                "General's Terrace",
                "Reservoir",
                "Hatchery",
                "Comb",
                "Watcher's Terrace"
            })
        })
    };
}

public class InstanceWing
{
    public InstanceWing(string name, List<string> areas)
    {
        this.name = name;
        this.areas = areas;
    }

    public string name;
    public List<string> areas;
}
