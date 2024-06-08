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
        inputLineName = text.inputLine.name;
        inputDestination = text.inputLine.text.text;
        inputLineMarker = marker == 0 ? text.inputLine.text.text.value.Length : marker;
        region.regionGroup.window.Respawn();
    }

    public int Length() => fonts["Tahoma Bold"].Length(text.text.Value());

    public static String inputDestination;
    public static string inputLineName;

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
                    SaveGames();
                }
                else
                    CloseWindow("ConfirmDeleteCharacter");
            }
        }
        else if (foo == creationName)
        {
            CloseWindow("CharacterCreationFinish");
            Respawn("CharacterCreationFinish");
        }
        else if (foo == splitAmount)
        {
            if (CDesktop.windows.Exists(x => x.title == "SplitItem"))
            {
                splitDelegate();
                CloseWindow("SplitItem");
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
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerMounts"))
            {
                Respawn("ObjectManagerMounts");
            }
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerFactions"))
            {
                Respawn("ObjectManagerFactions");
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
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerMountIconList"))
            {
                Respawn("ObjectManagerMountIconList");
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
                Item.item.price = int.Parse(foo.Value());
            }
            else if (CDesktop.title == "ObjectManagerMounts")
            {
                Mount.mount.price = int.Parse(foo.Value());
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
            else if (foo.Value() == "genenchants")
            {
                Enchant.enchants = new();
                foreach (var recipe in Recipe.recipes.Where(x => x.profession == "Enchanting"))
                    if (recipe.name.Contains("Enchant"))
                    {
                        var tl = recipe.name.ToLower();
                        var split = recipe.name.Split(" ");
                        var newEnchant = new Enchant()
                        {
                            name = recipe.name,
                            type = split[split.Length > 1 ? 1 : 0],
                            gains = new()
                        };
                        if (newEnchant.type == "2H")
                            newEnchant.type = "Two Handed";
                        else if (newEnchant.type == "Weapon")
                            newEnchant.type = "One Handed";
                        else if (newEnchant.type == "Cloak")
                            newEnchant.type = "Back";
                        else if (newEnchant.type == "Gloves")
                            newEnchant.type = "Hands";
                        else if (newEnchant.type == "Bracer")
                            newEnchant.type = "Wrists";
                        else if (newEnchant.type == "Boots")
                            newEnchant.type = "Feet";
                        if (tl.Contains("stats"))
                        {
                            if (tl.Contains("greater"))
                            {
                                newEnchant.gains.Add("Agility", 4);
                                newEnchant.gains.Add("Stamina", 4);
                                newEnchant.gains.Add("Strength", 4);
                                newEnchant.gains.Add("Spirit", 4);
                                newEnchant.gains.Add("Intellect", 4);
                            }
                            else if (tl.Contains("lesser"))
                            {
                                newEnchant.gains.Add("Agility", 2);
                                newEnchant.gains.Add("Stamina", 2);
                                newEnchant.gains.Add("Strength", 2);
                                newEnchant.gains.Add("Spirit", 2);
                                newEnchant.gains.Add("Intellect", 2);
                            }
                            else if (tl.Contains("minor"))
                            {
                                newEnchant.gains.Add("Agility", 1);
                                newEnchant.gains.Add("Stamina", 1);
                                newEnchant.gains.Add("Strength", 1);
                                newEnchant.gains.Add("Spirit", 1);
                                newEnchant.gains.Add("Intellect", 1);
                            }
                            else
                            {
                                newEnchant.gains.Add("Agility", 3);
                                newEnchant.gains.Add("Stamina", 3);
                                newEnchant.gains.Add("Strength", 3);
                                newEnchant.gains.Add("Spirit", 3);
                                newEnchant.gains.Add("Intellect", 3);
                            }
                        }
                        if (tl.Contains("agility"))
                        {
                            if (tl.Contains("superior") && newEnchant.type == "Gloves") newEnchant.gains.Add("Agility", 15);
                            else if (tl.Contains("superior")) newEnchant.gains.Add("Agility", 9);
                            else if (tl.Contains("greater")) newEnchant.gains.Add("Agility", 7);
                            else if (tl.Contains("lesser")) newEnchant.gains.Add("Agility", 3);
                            else if (tl.Contains("minor")) newEnchant.gains.Add("Agility", 1);
                            else if (tl.Contains("mighty")) newEnchant.gains.Add("Agility", 20);
                        }
                        else if (tl.Contains("stamina"))
                        {
                            if (tl.Contains("superior")) newEnchant.gains.Add("Stamina", 9);
                            else if (tl.Contains("greater")) newEnchant.gains.Add("Stamina", 7);
                            else if (tl.Contains("lesser")) newEnchant.gains.Add("Stamina", 3);
                            else if (tl.Contains("minor")) newEnchant.gains.Add("Stamina", 1);
                            else if (tl.Contains("mighty")) newEnchant.gains.Add("Stamina", 20);
                        }
                        else if (tl.Contains("strength"))
                        {
                            if (tl.Contains("superior")) newEnchant.gains.Add("Strength", 9);
                            else if (tl.Contains("greater")) newEnchant.gains.Add("Strength", 7);
                            else if (tl.Contains("lesser")) newEnchant.gains.Add("Strength", 3);
                            else if (tl.Contains("minor")) newEnchant.gains.Add("Strength", 1);
                            else if (tl.Contains("mighty")) newEnchant.gains.Add("Strength", 20);
                        }
                        else if (tl.Contains("intellect"))
                        {
                            if (tl.Contains("superior")) newEnchant.gains.Add("Intellect", 9);
                            else if (tl.Contains("greater")) newEnchant.gains.Add("Intellect", 7);
                            else if (tl.Contains("lesser")) newEnchant.gains.Add("Intellect", 3);
                            else if (tl.Contains("minor")) newEnchant.gains.Add("Intellect", 1);
                            else if (tl.Contains("mighty")) newEnchant.gains.Add("Intellect", 20);
                        }
                        else if (tl.Contains("spirit"))
                        {
                            if (tl.Contains("superior")) newEnchant.gains.Add("Spirit", 9);
                            else if (tl.Contains("greater")) newEnchant.gains.Add("Spirit", 7);
                            else if (tl.Contains("lesser")) newEnchant.gains.Add("Spirit", 3);
                            else if (tl.Contains("minor")) newEnchant.gains.Add("Spirit", 1);
                            else if (tl.Contains("mighty")) newEnchant.gains.Add("Spirit", 20);
                        }
                        else if (tl.Contains("defense"))
                        {
                            if (tl.Contains("superior")) newEnchant.gains.Add("Armor", 70);
                            else if (tl.Contains("greater")) newEnchant.gains.Add("Armor", 50);
                            else if (tl.Contains("lesser")) newEnchant.gains.Add("Armor", 3);
                            else if (tl.Contains("minor")) newEnchant.gains.Add("Armor", 1);
                            else if (tl.Contains("mighty")) newEnchant.gains.Add("Armor", 20);
                        }
                        else if (tl.Contains("protection"))
                        {
                            if (tl.Contains("lesser")) newEnchant.gains.Add("Armor", tl.Contains("shield") ? 30 : 20);
                            else if (tl.Contains("minor")) newEnchant.gains.Add("Armor", 10);
                        }
                        else if (tl.Contains("health"))
                        {
                            if (tl.Contains("superior")) newEnchant.gains.Add("Health", 50);
                            else if (tl.Contains("greater")) newEnchant.gains.Add("Health", 35);
                            else if (tl.Contains("lesser")) newEnchant.gains.Add("Health", 15);
                            else if (tl.Contains("minor")) newEnchant.gains.Add("Health", 5);
                            else if (tl.Contains("major")) newEnchant.gains.Add("Health", 100);
                            else newEnchant.gains.Add("Health", 25);
                        }
                        else if (tl.Contains("spell power"))
                        {
                            newEnchant.gains.Add("Spell Power", 30);
                        }
                        else if (tl.Contains("winter"))
                        {
                            newEnchant.gains.Add("Frost Mastery", 1);
                        }
                        else if (tl.Contains("power"))
                        {
                            if (tl.Contains("shadow")) newEnchant.gains.Add("Shadow Mastery", 2);
                            else if (tl.Contains("fire")) newEnchant.gains.Add("Fire Mastery", 2);
                            else if (tl.Contains("frost")) newEnchant.gains.Add("Frost Mastery", 2);
                            else if (tl.Contains("order")) newEnchant.gains.Add("Order Mastery", 2);
                            else if (tl.Contains("lightning")) newEnchant.gains.Add("Lightning Mastery", 2);
                            else if (tl.Contains("water")) newEnchant.gains.Add("Water Mastery", 2);
                            else if (tl.Contains("earth")) newEnchant.gains.Add("Earth Mastery", 2);
                            else if (tl.Contains("air")) newEnchant.gains.Add("Air Mastery", 2);
                            else if (tl.Contains("decay")) newEnchant.gains.Add("Decay Mastery", 2);
                            else if (tl.Contains("arcane")) newEnchant.gains.Add("Arcane Mastery", 2);
                        }
                        Enchant.enchants.Add(newEnchant);
                    }
                Serialization.Serialize(Enchant.enchants, "enchants", false, false, prefix);
            }
            else if (foo.Value() == "exploreall")
            {
                var expSum = 0;
                foreach (var site in SiteHostileArea.areas)
                    if (!currentSave.siteVisits.ContainsKey(site.name))
                    {
                        currentSave.siteVisits.Add(site.name, 1);
                        expSum += defines.expForExploration;
                    }
                foreach (var site in SiteTown.towns)
                    if (!currentSave.siteVisits.ContainsKey(site.name))
                    {
                        currentSave.siteVisits.Add(site.name, 1);
                        expSum += defines.expForExploration;
                    }
                foreach (var site in SiteInstance.instances)
                    if (!currentSave.siteVisits.ContainsKey(site.name))
                    {
                        currentSave.siteVisits.Add(site.name, 1);
                        expSum += defines.expForExploration;
                    }
                foreach (var site in SiteComplex.complexes)
                    if (!currentSave.siteVisits.ContainsKey(site.name))
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
                        Sound.PlaySound("DesktopZoneDiscovered", 0.3f);
                        currentSave.player.ReceiveExperience(defines.expForExploration);
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
                    Sound.PlaySound("DesktopButtonPressRight");
                    currentSave.player.mounts.Add(mount.name);
                }
            }
            else if (foo.Value().StartsWith("rmount "))
            {
                var which = foo.Value()[6..].Trim();
                var mount = Mount.mounts.Find(x => x.name.ToLower() == which);
                if (mount != null && currentSave.player.mounts.Contains(mount.name))
                {
                    Sound.PlaySound("DesktopButtonPressRight");
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
            else if (foo.Value().StartsWith("coo"))
                ProfessionSetter("Cooking");
            else if (foo.Value().StartsWith("tai"))
                ProfessionSetter("Tailoring");
            else if (foo.Value().StartsWith("addtime "))
            {
                var amount = int.Parse(foo.Value().Substring(7));
                currentSave.AddTime(amount);
            }
            else if (foo.Value().StartsWith("builderspacing "))
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
            else if (foo.Value().StartsWith("exp "))
            {
                currentSave.player.ReceiveExperience(int.Parse(foo.Value()[4..]));
            }
            else if (foo.Value().StartsWith("additem "))
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

        void ProfessionSetter(string profession)
        {
            if (foo.Value().Substring(3).StartsWith("r"))
            {
                if (currentSave.player.professionSkills.ContainsKey(profession))
                    currentSave.player.professionSkills.Remove(profession);
                Sound.PlaySound("DesktopCantClick");
            }
            else
            {
                var amount = int.Parse(foo.Value().Substring(3));
                if (currentSave.player.professionSkills.ContainsKey(profession))
                    currentSave.player.professionSkills[profession] = (amount, currentSave.player.professionSkills[profession].Item2);
                Sound.PlaySound("DesktopButtonPressRight");
            }
        }
    }
}
