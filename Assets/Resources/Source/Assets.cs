using System;
using System.Collections.Generic;

public class Assets
{
    public Assets() { }

    public Assets(List<string> ambience, List<string> sounds, List<string> itemIcons, List<string> abilityIcons, List<string> portraits)
    public Assets(List<string> ambience, List<string> sounds, List<string> itemIcons, List<string> abilityIcons, List<string> factionIcons, List<string> portraits)
    {
        this.ambience = ambience;
        this.sounds = sounds;
        this.itemIcons = itemIcons;
        this.abilityIcons = abilityIcons;
        this.factionIcons = factionIcons;
        this.portraits = portraits;
    }

    public List<string> ambience, sounds, itemIcons, abilityIcons, factionIcons, portraits;
    [NonSerialized] public List<string> ambienceSearch, soundsSearch, itemIconsSearch, abilityIconsSearch. factionIconsSearch, portraitsSearch;

    public static Assets assets;
}
