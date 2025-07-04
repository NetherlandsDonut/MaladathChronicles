using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Font;

public class FloatingText : MonoBehaviour
{
    public void Initialise(string text, string color, string borderColor, string align, bool fall = true)
    {
        var newObject = new GameObject("Text", typeof(LineText));
        newObject.transform.parent = transform;
        if (align == "Center") newObject.transform.localPosition = new Vector2(fonts[floatingTextFont].Length(text) / -2, 7);
        else if (align == "Left") newObject.transform.localPosition = new Vector2(0, 7);
        else if (align == "Right") newObject.transform.localPosition = new Vector2(-fonts[floatingTextFont].Length(text), 7);
        var temp = newObject.GetComponent<LineText>();
        temp.Initialise(Root.CDesktop.LBWindow(), text, color == "" ? "Gray" : color, "FallingText");
        int length = 0;
        temp.Erase();
        foreach (var character in temp.text)
        {
            var before = length;
            length = temp.SpawnCharacter(character, length, floatingTextFont);
            var ch = temp.characters.Last();
            var chs = ch.GetComponent<SpriteRenderer>();
            if (fall) ch.AddComponent<Shatter>().Initiate(1.7f, 1f, chs);
            if (borderColor != "Transparent")
            {
                var charBorder = new GameObject("CharBorder", typeof(SpriteRenderer));
                var charBorders = charBorder.GetComponent<SpriteRenderer>();
                charBorders.sprite = fonts[floatingTextFont].GetGlyphBorder(character);
                charBorders.color = Coloring.colors[borderColor == "" ? "FullBlack" : borderColor];
                charBorders.sortingLayerName = "FallingText";
                charBorder.transform.parent = newObject.transform;
                charBorder.transform.localPosition = new(-2 + ch.transform.localPosition.x, 2 + ch.transform.localPosition.y);
                if (fall) charBorder.AddComponent<Shatter>().Initiate(1.7f, 1f, charBorders);
            }
        }
    }

    public static string floatingTextFont = "Tahoma Bold";
}
