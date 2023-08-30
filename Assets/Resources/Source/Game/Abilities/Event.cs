using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Buff;
using static Sound;
using static Shatter;
using static FlyingElement;
using static FlyingMissile;

public class Event
{
    public void ExecuteEffects(Board board, FutureBoard futureBoard, string icon, Dictionary<string, string> trigger)
    {
        var effector = board == null ? null : (board.playerTurn ? board.player : board.enemy);
        var other = board == null ? null : (board.playerTurn ? board.enemy : board.player);
        var futureEffector = futureBoard == null ? null : (futureBoard.playerTurn ? futureBoard.player : futureBoard.enemy);
        var futureOther = futureBoard == null ? null : (futureBoard.playerTurn ? futureBoard.enemy : futureBoard.player);
        foreach (var effect in effects)
        {
            string type = effect.ContainsKey("Effect") ? effect["Effect"] : "None";
            string affect = effect.ContainsKey("Affect") ? effect["Affect"] : "None";
            string buffName = effect.ContainsKey("BuffName") ? effect["BuffName"] : "None";
            int buffDuration = effect.ContainsKey("BuffDuration") ? int.Parse(effect["BuffDuration"]) : 1;
            string powerSource = effect.ContainsKey("PowerSource") ? effect["PowerSource"] : "Effector";
            string powerType = effect.ContainsKey("PowerType") ? effect["PowerType"] : "None";
            double powerScale = effect.ContainsKey("PowerScale") ? double.Parse(effect["PowerScale"].Replace(".", ",")) : 1;
            double chance = effect.ContainsKey("Chance") ? double.Parse(effect["Chance"].Replace(".", ",")) : 100;
            double chanceBase = effect.ContainsKey("ChanceBase") ? double.Parse(effect["ChanceBase"].Replace(".", ",")) : 0;
            string chanceSource = effect.ContainsKey("ChanceSource") ? effect["ChanceSource"] : powerSource;
            string chanceScale = effect.ContainsKey("ChanceScale") ? effect["ChanceScale"] : "None";
            string shatterTarget = effect.ContainsKey("ShatterTarget") ? effect["ShatterTarget"] : "None";
            string animationType = effect.ContainsKey("AnimationType") ? effect["AnimationType"] : "None";
            bool shatterDirectional = effect.ContainsKey("ShatterType") && effect["ShatterType"] == "Directional";
            int shatterDensity = effect.ContainsKey("ShatterDensity") ? int.Parse(effect["ShatterDensity"]) : 2;
            double shatterDegree = effect.ContainsKey("ShatterDegree") ? double.Parse(effect["ShatterDegree"].Replace(".", ",")) : 0.7;
            double shatterSpeed = effect.ContainsKey("ShatterSpeed") ? double.Parse(effect["ShatterSpeed"].Replace(".", ",")) : 4;
            float await = effect.ContainsKey("Await") ? float.Parse(effect["Await"]) : 0;
            var rand = random.Next(0, 100);
            if (rand >= chanceBase + chance * (chanceScale == "None" ? 1 : (chanceSource != "None" ? (futureBoard == null ? (chanceSource == "Effector" ? effector : other).Stats()[chanceScale] : (chanceSource == "Effector" ? futureEffector : futureOther).Stats()[chanceScale]) : 1))) continue;
            if (futureBoard != null)
            {
                if (type == "Damage")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var source = powerSource == "Effector" ? futureEffector : futureOther;
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        target.Damage(futureBoard, source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale, trigger["Trigger"] == "Damage");
                    }
                }
                else if (type == "Heal")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var source = powerSource == "Effector" ? futureEffector : futureOther;
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        target.Heal(futureBoard, source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale, trigger["Trigger"] == "Heal");
                    }
                }
                else if (type == "DetractResource")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                        if (resourceType == "None" && trigger.ContainsKey("ResourceDetracted"))
                            resourceType = trigger["ResourceType"];
                        int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 1;
                        if (resourceType != "None") target.DetractResource(futureBoard, resourceType, resourceAmount);
                    }
                }
                else if (type == "GiveResource")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                        if (resourceType == "None" && trigger.ContainsKey("ResourceCollected"))
                            resourceType = trigger["ResourceType"];
                        int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 1;
                        if (resourceType != "None") target.AddResource(futureBoard, resourceType, resourceAmount);
                    }
                }
                else if (type == "AddBuff")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        target.AddBuff(buffs.Find(x => x.name == buffName), buffDuration);
                        futureBoard.CallEvents(target, futureBoard, new() { { "Trigger", "BuffAdd" }, { "Triggerer", "Effector" }, { "BuffName", buffName } });
                        futureBoard.CallEvents(target == futureEffector ? futureOther : futureEffector, futureBoard, new() { { "Trigger", "BuffAdd" }, { "Triggerer", "Other" }, { "BuffName", buffName } });
                    }
                }
                else if (type == "RemoveBuff")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        target.RemoveBuff(target.buffs.Find(x => x.Item1.name == buffName));
                        futureBoard.CallEvents(target, futureBoard, new() { { "Trigger", "BuffRemove" }, { "Triggerer", "Effector" }, { "BuffName", buffName } });
                        futureBoard.CallEvents(target == futureEffector ? futureOther : futureEffector, futureBoard, new() { { "Trigger", "BuffRemove" }, { "Triggerer", "Other" }, { "BuffName", buffName } });
                    }
                }
                else if (type == "ResetBoard")
                {

                }
                else if (type == "EndTurn")
                {
                    if (futureBoard.playerTurn) futureBoard.playerFinishedMoving = true;
                    else futureBoard.enemyFinishedMoving = true;
                }
            }
            else if (board != null)
            {
                var triggerCopy = trigger.ToDictionary(x => x.Key, x => x.Value);
                if (animationType != "None" || animationType != "Disble" && affect == "Other")
                    board.actions.Add(() => { SpawnFlyingMissile(icon, (affect == "Effector" ? effector : other) != Board.board.player, 5, 2); });
                board.actions.Add(() =>
                {
                    if (type == "Damage")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var source = powerSource == "Effector" ? effector : other;
                            var target = affect == "Effector" ? effector : other;
                            target.Damage(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale, triggerCopy["Trigger"] == "Damage");
                        }
                    }
                    else if (type == "Heal")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var source = powerSource == "Effector" ? effector : other;
                            var target = affect == "Effector" ? effector : other;
                            target.Heal(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale, triggerCopy["Trigger"] == "Heal");
                        }
                    }
                    else if (type == "DetractResource")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var target = affect == "Effector" ? effector : other;
                            string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                            if (resourceType == "None" && triggerCopy.ContainsKey("ResourceDetracted"))
                                resourceType = triggerCopy["ResourceType"];
                            int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 1;
                            if (resourceType != "None") target.DetractResource(resourceType, resourceAmount);
                        }
                    }
                    else if (type == "GiveResource")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var target = affect == "Effector" ? effector : other;
                            string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                            string flyingResources = effect.ContainsKey("FlyingResources") ? effect["FlyingResources"] : "No";
                            if (resourceType == "None" && triggerCopy.ContainsKey("ResourceType"))
                                resourceType = triggerCopy["ResourceType"];
                            int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 1;
                            if (resourceType != "None") target.AddResource(resourceType, resourceAmount);
                            if (flyingResources == "Yes")
                                for (int i = 0; i < resourceAmount; i++)
                                    SpawnFlyingElement(new Vector3(affect == "Other" ? (board.playerTurn ? 166 : -302) : (board.playerTurn ? -302 : 166), 142), "Element" + resourceType + "Rousing", affect == "Other" ? !board.playerTurn : board.playerTurn);
                        }
                    }
                    else if (type == "AddBuff")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var target = affect == "Effector" ? effector : other;
                            target.AddBuff(buffs.Find(x => x.name == buffName), buffDuration, SpawnBuffObject(new Vector3(affect == "Other" ? (board.playerTurn ? 166 : -302) : (board.playerTurn ? -302 : 166), 142), icon, target));
                            board.CallEvents(target, new() { { "Trigger", "BuffAdd" }, { "Triggerer", "Effector" }, { "BuffName", buffName } });
                            board.CallEvents(target == effector ? other : effector, new() { { "Trigger", "BuffAdd" }, { "Triggerer", "Other" }, { "BuffName", buffName } });
                        }
                    }
                    else if (type == "RemoveBuff")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var target = affect == "Effector" ? effector : other;
                            target.RemoveBuff(target.buffs.Find(x => x.Item1 == buffs.Find(x => x.name == buffName)));
                            board.CallEvents(target, new() { { "Trigger", "BuffRemove" }, { "Triggerer", "Effector" }, { "BuffName", buffName } });
                            board.CallEvents(target == effector ? other : effector, new() { { "Trigger", "BuffRemove" }, { "Triggerer", "Other" }, { "BuffName", buffName } });
                        }
                    }
                    else if (type == "ResetBoard")
                    {

                    }
                    else if (type == "EndTurn")
                    {
                        if (board.playerTurn) board.playerFinishedMoving = true;
                        else board.enemyFinishedMoving = true;
                    }
                    if (shatterTarget != "None")
                        for (int i = 0; i < shatterDensity; i++)
                            SpawnShatter(shatterSpeed, shatterDegree, new Vector3(shatterTarget == "Other" ? (board.playerTurn ? 150 : -318) : (board.playerTurn ? -318 : 150), 122), icon, shatterDirectional ? shatterTarget == "Other" ? (board.playerTurn ? "1000" : "1001") : (board.playerTurn ? "1001" : "1000") : "0000");
                    if (effect.ContainsKey("SoundEffect"))
                        PlaySound(effect["SoundEffect"]);
                });
                if (await > 0) board.actions.Add(() => animationTime += frameTime * await);
                CDesktop.LockScreen();
            }
        }
    }

    public List<Dictionary<string, string>> triggers, effects;

    public static Event abilityEvent;
    public static int selectedEffect, selectedTrigger;
    public static List<string> possibleTriggers = new()
    {
        "BuffAdd",
        "BuffFlare",
        "BuffRemove",
        "ResourceCollected",
        "ResourceDetracted",
        "ResourceMaxed",
        "ResourceDeplated",
        "AbilityCast",
        "Cooldown",
        "Damage",
        "Heal",
        "HealthMaxed",
        "HealthDeplated",
        "CombatBegin",
        "TurnBegin",
        "TurnEnd"
    };
    public static List<string> possibleEffects = new()
    {
        "Damage",
        "Heal",
        "DestroyRows",
        "DestroyColumns",
        "DestroyRegion",
        "EndTurn",
        "ResetBoard",
        "GiveResource",
        "DetractResource",
        "AddBuff",
        "RemoveBuff"
    };
}
