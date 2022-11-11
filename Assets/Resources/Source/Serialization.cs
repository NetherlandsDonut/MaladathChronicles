using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;

using static Newtonsoft.Json.JsonConvert;
using static Newtonsoft.Json.Formatting;

class Serialization
{
    public static void Deserialize<T>(ref T target, string file, bool encoded = false)
    {
        if (!Directory.Exists("Torf_Data_2"))
            Directory.CreateDirectory("Torf_Data_2");
        if (!File.Exists("Torf_Data_2\\" + file + (encoded ? ".TORF" : ".json"))) return;
        var content = File.ReadAllText("Haldern_Data_2\\" + file + (encoded ? ".TORF" : ".json"));
        if (encoded) content = Decrypt(content);
        target = DeserializeObject<T>(content);
    }

    public static void Serialize(object what, string where, bool encoded = false)
    {
        if (!Directory.Exists("Torf_Data_2"))
            Directory.CreateDirectory("Torf_Data_2");
        var sett = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        var data = SerializeObject(what, encoded ? None : Indented, sett);
        if (encoded) data = Encrypt(data);
        File.WriteAllText("Torf_Data_2\\" + where + (encoded ? ".TORF" : ".json"), data);
    }

    public static string IV = "1a1a1a1a1a1a1a1a";
    public static string Key = "1a1a1a1a1a1a1a1a1a1a1a1a1a1a1a13";

    public static string Encrypt(string what)
    {
        byte[] textbytes = Encoding.UTF8.GetBytes(what);
        var endec = new AesCryptoServiceProvider()
        {
            BlockSize = 128,
            KeySize = 256,
            IV = Encoding.UTF8.GetBytes(IV),
            Key = Encoding.UTF8.GetBytes(Key),
            Padding = PaddingMode.Zeros,
            Mode = CipherMode.ECB
        };
        ICryptoTransform icrypt = endec.CreateEncryptor(endec.Key, endec.IV);
        byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
        icrypt.Dispose();
        return System.Convert.ToBase64String(enc);
    }

    public static string Decrypt(string what)
    {
        byte[] textbytes = System.Convert.FromBase64String(what);
        var endec = new AesCryptoServiceProvider()
        {
            BlockSize = 128,
            KeySize = 256,
            IV = Encoding.UTF8.GetBytes(IV),
            Key = Encoding.UTF8.GetBytes(Key),
            Padding = PaddingMode.Zeros,
            Mode = CipherMode.ECB
        };
        ICryptoTransform icrypt = endec.CreateDecryptor(endec.Key, endec.IV);
        byte[] enc = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
        icrypt.Dispose();
        return Encoding.UTF8.GetString(enc);
    }
}
