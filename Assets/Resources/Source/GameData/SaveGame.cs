using System;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static GameSettings;

public class SaveGame
{
    public Entity player;
    public int cameraX, cameraY;
    public Dictionary<string, int> siteProgress, commonsKilled, raresKilled, elitesKilled, factionStanding;
    public Dictionary<string, int> siteProgress, commonsKilled, raresKilled, elitesKilled;
    public Dictionary<string, List<Inventory>> banks;
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
        var startingSite = Race.races.Find(x => x.name == creationRace).startingSite;
        var temp1 = SiteHostileArea.areas.Find(x => x.name == startingSite);
        if (temp1 != null) (newSlot.cameraX, newSlot.cameraY) = (temp1.x - 17, temp1.y + 9);
        var temp2 = SiteTown.towns.Find(x => x.name == startingSite);
        if (temp2 != null) (newSlot.cameraX, newSlot.cameraY) = (temp2.x - 17, temp2.y + 9);
        var temp3 = SiteComplex.complexes.Find(x => x.name == startingSite);
        if (temp3 != null) (newSlot.cameraX, newSlot.cameraY) = (temp3.x - 17, temp3.y + 9);
        var temp4 = SiteInstance.instances.Find(x => x.name == startingSite);
        if (temp4 != null) (newSlot.cameraX, newSlot.cameraY) = (temp4.x - 17, temp4.y + 9);
        saves[settings.selectedRealm].Add(newSlot);
        settings.selectedCharacter = newSlot.player.name;
    }

    public static void CloseSave()
    {
        Save();
        currentSave = null;
    }

    public static void Save()
    {
        currentSave.timePlayed.Add(DateTime.Now - currentSave.lastLoaded);
        currentSave.lastPlayed = DateTime.Now;
        var temp = desktops.Find(x => x.title == "Map");
        currentSave.cameraX = (int)Math.Round(temp.cameraDestination.x);
        currentSave.cameraY = (int)Math.Round(temp.cameraDestination.y);
    }

    public static void Login()
    {
        currentSave = saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter);
        currentSave.lastLoaded = DateTime.Now;
        CDesktop.cameraDestination = new Vector2(currentSave.cameraX, currentSave.cameraY);
    }

    public static void SaveGames()
    {
        if (currentSave != null) Save();
        Serialization.Serialize(saves, "characters", false, false, prefix);
        Serialization.Serialize(settings, "settings", false, false, prefix);
    }

    public static SaveGame currentSave;
    public static Dictionary<string, List<SaveGame>> saves;
}
