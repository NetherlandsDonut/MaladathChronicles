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

    public KeyCode key;
    public Action action;
    public bool keyDown;
}
