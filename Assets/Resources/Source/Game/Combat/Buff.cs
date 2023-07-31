using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Color;

public class Buff
{
    public Buff(string name, string dispelType, List<string> tags, bool stackable, string icon, Action description, Func<bool, Action> effects, Func<bool, Action> killEffects, Func<bool, FutureBoard, Action> futureEffects, Func<bool, FutureBoard, Action> futureKillEffects)
    {
        this.name = name;
        this.dispelType = dispelType;
        this.tags = tags;
        this.stackable = stackable;
        this.icon = icon;
        this.description = description;
        this.effects = effects;
        this.killEffects = killEffects;
        this.futureEffects = futureEffects;
        this.futureKillEffects = futureKillEffects;
    }

    public string name, icon, dispelType;
    public List<string> tags;
    public bool stackable;
    public Action description;
    public Func<bool, Action> effects, killEffects;
    public Func<bool, FutureBoard, Action> futureEffects, futureKillEffects;

    public static List<Buff> buffs = new()
    {
        new Buff("Blizzard", "None", new() { "Damage" }, false, "AbilityBlizzard",
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

        },
        (p, board) => () =>
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
        (p, board) => () =>
        {

        }),
        new Buff("Withering Cloud", "None", new() { "Gathering" }, false, "AbilityWitheringCloud",
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
            var list = new List<(int, int)>();
            for (int i = 0; i < Board.board.field.GetLength(0); i++)
                for (int j = 0; j < Board.board.field.GetLength(1); j++)
                    if (Board.board.field[i, j] == 17)
                        list.Add((i, j));
            var newList = list.Select(x => (x.Item1, x.Item2, Board.board.FloodCount(x.Item1, x.Item2).Count)).ToList();
            if (newList.Count > 0)
            {
                Board.board.actions.Add(() => { PlaySound("AbilityWitheringCloudFlare"); });
                if (newList.Max(x => x.Count > 3))
                    newList.RemoveAll(x => x.Count != 3);
                else if (newList.Max(x => x.Count > 2))
                    newList.RemoveAll(x => x.Count < 3);
                else if (newList.Max(x => x.Count > 1))
                    newList.RemoveAll(x => x.Count < 2);
                while (true)
                {
                    var rand1 = list[random.Next(0, newList.Count)];
                    var rand2 = random.Next(0, 4);
                    if (rand2 == 0 && rand1.Item1 > 0 && Board.board.field[rand1.Item1 - 1, rand1.Item2] != 17)
                    {
                        Board.board.field[rand1.Item1 - 1, rand1.Item2] = 17;
                        SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[rand1.Item2].bigButtons[rand1.Item1 - 1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[rand1.Item1 - 1, rand1.Item2]]);
                        break;
                    }
                    else if (rand2 == 1 && rand1.Item2 > 0 && Board.board.field[rand1.Item1, rand1.Item2 - 1] != 17)
                    {
                        Board.board.field[rand1.Item1, rand1.Item2 - 1] = 17;
                        SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[rand1.Item2 - 1].bigButtons[rand1.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[rand1.Item1, rand1.Item2 - 1]]);
                        break;
                    }
                    else if (rand2 == 2 && rand1.Item1 < Board.board.field.GetLength(0) - 1 && Board.board.field[rand1.Item1 + 1, rand1.Item2] != 17)
                    {
                        Board.board.field[rand1.Item1 + 1, rand1.Item2] = 17;
                        SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[rand1.Item2].bigButtons[rand1.Item1 + 1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[rand1.Item1 + 1, rand1.Item2]]);
                        break;
                    }
                    else if (rand2 == 3 && rand1.Item2 < Board.board.field.GetLength(1) - 1 && Board.board.field[rand1.Item1, rand1.Item2 + 1] != 17)
                    {
                        Board.board.field[rand1.Item1, rand1.Item2 + 1] = 17;
                        SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[rand1.Item2 + 1].bigButtons[rand1.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[rand1.Item1, rand1.Item2 + 1]]);
                        break;
                    }
                }
            }
            else
            {
                var x = random.Next(0, Board.board.field.GetLength(0));
                var y = random.Next(0, Board.board.field.GetLength(1));
                Board.board.field[x, y] = 17;
                SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[y].bigButtons[x].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[x, y]]);
            }
        },
        (p) => () =>
        {

        },
        (p, board) => () =>
        {
            var list = new List<(int, int)>();
            for (int i = 0; i < board.field.GetLength(0); i++)
                for (int j = 0; j < board.field.GetLength(1); j++)
                    if (board.field[i, j] == 17)
                        list.Add((i, j));
            var newList = list.Select(x => (x.Item1, x.Item2, board.FloodCount(x.Item1, x.Item2).Count)).ToList();
            newList.RemoveAll(x => x.Count == 4);
            if (newList.Count > 0)
            {
                if (newList.Max(x => x.Count) == 3)
                    newList.RemoveAll(x => x.Count < 3);
                else if (newList.Max(x => x.Count) == 2)
                    newList.RemoveAll(x => x.Count < 2);
                while (true)
                {
                    var rand1 = newList[random.Next(0, newList.Count)];
                    var rand2 = random.Next(0, 4);
                    if (rand2 == 0 && rand1.Item1 > 0 && board.field[rand1.Item1 - 1, rand1.Item2] != 17)
                    {
                        board.field[rand1.Item1 - 1, rand1.Item2] = 17;
                        break;
                    }
                    else if (rand2 == 1 && rand1.Item2 > 0 && board.field[rand1.Item1, rand1.Item2 - 1] != 17)
                    {
                        board.field[rand1.Item1, rand1.Item2 - 1] = 17;
                        break;
                    }
                    else if (rand2 == 2 && rand1.Item1 < board.field.GetLength(0) - 1 && board.field[rand1.Item1 + 1, rand1.Item2] != 17)
                    {
                        board.field[rand1.Item1 + 1, rand1.Item2] = 17;
                        break;
                    }
                    else if (rand2 == 3 && rand1.Item2 < board.field.GetLength(1) - 1 && board.field[rand1.Item1, rand1.Item2 + 1] != 17)
                    {
                        board.field[rand1.Item1, rand1.Item2 + 1] = 17;
                        break;
                    }
                }
            }
            else
                board.field[random.Next(0, board.field.GetLength(0)), random.Next(0, board.field.GetLength(1))] = 17;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Stoneform", "None", new() { "Defensive" }, false, "AbilityStoneform",
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

            },
            (p, board) => () =>
            {

            },
            (p, board) => () =>
            {

            }
        ),
        new Buff("Ice Block", "None", new() { "Defensive" }, false, "AbilityIceBlock",
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

        },
        (p, board) => () =>
        {
            if (p) board.enemyFinishedMoving = true;
            else board.playerFinishedMoving = true;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Hammer Of Justice", "None", new() { "Stun" }, false, "AbilityHammerOfJustice",
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

        },
        (p, board) => () =>
        {
            if (p) board.enemyFinishedMoving = true;
            else board.playerFinishedMoving = true;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Web Burst", "None", new() { "Stun" }, false, "AbilityWebBurst",
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

        },
        (p, board) => () =>
        {
            if (p) board.enemyFinishedMoving = true;
            else board.playerFinishedMoving = true;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Summoned Infernal", "None", new() { }, true, "AbilitySummonInfernal",
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

        },
        (p, board) => () =>
        {
            var caster = p ? board.enemy : board.player;
            caster.resources["Fire"] += 3;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Summoned Felhunter", "None", new() { }, true, "AbilitySummonFelhunter",
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

        },
        (p, board) => () =>
        {
            var caster = p ? board.enemy : board.player;
            caster.resources["Arcane"] += 2;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Summoned Voidwalker", "None", new() { }, true, "AbilitySummonVoidwalker",
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

        },
        (p, board) => () =>
        {
            var caster = p ? board.enemy : board.player;
            caster.resources["Shadow"] += 2;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Summoned Imp", "None", new() { }, true, "AbilitySummonImp",
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

        },
        (p, board) => () =>
        {
            var caster = p ? board.enemy : board.player;
            caster.resources["Fire"]++;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Scorch", "None", new() { "Damage" }, false, "AbilityScorch",
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

        },
        (p, board) => () =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 3;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Venomous Bite", "None", new() { "Damage" }, false, "AbilityVenomousBite",
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
            target.health -= 4;
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityVenomousBite");
            PlaySound("AbilityVenomousBiteFlare");
            animationTime += frameTime * 3;
        },
        (p) => () =>
        {

        },
        (p, board) => () =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 4;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Putrid Bite", "None", new() { "Damage" }, false, "AbilityPutridBite",
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
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityPutridBite");
            PlaySound("AbilityVenomousBiteFlare");
            animationTime += frameTime * 3;
        },
        (p) => () =>
        {

        },
        (p, board) => () =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 3;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Corruption", "None", new() { "Damage" }, false, "AbilityCorruption",
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

        },
        (p, board) => () =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 2;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Curse Of Agony", "None", new() { "Damage" }, false, "AbilityCurseOfAgony",
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

        },
        (p, board) => () =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 3;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Shadow Word: Pain", "None", new() { "Damage" }, false, "AbilityShadowWordPain",
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

        },
        (p, board) => () =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 3;
        },
        (p, board) => () =>
        {

        }),
        new Buff("Fel Armor", "None", new() { "Defensive" }, false, "AbilityFelArmor",
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

        },
        (p, board) => () =>
        {

        },
        (p, board) => () =>
        {

        }),
        new Buff("Power Word: Shield", "None", new() { "Defensive" }, false, "AbilityPowerWordShield",
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

        },
        (p, board) => () =>
        {

        },
        (p, board) => () =>
        {

        }),
        new Buff("Demon Skin", "None", new() { "Defensive" }, false, "AbilityDemonSkin",
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

        },
        (p, board) => () =>
        {

        },
        (p, board) => () =>
        {

        }),
    };
}
