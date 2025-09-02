using System.Linq;
using UnityEngine;

using static Root;
using static SaveGame;

public class Chest
{
    //Site where this chest was opened at
    public string area;

    //Insides of the chest
    public Inventory inventory;

    //Generate chest
    public static Chest GenerateChest(SiteArea area)
    {
        var chest = new Chest
        {
            area = area.name,
            inventory = new Inventory(true)
        };
        var worldDrop = Item.items.FindAll(x => x.lvl >= area.recommendedLevel[currentSave.playerSide] - 6 && x.lvl <= area.recommendedLevel[currentSave.playerSide] && x.source == "RareDrop");
        var zone = Zone.zones.Find(x => x.name == area.zone);
        var zoneDrop = zone == null || zone.zoneDrops == null ? new() : Item.items.FindAll(x => zone.zoneDrops.Contains(x.name));
        var everything = zoneDrop.Concat(worldDrop).Where(x => x.CanEquip(currentSave.player, false, false) && (!x.unique || !currentSave.player.uniquesGotten.Contains(x.name)));
        var dropGray = everything.Where(x => x.rarity == "Poor").ToList();
        var dropWhite = everything.Where(x => x.rarity == "Common").ToList();
        var dropGreen = everything.Where(x => x.rarity == "Uncommon").ToList();
        var dropBlue = everything.Where(x => x.rarity == "Rare").ToList();
        var dropPurple = everything.Where(x => x.rarity == "Epic").ToList();
        if (dropPurple.Count > 0 && Roll(0.1)) chest.inventory.AddItem(dropPurple[random.Next(dropPurple.Count)].CopyItem());
        else if (dropBlue.Count > 0 && Roll(5)) chest.inventory.AddItem(dropBlue[random.Next(dropBlue.Count)].CopyItem());
        else if (dropGreen.Count > 0) chest.inventory.AddItem(dropGreen[random.Next(dropGreen.Count)].CopyItem());
        else if (dropWhite.Count > 0) chest.inventory.AddItem(dropWhite[random.Next(dropWhite.Count)].CopyItem());
        var dropsWithinLevelRange = GeneralDrop.generalDrops.FindAll(x => (x.tags == null || !x.tags.Contains("OnlySkinning")) && x.dropStart <= area.recommendedLevel[currentSave.playerSide] && x.dropEnd >= area.recommendedLevel[currentSave.playerSide]);
        if (dropsWithinLevelRange.Count > 0)
        {
            dropsWithinLevelRange.Shuffle();
            var find = dropsWithinLevelRange.Find(x => x.tags != null && x.tags.Contains("CommonMaterial"));
            if (find != null) chest.inventory.AddItem(Item.items.Find(x => x.name == find.item).CopyItem(random.Next(1, find.dropCount + 1)));
            if (Roll(50))
            {
                find = dropsWithinLevelRange.Find(x => x.tags != null && x.tags.Contains("RareMaterial"));
                if (find != null) chest.inventory.AddItem(Item.items.Find(x => x.name == find.item).CopyItem(random.Next(1, find.dropCount + 1)));
            }
            else
            {
                find = dropsWithinLevelRange.FindLast(x => x.tags != null && x.tags.Contains("CommonMaterial"));
                if (find != null) chest.inventory.AddItem(Item.items.Find(x => x.name == find.item).CopyItem(random.Next(1, find.dropCount + 1)));
            }
            if (Roll(25))
            {
                find = dropsWithinLevelRange.Find(x => x.tags != null && x.tags.Contains("Potion"));
                if (find != null) chest.inventory.AddItem(Item.items.Find(x => x.name == find.item).CopyItem(random.Next(1, find.dropCount + 1)));
            }
            else if (Roll(25))
            {
                find = dropsWithinLevelRange.Find(x => x.tags != null && x.tags.Contains("Battle Elixir"));
                if (find != null) chest.inventory.AddItem(Item.items.Find(x => x.name == find.item).CopyItem(random.Next(1, find.dropCount + 1)));
            }
            else
            {
                find = dropsWithinLevelRange.Find(x => x.tags != null && x.tags.Contains("Scroll"));
                if (find != null) chest.inventory.AddItem(Item.items.Find(x => x.name == find.item).CopyItem(random.Next(1, find.dropCount + 1)));
            }
        }
        if (area.chestBonus != null)
            foreach (var item in area.chestBonus.Select(x => Item.items.Find(y => y.name == x.Key).CopyItem(x.Value)).Where(x => !x.unique || !currentSave.player.uniquesGotten.Contains(x.name)).Where(x => x.specDropRestriction == null || x.specDropRestriction.Contains(currentSave.player.spec)).Where(x => x.raceDropRestriction == null || x.raceDropRestriction.Contains(currentSave.player.race)))
                chest.inventory.AddItem(item);
        chest.inventory.items.ForEach(x => x.SetRandomEnchantment());
        foreach (var item in chest.inventory.items)
            if (item.unique && !currentSave.player.uniquesGotten.Contains(item.name))
                currentSave.player.uniquesGotten.Add(item.name);
        return chest;
    }

    //Spawns a clickable chest object that leads the user to the chest loot screen
    public static GameObject SpawnChestObject(Vector3 position)
    {
        var zone = Zone.zones.Find(x => x.name == SiteArea.area.zone);
        var chestID = SiteArea.area.chestVariant != 0 ? SiteArea.area.chestVariant : (zone.chestVariant != 0 ? zone.chestVariant : 6);
        var chest = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabChest" + chestID));
        chest.transform.parent = CDesktop.LBWindow().transform;
        chest.transform.position = position;
        chest.GetComponent<Highlightable>().Initialise(null,
            (h) =>
            {
                if (!currentSave.openedChests.ContainsKey(SiteArea.area.name))
                    currentSave.openedChests.Add(SiteArea.area.name, GenerateChest(SiteArea.area));
                Sound.PlaySound("DesktopChest" + chestID + "Open");
                SpawnDesktopBlueprint("ChestLoot");
            },
            null, null, null
        );
        return chest;
    }
}
