using System;

public class SaveGame
{
    public SaveGame()
    {
        player = new Entity(
            "Gaval",
            Race.races.Find(x => x.name == "Human"),
            Class.classes.Find(x => x.name == "Paladin"),
            new()
            {
                "Scaled Silithid Gauntlets",
                "Carapace Spine Crossbow",
                "Angerforge's Battle Axe",
                "Ribbly's Quiver",
                "Haunting Specter Leggings",
                "Shadefiend Boots",
                "Rubicund Armguards",
                "Carapace of Anub'shiah"
            });
    }

    public Entity player;
    public DateTime date;
    public bool hardcore;
}
