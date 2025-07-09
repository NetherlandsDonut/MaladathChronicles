using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Item;
using static Sound;
using static Board;
using static MapGrid;
using static SaveGame;
using static Coloring;
using static Blueprint;
using static GameSettings;

using static Root.Anchor;
using static Root.RegionBackgroundType;

public static class Root
{
    //Width of the screen in pixels
    public static int screenX = 640;

    //Height of the screen in pixels
    public static int screenY = 360;

    //Checks whether the screen can be unlocked (will happen a frame later)
    public static bool canUnlockScreen;

    //Cheat setting that makes the player see all areas regardless of exploration progress
    public static bool showAreasUnconditional;

    //Cheat setting that enables going out of bounds with camera in adventure map
    public static bool disableCameraBounds;

    //Instance of Random that helps in generating random numbers
    public static System.Random random;

    //Page in the chart
    public static string chartPage;

    //Row for the icons in the chart
    public static Region iconRow;

    public static string creationRace;
    public static string creationSpec;
    public static string creationGender;

    public static Highlightable mouseOver;

    //Loading bar elements
    public static GameObject[] loadingBar;

    //Current amount of loaded objects out of the total
    public static int loadingScreenObjectLoad;

    //Amount of objects to be loaded in the loading screen
    public static int loadingScreenObjectLoadAim;

    //Sites to load
    public static List<Blueprint> loadSites;

    //Delegated action to the split window confirmation
    public static Action splitDelegate;

    //Index of the input line marker
    public static int inputLineMarker;

    //Planned tooltip to be shown
    public static Tooltip tooltip;

    //Amount of time left to show the tooltip
    public static float tooltipChanneling;

    //Position to print out the elite on the screen
    public static float positionOfElite;

    //Point at which player stopped for skirmish
    public static int pointsForRetreat;

    //Indicates whether the debug mode is active
    public static bool debug;

    public static float lastFunnyEffectTime;
    public static Vector3 lastFunnyEffectPosition;
    public static List<(int, int)> titleScreenFunnyEffect = new();

    public static int keyStack;
    public static KeyCode lastKey;
    public static float keyTimer;
    public static float animationTime;
    public static float animatedSpriteTime;
    public static bool awaitingNewBind;
    public static string newBindFor;

    #region Spell Targeting

    //Item that is being moved
    public static Ability abilityTargetted;

    //Participant targetted
    public static CombatParticipant participantTargetted;

    //Tile on the board that is targetted
    public static (int, int) tileTargetted;

    //Pick up an item to move it
    public static void ClearTargettingAbility(bool shouldClear = true)
    {
        if (!shouldClear) return;
        abilityTargetted = null;
        Cursor.cursor.SetCursor(CursorType.Default);
        //CloseWindow("TargettingInfo");
    }

    //Pick up an item to move it
    public static void StartTargettingAbility(Ability ability)
    {
        abilityTargetted = ability;
        if (abilityTargetted.possibleTargets == "Self") FinishTargettingAbility(null);
        else Cursor.cursor.SetCursor(CursorType.Crosshair);
        //if (abilityTargetted != null) Respawn("TargettingInfo");
    }

    //Pick up an item to move it
    public static void FinishTargettingAbility(CombatParticipant targetParticipant)
    {
        if (abilityTargetted == null) return;
        participantTargetted = targetParticipant;
        if (abilityTargetted.possibleTargets == "Tile") SpawnFallingText(new Vector2(0, 34), "This ability can only target the board", "Red");
        else if (abilityTargetted.possibleTargets == "Enemies" && participantTargetted.team == board.participants[board.whosTurn].team) SpawnFallingText(new Vector2(0, 34), "This ability can only target enemies", "Red");
        else if (abilityTargetted.possibleTargets == "Friendly" && participantTargetted.team != board.participants[board.whosTurn].team) SpawnFallingText(new Vector2(0, 34), "This ability can only target friendly targets", "Red");
        else if (!participantTargetted.who.CanBeTargetted(true)) { }
        else
        {
            var notMet = abilityTargetted.ConditionsNotMet("AbilityCast", abilityTargetted, board.participants[board.whosTurn].combatAbilities[abilityTargetted], currentSave, board);
            if (notMet.Count == 0)
            {
                foreach (var participant in board.participants)
                {
                    if (participant == board.participants[board.whosTurn]) board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, { "Triggerer", "Effector" }, { "AbilityName", abilityTargetted.name } });
                    else board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, { "Triggerer", "Other" }, { "AbilityName", abilityTargetted.name } });
                }
                board.participants[board.whosTurn].who.DetractResources(abilityTargetted.cost);
                foreach (var element in abilityTargetted.cost)
                    board.log.elementsUsed.Inc(element.Key, element.Value);
            }
            else SpawnFallingText(new Vector2(0, 34), notMet[0].failedMessage, "Red");
        }
        abilityTargetted = null;
        participantTargetted = null;
        //CloseWindow("TargettingInfo");
    }

    //Pick up an item to move it
    public static void FinishTargettingAbility(int x, int y)
    {
        if (abilityTargetted == null) return;
        if (abilityTargetted.possibleTargets == "Friendly") SpawnFallingText(new Vector2(0, 34), "This ability can only target friendly targets", "Red");
        else if (abilityTargetted.possibleTargets == "Enemies") SpawnFallingText(new Vector2(0, 34), "This ability can only target enemies", "Red");
        else if (abilityTargetted.possibleTargets == "Entity") SpawnFallingText(new Vector2(0, 34), "This ability cannot target the board", "Red");
        else
        {
            tileTargetted = (x, y);
            foreach (var participant in board.participants)
            {
                if (participant == board.participants[board.whosTurn]) board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, { "Triggerer", "Effector" }, { "AbilityName", abilityTargetted.name } });
                else board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, { "Triggerer", "Other" }, { "AbilityName", abilityTargetted.name } });
            }
            board.participants[board.whosTurn].who.DetractResources(abilityTargetted.cost);
            foreach (var element in abilityTargetted.cost)
                board.log.elementsUsed.Inc(element.Key, element.Value);
        }
        abilityTargetted = null;
        tileTargetted = (0, 0);
        //CloseWindow("TargettingInfo");
    }

    #endregion

    #region Moving Items

    //Item that is being moved
    public static Item movingItem;

    //From which window is the item being moved from
    public static string movingItemFrom;

    //Coordinates of the moving item for split pick up
    public static int movingItemX, movingItemY;

    //Is the picked up moving item a split off
    //If it is, you cannot place a split off on an already
    //existing item as that would make you lack one space for the previous one
    public static bool movingItemSplitOff;

    //Close moving item on closing of a window or switch of a desktop
    public static void CloseMovingItem()
    {
        if (movingItemFrom == "Bank") SaveGame.currentSave.banks[SiteTown.town.name].items.Add(movingItem);
        else SaveGame.currentSave.player.inventory.items.Add(movingItem);
        Cursor.cursor.iconAttached.SetActive(false);
        movingItem = null;
        Respawn("Inventory", true);
        Respawn("Bank", true);
    }

    //Pick up an item to move it
    public static void PickUpMovingItem(string window, Highlightable h, int amount = 0)
    {
        movingItemFrom = window;
        var bigButtonIndex = h == null ? movingItemX : h.region.bigButtons.IndexOf(h.GetComponent<LineBigButton>());
        var regionIndex = h == null ? movingItemY : h.window.headerGroup.regions.IndexOf(h.region) - (movingItemFrom == "Bank" ? 2 : 1);
        movingItem = (movingItemFrom == "Bank" ? SaveGame.currentSave.banks[SiteTown.town.name].items : SaveGame.currentSave.player.inventory.items).Find(x => x.x == bigButtonIndex && x.y == regionIndex);
        movingItemSplitOff = false;
        if (amount != 0 && amount < movingItem.amount)
        {
            movingItem.amount -= amount;
            movingItem = movingItem.CopyItem(amount);
            movingItemSplitOff = true;
        }
        else if (movingItemFrom == "Bank") SaveGame.currentSave.banks[SiteTown.town.name].items.Remove(movingItem);
        else SaveGame.currentSave.player.inventory.items.Remove(movingItem);
        PlaySound(movingItem.ItemSound("PickUp"), 0.8f);
        Cursor.cursor.iconAttached.SetActive(true);
        Cursor.cursor.iconAttached.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buttons/" + movingItem.icon);
        if (movingItemFrom == "Bank") Respawn("Inventory", true);
        else Respawn("Bank", true);
    }

    //Put down an item into an inventory
    public static void PutDownMovingItem(Highlightable h)
    {
        if (h.window.title == "PlayerEquipmentInfo")
        {
            if (movingItem.CanEquip(currentSave.player, true, true) && h.region.LBLine().LBText().text == movingItem.type)
            {
                movingItem.Equip(currentSave.player, false, false);
                movingItem.x = -1;
                movingItem.y = -1;
                PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
                movingItem = null;
                Cursor.cursor.iconAttached.SetActive(false);
                Respawn("Inventory", true);
                Respawn("PlayerEquipmentInfo", true);
                Respawn("PlayerWeaponsInfo", true);
            }
        }
        else if (h.window.title == "PlayerWeaponsInfo")
        {
            var typeWritten = h.region.LBLine().LBText().text;
            if (movingItem.CanEquip(currentSave.player, true, false) && ((typeWritten == "Off Hand" && movingItem.type == "Off Hand") || (typeWritten == "Main Hand" || typeWritten == "Off Hand") && (movingItem.type == "One Handed" || movingItem.type == "Two Handed") || typeWritten == "Ranged Weapon" && movingItem.type == "Ranged Weapon"))
            {
                movingItem.Equip(currentSave.player, false, typeWritten == "Off Hand");
                movingItem.x = -1;
                movingItem.y = -1;
                PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
                movingItem = null;
                Cursor.cursor.iconAttached.SetActive(false);
                Respawn("Inventory", true);
                Respawn("PlayerEquipmentInfo", true);
                Respawn("PlayerWeaponsInfo", true);
            }
        }
        else
        {
            if (h.window.title == "Bank") currentSave.banks[SiteTown.town.name].AddNewItem(movingItem);
            else currentSave.player.inventory.AddNewItem(movingItem);
            var bigButtonIndex = h.region.bigButtons.IndexOf(h.GetComponent<LineBigButton>());
            var regionIndex = h.window.headerGroup.regions.IndexOf(h.region) - (h.window.title == "Bank" ? 2 : 1);
            movingItem.x = bigButtonIndex;
            movingItem.y = regionIndex;
            PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
            movingItem = null;
            Cursor.cursor.iconAttached.SetActive(false);
            if (h.window.title == "Bank") Respawn("Inventory", true);
            else Respawn("Bank", true);
        }
    }

    //Swap moving item with the one at clicked position
    //If they can stack together, do that
    //If moving a split off you have to put it on a free space or same item type
    public static void SwapMovingItem(Highlightable h)
    {
        var bigButtonIndex = h.region.bigButtons.IndexOf(h.GetComponent<LineBigButton>());
        var regionIndex = h.window.headerGroup.regions.IndexOf(h.region) - (h.window.title == "Bank" ? 2 : 1);
        var temp = (h.window.title == "Bank" ? currentSave.banks[SiteTown.town.name].items : currentSave.player.inventory.items).Find(x => x.x == bigButtonIndex && x.y == regionIndex);
        PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
        if (temp.name == movingItem.name && temp.amount + movingItem.amount <= temp.maxStack)
        {
            temp.amount += movingItem.amount;
            movingItem = null;
            Cursor.cursor.iconAttached.SetActive(false);
        }
        else if (temp.name == movingItem.name && temp.amount + movingItem.amount > temp.maxStack)
        {
            movingItem.amount -= temp.maxStack - temp.amount;
            temp.amount = temp.maxStack;
        }
        else if (!movingItemSplitOff)
        {
            if (h.window.title == "Bank") currentSave.banks[SiteTown.town.name].AddNewItem(movingItem);
            else currentSave.player.inventory.AddNewItem(movingItem);
            movingItem.x = bigButtonIndex;
            movingItem.y = regionIndex;
            movingItem = temp;
            if (h.window.title == "Bank") currentSave.banks[SiteTown.town.name].items.Remove(movingItem);
            else currentSave.player.inventory.items.Remove(movingItem);
            Cursor.cursor.iconAttached.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buttons/" + movingItem.icon);
        }
        else
        {
            if (movingItemFrom == "Bank") currentSave.banks[SiteTown.town.name].AddItem(movingItem);
            else currentSave.player.inventory.AddItem(movingItem);
            movingItem = null;
            Cursor.cursor.iconAttached.SetActive(false);
            SpawnFallingText(new Vector2(0, 34), "Couldn't split the items", "Red");
            if (h.window.title == "Bank") Respawn("Inventory", true);
            else Respawn("Bank", true);
        }
    }

    #endregion

    public static Desktop CDesktop, LBDesktop;

    public static List<Desktop> desktops;

    #region Desktop

    private static Blueprint FindDesktopBlueprint(string name)
    {
        var find = desktopBlueprints.Find(x => x.title == name);
        find ??= BlueprintDev.desktopBlueprints.Find(x => x.title == name);
        return find;
    }

    //Spawns a new desktop and switches automatically by default
    public static void SpawnDesktopBlueprint(string blueprintTitle, bool autoSwitch = true, bool transition = false)
    {
        var blueprint = FindDesktopBlueprint(blueprintTitle);
        if (blueprint == null) return;
        var spawnedNew = false;
        if (!desktops.Exists(x => x.title == blueprintTitle))
            { AddDesktop(blueprint.title); spawnedNew = true; }
        if (autoSwitch) SwitchDesktop(blueprintTitle, transition);
        if (spawnedNew) blueprint.actions();
    }

    //Closes a desktop whether it was active or not
    public static bool CloseDesktop(string desktopName, bool transition = true)
    {
        var find = desktops.Find(x => x.title == desktopName);
        if (find == null) return false;
        desktops.Remove(find);
        if (find == CDesktop)
            if (desktops.Count > 0) SwitchDesktop(desktops[0].title, transition);
            else CDesktop = null;
        UnityEngine.Object.Destroy(find.gameObject);
        return true;
    }

    private static void AddDesktop(string title)
    {
        var newObject = new GameObject("Desktop: " + title, typeof(Desktop), typeof(SpriteRenderer));
        newObject.transform.localPosition = new Vector3();
        var newDesktop = newObject.GetComponent<Desktop>();
        LBDesktop = newDesktop;
        newDesktop.Initialise(title);
        desktops.Add(newDesktop);
        newDesktop.screen = new GameObject("Camera", typeof(Camera), typeof(SpriteRenderer)).GetComponent<Camera>();
        newDesktop.screen.transform.parent = newDesktop.transform;
        var screenOffsetter = new GameObject("CameraOffset");
        screenOffsetter.transform.parent = newDesktop.transform;
        screenOffsetter.transform.localPosition = new Vector2(10, -9);
        newDesktop.screen.transform.parent = screenOffsetter.transform;
        newDesktop.GetComponent<SpriteRenderer>().sortingLayerName = "DesktopBackground";
        newDesktop.screen.GetComponent<SpriteRenderer>().sortingLayerName = "DesktopBackground";
        newDesktop.screen.orthographicSize = 180;
        newDesktop.screen.nearClipPlane = -1024;
        newDesktop.screen.farClipPlane = 4096;
        newDesktop.screen.clearFlags = CameraClearFlags.SolidColor;
        newDesktop.screen.backgroundColor = new Color32(0, 29, 41, 255);
        newDesktop.screen.orthographic = true;
        if (settings.pixelPerfectVision.Value()) newDesktop.screen.gameObject.AddComponent<PixelCamera>();
        var cameraBorder = new GameObject("CameraBorder", typeof(SpriteRenderer));
        var cameraShadow = new GameObject("CameraShadow", typeof(SpriteRenderer));
        cameraShadow.transform.parent = cameraBorder.transform.parent = newDesktop.screen.transform;
        cameraBorder.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Fullscreen/Camera/CameraBorder");
        cameraShadow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Fullscreen/Camera/CameraShadow");
        cameraBorder.GetComponent<SpriteRenderer>().sortingLayerName = "CameraBorder";
        cameraShadow.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
        newDesktop.screenlock = new GameObject("Screenlock", typeof(BoxCollider2D), typeof(SpriteRenderer));
        newDesktop.screenlock.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Camera/CameraScreenlock");
        newDesktop.screenlock.GetComponent<SpriteRenderer>().sortingLayerName = "DesktopBackground";
        newDesktop.screenlock.GetComponent<SpriteRenderer>().sortingOrder = 1;
        newDesktop.screenlock.GetComponent<BoxCollider2D>().size = new Vector2(640, 360);
        newDesktop.screenlock.transform.parent = newDesktop.screen.transform;
        newDesktop.UnlockScreen();
        newObject.SetActive(false);
    }

    public static void SwitchDesktop(string name, bool transition = true)
    {
        Cursor.cursor.ResetColor();
        if (CDesktop != null && CDesktop.title == name) return;
        var windows = CDesktop != null ? CDesktop.windows.Select(x => x.title).ToList() : null;
        if (movingItem != null && CDesktop != null && CDesktop.windows.Any(x => x.title == "Inventory")) CloseMovingItem();
        if (CDesktop != null && CDesktop.title == "GameKeybinds") Serialization.Serialize(Keybinds.keybinds, "keybinds");
        if (mouseOver != null) mouseOver.OnMouseExit();
        if (CDesktop != null) CDesktop.gameObject.SetActive(false);
        var find = desktops.Find(x => x.title == name);
        if (find != null) CDesktop = find;
        if (CDesktop != null)
        {
            if (name == "Map") mapGrid.UpdateTextureColors(true);
            CDesktop.gameObject.SetActive(true);
            desktops.Remove(CDesktop);
            desktops.Insert(0, CDesktop);
            if (transition) SpawnTransition();
        }
        if (CDesktop.cameraDestination != Vector2.zero)
        {
            Cursor.cursor.transform.position += (Vector3)CDesktop.cameraDestination - CDesktop.screen.transform.localPosition;
            CDesktop.screen.transform.localPosition = (Vector3)CDesktop.cameraDestination;
        }
        if (WindowUp("HostileArea"))
        {
            CloseWindow("HostileAreaQuestTracker");
            Respawn("HostileArea");
        }
        Respawn("HostileAreaQuestAvailable", true);
        Respawn("HostileAreaQuestDone", true);
        Respawn("TownQuestAvailable", true);
        Respawn("TownQuestDone", true);
        CloseWindow("QuestAdd");
        CloseWindow("QuestTurn");
        if (CDesktop.title == "Map") Respawn("WorldBuffs", true);
        if (windows != null)
            foreach (var window in windows)
                Respawn(window, true);
        if (!settings.pixelPerfectVision.Value() && CDesktop.screen.GetComponent<PixelCamera>() != null) UnityEngine.Object.Destroy(CDesktop.screen.GetComponent<PixelCamera>());
        else if (settings.pixelPerfectVision.Value() && CDesktop.screen.GetComponent<PixelCamera>() == null) CDesktop.screen.gameObject.AddComponent<PixelCamera>();
    }

    public static void OrderLoadingMap()
    {
        Quest.sitesToRespawn = new();
        Quest.sitesWithQuestMarkers = new();
        mapGrid.cameraBoundaryPoints = new();
        loadSites = windowBlueprints.FindAll(x => x.title.StartsWith("Site: "));
        for (int i = loadSites.Count - 1; i >= 0; i--)
        {
            var site = Site.FindSite(x => "Site: " + x.name == loadSites[i].title);
            if (site != null)
            {
                mapGrid.cameraBoundaryPoints.Add(new Vector2(site.x, site.y));
                if (!site.CanBeSeen()) loadSites.RemoveAt(i);
            }
        }
        loadingScreenObjectLoad = 0;
        loadingScreenObjectLoadAim = loadSites.Count;
    }

    public static void SpawnTransition(bool single = true, float speed = 2f)
    {
        if (CDesktop.transition != null && single) return;
        var transition = new GameObject("CameraTransition", typeof(SpriteRenderer), typeof(Shatter));
        transition.transform.parent = CDesktop.screen.transform;
        transition.transform.localPosition = new Vector3(0, 0, -0.01f);
        transition.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Fullscreen/Camera/CameraTransition");
        transition.GetComponent<SpriteRenderer>().sortingLayerName = "CameraBorder";
        transition.GetComponent<Shatter>().Initiate(speed, 0, transition.GetComponent<SpriteRenderer>());
        CDesktop.transition = transition;
    }

    public static void RemoveDesktopBackground(bool followCamera = true)
    {
        if (followCamera) CDesktop.screen.GetComponent<SpriteRenderer>().sprite = null;
        else CDesktop.GetComponent<SpriteRenderer>().sprite = null;
    }

    public static void SetDesktopBackground(string texture, bool followCamera = true)
    {
        var sprite = Resources.Load<Sprite>("Sprites/Fullscreen/" + texture);
        var temp = (followCamera ? CDesktop.screen.gameObject : CDesktop.gameObject).GetComponent<SpriteRenderer>();
        if (sprite == null) Debug.Log("ERROR 004: Desktop background not found: \"Sprites/Fullscreen/" + texture + "\"");
        else if (temp.sprite != sprite)
        {
            if (temp.sprite != null)
                SpawnTransition();
            temp.sprite = sprite;
        }
    }

    //Hotkeys can be added only on desktop creation!
    public static void AddHotkey(string function, Action action, bool keyDown = true, bool closesTooltip = true)
    {
        if (!Keybinds.keybinds.ContainsKey(function)) return;
        var key = Keybinds.keybinds[function].key;
        if (LBDesktop.hotkeys.Exists(x => x.key == key && x.keyDown == keyDown)) return;
        LBDesktop.hotkeys.Add(new Hotkey(key, action, keyDown, closesTooltip));
    }

    //Hotkeys can be added only on desktop creation!
    public static void AddHotkey(KeyCode key, Action action, bool keyDown = true, bool closesTooltip = true)
    {
        if (LBDesktop.hotkeys.Exists(x => x.key == key && x.keyDown == keyDown)) return;
        LBDesktop.hotkeys.Add(new Hotkey(key, action, keyDown, closesTooltip));
    }

    #endregion

    #region Windows

    public static Blueprint FindWindowBlueprint(string name)
    {
        var find = windowBlueprints.Find(x => x.title == name);
        find ??= BlueprintDev.windowBlueprints.Find(x => x.title == name);
        return find;
    }

    public static Window SpawnWindowBlueprint(string blueprintTitle, bool resetSearch = true)
    {
        return SpawnWindowBlueprint(FindWindowBlueprint(blueprintTitle), resetSearch);
    }

    public static Window SpawnWindowBlueprint(Blueprint blueprint, bool resetSearch = true)
    {
        if (blueprint == null) return null;
        if (WindowUp(blueprint.title)) return null;
        AddWindow(blueprint.title, blueprint.upperUI);
        blueprint.actions();
        if (resetSearch && CDesktop.LBWindow().maxPaginationReq != null) String.search.Set("");
        CDesktop.LBWindow().Rebuild();
        CDesktop.LBWindow().ResetPosition();
        return CDesktop.LBWindow();
    }

    public static bool WindowUp(string title) => CDesktop.windows.Exists(x => x.title == title);

    public static void AddWindow(string title, bool upperUI)
    {
        var newObject = new GameObject("Window: " + title, typeof(Window));
        newObject.transform.parent = CDesktop.transform;
        newObject.GetComponent<Window>().Initialise(CDesktop, title, upperUI);
    }

    public static bool Respawn(string windowName, bool onlyWhenActive = false)
    {
        var window = CDesktop.windows.Find(x => x.title == windowName);
        bool wasThere = window != null;
        if (wasThere) window.Respawn(onlyWhenActive);
        else if (!onlyWhenActive) SpawnWindowBlueprint(windowName, true);
        return wasThere;
    }

    public static bool CloseWindow(string windowName, bool resetPagination = true)
    {
        if (windowName == "Inventory" && movingItem != null) CloseMovingItem();
        return CDesktop != null && CloseWindow(CDesktop.windows.Find(x => x.title == windowName), resetPagination);
    }

    public static bool CloseWindow(Window window, bool resetPagination = true)
    {
        if (window == null) return false;
        if (resetPagination && staticPagination.ContainsKey(window.title))
            staticPagination.Remove(window.title);
        CDesktop.windows.Remove(window);
        UnityEngine.Object.Destroy(window.gameObject);
        return true;
    }

    public static void SetAnchor(float x = 0, float y = 0)
    {
        CDesktop.LBWindow().anchor = new WindowAnchor(None, x, y);
    }

    public static void SetAnchor(Anchor anchor, float x = 0, float y = 0)
    {
        CDesktop.LBWindow().anchor = new WindowAnchor(anchor, x, y);
    }

    public static void DisableGeneralSprites()
    {
        CDesktop.LBWindow().disabledGeneralSprites = true;
    }

    public static void DisableCollisions()
    {
        CDesktop.LBWindow().disabledCollisions = true;
    }

    public static void DisableShadows()
    {
        CDesktop.LBWindow().disabledShadows = true;
    }

    public static void MaskWindow()
    {
        CDesktop.LBWindow().masked = true;
    }

    #endregion

    #region RegionGroups

    public static void AddHeaderGroup(bool paged = false)
    {
        var newObject = new GameObject("HeaderGroup", typeof(RegionGroup));
        newObject.transform.parent = CDesktop.LBWindow().transform;
        newObject.GetComponent<RegionGroup>().Initialise(CDesktop.LBWindow(), true);
    }

    public static void AddRegionGroup()
    {
        var newObject = new GameObject("RegionGroup", typeof(RegionGroup));
        newObject.transform.parent = CDesktop.LBWindow().transform;
        newObject.GetComponent<RegionGroup>().Initialise(CDesktop.LBWindow(), false);
    }

    public static void SetRegionGroupWidth(int width)
    {
        CDesktop.LBWindow().LBRegionGroup().setWidth = width < 0 ? 0 : width;
    }

    public static void SetRegionGroupHeight(int height)
    {
        CDesktop.LBWindow().LBRegionGroup().setHeight = height < 0 ? 0 : height;
    }

    #endregion

    #region Regions

    private static void AddRegion(RegionBackgroundType backgroundType, Action draw, Action<Highlightable> pressEvent, Action<Highlightable> rightPressEvent, Func<Highlightable, Action> tooltip, Action<Highlightable> middlePressEvent)
    {
        var region = new GameObject("Region", typeof(Region)).GetComponent<Region>();
        var regionGroup = CDesktop.LBWindow().LBRegionGroup();
        region.transform.parent = regionGroup.transform;
        region.background = new GameObject("Background", typeof(SpriteRenderer), typeof(RegionBackground));
        region.background.transform.parent = region.transform;
        region.Initialise(regionGroup, backgroundType, draw);
        if (pressEvent != null || rightPressEvent != null || middlePressEvent != null || tooltip != null)
            region.background.AddComponent<Highlightable>().Initialise(region, pressEvent, rightPressEvent, tooltip, middlePressEvent);
    }

    public static void AddRegionOverlay(Region onWhat, string overlay, float time = 0)
    {
        var newObject = new GameObject("RegionOverlay", typeof(SpriteRenderer));
        newObject.transform.parent = onWhat.transform;
        newObject.transform.localPosition = onWhat.background.transform.localPosition - new Vector3(0, 0, 0.1f);
        newObject.transform.parent = CDesktop.transform;
        newObject.transform.localScale = onWhat.background.transform.localScale;
        newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Fills/" + overlay);
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(time);
        }
    }

    public static void AddButtonRegion(Action draw, Action<Highlightable> pressEvent = null, Action<Highlightable> rightPressEvent = null, Func<Highlightable, Action> tooltip = null, Action<Highlightable> middlePressEvent = null)
    {
        AddRegion(Button, draw, pressEvent, rightPressEvent, tooltip, middlePressEvent);
    }

    public static void AddEmptyRegion()
    {
        AddRegion(Empty, () => { AddLine(""); }, null, null, null, null);
    }

    public static void AddSmallEmptyRegion()
    {
        AddRegion(Empty, () => { }, null, null, null, null);
    }

    public static void AddHeaderRegion(Action draw)
    {
        AddRegion(Header, draw, null, null, null, null);
    }

    public static void AddPaddingRegion(Action draw)
    {
        AddRegion(Padding, draw, null, null, null, null);
    }

    public static void AddPaginationLine()
    {
        AddPaddingRegion(() =>
        {
            var thisWindow = CDesktop.LBWindow();
            AddLine("Page: ", "DarkGray");
            if (thisWindow.paginateFullPages)
                AddText((thisWindow.pagination() + 1) / thisWindow.perPage + 1 + "");
            else AddText(thisWindow.pagination() + 1 + "");
            AddText(" / ", "DarkGray");
            if (thisWindow.paginateFullPages)
                AddText(Math.Ceiling((double)thisWindow.maxPagination() / thisWindow.perPage) + 1 + "");
            else AddText(thisWindow.maxPagination() + 1 + "");
            AddSmallButton("OtherNextPage", (h) =>
            {
                if (thisWindow.pagination() < thisWindow.maxPagination())
                {
                    PlaySound("DesktopChangePage", 0.6f);
                    thisWindow.IncrementPagination();
                }
            });
            AddSmallButton("OtherPreviousPage", (h) =>
            {
                if (thisWindow.pagination() > 0)
                {
                    PlaySound("DesktopChangePage", 0.6f);
                    thisWindow.DecrementPagination();
                }
            });
        });
    }

    public static void ReverseButtons()
    {
        CDesktop.LBWindow().LBRegionGroup().LBRegion().reverseButtons ^= true;
    }

    public static void SetRegionBackgroundToGrayscale()
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        region.background.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Grayscale");
    }

    //When other region groups are lenghier than the
    //one this region is in then the unique extender will
    //be extended to match length of the other group regions
    public static void SetRegionAsGroupExtender()
    {
        var temp = CDesktop.LBWindow().LBRegionGroup();
        temp.stretchRegion = temp.LBRegion();
    }

    public static void SetRegionBackground(RegionBackgroundType backgroundType)
    {
        CDesktop.LBWindow().LBRegionGroup().LBRegion().backgroundType = backgroundType;
    }

    public static void SetRegionBackgroundAsImage(string replacement)
    {
        SetRegionBackground(Image);
        CDesktop.LBWindow().LBRegionGroup().LBRegion().backgroundImage = Resources.Load<Sprite>("Sprites/RegionReplacements/" + replacement);
    }

    public static void PrintPriceRegion(int price, int width1, int width2, int width3)
    {
        var last = "";
        var lacking = 0;
        if (price > 9999) { Foo("ItemCoinsGold", price / 10000 + "", "Gold", width1); last = "Gold"; } else lacking += width1 + 19;
        if (price / 100 % 100 > 0) { Foo("ItemCoinsSilver", price / 100 % 100 + "", "Silver", width2); last = "Silver"; } else lacking += width2 + 19;
        if (price % 100 > 0 || price == 0) { Foo("ItemCoinsCopper", price % 100 + "", "Copper", width3); last = "Copper"; } else lacking += width3 + 19;
        var showDisenchant = CDesktop.LBWindow().title == "Inventory" && currentSave.player.professionSkills.ContainsKey("Enchanting");
        SetRegionGroupWidth(lacking + (last == "Gold" ? width1 : (last == "Silver" ? width2 : width3)) - (showDisenchant ? 19 : 0));
        if (showDisenchant)
        {
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                if (CDesktop.title == "EquipmentScreen")
                    AddSmallButton(Cursor.cursor.color != "Pink" ? "ItemDisenchant" : "OtherCloseDisenchant", (h) =>
                    {
                        if (Cursor.cursor.color != "Pink")
                            Cursor.cursor.SetColor("Pink");
                        else Cursor.cursor.ResetColor();
                        Respawn("PlayerEquipmentInfo");
                        Respawn("PlayerWeaponsInfo");
                    });
                else
                {
                    AddSmallButton("ItemDisenchant");
                    SetSmallButtonToGrayscale();
                }
            });
        }

        void Foo(string icon, string text, string color, int size)
        {
            AddRegionGroup();
            AddPaddingRegion(() => { AddSmallButton(icon); });
            AddRegionGroup();
            SetRegionGroupWidth(size);
            AddPaddingRegion(() => { AddLine(text, color); });
        }
    }

    public static void PrintEquipmentSlot(string slot, Item item)
    {
        if (item != null)
            AddHeaderRegion(() =>
            {
                ReverseButtons();
                AddLine(item.name, item.rarity, "Left");
                AddSmallButton(item.icon,
                (h) =>
                {
                    if (Cursor.cursor.color == "Pink") return;
                    if (WindowUp("InventorySort")) return;
                    if (WindowUp("ConfirmItemDisenchant")) return;
                    if (WindowUp("ConfirmItemDestroy")) return;
                    if (WindowUp("Inventory") && movingItem == null && currentSave.player.inventory.CanAddItem(currentSave.player.equipment[slot]))
                    {
                        PlaySound(item.ItemSound("PutDown"), 0.8f);
                        foreach (var unequiped in currentSave.player.Unequip(new() { slot }))
                            currentSave.player.inventory.AddItem(unequiped);
                        Respawn("PlayerEquipmentInfo");
                        Respawn("PlayerWeaponsInfo");
                        Respawn("Inventory");
                    }
                },
                (h) =>
                {
                    if (WindowUp("InventorySort")) return;
                    if (WindowUp("ConfirmItemDisenchant")) return;
                    if (WindowUp("ConfirmItemDestroy")) return;
                    if (item.CanUse(currentSave.player, true))
                    {
                        PlaySound(item.ItemSound("Use"), 0.8f);
                        item.Use(currentSave.player);
                        Respawn("Inventory", true);
                        Respawn("PlayerEquipmentInfo", true);
                        Respawn("PlayerWeaponsInfo", true);
                    }
                },
                (h) => () =>
                {
                    if (WindowUp("InventorySort")) return;
                    if (WindowUp("ConfirmItemDisenchant")) return;
                    if (WindowUp("ConfirmItemDestroy")) return;
                    PrintItemTooltip(item);
                });
                if (settings.rarityIndicators.Value())
                    AddSmallButtonOverlay("OtherRarity" + item.rarity, 0, 2);
                if (Cursor.cursor.color == "Pink")
                    if (!item.IsDisenchantable()) SetSmallButtonToGrayscale();
                    else SetSmallButtonToRed();
            });
        else
            AddPaddingRegion(() =>
            {
                ReverseButtons();
                AddLine(slot, "DarkGray", "Left");
                AddSmallButton("OtherEmpty", (h) =>
                {
                    if (WindowUp("Inventory") && movingItem != null)
                        PutDownMovingItem(h);
                });
            });
    }

    #endregion

    #region Lines

    public static void AddLine(string text = "", string color = "", string align = "Left")
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        if (region.lines.Count > 0 && region.smallButtons.Count > 0) return;
        var newObject = new GameObject("Line", typeof(Line));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<Line>().Initialise(region, align);
        AddText(text, color == "" ? DefaultTextColorForRegion(region.backgroundType) : color);
    }

    public static string DefaultTextColorForRegion(RegionBackgroundType type)
    {
        if (type == Header) return "Gray";
        if (type == Padding) return "Gray";
        if (type == Button) return "Black";
        if (type == ButtonRed) return "Black";
        else return "Gray";
    }

    #endregion

    #region SmallButtons

    public static void SetSmallButtonToRed()
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var button = region.LBSmallButton().gameObject;
        button.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Red");
    }

    public static void SetSmallButtonToGrayscale()
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var button = region.LBSmallButton().gameObject;
        button.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Grayscale");
    }

    public static void AddSmallButtonOverlay(string overlay, float time = 0, int sortingOrder = 0)
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var button = region.LBSmallButton().gameObject;
        AddSmallButtonOverlay(button, overlay, time, sortingOrder);
    }

    public static void SmallButtonFlipX()
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var button = region.LBSmallButton().gameObject;
        button.GetComponent<SpriteRenderer>().flipX ^= true;
    }

    public static void SmallButtonFlipY()
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var button = region.LBSmallButton().gameObject;
        button.GetComponent<SpriteRenderer>().flipY ^= true;
    }

    public static GameObject AddSmallButtonOverlay(GameObject onWhat, string overlay, float time = 0, int sortingOrder = 0)
    {
        var newObject = new GameObject("SmallButtonOverlay", typeof(SpriteRenderer));
        newObject.transform.parent = onWhat.transform;
        newObject.transform.localPosition = new Vector3(overlay == "PlayerLocationFromBelow" || overlay == "EnemyLocationFromBelow" || overlay == "FriendLocationFromBelow" || overlay == "PlayerLocationSmall" || overlay == "AvailableQuest" ? 1 : (overlay == "YellowGlowBig" ? 0.5f : 0), overlay == "AvailableQuest" ? -8.5f : (overlay == "PlayerLocationSmall" || overlay == "PlayerLocationFromBelow" ? -6f : (overlay == "YellowGlowBig" ? -0.5f : 0)), -0.01f);
        if (overlay == "Cooldown") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/Cooldown", true);
        else if (overlay == "YellowGlow") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/YellowGlow", true);
        else if (overlay == "YellowGlowBig") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/YellowGlowBig", false, 0.07f);
        else if (overlay == "AutoCast") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/AutoCastFull", true);
        else if (overlay == "PlayerLocationFromBelow") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/PlayerLocationFromBelow", false, 0.07f);
        else if (overlay == "EnemyLocationFromBelow") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/EnemyLocationFromBelow", false, 0.07f);
        else if (overlay == "FriendLocationFromBelow") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/FriendLocationFromBelow", false, 0.07f);
        else if (overlay == "PlayerLocationSmall") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/PlayerLocationSmall", false, 0.07f);
        else if (overlay == "AvailableQuest") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/AvailableQuest", false, 0.07f);
        else newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Buttons/" + overlay);
        newObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = CDesktop.LBWindow().layer;
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(time);
        }
        return newObject;
    }

    public static void AddSmallButton(string type, Action<Highlightable> pressEvent = null, Action<Highlightable> rightPressEvent = null, Func<Highlightable, Action> tooltip = null, Action<Highlightable> middlePressEvent = null)
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var newObject = new GameObject("SmallButton: " + type, typeof(LineSmallButton), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineSmallButton>().Initialise(region, type);
        if (pressEvent != null || rightPressEvent != null || tooltip != null)
            newObject.AddComponent<Highlightable>().Initialise(region, pressEvent, rightPressEvent, tooltip, middlePressEvent);
    }

    #endregion

    #region BigButtons

    public static void SetBigButtonToRed()
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var button = region.LBBigButton().gameObject;
        button.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Red");
    }

    public static void SetBigButtonToGrayscale()
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var button = region.LBBigButton().gameObject;
        button.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Shaders/Grayscale");
    }

    public static GameObject AddBigButtonOverlay(string overlay, float time = 0, int sortingOrder = 0)
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var button = region.LBBigButton().gameObject;
        return AddBigButtonOverlay(button, overlay, time, sortingOrder);
    }

    public static void BigButtonFlipX()
    {
        var button = CDesktop.LBWindow().LBRegionGroup().LBRegion().LBBigButton().gameObject;
        button.GetComponent<SpriteRenderer>().flipX ^= true;
    }

    public static void BigButtonFlipY()
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var button = region.LBBigButton().gameObject;
        button.GetComponent<SpriteRenderer>().flipY ^= true;
    }

    public static GameObject AddBigButtonCooldownOverlay(double percentage)
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var button = region.LBBigButton().gameObject;
        var newObject = new GameObject("BigButtonGrid", typeof(SpriteRenderer));
        newObject.transform.parent = button.transform;
        newObject.transform.localPosition = new Vector3(0, 0, -0.01f);
        var sprites = Resources.LoadAll<Sprite>("Sprites/Other/CooldownBig");
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = CDesktop.LBWindow().layer;
        var value = 1.0 / sprites.Length;
        var first = 0;
        for (int i = 0; i < sprites.Length - 1; i++)
            if (percentage > value)
            {
                percentage -= value;
                first++;
            }
        newObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 225);
        newObject.GetComponent<SpriteRenderer>().sprite = sprites[Math.Abs(sprites.Length - 1 - first)];
        return newObject;
    }

    public static GameObject AddBigButtonBuybackOverlay(double percentage)
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var button = region.LBBigButton().gameObject;
        var newObject = new GameObject("BigButtonGrid", typeof(SpriteRenderer));
        newObject.transform.parent = button.transform;
        newObject.transform.localPosition = new Vector3(0, 0, -0.01f);
        var sprites = Resources.LoadAll<Sprite>("Sprites/Other/BuybackBig");
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = CDesktop.LBWindow().layer;
        var value = 1.0 / sprites.Length;
        var first = 0;
        for (int i = 0; i < sprites.Length - 1; i++)
            if (percentage > value)
            {
                percentage -= value;
                first++;
            }
        newObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 225);
        newObject.GetComponent<SpriteRenderer>().sprite = sprites[Math.Abs(sprites.Length - 1 - first)];
        return newObject;
    }

    public static GameObject AddBigButtonOverlay(GameObject onWhat, string overlay, float time = 0, int sortingOrder = 0)
    {
        var newObject = new GameObject("BigButtonOverlay", typeof(SpriteRenderer));
        newObject.transform.parent = onWhat.transform;
        newObject.transform.localPosition = new Vector3(0, 0, -0.01f);
        if (overlay == "CooldownBig") newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/CooldownBig", true);
        else if (overlay == "SneakingBig")
        {
            newObject.transform.localPosition = new Vector3(-18, 18, -0.01f);
            newObject.AddComponent<AnimatedSprite>().Initiate("Sprites/Other/SneakingBig", true);
        }
        else newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ButtonsBig/" + overlay);
        newObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = CDesktop.LBWindow().layer;
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(time);
        }
        return newObject;
    }

    public static GameObject AddBigButtonOverlay(Vector2 position, string overlay, float time = 0, int sortingOrder = 0)
    {
        var newObject = new GameObject("BigButtonGrid", typeof(SpriteRenderer));
        newObject.transform.parent = CDesktop.transform;
        newObject.transform.localPosition = new Vector3(position.x, position.y, -0.01f);
        newObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ButtonsBig/" + overlay);
        newObject.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        newObject.GetComponent<SpriteRenderer>().sortingLayerName = CDesktop.LBWindow().layer;
        if (time > 0)
        {
            newObject.AddComponent<Shatter>().render = newObject.GetComponent<SpriteRenderer>();
            newObject.GetComponent<Shatter>().Initiate(time);
        }
        return newObject;
    }

    public static void AddBigButton(string type, Action<Highlightable> pressEvent = null, Action<Highlightable> rightPressEvent = null, Func<Highlightable, Action> tooltip = null, Action<Highlightable> middlePressEvent = null)
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        var newObject = new GameObject("BigButton: " + (type == null ? "Empty" : type), typeof(LineBigButton), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineBigButton>().Initialise(region, type);
        if (pressEvent != null || rightPressEvent != null || tooltip != null)
            newObject.AddComponent<Highlightable>().Initialise(region, pressEvent, rightPressEvent, tooltip, middlePressEvent);
    }

    #endregion

    #region Checkboxes

    public static void AddCheckbox(Bool value)
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        if (region.checkbox != null) return;
        var newObject = new GameObject("Checkbox", typeof(LineCheckbox), typeof(SpriteRenderer));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<LineCheckbox>().Initialise(region, value);
    }

    #endregion

    #region Text

    public static void AddText(string text = "", string color = "")
    {
        text ??= "";
        var newObject = new GameObject("Text", typeof(LineText));
        var line = CDesktop.LBWindow().LBRegionGroup().LBRegion().LBLine();
        newObject.transform.parent = line.transform;
        newObject.GetComponent<LineText>().Initialise(line, text, color == "" ? line.LBText().color : color);
    }

    public static void SpawnFloatingText(Vector2 position, string text = "", string color = "", string borderColor = "", string align = "Center")
    {
        text ??= "";
        var newObject = new GameObject("FloatingText", typeof(FloatingText));
        newObject.transform.parent = CDesktop.LBWindow().LBRegionGroup().LBRegion().transform;
        newObject.transform.localPosition = position;
        var temp = newObject.GetComponent<FloatingText>();
        temp.Initialise(text, color == "" ? "Gray" : color, borderColor, align, false);
    }

    public static void SpawnFallingText(Vector2 position, string text = "", string color = "", string borderColor = "", string align = "Center")
    {
        text ??= "";
        var newObject = new GameObject("FallingText", typeof(FloatingText));
        newObject.transform.parent = CDesktop.transform;
        newObject.transform.localPosition = position;
        newObject.AddComponent<Rigidbody2D>().gravityScale = 2.0f;
        newObject.AddComponent<Shatter>().Initiate(7);
        var temp = newObject.GetComponent<FloatingText>();
        temp.Initialise(text, color == "" ? "Gray" : color, borderColor, align);
    }

    #endregion  

    #region InputLines

    public static void AddInputLine(String refText, string color = "", string align = "Left")
    {
        var region = CDesktop.LBWindow().LBRegionGroup().LBRegion();
        if (region.lines.Count > 0 && region.checkbox != null) return;
        var newObject = new GameObject("InputLine", typeof(InputLine));
        newObject.transform.parent = region.transform;
        newObject.GetComponent<InputLine>().Initialise(region, refText, color, align);
    }

    #endregion

    #region Charts

    public static void AddChart()
    {
        Dictionary<string, int> dic = new();
        var rising = false;
        if (chartPage == "Damage Dealt") dic = board.log.damageDealt;
        else if (chartPage == "Damage Taken") dic = board.log.damageTaken;
        else if (chartPage == "Healing Received") dic = board.log.healingReceived;
        else if (chartPage == "Elements Used") dic = board.log.elementsUsed;
        var temp = dic.Count * (settings.chartBigIcons.Value() ? 38 : 19);
        if (temp == 0)
        {
            AddRegionGroup();
            SetRegionGroupWidth(600);
            SetRegionGroupHeight(243);
            AddPaddingRegion(() =>
            {
                SetRegionBackground(ChartBackground);
                SetRegionAsGroupExtender();
            });
        }
        else
        {
            var list = dic.OrderBy(x => x.Value * (rising ? 1 : -1)).Select(x => (x.Key, x.Value)).ToList();
            AddRegionGroup();
            var left = 600 - temp;
            SetRegionGroupWidth((int)Math.Floor(left / 2.0));
            SetRegionGroupHeight(243);
            AddPaddingRegion(() =>
            {
                SetRegionBackground(ChartBackground);
                SetRegionAsGroupExtender();
            });
            AddRegionGroup();
            SetRegionGroupWidth(temp);
            SetRegionGroupHeight(243);
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
            });
            AddHeaderRegion(() =>
            {
                if (settings.chartBigIcons.Value()) for (int i = list.Count - 1; i >= 0 && i - list.Count > -15; i--)
                    if (chartPage == "Elements Used") AddBigButton("Element" + list[i].Key + "Rousing");
                    else AddBigButton(Ability.abilities.Find(y => y.name == list[i].Key).icon);
                else for (int i = 0; i < list.Count && i < 30; i++)
                    if (chartPage == "Elements Used") AddSmallButton("Element" + list[i].Key + "Rousing");
                    else AddSmallButton(Ability.abilities.Find(y => y.name == list[i].Key).icon);
            });
            iconRow = CDesktop.LBWindow().LBRegionGroup().LBRegion();
            AddRegionGroup();
            SetRegionGroupWidth((int)Math.Ceiling(left / 2.0));
            SetRegionGroupHeight(243);
            AddPaddingRegion(() =>
            {
                SetRegionBackground(ChartBackground);
                SetRegionAsGroupExtender();
            });
        }
    }

    public static void FillChart()
    {
        Dictionary<string, int> dic = new();
        var rising = false;
        if (chartPage == "Damage Dealt") dic = board.log.damageDealt;
        else if (chartPage == "Damage Taken") dic = board.log.damageTaken;
        else if (chartPage == "Healing Received") dic = board.log.healingReceived;
        else if (chartPage == "Elements Used") dic = board.log.elementsUsed;
        var temp = dic.Count * (settings.chartBigIcons.Value() ? 38 : 19);
        if (temp > 0)
        {
            var list = dic.OrderBy(x => x.Value * (rising ? 1 : -1)).Select(x => (x.Key, x.Value)).ToList();
            var highestValue = list.Max(x => x.Value);
            if (settings.chartBigIcons.Value())
                for (int i = list.Count - 1; i >= 0 && i > -15; i--)
                    AddChartColumn(list.Count, Math.Abs(list.Count - 1 - i), list.Sum(x => x.Value), highestValue, list[i].Value);
            else for (int i = 0; i < list.Count && i < 30; i++)
                AddChartColumn(list.Count, i, list.Sum(x => x.Value), highestValue, list[i].Value);
        }
    }

    public static void AddChartColumn(int amount, int index, int total, int highestValue, int value)
    {
        var height = (int)((224.0 - (settings.chartBigIcons.Value() ? 19 : 0)) / highestValue * value);
        SpawnWindowBlueprint(new Blueprint("ChartColumn" + index, () =>
        {
            var foo = (settings.chartBigIcons.Value() ? iconRow.bigButtons.Select(x => x.transform) : iconRow.smallButtons.Select(x => x.transform)).Last().position;
            SetAnchor(foo.x + (settings.chartBigIcons.Value() ? -38 : 19) * (amount - 1 - index) - (settings.chartBigIcons.Value() ? 20f : 10.5f), foo.y + (settings.chartBigIcons.Value() ? 24 + (height < 15 ? 15 : height) : 14.5f + height));
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(settings.chartBigIcons.Value() ? 38 : 19);
            SetRegionGroupHeight(height);
            AddPaddingRegion(() =>
            {
                SetRegionBackground(Button);
                if (settings.chartBigIcons.Value())
                    AddLine(/*Math.Round(100.0 / total * value)*/value + "");
                SetRegionAsGroupExtender();
            });
        },
        true));
    }

    #endregion

    #region QuestLists

    public static void AddQuestList(List<Quest> quests, string f = "Add")
    {
        AddHeaderGroup();
        SetRegionGroupWidth(182);
        AddHeaderRegion(() =>
        {
            if (f == "Add")
                AddLine("Available quests:");
            else if (f == "Turn")
                AddLine("Quests ready to turn in:");
        });
        foreach (var quest in quests)
        {
            AddButtonRegion(() =>
            {
                AddLine((settings.questLevel.Value() ? "[" + quest.questLevel + "] " : "") + quest.name, "Black");
                AddSmallButton(quest.ZoneIcon());
            },
            (h) =>
            {
                Quest.quest = quest;
                if (f == "Add")
                {
                    if (staticPagination.ContainsKey("QuestAdd"))
                        staticPagination.Remove("QuestAdd");
                    if (!Respawn("QuestAdd")) PlaySound("DesktopWriteQuest" + random.Next(1, 4));
                    CloseWindow("QuestTurn");
                }
                else if (f == "Turn")
                {
                    if (staticPagination.ContainsKey("QuestTurn"))
                        staticPagination.Remove("QuestTurn");
                    if (quest.rewards != null && quest.rewards.Count == 1) Quest.chosenReward = quest.rewards.ToList()[0].Key;
                    else Quest.chosenReward = null;
                    if (!Respawn("QuestTurn")) PlaySound("DesktopWriteQuest" + random.Next(1, 4));
                    CloseWindow("QuestAdd");
                }
                Respawn("Chest", true);
                Respawn("PlayerMoney", true);
                Respawn("Capital", true);
                Respawn("InstanceWing", true);
                Respawn("Instance", true);
                Respawn("Complex", true);
            });
            var color = ColorQuestLevel(quest.questLevel);
            if (color != null) SetRegionBackgroundAsImage("SkillUp" + color);
        }
    }

    #endregion

    #region FluidBar

    public static void AddSkillBar(int x, int y, Profession profession, Entity entity)
    {
        var skillBar = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabSkillBar"));
        skillBar.transform.parent = CDesktop.LBWindow().LBRegionGroup().LBRegion().transform;
        skillBar.transform.localPosition = new Vector3(x, y, 0);
        var thisBar = skillBar.GetComponent<FluidBar>();
        var maxSkill = profession.levels.Where(x => entity.professionSkills[profession.name].Item2.Contains(x.name)).Max(x => x.maxSkill);
        var skill = entity.professionSkills[profession.name].Item1;
        thisBar.Initialise(150, () => maxSkill - 1, () => skill - 1, false);
        thisBar.UpdateFluidBar();
    }

    public static void AddResourceBar(int x, int y, string resource, int forWho, Entity entity)
    {
        var resourceBar = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabResourceBar"));
        resourceBar.transform.parent = CDesktop.LBWindow().transform;
        resourceBar.transform.localPosition = new Vector3(x, y, 0);
        var thisBar = resourceBar.GetComponent<FluidBar>();
        resourceBar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/FluidBars/ResourceBar/Resource" + resource + "Bar/FilledBar");
        thisBar.Initialise(entity.MaxResource(resource) * 8, () => entity.MaxResource(resource), () => entity.resources[resource], true);
        thisBar.split.sprite = Resources.Load<Sprite>("Sprites/FluidBars/ResourceBar/Resource" + resource + "Bar/Splitter");
        thisBar.GetComponentsInChildren<SpriteRenderer>().First(x => x.name == "Capstone").sprite = Resources.Load<Sprite>("Sprites/FluidBars/ResourceBar/Resource" + resource + "Bar/Capstone");
        if (board.resourceBars.ContainsKey(forWho))
            if (board.resourceBars[forWho].ContainsKey(resource)) board.resourceBars[forWho][resource] = thisBar;
            else board.resourceBars[forWho].Add(resource, thisBar);
        else board.resourceBars.Add(forWho, new() { { resource, thisBar } });
        thisBar.UpdateFluidBar();
    }

    public static void AddHealthBar(int x, int y, int forWho, Entity entity)
    {
        var healthBar = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/PrefabHealthBar"));
        healthBar.name += " " + entity.name;
        healthBar.transform.parent = CDesktop.LBWindow().LBRegionGroup().LBRegion().transform;
        healthBar.transform.localPosition = new Vector3(x, y, 0);
        var thisBar = healthBar.GetComponent<FluidBar>();
        thisBar.Initialise(150, () => entity.MaxHealth(), () => entity.health, false);
        if (board.healthBars.ContainsKey(forWho)) board.healthBars[forWho] = thisBar;
        else board.healthBars.Add(forWho, thisBar);
        thisBar.UpdateFluidBar();
    }

    #endregion

    #region Static Pagination

    //Saved static pagination
    public static Dictionary<string, int> staticPagination;

    public static void PreparePagination(this Window w)
    {
        if (!staticPagination.ContainsKey(w.title))
            staticPagination.Add(w.title, 0);
    }

    public static void CorrectPagination(this Window w)
    {
        var pg = w.pagination();
        var mpg = w.maxPagination();
        if (pg > mpg && !w.paginateFullPages) staticPagination[w.title] = mpg;
        else if (pg > Math.Ceiling((double)mpg / w.perPage) * w.perPage && w.paginateFullPages) staticPagination[w.title] = (int)Math.Ceiling((double)mpg / w.perPage) * w.perPage;
        else if (pg < 0) staticPagination[w.title] = 0;
    }

    public static void SetPagination(this Window w, int to)
    {
        w.PreparePagination();
        staticPagination[w.title] = to;
        w.CorrectPagination();
    }

    public static void IncrementPagination(this Window w)
    {
        w.PreparePagination();
        if (w.paginateFullPages) staticPagination[w.title] += w.perPage;
        else staticPagination[w.title]++;
        w.CorrectPagination();
    }

    public static void IncrementPaginationEuler(this Window w)
    {
        w.PreparePagination();
        if (w.paginateFullPages) staticPagination[w.title] += ((int)Math.Round(EuelerGrowth()) / 2) * w.perPage;
        else staticPagination[w.title] += (int)Math.Round(EuelerGrowth()) / 2;
        w.CorrectPagination();
    }

    public static void DecrementPagination(this Window w)
    {
        w.PreparePagination();
        if (w.paginateFullPages) staticPagination[w.title] -= w.perPage;
        else staticPagination[w.title]--;
        w.CorrectPagination();
    }

    public static void DecrementPaginationEuler(this Window w)
    {
        w.PreparePagination();
        if (w.paginateFullPages) staticPagination[w.title] -= ((int)Math.Round(EuelerGrowth()) / 2) * w.perPage;
        else staticPagination[w.title] -= (int)Math.Round(EuelerGrowth()) / 2;
        w.CorrectPagination();
    }

    #endregion

    #region General

    //Rolls a chance with a provided % of something happening [0 - 100]
    public static bool Roll(double chance) => chance > 0 && (chance >= 100 || random.Next(0, 100000) < chance * 1000);

    //Set what highlightible object mouse is currently hovering over
    public static void SetMouseOver(Highlightable highlightable) => mouseOver = highlightable;

    //Bezier function
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return (((-p0 + 3 * (p1 - p2) + p3) * t + (3 * (p0 + p2) - 6 * p1)) * t + 3 * (p1 - p0)) * t + p0;
    }

    //Euler function
    public static float EuelerGrowth()
    {
        return (float)Math.Pow(keyStack / 150.0 + 1.0, Math.E);
    }

    //Compares values with a operator provided in the form of a string
    public static bool CompareValues(double x, double y, string compare)
    {
        if (compare == ">=") return x >= y;
        if (compare == ">") return x > y;
        if (compare == "<=") return x <= y;
        if (compare == "<") return x < y;
        if (compare == "==") return x == y;
        if (compare == "!=") return x != y;
        if (compare == "<>") return x != y;
        return false;
    }

    //Compares values with a operator provided in the form of a string
    public static bool CompareValues(string x, string y, string compare)
    {
        if (compare == "==") return x == y;
        if (compare == "!=") return x != y;
        if (compare == "<>") return x != y;
        return false;
    }

    //Converts a number into the roman notation
    public static string ToRoman(int number)
    {
        if (number < 0 || number > 3999) return "";
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        return "";
    }

    //Converts a number into the roman notation
    public static string FormatTime(int seconds, int minutes = 0)
    {
        var sec = seconds + minutes * 60;
        var min = sec / 60;
        sec %= 60;
        return (min * 60 + " minutes ") + sec + " seconds";
    }

    #endregion

    #region Enumerations

    public enum InputType
    {
        Everything,
        Letters,
        StrictLetters,
        Capitals,
        Numbers,
        Decimal
    }

    public enum RegionBackgroundType
    {
        Empty,
        Image,
        Padding,
        Header,
        Button,
        ButtonRed,
        Experience,
        ExperienceNew,
        ExperienceNone,
        ExperienceRested,
        ProgressDone,
        ProgressEmpty,
        ChartBackground
    }

    public enum Anchor
    {
        None,
        Center,
        Bottom,
        BottomRight,
        BottomLeft,
        Top,
        TopRight,
        TopLeft,
        LeftBottom,
        Left,
        LeftTop,
        RightBottom,
        Right,
        RightTop
    }

    public enum CursorType
    {
        None,
        Default,
        Click,
        Grab,
        Await,
        Write,
        Hook,
        Crosshair
    }

    #endregion
}
