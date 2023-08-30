using UnityEngine;
using Newtonsoft.Json;

public class String
{
    public string value = "";
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

    public static String promptConfirm = new();
    public static String consoleInput = new();
    public static String search = new();
    public static String objectName = new();
    public static String vitality = new();
    public static String price = new();
    public static String itemPower = new();
    public static String cooldown = new();
    public static String requiredLevel = new();
    public static String await = new();
}
