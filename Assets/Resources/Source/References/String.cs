using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Root.InputType;

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
            case Letters:
                return char.IsLetter(letter) || letter == ' ' && value.Last() != ' ';
            case Capitals:
                return char.IsLetter(letter) || letter == ' ' && value.Last() != ' ';
            case Numbers:
                return char.IsDigit(letter);
            case Decimal:
                return char.IsDigit(letter) || letter == ',' && !Value().Contains(',');
            default:
                return true;
        }
    }

    public static String promptConfirm = new() { inputType = Capitals };
    public static String consoleInput = new() { inputType = Everything };
    public static String search = new() { inputType = Everything };
    public static String objectName = new() { inputType = Everything };
    public static String vitality = new() { inputType = Decimal };
    public static String price = new() { inputType = Decimal };
    public static String itemPower = new() { inputType = Numbers };
    public static String cooldown = new() { inputType = Numbers };
    public static String fire = new() { inputType = Numbers };
    public static String earth = new() { inputType = Numbers };
    public static String water = new() { inputType = Numbers };
    public static String air = new() { inputType = Numbers };
    public static String frost = new() { inputType = Numbers };
    public static String decay = new() { inputType = Numbers };
    public static String shadow = new() { inputType = Numbers };
    public static String order = new() { inputType = Numbers };
    public static String arcane = new() { inputType = Numbers };
    public static String lightning = new() { inputType = Numbers };
    public static String requiredLevel = new() { inputType = Numbers };
    public static String animationSpeed = new() { inputType = Decimal };
    public static String chance = new() { inputType = Numbers };
    public static String chanceBase = new() { inputType = Numbers };
    public static String chanceScale = new() { inputType = Numbers };
    public static String animationArc = new() { inputType = Numbers };
    public static String trailStrength = new() { inputType = Numbers };
    public static String shatterDegree = new() { inputType = Numbers };
    public static String shatterDensity = new() { inputType = Numbers };
    public static String shatterSpeed = new() { inputType = Numbers };
    public static String await = new() { inputType = Numbers };
    public static String powerScale = new() { inputType = Decimal };
    public static String buffDuration = new() { inputType = Numbers };
    public static String resourceAmount = new() { inputType = Numbers };
    public static String changeAmount = new() { inputType = Numbers };
    public static String stamina = new() { inputType = Numbers };
    public static String strength = new() { inputType = Numbers };
    public static String agility = new() { inputType = Numbers };
    public static String intellect = new() { inputType = Numbers };
    public static String spirit = new() { inputType = Numbers };
    public static Dictionary<Encounter, (String, String)> encounterLevels = new() { };
}
