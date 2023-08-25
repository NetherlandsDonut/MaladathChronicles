using System.Collections.Generic;

public class Assets
{
    public Assets(List<string> ambience, List<string> sounds, List<string> icons, List<string> portraits)
    {
        this.ambience = ambience;
        this.sounds = sounds;
        this.icons = icons;
        this.portraits = portraits;
    }

    public List<string> ambience, sounds, icons, portraits;

    public static Assets assets;
}
