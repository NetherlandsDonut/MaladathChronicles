using System;
using System.Collections.Generic;
using System.Linq;
using static Faction;

public class Race
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        if (abilities != null)
            foreach (var ability in abilities)
                if (!Ability.abilities.Exists(x => x.name == ability.Key))
                    Ability.abilities.Insert(0, new Ability()
                    {
                        name = ability.Key,
                        icon = "Ability" + ability.Key.Replace(" ", ""),
                        events = new(),
                        tags = new()
                    });
        if (faction != null)
            if (!factions.Exists(x => x.name == faction))
                factions.Insert(0, new Faction()
                {
                    name = faction,
                    icon = "Faction" + faction,
                    side = "Hostile"
                });
        droppedItems = Item.items.FindAll(x => x.droppedBy != null && x.droppedBy.Contains(name)).Select(x => x.name).ToList();
        category = portrait.ToLower().Contains("draco") || portrait.ToLower().Contains("drago") ? "Dragonkin" : (portrait.ToLower().Contains("giant") ? "Giant" : (portrait.ToLower().Contains("element") || portrait.ToLower().Contains("bog") ? "Elemental" : (portrait.ToLower().Contains("fel") || portrait.ToLower().Contains("imp") || portrait.ToLower().Contains("demon") || portrait.ToLower().Contains("infernal") || portrait.ToLower().Contains("sayaad") ? "Demon" : (new List<string>() { "boar", "hippo", "wyvern", "gryph", "borer", "bear", "bird", "cat", "crab", "hydra", "makrura", "hyena", "giraffe", "bat", "croc", "owl", "gorilla", "serpent", "turtle", "tiger", "scorpid", "spider", "tarantula", "wolf", "worg", "raptor", "owl", "saur" }.Any(x => portrait.ToLower().Contains(x)) ? "Beast" : (new List<string>() { "abomination", "skelet", "ghoul", "banshee", "crypt", "zombie" }.Any(x => portrait.ToLower().Contains(x)) ? "Undead" : (new List<string>() { "silithid", "aqir", "qiraji", "wasp" }.Any(x => portrait.ToLower().Contains(x)) ? "Insect" : "Humanoid"))))));
        if (category == "Beast")
        {
            if (portrait.ToLower().Contains("bear")) subcategory = "Bear";
            else if (portrait.ToLower().Contains("wolf")) subcategory = "Wolf";
            else if (portrait.ToLower().Contains("wolf")) subcategory = "Worg";
            else if (portrait.ToLower().Contains("saur") || portrait.ToLower().Contains("raptor")) subcategory = "Raptor";
            else if (portrait.ToLower().Contains("crab")) subcategory = "Crab";
            else if (portrait.ToLower().Contains("makrura")) subcategory = "Makrura";
            else if (portrait.ToLower().Contains("spider")) subcategory = "Spider";
            else if (portrait.ToLower().Contains("tarantula")) subcategory = "Spider";
            else if (portrait.ToLower().Contains("bird")) subcategory = "Carrion Bird";
            else if (portrait.ToLower().Contains("gorilla")) subcategory = "Gorilla";
            else if (portrait.ToLower().Contains("scorpid")) subcategory = "Scorpid";
            else if (portrait.ToLower().Contains("turtle")) subcategory = "Turtle";
            else if (portrait.ToLower().Contains("giraffe")) subcategory = "Giraffe";
            else if (portrait.ToLower().Contains("owl")) subcategory = "Owl";
            else if (portrait.ToLower().Contains("cat")) subcategory = "Cat";
            else if (portrait.ToLower().Contains("tiger")) subcategory = "Cat";
            else if (portrait.ToLower().Contains("croc")) subcategory = "Crocolisk";
            else if (portrait.ToLower().Contains("serpent")) subcategory = "Wind Serpent";
            else if (portrait.ToLower().Contains("hyena")) subcategory = "Hyena";
            else if (portrait.ToLower().Contains("borer")) subcategory = "Borer";
            else if (portrait.ToLower().Contains("boar")) subcategory = "Boar";
            else if (portrait.ToLower().Contains("bat")) subcategory = "Bat";
            else if (portrait.ToLower().Contains("hydra")) subcategory = "Hydra";
            else if (portrait.ToLower().Contains("wyvern")) subcategory = "Wyvern";
            else if (portrait.ToLower().Contains("hippo")) subcategory = "Hippogryph";
            else if (portrait.ToLower().Contains("gryph")) subcategory = "Gryphon";
        }
        else if (category == "Humanoid")
        {
            if (portrait.ToLower().Contains("gnoll")) subcategory = "Gnoll";
            else if (portrait.ToLower().Contains("kobold")) subcategory = "Kobold";
            else if (portrait.ToLower().Contains("ogre")) subcategory = "Ogre";
            else if (portrait.ToLower().Contains("satyr")) subcategory = "Satyr";
            else if (portrait.ToLower().Contains("burningblade")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("searingblade")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("blackrock")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("blackhand")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("blackwing")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("dragonmaw")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("naga")) subcategory = "Naga";
            else if (portrait.ToLower().Contains("centaur")) subcategory = "Centaur";
            else if (portrait.ToLower().Contains("darkiron")) subcategory = "Dwarf";
            else if (portrait.ToLower().Contains("wildhammer")) subcategory = "Dwarf";
            else if (portrait.ToLower().Contains("dwarf")) subcategory = "Dwarf";
            else if (portrait.ToLower().Contains("gnome")) subcategory = "Gnome";
            else if (portrait.ToLower().Contains("goblin")) subcategory = "Goblin";
            else if (portrait.ToLower().Contains("murloc")) subcategory = "Murloc";
            else if (portrait.ToLower().Contains("trogg")) subcategory = "Trogg";
            else if (portrait.ToLower().Contains("furbolg")) subcategory = "Furbolg";
            else if (portrait.ToLower().Contains("vilebranch")) subcategory = "Troll";
            else if (portrait.ToLower().Contains("sandfury")) subcategory = "Troll";
            else if (portrait.ToLower().Contains("sandscalp")) subcategory = "Troll";
            else if (portrait.ToLower().Contains("gurubashi")) subcategory = "Troll";
            else if (portrait.ToLower().Contains("amani")) subcategory = "Troll";
            else if (portrait.ToLower().Contains("troll")) subcategory = "Troll";
            else if (portrait.ToLower().Contains("atalai")) subcategory = "Troll";
            else if (portrait.ToLower().Contains("bloodscalp")) subcategory = "Troll";
            else subcategory = "Human";
        }
        else if (category == "Dragonkin")
        {
            if (portrait.ToLower().Contains("spawn")) subcategory = "Dragonspawn";
            else if(portrait.ToLower().Contains("draconid")) subcategory = "Draconid";
            else subcategory = "Dragon";
        }
        else if (category == "Insect")
        {
            if (portrait.ToLower().Contains("qiraji")) subcategory = "Qiraji";
            if (portrait.ToLower().Contains("queen")) subcategory = "Qiraji";
            else subcategory = "Silithid";
        }
        else if (category == "Demon")
        {
            if (portrait.ToLower().Contains("imp")) subcategory = "Imp";
            if (portrait.ToLower().Contains("sayaad")) subcategory = "Sayaad";
            if (portrait.ToLower().Contains("infernal")) subcategory = "Infernal";
        }
        if (portrait.ToLower().Contains("golem")) { category = "Mechanical"; subcategory = "Golem"; }
    }

    //Name of the race
    public string name;

    //Category this race belongs to
    public string category, subcategory;

    //Faction of the race
    //Killing entities of this race will grant negative reputation with this faction
    //and grant positive reputation standing with factions that are it's main enemies
    public string faction;
    public Faction Faction()
    {
        if (faction == null) return null;
        return factions.Find(x => x.name == faction);
    }

    //Starting site of a race.
    //This is useful only to races playable by player
    //as it provides position to center camera on when creating a new character
    public string startingSite;

    //Indicates whether this race uses separate gender portraits.
    //On this value being true program will search for portraits based on entity's gender
    //EXAMPLE: on race name being "Troll" it will search for "TrollMale" and "TrollFemale" in assets.
    public bool genderedPortrait;
    
    //Kind of the race, kind of like rarity.
    //This can be one of three values: "Common", "Rare" or "Elite"
    public string kind;

    //Portrait icon of the race
    public string portrait;

    //Race's vitality which affects how much stamina will the enemy have
    //Generally this determines how hard of a fight an enemy is going to put up against the player.
    //Common enemies usually oscilate around 1.0 vitality with enemies like abominations
    //being really tough and enemies like skeletons being weaker than that. Rare enemies usually
    //oscilate around 2.0 and elite's around 3.0 with raid bosses being exceptionally tough with
    //vitality reaching up to 6.0 which indidacte hardest fights in the game like Doom Lord Kazzak
    public double vitality;

    //Race starting bonus stats for characters
    //This is generally used only by player races as enemies have their stats calculated automatically 
    public Stats stats;

    //List of racial abilities provided to all entities of this race
    public Dictionary<string, int> abilities;

    //List of possible item drops by this race
    [NonSerialized] public List<string> droppedItems;

    //List of male names that are used when creating player characters
    public List<string> maleNames;

    //List of female names that are used when creating player characters
    public List<string> femaleNames;

    //Loot that this race can drop
    //This is problematic right now because it is not dependant on entity's level
    public List<(int, string)> loot;

    //Currently opened race
    public static Race race;

    //EXTERNAL FILE: List containing all races in-game
    public static List<Race> races;

    //List of all filtered races by input search
    public static List<Race> racesSearch;
}
