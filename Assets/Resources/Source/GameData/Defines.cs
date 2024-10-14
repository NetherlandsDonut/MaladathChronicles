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
        if (defaultStanding < 0)
            defaultStanding = 4200;
        if (lvlRequiredFastMounts < 0)
            lvlRequiredFastMounts = 40;
        if (lvlRequiredVeryFastMounts < 0)
            lvlRequiredVeryFastMounts = 60;
        if (maxPrimaryProfessions < 0)
            maxPrimaryProfessions = 2;
        if (expForExploration < 20)
            expForExploration = 20;
        if (markerCharacter == null || markerCharacter == "")
            markerCharacter ??= "_";
        if (textWrapEnding == null || textWrapEnding == "")
            textWrapEnding ??= "...";
        if (frameTime < 0.01f)
            frameTime = 0.08f;
        if (cascadeMinimum < 2)
            cascadeMinimum = 2;
    }

    public int maxPathLength;

    //Maximum level achievable by the player
    public int maxPlayerLevel;

    //Maximum amount of bags equipped by player
    public int maxBagsEquipped;

    //Amount of space provided to the player in their inventory without any bags equipped
    public int backpackSpace;

    //Indicates how many board moves will AI consider during move calculations
    public int aiManualBranches;

    //Amount of padding on the left side of text lines
    public int textPaddingLeft;
    
    //Amount of padding on the right side of text lines
    public int textPaddingRight;

    //Shadow system used by the program
    public int shadowSystem;

    //Amount of talent points spent required in the novice tree for the adept tree to be available
    public int adeptTreeRequirement;

    //Default amount of minutes that take for an item to disappear from merchant buyback
    public int buybackDecay;

    //Amount of score awarded for exploring a site
    public int scoreForExploredSite;

    //Amount of score awarded for killing a common enemy
    public int scoreForKilledCommon;

    //Amount of score awarded for killing a rare enemy
    public int scoreForKilledRare;

    //Amount of score awarded for killing an elite enemy
    public int scoreForKilledElite;

    //Level required for using fast mounts
    public int lvlRequiredFastMounts;

    //Level required for using very fast mounts
    public int lvlRequiredVeryFastMounts;

    //Default reputation standing for factions
    public int defaultStanding;

    //Max amount of primary professions that player can have at once
    public int maxPrimaryProfessions;

    //Amount of experience given to player when they explore a new area
    public int expForExploration;

    //Amount of elements on the board next to each other required for the elements to cascade
    public int cascadeMinimum;

    //Text displayed when an input field is active, this 
    public string markerCharacter;

    //Indicates whether the program will use the specail text wrap ending indicator at the end of text lines that don't fit all the text
    public string textWrapEnding;

    //Amount of time that program counts as a basic unit of animation updates
    public float frameTime;

    //Indicates whether the resource particles spawned during combat fly towards collector portrait
    public bool animatedResourceParticles;

    //Indicates whether the program will use the faster but less accurate pathfinding method
    public bool fasterPathfinding;

    //Indicates whether the experience bar is split into smaller fragments
    public bool splitExperienceBar;

    //Indicates whether the full rectangular borders of the window are drawn
    public bool windowBorders;

    //EXTERNAL FILE: Collection of constant values or alternate ways of handling engine
    public static Defines defines;
}
