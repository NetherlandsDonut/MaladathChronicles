using UnityEngine;

public class HealthBar : MonoBehaviour
{
    //Entity this health bar referes to
    public Entity entity;

    //Reference to the splitter
    public SpriteRenderer split;

    //Updates the length of the health bar
    public void UpdateHealthBar()
    {
        var aim = (int)(131.0 / entity.MaxHealth() * entity.health);
        if (aim < 0) aim = 0;
        else if (aim < 2) aim = 2;
        else if (aim > 127) aim = 131;
        split.transform.localPosition = new Vector2(aim - 2, -2);
    }
}
