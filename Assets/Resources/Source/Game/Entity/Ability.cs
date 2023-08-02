using System;
using System.Collections.Generic;
using UnityEngine;
using static Root;
using static Root.Color;
using static UnityEngine.GraphicsBuffer;

public class Ability
{
    //Passive ability
    public Ability(string name)
    {
        this.name = name;
    }

    //Active combat ability
    public Ability(string name, int cooldown, List<string> tags, Dictionary<string, int> cost, Action description, Action<bool> effects, Action<bool, FutureBoard> futureEffects)
    {
        this.name = name;
        this.cooldown = cooldown;
        this.tags = tags;
        this.cost = cost;
        this.description = description;
        this.effects = effects;
        this.futureEffects = futureEffects;
    }

    public bool EnoughResources(Entity entity)
    {
        foreach (var resource in cost)
            if (entity.resources[resource.Key] < resource.Value)
                return false;
        return true;
    }

    public bool EnoughResources(FutureEntity entity)
    {
        foreach (var resource in cost)
            if (entity.resources[resource.Key] < resource.Value)
                return false;
        return true;
    }

    public string name;
    public int cooldown;
    public List<string> tags;
    public Dictionary<string, int> cost;
    public Action description;
    public Action<bool> effects;
    public Action<bool, FutureBoard> futureEffects;

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
        new Ability("Envenom", 0, new() { "Damage", "Overtime" }, new()
        {
            { "Order", 1 }
        },
        () =>
        {

        },
        (p) =>
        {

        },
        (p, board) =>
        {

        }),
        new Ability("Rupture", 0, new() { "Damage", "Overtime" }, new()
        {
            { "Order", 1 }
        },
        () =>
        {

        },
        (p) =>
        {

        },
        (p, board) =>
        {

        }),
        new Ability("Mutilate", 0, new() { "Damage" }, new()
        {
            { "Order", 1 }
        },
        () =>
        {

        },
        (p) =>
        {

        },
        (p, board) =>
        {

        }),
        new Ability("Kidney Shot", 0, new() { "Stun" }, new()
        {
            { "Order", 1 }
        },
        () =>
        {

        },
        (p) =>
        {

        },
        (p, board) =>
        {

        }),
        new Ability("Evasion", 0, new() { "Defensive", "Emergency" }, new()
        {
            { "Order", 1 }
        },
        () =>
        {

        },
        (p) =>
        {

        },
        (p, board) =>
        {

        }),
        new Ability("Garrote", 0, new() { "Damage", "Overtime" }, new()
        {
            { "Order", 1 }
        },
        () =>
        {

        },
        (p) =>
        {

        },
        (p, board) =>
        {

        }),
        new Ability("Arcane Missiles", 0, new() { "Damage" }, new()
        {
            { "Arcane", 4 }
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
                PlaySound("AbilityArcaneMissilesCast", 0.3f);
                animationTime += frameTime * 4;
            });
            Board.board.actions.Add(() =>
            {
                target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 100.0 + 1));
                SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityArcaneMissiles");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityArcaneMissiles", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityArcaneMissiles", true, p ? "1000" : "1001");
                PlaySound("AbilityArcaneMissilesImpact1");
                animationTime += frameTime * 6;
            });
            Board.board.actions.Add(() =>
            {
                target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 100.0 + 1));
                SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityArcaneMissiles");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityArcaneMissiles", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityArcaneMissiles", true, p ? "1000" : "1001");
                PlaySound("AbilityArcaneMissilesImpact2");
                animationTime += frameTime * 6;
            });
            Board.board.actions.Add(() =>
            {
                target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 100.0 + 1));
                SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityArcaneMissiles");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityArcaneMissiles", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityArcaneMissiles", true, p ? "1000" : "1001");
                PlaySound("AbilityArcaneMissilesImpact3");
                animationTime += frameTime * 6;
            });
            Board.board.playerFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            var target = p ? board.enemy : board.player;
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 100.0 + 1));
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 100.0 + 1));
            target.Damage(caster.RollWeaponDamage() * (caster.SpellPower() / 100.0 + 1));
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Curse Of Agony", 4, new() { "Damage", "Overtime" }, new()
        {
            { "Shadow", 7 },
            { "Fire", 3 }
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
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                target.AddBuff("Curse Of Agony", 10, SpawnShatterBuff(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityCurseOfAgony", target));
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityCurseOfAgony", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityCurseOfAgony", true, p ? "1000" : "1001");
                PlaySound("AbilityCurseOfAgonyCast");
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.AddBuff("Curse Of Agony", 10);
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Venomous Bite", 3, new() { "Damage", "Overtime" }, new()
        {
            { "Decay", 6 },
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
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                target.AddBuff("Venomous Bite", 10, SpawnShatterBuff(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityVenomousBite", target));
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityVenomousBite", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityVenomousBite", true, p ? "1000" : "1001");
                PlaySound("AbilityVenomousBiteImpact");
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.AddBuff("Venomous Bite", 10);
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Withering Cloud", 5, new() { "Gathering" }, new()
        {
            { "Air", 6 },
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Withering Cloud will shroud the", Gray);
                AddLine("board with in a poisonous gas.", Gray);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Every turn a randomly chosen decay", Gray);
                AddLine("element on the board will spread itself", Gray);
                AddLine("and spawn a second one in one", Gray);
                AddLine("of the neighboring tiles.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Each point in Decay Mastery adds 1%", Gray);
                AddLine("to trigger this effect twice.", Gray);
            });
        },
        (p) =>
        {
            var caster = p ? Board.board.player : Board.board.enemy;
            Board.board.actions.Add(() =>
            {
                caster.AddBuff("Withering Cloud", 7, SpawnShatterBuff(2, 0.8, new Vector3(!p ? 148 : -318, 122), "AbilityWitheringCloud", caster));
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityWitheringCloud", true, !p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityWitheringCloud", true, !p ? "1000" : "1001");
                PlaySound("AbilityWitheringCloudCast");
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.AddBuff("Withering Cloud", 7);
        }),
        new Ability("Web Burst", 4, new() { "Stun" }, new()
        {
            { "Shadow", 4 },
            { "Decay", 4 }
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
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                PlaySound("AbilityWebBurstCast");
                animationTime += frameTime * 4;
            });
            Board.board.actions.Add(() =>
            {
                target.AddBuff("Web Burst", 2, SpawnShatterBuff(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityWebBurst", target));
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityWebBurst", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityWebBurst", true, p ? "1000" : "1001");
                PlaySound("AbilityWebBurstImpact");
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.AddBuff("Web Burst", 2);
        }),
        new Ability("Demon Skin", 0, new() { "Defensive" }, new()
        {
            { "Shadow", 6 },
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
            Board.board.actions.Add(() =>
            {
                caster.AddBuff("Demon Skin", 5, SpawnShatterBuff(2, 0.8, new Vector3(!p ? 148 : -318, 122), "AbilityDemonSkin", caster));
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityDemonSkin", true, !p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityDemonSkin", true, !p ? "1000" : "1001");
                PlaySound("AbilityDemonSkinCast");
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.AddBuff("Demon Skin", 5);
        }),
        new Ability("Stoneform", 20, new() { "Defensive" }, new()
        {
            { "Earth", 4 },
            { "Lightning", 4 },
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
            Board.board.actions.Add(() =>
            {
                caster.AddBuff("Stoneform", 4, SpawnShatterBuff(2, 0.8, new Vector3(!p ? 148 : -318, 122), "AbilityStoneform", caster));
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityStoneform", true, !p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityStoneform", true, !p ? "1000" : "1001");
                PlaySound("AbilityStoneformCast");
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.AddBuff("Stoneform", 4);
        }),
        new Ability("Ice Block", 0, new() { "Defensive", "Emergency" }, new()
        {
            { "Frost", 4 },
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
            Board.board.actions.Add(() =>
            {
                caster.AddBuff("Ice Block", 3, SpawnShatterBuff(2, 0.8, new Vector3(!p ? 148 : -318, 122), "AbilityIceBlock", caster));
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityIceBlock", true, !p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityIceBlock", true, !p ? "1000" : "1001");
                PlaySound("AbilityIceBlockCast");
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.AddBuff("Ice Block", 3);
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Power Word: Shield", 0, new() { "Defensive" }, new()
        {
            { "Order", 6 },
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
            Board.board.actions.Add(() =>
            {
                caster.AddBuff("Power Word: Shield", 5, SpawnShatterBuff(2, 0.8, new Vector3(!p ? 148 : -318, 122), "AbilityPowerWordShield", caster));
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityPowerWordShield", true, !p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityPowerWordShield", true, !p ? "1000" : "1001");
                //PlaySound("AbilityFelArmorCast");
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.AddBuff("Power Word: Shield", 5);
        }),
        new Ability("Fel Armor", 0, new() { "Defensive" }, new()
        {
            { "Shadow", 3 },
            { "Fire", 3 }
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
            Board.board.actions.Add(() =>
            {
                caster.AddBuff("Fel Armor", 5, SpawnShatterBuff(2, 0.8, new Vector3(!p ? 148 : -318, 122), "AbilityFelArmor", caster));
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityFelArmor", true, !p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilityFelArmor", true, !p ? "1000" : "1001");
                PlaySound("AbilityFelArmorCast");
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.AddBuff("Fel Armor", 5);
        }),
        new Ability("Hammer Of Justice", 0, new() { "Stun" }, new()
        {
            { "Order", 8 },
            { "Air", 2 }
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
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                PlaySound("AbilityHammerOfJusticeCast");
                animationTime += frameTime * 4;
            });
            Board.board.actions.Add(() =>
            {
                target.AddBuff("Hammer Of Justice", 2, SpawnShatterBuff(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityHammerOfJustice", target));
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityHammerOfJustice", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityHammerOfJustice", true, p ? "1000" : "1001");
                PlaySound("AbilityHammerOfJusticeImpact");
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.AddBuff("Hammer Of Justice", 2);
        }),
        new Ability("Shadow Word: Pain", 0, new() { "Damage", "Overtime" }, new()
        {
            { "Shadow", 6 }
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
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                target.AddBuff("Shadow Word: Pain", 5, SpawnShatterBuff(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityShadowWordPain", target));
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityShadowWordPain", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityShadowWordPain", true, p ? "1000" : "1001");
                PlaySound("AbilityShadowWordPainCast");
                animationTime += frameTime * 2;
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.AddBuff("Hammer Of Justice", 2);
        }),
        new Ability("Putrid Bite", 2, new() { "Damage", "Overtime" }, new()
        {
            { "Decay", 4 },
            { "Air", 2 },
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
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                target.AddBuff("Putrid Bite", 3, SpawnShatterBuff(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityPutridBite", target));
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityPutridBite", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityPutridBite", true, p ? "1000" : "1001");
                PlaySound("AbilityPutridBiteImpact");
                animationTime += frameTime * 2;
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.AddBuff("Putrid Bite", 3);
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Corruption", 1, new() { "Damage", "Overtime" }, new()
        {
            { "Shadow", 2 },
            { "Decay", 5 },
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
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                target.AddBuff("Corruption", 5, SpawnShatterBuff(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityCorruption", target));
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityCorruption", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityCorruption", true, p ? "1000" : "1001");
                PlaySound("AbilityCorruptionImpact");
                animationTime += frameTime * 2;
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.AddBuff("Corruption", 5);
        }),
        new Ability("Summon Infernal", 0, new() { "Damage", "Overtime" }, new()
        {
            { "Fire", 20 },
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
            Board.board.actions.Add(() =>
            {
                PlaySound("AbilitySummonInfernalCast");
                animationTime += frameTime * 6;
            });
            Board.board.actions.Add(() =>
            {
                caster.AddBuff("Summoned Infernal", 7, SpawnShatterBuff(2, 0.8, new Vector3(!p ? 148 : -318, 122), "AbilitySummonInfernal", caster));
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilitySummonInfernal", true, !p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilitySummonInfernal", true, !p ? "1000" : "1001");
                PlaySound("AbilitySummonInfernalImpact");
                animationTime += frameTime * 6;
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.AddBuff("Summoned Infernal", 7);
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Summon Felhunter", 0, new() { "Damage", "Overtime" }, new()
        {
            { "Shadow", 10 },
            { "Fire", 5 },
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
            Board.board.actions.Add(() =>
            {
                PlaySound("AbilitySummonFelhunterCast");
                animationTime += frameTime * 6;
            });
            Board.board.actions.Add(() =>
            {
                caster.AddBuff("Summoned Felhunter", 7, SpawnShatterBuff(2, 0.8, new Vector3(!p ? 148 : -318, 122), "AbilitySummonFelhunter", caster));
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilitySummonFelhunter", true, !p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilitySummonFelhunter", true, !p ? "1000" : "1001");
                PlaySound("AbilitySummonFelhunterImpact");
                animationTime += frameTime * 6;
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.AddBuff("Summoned Felhunter", 7);
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Summon Voidwalker", 0, new() { "Damage", "Overtime" }, new()
        {
            { "Shadow", 12 },
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
            Board.board.actions.Add(() =>
            {
                PlaySound("AbilitySummonVoidwalkerCast");
                animationTime += frameTime * 4;
            });
            Board.board.actions.Add(() =>
            {
                caster.AddBuff("Summoned Voidwalker", 7, SpawnShatterBuff(2, 0.8, new Vector3(!p ? 148 : -318, 122), "AbilitySummonVoidwalker", caster));
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilitySummonVoidwalker", true, !p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilitySummonVoidwalker", true, !p ? "1000" : "1001");
                PlaySound("AbilitySummonVoidwalkerImpact");
                animationTime += frameTime * 6;
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.AddBuff("Summoned Voidwalker", 7);
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Summon Imp", 0, new() { "Damage", "Overtime" }, new()
        {
            { "Shadow", 6 },
            { "Fire", 2 }
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
            Board.board.actions.Add(() =>
            {
                PlaySound("AbilitySummonImpCast");
                animationTime += frameTime * 2;
            });
            Board.board.actions.Add(() =>
            {
                caster.AddBuff("Summoned Imp", 7, SpawnShatterBuff(2, 0.8, new Vector3(!p ? 148 : -318, 122), "AbilitySummonImp", caster));
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilitySummonImp", true, !p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(!p ? 148 : -318, 122), "AbilitySummonImp", true, !p ? "1000" : "1001");
                PlaySound("AbilitySummonImpImpact");
                animationTime += frameTime * 2;
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.AddBuff("Summoned Imp", 7);
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Shadow Bolt", 0, new() { "Damage" }, new()
        {
            { "Shadow", 6 }
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
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                PlaySound("AbilityShadowboltCast");
                animationTime += frameTime * 6;
            });
            Board.board.actions.Add(() =>
            {
                target.health -= 8;
                SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityShadowbolt");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityShadowbolt", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityShadowbolt", true, p ? "1000" : "1001");
                PlaySound("AbilityShadowboltImpact");
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 8;
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Fireball", 0, new() { "Damage" }, new()
        {
            { "Fire", 6 }
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
                PlaySound("AbilityFireballCast");
                animationTime += frameTime * 4;
            });
            Board.board.actions.Add(() =>
            {
                target.health -= 8;
                SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityFireball");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityFireball", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityFireball", true, p ? "1000" : "1001");
                PlaySound("AbilityFireballImpact");
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 8;
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Scorch", 0, new() { "Damage", "Overtime" }, new()
        {
            { "Fire", 4 },
            { "Air", 2 }
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
                AddLine("Deals 10 damage times caster's intelligence.", Gray);
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
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                target.AddBuff("Scorch", 4, SpawnShatterBuff(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityScorch", target));
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityScorch", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityScorch", true, p ? "1000" : "1001");
                PlaySound("AbilityScorchImpact");
                animationTime += frameTime * 4;
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.AddBuff("Scorch", 4);
        }),
        new Ability("Healing Wave", 0, new() { "Healing" }, new()
        {
            { "Water", 6 },
            { "Air", 2 }
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
                AddLine("Deals 10 damage times caster's intelligence.", Gray);
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
                caster.health += 10;
                if (caster.health >= caster.MaxHealth())
                    caster.health = caster.MaxHealth();
                SpawnShatter(2, 0.8, new Vector3(p ? -318 : 148, 122), "AbilityHealingWave");
                SpawnShatter(6, 0.7, new Vector3(p ? -318 : 148, 122), "AbilityHealingWave", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? -318 : 148, 122), "AbilityHealingWave", true, p ? "1000" : "1001");
                PlaySound("AbilityHealingWaveImpact");
                animationTime += frameTime * 4;
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.health += 10;
            if (caster.health >= caster.MaxHealth())
                caster.health = caster.MaxHealth();
        }),
        new Ability("Muscle Tear", 1, new() { "Damage" }, new()
        {
            { "Air", 6 }
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
                PlaySound("AbilityMuscleTearCast");
                animationTime += frameTime * 6;
            });
            Board.board.actions.Add(() =>
            {
                target.health -= 8;
                SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityMuscleTear");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityMuscleTear", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityMuscleTear", true, p ? "1000" : "1001");
                PlaySound("AbilityMuscleTearImpact");
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 8;
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Frostbolt", 0, new() { "Damage" }, new()
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
                PlaySound("AbilityFrostboltCast");
                animationTime += frameTime * 6;
            });
            Board.board.actions.Add(() =>
            {
                target.health -= 8;
                SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityFrostbolt");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityFrostbolt", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityFrostbolt", true, p ? "1000" : "1001");
                PlaySound("AbilityFrostboltImpact");
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 8;
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Ice Lance", 0, new() { "Damage" }, new()
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
                PlaySound("AbilityIceLanceCast");
                animationTime += frameTime * 3;
            });
            Board.board.actions.Add(() =>
            {
                target.health -= 6;
                SpawnShatter(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityIceLance");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityIceLance", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityIceLance", true, p ? "1000" : "1001");
                PlaySound("AbilityIceLanceImpact");
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 6;
        }),
        new Ability("Freezing Nova", 1, new() { "Gathering" }, new()
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
                AddLine("element and 20% for a second bonus one.", Gray);
            });
        },
        (p) =>
        {
            Board.board.actions.Add(() =>
            {
                PlaySound("AbilityFreezingNovaCast");
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
                foreach (var e in list)
                {
                    Board.board.field[e.Item1, e.Item2] = 16;
                    SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[e.Item2].bigButtons[e.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[e.Item1, e.Item2]]);
                }
                PlaySound("AbilityFreezingNovaImpact");
            });
            Board.board.actions.Add(() =>
            {
                animationTime += frameTime * 15;
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var list = new List<(int, int)>();
            while (list.Count < 5)
            {
                var x = random.Next(0, board.field.GetLength(0));
                var y = random.Next(0, board.field.GetLength(1));
                if (board.field[x, y] != 16 && !list.Contains((x, y)))
                    list.Add((x, y));
            }
            foreach (var e in list)
                board.field[e.Item1, e.Item2] = 16;
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Blizzard", 4, new() { "Damage", "Overtime" }, new()
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
                AddLine("scaled with caster's Intelligence.", Gray);
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
            var caster = p ? Board.board.player : Board.board.enemy;
            Board.board.actions.Add(() =>
            {
                caster.AddBuff("Blizzard", 7, SpawnShatterBuff(2, 0.8, new Vector3(p ? 148 : -318, 122), "AbilityBlizzard", caster));
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityBlizzard", true, p ? "1000" : "1001");
                SpawnShatter(6, 0.7, new Vector3(p ? 148 : -318, 122), "AbilityBlizzard", true, p ? "1000" : "1001");
                PlaySound("AbilityBlizzardImpact");
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var caster = p ? board.player : board.enemy;
            caster.AddBuff("Blizzard", 7);
        }),
        new Ability("Deep Freeze", 5, new() { "Gathering" }, new()
        {
            { "Frost", 5 },
            { "Air", 5 },
            { "Earth", 10 },
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
                PlaySound("AbilityDeepFreezeCast");
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
                    SpawnShatter(4, 0.3, Board.board.window.LBRegionGroup.regions[e.Item2].bigButtons[e.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[e.Item1, e.Item2]]);
                }
                foreach (var e in list2)
                {
                    Board.board.field[e.Item1, e.Item2] = 16;
                    SpawnShatter(4, 1.0, Board.board.window.LBRegionGroup.regions[e.Item2].bigButtons[e.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[e.Item1, e.Item2]]);
                }
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var list1 = new List<(int, int)>();
            var list2 = new List<(int, int)>();
            for (int i = 0; i < board.field.GetLength(0); i++)
                for (int j = 0; j < board.field.GetLength(1); j++)
                    if (board.field[i, j] == 12)
                        list1.Add((i, j));
                    else if (board.field[i, j] == 13 || board.field[i, j] == 17)
                        list2.Add((i, j));
            foreach (var e in list1)
                board.field[e.Item1, e.Item2] = 14;
            foreach (var e in list2)
                board.field[e.Item1, e.Item2] = 16;
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Meteor", 5, new() { "Gathering" }, new()
        {
            { "Fire", 10 },
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
            var caster = p ? Board.board.player : Board.board.enemy;
            var target = p ? Board.board.enemy : Board.board.player;
            Board.board.actions.Add(() =>
            {
                PlaySound("AbilityMeteorCast");
                animationTime += frameTime * 10;
            });
            var list1 = new List<(int, int)>();
            for (int i = 1; i < Board.board.field.GetLength(0) - 1; i++)
                for (int j = 1; j < Board.board.field.GetLength(1) - 1; j++)
                    list1.Add((i, j));
            Board.board.actions.Add(() =>
            {
                foreach (var e in list1)
                {
                    Board.board.GiveResource(caster, e.Item1, e.Item2);
                    SpawnShatterElement(5, 0.8, Board.board.window.LBRegionGroup.regions[e.Item2].bigButtons[e.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[e.Item1, e.Item2]]);
                    Board.board.field[e.Item1, e.Item2] = 0;
                }
                PlaySound("AbilityMeteorImpact");
            });
            Board.board.actions.Add(() =>
            {
                animationTime += frameTime * 15;
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var list1 = new List<(int, int)>();
            var list2 = new List<(int, int)>();
            for (int i = 0; i < board.field.GetLength(0); i++)
                for (int j = 0; j < board.field.GetLength(1); j++)
                    if (board.field[i, j] == 12)
                        list1.Add((i, j));
                    else if (board.field[i, j] == 13 || board.field[i, j] == 17)
                        list2.Add((i, j));
            foreach (var e in list1)
                board.field[e.Item1, e.Item2] = 14;
            foreach (var e in list2)
                board.field[e.Item1, e.Item2] = 16;
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Shadowflame Breath", 3, new() { "Damage", "Gathering" }, new()
        {
            { "Air", 10 },
            { "Fire", 5 },
            { "Shadow", 5 },
        },
        () =>
        {
            AddPaddingRegion(() =>
            {
                AddLine("Casting Shadowflame Breath will breath", Gray);
                AddLine("fire at the board burning the elements.", Gray);
            });
            AddHeaderRegion(() =>
            {
                SetRegionAsGroupExtender();
                AddLine("Turns 20 random elements on the board into", Gray);
                AddLine("fire or shadow elements randomly chosen.", Gray);
            });
        },
        (p) =>
        {
            Board.board.actions.Add(() =>
            {
                PlaySound("AbilityShadowflameBreathCast");
            });
            var list = new List<(int, int)>();
            while (list.Count < 20)
            {
                var x = random.Next(0, Board.board.field.GetLength(0));
                var y = random.Next(0, Board.board.field.GetLength(1));
                if (Board.board.field[x, y] != 12 && Board.board.field[x, y] != 20 && !list.Contains((x, y)))
                    list.Add((x, y));
            }
            foreach (var e in list)
                Board.board.actions.Add(() =>
                {
                    Board.board.field[e.Item1, e.Item2] = random.Next(0, 2) == 0 ? 12 : 20;
                    SpawnShatter(2, 0.8, Board.board.window.LBRegionGroup.regions[e.Item2].bigButtons[e.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[Board.board.field[e.Item1, e.Item2]]);
                });
            Board.board.actions.Add(() =>
            {
                animationTime += frameTime * 15;
            });
            if (p) Board.board.playerFinishedMoving = true;
            else Board.board.enemyFinishedMoving = true;
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var list = new List<(int, int)>();
            while (list.Count < 20)
            {
                var x = random.Next(0, board.field.GetLength(0));
                var y = random.Next(0, board.field.GetLength(1));
                if (board.field[x, y] != 12 && board.field[x, y] != 20 && !list.Contains((x, y)))
                    list.Add((x, y));
            }
            foreach (var e in list)
                board.field[e.Item1, e.Item2] = random.Next(0, 2) == 0 ? 12 : 20;
            if (p) board.playerFinishedMoving = true;
            else board.enemyFinishedMoving = true;
        }),
        new Ability("Veil of Shadow", 0, new() { "Gathering" }, new()
        {
            { "Air", 2 },
            { "Shadow", 6 }
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
                PlaySound("AbilityIceLanceCast");
                animationTime += frameTime * 3;
            });
            Board.board.actions.Add(() =>
            {
                target.health -= 4;
                PlaySound("AbilityIceLanceImpact");
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy :board.player;
            target.health -= 4;
        }),
        new Ability("Bellowing Roar", 0, new() { "Gathering" }, new()
        {
            { "Fire", 4 },
            { "Shadow", 4 }
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
                PlaySound("AbilityIceLanceCast");
                animationTime += frameTime * 3;
            });
            Board.board.actions.Add(() =>
            {
                target.health -= 4;
                PlaySound("AbilityIceLanceImpact");
            });
            CDesktop.LockScreen();
        },
        (p, board) =>
        {
            var target = p ? board.enemy : board.player;
            target.health -= 4;
        })
    };
}
