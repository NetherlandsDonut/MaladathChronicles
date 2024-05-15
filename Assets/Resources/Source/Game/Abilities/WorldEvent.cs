using System;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Sound;
using static MapGrid;
using static Defines;
using static SaveGame;

using UnityEngine;

public class WorldEvent
{
    //List of triggers of this world event
    //Those triggers will trigger this event's effects
    //Each trigger will make all of the effects flare up
    public List<Dictionary<string, string>> triggers;

    //List of effects of this world event
    //Whenever a single trigger has met it's conditions
    //all of these will be called to function
    public List<Dictionary<string, string>> effects;

    #region Execution

    public void ExecuteEffects(string icon, Dictionary<string, string> triggerBase, Item itemSource)
    {
        //Define entities that take place in the event's effects
        var effector = currentSave.player;

        //Main loop of executing effects, each effect is calculated here separately
        foreach (var effect in effects)
        {
            //Collect all effect data from object
            string type = effect.ContainsKey("Effect") ? effect["Effect"] : "None";
            string affect = effect.ContainsKey("Affect") ? effect["Affect"] : "None";
            string worldBuffName = effect.ContainsKey("WorldBuffName") ? effect["WorldBuffName"] : "None";
            int worldBuffDuration = effect.ContainsKey("WorldBuffDuration") ? int.Parse(effect["WorldBuffDuration"]) : 1;
            string powerSource = effect.ContainsKey("PowerSource") ? effect["PowerSource"] : "Effector";
            string powerType = effect.ContainsKey("PowerType") ? effect["PowerType"] : "None";
            double powerScale = effect.ContainsKey("PowerScale") ? double.Parse(effect["PowerScale"].Replace(".", ",")) : 1;
            double chance = effect.ContainsKey("Chance") ? double.Parse(effect["Chance"].Replace(".", ",")) : 0;
            double chanceBase = effect.ContainsKey("ChanceBase") ? double.Parse(effect["ChanceBase"].Replace(".", ",")) : 100;
            string chanceSource = effect.ContainsKey("ChanceSource") ? effect["ChanceSource"] : powerSource;
            string chanceScale = effect.ContainsKey("ChanceScale") ? effect["ChanceScale"] : "None";
            string shatterType = effect.ContainsKey("ShatterType") ? effect["ShatterType"] : "None";
            float await = effect.ContainsKey("Await") ? float.Parse(effect["Await"]) : 0;

            //On a failed chance check of an effect program continues to the next effect
            //if (CheckFailChance()) continue;

            //Copy trigger base to make unique object for each effect
            var trigger = triggerBase.ToDictionary(x => x.Key, x => x.Value);

            //..but in case of real time board add the effect to the queue
            //board.actions.Add(() => ExecuteEffect());
            //board.actions.Add(() => ExecuteAwait());
            ExecuteEffect();
            //ExecuteAwait();
            //CDesktop.LockScreen();

            //Checks whether the effects failed to present themselves
            //bool CheckFailChance()
            //{
            //    var randomRange = random.Next(0, 100);
            //    return randomRange >= chanceBase + chance * (chanceScale == "None" ? 1 : (chanceSource != "None" ? (futureBoard == null ? (chanceSource == "Effector" ? effector : other).Stats()[chanceScale] : (chanceSource == "Effector" ? futureEffector : futureOther).Stats()[chanceScale]) : 1));
            //}

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
                else if (type == "ConsumeItem") EffectConsumeItem();
                //else if (type == "StatMod") EffectStatMod();
                //else if (type == "ItemMod") EffectItemMod();
                else if (type == "AddWorldBuff") EffectAddWorldBuff();
                else if (type == "RemoveWorldBuff") EffectRemoveWorldBuff();
                else if (type == "TeleportPlayer") EffectTeleportPlayer();
                else if (type == "HearthstonePlayer") EffectHearthstonePlayer();
                //else if (type == "Alcohol") EffectAlcohol();
                //else if (type == "Container") EffectContainer();

                ExecuteShatter();
                ExecuteSoundEffect();

                //Spawns a shatter effect if it was specified in the effect
                void ExecuteShatter()
                {
                    //if (shatterType == "None") return;
                    //string shatterTarget = effect.ContainsKey("ShatterTarget") ? effect["ShatterTarget"] : "None";
                    //int shatterDensity = effect.ContainsKey("ShatterDensity") ? int.Parse(effect["ShatterDensity"]) : 1;
                    //double shatterDegree = effect.ContainsKey("ShatterDegree") ? double.Parse(effect["ShatterDegree"].Replace(".", ",")) : 20;
                    //double shatterSpeed = effect.ContainsKey("ShatterSpeed") ? double.Parse(effect["ShatterSpeed"].Replace(".", ",")) : 6;
                    //if (shatterTarget == "None") return;
                    //for (int i = 0; i < shatterDensity; i++)
                    //    SpawnShatter(shatterSpeed, shatterDegree, new Vector3(shatterTarget == "Other" ? (board.playerTurn ? 148 : -318) : (board.playerTurn ? -318 : 148), 124), icon, shatterType == "Directional" ? shatterTarget == "Other" ? (board.playerTurn ? "1011" : "1110") : (board.playerTurn ? "1011" : "1110") : "0000");
                }

                //Plays a sound effect if it was specified in the effect
                void ExecuteSoundEffect()
                {
                    if (!effect.ContainsKey("SoundEffect")) return;
                    PlaySound(effect["SoundEffect"]);
                }
            }

            //This effect detracts a specific amount of a resource from the targetted entity
            void EffectDamage()
            {
                var source = effector;
                var target = effector;
                var amount = (int)Math.Round(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale);
                target.Damage(amount, trigger["Trigger"] == "Damage");
                //AddBigButtonOverlay(new Vector2(target == board.player ? -300 : 167, 141), "OtherDamaged", 0.1f, 5);
                //SpawnFallingText(new Vector2(target == board.player ? -300 : 167, 141), "" + amount, "White");
                //board.UpdateHealthBars();
            }

            //This effect detracts a specific amount of a resource from the targetted entity
            void EffectHeal()
            {
                var source = effector;
                var target = effector;
                var amount = (int)Math.Round(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale);
                target.Heal(amount, trigger["Trigger"] == "Heal");
                //AddBigButtonOverlay(new Vector2(target == board.player ? -300 : 167, 141), "OtherHealed", 0.1f, 5);
                //SpawnFallingText(new Vector2(target == board.player ? -300 : 167, 141), "" + amount, "Uncommon");
                //board.UpdateHealthBars();
            }

            //This effect consumes one stack of the item
            void EffectConsumeItem()
            {
                var target = effector;
                var item = target.inventory.items.Find(x => x == itemSource);
                item.amount--;
                if (item.amount == 0) target.inventory.items.Remove(item);
            }

            //This effect gives a buff to the targetted entity
            void EffectAddWorldBuff()
            {
                var target = effector;
                //var pos = new Vector3(affect == "Other" ? (board.playerTurn ? 166 : -302) : (board.playerTurn ? -302 : 166), 142);
                //target.AddBuff(buffs.Find(x => x.name == worldBuffName), worldBuffDuration, SpawnBuffObject(pos, icon, target), sourceRank);
                //board.CallEvents(target, new()
                //{
                //    { "Trigger", "BuffAdd" },
                //    { "Triggerer", "Effector" },
                //    { "BuffName", worldBuffName }
                //});
                //board.CallEvents(target == effector ? other : effector, new()
                //{
                //    { "Trigger", "BuffAdd" },
                //    { "Triggerer", "Other" },
                //    { "BuffName", worldBuffName }
                //});
                //SpawnFallingText(new Vector2(target == board.player ? -300 : 167, 141), worldBuffDuration + " turn" + (worldBuffDuration > 1 ? "s" : ""), "White");
            }

            //This effect removes a buff from the targetted entity
            void EffectRemoveWorldBuff()
            {
                var target = effector;
                //target.RemoveBuff(target.buffs.Find(x => x.Item1 == buffs.Find(x => x.name == worldBuffName)));
                //board.CallEvents(target, new()
                //{
                //    { "Trigger", "BuffRemove" },
                //    { "Triggerer", "Effector" },
                //    { "BuffName", worldBuffName }
                //});
                //board.CallEvents(target == effector ? other : effector, new()
                //{
                //    { "Trigger", "BuffRemove" },
                //    { "Triggerer", "Other" },
                //    { "BuffName", worldBuffName }
                //});
            }

            //This effect consumes one stack of the item
            void EffectTeleportPlayer()
            {
                //CloseDesktop("EquipmentScreen");
                //currentSave.currentSite = currentSave.player.homeLocation;
            }

            //This effect consumes one stack of the item
            void EffectHearthstonePlayer()
            {
                CloseDesktop("EquipmentScreen");
                SwitchDesktop("Map");
                var prevSite = currentSave.currentSite;
                currentSave.currentSite = currentSave.player.homeLocation;
                SiteTown.town = (SiteTown)Site.FindSite(x => x.name == currentSave.currentSite);
                Respawn("Site: " + prevSite);
                Respawn("Site: " + currentSave.currentSite);
                CDesktop.cameraDestination = new Vector2(SiteTown.town.x, SiteTown.town.y);
            }
        }
    }

    #endregion

    //Stores information about what type of object
    //is the parent of the currently edited world event
    public static string worldEventParentType;

    //Currently edited world event in the dev panel
    public static WorldEvent worldEventEdit;

    //Currently selected world effect for editing in dev panel
    public static int selectedWorldEffect;

    //Currently selected world trigger for editing in dev panel
    public static int selectedWorldTrigger;

    //List of all possible triggers in the event system
    //Triggers determine at what point in time a certain event should happen
    public static List<string> possibleWorldTriggers = new()
    {
        "None",
        "WorldBuffAdd",
        "WorldBuffRemove",
        "ItemUsed"
    };

    //List of all possible effects in the event system
    //Effects are things that happen after a certain trigger
    //was triggered by player action or another effect
    public static List<string> possibleWorldEffects = new()
    {
        "None",
        "Damage",
        "Heal",
        "Alcohol",
        "ItemMod",
        "Hearthstone",
        "Container",
        "StatMod",
        "AddWorldBuff",
        "RemoveWorldBuff",
        "TeleportPlayer"
    };
}
