using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Color;

public class Buff
{
    public Buff(string name, string dispelType, int duration, bool durationInTurns, string icon, Action description, Func<bool, Action> effects, Func<bool, Action> killEffects)
    {
        this.name = name;
        this.dispelType = dispelType;
        this.duration = duration;
        this.durationInTurns = durationInTurns;
        this.icon = icon;
        this.description = description;
        this.effects = effects;
        this.killEffects = killEffects;
    }

    public string name, icon, dispelType;
    public int duration;
    public bool durationInTurns;
    public Action description;
    public Func<bool, Action> effects, killEffects;

    public static List<Buff> buffs = new()
    {
        new Buff("Blizzard", "None", 0, true, "AbilityBlizzard",
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
            //var target = p ? Board.board.player : Board.board.enemy;
            //var count = (p ? Board.board.temporaryElementsEnemy : Board.board.temporaryElementsPlayer).Count(x => x.Item2 == 16);
            //if (count > 0)
            //{
            //    target.health -= count;
            //    SpawnShatter(2, 0.8, new Vector3(!p ? 148 : -318, 122), "AbilityBlizzard");
            //    PlaySound("AbilityFrostBoltImpact");
            //}
        },
        (p) => () =>
        {

        }),
        new Buff("Ice Block", "None", 0, true, "AbilityIceBlock",
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
            if (p) Board.board.enemyFinishedMoving = true;
            else Board.board.playerFinishedMoving = true;
        },
        (p) => () =>
        {

        }),
        new Buff("Hammer Of Justice", "None", 0, true, "AbilityHammerOfJustice",
        () =>
        {
            AddHeaderRegion(() =>
            {
                AddLine("Target is stunned till the debuff runs out.", Gray);
            });
        },
        (p) => () =>
        {
            if (p) Board.board.enemyFinishedMoving = true;
            else Board.board.playerFinishedMoving = true;
        },
        (p) => () =>
        {

        }),
        new Buff("Corruption", "None", 0, true, "AbilityCorruption",
        () =>
        {
            AddHeaderRegion(() =>
            {
                AddLine("Target burns for 3 damage every turn.", Gray);
            });
        },
        (p) => () =>
        {
            var target = p ? Board.board.enemy : Board.board.player;
            target.health -= 2;
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityCorruption");
            PlaySound("AbilityCorruptionFlare");
            animationTime += frameTime * 3;
        },
        (p) => () =>
        {

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
        },
        (p) => () =>
        {

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

        },
        (p) => () =>
        {

        }),
    };
}
