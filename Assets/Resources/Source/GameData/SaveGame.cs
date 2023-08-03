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
                "Staff of Dominance",
                "Apprentice's Robe",
                "Apprentice's Pants",
                "Apprentice's Boots",
                "Band of Flesh",
                "Mantle of Phrenic Power",
                "Cloak of the Brood Lord",
                "Prestor's Talisman of Connivery",
                "Mish'undare, Circlet of the Mind Flayer",
                "Gossamer Belt",
                "Grizzle's Skinner",
                "Perdition's Blade"
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
