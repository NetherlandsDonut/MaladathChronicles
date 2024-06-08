using System.Collections.Generic;

public class Quest
{
    public void Initialise()
    {
        if (siteStart != null)
        {
            var find = Site.FindSite(x => x.name == siteStart);
            find.questsStarted ??= new();
            find.questsStarted.Add(this);
        }
    }

    //ID of the quest
    public int questID;

    //Prerequisite quest needed to be done before this becomes available
    public int previous;

    //Required level to have this quest
    public int requiredLevel;

    //Level of the quest
    public int questLevel;

    //Name of the quest
    public string name;

    //Description of the quest
    public string description;

    //Site where the quest is available for pickup
    public string siteStart;

    //Site where the quest can be handed in
    public string siteEnd;

    //Zone of the quest
    public string zone;

    //Reputation connected with the quest
    public string faction;

    //Required reputation rank from the faction
    //for the quest to be available
    public string requiredRank;

    //Amount of money awarded
    public int money;

    //Amount of experience awarded
    public int experience;

    //Eligble races for this quest
    public List<string> races;

    //Eligble classes for this quest
    public List<string> classes;

    //Amount of reputation awarded to the quest faction
    public Dictionary<string, int> reputationGain;

    //Items provided by the quest when player accepts it
    public Dictionary<string, int> providedItems;

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
