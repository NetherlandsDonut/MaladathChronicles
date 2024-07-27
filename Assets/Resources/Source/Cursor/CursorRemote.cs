using System;
using UnityEngine;

using static Root;

public class CursorRemote : MonoBehaviour
{
    //Renderer of the cursor that makes the cursor be visible
    public SpriteRenderer render;

    //Target position for the cursor
    public Vector3 target;

    //Indicator that the cursor should fade in
    public bool fadeIn;

    //Indicator that the cursor should fade out
    public bool fadeOut;

    void Start() => render = GetComponent<SpriteRenderer>();

    void Update()
    {
        if (fadeOut)
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b, render.color.a - 0.04f);
            if (render.color.a <= 0) { render.color = new Color(render.color.r, render.color.g, render.color.b, 0); fadeOut = false; }
        }
        else if (fadeIn)
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b, render.color.a + 0.04f);
            if (render.color.a >= 1) { render.color = new Color(render.color.r, render.color.g, render.color.b, 1); fadeIn = false; }
        }
        if (target == Vector3.zero) return;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 7);
        var temp = transform.position - target;
        if (Math.Abs(temp.x) + Math.Abs(temp.y) < 0.5f)
            target = Vector3.zero;
    }

    public void SetCursor(CursorType cursor)
    {
        if (cursor == CursorType.None)
            render.sprite = null;
        else
            render.sprite = Resources.Load<Sprite>("Sprites/Cursor/" + cursor);
    }

    public void Move(Vector3 where) => target = where;

    public static CursorRemote cursorEnemy;
}
