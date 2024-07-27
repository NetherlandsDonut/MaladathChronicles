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

   //List of conditions that make the effects trigger
   public List<Condition> conditions;

   #region Execution

   public void ExecuteEffects(SaveGame save, Item itemUsed, Dictionary<string, string> triggerBase, Dictionary<string, string> variables, Ability abilityUsed, int abilityRank)
   {
      //Define entities that take place in the event's effects
      var effector = save.player;

      //Main loop of executing effects, each effect is calculated here separately
      foreach (var effect in effects)
      {
         //Collect all effect data from object
         string type = effect.ContainsKey("Effect") ? effect["Effect"] : "None";
         string affect = effect.ContainsKey("Affect") ? effect["Affect"] : "None";
         string worldBuffName = effect.ContainsKey("WorldBuffName") ? effect["WorldBuffName"] : "None";
         int worldBuffDuration = effect.ContainsKey("WorldBuffDuration") ? int.Parse(effect["WorldBuffDuration"]) : 1;
         int worldBuffRank = effect.ContainsKey("WorldBuffRank") ? int.Parse(effect["WorldBuffRank"]) : 0;
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

         //Execute the effect
         ExecuteEffect();

         bool CheckFailChance()
         {
            var randomRange = random.Next(0, 100);
            return randomRange >= chanceBase + chance * (chanceScale == "None" ? 1 : (chanceSource != "None" ? effector.Stats()[chanceScale] : 1));
         }

         //Executes a single effect from the list
         void ExecuteEffect()
         {
            //Execute a specific effect
            if (type == "ConsumeItem") EffectConsumeItem();
            else if (type == "AddWorldBuff") EffectAddWorldBuff();
            else if (type == "RemoveWorldBuff") EffectRemoveWorldBuff();
            else if (type == "TeleportPlayer") EffectTeleportPlayer();
            else if (type == "HearthstonePlayer") EffectHearthstonePlayer();
            else if (type == "Combat") EffectCombat();
            else if (type == "Loot") EffectLoot();

            ExecuteSoundEffect();

            //Plays a sound effect if it was specified in the effect
            void ExecuteSoundEffect()
            {
               if (!effect.ContainsKey("SoundEffect")) return;
               var volume = effect.ContainsKey("SoundEffectVolume") ? float.Parse(effect["SoundEffectVolume"]) : 0.7f;
               PlaySound(effect["SoundEffect"], volume);
            }
         }

         //This effect consumes one stack of the item
         void EffectConsumeItem()
         {
            var target = effector;
            itemUsed.amount--;
            if (itemUsed.amount == 0)
               target.inventory.items.Remove(itemUsed);
         }

         //This effect starts a combat with a specific enemy
         void EffectCombat()
         {
            var target = effector;
            string enemy = effect.ContainsKey("CombatEnemy") ? effect["CombatEnemy"] : "None";
            int level = effect.ContainsKey("CombatEnemyLevel") ? int.Parse(effect["CombatEnemyLevel"]) : 1;
            int killCap = effect.ContainsKey("CombatKillCap") ? int.Parse(effect["CombatKillCap"]) : 999;
            var race = Race.races.Find(x => x.name == enemy);
            if (race != null)
            {
               var can = false;
               if (race.kind == "Common" && (save.commonsKilled.ContainsKey(race.name) ? save.commonsKilled[race.name] : 0) < killCap) can = true;
               else if (race.kind == "Rare" && (save.raresKilled.ContainsKey(race.name) ? save.raresKilled[race.name] : 0) < killCap) can = true;
               else if (race.kind == "Elite" && (save.elitesKilled.ContainsKey(race.name) ? save.elitesKilled[race.name] : 0) < killCap) can = true;
               if (can)
               {
                  Board.NewBoard(new Entity(level, race), Site.FindSite(x => x.name == save.currentSite));
                  SpawnDesktopBlueprint("Game");
                  CloseDesktop("EquipmentScreen");
               }
            }
         }

         //This effect generates loot and opens it
         void EffectLoot()
         {
            var target = effector;
            if (itemUsed.itemsInside == null)
            {
               itemUsed.itemsInside = new();
               string generalDrop = effect.ContainsKey("GeneralDrop") ? effect["GeneralDrop"] : "None";
               int minMoney = effect.ContainsKey("MinMoney") ? int.Parse(effect["MinMoney"]) : 0;
               int maxMoney = effect.ContainsKey("MaxMoney") ? int.Parse(effect["MaxMoney"]) : 0;
               if (maxMoney > 0 && minMoney >= 0)
               {
                  var amount = random.Next(minMoney, maxMoney + 1);
                  if (amount / 10000 > 0) itemUsed.itemsInside.Add(Item.items.Find(x => x.name == "Gold").CopyItem(amount));
                  else if (amount / 100 > 0) itemUsed.itemsInside.Add(Item.items.Find(x => x.name == "Silver").CopyItem(amount));
                  else itemUsed.itemsInside.Add(Item.items.Find(x => x.name == "Copper").CopyItem(amount));
               }
               var drops = GeneralDrop.generalDrops.FindAll(x => x.category == generalDrop);
               if (drops != null && drops.Count > 0)
                  foreach (var drop in drops)
                     if (Roll(drop.rarity))
                     {
                        int amount = 1;
                        for (int i = 1; i < drop.dropCount; i++) amount += Roll(50) ? 1 : 0;
                        itemUsed.itemsInside.Add(Item.items.Find(x => x.name == drop.item).CopyItem(amount));
                     }
            }
            Item.item = itemUsed;
            SpawnDesktopBlueprint("ContainerLoot");
         }

         //This effect gives a buff to the targetted entity
         void EffectAddWorldBuff()
         {
            var target = effector;
            var buff = buffs.Find(x => x.name == worldBuffName);
            if (buff == null) Debug.Log("ERROR 017: World buff named \"" + worldBuffName + "\" was not found");
            else
            {
               target.AddWorldBuff(buff, worldBuffDuration, worldBuffRank);
               save.CallEvents(new()
               {
                  { "Trigger", "BuffAdd" },
                  { "WorldBuffName", worldBuffName }
               });
               SpawnFallingText(new Vector2(0, 34), worldBuffDuration + " minute" + (worldBuffDuration > 1 ? "s" : ""), "White");
            }
         }

         //This effect removes a buff from the targetted entity
         void EffectRemoveWorldBuff()
         {
            var target = effector;
            target.RemoveWorldBuff(target.worldBuffs.Find(x => x.buff.name == worldBuffName));
            save.CallEvents(new()
            {
               { "Trigger", "BuffRemove" },
               { "WorldBuffName", worldBuffName }
            });
         }

         //This effect consumes one stack of the item
         void EffectTeleportPlayer()
         {
            string destination = effect.ContainsKey("TeleportDestinaton") ? effect["TeleportDestinaton"] : "None";
            if (destination == "None") return;
            CloseDesktop("EquipmentScreen");
            SwitchDesktop("Map");
            var prevSite = save.currentSite;
            save.currentSite = destination;
            if (!save.siteVisits.ContainsKey(destination))
            {
               save.siteVisits.Add(destination, 0);
               PlaySound("DesktopZoneDiscovered", 0.5f);
               save.player.ReceiveExperience(defines.expForExploration);
            }
            Respawn("Site: " + destination);
            foreach (var connection in SitePath.paths.FindAll(x => x.sites.Contains(destination)))
            {
               var didRespawn = Respawn("Site: " + connection.sites.Find(x => x != destination));
               if (!didRespawn) CDesktop.LBWindow.GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.gameObject.AddComponent<FadeIn>());
            }
            Respawn("Site: " + prevSite);
            var site = Site.FindSite(x => x.name == save.currentSite);
            CDesktop.cameraDestination = new Vector2(site.x, site.y);
         }

         //This effect returns player to their home location
         void EffectHearthstonePlayer()
         {
            CloseDesktop("EquipmentScreen");
            SwitchDesktop("Map");
            var prevSite = save.currentSite;
            save.currentSite = save.player.homeLocation;
            SiteTown.town = (SiteTown)Site.FindSite(x => x.name == save.currentSite);
            Respawn("Site: " + prevSite);
            Respawn("Site: " + save.currentSite);
            CDesktop.cameraDestination = new Vector2(SiteTown.town.x, SiteTown.town.y);
         }
      }
   }

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
            else if (type == "ConsumeItem") EffectConsumeItem();
            else if (type == "ChangeActionSet") EffectChangeActionSet();

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
                  SpawnShatter(shatterSpeed, shatterDegree, new Vector3(shatterTarget == "Other" ? (board.playerTurn ? 129 : -167) : (board.playerTurn ? -167 : 129), 124), icon, false, shatterType == "Directional" ? shatterTarget == "Other" ? (board.playerTurn ? "1011" : "1110") : (board.playerTurn ? "1011" : "1110") : "0000");
            }

            //Plays a sound effect if it was specified in the effect
            void ExecuteSoundEffect()
            {
               if (board == null || !effect.ContainsKey("SoundEffect")) return;
               var volume = effect.ContainsKey("SoundEffectVolume") ? float.Parse(effect["SoundEffectVolume"]) : 0.7f;
               PlaySound(effect["SoundEffect"], volume);
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
               string damageAmount = effect.ContainsKey("DamageAmount") ? effect["DamageAmount"] : "None";
               var amount = damageAmount != "None" ? int.Parse(damageAmount) : source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale;
               target.Damage(futureBoard, amount, trigger["Trigger"] == "Damage");
            }
            else if (board != null)
            {
               var source = powerSource == "Effector" ? effector : other;
               var target = affect == "Effector" ? effector : other;
               string damageAmount = effect.ContainsKey("DamageAmount") ? effect["DamageAmount"] : "None";
               var amount = damageAmount != "None" ? int.Parse(damageAmount) : (int)Math.Round(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale);
               if (amount > 0 && target == board.enemy) PlayEnemyLine(board.enemy.EnemyLine("Wound"));
               target.Damage(amount, trigger["Trigger"] == "Damage");
               AddBigButtonOverlay(new Vector2(target == board.player ? -148 : 148, 141), "OtherDamaged", 1f, -1);
               SpawnFallingText(new Vector2(target == board.player ? -148 : 148, 141), "" + amount, "White");
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
               string healAmount = effect.ContainsKey("HealAmount") ? effect["HealAmount"] : "None";
               var amount = healAmount != "None" ? int.Parse(healAmount) : source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale;
               target.Heal(futureBoard, amount, trigger["Trigger"] == "Heal");
            }
            else if (board != null)
            {
               var source = powerSource == "Effector" ? effector : other;
               var target = affect == "Effector" ? effector : other;
               string healAmount = effect.ContainsKey("HealAmount") ? effect["HealAmount"] : "None";
               var amount = healAmount != "None" ? int.Parse(healAmount) : (int)Math.Round(source.RollWeaponDamage() * ((powerType == "Melee" ? source.MeleeAttackPower() : (powerType == "Spell" ? source.SpellPower() : (powerType == "Ranged" ? source.RangedAttackPower() : 1))) / 10.0 + 1) * powerScale);
               target.Heal(amount, trigger["Trigger"] == "Heal");
               AddBigButtonOverlay(new Vector2(target == board.player ? -148 : 148, 141), "OtherHealed", 1f, 5);
               SpawnFallingText(new Vector2(target == board.player ? -148 : 148, 141), "" + amount, "Uncommon");
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
               var pos = new Vector3(affect == "Other" ? (board.playerTurn ? 148 : -148) : (board.playerTurn ? -148 : 148), 142);
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
               SpawnFallingText(new Vector2(target == board.player ? -148 : 148, 142), buffDuration + " turn" + (buffDuration > 1 ? "s" : ""), "White");
            }
         }

         //This effect removes a buff from the targetted entity
         void EffectRemoveBuff()
         {
            if (affect != "Effector" && affect != "Other") return;
            else if (futureBoard != null)
            {
               var target = affect == "Effector" ? futureEffector : futureOther;
               var buff = target.buffs.Find(x => x.Item1.name == buffName);
               if (buff.Item1 != null)
               {
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
                  target.RemoveBuff(buff);
               }
            }
            else if (board != null)
            {
               var target = affect == "Effector" ? effector : other;
               var buff = target.buffs.Find(x => x.Item1 == buffs.Find(x => x.name == buffName));
               if (buff.Item1 != null)
               {
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
                  target.RemoveBuff(buff);
               }
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

         //This effect consumes one stack of the item
         void EffectConsumeItem()
         {
            if (board != null)
            {
               var target = effector;
               var itemUsed = target.inventory.items.Find(x => x.GetHashCode() + "" == trigger["ItemHash"]);
               itemUsed.amount--;
               if (itemUsed.amount == 0)
                  target.inventory.items.Remove(itemUsed);
               Respawn("PlayerQuickUse", true);
            }
         }

         //This effect changes the action set that the entity has in combat
         void EffectChangeActionSet()
         {
            if (futureBoard != null)
            {
               var target = affect == "Effector" ? futureEffector : futureOther;
               string to = effect.ContainsKey("ActionSet") ? effect["ActionSet"] : "Default";
               target.currentActionSet = to;
            }
            else if (board != null)
            {
               var target = affect == "Effector" ? effector : other;
               string to = effect.ContainsKey("ActionSet") ? effect["ActionSet"] : "Default";
               target.currentActionSet = to;
               if (target == board.enemy)
               {
                  CloseWindow("EnemyBattleInfo");
                  Respawn("PlayerBattleInfo");
               }
               else if (target == board.player)
               {
                  CloseWindow("PlayerBattleInfo");
                  Respawn("PlayerBattleInfo");
               }
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
                     SpawnShatter(shatterSpeed, shatterDegree, board.window.LBRegionGroup.regions[e.Item2].bigButtons[e.Item1].transform.position + new Vector3(-17.5f, -17.5f), Board.boardButtonDictionary[board.field[e.Item1, e.Item2]], false);
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
      "ChangeElements",
      "ChangeActionSet"
   };
}
