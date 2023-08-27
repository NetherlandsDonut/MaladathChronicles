using System;
using System.Collections.Generic;

using static Root;

public class SaveSlot
{
    public string realm;
    public Entity player;
    public Dictionary<string, int> siteProgress, commonsKilled, raresKilled, elitesKilled;
    public DateTime startDate, lastLoaded, lastPlayed;
    public TimeSpan timePlayed;

    public Realm GetRealm()
    {
        return Realm.realms.Find(x => x.name == realm);
    }

    public static void AddNewSave()
    {
        var newSlot = new SaveSlot
        {
            realm = GameSettings.settings.selectedRealm,
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
        slots.Add(newSlot);
        if (slots.Count == 1)
            GameSettings.settings.selectedCharacter = slots[0].player.name;
    }

    public static void CloseSave()
    {
        currentSlot.lastPlayed = DateTime.Now;
        currentSlot.timePlayed.Add(DateTime.Now - currentSlot.lastLoaded);
        currentSlot = null;
    }

    public static void Login()
    {
        currentSlot = slots.Find(x => x.player.name == GameSettings.settings.selectedCharacter);
        currentSlot.lastLoaded = DateTime.Now;
    }

    public static void SaveGames()
    {
        if (currentSlot != null)
        {
            currentSlot.lastPlayed = DateTime.Now;
            currentSlot.timePlayed.Add(DateTime.Now - currentSlot.lastLoaded);
        }
        Serialization.Serialize(slots, "characters");
        Serialization.Serialize(GameSettings.settings, "settings");
    }

    public static SaveSlot currentSlot;
    public static List<SaveSlot> slots;
}
