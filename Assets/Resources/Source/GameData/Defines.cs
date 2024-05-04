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
            maxBagsEquipped = 3;
        if (backpackSpace < 1)
            backpackSpace = 9;
        if (aiDepth < 1)
            aiDepth = 7;
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
        if (buybackDecay < 0)
            buybackDecay = 30;
        if (scoreForExploredSite < 0)
            scoreForExploredSite = 2;
        if (scoreForKilledCommon < 0)
            scoreForKilledCommon = 1;
        if (scoreForKilledRare < 0)
            scoreForKilledRare = 5;
        if (scoreForKilledElite < 0)
            scoreForKilledElite = 10;
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
    public int buybackDecay;
    public int scoreForExploredSite;
    public int scoreForKilledCommon;
    public int scoreForKilledRare;
    public int scoreForKilledElite;
    public string markerCharacter;
    public string textWrapEnding;
    public float frameTime;
    public bool fasterPathfinding;
    public bool splitExperienceBar;

    //EXTERNAL FILE: Collection of constant values or alternate ways of handling engine
    public static Defines defines;
}
