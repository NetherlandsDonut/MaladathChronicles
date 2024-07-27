using System.Collections.Generic;

public class Stats
{
   public Stats(Dictionary<string, int> stats) => this.stats = stats ?? new();

   public Dictionary<string, int> stats;
}
