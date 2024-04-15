using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using static Root;
using static Sound;

public class DeathInfo
{
    public DeathInfo() { }
    public DeathInfo(string source, string area)
    {
        this.source = source;
        this.area = area;
    }
    
    public string source, area;
}
