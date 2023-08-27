
public class Bool
{
    public Bool() { }
    public Bool(bool value) => Set(value);

    private int value = 0;

    public bool Value() => value == 1;
    public void Invert() => value = value == 0 ? 1 : 0;
    public void Set(bool value) => this.value = value ? 1 : 0;
}
