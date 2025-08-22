public class GameSettings
{
    public GameSettings()
    {
        FillNulls();
    }

    //This function generates default values for the game settings
    public void FillNulls()
    {
        pixelPerfectVision ??= new Bool(true);
        music ??= new Bool(false);
        ambience ??= new Bool(false);
        soundEffects ??= new Bool(false);
        chartBigIcons ??= new Bool(true);
        fastCascading ??= new Bool(false);
        onlyHavingMaterials ??= new Bool(false);
        onlySkillUp ??= new Bool(false);
        questLevel ??= new Bool(false);
        autoCloseLoot ??= new Bool(false);
        rarityIndicators ??= new Bool(true);
        bigRarityIndicators ??= new Bool(false);
        upgradeIndicators ??= new Bool(false);
        newSlotIndicators ??= new Bool(false);
        runsInBackground ??= new Bool(false);
        showLowLevelQuests ??= new Bool(true);
    }

    //Currently character in the login screen
    public int lastActiveCharacter;
    
    //Indicates whether camera rendering is being sharp to keep the pixel ratio
    public Bool pixelPerfectVision;

    //Indicates whether program plays music in the background
    public Bool music;

    //Indicates whether program plays ambience in the background
    public Bool ambience;

    //Indicates whether program plays sound effects
    public Bool soundEffects;

    //Indicates whether items in-game have rarity indicator triangles in the top-right corners
    public Bool rarityIndicators;

    //Indicates whether the rarity indicators in the items are big or small
    public Bool bigRarityIndicators;

    //Indicates whether quest level is shown in the quest log list
    public Bool questLevel;

    //Indicates whether low level quests are displayed on the map
    public Bool showLowLevelQuests;

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

    //Determines whether the program runs while in background
    public Bool runsInBackground;

    //EXTERNAL FILE: Collection of all settings in game
    public static GameSettings settings;
}
