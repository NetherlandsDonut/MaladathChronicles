using System;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Font;

public class Description
{
    public void Print(Entity effector, Entity other, int width, Dictionary<string, string> variables)
    {
        if (regions.Count(x => x.isExtender) != 1)
        {
            regions.ForEach(x => x.isExtender = false);
            regions.Last().isExtender = true;
        }
        foreach (var region in regions)
            region.PrintRegion(effector, other, width, variables);
    }

    public List<DescriptionRegion> regions;
}

public class DescriptionRegion
{
    public string regionType;
    public bool isExtender;
    public List<Dictionary<string, string>> contents;

    public void PrintRegion(Entity effector, Entity other, int width, Dictionary<string, string> variables)
    {
        if (regionType == "Header")
            AddHeaderRegion(() => PrintContents(effector, other, width, variables));
        else if (regionType == "Padding")
            AddPaddingRegion(() => PrintContents(effector, other, width, variables));
    }

    public void PrintContents(Entity effector, Entity other, int width, Dictionary<string, string> variables)
    {
        var list = contents.Select(x => (Process(x["Text"]), x["Color"], x.ContainsKey("Split") ? x["Split"] : "Yes")).SelectMany(x => x.Item3 == "No" ? new() { (x.Item1 + " ", x.Item2) } : x.Item1.Split(" ").Select(y => (y + " ", x.Item2)).ToList()).Select(x => (x.Item1, x.Item2, font.Length(x.Item1))).ToList();
        if (isExtender) SetRegionAsGroupExtender();
        var sum = width;
        while (list.Count > 0)
        {
            if (sum + list[0].Item3 >= width - 15)
            {
                sum = 0;
                AddLine();
            }
            AddText(list[0].Item1, list[0].Item2);
            sum += list[0].Item3;
            list.RemoveAt(0);
        }

        string Process(string text)
        {
            if (text.StartsWith("PowerRange("))
            {
                if (effector == null) return "? - ?";
                var split = text.Split("(").Last().Split(",").Select(x => x.Trim().Replace(")", "")).ToArray();
                if (split.Length == 4)
                    if (double.TryParse(split[2].StartsWith("#") ? variables[split[2]].Replace(".", ",") : split[2].Replace(".", ","), out double powerScale))
                        if (int.TryParse(split[3], out int multiplier))
                        {
                            var source = split[0] == "Effector" ? effector : other;
                            var weaponPower = source.WeaponDamage();
                            var scaler = (split[1] == "Melee" ? source.MeleeAttackPower() : (split[1] == "Spell" ? source.SpellPower() : (split[1] == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1;
                            return Math.Ceiling(weaponPower.Item1 * scaler * powerScale * multiplier) + " - " + Math.Ceiling(weaponPower.Item2 * scaler * powerScale * multiplier);
                        }
            }
            else if (text.StartsWith("Chance("))
            {
                if (effector == null) return "?";
                var split = text.Split("(").Last().Split(",").Select(x => x.Trim().Replace(")", "")).ToArray();
                if (split.Length == 3)
                    if (double.TryParse(split[2].Replace(".", ","), out double multiplier))
                    {
                        var source = split[0] == "Effector" ? effector : other;
                        var stats = source.Stats();
                        var stat = stats.ContainsKey(split[1]) ? stats[split[1]] : 1;
                        return stat * multiplier + "%";
                    }
            }
            return text;
        }
    }
}