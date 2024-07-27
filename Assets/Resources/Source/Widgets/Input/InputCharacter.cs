using UnityEngine;

using static Root;

public class InputCharacter : MonoBehaviour
{
    public InputText inputText;

    public void Initialise(InputText inputText)
    {
        this.inputText = inputText;
    }

    public void OnMouseUp()
    {
        var newMarker = inputText.characters.IndexOf(gameObject);
        inputText.inputLine.Activate(newMarker > inputLineMarker ? newMarker - 1 : newMarker);
    }
}
