using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame
{
    public SaveGame()
    {
        player = new Entity(
            "Ji'nta",
            Race.races.Find(x => x.name == "Troll"),
            Class.classes.Find(x => x.name == "Mage"),
            new()
            {
                "Fang of Venoxis",
                "Enthralled Sphere",
                "Band of Flesh",
                "Haunting Specter Leggings",
                "Mantle of Phrenic Power",
                "Mish'undare, Circlet of the Mind Flayer",
                "Cloak of the Brood Lord",
                "Prestor's Talisman of Connivery",
            });
        siteProgress = new Dictionary<string, int>();
        date = DateTime.Now;
    }

    public Entity player;
    public Dictionary<string, int> siteProgress;
    public List<string> rareKilled;
    public DateTime date;
    public bool hardcore;
}
