using System;

public class SaveGame
{
    public SaveGame()
    {
        player = new Entity(Race.races.Find(x => x.name == "Forsaken"));
    }

    public Entity player;
    public DateTime date;
    public bool hardcore;
}
