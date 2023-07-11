using UnityEngine;
using System.Collections.Generic;

using static Root;

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
        settings = new GameSettings();
        saveGames = new List<SaveGame>();
        SpawnDesktopBlueprint("TitleScreen");

        //Serialize(Data.data, "Data");

        Destroy(gameObject);
    }
}
