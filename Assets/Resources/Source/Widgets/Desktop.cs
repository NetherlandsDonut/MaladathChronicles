using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

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

public class Desktop : MonoBehaviour
{
    public Window LBWindow;
    public List<Window> windows;
    public Camera screen;
    public GameObject transition;
    public string title;
    public Tooltip tooltip;
    public float tooltipChanneling;
    public List<Hotkey> hotkeys;
    public GameObject screenlock;
    public bool screenLocked;
    public Vector2 cameraDestination;
    public string queuedSiteOpen;
    public List<(SitePath, List<Transform>)> queuedPath;

    public void Initialise(string title)
    {
        this.title = title;
        windows = new();
        hotkeys = new();
        queuedSiteOpen = "";
        queuedPath = new();
        cameraDestination = new Vector2();
    }

    public void RespawnAll(bool onlyThoseWithMatchingInput = false)
    {
        Debug.Log("RESPAWNED ALL");
        for (int i = windows.Count - 1; i >= 0; i--)
            if (!onlyThoseWithMatchingInput || windows[i].regionGroups.Any(x => x.regions.Any(y => y.inputLine != null && y.inputLine.text.text == inputDestination)))
                windows[i].Respawn();
    }

    public void RebuildAll()
    {
        //Debug.Log("REBUILT ALL");
        //for (int i = windows.Count - 1; i >= 0; i--)
        //    windows[i].Respawn();
    }

    public void SetTooltip(Tooltip tooltip)
    {
        this.tooltip = tooltip;
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

    public void Start()
    {
        if (title.Contains("Map"))
            screen.transform.localPosition = new Vector3(-1000, 1000);
    }

    public void ReloadAssets()
    {
        PlaySound("DesktopMagicClick");
        Starter.LoadData();
        if (Board.board != null)
        {
            Board.board.playerCombatAbilities = Board.board.playerCombatAbilities.ToDictionary(x => Ability.abilities.Find(y => x.Key.name == y.name), x => x.Value);
            Board.board.enemyCombatAbilities = Board.board.enemyCombatAbilities.ToDictionary(x => Ability.abilities.Find(y => x.Key.name == y.name), x => x.Value);
        }
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
        if (animatedSpriteTime > 0)
            animatedSpriteTime -= Time.deltaTime;
        if (animatedSpriteTime <= 0)
        {
            animatedSpriteTime = 0.1f;
            AnimatedSprite.globalIndex++;
            if (AnimatedSprite.globalIndex == 24)
                AnimatedSprite.globalIndex = 0;
        }
        soundsPlayedThisFrame = 0;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift))
            CloseWindow("Tooltip");
        if (mouseOver != null)
        {
            if (mouseOver.pressedState == "None")
            {
                if (Input.GetMouseButtonDown(0))
                    mouseOver.MouseDown("Left");
                if (Input.GetMouseButtonDown(1))
                    mouseOver.MouseDown("Right");
                if (Input.GetMouseButtonDown(2))
                    mouseOver.MouseDown("Middle");
            }
            else if (Input.GetMouseButtonUp(0))
                mouseOver.MouseUp("Left");
            else if (Input.GetMouseButtonUp(1))
                mouseOver.MouseUp("Right");
            else if (Input.GetMouseButtonUp(2))
                mouseOver.MouseUp("Middle");
        }
        if (title == "GameSimulation" && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseDesktop("GameSimulation");
            CDesktop.UnlockScreen();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            ReloadAssets();
        }
        if (!GameSettings.settings.music.Value())
        {
            if (ambience.volume > 0) ambience.volume -= 0.002f;
        }
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
            for (int i = 0; i < 20; i++)
            {
                titleScreenFunnyEffect = new();
                var site = loadSites[0];
                loadingScreenObjectLoad++;
                var spawn = SpawnWindowBlueprint(site);
                if (spawn != null && !cameraBoundaryPoints.Contains(spawn.transform.position))
                    cameraBoundaryPoints.Add(spawn.transform.position);
                loadSites.RemoveAt(0);
                loadingBar[1].transform.localScale = new Vector3((int)(357.0 / loadingScreenObjectLoadAim * loadingScreenObjectLoad), 1, 1);
                if (loadSites.Count == 0)
                {
                    RemoveDesktopBackground();
                    SpawnWindowBlueprint("MapToolbarShadow");
                    SpawnWindowBlueprint("MapToolbarClockLeft");
                    SpawnWindowBlueprint("MapToolbar");
                    SpawnWindowBlueprint("MapToolbarClockRight");
                    SpawnWindowBlueprint("MapToolbarStatusLeft");
                    SpawnWindowBlueprint("MapToolbarStatusRight");
                    SpawnWindowBlueprint("MapLocationInfo");
                    SpawnWindowBlueprint("ExperienceBarBorder");
                    SpawnWindowBlueprint("ExperienceBar");
                    grid.SwitchMapTexture(currentSave.playerDead);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    Destroy(loadingBar[0]);
                    Destroy(loadingBar[1]);
                    loadingBar = null;
                    PlaySound("DesktopLoadSuccess");
                    screen.transform.localPosition = cameraDestination;
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
                    titleScreenFunnyEffect = foo[random.Next(foo.Count)].points;
                    lastFunnyEffectTime = 0;
                    lastFunnyEffectPosition = screen.transform.localPosition = new Vector3(titleScreenFunnyEffect[0].Item1, titleScreenFunnyEffect[0].Item2);
                    SpawnTransition();
                }
                else
                {
                    var temp = screen.transform.localPosition;
                    var where = new Vector2(titleScreenFunnyEffect[0].Item1, titleScreenFunnyEffect[0].Item2);
                    if (Vector2.Distance(temp, where) > 1f)
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
                            Respawn("Site: " + sitesToRespawn[i].name);
                        sitesToRespawn.RemoveAt(i);
                    }
                var temp = screen.transform.localPosition;
                if (queuedPath.Count > 0)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        queuedPath.FindAll(x => x != queuedPath[0]).SelectMany(x => x.Item2).ToList().ForEach(x => Destroy(x.gameObject));
                        queuedPath.RemoveAll(x => x != queuedPath[0]);
                        var siteA = FindSite(x => x.name == queuedPath[0].Item1.sites[0]);
                        var siteB = FindSite(x => x.name == queuedPath[0].Item1.sites[1]);
                        queuedSiteOpen = Vector2.Distance(new Vector2(siteA.x, siteA.y), queuedPath[0].Item2.Last().transform.position) < Vector2.Distance(new Vector2(siteB.x, siteB.y), queuedPath[0].Item2.Last().transform.position) ? siteA.name : siteB.name;
                    }
                    if (Vector2.Distance(temp, cameraDestination) > 5)
                    {
                        var newPosition = Vector3.Lerp(temp, cameraDestination, Time.deltaTime * (queuedPath[0].Item1.means == "Land" ? currentSave.player.Speed() : 9999));
                        cursor.transform.position += newPosition - temp;
                        screen.transform.localPosition = newPosition;
                    }
                    else
                    {
                        if (queuedPath[0].Item2.Count > 0)
                        {
                            if (queuedPath[0].Item2.Count % 2 == 0 && queuedPath[0].Item1.means == "Land")
                            {
                                var what = groundData[Math.Abs((int)queuedPath[0].Item2[0].position.x / 19), Math.Abs((int)queuedPath[0].Item2[0].position.y / 19)];
                                PlaySound("Step" + what + random.Next(1, 6), what == "Sand" ? 0.4f : 0.5f);
                            }
                            currentSave.AddTime(queuedPath[0].Item1.fixedDuration != 0 ? queuedPath[0].Item1.fixedDuration : currentSave.player.TravelPassTime());
                            Destroy(queuedPath[0].Item2.First(x => x.name == "PathDot").gameObject);
                            queuedPath[0].Item2.RemoveAt(0);
                            if (queuedPath[0].Item2.Count == 0)
                            {
                                if (queuedPath[0].Item1.means == "Tram")
                                    PlaySound("TramStop", 0.4f);
                                else if (queuedPath[0].Item1.means == "Zeppelin")
                                    PlaySound("ZeppelinStop", 0.25f);
                                else if (queuedPath[0].Item1.means == "Ship")
                                    PlaySound("ShipStop", 0.25f);
                                else if (queuedPath[0].Item1.means == "Darnassus")
                                    PlaySound("TeleportStop", 0.3f);
                                queuedPath.RemoveAt(0);
                            }
                        }
                        if (queuedPath.Count == 0)
                        {
                            UnlockScreen();
                            currentSave.currentSite = queuedSiteOpen;
                            var find = FindSite(x => x.name == queuedSiteOpen);
                            if (!currentSave.siteVisits.ContainsKey(queuedSiteOpen))
                            {
                                currentSave.siteVisits.Add(queuedSiteOpen, 0);
                                PlaySound("DesktopZoneDiscovered", 1f);
                                currentSave.player.ReceiveExperience(defines.expForExploration);
                            }
                            Respawn("Site: " + queuedSiteOpen);
                            foreach (var connection in paths.FindAll(x => x.sites.Contains(queuedSiteOpen)))
                            {
                                var site = connection.sites.Find(x => x != queuedSiteOpen);
                                if (!CDesktop.windows.Exists(x => x.title == "Site: " + site))
                                    if (!Respawn("Site: " + connection.sites.Find(x => x != queuedSiteOpen)))
                                        LBWindow.GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.gameObject.AddComponent<FadeIn>());
                            }
                            queuedSiteOpen = "";
                            cameraDestination = new Vector2(find.x, find.y);
                            Respawn("MapLocationInfo");
                        }
                        else
                        {
                            var first = queuedPath[0].Item2.First(x => x.name == "PathDot");
                            cameraDestination = first.position;
                            if (first.TryGetComponent<SpriteRenderer>(out var r))
                                r.color = queuedPath[0].Item1.means == "Tram" ? Color.yellow : (queuedPath[0].Item1.means == "Ship" ? Color.blue : Color.green);
                        }
                    }
                }
                else
                {
                    if (!disableCameraBounds) EnforceBoundary();
                    var newPosition = Vector3.Lerp(temp, cameraDestination, Time.deltaTime * 5);
                    cursor.transform.position += newPosition - temp;
                    screen.transform.localPosition = newPosition;
                    if (screenLocked && Vector3.Distance(screen.transform.localPosition, cameraDestination) <= 5)
                    {
                        currentSave.siteVisits ??= new();
                        if (currentSave.siteVisits.ContainsKey(currentSave.currentSite))
                        {
                            currentSave.siteVisits[currentSave.currentSite]++;
                            currentSave.player.QuestVisit(currentSave.currentSite);
                        }
                        else currentSave.siteVisits.Add(currentSave.currentSite, 1);
                        Respawn("Site: " + currentSave.currentSite);
                        UnlockScreen();
                        if (queuedSiteOpen == "Instance")
                        {
                            PlaySound("DesktopInstanceOpen");
                            SpawnDesktopBlueprint("Instance");
                            SwitchDesktop("Instance");
                        }
                        else if (queuedSiteOpen == "Complex")
                        {
                            PlaySound("DesktopInstanceOpen");
                            SpawnDesktopBlueprint("Complex");
                            SwitchDesktop("Complex");
                        }
                        else if (queuedSiteOpen == "HostileArea")
                        {
                            PlaySound("DesktopInstanceOpen");
                            SpawnDesktopBlueprint("HostileArea");
                            SwitchDesktop("HostileArea");
                        }
                        else if (queuedSiteOpen == "Town")
                        {
                            PlaySound("DesktopInstanceOpen");
                            SpawnDesktopBlueprint("Town");
                            SwitchDesktop("Town");
                        }
                        if (queuedSiteOpen == "SpiritHealer")
                        {
                            StopAmbience();
                            PlaySound("DesktopRevive");
                            currentSave.RevivePlayer();
                        }
                        queuedSiteOpen = "";
                    }
                }
            }
            if (screenLocked)
            {
                if (title == "Game" || title == "GameSimulation")
                {
                    if (animationTime > 0)
                        animationTime -= Time.deltaTime;
                    if (flyingMissiles.Count == 0 && animationTime <= 0 && fallingElements.Count == 0)
                    {
                        if (!Board.board.playerTurn && CursorRemote.cursorEnemy.fadeIn)
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
            if (screenLocked)
            {
                if (title == "FishingGame")
                {
                    if (animationTime > 0)
                        animationTime -= Time.deltaTime;
                    if (animationTime <= 0 && fallingElements.Count == 0)
                    {
                        if (fallingElements.Count == 0)
                        {
                            Respawn("FishingBoard");
                            Respawn("FishingBufferBoard");
                        }
                        if (canUnlockScreen) UnlockScreen();
                        else FishingBoard.fishingBoard.AnimateBoard();
                    }
                }
            }
            else
            {
                if (tooltip != null && !CDesktop.windows.Exists(x => x.title == "Tooltip"))
                {
                    tooltipChanneling -= Time.deltaTime;
                    if (tooltipChanneling <= 0 && tooltip.caller != null && tooltip.caller() != null)
                        tooltip.SpawnTooltip();
                }
                if (heldKeyTime > 0) heldKeyTime -= Time.deltaTime;
                if (inputLineName != null)
                {
                    var didSomething = false;
                    var length = inputDestination.Value().Length;
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
                    {
                        inputLineName = null;
                        UnityEngine.Cursor.lockState = CursorLockMode.None;
                        cursor.SetCursor(CursorType.Default);
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            inputDestination.Confirm();
                            ExecuteChange(inputDestination);
                            didSomething = true;
                        }
                        else
                        {
                            inputDestination.Reset();
                            ExecuteQuit(inputDestination);
                            didSomething = true;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Delete) && inputLineMarker < length)
                    {
                        heldKeyTime = 0.4f;
                        inputDestination.RemoveNextOne(inputLineMarker);
                        didSomething = true;
                    }
                    else if (Input.GetKey(KeyCode.Delete) && inputLineMarker < length && heldKeyTime <= 0)
                    {
                        heldKeyTime = 0.0245f;
                        inputDestination.RemoveNextOne(inputLineMarker);
                        didSomething = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow) && inputLineMarker > 0)
                    {
                        heldKeyTime = 0.4f;
                        inputLineMarker--;
                        didSomething = true;
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow) && inputLineMarker > 0 && heldKeyTime <= 0)
                    {
                        heldKeyTime = 0.0245f;
                        inputLineMarker--;
                        didSomething = true;
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) && inputLineMarker < length)
                    {
                        heldKeyTime = 0.4f;
                        inputLineMarker++;
                        didSomething = true;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow) && inputLineMarker < length && heldKeyTime <= 0)
                    {
                        heldKeyTime = 0.0245f;
                        inputLineMarker++;
                        didSomething = true;
                    }
                    else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.LeftControl))
                    {
                        inputDestination.Clear();
                        inputLineMarker = 0;
                        didSomething = true;
                    }
                    else if (Input.GetKey(KeyCode.V) && Input.GetKey(KeyCode.LeftControl))
                    {
                        inputDestination.Paste();
                        inputLineMarker = inputDestination.Value().Length;
                        didSomething = true;
                    }
                    else foreach (char c in Input.inputString)
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
                            if (windows.Exists(x => x.title == "ObjectManagerItems"))
                            {
                                Item.itemsSearch = Item.items.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerItems");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerItemSets"))
                            {
                                ItemSet.itemSetsSearch = ItemSet.itemSets.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerItemSets");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerAbilities"))
                            {
                                Ability.abilitiesSearch = Ability.abilities.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerAbilities");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerBuffs"))
                            {
                                Buff.buffsSearch = Buff.buffs.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerBuffs");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerRaces"))
                            {
                                Race.racesSearch = Race.races.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerRaces");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerSpecs"))
                            {
                                Spec.specsSearch = Spec.specs.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerSpecs");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerHostileAreas"))
                            {
                                SiteHostileArea.areasSearch = SiteHostileArea.areas.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerHostileAreas");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerInstances"))
                            {
                                SiteInstance.instancesSearch = SiteInstance.instances.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerInstances");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerComplexes"))
                            {
                                SiteComplex.complexesSearch = SiteComplex.complexes.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerComplexes");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerTowns"))
                            {
                                SiteTown.townsSearch = SiteTown.towns.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerTowns");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerFactions"))
                            {
                                Faction.factionsSearch = Faction.factions.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerFactions");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerAmbienceList"))
                            {
                                Assets.assets.ambienceSearch = Assets.assets.ambience.FindAll(x => x.ToLower().Contains(val));
                                Respawn("ObjectManagerAmbienceList");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerPortraitList"))
                            {
                                Assets.assets.portraitsSearch = Assets.assets.portraits.FindAll(x => x.ToLower().Contains(val));
                                Respawn("ObjectManagerPortraitList");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerSoundsList"))
                            {
                                Assets.assets.soundsSearch = Assets.assets.sounds.FindAll(x => x.ToLower().Contains(val));
                                Respawn("ObjectManagerSoundsList");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerItemIconList"))
                            {
                                Assets.assets.itemIconsSearch = Assets.assets.itemIcons.FindAll(x => x.ToLower().Contains(val));
                                Respawn("ObjectManagerItemIconList");
                            }
                            else if (windows.Exists(x => x.title == "ObjectManagerAbilityIconList"))
                            {
                                Assets.assets.abilityIconsSearch = Assets.assets.abilityIcons.FindAll(x => x.ToLower().Contains(val));
                                Respawn("ObjectManagerAbilityIconList");
                            }
                        }
                        RespawnAll(true);
                    }
                }
                else
                {
                    int helds = 0;
                    foreach (var hotkey in hotkeys.OrderByDescending(x => x.keyDown))
                        if (Input.GetKeyDown(hotkey.key) && hotkey.keyDown || Input.GetKey(hotkey.key) && !hotkey.keyDown)
                        {
                            CloseWindow("Tooltip");
                            CDesktop.tooltip = null;
                            if (Input.GetKeyDown(hotkey.key)) keyStack = 0;
                            else helds++;
                            hotkey.action();
                        }
                    if (helds > 0 && keyStack < 100) keyStack++;
                }
            }
        }
    }
}
