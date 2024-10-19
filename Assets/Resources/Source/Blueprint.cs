using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static UnityEngine.KeyCode;

using static Root;
using static Root.Anchor;
using static Root.RegionBackgroundType;

using static Item;
using static Race;
using static Zone;
using static Site;
using static Spec;
using static Quest;
using static Sound;
using static Mount;
using static Board;
using static Recipe;
using static Person;
using static Defines;
using static Faction;
using static Ability;
using static Enchant;
using static SitePath;
using static SaveGame;
using static Coloring;
using static Inventory;
using static PersonType;
using static Profession;
using static FishingSpot;
using static GameSettings;
using static PersonCategory;
using static SiteHostileArea;
using static SiteInstance;
using static SiteComplex;
using static SiteTown;
using System.Xml.Linq;

public class Blueprint
{
    public Blueprint(string title, Action actions, bool upperUI = false)
    {
        this.title = title;
        this.actions = actions;
        this.upperUI = upperUI;
    }

    public string title;
    public Action actions;
    public bool upperUI;    

    public static List<Blueprint> windowBlueprints = new()
    {
        //Bestiary
        new("BestiaryKalimdor", () => {
            SetAnchor(-210, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(200);
            SetRegionGroupHeight(176);
            AddHeaderRegion(() =>
            {
                AddLine("Kalimdor", "", "Center");
            });
            AddPaddingRegion(() =>
            {
                SetRegionBackgroundAsImage("BestiaryKalimdor");
                SetRegionAsGroupExtender();
            });
            AddRegionGroup();
            SetRegionGroupWidth(200);
            SetRegionGroupHeight(95);
            AddPaddingRegion(() =>
            {
                var allSites = new List<Site>();
                for (int i = 0; i < towns.Count; i++)
                    allSites.Add(towns[i]);
                for (int i = 0; i < areas.Count; i++)
                    allSites.Add(areas[i]);
                for (int i = 0; i < complexes.Count; i++)
                    allSites.Add(complexes[i]);
                for (int i = 0; i < instances.Count; i++)
                    allSites.Add(instances[i]);
                var zonesExcluded = zones.FindAll(x => x.continent != "Kalimdor").Select(x => x.name);
                allSites.RemoveAll(x => x.x == 0 && x.y == 0 || zonesExcluded.Contains(x.zone));
                AddLine("Explored areas: " + allSites.Count(x => currentSave.siteVisits.ContainsKey(x.name)) + " / " + allSites.Count, "DarkGray", "Center");
                var commons = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.commonEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Common entries: " + commons.Count(x => currentSave.commonsKilled.ContainsKey(x)) + " / " + commons.Count, "DarkGray", "Center");
                var rares = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.rareEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Rares killed: " + rares.Count(x => currentSave.raresKilled.ContainsKey(x)) + " / " + rares.Count, "DarkGray", "Center");
                var elites = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.eliteEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Elites killed: " + elites.Count(x => currentSave.elitesKilled.ContainsKey(x)) + " / " + elites.Count, "DarkGray", "Center");
                SetRegionAsGroupExtender();
            });
            //AddButtonRegion(() =>
            //{
            //    AddLine("Explore", "", "Center");
            //},
            //(h) =>
            //{
            //    //PlaySound("DesktopInstanceOpen");
            //    //SpawnDesktopBlueprint("Bestiary");
            //});
        }),
        new("BestiaryEasternKingdoms", () => {
            SetAnchor(9, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(200);
            SetRegionGroupHeight(176);
            AddHeaderRegion(() =>
            {
                AddLine("Eastern Kingdoms", "", "Center");
            });
            AddPaddingRegion(() =>
            {
                SetRegionBackgroundAsImage("BestiaryEasternKingdoms");
                SetRegionAsGroupExtender();
            });
            AddRegionGroup();
            SetRegionGroupWidth(200);
            SetRegionGroupHeight(95);
            AddPaddingRegion(() =>
            {
                var allSites = new List<Site>();
                for (int i = 0; i < towns.Count; i++)
                    allSites.Add(towns[i]);
                for (int i = 0; i < areas.Count; i++)
                    allSites.Add(areas[i]);
                for (int i = 0; i < complexes.Count; i++)
                    allSites.Add(complexes[i]);
                for (int i = 0; i < instances.Count; i++)
                    allSites.Add(instances[i]);
                var zonesExcluded = zones.FindAll(x => x.continent != "Eastern Kingdoms").Select(x => x.name);
                allSites.RemoveAll(x => x.x == 0 && x.y == 0 || zonesExcluded.Contains(x.zone));
                AddLine("Explored areas: " + allSites.Count(x => currentSave.siteVisits.ContainsKey(x.name)) + " / " + allSites.Count, "DarkGray", "Center");
                var commons = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.commonEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Common entries: " + commons.Count(x => currentSave.commonsKilled.ContainsKey(x)) + " / " + commons.Count, "DarkGray", "Center");
                var rares = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.rareEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Rares killed: " + rares.Count(x => currentSave.raresKilled.ContainsKey(x)) + " / " + rares.Count, "DarkGray", "Center");
                var elites = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.eliteEncounters ?? new()).Select(x => x.who).Distinct().ToList();
                AddLine("Elites killed: " + elites.Count(x => currentSave.elitesKilled.ContainsKey(x)) + " / " + elites.Count, "DarkGray", "Center");
                SetRegionAsGroupExtender();
            });
            //AddButtonRegion(() =>
            //{
            //    AddLine("Explore", "", "Center");
            //},
            //(h) =>
            //{
            //    //PlaySound("DesktopInstanceOpen");
            //    //SpawnDesktopBlueprint("Bestiary");
            //});
        }),

        //Game
        new("BoardFrame", () => {
            SetAnchor(-115, 146);
            var boardBackground = new GameObject("BoardBackground", typeof(SpriteRenderer), typeof(SpriteMask));
            boardBackground.transform.parent = CDesktop.LBWindow().transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/BoardFrames/BoardBackground" + board.field.GetLength(0) + "x" + board.field.GetLength(1));
            var mask = boardBackground.GetComponent<SpriteMask>();
            mask.sprite = Resources.Load<Sprite>("Sprites/BoardFrames/BoardMask" + board.field.GetLength(0) + "x" + board.field.GetLength(1));
            mask.isCustomRangeActive = true;
            mask.frontSortingLayerID = SortingLayer.NameToID("Missile");
            mask.backSortingLayerID = SortingLayer.NameToID("Default");
            boardBackground = new GameObject("BoardInShadow", typeof(SpriteRenderer));
            boardBackground.transform.parent = CDesktop.LBWindow().transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/BoardFrames/BoardShadow" + board.field.GetLength(0) + "x" + board.field.GetLength(1));
            boardBackground.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
        }, true),
        new("Board", () => {
            SetAnchor(Top, 0, -15 + 19 * (board.field.GetLength(1) - 7));
            DisableGeneralSprites();
            AddRegionGroup();
            for (int i = 0; i < board.field.GetLength(1); i++)
            {
                var I = i;
                AddPaddingRegion(() =>
                {
                    for (int j = 0; j < board.field.GetLength(0); j++)
                    {
                        var J = j;
                        AddBigButton(boardButtonDictionary[board.field[J, I]],
                        (h) =>
                        {
                            var list = board.FloodCount(h.region.bigButtons.FindIndex(x => x.GetComponent<Highlightable>() == h), h.region.regionGroup.regions.IndexOf(h.region));
                            board.finishedMoving = true;
                            board.FloodDestroy(list);
                        });
                    }
                });
            }
        }),
        new("BufferBoard", () => {
            SetAnchor(Top, 0, 213 + 19 * (BufferBoard.bufferBoard.field.GetLength(1) - 7));
            MaskWindow();
            DisableGeneralSprites();
            DisableCollisions();
            AddRegionGroup();
            for (int i = 0; i < BufferBoard.bufferBoard.field.GetLength(0); i++)
            {
                var I = i;
                AddPaddingRegion(() =>
                {
                    for (int j = 0; j < BufferBoard.bufferBoard.field.GetLength(1); j++)
                    {
                        var J = j;
                        AddBigButton(BufferBoard.bufferBoard.GetFieldButton(J, I));
                    }
                });
            }
        }, true),
        new("EnemyBattleInfo", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            for (int i = board.spotlightEnemy.Count - 1; i >= 0; i--)
            {
                var index = i;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(board.participants[board.spotlightEnemy[index]].who.name);
                    },
                    (h) =>
                    {
                        if (index == 0) return;
                        var temp = board.spotlightEnemy[index];
                        board.spotlightEnemy.RemoveAt(index);
                        board.spotlightEnemy.Insert(0, temp);
                        foreach (var res in board.participants[board.spotlightEnemy[0]].who.resources)
                        {
                            CloseWindow("Enemy" + res.Key + "Resource");
                            SpawnWindowBlueprint("Enemy" + res.Key + "Resource");
                        }
                    }
                );
                AddHeaderRegion(() =>
                {
                    var race = races.Find(x => x.name == board.participants[board.spotlightEnemy[index]].who.race);
                    AddBigButton(race.portrait == "" ? "OtherUnknown" : race.portrait + (race.genderedPortrait ? board.participants[index].who.gender : ""));
                    if (board.participants[board.spotlightEnemy[index]].who.dead) SetBigButtonToGrayscale();
                    AddLine("Level: ", "DarkGray");
                    AddText(board.participants[board.spotlightEnemy[index]].who.level - 10 > board.participants[board.spotlightFriendly[0]].who.level ? "??" : "" + board.participants[board.spotlightEnemy[index]].who.level, ColorEntityLevel(board.participants[board.spotlightEnemy[index]].who.level));
                });
                AddHealthBar(40, -38 - 65 * (board.spotlightEnemy.Count - index - 1), board.spotlightEnemy[index], board.participants[board.spotlightEnemy[index]].who);
                foreach (var actionBar in board.participants[board.spotlightEnemy[index]].who.actionBars[board.participants[board.spotlightEnemy[index]].who.currentActionSet])
                {
                    var abilityObj = abilities.Find(x => x.name == actionBar);
                    if (abilityObj == null || abilityObj.cost == null) continue;
                    AddButtonRegion(
                        () =>
                        {
                            ReverseButtons();
                            if (board.cooldowns[board.spotlightEnemy[index]].ContainsKey(actionBar))
                            {
                                AddLine(actionBar, "Black");
                                AddText(" \\ " + board.cooldowns[board.spotlightEnemy[index]][actionBar], "DimGray");
                            }
                            else AddLine(actionBar);
                            AddSmallButton(abilityObj.icon);
                            if (!abilityObj.EnoughResources(board.participants[board.spotlightEnemy[index]].who) || !abilityObj.AreAnyConditionsMet("AbilityCast", currentSave, board))
                            {
                                SetSmallButtonToGrayscale();
                                AddSmallButtonOverlay("OtherGridBlurred");
                            }
                            if (board.CooldownOn(board.spotlightEnemy[index], actionBar) > 0)
                                AddSmallButtonOverlay("AutoCast");
                        },
                        null,
                        null,
                        (h) => () =>
                        {
                            PrintAbilityTooltip(board.participants[board.spotlightEnemy[index]].who, abilityObj, board.participants[board.spotlightEnemy[index]].who.abilities[abilityObj.name]);
                        }
                    );
                }
                if (index > 0)
                {
                    AddSmallEmptyRegion();
                    AddSmallEmptyRegion();
                }
            }
        }),
        new("PlayerBattleInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            for (int i = board.spotlightFriendly.Count - 1; i >= 0; i--)
            {
                var index = i;
                AddButtonRegion(
                    () =>
                    {
                        AddLine(board.participants[board.spotlightFriendly[index]].who.name, "", "Right");
                    },
                    (h) =>
                    {
                        if (index == 0) return;
                        var temp = board.spotlightFriendly[index];
                        board.spotlightFriendly.RemoveAt(index);
                        board.spotlightFriendly.Insert(0, temp);
                        foreach (var res in board.participants[board.spotlightFriendly[0]].who.resources)
                        {
                            CloseWindow("Player" + res.Key + "Resource");
                            SpawnWindowBlueprint("Player" + res.Key + "Resource");
                        }
                    }
                );
                AddHeaderRegion(() =>
                {
                    ReverseButtons();
                    if (board.participants[board.spotlightFriendly[index]].who.spec != null)
                        AddBigButton(board.participants[board.spotlightFriendly[index]].who.Spec().icon);
                    else
                    {
                        var race = races.Find(x => x.name == board.participants[board.spotlightFriendly[index]].who.race);
                        AddBigButton(race.portrait == "" ? "OtherUnknown" : race.portrait + (race.genderedPortrait ? board.participants[board.spotlightFriendly[0]].who.gender : ""));
                    }
                    if (board.participants[board.spotlightFriendly[index]].who.dead) SetBigButtonToGrayscale();
                    AddLine("Level: " , "DarkGray");
                    AddText("" + board.participants[board.spotlightFriendly[index]].who.level, "Gray");
                });
                AddHealthBar(2, -38 - 65 * (board.spotlightFriendly.Count - index - 1), board.spotlightFriendly[index], board.participants[board.spotlightFriendly[index]].who);
                if (index > 0)
                {
                    AddSmallEmptyRegion();
                    AddSmallEmptyRegion();
                }
            }
            var aimedLength = board.participants[board.spotlightFriendly[0]].who.actionBars.Max(x => x.Value.Count());
            foreach (var actionBar in board.participants[board.spotlightFriendly[0]].who.actionBars[board.participants[0].who.currentActionSet])
            {
                aimedLength--;
                var abilityObj = abilities.Find(x => x.name == actionBar);
                if (abilityObj == null || abilityObj.cost == null) continue;
                AddButtonRegion(
                    () =>
                    {
                        if (board.cooldowns[board.spotlightFriendly[0]].ContainsKey(actionBar))
                        {
                            AddLine(board.cooldowns[board.spotlightFriendly[0]][actionBar] + " / ", "DimGray", "Right");
                            AddText(actionBar, "Black");
                        }
                        else AddLine(actionBar, "", "Right");
                        AddSmallButton(abilityObj.icon);
                        if (!abilityObj.EnoughResources(board.participants[board.spotlightFriendly[0]].who) || !abilityObj.AreAnyConditionsMet("AbilityCast", currentSave, board))
                        {
                            SetSmallButtonToGrayscale();
                            AddSmallButtonOverlay("OtherGridBlurred");
                        }
                        if (board.CooldownOn(board.spotlightFriendly[0], actionBar) > 0)
                            AddSmallButtonOverlay("AutoCast");
                    },
                    (h) =>
                    {
                        if (board.spotlightFriendly[0] == board.whosTurn)
                            if (abilityObj.EnoughResources(board.participants[board.spotlightFriendly[0]].who))
                                if (board.CooldownOn(board.spotlightFriendly[0], actionBar) <= 0 && abilityObj.AreAnyConditionsMet("AbilityCast", currentSave, board))
                                {
                                    foreach (var participant in board.participants)
                                    {
                                        if (participant == board.participants[board.spotlightFriendly[0]]) board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, {"Triggerer", "Effector" }, { "AbilityName", abilityObj.name } });
                                        else board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, {"Triggerer", "Other" }, { "AbilityName", abilityObj.name } });
                                    }
                                    board.participants[board.spotlightFriendly[0]].who.DetractResources(abilityObj.cost);
                                    foreach (var element in abilityObj.cost)
                                        board.log.elementsUsed.Inc(element.Key, element.Value);
                                }
                    },
                    null,
                    (h) => () =>
                    {
                        PrintAbilityTooltip(board.participants[board.spotlightFriendly[0]].who, abilityObj, board.participants[board.spotlightFriendly[0]].who.abilities.ContainsKey(abilityObj.name) ? board.participants[board.spotlightFriendly[0]].who.abilities[abilityObj.name] : 0);
                    }
                );
            }
            var item = board.participants[board.spotlightFriendly[0]].who.equipment.ContainsKey("Trinket") ? board.participants[board.spotlightFriendly[0]].who.equipment["Trinket"] : null;
            if (item != null && item.abilities != null && item.combatUse)
            {
                aimedLength--;
                var ability = item.abilities.ToList()[0];
                var abilityObj = abilities.Find(x => x.name == ability.Key);
                AddButtonRegion(
                    () =>
                    {
                        if (board.cooldowns[board.spotlightFriendly[0]].ContainsKey(ability.Key))
                        {
                            AddLine("" + board.cooldowns[board.spotlightFriendly[0]][ability.Key] + " / ", "DimGray", "Right");
                            AddText(ability.Key, "Black");
                        }
                        else AddLine(ability.Key, "", "Right");
                        AddSmallButton(item.icon);
                        if (!abilityObj.EnoughResources(board.participants[board.spotlightFriendly[0]].who))
                        {
                            SetSmallButtonToGrayscale();
                            AddSmallButtonOverlay("OtherGridBlurred");
                        }
                        if (board.CooldownOn(board.spotlightFriendly[0], ability.Key) > 0)
                            AddSmallButtonOverlay("AutoCast");
                    },
                    (h) =>
                    {
                        if (abilityObj.EnoughResources(board.participants[board.spotlightFriendly[0]].who) && board.CooldownOn(board.spotlightFriendly[0], ability.Key) <= 0)
                        {
                            foreach (var participant in board.participants)
                            {
                                if (participant == board.participants[board.spotlightFriendly[0]]) board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, {"Triggerer", "Effector" }, { "AbilityName", abilityObj.name } });
                                else board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, {"Triggerer", "Other" }, { "AbilityName", abilityObj.name } });
                            }
                            board.participants[board.spotlightFriendly[0]].who.DetractResources(abilityObj.cost);
                            foreach (var element in abilityObj.cost)
                                board.log.elementsUsed.Inc(element.Key, element.Value);
                        }
                    },
                    null,
                    (h) => () =>
                    {
                        PrintAbilityTooltip(board.participants[board.spotlightFriendly[0]].who, abilityObj, board.participants[board.spotlightFriendly[0]].who.abilities[abilityObj.name], item);
                    }
                );
            }
            for (int i = 0; i < aimedLength; i++)
                AddPaddingRegion(() => AddLine(""));
        }),
        new("PlayerQuickUse", () => {
            SetAnchor(Bottom, 0, 9);
            var entity = board.participants[board.whosTurn].who;
            if (entity.inventory.items.FindAll(x => x.combatUse && !x.CanEquip(entity)).Count == 0) return;
            else
            {
                AddRegionGroup();
                AddPaddingRegion(() =>
                {
                    foreach (var item in entity.inventory.items.FindAll(x => x.combatUse && !x.CanEquip(entity)))
                        if (item != null && item.abilities != null && item.combatUse)
                        {
                            var ability = item.abilities.ToList()[0];
                            var abilityObj = abilities.Find(x => x.name == ability.Key);
                            AddSmallButton(item.icon,
                            (h) =>
                            {
                                foreach (var participant in board.participants)
                                {
                                    if (participant == board.participants[0]) board.CallEvents(participant.who, new() { { "Trigger", "ItemUsed" }, { "Triggerer", "Effector" }, { "ItemHash", item.GetHashCode() + "" } });
                                    else board.CallEvents(participant.who, new() { { "Trigger", "ItemUsed" }, { "Triggerer", "Other" }, { "ItemHash", item.GetHashCode() + "" } });
                                }
                            },
                            null,
                            (h) => () => PrintItemTooltip(item));
                            if (board.CooldownOn(0, ability.Key) > 0)
                                AddSmallButtonOverlay("AutoCast");
                        }
                });
            }
        }),

        //Character
        new("CharacterInfoStats", () => {
            SetAnchor(-92, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(182);
            SetRegionGroupHeight(271);
            var stats = currentSave.player.Stats();
            AddHeaderRegion(() =>
            {
                AddLine("Character stats:");
            });
            AddHeaderRegion(() =>
            {
                foreach (var foo in stats)
                    if (!foo.Key.Contains("Mastery") && foo.Key != "Armor")
                    {
                        AddLine(foo.Key + ": ", "Gray", "Right");
                        AddText(foo.Value + "", "Uncommon");
                    }
            });
            AddHeaderRegion(() =>
            {
                AddLine("Armor: ", "Gray", "Right");
                AddText(currentSave.player.Armor() + "", "Uncommon");
                AddLine("Max health: ", "Gray", "Right");
                AddText(currentSave.player.MaxHealth() + "", "Uncommon");
                AddLine("Melee attack power: ", "Gray", "Right");
                AddText(currentSave.player.MeleeAttackPower() + "", "Uncommon");
                AddLine("Ranged attack power: ", "Gray", "Right");
                AddText(currentSave.player.RangedAttackPower() + "", "Uncommon");
                AddLine("Spell power: ", "Gray", "Right");
                AddText(currentSave.player.SpellPower() + "", "Uncommon");
                AddLine("Critical strike: ", "Gray", "Right");
                AddText(currentSave.player.CriticalStrike().ToString("0.00") + "%", "Uncommon");
                AddLine("Spell critical: ", "Gray", "Right");
                AddText(currentSave.player.SpellCritical().ToString("0.00") + "%", "Uncommon");
            });
            AddPaddingRegion(() => SetRegionAsGroupExtender());
        }, true),
        new("CharacterInfoStatsRight", () => {
            SetAnchor(TopRight, -19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(271);
            var stats = currentSave.player.Stats();
            AddHeaderRegion(() =>
            {
                AddLine("Character mastery:");
            });
            AddHeaderRegion(() =>
            {
                var ordered = stats.ToList().FindAll(x => x.Key.Contains("Mastery")).OrderBy(x => x.Key).OrderByDescending(x => x.Value).ToList();
                foreach (var foo in ordered)
                {
                    AddLine(foo.Key + ": ", "Gray", "Right");
                    AddText(foo.Value + "", "Uncommon");
                }
            });
            AddPaddingRegion(() => SetRegionAsGroupExtender());
        }, true),
        new("CharacterRankingTop", () => 
        {
            SetAnchor(-293, 153);
            DisableShadows();
            AddHeaderGroup();
            SetRegionGroupWidth(588);
            AddHeaderRegion(() =>
            {
                AddLine("Ranking", "Gray", "Center");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseDesktop("RankingScreen");
                });
            });
            if (settings.selectedRealmRanking == "")
                settings.selectedRealmRanking = Realm.realms[0].name;
            foreach (var realmRef in Realm.realms)
            {
                var realm = realmRef;
                AddRegionGroup();
                SetRegionGroupWidth(147);
                if (settings.selectedRealmRanking == realm.name)
                    AddPaddingRegion(() =>
                    {
                        AddLine(realm.name, "", "Center");
                    });
                else
                    AddButtonRegion(() =>
                    {
                        AddLine(realm.name, "", "Center");
                    },
                    (h) =>
                    {
                        settings.selectedRealmRanking = realm.name;
                        Respawn("CharacterRankingList");
                        Respawn("CharacterRankingListRight");
                    });
            }
        }),
        new("CharacterRankingList", () => 
        {
            SetAnchor(-293, 115);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(550);
            SetRegionGroupHeight(262);
            var slots = saves[settings.selectedRealmRanking].OrderByDescending(x => x.Score()).ToList();
            for (int i = 0; i < 7; i++)
                if (i < slots.Count)
                {
                    var slot = slots[i];
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("Portrait" + slot.player.race.Clean() + (slot.player.Race().genderedPortrait ? slot.player.gender : ""));
                        AddBigButton("Class" + slot.player.spec);
                        AddLine(slot.player.name + ", a level " + slot.player.level + " ");
                        AddText(slot.player.spec, slot.player.spec);
                        AddLine("Score: " + slot.Score());
                        if (slot.player.dead) AddText(", died fighting " + (slot.deathInfo.commonSource ? "a " : "") + slot.deathInfo.source + " in " + slot.deathInfo.area);
                    });
                }
                else
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("OtherBlank");
                        AddBigButton("OtherBlank");
                    });
        }),
        new("CharacterRankingListRight", () => 
        {
            SetAnchor(257, 115);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(38);
            SetRegionGroupHeight(262);
            var slots = saves[settings.selectedRealmRanking].OrderByDescending(x => x.Score()).ToList();
            for (int i = 0; i < 7; i++)
                if (i < slots.Count)
                {
                    var slot = slots[i];
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("PVP" + (slot.player.Side() == "Alliance" ? "A" : "H") + slot.player.Rank().rank);
                    });
                }
                else
                    AddPaddingRegion(() =>
                    {
                        AddBigButton("OtherBlank");
                    });
        }),
        new("CharacterRankingShadow", () => 
        {
            SetAnchor(-293, 153);
            AddRegionGroup();
            SetRegionGroupWidth(588);
            SetRegionGroupHeight(300);
            AddPaddingRegion(() => { });
        }),
        new("ExperienceBar", () => {
            SetAnchor(Bottom);
            var experience = currentSave == null ? 0 : (int)(319 * (currentSave.player.experience / (double)currentSave.player.ExperienceNeeded()));
            AddRegionGroup();
            SetRegionGroupWidth(experience * 2);
            SetRegionGroupHeight(12);
            AddPaddingRegion(() => { SetRegionBackground(Experience); });
            AddRegionGroup();
            SetRegionGroupWidth((319 - experience) * 2);
            SetRegionGroupHeight(12);
            AddPaddingRegion(() => { SetRegionBackground(ExperienceNone); });
        }, true),
        new("ExperienceBarBorder", () => {
            SetAnchor(Bottom);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(638);
            SetRegionGroupHeight(12);
            AddPaddingRegion(() => { });
        }),

        //Chest
        new("Chest", () => {
            if (WindowUp("Inventory")) return;
            if (WindowUp("Quest")) return;
            if (WindowUp("QuestAdd")) return;
            if (WindowUp("QuestTurn")) return;
            if (area == null || !currentSave.siteProgress.ContainsKey(area.name) || !area.progression.Any(x => x.type == "Treasure")) return;
            if (area.progression.First(x => x.type == "Treasure").point > currentSave.siteProgress[area.name] || currentSave.openedChests.ContainsKey(area.name) && currentSave.openedChests[area.name].inventory.items.Count == 0) return;
            SetAnchor(259, -111);
            Chest.SpawnChestObject(new Vector2(0, 0), "Chest");
        }),
        new("ChestInfo", () => {
            SetAnchor(-92, -86);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(
                () =>
                {
                    AddLine(area.name + " spoils:");
                    AddSmallButton("OtherClose", (h) =>
                    {
                        PlaySound("DesktopCloseChest");
                        CloseDesktop("ChestLoot");
                        Respawn("Chest");
                    });
                }
            );
        }),
        new("ChestLoot", () => {
            SetAnchor(-92, -105);
            AddRegionGroup();
            SetRegionGroupHeight(34);
            SetRegionGroupWidth(182);
            AddPaddingRegion(
                () =>
                {
                    for (int j = 0; j < 4 && j < currentSave.openedChests[area.name].inventory.items.Count; j++)
                        PrintLootItem(currentSave.openedChests[area.name].inventory.items[j]);
                }
            );
        }),

        //Login Screen
        new("CharacterRoster", () => {
            if (settings.selectedCharacter != "") SetDesktopBackground(saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter).LoginBackground(), true);
            else SetDesktopBackground("Backgrounds/Sky", true);
            SetAnchor(TopRight, -19, -92);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            AddHeaderRegion(() =>
            {
                AddLine("Realm:", "Gray");
                AddSmallButton("OtherClose", (h) => CloseDesktop("LoginScreen"));
            });
            AddButtonRegion(() => AddLine(settings.selectedRealm), (h) => SpawnDesktopBlueprint("ChangeRealm"));
            var aliveSlots = saves[settings.selectedRealm].FindAll(x => !x.player.dead);
            AddHeaderRegion(() =>
            {
                AddLine("Characters:", "Gray");
                if (aliveSlots.Count < 5)
                    AddSmallButton("OtherAdd", (h) =>
                    {
                        creationRace = "";
                        creationSpec = "";
                        creationGender = "";
                        String.creationName.Set("");
                        SpawnDesktopBlueprint("CharCreatorScreen");
                    });
                else AddSmallButton("OtherAddOff", (h) => { });
            });
            foreach (var slot in aliveSlots)
            {
                AddPaddingRegion(() =>
                {
                    AddBigButton("Portrait" + slot.player.race.Clean() + (slot.player.Race().genderedPortrait ? slot.player.gender : ""), (h) =>
                    {
                        CloseWindow("RealmRoster");
                        if (settings.selectedCharacter != slot.player.name)
                        {
                            settings.selectedCharacter = slot.player.name;
                            SetDesktopBackground(slot.LoginBackground(), true);
                            Respawn("CharacterInfo");
                        }
                    });
                    if (settings.selectedCharacter != slot.player.name)
                    {
                        SetBigButtonToGrayscale();
                        AddBigButtonOverlay("OtherGridBlurred");
                    }
                    AddLine(slot.player.name);
                    AddLine("Level: " + slot.player.level + " ");
                    AddText(slot.player.spec, slot.player.spec);
                });
            }
            for (int i = 0; i < 5 - aliveSlots.Count; i++)
                AddPaddingRegion(() => AddBigButton("OtherEmpty"));
        }, true),
        new("RealmRoster", () => 
        {
            SetAnchor(Center);
            AddHeaderGroup();
            SetRegionGroupWidth(258);
            AddHeaderRegion(() =>
            {
                AddLine("Realms:");
                AddSmallButton("OtherClose", (h) => CloseDesktop("ChangeRealm"));
            });
            AddRegionGroup();
            foreach (var realm in Realm.realms)
            {
                if (realm.name == settings.selectedRealm)
                    AddHeaderRegion(() =>
                    {
                        AddLine(realm.name);
                    });
                else
                    AddButtonRegion(() =>
                    {
                        AddLine(realm.name);
                    },
                    (h) =>
                    {
                        settings.selectedRealm = realm.name;
                        if (saves[settings.selectedRealm].Count(x => !x.player.dead) > 0)
                        {
                            settings.selectedCharacter = saves[settings.selectedRealm].First(x => !x.player.dead).player.name;
                            SpawnTransition();
                        }
                        else if (settings.selectedCharacter != "")
                        {
                            settings.selectedCharacter = "";
                            SpawnTransition();
                        }
                        CloseDesktop("ChangeRealm");
                        CloseDesktop("LoginScreen");
                        SpawnDesktopBlueprint("LoginScreen");
                    });
            }
            AddRegionGroup();
            foreach (var realm in Realm.realms)
            {
                AddPaddingRegion(() =>
                {
                    AddLine(realm.pvp ? "PVP" : "PVE", realm.pvp ? "DangerousRed" : "Gray");
                });
            }
            AddRegionGroup();
            foreach (var realm in Realm.realms)
            {
                AddPaddingRegion(() =>
                {
                    AddLine(realm.hardcore ? "Hardcore" : "Softcore", realm.hardcore ? "DangerousRed" : "Gray");
                });
            }
            AddRegionGroup();
            SetRegionGroupWidth(95);
            foreach (var realm in Realm.realms)
            {
                AddPaddingRegion(() =>
                {
                    var countAlive = saves[realm.name].Count(x => !x.player.dead);
                    AddLine(countAlive + "", countAlive == 5 ? "DangerousRed" : "Gray");
                    AddText(" / ", "DarkGray");
                    AddText("5", countAlive == 5 ? "DangerousRed" : "Gray");
                    AddText(" chars", "DarkGray");
                });
            }
        }, true),
        new("ConfirmDeleteCharacter", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(220);
            AddHeaderRegion(() =>
            {
                AddLine("Confirm deletion:");
            });
            AddPaddingRegion(() =>
            {
                AddLine("Type", "HalfGray");
                AddText(" DELETE ", "DangerousRed");
                AddText("to confirm deletion", "HalfGray");
            });
            AddPaddingRegion(() =>
            {
                AddInputLine(String.promptConfirm, "DangerousRed");
            });
        }, true),
        new("CharacterInfo", () => {
            SetAnchor(TopLeft, 19, -19);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            if (settings.selectedCharacter != "")
            {
                var slot = saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter);
                var spec = slot.player.Spec();
                AddHeaderRegion(() =>
                {
                    AddLine("Character:");
                    AddSmallButton("OtherTrash", (h) =>
                    {
                        String.promptConfirm.Set("");
                        PlaySound("DesktopMenuOpen", 0.6f);
                        SpawnWindowBlueprint("ConfirmDeleteCharacter");
                        CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                    });
                });
                AddHeaderRegion(() =>
                {
                    AddBigButton("Portrait" + slot.player.race.Clean() + (slot.player.Race().genderedPortrait ? slot.player.gender : ""));
                    AddLine(slot.player.name, "Gray");
                    AddLine("Level: " + slot.player.level + " ", "Gray");
                    AddText(spec.name, spec.name);
                });
                AddButtonRegion(() => AddLine("Enter World", "", "Center"),
                (h) =>
                {
                    Login();
                    SpawnDesktopBlueprint("Map");
                    CloseDesktop("TitleScreen");
                    var find = FindSite(x => x.name == currentSave.currentSite);
                    if (find != null) CDesktop.cameraDestination = new Vector2(find.x, find.y);
                    Cursor.cursor.transform.position += (Vector3)CDesktop.cameraDestination - CDesktop.screen.transform.position;
                    CDesktop.screen.transform.localPosition = CDesktop.cameraDestination;
                });
                AddEmptyRegion();
                AddHeaderRegion(() => AddLine("Total time played:"));
                AddPaddingRegion(() =>
                {
                    SetRegionAsGroupExtender();
                    AddLine(slot.timePlayed.Hours + "h "  + slot.timePlayed.Minutes + "m", "DarkGray");
                });
            }
        }, true),
        new("CharacterCreationFactionHorde", () => {
            SetAnchor(BottomLeft, 19, 19);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddBigButton("TabardOrgrimmar",
                (h) =>
                {
                    if (creationRace == "Orc") return;
                    creationRace = "Orc";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Orc").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Orcs of Orgrimmar", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Orcs are a proud and powerful race with warrior culture deeply rooted in honor and combat. Originally from the shattered world of Draenor, they now inhabit the harsh landscapes of Durotar, with their capital at Orgrimmar. They strive to overcome their dark past and build a new future for their people." }
                        }
                    } } } }.Print(null, 258, null);
                });
                if (creationRace != "Orc")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardDarkspearTribe",
                (h) =>
                {
                    if (creationRace == "Troll") return;
                    creationRace = "Troll";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Troll").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Trolls of the Darkspear Tribe", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Trolls are a fierce and agile race with a long history of mysticism and shamanism. The Darkspear Tribe, having allied with the Horde, has established itself in the Echo Isles and the coastal regions of Durotar. Known for their cunning and resourcefulness, they are formidable warriors and mystics." }
                        }
                    } } } }.Print(null, 258, null);
                });
                if (creationRace != "Troll")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardThunderBluff",
                (h) =>
                {
                    if (creationRace == "Tauren") return;
                    creationRace = "Tauren";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Tauren").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Tauren of Thunder Bluff", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Tauren are massive, bovine-like beings with a deep spiritual connection to nature and the Earth Mother. They dwell in the grassy plains of Mulgore, with their capital in Thunder Bluff. Renowned for their strength and wisdom, they serve as staunch protectors of the natural world." }
                        }
                    } } } }.Print(null, 258, null);
                });
                if (creationRace != "Tauren")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardUndercity",
                (h) =>
                {
                    if (creationRace == "Forsaken") return;
                    creationRace = "Forsaken";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Forsaken").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Forsaken of the Undercity", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "The Forsaken are former humans who have broken free from the Lich King's control, now seeking vengeance and a place in the world. They inhabit the eerie and decaying ruins of the Undercity, beneath the fallen kingdom of Lordaeron. Driven by a desire for autonomy and revenge, they are both feared and misunderstood by the living." }
                        }
                    } } } }.Print(null, 258, null);
                });
                if (creationRace != "Forsaken")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
            });
        }),
        new("CharacterCreationFactionAlliance", () => {
            SetAnchor(BottomRight, -19, 19);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddBigButton("TabardStormwind",
                (h) =>
                {
                    if (creationRace == "Human") return;
                    creationRace = "Human";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Human").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");

                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Humans of Stormwind", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Humans are a resilient and versatile race known for their unyielding spirit and strong sense of justice. They have a rich history of surviving numerous wars and catastrophes, making them natural leaders in the Alliance. Their capital city is Stormwind, a bustling hub of trade and governance." }
                        }
                    } } } }.Print(null, 258, null);
                });
                if (creationRace != "Human")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardIronforge",
                (h) =>
                {
                    if (creationRace == "Dwarf") return;
                    creationRace = "Dwarf";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Dwarf").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Dwarfs of Ironforge", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Dwarves are hardy and stout creatures famed for their skills in mining and blacksmithing. They hail from the snowy peaks of Dun Morogh and are deeply connected to their ancestral homeland of Ironforge. Their adventurous nature drives them to uncover ancient relics and forgotten lore." }
                        }
                    } } } }.Print(null, 258, null);
                });
                if (creationRace != "Dwarf")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardGnomeregan",
                (h) =>
                {
                    if (creationRace == "Gnome") return;
                    creationRace = "Gnome";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Gnome").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Gnomes of Gnomeregan", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Gnomes are brilliant inventors and tinkerers, known for their technological prowess and innovative gadgets. Originally from the subterranean city of Gnomeregan, many now reside with their Dwarven allies in Ironforge. Despite their small stature, they possess an insatiable curiosity and boundless energy." }
                        }
                    } } } }.Print(null, 258, null);
                });
                if (creationRace != "Gnome")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
                AddBigButton("TabardDarnassus",
                (h) =>
                {
                    if (creationRace == "Night Elf") return;
                    creationRace = "Night Elf";
                    creationSpec = "";
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Night Elf").previewSite).Background());
                    Respawn("CharacterCreationWho");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(Bottom, 0, 114);
                    AddRegionGroup();
                    SetRegionGroupWidth(258);
                    AddHeaderRegion(() => AddLine("Night Elfs of Darnassus", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Night Elves are an ancient and mystical race with a profound connection to nature and druidic magic. They once lived in isolation in the lush forests of Kalimdor with their majestic city of Darnassus. Renowned for their agility and wisdom, they strive to protect the world of nature from harm." }
                        }
                    } } } }.Print(null, 258, null);
                });
                if (creationRace != "Night Elf")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
            });
        }),
        new("CharacterCreationSpec", () => {
            SetAnchor(Top, 0, -19);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                var availableSpecs = specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace));
                foreach (var foo in availableSpecs)
                {
                    var spec = foo;
                    AddBigButton(spec.icon,
                    (h) =>
                    {
                        if (creationSpec == spec.name) return;
                        creationSpec = spec.name;
                        Respawn("CharacterCreationWho");
                        Respawn("CharacterCreationFactionRaceChoice");
                        CloseWindow("CharacterCreationFinish");
                        SpawnWindowBlueprint("CharacterCreationFinish");
                    },
                    null, (h) => () =>
                    {
                        SetAnchor(Top, 0, -76);
                        AddRegionGroup();
                        SetRegionGroupWidth(296);
                        AddHeaderRegion(() => AddLine(spec.name, "", "Center"));
                        Description desc = null;
                        if (spec.name == "Warlock")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Warlocks are feared practitioners of dark magic, summoning demons and wielding fel energies that corrupt and destroy. Often shunned by mainstream society, they are driven by a desire for power and knowledge forbidden to others. Warlocks walk a perilous path, balancing the destructive forces they command with the ever-present risk of their own corruption." }
                                }
                            } } } };
                        else if (spec.name == "Mage")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Mages are scholarly spellcasters who delve into the arcane arts, harnessing the raw energies of magic to alter reality. They are often members of esteemed magical institutions like the Kirin Tor, dedicating their lives to the pursuit of knowledge and mastery of the arcane. Mages wield immense power, capable of both creating wonders and unleashing devastating destruction." }
                                }
                            } } } };
                        else if (spec.name == "Priest")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Priests are devout servants of the divine, channeling the powers of the Light or the Void to heal and guide their followers. They serve as spiritual leaders within their communities, offering solace and wisdom in times of need. Whether upholding the Light's purity or delving into the shadows of the Void, priests are driven by their faith and commitment to their spiritual path." }
                                }
                            } } } };
                        else if (spec.name == "Rogue")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Rogues are shadowy figures who thrive in the underbelly of society, mastering the arts of stealth, subterfuge, and assassination. They are often found as spies, thieves, and mercenaries, using their cunning and agility to outmaneuver their foes. Rogues are the unseen enforcers of their factions, executing their tasks with lethal precision and leaving no trace behind." }
                                }
                            } } } };
                        else if (spec.name == "Warrior")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Warriors are the embodiment of physical strength and martial prowess, drawing on centuries-old traditions of combat and honor. They are often seen as the backbone of their respective societies, respected for their bravery and skill in battle. From the disciplined ranks of Stormwind's knights to the fierce clans of orcish berserkers, warriors are found in every culture, upholding their people's martial heritage." }
                                }
                            } } } };
                        else if (spec.name == "Druid")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Druids are guardians of nature who draw their strength from the natural world and the primal forces that govern it. They follow the teachings of ancient demigods like Cenarius, learning to shapeshift and harness the power of the wild. Druids are deeply connected to the balance of nature, serving as its protectors and stewards in a world often threatened by chaos and destruction." }
                                }
                            } } } };
                        else if (spec.name == "Shaman")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Shamans are spiritual mediators who commune with the elemental forces of nature, invoking the power of earth, fire, water, and air. They are deeply respected within their societies as guides and visionaries, capable of bridging the physical and spiritual worlds. Shamans draw upon ancient traditions and rituals, channeling the elements to maintain balance and harmony within the world." }
                                }
                            } } } };
                        else if (spec.name == "Paladin")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Paladins are holy warriors dedicated to the Light, wielding divine power to vanquish evil and protect the innocent. Originating from the ancient orders such as the Knights of the Silver Hand, they are bound by a sacred oath to uphold justice and righteousness. Paladins are revered as champions of their faith, standing as beacons of hope in the darkest times." }
                                }
                            } } } };
                        else if (spec.name == "Hunter")
                            desc = new Description()
                            { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                                {
                                    { "Color", "DarkGray" },
                                    { "Align", "Center" },
                                    { "Text", "Hunters are expert survivalists and marksmen, deeply attuned to the wilderness and the creatures that inhabit it. Often raised in the wilds, they form profound bonds with their animal companions and learn to navigate and master their environment. From the forests of Ashenvale to the savannas of the Barrens, hunters are the quintessential rangers and protectors of the natural world." }
                                }
                            } } } };
                        desc?.Print(null, 296, null);
                    });
                    if (creationSpec != spec.name)
                    {
                        AddBigButtonOverlay("OtherGridBlurred");
                        SetBigButtonToGrayscale();
                    }
                }
            });
        }),
        new("CharacterCreationFinish", () => {
            SetAnchor(Bottom, 0, 76);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton("ActionReroll",
                (h) =>
                {
                    var temp = random.Next(8);
                    creationRace = temp == 0 ? "Orc" : (temp == 1 ? "Troll" : (temp == 2 ? "Tauren" : (temp == 3 ? "Forsaken" : (temp == 4 ? "Human" : (temp == 5 ? "Dwarf" : (temp == 6 ? "Gnome" : "Night Elf"))))));
                    creationGender = random.Next(2) == 0 ? "Male" : "Female";
                    var availableSpecs = specs.FindAll(x => x.startingEquipment.ContainsKey(creationRace));
                    creationSpec = availableSpecs[random.Next(availableSpecs.Count)].name;
                    var name = "";
                    var race = races.Find(x => x.name == creationRace);
                    do name = creationGender == "Female" ? race.femaleNames[random.Next(race.femaleNames.Count)] : race.maleNames[random.Next(race.maleNames.Count)];
                    while (saves[settings.selectedRealm].Any(x => x.player.name == name));
                    String.creationName.Set(name);
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == creationRace).previewSite).Background());
                    CloseWindow("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationFactionRaceChoice");
                    CloseWindow("CharacterCreationFinish");
                    CloseWindow("CharacterCreationSpec");
                    CloseWindow("CharacterCreationWho");
                    SpawnWindowBlueprint("CharacterCreationFactionHorde");
                    SpawnWindowBlueprint("CharacterCreationFactionAlliance");
                    SpawnWindowBlueprint("CharacterCreationFactionRaceChoice");
                    SpawnWindowBlueprint("CharacterCreationFinish");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationWho");
                    PlaySound("DesktopReroll" + random.Next(1, 3), 0.6f);
                });
            });
            AddRegionGroup();
            SetRegionGroupWidth(114);
            if (creationSpec != "" && creationGender != "" && creationRace != "" && String.creationName.Value().Length < 3) AddButtonRegion(() => { SetRegionBackground(ButtonRed); AddLine("Finish Creation", "Black", "Center"); });
            else if (creationSpec != "" && creationGender != "" && creationRace != "")
            {
                AddButtonRegion(() =>
                {
                    AddLine("Finish Creation", "", "Center");
                },
                (h) =>
                {
                    PlaySound("DesktopCreateCharacter");
                    AddNewSave();
                    CloseDesktop("CharCreatorScreen");
                    CloseDesktop("LoginScreen");
                    SpawnDesktopBlueprint("LoginScreen");
                    SpawnTransition();
                    SaveGames();
                });
            }
            else AddPaddingRegion(() => AddLine("Finish Creation", "DarkGray", "Center"));
            AddRegionGroup();
            AddPaddingRegion(() => AddSmallButton("OtherClose", (h) => CloseDesktop("CharCreatorScreen")));
        }),
        new("CharacterCreationWho", () => {
            SetAnchor(Bottom, 0, 19);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddBigButton("Portrait" + creationRace.Clean() + "Male",
                (h) =>
                {
                    if (creationRace != "")
                    {
                        creationGender = "Male";
                        var oldName = String.creationName.Value();
                        var name = "";
                        var race = races.Find(x => x.name == creationRace);
                        do name = race.maleNames[random.Next(race.maleNames.Count)];
                        while (name == oldName || saves[settings.selectedRealm].Any(x => x.player.name == name));
                        String.creationName.Set(name);
                        CloseWindow("CharacterCreationWho");
                        Respawn("CharacterCreationWho");
                        CloseWindow("CharacterCreationFinish");
                        Respawn("CharacterCreationFinish");
                    }
                });
                if (creationRace != "" && creationGender != "Male")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
            });
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                if (creationRace == "") AddLine("Choose Race", "DarkGray", "Center");
                else if (creationGender == "" && creationSpec == "") AddLine("Choose Portrait and Class", "DarkGray", "Center");
                else if (creationSpec == "") AddLine("Choose Class", "DarkGray", "Center");
                else if (creationGender == "") AddLine("Choose Portrait", "DarkGray", "Center");
                else AddInputLine(String.creationName, "White", "Center");
            });
            AddPaddingRegion(() =>
            {
                AddLine(creationRace != "" ? creationRace + (creationSpec != "" ? " " + creationSpec : "") : "", "", "Center");
            });
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddBigButton("Portrait" + creationRace.Clean() + "Female",
                (h) =>
                {
                    if (creationRace != "")
                    {
                        creationGender = "Female";
                        var oldName = String.creationName.Value();
                        var name = "";
                        var race = races.Find(x => x.name == creationRace);
                        do name = race.femaleNames[random.Next(race.femaleNames.Count)];
                        while (name == oldName || saves[settings.selectedRealm].Any(x => x.player.name == name));
                        String.creationName.Set(name);
                        CloseWindow("CharacterCreationWho");
                        Respawn("CharacterCreationWho");
                        CloseWindow("CharacterCreationFinish");
                        Respawn("CharacterCreationFinish");
                    }
                });
                if (creationRace != "" && creationGender != "Female")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
            });
        }),

        //Crafting Screen
        new("ProfessionListPrimary", () => {
            SetAnchor(-301, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var professions = Profession.professions.FindAll(x => currentSave.player.professionSkills.ContainsKey(x.name));
            AddHeaderRegion(() =>
            {
                AddLine("Primary professions:");
            });
            var primary = professions.Where(x => x.primary).ToList();
            for (int i = 0; i < defines.maxPrimaryProfessions; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (primary.Count() > index)
                    {
                        AddLine(primary[index].name);
                        AddLine("Skill: ", "DarkGray");
                        AddText(currentSave.player.professionSkills[primary[index].name].Item1 + "", "Gray");
                        AddText(" / ", "DarkGray");
                        AddText(primary[index].levels.FindAll(x => currentSave.player.professionSkills[primary[index].name].Item2.Contains(x.name)).Max(x => x.maxSkill) + "", "Gray");
                        AddBigButton(primary[index].icon,
                        (h) =>
                        {
                            profession = primary[index];
                            if (profession.recipeType == null) return;
                            CloseWindow("ProfessionListPrimary");
                            CloseWindow("ProfessionListSecondary");
                            Respawn("CraftingList");
                            PlaySound("DesktopInstanceOpen");
                            SetDesktopBackground("Backgrounds/Profession");
                        });
                    }
                    else AddBigButton("OtherDisabled");
                });
            }
        }),
        new("ProfessionListSecondary", () => {
            SetAnchor(-301, 28);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine("Secondary professions:");
            });
            var secondary = professions.Where(x => !x.primary).ToList();
            for (int i = 0; i < secondary.Count(); i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (currentSave.player.professionSkills.ContainsKey(secondary[index].name))
                    {
                        AddLine(secondary[index].name);
                        AddLine("Skill: ", "DarkGray");
                        AddText(currentSave.player.professionSkills[secondary[index].name].Item1 + "", "Gray");
                        AddText(" / ", "DarkGray");
                        AddText(secondary[index].levels.FindAll(x => currentSave.player.professionSkills[secondary[index].name].Item2.Contains(x.name)).Max(x => x.maxSkill) + "", "Gray");
                        AddBigButton(secondary[index].icon,
                        (h) =>
                        {
                            profession = secondary[index];
                            if (profession.recipeType == null) return;
                            CloseWindow("ProfessionListPrimary");
                            CloseWindow("ProfessionListSecondary");
                            Respawn("CraftingList");
                            PlaySound("DesktopInstanceOpen");
                            SetDesktopBackground("Backgrounds/Profession");
                        });
                    }
                    else AddBigButton("OtherDisabled");
                });
            }
        }),
        new("CraftingList", () => {
            SetAnchor(TopLeft, 19, -38);
            var recipes = currentSave.player.learnedRecipes[profession.name].Select(x => Recipe.recipes.Find(y => y.name == x)).Where(x => (!settings.onlyHavingMaterials.Value() || currentSave.player.CanCraft(x, true, true) > 0) && (!settings.onlySkillUp.Value() || x.skillUpGray > currentSave.player.professionSkills[profession.name].Item1)).ToList();
            AddRegionGroup(() => recipes.Count, 11);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                AddLine(profession.name);
                //AddLine("Skill: ", "DarkGray");
                //AddText(currentSave.player.professionSkills[profession.name].Item1 + "", "Gray");
                //AddText(" / ", "DarkGray");
                //AddText(profession.levels.FindAll(x => currentSave.player.professionSkills[profession.name].Item2.Contains(x.levelName)).Max(x => x.maxSkill) + "", "Gray");
                AddBigButton(profession.icon);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("CraftingRecipe");
                    CloseWindow("CraftingList");
                    Respawn("ProfessionListPrimary");
                    Respawn("ProfessionListSecondary");
                    PlaySound("DesktopInstanceClose");
                    SetDesktopBackground("Backgrounds/Professions");
                });
                AddSkillBar(40, -19, profession, currentSave.player);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Known " + profession.recipeType.ToLower() + (profession.recipeType.Last() == 's' ? ":" : "s:"), "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.player.learnedRecipes[profession.name].Reverse();
                    CloseWindow("CraftingList");
                    Respawn("CraftingList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!WindowUp("CraftingSettings") && !WindowUp("CraftingSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("CraftingSort");
                        CloseWindow("CraftingList");
                        Respawn("CraftingList");
                    });
                else
                    AddSmallButton("OtherSortOff");
                if (!WindowUp("CraftingSettings") && !WindowUp("CraftingSort"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("CraftingSettings");
                        CloseWindow("CraftingList");
                        Respawn("CraftingList");
                    });
                else
                    AddSmallButton("OtherSettingsOff");
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            for (int i = 0; i < 11; i++)
            {
                var index = i;
                if (recipes.Count > index + 11 * regionGroup.pagination())
                {
                    AddButtonRegion(() =>
                    {
                        var recipe = recipes[index + 11 * regionGroup.pagination()];
                        AddLine(recipe.name, "Black");
                        var amountPossible = currentSave.player.CanCraft(recipe, false, true);
                        AddText(amountPossible > 0 ? " [" + amountPossible + "]" : "", "Black");
                        AddSmallButton(recipe.Icon());
                        if (settings.rarityIndicators.Value() && recipe.results.Count > 0)
                            AddSmallButtonOverlay("OtherRarity" + items.Find(x => x.name == recipe.results.ToList()[0].Key), 0, 2);
                    },
                    (h) =>
                    {
                        recipe = recipes[index + 11 * regionGroup.pagination()];
                        enchant = recipe.enchantment ? enchants.Find(x => x.name == recipe.name) : null;
                        if (enchantmentTarget != null && (enchant == null || enchant.type != enchantmentTarget.type))
                            enchantmentTarget = null;
                        Respawn("CraftingRecipe");
                        PlaySound("DesktopInstanceOpen");
                    });
                    var skill = currentSave.player.professionSkills[profession.name].Item1;
                    if (recipes[index + 11 * regionGroup.pagination()].skillUpYellow > skill)
                        SetRegionBackgroundAsImage("SkillUpOrange");
                    else if (recipes[index + 11 * regionGroup.pagination()].skillUpGreen > skill)
                        SetRegionBackgroundAsImage("SkillUpYellow");
                    else if (recipes[index + 11 * regionGroup.pagination()].skillUpGray > skill)
                        SetRegionBackgroundAsImage("SkillUpGreen");
                    else SetRegionBackgroundAsImage("SkillUpGray");
                }
                else if (recipes.Count == index + 11 * regionGroup.pagination())
                {
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
                }
            }
            AddPaginationLine(regionGroup);
        }),
        new("CraftingRecipe", () => {
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                var skill = currentSave.player.professionSkills[profession.name].Item1;
                AddLine(recipe.name + ":", "Black");
                AddSmallButton("OtherClose", (h) => 
                {
                    CloseWindow("CraftingRecipe");
                    PlaySound("DesktopInstanceClose");
                });
                if (recipe.skillUpYellow > skill) SetRegionBackgroundAsImage("SkillUpOrange");
                else if (recipe.skillUpGreen > skill) SetRegionBackgroundAsImage("SkillUpYellow");
                else if (recipe.skillUpGray > skill) SetRegionBackgroundAsImage("SkillUpGreen");
                else SetRegionBackgroundAsImage("SkillUpGray");
                
            });
            if (recipe.results.Count > 0)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Results:", "Gray");
                });
                AddPaddingRegion(() =>
                {
                    var results = recipe.results.Select(x => items.Find(y => y.name == x.Key).CopyItem(x.Value)).ToList();
                    foreach (var result in results)
                    {
                        AddBigButton(result.icon, null, null, (h) => () =>
                        {
                            if (WindowUp("CraftingSort")) return;
                            if (WindowUp("CraftingSettings")) return;
                            PrintItemTooltip(result, Input.GetKey(LeftShift));
                        });
                        SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * (results.IndexOf(result) % 5), result.amount + "", "", "Right");
                    }
                });
            }
            if (recipe.enchantment)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Enchantment:", "Gray");
                });
                AddPaddingRegion(() =>
                {
                    AddBigButton("AbilityGreaterHeal");
                });
            }
            if (recipe.reagents.Count > 0)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Reagents:", "Gray");
                });
                AddPaddingRegion(() =>
                {
                    var reagents = recipe.reagents.Select(x => items.Find(y => y.name == x.Key).CopyItem(x.Value)).ToList();
                    foreach (var reagent in reagents)
                    {
                        AddBigButton(reagent.icon, null, null, (h) => () =>
                        {
                            if (WindowUp("CraftingSort")) return;
                            if (WindowUp("CraftingSettings")) return;
                            PrintItemTooltip(reagent, Input.GetKey(LeftShift));
                        });
                        var playerPossesion = currentSave.player.inventory.items.Sum(x => x.name == reagent.name ? x.amount : 0);
                        SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * (reagents.IndexOf(reagent) % 5), playerPossesion + "/" + reagent.amount, playerPossesion < reagent.amount ? "DangerousRed" : "", "Right");
                    }
                });
            }
            if (recipe.enchantment)
            {
                AddHeaderRegion(() => AddLine("Enchantment target:", "Gray"));
                AddPaddingRegion(() =>
                {
                    AddBigButton(enchantmentTarget != null ? enchantmentTarget.icon : "OtherUnknown",
                    (h) =>
                    {
                        CloseWindow("CraftingSort");
                        CloseWindow("CraftingSettings");
                        CloseWindow("CraftingList");
                        Respawn("EnchantingList");
                    },
                    null,
                    (h) => () =>
                    {
                        if (enchantmentTarget == null) return;
                        if (WindowUp("CraftingSort")) return;
                        if (WindowUp("CraftingSettings")) return;
                        PrintItemTooltip(enchantmentTarget, Input.GetKey(LeftShift));
                    });
                });
            }
            AddPaddingRegion(() =>
            {
                if (recipe.enchantment && enchantmentTarget?.enchant != null)
                {
                    AddLine("This process will override", "DangerousRed");
                    AddLine("the previous enchantment!", "DangerousRed");
                }
                SetRegionAsGroupExtender();
            });
            if (currentSave.player.CanCraft(recipe) > 0 && (!recipe.enchantment || enchantmentTarget != null))
                AddButtonRegion(() => AddLine(recipe.enchantment ? "Enchant" : "Craft"),
                (h) =>
                {
                    var crafted = currentSave.player.Craft(recipe);
                    var skill = currentSave.player.professionSkills;
                    var isMaxed = skill[recipe.profession].Item1 == profession.levels.FindAll(x => skill[recipe.profession].Item2.Contains(x.name)).Max(x => x.maxSkill);
                    if (!isMaxed)
                    {
                        var suc = false;
                        if (recipe.skillUpYellow > skill[recipe.profession].Item1)
                            (skill[recipe.profession], suc) = ((skill[recipe.profession].Item1 + 1, skill[recipe.profession].Item2), true);
                        else if (recipe.skillUpGreen > skill[recipe.profession].Item1 && Roll(75))
                            (skill[recipe.profession], suc) = ((skill[recipe.profession].Item1 + 1, skill[recipe.profession].Item2), true);
                        else if (recipe.skillUpGray > skill[recipe.profession].Item1 && Roll(25))
                            (skill[recipe.profession], suc) = ((skill[recipe.profession].Item1 + 1, skill[recipe.profession].Item2), true);
                        if (suc) SpawnFallingText(new Vector2(0, 34), recipe.profession + " increased to " + skill[recipe.profession].Item1, "Blue");
                    }
                    foreach (var item in crafted)
                    {
                        currentSave.player.inventory.AddItem(item);
                        PlaySound(item.ItemSound("PutDown"), 0.8f);
                    }
                    if (recipe.enchantment)
                    {
                        currentSave.player.Unequip(new() { enchant.type });
                        enchantmentTarget.enchant = enchant;
                        enchantmentTarget = null;
                        enchantmentTarget.Equip(currentSave.player, enchant.type);
                        PlaySound("PutDownGems", 0.8f);
                    }
                    Respawn("CraftingList");
                    CloseWindow("CraftingRecipe");
                    SpawnWindowBlueprint("CraftingRecipe");
                });
            else
                AddPaddingRegion(() => AddLine(recipe.enchantment ? "Enchant" : "Craft", "DarkGray"));
        }),
        new("CraftingSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort " + profession.recipeType.ToLower() + (profession.recipeType.Last() == 's' ? ":" : "s:"), "Gray");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("CraftingSort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() => AddLine("By name", "Black"),
            (h) =>
            {
                currentSave.player.learnedRecipes[profession.name] = currentSave.player.learnedRecipes[profession.name].OrderBy(x => recipes.Find(y => y.name == x).name).ToList();
                CloseWindow("CraftingSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By possible crafts", "Black"),
            (h) =>
            {
                currentSave.player.learnedRecipes[profession.name] = currentSave.player.learnedRecipes[profession.name].OrderByDescending(x => currentSave.player.CanCraft(recipes.Find(y => y.name == x), false, true)).ToList();
                CloseWindow("CraftingSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By skill up", "Black"),
            (h) =>
            {
                currentSave.player.learnedRecipes[profession.name] = currentSave.player.learnedRecipes[profession.name].OrderByDescending(x => recipes.Find(y => y.name == x).skillUpYellow).ToList();
                CloseWindow("CraftingSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("CraftingSettings", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Recipe list settings:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("CraftingSettings");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Has materials", "Black");
                AddCheckbox(settings.onlyHavingMaterials);
            },
            (h) =>
            {
                settings.onlyHavingMaterials.Invert();
                CDesktop.RespawnAll();
            });
            AddButtonRegion(() =>
            {
                AddLine("Has skill up", "Black");
                AddCheckbox(settings.onlySkillUp);
            },
            (h) =>
            {
                settings.onlySkillUp.Invert();
                CDesktop.RespawnAll();
            });
        }),
        new("EnchantingList", () => {
            SetAnchor(TopLeft, 19, -38);
            var possibleItems = currentSave.player.inventory.items.Concat(currentSave.player.equipment.Select(x => x.Value)).Where(x => x.type == enchant.type).OrderBy(x => x.name).ToList();
            AddRegionGroup(() => possibleItems.Count, 12);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                AddLine("Possible items:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("EnchantingList");
                    Respawn("CraftingList");
                });
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            for (int i = 0; i < 12; i++)
            {
                var index = i;
                if (possibleItems.Count > index + 12 * regionGroup.pagination())
                    AddButtonRegion(() =>
                    {
                        var item = possibleItems[index + 12 * regionGroup.pagination()];
                        AddLine(item.name);
                        AddSmallButton(item.icon, null, null,
                        (h) => () =>
                        {
                            if (WindowUp("CraftingSort")) return;
                            if (WindowUp("CraftingSettings")) return;
                            PrintItemTooltip(item, Input.GetKey(LeftShift));
                        });
                    },
                    (h) =>
                    {
                        var item = possibleItems[index + 12 * regionGroup.pagination()];
                        enchantmentTarget = item;
                        PlaySound("DesktopEnchantingTarget");
                        CloseWindow("EnchantingList");
                        Respawn("CraftingRecipe");
                        Respawn("CraftingList");
                    });
                else if (possibleItems.Count == index + 12 * regionGroup.pagination())
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
            }
            AddPaginationLine(regionGroup);
        }),

        //Quest Log
        new("QuestList", () => {
            SetAnchor(TopLeft, 19, -38);
            var quests = currentSave.player.currentQuests;
            AddRegionGroup(() => quests.Count, 11);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() => AddLine("Quest Log:"));
            AddHeaderRegion(() =>
            {
                AddLine("Completed quests: ", "DarkGray");
                AddText("" + currentSave.player.completedQuests.Count, "Gray");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Current quests: ", "DarkGray");
                AddText("" + currentSave.player.currentQuests.Count, "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.player.currentQuests.Reverse();
                    CloseWindow("QuestList");
                    Respawn("QuestList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!WindowUp("QuestConfirmAbandon") && !WindowUp("QuestSettings") && !WindowUp("QuestSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("QuestSort");
                        CloseWindow("QuestList");
                        Respawn("Quest", true);
                        Respawn("QuestList");
                    });
                else
                    AddSmallButton("OtherSortOff");
                if (!WindowUp("QuestConfirmAbandon") && !WindowUp("QuestSettings") && !WindowUp("QuestSort"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("QuestSettings");
                        CloseWindow("QuestList");
                        Respawn("Quest", true);
                        Respawn("QuestList");
                    });
                else
                    AddSmallButton("OtherSettingsOff");
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            for (int i = 0; i < 11; i++)
            {
                var index = i;
                if (quests.Count > index + 11 * regionGroup.pagination())
                {
                    AddButtonRegion(() =>
                    {
                        var quest = quests[index + 11 * regionGroup.pagination()];
                        AddLine((settings.questLevel.Value() ? "[" + quest.questLevel + "] " : "") + quest.name, "Black");
                        AddSmallButton(quest.ZoneIcon());
                    },
                    (h) =>
                    {
                        quest = quests[index + 11 * regionGroup.pagination()];
                        if (staticPagination.ContainsKey("Quest"))
                            staticPagination.Remove("Quest");
                        Respawn("Quest");
                        PlaySound("DesktopInstanceOpen");
                    });
                    var color = ColorQuestLevel(quests[index + 11 * regionGroup.pagination()].questLevel);
                    if (color != null) SetRegionBackgroundAsImage("SkillUp" + color);
                }
                else if (quests.Count == index + 11 * regionGroup.pagination())
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
            }
            AddPaginationLine(regionGroup);
        }),
        new("Quest", () => {
            SetAnchor(TopRight, -19, -38);
            quest.Print();
        }),
        new("QuestAdd", () => {
            SetAnchor(TopRight, -19, -38);
            quest.Print("Add");
        }),
        new("QuestTurn", () => {
            SetAnchor(TopRight, -19, -38);
            quest.Print("Turn");
        }),
        new("QuestConfirmAbandon", () => {
            SetAnchor(-92, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(182);
            AddPaddingRegion(() =>
            {
                AddLine("You are about to abandon", "", "Center");
                AddLine(quest.name, "", "Center");
            });
            AddRegionGroup();
            SetRegionGroupWidth(91);
            AddButtonRegion(() =>
            {
                SetRegionBackground(ButtonRed);
                AddLine("Proceed", "", "Center");
            },
            (h) =>
            {
                PlaySound("DesktopMenuClose");
                PlaySound("QuestFailed", 0.4f);
                currentSave.player.RemoveQuest(quest);
                quest = null;
                CloseWindow("QuestConfirmAbandon");
                CloseWindow("Quest");
                Respawn("QuestList");
            });
            AddRegionGroup();
            SetRegionGroupWidth(91);
            AddButtonRegion(() => AddLine("Cancel", "", "Center"),
            (h) =>
            {
                PlaySound("DesktopMenuClose");
                CloseWindow("QuestConfirmAbandon");
                Respawn("Quest");
                Respawn("QuestList");
            });
        }, true),
        new("QuestSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort quests:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("QuestSort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() => AddLine("By name", "Black"),
            (h) =>
            {
                currentSave.player.currentQuests = currentSave.player.currentQuests.OrderBy(x => x.name).ToList();
                CloseWindow("QuestSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By quest level", "Black"),
            (h) =>
            {
                currentSave.player.currentQuests = currentSave.player.currentQuests.OrderBy(x => x.questLevel).ToList();
                CloseWindow("QuestSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By zone", "Black"),
            (h) =>
            {
                currentSave.player.currentQuests = currentSave.player.currentQuests.OrderBy(x => x.zone).ToList();
                CloseWindow("QuestSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("QuestSettings", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Quest Log settings:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("QuestSettings");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Show quest level", "Black");
                AddCheckbox(settings.questLevel);
            },
            (h) =>
            {
                settings.questLevel.Invert();
                CDesktop.RespawnAll();
            });
        }),

        //Inventory
        new("PlayerEquipmentInfo", () => {
            if (CDesktop.title == "Map") return;
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(252);
            AddHeaderRegion(() => AddLine("Equipment:"));
            Foo("Head", currentSave.player.GetItemInSlot("Head"));
            Foo("Shoulders", currentSave.player.GetItemInSlot("Shoulders"));
            Foo("Back", currentSave.player.GetItemInSlot("Back"));
            Foo("Chest", currentSave.player.GetItemInSlot("Chest"));
            Foo("Wrists", currentSave.player.GetItemInSlot("Wrists"));
            Foo("Hands", currentSave.player.GetItemInSlot("Hands"));
            Foo("Waist", currentSave.player.GetItemInSlot("Waist"));
            Foo("Legs", currentSave.player.GetItemInSlot("Legs"));
            Foo("Feet", currentSave.player.GetItemInSlot("Feet"));
            Foo("Main Hand", currentSave.player.GetItemInSlot("Main Hand"));
            bool showOff = currentSave.player.GetItemInSlot("Main Hand") == null || currentSave.player.GetItemInSlot("Main Hand") != null && currentSave.player.GetItemInSlot("Main Hand").type != "Two Handed";
            if (showOff) Foo("Off Hand", currentSave.player.GetItemInSlot("Off Hand"));
            Foo("Neck", currentSave.player.GetItemInSlot("Neck"));
            Foo("Finger", currentSave.player.GetItemInSlot("Finger"));
            Foo("Trinket", currentSave.player.GetItemInSlot("Trinket"));
            if (!showOff) AddPaddingRegion(() => { AddLine(""); });
            //if (currentSave.player.spec == "Druid")
            //    Foo("Idol", currentSave.player.GetItemInSlot("Special"));
            //if (currentSave.player.spec == "Paladin")
            //    Foo("Libram", currentSave.player.GetItemInSlot("Special"));
            //if (currentSave.player.spec == "Shaman")
            //    Foo("Totem", currentSave.player.GetItemInSlot("Special"));
            //AddPaddingRegion(() => { AddLine(); AddLine(); AddLine(); AddLine(); });

            void Foo(string slot, Item item)
            {
                if (item != null)
                    AddHeaderRegion(
                        () =>
                        {
                            AddLine(item.name, item.rarity, "Right");
                            AddSmallButton(item.icon,
                            (h) =>
                            {
                                if (Cursor.cursor.color == "Pink") return;
                                if (WindowUp("Inventory") && currentSave.player.inventory.CanAddItem(currentSave.player.equipment[slot]))
                                {
                                    PlaySound(item.ItemSound("PutDown"), 0.8f);
                                    currentSave.player.Unequip(new() { slot });
                                    Respawn("PlayerEquipmentInfo");
                                    Respawn("Inventory");
                                }
                            },
                            (h) =>
                            {
                                if (item.CanUse(currentSave.player))
                                {
                                    PlaySound(item.ItemSound("Use"), 0.8f);
                                    item.Use(currentSave.player);
                                    Respawn("Inventory", true);
                                    Respawn("PlayerEquipmentInfo", true);
                                }
                            },
                            (h) => () =>
                            {
                                if (WindowUp("Inventory"))
                                    PrintItemTooltip(item);
                            });
                            if (settings.rarityIndicators.Value())
                                AddSmallButtonOverlay("OtherRarity" + item.rarity, 0, 2);
                            if (Cursor.cursor.color == "Pink")
                                if (!item.IsDisenchantable()) SetSmallButtonToGrayscale();
                                else SetSmallButtonToRed();
                        }
                    );
                else
                    AddPaddingRegion(() =>
                    {
                        AddLine(slot, "DarkGray", "Right");
                        AddSmallButton("OtherEmpty");
                    });
            }
        }),
        new("Inventory", () => {
            if (CDesktop.title == "Map") return;
            SetAnchor(TopRight, -19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var items = currentSave.player.inventory.items;
            AddHeaderRegion(() =>
            {
                AddLine("Inventory:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.player.inventory.items.Reverse();
                    CloseWindow("Inventory");
                    SpawnWindowBlueprint("Inventory");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!WindowUp("ConfirmItemDestroy") && !WindowUp("ConfirmItemDisenchant") && !WindowUp("InventorySettings") && !WindowUp("BankSort") && !WindowUp("VendorSort") && !WindowUp("InventorySort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("InventorySort");
                        Respawn("Inventory");
                        Respawn("Bank", true);
                        Respawn("ExperienceBarBorder", true);
                        Respawn("ExperienceBar", true);
                    });
                else
                    AddSmallButton("OtherSortOff");
                if (!WindowUp("ConfirmItemDestroy") && !WindowUp("ConfirmItemDisenchant") && !WindowUp("InventorySettings") && !WindowUp("BankSort") && !WindowUp("VendorSort") && !WindowUp("InventorySort"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("InventorySettings");
                        Respawn("Inventory");
                        Respawn("Bank", true);
                        Respawn("ExperienceBarBorder", true);
                        Respawn("ExperienceBar", true);
                    });
                else
                    AddSmallButton("OtherSettingsOff");
            });
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (index * 5 + j >= currentSave.player.inventory.BagSpace()) AddBigButton("OtherDisabled");
                            else if (items.Count > index * 5 + j) PrintInventoryItem(items[index * 5 + j]);
                            else AddBigButton("OtherEmpty");
                    }
                );
            }
            AddHeaderRegion(() =>
            {
                AddLine("Bags:");
                for (int i = 0; i < defines.maxBagsEquipped; i++)
                {
                    var index = i;
                    if (currentSave.player.inventory.bags.Count > index)
                        AddSmallButton(currentSave.player.inventory.bags[index].icon,
                            (h) =>
                            {
                                if (currentSave.player.inventory.items.Count < currentSave.player.inventory.BagSpace() - currentSave.player.inventory.bags[index].bagSpace)
                                    currentSave.player.UnequipBag(index);
                            },
                            null,
                            (h) => () => PrintItemTooltip(currentSave.player.inventory.bags[index]));
                    else AddSmallButton("OtherEmpty");
                }
            });
            PrintPriceRegion(currentSave.player.inventory.money);
        }, true),
        new("InventorySort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort inventory:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("InventorySort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() => AddLine("By name", "Black"),
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderBy(x => x.name).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By amount", "Black"),
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderBy(x => x.amount).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By rarity", "Black"),
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.rarity == "Poor" ? 0 : (x.rarity == "Common" ? 1 : (x.rarity == "Uncommon" ? 2 : (x.rarity == "Rare" ? 3 : (x.rarity == "Epic" ? 4 : 5))))).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By item power", "Black"),
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.ilvl).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By price", "Black"),
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.price).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By type", "Black"),
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.type).ToList();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("InventorySettings", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Inventory settings:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("InventorySettings");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Rarity indicators", "Black");
                AddCheckbox(settings.rarityIndicators);
            },
            (h) =>
            {
                settings.rarityIndicators.Invert();
                CDesktop.RespawnAll();
            });
            if (settings.rarityIndicators.Value())
                AddButtonRegion(() =>
                {
                    AddLine("Big Rarity indicators", "Black");
                    AddCheckbox(settings.bigRarityIndicators);
                },
                (h) =>
                {
                    settings.bigRarityIndicators.Invert();
                    CDesktop.RespawnAll();
                });
            AddButtonRegion(() =>
            {
                AddLine("Upgrade indicators", "Black");
                AddCheckbox(settings.upgradeIndicators);
            },
            (h) =>
            {
                settings.upgradeIndicators.Invert();
                CDesktop.RespawnAll();
            });
            AddButtonRegion(() =>
            {
                AddLine("New slot indicators", "Black");
                AddCheckbox(settings.newSlotIndicators);
            },
            (h) =>
            {
                settings.newSlotIndicators.Invert();
                CDesktop.RespawnAll();
            });
        }),
        new("ConfirmItemDestroy", () => {
            SetAnchor(-92, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(182);
            AddPaddingRegion(() =>
            {
                AddLine("You are about to destroy", "", "Center");
                AddLine(itemToDestroy.name, itemToDestroy.rarity, "Center");
            });
            AddRegionGroup();
            SetRegionGroupWidth(91);
            AddButtonRegion(() =>
            {
                SetRegionBackground(ButtonRed);
                AddLine("Proceed", "", "Center");
            },
            (h) =>
            {
                if (currentSave.player.uniquesGotten.Contains(itemToDestroy.name))
                    currentSave.player.uniquesGotten.Remove(itemToDestroy.name);
                currentSave.player.inventory.items.Remove(itemToDestroy);
                PlaySound("DesktopMenuClose");
                CloseWindow("ConfirmItemDestroy");
                itemToDestroy = null;
                Respawn("Inventory");
            });
            AddRegionGroup();
            SetRegionGroupWidth(91);
            AddButtonRegion(() => AddLine("Cancel", "", "Center"),
            (h) =>
            {
                PlaySound("DesktopMenuClose");
                CloseWindow("ConfirmItemDestroy");
                itemToDestroy = null;
                Respawn("Inventory");
            });
        }, true),
        new("ConfirmItemDisenchant", () => {
            SetAnchor(-92, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(182);
            AddPaddingRegion(() =>
            {
                AddLine("You are about to disenchant", "", "Center");
                AddLine(itemToDisenchant.name, itemToDisenchant.rarity, "Center");
            });
            AddRegionGroup();
            SetRegionGroupWidth(91);
            AddButtonRegion(() =>
            {
                SetRegionBackground(ButtonRed);
                AddLine("Proceed", "", "Center");
            },
            (h) =>
            {
                PlaySound("Disenchant");
                currentSave.AddTime(30);
                currentSave.player.inventory.items.Remove(itemToDisenchant);
                CloseWindow("ConfirmItemDisenchant");
                Respawn("Inventory");
                disenchantLoot = itemToDisenchant.GenerateDisenchantLoot();
                SpawnDesktopBlueprint("DisenchantLoot");
            });
            AddRegionGroup();
            SetRegionGroupWidth(91);
            AddButtonRegion(() => AddLine("Cancel", "", "Center"),
            (h) =>
            {
                PlaySound("DesktopMenuClose");
                CloseWindow("ConfirmItemDisenchant");
                Respawn("Inventory");
            });
        }, true),
        new("SplitItem", () => {
            SetAnchor(-92, 142);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() => AddLine("Enter amount to pick up:"));
            AddPaddingRegion(() => AddInputLine(String.splitAmount));
        }, true),
        new("ContainerLoot", () => {
            SetAnchor(-92, -105);
            AddRegionGroup();
            SetRegionGroupHeight(34);
            SetRegionGroupWidth(182);
            AddPaddingRegion(
                () =>
                {
                    for (int j = 0; j < 4 && j < openedItem.itemsInside.Count; j++)
                        PrintLootItem(openedItem.itemsInside[j]);
                }
            );
        }),

        //Combat Results
        new("CombatResults", () => {
            SetAnchor(Center, 0, 11);
            AddRegionGroup();
            SetRegionGroupWidth(262);
            SetRegionGroupHeight(113);
            AddHeaderRegion(() =>
            {
                AddLine("Combat Results", "", "Center");
                if (board.results.result == "Team1Won")
                    AddSmallButton("OtherClose", (h) =>
                    {
                        var hard = Realm.realms.Find(x => x.name == settings.selectedRealm).hardcore;
                        if (hard && board.results.result == "Team2Won")
                        {
                            CloseSave();
                            SaveGames();
                            CloseDesktop("CombatResults");
                            CloseDesktop("Map");
                            CloseDesktop("TitleScreen");
                            SpawnDesktopBlueprint("TitleScreen");
                        }
                        else
                        {
                            if (area.instancePart)
                            {
                                CloseDesktop("Instance");
                                SpawnDesktopBlueprint("Instance");
                                Respawn("HostileArea");
                                Respawn("HostileAreaProgress");
                                Respawn("HostileAreaDenizens");
                                Respawn("HostileAreaElites");
                                Respawn("Chest");
                                SetDesktopBackground(area.Background());
                            }
                            else
                            {
                                CloseDesktop("HostileArea");
                                SpawnDesktopBlueprint("HostileArea");
                            }
                            CloseDesktop("CombatResults");
                        }
                    });
            });
            AddPaddingRegion(() =>
            {
                foreach (var result in board.results.experience)
                    if (result.Value > 0)
                    {
                        AddLine(result.Key.name, "", "Center");
                        if (result.Key.name.Length > 15) AddLine(" earned " + result.Value + " experience", "", "Center");
                        else AddText(" earned " + result.Value + " experience");
                    }
                    else
                    {
                        AddLine(result.Key.name, "", "Center");
                        if (result.Key.name.Length > 15) AddLine(" earned no experience", "", "Center");
                        else AddText(" earned no experience");
                    }
                SetRegionAsGroupExtender();
            });
            AddButtonRegion(() =>
            {
                if (board.results.result == "Team1Won")
                {
                    if (board.results.inventory.items.Count > 0) AddLine("Show Loot", "", "Center");
                    else AddLine("OK", "", "Center");
                }
                else if (Realm.realms.Find(x => x.name == settings.selectedRealm).hardcore)
                {
                    SetRegionBackground(ButtonRed);
                    AddLine("Game Over", "", "Center");
                }
                else AddLine("Release Spirit", "", "Center");
            },
            (h) =>
            {
                var hardcore = Realm.realms.Find(x => x.name == settings.selectedRealm).hardcore;
                if (hardcore && board.results.result == "Team2Won")
                {
                    CloseSave();
                    SaveGames();
                    CloseDesktop("Map");
                    CloseDesktop("Complex");
                    CloseDesktop("Instance");
                    CloseDesktop("HostileArea");
                    CloseDesktop("CombatResults");
                    SpawnDesktopBlueprint("TitleScreen");
                }
                else
                {
                    if (board.results.result == "Team1Won")
                    {
                        if (board.area.instancePart)
                        {
                            CloseDesktop("Instance");
                            SpawnDesktopBlueprint("Instance");
                            Respawn("HostileArea");
                            Respawn("HostileAreaProgress");
                            Respawn("HostileAreaDenizens");
                            Respawn("HostileAreaElites");
                            Respawn("Chest");
                            SetDesktopBackground(area.Background());
                        }
                        else
                        {
                            CloseDesktop("HostileArea");
                            SpawnDesktopBlueprint("HostileArea");
                        }
                        if (board.results.inventory.items.Count > 0)
                        {
                            PlaySound("DesktopInventoryOpen");
                            SpawnDesktopBlueprint("CombatResultsLoot");
                        }
                        else CloseDesktop("CombatResults");
                    }
                    else
                    {
                        CloseDesktop("CombatResults");
                        if (board.results.result == "Team2Won")
                        {
                            if (area.instancePart) CloseDesktop("Instance");
                            else CloseDesktop("HostileArea");
                            var curr = FindSite(x => x.name == currentSave.currentSite);
                            var vect = new Vector2(curr.x, curr.y);
                            var distances = SiteSpiritHealer.spiritHealers.Select(x => (x, Vector2.Distance(new Vector2(x.x, x.y), vect))).OrderBy(x => x.Item2).ToList();
                            var sites = distances.Select(y => FindSite(x => x.name == y.x.name)).ToList();
                            var top = sites.Take(5).OrderBy(x => FindPath(x, curr, true).Count).ToList();
                            distances.Find(x => x.x.name == top[0].name).x.QueueSiteOpen("SpiritHealer");
                        }
                    }
                }
            });
        }),
        new("CombatResultsLoot", () => {
            SetAnchor(-92, -105);
            AddRegionGroup();
            SetRegionGroupHeight(34);
            SetRegionGroupWidth(182);
            AddPaddingRegion(
                () =>
                {
                    for (int j = 0; j < 4 && j < board.results.inventory.items.Count; j++)
                        PrintLootItem(board.results.inventory.items[j]);
                }
            );
        }),
        new("CombatResultsSkinning", () => {
            if (board.results.result != "Team1Won") return;
            if (board.results.skinningNode.Item1 == null) return;
            if (board.results.skinningLoot.items.Count == 0) return;
            SetAnchor(Bottom, 0, 35);
            AddRegionGroup();
            SetRegionGroupWidth(186);
            AddHeaderRegion(() =>
            {
                AddLine("Skinning");
                AddSmallButton("TradeSkinning");
            });
            var can = currentSave.player.professionSkills.ContainsKey("Skinning") && board.results.skinningNode.Item2 <= currentSave.player.professionSkills["Skinning"].Item1;
            AddPaddingRegion(() =>
            {
                var drop = GeneralDrop.generalDrops.Find(x => x.category == board.results.skinningNode.Item1 && x.tags.Contains("Main"));
                var item = items.Find(x => x.name == drop.item);
                AddLine(item.name);
                AddLine("Required skill: ", "DarkGray");
                AddText("" + board.results.skinningNode.Item2, can ? "Gray" : "DangerousRed");
                AddBigButton(item.icon);
            });
            if (can)
                AddButtonRegion(() => AddLine("Gather"),
                (h) =>
                {
                    PlaySound("SkinGather" + random.Next(1, 4));
                    SpawnDesktopBlueprint("SkinningLoot");
                });
            else AddPaddingRegion(() => AddLine("Gather", "DarkGray"));
        }),
        new("CombatResultsMining", () => {
            if (board.results.result != "Team1Won") return;
            if (board.results.miningNode.Item1 == null) return;
            if (board.results.miningLoot.items.Count == 0) return;
            SetAnchor(BottomLeft, 19, 35);
            AddRegionGroup();
            SetRegionGroupWidth(188);
            AddHeaderRegion(() =>
            {
                AddLine("Mining");
                AddSmallButton("TradeMining");
            });
            var can = currentSave.player.professionSkills.ContainsKey("Mining") && board.results.miningNode.Item2 <= currentSave.player.professionSkills["Mining"].Item1;
            AddPaddingRegion(() =>
            {
                AddLine(board.results.miningNode.Item1);
                AddLine("Required skill: ", "DarkGray");
                AddText("" + board.results.miningNode.Item2, can ? "Gray" : "DangerousRed");
                AddBigButton("MiningNode" + board.results.miningNode.Item1.Clean());
            });
            if (can)
            {
                AddButtonRegion(() => AddLine("Gather"),
                (h) =>
                {
                    PlaySound("VeinCrack" + random.Next(1, 4), 0.6f);
                    SpawnDesktopBlueprint("MiningLoot");
                });
            }
            else AddPaddingRegion(() => AddLine("Gather", "DarkGray"));
        }),
        new("MiningLoot", () => {
            SetAnchor(-92, -105);
            AddRegionGroup();
            SetRegionGroupHeight(34);
            SetRegionGroupWidth(182);
            AddPaddingRegion(
                () =>
                {
                    for (int j = 0; j < 4 && j < board.results.miningLoot.items.Count; j++)
                        PrintLootItem(board.results.miningLoot.items[j]);
                }
            );
        }),
        new("CombatResultsHerbalism", () => {
            if (board.results.result != "Team1Won") return;
            if (board.results.herb.Item1 == null) return;
            if (board.results.herbalismLoot.items.Count == 0) return;
            SetAnchor(BottomRight, -19, 35);
            AddRegionGroup();
            SetRegionGroupWidth(188);
            AddHeaderRegion(() =>
            {
                AddLine("Herbalism");
                AddSmallButton("TradeHerbalism");
            });
            var can = currentSave.player.professionSkills.ContainsKey("Herbalism") && board.results.herb.Item2 <= currentSave.player.professionSkills["Herbalism"].Item1;
            AddPaddingRegion(() =>
            {
                AddLine(board.results.herb.Item1);
                AddLine("Required skill: ", "DarkGray");
                AddText("" + board.results.herb.Item2, can ? "Gray" : "DangerousRed");
                AddBigButton("Herb" + board.results.herb.Item1.Clean());
            });
            if (can)
            {
                AddButtonRegion(() => AddLine("Gather"),
                (h) =>
                {
                    PlaySound("HerbGather" + random.Next(1, 5));
                    SpawnDesktopBlueprint("HerbalismLoot");
                });
            }
            else AddPaddingRegion(() => AddLine("Gather", "DarkGray"));
        }),
        new("HerbalismLoot", () => {
            SetAnchor(-92, -105);
            AddRegionGroup();
            SetRegionGroupHeight(34);
            SetRegionGroupWidth(182);
            AddPaddingRegion(
                () =>
                {
                    AddLine("");
                    for (int j = 0; j < 4 && j < board.results.herbalismLoot.items.Count; j++)
                        PrintLootItem(board.results.herbalismLoot.items[j]);
                }
            );
        }),
        new("SkinningLoot", () => {
            SetAnchor(-92, -105);
            AddRegionGroup();
            SetRegionGroupHeight(34);
            SetRegionGroupWidth(182);
            AddPaddingRegion(
                () =>
                {
                    for (int j = 0; j < 4 && j < board.results.skinningLoot.items.Count; j++)
                        PrintLootItem(board.results.skinningLoot.items[j]);
                }
            );
        }),
        new("DisenchantLoot", () => {
            SetAnchor(-92, -105);
            AddRegionGroup();
            SetRegionGroupHeight(34);
            SetRegionGroupWidth(182);
            AddPaddingRegion(
                () =>
                {
                    for (int j = 0; j < 4 && j < disenchantLoot.items.Count; j++)
                        PrintLootItem(disenchantLoot.items[j]);
                }
            );
        }),
        new("CombatResultsChartButton", () => {
            SetAnchor(-132, 69);
            DisableShadows();
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddSmallButton("OtherChart", (h) => { PlaySound("DesktopInstanceOpen"); SpawnDesktopBlueprint("CombatLog"); });
            });
        }, true),
        new("CombatResultsChart", () => {
            SetAnchor(-301, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(600);
            AddHeaderRegion(() =>
            {
                AddLine("Combat Log", "", "Center");
                AddSmallButton("OtherClose", (h) => { PlaySound("DesktopInstanceClose"); CloseDesktop("CombatLog"); });
                AddSmallButton(settings.chartBigIcons.Value() ? "OtherSmaller" : "OtherBigger",
                    (h) =>
                    {
                        settings.chartBigIcons.Invert();
                        PlaySound("DesktopChartSwitch");
                        CloseDesktop("CombatLog");
                        SpawnDesktopBlueprint("CombatLog");
                    });
            });
            AddPaddingRegion(() => { AddLine(chartPage, "", "Center"); });
            AddChart();
        }),
        new("CombatResultsChartLeftArrow", () => {
            DisableShadows();
            SetAnchor(-301, 123);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddSmallButton("OtherPreviousPage", (h) =>
                {
                    PlaySound("DesktopChartSwitch");
                    if (chartPage == "Damage Taken") chartPage = "Damage Dealt";
                    else if (chartPage == "Healing Received") chartPage = "Damage Taken";
                    else if (chartPage == "Elements Used") chartPage = "Healing Received";
                    else if (chartPage == "Damage Dealt") chartPage = "Elements Used";
                    CloseDesktop("CombatLog");
                    SpawnDesktopBlueprint("CombatLog");
                });
            });
        }, true),
        new("CombatResultsChartRightArrow", () => {
            DisableShadows();
            SetAnchor(280, 123);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddSmallButton("OtherNextPage", (h) =>
                {
                    PlaySound("DesktopChartSwitch");
                    if (chartPage == "Damage Dealt") chartPage = "Damage Taken";
                    else if (chartPage == "Damage Taken") chartPage = "Healing Received";
                    else if (chartPage == "Healing Received") chartPage = "Elements Used";
                    else if (chartPage == "Elements Used") chartPage = "Damage Dealt";
                    CloseDesktop("CombatLog");
                    SpawnDesktopBlueprint("CombatLog");
                });
            });
        }, true),
        new("LootInfo", () => {
            SetAnchor(-92, -86);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(
                () =>
                {
                    if (CDesktop.title == "MiningLoot")
                    {
                        AddLine(board.results.miningNode.Item1 + ":");
                        AddSmallButton("OtherClose", (h) =>
                        {
                            PlaySound("DesktopInventoryClose");
                            CloseDesktop("MiningLoot");
                            Respawn("CombatResultsMining");
                        });
                    }
                    else if (CDesktop.title == "HerbalismLoot")
                    {
                        AddLine(board.results.herb.Item1 + ":");
                        AddSmallButton("OtherClose", (h) =>
                        {
                            PlaySound("DesktopInventoryClose");
                            CloseDesktop("HerbalismLoot");
                            Respawn("CombatResultsHerbalism");
                        });
                    }
                    else if (CDesktop.title == "SkinningLoot")
                    {
                        AddLine(board.participants[1].who.name + ":");
                        AddSmallButton("OtherClose", (h) =>
                        {
                            PlaySound("DesktopInventoryClose");
                            CloseDesktop("SkinningLoot");
                            Respawn("CombatResultsSkinning");
                        });
                    }
                    else if (CDesktop.title == "ContainerLoot")
                    {
                        AddLine(openedItem.name + ":");
                        AddSmallButton("OtherClose", (h) =>
                        {
                            if (openedItem.itemsInside.Count == 0)
                                currentSave.player.inventory.items.Remove(openedItem);
                            openedItem = null;
                            PlaySound("DesktopInventoryClose");
                            CloseDesktop("ContainerLoot");
                        });
                    }
                    else if (CDesktop.title == "DisenchantLoot")
                    {
                        AddLine("Disenchant spoils" + ":");
                        AddSmallButton("OtherClose", (h) =>
                        {
                            disenchantLoot = null;
                            enchantingSkillChange = false;
                            CloseDesktop("DisenchantLoot");
                        });
                    }
                    else
                    {
                        AddLine(board.participants[1].who.name + ":");
                        AddSmallButton("OtherClose", (h) =>
                        {
                            PlaySound("DesktopInventoryClose");
                            CloseDesktop("CombatResultsLoot");
                            SwitchDesktop("CombatResults");
                            Respawn("CombatResults");
                        });
                    }
                }
            );
        }),
        
        //Complex
        new("Complex", () => 
        {
            if (complex.ambience == null)
            {
                var zone = zones.Find(x => x.name == complex.zone);
                if (zone != null) PlayAmbience(currentSave.IsNight() ? zone.ambienceNight : zone.ambienceDay);
            }
            else PlayAmbience(complex.ambience);
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine(complex.name, "Gray");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    var title = CDesktop.title;
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop(title);
                    SwitchDesktop("Map");
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Level range: ", "DarkGray");
                var range = (0, 0);
                var areas = complex.sites.Where(x => x["SiteType"] == "HostileArea").Select(x => SiteHostileArea.areas.Find(y => y.name == x["SiteName"]).recommendedLevel).ToList();
                if (areas.Count > 0)
                {
                    var min = areas.Min(x => x);
                    var max = areas.Max(x => x);
                    if (range.Item1 < min) range = (min, range.Item2);
                    if (range.Item2 < max) range = (range.Item1, max);
                }
                var ranges = complex.sites.Where(x => x["SiteType"] == "Dungeon" || x["SiteType"] == "Raid").Select(x => instances.Find(y => y.name == x["SiteName"]).LevelRange()).ToList();
                if (ranges.Count > 0)
                {
                    var min = ranges.Min(x => x.Item1);
                    var max = ranges.Max(x => x.Item2);
                    if (range.Item1 < min) range = (min, range.Item2);
                    if (range.Item2 < max) range = (range.Item1, max);
                }
                AddText(range.Item1 + "", ColorEntityLevel(range.Item1));
                AddText(" - ", "DarkGray");
                AddText(range.Item2 + "", ColorEntityLevel(range.Item2));
            });
            foreach (var site in complex.sites)
                PrintComplexSite(site);
        }),
        new("ComplexQuestAvailable", () => 
        {
            var quests = currentSave.player.AvailableQuestsAt(complex).OrderBy(x => x.questLevel).ToList();
            if (quests.Count == 0) return;
            SetAnchor(Top, 0, -38);
            AddQuestList(quests);
        }),
        new("ComplexQuestDone", () => 
        {
            var quests = currentSave.player.QuestsDoneAt(complex).OrderBy(x => x.questLevel).ToList();
            if (quests.Count == 0) return;
            SetAnchor(Bottom, 0, 35);
            AddQuestList(quests, "Turn");
        }),

        //Instance
        new("Instance", () => 
        {
            if (instance.ambience == null)
            {
                var zone = zones.Find(x => x.name == instance.zone);
                if (zone != null) PlayAmbience(currentSave.IsNight() ? zone.ambienceNight : zone.ambienceDay);
            }
            else PlayAmbience(instance.ambience);
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine(instance.name, "Gray");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    var title = CDesktop.title;
                    CloseDesktop(title);
                    if (instance.complexPart) SpawnDesktopBlueprint("Complex");
                    else
                    {
                        PlaySound("DesktopInstanceClose");
                        SwitchDesktop("Map");
                    }
                    instance = null;
                    wing = null;
                });
            });
            if (WindowUp("InstanceWing")) return;
            AddPaddingRegion(() =>
            {
                AddLine("Level range: ", "DarkGray");
                var range = instance.LevelRange();
                AddText(range.Item1 + "", ColorEntityLevel(range.Item1));
                AddText(" - ", "DarkGray");
                AddText(range.Item2 + "", ColorEntityLevel(range.Item2));
            });
            if (instance.wings.Count == 1)
                for (int i = 0; i < instance.wings[0].areas.Count; i++)
                {
                    var index = i;
                    var area = instance.wings[0].areas[index];
                    var find = areas.Find(x => x.name == area["AreaName"]);
                    if (find != null && (showAreasUnconditional || area.ContainsKey("OpenByDefault") && area["OpenByDefault"] == "True" || currentSave.unlockedAreas.Contains(find.name)))
                        AddButtonRegion(() =>
                        {
                            AddLine(find.name);
                            if (currentSave.siteProgress.ContainsKey(find.name) && find.areaSize == currentSave.siteProgress[find.name])
                                SetRegionBackgroundAsImage("ClearedArea");
                        },
                        (h) =>
                        {
                            SiteHostileArea.area = find;
                            Respawn("HostileArea");
                            Respawn("HostileAreaProgress");
                            Respawn("HostileAreaDenizens");
                            Respawn("HostileAreaElites");
                            Respawn("Chest");
                            SetDesktopBackground(find.Background());
                        });
                    else AddHeaderRegion(() => AddLine("?", "DimGray"));
                }
            else
                for (int i = 0; i < instance.wings.Count; i++)
                {
                    var index = i;
                    var find = instance.wings[index];
                    if (showAreasUnconditional || find.areas.Any(x => x.ContainsKey("OpenByDefault") && x["OpenByDefault"] == "True" || currentSave.unlockedAreas.Contains(x["AreaName"])))
                        AddButtonRegion(() =>
                        {
                            AddLine(find.name);
                            var allAreas = areas.FindAll(x => find.areas.Exists(y => y["AreaName"] == x.name));
                            if (allAreas.All(x => currentSave.siteProgress.ContainsKey(x.name) && x.areaSize <= currentSave.siteProgress[x.name]))
                                SetRegionBackgroundAsImage("ClearedArea");
                        },
                        (h) =>
                        {
                            wing = find;
                            Respawn("InstanceWing");
                            Respawn("Instance");
                            SetDesktopBackground(wing.Background());
                        });
                    else AddHeaderRegion(() => AddLine("?", "DimGray"));
                }
        }),
        new("InstanceWing", () => 
        {
            SetAnchor(TopRight, -19, -57);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddPaddingRegion(() =>
            {
                AddLine(wing.name + ":", "Gray");
                AddSmallButton("OtherReverse",
                (h) =>
                {
                    wing = null;
                    area = null;
                    SetDesktopBackground(instance.Background());
                    CloseWindow("HostileArea");
                    CloseWindow("HostileAreaProgress");
                    CloseWindow("HostileAreaDenizens");
                    CloseWindow("HostileAreaElites");
                    CloseWindow("Chest");
                    CloseWindow("InstanceWing");
                    Respawn("Instance");
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Level range: ", "DarkGray");
                var range = instance.LevelRange(instance.wings.IndexOf(wing));
                AddText(range.Item1 + "", ColorEntityLevel(range.Item1));
                AddText(" - ", "DarkGray");
                AddText(range.Item2 + "", ColorEntityLevel(range.Item2));
            });
            for (int i = 0; i < wing.areas.Count; i++)
            {
                var index = i;
                var area = wing.areas[index];
                var find = areas.Find(x => x.name == area["AreaName"]);
                if (find != null && (showAreasUnconditional || area.ContainsKey("OpenByDefault") && area["OpenByDefault"] == "True" || currentSave.unlockedAreas.Contains(find.name)))
                    AddButtonRegion(() =>
                    {
                        AddLine(find.name);
                        if (currentSave.siteProgress.ContainsKey(find.name) && find.areaSize == currentSave.siteProgress[find.name])
                            SetRegionBackgroundAsImage("ClearedArea");
                    },
                    (h) =>
                    {
                        SiteHostileArea.area = find;
                        Respawn("HostileArea");
                        Respawn("HostileAreaProgress");
                        Respawn("HostileAreaDenizens");
                        Respawn("HostileAreaElites");
                        Respawn("Chest");
                        SetDesktopBackground(find.Background());
                    });
                else AddHeaderRegion(() => AddLine("?", "DimGray"));
            }
        }),
        new("InstanceQuestAvailable", () => 
        {
            var quests = currentSave.player.AvailableQuestsAt(instance).OrderBy(x => x.questLevel).ToList();
            if (quests.Count == 0) return;
            SetAnchor(Top, 0, -38);
            AddQuestList(quests);
        }),
        new("InstanceQuestDone", () => 
        {
            var quests = currentSave.player.QuestsDoneAt(instance).OrderBy(x => x.questLevel).ToList();
            if (quests.Count == 0) return;
            SetAnchor(Bottom, 0, 35);
            AddQuestList(quests, "Turn");
        }),

        //Hostile Area
        new("HostileArea", () => 
        {
            if (area.ambience == null)
            {
                var zone = zones.Find(x => x.name == area.zone);
                if (zone != null) PlayAmbience(currentSave.IsNight() ? zone.ambienceNight : zone.ambienceDay);
            }
            else PlayAmbience(area.ambience);
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() => 
            {
                AddLine(area.name, "Gray");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    PlaySound("DesktopInstanceClose");
                    if (area.instancePart)
                    {
                        SetDesktopBackground(instance.Background());
                        if (wing != null) SetDesktopBackground(wing.Background());
                        CloseWindow(h.window);
                        CloseWindow("HostileAreaProgress");
                        CloseWindow("HostileAreaDenizens");
                        CloseWindow("HostileAreaElites");
                        CloseWindow("Chest");
                    }
                    else if (area.complexPart)
                    {
                        SetDesktopBackground(complex.Background());
                        CloseWindow(h.window);
                        CloseWindow("HostileAreaProgress");
                        CloseWindow("HostileAreaDenizens");
                        CloseWindow("HostileAreaElites");
                        CloseWindow("Chest");
                    }
                    else CloseDesktop("HostileArea");
                });
                if (area.fishing)
                    AddSmallButton("OtherFish" + (!currentSave.player.professionSkills.ContainsKey("Fishing") ? "Off" : ""),
                    (h) =>
                    {
                        if (currentSave.player.professionSkills.ContainsKey("Fishing"))
                        {
                            fishingSpot = fishingSpots.Find(x => x.name == area.name);
                            SpawnDesktopBlueprint("FishingGame");
                        }
                    });
            });
            AddPaddingRegion(() => 
            {
                AddLine("Recommended level: ", "DarkGray");
                AddText(area.recommendedLevel + "", ColorEntityLevel(area.recommendedLevel));
                if (WindowUp("HostileAreaQuestTracker"))
                    AddSmallButton("OtherQuestClose", (h) => CloseWindow("HostileAreaQuestTracker"));
                else if (currentSave.player.QuestsAt(area).Count > 0)
                    AddSmallButton("OtherQuestOpen", (h) =>
                    {
                        CloseWindow("HostileAreaQuestAvailable");
                        Respawn("HostileAreaQuestTracker");
                    });
            });
            AddButtonRegion(() => { AddLine("Explore", "Black"); },
            (h) =>
            {
                NewBoard(area.RollEncounters(area.instancePart ? 2 : 1), area);
                SpawnDesktopBlueprint("Game");
            });
        }),
        new("HostileAreaQuestTracker", () => 
        {
            SetAnchor(Top, 0, -38);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            var q = currentSave.player.QuestsAt(area);
            foreach (var quest in q)
            {
                var con = quest.conditions.FindAll(x => !x.IsDone() && x.Where().Contains(area));
                AddButtonRegion(() =>
                {
                    AddLine(quest.name, "Black");
                    AddSmallButton(quest.ZoneIcon());
                },
                (h) =>
                {
                    SpawnDesktopBlueprint("QuestLog");
                    Quest.quest = quest;
                    if (staticPagination.ContainsKey("Quest"))
                        staticPagination.Remove("Quest");
                    Respawn("Quest");
                });
                var color = ColorQuestLevel(quest.questLevel);
                if (color != null) SetRegionBackgroundAsImage("SkillUp" + color);
                if (con.Count > 0)
                    foreach (var condition in con)
                        condition.Print(false);
            }
        }),
        new("HostileAreaProgress", () => 
        {
            SetAnchor(BottomLeft, 19, 35);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine("Exploration progress:", "Gray");
            });
            var thickness = 5;
            if (area.progression != null && area.progression.Count > 0)
                for (int i = 0; i <= area.areaSize; i++)
                {
                    var index = i;
                    if (index > 0)
                    {
                        var progressions = area.progression.FindAll(x => x.point == index);
                        var printType = "";
                        if (progressions.Exists(x => x.type == "Boss") && progressions.Exists(x => x.type == "Area" && x.all)) printType = "BossAreaAll";
                        else if (progressions.Exists(x => x.type == "Boss") && progressions.Exists(x => x.type == "Area")) printType = "BossArea";
                        else if (progressions.Exists(x => x.type == "Treasure") && progressions.Exists(x => x.type == "Area" && x.all)) printType = "TreasureAreaAll";
                        else if (progressions.Exists(x => x.type == "Treasure") && progressions.Exists(x => x.type == "Area")) printType = "TreasureArea";
                        else if (progressions.Exists(x => x.type == "Boss")) printType = "Boss";
                        else if (progressions.Exists(x => x.type == "Treasure")) printType = "Treasure";
                        else if (progressions.Exists(x => x.type == "Area" && x.all)) printType = "AreaAll";
                        else if (progressions.Exists(x => x.type == "Area")) printType = "Area";
                        if (printType != "")
                        {
                            var marker = new GameObject("ProgressionMarker", typeof(SpriteRenderer));
                            marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/Progress" + printType);
                            marker.transform.parent = CDesktop.LBWindow().LBRegionGroup().LBRegion().transform;
                            marker.transform.localPosition = new Vector3(1 + CDesktop.LBWindow().LBRegionGroup().setWidth, -3 - thickness);
                        }
                    }
                    if (i < area.areaSize)
                    {
                        AddRegionGroup();
                        SetRegionGroupWidth((i == area.areaSize - 1 ? 190 % area.areaSize : 0) + 190 / area.areaSize);
                        SetRegionGroupHeight(thickness);
                        AddPaddingRegion(() =>
                        {
                            var temp = currentSave.siteProgress.ContainsKey(area.name) ? currentSave.siteProgress[area.name] : 0;
                            if (temp > index) SetRegionBackground(ProgressDone);
                            else SetRegionBackground(ProgressEmpty);
                        });
                    }
                }
        }),
        new("HostileAreaDenizens", () => 
        {
            if (area.commonEncounters == null || area.commonEncounters.Count == 0) return;
            SetAnchor(TopLeft, 19, -95);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            foreach (var encounter in area.commonEncounters)
                AddHeaderRegion(() =>
                {
                    AddLine(encounter.who, "DarkGray", "Right");
                    var race = races.Find(x => x.name == encounter.who);
                    AddSmallButton(race == null ? "OtherUnknown" : race.portrait);
                    //AddSmallButtonOverlay("QuestMarkerOneSided");
                });
        }),
        new("HostileAreaElites", () => 
        {
            if (area.eliteEncounters == null || area.eliteEncounters.Count == 0) return;
            var boss = area.progression.Find(x => x.type == "Boss" && currentSave.siteProgress.ContainsKey(area.name) && x.point == currentSave.siteProgress[area.name]);
            if (boss == null) return;
            var bossName = boss.bossName == "<RingofLaw>" ? currentSave.ringOfLaw : (boss.bossName == "<ForlornCloister>" ? currentSave.forlornCloister : boss.bossName);
            if (boss == null || currentSave.elitesKilled.ContainsKey(bossName)) return;
            var encounter = area.eliteEncounters.Find(x => x.who == bossName);
            if (encounter == null) return;
            SetAnchor(BottomLeft, 19, 82);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddPaddingRegion(() =>
            {
                AddLine(encounter.who);
                AddLine("Level: ", "DarkGray");
                AddText("" + encounter.levelMin, ColorEntityLevel(encounter.levelMin));
                var race = races.Find(x => x.name == encounter.who);
                AddBigButton(race == null ? "OtherUnknown" : race.portrait,
                    (h) =>
                    {
                        NewBoard(new() { area.RollEncounter(encounter) }, area);
                        SpawnDesktopBlueprint("Game");
                    }
                );
            });
        }),
        new("HostileAreaQuestAvailable", () => 
        {
            var quests = currentSave.player.AvailableQuestsAt(area).OrderBy(x => x.questLevel).ToList();
            if (quests.Count == 0) return;
            SetAnchor(Top, 0, -38);
            AddQuestList(quests);
        }),
        new("HostileAreaQuestDone", () => 
        {
            var quests = currentSave.player.QuestsDoneAt(area).OrderBy(x => x.questLevel).ToList();
            if (quests.Count == 0) return;
            SetAnchor(Bottom, 0, 35);
            AddQuestList(quests, "Turn");
        }),

        //Town
        new("Town", () => 
        {
            PlayAmbience(town.ambience);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine(town.name, "Gray");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    var title = CDesktop.title;
                    CloseDesktop(title);
                    PlaySound("DesktopInstanceClose");
                    SwitchDesktop("Map");
                });
                if (town.fishing)
                    AddSmallButton("OtherFish" + (!currentSave.player.professionSkills.ContainsKey("Fishing") ? "Off" : ""),
                    (h) =>
                    {
                        if (currentSave.player.professionSkills.ContainsKey("Fishing"))
                        {
                            fishingSpot = fishingSpots.Find(x => x.name == town.name);
                            SpawnDesktopBlueprint("FishingGame");
                        }
                    });
            });
            if (WindowUp("Persons")) return;
            if (transportationConnectedToSite.ContainsKey(town.name))
            {
                var transportOptions = transportationConnectedToSite[town.name];
                AddPaddingRegion(() => { AddLine("Transportation:", "HalfGray"); });
                foreach (var transport in transportOptions)
                {
                    var desitnationName = transport.sites.Find(x => x != town.name);
                    var destination = towns.Find(x => x.name == desitnationName);
                    if (destination == null) continue;
                    AddButtonRegion(() =>
                    {
                        AddLine(desitnationName, "Black");
                        AddSmallButton("Transport" + transport.means);
                    },
                    (h) =>
                    {
                        if (transport.price > 0)
                        {
                            if (transport.price > currentSave.player.inventory.money) return;
                            PlaySound("DesktopTransportPay");
                            currentSave.player.inventory.money -= transport.price;
                        }

                        //Close town screen as we're beginning to travel on map
                        CloseDesktop("Town");

                        //Switch desktop to map
                        SwitchDesktop("Map");

                        //Lead path to the destination
                        LeadPath(transport, true);

                        //Queue moving player to the destination
                        destination.ExecutePath("Town");
                    },
                    null,
                    (h) => () => { transport.PrintTooltip(); });
                }
            }
            if (town.people != null)
            {
                var groups = town.people.Where(x => !x.hidden).OrderBy(x => x.type).GroupBy(x => x.category).OrderBy(x => x.Count()).ThenBy(x => x.Key != null ? x.Key.priority : 0);
                AddPaddingRegion(() => { AddLine("Points of interest:", "HalfGray"); });
                foreach (var group in groups)
                    if (group.Key == null) continue;
                    else if (group.Key.category == "Flight Master")
                        foreach (var person in group)
                        {
                            var faction = factions.Find(x => x.name == person.faction);
                            faction ??= factions.Find(x => x.name == races.Find(y => y.name == person.race).faction);
                            faction ??= factions.Find(x => x.name == currentSave.player.faction);
                            if (faction.side == currentSave.player.Side())
                            {
                                var personType = personTypes.Find(x => x.type == person.type);
                                AddButtonRegion(() =>
                                {
                                    AddLine(person.name, "Black");
                                    AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? faction.side : "") : "OtherUnknown");
                                },
                                (h) =>
                                {
                                    Person.person = person;
                                    CloseWindow(h.window.title);
                                    Respawn("Person");
                                    CloseWindow("QuestAdd");
                                    CloseWindow("QuestTurn");
                                    CloseWindow("TownQuestAvailable");
                                    CloseWindow("TownQuestDone");
                                    PlayVoiceLine(person.VoiceLine("Greeting"));
                                    PlaySound("DesktopInstanceOpen");
                                });
                            }
                        }
                    else if (group.Count() == 1)
                        foreach (var person in group)
                        {
                            var personType = personTypes.Find(x => x.type == person.type);
                            AddButtonRegion(() =>
                            {
                                AddLine(person.name, "Black");
                                AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? factions.Find(x => x.name == town.faction).side : "") : "OtherUnknown");
                            },
                            (h) =>
                            {
                                Person.person = person;
                                CloseWindow(h.window.title);
                                Respawn("Person");
                                CloseWindow("QuestAdd");
                                CloseWindow("QuestTurn");
                                CloseWindow("TownQuestAvailable");
                                CloseWindow("TownQuestDone");
                                PlayVoiceLine(person.VoiceLine("Greeting"));
                                PlaySound("DesktopInstanceOpen");
                            });
                        }
                    else
                    {
                        var person = group.First();
                        AddButtonRegion(() =>
                        {
                            AddLine(group.Key.category + "s (" + group.Count() + ")", "Black");
                            AddSmallButton(person.category != null ? person.category.icon + (person.category.factionVariant ? factions.Find(x => x.name == town.faction).side : "") : "OtherUnknown");
                        },
                        (h) =>
                        {
                            personCategory = group.Key;
                            CloseWindow("Person");
                            Respawn("Persons");
                            PlaySound("DesktopInstanceOpen");
                        });
                    }
            }
        }),
        new("TownHostile", () => 
        {
            PlayAmbience(town.ambience);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine(town.name);
                AddSmallButton("OtherClose",
                (h) =>
                {
                    var title = CDesktop.title;
                    CloseDesktop(title);
                    PlaySound("DesktopInstanceClose");
                    SwitchDesktop("Map");
                });
            });
            AddPaddingRegion(() =>
            {
                var rank = currentSave.player.ReputationRank(town.faction);
                if (rank == "Hated")
                {
                    AddLine("This town's folk consider you");
                    AddLine("to be their enemy");
                }
                else if (rank == "Hostile")
                {
                    AddLine("This town's folk consider you");
                    AddLine("to be an enemy");
                }
                else if (rank == "Unfriendly")
                {
                    AddLine("This town's folk are reluctant");
                    AddLine("towards you.");
                    AddLine("Consider improving your reputation");
                    AddLine("with " + town.faction + " in order");
                    AddLine("to be welcomed here");
                    AddLine("");
                }
            });
        }),
        new("TownQuestAvailable", () => 
        {
            var quests = currentSave.player.AvailableQuestsAt(town).OrderBy(x => x.questLevel).ToList();
            if (quests.Count == 0) return;
            SetAnchor(Top, 0, -38);
            AddQuestList(quests);
        }),
        new("TownQuestDone", () => 
        {
            var quests = currentSave.player.QuestsDoneAt(town).OrderBy(x => x.questLevel).ToList();
            if (quests.Count == 0) return;
            SetAnchor(Bottom, 0, 35);
            AddQuestList(quests, "Turn");
        }),
        new("Person", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var type = personTypes.Find(x => x.type == person.type);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""));
            });
            if (type.category == "Class Trainer")
            {
                if (person.type.ToLower().Contains(currentSave.player.spec.ToLower()))
                    AddButtonRegion(() =>
                    {
                        AddLine("I want to reset my talents");
                    },
                    (h) =>
                    {
                        PlaySound("DesktopInstanceOpen");
                        CloseWindow(h.window);
                        Respawn("ResetTalents");
                    });
            }
            else if (type.category == "Profession Trainer")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to learn the profession");
                },
                (h) =>
                {
                    PlaySound("DesktopInstanceOpen", 0.4f);
                    CloseWindow(h.window);
                    CloseWindow("Town");
                    Respawn("ProfessionLevelTrainer");
                });
                var pr = professions.Find(x => x.name == type.profession);
                if (pr == null) Debug.Log("ERROR 013: Profession was not found: \"" + type.profession + "\"");
                else
                {
                    var rt = professions.Find(x => x.name == type.profession).recipeType;
                    if (rt != null)
                        AddButtonRegion(() =>
                        {
                            AddLine("I would like to learn " + rt.ToLower() + (rt.Last() == 's' ? "" : "s"));
                        },
                        (h) =>
                        {
                            PlaySound("DesktopInstanceOpen", 0.4f);
                            CloseWindow(h.window);
                            CloseWindow("Town");
                            SpawnWindowBlueprint("ProfessionRecipeTrainer");
                            Respawn("ExperienceBarBorder");
                            Respawn("ExperienceBar");
                        });
                }
            }
            else if (type.category == "Banker")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to open my vault");
                },
                (h) =>
                {
                    if (!currentSave.banks.ContainsKey(town.name))
                        currentSave.banks.Add(town.name, new() { items = new() });
                    PlaySound("DesktopBankOpen", 0.4f);
                    CloseWindow(h.window);
                    CloseWindow("Town");
                    SpawnWindowBlueprint("Bank");
                    SpawnWindowBlueprint("Inventory");
                });
            }
            else if (type.category == "Auctioneer")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want browse the auctions");
                },
                (h) =>
                {
                    Market.exploredAuctionsGroups = new();
                    var foo = factions.Find(x => x.name == town.faction).side;
                    if (foo == "Neutral" || foo == "Horde")
                    {
                        var woo = currentSave.markets.Find(x => x.name == "Horde Market");
                        woo.UpdateMarket();
                        foreach (var type in woo.auctions.GroupBy(x => x.item.name).ToDictionary(x => x.Key, x => x.ToList()))
                            if (!Market.exploredAuctionsGroups.TryAdd(type.Key, type.Value))
                                Market.exploredAuctionsGroups[type.Key].AddRange(type.Value);
                    }
                    if (foo == "Neutral" || foo == "Alliance")
                    {
                        var woo = currentSave.markets.Find(x => x.name == "Alliance Market");
                        woo.UpdateMarket();
                        foreach (var type in woo.auctions.GroupBy(x => x.item.name).ToDictionary(x => x.Key, x => x.ToList()))
                            if (!Market.exploredAuctionsGroups.TryAdd(type.Key, type.Value))
                                Market.exploredAuctionsGroups[type.Key].AddRange(type.Value);
                    }
                    PlaySound("DesktopAuctionOpen", 0.4f);
                    SpawnWindowBlueprint("AuctionHouseOffersGroups");
                    CloseWindow(h.window);
                    CloseWindow("Town");
                });
            }
            else if (type.category == "Innkeeper")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to rest in this inn");
                });
                if (currentSave.player.homeLocation != town.name)
                    AddButtonRegion(() =>
                    {
                        AddLine("I want this inn to be my home");
                    },
                    (h) =>
                    {
                        PlaySound("DesktopInstanceOpen");
                        CloseWindow(h.window);
                        Respawn("MakeInnHome");
                    });
                if (!currentSave.player.inventory.items.Exists(x => x.name == "Hearthstone"))
                    AddButtonRegion(() =>
                    {
                        AddLine("I lost my hearthstone");
                    },
                    (h) =>
                    {
                        var item = items.Find(x => x.name == "Hearthstone");
                        if (currentSave.player.inventory.CanAddItem(item))
                        {
                            PlaySound(item.ItemSound("PickUp"));
                            currentSave.player.inventory.AddItem(item.CopyItem(1));
                        }
                        Respawn("ExperienceBarBorder");
                        Respawn("ExperienceBar");
                    });
            }
            else if (type.category == "Battlemaster")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to enter the arena");
                });
                AddButtonRegion(() =>
                {
                    AddLine("I want to buy equipment");
                });
            }
            else if (type.category == "Stable Master")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to swap my mount");
                },
                (h) =>
                {
                    PlaySound("DesktopInventoryOpen");
                    CloseWindow(h.window);
                    CloseWindow("Town");
                    SpawnWindowBlueprint("MountCollection");
                    if (mounts.Find(x => x.name == currentSave.player.mount) != null)
                        SpawnWindowBlueprint("CurrentMount");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
                if (mounts.Count(x => !currentSave.player.mounts.Contains(x.name) && x.factions != null && x.factions.Contains(person.faction == null ? town.faction : person.faction)) > 0)
                    AddButtonRegion(() =>
                    {
                        AddLine("I want to buy a new mount");
                    },
                    (h) =>
                    {
                        PlaySound("DesktopInventoryOpen");
                        CloseWindow(h.window);
                        CloseWindow("Town");
                        SpawnWindowBlueprint("MountVendor");
                    });
            }
            else if (type.category == "Flight Master")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to take a flight path");
                },
                (h) =>
                {
                    PlaySound("DesktopInventoryOpen");
                    CloseWindow(h.window);
                    CloseWindow("Town");
                    SpawnWindowBlueprint("FlightMaster");
                    if (mounts.Find(x => x.name == currentSave.player.mount) != null)
                        SpawnWindowBlueprint("CurrentMount");
                });
            }
            if (person.itemsSold != null && person.itemsSold.Count > 0)
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to browse your goods");
                },
                (h) =>
                {
                    if (!currentSave.vendorStock.ContainsKey(town.name + ":" + person.name) && person.itemsSold != null && person.itemsSold.Count > 0)
                        currentSave.vendorStock.Add(town.name + ":" + person.name, person.ExportStock());
                    PlayVoiceLine(person.VoiceLine("Vendor"));
                    PlaySound("DesktopInventoryOpen");
                    CloseWindow(h.window);
                    CloseWindow("Town");
                    SpawnWindowBlueprint("Vendor");
                    SpawnWindowBlueprint("Inventory");
                    Respawn("PlayerMoney", true);
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
            }
            AddButtonRegion(() =>
            {
                AddLine("Goodbye");
            },
            (h) =>
            {
                PlayVoiceLine(person.VoiceLine("Farewell"));
                PlaySound("DesktopInstanceClose");
                person = null;
                CloseWindow(h.window);
                if (personCategory != null) Respawn("Persons");
                Respawn("Town");
                Respawn("Persons", true);
                Respawn("TownQuestAvailable");
                Respawn("TownQuestDone");
            });
        }, true),
        new("Persons", () => {
            SetAnchor(TopLeft, 19, -57);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddPaddingRegion(() =>
            {
                AddLine(personCategory.category + "s:", "Gray");
                AddSmallButton("OtherReverse",
                (h) =>
                {
                    personCategory = null;
                    CloseWindow(h.window.title);
                    Respawn("Town");
                    PlaySound("DesktopInstanceClose");
                });
            });
            var people = town.people.FindAll(x => x.category == personCategory && !x.hidden);
            foreach (var person in people)
            {
                var personType = personTypes.Find(x => x.type == person.type);
                AddButtonRegion(() =>
                {
                    AddLine(person.name, "Black");
                    AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? factions.Find(x => x.name == town.faction).side : "") : "OtherUnknown");
                },
                (h) =>
                {
                    Person.person = person;
                    Respawn("Person");
                    CloseWindow("Persons");
                    CloseWindow("Town");
                    CloseWindow("QuestAdd");
                    CloseWindow("QuestTurn");
                    CloseWindow("TownQuestAvailable");
                    CloseWindow("TownQuestDone");
                    PlayVoiceLine(person.VoiceLine("Greeting"));
                    PlaySound("DesktopInstanceOpen");
                });
            }
        }, true),
        new("Vendor", () => {
            currentSave.buyback ??= new(true);
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var items = currentSave.vendorStock[town.name + ":" + person.name];
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("Vendor");
                    CloseWindow("Inventory");
                    Respawn("PlayerMoney", true);
                    Respawn("Person");
                    PlaySound("DesktopInventoryClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Vendor goods:");
            });
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (index * 5 + j >= 999) AddBigButton("OtherNoSlot");
                            else if (items.Count > index * 5 + j) PrintVendorItem(items[index * 5 + j], null);
                            else AddBigButton("OtherEmpty");
                    }
                );
            }
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Merchant", "", "Center"));
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Buyback", "", "Center"), (h) => { CloseWindow("Vendor"); SpawnWindowBlueprint("VendorBuyback"); PlaySound("VendorSwitchTab"); });
        }, true),
        new("VendorBuyback", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var items = new List<Item>();
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("VendorBuyback");
                    CloseWindow("Inventory");
                    Respawn("PlayerMoney", true);
                    Respawn("Person");
                    PlaySound("DesktopInventoryClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Buyback stock:");
            });
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (currentSave.buyback.items.Count > index * 5 + j) PrintVendorItem(null, currentSave.buyback.items[index * 5 + j]);
                            else AddBigButton("OtherEmpty");
                    }
                );
            }
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Merchant", "", "Center"), (h) => { CloseWindow("VendorBuyback"); SpawnWindowBlueprint("Vendor"); PlaySound("VendorSwitchTab"); });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Buyback", "", "Center"));
        }, true),
        new("ResetTalents", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var type = personTypes.Find(x => x.type == person.type);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""));
            });
            AddPaddingRegion(() =>
            {
                AddLine("Do you want to reset your", "DarkGray");
                AddLine("talents and regain all", "DarkGray");
                AddLine("spent points?", "DarkGray");
            });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() =>
            {
                AddLine("Cancel", "", "Center");
            },
            (h) =>
            {
                PlaySound("DesktopInstanceClose");
                CloseWindow("ResetTalents");
                Respawn("Person");
            });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() =>
            {
                AddLine("Yes", "", "Center");
            },
            (h) =>
            {
                PlaySound("DesktopHomeLocation");
                currentSave.player.ResetTalents();
                CloseWindow("ResetTalents");
                Respawn("Person");
                Respawn("MapToolbarStatusRight", true);
            });
        }),
        new("MakeInnHome", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var type = personTypes.Find(x => x.type == person.type);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""));
            });
            AddPaddingRegion(() =>
            {
                AddLine("Do you want to change your", "DarkGray");
                AddLine("home from ", "DarkGray");
                AddText(currentSave.player.homeLocation, "LightGray");
                AddLine("to ", "DarkGray");
                AddText(town.name, "LightGray");
                AddText("?", "DarkGray");
                AddLine("");
            });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() =>
            {
                AddLine("Cancel", "", "Center");
            },
            (h) =>
            {
                PlaySound("DesktopInstanceClose");
                CloseWindow("MakeInnHome");
                Respawn("Person");
            });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() =>
            {
                AddLine("Yes", "", "Center");
            },
            (h) =>
            {
                PlaySound("DesktopHomeLocation");
                currentSave.player.homeLocation = town.name;
                CloseWindow("MakeInnHome");
                Respawn("Person");
            });
        }),
        new("MountCollection", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup(() => currentSave.player.mounts.Count, 6);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(288);
            var type = personTypes.Find(x => x.type == person.type);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("MountCollection");
                    CloseWindow("CurrentMount");
                    Respawn("Person");
                    PlaySound("DesktopInventoryClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Stabled mounts:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.player.mounts.Reverse();
                    Respawn("MountCollection");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!WindowUp("MountsSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        Respawn("MountsSort");
                        Respawn("MountCollection");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            AddPaginationLine(regionGroup);
            var mounts = currentSave.player.mounts.Select(x => Mount.mounts.Find(y => y.name == x)).ToList();
            mounts.RemoveAll(x => x.name == currentSave.player.mount);
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (mounts.Count > index + 6 * regionGroup.pagination())
                    {
                        var mount = mounts[index + 6 * regionGroup.pagination()];
                        AddLine(mount.name, mount.speed == 7 ? "Rare" : "Epic");
                        AddLine("Speed: ", "DarkGray");
                        AddText(mount.speed == 7 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"));
                        AddBigButton(mount.icon,
                            (h) =>
                            {
                                var mount = mounts[index + 6 * regionGroup.pagination()];
                                if (currentSave.player.mount != mount.name && currentSave.player.level >= (mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts))
                                {
                                    currentSave.player.mount = mount.name;
                                    Respawn("MountCollection");
                                    Respawn("CurrentMount");
                                    PlaySound("DesktopActionBarAdd", 0.9f);
                                }
                            },
                            null,
                            (h) => () =>
                            {
                                var mount = mounts[index + 6 * regionGroup.pagination()];
                                PrintMountTooltip(currentSave.player, mount);
                            }
                        );
                        if (currentSave.player.level < (mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts)) { SetBigButtonToRed(); AddBigButtonOverlay("OtherGridBlurred"); }
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled");
                    }
                });
            }
        }),
        new("MountVendor", () => {
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup(() => Mount.mounts.Count(x => !currentSave.player.mounts.Contains(x.name) && x.factions != null && x.factions.Contains(person.faction == null ? town.faction : person.faction)), 6);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(285);
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("MountVendor");
                    Respawn("Person");
                    PlaySound("DesktopInstanceClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Available mounts:");
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            AddPaginationLine(regionGroup);
            var mounts = Mount.mounts.FindAll(x => !currentSave.player.mounts.Contains(x.name) && x.factions != null && x.factions.Contains(person.faction == null ? town.faction : person.faction)).OrderBy(x => x.speed).ThenBy(x => x.price).ThenBy(x => x.name).ToList();
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                if (mounts.Count >= index + 6 * regionGroup.pagination())
                    AddPaddingRegion(() =>
                    {
                        if (mounts.Count > index + 6 * regionGroup.pagination())
                        {
                            var mount = mounts[index + 6 * regionGroup.pagination()];
                            AddLine(mount.name, mount.speed == 7 ? "Rare" : "Epic");
                            AddLine("Speed: ", "DarkGray");
                            AddText(mount.speed == 7 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"));
                            AddBigButton(mount.icon,
                                (h) =>
                                {
                                    var mount = mounts[index + 6 * regionGroup.pagination()];
                                    if (currentSave.player.inventory.money >= mount.price)
                                    {
                                        currentSave.player.inventory.money -= mount.price;
                                        Respawn("PlayerMoney", true);
                                        currentSave.player.mounts.Add(mount.name);
                                        Respawn("MountVendor");
                                        PlaySound("DesktopTransportPay");
                                    }
                                },
                                null,
                                (h) => () =>
                                {
                                    var mount = mounts[index + 6 * regionGroup.pagination()];
                                    PrintMountTooltip(currentSave.player, mount);
                                }
                            );
                            if (currentSave.player.level < (mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts))
                            {
                                SetBigButtonToRed();
                                AddBigButtonOverlay("OtherGridBlurred");
                            }
                        }
                        else if (mounts.Count == index + 6 * regionGroup.pagination())
                        {
                            SetRegionBackground(Padding);
                            AddLine("");
                        }
                    });
            }
        }),
        new("MountsSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort mounts:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("MountsSort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                currentSave.player.mounts = currentSave.player.mounts.OrderBy(x => x).ToList();
                CloseWindow("MountsSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By speed", "Black");
            },
            (h) =>
            {
                currentSave.player.mounts = currentSave.player.mounts.OrderByDescending(x => Mount.mounts.Find(y => y.name == x).speed).ToList();
                CloseWindow("MountsSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("CurrentMount", () => {
            SetAnchor(-92, 142);
            AddHeaderGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Current mount:");
            });
            AddPaddingRegion(() =>
            {
                var mount = mounts.Find(x => x.name == currentSave.player.mount);
                if (mount != null)
                {
                    AddLine(mount.name);
                    AddLine("Speed: ", "DarkGray");
                    AddText(mount.speed == 7 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"));
                    AddBigButton(mount.icon);
                }
                else AddBigButton("OtherDisabled");
            });
            var mount = mounts.Find(x => x.name == currentSave.player.mount);
            if (WindowUp("MountCollection"))
                AddButtonRegion(() =>
                {
                    AddLine("Dismount");
                },
                (h) =>
                {
                    currentSave.player.mount = "";
                    Respawn("MountCollection");
                    CloseWindow(h.window);
                });
        }),
        new("Bank", () => {
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupHeight(281);
            var items = currentSave.banks[town.name].items;
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("Bank");
                    CloseWindow("Inventory");
                    Respawn("Person");
                    PlaySound("DesktopInventoryClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Bank:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    currentSave.banks[town.name].items.Reverse();
                    Respawn("Bank");
                    Respawn("Inventory");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!WindowUp("InventorySettings") && !WindowUp("InventorySort") && !WindowUp("BankSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        Respawn("BankSort");
                        Respawn("Bank");
                        Respawn("Inventory");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                            if (index * 5 + j >= currentSave.banks[town.name].BagSpace()) AddBigButton("OtherNoSlot");
                            else if (items.Count > index * 5 + j) PrintBankItem(items[index * 5 + j]);
                            else AddBigButton("OtherEmpty");
                    }
                );
            }
            AddHeaderRegion(() =>
            {
                AddLine("");
            });
        }, true),
        new("BankSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort bank inventory:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("BankSort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                currentSave.banks[town.name].items = currentSave.banks[town.name].items.OrderBy(x => x.name).ToList();
                CloseWindow("BankSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By name", "Black");
            },
            (h) =>
            {
                currentSave.banks[town.name].items = currentSave.banks[town.name].items.OrderBy(x => x.amount).ToList();
                CloseWindow("BankSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By item power", "Black");
            },
            (h) =>
            {
                currentSave.banks[town.name].items = currentSave.banks[town.name].items.OrderByDescending(x => x.ilvl).ToList();
                CloseWindow("BankSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By price", "Black");
            },
            (h) =>
            {
                currentSave.banks[town.name].items = currentSave.banks[town.name].items.OrderByDescending(x => x.price).ToList();
                CloseWindow("BankSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By type", "Black");
            },
            (h) =>
            {
                currentSave.banks[town.name].items = currentSave.banks[town.name].items.OrderByDescending(x => x.type).ToList();
                CloseWindow("BankSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("FlightMaster", () => {
            SetAnchor(TopLeft, 19, -38);
            var side = currentSave.player.Side();
            var destinations = town.flightPaths[side].FindAll(x => x != town).OrderBy(x => !currentSave.siteVisits.ContainsKey(x.name)).ThenBy(x => x.zone).ThenBy(x => x.name).ToList();
            AddRegionGroup(() => destinations.Count, 12);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("FlightMaster");
                    Respawn("Person");
                    PlaySound("DesktopInstanceClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Possible destinations:");
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            for (int i = 0; i < 12; i++)
            {
                var index = i;
                if (destinations.Count > index + 12 * regionGroup.pagination())
                    AddButtonRegion(() =>
                    {
                        var destination = destinations[index + 12 * regionGroup.pagination()];
                        if (currentSave.siteVisits.ContainsKey(destination.name))
                        {
                            AddLine(destination.name);
                            AddSmallButton("Zone" + destination.zone.Clean());
                        }
                        else
                        {
                            SetRegionBackground(Header);
                            AddLine("?", "DarkGray");
                            AddSmallButton("OtherDisabled");
                        }
                    },
                    (h) =>
                    {
                        var destination = destinations[index + 12 * regionGroup.pagination()];
                        currentSave.currentSite = destination.name;
                        Respawn("Site: " + town.name);
                        Respawn("Site: " + currentSave.currentSite);
                        town = destination;

                        //if (transport.price > 0)
                        //{
                        //    if (transport.price > currentSave.player.inventory.money) return;
                        //    PlaySound("DesktopTransportPay");
                        //    currentSave.player.inventory.money -= transport.price;
                        //}

                        //Close town screen as we're beginning to travel on map
                        CloseDesktop("Town");

                        //Switch desktop to map
                        SwitchDesktop("Map");

                        //Move camera to the newly visited town
                        CDesktop.cameraDestination = new Vector2(town.x, town.y);

                        ////Find current site
                        //var current = FindSite(x => x.name == currentSave.currentSite);

                        ////Lead path to the destination
                        //LeadPath(new SitePath() { means = "Flight", sites = new() { current.name, town.name }, points = new() { (town.x, town.y), (current.x, current.y) }, spacing = 9999 }, true);

                        ////Queue moving player to the destination
                        //town.ExecutePath("Town");
                    });
                else if (destinations.Count == index + 12 * regionGroup.pagination())
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
            }
            AddPaginationLine(regionGroup);
        }),
        new("AuctionHouse", () => {
            //SetAnchor(TopLeft, 19, -38);
            //var destinations = town.flightPaths[side].FindAll(x => x != town).OrderBy(x => !currentSave.siteVisits.ContainsKey(x.name)).ThenBy(x => x.zone).ThenBy(x => x.name).ToList();
            //AddRegionGroup(() => destinations.Count, 12);
            //SetRegionGroupWidth(190);
            //SetRegionGroupHeight(281);
            //AddHeaderRegion(() =>
            //{
            //    var type = personTypes.Find(x => x.type == person.type);
            //    AddLine(person.type + " ", "Gray");
            //    AddText(person.name);
            //    AddSmallButton("OtherClose", (h) =>
            //    {
            //        CloseWindow("FlightMaster");
            //        Respawn("Person");
            //        PlaySound("DesktopInstanceClose");
            //    });
            //});
            //AddHeaderRegion(() =>
            //{
            //    AddLine("Possible destinations:");
            //});
            //var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            //for (int i = 0; i < 12; i++)
            //{
            //    var index = i;
            //    if (destinations.Count > index + 12 * regionGroup.pagination())
            //        AddButtonRegion(() =>
            //        {
            //            var destination = destinations[index + 12 * regionGroup.pagination()];
            //            if (currentSave.siteVisits.ContainsKey(destination.name))
            //            {
            //                AddLine(destination.name);
            //                AddSmallButton("Zone" + destination.zone.Clean());
            //            }
            //            else
            //            {
            //                SetRegionBackground(Header);
            //                AddLine("?", "DarkGray");
            //                AddSmallButton("OtherDisabled");
            //            }
            //        },
            //        (h) =>
            //        {
            //            var destination = destinations[index + 12 * regionGroup.pagination()];
            //            currentSave.currentSite = destination.name;
            //            Respawn("Site: " + town.name);
            //            Respawn("Site: " + currentSave.currentSite);
            //            town = destination;

            //            //if (transport.price > 0)
            //            //{
            //            //    if (transport.price > currentSave.player.inventory.money) return;
            //            //    PlaySound("DesktopTransportPay");
            //            //    currentSave.player.inventory.money -= transport.price;
            //            //}

            //            //Close town screen as we're beginning to travel on map
            //            CloseDesktop("Town");

            //            //Switch desktop to map
            //            SwitchDesktop("Map");

            //            //Move camera to the newly visited town
            //            CDesktop.cameraDestination = new Vector2(town.x, town.y);

            //            ////Find current site
            //            //var current = FindSite(x => x.name == currentSave.currentSite);

            //            ////Lead path to the destination
            //            //LeadPath(new SitePath() { means = "Flight", sites = new() { current.name, town.name }, points = new() { (town.x, town.y), (current.x, current.y) }, spacing = 9999 }, true);

            //            ////Queue moving player to the destination
            //            //town.ExecutePath("Town");
            //        });
            //    else if (destinations.Count == index + 12 * regionGroup.pagination())
            //        AddPaddingRegion(() =>
            //        {
            //            SetRegionAsGroupExtender();
            //            AddLine("");
            //        });
            //}
            //AddPaginationLine(regionGroup);
        }),
        new("AuctionHouseOffersGroups", () => {
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup(() => Market.exploredAuctionsGroups.Count(), 12);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("AuctionHouseOffersGroups");
                    Respawn("Person");
                    PlaySound("DesktopInstanceClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Available auctions:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    Market.exploredAuctions.Reverse();
                    CloseWindow("AuctionHouseOffersGroups");
                    Respawn("AuctionHouseOffersGroups");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!WindowUp("AuctionHouseOffersSettings") && !WindowUp("AuctionHouseOffersSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("AuctionHouseOffersSort");
                        CloseWindow("AuctionHouseOffersGroups");
                        Respawn("AuctionHouseOffersGroups");
                    });
                else
                    AddSmallButton("OtherSortOff");
                if (!WindowUp("AuctionHouseOffersSettings") && !WindowUp("AuctionHouseOffersSort"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("AuctionHouseOffersSettings");
                        CloseWindow("AuctionHouseOffersGroups");
                        Respawn("AuctionHouseOffersGroups");
                    });
                else
                    AddSmallButton("OtherSettingsOff");
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            for (int i = 0; i < 12; i++)
            {
                var index = i;
                if (Market.exploredAuctionsGroups.Count() > index + 12 * regionGroup.pagination())
                    AddButtonRegion(() =>
                    {
                        var offerGroupKey = Market.exploredAuctionsGroups.Keys.ToList()[index + 12 * regionGroup.pagination()];
                        var offerGroup = Market.exploredAuctionsGroups[offerGroupKey];
                        var offerGroupFirst = Market.exploredAuctionsGroups[offerGroupKey][0];
                        AddLine(offerGroupFirst.item.name + " x" + offerGroup.Count);
                        AddSmallButton(offerGroupFirst.item.icon);
                    },
                    (h) =>
                    {
                        var offerGroupKey = Market.exploredAuctionsGroups.Keys.ToList()[index + 12 * regionGroup.pagination()];
                        Market.exploredAuctions = Market.exploredAuctionsGroups[offerGroupKey].OrderBy(x => x.price).ToList();
                        CloseWindow("AuctionHouseOffersGroups");
                        SpawnWindowBlueprint("AuctionHouseOffers");
                        SpawnWindowBlueprint("AuctionHouseChosenItem");
                    },
                    null, (h) => () =>
                    {
                        var offerGroupKey = Market.exploredAuctionsGroups.Keys.ToList()[index + 12 * regionGroup.pagination()];
                        PrintItemTooltip(Market.exploredAuctionsGroups[offerGroupKey][0].item);
                    });
                else if (Market.exploredAuctionsGroups.Count == index + 12 * regionGroup.pagination())
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
            }
            AddPaginationLine(regionGroup);
        }),
        new("AuctionHouseOffers", () => {
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup(() => Market.exploredAuctions.Count, 12);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("AuctionHouseOffers");
                    CloseWindow("AuctionHouseChosenItem");
                    Respawn("AuctionHouseOffersGroups");
                    PlaySound("DesktopInstanceClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Available auctions:");
                if (!WindowUp("AuctionHouseOffersSettings"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("AuctionHouseOffersSettings");
                        CloseWindow("AuctionHouseOffers");
                        Respawn("AuctionHouseOffers");
                    });
                else
                    AddSmallButton("OtherSettingsOff");
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            for (int i = 0; i < 12; i++)
            {
                var index = i;
                if (Market.exploredAuctions.Count > index + 12 * regionGroup.pagination())
                    AddPaddingRegion(() =>
                    {
                        var offer = Market.exploredAuctions[index + 12 * regionGroup.pagination()];
                        AddLine("x" + offer.item.amount);
                        AddText(" each for ", "DarkGray");
                        if (offer.price / 10000 > 0) AddText(offer.price / 10000 + " ", "Gold");
                        if (offer.price % 10000 / 100 > 0) AddText(offer.price % 10000 / 100 + " ", "Silver");
                        if (offer.price % 100 > 0) AddText(offer.price % 100 + "", "Copper");
                        if (settings.sourcedMarket.Value())
                            AddSmallButton(offer.market == "Alliance Market" ? "FactionAlliance" : (offer.market == "Horde Market" ? "FactionHorde" : "ItemMiscQuestionMark"));
                    });
                else if (Market.exploredAuctions.Count == index + 12 * regionGroup.pagination())
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
            }
            AddPaginationLine(regionGroup);
        }),
        new("AuctionHouseChosenItem", () => {
            PrintItemTooltip(Market.exploredAuctions[0].item);
        }),
        new("AuctionHouseOffersSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort auctions:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("AuctionHouseOffersSort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() => AddLine("By item name", "Black"),
            (h) =>
            {
                Market.exploredAuctions = Market.exploredAuctions.OrderBy(x => x.item.name).ToList();
                CloseWindow("AuctionHouseOffersSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By unit price", "Black"),
            (h) =>
            {
                Market.exploredAuctions = Market.exploredAuctions.OrderByDescending(x => x.price).ToList();
                CloseWindow("AuctionHouseOffersSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("AuctionHouseOffersSettings", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Auction list settings:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("AuctionHouseOffersSettings");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Show sourced market", "Black");
                AddCheckbox(settings.sourcedMarket);
            },
            (h) =>
            {
                settings.sourcedMarket.Invert();
                CDesktop.RespawnAll();
            });
        }),
        new("ProfessionLevelTrainer", () => {
            SetAnchor(TopLeft, 19, -38);
            var type = personTypes.Find(x => x.type == person.type);
            var profession = professions.Find(x => x.name == type.profession);
            var levels = profession.levels.FindAll(x => x.requiredSkill <= type.skillCap).OrderBy(x => x.requiredSkill).ToList();
            if (currentSave.player.professionSkills.ContainsKey(profession.name))
                levels = levels.FindAll(x => !currentSave.player.professionSkills[profession.name].Item2.Contains(x.name));
            AddHeaderGroup(() => levels.Count, 6);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(288);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""));
            });
            AddHeaderRegion(() =>
            {
                AddLine("Learnable levels:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window.title);
                    Respawn("Person");
                    PlaySound("DesktopInstanceClose");
                });
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (levels.Count > index + 6 * regionGroup.pagination())
                    {
                        var key = levels[index + 6 * regionGroup.pagination()];
                        AddLine(key.name);
                        AddLine("", "DarkGray");
                        if (key.requiredLevel > 0)
                        {
                            AddText("Level: ", "DarkGray");
                            AddText(key.requiredLevel + "", ColorRequiredLevel(key.requiredLevel));
                        }
                        if (key.requiredSkill > 0)
                        {
                            AddText(", Skill: ", "DarkGray");
                            AddText(key.requiredSkill + "", ColorProfessionRequiredSkill(profession.name, key.requiredSkill));
                        }
                        AddBigButton(profession.icon,
                            (h) =>
                            {
                                var key = levels[index + 6 * regionGroup.pagination()];

                                //If player is high enough level and has the money..
                                if (currentSave.player.level >= key.requiredLevel && currentSave.player.inventory.money >= key.price)
                                {
                                    //If has the profession and at a proper level..
                                    if (key.requiredSkill == 0 || currentSave.player.professionSkills.ContainsKey(type.profession) && currentSave.player.professionSkills[type.profession].Item1 >= key.requiredSkill)
                                    {
                                        //If doesnt have the level yet..
                                        if (!currentSave.player.professionSkills.ContainsKey(type.profession) || currentSave.player.professionSkills.ContainsKey(type.profession) && !currentSave.player.professionSkills[type.profession].Item2.Contains(key.name))
                                        {
                                            //Learn the level
                                            currentSave.player.inventory.money -= key.price;
                                            Respawn("PlayerMoney", true);
                                            if (!currentSave.player.professionSkills.ContainsKey(type.profession))
                                            {
                                                currentSave.player.professionSkills.Add(type.profession, (1, new()));
                                                if (!currentSave.player.learnedRecipes.ContainsKey(type.profession))
                                                    currentSave.player.learnedRecipes.Add(type.profession, new());
                                                foreach (var recipe in professions.Find(x => x.name == type.profession).defaultRecipes)
                                                    currentSave.player.LearnRecipe(type.profession, recipe);
                                            }
                                            currentSave.player.professionSkills[type.profession].Item2.Add(key.name);
                                            Respawn(h.window.title);
                                            PlaySound("DesktopSkillLearned");
                                        }
                                    }
                                }
                            },
                            null,
                            (h) => () =>
                            {
                                var key = levels[index + 6 * regionGroup.pagination()];
                                PrintProfessionLevelTooltip(currentSave.player, profession, key);
                            }
                        );
                        var can = false;
                        if (currentSave.player.level >= key.requiredLevel)
                            if (key.requiredSkill == 0 || currentSave.player.professionSkills.ContainsKey(type.profession) && currentSave.player.professionSkills[type.profession].Item1 >= key.requiredSkill)
                                if (!currentSave.player.professionSkills.ContainsKey(type.profession) || currentSave.player.learnedRecipes.ContainsKey(type.profession) && !currentSave.player.professionSkills[type.profession].Item2.Contains(key.name))
                                    can = true;
                        if (!can)
                        {
                            SetBigButtonToRed();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled");
                    }
                });
            }
            AddPaginationLine(regionGroup);
        }),
        new("ProfessionRecipeTrainer", () => {
            SetAnchor(TopLeft, 19, -38);
            var type = personTypes.Find(x => x.type == person.type);
            var profession = professions.Find(x => x.name == type.profession);
            var recipes = Recipe.recipes.FindAll(x => x.profession == type.profession && x.price > 0 && (x.learnedAt <= type.skillCap || type.skillCap == 0));
            if (currentSave.player.learnedRecipes.ContainsKey(type.profession))
                recipes = recipes.FindAll(x => !currentSave.player.learnedRecipes[type.profession].Contains(x.name));
            AddHeaderGroup(() => recipes.Count, 6);
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(288);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == town.faction).side : ""));
            });
            AddHeaderRegion(() =>
            {
                AddLine("Learnable " + profession.recipeType.ToLower() + (profession.recipeType.Last() == 's' ? ":" : "s:"), "Gray");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window.title);
                    Respawn("Person");
                    PlaySound("DesktopInstanceClose");
                });
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (recipes.Count > index + 6 * regionGroup.pagination())
                    {
                        var key = recipes[index + 6 * regionGroup.pagination()];
                        AddLine(key.name, key.NameColor());
                        AddLine("", "DarkGray");
                        if (key.learnedAt > 0)
                        {
                            AddText("Required skill: ", "DarkGray");
                            AddText(key.learnedAt + " ", ColorProfessionRequiredSkill(key.profession, key.learnedAt));
                        }
                        AddBigButton(key.Icon(),
                            (h) =>
                            {
                                var key = recipes[index + 6 * regionGroup.pagination()];

                                //If player has the money and has the profession and at a proper level..
                                if (currentSave.player.inventory.money >= key.price && currentSave.player.professionSkills.ContainsKey(key.profession) && currentSave.player.professionSkills[key.profession].Item1 >= key.learnedAt)
                                {
                                    //If doesnt have the recipe..
                                    if (!currentSave.player.learnedRecipes.ContainsKey(type.profession) || currentSave.player.learnedRecipes.ContainsKey(type.profession) && !currentSave.player.learnedRecipes[type.profession].Contains(key.name))
                                    {
                                        //Add the recipe
                                        currentSave.player.inventory.money -= key.price;
                                        Respawn("PlayerMoney", true);
                                        currentSave.player.LearnRecipe(key);
                                        Respawn(h.window.title);
                                        PlaySound("DesktopSkillLearned");
                                    }
                                }
                            },
                            null,
                            (h) => () =>
                            {
                                var key = recipes[index + 6 * regionGroup.pagination()];
                                if (Input.GetKey(LeftControl) && key.results.Count > 0)
                                    PrintItemTooltip(items.Find(x => x.name == key.results.First().Key), Input.GetKey(LeftShift));
                                else PrintRecipeTooltip(currentSave.player, key);
                            }
                        );
                        var can = false;
                        if (currentSave.player.professionSkills.ContainsKey(key.profession) && currentSave.player.professionSkills[key.profession].Item1 >= key.learnedAt)
                            if (!currentSave.player.learnedRecipes.ContainsKey(type.profession) || currentSave.player.learnedRecipes.ContainsKey(type.profession) && !currentSave.player.learnedRecipes[type.profession].Contains(key.name))
                                can = true;
                        if (!can)
                        {
                            SetBigButtonToRed();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled");
                    }
                });
            }
            AddPaginationLine(regionGroup);
        }),
        new("PlayerMoney", () => {
            if (WindowUp("Inventory")) return;
            if (WindowUp("Quest")) return;
            if (WindowUp("QuestAdd")) return;
            if (WindowUp("QuestTurn")) return;
            SetAnchor(BottomRight, -19, 35);
            PrintPriceRegion(currentSave.player.inventory.money);
        }),

        //Fishing
        new("FishingBoardFrame", () => {
            SetAnchor(-115, 146);
            var boardBackground = new GameObject("BoardBackground", typeof(SpriteRenderer), typeof(SpriteMask));
            boardBackground.transform.parent = CDesktop.LBWindow().transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/BoardFrames/BoardBackground6x6Simple");
            var mask = boardBackground.GetComponent<SpriteMask>();
            mask.sprite = Resources.Load<Sprite>("Sprites/BoardFrames/BoardMask6x6");
            mask.isCustomRangeActive = true;
            mask.frontSortingLayerID = SortingLayer.NameToID("Missile");
            mask.backSortingLayerID = SortingLayer.NameToID("Default");
            boardBackground = new GameObject("FishingPool", typeof(SpriteRenderer), typeof(Highlightable), typeof(BoxCollider2D));
            boardBackground.transform.parent = CDesktop.LBWindow().transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/BoardFrames/BoardShadow6x6Water" + fishingSpot.waterType);
            boardBackground.GetComponent<SpriteRenderer>().sortingLayerName = "CameraShadow";
            boardBackground.GetComponent<BoxCollider2D>().offset = new Vector2(132, -132);
            boardBackground.GetComponent<BoxCollider2D>().size = new Vector2(226, 226);
            boardBackground.GetComponent<Highlightable>().Initialise(null, (h) =>
            {
                Cursor.cursor.SetCursor(CursorType.None);
                PlaySound("FishingCast");
            },
            null, null, null);
        }),
        new("FishingSpotInfo", () => {
            SetAnchor(TopRight);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            var site = FindSite(x => x.name == fishingSpot.name);
            AddButtonRegion(
                () =>
                {
                    AddLine(site.zone, "Black");
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        PlaySound("DesktopMenuOpen", 0.6f);
                        SpawnDesktopBlueprint("GameMenu");
                    });
                },
                (h) =>
                {

                }
            );
            AddHeaderRegion(() =>
            {
                AddBigButton("Zone" + site.zone.Clean());
                AddLine("Skill to fish: ", "DarkGray");
                AddText(fishingSpot.skillToFish + "", currentSave.player.professionSkills["Fishing"].Item1 < fishingSpot.skillToFish ? "DangerousRed" : "HalfGray");
            });
        }),
        new("FisherInfo", () => {
            SetAnchor(TopLeft);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddButtonRegion(
                () =>
                {
                    AddLine(currentSave.player.name, "Black");
                    AddSmallButton("MenuFlee", (h) =>
                    {
                        fishingSpot.EndFishing("Left");
                    });
                }
            );
            AddHeaderRegion(() =>
            {
                ReverseButtons();
                if (currentSave.player.spec != null)
                    AddBigButton(currentSave.player.Spec().icon);
                else
                {
                    var race = races.Find(x => x.name == currentSave.player.race);
                    AddBigButton(race.portrait == "" ? "OtherUnknown" : race.portrait + (race.genderedPortrait ? currentSave.player.gender : ""));
                }
                AddLine("Level: " , "DarkGray");
                AddText("" + currentSave.player.level, "Gray");
            });
        }),

        //Map
        new("MapToolbarShadow", () => {
            SetAnchor(Top);
            AddRegionGroup();
            SetRegionGroupWidth(638);
            SetRegionGroupHeight(15);
            AddPaddingRegion(() => { });
        }, true),
        new("MapToolbar", () => {
            AddHotkey(N, () =>
            {
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                CloseDesktop("QuestLog");
                if (CDesktop.title != "TalentScreen")
                {
                    PlaySound("DesktopTalentScreenOpen");
                    SpawnDesktopBlueprint("TalentScreen");
                }
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopTalentScreenClose");
                }
            });
            AddHotkey(P, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                CloseDesktop("QuestLog");
                if (CDesktop.title != "SpellbookScreen")
                    SpawnDesktopBlueprint("SpellbookScreen");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopSpellbookClose");
                }
            });
            AddHotkey(B, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                CloseDesktop("QuestLog");
                if (CDesktop.title != "ContainerLoot")
                {
                    if (CDesktop.title != "EquipmentScreen")
                        SpawnDesktopBlueprint("EquipmentScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopInventoryClose");
                    }
                }
            });
            AddHotkey(T, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                CloseDesktop("QuestLog");
                if (CDesktop.title != "BestiaryScreen")
                    SpawnDesktopBlueprint("BestiaryScreen");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopInstanceClose");
                }
            });
            AddHotkey(R, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CharacterSheet");
                CloseDesktop("QuestLog");
                if (CDesktop.title != "CraftingScreen")
                    SpawnDesktopBlueprint("CraftingScreen");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopSpellbookClose");
                }
            });
            AddHotkey(C, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("QuestLog");
                if (CDesktop.title != "CharacterSheet")
                    SpawnDesktopBlueprint("CharacterSheet");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopCharacterSheetClose");
                }
            });
            AddHotkey(L, () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                if (CDesktop.title != "QuestLog")
                    SpawnDesktopBlueprint("QuestLog");
                else
                {
                    CloseDesktop(CDesktop.title);
                    PlaySound("DesktopSpellbookClose");
                }
            });
            SetAnchor(Top);
            DisableShadows();
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddSmallButton(CDesktop.title == "CharacterSheet" ? "OtherClose" : "MenuCharacterSheet", (h) =>
                {
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("TalentScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("QuestLog");
                    if (CDesktop.title != "CharacterSheet")
                        SpawnDesktopBlueprint("CharacterSheet");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopCharacterSheetClose");
                    }
                });
                if (CDesktop.title != "ContainerLoot")
                    AddSmallButton(CDesktop.title == "EquipmentScreen" ? "OtherClose" : "MenuInventory", (h) =>
                    {
                        CloseDesktop("BestiaryScreen");
                        CloseDesktop("SpellbookScreen");
                        CloseDesktop("TalentScreen");
                        CloseDesktop("CraftingScreen");
                        CloseDesktop("CharacterSheet");
                        CloseDesktop("QuestLog");
                        if (desktops.All(x => x.title != "ContainerLoot"))
                            if (CDesktop.title != "EquipmentScreen")
                                SpawnDesktopBlueprint("EquipmentScreen");
                            else
                            {
                                CloseDesktop(CDesktop.title);
                                PlaySound("DesktopInventoryClose");
                            }
                    });
                else AddSmallButton("OtherCloseOff");
                AddSmallButton(CDesktop.title == "SpellbookScreen" ? "OtherClose" : "MenuSpellbook", (h) =>
                {
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("TalentScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("CharacterSheet");
                    CloseDesktop("QuestLog");
                    if (CDesktop.title != "SpellbookScreen")
                        SpawnDesktopBlueprint("SpellbookScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopSpellbookClose");
                    }
                });
                AddSmallButton(CDesktop.title == "TalentScreen" ? "OtherClose" : "MenuClasses", (h) =>
                {
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("CharacterSheet");
                    CloseDesktop("QuestLog");
                    if (CDesktop.title != "TalentScreen")
                    {
                        PlaySound("DesktopTalentScreenOpen");
                        SpawnDesktopBlueprint("TalentScreen");
                    }
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopTalentScreenClose");
                    }
                });
                AddSmallButton(CDesktop.title == "QuestLog" ? "OtherClose" : "MenuQuestLog", (h) =>
                {
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("TalentScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("CharacterSheet");
                    if (CDesktop.title != "QuestLog")
                        SpawnDesktopBlueprint("QuestLog");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopSpellbookClose");
                    }
                });
                AddSmallButton(CDesktop.title == "CraftingScreen" ? "OtherClose" : "MenuCrafting", (h) =>
                {
                    CloseDesktop("TalentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("CharacterSheet");
                    CloseDesktop("QuestLog");
                    if (CDesktop.title != "CraftingScreen")
                        SpawnDesktopBlueprint("CraftingScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopSpellbookClose");
                    }
                });
                AddSmallButton(CDesktop.title == "BestiaryScreen" ? "OtherClose" : "MenuCompletion", (h) =>
                {
                    CloseDesktop("TalentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("CharacterSheet");
                    CloseDesktop("QuestLog");
                    if (CDesktop.title != "BestiaryScreen")
                        SpawnDesktopBlueprint("BestiaryScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        PlaySound("DesktopInstanceClose");
                    }
                });
            });
        }, true),
        new("MapToolbarClockLeft", () => {
            SetAnchor(TopLeft);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(253);
            AddPaddingRegion(() =>
            {
                AddLine("Day " + (currentSave.day + 1), "", "Right");
            });
        }, true),
        new("MapToolbarStatusLeft", () => {
            SetAnchor(TopLeft);
            DisableGeneralSprites();
            AddRegionGroup();
            SetRegionGroupWidth(262);
            AddPaddingRegion(() =>
            {
                AddLine("Level: ", "DarkGray", "Left");
                AddText(currentSave.player.level + "", "Gray");
            });
        }, true),
        new("MapToolbarStatusRight", () => {
            SetAnchor(TopRight);
            DisableGeneralSprites();
            AddRegionGroup();
            SetRegionGroupWidth(262);
            AddPaddingRegion(() =>
            {
                if (currentSave.player.unspentTalentPoints > 0)
                {
                    AddLine("You have ", "Gray", "Right");
                    AddText(currentSave.player.unspentTalentPoints + "", "Uncommon");
                    AddText(" talent point" + (currentSave.player.unspentTalentPoints == 1 ? "!" : "s!"), "Gray");
                }
                AddSmallButton("OtherSettings", (h) =>
                {
                    PlaySound("DesktopMenuOpen", 0.6f);
                    SpawnDesktopBlueprint("GameMenu");
                });
            });
        }, true),
        new("MapToolbarClockRight", () => {
            SetAnchor(TopRight, -19);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(233);
            AddPaddingRegion(() =>
            {
                AddLine(currentSave.hour + (currentSave.minute < 10 ? ":0" : ":") + currentSave.minute, "", "Left");
            });
        }, true),
        new("WorldBuffs", () => {
            if (currentSave == null || currentSave.player.worldBuffs == null || currentSave.player.worldBuffs.Count == 0) return;
            SetAnchor(TopRight, -9, -28);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                foreach (var buff in currentSave.player.worldBuffs)
                    AddSmallButton(buff.Buff.icon, null, null, (h) => () => Buff.PrintWorldBuffTooltip(buff));
            });
        }, true),

        //Menu
        new("TitleScreenMenu", () => {
            SetAnchor(Top, 0, -19);
            AddRegionGroup();
            SetRegionGroupWidth(130);
            SetRegionGroupHeight(316);
            AddPaddingRegion(() =>
            {
                AddLine("", "Gray");
                AddLine("Maladath", "Epic", "Center");
                AddLine("Chronicles", "Epic", "Center");
                AddLine("0.7.1", "DimGray", "Center");
                AddLine("", "Gray");
                AddLine("", "Gray");
                AddLine("", "Gray");
                AddLine("", "Gray");
                AddLine("", "Gray");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Menu", "Gray", "Center");
            });
            AddButtonRegion(() =>
            {
                AddLine("Singleplayer", "", "Center");
            },
            (h) =>
            {
                if (settings.selectedRealm != "" && settings.selectedCharacter != "" && saves[settings.selectedRealm].Find(x => x.player.name == settings.selectedCharacter).player.dead) settings.selectedCharacter = "";
                else if (settings.selectedRealm == "" && settings.selectedCharacter != "") settings.selectedCharacter = "";
                if (settings.selectedRealm != "" && saves[settings.selectedRealm].Count(x => !x.player.dead) == 0) settings.selectedRealm = "";
                if (settings.selectedRealm == "") SpawnDesktopBlueprint("ChangeRealm");
                else SpawnDesktopBlueprint("LoginScreen");
            });
            AddButtonRegion(() =>
            {
                AddLine("Settings", "", "Center");
            },
            (h) =>
            {
                SpawnWindowBlueprint("GameSettings");
                CloseWindow(h.window);
            });
            AddButtonRegion(() =>
            {
                AddLine("Rankings", "", "Center");
            },
            (h) =>
            {
                SpawnDesktopBlueprint("RankingScreen");
            });
            AddButtonRegion(() =>
            {
                AddLine("Credits", "", "Center");
            },
            (h) =>
            {
                //BLIZZARD: MUSIC, SOUNDS AND TEXTURES
                //POOH: PROGRAMMING, DESIGN, 
                //RYVED & LEKRIS: CONSULTATION AND ADVICE
                //SPECIAL THANKS: ALL OF DISCO ADVANCE FOR BEING THE BEST TEAM EVER
            });
            AddButtonRegion(() =>
            {
                AddLine("Exit", "", "Center");
            },
            (h) =>
            {
                Application.Quit();
            });
            AddPaddingRegion(() => { });
            var maladathIcon = new GameObject("MaladathIcon", typeof(SpriteRenderer));
            maladathIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/MaladathIcon");
            maladathIcon.GetComponent<SpriteRenderer>().sortingLayerName = "Upper";
            maladathIcon.GetComponent<SpriteRenderer>().sortingOrder = 1;
            maladathIcon.transform.parent = CDesktop.LBWindow().transform;
            maladathIcon.transform.localPosition = new Vector3(69, -145);
        }, true),
        new("GameMenu", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine("Menu", "Gray");
                AddSmallButton("OtherClose", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                    CloseDesktop("GameMenu");
                    if (CDesktop.title == "EquipmentScreen")
                    {
                        Respawn("PlayerEquipmentInfo");
                        Respawn("Inventory");
                    }
                });
            });
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddButtonRegion(() =>
            {
                AddLine("Settings", "Black");
            },
            (h) =>
            {
                Respawn("GameSettings");
                CloseWindow(h.window);
            });
            if (CanSave())
            {
                AddButtonRegion(() => AddLine("Save and return to main menu", "Black"),
                (h) =>
                {
                    CloseSave();
                    SaveGames();
                    CloseDesktop("GameMenu");
                    CloseDesktop("Map");
                    SpawnDesktopBlueprint("TitleScreen");
                });
                AddButtonRegion(() => AddLine("Save and exit", "Black"),
                (h) =>
                {
                    CloseSave();
                    SaveGames();
                    Application.Quit();
                });
            }
            else
            {
                AddPaddingRegion(() => AddLine("Save and return to main menu", "DarkGray"));
                AddPaddingRegion(() => AddLine("Save and exit", "DarkGray"));
            }
        }, true),
        new("GameSettings", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine("Settings:", "Gray");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    Respawn(CDesktop.title == "GameMenu" ? "GameMenu" : "TitleScreenMenu");
                });
            });
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddPaddingRegion(() =>
            {
                AddLine("Visuals", "HalfGray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(settings.pixelPerfectVision);
                AddLine("Pixel perfect vision");
            },
            (h) =>
            {
                settings.pixelPerfectVision.Invert();
                CDesktop.RespawnAll();
            });
            AddPaddingRegion(() =>
            {
                AddLine("Gameplay", "HalfGray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(settings.fastCascading);
                AddLine("Fast cascading");
            },
            (h) =>
            {
                settings.fastCascading.Invert();
                CDesktop.RespawnAll();
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(settings.autoCloseLoot);
                AddLine("Automatically close loot");
            },
            (h) =>
            {
                settings.autoCloseLoot.Invert();
                CDesktop.RespawnAll();
            });
            AddPaddingRegion(() =>
            {
                AddLine("Sound", "HalfGray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(settings.music);
                AddLine("Music");
            },
            (h) =>
            {
                settings.music.Invert();
                CDesktop.RespawnAll();
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(settings.soundEffects);
                AddLine("Sound effects");
            },
            (h) =>
            {
                settings.soundEffects.Invert();
                CDesktop.RespawnAll();
            });
        }, true),
        new("LocationInfo", () => {
            SetAnchor(Top);
            AddRegionGroup();
            AddHeaderRegion(
                () =>
                {
                    AddSmallButton("MenuFlee", (h) => board.EndCombat(CDesktop.title == "Game" ? "Flee" : "Quit"));
                }
            );
            AddRegionGroup();
            SetRegionGroupWidth(204);
            AddPaddingRegion(
                () =>
                {
                    AddLine(board != null ? board.area.name : (fishingSpot != null ? fishingSpot.name : "?"), "Gray", "Center");
                }
            );
            AddRegionGroup();
            AddHeaderRegion(
                () =>
                {
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        PlaySound("DesktopMenuOpen", 0.6f);
                        SpawnDesktopBlueprint("GameMenu");
                    });
                }
            );
        }),
        new("MapLocationInfo", () => {
            if (currentSave.currentSite != "") return;
            if (currentSave.currentSite == "") return;
            SetAnchor(BottomRight, 0, 16);
            AddRegionGroup();
            AddPaddingRegion(() => AddSmallButton("MenuFlag", (h) => FindSite(x => x.name == currentSave.currentSite).ExecutePath()));
        }, true),

        //Spellbook
        new("SpellbookAbilityListActivated", () => {
            SetAnchor(TopRight, -19, -38);
            var activeAbilities = abilities.FindAll(x => x.icon != null && !x.hide && x.events.Any(y => y.triggers.Any(z => z["Trigger"] == "AbilityCast")) && x.cost != null && currentSave.player.abilities.ContainsKey(x.name)).ToDictionary(x => x, x => currentSave.player.abilities[x.name]);
            AddHeaderGroup(() => abilities.Count(x => x.icon != null && !x.hide && x.events.Any(y => y.triggers.Any(z => z["Trigger"] == "AbilityCast")) && x.cost != null && currentSave.player.abilities.ContainsKey(x.name)), 7);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(288);
            AddHeaderRegion(() =>
            {
                AddLine("Active abilities:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    abilities.Reverse();
                    Respawn("SpellbookAbilityListActivated");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!WindowUp("AbilitiesSort") && !WindowUp("SwitchActionBars"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        Respawn("AbilitiesSort");
                        CloseWindow("PlayerSpellbookInfo");
                        Respawn("PlayerSpellbookInfo");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            AddPaginationLine(regionGroup);
            var bars = currentSave.player.actionBars[currentSave.player.currentActionSet];
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (activeAbilities.Count > index + 6 * regionGroup.pagination())
                    {
                        var key = activeAbilities.ToList()[index + 6 * regionGroup.pagination()];
                        AddLine(key.Key.name);
                        AddLine("Rank: ", "DarkGray");
                        AddText("" + ToRoman(key.Value + 1));
                        AddBigButton(key.Key.icon,
                            (h) =>
                            {
                                var key = activeAbilities.ToList()[index + 6 * regionGroup.pagination()];
                                if (key.Key.name != currentSave.player.currentActionSet && !bars.Contains(key.Key.name) && bars.Count < currentSave.player.ActionBarsAmount())
                                {
                                    bars.Add(key.Key.name);
                                    Respawn("PlayerSpellbookInfo");
                                    Respawn("SpellbookAbilityListActivated", true);
                                    PlaySound("DesktopActionBarAdd", 0.9f);
                                }
                            },
                            null,
                            (h) => () =>
                            {
                                SetAnchor(Center);
                                var key = activeAbilities.ToList()[index + 6 * regionGroup.pagination()].Key;
                                PrintAbilityTooltip(currentSave.player, key, activeAbilities[key]);
                            }
                        );
                        if (bars.Contains(key.Key.name) || key.Key.name == currentSave.player.currentActionSet)
                        {
                            SetBigButtonToGrayscale();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                        else if (bars.Count < currentSave.player.ActionBarsAmount())
                            AddBigButtonOverlay("OtherGlowLearnable");
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled");
                    }
                });
            }
            AddRegionGroup();
            SetRegionGroupWidth(86);
            AddPaddingRegion(() => AddLine("Activated", "", "Center"));
            AddRegionGroup();
            SetRegionGroupWidth(85);
            AddButtonRegion(() => AddLine("Passive", "", "Center"), (h) =>
            {
                CloseWindow("SpellbookAbilityListActivated");
                Respawn("SpellbookAbilityListPassive");
            });
        }),
        new("SpellbookAbilityListPassive", () => {
            SetAnchor(TopRight, -19, -38);
            var passiveAbilities = abilities.FindAll(x => x.icon != null && !x.hide && x.cost == null && currentSave.player.abilities.ContainsKey(x.name)).ToDictionary(x => x, x => currentSave.player.abilities[x.name]);
            AddHeaderGroup(() => abilities.Count(x => x.icon != null && !x.hide && x.cost == null && currentSave.player.abilities.ContainsKey(x.name)), 7);
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(288);
            AddHeaderRegion(() =>
            {
                AddLine("Passive abilities:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    abilities.Reverse();
                    Respawn("SpellbookAbilityListPassive");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!WindowUp("AbilitiesSort") && !WindowUp("SwitchActionBars"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        Respawn("AbilitiesSort");
                        CloseWindow("PlayerSpellbookInfo");
                        Respawn("PlayerSpellbookInfo");
                    });
                else
                    AddSmallButton("OtherSortOff");
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            AddPaginationLine(regionGroup);
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (passiveAbilities.Count > index + 6 * regionGroup.pagination())
                    {
                        var key = passiveAbilities.ToList()[index + 6 * regionGroup.pagination()];
                        AddLine(key.Key.name);
                        AddLine("Rank: ", "DarkGray");
                        AddText("" + ToRoman(key.Value + 1));
                        AddBigButton(key.Key.icon,
                            null,
                            null,
                            (h) => () =>
                            {
                                SetAnchor(Center);
                                var key = passiveAbilities.ToList()[index + 6 * regionGroup.pagination()].Key;
                                PrintAbilityTooltip(currentSave.player, key, passiveAbilities[key]);
                            }
                        );
                    }
                    else
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled");
                    }
                });
            }
            AddRegionGroup();
            SetRegionGroupWidth(86);
            AddButtonRegion(() => AddLine("Activated", "", "Center"), (h) =>
            {
                CloseWindow("SpellbookAbilityListPassive");
                Respawn("SpellbookAbilityListActivated");
            });
            AddRegionGroup();
            SetRegionGroupWidth(85);
            AddPaddingRegion(() => AddLine("Passive", "", "Center"));
        }),
        new("SpellbookResources", () => {
            SetAnchor(-301, -29);
            AddHeaderGroup();
            SetRegionGroupWidth(171);
            AddHeaderRegion(() => { AddLine("Starting elements:"); });
            var elements = new List<string> { "Fire", "Water", "Earth", "Air", "Frost" };
            AddRegionGroup();
            foreach (var element in elements)
                AddHeaderRegion(() => AddSmallButton("Element" + element + "Rousing"));
            AddRegionGroup();
            SetRegionGroupWidth(66);
            foreach (var element in elements)
                AddHeaderRegion(() =>
                {
                    var value = 0;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value > currentSave.player.MaxResource(element) ? "Red" : "Green"));
                    AddText(" / " + currentSave.player.MaxResource(element), "DarkGray");
                });
            elements = new List<string> { "Lightning", "Arcane", "Decay", "Order", "Shadow" };
            AddRegionGroup();
            foreach (var element in elements)
                AddHeaderRegion(() => AddSmallButton("Element" + element + "Rousing"));
            AddRegionGroup();
            SetRegionGroupWidth(67);
            foreach (var element in elements)
                AddHeaderRegion(() =>
                {
                    var value = 0;
                    AddLine(value + "", value == 0 ? "DarkGray" : (value > currentSave.player.MaxResource(element) ? "Red" : "Green"));
                    AddText(" / " + currentSave.player.MaxResource(element), "DarkGray");
                });
        }, true),
        new("PlayerSpellbookInfo", () => {
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(171);
            SetRegionGroupHeight(148);
            AddHeaderRegion(() =>
            {
                AddLine(currentSave.player.currentActionSet + " action bars:");
                var list = new List<string> { "Moonkin Form", "Bear Form", "Shadowform" };
                if (currentSave.player.abilities.Any(x => list.Any(y => y == x.Key)))
                    if (!WindowUp("AbilitiesSort") && !WindowUp("SwitchActionBars"))
                        AddSmallButton("OtherSwitch", (h) =>
                        {
                            SpawnWindowBlueprint("SwitchActionBars");
                            CloseWindow("PlayerSpellbookInfo");
                            Respawn("PlayerSpellbookInfo");
                            if (CloseWindow("SpellbookAbilityListActivated"))
                                Respawn("SpellbookAbilityListActivated");
                            if (CloseWindow("SpellbookAbilityListPassive"))
                                Respawn("SpellbookAbilityListPassive");
                        });
                    else
                        AddSmallButton("OtherSwitchOff");
            });
            var bars = currentSave.player.actionBars[currentSave.player.currentActionSet];
            int amount = currentSave.player.ActionBarsAmount();
            for (int i = 0; i < amount; i++)
            {
                var index = i;
                var abilityObj = bars.Count <= index ? null : abilities.Find(x => x.name == bars[index]);
                if (abilityObj != null)
                    AddButtonRegion(
                        () =>
                        {
                            AddLine(abilityObj.name, "", "Right");
                            AddSmallButton(abilityObj.icon);
                        },
                        (h) =>
                        {
                            if (currentSave.player.abilities.ContainsKey(bars[index]))
                            {
                                bars.RemoveAt(index);
                                Respawn("SpellbookAbilityListActivated", true);
                                Respawn("SpellbookAbilityListPassive", true);
                                Respawn("PlayerSpellbookInfo");
                                PlaySound("DesktopActionBarRemove", 0.9f);
                            }
                            else PlaySound("DesktopCantClick");
                        },
                        null,
                        (h) => () =>
                        {
                            PrintAbilityTooltip(currentSave.player, abilityObj, currentSave.player.abilities.ContainsKey(abilityObj.name) ? currentSave.player.abilities[abilityObj.name] : 0);
                        }
                    );
                else
                    AddHeaderRegion(
                        () =>
                        {
                            AddLine("", "Black");
                            AddSmallButton("OtherEmpty");
                        }
                    );
            }
            if (amount < 7) AddPaddingRegion(() => { });
        }),
        new("SwitchActionBars", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Select action set:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("SwitchActionBars");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Default", "Black", "Center");
            },
            (h) =>
            {
                currentSave.player.currentActionSet = "Default";
                CloseWindow("SwitchActionBars");
                CDesktop.RespawnAll();
            });
            var list = new List<string> { "Moonkin Form", "Bear Form", "Shadowform" };
            foreach (var set in list)
                if (currentSave.player.abilities.ContainsKey(set))
                    AddButtonRegion(() =>
                    {
                        AddLine(set, "Black", "Center");
                    },
                    (h) =>
                    {
                        currentSave.player.currentActionSet = set;
                        if (!currentSave.player.actionBars.ContainsKey(set))
                        {
                            currentSave.player.actionBars.Add(set, new());
                            if (set == "Bear Form") currentSave.player.actionBars[set].Add("Leave Bear Form");
                            else if (set == "Moonkin Form") currentSave.player.actionBars[set].Add("Leave Moonkin Form");
                            else if (set == "Shadowform") currentSave.player.actionBars[set].Add("Leave Shadowform");
                        }
                        CloseWindow("SwitchActionBars");
                        CloseWindow("PlayerSpellbookInfo");
                        CDesktop.RespawnAll();
                        Respawn("PlayerSpellbookInfo");
                    });
        }),

        //Talents
        new("TalentScreenHeader", () => {
            SetAnchor(Top, 0, -19);
            AddRegionGroup();
            AddHeaderRegion(
                () =>
                {
                    AddSmallButton("OtherPreviousPage", (h) =>
                    {
                        PlaySound("DesktopSwitchPage");
                        currentSave.lastVisitedTalents--;
                        if (currentSave.lastVisitedTalents < 0)
                            currentSave.lastVisitedTalents = currentSave.player.Spec().talentTrees.Count - 1;
                        CloseDesktop("TalentScreen");
                        SpawnDesktopBlueprint("TalentScreen");
                    });
                }
            );
            AddRegionGroup();
            SetRegionGroupWidth(220);
            AddHeaderRegion(
                () =>
                {
                    AddLine(currentSave.player.Spec().talentTrees[currentSave.lastVisitedTalents].name, "", "Center");
                }
            );
            AddRegionGroup();
            AddHeaderRegion(
                () =>
                {
                    AddSmallButton("OtherNextPage", (h) =>
                    {
                        PlaySound("DesktopSwitchPage");
                        currentSave.lastVisitedTalents++;
                        if (currentSave.lastVisitedTalents >= currentSave.player.Spec().talentTrees.Count)
                            currentSave.lastVisitedTalents = 0;
                        CloseDesktop("TalentScreen");
                        SpawnDesktopBlueprint("TalentScreen");
                    });
                }
            );
        }),
        new("TalentScreenFooter", () => {
            SetAnchor(-130, -143);
            AddRegionGroup();
            SetRegionGroupWidth(258);
            AddPaddingRegion(
                () =>
                {
                    AddLine(">> " + (currentSave.player.TreeCompletion(currentSave.lastVisitedTalents, 1) + currentSave.player.TreeCompletion(currentSave.lastVisitedTalents, 0)) + " / " + (currentSave.player.TreeSize(currentSave.lastVisitedTalents, 1) + currentSave.player.TreeSize(currentSave.lastVisitedTalents, 0)) + " <<", "", "Center");
                }
            );
        }),
        new("TalentTreeRight", () => {
            SetAnchor(TopRight, 0, -19);
            var specShadow = new GameObject("SpecShadow", typeof(SpriteRenderer));
            specShadow.transform.parent = CDesktop.LBWindow().transform;
            specShadow.transform.localPosition = new Vector3(2, -2, 0.1f);
            specShadow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/SpecShadow");
            DisableShadows();
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(309);
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                SetRegionBackgroundAsImage("Specs/" + currentSave.player.spec.Clean() + currentSave.player.Spec().talentTrees[currentSave.lastVisitedTalents].name.Clean() + "Right");
                if (currentSave.player.TreeCompletion(currentSave.lastVisitedTalents, 0) < defines.adeptTreeRequirement)
                    SetRegionBackgroundToGrayscale();
            });
            AddHeaderRegion(() =>
            {
                AddLine(currentSave.player.TreeCompletion(currentSave.lastVisitedTalents, 1) + "", "", "Center");
                AddText(" / ", "DarkGray");
                AddText(currentSave.player.TreeSize(currentSave.lastVisitedTalents, 1) + "");
            });
        }),
        new("TalentTreeLeft", () => {
            SetAnchor(TopLeft, 0, -19);
            var specShadow = new GameObject("SpecShadow", typeof(SpriteRenderer));
            specShadow.transform.parent = CDesktop.LBWindow().transform;
            specShadow.transform.localPosition = new Vector3(2, -2, 0.1f);
            specShadow.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/SpecShadow");
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(309);
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
                SetRegionBackgroundAsImage("Specs/" + currentSave.player.spec.Clean() + currentSave.player.Spec().talentTrees[currentSave.lastVisitedTalents].name.Clean() + "Left");
            });
            AddHeaderRegion(() =>
            {
                AddLine(currentSave.player.TreeCompletion(currentSave.lastVisitedTalents, 0) + "", "", "Center");
                AddText(" / ", "DarkGray");
                AddText(currentSave.player.TreeSize(currentSave.lastVisitedTalents, 0) + "");
            });
        }),

        //Console
        new("Console", () => {
            SetAnchor(Top);
            if (WindowUp("MapToolbar"))
                DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(638);
            AddPaddingRegion(() =>
            {
                AddInputLine(String.consoleInput);
            });
        }, true),

        //Card Game
        new("CardTest", () => {
            SetAnchor(BottomLeft, 19, 35);
            AddHeaderGroup();
            AddHeaderRegion(() =>
            {
                AddBigButton("PortraitScarletWizard", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
                AddBigButton("PortraitScarletMonk", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
                AddBigButton("PortraitScarletEnchanter", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddBigButton("PortraitScarletAugur", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
                AddBigButton("PortraitScarletCavalier", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
                AddBigButton("PortraitScarletExecutioner", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddBigButton("PortraitBlackhandVeteran", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
                AddBigButton("PortraitBlackhandIronGuard", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
                AddBigButton("PortraitBlackrockSlayer", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddBigButton("PortraitBlackrockWarlock", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
                AddBigButton("PortraitBlackrockWarlock", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
                AddBigButton("PortraitBlackhandIncarcerator", (h) =>
                {
                    PlaySound("DesktopMenuClose");
                });
            });
        }, true),
    };

    public static List<Blueprint> desktopBlueprints = new()
    {
        new("TitleScreen", () => 
        {
            PlayAmbience("AmbienceMainScreen", 0.2f, true);
            SpawnWindowBlueprint("TitleScreenMenu");
            AddHotkey(BackQuote, () =>
            {
                if (SpawnWindowBlueprint("Console") != null)
                {
                    PlaySound("DesktopTooltipShow", 0.4f);
                    CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                }
            });
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("GameSettings"))
                {
                    PlaySound("DesktopButtonClose");
                    SpawnWindowBlueprint("TitleScreenMenu");
                }
            });
        }),
        new("LoginScreen", () =>
        {
            SpawnWindowBlueprint("CharacterRoster");
            SpawnWindowBlueprint("CharacterInfo");
            SpawnWindowBlueprint("EnterWorld");
            AddHotkey(BackQuote, () =>
            {
                if (SpawnWindowBlueprint("Console") != null)
                {
                    PlaySound("DesktopTooltipShow", 0.4f);
                    CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                }
            });
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("ConfirmDeleteCharacter"))
                {
                    PlaySound("DesktopButtonClose");
                    CDesktop.RespawnAll();
                }
                else
                {
                    PlaySound("DesktopButtonClose");
                    CloseDesktop("LoginScreen");
                }
            });
        }),
        new("ChangeRealm", () =>
        {
            SetDesktopBackground("Backgrounds/Sky");
            SpawnWindowBlueprint("RealmRoster");
            AddHotkey(BackQuote, () =>
            {
                if (SpawnWindowBlueprint("Console") != null)
                {
                    PlaySound("DesktopTooltipShow", 0.4f);
                    CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                }
            });
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopButtonClose");
                CloseDesktop("ChangeRealm");
                CloseDesktop("LoginScreen");
                SpawnDesktopBlueprint("LoginScreen");
            });
        }),
        new("CharCreatorScreen", () =>
        {
            SetDesktopBackground("Backgrounds/Sky");
            SpawnWindowBlueprint("CharacterCreationFactionHorde");
            SpawnWindowBlueprint("CharacterCreationFactionAlliance");
            SpawnWindowBlueprint("CharacterCreationFactionRaceChoice");
            SpawnWindowBlueprint("CharacterCreationFinish");
            SpawnWindowBlueprint("CharacterCreationWho");
            AddHotkey(BackQuote, () =>
            {
                if (SpawnWindowBlueprint("Console") != null)
                {
                    PlaySound("DesktopTooltipShow", 0.4f);
                    CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                }
            });
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopButtonClose");
                CloseDesktop("CharCreatorScreen");
            });
        }),
        new("Map", () => 
        {
            PlaySound("DesktopOpenSave", 0.4f);
            SetDesktopBackground("LoadingScreens/" + (CDesktop.cameraDestination.x < 2470 ? "Kalimdor" : "EasternKingdoms"));
            loadingBar = new GameObject[2];
            loadingBar[0] = new GameObject("LoadingBarBegin", typeof(SpriteRenderer));
            loadingBar[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/LoadingBarEnd");
            loadingBar[0].transform.position = new Vector3(-1171, 854);
            loadingBar[1] = new GameObject("LoadingBar", typeof(SpriteRenderer));
            loadingBar[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/LoadingBarStretch");
            loadingBar[1].transform.position = new Vector3(-1168, 854);
            OrderLoadingMap();
            AddHotkey(W, () => { MoveCamera(new Vector2(0, EuelerGrowth())); }, false);
            AddHotkey(A, () => { MoveCamera(new Vector2(-EuelerGrowth(), 0)); }, false);
            AddHotkey(S, () => { MoveCamera(new Vector2(0, -EuelerGrowth())); }, false);
            AddHotkey(D, () => { MoveCamera(new Vector2(EuelerGrowth(), 0)); }, false);
            AddHotkey(UpArrow, () =>
            {
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.y += (int)Math.Sqrt(EuelerGrowth());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Remove(find);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey(LeftArrow, () =>
            {
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.x -= (int)Math.Sqrt(EuelerGrowth());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey(DownArrow, () =>
            {
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.y -= (int)Math.Sqrt(EuelerGrowth());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey(RightArrow, () =>
            {
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.x += (int)Math.Sqrt(EuelerGrowth());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopMenuOpen", 0.6f);
                SpawnDesktopBlueprint("GameMenu");
            });
            AddHotkey(Delete, () =>
            {
                if (sitePathBuilder != null)
                {
                    UnityEngine.Object.Destroy(pathTest.Item2);
                    sitePathBuilder = null;
                }
            });
            AddHotkey(BackQuote, () =>
            {
                if (SpawnWindowBlueprint("Console") != null)
                {
                    PlaySound("DesktopTooltipShow", 0.4f);
                    CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                }
            });
            AddHotkey(KeyCode.Space, () =>
            {
                var whereTo = FindSite(x => x.name == currentSave.currentSite);
                CDesktop.cameraDestination = new Vector2(whereTo.x, whereTo.y);
            });

            void MoveCamera(Vector2 amount)
            {
                var temp = CDesktop.cameraDestination + amount * 2;
                //temp = new Vector2Int((int)temp.x, (int)temp.y) / mapGridSize;
                CDesktop.cameraDestination = new Vector2(temp.x, temp.y);
            }
        }),
        new("HostileArea", () => 
        {
            SetDesktopBackground(area.Background());
            SpawnWindowBlueprint("HostileArea");
            SpawnWindowBlueprint("HostileAreaProgress");
            SpawnWindowBlueprint("HostileAreaDenizens");
            SpawnWindowBlueprint("HostileAreaElites");
            SpawnWindowBlueprint("HostileAreaQuestAvailable");
            SpawnWindowBlueprint("HostileAreaQuestDone");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            SpawnWindowBlueprint("Chest");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("HostileAreaQuestTracker"))
                {
                    PlaySound("DesktopButtonClose");
                    Respawn("HostileArea");
                    Respawn("HostileAreaQuestAvailable");
                }
                else if (area.complexPart)
                {
                    CloseDesktop("HostileArea");
                    SpawnDesktopBlueprint("Complex");
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("HostileArea");
                }
            });
        }),
        new("CombatResults", () => 
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("CombatResults");
            SpawnWindowBlueprint("CombatResultsChartButton");
            SpawnWindowBlueprint("CombatResultsMining");
            SpawnWindowBlueprint("CombatResultsHerbalism");
            SpawnWindowBlueprint("CombatResultsSkinning");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                if (board.results.result == "Team1Won")
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("CombatResults");
                }
            });
        }),
        new("CombatLog", () => 
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("CombatResultsChart");
            SpawnWindowBlueprint("CombatResultsChartLeftArrow");
            SpawnWindowBlueprint("CombatResultsChartRightArrow");
            FillChart();
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(A, () =>
            {
                PlaySound("DesktopChartSwitch");
                if (chartPage == "Damage Taken") chartPage = "Damage Dealt";
                else if (chartPage == "Healing Received") chartPage = "Damage Taken";
                else if (chartPage == "Elements Used") chartPage = "Healing Received";
                else if (chartPage == "Damage Dealt") chartPage = "Elements Used";
                CloseDesktop("CombatLog");
                SpawnDesktopBlueprint("CombatLog");
            });
            AddHotkey(D, () =>
            {
                PlaySound("DesktopChartSwitch");
                if (chartPage == "Damage Dealt") chartPage = "Damage Taken";
                else if (chartPage == "Damage Taken") chartPage = "Healing Received";
                else if (chartPage == "Healing Received") chartPage = "Elements Used";
                else if (chartPage == "Elements Used") chartPage = "Damage Dealt";
                CloseDesktop("CombatLog");
                SpawnDesktopBlueprint("CombatLog");
            });
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInstanceClose");
                CloseDesktop("CombatLog");
            });
        }),
        new("CombatResultsLoot", () => 
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("CombatResultsLoot");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInventoryClose");
                CloseDesktop("CombatResultsLoot");
                SwitchDesktop("CombatResults");
                Respawn("CombatResults");
            });
        }),
        new("ContainerLoot", () =>
        {
            SetDesktopBackground("Backgrounds/Leather");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("ContainerLoot");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                if (openedItem.itemsInside.Count == 0)
                    currentSave.player.inventory.items.Remove(openedItem);
                openedItem = null;
                PlaySound("DesktopInventoryClose");
                CloseDesktop("ContainerLoot");
                SpawnDesktopBlueprint("EquipmentScreen");
            });
        }),
        new("MiningLoot", () =>
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("MiningLoot");
            var s = currentSave.player.professionSkills["Mining"];
            if (!board.results.miningSkillChange)
            {
                currentSave.AddTime(30);
                board.results.miningSkillChange = true;
                if (board.results.miningNode.Item2 + 50 >= s.Item1 && s.Item1 < professions.Find(x => x.name == "Mining").levels.FindAll(x => s.Item2.Contains(x.name)).Max(x => x.maxSkill))
                {
                    currentSave.player.professionSkills["Mining"] = (s.Item1 + 1, s.Item2);
                    SpawnFallingText(new Vector2(0, 34), "Mining increased to " + (s.Item1 + 1), "Blue");
                }
            }
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInventoryClose");
                CloseDesktop("MiningLoot");
                Respawn("CombatResultsMining");
            });
        }),
        new("HerbalismLoot", () =>
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("HerbalismLoot");
            if (!board.results.herbalismSkillChange)
            {
                var s = currentSave.player.professionSkills["Herbalism"];
                currentSave.AddTime(30);
                board.results.herbalismSkillChange = true;
                if (board.results.herb.Item2 + 50 >= s.Item1 && s.Item1 < professions.Find(x => x.name == "Herbalism").levels.FindAll(x => s.Item2.Contains(x.name)).Max(x => x.maxSkill))
                {
                    currentSave.player.professionSkills["Herbalism"] = (s.Item1 + 1, s.Item2);
                    SpawnFallingText(new Vector2(0, 34), "Herbalism increased to " + (s.Item1 + 1), "Blue");
                }
            }
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInventoryClose");
                CloseDesktop("HerbalismLoot");
                Respawn("CombatResultsHerbalism");
            });
        }),
        new("SkinningLoot", () =>
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("SkinningLoot");
            var s = currentSave.player.professionSkills["Skinning"];
            if (!board.results.skinningSkillChange)
            {
                currentSave.AddTime(30);
                board.results.skinningSkillChange = true;
                if (board.results.skinningNode.Item2 + 50 >= s.Item1 && s.Item1 < professions.Find(x => x.name == "Skinning").levels.FindAll(x => s.Item2.Contains(x.name)).Max(x => x.maxSkill))
                {
                    currentSave.player.professionSkills["Skinning"] = (s.Item1 + 1, s.Item2);
                    SpawnFallingText(new Vector2(0, 34), "Skinning increased to " + (s.Item1 + 1), "Blue");
                }
            }
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInventoryClose");
                CloseDesktop("SkinningLoot");
                Respawn("CombatResultsSkinning");
            });
        }),
        new("ChestLoot", () => 
        {
            SetDesktopBackground(area.Background());
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("ChestInfo");
            SpawnWindowBlueprint("ChestLoot");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopCloseChest");
                CloseDesktop("ChestLoot");
            });
        }),
        new("DisenchantLoot", () => 
        {
            SetDesktopBackground("Backgrounds/Leather");
            SpawnWindowBlueprint("DisenchantLoot");
            var s = currentSave.player.professionSkills["Enchanting"];
            if (!enchantingSkillChange)
            {
                currentSave.AddTime(20);
                enchantingSkillChange = true;
                if (s.Item1 < 70 && s.Item1 < professions.Find(x => x.name == "Enchanting").levels.FindAll(x => s.Item2.Contains(x.name)).Max(x => x.maxSkill))
                {
                    currentSave.player.professionSkills["Enchanting"] = (s.Item1 + 1, s.Item2);
                    SpawnFallingText(new Vector2(0, 34), "Enchanting increased to " + (s.Item1 + 1), "Blue");
                }
            }
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                disenchantLoot = null;
                enchantingSkillChange = false;
                CloseDesktop("DisenchantLoot");
            });
        }),
        new("Town", () => 
        {
            personCategory = null;
            SetDesktopBackground(town.Background());
            SpawnWindowBlueprint("TownQuestAvailable");
            SpawnWindowBlueprint("TownQuestDone");
            SpawnWindowBlueprint("PlayerMoney");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddPaginationHotkeys();
            if (currentSave.player.Reputation(town.faction) >= 4200)
            {
                SpawnWindowBlueprint("Town");
                AddHotkey(Tab, () =>
                {
                    if (CloseWindow("Vendor"))
                    {
                        PlaySound("VendorSwitchTab");
                        Respawn("VendorBuyback");
                    }
                    else if (CloseWindow("VendorBuyback"))
                    {
                        PlaySound("VendorSwitchTab");
                        Respawn("Vendor");
                    }
                });
                AddHotkey(Escape, () =>
                {
                    if (CloseWindow("MountCollection"))
                    {
                        PlaySound("DesktopInstanceClose");
                        CloseWindow("CurrentMount");
                        Respawn("Person");
                    }
                    else if (CloseWindow("Inventory"))
                    {
                        PlaySound("DesktopInventoryClose");
                        CloseWindow("Bank");
                        CloseWindow("Vendor");
                        CloseWindow("VendorBuyback");
                        Respawn("PlayerMoney", true);
                        Respawn("Person");
                    }
                    else if (CloseWindow("MakeInnHome"))
                    {
                        PlaySound("DesktopInstanceClose");
                        CloseWindow("MakeInnHome");
                        Respawn("Person");
                    }
                    else if (CloseWindow("ResetTalents"))
                    {
                        PlaySound("DesktopInstanceClose");
                        CloseWindow("ResetTalents");
                        Respawn("Person");
                    }
                    else if (CloseWindow("FlightMaster"))
                    {
                        PlaySound("DesktopInstanceClose");
                        Respawn("Person");
                    }
                    else if (CloseWindow("ProfessionRecipeTrainer"))
                    {
                        PlaySound("DesktopInstanceClose");
                        CloseWindow("ProfessionRecipeTrainer");
                        Respawn("Person");
                    }
                    else if (CloseWindow("ProfessionLevelTrainer"))
                    {
                        PlaySound("DesktopInstanceClose");
                        CloseWindow("ProfessionLevelTrainer");
                        Respawn("Person");
                    }
                    else if (CloseWindow("Person"))
                    {
                        PlaySound("DesktopInstanceClose");
                        person = null;
                        if (personCategory != null) Respawn("Persons");
                        Respawn("Town");
                        Respawn("Persons", true);
                        Respawn("TownQuestAvailable");
                        Respawn("TownQuestDone");
                    }
                    else if (CloseWindow("Persons"))
                    {
                        PlaySound("DesktopInstanceClose");
                        personCategory = null;
                        Respawn("Town");
                    }
                    else
                    {
                        PlaySound("DesktopInstanceClose");
                        town = null;
                        CloseDesktop("Town");
                    }
                });
            }
            else
            {

                SpawnWindowBlueprint("TownHostile");
                AddHotkey(Escape, () =>
                {
                    PlaySound("DesktopInstanceClose");
                    town = null;
                    CloseDesktop("Town");
                });
            }
        }),
        new("Instance", () => 
        {
            SetDesktopBackground(instance.Background());
            if (wing != null) SpawnWindowBlueprint("InstanceWing");
            SpawnWindowBlueprint("Instance");
            SpawnWindowBlueprint("InstanceQuestAvailable");
            SpawnWindowBlueprint("InstanceQuestDone");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("HostileAreaQuestTracker"))
                {
                    PlaySound("DesktopButtonClose");
                    Respawn("HostileArea");
                    Respawn("HostileAreaQuestAvailable");
                }
                else if (CloseWindow("HostileArea"))
                {
                    area = null;
                    CloseWindow("HostileAreaQuestAvailable");
                    CloseWindow("HostileAreaProgress");
                    CloseWindow("HostileAreaDenizens");
                    CloseWindow("HostileAreaElites");
                    CloseWindow("Chest");
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground(instance.Background());
                    if (wing != null) SetDesktopBackground(wing.Background());
                    SpawnWindowBlueprint("InstanceQuestAvailable");
                }
                else if (CloseWindow("InstanceWing"))
                {
                    wing = null;
                    Respawn("Instance");
                    PlaySound("DesktopButtonClose");
                    SetDesktopBackground(instance.Background());
                }
                else if (instance.complexPart)
                {
                    CloseDesktop("Instance");
                    SpawnDesktopBlueprint("Complex");
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("Instance");
                }
            });
            AddPaginationHotkeys();
        }),
        new("Complex", () => 
        {
            SetDesktopBackground(complex.Background());
            SpawnWindowBlueprint("Complex");
            SpawnWindowBlueprint("ComplexQuestAvailable");
            SpawnWindowBlueprint("ComplexQuestDone");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("HostileAreaQuestTracker"))
                {
                    PlaySound("DesktopButtonClose");
                    Respawn("HostileArea");
                    Respawn("HostileAreaQuestAvailable");
                }
                else if (CloseWindow("HostileArea"))
                {
                    area = null;
                    PlaySound("DesktopButtonClose");
                    CloseWindow("HostileAreaQuestAvailable");
                    CloseWindow("HostileAreaProgress");
                    CloseWindow("HostileAreaDenizens");
                    CloseWindow("HostileAreaElites");
                    CloseWindow("Chest");
                    SetDesktopBackground(complex.Background());
                    SpawnWindowBlueprint("ComplexQuestAvailable");
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("Complex");
                }
            });
        }),
        new("Game", () => 
        {
            SpawnTransition();
            PlaySound("DesktopEnterCombat");
            SetDesktopBackground(board.area.Background());
            board.Reset();
            SpawnWindowBlueprint("BoardFrame");
            SpawnWindowBlueprint("Board");
            SpawnWindowBlueprint("BufferBoard");
            SpawnWindowBlueprint("PlayerBattleInfo");
            SpawnWindowBlueprint("LocationInfo");
            SpawnWindowBlueprint("EnemyBattleInfo");
            SpawnWindowBlueprint("PlayerQuickUse");
            var elements = new List<string> { "Fire", "Water", "Earth", "Air", "Frost", "Lightning", "Arcane", "Decay", "Order", "Shadow" };
            foreach (var element in elements)
            {
                SpawnWindowBlueprint("Player" + element + "Resource");
                SpawnWindowBlueprint("Enemy" + element + "Resource");
            }
            AddHotkey(PageUp, () => {
                board.participants[0].who.resources = new Dictionary<string, int>
                {
                    { "Earth", 99 },
                    { "Fire", 99 },
                    { "Air", 99 },
                    { "Water", 99 },
                    { "Frost", 99 },
                    { "Lightning", 99 },
                    { "Arcane", 99 },
                    { "Decay", 99 },
                    { "Order", 99 },
                    { "Shadow", 99 },
                };
                Respawn("PlayerBattleInfo");
                board.UpdateResourceBars(0, elements);
            });
            AddHotkey(PageDown, () => {
                board.participants[1].who.resources = new Dictionary<string, int>
                {
                    { "Earth", 99 },
                    { "Fire", 99 },
                    { "Air", 99 },
                    { "Water", 99 },
                    { "Frost", 99 },
                    { "Lightning", 99 },
                    { "Arcane", 99 },
                    { "Decay", 99 },
                    { "Order", 99 },
                    { "Shadow", 99 },
                };
                Respawn("EnemyBattleInfo");
                board.UpdateResourceBars(1, elements);
            });
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopMenuOpen", 0.6f);
                SpawnDesktopBlueprint("GameMenu");
            });
            AddHotkey(BackQuote, () => { SpawnDesktopBlueprint("DevPanel"); });
            AddHotkey(KeypadMultiply, () => { board.EndCombat("Team1Won"); });
            AddHotkey(KeypadDivide, () => { board.EndCombat("Team2Won"); });
        }),
        new("FishingGame", () => 
        {
            SpawnTransition();
            PlaySound("DesktopEnterCombat");
            SetDesktopBackground(FindSite(x => x.name == fishingSpot.name).Background());
            SpawnWindowBlueprint("FishingBoardFrame");
            SpawnWindowBlueprint("FishingSpotInfo");
            SpawnWindowBlueprint("FisherInfo");
            SpawnWindowBlueprint("LocationInfo");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopMenuOpen", 0.6f);
                SpawnDesktopBlueprint("GameMenu");
            });
            AddHotkey(KeypadMultiply, () => { fishingSpot.EndFishing("Team1Won"); });
            AddHotkey(KeypadDivide, () => { fishingSpot.EndFishing("Team2Won"); });
        }),
        new("CharacterSheet", () => 
        {
            PlaySound("DesktopCharacterSheetOpen");
            SetDesktopBackground("Backgrounds/RedWood");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("CharacterInfoStats");
            SpawnWindowBlueprint("CharacterInfoStatsRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopCharacterSheetClose");
                CloseDesktop("CharacterSheet");
            });
        }),
        new("QuestLog", () => 
        {
            PlaySound("DesktopInventoryOpen");
            SetDesktopBackground("Backgrounds/RuggedLeather");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            SpawnWindowBlueprint("QuestList");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("QuestSort"))
                {
                    PlaySound("DesktopInstanceClose");
                    Respawn("QuestList");
                }
                else if (CloseWindow("QuestSettings"))
                {
                    PlaySound("DesktopInstanceClose");
                    Respawn("QuestList");
                }
                else if (CloseWindow("QuestConfirmAbandon"))
                {
                    PlaySound("DesktopMenuClose");
                    Respawn("Quest");
                    Respawn("QuestList");
                }
                else if (CloseWindow("Quest"))
                {
                    quest = null;
                    Respawn("QuestList");
                    PlaySound("DesktopInstanceClose");
                }
                else
                {
                    PlaySound("DesktopSpellbookClose");
                    CloseDesktop("QuestLog");
                }
            });
            AddPaginationHotkeys();
        }),
        new("TalentScreen", () => 
        {
            SetDesktopBackground("Backgrounds/Stone");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("TalentTreeLeft");
            SpawnWindowBlueprint("TalentScreenHeader");
            SpawnWindowBlueprint("TalentTreeRight");
            var playerSpec = currentSave.player.Spec();
            for (int tree = 0; tree < 2; tree++)
                for (int row = 0; row < 5; row++)
                    for (int col = 0; col < 3; col++)
                        if (windowBlueprints.Exists(x => x.title == "TalentButton" + tree + row + col))
                            if (playerSpec.talentTrees[currentSave.lastVisitedTalents].talents.Exists(x => x.row == row && x.col == col && x.tree == tree))
                                SpawnWindowBlueprint("TalentButton" + tree + row + col);
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(A, () =>
            {
                PlaySound("DesktopSwitchPage");
                currentSave.lastVisitedTalents--;
                if (currentSave.lastVisitedTalents < 0)
                    currentSave.lastVisitedTalents = currentSave.player.Spec().talentTrees.Count - 1;
                CloseDesktop("TalentScreen");
                SpawnDesktopBlueprint("TalentScreen");
            });
            AddHotkey(D, () =>
            {
                PlaySound("DesktopSwitchPage");
                currentSave.lastVisitedTalents++;
                if (currentSave.lastVisitedTalents >= currentSave.player.Spec().talentTrees.Count)
                    currentSave.lastVisitedTalents = 0;
                CloseDesktop("TalentScreen");
                SpawnDesktopBlueprint("TalentScreen");
            });
            AddHotkey(Escape, () => { CloseDesktop("TalentScreen"); PlaySound("DesktopTalentScreenClose"); });
        }),
        new("SpellbookScreen", () => 
        {
            currentSave.player.currentActionSet = "Default";
            PlaySound("DesktopSpellbookOpen");
            SetDesktopBackground("Backgrounds/Skin");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("SpellbookAbilityListActivated");
            SpawnWindowBlueprint("PlayerSpellbookInfo");
            SpawnWindowBlueprint("SpellbookResources");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () => { CloseDesktop("SpellbookScreen"); PlaySound("DesktopSpellbookClose"); });
            AddPaginationHotkeys();
        }),
        new("EquipmentScreen", () => 
        {
            openedItem = null;
            itemToDestroy = null;
            itemToDisenchant = null;
            PlaySound("DesktopInventoryOpen");
            SetDesktopBackground("Backgrounds/Leather");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInventoryClose");
                CloseDesktop("EquipmentScreen");
            });
        }),
        new("BestiaryScreen", () => 
        {
            PlaySound("DesktopInstanceOpen");
            SetDesktopBackground("Backgrounds/SkinDark");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("BestiaryKalimdor");
            SpawnWindowBlueprint("BestiaryEasternKingdoms");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopInstanceClose");
                CloseDesktop("BestiaryScreen");
            });
        }),
        new("CraftingScreen", () => 
        {
            PlaySound("DesktopSpellbookOpen");
            SetDesktopBackground("Backgrounds/Professions");
            SpawnWindowBlueprint("ProfessionListPrimary");
            SpawnWindowBlueprint("ProfessionListSecondary");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("MapToolbarStatusLeft");
            SpawnWindowBlueprint("MapToolbarStatusRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("CraftingRecipe"))
                {
                    PlaySound("DesktopInstanceClose");
                }
                else if (CloseWindow("CraftingList"))
                {
                    Respawn("ProfessionListPrimary");
                    Respawn("ProfessionListSecondary");
                    PlaySound("DesktopInstanceClose");
                    SetDesktopBackground("Backgrounds/Professions");
                }
                else
                {
                    PlaySound("DesktopSpellbookClose");
                    CloseDesktop("CraftingScreen");
                }
            });
            AddPaginationHotkeys();
        }),
        new("GameMenu", () => 
        {
            SetDesktopBackground("Backgrounds/StoneFull");
            SpawnWindowBlueprint("GameMenu");
            AddHotkey(Escape, () =>
            {
                if (CloseWindow("Settings"))
                {
                    PlaySound("DesktopButtonClose");
                    SpawnWindowBlueprint("GameMenu");
                }
                else
                {
                    PlaySound("DesktopMenuClose");
                    CloseDesktop("GameMenu");
                    if (CDesktop.title == "EquipmentScreen")
                    {
                        Respawn("PlayerEquipmentInfo");
                        Respawn("Inventory");
                    }
                }
            });
        }),
        new("RankingScreen", () => 
        {
            SetDesktopBackground("Backgrounds/SkyRed");
            SpawnWindowBlueprint("CharacterRankingShadow");
            SpawnWindowBlueprint("CharacterRankingTop");
            SpawnWindowBlueprint("CharacterRankingList");
            SpawnWindowBlueprint("CharacterRankingListRight");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopButtonClose");
                CloseDesktop("RankingScreen");
            });
        }),
        new("CardGame", () =>
        {
            SetDesktopBackground("Backgrounds/SkyRed");
            SpawnWindowBlueprint("CardTest");
            SpawnWindowBlueprint("ExperienceBarBorder");
            AddHotkey(Escape, () =>
            {
                PlaySound("DesktopButtonClose");
                CloseDesktop("CardGame");
            });
        })
    };

    public static void AddPaginationHotkeys()
    {
        AddHotkey(D, () => 
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup != null && x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            if (group == null && window.headerGroup != null && window.headerGroup.maxPaginationReq != null)
                group = window.headerGroup;
            if (group == null) return;
            var temp = group.pagination();
            group.IncrementPagination();
            if (temp != group.pagination())
                PlaySound("DesktopChangePage", 0.6f);
            window.Respawn();
        });
        AddHotkey(D, () => 
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup != null && x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            if (group == null && window.headerGroup != null && window.headerGroup.maxPaginationReq != null)
                group = window.headerGroup;
            if (group == null) return;
            var temp = group.pagination();
            group.IncrementPaginationEuler();
            if (temp != group.pagination())
                PlaySound("DesktopChangePage", 0.6f);
            window.Respawn();
        }, false);
        AddHotkey(A, () => 
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup != null && x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            if (group == null && window.headerGroup != null && window.headerGroup.maxPaginationReq != null)
                group = window.headerGroup;
            if (group == null) return;
            var temp = group.pagination();
            group.DecrementPagination();
            if (temp != group.pagination())
                PlaySound("DesktopChangePage", 0.6f);
            window.Respawn();
        });
        AddHotkey(A, () => 
        {
            var window = CDesktop.windows.Find(x => x.regionGroups.Any(y => y.maxPaginationReq != null));
            if (window == null)
            {
                window = CDesktop.windows.Find(x => x.headerGroup != null && x.headerGroup.maxPaginationReq != null);
                if (window == null) return;
            }
            var group = window.regionGroups.Find(x => x.maxPaginationReq != null);
            if (group == null && window.headerGroup != null && window.headerGroup.maxPaginationReq != null)
                group = window.headerGroup;
            if (group == null) return;
            var temp = group.pagination();
            group.DecrementPaginationEuler();
            if (temp != group.pagination())
                PlaySound("DesktopChangePage", 0.6f);
            window.Respawn();
        }, false);
    }
}
