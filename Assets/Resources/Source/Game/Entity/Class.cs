using System.Collections.Generic;

public class Class
{
    public string name, icon;
    public Dictionary<string, List<string>> startingEquipment;
    public Dictionary<string, double> rules;
    public List<(string, int)> abilities;
    public List<TalentTree> talentTrees;

    public static Class spec;
    public static List<Class> specs, specsSearch;
}
