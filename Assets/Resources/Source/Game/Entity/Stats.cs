using System.Linq;
using System.Collections.Generic;

public class Stats
{
    public Stats(Dictionary<string, int> stats)
    {
        this.stats = stats;
    }

    public Dictionary<string, int> stats;

    public Dictionary<string, int> Copy() => stats.ToDictionary(x => x.Key, x => x.Value);
}
