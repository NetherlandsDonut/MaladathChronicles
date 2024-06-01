using System.Collections.Generic;

public class Quest
{
    //Name of the quest
    public string name;

    //Required level to have this quest
    public int requiredLevel;

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
    public int reputation;

    //Site where the quest is available for pickup
    public string siteStart;

    //Site where the quest can be handed in
    public string siteEnd;

    //Conditions for completing the quest
    public List<QuestCondition> conditions;

    public string Icon() => "Zone" + Site.FindSite(x => x.name == siteStart).zone.Clean();

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
