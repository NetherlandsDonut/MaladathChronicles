using UnityEngine;

using System.Linq;
using System.Collections.Generic;

using static Root;

public static class Extensions
{
    //Shuffles a list
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count, rnd = random.Next(i--); i >= 1; rnd = random.Next(i--))
            (list[i], list[rnd]) = (list[rnd], list[i]);
    }

    public static T SafeLast<T>(this IList<T> list) => list.Count != 0 ? list.Last() : default;

    //Increases a key by a specified amount and automatically adds it if it was not present
    public static void Inc<TKey>(this Dictionary<TKey, int> dic, TKey source, int amount = 1)
    {
        if (dic.ContainsKey(source)) dic[source] += amount;
        else dic.Add(source, amount);
    }

    //Removes all nasty characters from a string (Usually used for accessing files with names based of something)
    public static string Clean(this string text)
    {
        return text?.Replace("'", "").Replace(".", "").Replace(" ", "");
    }

    //Cuts the tail of a string to be displayed without the suffix
    public static string CutTail(this string text)
    {
        return text.Contains("!") ? text[..text.LastIndexOf("!")] : text;
    }

    //Removes all nasty characters from a string (Usually used for accessing files with names based of something)
    public static string ToFirstUpper(this string text)
    {
        return text == null ? text : text[..1].ToUpper() + text[1..].ToLower();
    }

    //Removes all nasty characters from a string (Usually used for accessing files with names based of something)
    public static string OnlyNameCategory(this string text)
    {
        var a = text != null && text.Contains(" @ ") ? text[..text.IndexOf(" @ ")] : "";
        return text != null && text.Contains(" @ ") ? text[..text.IndexOf(" @ ")] : text;
    }

    //Removes all nasty characters from a string (Usually used for accessing files with names based of something)
    public static string OnlyGeneralCategory(this string text)
    {
        var a = text != null && text.Contains(" @ ") ? text[text.IndexOf(" @ ")..] : "";
        return text != null && text.Contains(" @ ") ? text[text.IndexOf(" @ ")..] : text;
    }

    //Returns a grayscale from a color
    public static float Grayscale(this Color32 color)
    {
        return (0.299f * color.r) + (0.587f * color.g) + (0.114f * color.b);
    }

    public static List<(string, string, string)> TrimLast(this List<(string, string, string)> list, bool should)
    {
        if (should == false) return list;
        list[^1] = (list[^1].Item1.TrimEnd(), list[^1].Item2, list[^1].Item3);
        return list;
    }

    public static List<Item> Multilate(this List<Item> list, int times)
    {
        var output = list.ToList();
        for (int i = 0; i < times - 1; i++)
            output.AddRange(list.Select(x => x.CopyItem(x.amount)));
        return output;
    }

    public static Dictionary<T, U> Merge<T, U>(this Dictionary<T, U> A, Dictionary<T, U> B)
    {
        var temp = A.ToDictionary(x => x.Key, x => x.Value);
        foreach (var pair in B)
            if (temp.ContainsKey(pair.Key)) continue;
            else temp.Add(pair.Key, pair.Value);
        return temp;
    }

    //Gets a value from a dictionary, if it does not have the
    //key it returns the default value of the type
    public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey source)
    {
        if (dic.ContainsKey(source)) return dic[source];
        else return default;
    }

    //Converts a number into the roman notation
    public static string ToRoman(this int number)
    {
        if (number < 0 || number > 3999) return "";
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        return "";
    }
}
