using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Faction;

public class Race
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        side = Faction().side;
        droppedItems = Item.items.FindAll(x => x.droppedBy != null && x.droppedBy.Contains(name)).Select(x => x.name).ToList();
        if (category == null || category == string.Empty) Debug.Log("ERROR 018: Race lacks category: \"" + name + "\"");
    }

    //Name of the race
    public string name;

    //Category this race belongs to
    public string category, subcategory;

    //Faction of the race
    //Killing entities of this race will grant negative reputation with this faction
    //and grant positive reputation standing with factions that are it's main enemies
    public string faction;

    //Side of the conflict this faction is
    [NonSerialized] public string side;

    //Gets the faction of this entity
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

    //Forced gender for this race
    public string gender;

    //Race's vitality which affects how much stamina will the enemy have
    //Generally this determines how hard of a fight an enemy is going to put up against the player.
    //Common enemies usually oscilate around 1.0 vitality with enemies like abominations
    //being really tough and enemies like skeletons being weaker than that. Rare enemies usually
    //oscilate around 2.0 and elite's around 3.0 with raid bosses being exceptionally tough with
    //vitality reaching up to 6.0 which indicate hardest fights in the game like Doom Lord Kazzak
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
