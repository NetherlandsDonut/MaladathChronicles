using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Font;

public class Description
{    
    public void Print(Entity effector, int width, Dictionary<string, string> variables, bool extend = true)
    {
        if (extend && regions.Count(x => x.isExtender) != 1)
        {
            regions.ForEach(x => x.isExtender = false);
            regions.Last().isExtender = true;
        }
        foreach (var region in regions)
            region.PrintRegion(effector, width, variables);
    }

    public List<DescriptionRegion> regions;
}

public class DescriptionRegion
{
    public string regionType;
    public bool isExtender;
    public List<Dictionary<string, string>> contents;

    public void PrintRegion(Entity effector, int width, Dictionary<string, string> variables)
    {
        if (regionType == "Header")
            AddHeaderRegion(() => PrintContents(effector, width, variables));
        else if (regionType == "Padding")
            AddPaddingRegion(() => PrintContents(effector, width, variables));
    }

    public void PrintContents(Entity effector, int width, Dictionary<string, string> variables)
    {
        var li = contents.Select(x => (Process(x.ContainsKey("Text") ? x["Text"] : ""), x.ContainsKey("Color") ? x["Color"] : "", x.ContainsKey("Split") ? x["Split"] : "Yes", x.ContainsKey("Spacing") ? x["Spacing"] : "Yes", x.ContainsKey("Align") ? x["Align"] : "Left")).Where(x => x.Item1 != null && x.Item1.Length > 0);
        var lis = li.SelectMany(x => x.Item3 == "No" ? new() { (x.Item1 + (x.Item4 == "No" ? "" : " "), x.Item2, x.Item5) } : x.Item1.Split(" ").Select(y => (y + " ", x.Item2, x.Item5)).ToList().TrimLast(x.Item4 == "No"));
        var list = lis.Select(x => (x.Item1, x.Item2, fonts["Tahoma Bold"].Length(x.Item1), x.Item3)).ToList();
        if (isExtender) SetRegionAsGroupExtender();
        var sum = width;
        while (list.Count > 0)
        {
            if (sum + list[0].Item3 >= width - 25)
            {
                sum = 0;
                AddLine("", "", list[0].Item4);
            }
            AddText(list[0].Item1, list[0].Item2);
            sum += list[0].Item3;
            list.RemoveAt(0);
        }

        string Process(string text)
        {
            int startIndex, endIndex;
            string toReplace = "";
            if (text == null) return "";
            if (Fn("PowerRange"))
            {
                if (effector == null) return "? - ?";
                var split = toReplace.Split("(").Last().Split(",").Select(x => x.Trim().Replace(")", "")).ToArray();
                if (split.Length == 4)
                    if (double.TryParse(split[2].StartsWith("#") ? variables[split[2]].Replace(".", ",") : split[2].Replace(".", ","), out double powerScale))
                        if (int.TryParse(split[3], out int multiplier))
                        {
                            var weaponPower = effector.WeaponDamage(split[1]);
                            var scaler = split[1] == "Melee" ? effector.MeleeAttackPower() : (split[1] == "Spell" ? effector.SpellPower() : (split[1] == "Ranged" ? effector.RangedAttackPower() : 1));
                            if (!Input.GetKey(KeyCode.LeftControl)) text = text.Replace(toReplace, Math.Ceiling(weaponPower.Item1 * scaler * powerScale * multiplier) + " - " + Math.Ceiling(weaponPower.Item2 * scaler * powerScale * multiplier));
                            else text = text.Replace(toReplace, powerScale * 100 + (split[1] == "Melee" ? "% of MAP" : (split[1] == "Spell" ? "% of SP" : (split[1] == "Ranged" ? "% of RAP" : ""))));
                        }
            }
            if (Fn("Chance"))
            {
                if (effector == null) return "?";
                var split = toReplace.Split("(").Last().Split(",").Select(x => x.Trim().Replace(")", "")).ToArray();
                if (split.Length == 3)
                    if (double.TryParse(split[2].Replace(".", ","), out double multiplier))
                    {
                        var stats = effector.Stats();
                        var stat = stats.ContainsKey(split[1]) ? stats[split[1]] : 1;
                        text = text.Replace(toReplace, stat * multiplier + "%");
                    }
            }
            if (Fn("Hearthstone"))
            {
                if (effector == null) return "?";
                text = text.Replace(toReplace, effector.homeLocation);
            }
            if (Fn("Math"))
            {
                var split = toReplace.Split("(").Last().Split(",").Select(x => x.Trim().Replace(")", "")).ToArray();
                if (split.Length == 3)
                    if (double.TryParse(split[0].StartsWith("#") ? variables[split[0]].Replace(".", ",") : split[0].Replace(".", ","), out double first))
                        if (double.TryParse(split[2].StartsWith("#") ? variables[split[2]].Replace(".", ",") : split[2].Replace(".", ","), out double second))
                            text = text.Replace(toReplace, (split[1] == "add" ? first + second : (split[1] == "sub" ? first - second : (split[1] == "div" ? first / second : (split[1] == "mul" ? first * second : first)))) + "");
            }
            if (Fn("v"))
            {
                var split = toReplace.Split("(").Last().Split(",").Select(x => x.Trim().Replace(")", "")).ToArray();
                if (split.Length == 1)
                    if (double.TryParse(split[0].StartsWith("#") ? variables[split[0]].Replace(".", ",") : split[0].Replace(".", ","), out double content))
                        text = text.Replace(toReplace, content + "");
                    else if (split[0].StartsWith("#") && variables.ContainsKey(split[0]))
                        text = text.Replace(toReplace, variables[split[0]]);
            }
            if (text.Contains("$")) return FnDollar(text);
            return text;

            bool Fn(string functionName)
            {
                if (text.Contains(functionName + "("))
                {
                    startIndex = text.IndexOf(functionName + "(");
                    endIndex = text[startIndex..].IndexOf(")") + startIndex + 1;
                    toReplace = text[startIndex..endIndex];
                    return true;
                }
                return false;
            }

            string FnDollar(string text)
            {
                var split = text.Split(" ");
                var output = "";
                foreach (var splitty in split)
                {
                    if (splitty.Contains("$C"))
                    {
                        if (effector == null) output += "?";
                        output += splitty.Replace("$C", effector.spec.ToFirstUpper()) + " ";
                    }
                    else if (splitty.Contains("$N"))
                    {
                        if (effector == null) output += "?";
                        output += splitty.Replace("$N", effector.name) + " ";
                    }
                    else if (splitty.Contains("$R"))
                    {
                        if (effector == null) output += "?";
                        output += splitty.Replace("$R", effector.race.ToLower()) + " ";
                    }
                    else if (splitty.Contains("$G"))
                    {
                        if (effector == null) output += "?";
                        var temp1 = text[(text.IndexOf("$") + 2)..].Split(";");
                        var temp2 = temp1[0].Split(":");
                        output += ((effector.gender == "Male" ? temp2[0] : temp2[1]) + temp1[1].Split(' ')[0] + " ").Replace("_", " ");
                    }
                    else output += splitty + " ";
                }
                return output.TrimEnd();
            }
        }
    }
}