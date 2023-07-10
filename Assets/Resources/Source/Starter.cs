using UnityEngine;
using System.Collections.Generic;

using static Root;
using static Serialization;

public class Starter : MonoBehaviour
{
    void Start()
    {
        random = new System.Random();
        font = new Font("Tahoma Bold");
        desktops = new();
        testText.Set("Aqualung");
        testText2.Set("Baby Yoda");
        cursor = FindObjectOfType<Cursor>();
        Board.board = new Board(8, 8);
        Entity.player = new Entity(Race.races.Find(x => x.name == "Night Elf"), Class.classes.Find(x => x.name == "Rogue"), "Hoolahop");
        settings = new Settings();
        saveGames = new List<SaveGame>();
        SpawnDesktopBlueprint("TitleScreen");

        //Serialize(Data.data, "Data");

        Destroy(gameObject);
    }
}
