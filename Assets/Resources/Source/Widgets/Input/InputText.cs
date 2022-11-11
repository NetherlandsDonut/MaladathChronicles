using UnityEngine;
using System.Collections.Generic;

using static Root;
using static Root.Color;

public class InputText : MonoBehaviour
{
    //Parent
    public InputLine inputLine;

    //Children
    public List<GameObject> characters;

    //Fields
    public String text;

    public void Initialise(InputLine inputLine, String text)
    {
        this.text = text;
        characters = new();
        this.inputLine = inputLine;

        inputLine.text = this;
    }

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
        newCharacter.GetComponent<SpriteRenderer>().color = GetColor(character + "" == markerCharacter ? Gray : LightGray);
        if (character + "" == markerCharacter) newCharacter.AddComponent<Blinking>();
        else
        {
            newCharacter.AddComponent<Highlightable>().Initialise(inputLine.region.regionGroup.window, inputLine.region);
            newCharacter.AddComponent<InputCharacter>().Initialise(this);
            newCharacter.AddComponent<BoxCollider2D>().size += new Vector2(1f, 0);
        }
        characters.Add(newCharacter);
        return offset + (int)glyph.rect.width + 1;
    }
}
