using UnityEngine;

using static Root;
using static Font;
using static Event;
using static Cursor;
using static String;
using System.Linq;
using UnityEngine.Assertions.Must;

public class InputLine : MonoBehaviour
{
    public Region region;
    public InputText text;
    public InputType inputType;
    public string color;

    public bool CheckInput(char letter)
    {
        switch (inputType)
        {
            case InputType.Letters:
                return char.IsLetter(letter);
            case InputType.Capitals:
                return char.IsLetter(letter);
            case InputType.Numbers:
                return char.IsDigit(letter);
            case InputType.Decimal:
                return char.IsDigit(letter) || letter == ',' && !text.text.Value().Contains(',');
            default:
                return true;
        }
    }

    public void Initialise(Region region, String refText, InputType inputType, string color = "")
    {
        this.region = region;
        this.inputType = inputType;
        this.color = Coloring.colors.ContainsKey(color) ? color : "";
        text = new GameObject("InputText", typeof(InputText)).GetComponent<InputText>();
        text.transform.parent = transform;
        text.Initialise(this, refText);

        this.region.inputLine = this;
    }

    public void Activate()
    {
        var desktop = text.inputLine.region.regionGroup.window.desktop;
        cursor.SetCursor(CursorType.None);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        inputLineMarker = 0;
        inputLine = text.inputLine;
        desktop.windows.ForEach(x => x.Rebuild());
    }

    public int Length() => font.Length(text.text.Value());

    public static InputLine inputLine;

    public static void ExecuteQuit(String foo)
    {
        if (foo == promptConfirm)
        {
            if (CDesktop.windows.Exists(x => x.title == "ConfirmDeleteCharacter"))
                CloseWindow("ConfirmDeleteCharacter");
        }
    }

    public static void ExecuteChange(String foo)
    {
        if (foo == promptConfirm)
        {
            if (CDesktop.windows.Exists(x => x.title == "ConfirmDeleteCharacter"))
            {
                if (foo.Value() == "DELETE")
                {
                    SaveGame.saves[GameSettings.settings.selectedRealm].RemoveAll(x => x.player.name == GameSettings.settings.selectedCharacter);
                    if (SaveGame.saves[GameSettings.settings.selectedRealm].Count > 0)
                        GameSettings.settings.selectedCharacter = SaveGame.saves[GameSettings.settings.selectedRealm].First().player.name;
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
                eventEdit.effects[selectedEffect]["TrailStrength"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == shatterDegree)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["ShatterDegree"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
            }
        }
        else if (foo == shatterDensity)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
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
        else if (foo == buffDuration)
        {
            if (CDesktop.windows.Exists(x => x.title == "ObjectManagerEventEffect"))
            {
                eventEdit.effects[selectedEffect]["BuffDuration"] = foo.Value();
                Respawn("ObjectManagerEventEffect");
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
            else if (CDesktop.windows.Exists(x => x.title == "ObjectManagerClasses"))
            {
                Respawn("ObjectManagerClasses");
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
                if (find != null) find.Rebuild();
            }
            else if (CDesktop.title == "ObjectManagerItemSets")
            {
                ItemSet.itemSet.name = foo.Value();
                var find = CDesktop.windows.Find(x => x.title == "ObjectManagerItemSets");
                if (find != null) find.Rebuild();
            }
            else if (CDesktop.title == "ObjectManagerAbilities")
            {
                Ability.ability.name = foo.Value();
                var find = CDesktop.windows.Find(x => x.title == "ObjectManagerAbilities");
                if (find != null) find.Rebuild();
            }
            else if (CDesktop.title == "ObjectManagerBuffs")
            {
                Buff.buff.name = foo.Value();
                var find = CDesktop.windows.Find(x => x.title == "ObjectManagerBuffs");
                if (find != null) find.Rebuild();
            }
            else if (CDesktop.title == "ObjectManagerRaces")
            {
                Race.race.name = foo.Value();
                var find = CDesktop.windows.Find(x => x.title == "ObjectManagerRaces");
                if (find != null) find.Rebuild();
            }
            else if (CDesktop.title == "ObjectManagerClasses")
            {
                Class.spec.name = foo.Value();
                var find = CDesktop.windows.Find(x => x.title == "ObjectManagerClasses");
                if (find != null) find.Rebuild();
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
        else if (foo == consoleInput)
        {
            CloseWindow(CDesktop.windows.Find(x => x.title == "Console"));
            if (foo.Value() == "DevPanel")
            {
                SpawnDesktopBlueprint("DevPanel");
            }
        }
    }
}
