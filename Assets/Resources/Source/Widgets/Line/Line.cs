using UnityEngine;

using System.Linq;
using System.Collections.Generic;

using static Font;

public class Line : MonoBehaviour
{
    public Region region;
    public List<LineText> texts;
    public string align;

    public void Initialise(Region region, string align)
    {
        this.region = region;
        this.align = align;
        texts = new();
        region.lines.Add(this);
    }

    public LineText LBText() => texts.Last();

    public int Length() => texts.Sum(x => x.text.Length == 0 ? 0 : fonts["Tahoma Bold"].Length(x.text));
}
