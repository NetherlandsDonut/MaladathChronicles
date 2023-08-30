public class Bool
{
    public Bool() { }
    public Bool(bool value) => Set(value);

    public string value = "False";

    public bool Value() => value == "True";
    public void Invert() => value = value == "True" ? "False" : "True";
    public void Set(bool value) => this.value = value ? "True" : "False";
}
