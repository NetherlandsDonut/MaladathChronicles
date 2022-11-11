using UnityEngine;

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
        var newMarker = inputText.characters.IndexOf(gameObject) + 1;
        Root.cursor.SetCursor(Root.CursorType.None);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Root.inputLineMarker = newMarker > Root.inputLineMarker && Root.currentInputLine != "" ? newMarker - 1 : newMarker;
        Root.currentInputLine = inputText.inputLine.FindID();
        desktop.windows.ForEach(x => x.Rebuild());
    }
}
