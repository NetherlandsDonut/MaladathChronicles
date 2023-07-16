using System;
using System.Collections.Generic;
using UnityEngine;
using static Root;
using static Root.Color;

public class Ability
{
    //Passive ability
    public Ability(string name)
    {
        this.name = name;
    }

    //Active combat ability
    public Ability(string name, int cooldown, Dictionary<string, int> cost, Action description, Action<bool> effects)
    {
        this.name = name;
        this.cooldown = cooldown;
        this.cost = cost;
        this.description = description;
        this.effects = effects;
    }

    public bool EnoughResources(Entity entity)
    {
        foreach (var resource in cost)
            if (entity.resources[resource.Key] < resource.Value)
                return false;
        return true;
    }

    public string name;
    public int cooldown;
    public Dictionary<string, int> cost;
    public Action description;
    public Action<bool> effects;

    public static List<Ability> abilities = new()
    {
        new Ability("Two Handed Axe Proficiency"),
        new Ability("Two Handed Mace Proficiency"),
        new Ability("Two Handed Sword Proficiency"),
        new Ability("One Handed Axe Proficiency"),
        new Ability("One Handed Mace Proficiency"),
        new Ability("One Handed Sword Proficiency"),
        new Ability("Fist Weapon Proficiency"),
        new Ability("Off Hand Proficiency"),
        new Ability("Polearm Proficiency"),
        new Ability("Dagger Proficiency"),
        new Ability("Staff Proficiency"),
        new Ability("Wand Proficiency"),
        new Ability("Totem Proficiency"),
        new Ability("Relic Proficiency"),
        new Ability("Libram Proficiency"),
        new Ability("Idol Proficiency"),
        new Ability("Shield Proficiency"),
        new Ability("Quiver Proficiency"),
        new Ability("Pouch Proficiency"),
        new Ability("Bow Proficiency"),
        new Ability("Crossbow Proficiency"),
        new Ability("Gun Proficiency"),
        new Ability("Cloth Proficiency"),
        new Ability("Leather Proficiency"),
        new Ability("Mail Proficiency"),
        new Ability("Plate Proficiency"),
        new Ability("Envenom", 0, new()
        {
            { "Order", 1 }
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Blizzard will shroud a target in a", Gray);
                AddLine("freezing cloud that will rain ice shards at", Gray);
                AddLine("them whenever the spelll caster gains", Gray);
                AddLine("new frost elements from the board.", Gray);
            });
            AddPaddingRegion(() =>
            {
                AddLine("Each frost element collected deals 3 damage ", Gray);
                AddLine("scaled with caster\'s Intelligence.", Gray);
            });
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("for a falling frost shard to call upon", Gray);
                AddLine("another one to hit the target again for", Gray);
                AddLine("the same amount of damage.", Gray);
            });
        },
        (p) =>
        {

        }),
        new Ability("Rupture", 0, new()
        {
            { "Order", 1 }
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Blizzard will shroud a target in a", Gray);
                AddLine("freezing cloud that will rain ice shards at", Gray);
                AddLine("them whenever the spelll caster gains", Gray);
                AddLine("new frost elements from the board.", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Each frost element collected deals 3 damage ", Gray);
                AddLine("scaled with caster\'s Intelligence.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("for a falling frost shard to call upon", Gray);
                AddLine("another one to hit the target again for", Gray);
                AddLine("the same amount of damage.", Gray);
            });
        },
        (p) =>
        {

        }),
        new Ability("Mutilate", 0, new()
        {
            { "Order", 1 }
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Blizzard will shroud a target in a", Gray);
                AddLine("freezing cloud that will rain ice shards at", Gray);
                AddLine("them whenever the spelll caster gains", Gray);
                AddLine("new frost elements from the board.", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Each frost element collected deals 3 damage ", Gray);
                AddLine("scaled with caster\'s Intelligence.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("for a falling frost shard to call upon", Gray);
                AddLine("another one to hit the target again for", Gray);
                AddLine("the same amount of damage.", Gray);
            });
        },
        (p) =>
        {

        }),
        new Ability("Kidney Shot", 0, new()
        {
            { "Order", 1 }
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Blizzard will shroud a target in a", Gray);
                AddLine("freezing cloud that will rain ice shards at", Gray);
                AddLine("them whenever the spelll caster gains", Gray);
                AddLine("new frost elements from the board.", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Each frost element collected deals 3 damage ", Gray);
                AddLine("scaled with caster\'s Intelligence.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("for a falling frost shard to call upon", Gray);
                AddLine("another one to hit the target again for", Gray);
                AddLine("the same amount of damage.", Gray);
            });
        },
        (p) =>
        {

        }),
        new Ability("Evasion", 0, new()
        {
            { "Order", 1 }
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Blizzard will shroud a target in a", Gray);
                AddLine("freezing cloud that will rain ice shards at", Gray);
                AddLine("them whenever the spelll caster gains", Gray);
                AddLine("new frost elements from the board.", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Each frost element collected deals 3 damage ", Gray);
                AddLine("scaled with caster\'s Intelligence.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("for a falling frost shard to call upon", Gray);
                AddLine("another one to hit the target again for", Gray);
                AddLine("the same amount of damage.", Gray);
            });
        },
        (p) =>
        {

        }),
        new Ability("Garrote", 0, new()
        {
            { "Order", 1 }
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Blizzard will shroud a target in a", Gray);
                AddLine("freezing cloud that will rain ice shards at", Gray);
                AddLine("them whenever the spelll caster gains", Gray);
                AddLine("new frost elements from the board.", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Each frost element collected deals 3 damage ", Gray);
                AddLine("scaled with caster\'s Intelligence.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("for a falling frost shard to call upon", Gray);
                AddLine("another one to hit the target again for", Gray);
                AddLine("the same amount of damage.", Gray);
            });
        },
        (p) =>
        {

        }),
        new Ability("Frostbolt", 0, new()
        {
            { "Frost", 4 }
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Frosbolt will channel a frosty", Gray);
                AddLine("projectile at the target dealing damage.", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Deals 8 damage times caster's intelligence.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("to refund the cost of casting this spell.", Gray);
            });
        },
        (p) =>
        {
            var caster = p ? Board.board.player : Board.board.enemy;
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                Board.board.window.PlaySound("AbilityFrostboltCast");
                animationTime += frameTime * 6;
            });
            Board.board.actions.Add(() =>
            {
                target.health -= 8;
                Board.board.window.PlaySound("AbilityFrostboltImpact");
            });
            Board.board.playerFinishedMoving = true;
            Board.board.StartAnimations();
        }),
        new Ability("Ice Lance", 0, new()
        {
            { "Frost", 5 },
            { "Air", 2 }
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Ice Lance will hurl an ice spike", Gray);
                AddLine("at the target dealing damage.", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Deals 6 damage times caster's intelligence.", Gray);
                AddLine("Does not end the caster's turn.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("to turn a random non frost element on the", Gray);
                AddLine("board into a frost element.", Gray);
            });
        },
        (p) =>
        {
            var caster = p ? Board.board.player : Board.board.enemy;
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                Board.board.window.PlaySound("AbilityIceLanceCast");
                animationTime += frameTime * 3;
            });
            Board.board.actions.Add(() =>
            {
                target.health -= 6;
                Board.board.window.PlaySound("AbilityIceLanceImpact");
            });
            Board.board.StartAnimations();
        }),
        new Ability("Freezing Nova", 1, new()
        {
            { "Frost", 2 },
            { "Air", 6 },
            { "Water", 2 }
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Freezing Nova will spawn a", Gray);
                AddLine("freezing shockwave that will freeze", Gray);
                AddLine("random elements on the board.", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Turns random 5 non frost elements", Gray);
                AddLine("on the board into frost ones.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 5% chance", Gray);
                AddLine("to turn another element into a frost one.", Gray);
                AddLine("This effect loops and for example 120%", Gray);
                AddLine("has a 100% chance to turn another", Gray);
                AddLine("element and 20%for a second bonus one.", Gray);
            });
        },
        (p) =>
        {
            Board.board.actions.Add(() =>
            {
                Board.board.window.PlaySound("AbilityFreezingNovaCast");
            });
            var list = new List<(int, int)>();
            while (list.Count < 5)
            {
                var x = random.Next(0, Board.board.field.GetLength(0));
                var y = random.Next(0, Board.board.field.GetLength(1));
                if (Board.board.field[x, y] != 16 && !list.Contains((x, y)))
                    list.Add((x, y));
            }
            Board.board.actions.Add(() =>
            {
                Board.board.field[list[0].Item1, list[0].Item2] = 16;
                Board.board.field[list[1].Item1, list[1].Item2] = 16;
                Board.board.field[list[2].Item1, list[2].Item2] = 16;
                Board.board.field[list[3].Item1, list[3].Item2] = 16;
                Board.board.field[list[4].Item1, list[4].Item2] = 16;
                SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[list[0].Item2].bigButtons[list[0].Item1].transform.position + new Vector3(-17.5f, -17.5f), "ElementFrostRousing", false);
                SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[list[1].Item2].bigButtons[list[1].Item1].transform.position + new Vector3(-17.5f, -17.5f), "ElementFrostRousing", false);
                SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[list[2].Item2].bigButtons[list[2].Item1].transform.position + new Vector3(-17.5f, -17.5f), "ElementFrostRousing", false);
                SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[list[3].Item2].bigButtons[list[3].Item1].transform.position + new Vector3(-17.5f, -17.5f), "ElementFrostRousing", false);
                SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[list[4].Item2].bigButtons[list[4].Item1].transform.position + new Vector3(-17.5f, -17.5f), "ElementFrostRousing", false);
                Board.board.window.PlaySound("AbilityFreezingNovaImpact");
            });
            Board.board.actions.Add(() =>
            {
                animationTime += frameTime * 15;
            });
            Board.board.playerFinishedMoving = true;
            Board.board.StartAnimations();
        }),
        new Ability("Blizzard", 4, new()
        {
            { "Frost", 10 },
            { "Air", 10 }
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Blizzard will shroud a target in a", Gray);
                AddLine("freezing cloud that will rain ice shards at", Gray);
                AddLine("them whenever the spelll caster gains", Gray);
                AddLine("new frost elements from the board.", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Each frost element collected deals 3 damage ", Gray);
                AddLine("scaled with caster\'s Intelligence.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 1% chance", Gray);
                AddLine("for a falling frost shard to call upon", Gray);
                AddLine("another one to hit the target again for", Gray);
                AddLine("the same amount of damage.", Gray);
            });
        },
        (p) =>
        {

        }),
        new Ability("Deep Freeze", 5, new()
        {
            { "Frost", 15 }
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Deep Freeze will freeze the board", Gray);
                AddLine("turning elements into different ones.", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("All fire elements will be turned into", Gray);
                AddLine("air elements and water and decay elements", Gray);
                AddLine("into frost elements.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Frost Mastery adds 10%", Gray);
                AddLine("chance to turn a random non water nor", Gray);
                AddLine("fire element into a frost element.", Gray);
                AddLine("This effect loops and for example 140%", Gray);
                AddLine("has a 100% chance to spawn one element", Gray);
                AddLine("and 40% to spawn another one.", Gray);
            });
        },
        (p) =>
        {
            Board.board.actions.Add(() =>
            {
                Board.board.window.PlaySound("AbilityDeepFreezeCast");
                animationTime += frameTime * 6;
            });
            var list1 = new List<(int, int)>();
            var list2 = new List<(int, int)>();
            for (int i = 0; i < Board.board.field.GetLength(0); i++)
                for (int j = 0; j < Board.board.field.GetLength(1); j++)
                    if (Board.board.field[i, j] == 12)
                        list1.Add((i, j));
                    else if (Board.board.field[i, j] == 13 || Board.board.field[i, j] == 17)
                        list2.Add((i, j));
            Board.board.actions.Add(() =>
            {
                foreach (var e in list1)
                {
                    Board.board.field[e.Item1, e.Item2] = 14;
                    SpawnShatter(4, 0.3, Board.board.window.LBRegionGroup.regions[e.Item2].bigButtons[e.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[e.Item1, e.Item2]], false);
                }
                foreach (var e in list2)
                {
                    Board.board.field[e.Item1, e.Item2] = 16;
                    SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[e.Item2].bigButtons[e.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[e.Item1, e.Item2]], false);
                }
            });
            Board.board.actions.Add(() =>
            {
                animationTime += frameTime * 15;
            });
            Board.board.playerFinishedMoving = true;
            Board.board.StartAnimations();
        })
    };
}
