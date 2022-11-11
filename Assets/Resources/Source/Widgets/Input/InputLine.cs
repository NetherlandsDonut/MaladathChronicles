using System.Linq;
using UnityEngine;

using static Root;

public class InputLine : MonoBehaviour
{
    //Parent
    public Region region;

    //Children
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

    public string FindID() => CDesktop.globalInputLines.ToList().Find(x => x.Value == this).Key;

    public int Length() => font.Length(text.text.Value());
}
