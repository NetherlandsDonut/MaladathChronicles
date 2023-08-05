using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame
{
    public SaveGame()
    {
        player = new Entity
        (
            "Ji'nta",
            Race.races.Find(x => x.name == "Forsaken"),
            Class.classes.Find(x => x.name == "Mage"),
            new()
            {
                "Apprentice's Robe",
                "Apprentice's Pants",
                "Apprentice's Boots",
                "Band of Flesh",
                "Grasp of the Old God",
                "Mantle of Phrenic Power",
                "Cloak of the Brood Lord",
                "Prestor's Talisman of Connivery",
                "Mish'undare, Circlet of the Mind Flayer",
                "Grizzle's Skinner",
                "Perdition's Blade",
                "Vanquished Tentacle of C'Thun",
                "Staff of Dominance",
                "Bent Staff",
                "Ring of the Godslayer",
                "Gossamer Belt",
            }
        );
        siteProgress = new Dictionary<string, int>();
        date = DateTime.Now;
    }

    public Entity player;
    public Dictionary<string, int> siteProgress;
    public List<string> rareKilled;
    public DateTime date;
    public bool hardcore;
}
