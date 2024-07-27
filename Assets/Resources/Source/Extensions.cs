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

   //Removes all nasty characters from a string (Usually used for accessing files with names based of something)
   public static string ToFirstUpper(this string text)
   {
      return text == null ? text : text[..1].ToUpper() + text[1..].ToLower();
   }

   //Removes all nasty characters from a string (Usually used for accessing files with names based of something)
   public static string OnlyNameCategory(this string text)
   {
      return text != null && text.Contains(" @ ") ? text[..text.IndexOf(" @ ")] : text;
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

   public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey source)
   {
      if (dic.ContainsKey(source)) return dic[source];
      else return default;
   }
}
