using UnityEngine;

using static Root;
using static Sound;

public class FallingElement : MonoBehaviour
{
    public int howFar;
    public float timeAlive;
    public Vector3 start, destination;

    public void Initiate(int howFar)
    {
        timeAlive = 0;
        this.howFar = howFar;
        fallingElements.Add(this);
        start = transform.position;
        destination = transform.position + new Vector3(0, -38 * howFar);
    }

    public void Update()
    {
        timeAlive += Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, destination, timeAlive * fallSpeed / (float)System.Math.Sqrt(howFar) * 0.9f);
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

    public static float fallSpeed = 0.8f;
}
