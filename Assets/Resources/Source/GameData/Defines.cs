public class Defines
{
    public Defines()
    {
        FillNulls();
    }

    //This function generates default values for the game settings
    public void FillNulls()
    {
        maxPathLength ??= 9999;
        maxPlayerLevel ??= 60;
        maxBagsEquipped ??= 3;
        backpackSpace ??= 12;
        aiDepth ??= 5;
        aiManualBranches ??= 1;
        textPaddingLeft ??= 4;
        textPaddingRight ??= 12;
        shadowSystem ??= 1;
        adeptTreeRequirement ??= 10;
        mapGridSize ??= 19;
        markerCharacter ??= "_";
        textWrapEnding ??= "...";
        frameTime ??= 0.08f;
    }

    public int maxPathLength;
    public int maxPlayerLevel;
    public int maxBagsEquipped;
    public int backpackSpace;
    public int aiDepth;
    public int aiManualBranches;
    public int textPaddingLeft;
    public int textPaddingRight;
    public int shadowSystem;
    public int adeptTreeRequirement;
    public int mapGridSize;
    public string markerCharacter;
    public string textWrapEnding;
    public float frameTime;

    //EXTERNAL FILE: Collection of all settings in game
    public static Defines defines;
}
