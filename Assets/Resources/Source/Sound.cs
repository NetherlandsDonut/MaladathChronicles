using UnityEngine;

using static Cursor;

public static class Sound
{
    public static void PlaySound(string path, float volume = 0.5f)
    {
        var temp = Resources.Load<AudioClip>("Sounds/" + path);
        if (temp == null) return;
        cursor.GetComponent<AudioSource>().PlayOneShot(temp, volume);
    }

    public static int fallingSoundsPlayedThisFrame;
}
