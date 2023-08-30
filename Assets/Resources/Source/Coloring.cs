using UnityEngine;
using System.Collections.Generic;

using static SaveGame;

public static class Coloring
{
    public static string ColorItemRequiredLevel(int level)
    {
        if (currentSave != null && currentSave.player != null && level > currentSave.player.level) return "Red";
        else return "Gray";
    }

    public static string ColorProgress(int progress)
    {
        if (progress == 0) return "DarkGray";
        else if (progress <= 20) return "Red";
        else if (progress <= 40) return "Orange";
        else if (progress <= 60) return "Yellow";
        else return "Green";
    }

    public static string ColorEntityLevel(int level)
    {
        if (currentSave == null) return "Gray";
        if (level - 4 > currentSave.player.level) return "DangerousRed";
        else if (level - 2 > currentSave.player.level) return "Orange";
        else if (level + 2 < currentSave.player.level && currentSave.player.WillGetExperience(level)) return "Green";
        else if (!currentSave.player.WillGetExperience(level)) return "DarkGray";
        else return "Yellow";
    }

    public static Dictionary<string, Color32> colors =
    new()
    {
        { "White", new Color32(234, 234, 234, 255) },
        { "LightGray", new Color32(202, 202, 202, 255) },
        { "Gray", new Color32(183, 183, 183, 255) },
        { "DarkGray", new Color32(114, 114, 114, 255) },
        { "Black", new Color32(31, 31, 31, 255) },
        { "Red", new Color32(181, 77, 77, 255) },
        { "DangerousRed", new Color32(173, 36, 45, 255) },
        { "Yellow", new Color32(181, 159, 77, 255) },
        { "Orange", new Color32(185, 104, 57, 255) },
        { "LightOrange", new Color32(168, 110, 38, 255) },
        { "Green", new Color32(81, 181, 77, 255) },
        { "Druid", new Color32(184, 90, 7, 255) },
        { "Warrior", new Color32(144, 113, 79, 255) },
        { "Rogue", new Color32(184, 177, 76, 255) },
        { "Hunter", new Color32(124, 153, 83, 255) },
        { "Mage", new Color32(45, 144, 170, 255) },
        { "Shaman", new Color32(0, 81, 160, 255) },
        { "Warlock", new Color32(97, 98, 172, 255) },
        { "Paladin", new Color32(177, 101, 134, 255) },
        { "Priest", new Color32(191, 175, 164, 255) },
        { "Monk", new Color32(7, 209, 124, 255) },
        { "Copper", new Color32(184, 80, 41, 255) },
        { "Silver", new Color32(170, 188, 210, 255) },
        { "Gold", new Color32(255, 210, 11, 255) },
        { "Poor", new Color32(114, 114, 114, 255) },
        { "Common", new Color32(183, 183, 183, 255) },
        { "Uncommon", new Color32(26, 201, 0, 255) },
        { "Rare", new Color32(0, 117, 226, 255) },
        { "Epic", new Color32(163, 53, 238, 255) },
        { "Legendary", new Color32(221, 110, 0, 255) },
        { "Fire", new Color32(227, 99, 50, 255) },
        { "Water", new Color32(66, 169, 167, 255) },
        { "Earth", new Color32(128, 94, 68, 255) },
        { "Shadow", new Color32(169, 63, 219, 255) },
        { "Lightning", new Color32(63, 158, 245, 255) },
        { "Order", new Color32(241, 229, 125, 255) },
        { "Frost", new Color32(68, 198, 229, 255) },
        { "Decay", new Color32(201, 208, 19, 255) },
        { "Arcane", new Color32(204, 101, 221, 255) },
        { "Air", new Color32(175, 190, 202, 255) },
    };

}
