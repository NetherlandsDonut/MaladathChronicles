using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static UnityEngine.Input;

using static Site;
using static Root;
using static Quest;
using static Sound;
using static Cursor;
using static Defines;
using static MapGrid;
using static SaveGame;
using static SitePath;
using static InputLine;
using static FlyingMissile;
using static FallingElement;

public class Desktop : MonoBehaviour
{
    //Name of the desktop screen
    public string title;

    //List of all windows active on this desktop
    public List<Window> windows;

    //Camera that is rendering the screen of this desktop
    public Camera screen;

    //Transition object that shows desktop transition effects
    public GameObject transition;

    //List of hotkeys for this desktop
    public List<Hotkey> hotkeys;

    //Screenlock object that will cover the
    //screen if needed and prevent cursor with interacting with anything
    public GameObject screenlock;

    //Status of the screen lock
    public bool screenLocked;

    //Destination where the camera should be on this desktop
    public Vector2 cameraDestination;

    public void Start()
    {
        if (title == "Map") screen.transform.localPosition = new Vector3(-1000, 1000);
    }

    public void Initialise(string title)
    {
        this.title = title;
        windows = new();
        hotkeys = new();
        cameraDestination = new Vector2();
    }

    public void RespawnAll()
    {
        for (int i = windows.Count - 1; i >= 0; i--)
            if (windows[i].title.StartsWith("ChartColumn"))
                CloseWindow(windows[i]);
        for (int i = windows.Count - 1; i >= 0; i--)
            windows[i].Respawn();
    }

    //Last built window
    public Window LBWindow() => windows.SafeLast();

    //Is a specific window up
    public bool WindowUp(string title) => windows.Exists(x => x.title == title);

    public void SetTooltip(Tooltip tooltip)
    {
        Root.tooltip = tooltip;
        tooltipChanneling = 0.4f;
    }

    public void LockScreen()
    {
        cursor.SetCursor(CursorType.Await);
        screenLocked = true;
        screenlock.SetActive(true);
    }

    public void UnlockScreen()
    {
        cursor.SetCursor(CursorType.Default);
        canUnlockScreen = false;
        screenLocked = false;
        screenlock.SetActive(false);
    }

    public void ReloadAssets()
    {
        PlaySound("DesktopMagicClick");
        Starter.LoadData();
        if (Board.board != null)
            foreach (var participant in Board.board.participants)
                participant.combatAbilities = participant.combatAbilities.ToDictionary(x => Ability.abilities.Find(y => x.Key.name == y.name), x => x.Value);
        if (title == "Map")
        {
            var temp = cameraDestination;
            CloseDesktop(title);
            SpawnDesktopBlueprint(title);
            CDesktop.cameraDestination = temp;
        }
    }

    public void FixedUpdate()
    {
        soundsPlayedThisFrame = new();
        if (animatedSpriteTime >= 0)
        {
            animatedSpriteTime -= Time.deltaTime;
            if (animatedSpriteTime <= 0)
            {
                animatedSpriteTime = 0.1f;
                if (++AnimatedSprite.globalIndex == 24)
                    AnimatedSprite.globalIndex = 0;
            }
        }
    }

    public void Update()
    {
        if (GetKey(KeyCode.LeftControl) && GetKeyDown(KeyCode.S)) GameSettings.settings.soundEffects.Invert();
        if (GetKey(KeyCode.LeftControl) && GetKeyDown(KeyCode.M)) GameSettings.settings.music.Invert();
        if (GetKeyDown(KeyCode.LeftShift) || GetKeyUp(KeyCode.LeftShift) || GetKeyDown(KeyCode.LeftControl) || GetKeyUp(KeyCode.LeftControl)) CloseWindow("Tooltip");
        if (mouseOver != null)
        {
            if (GetMouseButtonDown(0)) mouseOver.MouseDown("Left");
            else if (GetMouseButtonDown(1)) mouseOver.MouseDown("Right");
            else if (GetMouseButtonDown(2)) mouseOver.MouseDown("Middle");
            else if (GetMouseButtonUp(0)) mouseOver.MouseUp("Left");
            else if (GetMouseButtonUp(1)) mouseOver.MouseUp("Right");
            else if (GetMouseButtonUp(2)) mouseOver.MouseUp("Middle");
        }
        if (title == "GameSimulation" && GetKeyDown(KeyCode.Escape))
        {
            CloseDesktop("GameSimulation");
            CDesktop.UnlockScreen();
        }
        if (GetKey(KeyCode.LeftControl) && GetKey(KeyCode.Tab) && GetKeyDown(KeyCode.LeftAlt)) ReloadAssets();
        if (!GameSettings.settings.music.Value() && ambience.volume > 0) ambience.volume -= 0.002f;
        else if (queuedAmbience.Item1 != ambience.clip)
        {
            if (ambience.volume > 0) ambience.volume -= 0.002f;
            else
            {
                ambience.clip = queuedAmbience.Item1;
                ambience.Play();
            }
        }
        else if (queuedAmbience.Item1 == ambience.clip && ambience.volume < queuedAmbience.Item2)
        {
            if (queuedAmbience.Item3 && ambience.clip != queuedAmbience.Item1) ambience.volume = queuedAmbience.Item2;
            else ambience.volume += 0.002f;
        }
        if (loadSites != null && loadSites.Count > 0)
            for (int i = 0; i < 10; i++)
            {
                titleScreenFunnyEffect = new();
                var site = loadSites[0];
                loadingScreenObjectLoad++;
                var spawn = SpawnWindowBlueprint(site);
                if (spawn != null && !mapGrid.cameraBoundaryPoints.Contains(spawn.transform.position))
                    mapGrid.cameraBoundaryPoints.Add(spawn.transform.position);
                loadSites.RemoveAt(0);
                loadingBar[1].transform.localScale = new Vector3((int)(357.0 / loadingScreenObjectLoadAim * loadingScreenObjectLoad), 1, 1);
                if (loadSites.Count == 0)
                {
                    RemoveDesktopBackground();
                    Respawn("MapToolbarShadow");
                    Respawn("MapToolbarClockLeft");
                    Respawn("MapToolbar");
                    Respawn("MapToolbarClockRight");
                    Respawn("MapToolbarStatusLeft");
                    Respawn("MapToolbarStatusRight");
                    Respawn("MapLocationInfo");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                    Respawn("WorldBuffs");
                    mapGrid.SwitchMapTexture(currentSave.player.dead);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    Destroy(loadingBar[0]);
                    Destroy(loadingBar[1]);
                    loadingBar = null;
                    screen.transform.localPosition = cameraDestination;
                    PlaySound("DesktopLoadSuccess");
                    break;
                }
            }
        else
        {
            if (title == "TitleScreen" && screen.GetComponent<SpriteRenderer>().sprite == null)
            {
                if (titleScreenFunnyEffect.Count == 0)
                {
                    Site rSite;
                    do rSite = SiteHostileArea.areas[random.Next(SiteHostileArea.areas.Count)];
                    while (!pathsConnectedToSite.ContainsKey(rSite.name) || pathsConnectedToSite[rSite.name].All(x => x.points.Count < 15));
                    var foo = pathsConnectedToSite[rSite.name].Where(x => x.points.Count >= 15).ToList();
                    titleScreenFunnyEffect = foo[random.Next(foo.Count)].points.ToList();
                    lastFunnyEffectTime = 0;
                    lastFunnyEffectPosition = screen.transform.localPosition = new Vector3(titleScreenFunnyEffect[0].Item1, titleScreenFunnyEffect[0].Item2);
                    SpawnTransition();
                }
                else
                {
                    var temp = screen.transform.localPosition;
                    var where = new Vector2(titleScreenFunnyEffect[0].Item1, titleScreenFunnyEffect[0].Item2);
                    if (Vector2.Distance(temp, where) > 1)
                    {
                        lastFunnyEffectTime += Time.deltaTime * 2;
                        var newPosition = Vector3.Lerp(lastFunnyEffectPosition, where, lastFunnyEffectTime);
                        cursor.transform.position += newPosition - temp;
                        screen.transform.localPosition = newPosition;
                    }
                    else
                    {
                        lastFunnyEffectTime = 0;
                        lastFunnyEffectPosition = screen.transform.localPosition;
                        titleScreenFunnyEffect.RemoveAt(0);
                    }
                }
            }
            else if (title == "Map")
            {
                if (sitesToRespawn.Count > 0)
                    for (int i = sitesToRespawn.Count - 1; i >= 0; i--)
                    {
                        if (sitesToRespawn[i] != null)
                            Respawn("Site: " + sitesToRespawn[i].name, true);
                        sitesToRespawn.RemoveAt(i);
                    }
                var temp = screen.transform.localPosition;
                if (mapGrid.queuedPath.Count > 0)
                {
                    if (GetKeyDown(KeyCode.Escape))
                    {
                        mapGrid.queuedPath.FindAll(x => x != mapGrid.queuedPath[0]).SelectMany(x => x.Item2).ToList().ForEach(x => Destroy(x.gameObject));
                        mapGrid.queuedPath.RemoveAll(x => x != mapGrid.queuedPath[0]);
                        var siteA = FindSite(x => x.name == mapGrid.queuedPath[0].Item1.sites[0]);
                        var siteB = FindSite(x => x.name == mapGrid.queuedPath[0].Item1.sites[1]);
                        mapGrid.queuedSiteOpen = Vector2.Distance(new Vector2(siteA.x, siteA.y), mapGrid.queuedPath[0].Item2.Last().transform.position) < Vector2.Distance(new Vector2(siteB.x, siteB.y), mapGrid.queuedPath[0].Item2.Last().transform.position) ? siteA.name : siteB.name;
                    }
                    if (Vector2.Distance(temp, cameraDestination) > 5)
                    {
                        var newPosition = Vector3.Lerp(temp, cameraDestination, Time.deltaTime * (mapGrid.queuedPath[0].Item1.means == "Land" ? currentSave.player.Speed() : 9999));
                        cursor.transform.position += newPosition - temp;
                        screen.transform.localPosition = newPosition;
                    }
                    else
                    {
                        if (mapGrid.queuedPath[0].Item2.Count > 0)
                        {
                            if (mapGrid.queuedPath[0].Item2.Count % 2 == 0 && mapGrid.queuedPath[0].Item1.means == "Land")
                            {
                                var what = mapGrid.groundData[Math.Abs((int)mapGrid.queuedPath[0].Item2[0].position.x / 19), Math.Abs((int)mapGrid.queuedPath[0].Item2[0].position.y / 19)];
                                PlaySound("Step" + what + random.Next(1, 6), what == "Sand" ? 0.6f : 0.7f);
                            }
                            currentSave.AddTime(mapGrid.queuedPath[0].Item1.fixedDuration != 0 ? mapGrid.queuedPath[0].Item1.fixedDuration : currentSave.player.TravelPassTime());
                            Destroy(mapGrid.queuedPath[0].Item2.First(x => x.name == "PathDot").gameObject);
                            mapGrid.queuedPath[0].Item2.RemoveAt(0);
                            if (mapGrid.queuedPath[0].Item2.Count == 0)
                            {
                                mapGrid.queuedPath[0].Item1.PlayPathEndSound();
                                mapGrid.queuedPath.RemoveAt(0);
                            }
                        }
                        if (mapGrid.queuedPath.Count == 0)
                        {
                            UnlockScreen();
                            currentSave.currentSite = mapGrid.queuedSiteOpen;
                            var find = FindSite(x => x.name == mapGrid.queuedSiteOpen);
                            if (!currentSave.Visited(mapGrid.queuedSiteOpen))
                            {
                                currentSave.siteVisits.Add(mapGrid.queuedSiteOpen, 0);
                                PlaySound("DesktopZoneDiscovered", 1f);
                                currentSave.player.ReceiveExperience(defines.expForExploration);
                            }
                            Respawn("Site: " + mapGrid.queuedSiteOpen);
                            foreach (var connection in paths.FindAll(x => x.sites.Contains(mapGrid.queuedSiteOpen)))
                            {
                                var site = connection.sites.Find(x => x != mapGrid.queuedSiteOpen);
                                if (!WindowUp("Site: " + site))
                                    if (!Respawn("Site: " + connection.sites.Find(x => x != mapGrid.queuedSiteOpen)))
                                        LBWindow().GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.gameObject.AddComponent<FadeIn>());
                            }
                            mapGrid.queuedSiteOpen = "";
                            cameraDestination = new Vector2(find.x, find.y);
                            Respawn("MapLocationInfo");
                        }
                        else
                        {
                            var first = mapGrid.queuedPath[0].Item2.First(x => x.name == "PathDot");
                            cameraDestination = first.position;
                            if (first.TryGetComponent<SpriteRenderer>(out var r))
                                r.color = mapGrid.queuedPath[0].Item1.means == "Tram" ? Color.yellow : (mapGrid.queuedPath[0].Item1.means == "Ship" ? Color.blue : Color.green);
                        }
                    }
                }
                else
                {
                    if (!disableCameraBounds) mapGrid.EnforceBoundary();
                    var newPosition = Vector3.Lerp(temp, cameraDestination, Time.deltaTime * 5);
                    cursor.transform.position += newPosition - temp;
                    screen.transform.localPosition = newPosition;
                    if (screenLocked && Vector3.Distance(screen.transform.localPosition, cameraDestination) <= 5)
                    {
                        currentSave.siteVisits ??= new();
                        if (mapGrid.queuedSiteOpen != "SpiritHealer")
                        {
                            if (currentSave.siteVisits.ContainsKey(currentSave.currentSite))
                            {
                                currentSave.siteVisits[currentSave.currentSite]++;
                                currentSave.player.QuestVisit(currentSave.currentSite);
                            }
                            else currentSave.siteVisits.Add(currentSave.currentSite, 1);
                        }
                        else
                        {
                            if (!currentSave.siteVisits.ContainsKey(currentSave.currentSite))
                            {
                                currentSave.siteVisits.Add(currentSave.currentSite, 0);
                                PlaySound("DesktopZoneDiscovered", 1f);
                                currentSave.player.ReceiveExperience(defines.expForExploration);
                            }
                            foreach (var connection in paths.FindAll(x => x.sites.Contains(currentSave.currentSite)))
                            {
                                var site = connection.sites.Find(x => x != currentSave.currentSite);
                                if (!WindowUp("Site: " + site))
                                    if (!Respawn("Site: " + connection.sites.Find(x => x != currentSave.currentSite)))
                                        LBWindow().GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.gameObject.AddComponent<FadeIn>());
                            }
                        }
                        Respawn("Site: " + currentSave.currentSite);
                        UnlockScreen();
                        if (mapGrid.queuedSiteOpen == "Instance")
                        {
                            PlaySound("DesktopInstanceOpen");
                            SpawnDesktopBlueprint("Instance");
                            SwitchDesktop("Instance");
                        }
                        else if (mapGrid.queuedSiteOpen == "Complex")
                        {
                            PlaySound("DesktopInstanceOpen");
                            SpawnDesktopBlueprint("Complex");
                            SwitchDesktop("Complex");
                        }
                        else if (mapGrid.queuedSiteOpen == "HostileArea")
                        {
                            PlaySound("DesktopInstanceOpen");
                            SpawnDesktopBlueprint("HostileArea");
                            SwitchDesktop("HostileArea");
                        }
                        else if (mapGrid.queuedSiteOpen == "Town")
                        {
                            PlaySound("DesktopInstanceOpen");
                            SpawnDesktopBlueprint("Town");
                            SwitchDesktop("Town");
                        }
                        if (mapGrid.queuedSiteOpen == "SpiritHealer")
                        {
                            StopAmbience();
                            PlaySound("DesktopRevive");
                            currentSave.RevivePlayer();
                        }
                        mapGrid.queuedSiteOpen = "";
                    }
                }
            }
            if (screenLocked)
            {
                if (title == "Game" || title == "GameSimulation")
                {
                    if (animationTime > 0) animationTime -= Time.deltaTime;
                    if (flyingMissiles.Count == 0 && animationTime <= 0 && fallingElements.Count == 0)
                    {
                        if (!Board.board.participants[Board.board.whosTurn].human && CursorRemote.cursorEnemy.fadeIn)
                            animationTime += defines.frameTime;
                        if (fallingElements.Count == 0)
                        {
                            Respawn("Board");
                            Respawn("BufferBoard");
                        }
                        if (canUnlockScreen) UnlockScreen();
                        else Board.board.AnimateBoard();
                    }
                }
            }
            else
            {
                if (tooltip != null && !WindowUp("Tooltip"))
                {
                    tooltipChanneling -= Time.deltaTime;
                    if (tooltipChanneling <= 0 && tooltip.caller != null && tooltip.caller() != null)
                        tooltip.SpawnTooltip();
                }
                if (heldKeyTime > 0) heldKeyTime -= Time.deltaTime;
                if (inputLineWindow != null)
                {
                    var didSomething = false;
                    var length = inputDestination.Value().Length;
                    if (GetKeyDown(KeyCode.Escape) || GetKeyDown(KeyCode.Return))
                    {
                        inputLineWindow = null;
                        UnityEngine.Cursor.lockState = CursorLockMode.None;
                        cursor.SetCursor(CursorType.Default);
                        if (GetKeyDown(KeyCode.Return))
                        {
                            inputDestination.Confirm();
                            ExecuteChange(inputDestination);
                            didSomething = true;
                        }
                        else
                        {
                            PlaySound("DesktopMenuClose");
                            inputDestination.Reset();
                            ExecuteQuit(inputDestination);
                            didSomething = true;
                        }
                    }
                    else if (GetKeyDown(KeyCode.Delete) && inputLineMarker < length)
                    {
                        heldKeyTime = 0.4f;
                        inputDestination.RemoveNextOne(inputLineMarker);
                        didSomething = true;
                    }
                    else if (GetKey(KeyCode.Delete) && inputLineMarker < length && heldKeyTime <= 0)
                    {
                        heldKeyTime = 0.0245f;
                        inputDestination.RemoveNextOne(inputLineMarker);
                        didSomething = true;
                    }
                    else if (GetKeyDown(KeyCode.LeftArrow) && inputLineMarker > 0)
                    {
                        heldKeyTime = 0.4f;
                        inputLineMarker--;
                        didSomething = true;
                    }
                    else if (GetKey(KeyCode.LeftArrow) && inputLineMarker > 0 && heldKeyTime <= 0)
                    {
                        heldKeyTime = 0.0245f;
                        inputLineMarker--;
                        didSomething = true;
                    }
                    else if (GetKeyDown(KeyCode.RightArrow) && inputLineMarker < length)
                    {
                        heldKeyTime = 0.4f;
                        inputLineMarker++;
                        didSomething = true;
                    }
                    else if (GetKey(KeyCode.RightArrow) && inputLineMarker < length && heldKeyTime <= 0)
                    {
                        heldKeyTime = 0.0245f;
                        inputLineMarker++;
                        didSomething = true;
                    }
                    else if (GetKey(KeyCode.A) && GetKey(KeyCode.LeftControl))
                    {
                        inputDestination.Clear();
                        inputLineMarker = 0;
                        didSomething = true;
                    }
                    else if (GetKey(KeyCode.V) && GetKey(KeyCode.LeftControl))
                    {
                        inputDestination.Paste();
                        inputLineMarker = inputDestination.Value().Length;
                        didSomething = true;
                    }
                    else foreach (char c in inputString)
                    {
                        var a = inputLineMarker;
                        if (c == '\b')
                        {
                            if (inputLineMarker > 0 && length > 0)
                                inputDestination.RemovePreviousOne(inputLineMarker--);
                        }
                        else if (c != '\n' && c != '\r' && inputDestination.CheckInput(c))
                        {
                            inputDestination.Insert(inputLineMarker, inputDestination.inputType == InputType.StrictLetters ? (inputDestination.Value().Length == 0 ? char.ToUpper(c) : char.ToLower(c)) : (inputDestination.inputType == InputType.Capitals ? char.ToUpper(c) : c));
                            inputLineMarker++;
                        }
                        if (length == inputDestination.Value().Length)
                            inputLineMarker = a;
                        didSomething = true;
                    }
                    if (didSomething)
                    {
                        if (inputDestination == String.search)
                        {
                            var val = inputDestination.Value().ToLower();
                            if (WindowUp("ObjectManagerItems"))
                            {
                                Item.itemsSearch = Item.items.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerItems");
                            }
                            else if (WindowUp("ObjectManagerItemSets"))
                            {
                                ItemSet.itemSetsSearch = ItemSet.itemSets.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerItemSets");
                            }
                            else if (WindowUp("ObjectManagerAbilities"))
                            {
                                Ability.abilitiesSearch = Ability.abilities.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerAbilities");
                            }
                            else if (WindowUp("ObjectManagerBuffs"))
                            {
                                Buff.buffsSearch = Buff.buffs.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerBuffs");
                            }
                            else if (WindowUp("ObjectManagerRaces"))
                            {
                                Race.racesSearch = Race.races.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerRaces");
                            }
                            else if (WindowUp("ObjectManagerSpecs"))
                            {
                                Spec.specsSearch = Spec.specs.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerSpecs");
                            }
                            else if (WindowUp("ObjectManagerHostileAreas"))
                            {
                                SiteHostileArea.areasSearch = SiteHostileArea.areas.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerHostileAreas");
                            }
                            else if (WindowUp("ObjectManagerInstances"))
                            {
                                SiteInstance.instancesSearch = SiteInstance.instances.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerInstances");
                            }
                            else if (WindowUp("ObjectManagerComplexes"))
                            {
                                SiteComplex.complexesSearch = SiteComplex.complexes.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerComplexes");
                            }
                            else if (WindowUp("ObjectManagerTowns"))
                            {
                                SiteTown.townsSearch = SiteTown.towns.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerTowns");
                            }
                            else if (WindowUp("ObjectManagerFactions"))
                            {
                                Faction.factionsSearch = Faction.factions.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerFactions");
                            }
                            else if (WindowUp("ObjectManagerAmbienceList"))
                            {
                                Assets.assets.ambienceSearch = Assets.assets.ambience.FindAll(x => x.ToLower().Contains(val));
                                Respawn("ObjectManagerAmbienceList");
                            }
                            else if (WindowUp("ObjectManagerPortraitList"))
                            {
                                Assets.assets.portraitsSearch = Assets.assets.portraits.FindAll(x => x.ToLower().Contains(val));
                                Respawn("ObjectManagerPortraitList");
                            }
                            else if (WindowUp("ObjectManagerSoundsList"))
                            {
                                Assets.assets.soundsSearch = Assets.assets.sounds.FindAll(x => x.ToLower().Contains(val));
                                Respawn("ObjectManagerSoundsList");
                            }
                            else if (WindowUp("ObjectManagerItemIconList"))
                            {
                                Assets.assets.itemIconsSearch = Assets.assets.itemIcons.FindAll(x => x.ToLower().Contains(val));
                                Respawn("ObjectManagerItemIconList");
                            }
                            else if (WindowUp("ObjectManagerAbilityIconList"))
                            {
                                Assets.assets.abilityIconsSearch = Assets.assets.abilityIcons.FindAll(x => x.ToLower().Contains(val));
                                Respawn("ObjectManagerAbilityIconList");
                            }
                        }
                        Respawn(inputLineWindow);
                    }
                }
                else
                {
                    int helds = 0;
                    foreach (var hotkey in hotkeys.OrderByDescending(x => x.keyDown))
                        if (GetKeyDown(hotkey.key) && hotkey.keyDown || GetKey(hotkey.key) && !hotkey.keyDown)
                        {
                            CloseWindow("Tooltip");
                            tooltip = null;
                            if (GetKeyDown(hotkey.key)) keyStack = 0;
                            else helds++;
                            hotkey.action();
                        }
                    if (helds > 0 && keyStack < 100) keyStack++;
                }
            }
        }
    }
}
