using System;
using System.Diagnostics;

public class SaveGame
{
    public SaveGame()
    {
        player = new Entity(
            "Gaval",
            Race.races.Find(x => x.name == "Human"),
            Class.classes.Find(x => x.name == "Mage"),
            new()
            {

            });
        UnityEngine.Debug.Log(player.ExperienceNeededOverall());
    }

    public Entity player;
    public DateTime date;
    public bool hardcore;
}
