using System;

public class SaveGame
{
    public SaveGame()
    {
        player = new Entity(
            "Gaval",
            Race.races.Find(x => x.name == "Human"),
            Class.classes.Find(x => x.name == "Priest"),
            new()
            {

            });
    }

    public Entity player;
    public DateTime date;
    public bool hardcore;
}
