using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Font
{
    public Font(string name, string charset)
    {
        this.name = name;
        glyphs = Resources.LoadAll<Sprite>("Sprites/Fonts/" + name);
        widths = glyphs.Select(x => (int)x.rect.width).ToArray();
        this.charset = charset;
    }

    //Name of the font
    public string name;

    //Textures of all characters provided by the font in the order of the charset variable
    public Sprite[] glyphs;

    //Widths of the textures, later used in calculating overall text length
    public int[] widths;

    //Provides information on how many pixels does specific text take up to be printed.
    //This is the basic way to calculate the width of regions and overally of UI
    public int Length(string text) => text.Sum(x => widths[charset.IndexOf(x)]) + text.Length - 1;

    //Set of all characters available to print in UI
    public string charset;

    //Returns a texture corresponding to the given character
    //based on the order of the characters in the charset variable
    public Sprite GetGlyph(char character)
    {
        var index = charset.IndexOf(character);
        if (index == -1) { Debug.LogError("This character was not found in the font character data: " + character); return null; }
        else if (fonts[name].glyphs.Length < index) { Debug.LogError("This character was not found in the font glyph set: " + character); return null; }
        return fonts[name].glyphs[index];
    }

    //Current font loaded into memory
    public static Dictionary<string, Font> fonts;
}
