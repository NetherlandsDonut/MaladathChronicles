using System.Linq;

using UnityEngine;

using static Root;
using static Font;
using static Event;
using static Cursor;
using static String;
using static Defines;
using static SaveGame;

public class InputLine : MonoBehaviour
{
    public Region region;
    public InputText text;
    public string color;
    public string align;

    public void Initialise(Region region, String refText, string color, string align)
    {
        this.region = region;
        this.color = Coloring.colors.ContainsKey(color) ? color : "";
        this.align = align;
        text = new GameObject("InputText", typeof(InputText)).GetComponent<InputText>();
        text.transform.parent = transform;
        text.Initialise(this, refText);
        this.region.inputLine = this;
    }

    public void Activate(int marker = 0)
    {
        cursor.SetCursor(CursorType.None);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        inputLineWindow = region.regionGroup.window.title;
        inputDestination = text.inputLine.text.text;
        inputLineMarker = marker == 0 ? text.inputLine.text.text.value.Length : marker;
        region.regionGroup.window.Respawn();
    }

    public int Length() => fonts["Tahoma Bold"].Length(text.text.Value());

    //String which is modified by interacting with the input field
    public static String inputDestination;

    //Window where the current input line resides at
    public static string inputLineWindow;

    public static void ExecuteQuit(String foo)
    {
        if (foo == promptConfirm)
            CloseWindow("ConfirmDeleteCharacter");
        else if (foo == splitAmount)
            CloseWindow("SplitItem");
        else if (foo == consoleInput)
            CloseWindow("Console");
    }

    public static void ExecuteChange(String foo)
    {
        if (foo == promptConfirm)
        {
            if (WindowUp("ConfirmDeleteCharacter"))
            {
                if (foo.Value() == "DELETE")
                {
                    saves.Remove(selectedSave);
                    selectedSave = null;
                    CloseWindow("ConfirmDeleteCharacter");
                    Sound.PlaySound("DesktopButtonClose");
                    RemoveDesktopBackground();
                    Respawn("CharacterInfo");
                    Respawn("CharacterRoster");
                    SpawnTransition();
                    SaveGames();
                }
                else CloseWindow("ConfirmDeleteCharacter");
            }
        }
        else if (foo == creationName)
        {
            CloseWindow("TitleScreenNewSaveFinish");
            Respawn("TitleScreenNewSaveFinish");
        }
        else if (foo == splitAmount)
        {
            if (WindowUp("SplitItem"))
            {
                splitDelegate();
                CloseWindow("SplitItem");
            }
        }
        else if (foo == await)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["Await"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == powerScale)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["PowerScale"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == animationSpeed)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["AnimationSpeed"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == animationArc)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["AnimationArc"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == trailStrength)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                if (int.Parse(foo.Value()) > 100) foo.Set("100");
                eventEdit.effects[selectedEffect]["TrailStrength"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == shatterDegree)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                if (int.Parse(foo.Value()) > 100) foo.Set("100");
                eventEdit.effects[selectedEffect]["ShatterDegree"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == shatterDensity)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                if (int.Parse(foo.Value()) > 100) foo.Set("100");
                eventEdit.effects[selectedEffect]["ShatterDensity"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == shatterSpeed)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ShatterSpeed"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == elementShatterDegree)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                if (int.Parse(foo.Value()) > 100) foo.Set("100");
                eventEdit.effects[selectedEffect]["ElementShatterDegree"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == elementShatterDensity)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                if (int.Parse(foo.Value()) > 100) foo.Set("100");
                eventEdit.effects[selectedEffect]["ElementShatterDensity"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == elementShatterSpeed)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ElementShatterSpeed"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == resourceAmount)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ResourceAmount"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
            else if (WindowUp("ObjectManagerEventTrigger"))
            {
                eventEdit.triggers[selectedTrigger]["ResourceAmount"] = foo.Value();
                Respawn("ObjectManagerEventTrigger");
            }
        }
        else if (foo == changeAmount)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ChangeAmount"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == buffDuration)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["BuffDuration"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == cooldown)
        {
            if (WindowUp("ObjectManagerAbility"))
                Ability.ability.cooldown = int.Parse(foo.Value());
        }
        else if (foo == fire)
        {
            if (Ability.ability != null)
                Ability.ability.cost["Fire"] = int.Parse(foo.Value());
        }
        else if (foo == earth)
        {
            if (Ability.ability != null)
                Ability.ability.cost["Earth"] = int.Parse(foo.Value());
        }
        else if (foo == water)
        {
            if (Ability.ability != null)
                Ability.ability.cost["Water"] = int.Parse(foo.Value());
        }
        else if (foo == air)
        {
            if (Ability.ability != null)
                Ability.ability.cost["Air"] = int.Parse(foo.Value());
        }
        else if (foo == frost)
        {
            if (Ability.ability != null)
                Ability.ability.cost["Frost"] = int.Parse(foo.Value());
        }
        else if (foo == decay)
        {
            if (Ability.ability != null)
                Ability.ability.cost["Decay"] = int.Parse(foo.Value());
        }
        else if (foo == shadow)
        {
            if (Ability.ability != null)
                Ability.ability.cost["Shadow"] = int.Parse(foo.Value());
        }
        else if (foo == order)
        {
            if (Ability.ability != null)
                Ability.ability.cost["Order"] = int.Parse(foo.Value());
        }
        else if (foo == arcane)
        {
            if (Ability.ability != null)
                Ability.ability.cost["Arcane"] = int.Parse(foo.Value());
        }
        else if (foo == lightning)
        {
            if (Ability.ability != null)
                Ability.ability.cost["Lightning"] = int.Parse(foo.Value());
        }
        else if (foo == earth)
        {
            if (Ability.ability != null)
                Ability.ability.cost["Earth"] = int.Parse(foo.Value());
        }
        else if (foo == chance)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["Chance"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
            else if (WindowUp("ObjectManagerEventTrigger"))
            {
                eventEdit.triggers[selectedTrigger]["Chance"] = foo.Value();
                Respawn("ObjectManagerEventTrigger");
            }
        }
        else if (foo == chanceBase)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ChanceBase"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
            else if (WindowUp("ObjectManagerEventTrigger"))
            {
                eventEdit.triggers[selectedTrigger]["ChanceBase"] = foo.Value();
                Respawn("ObjectManagerEventTrigger");
            }
        }
        else if (foo == chanceScale)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ChanceScale"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
            else if (WindowUp("ObjectManagerEventTrigger"))
            {
                eventEdit.triggers[selectedTrigger]["ChanceScale"] = foo.Value();
                Respawn("ObjectManagerEventTrigger");
            }
        }
        else if (foo == search)
        {
            if (WindowUp("ObjectManagerItems"))
                Respawn("ObjectManagerItems");
            else if (WindowUp("ObjectManagerItemSets"))
                Respawn("ObjectManagerItemSets");
            else if (WindowUp("ObjectManagerAbilities"))
                Respawn("ObjectManagerAbilities");
            else if (WindowUp("ObjectManagerBuffs"))
                Respawn("ObjectManagerBuffs");
            else if (WindowUp("ObjectManagerRaces"))
                Respawn("ObjectManagerRaces");
            else if (WindowUp("ObjectManagerSpecs"))
                Respawn("ObjectManagerSpecs");
            else if (WindowUp("ObjectManagerHostileAreas"))
                Respawn("ObjectManagerHostileAreas");
            else if (WindowUp("ObjectManagerInstances"))
                Respawn("ObjectManagerInstances");
            else if (WindowUp("ObjectManagerComplexes"))
                Respawn("ObjectManagerComplexes");
            else if (WindowUp("ObjectManagerTowns"))
                Respawn("ObjectManagerTowns");
            else if (WindowUp("ObjectManagerMounts"))
                Respawn("ObjectManagerMounts");
            else if (WindowUp("ObjectManagerFactions"))
                Respawn("ObjectManagerFactions");
            else if (WindowUp("ObjectManagerTriggerList"))
                Respawn("ObjectManagerTriggerList");
            else if (WindowUp("ObjectManagerEffectList"))
                Respawn("ObjectManagerEffectList");
            else if (WindowUp("ObjectManagerPortraitList"))
                Respawn("ObjectManagerPortraitList");
            else if (WindowUp("ObjectManagerAbilityIconList"))
                Respawn("ObjectManagerAbilityIconList");
            else if (WindowUp("ObjectManagerItemIconList"))
                Respawn("ObjectManagerItemIconList");
            else if (WindowUp("ObjectManagerMountIconList"))
                Respawn("ObjectManagerMountIconList");
            else if (WindowUp("ObjectManagerAmbienceList"))
                Respawn("ObjectManagerAmbienceList");
            else if (WindowUp("ObjectManagerSoundsList"))
                Respawn("ObjectManagerSoundsList");
        }
        else if (foo == objectName)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                Item.item.name = foo.Value();
                var find = CDesktop.windows.Find(x => x.title == "ObjectManagerItems");
                if (find != null) find.Respawn();
            }
            else if (CDesktop.title == "ObjectManagerItemSets")
            {
                ItemSet.itemSet.name = foo.Value();
                var find = CDesktop.windows.Find(x => x.title == "ObjectManagerItemSets");
                if (find != null) find.Respawn();
            }
            else if (CDesktop.title == "ObjectManagerAbilities")
            {
                Ability.ability.name = foo.Value();
                var find = CDesktop.windows.Find(x => x.title == "ObjectManagerAbilities");
                if (find != null) find.Respawn();
            }
            else if (CDesktop.title == "ObjectManagerBuffs")
            {
                Buff.buff.name = foo.Value();
                var find = CDesktop.windows.Find(x => x.title == "ObjectManagerBuffs");
                if (find != null) find.Respawn();
            }
            else if (CDesktop.title == "ObjectManagerRaces")
            {
                Race.race.name = foo.Value();
                var find = CDesktop.windows.Find(x => x.title == "ObjectManagerRaces");
                if (find != null) find.Respawn();
            }
            else if (CDesktop.title == "ObjectManagerSpecs")
            {
                Spec.spec.name = foo.Value();
                var find = CDesktop.windows.Find(x => x.title == "ObjectManagerSpecs");
                if (find != null) find.Respawn();
            }
        }
        else if (foo == vitality)
        {
            if (CDesktop.title == "ObjectManagerRaces")
                Race.race.vitality = double.Parse(foo.Value());
        }
        else if (foo == soundVolume)
        {
            if (WindowUp("ObjectManagerEventEffect"))
            {
                if (double.Parse(foo.Value()) > 1) eventEdit.effects[selectedEffect]["SoundEffectVolume"] = "1";
                else if (double.Parse(foo.Value()) < 0) eventEdit.effects[selectedEffect]["SoundEffectVolume"] = "0";
                else eventEdit.effects[selectedEffect]["SoundEffectVolume"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == price)
        {
            if (CDesktop.title == "ObjectManagerItems")
                Item.item.price = int.Parse(foo.Value());
            else if (CDesktop.title == "ObjectManagerMounts")
                Mount.mount.price = int.Parse(foo.Value());
        }
        else if (foo == itemPower)
        {
            if (CDesktop.title == "ObjectManagerItems")
                Item.item.ilvl = int.Parse(foo.Value());
        }
        else if (foo == requiredLevel)
        {
            if (CDesktop.title == "ObjectManagerItems")
                Item.item.lvl = int.Parse(foo.Value());
        }
        else if (foo == stamina)
        {
            if (CDesktop.title == "ObjectManagerItems")
                Item.item.stats["Stamina"] = int.Parse(foo.Value());
        }
        else if (foo == strength)
        {
            if (CDesktop.title == "ObjectManagerItems")
                Item.item.stats["Strength"] = int.Parse(foo.Value());
        }
        else if (foo == agility)
        {
            if (CDesktop.title == "ObjectManagerItems")
                Item.item.stats["Agility"] = int.Parse(foo.Value());
        }
        else if (foo == intellect)
        {
            if (CDesktop.title == "ObjectManagerItems")
                Item.item.stats["Intellect"] = int.Parse(foo.Value());
        }
        else if (foo == spirit)
        {
            if (CDesktop.title == "ObjectManagerItems")
                Item.item.stats["Spirit"] = int.Parse(foo.Value());
        }
        else if (foo == consoleInput)
        {
            CloseWindow("Console");
            if (foo.Value() == "d")
                SpawnDesktopBlueprint("DevPanel");
            else if (foo.Value() == "avglvl")
                Debug.Log(string.Join('\n', SiteArea.areas.FindAll(x => x.recommendedLevel[currentSave.playerSide] > 0).GroupBy(x => x.zone).Select(x => (x.Key, x.Average(y => y.recommendedLevel[currentSave.playerSide]))).OrderBy(x => x.Item2).Select(x => x.Key + ": " + System.Math.Round(x.Item2))));
            else if (foo.Value() == "showlockedareas")
                showAreasUnconditional = true;
            else if (foo.Value() == "hidelockedareas")
                showAreasUnconditional = false;
            else if (foo.Value() == "respec")
                currentSave.player.ResetTalents();
            else if (foo.Value() == "cardgame")
                SpawnDesktopBlueprint("CardGame");
            else if (foo.Value() == "cardtest")
                SpawnDesktopBlueprint("CardGame");
            else if (foo.Value() == "disablebounds")
                disableCameraBounds = true;
            else if (foo.Value() == "enablebounds")
                disableCameraBounds = false;
            else if (foo.Value() == "generateauctionables")
            {
                Auctionable.auctionables = new();
                foreach (var item in Item.items)
                {
                    if (item.unique) continue;
                    if (item.price == 0) continue;
                    if (item.source == "Quest") continue;
                    if (item.source == "DirectDrop") continue;
                    if (item.type == "Miscellaneous" && (item.abilities == null || item.abilities.Count == 0)) continue;
                    if (item.rarity == "Poor") continue;
                    if (item.indestructible == true) continue;
                    if (item.questsStarted != null) continue;
                    if (item.droppedBy != null) continue;
                    var newAuctionable = new Auctionable()
                    {
                        item = item.name,
                        minPrice = (int)(item.price * 6.2),
                        maxPrice = (int)(item.price * 16.3),
                        frequency = item.maxStack > 10 ? (item.rarity == "Common" ? 20 : (item.rarity == "Uncommon" || item.rarity == "Poor" ? 12 : (item.rarity == "Rare" ? 5 : (item.rarity == "Epic" ? 2 : 0)))) :
                                    (item.maxStack > 1 ? (item.rarity == "Common" ? 10 : (item.rarity == "Uncommon" || item.rarity == "Poor" ? 6 : (item.rarity == "Rare" ? 3 : (item.rarity == "Epic" ? 1 : 0)))) :
                                                         (item.rarity == "Common" ? 3 : (item.rarity == "Uncommon" || item.rarity == "Poor" ? (item.source == "Profession" ? 2.5f : 1.5f) : (item.rarity == "Rare" ? (item.source == "Profession" ? 1.2f : 0.8f) : (item.rarity == "Epic" ? (item.source == "Profession" ? 0.05f : 0) : 0)))))
                    };
                    if (newAuctionable.frequency > 0)
                        Auctionable.auctionables.Add(newAuctionable);
                }
                Serialization.Serialize(Auctionable.auctionables, "auctionables");
            }
            else if (foo.Value() == "exploreall")
            {
                var expSum = 0;
                foreach (var site in SiteArea.areas)
                    if (site.x != 0 && site.y != 0 && !currentSave.siteVisits.ContainsKey(site.name))
                    {
                        currentSave.siteVisits.Add(site.name, 1);
                        expSum += defines.expForExploration;
                    }
                foreach (var site in SiteInstance.instances)
                    if (site.x != 0 && site.y != 0 && !currentSave.siteVisits.ContainsKey(site.name))
                    {
                        currentSave.siteVisits.Add(site.name, 1);
                        expSum += defines.expForExploration;
                    }
                foreach (var site in SiteComplex.complexes)
                    if (site.x != 0 && site.y != 0 && !currentSave.siteVisits.ContainsKey(site.name))
                    {
                        currentSave.siteVisits.Add(site.name, 1);
                        expSum += defines.expForExploration;
                    }
                currentSave.player.ReceiveExperience(expSum);
                CDesktop.ReloadAssets();
            }
            else if (foo.Value().StartsWith("tele"))
            {
                var site = Site.FindSite(x => x.x != 0 && x.y != 0 && x.name.ToLower().Contains(foo.Value().Substring(5).ToLower()));
                if (site != null)
                {
                    var prev = currentSave.currentSite;
                    currentSave.currentSite = site.name;
                    if (!currentSave.siteVisits.ContainsKey(site.name))
                        currentSave.siteVisits.Add(site.name, 1);
                    CDesktop.cameraDestination = new Vector2(site.x, site.y);
                    Respawn("Site: " + prev);
                    if (!currentSave.siteVisits.ContainsKey(site.name))
                    {
                        currentSave.siteVisits.Add(site.name, 0);
                        Sound.PlaySound("DesktopZoneDiscovered", 0.5f);
                        currentSave.player.ReceiveExperience(defines.expForExploration);
                    }
                    Respawn("Site: " + site.name);
                    foreach (var connection in SitePath.paths.FindAll(x => x.sites.Contains(site.name)).Where(x => x.onlyFor == null || x.onlyFor == currentSave.playerSide))
                    {
                        var siteRespawn = connection.sites.Find(x => x != site.name);
                        if (!WindowUp("Site: " + siteRespawn))
                            if (!Respawn("Site: " + siteRespawn))
                                CDesktop.LBWindow().GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.gameObject.AddComponent<FadeIn>());
                    }
                    CDesktop.LockScreen();
                }
            }
            else if (foo.Value().StartsWith("money "))
            {
                var amount = int.Parse(foo.Value()[5..]);
                currentSave.player.inventory.money += amount;
            }
            else if (foo.Value().StartsWith("mount "))
            {
                var which = foo.Value()[5..].Trim();
                var mount = Mount.mounts.Find(x => x.name.ToLower() == which);
                if (mount != null && !currentSave.player.mounts.Contains(mount.name))
                {
                    Sound.PlaySound("DesktopButtonRight");
                    currentSave.player.mounts.Add(mount.name);
                }
            }
            else if (foo.Value().StartsWith("rmount "))
            {
                var which = foo.Value()[6..].Trim();
                var mount = Mount.mounts.Find(x => x.name.ToLower() == which);
                if (mount != null && currentSave.player.mounts.Contains(mount.name))
                {
                    Sound.PlaySound("DesktopButtonRight");
                    currentSave.player.mounts.Remove(mount.name);
                }
            }
            else if (foo.Value().StartsWith("bla"))
                ProfessionSetter("Blacksmithing");
            else if (foo.Value().StartsWith("fir"))
                ProfessionSetter("First Aid");
            else if (foo.Value().StartsWith("lea"))
                ProfessionSetter("Leatherworking");
            else if (foo.Value().StartsWith("alc"))
                ProfessionSetter("Alchemy");
            else if (foo.Value().StartsWith("her"))
                ProfessionSetter("Herbalism");
            else if (foo.Value().StartsWith("min"))
                ProfessionSetter("Mining");
            else if (foo.Value().StartsWith("enc"))
                ProfessionSetter("Enchanting");
            else if (foo.Value().StartsWith("eng"))
                ProfessionSetter("Engineering");
            else if (foo.Value().StartsWith("coo"))
                ProfessionSetter("Cooking");
            else if (foo.Value().StartsWith("tai"))
                ProfessionSetter("Tailoring");
            else if (foo.Value().StartsWith("ski"))
                ProfessionSetter("Skinning");
            else if (foo.Value().StartsWith("fis"))
                ProfessionSetter("Fishing");
            else if (foo.Value().StartsWith("addtime "))
            {
                var amount = int.Parse(foo.Value().Substring(7));
                currentSave.AddTime(amount);
            }
            else if (foo.Value().StartsWith("ft "))
            {
                var amount = foo.Value().Substring(3);
                SpawnFallingText(new Vector2(0, 34), amount);
            }
            else if (foo.Value().StartsWith("showpaths"))
            {
                foreach (var path in SitePath.paths)
                    SitePath.pathsDrawn.Add(path.DrawPath());
            }
            else if (foo.Value().StartsWith("delpaths"))
            {
                var site = Site.FindSite(x => x.name.ToLower() == foo.Value().Substring(9));
                if (site != null)
                {
                    var list = SitePath.pathsConnectedToSite[site.name];
                    SitePath.paths.RemoveAll(x => list.Contains(x));
                    SitePath.pathsConnectedToSite = new();
                    for (int i = 0; i < SitePath.paths.Count; i++)
                        SitePath.paths[i].Initialise();
                }
            }
            else if (foo.Value().StartsWith("exp "))
            {
                currentSave.player.ReceiveExperience(int.Parse(foo.Value()[4..]));
            }
            else if (foo.Value().StartsWith("additem "))
            {
                var item = Item.items.Find(x => x.name.ToLower() == foo.Value()[8..].ToLower());
                if (item != null)
                {
                    var copy = item.CopyItem(1);
                    copy.SetRandomEnchantment();
                    currentSave.player.inventory.AddItem(copy);
                }
            }
            foo.Set("");
        }
        else if (encounterLevels.ToList().Exists(x => x.Value.Item1 == foo))
        {
            var find = encounterLevels.ToList().Find(x => x.Value.Item1 == foo);
            find.Key.levelMin = int.Parse(foo.Value());
        }
        else if (encounterLevels.ToList().Exists(x => x.Value.Item2 == foo))
        {
            var find = encounterLevels.ToList().Find(x => x.Value.Item2 == foo);
            find.Key.levelMax = int.Parse(foo.Value());
        }

        void ProfessionSetter(string profession)
        {
            if (foo.Value()[3..].StartsWith("r"))
            {
                if (currentSave.player.professionSkills.ContainsKey(profession))
                    currentSave.player.professionSkills.Remove(profession);
                Sound.PlaySound("DesktopCantClick");
            }
            else
            {
                var amount = int.Parse(foo.Value()[3..]);
                if (currentSave.player.professionSkills.ContainsKey(profession))
                    currentSave.player.professionSkills[profession] = (amount, currentSave.player.professionSkills[profession].Item2);
                Sound.PlaySound("DesktopButtonRight");
            }
        }
    }
}
