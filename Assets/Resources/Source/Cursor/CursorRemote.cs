using System;
using UnityEngine;

using static Root;

public class CursorRemote : MonoBehaviour
{
    //Color of the cursor
    public string color;

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
        //Don't update the cursor if any tile on the board is empty or if they are falling elements, 
        if (Board.board != null && Board.board.field[0, 0] == -1 || FallingElement.fallingElements.Count > 0) return;

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

    //Sets the color of the cursor to a different color from default white
    public void SetColor(string color)
    {
        var currentAlpha = render.color.a;
        var newColor = (Color)Coloring.colors[color];
        render.color = new Color(newColor.r, newColor.g, newColor.b, currentAlpha);
        this.color = color;
    }

    public void SetCursor(CursorType cursor)
    {
        if (cursor == CursorType.None) render.sprite = null;
        else render.sprite = Resources.Load<Sprite>("Sprites/Cursor/" + cursor);
    }

    public void Move(Vector3 where) => target = where;

    public static CursorRemote cursorEnemy;
}
