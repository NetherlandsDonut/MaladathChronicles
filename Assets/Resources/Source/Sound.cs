using System.Collections.Generic;

using UnityEngine;

using static GameSettings;

public static class Sound
{
    //All sound effects loaded up into the memory
    public static Dictionary<string, AudioClip> sounds;

    //Plays a singular sound effect
    public static void PlaySound(string path, float volume = 0.5f)
    {
        if (!settings.soundEffects.Value() || soundsPlayedThisFrame > 5) return;
        if (!sounds.ContainsKey(path)) return;
        soundEffects.PlayOneShot(sounds[path], volume);
        soundsPlayedThisFrame++;
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

    //Stops playing the background ambience
    public static void StopAmbience(bool instant = false) => queuedAmbience = (null, 0, instant);

    //Ambience controller that plays ambience tracks
    public static AudioSource ambience;

    //Sound effect controller that plays sound effects
    public static AudioSource soundEffects;

    //Amount of falling element sounds played this frame
    //It is used to ensure that user's headphones are not
    //destroyed with a stacked sound of 47 falling sounds played at once
    public static int soundsPlayedThisFrame;

    //Queued ambience to be played by the ambience controller
    public static (AudioClip, float, bool) queuedAmbience;
}
