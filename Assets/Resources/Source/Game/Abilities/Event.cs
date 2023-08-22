using UnityEngine;
using System.Collections.Generic;

using static Root;

public class Event
{
    public void ExecuteEffects(Board board, FutureBoard futureBoard, string icon)
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
            string shatterTarget = effect.ContainsKey("ShatterTarget") ? effect["ShatterTarget"] : "None";
            bool shatterDirectional = effect.ContainsKey("ShatterType") && effect["ShatterType"] == "Directional";
            int shatterDensity = effect.ContainsKey("ShatterDensity") ? int.Parse(effect["ShatterDensity"]) : 0;
            double shatterDegree = effect.ContainsKey("ShatterDegree") ? double.Parse(effect["ShatterDegree"].Replace(".", ",")) : 0.5;
            double shatterSpeed = effect.ContainsKey("shatterSpeed") ? double.Parse(effect["shatterSpeed"].Replace(".", ",")) : 2;
            float await = effect.ContainsKey("Await") ? float.Parse(effect["Await"]) : 0;
            if (futureBoard != null)
            {
                if (type == "Damage")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var source = powerSource == "Effector" ? futureEffector : futureOther;
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        target.Damage(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale);
                    }
                }
                else if (type == "Heal")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var source = powerSource == "Effector" ? futureEffector : futureOther;
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        target.Heal(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale);
                    }
                }
                else if (type == "DetractResource")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                        int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 0;
                        if (resourceType != "None")
                            target.DetractResource(resourceType, resourceAmount);
                    }
                }
                else if (type == "GiveResource")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                        int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 0;
                        if (resourceType != "None")
                            target.AddResource(resourceType, resourceAmount);
                    }
                }
                else if (type == "AddBuff")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        target.AddBuff(Buff.buffs.Find(x => x.name == buffName), buffDuration);
                    }
                }
                else if (type == "RemoveBuff")
                {
                    if (affect == "Effector" || affect == "Other")
                    {
                        var target = affect == "Effector" ? futureEffector : futureOther;
                        target.RemoveBuff(target.buffs.Find(x => x.Item1.name == buffName));
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
                board.actions.Add(() =>
                {
                    if (type == "Damage")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var source = powerSource == "Effector" ? effector : other;
                            var target = affect == "Effector" ? effector : other;
                            target.Damage(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale);
                        }
                    }
                    else if (type == "Heal")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var source = powerSource == "Effector" ? effector : other;
                            var target = affect == "Effector" ? effector : other;
                            target.Heal(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale);
                        }
                    }
                    else if (type == "DetractResource")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var target = affect == "Effector" ? effector : other;
                            string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                            int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 0;
                            if (resourceType != "None")
                                target.DetractResource(resourceType, resourceAmount);
                        }
                    }
                    else if (type == "GiveResource")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var target = affect == "Effector" ? effector : other;
                            string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                            int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 0;
                            if (resourceType != "None")
                                target.AddResource(resourceType, resourceAmount);
                        }
                    }
                    else if (type == "AddBuff")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var target = affect == "Effector" ? effector : other;
                            target.AddBuff(Buff.buffs.Find(x => x.name == buffName), buffDuration, SpawnBuff(new Vector3(affect == "Effector" ? (board.playerTurn ? 148 : -318) : (board.playerTurn ? -318 : 148), 122), icon, target));
                        }
                    }
                    else if (type == "RemoveBuff")
                    {
                        if (affect == "Effector" || affect == "Other")
                        {
                            var target = affect == "Effector" ? effector : other;
                            target.RemoveBuff(target.buffs.Find(x => x.Item1 == Buff.buffs.Find(x => x.name == buffName)));
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
                            SpawnShatter(shatterSpeed, shatterDegree, new Vector3(shatterTarget == "Other" ? (board.playerTurn ? 148 : -318) : (board.playerTurn ? -318 : 148), 122), icon, shatterDirectional, shatterTarget == "Other" ? (board.playerTurn ? "1000" : "1001") : (board.playerTurn ? "1001" : "1000"));
                    if (effect.ContainsKey("SoundEffect"))
                        PlaySound(effect["SoundEffect"]);
                });
                if (await > 0) board.actions.Add(() => animationTime += frameTime * await);
                CDesktop.LockScreen();
            }
        }
    }

    public List<Dictionary<string, string>> triggers, effects;
}
