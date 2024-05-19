using UnityEngine;

using static Root;
using static Sound;

//Class controling the falling effect of the elements on the board
//The elements that fall into the board are generated in the buffer board
public class FallingElement : MonoBehaviour
{
    //How many rows this element has to travel
    public int howFar;

    //Amount of time this element already is traveling
    public float timeAlive;

    //Entry and final position for this element
    public Vector3 start, destination;

    //Initiaties the effect
    //Parameter states how many rows does the element has to travel
    public void Initiate(int howFar)
    {
        timeAlive = 0;
        this.howFar = howFar;
        fallingElements.Add(this);
        start = transform.position;
        destination = transform.position + new Vector3(0, -38 * howFar);
    }

    //Every frame move this element towards it's destination on the board
    public void Update()
    {
        timeAlive += Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, destination, timeAlive * 0.8f / (float)System.Math.Sqrt(howFar) * (GameSettings.settings.fastCascading.Value() ? 0.9f : 0.6f));
        if (Mathf.Abs(transform.position.x - destination.x) + Mathf.Abs(transform.position.y - destination.y) < 0.4f)
        {
            if (soundsPlayedThisFrame == 0)
            {
                soundsPlayedThisFrame++;
                PlaySound("PutDownWoodSmall", 0.8f);
            }
            fallingElements.Remove(this);
            Destroy(this);
        }
    }
}
