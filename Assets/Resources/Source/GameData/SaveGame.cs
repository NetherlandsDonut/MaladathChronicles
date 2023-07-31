using System;
using System.Collections.Generic;

public class SaveGame
{
    public SaveGame()
    {
        player = new Entity(
            "Gaval",
            Race.races.Find(x => x.name == "Dwarf"),
            Class.classes.Find(x => x.name == "Mage"),
            new()
            {

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
