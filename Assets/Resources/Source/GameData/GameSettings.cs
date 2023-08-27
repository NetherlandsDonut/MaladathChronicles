public class GameSettings
{
    public GameSettings()
    {
        selectedRealm = "";
        selectedCharacter = "";
        shadows = new Bool(true);
        pixelPerfectVision = new Bool(false);

        rarityIndicators = new Bool(true);
        bigRarityIndicators = new Bool(true);
        upgradeIndicators = new Bool(false);
        newSlotIndicators = new Bool(false);
    }

    public string selectedRealm = "Firemaw", selectedCharacter;

    public Bool shadows;
    public Bool pixelPerfectVision;

    public Bool rarityIndicators;
    public Bool bigRarityIndicators;
    public Bool upgradeIndicators;
    public Bool newSlotIndicators;

    public static GameSettings settings;
}
