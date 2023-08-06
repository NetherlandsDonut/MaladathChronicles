using System;
using System.Collections.Generic;

using static Root;
using static Race;
using System.Linq;

public class SiteInstance
{
    public SiteInstance(string name, List<string> description, List<InstanceWing> wings)
    {
        this.name = name;
        this.description = description;
        this.wings = wings;
    }

    public string name;
    public bool complexPart;
    public List<string> description;
    public List<InstanceWing> wings;

    public (int, int) LevelRange()
    {
        var areas = wings.SelectMany(x => SiteHostileArea.hostileAreas.FindAll(y => x.areas.Exists(z => z.Item2 == y.name)));
        if (areas.Count() == 0) return (0, 0);
        return (areas.Min(x => x.recommendedLevel), areas.Max(x => x.recommendedLevel));
    }

    public static SiteInstance instance;

    public static List<SiteInstance> dungeons = new()
    {
        new SiteInstance("Stratholme", new()
        {

        },
        new()
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

        },
        new()
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
        new SiteInstance("Blackrock Depths", new()
        {
            "The smoldering Blackrock",
            "Depths are home to the",
            "Dark Iron dwarves and",
            "their emperor, Dagran",
            "Thaurissan. Like his",
            "predecessors, he serves",
            "under the iron rule of",
            "Ragnaros the Firelord,",
            "a merciless being",
            "summoned into the world",
            "centuries ago.",
            "The presence of chaotic",
            "elementals has attracted",
            "Twilight's Hammer cultists",
            "to the mountain domain.",
            "Along with Ragnaros'",
            "servants, they have",
            "pushed the dwarves ",
            "toward increasingly",
            "destructive ends that",
            "could soon spell doom",
            "for all of Azeroth."
        },
        new ()
        {
            new InstanceWing("Outer Depths", new()
            {
                ("None", "Detention Block"),
                ("Boss", "Halls of the Law"),
                ("Boss", "Ring of Law"),
                //("Boss", "The Black Vault"),
                //("Boss", "The Domicle"),
                ("Boss", "Dark Iron Highway"),
                ("Boss", "The Black Anvil"),
                ("Boss", "Hall of Crafting"),
            }),
            new InstanceWing("Inner City", new()
            {
                //("Boss", "Shrine of Thaurissan"),
                //("Boss", "The Grim Guzzler"),
                ("Boss", "West Garrison"),
                ("Boss", "The Manufactory"),
                ("Boss", "Chamber of Enchantment"),
                ("Boss", "Mold Foundry"),
                ("Boss", "Summoners' Tomb"),
                //("Boss", "The Lyceum"),
                ("Boss", "The Iron Hall"),
                ("Boss", "The Imperial Seat")
            }),
        }),
        new SiteInstance("Lower Blackrock Spire", new()
        {
            "The majority of Rend's",
            "Dark Horde reside in",
            "Lower Blackrock Spire,",
            "making the city a sort of",
            "sprawling barracks.",
            "Only the elite members of",
            "the Dark Horde are allowed",
            "to live in the upper",
            "reaches of the city.",
            "Overlord Wyrmthalak acts as",
            "the ruler and taskmaster",
            "of these soldiers."
        },
        new ()
        {
            new InstanceWing("Lower", new()
            {
                ("None", "Hordemar City"),
                ("Boss", "Mok'Doom"),
                ("Boss", "Tazz'Alor"),
                ("Boss", "Skitterweb Tunnels"),
                ("Boss", "The Storehouse"),
                ("Boss", "Halycon's Lair"),
                ("Boss", "Chamber of Battle"),
            }),
        }),
        new SiteInstance("Upper Blackrock Spire", new()
        {
            "The smoldering Blackrock",
            "The history of this",
            "fortress is long and",
            "complex. Carved into the",
            "fiery core of Blackrock",
            "Mountain by the Dark Iron",
            "clan centuries ago, and",
            "eventually taken by",
            "the black dragon Nefarian",
            "and his brood"
        },
        new ()
        {
            new InstanceWing("Upper", new()
            {
                ("Boss", "Hall of Binding"),
                ("Boss", "The Rookery"),
                ("Boss", "Hall of Blackhand"),
                ("Boss", "Blackrock Stadium"),
                ("Boss", "The Furnace"),
                ("Boss", "Spire Throne")
            }),
        }),
        new SiteInstance("Zul'Farrak", new()
        {

        },
        new()
        {
            new InstanceWing("Zul'Farrak", new()
            {
                ("None", "Ass"),
            })
        }),
    };

    public static List<SiteInstance> raids = new()
    {
        new SiteInstance("Molten Core", new()
        {
            "The Molten Core lies at",
            "the very bottom of",
            "Blackrock Depths.",
            "It is the heart of",
            "Blackrock Mountain and",
            "the exact spot where,",
            "long ago in a desperate",
            "bid to turn the tide of",
            "the dwarven civil war,",
            "Emperor Thaurissan",
            "summoned the elemental",
            "Firelord, Ragnaros, into",
            "the world. The burning",
            "lake where Ragnaros lies",
            "sleeping acts as a rift",
            "connecting to the plane",
            "of fire, allowing the",
            "malicious elementals to",
            "pass through."
        },
        new ()
        {
            new InstanceWing("Lower", new()
            {
                ("None", "Magmadar Cavern"),
                ("Boss", "The Lavafalls"),
                ("Boss", "Ragnaros' Lair"),
            }),
        }),
        new SiteInstance("Blackwing Lair", new()
        {
            "In the dark recesses of",
            "the mountain's peak,",
            "Nefarian, the eldest son",
            "of Deathwing, conducts",
            "some of his most awful",
            "experimentation,",
            "controlling mighty beings",
            "like puppets and combining",
            "the eggs of different",
            "dragonflights with",
            "horrific results.",
            "Should he prove",
            "successful, even darker",
            "pursuits rest",
            "on the horizon."
        },
        new ()
        {
            new InstanceWing("Lower", new()
            {
                ("None", "Dragonmaw Garrison"),
                ("Boss", "Shadow Wing Lair"),
                ("Boss", "Halls of Strife"),
                ("Boss", "Crimson Laboratories"),
                ("Boss", "Chromaggus' Lair"),
                ("Boss", "Nefarian's Lair"),
            }),
        }),
        new SiteInstance("Ruins Of Ahn'Qiraj", new()
        {

        },
        new()
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
        SiteHostileArea.hostileAreas.FindAll(x => areas.Exists(y => y.Item2 == x.name)).ForEach(x => x.instancePart = true);
    }

    public string name;
    public List<(string, string)> areas;
}
