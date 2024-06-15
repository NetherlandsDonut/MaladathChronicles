using UnityEngine;
using System.Collections.Generic;

using static SaveGame;

public static class Coloring
{
    public static string ColorRequiredLevel(int level)
    {
        if (currentSave != null && currentSave.player != null && level > currentSave.player.level) return "DangerousRed";
        else return "Gray";
    }

    public static string ColorProfessionRequiredSkill(string profession, int skill)
    {
        if (currentSave.player.professionSkills.ContainsKey(profession) && skill <= currentSave.player.professionSkills[profession].Item1) return "DarkGray";
        else return "DangerousRed";
    }

    public static string ColorProgress(int progress)
    {
        if (progress == 0) return "DarkGray";
        else if (progress <= 20) return "Red";
        else if (progress <= 40) return "Orange";
        else if (progress <= 60) return "Yellow";
        else return "Green";
    }

    public static string ColorReputation(int progress)
    {
        if (progress >= 8400) return "Exalted";
        else if (progress >= 6300) return "Revered";
        else if (progress >= 5100) return "Honored";
        else if (progress >= 4500) return "Friendly";
        else if (progress >= 4200) return "Neutral";
        else if (progress >= 3900) return "Unfriendly";
        else if (progress >= 3600) return "Hostile";
        else return "Hated";
    }

    public static string ColorQuestLevel(int level)
    {
        if (currentSave == null) return null;
        if (level - 4 > currentSave.player.level) return "Red";
        else if (level - 2 > currentSave.player.level) return "Orange";
        else if (level + 2 < currentSave.player.level && currentSave.player.WillGetExperience(level)) return "Green";
        else if (!currentSave.player.WillGetExperience(level)) return "Gray";
        else return "Yellow";
    }

    public static string ColorEntityLevel(int level)
    {
        if (currentSave == null) return "DarkGray";
        if (level - 4 > currentSave.player.level) return "DangerousRed";
        else if (level - 2 > currentSave.player.level) return "Orange";
        else if (level + 2 < currentSave.player.level && currentSave.player.WillGetExperience(level)) return "Green";
        else if (!currentSave.player.WillGetExperience(level)) return "DarkGray";
        else return "Yellow";
    }

    //Color pallete of the UI
    public static Dictionary<string, Color32> colors = new()
    {
        //Basic UI colors
        { "White",          new Color32(234, 234, 234, 255) },
        { "LightGray",      new Color32(202, 202, 202, 255) },
        { "Gray",           new Color32(183, 183, 183, 255) },
        { "HalfGray",       new Color32(140, 140, 140, 255) },
        { "DarkGray",       new Color32(114, 114, 114, 255) },
        { "DimGray",        new Color32(045, 045, 045, 255) },
        { "Black",          new Color32(031, 031, 031, 255) },
        { "Red",            new Color32(181, 077, 077, 255) },
        { "DangerousRed",   new Color32(173, 036, 045, 255) },
        { "Yellow",         new Color32(181, 159, 077, 255) },
        { "Orange",         new Color32(185, 104, 057, 255) },
        { "LightOrange",    new Color32(168, 110, 038, 255) },
        { "Green",          new Color32(081, 181, 077, 255) },

        //Class colors
        { "Druid",          new Color32(184, 090, 007, 255) },
        { "Warrior",        new Color32(144, 113, 079, 255) },
        { "Rogue",          new Color32(184, 177, 076, 255) },
        { "Hunter",         new Color32(124, 153, 083, 255) },
        { "Mage",           new Color32(045, 144, 170, 255) },
        { "Shaman",         new Color32(000, 081, 160, 255) },
        { "Warlock",        new Color32(097, 098, 172, 255) },
        { "Paladin",        new Color32(177, 101, 134, 255) },
        { "Priest",         new Color32(191, 175, 164, 255) },
        { "Monk",           new Color32(007, 209, 124, 255) },

        //Currency colors
        { "Copper",         new Color32(184, 080, 041, 255) },
        { "Silver",         new Color32(170, 188, 210, 255) },
        { "Gold",           new Color32(255, 210, 011, 255) },

        //Item rarity colors
        { "Poor",           new Color32(114, 114, 114, 255) },
        { "Common",         new Color32(183, 183, 183, 255) },
        { "Uncommon",       new Color32(026, 201, 000, 255) },
        { "Rare",           new Color32(000, 117, 226, 255) },
        { "Epic",           new Color32(163, 053, 238, 255) },
        { "Legendary",      new Color32(221, 110, 000, 255) },

        //Colors of the elements
        { "Fire",           new Color32(227, 099, 050, 255) },
        { "Water",          new Color32(066, 169, 167, 255) },
        { "Earth",          new Color32(128, 094, 068, 255) },
        { "Shadow",         new Color32(169, 063, 219, 255) },
        { "Lightning",      new Color32(063, 158, 245, 255) },
        { "Order",          new Color32(241, 229, 125, 255) },
        { "Frost",          new Color32(068, 198, 229, 255) },
        { "Decay",          new Color32(201, 208, 019, 255) },
        { "Arcane",         new Color32(204, 101, 221, 255) },
        { "Air",            new Color32(175, 190, 202, 255) },

        //Reputation colors
        { "Exalted",        new Color32(055, 105, 221, 255) },
        { "Revered",        new Color32(048, 169, 193, 255) },
        { "Honored",        new Color32(049, 196, 149, 255) },
        { "Friendly",       new Color32(070, 201, 050, 255) },
        { "Neutral",        new Color32(203, 203, 050, 255) },
        { "Unfriendly",     new Color32(201, 098, 050, 255) },
        { "Hostile",        new Color32(196, 061, 049, 255) },
        { "Hated",          new Color32(204, 034, 034, 255) },
    };
}
