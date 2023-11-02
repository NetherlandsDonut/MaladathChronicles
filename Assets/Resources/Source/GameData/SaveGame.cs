using System;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static GameSettings;
using static SiteHostileArea;
using static SiteInstance;
using static SiteComplex;
using static SiteTown;

public class SaveGame
{
    //Player character in the save
    public Entity player;

    //Hour in-game
    public int hour;

    //Minute in-game
    public int minute;

    public void AddTime(int minutes, int hours = 0, int days = 0)
    {
        minute += minutes;
        hour += hours + minute / 60;
        minute %= 60;
        day += days + hour / 24;
        hour %= 24;
        Respawn("MapToolbarClockLeft", true);
        Respawn("MapToolbarClockRight", true);
    }

    //Site at which player currently resides
    public string currentSite;

    //Stores progress done while exploring sites in the world
    public Dictionary<string, int> siteProgress;

    //Stores information about how many common enemies were killed by player
    public Dictionary<string, int> commonsKilled;

    //Stores information about how many rare enemies were killed by player
    public Dictionary<string, int> raresKilled;

    //Stores information about how many elite enemies were killed by player
    public Dictionary<string, int> elitesKilled;

    //Stores all reputation standings with all factions
    public Dictionary<string, int> factionStanding;

    //Stores amounts of visits to all sites
    //If one is equal at least to 1 the site is considered to be discovered
    public Dictionary<string, int> siteVisits;
    
    //Stores all bank accounts of this character in towns
    public Dictionary<string, Inventory> banks;

    //List of items available for buying back from vendors
    public List<Item> buyback;

    //Date of the character creation
    public DateTime startDate;

    //Date of the last time this character was logging in
    public DateTime lastLoaded;

    //Date of the last time this character was logging out
    public DateTime lastPlayed;
    
    //This variable stores information about entity's death
    public DeathInfo deathInfo;

    //Indicates whether the character is dead at the momentt
    public bool playerDead;

    //Overall time player played this character
    public TimeSpan timePlayed;

    //Keeps information about last visited talents page
    public int lastVisitedTalents;

    public bool IsNight()
    {
        return false;
        //return hour >= 20 && hour <= 6;
    }

    //Provides information which background should be used for character
    //logging screen which will depend on the place of the logout
    public string LoginBackground()
    {
        var find = Site.FindSite(x => x.name == currentSite);
        return find != null ? find.Background() : "Sky";
    }

    //Revives the player
    public void RevivePlayer()
    {
        if (!playerDead) return;
        playerDead = false;
        grid.SwitchMapTexture(false);
        SpawnTransition();
        SpawnTransition();
        SpawnTransition();
        SpawnTransition();
        SpawnTransition();
    }

    //Creates a new character
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
                Spec.specs.Find(x => x.name == creationSpec),
                Spec.specs.Find(x => x.name == creationSpec).startingEquipment[creationRace]
            )
        };
        newSlot.currentSite = startingSite;
        newSlot.siteVisits = new() { { startingSite, 1 } };
        saves[settings.selectedRealm].Add(newSlot);
        settings.selectedCharacter = newSlot.player.name;
    }

    //Logs a character out of the world
    public static void CloseSave()
    {
        Save();
        grid.SwitchMapTexture(false);
        currentSave = null;
    }

    //Saves the character in the database
    public static void Save()
    {
        if (currentSave.timePlayed == null) currentSave.timePlayed = new TimeSpan();
        currentSave.timePlayed = currentSave.timePlayed.Add(DateTime.Now - currentSave.lastLoaded);
        currentSave.lastPlayed = DateTime.Now;
        var temp = desktops.Find(x => x.title.Contains("Map"));
    }

    //Logs the character into the world
    //Map can now be opened to start playing
    public static void Login()
    {
        currentSave = saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter);
        currentSave.lastLoaded = DateTime.Now;
        currentSave.currentSite ??= currentSave.player.Race().startingSite;
    }

    //Saves all characters on the account
    public static void SaveGames()
    {
        if (currentSave != null) Save();
        Serialization.Serialize(saves, "characters", false, false, prefix);
        Serialization.Serialize(settings, "settings", false, false, prefix);
    }

    //Currently opened save
    public static SaveGame currentSave;

    //EXTERNAL FILE: List containing all account characters
    public static Dictionary<string, List<SaveGame>> saves;
}
