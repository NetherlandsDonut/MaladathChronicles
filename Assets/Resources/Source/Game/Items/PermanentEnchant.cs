using System;
using System.Collections.Generic;

public class PermanentEnchant
{
   //Text that will appear in the item name with this enchantment
   public string suffix;

   //Stats provided by this enchantment
   //Format ranging from * to *****
   public Dictionary<string, string> stats;

   //Variables providing information on which items this enchantment can be found
   public List<string> commonlyOn, rarelyOn;

   //Currently opened permanent enchant
   public static PermanentEnchant pEnchant;

   //EXTERNAL FILE: List containing all permanent enchants in-game
   public static List<PermanentEnchant> pEnchants;

   //List of all filtered permanent enchants by input search
   public static List<PermanentEnchant> pEnchantsSearch;

   //Calculates how much stats does a permanent enchant grant
   public static int EnchantmentStatGrowth(int ilvl, int amount)
   {
      return (int)Math.Ceiling(1 / 500.0f * (ilvl * ilvl) * (amount > 1 ? (amount > 5 ? 4 : amount - 1) : 0.2f) + 2);
   }
}
