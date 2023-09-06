using UnityEngine;

using static Cursor;
using static GameSettings;

public static class Sound
{
    public static void PlaySound(string path, float volume = 0.5f)
    {
        if (!settings.soundEffects.Value()) return;
        var temp = Resources.Load<AudioClip>("Sounds/" + path);
        if (temp == null) return;
        soundEffects.PlayOneShot(temp, volume);
    }

    public static void PlayAmbience(string path, float volume = 0.5f, bool instant = false)
    {
        var temp = Resources.Load<AudioClip>("Ambience/" + path);
        if (temp == null) return;
        if (ambience.clip == temp) return;
        queuedAmbience = (temp, volume, instant);
    }

    public static AudioSource ambience, soundEffects;
    public static int fallingSoundsPlayedThisFrame;
    public static (AudioClip, float, bool) queuedAmbience;
}
