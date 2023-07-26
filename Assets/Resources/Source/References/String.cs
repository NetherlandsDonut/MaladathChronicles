
public class String
{
    //Fields
    private string value = "";
    private string backupValue = "";

    public string Value() => value;
    public string Insert(int i, char x) => value = value.Length == 0 ? value += x : value.Insert(i, x + "");
    public string Add(char x) => value += x;
    public string RemovePreviousOne(int i) => value = value.Remove(i - 1, 1);
    public string RemoveNextOne(int i) => value = value.Remove(i, 1);

    public void Set(string value) => backupValue = this.value = value;
    public void Confirm() => backupValue = value;
    public void Reset() => value = backupValue;

    public static String consoleInput = new String();
}
