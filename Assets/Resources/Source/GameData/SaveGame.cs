using System;
using System.Collections.Generic;

using static Root;

public class SaveGame
{
    public SaveGame()
    {
        raresKilled = new();
        bossesKilled = new();
        player = new Entity
        (
            creationName,
            creationGender,
            Race.races.Find(x => x.name == creationRace),
            Class.classes.Find(x => x.name == creationClass),
            Class.classes.Find(x => x.name == creationClass).startingEquipment[creationRace]
        );
        siteProgress = new Dictionary<string, int>();
        date = DateTime.Now;
    }

    public Entity player;
    public Dictionary<string, int> siteProgress;
    public List<string> raresKilled, bossesKilled;
    public DateTime date;
    public bool hardcore;
}
