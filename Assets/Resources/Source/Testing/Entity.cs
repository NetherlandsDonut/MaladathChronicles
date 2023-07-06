using UnityEngine;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    public static Entity player;

    public Entity()
    {
        name = "Roowr";
        SetStartingResources();
    }

    public string name;

    public Dictionary<string, int> resources;

    public void SetStartingResources()
    {
        resources = new() 
        {
            { "Earth", 0 },
            { "Fire", 0 },
            { "Air", 0 },
            { "Water", 0 },
            { "Frost", 0 },
            { "Arcane", 0 },
            { "Order", 0 },
            { "Shadow", 0 },
        };
    }
}
