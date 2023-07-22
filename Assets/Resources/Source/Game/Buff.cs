using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Color;

public class Buff
{
    public Buff(string name, string dispelType, int duration, bool stackable, string icon, Action description, Func<bool, Action> effects, Func<bool, Action> killEffects)
    {
        this.name = name;
        this.dispelType = dispelType;
        this.duration = duration;
        this.stackable = stackable;
        this.icon = icon;
        this.description = description;
        this.effects = effects;
        this.killEffects = killEffects;
    }

    public string name, icon, dispelType;
    public int duration;
    public bool stackable;
    public Action description;
    public Func<bool, Action> effects, killEffects;

    public static List<Buff> buffs = new()
    {
        new Buff("Blizzard", "None", 0, false, "AbilityBlizzard",
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
        new Buff("Ice Block", "None", 0, false, "AbilityIceBlock",
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
        new Buff("Hammer Of Justice", "None", 0, false, "AbilityHammerOfJustice",
        () =>
        {
            AddHeaderRegion(() =>
            {
                AddLine("Target is stunned till the debuff runs out.", Gray);
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
        new Buff("Summoned Infernal", "None", 0, true, "AbilitySummonInfernal",
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
            var caster = p ? Board.board.enemy : Board.board.player;
            caster.resources["Fire"] += 3;
            SpawnShatterElement(2, 0.8, new Vector3(p ? 148 : -318, 122), "ElementFireRousing");
            SpawnShatterElement(2, 0.8, new Vector3(p ? 148 : -318, 122), "ElementFireRousing");
            SpawnShatterElement(2, 0.8, new Vector3(p ? 148 : -318, 122), "ElementFireRousing");
        },
        (p) => () =>
        {

        }),
        new Buff("Summoned Felhunter", "None", 0, true, "AbilitySummonFelhunter",
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
            var caster = p ? Board.board.enemy : Board.board.player;
            caster.resources["Arcane"] += 2;
            SpawnShatterElement(2, 0.8, new Vector3(p ? 148 : -318, 122), "ElementArcaneRousing");
            SpawnShatterElement(2, 0.8, new Vector3(p ? 148 : -318, 122), "ElementArcaneRousing");
        },
        (p) => () =>
        {

        }),
        new Buff("Summoned Voidwalker", "None", 0, true, "AbilitySummonVoidwalker",
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
            var caster = p ? Board.board.enemy : Board.board.player;
            caster.resources["Shadow"] += 2;
            SpawnShatterElement(2, 0.8, new Vector3(p ? 148 : -318, 122), "ElementShadowRousing");
            SpawnShatterElement(2, 0.8, new Vector3(p ? 148 : -318, 122), "ElementShadowRousing");
        },
        (p) => () =>
        {

        }),
        new Buff("Summoned Imp", "None", 0, true, "AbilitySummonImp",
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
            var caster = p ? Board.board.enemy : Board.board.player;
            caster.resources["Fire"]++;
            SpawnShatterElement(2, 0.8, new Vector3(p ? 148 : -318, 122), "ElementFireRousing");
        },
        (p) => () =>
        {

        }),
        new Buff("Scorch", "None", 0, false, "AbilityScorch",
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
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityScorch");
            PlaySound("AbilityScorchFlare");
            animationTime += frameTime * 3;
        },
        (p) => () =>
        {

        }),
        new Buff("Corruption", "None", 0, false, "AbilityCorruption",
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
        },
        (p) => () =>
        {

        }),
        new Buff("Curse Of Agony", "None", 0, false, "AbilityCurseOfAgony",
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
        new Buff("Shadow Word: Pain", "None", 0, false, "AbilityShadowWordPain",
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
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityShadowWordPain");
            PlaySound("AbilityShadowWordPainImpact");
            animationTime += frameTime * 3;
        },
        (p) => () =>
        {

        }),
        new Buff("Fel Armor", "None", 0, false, "AbilityFelArmor",
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
        new Buff("Power Word: Shield", "None", 0, false, "AbilityPowerWordShield",
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
        new Buff("Demon Skin", "None", 0, false, "AbilityDemonSkin",
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
