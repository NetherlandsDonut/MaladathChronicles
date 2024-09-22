public class GameSettings
{
    public GameSettings()
    {
        FillNulls();
    }

    //This function generates default values for the game settings
    public void FillNulls()
    {
        selectedRealm ??= "";
        selectedRealmRanking ??= "";
        selectedCharacter ??= "";
        pixelPerfectVision ??= new Bool(false);
        music ??= new Bool(true);
        soundEffects ??= new Bool(true);
        chartBigIcons ??= new Bool(true);
        fastCascading ??= new Bool(true);
        onlyHavingMaterials ??= new Bool(false);
        onlySkillUp ??= new Bool(false);
        questLevel ??= new Bool(false);
        sourcedMarket ??= new Bool(false);
        autoCloseLoot ??= new Bool(false);
        rarityIndicators ??= new Bool(false);
        bigRarityIndicators ??= new Bool(false);
        upgradeIndicators ??= new Bool(false);
        newSlotIndicators ??= new Bool(false);
    }

    //Currently selected realm in login screen
    public string selectedRealm;

    //Currently selected realm in ranking screen
    public string selectedRealmRanking;

    //Currently character in the login screen
    public string selectedCharacter;
    
    //Indicates whether camera rendering is being sharp to keep the pixel ratio
    public Bool pixelPerfectVision;

    //Indicates whether program plays music in the background
    public Bool music;

    //Indicates whether program plays sound effects
    public Bool soundEffects;

    //Indicates whether items in-game have rarity indicator triangles in the top-right corners
    public Bool rarityIndicators;

    //Indicates whether the rarity indicators in the items are big or small
    public Bool bigRarityIndicators;

    //Indicates whether quest level is shown in the quest log list
    public Bool questLevel;

    //Indicates whether quest level is shown in the quest log list
    public Bool sourcedMarket;

    //Indicates whether the items in the inventory show green arrows indicating that an item is an upgrade
    public Bool upgradeIndicators;

    //Indicates whether the item in the inventory is a new slot item for the player character
    public Bool newSlotIndicators;

    //Indicates whether the camera in the adventure map snaps to nearest sites
    public Bool chartBigIcons;

    //Tells whether the cascading process in boards will be fast or slow
    public Bool fastCascading;

    //Tells whether player sees only recipes that player has all the required materials for
    public Bool onlyHavingMaterials;

    //Tells whether player sees only recipes that can improve their skill in the profession
    public Bool onlySkillUp;

    //Tells whether the loot windows will close automatically after taking all the items
    public Bool autoCloseLoot;

    //EXTERNAL FILE: Collection of all settings in game
    public static GameSettings settings;
}
