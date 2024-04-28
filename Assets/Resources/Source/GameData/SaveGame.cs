using System;
using UnityEngine;
using System.Collections.Generic;

using static Root;
using static GameSettings;
using System.Linq;

public class SaveGame
{
    //Player character in the save
    public Entity player;

    //Day in-game
    public int day;

    //Hour in-game
    public int hour;

    //Minute in-game
    public int minute;

    //Seconds in-game
    public int second;

    public void AddTime(int seconds, int minutes = 0, int hours = 0, int days = 0)
    {
        var prev = minute + ":" + hour + ":" + day;
        second += seconds;
        minute += minutes + second / 60;
        Restock(minutes + second / 60);
        DecayItems(minutes + second / 60);
        second %= 60;
        hour += hours + minute / 60;
        minute %= 60;
        day += days + hour / 24;
        hour %= 24;
        if (prev == minute + ":" + hour + ":" + day) return;
        grid.updateTextureColors = true;
        Respawn("MapToolbarShadow", true);
        Respawn("MapToolbar", true);
        Respawn("MapToolbarClockLeft", true);
        Respawn("MapToolbarClockRight", true);
        Respawn("MapToolbarStatusLeft", true);
        Respawn("MapToolbarStatusRight", true);
    }

    //Restocks items to vendors
    public void Restock(int minutes)
    {
        var keys = vendorStock.Keys.ToList();
        foreach (var key in keys)
            vendorStock[key].ForEach(x =>
            {
                var min = minutes;
                while (min > 0)
                {
                    if (x.amount == x.maxAmount)
                    {
                        x.minutesLeft = 0;
                        break;
                    }
                    x.minutesLeft--;
                    min--;
                    if (x.minutesLeft == 0)
                    {
                        x.amount++;
                        x.minutesLeft = x.restockSpeed;
                    }
                }
            });
    }

    //Decays items that have duration left of their existance
    //This is used mainly for buyback items from vendors
    public void DecayItems(int minutes)
    {
        buyback?.DecayItems(minutes);
        player.inventory.DecayItems(minutes);
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

    //Stores information about all unlocked areas in instances
    public List<string> unlockedAreas;

    //Stores all inventory of all vendors in game
    public Dictionary<string, List<StockItem>> vendorStock;
    
    //Stores all bank accounts of this character in towns
    public Dictionary<string, Inventory> banks;

    //List of items available for buying back from vendors
    public Inventory buyback;

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
        return hour >= 20 || hour < 6;
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
        var race = Race.races.Find(x => x.name == creationRace);
        var spec = Spec.specs.Find(x => x.name == creationSpec);
        var newSlot = new SaveGame
        {
            siteProgress = new(),
            commonsKilled = new(),
            raresKilled = new(),
            elitesKilled = new(),
            unlockedAreas = new(),
            vendorStock = new(),
            startDate = DateTime.Now,
            player = new Entity
            (
                creationName,
                creationGender,
                race,
                spec,
                spec.startingEquipment[creationRace]
            )
        };
        foreach (var town in SiteTown.towns)
            if (town.people != null)
                foreach (var person in town.people)
                    if (person.itemsSold != null && person.itemsSold.Count > 0)
                        newSlot.vendorStock.Add(town.name + ":" + person.name, person.ExportStock());
        newSlot.currentSite = race.startingSite;
        newSlot.siteVisits = new() { { race.startingSite, 1 } };
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
        if (currentSave.currentSite != null && Site.FindSite(x => x.name == currentSave.currentSite) == null) currentSave.currentSite = null;
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
