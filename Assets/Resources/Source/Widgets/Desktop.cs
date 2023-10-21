using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Sound;
using static Cursor;
using static SaveGame;
using static InputLine;

public class Desktop : MonoBehaviour
{
    public Window LBWindow;
    public List<Window> windows;
    public Camera screen;
    public string title;
    public Tooltip tooltip;
    public float tooltipChanneling;
    public List<Hotkey> hotkeys;
    public GameObject screenlock;
    public bool screenLocked;
    public Vector2 cameraDestination;
    public string queuedSiteOpen;

    public void Initialise(string title)
    {
        this.title = title;
        windows = new();
        hotkeys = new();
        queuedSiteOpen = "";
        cameraDestination = new Vector2();
    }

    public void RespawnAll(bool onlyThoseWithMatchingInput = false)
    {
        for (int i = windows.Count - 1; i >= 0; i--)
            if (!onlyThoseWithMatchingInput || windows[i].regionGroups.Any(x => x.regions.Any(y => y.inputLine != null && y.inputLine.text.text == inputDestination)))
                windows[i].Respawn();
    }

    public void RebuildAll()
    {
        for (int i = windows.Count - 1; i >= 0; i--)
            windows[i].Respawn();
    }

    public void SetTooltip(Tooltip tooltip)
    {
        this.tooltip = tooltip;
        tooltipChanneling = 0.4f;
    }

    public void LockScreen()
    {
        screenLocked = true;
        screenlock.SetActive(true);
    }

    public void UnlockScreen()
    {
        canUnlockScreen = false;
        screenLocked = false;
        screenlock.SetActive(false);
    }

    public void Start()
    {
        if (title.Contains("Map"))
            screen.transform.localPosition = new Vector3(-1000, 1000);
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
        fallingSoundsPlayedThisFrame = 0;
        if (loadSites != null && loadSites.Count > 0)
            for (int i = 0; i < 6; i++)
            {
                var site = loadSites[0];
                loadingScreenObjectLoad++;
                var spawn = SpawnWindowBlueprint(site);
                if (spawn != null && !cameraBoundaryPoints.Contains(spawn.transform.position))
                    cameraBoundaryPoints.Add(spawn.transform.position + new Vector3(10, -10));
                loadSites.RemoveAt(0);
                loadingBar[1].transform.localScale = new Vector3((int)(357.0 / loadingScreenObjectLoadAim * loadingScreenObjectLoad), 1, 1);
                if (loadSites.Count == 0)
                {
                    RemoveDesktopBackground();
                    var rounded = new Vector2((float)Math.Round(cameraDestination.x), (float)Math.Round(cameraDestination.y));
                    var newPosition = rounded * 19;
                    cursor.transform.position += (Vector3)newPosition - screen.transform.position;
                    screen.transform.position = newPosition + new Vector2(333, -180);
                    SpawnWindowBlueprint("MapToolbarLeft");
                    SpawnWindowBlueprint("MapToolbar");
                    SpawnWindowBlueprint("MapToolbarRight");
                    SpawnWindowBlueprint("ExperienceBar");
                    grid.SwitchMapTexture(currentSave.playerDead);
                    SpawnTransition(0.1f);
                    SpawnTransition(0.1f);
                    SpawnTransition(0.1f);
                    SpawnTransition(0.1f);
                    SpawnTransition(0.1f);
                    SpawnTransition(0.1f);
                    Destroy(loadingBar[0]);
                    Destroy(loadingBar[1]);
                    loadingBar = null;
                    PlaySound("DesktopLoadSuccess");
                    break;
                }
            }
        else
        {
            if (title == "TitleScreen" && screen.GetComponent<SpriteRenderer>().sprite == null && !windows.Exists(x => x.name == "CharacterCreation"))
            {
                var amount = new Vector3(titleScreenCameraDirection < 2 ? -1 / 3f : 1 / 3f, titleScreenCameraDirection > 2 ? -1 / 3f : (titleScreenCameraDirection < 1 ? -1 / 3f : 1 / 3f));
                screen.transform.localPosition += amount;
                cursor.transform.localPosition += amount;
                if (Math.Abs(screen.transform.localPosition.x - 1762) > 750 && screen.transform.localPosition.x < 3774 || Math.Abs(screen.transform.localPosition.x - 5374) > 750 && screen.transform.localPosition.x >= 3774)
                {
                    titleScreenCameraDirection = random.Next(0, 4);
                    screen.transform.localPosition = new Vector3(random.Next(0, 2) == 0 ? 1762 : 5374, random.Next(-3683, -1567));
                    SpawnTransition();
                }
            }
            else if (title == "Map")
            {
                var temp = screen.transform.position;
                if ((float)Math.Round(temp.y / 19) != cameraDestination.y || (float)Math.Round(temp.x / 19) != cameraDestination.x)
                {
                    MapGrid.EnforceBoundary();
                    var rounded = new Vector2((float)Math.Round(cameraDestination.x), (float)Math.Round(cameraDestination.y));
                    var newPosition = Vector3.Lerp(temp, rounded * 19 + new Vector2(333, -180), Time.deltaTime * 4);
                    cursor.transform.position += newPosition - temp;
                    screen.transform.position = newPosition;
                    if (screenLocked && Vector3.Distance(screen.transform.position, (cameraDestination + new Vector2(17, -9)) * 19 + new Vector2(10, -10)) <= 10)
                    {
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
                            animationTime += frameTime;
                        if (fallingElements.Count == 0) RebuildAll();
                        if (canUnlockScreen) UnlockScreen();
                        else Board.board.AnimateBoard();
                        if (fallingElements.Count == 0) RebuildAll();
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
                                inputDestination.Insert(inputLineMarker, inputDestination.inputType == InputType.Capitals ? char.ToUpper(c) : c);
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
