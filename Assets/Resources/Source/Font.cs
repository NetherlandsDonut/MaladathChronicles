using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Font
{
    //Initialises a font
    public Font(string name, string charset)
    {
        this.name = name;
        glyphs = Resources.LoadAll<Sprite>("Sprites/Fonts/" + name);
        foreach (var glyph in glyphs)
        {
            var pixelList = new List<(int, int)>();
            for (int i = 0; i < glyph.textureRect.width; i++)
                for (int j = 0; j < glyph.textureRect.height; j++)
                {
                    var c = glyph.texture.GetPixel((int)glyph.textureRect.x + i, (int)glyph.textureRect.y + j);
                    if (c.r == 1 && c.g == 1 && c.b == 1 && c.a == 1)
                    {
                        if (!pixelList.Contains((i - 2, j + 1)))
                            pixelList.Add((i - 2, j + 1));
                        if (!pixelList.Contains((i - 2, j)))
                            pixelList.Add((i - 2, j));
                        if (!pixelList.Contains((i - 2, j - 1)))
                            pixelList.Add((i - 2, j - 1));
                        if (!pixelList.Contains((i - 1, j + 2)))
                            pixelList.Add((i - 1, j + 2));
                        if (!pixelList.Contains((i - 1, j + 1)))
                            pixelList.Add((i - 1, j + 1));
                        if (!pixelList.Contains((i - 1, j)))
                            pixelList.Add((i - 1, j));
                        if (!pixelList.Contains((i - 1, j - 1)))
                            pixelList.Add((i - 1, j - 1));
                        if (!pixelList.Contains((i - 1, j - 2)))
                            pixelList.Add((i - 1, j - 2));
                        if (!pixelList.Contains((i + 2, j + 1)))
                            pixelList.Add((i + 2, j + 1));
                        if (!pixelList.Contains((i + 2, j)))
                            pixelList.Add((i + 2, j));
                        if (!pixelList.Contains((i + 2, j - 1)))
                            pixelList.Add((i + 2, j - 1));
                        if (!pixelList.Contains((i + 1, j - 1)))
                            pixelList.Add((i + 1, j - 1));
                        if (!pixelList.Contains((i + 1, j - 2)))
                            pixelList.Add((i + 1, j - 2));
                        if (!pixelList.Contains((i + 1, j)))
                            pixelList.Add((i + 1, j));
                        if (!pixelList.Contains((i, j + 2)))
                            pixelList.Add((i, j + 2));
                        if (!pixelList.Contains((i + 1, j + 2)))
                            pixelList.Add((i + 1, j + 2));
                        if (!pixelList.Contains((i, j + 1)))
                            pixelList.Add((i, j + 1));
                        if (!pixelList.Contains((i, j - 1)))
                            pixelList.Add((i, j - 1));
                        if (!pixelList.Contains((i, j - 2)))
                            pixelList.Add((i, j - 2));
                    }
                }
            var texture = new Texture2D(pixelList.Max(x => x.Item1) + 5, 19, TextureFormat.ARGB32, true) { filterMode = FilterMode.Point };
            for (int i = 0; i < texture.width; i++)
                for (int j = 0; j < texture.height; j++)
                    if (pixelList.Contains((i, j - 2))) texture.SetPixel(i, j, Color.white);
                    else texture.SetPixel(i, j, new Color(0, 0, 0, 0));
            texture.Apply();
            File.WriteAllBytes(glyphs.ToList().IndexOf(glyph) + "", texture.GetRawTextureData());
        }
        glyphBorders = Resources.LoadAll<Sprite>("Sprites/FontBorders/" + name + "/");
        widths = glyphs.Select(x => (int)x.rect.width).ToArray();
        this.charset = charset;
    }

    //Name of the font
    public string name;

    //List of all characters provided by the font in the order of the charset variable
    public Sprite[] glyphs;

    //List of all characters provided by the font in the order of the charset variable
    public Sprite[] glyphBorders;

    //Widths of the textures, later used in calculating overall text length
    public int[] widths;

    //Provides information on how many pixels does specific text take up to be printed.
    //This is the basic way to calculate the width of regions and overally of UI
    public int Length(string text) => text.Sum(x => charset.IndexOf(x) == -1 ? 0 : widths[charset.IndexOf(x)]) + text.Length - 1;
    public int Length(char character) => charset.IndexOf(character) == -1 ? 0 : widths[charset.IndexOf(character)];

    //Set of all characters available to print in UI
    public string charset;

    //Returns a texture corresponding to the given character
    //based on the order of the characters in the charset variable
    public Sprite GetGlyph(char character)
    {
        var index = charset.IndexOf(character);
        if (index == -1) { Debug.LogError("This character was not found in the charset: " + character); return null; }
        else if (fonts[name].glyphs.Length < index) { Debug.LogError("This character was not found in the font glyph set: " + character); return null; }
        return fonts[name].glyphs[index];
    }

    //Current font loaded into memory
    public static Dictionary<string, Font> fonts;
}
