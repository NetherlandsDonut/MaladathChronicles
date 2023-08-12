using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Root.Color;

public class Buff
{
    public Buff(string name, string dispelType, List<string> tags, bool stackable, string icon, Func<bool, Action> description, Func<bool, Action> effects, Func<bool, Action> killEffects, Func<bool, FutureBoard, Action> futureEffects, Func<bool, FutureBoard, Action> futureKillEffects)
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
    public Func<bool, Action> description, effects, killEffects;
    public Func<bool, FutureBoard, Action> futureEffects, futureKillEffects;

    public static Entity Target(bool player)
    {
        return !player ? Board.board.player : Board.board.enemy;
    }

    public static FutureEntity Target(bool player, FutureBoard futureBoard)
    {
        return !player ? futureBoard.player : futureBoard.enemy;
    }

    public static Entity Caster(bool player)
    {
        if (Board.board == null) return currentSave.player;
        return player ? Board.board.player : Board.board.enemy;
    }

    public static FutureEntity Caster(bool player, FutureBoard futureBoard)
    {
        return player ? futureBoard.player : futureBoard.enemy;
    }

    public static List<Buff> buffs = new()
    {
        new Buff("Thunderstorm", "None", new() { "Damage" }, true, "AbilityThunderstorm",
        (p) => () =>
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
            var caster = Caster(p);
            var target = Target(p);
            var list = new List<(int, int)>();
            var col = random.Next(0, Board.board.field.GetLength(0));
            for (int j = 0; j < Board.board.field.GetLength(1); j++)
                list.Add((col, j));
            PlaySound("AbilityThunderstormImpact" + random.Next(0, 4));
            PlaySound("AbilityThunderstormImpact4");
            foreach (var e in list)
            {
                SpawnShatter(5, 0.8, Board.board.window.LBRegionGroup.regions[e.Item2].bigButtons[e.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[5]);
                SpawnShatter(3, 0.6, Board.board.window.LBRegionGroup.regions[e.Item2].bigButtons[e.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[e.Item1, e.Item2]]);
                Board.board.field[e.Item1, e.Item2] = 0;
            }
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
        new Buff("Blizzard", "None", new() { "Damage" }, false, "AbilityBlizzard",
        (p) => () =>
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
        (p) => () =>
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
            (p) => () =>
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
        (p) => () =>
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
        (p) => () =>
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
        (p) => () =>
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
        (p) => () =>
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
        (p) => () =>
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
        (p) => () =>
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
        (p) => () =>
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
            (p) => () =>
            {
                var caster = Caster(p);
                AddHeaderRegion(() =>
                {
                    var damage = caster.WeaponDamage();
                    AddLine("Burn for " + (int)(damage.Item1 * (caster.SpellPower() / 10.0 + 1) * 1.5) + " - " + (int)(damage.Item2 * (caster.SpellPower() / 10.0 + 1) * 1.5), Gray);
                    AddLine("damage every turn.", Gray);
                });
                AddHeaderRegion(() =>
                {
                    SetRegionAsGroupExtender();
                    AddLine(caster.Stats()["Fire Mastery"] + "% chance on flaring", Gray);
                    AddLine("to prolong debuff's duration by 1 turn.", Gray);
                });
            },
            (p) => () =>
            {
                var caster = Caster(p);
                var target = Target(p);
                target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 1.5);
                SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityScorch");
                PlaySound("AbilityScorchFlare");
                animationTime += frameTime * 3;
            },
            (p) => () =>
            {

            },
            (p, board) => () =>
            {
                var caster = Caster(p, board);
                var target = Target(p, board);
                target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 1.5);
            },
            (p, board) => () =>
            {

            }
        ),
        new Buff("Vanquished Tentacle of C'Thun", "None", new() { "Damage" }, false, "itemmiscahnqirajtrinket05",
            (p) => () =>
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
                var caster = Caster(p);
                var target = Target(p);
                caster.Damage(target.RollWeaponDamage() * target.SpellPower() * 1.0);
                SpawnShatter(2, 0.8, new Vector3(!p ? 148 : -318, 122), "ItemTrinketQiraj5");
                PlaySound("AbilityEnvenomImpact");
                animationTime += frameTime * 3;
            },
            (p) => () =>
            {

            },
            (p, board) => () =>
            {
                var caster = Caster(p, board);
                var target = Target(p, board);
                caster.Damage(target.RollWeaponDamage());
            },
            (p, board) => () =>
            {

            }
        ),
        new Buff("Venomous Bite", "None", new() { "Damage" }, false, "AbilityVenomousBite",
        (p) => () =>
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
            var caster = Caster(p);
            var target = Target(p);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 1.175);
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityVenomousBite");
            PlaySound("AbilityVenomousBiteFlare");
            animationTime += frameTime * 3;
        },
        (p) => () =>
        {

        },
        (p, board) => () =>
        {
            var caster = Caster(p, board);
            var target = Target(p, board);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 1.175);
        },
        (p, board) => () =>
        {

        }),
        new Buff("Putrid Bite", "None", new() { "Damage" }, false, "AbilityPutridBite",
        (p) => () =>
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
            var caster = Caster(p);
            var target = Target(p);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 1.35);
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityPutridBite");
            PlaySound("AbilityVenomousBiteFlare");
            animationTime += frameTime * 3;
        },
        (p) => () =>
        {

        },
        (p, board) => () =>
        {
            var caster = Caster(p, board);
            var target = Target(p, board);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 1.35);
        },
        (p, board) => () =>
        {

        }),
        new Buff("Corruption", "None", new() { "Damage" }, false, "AbilityCorruption",
        (p) => () =>
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
            var caster = Caster(p);
            var target = Target(p);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 0.8);
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityCorruption");
            PlaySound("AbilityCorruptionFlare");
            animationTime += frameTime * 3;
        },
        (p) => () =>
        {

        },
        (p, board) => () =>
        {
            var caster = Caster(p, board);
            var target = Target(p, board);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 0.8);
        },
        (p, board) => () =>
        {

        }),
        new Buff("Holy Fire", "None", new() { "Damage" }, false, "AbilityHolyFire",
        (p) => () =>
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
            var caster = Caster(p);
            var target = Target(p);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 1.0);
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityHolyFire");
            PlaySound("AbilityHolyFireFlare");
            animationTime += frameTime * 3;
        },
        (p) => () =>
        {

        },
        (p, board) => () =>
        {
            var caster = Caster(p, board);
            var target = Target(p, board);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 1.0);
        },
        (p, board) => () =>
        {

        }),
        new Buff("Curse Of Agony", "None", new() { "Damage" }, false, "AbilityCurseOfAgony",
        (p) => () =>
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
            var caster = Caster(p);
            var target = Target(p);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 0.9);
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityCurseOfAgony");
            PlaySound("AbilityCurseOfAgonyFlare");
            animationTime += frameTime * 3;
        },
        (p) => () =>
        {

        },
        (p, board) => () =>
        {
            var caster = Caster(p);
            var target = Target(p);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 0.9);
        },
        (p, board) => () =>
        {

        }),
        new Buff("Shadow Word: Pain", "None", new() { "Damage" }, false, "AbilityShadowWordPain",
        (p) => () =>
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
            var caster = Caster(p);
            var target = Target(p);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 0.6);
            SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityShadowWordPain");
            PlaySound("AbilityShadowWordPainImpact");
            animationTime += frameTime * 3;
        },
        (p) => () =>
        {

        },
        (p, board) => () =>
        {
            var caster = Caster(p, board);
            var target = Target(p, board);
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 10.0 + 1) * 0.6);
        },
        (p, board) => () =>
        {

        }),
        new Buff("Fel Armor", "None", new() { "Defensive" }, false, "AbilityFelArmor",
        (p) => () =>
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
        (p) => () =>
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
        (p) => () =>
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
