using System;
using UnityEngine;
using System.Collections.Generic;

using static Root;
using static Root.Color;

public class Buff
{
    public Buff(string name, string dispelType, int duration, bool durationInTurns, string icon, Action description, Func<bool, Action> effects)
    {
        this.name = name;
        this.dispelType = dispelType;
        this.duration = duration;
        this.durationInTurns = durationInTurns;
        this.description = description;
        this.effects = effects;
    }

    public string name, icon, dispelType;
    public int duration;
    public bool durationInTurns;
    public Action description;
    public Func<bool, Action> effects;

    public static List<Buff> buffs = new()
    {
        new Buff("Corruption", "None", 0, true, "AbilityCorruption",
        () =>
        {
            AddHeaderRegion(() =>
            {
                AddLine("Target burns for 3 damage every turn.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("to refund the cost of casting this spell.", Gray);
            });
        },
        (p) => () =>
        {
            var target = p ? Board.board.enemy : Board.board.player;
            target.health -= 2;
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityCorruption");
            PlaySound("AbilityCorruptionFlare");
            animationTime += frameTime * 3;
        }),
        new Buff("Curse Of Agony", "None", 0, true, "AbilityCurseOfAgony",
        () =>
        {
            AddHeaderRegion(() =>
            {
                AddLine("Target burns for 3 damage every turn.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("to refund the cost of casting this spell.", Gray);
            });
        },
        (p) => () =>
        {
            var target = p ? Board.board.enemy : Board.board.player;
            target.health -= 3;
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityCurseOfAgony");
            PlaySound("AbilityCurseOfAgonyFlare");
            animationTime += frameTime * 3;
        }),
        new Buff("Fel Armor", "None", 0, true, "AbilityFelArmor",
        () =>
        {
            AddHeaderRegion(() =>
            {
                AddLine("Target burns for 3 damage every turn.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("to refund the cost of casting this spell.", Gray);
            });
        },
        (p) => () =>
        {

        }),
        new Buff("Demon Skin", "None", 0, true, "AbilityDemonSkin",
        () =>
        {
            AddHeaderRegion(() =>
            {
                AddLine("Target burns for 3 damage every turn.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("to refund the cost of casting this spell.", Gray);
            });
        },
        (p) => () =>
        {

        }),
    };
}
