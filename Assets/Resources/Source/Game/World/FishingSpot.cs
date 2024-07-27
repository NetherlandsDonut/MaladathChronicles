using System.Linq;
using System.Collections.Generic;

using static Root;
using static Sound;
using static SaveGame;

public class FishingSpot
{
    //Site where this fishing spot was started
    public string name;

    //Zone where this fishing spot is at
    public string zone;

    //Type of the water used for visual representation
    public string waterType;

    //Difficulty of this fishing spot
    public int skillToFish;

    //Difficulty of this fishing spot
    public int difficulty;

    //Fished up item
    public List<string> possibleLoot;

    //Fished up item
    public List<string> rareLoot;

    //Checks wether the fisher can get skill from fishing in this spot
    public bool ShouldImproveSkill() => currentSave.player.professionSkills["Fishing"].Item1 - 20 <= difficulty; 

    //Ends fishing in this fishing spot
    public void EndFishing(string result)
    {
        CloseDesktop("FishingGame");
        if (result == "Won")
        {
            PlaySound("FishingReelIn");
            if (ShouldImproveSkill())
            {
                var limit = Profession.professions.Find(x => x.name == "Fishing").levels.FindAll(x => currentSave.player.professionSkills["Fishing"].Item2.Contains(x.name)).Max(x => x.maxSkill);
                var temp = currentSave.player.professionSkills["Fishing"];
                if (temp.Item1 < limit) currentSave.player.professionSkills["Fishing"] = (temp.Item1 + 1, temp.Item2);
            }
            if (!currentSave.fishingSpots.ContainsKey(name))
                currentSave.fishingSpots.Add(name, 1);
            else currentSave.fishingSpots[name]++;
            fishingLoot = Item.items.Find(x => x.name == possibleLoot[random.Next(possibleLoot.Count)]).CopyItem(1);
            if (!currentSave.fishingSpoils.ContainsKey(fishingLoot.name))
                currentSave.fishingSpoils.Add(fishingLoot.name, 1);
            else currentSave.fishingSpoils[fishingLoot.name]++;
            //var output = "";
            if (fishingLoot.unique && !currentSave.player.uniquesGotten.Contains(fishingLoot.name))
                currentSave.player.uniquesGotten.Add(fishingLoot.name);
            fishingLoot.SetRandomEnchantment();
            //if (!output.EndsWith(" ")) SpawnFallingText(new Vector2(0, -72), output, "Yellow");
        }
        else if (result == "Lost")
        {

        }
        else if (result == "Left")
        {

        }
        if (CDesktop.screenLocked)
            CDesktop.UnlockScreen();
    }

    //Fished up item
    public static Item fishingLoot;

    //Currently opened fishing spot
    public static FishingSpot fishingSpot;

    //EXTERNAL FILE: List containing all fishing spots in-game
    public static List<FishingSpot> fishingSpots;
}
