using System.Collections.Generic;

public class Quest
{
    //Name of the quest
    public string name;

    //Required level to have this quest
    public int requiredLevel;

    //Level of the quest
    public int questLevel;

    //Reputation connected with the quest
    public string faction;

    //Required reputation rank from the faction
    //for the quest to be available
    public string requiredRank;

    //Amount of money awarded
    public int money;

    //Amount of experience awarded
    public int experience;

    //Amount of reputation awarded to the quest faction
    public Dictionary<string, int> reputationGain;

    //Eligble races for this quest
    public List<string> races;

    //Eligble classes for this quest
    public List<string> classes;

    //Description of the quest
    public string description;

    //Zone of the quest
    public string zone;

    //Site where the quest is available for pickup
    public string siteStart;

    //Site where the quest can be handed in
    public string siteEnd;

    //Conditions for completing the quest
    public List<QuestCondition> conditions;

    //Icon for the quest
    public string Icon() => "Zone" + zone.Clean();

    //Currently selected quest
    public static Quest quest;

    //EXTERNAL FILE: List containing all quests in-game
    public static List<Quest> quests;

    //List of all filtered quests by input search
    public static List<Quest> questsSearch;

    //All sites that currently have quest markers on them
    public static List<Site> sitesWithQuestMarkers;

    //All the sites that need to be respawned after entering the map again
    public static List<Site> sitesToRespawn;
}
