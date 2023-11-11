using UnityEngine;
using System.Collections.Generic;

using static Font;
using static Defines;
using static Coloring;

public class InputText : MonoBehaviour
{
    public InputLine inputLine;
    public List<GameObject> characters;
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
            Destroy(characters[0]);
            characters.RemoveAt(0);
        }
    }

    public int SpawnCharacter(char character, int offset, string color = "")
    {
        var newCharacter = new GameObject("Character", typeof(SpriteRenderer));
        newCharacter.transform.parent = transform;
        newCharacter.transform.localPosition = new Vector3(offset, 0, 0.2f);
        var glyph = fonts["Tahoma Bold"].GetGlyph(character);
        newCharacter.GetComponent<SpriteRenderer>().sortingLayerName = inputLine.region.regionGroup.window.layer;
        newCharacter.GetComponent<SpriteRenderer>().sprite = glyph;
        newCharacter.GetComponent<SpriteRenderer>().color = colors[character + "" == defines.markerCharacter ? "Gray" : (color != "" ? color : "LightGray")];
        if (character + "" == defines.markerCharacter) newCharacter.AddComponent<Blinking>();
        else
        {
            newCharacter.AddComponent<Highlightable>().Initialise(inputLine.region, null, null, null, null);
            newCharacter.AddComponent<InputCharacter>().Initialise(this);
            newCharacter.AddComponent<BoxCollider2D>().size += new Vector2(1f, 0);
        }
        characters.Add(newCharacter);
        return offset + (int)glyph.rect.width + 1;
    }
}
