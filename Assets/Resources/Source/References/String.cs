using UnityEngine;

using static Root;

public class String
{
    public string value = "";
    public InputType inputType = InputType.Everything;
    private string backupValue = "";

    public string Value() => value;
    public string Insert(int i, char x) => value = value.Length == 0 ? value += x : value.Insert(i, x + "");
    public string Add(char x) => value += x;
    public string RemovePreviousOne(int i) => value = value.Remove(i - 1, 1);
    public string RemoveNextOne(int i) => value = value.Remove(i, 1);

    public void Clear() => value = "";
    public void Paste() => value = GUIUtility.systemCopyBuffer.Replace("\b", "").Replace("\r", "").Replace("\n", "");
    public void Set(string value) => backupValue = this.value = value;
    public void Confirm() => backupValue = value;
    public void Reset() => value = backupValue;

    public bool CheckInput(char letter)
    {
        switch (inputType)
        {
            case InputType.Letters:
                return char.IsLetter(letter);
            case InputType.Capitals:
                return char.IsLetter(letter);
            case InputType.Numbers:
                return char.IsDigit(letter);
            case InputType.Decimal:
                return char.IsDigit(letter) || letter == ',' && !Value().Contains(',');
            default:
                return true;
        }
    }

    public static String promptConfirm = new();
    public static String consoleInput = new();
    public static String search = new();
    public static String objectName = new();
    public static String vitality = new();
    public static String price = new();
    public static String itemPower = new();
    public static String cooldown = new();
    public static String fire = new();
    public static String earth = new();
    public static String water = new();
    public static String air = new();
    public static String frost = new();
    public static String decay = new();
    public static String shadow = new();
    public static String order = new();
    public static String arcane = new();
    public static String lightning = new();
    public static String requiredLevel = new();
    public static String animationSpeed = new();
    public static String chance = new();
    public static String chanceBase = new();
    public static String chanceScale = new();
    public static String animationArc = new();
    public static String trailStrength = new();
    public static String shatterDegree = new();
    public static String shatterDensity = new();
    public static String shatterSpeed = new();
    public static String await = new();
    public static String powerScale = new();
    public static String buffDuration = new();
    public static String resourceAmount = new();
}
