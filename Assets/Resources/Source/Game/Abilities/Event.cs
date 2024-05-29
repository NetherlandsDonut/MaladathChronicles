using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Buff;
using static Sound;
using static Defines;
using static Shatter;
using static FlyingBuff;
using static FlyingElement;
using static FlyingMissile;

public class Event
{
    //List of triggers of this event
    //Those triggers will trigger this event's effects
    //Each trigger will make all of the effects flare up
    public List<Dictionary<string, string>> triggers;

    //List of effects of this event
    //Whenever a single trigger has met it's conditions
    //all of these will be called to function
    public List<Dictionary<string, string>> effects;

    #region Execution

    public void ExecuteEffects(Board board, FutureBoard futureBoard, string icon, Dictionary<string, string> triggerBase, Dictionary<string, string> variables, string sourceName, int sourceRank)
    {
        //Define entities that take place in the event's effects
        var effector = board == null ? null : (board.playerTurn ? board.player : board.enemy);
        var other = board == null ? null : (board.playerTurn ? board.enemy : board.player);
        var futureEffector = futureBoard == null ? null : (futureBoard.playerTurn ? futureBoard.player : futureBoard.enemy);
        var futureOther = futureBoard == null ? null : (futureBoard.playerTurn ? futureBoard.enemy : futureBoard.player);

        //If we are in realtime combat then print name of the ability of which effects are being executed
        //board?.actions.Add(() => SpawnFallingText(new Vector2(0, 14), sourceName, effector == board.player ? "White" : "Red"));

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
            double powerScale = effect.ContainsKey("PowerScale") ? double.Parse(effect["PowerScale"].StartsWith("#") ? variables[effect["PowerScale"]] : effect["PowerScale"].Replace(".", ",")) : 1;
            double chance = effect.ContainsKey("Chance") ? double.Parse(effect["Chance"].Replace(".", ",")) : 0;
            double chanceBase = effect.ContainsKey("ChanceBase") ? double.Parse(effect["ChanceBase"].Replace(".", ",")) : 100;
            string chanceSource = effect.ContainsKey("ChanceSource") ? effect["ChanceSource"] : powerSource;
            string chanceScale = effect.ContainsKey("ChanceScale") ? effect["ChanceScale"] : "None";
            string animationType = effect.ContainsKey("AnimationType") ? effect["AnimationType"] : "None";
            string shatterType = effect.ContainsKey("ShatterType") ? effect["ShatterType"] : "None";
            float await = effect.ContainsKey("Await") ? float.Parse(effect["Await"]) : 0;

            //On a failed chance check of an effect program continues to the next effect
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
                board.actions.Add(() => ExecuteAwait());
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
                double animationSpeed = effect.ContainsKey("AnimationSpeed") ? double.Parse(effect["AnimationSpeed"].Replace(".", ",")) : 1.5;
                double trailStrength = effect.ContainsKey("TrailStrength") ? double.Parse(effect["TrailStrength"].Replace(".", ",")) : 5;
                board.actions.Add(() =>
                {
                    if (animationType == "Missile")
                        SpawnFlyingMissile(icon, (affect == "Effector" ? effector : other) != Board.board.player, animationArc, animationSpeed, trailStrength);
                });
            }

            //Prolongs wait time after effect
            void ExecuteAwait()
            {
                animationTime = defines.frameTime * await;
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
                else if (type == "ChangeElements") EffectChangeElements();

                ExecuteShatter();
                ExecuteSoundEffect();

                //Spawns a shatter effect if it was specified in the effect
                void ExecuteShatter()
                {
                    if (board == null || shatterType == "None") return;
                    string shatterTarget = effect.ContainsKey("ShatterTarget") ? effect["ShatterTarget"] : "None";
                    int shatterDensity = effect.ContainsKey("ShatterDensity") ? int.Parse(effect["ShatterDensity"]) : 1;
                    double shatterDegree = effect.ContainsKey("ShatterDegree") ? double.Parse(effect["ShatterDegree"].Replace(".", ",")) : 20;
                    double shatterSpeed = effect.ContainsKey("ShatterSpeed") ? double.Parse(effect["ShatterSpeed"].Replace(".", ",")) : 6;
                    if (shatterTarget == "None") return;
                    for (int i = 0; i < shatterDensity; i++)
                        SpawnShatter(shatterSpeed, shatterDegree, new Vector3(shatterTarget == "Other" ? (board.playerTurn ? 148 : -318) : (board.playerTurn ? -318 : 148), 124), icon, shatterType == "Directional" ? shatterTarget == "Other" ? (board.playerTurn ? "1011" : "1110") : (board.playerTurn ? "1011" : "1110") : "0000");
                }

                //Plays a sound effect if it was specified in the effect
                void ExecuteSoundEffect()
                {
                    if (board == null || !effect.ContainsKey("SoundEffect")) return;
                    PlaySound(effect["SoundEffect"]);
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
                    var amount = (int)Math.Round(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale);
                    target.Damage(amount, trigger["Trigger"] == "Damage");
                    AddBigButtonOverlay(new Vector2(target == board.player ? -300 : 167, 141), "OtherDamaged", 1f, -1);
                    SpawnFallingText(new Vector2(target == board.player ? -300 : 167, 141), "" + amount, "White");
                    if (target == board.player) board.log.damageTaken.Inc(sourceName, amount);
                    else board.log.damageDealt.Inc(sourceName, amount);
                    board.UpdateHealthBars();
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
                    var amount = (int)Math.Round(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale);
                    target.Heal(amount, trigger["Trigger"] == "Heal");
                    AddBigButtonOverlay(new Vector2(target == board.player ? -300 : 167, 141), "OtherHealed", 1f, 5);
                    SpawnFallingText(new Vector2(target == board.player ? -300 : 167, 141), "" + amount, "Uncommon");
                    if (target == board.player) board.log.healingReceived.Inc(sourceName, amount);
                    board.UpdateHealthBars();
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
                    target.AddBuff(buffs.Find(x => x.name == buffName), buffDuration, sourceRank);
                    futureBoard.CallEvents(target, new()
                    {
                        { "Trigger", "BuffAdd" },
                        { "Triggerer", "Effector" },
                        { "BuffName", buffName }
                    });
                    futureBoard.CallEvents(target == futureEffector ? futureOther : futureEffector, new()
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
                    target.AddBuff(buffs.Find(x => x.name == buffName), buffDuration, SpawnBuffObject(pos, icon, target), sourceRank);
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
                    SpawnFallingText(new Vector2(target == board.player ? -300 : 167, 141), buffDuration + " turn" + (buffDuration > 1 ? "s" : ""), "White");
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
                    futureBoard.CallEvents(target, new()
                    {
                        { "Trigger", "BuffRemove" },
                        { "Triggerer", "Effector" },
                        { "BuffName", buffName }
                    });
                    futureBoard.CallEvents(target == futureEffector ? futureOther : futureEffector, new()
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

            //This effect gives a specific amount of a resource to the targetted entity
            void EffectChangeElements()
            {
                if (futureBoard != null)
                {
                    int amount = effect.ContainsKey("ChangeAmount") ? int.Parse(effect["ChangeAmount"]) : 1;
                    string from = effect.ContainsKey("ElementFrom") ? effect["ElementFrom"] : "Random";
                    string to = effect.ContainsKey("ElementTo") ? effect["ElementTo"] : "Random";
                    var list = new List<(int, int)>();
                    var possible = new List<(int, int)>();
                    for (int i = 0; i < futureBoard.field.GetLength(0); i++)
                        for (int j = 0; j < futureBoard.field.GetLength(1); j++)
                            if (from == "Random" || from != "Random" && Board.boardNameDictionary[futureBoard.field[i, j]].Contains(from))
                                possible.Add((i, j));
                    for (int i = 0; i < amount && possible.Count > 0; i++)
                    {
                        list.Add(possible[random.Next(possible.Count)]);
                        possible.Remove(list.Last());
                    }
                    foreach (var element in list)
                        futureBoard.field[element.Item1, element.Item2] = (to == "Random" ? random.Next(0, 10) : board.ResourceReverse(to)) + futureBoard.field[element.Item1, element.Item2] / 10 * 10;
                }
                else if (board != null)
                {
                    int amount = effect.ContainsKey("ChangeAmount") ? int.Parse(effect["ChangeAmount"]) : 1;
                    string from = effect.ContainsKey("ElementFrom") ? effect["ElementFrom"] : "Random";
                    string to = effect.ContainsKey("ElementTo") ? effect["ElementTo"] : "Random";
                    var list = new List<(int, int)>();
                    var possible = new List<(int, int)>();
                    for (int i = 0; i < board.field.GetLength(0); i++)
                        for (int j = 0; j < board.field.GetLength(1); j++)
                            if (from == "Random" || Board.boardNameDictionary[board.field[i, j]].Contains(from))
                                if (to == "Random" || !Board.boardNameDictionary[board.field[i, j]].Contains(to))
                                    possible.Add((i, j));
                    for (int i = 0; i < amount && possible.Count > 0; i++)
                    {
                        list.Add(possible[random.Next(possible.Count)]);
                        possible.Remove(list.Last());
                    }
                    int shatterDensity = effect.ContainsKey("ElementShatterDensity") ? int.Parse(effect["ElementShatterDensity"]) : 1;
                    double shatterDegree = effect.ContainsKey("ElementShatterDegree") ? double.Parse(effect["ElementShatterDegree"].Replace(".", ",")) : 8;
                    double shatterSpeed = effect.ContainsKey("ElementShatterSpeed") ? double.Parse(effect["ElementShatterSpeed"].Replace(".", ",")) : 5;
                    foreach (var e in list)
                    {
                        var newValue = 0;
                        do newValue = (to == "Random" ? random.Next(0, 10) : board.ResourceReverse(to)) + board.field[e.Item1, e.Item2] / 10 * 10;
                        while (newValue == board.field[e.Item1, e.Item2]);
                        board.field[e.Item1, e.Item2] = newValue;
                        for (int i = 0; i < shatterDensity; i++)
                            SpawnShatter(shatterSpeed, shatterDegree, board.window.LBRegionGroup.regions[e.Item2].bigButtons[e.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[board.field[e.Item1, e.Item2]]);
                    }
                }
            }
        }
    }

    #endregion

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
        "DestroyRegion",
        "ChangeElements"
    };
}
