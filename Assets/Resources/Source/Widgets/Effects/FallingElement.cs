using System;
using System.Collections.Generic;
using UnityEngine;

using static Sound;

//Class controling the falling effect of the elements on the board
//The elements that fall into the board are generated in the buffer board
public class FallingElement : MonoBehaviour
{
    //Amount of time till the effect starts to play out
    public float delay;

    //How many rows this element has to travel
    public int howFar;

    //Amount of time this element already is traveling
    public float timeAlive;

    //Final position for this element
    public Vector3 destination;

    //Did this falling element already play it's falling sound
    public bool playedSound;

    //Initiaties the effect
    //Parameter states how many rows does the element has to travel
    public void Initiate(int howFar, int delay)
    {
        timeAlive = 0;
        this.howFar = howFar;
        fallingElements.Add(this);
        destination = transform.position + new Vector3(0, -38 * howFar);
        this.delay = delay * 0.07f;
    }

    //Every frame move this element towards it's destination on the board
    public void Update()
    {
        if (delay > 0) delay -= Time.deltaTime;
        else
        {
            timeAlive += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, destination, timeAlive / (float)Math.Sqrt(howFar) * (GameSettings.settings.fastCascading.Value() ? 0.8f : 0.5f));
            var temp = Vector3.Distance(transform.position, destination);
            if (temp <= 1.5f)
                if (!playedSound)
                {
                    playedSound = true;
                    PlaySound("PutDownWoodSmall", 0.5f);
                }
                else if (temp < 0.05f)
                {
                    fallingElements.Remove(this);
                    Destroy(this);
                }
        }
    }

    //Currently falling elements on the board
    public static List<FallingElement> fallingElements;
}
