using System;
using UnityEngine;

using static Sound;
using static Cursor;

public class FlyingBuff : MonoBehaviour
{
    public bool onPlayer;
    public int dyingIndex;
    public SpriteRenderer render;
    public Action<Highlightable> pressEvent;
    public Tooltip tooltip;

    public static int flySpeed = 6;
    public static int rowAmount = 7;

    public void Initiate(bool targettedPlayer, Action<Highlightable> pressEvent, Func<Highlightable, Action> tooltip)
    {
        onPlayer = targettedPlayer;
        (onPlayer ? Board.board.temporaryBuffsPlayer : Board.board.temporaryBuffsEnemy).Add(gameObject);
        this.pressEvent = pressEvent;
        if (tooltip != null)
            this.tooltip = new Tooltip(() => GetComponent<Highlightable>(), tooltip);
    }

    public void Update()
    {
        if (transform.localPosition.x < -322.5f || transform.localPosition.x > 322.5f) Destroy(gameObject);
        else if (onPlayer && !Board.board.temporaryBuffsPlayer.Contains(gameObject) || !onPlayer && !Board.board.temporaryBuffsEnemy.Contains(gameObject)) transform.position = Vector3.Lerp(transform.position, new Vector3(onPlayer ? -302.5f - (23 * Mathf.Abs(dyingIndex % rowAmount - rowAmount) + 1) : 302.5f + 23 * (Mathf.Abs(dyingIndex % rowAmount - rowAmount) + 1), 67.5f - 23 * (dyingIndex / rowAmount)), Time.deltaTime * flySpeed);
        else transform.position = Vector3.Lerp(transform.position, onPlayer ? new Vector3(-302.5f + 23 * (Index() % rowAmount), 105.5f - 23 * (Index() / rowAmount) - 19 * Board.board.player.actionBars.Count) : new Vector3(302.5f - 23 * (Index() % rowAmount), 105.5f - 23 * (Index() / rowAmount) - 19 * Board.board.enemy.actionBars.Count), Time.deltaTime * flySpeed);
    }

    public int Index() 
    {
        return onPlayer ? Board.board.temporaryBuffsPlayer.IndexOf(gameObject) : Board.board.temporaryBuffsEnemy.IndexOf(gameObject);
    }

    public void OnMouseUp()
    {
        if (cursor.render.sprite == null) return;
        if (pressEvent != null && GetComponent<Highlightable>().over)
        {
            PlaySound("DesktopButtonPress", 0.6f);
            pressEvent(GetComponent<Highlightable>());
        }
    }
}
