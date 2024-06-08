using System.Linq;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using static Root;
using static Zone;
using static Font;
using static Quest;
using static Sound;
using static Cursor;
using static Talent;
using static Defines;
using static Enchant;
using static SaveGame;
using static SitePath;
using static GameSettings;
using static CursorRemote;
using static SiteSpiritHealer;
using static PermanentEnchant;
using static FlightPathGroup;
using static Serialization;
using static SiteInstance;
using static SiteComplex;

using static Root.Anchor;

public class Starter : MonoBehaviour
{
    void Start()
    {
        //Sets the initial values for the base variables
        //of the program such as list of desktops or cursor handle
        #region Initial Variables

        //This variable stores random number generator for
        //things such as damage / heal rolls or chance for effects to happen
        random = new System.Random();

        //Initialise storage for pagination
        staticPagination = new();

        //This is the font that will be used
        //by the game's UI system and is the basis of the program
        fonts = new()
        {
            { "Tahoma Bold", new Font("Tahoma Bold", "!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~¡¢£¤¥¦§¨©ª«¬®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷ø ùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſƀƁƂƃƄƅƆƇƈƊƋƌƍƎƏƐƑƒƓƔƕƖƗƘƙƚƛƜƝƞƟƠơƢƣƤƥƦƧƨƩƪƫƬƭƮƯưƱƲƳƴƵƶƷƸƹƺƻƼƽƾƿǀǁǂǃǄǅǆǇǈǉǊǋǌǍǎǏǐǑǒǓǔǕǖǗǘǙǚǛǜǝǞǟǠǡǢǣǤǥǦǧǨǩǪǫǬǭǮǯǰǱǲǳǴǵǶǷǸǹǺǻǼǽǾǿȀȁȂȃȄȅȆȇȈȉȊȋȌȍȎȏȐȑȒȓȔȕȖȗȘșȚțȜȝȞȟȠȡȢȣȤȥȦȧȨȩȪȫȬȭȮȯȰȱȲȳȴȵȶȷȸȹȺȻȼȽȾȿɀɁɂɃɄɅɆɇɈɉɊɋɌɍɎɏɐɑɒɓɔɕɖɗɘəɚɛɜɝɞɟɠɡɢɣɤɥɦɧɨɩɪɫɬɭɮɯɰɱɲɳɴɵɶɷɸɹɺɻɼɽɾɿʀʁʂʃʄʅʆʇʈʉʊʋʌʍʎʏʐʑʒʓʔʕʖʗʘʙʚʛʜʝʞʟʠʡʢʬʭʮʯʰˆˇˉ˘˙˚˛˜˝;΄΅Ά·ΈΉΊΌΎΏΐΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθικλμνξοπρςστυφχψωϊϋόύώЀЁЂЃЄЅІЇЈЉЊЋЌЍЎЏАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюяѐёђѓєѕіїјљњћќѝўџҐґҒғҖҗҚқҜҝҢңҮүҰұҲҳҸҹҺһӘә׃אבגדהוזחטיךכלםמןנסעףפץצקרשתװױײ׳״´῾‌‍‎‏–—―‗‘’‚‛“”„†‡•…‰′″‹›‼‾⁄ⁿ₣₤₥₦₧₨₩₪₫€₭₮₯℅ℓ№™Ω℮⅛⅜⅝⅞∂∆∏∑−∕∙√∞∫≈≠≤≥□▪▫◊●◦ﬁﬂ") },
            { "Alagard", new Font("Alagard", "!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ ") },
            { "Skurri", new Font("Skurri", "0123456789") }
        };

        //List of active desktops
        //Thanks to this list the user can switch between screens while 
        //retaining their data as the desktops are disabled only temporarily
        //unless you call CloseDesktop() which will remove it from the list and wipe it's data
        desktops = new();

        //List of in-game settings that describe visual style of the game,
        //control how the audio works and other things for personalisation and user info storage
        settings = new GameSettings();

        //List of animated falling elements on the board
        //This list needs to be empty for user to be able to perform actions
        //Every time elements on the board are collected new ones will fall
        //from the top to fill the board. When they begin their fall they are
        //added to this list and after landing they are removed.
        fallingElements = new List<FallingElement>();

        //This object contains data on the map grid on which the camera
        //will be moving around. The map grid size is set to the size of icons
        //of sites on it so in this example game it's 19px
        grid = FindAnyObjectByType<MapGrid>();

        //This is the player cursor that follows the hidden system cursor
        cursor = FindAnyObjectByType<Cursor>();

        //This is the enemy cursor that indicates what actions the enemy is performing in their turn
        cursorEnemy = FindAnyObjectByType<CursorRemote>();

        //This is audio source for all quick and single sound effects.
        //Audio played through this medium cannot be stopped or changed in volume
        soundEffects = cursor.GetComponent<AudioSource>();

        //This is audio source for all music or ambience played in the background.
        //Tracks played through this medium are looped and can be changed in volume during playing
        //Whenever a new track is ought to be played throught these means it is first queued.
        //Queued track will force the current one to be smoothly silenced.
        //After that the queued track will starting playing again, smoothly increased in volume
        ambience = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None).First(x => x.name == "Ambience");

        //In case of Unity debugging set data directory
        //to that of the build so we don't have to store game data in two places
        #if (UNITY_EDITOR)
        prefix = "D:/Games/Maladath/";
        #endif

        #endregion

        //Gets the user characters and settings into the game
        #region User Data Deserialization

        //Get all characters..
        Deserialize(ref saves, "characters", false, prefix);
        saves ??= new();

        //Get user settings..
        Deserialize(ref settings, "settings", false, prefix);
        settings ??= new();
        settings.FillNulls();

        //Settings file contains last selected character and it's realm.
        //If the game content doesn't posses either the realm or the character that is supposed
        //to be on that realm, program will reset those values to default so the user
        //won't have a non existant character selected in the logging screen
        if (!saves.Any(x => x.Value.Exists(y => y.player.name == settings.selectedCharacter)))
        {
            settings.selectedCharacter = "";
            settings.selectedRealm = "";
        }

        #endregion

        //Loads the game content data from the game directory
        LoadData();

        //Spawn the initial desktop so the user can perform all actions from there
        SpawnDesktopBlueprint("TitleScreen");

        //Destroy this object as it's only used for program initialization
        Destroy(gameObject);
    }

    public static void LoadData()
    {
        //This region is responsible for deserializing the game content
        //into the game. By game content I mean specs, Abilities, Instances etc
        #region Data Deserialization

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
        Deserialize(ref PersonCategory.personCategories, "personcategories", false, prefix);
        PersonCategory.personCategories ??= new();
        Deserialize(ref Spec.specs, "specs", false, prefix);
        Spec.specs ??= new();
        Deserialize(ref Race.races, "races", false, prefix);
        Race.races ??= new();
        Deserialize(ref ItemSet.itemSets, "sets", false, prefix);
        ItemSet.itemSets ??= new();
        Deserialize(ref PVPRank.pvpRanks, "pvpranks", false, prefix);
        PVPRank.pvpRanks ??= new();
        Deserialize(ref Item.items, "items", false, prefix);
        Item.items ??= new();
        Deserialize(ref Ability.abilities, "abilities", false, prefix);
        Ability.abilities ??= new();
        Deserialize(ref WorldAbility.worldAbilities, "worldabilities", false, prefix);
        WorldAbility.worldAbilities ??= new();
        Deserialize(ref Buff.buffs, "buffs", false, prefix);
        Buff.buffs ??= new();
        Deserialize(ref Mount.mounts, "mounts", false, prefix);
        Mount.mounts ??= new();
        Deserialize(ref Recipe.recipes, "recipes", false, prefix);
        Recipe.recipes ??= new();
        Deserialize(ref Profession.professions, "professions", false, prefix);
        Profession.professions ??= new();
        Deserialize(ref GeneralDrop.generalDrops, "generaldrops", false, prefix);
        GeneralDrop.generalDrops ??= new();
        Deserialize(ref Faction.factions, "factions", false, prefix);
        Faction.factions ??= new();
        Deserialize(ref spiritHealers, "spirithealers", false, prefix);
        spiritHealers ??= new();
        Deserialize(ref flightPathGroups, "flightpaths", false, prefix);
        flightPathGroups ??= new();
        Deserialize(ref pEnchants, "permanentenchants", false, prefix);
        pEnchants ??= new();
        Deserialize(ref enchants, "enchants", false, prefix);
        enchants ??= new();
        Deserialize(ref zones, "zones", false, prefix);
        zones ??= new();
        Deserialize(ref quests, "quests", false, prefix);
        quests ??= new();
        Deserialize(ref paths, "paths", false, prefix);
        paths ??= new();
        Deserialize(ref defines, "defines", false, prefix);
        defines ??= new();
        var file = Resources.Load<Sprite>("Sprites/Textures/Map/Ground").texture;
        groundData = new string[file.width, file.height];
        for (int i = 0; i < file.width; i++)
            for (int j = 0; j < file.height; j++)
            {
                var color = (Color32)file.GetPixel(i, file.height - 1 - j);
                if (color.r == 127 && color.g == 51 && color.b == 0)
                    groundData[i, j] = "Dirt";
                else if (color.r == 64 && color.g == 64 && color.b == 64)
                    groundData[i, j] = "Stone";
                else if (color.r == 10 && color.g == 124 && color.b == 0)
                    groundData[i, j] = "Grass";
                else if (color.r == 124 && color.g == 85 && color.b == 0)
                    groundData[i, j] = "Wood";
                else if (color.r == 0 && color.g == 24 && color.b == 124)
                    groundData[i, j] = "Water";
                else if (color.r == 128 && color.g == 128 && color.b == 128)
                    groundData[i, j] = "Snow";
                else if (color.r == 0 && color.g == 122 && color.b == 122)
                    groundData[i, j] = "Ice";
                else if (color.r == 114 && color.g == 88 && color.b == 88)
                    groundData[i, j] = "Metal";
                else if (color.r == 84 && color.g == 64 && color.b == 64)
                    groundData[i, j] = "MetalGrate";
                else if (color.r == 178 && color.g == 140 && color.b == 0)
                    groundData[i, j] = "Sand";
                else if (color.r == 60 && color.g == 79 && color.b == 42)
                    groundData[i, j] = "Swamp";
                else
                    groundData[i, j] = "None";
            }

        #endregion

        //This region is responsible for serializing and deserializing
        //asset base information for in-game content to use such as music or textures
        #region Asset Database

        #if (UNITY_EDITOR)

        var ambienceList = AssetDatabase.FindAssets("t:AudioClip Ambience", new[] { "Assets/Resources/Ambience/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Ambience/", "")).ToList();
        var soundList = AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets/Resources/Sounds/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sounds/", "")).ToList();
        var itemIconList = AssetDatabase.FindAssets("t:Texture Item", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var abilityIconList = AssetDatabase.FindAssets("t:Texture Ability", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var mountIconList = AssetDatabase.FindAssets("t:Texture Mount", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var factionIconList = AssetDatabase.FindAssets("t:Texture Faction", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        var portraitList = AssetDatabase.FindAssets("t:Texture Portrait", new[] { "Assets/Resources/Sprites/Building/BigButtons/" }).Select(x => AssetDatabase.GUIDToAssetPath(x).Replace("Assets/Resources/Sprites/Building/BigButtons/", "")).ToList();
        ambienceList.RemoveAll(x => !x.StartsWith("Ambience"));
        itemIconList.RemoveAll(x => !x.StartsWith("Item"));
        abilityIconList.RemoveAll(x => !x.StartsWith("Ability"));
        mountIconList.RemoveAll(x => !x.StartsWith("Mount"));
        factionIconList.RemoveAll(x => !x.StartsWith("Faction"));
        portraitList.RemoveAll(x => !x.StartsWith("Portrait"));
        Assets.assets = new Assets()
        {
            ambience = ambienceList,
            sounds = soundList,
            itemIcons = itemIconList,
            abilityIcons = abilityIconList,
            mountIcons = mountIconList,
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
            mountIcons = new(),
            factionIcons = new(),
            portraits = new()
        };

        #endif

        #endregion

        //Would be amazing to have this in a separate loading screen to load up
        //Because this will increase in size with time and will require more time to process
        #region Initialise Objects

        for (int i = 0; i < SiteTown.towns.Count; i++)
            SiteTown.towns[i].Initialise();
        for (int i = 0; i < complexes.Count; i++)
            complexes[i].Initialise();
        for (int i = 0; i < instances.Count; i++)
            instances[i].Initialise();
        for (int i = 0; i < SiteHostileArea.areas.Count; i++)
            SiteHostileArea.areas[i].Initialise();
        for (int i = 0; i < Spec.specs.Count; i++)
            Spec.specs[i].Initialise();
        for (int i = 0; i < Item.items.Count; i++)
            Item.items[i].Initialise();
        for (int i = 0; i < WorldAbility.worldAbilities.Count; i++)
            WorldAbility.worldAbilities[i].Initialise();
        for (int i = 0; i < ItemSet.itemSets.Count; i++)
            ItemSet.itemSets[i].Initialise();
        for (int i = 0; i < Race.races.Count; i++)
            Race.races[i].Initialise();
        for (int i = 0; i < Realm.realms.Count; i++)
            Realm.realms[i].Initialise();
        for (int i = 0; i < Ability.abilities.Count; i++)
            Ability.abilities[i].Initialise();
        for (int i = 0; i < Buff.buffs.Count; i++)
            Buff.buffs[i].Initialise();
        for (int i = 0; i < Mount.mounts.Count; i++)
            Mount.mounts[i].Initialise();
        for (int i = 0; i < Recipe.recipes.Count; i++)
            Recipe.recipes[i].Initialise();
        for (int i = 0; i < GeneralDrop.generalDrops.Count; i++)
            GeneralDrop.generalDrops[i].Initialise();
        for (int i = 0; i < quests.Count; i++)
            quests[i].Initialise();
        for (int i = 0; i < spiritHealers.Count; i++)
            spiritHealers[i].Initialise();
        for (int i = 0; i < paths.Count; i++)
            paths[i].Initialise();
        //var deb = "";
        //var groups = quests.GroupBy(x => x.zone).Where(x => Resources.Load<Sprite>("Sprites/Building/Buttons/Zone" + x.Key.Clean()) == null).OrderByDescending(x => x.Count()).ToList();
        //for (int i = 0; i < groups.Count; i++)
        //    deb += groups[i].Key + " (" + groups[i].Count() + ")\n";
        //Debug.Log(deb);
        var elements = new List<string> { "Fire", "Water", "Earth", "Air", "Frost", "Lightning", "Arcane", "Decay", "Order", "Shadow" };
        foreach (var element in elements)
        {
            Blueprint.windowBlueprints.Add(new Blueprint("Player" + element + "Resource", () =>
            {
                SetAnchor(-320 + 19 * elements.IndexOf(element), 123 - 19 * Board.board.player.actionBars.Count);
                AddRegionGroup();
                SetRegionGroupHeight(Board.board.player.MaxResource(element) * 8);
                SetRegionGroupWidth(19);
                AddPaddingRegion(() =>
                {
                    AddLine("");
                    AddResourceBar(2, -2, element, "Player", Board.board.player);
                });
            }));
            Blueprint.windowBlueprints.Add(new Blueprint("Enemy" + element + "Resource", () =>
            {
                SetAnchor(299 - 19 * elements.IndexOf(element), 123 - 19 * Board.board.enemy.actionBars.Count);
                AddRegionGroup();
                SetRegionGroupHeight(Board.board.enemy.MaxResource(element) * 8);
                SetRegionGroupWidth(19);
                AddPaddingRegion(() =>
                {
                    AddLine("");
                    AddResourceBar(2, -2, element, "Enemy", Board.board.enemy);
                });
            }));
        }
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 5; j++)
                for (int k = 0; k < 3; k++)
                {
                    var tree = i; var row = j; var col = k;
                    if (Blueprint.windowBlueprints.Exists(x => x.title == "TalentButton" + tree + row + col)) continue;
                    Blueprint.windowBlueprints.Add(new Blueprint("TalentButton" + tree + row + col, () =>
                    {
                        var talent = PrintTalent(currentSave.lastVisitedTalents, row, col, tree);
                        var advancement = (currentSave.player.abilities.ContainsKey(talent.ability) ? (currentSave.player.abilities[talent.ability] + 1) * 2 : 0) + (currentSave.player.CanPickTalent(currentSave.lastVisitedTalents, talent) ? 1 : 0);
                        var dotAmount = Ability.abilities.Find(x => x.name == talent.ability).ranks.Count;
                        dotAmount = dotAmount == 0 ? 1 : dotAmount;
                        if (dotAmount > 1)
                            for (int i = 0; i < dotAmount && i < 7; i++)
                            {
                                var dot = new GameObject("TalentDot", typeof(SpriteRenderer));
                                dot.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/TalentDot" + (dotAmount == 1 ? "Single" : (i == 0 ? "First" : (i < dotAmount - 1 ? "Next" : "Last"))));
                                dot.GetComponent<SpriteRenderer>().sortingLayerName = "Upper";
                                dot.GetComponent<SpriteRenderer>().sortingOrder = 5 + i * 2;
                                dot.transform.parent = LBDesktop.LBWindow.LBRegionGroup.LBRegion.transform;
                                dot.transform.localPosition = new Vector3(38, -5 * i - 5);
                                var fill = new GameObject("TalentDotFill", typeof(SpriteRenderer));
                                fill.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/TalentDotFill" + (advancement >= (i + 1) * 2 ? "Picked" : (currentSave.player.unspentTalentPoints > 0 && advancement >= (i + 1) * 2 - 1 ? "Available" : "Locked")) + (dotAmount == 1 ? "Single" : (i == 0 ? "First" : (i < dotAmount - 1 ? "Next" : "Last"))));
                                fill.GetComponent<SpriteRenderer>().sortingLayerName = "Upper";
                                fill.GetComponent<SpriteRenderer>().sortingOrder = 4 + i * 2;
                                fill.transform.parent = dot.transform;
                                fill.transform.localPosition = new Vector3(0, 0);
                            }
                    }, true));
                }

        #endregion
    }
}
