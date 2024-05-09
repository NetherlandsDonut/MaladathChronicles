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
    public static Chest GenerateChest(SiteHostileArea area)
    {
        var chest = new Chest
        {
            area = area.name,
            inventory = new Inventory(true)
        };
        var worldDrop = Item.items.FindAll(x => (x.dropRange == null && x.lvl >= area.recommendedLevel - 6 && x.lvl <= area.recommendedLevel || x.dropRange != null && area.recommendedLevel >= int.Parse(x.dropRange.Split('-')[0]) && area.recommendedLevel <= int.Parse(x.dropRange.Split('-')[1])) && x.source == "Rare Drop");
        var instance = area.instancePart ? SiteInstance.instances.Find(x => x.wings.Any(y => y.areas.Any(z => z["AreaName"] == area.name))) : null;
        var zoneDrop = instance == null || instance.zoneDrop == null ? new() : Item.items.FindAll(x => instance.zoneDrop.Contains(x.name));
        var everything = zoneDrop.Concat(worldDrop).Where(x => x.CanEquip(currentSave.player));
        var dropGray = everything.Where(x => x.rarity == "Poor").ToList();
        var dropWhite = everything.Where(x => x.rarity == "Common").ToList();
        var dropGreen = everything.Where(x => x.rarity == "Uncommon").ToList();
        var dropBlue = everything.Where(x => x.rarity == "Rare").ToList();
        var dropPurple = everything.Where(x => x.rarity == "Epic").ToList();
        if (dropPurple.Count > 0 && Roll(0.1))
            chest.inventory.AddItem(dropPurple[random.Next(dropPurple.Count)].CopyItem());
        else if (dropBlue.Count > 0 && Roll(5))
            chest.inventory.AddItem(dropBlue[random.Next(dropBlue.Count)].CopyItem());
        else if (dropGreen.Count > 0)
            chest.inventory.AddItem(dropGreen[random.Next(dropGreen.Count)].CopyItem());
        else if (dropWhite.Count > 0)
            chest.inventory.AddItem(dropWhite[random.Next(dropWhite.Count)].CopyItem());
        chest.inventory.items.ForEach(x => x.SetRandomEnchantment());
        return chest;
    }

    public static GameObject SpawnChestObject(Vector3 position, string chestTexture)
    {
        var chest = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabChest"));
        chest.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Textures/Chests/" + chestTexture);
        chest.transform.parent = CDesktop.LBWindow.transform;
        chest.transform.position = position;
        chest.GetComponent<Highlightable>().Initialise(null,
            (h) =>
            {
                if (currentSave.lastChest == null || currentSave.lastChest.area != currentSave.currentSite)
                    currentSave.lastChest = GenerateChest(SiteHostileArea.areas.Find(x => x.name == currentSave.currentSite));
                Sound.PlaySound("DesktopOpenChest");
                SpawnDesktopBlueprint("ChestLoot");
            },
            null, null, null
        );
        return chest;
    }
}
