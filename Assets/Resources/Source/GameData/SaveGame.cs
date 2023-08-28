using System;
using System.Collections.Generic;

using static Root;
using static GameSettings;

public class SaveGame
{
    public Entity player;
    public Dictionary<string, int> siteProgress, commonsKilled, raresKilled, elitesKilled;
    public DateTime startDate, lastLoaded, lastPlayed;
    public TimeSpan timePlayed;

    public static void AddNewSave()
    {
        var newSlot = new SaveGame
        {
            siteProgress = new(),
            commonsKilled = new(),
            raresKilled = new(),
            elitesKilled = new(),
            startDate = DateTime.Now,
            player = new Entity
            (
                creationName,
                creationGender,
                Race.races.Find(x => x.name == creationRace),
                Class.specs.Find(x => x.name == creationClass),
                Class.specs.Find(x => x.name == creationClass).startingEquipment[creationRace]
            )
        };
        saves[settings.selectedRealm].Add(newSlot);
    }

    public static void CloseSave()
    {
        currentSave.lastPlayed = DateTime.Now;
        currentSave.timePlayed.Add(DateTime.Now - currentSave.lastLoaded);
        currentSave = null;
    }

    public static void Login()
    {
        currentSave = saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter);
        currentSave.lastLoaded = DateTime.Now;
    }

    public static void SaveGames()
    {
        if (currentSave != null)
        {
            currentSave.lastPlayed = DateTime.Now;
            currentSave.timePlayed.Add(DateTime.Now - currentSave.lastLoaded);
        }
        Serialization.Serialize(saves, "characters");
        Serialization.Serialize(settings, "settings");
    }

    public static SaveGame currentSave;
    public static Dictionary<string, List<SaveGame>> saves;
}
