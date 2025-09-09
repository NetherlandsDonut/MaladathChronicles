using System.Linq;
using System.Collections.Generic;

using static Root;

public class Recipe
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        reagents ??= new();
        results ??= new();
    }

    //Provides an icon for the recipe in the interface
    public string NameColor()
    {
        if (results.Count == 0) return "Gray";
        else
        {
            var find = Item.items.Find(x => x.name == results.Keys.First());
            if (find != null) return find.rarity;
            return "Gray";
        }
    }

    //Provides an icon for the recipe in the interface
    public (string, Item) Icon()
    {
        if (results.Count == 0)
        {
            if (enchantment) return ("AbilityGreaterHeal", null);
            else return ("OtherUnknown", null);
        }
        else
        {
            var find = Item.items.Find(x => x.name == results.Keys.First());
            if (find != null) return (find.icon, find);
            return ("OtherUnknown", null);
        }
    }

    #region Description

    public static void PrintRecipeTooltip(Entity forWho, Recipe recipe)
    {
        SetAnchor(-92, 142);
        AddHeaderGroup();
        SetRegionGroupWidth(182);
        AddHeaderRegion(() =>
        {
            AddLine(recipe.name, recipe.NameColor());
            var icon = recipe.Icon();
            AddSmallButton(icon.Item1);
            if (GameSettings.settings.rarityIndicators.Value() && icon.Item2 != null)
                AddSmallButtonOverlay("OtherRarity" + icon.Item2.rarity, 0, 2);
        });
        AddPaddingRegion(() => AddLine(recipe.profession + " " + Profession.professions.Find(x => x.name == recipe.profession).recipeType.ToLower(), SaveGame.currentSave != null && SaveGame.currentSave.player.professionSkills.ContainsKey(recipe.profession) ? "HalfGray" : "DangerousRed"));
        if (recipe.results.Count > 0)
        {
            AddHeaderRegion(() => AddLine("Results:", "DarkGray"));
            foreach (var result in recipe.results)
            {
                var resultItem = Item.items.Find(x => x.name == result.Key);
                AddPaddingRegion(() =>
                {
                    SetRegionTextOffset(0, 19);
                    AddLine("x" + result.Value, "DarkGray", "Right");
                    AddLine(result.Key, resultItem.rarity);
                    AddSmallButton(resultItem.icon);
                    if (GameSettings.settings.rarityIndicators.Value())
                        AddSmallButtonOverlay("OtherRarity" + resultItem.rarity, 0, 2);
                });
            }
        }
        else if (recipe.enchantment)
        {
            AddHeaderRegion(() => AddLine("Enchantment:", "DarkGray"));
            var e = Enchant.enchants.Find(x => x.name == recipe.name);
            AddPaddingRegion(() =>
            {
                AddLine(e.type);
                AddLine(e.Name());
            });
        }
        AddHeaderRegion(() => AddLine("Reagents:", "DarkGray"));
        foreach (var reagent in recipe.reagents)
        {
            var reagentItem = Item.items.Find(x => x.name == reagent.Key);
            AddPaddingRegion(() =>
            {
                SetRegionTextOffset(0, 19);
                AddLine("x" + reagent.Value, "DarkGray", "Right");
                AddLine(reagent.Key, reagentItem.rarity);
                AddSmallButton(reagentItem.icon);
                if (GameSettings.settings.rarityIndicators.Value())
                    AddSmallButtonOverlay("OtherRarity" + reagentItem.rarity, 0, 2);
            });
        }
        AddHeaderRegion(() =>
        {
            AddLine("Required skill: ", "DarkGray");
            AddText(recipe.learnedAt + "", forWho.professionSkills.ContainsKey(recipe.profession) && recipe.learnedAt <= forWho.professionSkills[recipe.profession].Item1 ? "HalfGray" : "DangerousRed");
        });
        if (recipe.price > 0) PrintPriceRegion(recipe.price, 38, 38, 49);
    }

    #endregion

    //Name of this recipe
    public string name;

    //Profession this recipe belongs to
    public string profession;

    //Profession specialization this recipe belongs to
    public string specialization;

    //Whether this recipe is a an enchantment process
    public bool enchantment;

    //Thresholds of getting skill ups
    public int skillUpOrange, skillUpYellow, skillUpGreen, skillUpGray;

    //Price of training this recipe at a trainer
    public int price;

    //Indicates at what skill level of profession player can learn this recipe
    public int learnedAt;

    //Items required to fullfil the recipe and the items provided by finishing it
    public Dictionary<string, int> reagents, results;

    //Currently opened recipe
    public static Recipe recipe;

    //EXTERNAL FILE: List containing all recipes in-game
    public static List<Recipe> recipes;

    //List of all filtered recipes by input search
    public static List<Recipe> recipesSearch;
}
