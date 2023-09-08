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

    //Plays new background ambience, in case of one already playing we queue them.
    //Then application slowly lowers the volume of the current one and then softly starts the new one
    public static void PlayAmbience(string path, float volume = 0.5f, bool instant = false)
    {
        var temp = Resources.Load<AudioClip>("Ambience/" + path);
        if (temp == null) return;
        if (ambience.clip == temp) return;
        queuedAmbience = (temp, volume, instant);
    }

    public static void StopAmbience(bool instant = false)
    {
        queuedAmbience = (null, 0, instant);
    }

    public static AudioSource ambience, soundEffects;
    public static int fallingSoundsPlayedThisFrame;
    public static (AudioClip, float, bool) queuedAmbience;
}
