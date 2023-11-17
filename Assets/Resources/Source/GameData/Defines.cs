public class Defines
{
    public Defines()
    {
        FillNulls();
    }

    //This function generates default values for the game settings
    public void FillNulls()
    {
        if (maxPathLength < 0)
            maxPathLength = 9999;
        if (maxPlayerLevel < 1)
            maxPlayerLevel = 60;
        if (maxBagsEquipped < 1)
            maxBagsEquipped = 4;
        if (backpackSpace < 1)
            backpackSpace = 3;
        if (aiDepth < 1)
            aiDepth = 5;
        if (aiManualBranches < 1)
            aiManualBranches = 1;
        if (textPaddingLeft < 0)
            textPaddingLeft = 4;
        if (textPaddingRight < 0)
            textPaddingRight = 12;
        if (shadowSystem < 0)
            shadowSystem = 1;
        if (adeptTreeRequirement < 0)
            adeptTreeRequirement = 10;
        if (mapGridSize < 0)
            mapGridSize = 19;
        if (markerCharacter == null || markerCharacter == "")
            markerCharacter ??= "_";
        if (textWrapEnding == null || textWrapEnding == "")
            textWrapEnding ??= "...";
        if (frameTime < 0.01f)
            frameTime = 0.08f;
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
