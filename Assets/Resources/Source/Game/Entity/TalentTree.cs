using System.Collections.Generic;

public class TalentTree
{
    public TalentTree(string name, List<Talent> talents)
    {
        this.name = name;
        this.talents = talents;
    }

    public string name;
    public List<Talent> talents;
}
