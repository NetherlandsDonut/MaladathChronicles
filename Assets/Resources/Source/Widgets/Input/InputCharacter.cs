using UnityEngine;

using static Root;
using static Cursor;
using static InputLine;

public class InputCharacter : MonoBehaviour
{
    //Parent
    public InputText inputText;

    public void Initialise(InputText inputText)
    {
        this.inputText = inputText;
    }

    public void OnMouseUp()
    {
        var desktop = inputText.inputLine.region.regionGroup.window.desktop;
        var newMarker = inputText.characters.IndexOf(gameObject);
        cursor.SetCursor(CursorType.None);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        inputLineMarker = newMarker > inputLineMarker && inputLine != null ? newMarker - 1 : newMarker;
        inputLine = inputText.inputLine;
        desktop.windows.ForEach(x => x.Rebuild());
    }
}
