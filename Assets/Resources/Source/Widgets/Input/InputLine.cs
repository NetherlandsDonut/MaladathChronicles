using UnityEngine;

using static Root;
using static Font;
using static String;

public class InputLine : MonoBehaviour
{
    public Region region;
    public InputText text;
    public InputType inputType;

    public bool CheckInput(char letter)
    {
        switch (inputType)
        {
            case InputType.Letters:
                return char.IsLetter(letter);
            case InputType.Numbers:
                return char.IsDigit(letter);
            default:
                return true;
        }
    }

    public void Initialise(Region region, String refText, InputType inputType)
    {
        this.region = region;
        this.inputType = inputType;
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
            //if (CDesktop.title == "ObjectManagerComplexes")
                
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
