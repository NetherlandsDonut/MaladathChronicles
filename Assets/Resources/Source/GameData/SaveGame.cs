using System;

public class SaveGame
{
    public SaveGame()
    {
        player = new Entity(Race.races.Find(x => x.name == "Night Elf"), Class.classes.Find(x => x.name == "Rogue"), "Gavel");
    }

    public Entity player;
    public DateTime date;
    public bool hardcore;
}
