using UnityEngine;

using System.Linq;
using System.Collections.Generic;

using static Font;

public class Line : MonoBehaviour
{
    public Region region;
    public LineText LBText;
    public List<LineText> texts;
    public string align;

    public void Initialise(Region region, string align)
    {
        this.region = region;
        this.align = align;
        texts = new();
        region.LBLine = this;
        region.lines.Add(this);
    }

    public int Length() => texts.Sum(x => font.Length(x.text));
}
