using System;

using UnityEngine;

public class Hotkey
{
    public Hotkey(KeyCode key, Action action, bool keyDown)
    {
        this.key = key;
        this.action = action;
        this.keyDown = keyDown;
    }

    //Key to be pressed in order to activate the action
    public KeyCode key;

    //Hotkey action that will take place after detecting the input keu
    public Action action;

    //Indicates whether the hotkey is called only once on the button press
    //On the value being false the hotkey will be called every frame as long as the key is detected
    public bool keyDown;
}
