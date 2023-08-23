using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Font;

public class Line : MonoBehaviour
{
    //Parent
    public Region region;

    //Children
    public LineText LBText;
    public List<LineText> texts;

    public void Initialise(Region region)
    {
        this.region = region;
        texts = new();

        this.region.LBLine = this;
        region.lines.Add(this);
    }

    public int Length() => texts.Sum(x => font.Length(x.text));
}
