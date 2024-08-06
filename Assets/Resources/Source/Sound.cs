using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static GameSettings;

public static class Sound
{
    //All sound effects loaded up into the memory
    public static Dictionary<string, AudioClip> sounds;

    //Plays a singular sound effect
    public static void PlaySound(string path, float volume = 0.7f)
    {
        if (soundsPlayedThisFrame.Contains(path)) return;
        if (!settings.soundEffects.Value()) return;
        if (!sounds.ContainsKey(path)) return;
        soundEffects.PlayOneShot(sounds[path], volume);
        soundsPlayedThisFrame.Add(path);
    }

    //Plays a singular sound effect
    public static void PlayVoiceLine(string path, float volume = 0.4f)
    {
        if (!settings.soundEffects.Value()) return;
        var find = sounds.Where(x => x.Key.StartsWith(path));
        if (find.Count() == 0) return;
        voiceLines.clip = find.ToList()[Root.random.Next(find.Count())].Value;
        voiceLines.volume = volume;
        voiceLines.Play();
    }

    //Plays a singular sound effect
    public static void PlayEnemyLine(string path, float volume = 0.3f)
    {
        if (!settings.soundEffects.Value()) return;
        var find = sounds.Where(x => x.Key.StartsWith(path));
        if (find.Count() == 0) return;
        enemyLines.clip = find.ToList()[Root.random.Next(find.Count())].Value;
        enemyLines.volume = volume;
        enemyLines.Play();
    }

    //Plays new background ambience, in case of one already playing we queue them.
    //Then application slowly lowers the volume of the current one and then softly starts the new one
    public static void PlayAmbience(string path, float volume = 0.2f, bool instant = false)
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

    //Sound effect controller that plays sound effects
    public static AudioSource voiceLines;

    //Sound effect controller that plays sound effects
    public static AudioSource enemyLines;

    //Amount of falling element sounds played this frame
    //It is used to ensure that user's headphones are not
    //destroyed with a stacked sound of 47 falling sounds played at once
    public static List<string> soundsPlayedThisFrame;

    //Queued ambience to be played by the ambience controller
    public static (AudioClip, float, bool) queuedAmbience;
}
