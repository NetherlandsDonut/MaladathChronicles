using UnityEngine;
using System.Collections.Generic;

using static Font;
using static Coloring;

public class LineText : MonoBehaviour
{
    //Parent
    public Line line;

    //Children
    public List<GameObject> characters;

    //Fields
    public string text, color;

    public void Initialise(Line line, string text, string color)
    {
        this.color = color;
        this.text = text;
        this.line = line;
        characters = new();
        line.LBText = this;
        line.texts.Add(this);
    }

    public void Initialise(FallingText fallingtext, string text, string color)
    {
        this.color = color;
        this.text = text;
        characters = new();
    }

    public void Erase()
    {
        while (characters.Count > 0)
        {
            Destroy(characters[0]);
            characters.RemoveAt(0);
        }
    }

    public int SpawnCharacter(char character, int offset, string font = "Tahoma Bold")
    {
        var newCharacter = new GameObject("Character", typeof(SpriteRenderer));
        newCharacter.transform.parent = transform;
        newCharacter.transform.localPosition = new Vector3(offset, 0, 0.2f);
        var glyph = fonts[font].GetGlyph(character);
        newCharacter.GetComponent<SpriteRenderer>().sprite = glyph;
        newCharacter.GetComponent<SpriteRenderer>().color = colors[color];
        newCharacter.GetComponent<SpriteRenderer>().sortingLayerName = line == null ? "FallingText" : line.region.regionGroup.window.layer;
        characters.Add(newCharacter);
        return offset + (int)glyph.rect.width + 1;
    }
}
