using System;
using System.Linq;
using System.Collections.Generic;

using UnityEditor;

using static Root;
using static Defines;
using static MapGrid;
using static SiteTown;
using static SiteComplex;
using static SiteInstance;
using static GameSettings;
using static Serialization;
using static SiteHostileArea;

public class SaveGame
{
    //Player character in the save
    public Entity player;

    #region Creation

    //Creates a new character
    public static void AddNewSave()
    {
        var race = Race.races.Find(x => x.name == creationRace);
        var spec = Spec.specs.Find(x => x.name == creationSpec);
        var newSlot = new SaveGame
        {
            siteProgress = new(),
            fishingSpots = new(),
            fishingSpoils = new(),
            commonsKilled = new(),
            raresKilled = new(),
            elitesKilled = new(),
            unlockedAreas = new(),
            vendorStock = new(),
            openedChests = new(),
            banks = new(),
            startDate = DateTime.Now,
            player = new Entity
            (
                String.creationName.Value(),
                creationGender,
                race,
                spec,
                spec.startingEquipment[creationRace]
            )
        };
        //foreach (var town in towns)
        //    if (town.people != null)
        //        foreach (var person in town.people)
        //            if (person.itemsSold != null && person.itemsSold.Count > 0)
        //                newSlot.vendorStock.Add(town.name + ":" + person.name, person.ExportStock());
        var possibleBosses = new List<string>()
        {
            "Anub'shiah",
            "Eviscerator",
            "Gorosh the Dervish",
            "Grizzle",
            "Hedrum the Creeper",
            "Ok'thor the Breaker"
        };
        newSlot.ringOfLaw = possibleBosses[random.Next(possibleBosses.Count)];
        possibleBosses = new List<string>()
        {
            "Azshir the Sleepless",
            "Fallen Champion",
            "Ironspine"
        };
        newSlot.forlornCloister = possibleBosses[random.Next(possibleBosses.Count)];
        newSlot.hour = 7;
        newSlot.currentSite = race.startingSite;
        newSlot.siteVisits = new() { { race.startingSite, 1 } };
        saves[settings.selectedRealm].Add(newSlot);
        settings.selectedCharacter = newSlot.player.name;
    }

    #endregion

    #region Score

    //Tells the player what is their current score
    public int Score(bool maxScore = false)
    {
        var sum = 0;
        var allSites = new List<Site>();
        for (int i = 0; i < towns.Count; i++) allSites.Add(towns[i]);
        for (int i = 0; i < areas.Count; i++) allSites.Add(areas[i]);
        for (int i = 0; i < complexes.Count; i++) allSites.Add(complexes[i]);
        for (int i = 0; i < instances.Count; i++) allSites.Add(instances[i]);
        allSites.RemoveAll(x => x.x == 0 && x.y == 0);
        sum += defines.scoreForExploredSite * allSites.Count(x => siteVisits.ContainsKey(x.name) || maxScore);
        var commons = areas.SelectMany(x => x.commonEncounters ?? new()).Select(x => x.who).Distinct().ToList();
        sum += defines.scoreForKilledCommon * commons.Count(x => commonsKilled.ContainsKey(x) || maxScore);
        var rares = areas.SelectMany(x => x.rareEncounters ?? new()).Select(x => x.who).Distinct().ToList();
        sum += defines.scoreForKilledRare * rares.Count(x => raresKilled.ContainsKey(x) || maxScore);
        var elites = areas.SelectMany(x => x.eliteEncounters ?? new()).Select(x => x.who).Distinct().ToList();
        sum += defines.scoreForKilledElite * elites.Count(x => elitesKilled.ContainsKey(x) || maxScore);
        return sum;
    }

    #endregion

    #region Time and day

    //Day in-game
    public int day;

    //Hour in-game
    public int hour;

    //Minute in-game
    public int minute;

    //Seconds in-game
    public int second;

    //Indicates whether it is night at the moment or not
    public bool IsNight() => hour >= 20 || hour < 6;

    //Adds time to the world in-game clock
    public void AddTime(int seconds, int minutes = 0, int hours = 0, int days = 0)
    {
        var prev = minute + ":" + hour + ":" + day;
        second += seconds;
        minute += minutes + second / 60;
        DecayWorldBuffs(minutes + second / 60);
        DecayItems(minutes + second / 60);
        Restock(minutes + second / 60);
        second %= 60;
        hour += hours + minute / 60;
        minute %= 60;
        day += days + hour / 24;
        hour %= 24;
        if (prev == minute + ":" + hour + ":" + day) return;
        mapGrid.updateTextureColors = true;
        Respawn("MapToolbarShadow", true);
        Respawn("MapToolbar", true);
        Respawn("MapToolbarClockLeft", true);
        Respawn("MapToolbarClockRight", true);
        Respawn("MapToolbarStatusLeft", true);
        Respawn("MapToolbarStatusRight", true);
    }

    //Decays items that have duration left of their existance
    //This is used mainly for buyback items from vendors
    public void DecayItems(int minutes)
    {
        buyback?.DecayItems(minutes);
        player.inventory.DecayItems(minutes);
    }

    //Restocks items to vendors
    public void DecayWorldBuffs(int minutes)
    {
        var count = player.worldBuffs.Count;
        for (int i = player.worldBuffs.Count - 1; i >= 0; i--)
        {
            player.worldBuffs[i].minutesLeft -= minutes;
            if (player.worldBuffs[i].minutesLeft <= 0)
                player.worldBuffs.RemoveAt(i);
        }
        if (player.worldBuffs.Count != count)
            Respawn("WorldBuffs", true);
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

    #endregion

    #region Progress

    //Site at which player currently resides
    public string currentSite;

    //Randomly generated bosses for this run
    public string ringOfLaw, forlornCloister;

    //All chests opened in the game are saved here
    public Dictionary<string, Chest> openedChests;

    //Stores progress done while exploring sites in the world
    public Dictionary<string, int> fishingSpots;

    //Stores progress done while exploring sites in the world
    public Dictionary<string, int> fishingSpoils;

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

    #endregion

    #region World

    //Stores all inventory of all vendors in game
    public Dictionary<string, List<StockItem>> vendorStock;
    
    //Stores all bank accounts of this character in towns
    public Dictionary<string, Inventory> banks;

    //List of items available for buying back from vendors
    public Inventory buyback;

    #endregion

    #region Management

    //Date of the character creation
    public DateTime startDate;

    //Date of the last time this character was logging in
    public DateTime lastLoaded;

    //Date of the last time this character was logging out
    public DateTime lastPlayed;

    //Overall time player played this character
    public TimeSpan timePlayed;

    //Keeps information about last visited talents page
    //so that when talent screen is reopened it opens on the last
    //visited page of it to not favorize specs based on order
    public int lastVisitedTalents;

    //Relinks references to static lists for items loaded from saved games
    public void Initialise()
    {
        if (currentSite != null && Site.FindSite(x => x.name == currentSite) == null) currentSite = null;
        currentSite ??= player.Race().startingSite;
        player.homeLocation ??= player.Race().startingSite;
    }

    //Provides information which background should be used for character
    //logging screen which will depend on the place of the logout
    public string LoginBackground()
    {
        var find = Site.FindSite(x => x.name == currentSite);
        return find != null ? find.Background() : "Backgrounds/Sky";
    }

    //Logs a character out of the world
    public static void CloseSave()
    {
        if (currentSave == null) return;
        if (currentSave.playerDead) settings.selectedCharacter = "";
        Save();
        mapGrid.SwitchMapTexture(false);
        currentSave = null;
    }

    //Logs the character into the world
    //Map can now be opened to start playing
    public static void Login()
    {
        currentSave = saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter);
        currentSave.lastLoaded = DateTime.Now;
        currentSave.Initialise();
    }

    //Saves the character in the database
    public static void Save()
    {
        if (currentSave.timePlayed == null) currentSave.timePlayed = new TimeSpan();
        currentSave.timePlayed = currentSave.timePlayed.Add(DateTime.Now - currentSave.lastLoaded);
        currentSave.lastPlayed = DateTime.Now;
        var temp = desktops.Find(x => x.title.Contains("Map"));
    }

    //Saves all characters on the account
    public static void SaveGames()
    {
        if (currentSave != null) Save();
        Serialization.Serialize(saves, "characters", false, false, prefix);
        Serialization.Serialize(settings, "settings", false, false, prefix);
    }

    //Can you save at the moment
    public static bool CanSave()
    {
        var blockedDesktops = new List<string> { "ChestLoot", "ContainerLoot", "CombatResults", "CombatLog", "MiningLoot", "CombatResultsLoot", "HerbalismLoot", "SkinningLoot", "FishingGame", "Game" };
        return !desktops.Any(x => blockedDesktops.Contains(x.title));
    }

    #endregion

    #region Death

    //This variable stores information about entity's death
    public DeathInfo deathInfo;

    //Indicates whether the character is dead at the momentt
    public bool playerDead;

    //Revives the player
    public void RevivePlayer()
    {
        if (!playerDead) return;
        playerDead = false;
        mapGrid.SwitchMapTexture(false);
        SpawnTransition();
        SpawnTransition();
        SpawnTransition();
        SpawnTransition();
        SpawnTransition();
    }

    public void CallEvents(Dictionary<string, string> trigger)
    {
        //Callinig events on player abilities seems useless at the moment, maybe in the future
        //foreach (var ability in player.abilities.Select(x => (Ability.abilities.Find(y => y.name == x.Key), x.Value)))
        //    ability.Item1.ExecuteEvents(this, trigger, ability.Value);

        foreach (var item in player.inventory.items.Concat(player.equipment.Select(x => x.Value)).ToList())
            if (item.abilities != null)
                foreach (var ability in item.abilities.Select(x => (Ability.abilities.Find(y => y.name == x.Key), x.Value)))
                    ability.Item1.ExecuteEvents(this, trigger, item, ability.Value);

        //Calling events on player buffs is not done yet
        //foreach (var buff in player.buffs)
        //    buff.Item1.ExecuteEvents(this, trigger);
    }

    #endregion

    //Currently opened save
    public static SaveGame currentSave;

    //EXTERNAL FILE: List containing all account characters
    public static Dictionary<string, List<SaveGame>> saves;
}
