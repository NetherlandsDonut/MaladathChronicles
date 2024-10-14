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
        side = Faction().side;
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
        category = portrait.ToLower().Contains("quilboar") ? "Humanoid" : (portrait.ToLower().Contains("draco") || portrait.ToLower().Contains("drago") ? "Dragonkin" : (portrait.ToLower().Contains("giant") ? "Giant" : (portrait.ToLower().Contains("element") || portrait.ToLower().Contains("bog") || portrait.ToLower().Contains("ooze")|| portrait.ToLower().Contains("obsidian") || portrait.ToLower().Contains("slime") || portrait.ToLower().Contains("treant") || portrait.ToLower().Contains("lasher") || portrait.ToLower().Contains("ancient") ? "Elemental" : (portrait.ToLower().Contains("fel") || portrait.ToLower().Contains("imp") || portrait.ToLower().Contains("demon") || portrait.ToLower().Contains("infernal") || portrait.ToLower().Contains("sayaad") || portrait.ToLower().Contains("satyr") ? "Demon" : (new List<string>() { "boar", "darkhound", "tallstrider", "kodo", "hippo", "wyvern", "gryph", "borer", "bear", "bird", "cat", "crab", "zhevra", "hydra", "makrura", "hyena", "hound", "giraffe", "bat", "croc", "core", "owl", "basilisk", "thunderlizard", "gorilla", "serpent", "turtle", "panther", "coyote", "saber", "cheetah", "cougar", "tiger", "scorpid", "spider", "tarantula", "wolf", "worg", "raptor", "owl", "saur" }.Any(x => portrait.ToLower().Contains(x)) ? "Beast" : (new List<string>() { "abomination", "skelet", "ghoul", "banshee", "crypt", "zombie", "wraith", "risen", "construct" }.Any(x => portrait.ToLower().Contains(x)) ? "Undead" : (new List<string>() { "silithid", "aqir", "qiraji", "wasp", "larva", "carrion" }.Any(x => portrait.ToLower().Contains(x)) ? "Insect" : "Humanoid")))))));
        if (category == "Beast")
        {
            if (portrait.ToLower().Contains("bear")) subcategory = "Bear";
            else if (portrait.ToLower().Contains("wolf")) subcategory = "Wolf";
            else if (portrait.ToLower().Contains("worg")) subcategory = "Worg";
            else if (portrait.ToLower().Contains("saur") || portrait.ToLower().Contains("raptor")) subcategory = "Raptor";
            else if (portrait.ToLower().Contains("crab")) subcategory = "Crab";
            else if (portrait.ToLower().Contains("makrura")) subcategory = "Makrura";
            else if (portrait.ToLower().Contains("spider")) subcategory = "Spider";
            else if (portrait.ToLower().Contains("tarantula")) subcategory = "Spider";
            else if (portrait.ToLower().Contains("bird")) subcategory = "Carrion Bird";
            else if (portrait.ToLower().Contains("gorilla")) subcategory = "Gorilla";
            else if (portrait.ToLower().Contains("scorpid")) subcategory = "Scorpid";
            else if (portrait.ToLower().Contains("turtle")) subcategory = "Turtle";
            else if (portrait.ToLower().Contains("basilisk")) subcategory = "Basilisk";
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
            else if (portrait.ToLower().Contains("tallstrider")) subcategory = "Tallstrider";
            else if (portrait.ToLower().Contains("darkhound")) subcategory = "Dark Hound";
            else if (portrait.ToLower().Contains("worgen")) { category = "Humanoid"; subcategory = "Worgen"; }
            else if (portrait.ToLower().Contains("quil")) { category = "Humanoid"; subcategory = "Quilboar"; }
            else if (portrait.ToLower().Contains("kodo")) subcategory = "Kodo";
            else if (portrait.ToLower().Contains("thunderlizard")) subcategory = "Thunder Lizard";
            else if (portrait.ToLower().Contains("core")) subcategory = "Core Hound";
            else if (portrait.ToLower().Contains("hound")) subcategory = "Hound";
            else if (portrait.ToLower().Contains("cheetah")) subcategory = "Cat";
            else if (portrait.ToLower().Contains("cougar")) subcategory = "Cat";
            else if (portrait.ToLower().Contains("humar")) subcategory = "Cat";
            else if (portrait.ToLower().Contains("saber")) subcategory = "Cat";
            else if (portrait.ToLower().Contains("panther")) subcategory = "Cat";
            else if (portrait.ToLower().Contains("coyote")) subcategory = "Coyote";
        }
        else if (category == "Humanoid")
        {
            if (portrait.ToLower().Contains("gnoll")) subcategory = "Gnoll";
            else if (portrait.ToLower().Contains("boar")) subcategory = "Quilboar";
            else if (portrait.ToLower().Contains("kobold")) subcategory = "Kobold";
            else if (portrait.ToLower().Contains("ogre")) subcategory = "Ogre";
            else if (portrait.ToLower().Contains("burningblade")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("searingblade")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("blackrock")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("blackhand")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("blackwing")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("dragonmaw")) subcategory = "Orc";
            else if (portrait.ToLower().Contains("nagafemale")) subcategory = "Naga Siren";
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
            else if (portrait.ToLower().Contains("witherbark")) subcategory = "Troll";
            else if (portrait.ToLower().Contains("wildkin")) subcategory = "Wildkin";
            else if (portrait.ToLower().Contains("mrsmite")) subcategory = "Tauren";
            else if (portrait.ToLower().Contains("grimtotem")) subcategory = "Tauren";
            else if (portrait.ToLower().Contains("anathek")) subcategory = "Troll";
            else subcategory = "Human";
        }
        else if (category == "Undead")
        {
            if (portrait.ToLower().Contains("abomination")) subcategory = "Abomination";
            else if (portrait.ToLower().Contains("skeletalwarrior")) subcategory = "Armored Skeleton";
            else if (portrait.ToLower().Contains("skele")) subcategory = "Skeleton";
            else if (portrait.ToLower().Contains("wraith")) subcategory = "Ghost";
            else if (portrait.ToLower().Contains("banshee")) subcategory = "Banshee";
            else if (portrait.ToLower().Contains("zombie")) subcategory = "Zombie";
            else if (portrait.ToLower().Contains("ghoul")) subcategory = "Ghoul";
            else if (portrait.ToLower().Contains("crypt")) subcategory = "Crypt Fiend";
            else if (portrait.ToLower().Contains("wight")) subcategory = "Wight";
            else subcategory = "Undead";
        }
        else if (category == "Dragonkin")
        {
            if (portrait.ToLower().Contains("spawn")) subcategory = "Dragonspawn";
            else if (portrait.ToLower().Contains("draconid")) subcategory = "Draconid";
            else if (portrait.ToLower().Contains("faerie")) subcategory = "Faerie Dragon";
            else subcategory = "Dragon";
        }
        else if (category == "Elemental")
        {
            if (portrait.ToLower().Contains("bog")) subcategory = "Bog Beast";
            else if (portrait.ToLower().Contains("obsidian")) subcategory = "Qiraji Construct";
            else if (portrait.ToLower().Contains("ooze")) subcategory = "Ooze";
            else if (portrait.ToLower().Contains("slime")) subcategory = "Ooze";
            else if (portrait.ToLower().Contains("lasher")) subcategory = "Lasher";
            else if (portrait.ToLower().Contains("treant")) subcategory = "Treant";
            else if (portrait.ToLower().Contains("ancient")) subcategory = "Ancient";
            else subcategory = null;
        }
        else if (category == "Insect")
        {
            if (portrait.ToLower().Contains("qiraji")) subcategory = "Qiraji";
            else if (portrait.ToLower().Contains("queen")) subcategory = "Qiraji";
            else if (portrait.ToLower().Contains("larva")) subcategory = "Larva";
            else subcategory = "Silithid";
        }
        else if (category == "Demon")
        {
            if (portrait.ToLower().Contains("imp")) subcategory = "Imp";
            else if (portrait.ToLower().Contains("satyr")) subcategory = "Satyr";
            else if (portrait.ToLower().Contains("sayaad")) subcategory = "Sayaad";
            else if (portrait.ToLower().Contains("infernal")) subcategory = "Infernal";
            else if (portrait.ToLower().Contains("guard")) subcategory = "Fel Guard";
            else if (portrait.ToLower().Contains("stalker")) subcategory = "Fel Stalker";
            else if (portrait.ToLower().Contains("hunter")) subcategory = "Fel Stalker";
        }
        else if (category == "Giant")
        {
            if (portrait.ToLower().Contains("slime")) (category, subcategory) = ("Elemental", "Ooze");
        }
        if (portrait.ToLower().Contains("golem")) { category = "Mechanical"; subcategory = "Golem"; }
        if (portrait.ToLower().Contains("cthun")) { category = "Old God"; subcategory = "Old God"; }
        if (portrait.ToLower().Contains("7xt")) { category = "Mechanical"; subcategory = "Robot"; }
        if (portrait.ToLower().Contains("anubisath")) { category = "Elemental"; subcategory = "Anubisath"; }
    }

    //Name of the race
    public string name;

    //Category this race belongs to
    public string category, subcategory;

    //Faction of the race
    //Killing entities of this race will grant negative reputation with this faction
    //and grant positive reputation standing with factions that are it's main enemies
    public string faction, side;
    public Faction Faction()
    {
        if (faction == null) return new Faction() { name = "None", side = "Neutral" };
        return factions.Find(x => x.name == faction);
    }

    //Portrait icon of the race
    public string portrait;

    //Race starting bonus stats for characters
    //This is generally used only by player races as enemies have their stats calculated automatically 
    public Dictionary<string, int> stats;

    //List of abilities provided to all entities of this race
    public Dictionary<string, int> abilities;

    #region Non-playable Race Fields

    //Kind of the race, kind of like rarity.
    //This can be one of three values: "Common", "Rare" or "Elite"
    public string kind;

    //Race's vitality which affects how much stamina will the enemy have
    //Generally this determines how hard of a fight an enemy is going to put up against the player.
    //Common enemies usually oscilate around 1.0 vitality with enemies like abominations
    //being really tough and enemies like skeletons being weaker than that. Rare enemies usually
    //oscilate around 2.0 and elite's around 3.0 with raid bosses being exceptionally tough with
    //vitality reaching up to 6.0 which indidacte hardest fights in the game like Doom Lord Kazzak
    public double vitality;

    //List of possible item drops by this race
    [NonSerialized] public List<string> droppedItems;

    //Tags for entity's abilities in order for it to know when to cast what
    public Dictionary<string, string> abilityAITags;

    #endregion

    #region Playable Race Fields

    //Starting site of a race.
    //This is useful only to races playable by player
    //as it provides position to center camera on when creating a new character
    public string startingSite;

    //Preview site of a race.
    //This is the site that is shown in the background while in character creation screen
    public string previewSite;

    //Indicates whether this race uses separate gender portraits.
    //On this value being true program will search for portraits based on entity's gender
    //EXAMPLE: on race name being "Troll" it will search for "TrollMale" and "TrollFemale" in assets.
    public bool genderedPortrait;

    //List of male names that are used when creating player characters
    public List<string> maleNames;

    //List of female names that are used when creating player characters
    public List<string> femaleNames;

    #endregion

    //Currently opened race
    public static Race race;

    //EXTERNAL FILE: List containing all races in-game
    public static List<Race> races;

    //List of all filtered races by input search
    public static List<Race> racesSearch;
}
