using System;
using System.Collections.Generic;

using static Root;

public class SaveGame
{
    public SaveGame()
    {
        player = new Entity
        (
            creationName,
            creationGender,
            Race.races.Find(x => x.name == creationRace),
            Class.classes.Find(x => x.name == creationClass),
            new()
            {
                "Apprentice's Robe",
                "Apprentice's Pants",
                "Apprentice's Boots",
                //"Vanquished Tentacle of C'Thun",
                "Bent Staff",
            }
        );
        siteProgress = new Dictionary<string, int>();
        date = DateTime.Now;
    }

    public Entity player;
    public Dictionary<string, int> siteProgress;
    public List<string> raresKilled;
    public DateTime date;
    public bool hardcore;
}
