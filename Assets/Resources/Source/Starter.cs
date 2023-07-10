using UnityEngine;

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
        Root.camera = FindObjectOfType<Camera>();
        SpawnDesktopBlueprint("Map");

        //Serialize(Data.data, "Data");

        Destroy(gameObject);
    }
}
