using System;
using System.Linq;
using System.Collections.Generic;

public class Profession
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        maxLevel = levels.Max(x => x.maxSkill);
    }

    //Profession name
    public string name;

    //Icon of the profession skill
    public string icon;

    //Keyword of reference when referencing recipes for this profession
    //When not specified, player cannot learn recipes directly from trainers
    public string recipeType;

    //All possible learnable levels of the profession
    public List<ProfessionLevel> levels;

    //Max level of the professions skill
    [NonSerialized] public int maxLevel;

    //Currently opened profession
    public static Race profession;

    //EXTERNAL FILE: List containing all professions in-game
    public static List<Profession> professions;

    //List of all filtered professions by input search
    public static List<Profession> professionsSearch;
}

public class ProfessionLevel
{
    //Name of the level
    public string levelName;

    //Cost of training this level at a trainer
    public int trainingCost;

    //Minimum required level of player to learn this level
    public int requiredLevel;

    //Minimum required skill in the profession needed to learn this level
    public int requiredSkill;

    //Maximum possible skill to have in the profession with this level learned
    public int maxSkill;
}