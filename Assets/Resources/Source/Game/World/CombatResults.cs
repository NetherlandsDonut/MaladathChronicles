using System.Linq;
using System.Collections.Generic;

using static Root;

public class CombatResults
{
    public CombatResults(string result, string zone, int level)
    {
        this.result = result;
        inventory = new(true);
        exclusiveItems = new();
        GenerateMiningNode(zone, level);
        GenerateHerb(zone, level);
    }

    //Result of the combat
    public string result;

    //Amount of experience awarded to the player
    public int experience;

    public (string, int) miningNode;

    public Inventory miningLoot;

    public void GenerateMiningNode(string zone, int level)
    {
        var find = Zone.zones.Find(x => x.name == zone);
        if (find == null || find.miningNodes == null) return;
        var possibleNodes = find.miningNodes.Select(x => GeneralDrop.generalDrops.Find(y => y.category == x && y.tags.Contains("Main"))).ToList();
        possibleNodes.RemoveAll(x => !x.DoesLevelFit(level));
        if (possibleNodes.Count > 0)
        {
            var common = possibleNodes.Where(x => x.tags.Contains("CommonMaterial")).ToList();
            var rare = possibleNodes.Where(x => x.tags.Contains("RareMaterial")).ToList();
            GeneralDrop r = null;
            if (rare.Count > 0 && Roll(possibleNodes.Count == 1 ? 5 : (possibleNodes.Count == 2 ? 4 : (possibleNodes.Count == 3 ? 3 : (possibleNodes.Count == 4 ? 2 : 1))))) r = rare[random.Next(rare.Count)];
            else r = common[random.Next(common.Count)];
            miningNode = (r.category, r.requiredSkill);
        }
        var drops = GeneralDrop.generalDrops.FindAll(x => x.category == miningNode.Item1 && x.DoesLevelFit(level));
        miningLoot = new Inventory(true);
        if (drops.Count > 0)
            foreach (var drop in drops)
                if (Roll(drop.rarity))
                {
                    int amount = 1;
                    for (int i = 1; i < drop.dropCount; i++) amount += Roll(10) ? 1 : 0;
                    miningLoot.AddItem(Item.items.Find(x => x.name == drop.item).CopyItem(amount));
                }
    }

    public (string, int) herb;

    public Inventory herbalismLoot;

    public void GenerateHerb(string zone, int level)
    {
        var find = Zone.zones.Find(x => x.name == zone);
        if (find == null || find.herbs == null) return;
        var possibleNodes = find.herbs.Select(x => GeneralDrop.generalDrops.Find(y => y.category == x && y.tags.Contains("Main"))).ToList();
        possibleNodes.RemoveAll(x => x == null || !x.DoesLevelFit(level));
        if (possibleNodes.Count > 0)
        {
            var common = possibleNodes.Where(x => x.tags.Contains("CommonMaterial")).ToList();
            var rare = possibleNodes.Where(x => x.tags.Contains("RareMaterial")).ToList();
            GeneralDrop r = null;
            if (rare.Count > 0 && Roll(possibleNodes.Count == 1 ? 5 : (possibleNodes.Count == 2 ? 4 : (possibleNodes.Count == 3 ? 3 : (possibleNodes.Count == 4 ? 2 : 1))))) r = rare[random.Next(rare.Count)];
            else r = common[random.Next(common.Count)];
            herb = (r.category, r.requiredSkill);
        }
        var drops = GeneralDrop.generalDrops.FindAll(x => x.category == herb.Item1 && x.DoesLevelFit(level));
        herbalismLoot = new Inventory(true);
        if (drops.Count > 0)
            foreach (var drop in drops)
                if (Roll(drop.rarity))
                {
                    int amount = 1;
                    for (int i = 1; i < drop.dropCount; i++) amount += Roll(10) ? 1 : 0;
                    herbalismLoot.AddItem(Item.items.Find(x => x.name == drop.item).CopyItem(amount));
                }
    }

    //Dropped items from the enemy
    public Inventory inventory;

    //?
    public List<string> exclusiveItems;
}
