using UnityEngine;

using static Root;
using static Font;
using static String;

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

    public int Length() => font.Length(text.text.Value());

    public static InputLine inputLine;

    public static void ExecuteChange(String foo)
    {
        if (foo == search)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                CloseWindow("ObjectManagerItems");
                SpawnWindowBlueprint("ObjectManagerItems");
            }
            else if (CDesktop.title == "ObjectManagerItemSets")
            {
                CloseWindow("ObjectManagerItemSets");
                SpawnWindowBlueprint("ObjectManagerItemSets");
            }
            else if (CDesktop.title == "ObjectManagerAbilities")
            {
                CloseWindow("ObjectManagerAbilities");
                SpawnWindowBlueprint("ObjectManagerAbilities");
            }
            else if (CDesktop.title == "ObjectManagerBuffs")
            {
                CloseWindow("ObjectManagerBuffs");
                SpawnWindowBlueprint("ObjectManagerBuffs");
            }
            else if (CDesktop.title == "ObjectManagerRaces")
            {
                CloseWindow("ObjectManagerRaces");
                SpawnWindowBlueprint("ObjectManagerRaces");
            }
            else if (CDesktop.title == "ObjectManagerClasses")
            {
                CloseWindow("ObjectManagerClasses");
                SpawnWindowBlueprint("ObjectManagerClasses");
            }
            else if (CDesktop.title == "ObjectManagerHostileAreas")
            {
                CloseWindow("ObjectManagerHostileAreas");
                SpawnWindowBlueprint("ObjectManagerHostileAreas");
            }
            else if (CDesktop.title == "ObjectManagerInstances")
            {
                CloseWindow("ObjectManagerInstances");
                SpawnWindowBlueprint("ObjectManagerInstances");
            }
            else if (CDesktop.title == "ObjectManagerComplexes")
            {
                CloseWindow("ObjectManagerComplexes");
                SpawnWindowBlueprint("ObjectManagerComplexes");
            }
        }
        else if (foo == objectName)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                Item.item.name = foo.Value();
                CDesktop.windows.Find(x => x.title == "ObjectManagerItems").Rebuild();
            }
            else if (CDesktop.title == "ObjectManagerItemSets")
            {
                ItemSet.itemSet.name = foo.Value();
                CDesktop.windows.Find(x => x.title == "ObjectManagerItemSets").Rebuild();
            }
            else if (CDesktop.title == "ObjectManagerAbilities")
            {
                Ability.ability.name = foo.Value();
                CDesktop.windows.Find(x => x.title == "ObjectManagerAbilities").Rebuild();
            }
            else if (CDesktop.title == "ObjectManagerBuffs")
            {
                Buff.buff.name = foo.Value();
                CDesktop.windows.Find(x => x.title == "ObjectManagerBuffs").Rebuild();
            }
            else if (CDesktop.title == "ObjectManagerRaces")
            {
                Race.race.name = foo.Value();
                CDesktop.windows.Find(x => x.title == "ObjectManagerRaces").Rebuild();
            }
            else if (CDesktop.title == "ObjectManagerClasses")
            {
                Class.spec.name = foo.Value();
                CDesktop.windows.Find(x => x.title == "ObjectManagerClasses").Rebuild();
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
