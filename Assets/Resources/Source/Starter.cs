using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using static Root;
using static Root.Anchor;

using static Font;
using static Sound;
using static Cursor;
using static Talent;
using static SaveGame;
using static Coloring;
using static Transport;
using static GameSettings;
using static CursorRemote;
using static SiteSpiritHealer;
using static Serialization;
using static SiteInstance;
using static SiteComplex;

public class Starter : MonoBehaviour
{
    void Start()
    {
        random = new System.Random();
        font = new Font("Tahoma Bold");
        desktops = new();
        settings = new GameSettings();
        fallingElements = new List<FallingElement>();
        grid = FindObjectOfType<MapGrid>();
        #if (UNITY_EDITOR)
        prefix = "D:\\Games\\Warcraft Elements\\";
        #endif
        Deserialize(ref saves, "characters", false, prefix);
        if (saves == null) saves = new();
        Deserialize(ref settings, "settings", false, prefix);
        if (settings == null) settings = new();
        else settings.FillNulls();
        if (!saves.Any(x => x.Value.Exists(y => y.player.name == settings.selectedCharacter)))
        {
            settings.selectedCharacter = "";
            settings.selectedRealm = "";
        }
        LoadData();
        cursor = FindObjectOfType<Cursor>();
        cursorEnemy = FindObjectOfType<CursorRemote>();
        ambience = FindObjectsOfType<AudioSource>().First(x => x.name == "Ambience");
        soundEffects = cursor.GetComponent<AudioSource>();
        SpawnDesktopBlueprint("TitleScreen");
        Destroy(gameObject);
    }

    public static void LoadData()
    {
        Deserialize(ref SiteHostileArea.areas, "areas", false, prefix);
        SiteHostileArea.areas ??= new();
        Deserialize(ref instances, "instances", false, prefix);
        instances ??= new();
        Deserialize(ref complexes, "complexes", false, prefix);
        complexes ??= new();
        Deserialize(ref SiteTown.towns, "towns", false, prefix);
        SiteTown.towns ??= new();
        Deserialize(ref Realm.realms, "realms", false, prefix);
        Realm.realms ??= new();
        Deserialize(ref PersonType.personTypes, "persontypes", false, prefix);
        PersonType.personTypes ??= new();
        Deserialize(ref Class.specs, "classes", false, prefix);
        Class.specs ??= new();
        Deserialize(ref Race.races, "races", false, prefix);
        Race.races ??= new();
        Deserialize(ref ItemSet.itemSets, "sets", false, prefix);
        ItemSet.itemSets ??= new();
        Deserialize(ref Item.items, "items", false, prefix);
        Item.items ??= new();
        Deserialize(ref Ability.abilities, "abilities", false, prefix);
        Ability.abilities ??= new();
        Deserialize(ref Buff.buffs, "buffs", false, prefix);
        Buff.buffs ??= new();
        Deserialize(ref Faction.factions, "factions", false, prefix);
        Faction.factions ??= new();
        Deserialize(ref spiritHealers, "spirithealers", false, prefix);
        spiritHealers ??= new();
        #if (UNITY_EDITOR)
        var ambienceList = AssetDatabase.FindAssets("t:AudioClip Ambience", new[] { "Assets/Resources/Ambience/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Ambience/", "")).ToList();
        var soundList = AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets/Resources/Sounds/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sounds/", "")).ToList();
        var itemIconList = AssetDatabase.FindAssets("t:Texture Item", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var abilityIconList = AssetDatabase.FindAssets("t:Texture Ability", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var factionIconList = AssetDatabase.FindAssets("t:Texture Faction", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var portraitList = AssetDatabase.FindAssets("t:Texture Portrait", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        ambienceList.RemoveAll(x => !x.StartsWith("Ambience"));
        itemIconList.RemoveAll(x => !x.StartsWith("Item"));
        abilityIconList.RemoveAll(x => !x.StartsWith("Ability"));
        factionIconList.RemoveAll(x => !x.StartsWith("Faction"));
        portraitList.RemoveAll(x => !x.StartsWith("Portrait"));
        Assets.assets = new Assets()
        {
            ambience = ambienceList,
            sounds = soundList,
            itemIcons = itemIconList,
            abilityIcons = abilityIconList,
            factionIcons = factionIconList,
            portraits = portraitList
        };
        Serialize(Assets.assets, "assets", false, false, prefix);
        #else
        Deserialize(ref Assets.assets, "assets", false, prefix);
        Assets.assets ??= new()
        {
            ambience = new(),
            sounds = new(),
            itemIcons = new(),
            abilityIcons = new(),
            portraits = new()
        };
        #endif
        var countHA = SiteHostileArea.areas.Count;
        var countI = instances.Count;
        var countR = Race.races.Count;
        var countA = Ability.abilities.Count;
        var countIS = ItemSet.itemSets.Count;
        var countF = Faction.factions.Count;
        for (int i = 0; i < SiteTown.towns.Count; i++)
            SiteTown.towns[i].Initialise();
        for (int i = 0; i < complexes.Count; i++)
            complexes[i].Initialise();
        for (int i = 0; i < instances.Count; i++)
            instances[i].Initialise();
        for (int i = 0; i < SiteHostileArea.areas.Count; i++)
            SiteHostileArea.areas[i].Initialise();
        for (int i = 0; i < Class.specs.Count; i++)
            Class.specs[i].Initialise();
        for (int i = 0; i < Item.items.Count; i++)
            Item.items[i].Initialise();
        for (int i = 0; i < ItemSet.itemSets.Count; i++)
            ItemSet.itemSets[i].Initialise();
        for (int i = 0; i < Race.races.Count; i++)
            Race.races[i].Initialise();
        for (int i = 0; i < Realm.realms.Count; i++)
            Realm.realms[i].Initialise();
        for (int i = 0; i < Ability.abilities.Count; i++)
            Ability.abilities[i].Initialise();
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 12; j++)
                for (int k = 0; k < 3; k++)
                {
                    var spec = i; var row = j; var col = k;
                    if (Blueprint.windowBlueprints.Exists(x => x.title == "Talent" + spec + row + col)) continue;
                    Blueprint.windowBlueprints.Add(new Blueprint("Talent" + spec + row + col, () => PrintTalent(spec, row, col)));
                }
        if (countHA != SiteHostileArea.areas.Count) Debug.Log("Added " + (SiteHostileArea.areas.Count - countHA) + " lacking areas.");
        if (countI != instances.Count) Debug.Log("Added " + (instances.Count - countI) + " lacking instances.");
        if (countR != Race.races.Count) Debug.Log("Added " + (Race.races.Count - countR) + " lacking races.");
        if (countA != Ability.abilities.Count) Debug.Log("Added " + (Ability.abilities.Count - countA) + " lacking abilities.");
        if (countIS != ItemSet.itemSets.Count) Debug.Log("Added " + (ItemSet.itemSets.Count - countIS) + " lacking item sets.");
        if (countF != Faction.factions.Count) Debug.Log("Added " + (Faction.factions.Count - countF) + " lacking factions.");
    }
}
