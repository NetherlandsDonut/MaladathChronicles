using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Font;

public class FallingText : MonoBehaviour
{
    public static string fallingTextFont = "Tahoma Bold";

    public void Initialise(string text, string color)
    {
        var pixelList = new List<(int, int)>();
        var newObject = new GameObject("Text", typeof(LineText));
        newObject.transform.parent = transform;
        newObject.transform.localPosition = new Vector2(fonts[fallingTextFont].Length(text) / -2, 7);
        var temp = newObject.GetComponent<LineText>();
        temp.Initialise(this, text, color == "" ? "Gray" : color);
        int length = 0;
        temp.Erase();
        foreach (var character in temp.text)
        {
            var before = length;
            length = temp.SpawnCharacter(character, length, fallingTextFont);
            var ch = temp.characters.Last();
            var chs = ch.GetComponent<SpriteRenderer>();
            ch.AddComponent<Shatter>().Initiate(0.015f, chs);
            for (int i = 0; i < chs.sprite.textureRect.width; i++)
                for (int j = 0; j < chs.sprite.textureRect.height; j++)
                {
                    var c = chs.sprite.texture.GetPixel((int)chs.sprite.textureRect.x + i, (int)chs.sprite.textureRect.y + j);
                    if (c.r == 1 && c.g == 1 && c.b == 1 && c.a == 1)
                    {
                        if (!pixelList.Contains((i - 2 + before, j + 2)))
                            pixelList.Add((i - 2 + before, j + 2));
                        if (!pixelList.Contains((i - 2 + before, j + 1)))
                            pixelList.Add((i - 2 + before, j + 1));
                        if (!pixelList.Contains((i - 2 + before, j)))
                            pixelList.Add((i - 2 + before, j));
                        if (!pixelList.Contains((i - 2 + before, j - 1)))
                            pixelList.Add((i - 2 + before, j - 1));
                        if (!pixelList.Contains((i - 2 + before, j - 2)))
                            pixelList.Add((i - 2 + before, j - 2));
                        if (!pixelList.Contains((i - 1 + before, j + 2)))
                            pixelList.Add((i - 1 + before, j + 2));
                        if (!pixelList.Contains((i - 1 + before, j + 1)))
                            pixelList.Add((i - 1 + before, j + 1));
                        if (!pixelList.Contains((i - 1 + before, j)))
                            pixelList.Add((i - 1 + before, j));
                        if (!pixelList.Contains((i - 1 + before, j - 1)))
                            pixelList.Add((i - 1 + before, j - 1));
                        if (!pixelList.Contains((i - 1 + before, j - 2)))
                            pixelList.Add((i - 1 + before, j - 2));
                        if (!pixelList.Contains((i + 2 + before, j + 2)))
                            pixelList.Add((i + 2 + before, j + 2));
                        if (!pixelList.Contains((i + 2 + before, j + 1)))
                            pixelList.Add((i + 2 + before, j + 1));
                        if (!pixelList.Contains((i + 2 + before, j)))
                            pixelList.Add((i + 2 + before, j));
                        if (!pixelList.Contains((i + 2 + before, j - 1)))
                            pixelList.Add((i + 2 + before, j - 1));
                        if (!pixelList.Contains((i + 2 + before, j - 2)))
                            pixelList.Add((i + 2 + before, j - 2));
                        if (!pixelList.Contains((i + 1 + before, j - 1)))
                            pixelList.Add((i + 1 + before, j - 1));
                        if (!pixelList.Contains((i + 1 + before, j - 2)))
                            pixelList.Add((i + 1 + before, j - 2));
                        if (!pixelList.Contains((i + 1 + before, j)))
                            pixelList.Add((i + 1 + before, j));
                        if (!pixelList.Contains((i + before, j + 2)))
                            pixelList.Add((i + before, j + 2));
                        if (!pixelList.Contains((i + before, j + 1)))
                            pixelList.Add((i + before, j + 1));
                        if (!pixelList.Contains((i, j - 1)))
                            pixelList.Add((i + before, j - 1));
                        if (!pixelList.Contains((i, j - 2)))
                            pixelList.Add((i + before, j - 2));
                    }
                }
        }
        var textBorder = new GameObject("TextBorder", typeof(SpriteRenderer));
        textBorder.transform.parent = transform;
        textBorder.transform.localPosition = new Vector3(fonts[fallingTextFont].Length(text) / -2 - 2, 9, 1);
        var xPlus = pixelList.Min(x => x.Item1);
        var yPlus = pixelList.Min(x => x.Item2);
        var texture = new Texture2D(pixelList.Max(x => x.Item1) - xPlus + 5, pixelList.Max(x => x.Item2) - yPlus + 5, TextureFormat.ARGB32, true) { filterMode = FilterMode.Point };
        for (int i = 0; i < texture.width; i++)
            for (int j = 0; j < texture.height ; j++)
                if (pixelList.Contains((i + xPlus, j + yPlus)))
                    texture.SetPixel(i, j, new Color(0, 0, 0, 1));
                else texture.SetPixel(i, j, new Color(0, 0, 0, 0));
        texture.Apply();
        var sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), new Vector2(0, 1), 1);
        textBorder.AddComponent<Shatter>().Initiate(0.015f, textBorder.GetComponent<SpriteRenderer>());
        textBorder.GetComponent<SpriteRenderer>().sprite = sprite;
        textBorder.GetComponent<SpriteRenderer>().sortingLayerName = "FallingText";
    }
}
