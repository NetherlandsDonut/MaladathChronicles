using System.Linq;
using System.Collections.Generic;

using static Root;

public class CombatResults
{
    public CombatResults(string result, string zone, int areaLevel)
    {
        this.result = result;
        experience = new();
        inventory = new(true);
        exclusiveItems = new();
        GenerateSkinningNode(areaLevel);
        GenerateMiningNode(zone, areaLevel);
        GenerateHerb(zone, areaLevel);
    }

    //Result of the combat
    public string result;

    //Amount of experience awarded to the player
    public Dictionary<Entity, int> experience;

    //Dropped items from the enemy
    public Inventory inventory;

    //List of items that partake in the exclusive list
    //If any of the items from this list is taken, the rest disappear
    public List<string> exclusiveItems;

    #region Skinning

    //Found skinning node in the area after combat (node name, skill required)
    public (string, int) skinningNode;

    //Generated skinning loot based on player skill in skinning and the node found
    public Inventory skinningLoot;

    //Stores whether the player skill was already modified after a successful gathering
    public bool skinningSkillChange;

    //Generates a skinning node and asignes it a generated loot based on the node
    public void GenerateSkinningNode(int areaLevel)
    {
        var skinningTarget = Board.board.participants.Find(x => x.team == 2);
        if (!new List<string> { "Beast", "Dragonkin" }.Contains(skinningTarget.who.Race().category)) return;
        var possibleNodes = GeneralDrop.generalDrops.FindAll(y => y.requiredProfession == "Skinning" && y.tags.Contains("Main")).ToList();
        possibleNodes.RemoveAll(x => !x.DoesLevelFit(areaLevel));
        if (possibleNodes.Count > 0)
        {
            var common = possibleNodes.Where(x => x.tags.Contains("CommonMaterial")).ToList();
            var rare = possibleNodes.Where(x => x.tags.Contains("RareMaterial")).ToList();
            GeneralDrop r = null;
            if (possibleNodes.Any(x => x.category == skinningTarget.who.name)) r = possibleNodes.First(x => x.category == skinningTarget.who.name);
            else if (rare.Count > 0 && Roll(possibleNodes.Count == 1 ? 5 : (possibleNodes.Count == 2 ? 4 : (possibleNodes.Count == 3 ? 3 : (possibleNodes.Count == 4 ? 2 : 1))))) r = rare[random.Next(rare.Count)];
            else r = common[random.Next(common.Count)];
            skinningNode = (r.category, r.requiredSkill);
            var drops = GeneralDrop.generalDrops.FindAll(x => x.category == skinningNode.Item1 && x.DoesLevelFit(areaLevel));
            skinningLoot = new Inventory(true);
            if (drops.Count > 0)
                foreach (var drop in drops)
                    if (Roll(drop.rarity))
                    {
                        int amount = 1;
                        for (int i = 1; i < drop.dropCount; i++) amount += Roll(50) ? 1 : 0;
                        skinningLoot.AddItem(Item.items.Find(x => x.name == drop.item).CopyItem(amount));
                    }
        }
    }

    #endregion

    #region Mining

    //Found mining node in the area after combat (node name, skill required)
    public (string, int) miningNode;

    //Generated mining loot based on player skill in mining and the node found
    public Inventory miningLoot;

    //Stores whether the player skill was already modified after a successful gathering
    public bool miningSkillChange;

    //Generates a mining node and asignes it a generated loot based on the node
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
            var drops = GeneralDrop.generalDrops.FindAll(x => x.category == miningNode.Item1 && x.DoesLevelFit(level));
            miningLoot = new Inventory(true);
            if (drops.Count > 0)
                foreach (var drop in drops)
                    if (Roll(drop.rarity))
                    {
                        int amount = 1;
                        for (int i = 1; i < drop.dropCount; i++) amount += Roll(50) ? 1 : 0;
                        miningLoot.AddItem(Item.items.Find(x => x.name == drop.item).CopyItem(amount));
                    }
        }
    }

    #endregion

    #region Herbalism

    //Found herbalism node in the area after combat (node name, skill required)
    public (string, int) herb;

    //Generated herbalism loot based on player skill in herbalism and the node found
    public Inventory herbalismLoot;

    //Stores whether the player skill was already modified after a successful gathering
    public bool herbalismSkillChange;

    //Generates a herbalism node and asignes it a generated loot based on the node
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
            var drops = GeneralDrop.generalDrops.FindAll(x => x.category == herb.Item1 && x.DoesLevelFit(level));
            herbalismLoot = new Inventory(true);
            if (drops.Count > 0)
                foreach (var drop in drops)
                    if (Roll(drop.rarity))
                    {
                        int amount = 1;
                        for (int i = 1; i < drop.dropCount; i++) amount += Roll(50) ? 1 : 0;
                        herbalismLoot.AddItem(Item.items.Find(x => x.name == drop.item).CopyItem(amount));
                    }
        }
    }

    #endregion
}
