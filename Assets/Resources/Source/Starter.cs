using UnityEngine;
using System.Collections.Generic;

using static Root;
using System.Linq;

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
        var temp = FindObjectsByType<WindowAnchorRemote>(FindObjectsSortMode.None);
        windowRemoteAnchors = temp.Select(x => (x.name, new Vector2(x.transform.position.x, x.transform.position.y))).ToList();
        for (int i = temp.Length - 1; i >= 0; i--)
            Destroy(temp[i].gameObject);
        SpawnDesktopBlueprint("TitleScreen");
        for (int i = 0; i < windowRemoteAnchors.Count; i++)
        {
            var index = i;
            if (!windowRemoteAnchors[index].Item1.Contains(": ")) continue;
            var split = windowRemoteAnchors[index].Item1.Split(": ");
            var name = split[1];
            var type = split[0].Substring(4);
            Blueprint.windowBlueprints.Add(new Blueprint("Site: " + name, () => PrintSite(name, type, windowRemoteAnchors[index].Item2)));
        }
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
