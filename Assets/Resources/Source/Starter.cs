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
        SpawnDesktopBlueprint("Menu");

        //Serialize(Data.data, "Data");

        Destroy(gameObject);
    }
}
