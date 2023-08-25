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
        if (foo == objectName)
        {
            if (CDesktop.title == "ObjectManagerItems")
            {
                Item.item.name = foo.Value();
                CloseWindow("ObjectManagerItems");
                SpawnWindowBlueprint("ObjectManagerItems");
            }
            else if (CDesktop.title == "ObjectManagerItemSets")
            {
                ItemSet.itemSet.name = foo.Value();
                CloseWindow("ObjectManagerItemSets");
                SpawnWindowBlueprint("ObjectManagerItemSets");
            }
            else if (CDesktop.title == "ObjectManagerAbilities")
            {
                Ability.ability.name = foo.Value();
                CloseWindow("ObjectManagerAbilities");
                SpawnWindowBlueprint("ObjectManagerAbilities");
            }
            else if (CDesktop.title == "ObjectManagerBuffs")
            {
                Buff.buff.name = foo.Value();
                CloseWindow("ObjectManagerBuffs");
                SpawnWindowBlueprint("ObjectManagerBuffs");
            }
            else if (CDesktop.title == "ObjectManagerRaces")
            {
                Race.race.name = foo.Value();
                CloseWindow("ObjectManagerRaces");
                SpawnWindowBlueprint("ObjectManagerRaces");
            }
            else if (CDesktop.title == "ObjectManagerClasses")
            {
                Class.spec.name = foo.Value();
                CloseWindow("ObjectManagerClasses");
                SpawnWindowBlueprint("ObjectManagerClasses");
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
