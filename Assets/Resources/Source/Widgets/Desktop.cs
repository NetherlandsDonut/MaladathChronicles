using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Sound;
using static Cursor;
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

    public void Initialise(string title)
    {
        this.title = title;
        windows = new();
        hotkeys = new();
    }

    public void Rebuild()
    {
        windows.ForEach(x => x.Rebuild());
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
        if (CDesktop.name == "Desktop: TalentScreen")
            screen.transform.localPosition = new Vector3(320, -140);
        else if (CDesktop.name == "Desktop: SpellbookScreen")
            screen.transform.localPosition = new Vector3(0, -180);
        else if (CDesktop.name == "Desktop: Map")
            screen.transform.localPosition = new Vector3(-1000, 1000);
    }

    public void Update()
    {
        if (CDesktop.title == "GameSimulation" && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseDesktop("GameSimulation");
            CDesktop.UnlockScreen();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Starter.LoadData();
            if (Board.board != null)
            {
                Board.board.playerCombatAbilities = Board.board.playerCombatAbilities.Select(x => Ability.abilities.Find(y => x.name == y.name)).ToList();
                Board.board.enemyCombatAbilities = Board.board.enemyCombatAbilities.Select(x => Ability.abilities.Find(y => x.name == y.name)).ToList();
            }
            PlaySound("DesktopMagicClick");
        }
        if (queuedAmbience.Item1 != null || !GameSettings.settings.music.Value() && ambience.volume > 0)
        {
            if (!GameSettings.settings.music.Value())
            {
                if (ambience.volume > 0) ambience.volume -= 0.002f;
            }
            else if (queuedAmbience.Item1 == ambience.clip && ambience.volume < queuedAmbience.Item2)
            {
                if (queuedAmbience.Item3 && ambience.clip != queuedAmbience.Item1) ambience.volume = queuedAmbience.Item2;
                else ambience.volume += 0.002f;
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
        }
        fallingSoundsPlayedThisFrame = 0;
        if (loadSites != null && loadSites.Count > 0)
            for (int i = 0; i < 6; i++)
            {
                var site = loadSites[0];
                loadingScreenObjectLoad++;
                SpawnWindowBlueprint(site);
                loadSites.RemoveAt(0);
                loadingBar[1].transform.localScale = new Vector3((int)(357.0 / loadingScreenObjectLoadAim * loadingScreenObjectLoad), 1, 1);
                if (loadSites.Count == 0)
                {
                    RemoveDesktopBackground();
                    screen.transform.localPosition = new Vector3(2248, -2193);
                    SpawnWindowBlueprint("MapToolbar");
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
            if (CDesktop.title == "TitleScreen" && CDesktop.screen.GetComponent<SpriteRenderer>().sprite == null && !windows.Exists(x => x.name == "CharacterCreation"))
            {
                var amount = new Vector3(titleScreenCameraDirection < 2 ? -1f : 1f, titleScreenCameraDirection > 2 ? -1f : (titleScreenCameraDirection < 1 ? -1f : 1f));
                screen.transform.localPosition += amount;
                cursor.transform.localPosition += amount;
                if (Math.Abs(screen.transform.localPosition.x - 1762) > 750 && screen.transform.localPosition.x < 3774 || Math.Abs(screen.transform.localPosition.x - 5374) > 750 && screen.transform.localPosition.x >= 3774)
                {
                    titleScreenCameraDirection = random.Next(0, 4);
                    screen.transform.localPosition = new Vector3(random.Next(0, 2) == 0 ? 1762 : 5374, random.Next(-3683, -1567));
                    SpawnTransition();
                }
            }
            else if (CDesktop.title == "Map")
            {
            }
            if (screenLocked)
            {
                if (fastTravelCamera != null)
                {
                    var dest = fastTravelCamera.transform.position - new Vector3(66, 0);
                    var lerp = Vector3.zero;
                    if (Math.Abs(screen.transform.position.x - dest.x) + Math.Abs(screen.transform.position.y - dest.y) > 500)
                        lerp = Vector3.Lerp(screen.transform.position, dest, 0.02f);
                    else lerp = Vector3.LerpUnclamped(screen.transform.position, dest, 0.02f);
                    screen.transform.position = new Vector3(lerp.x, lerp.y, screen.transform.position.z);
                    if (Math.Abs(screen.transform.position.x - dest.x) + Math.Abs(screen.transform.position.y - dest.y) < 5)
                    {
                        fastTravelCamera = null;
                        UnlockScreen();
                    }
                }
                else if (title == "Game" || title == "GameSimulation")
                {
                    if (animationTime > 0)
                        animationTime -= Time.deltaTime;
                    if (flyingMissiles.Count == 0 && animationTime <= 0 && fallingElements.Count == 0)
                    {
                        animationTime = frameTime;
                        if (fallingElements.Count == 0) Rebuild();
                        if (canUnlockScreen) CDesktop.UnlockScreen();
                        else Board.board.AnimateBoard();
                        if (fallingElements.Count == 0) Rebuild();
                    }
                }
            }
            else
            {
                if (tooltip != null && tooltipChanneling > 0)
                {
                    tooltipChanneling -= Time.deltaTime;
                    if (tooltipChanneling <= 0 && tooltip.caller != null && tooltip.caller() != null)
                        tooltip.SpawnTooltip();
                }
                if (heldKeyTime > 0) heldKeyTime -= Time.deltaTime;
                if (inputLine != null)
                {
                    var temp = inputLine;
                    var didSomething = false;
                    var length = inputLine.text.text.Value().Length;
                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
                    {
                        inputLine = null;
                        cursor.SetCursor(CursorType.Default);
                        UnityEngine.Cursor.lockState = CursorLockMode.None;
                        if (Input.GetKeyDown(KeyCode.Return))
                        {
                            temp.text.text.Confirm();
                            ExecuteChange(temp.text.text);
                            didSomething = true;
                        }
                        else
                        {
                            temp.text.text.Reset();
                            ExecuteQuit(temp.text.text);
                            didSomething = true;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Delete) && inputLineMarker < length)
                    {
                        heldKeyTime = 0.4f;
                        temp.text.text.RemoveNextOne(inputLineMarker);
                        didSomething = true;
                    }
                    else if (Input.GetKey(KeyCode.Delete) && inputLineMarker < length && heldKeyTime <= 0)
                    {
                        heldKeyTime = 0.0245f;
                        temp.text.text.RemoveNextOne(inputLineMarker);
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
                        inputLine.text.text.Clear();
                        inputLineMarker = 0;
                        didSomething = true;
                    }
                    else if (Input.GetKey(KeyCode.V) && Input.GetKey(KeyCode.LeftControl))
                    {
                        inputLine.text.text.Paste();
                        inputLineMarker = inputLine.text.text.Value().Length;
                        didSomething = true;
                    }
                    else foreach (char c in Input.inputString)
                    {
                        var a = inputLineMarker;
                        if (c == '\b')
                        {
                            if (inputLineMarker > 0 && length > 0)
                                temp.text.text.RemovePreviousOne(inputLineMarker--);
                        }
                        else if (c != '\n' && c != '\r' && temp.CheckInput(c))
                        {
                            inputLine.text.text.Insert(inputLineMarker, temp.inputType == InputType.Capitals ? char.ToUpper(c) : c);
                            inputLineMarker++;
                        }
                        if (length == temp.text.text.Value().Length)
                            inputLineMarker = a;
                        didSomething = true;
                    }
                    if (didSomething)
                    {
                        if (temp.text.text == String.search)
                        {
                            var val = temp.text.text.Value().ToLower();
                            if (CDesktop.title == "ObjectManagerItems")
                            {
                                Item.itemsSearch = Item.items.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerItems");
                            }
                            else if (CDesktop.title == "ObjectManagerItemSets")
                            {
                                ItemSet.itemSetsSearch = ItemSet.itemSets.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerItemSets");
                            }
                            else if (CDesktop.title == "ObjectManagerAbilities")
                            {
                                Ability.abilitiesSearch = Ability.abilities.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerAbilities");
                            }
                            else if (CDesktop.title == "ObjectManagerBuffs")
                            {
                                Buff.buffsSearch = Buff.buffs.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerBuffs");
                            }
                            else if (CDesktop.title == "ObjectManagerRaces")
                            {
                                Race.racesSearch = Race.races.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerRaces");
                            }
                            else if (CDesktop.title == "ObjectManagerClasses")
                            {
                                Class.specsSearch = Class.specs.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerClasses");
                            }
                            else if (CDesktop.title == "ObjectManagerHostileAreas")
                            {
                                SiteHostileArea.areasSearch = SiteHostileArea.areas.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerHostileAreas");
                            }
                            else if (CDesktop.title == "ObjectManagerInstances")
                            {
                                SiteInstance.instancesSearch = SiteInstance.instances.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerInstances");
                            }
                            else if (CDesktop.title == "ObjectManagerComplexes")
                            {
                                SiteComplex.complexesSearch = SiteComplex.complexes.FindAll(x => x.name.ToLower().Contains(val));
                                Respawn("ObjectManagerComplexes");
                            }
                        }
                        temp.region.regionGroup.window.Rebuild();
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
                    if (helds > 0 && keyStack < 500) keyStack++;
                }
            }
        }
    }
}
