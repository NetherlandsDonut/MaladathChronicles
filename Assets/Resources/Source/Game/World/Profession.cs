using System;
using System.Linq;
using System.Collections.Generic;

using static Root;

public class Profession
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        maxLevel = levels.Max(x => x.maxSkill);
    }

    #region Description

    public static void PrintProfessionLevelTooltip(Entity forWho, Profession profession, ProfessionLevel level)
    {
        SetAnchor(-92, 142);
        AddHeaderGroup();
        SetRegionGroupWidth(182);
        AddHeaderRegion(() => AddLine(level.name, "Gray"));
        AddPaddingRegion(() =>
        {
            AddBigButton(profession.icon);
            if (forWho.level < level.requiredLevel || level.requiredSkill > 0 && !forWho.professionSkills.ContainsKey(profession.name) || level.requiredSkill > 0 && forWho.professionSkills[profession.name].Item1 < level.requiredSkill) { SetBigButtonToRed(); AddBigButtonOverlay("OtherGridBlurred"); }
            AddLine("Required level: ", "DarkGray");
            AddText(level.requiredLevel + "", Coloring.ColorRequiredLevel(level.requiredLevel));
            if (level.requiredSkill > 0)
            {
                AddLine("Required skill: ", "DarkGray");
                AddText(level.requiredSkill + "", !forWho.professionSkills.ContainsKey(profession.name) || forWho.professionSkills[profession.name].Item1 < level.requiredSkill ? "DangerousRed" : "HalfGray");
            }
        });
        PrintPriceRegion(level.price);
    }

    #endregion

    //Profession name
    public string name;

    //Icon of the profession skill
    public string icon;

    //Keyword of reference when referencing recipes for this profession
    //When not specified, player cannot learn recipes directly from trainers
    public string recipeType;

    //Indicates whether this profession is a primary one
    public bool primary;

    //List of recipes player learns with the profession at start
    public List<string> defaultRecipes;

    //All possible learnable levels of the profession
    public List<ProfessionLevel> levels;

    //Max level of the professions skill
    [NonSerialized] public int maxLevel;

    //Currently opened profession
    public static Profession profession;

    //EXTERNAL FILE: List containing all professions in-game
    public static List<Profession> professions;

    //List of all filtered professions by input search
    public static List<Profession> professionsSearch;
}

public class ProfessionLevel
{
    //Name of the level
    public string name;

    //Cost of training this level at a trainer
    public int price;

    //Minimum required level of player to learn this level
    public int requiredLevel;

    //Minimum required skill in the profession needed to learn this level
    public int requiredSkill;

    //Maximum possible skill to have in the profession with this level learned
    public int maxSkill;
}