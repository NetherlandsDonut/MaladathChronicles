using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Buff;
using static Sound;
using static Shatter;
using static FlyingBuff;
using static FlyingElement;
using static FlyingMissile;

public class Event
{
    public void ExecuteEffects(Board board, FutureBoard futureBoard, string icon, Dictionary<string, string> triggerBase)
    {
        //Define entities that take place in the event's effects
        var effector = board == null ? null : (board.playerTurn ? board.player : board.enemy);
        var other = board == null ? null : (board.playerTurn ? board.enemy : board.player);
        var futureEffector = futureBoard == null ? null : (futureBoard.playerTurn ? futureBoard.player : futureBoard.enemy);
        var futureOther = futureBoard == null ? null : (futureBoard.playerTurn ? futureBoard.enemy : futureBoard.player);
        
        //Main loop of executing effects, each effect is calculated here separately
        foreach (var effect in effects)
        {
            //Collect all effect data from object
            string type = effect.ContainsKey("Effect") ? effect["Effect"] : "None";
            string affect = effect.ContainsKey("Affect") ? effect["Affect"] : "None";
            string buffName = effect.ContainsKey("BuffName") ? effect["BuffName"] : "None";
            int buffDuration = effect.ContainsKey("BuffDuration") ? int.Parse(effect["BuffDuration"]) : 1;
            string powerSource = effect.ContainsKey("PowerSource") ? effect["PowerSource"] : "Effector";
            string powerType = effect.ContainsKey("PowerType") ? effect["PowerType"] : "None";
            double powerScale = effect.ContainsKey("PowerScale") ? double.Parse(effect["PowerScale"].Replace(".", ",")) : 1;
            double chance = effect.ContainsKey("Chance") ? double.Parse(effect["Chance"].Replace(".", ",")) : 0;
            double chanceBase = effect.ContainsKey("ChanceBase") ? double.Parse(effect["ChanceBase"].Replace(".", ",")) : 100;
            string chanceSource = effect.ContainsKey("ChanceSource") ? effect["ChanceSource"] : powerSource;
            string chanceScale = effect.ContainsKey("ChanceScale") ? effect["ChanceScale"] : "None";
            string animationType = effect.ContainsKey("AnimationType") ? effect["AnimationType"] : "None";
            string shatterType = effect.ContainsKey("ShatterType") ? effect["ShatterType"] : "None";
            float await = effect.ContainsKey("Await") ? float.Parse(effect["Await"]) : 0;

            //On a failed chance check of an effect program continues to the next one
            if (CheckFailChance()) continue;

            //Copy trigger base to make unique object for each effect
            var trigger = triggerBase.ToDictionary(x => x.Key, x => x.Value);

            //In case of future analysis just execute the effect instantly..
            if (futureBoard != null) ExecuteEffect();

            //..but in case of real time board add the effect to the queue
            else if (board != null)
            {
                ExecuteAnimation();
                board.actions.Add(() => ExecuteEffect());
                CDesktop.LockScreen();
            }


            bool CheckFailChance()
            {
                var randomRange = random.Next(0, 100);
                return randomRange >= chanceBase + chance * (chanceScale == "None" ? 1 : (chanceSource != "None" ? (futureBoard == null ? (chanceSource == "Effector" ? effector : other).Stats()[chanceScale] : (chanceSource == "Effector" ? futureEffector : futureOther).Stats()[chanceScale]) : 1));
            }

            //Executes effect's animation before the effect itself takes place
            void ExecuteAnimation()
            {
                if (animationType == "None") return;
                double animationArc = effect.ContainsKey("AnimationArc") ? double.Parse(effect["AnimationArc"].Replace(".", ",")) : 20.0;
                double animationSpeed = effect.ContainsKey("AnimationSpeed") ? double.Parse(effect["AnimationSpeed"].Replace(".", ",")) : 1;
                double trailStrength = effect.ContainsKey("TrailStrength") ? double.Parse(effect["TrailStrength"].Replace(".", ",")) : 1;
                board.actions.Add(() =>
                {
                    if (animationType == "Missile")
                        SpawnFlyingMissile(icon, (affect == "Effector" ? effector : other) != Board.board.player, animationArc, animationSpeed, trailStrength);
                });
            }

            //Executes a single effect from the list
            void ExecuteEffect()
            {
                //Execute a specific effect
                if (type == "Damage") EffectDamage();
                else if (type == "Heal") EffectHeal();
                else if (type == "DetractResource") EffectDetractResource();
                else if (type == "GiveResource") EffectGiveResource();
                else if (type == "AddBuff") EffectAddBuff();
                else if (type == "RemoveBuff") EffectRemoveBuff();
                else if (type == "EndTurn") EffectEndTurn();

                ExecuteShatter();
                ExecuteSoundEffect();
                ExecuteAwait();

                //Spawns a shatter effect if it was specified in the effect
                void ExecuteShatter()
                {
                    if (board == null || shatterType == "None") return;
                    string shatterTarget = effect.ContainsKey("ShatterTarget") ? effect["ShatterTarget"] : "None";
                    int shatterDensity = effect.ContainsKey("ShatterDensity") ? int.Parse(effect["ShatterDensity"]) : 2;
                    double shatterDegree = effect.ContainsKey("ShatterDegree") ? double.Parse(effect["ShatterDegree"].Replace(".", ",")) : 0.7;
                    double shatterSpeed = effect.ContainsKey("ShatterSpeed") ? double.Parse(effect["ShatterSpeed"].Replace(".", ",")) : 4;
                    if (shatterTarget == "None") return;
                    for (int i = 0; i < shatterDensity; i++)
                        SpawnShatter(shatterSpeed, shatterDegree, new Vector3(shatterTarget == "Other" ? (board.playerTurn ? 150 : -318) : (board.playerTurn ? -318 : 150), 122), icon, shatterType == "Directional" ? shatterTarget == "Other" ? (board.playerTurn ? "1011" : "1110") : (board.playerTurn ? "1011" : "1110") : "0000");
                }

                //Plays a sound effect if it was specified in the effect
                void ExecuteSoundEffect()
                {
                    if (board == null || !effect.ContainsKey("SoundEffect")) return;
                    PlaySound(effect["SoundEffect"]);
                }

                //Prolongs wait time after effect
                void ExecuteAwait()
                {
                    if (await != 0) return;
                    animationTime += frameTime * await;
                }
            }

            //This effect detracts a specific amount of a resource from the targetted entity
            void EffectDamage()
            {
                if (affect != "Effector" && affect != "Other") return;
                else if (futureBoard != null)
                {
                    var source = powerSource == "Effector" ? futureEffector : futureOther;
                    var target = affect == "Effector" ? futureEffector : futureOther;
                    target.Damage(futureBoard, source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale, trigger["Trigger"] == "Damage");
                }
                else if (board != null)
                {
                    var source = powerSource == "Effector" ? effector : other;
                    var target = affect == "Effector" ? effector : other;
                    target.Damage(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale, trigger["Trigger"] == "Damage");
                }
            }

            //This effect detracts a specific amount of a resource from the targetted entity
            void EffectHeal()
            {
                if (affect != "Effector" && affect != "Other") return;
                else if (futureBoard != null)
                {
                    var source = powerSource == "Effector" ? futureEffector : futureOther;
                    var target = affect == "Effector" ? futureEffector : futureOther;
                    target.Heal(futureBoard, source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale, trigger["Trigger"] == "Heal");
                }
                else if (board != null)
                {
                    var source = powerSource == "Effector" ? effector : other;
                    var target = affect == "Effector" ? effector : other;
                    target.Heal(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale, trigger["Trigger"] == "Heal");
                }
            }

            //This effect detracts a specific amount of a resource from the targetted entity
            void EffectDetractResource()
            {
                if (affect != "Effector" && affect != "Other") return;
                else if (futureBoard != null)
                {
                    var target = affect == "Effector" ? futureEffector : futureOther;
                    string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                    if (resourceType == "None" && trigger.ContainsKey("ResourceDetracted"))
                        resourceType = trigger["ResourceType"];
                    int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 1;
                    if (resourceType != "None") target.DetractResource(futureBoard, resourceType, resourceAmount);
                }
                else if (board != null)
                {
                    var target = affect == "Effector" ? effector : other;
                    string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                    if (resourceType == "None" && trigger.ContainsKey("ResourceDetracted"))
                        resourceType = trigger["ResourceType"];
                    int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 1;
                    if (resourceType != "None") target.DetractResource(resourceType, resourceAmount);
                }
            }

            //This effect gives a specific amount of a resource to the targetted entity
            void EffectGiveResource()
            {
                if (affect != "Effector" && affect != "Other") return;
                else if (futureBoard != null)
                {
                    var target = affect == "Effector" ? futureEffector : futureOther;
                    string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                    if (resourceType == "None" && trigger.ContainsKey("ResourceCollected"))
                        resourceType = trigger["ResourceType"];
                    int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 1;
                    if (resourceType != "None") target.AddResource(futureBoard, resourceType, resourceAmount);
                }
                else if (board != null)
                {
                    var target = affect == "Effector" ? effector : other;
                    string resourceType = effect.ContainsKey("ResourceType") ? effect["ResourceType"] : "None";
                    string flyingResources = effect.ContainsKey("FlyingResources") ? effect["FlyingResources"] : "No";
                    if (resourceType == "None" && trigger.ContainsKey("ResourceType"))
                        resourceType = trigger["ResourceType"];
                    int resourceAmount = effect.ContainsKey("ResourceAmount") ? int.Parse(effect["ResourceAmount"]) : 1;
                    if (resourceType != "None") target.AddResource(resourceType, resourceAmount);
                    if (flyingResources == "Yes")
                        for (int i = 0; i < resourceAmount; i++)
                            SpawnFlyingElement(new Vector3(affect == "Other" ? (board.playerTurn ? 166 : -302) : (board.playerTurn ? -302 : 166), 142), "Element" + resourceType + "Rousing", affect == "Other" ? !board.playerTurn : board.playerTurn);
                }
            }

            //This effect gives a buff to the targetted entity
            void EffectAddBuff()
            {
                if (affect != "Effector" && affect != "Other") return;
                else if (futureBoard != null)
                {
                    var target = affect == "Effector" ? futureEffector : futureOther;
                    target.AddBuff(buffs.Find(x => x.name == buffName), buffDuration);
                    futureBoard.CallEvents(target, futureBoard, new()
                    {
                        { "Trigger", "BuffAdd" },
                        { "Triggerer", "Effector" },
                        { "BuffName", buffName }
                    });
                    futureBoard.CallEvents(target == futureEffector ? futureOther : futureEffector, futureBoard, new()
                    {
                        { "Trigger", "BuffAdd" },
                        { "Triggerer", "Other" },
                        { "BuffName", buffName }
                    });
                }
                else if (board != null)
                {
                    var target = affect == "Effector" ? effector : other;
                    var pos = new Vector3(affect == "Other" ? (board.playerTurn ? 166 : -302) : (board.playerTurn ? -302 : 166), 142);
                    target.AddBuff(buffs.Find(x => x.name == buffName), buffDuration, SpawnBuffObject(pos, icon, target));
                    board.CallEvents(target, new()
                    {
                        { "Trigger", "BuffAdd" },
                        { "Triggerer", "Effector" },
                        { "BuffName", buffName }
                    });
                    board.CallEvents(target == effector ? other : effector, new()
                    {
                        { "Trigger", "BuffAdd" },
                        { "Triggerer", "Other" },
                        { "BuffName", buffName }
                    });
                }
            }

            //This effect removes a buff from the targetted entity
            void EffectRemoveBuff()
            {
                if (affect != "Effector" && affect != "Other") return;
                else if (futureBoard != null)
                {
                    var target = affect == "Effector" ? futureEffector : futureOther;
                    target.RemoveBuff(target.buffs.Find(x => x.Item1.name == buffName));
                    futureBoard.CallEvents(target, futureBoard, new()
                    {
                        { "Trigger", "BuffRemove" },
                        { "Triggerer", "Effector" },
                        { "BuffName", buffName }
                    });
                    futureBoard.CallEvents(target == futureEffector ? futureOther : futureEffector, futureBoard, new()
                    {
                        { "Trigger", "BuffRemove" },
                        { "Triggerer", "Other" },
                        { "BuffName", buffName }
                    });
                }
                else if (board != null)
                {
                    var target = affect == "Effector" ? effector : other;
                    target.RemoveBuff(target.buffs.Find(x => x.Item1 == buffs.Find(x => x.name == buffName)));
                    board.CallEvents(target, new()
                    {
                        { "Trigger", "BuffRemove" },
                        { "Triggerer", "Effector" },
                        { "BuffName", buffName }
                    });
                    board.CallEvents(target == effector ? other : effector, new()
                    {
                        { "Trigger", "BuffRemove" },
                        { "Triggerer", "Other" },
                        { "BuffName", buffName }
                    });
                }
            }

            //This effect ends the turn of the current entity
            void EffectEndTurn()
            {
                if (futureBoard != null)
                {
                    if (futureBoard.playerTurn) futureBoard.playerFinishedMoving = true;
                    else futureBoard.enemyFinishedMoving = true;
                }
                else if (board != null)
                {
                    if (board.playerTurn) board.playerFinishedMoving = true;
                    else board.enemyFinishedMoving = true;
                }
            }
        }
    }

    //List of triggers of this event
    //Those triggers will trigger this event's effects
    //Each trigger will make all of the effects flare up
    public List<Dictionary<string, string>> triggers;

    //List of effects of this event
    //Whenever a single trigger has met it's conditions
    //all of these will be called to function
    public List<Dictionary<string, string>> effects;

    //Stores information about what type of object
    //is the parent of the currently edited event
    public static string eventParentType;

    //Currently edited event in the dev panel
    public static Event eventEdit;

    //Currently selected effect for editing in dev panel
    public static int selectedEffect;

    //Currently selected trigger for editing in dev panel
    public static int selectedTrigger;

    //List of all possible triggers in the event system
    //Triggers determine at what point in time a certain event should happen
    public static List<string> possibleTriggers = new()
    {
        "None",
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

    //List of all possible effects in the event system
    //Effects are things that happen after a certain trigger
    //was triggered by player action or another effect
    public static List<string> possibleEffects = new()
    {
        "None",
        "Damage",
        "Heal",
        "EndTurn",
        "ResetBoard",
        "GiveResource",
        "DetractResource",
        "AddBuff",
        "RemoveBuff",
        "DestroyRows",
        "DestroyColumns",
        "DestroyRegion"
    };
}
