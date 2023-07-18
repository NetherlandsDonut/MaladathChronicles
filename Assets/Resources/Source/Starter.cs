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
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 12; j++)
                for (int k = 0; k < 3; k++)
                {
                    var spec = i; var row = j; var col = k;
                    Blueprint.windowBlueprints.Add(new Blueprint("Talent" + spec + row + col, () => PrintTalent(spec, row, col)));
                }

        //Serialize(Data.data, "Data");

        Destroy(gameObject);
    }
}
