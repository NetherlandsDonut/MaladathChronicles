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
        selectedCharacter ??= "";
        shadows ??= new Bool(true);
        pixelPerfectVision ??= new Bool(false);
        music ??= new Bool(true);
        soundEffects ??= new Bool(true);
        snapCamera ??= new Bool(true);

        rarityIndicators ??= new Bool(true);
        bigRarityIndicators ??= new Bool(true);
        upgradeIndicators ??= new Bool(false);
        newSlotIndicators ??= new Bool(false);
    }

    //Currently selected realm in login screen
    public string selectedRealm;

    //Currently character in the login screen
    public string selectedCharacter;

    //Indicates whether windows cast shadows
    public Bool shadows;
    
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

    //Indicates whether the items in the inventory show green arrows indicating that an item is an upgrade
    public Bool upgradeIndicators;

    //Indicates whether the item in the inventory is a new slot item for the player character
    public Bool newSlotIndicators;

    //Indicates whether the camera in the adventure map snaps to nearest sites
    public Bool snapCamera;

    //EXTERNAL FILE: Collection of all settings in game
    public static GameSettings settings;
}
