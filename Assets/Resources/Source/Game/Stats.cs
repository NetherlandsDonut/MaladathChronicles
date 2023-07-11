using System.Linq;
using System.Collections.Generic;

public class Stats : Dictionary<string, int>
{
    public Stats(Dictionary<string, int> stats)
    {
        this.stats = stats;
    }

    public Dictionary<string, int> stats;

    public Dictionary<string, int> Copy() => stats.ToDictionary(x => x.Key, x => x.Value);
}
