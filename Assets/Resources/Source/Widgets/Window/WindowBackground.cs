using UnityEngine;

public class WindowBackground : MonoBehaviour
{
    //Parent
    public Window window;

    public void OnMouseUp() => window.desktop.Focus(window);
    public void Initialise(Window window) => this.window = window;
}
