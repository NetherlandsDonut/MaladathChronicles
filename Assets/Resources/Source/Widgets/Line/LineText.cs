using UnityEngine;
using System.Collections.Generic;

using static Root;

public class LineText : MonoBehaviour
{
    //Parent
    public Line line;

    //Children
    public List<GameObject> characters;

    //Fields
    public string text;
    public Root.Color color;

    public void Initialise(Line line, string text, Root.Color color)
    {
        SetText(text);
        SetColor(color);
        characters = new();
        this.line = line;

        line.LBText = this;
        line.texts.Add(this);
    }

    public void SetColor(Root.Color color) => this.color = color;
    public void SetText(string text) => this.text = text;

    public void Erase()
    {
        while (characters.Count > 0)
        {
            Destroy(characters[0].gameObject);
            characters.RemoveAt(0);
        }
    }

    public int SpawnCharacter(char character, int offset)
    {
        var newCharacter = new GameObject("Character", typeof(SpriteRenderer));
        newCharacter.transform.parent = transform;
        newCharacter.transform.localPosition = new Vector3(offset, 0, 0.2f);
        var glyph = GetGlyph(character);
        newCharacter.GetComponent<SpriteRenderer>().sprite = glyph;
        newCharacter.GetComponent<SpriteRenderer>().color = GetColor(color);
        characters.Add(newCharacter);
        return offset + (int)glyph.rect.width + 1;
    }
}
