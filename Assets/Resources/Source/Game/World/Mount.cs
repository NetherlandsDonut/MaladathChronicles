using System.Collections.Generic;

public class Mount
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        if (abilities != null)
            foreach (var ability in abilities)
                if (!Ability.abilities.Exists(x => x.name == ability))
                    Ability.abilities.Insert(0, new Ability()
                    {
                        name = ability,
                        icon = "Ability" + ability,
                        events = new(),
                        tags = new()
                    });
    }

    //Name of the mount
    public string name;

    //Icon of the mount
    public string icon;

    //Speed of the mount, while mount is equipped,
    //this speed replaces the player speed
    public int speed;

    //Price of the mount
    public float price;

    //List of abilities provided by the mount
    public List<string> abilities;

    //Currently opened mount
    public static Mount mount;

    //EXTERNAL FILE: List containing all mounts in-game
    public static List<Mount> mounts;

    //List of all filtered mounts by input search
    public static List<Mount> mountsSearch;
}
