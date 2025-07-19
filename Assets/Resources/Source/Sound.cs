using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static GameSettings;

public static class Sound
{
    #region Music & Ambience

    //Music controller that plays music tracks
    public static AudioSource music;

    //List of all music tracks active in the loop
    public static List<AudioClip> musicList;

    //Queued music to be played by the music controller
    public static (AudioClip, float, bool) queuedMusic;

    //Plays new background ambience, in case of one already playing we queue them.
    //Then application slowly lowers the volume of the current one and then softly starts the new one
    public static void PlayMusic(List<string> paths, float volume = 0.2f, bool instant = false)
    {
        //If the provided list of music is empty, cancel
        if (paths == null || paths.Count == 0)
        {
            StopMusic();
            return;
        }

        //Get all audio clips for the list
        musicList = paths.Select(x => Resources.Load<AudioClip>("Music/" + x)).ToList();

        //Remove all tracks which weren't found
        musicList.RemoveAll(x => x == null);

        //If no tracks are on the list, cancel
        if (musicList.Count == 0)
        {
            StopMusic();
            return;
        }

        //Shuffle the music tracks
        musicList.Shuffle();

        //If there is some music playing and the currently
        //playing track is on the new list, simply cancel further actions
        if (music.clip != null && musicList.Exists(x => x.name == music.clip.name)) return;

        //Otherwise change current track the the first in the new list
        var temp = musicList[0];
        if (temp == null) return;
        if (music.clip == temp) return;
        queuedMusic = (temp, volume, instant);
    }

    //Stops playing the background music
    public static void StopMusic(bool instant = false)
    {
        musicList = new();
        queuedMusic = (null, 0, instant);
    }

    //Ambience controller that plays ambience tracks
    public static AudioSource ambience;

    //Queued ambience to be played by the ambience controller
    public static (AudioClip, float, bool) queuedAmbience;

    //Plays new background ambience, in case of one already playing we queue them.
    //Then application slowly lowers the volume of the current one and then softly starts the new one
    public static void PlayAmbience(string path, float volume = 0.1f, bool instant = false)
    {
        var temp = Resources.Load<AudioClip>("Ambience/" + path);
        if (temp == null) return;
        if (ambience.clip == temp) return;
        queuedAmbience = (temp, volume, instant);
    }

    //Stops playing the background ambience
    public static void StopAmbience(bool instant = false) => queuedAmbience = (null, 0, instant);

    #endregion

    #region Sound Effects

    //All sound effects loaded up into the memory
    public static Dictionary<string, AudioClip> sounds;

    //Sound effect controller that plays sound effects
    public static AudioSource soundEffects;

    //Plays a singular sound effect
    public static void PlaySound(string path, float volume = 0.7f)
    {
        if (soundsPlayedThisFrame.Contains(path)) return;
        if (!settings.soundEffects.Value()) return;
        var soundToPlay = path.Replace("<", "").Replace(">", "");
        if (path == "<WeaponLoad>")
        {
            var eq = Board.board.participants[Board.board.whosTurn].who.equipment;
            var rangedWeapon = eq.ContainsKey("Ranged Weapon") ? eq["Ranged Weapon"] : null;
            if (rangedWeapon != null) soundToPlay = rangedWeapon.detailedType + "Load";
        }
        else if (path == "<WeaponFire>")
        {
            var eq = Board.board.participants[Board.board.whosTurn].who.equipment;
            var rangedWeapon = eq.ContainsKey("Ranged Weapon") ? eq["Ranged Weapon"] : null;
            if (rangedWeapon != null) soundToPlay = rangedWeapon.detailedType + "Fire";
        }
        var find = sounds.Where(x => x.Key.StartsWith(soundToPlay));
        if (find.Count() == 0) return;
        soundEffects.PlayOneShot(find.ToList()[Root.random.Next(find.Count())].Value, volume);
        soundsPlayedThisFrame.Add(soundToPlay);
    }

    //Sound effect controller that plays sound effects
    public static AudioSource voiceLines;

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

    //Sound effect controller that plays sound effects
    public static AudioSource enemyLines;

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

    //Amount of sounds played this frame
    //It is used to ensure that user's headphones are not
    //destroyed with a stacked sound of 47 sounds played at once
    public static List<string> soundsPlayedThisFrame;

    #endregion
}
