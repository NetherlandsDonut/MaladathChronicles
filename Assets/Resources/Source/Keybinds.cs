using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public static class Keybinds
{
    //Adds missing keybinds from the default list to the current one
    //Also reorders the keybinds into the default order
    public static void AddMissingKeybinds()
    {
        foreach (var keybind in defaultKeybinds)
            if (!keybinds.ContainsKey(keybind.Key))
                keybinds.Add(keybind.Key, keybind.Value);
        var indexes = defaultKeybinds.Select(x => x.Key).ToList();
        keybinds = keybinds.OrderBy(x => indexes.IndexOf(x.Key)).ToDictionary(x => x.Key, x => x.Value);
    }

    //Resets all keybinds to default values
    public static void ResetAllKeybinds()
    {
        var list = keybinds.Select(x => x.Key);
        foreach (var function in list)
            ResetKeybind(function);
    }

    //Resets a keybind to it's default value
    public static void ResetKeybind(string function)
    {
        if (!keybinds.ContainsKey(function)) return;
        if (defaultKeybinds.ContainsKey(function)) keybinds[function].key = defaultKeybinds[function].key;
        else keybinds.Remove(function);
    }

    //List of all keybinds
    public static Dictionary<string, Keybind> keybinds;

    //List of the default keybinds, no interaction from outside
    public static Dictionary<string, Keybind> defaultKeybinds = new()
    {
        { "Open menu / Back", new() { key = KeyCode.Escape } },
        { "Open console", new() { key = KeyCode.BackQuote } },

        { "Move camera north", new() { key = KeyCode.W } },
        { "Move camera west", new() { key = KeyCode.A } },
        { "Move camera south", new() { key = KeyCode.S } },
        { "Move camera east", new() { key = KeyCode.D } },
        { "Focus camera on player", new() { key = KeyCode.Space } },

        { "Open talents", new() { key = KeyCode.N } },
        { "Open inventory", new() { key = KeyCode.B } },
        { "Open spellbook", new() { key = KeyCode.P } },
        { "Open quest log", new() { key = KeyCode.L } },
        { "Open professions", new() { key = KeyCode.R } },
        { "Open character sheet", new() { key = KeyCode.C } },
        { "Open bestiary", new() { key = KeyCode.T } },
    };
}

public class Keybind
{
    public string Key() => key.ToString();

    //Key that is asigned to be pressed to execute the function
    public KeyCode key;
}
