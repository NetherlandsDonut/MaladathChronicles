using System.Linq;
using System.Collections.Generic;

using static Root;
using static SaveSlot;
using static GameSettings;

public class Realm
{
    public string name;
    public bool hardcore, pvp;

    public void PrintRealm()
    {
        if (name == settings.selectedRealm)
            AddHeaderRegion(() =>
            {
                AddLine(name);
            });
        else
            AddButtonRegion(() =>
            {
                AddLine(name);
            },
            (h) =>
            {
                settings.selectedRealm = name;
                h.window.Respawn();
                Respawn("CharacterRoster");
            });
        AddPaddingRegion(() =>
        {
            AddLine("");
            AddText(hardcore ? "Hardcore " : "", hardcore ? "Red" : "Orange");
            AddText(pvp ? "PVP" : "PVE", pvp ? "Red" : "Orange");
        });
        AddPaddingRegion(() =>
        {
            AddLine(slots.Count(x => x.realm == name) + "/7 characters", "DarkGray");
        });
    }

    public static List<Realm> realms;
}
