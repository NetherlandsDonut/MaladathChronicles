using UnityEngine;

using static Root;
using static Cursor;
using static InputLine;

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
        cursor.SetCursor(CursorType.None);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        inputLineMarker = newMarker > inputLineMarker ? newMarker - 1 : newMarker;
        inputLineName = inputText.inputLine.name;
        inputText.inputLine.region.regionGroup.window.Respawn();
    }
}
