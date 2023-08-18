using System.Collections.Generic;

public class SiteComplex
{
    public SiteComplex(string name, List<string> description, List<(string, string)> sites)
    {
        this.name = name;
        this.description = description;
        this.sites = sites;
        SiteInstance.instances.FindAll(x => sites.Exists(y => (y.Item1 == "Raid" || y.Item1 == "Dungeon") && y.Item2 == x.name)).ForEach(x => x.complexPart = true);
        SiteHostileArea.hostileAreas.FindAll(x => sites.Exists(y => y.Item1 == "HostileArea" && y.Item2 == x.name)).ForEach(x => x.complexPart = true);
    }

    public string name;
    public List<string> description;
    public List<(string, string)> sites;

    public static SiteComplex complex;

    public static List<SiteComplex> complexes = new()
    {
        new SiteComplex("Blackrock Mountain", new()
        {
            ""
        },
        new()
        {
            ("Dungeon", "Blackrock Depths"),
            ("Raid", "Molten Core"),
            ("Dungeon", "Lower Blackrock Spire"),
            ("Dungeon", "Upper Blackrock Spire"),
            ("Raid", "Blackwing Lair")
        }),
        new SiteComplex("Ruins of Alterac", new()
        {
            ""
        },
        new()
        {
            ("HostileArea", "Town Center"),
            ("HostileArea", "Alterac Chapel"),
            ("HostileArea", "Alterac Keep"),
        }),
        new SiteComplex("Purgation Isle", new()
        {
            ""
        },
        new()
        {
            ("HostileArea", "Isle Landing"),
            ("HostileArea", "Mountain Peak"),
        }),
        new SiteComplex("Ruins of Stromgarde", new()
        {
            ""
        },
        new()
        {
            ("HostileArea", "North Town"),
            ("HostileArea", "The Sanctum"),
            ("HostileArea", "Western Town"),
            ("HostileArea", "Tower of Arathor"),
            ("HostileArea", "Stromgarde Keep"),
        }),
        new SiteComplex("Ruins of Alterac", new()
        {
            ""
        },
        new()
        {
            ("HostileArea", "Town Center"),
            ("HostileArea", "Alterac Chapel"),
            ("HostileArea", "Alterac Keep"),
        }),
        new SiteComplex("Scarlet Monastery Grounds", new()
        {
            ""
        },
        new()
        {
            ("HostileArea", "Whispering Gardens"),
            ("HostileArea", "Terrace of Repose"),
            ("HostileArea", "The Grand Vestibule"),
            ("Dungeon", "Scarlet Monastery"),
        }),
        new SiteComplex("Jaedenar", new()
        {
            ""
        },
        new()
        {
            ("HostileArea", "Jaedenar"),
            ("HostileArea", "Shrine of the Deceiver"),
            ("Dungeon", "Shadow Hold"),
        }),
        new SiteComplex("Alcaz Island", new()
        {
            ""
        },
        new()
        {
            ("HostileArea", "Alcaz Island"),
            ("HostileArea", "Alcaz Dungeon"),
        })
    };
}