using System.Linq;
using UnityEngine;

using static Root;
using static Font;
using static Event;
using static Cursor;
using static String;
using static SaveGame;

public class InputLine : MonoBehaviour
{
    public Region region;
    public InputText text;
    public string color;

    public void Initialise(Region region, String refText, string color = "")
    {
        this.region = region;
        this.color = Coloring.colors.ContainsKey(color) ? color : "";
        text = new GameObject("InputText", typeof(InputText)).GetComponent<InputText>();
        text.transform.parent = transform;
        text.Initialise(this, refText);

        this.region.inputLine = this;
    }

    public void Activate(int marker = 0)
    {
        cursor.SetCursor(CursorType.None);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        inputLineMarker = marker;
        inputLineName = text.inputLine.name;
        inputDestination = text.inputLine.text.text;
        region.regionGroup.window.Respawn();
    }

    public int Length() => fonts["Tahoma Bold"].Length(text.text.Value());

    public static String inputDestination;
    public static string inputLineName;

    public static void ExecuteQuit(String foo)
    {
        if (foo == promptConfirm)
            CloseWindow("ConfirmDeleteCharacter");
        else if (foo == consoleInput)
            CloseWindow("Console");
    }

    public static void ExecuteChange(String foo)
    {
        if (foo == promptConfirm)
        {
            if (CDesktop.windows.Exists(x => x.title == "ConfirmDeleteCharacter"))
            {
                if (foo.Value() == "DELETE")
                {
                    saves[GameSettings.settings.selectedRealm].RemoveAll(x => x.player.name == GameSettings.settings.selectedCharacter);
                    if (saves[GameSettings.settings.selectedRealm].Count > 0)
                        GameSettings.settings.selectedCharacter = saves[GameSettings.settings.selectedRealm].First().player.name;
                    else GameSettings.settings.selectedCharacter = "";
                    CloseWindow("ConfirmDeleteCharacter");
                    RemoveDesktopBackground();
                    Respawn("CharacterInfo");
                    Respawn("CharacterRoster");
                    Respawn("TitleScreenSingleplayer");
                }
                else
                    CloseWindow("ConfirmDeleteCharacter");
            }
        }
        else if (foo == await)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["Await"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == powerScale)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["PowerScale"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == animationSpeed)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["AnimationSpeed"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == animationArc)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["AnimationArc"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == trailStrength)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                if (int.Parse(foo.Value()) > 100) foo.Set("100");
                eventEdit.effects[selectedEffect]["TrailStrength"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == shatterDegree)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                if (int.Parse(foo.Value()) > 100) foo.Set("100");
                eventEdit.effects[selectedEffect]["ShatterDegree"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == shatterDensity)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                if (int.Parse(foo.Value()) > 100) foo.Set("100");
                eventEdit.effects[selectedEffect]["ShatterDensity"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == shatterSpeed)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ShatterSpeed"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == elementShatterDegree)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                if (int.Parse(foo.Value()) > 100) foo.Set("100");
                eventEdit.effects[selectedEffect]["ElementShatterDegree"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == elementShatterDensity)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                if (int.Parse(foo.Value()) > 100) foo.Set("100");
                eventEdit.effects[selectedEffect]["ElementShatterDensity"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == elementShatterSpeed)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ElementShatterSpeed"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == resourceAmount)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ResourceAmount"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventTrigger"))
            {
                eventEdit.triggers[selectedTrigger]["ResourceAmount"] = foo.Value();
                Respawn("ObjectManagerEventTrigger");
            }
        }
        else if (foo == changeAmount)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ChangeAmount"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == buffDuration)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["BuffDuration"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == cooldown)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerAbility"))
            {
                Ability.ability.cooldown = int.Parse(foo.Value());
            }
        }
        else if (foo == fire)
        {
            if (Ability.ability != null)
            {
                Ability.ability.cost["Fire"] = int.Parse(foo.Value());
            }
        }
        else if (foo == earth)
        {
            if (Ability.ability != null)
            {
                Ability.ability.cost["Earth"] = int.Parse(foo.Value());
            }
        }
        else if (foo == water)
        {
            if (Ability.ability != null)
            {
                Ability.ability.cost["Water"] = int.Parse(foo.Value());
            }
        }
        else if (foo == air)
        {
            if (Ability.ability != null)
            {
                Ability.ability.cost["Air"] = int.Parse(foo.Value());
            }
        }
        else if (foo == frost)
        {
            if (Ability.ability != null)
            {
                Ability.ability.cost["Frost"] = int.Parse(foo.Value());
            }
        }
        else if (foo == decay)
        {
            if (Ability.ability != null)
            {
                Ability.ability.cost["Decay"] = int.Parse(foo.Value());
            }
        }
        else if (foo == shadow)
        {
            if (Ability.ability != null)
            {
                Ability.ability.cost["Shadow"] = int.Parse(foo.Value());
            }
        }
        else if (foo == order)
        {
            if (Ability.ability != null)
            {
                Ability.ability.cost["Order"] = int.Parse(foo.Value());
            }
        }
        else if (foo == arcane)
        {
            if (Ability.ability != null)
            {
                Ability.ability.cost["Arcane"] = int.Parse(foo.Value());
            }
        }
        else if (foo == lightning)
        {
            if (Ability.ability != null)
            {
                Ability.ability.cost["Lightning"] = int.Parse(foo.Value());
            }
        }
        else if (foo == earth)
        {
            if (Ability.ability != null)
            {
                Ability.ability.cost["Earth"] = int.Parse(foo.Value());
            }
        }
        else if (foo == chance)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["Chance"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventTrigger"))
            {
                eventEdit.triggers[selectedTrigger]["Chance"] = foo.Value();
                Respawn("ObjectManagerEventTrigger");
            }
        }
        else if (foo == chanceBase)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ChanceBase"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventTrigger"))
            {
                eventEdit.triggers[selectedTrigger]["ChanceBase"] = foo.Value();
                Respawn("ObjectManagerEventTrigger");
            }
        }
        else if (foo == chanceScale)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ChanceScale"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventTrigger"))
            {
                eventEdit.triggers[selectedTrigger]["ChanceScale"] = foo.Value();
                Respawn("ObjectManagerEventTrigger");
            }
        }
        else if (foo == search)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerItems"))
            {
                Respawn("ObjectManagerItems");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerItemSets"))
            {
                Respawn("ObjectManagerItemSets");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerAbilities"))
            {
                Respawn("ObjectManagerAbilities");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerBuffs"))
            {
                Respawn("ObjectManagerBuffs");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerRaces"))
            {
                Respawn("ObjectManagerRaces");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerSpecs"))
            {
                Respawn("ObjectManagerSpecs");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerHostileAreas"))
            {
                Respawn("ObjectManagerHostileAreas");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerInstances"))
            {
                Respawn("ObjectManagerInstances");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerComplexes"))
            {
                Respawn("ObjectManagerComplexes");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerTowns"))
            {
                Respawn("ObjectManagerTowns");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerTriggerList"))
            {
                Respawn("ObjectManagerTriggerList");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEffectList"))
            {
                Respawn("ObjectManagerEffectList");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerPortraitList"))
            {
                Respawn("ObjectManagerPortraitList");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerAbilityIconList"))
            {
                Respawn("ObjectManagerAbilityIconList");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerItemIconList"))
            {
                Respawn("ObjectManagerItemIconList");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerAmbienceList"))
            {
                Respawn("ObjectManagerAmbienceList");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerSoundsList"))
            {
                Respawn("ObjectManagerSoundsList");
            }
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
            {
                Race.race.vitality = double.Parse(foo.Value());
            }
        }
        else if (foo == price)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                Item.item.price = double.Parse(foo.Value());
            }
        }
        else if (foo == itemPower)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                Item.item.ilvl = int.Parse(foo.Value());
            }
        }
        else if (foo == requiredLevel)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                Item.item.lvl = int.Parse(foo.Value());
            }
        }
        else if (foo == stamina)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                Item.item.stats.stats["Stamina"] = int.Parse(foo.Value());
            }
        }
        else if (foo == strength)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                Item.item.stats.stats["Strength"] = int.Parse(foo.Value());
            }
        }
        else if (foo == agility)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                Item.item.stats.stats["Agility"] = int.Parse(foo.Value());
            }
        }
        else if (foo == intellect)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                Item.item.stats.stats["Intellect"] = int.Parse(foo.Value());
            }
        }
        else if (foo == spirit)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                Item.item.stats.stats["Spirit"] = int.Parse(foo.Value());
            }
        }
        else if (foo == consoleInput)
        {
            CloseWindow("Console");
            if (foo.Value() == "d")
            {
                SpawnDesktopBlueprint("DevPanel");
            }
            else if (foo.Value() == "avglvl")
            {
                Debug.Log(string.Join('\n', SiteHostileArea.areas.FindAll(x => x.recommendedLevel > 0).GroupBy(x => x.zone).Select(x => (x.Key, x.Average(y => y.recommendedLevel))).OrderBy(x => x.Item2).Select(x => x.Key + ": " + System.Math.Round(x.Item2))));
            }
            else if (foo.Value() == "showsites")
            {
                showSitesUnconditional = true;
                CDesktop.ReloadAssets();
            }
            else if (foo.Value() == "hidesites")
            {
                showSitesUnconditional = false;
                CDesktop.ReloadAssets();
            }
            else if (foo.Value() == "showlockedareas")
            {
                showAreasUnconditional = true;
            }
            else if (foo.Value() == "hidelockedareas")
            {
                showAreasUnconditional = false;
            }
            else if (foo.Value() == "disablebounds")
            {
                disableCameraBounds = true;
            }
            else if (foo.Value() == "enablebounds")
            {
                disableCameraBounds = false;
            }
            else if (foo.Value() == "exploreall")
            {
                foreach (var site in SiteHostileArea.areas)
                    if (!currentSave.siteVisits.ContainsKey(site.name))
                        currentSave.siteVisits.Add(site.name, 1);
                foreach (var site in SiteTown.towns)
                    if (!currentSave.siteVisits.ContainsKey(site.name))
                        currentSave.siteVisits.Add(site.name, 1);
                foreach (var site in SiteInstance.instances)
                    if (!currentSave.siteVisits.ContainsKey(site.name))
                        currentSave.siteVisits.Add(site.name, 1);
                foreach (var site in SiteComplex.complexes)
                    if (!currentSave.siteVisits.ContainsKey(site.name))
                        currentSave.siteVisits.Add(site.name, 1);
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
                    CDesktop.cameraDestination = new Vector2(site.x, site.y) * 19;
                    Respawn("Site: " + prev);
                    if (!currentSave.siteVisits.ContainsKey(site.name))
                    {
                        currentSave.siteVisits.Add(site.name, 0);
                        Sound.PlaySound("DesktopZoneDiscovered", 0.3f);
                        currentSave.player.ReceiveExperience(20);
                    }
                    Respawn("Site: " + site.name);
                    foreach (var connection in SitePath.paths.FindAll(x => x.sites.Contains(site.name)))
                    {
                        var didRespawn = Respawn("Site: " + connection.sites.Find(x => x != site.name));
                        if (!didRespawn) CDesktop.LBWindow.GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.gameObject.AddComponent<FadeIn>());
                    }
                    CDesktop.LockScreen();
                }
            }
            else if (foo.Value().StartsWith("money"))
            {
                var amount = double.Parse(foo.Value().Substring(5));
                currentSave.player.inventory.money += amount;
            }
            else if (foo.Value().StartsWith("builderspacing"))
            {
                builderSpacing = int.Parse(foo.Value()[15..]);
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
            else if (foo.Value().StartsWith("exp"))
            {
                currentSave.player.ReceiveExperience(int.Parse(foo.Value()[4..]));
            }
            else if (foo.Value().StartsWith("additem"))
            {
                var item = Item.items.Find(x => x.name.ToLower() == foo.Value()[8..].ToLower());
                if (item != null) currentSave.player.inventory.AddItem(item.CopyItem(1));
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
    }
}
