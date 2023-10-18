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
    }

    //Name of the race
    public string name;

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
