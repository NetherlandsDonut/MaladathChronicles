using System;
using System.Collections.Generic;

using static Root;
using static Race;
using System.Linq;

public class SiteInstance
{
    public SiteInstance(string name, List<InstanceWing> wings)
    {
        this.name = name;
        this.wings = wings;
    }

    public string name;
    public List<InstanceWing> wings;

    public (int, int) LevelRange()
    {
        var areas = wings.SelectMany(x => SiteHostileArea.hostileAreas.FindAll(y => x.areas.Exists(z => z.Item2 == y.name)));
        return (areas.Min(x => x.recommendedLevel), areas.Max(x => x.recommendedLevel));
    }

    public static SiteInstance instance;

    public static List<SiteInstance> dungeons = new()
    {
        new SiteInstance("Stratholme", new()
        {
            new InstanceWing("Main Gate", new()
            {
                ("None", "King's Square"),
                ("Boss", "Market Row"),
                ("Boss", "Crusaders' Square"),
                ("Boss", "The Scarlet Bastion"),
                ("Boss", "The Crimson Throne")
            }),
            new InstanceWing("Service Gate", new()
            {
                ("None", "Elder's Square"),
                ("Boss", "The Gauntlet"),
                ("Boss", "Slaughter Square"),
                ("Boss", "The Slaughter House")
            })
        }),
        new SiteInstance("Dire Maul", new()
        {
            new InstanceWing("Eastern Wing", new()
            {
                ("None", "Warpwood Quarter"),
                ("Boss", "The Conservatory"),
                ("Boss", "The Shrine of Eldre'Tharr")
            }),
            new InstanceWing("Northern Wing", new()
            {
                ("None", "Gordok Commons"),
                ("Boss", "Halls Of Destruction"),
                ("Boss", "Gordok's Seat")
            }),
            new InstanceWing("Western Wing", new()
            {
                ("None", "Capital Gardens"),
                ("Boss", "Court Of The Highborne"),
                ("Boss", "Prison Of Immol'Thar"),
                ("Boss", "The Athenaeum")
            }),
        }),
        new SiteInstance("Zul'Farrak", new()
        {
            new InstanceWing("Zul'Farrak", new()
            {
                ("None", "Ass"),
            })
        }),
    };

    public static List<SiteInstance> raids = new()
    {
        new SiteInstance("Ruins Of Ahn'Qiraj", new()
        {
            new InstanceWing("Ruins Of Ahn'Qiraj", new()
            {
                ("None", "Scarab Terrace"),
                ("Boss", "General's Terrace"),
                ("Boss", "Reservoir"),
                ("Boss", "Hatchery"),
                ("Boss", "Comb"),
                ("Boss", "Watcher's Terrace")
            })
        })
    };
}

public class InstanceWing
{
    public InstanceWing(string name, List<(string, string)> areas)
    {
        this.name = name;
        this.areas = areas;
    }

    public string name;
    public List<(string, string)> areas;
}
