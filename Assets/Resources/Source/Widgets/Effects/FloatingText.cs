using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Font;

public class FloatingText : MonoBehaviour
{
    public void Initialise(string text, string color, string align, bool fall = true)
    {
        var pixelList = new List<(int, int)>();
        var newObject = new GameObject("Text", typeof(LineText));
        newObject.transform.parent = transform;
        if (align == "Center")
            newObject.transform.localPosition = new Vector2(fonts[floatingTextFont].Length(text) / -2, 7);
        else if (align == "Left")
            newObject.transform.localPosition = new Vector2(0, 7);
        else if (align == "Right")
            newObject.transform.localPosition = new Vector2(-fonts[floatingTextFont].Length(text), 7);
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
            for (int i = 0; i < chs.sprite.textureRect.width; i++)
                for (int j = 0; j < chs.sprite.textureRect.height; j++)
                {
                    var c = chs.sprite.texture.GetPixel((int)chs.sprite.textureRect.x + i, (int)chs.sprite.textureRect.y + j);
                    if (c.r == 1 && c.g == 1 && c.b == 1 && c.a == 1)
                    {
                        //if (!pixelList.Contains((i - 2 + before, j + 2)))
                        //    pixelList.Add((i - 2 + before, j + 2));
                        if (!pixelList.Contains((i - 2 + before, j + 1)))
                            pixelList.Add((i - 2 + before, j + 1));
                        if (!pixelList.Contains((i - 2 + before, j)))
                            pixelList.Add((i - 2 + before, j));
                        if (!pixelList.Contains((i - 2 + before, j - 1)))
                            pixelList.Add((i - 2 + before, j - 1));
                        //if (!pixelList.Contains((i - 2 + before, j - 2)))
                        //    pixelList.Add((i - 2 + before, j - 2));
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
                        //if (!pixelList.Contains((i + 2 + before, j + 2)))
                        //    pixelList.Add((i + 2 + before, j + 2));
                        if (!pixelList.Contains((i + 2 + before, j + 1)))
                            pixelList.Add((i + 2 + before, j + 1));
                        if (!pixelList.Contains((i + 2 + before, j)))
                            pixelList.Add((i + 2 + before, j));
                        if (!pixelList.Contains((i + 2 + before, j - 1)))
                            pixelList.Add((i + 2 + before, j - 1));
                        //if (!pixelList.Contains((i + 2 + before, j - 2)))
                        //    pixelList.Add((i + 2 + before, j - 2));
                        if (!pixelList.Contains((i + 1 + before, j - 1)))
                            pixelList.Add((i + 1 + before, j - 1));
                        if (!pixelList.Contains((i + 1 + before, j - 2)))
                            pixelList.Add((i + 1 + before, j - 2));
                        if (!pixelList.Contains((i + 1 + before, j)))
                            pixelList.Add((i + 1 + before, j));
                        if (!pixelList.Contains((i + before, j + 2)))
                            pixelList.Add((i + before, j + 2));
                        if (!pixelList.Contains((i + 1 + before, j + 2)))
                            pixelList.Add((i + 1 + before, j + 2));
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
        textBorder.transform.parent = newObject.transform;
        textBorder.transform.localPosition = new Vector3(-2, 3, -0.04f);
        var xPlus = pixelList.Min(x => x.Item1);
        var yPlus = pixelList.Min(x => x.Item2);
        if (yPlus == 1) textBorder.transform.localPosition -= new Vector3(0, 1, 0);
        var texture = new Texture2D(pixelList.Max(x => x.Item1) - xPlus + 5, pixelList.Max(x => x.Item2) - yPlus + 5, TextureFormat.ARGB32, true) { filterMode = FilterMode.Point };
        for (int i = 0; i < texture.width; i++)
            for (int j = 0; j < texture.height ; j++)
                if (pixelList.Contains((i + xPlus, j + yPlus)))
                    texture.SetPixel(i, j, new Color(0, 0, 0, 1));
                else texture.SetPixel(i, j, new Color(0, 0, 0, 0));
        texture.Apply();
        var sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), new Vector2(0, 1), 1);
        if (fall) textBorder.AddComponent<Shatter>().Initiate(1.7f, 1f, textBorder.GetComponent<SpriteRenderer>());
        textBorder.GetComponent<SpriteRenderer>().sprite = sprite;
        textBorder.GetComponent<SpriteRenderer>().sortingLayerName = fall ? "FallingText" : transform.parent.GetComponent<Region>().regionGroup.window.layer;
    }

    public static string floatingTextFont = "Tahoma Bold";
}
