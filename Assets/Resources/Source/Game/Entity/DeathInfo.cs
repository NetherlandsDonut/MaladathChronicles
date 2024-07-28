
public class DeathInfo
{
    public DeathInfo() { }
    public DeathInfo(string source, bool commonSource, string area)
    {
        this.source = source;
        this.commonSource = commonSource;
        this.area = area;
    }
    
    public string source;

    public bool commonSource;

    public string area;
}
