using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static UnityEngine.KeyCode;

using static Root.Anchor;
using static Root.RegionBackgroundType;

using static Item;
using static Root;
using static Race;
using static Site;
using static Spec;
using static Zone;
using static Quest;
using static Sound;
using static Mount;
using static Board;
using static Recipe;
using static Person;
using static Defines;
using static Enchant;
using static Faction;
using static Ability;
using static SitePath;
using static SaveGame;
using static SiteArea;
using static Coloring;
using static Inventory;
using static PersonType;
using static Profession;
using static SiteComplex;
using static FishingSpot;
using static SiteInstance;
using static GameSettings;
using static PersonCategory;

public class Blueprint
{
    public Blueprint(string title, Action actions, bool upperUI = false)
    {
        this.title = title;
        this.actions = actions;
        this.upperUI = upperUI;
    }

    //Title of this blueprint and to-be window for referencing
    public string title;

    //Draw actions of the blueprint that create the window
    public Action actions;

    //Whether window built from this blueprint will appear in front of others
    public bool upperUI;

    //List of all window blueprints in the game excluding:
    //1: Talent windows in talent screen
    //2: Resource windows in combat
    //3: Auction house price regions
    //Because these are generated at the launch of the game in class Starter
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
                for (int i = 0; i < areas.Count; i++)
                    allSites.Add(areas[i]);
                for (int i = 0; i < complexes.Count; i++)
                    allSites.Add(complexes[i]);
                for (int i = 0; i < instances.Count; i++)
                    allSites.Add(instances[i]);
                var side = currentSave.playerSide;
                var zonesExcluded = zones.FindAll(x => x.continent != "Eastern Kingdoms").Select(x => x.name);
                allSites.RemoveAll(x => x.x == 0 && x.y == 0 || zonesExcluded.Contains(x.zone));
                AddLine("Explored areas: " + allSites.Count(x => currentSave.siteVisits.ContainsKey(x.name)) + " / " + allSites.Count, "DarkGray", "Center");
                var commons = areas.FindAll(x => !zonesExcluded.Contains(x.zone)).SelectMany(x => x.CommonEncounters(side) ?? new()).Select(x => x.who).Distinct().ToList();
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
            AddRegionGroup();
            var boardBackground = new GameObject("BoardBackground", typeof(SpriteRenderer), typeof(SpriteMask));
            boardBackground.transform.parent = CDesktop.LBWindow().transform;
            boardBackground.transform.localPosition = new Vector2(-17, 17);
            boardBackground.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/BoardFrames/BoardBackground" + board.field.GetLength(0) + "x" + board.field.GetLength(1));
            var mask = boardBackground.GetComponent<SpriteMask>();
            mask.sprite = Resources.Load<Sprite>("Sprites/BoardFrames/BoardMask" + board.field.GetLength(0) + "x" + board.field.GetLength(1));
            mask.isCustomRangeActive = true;
            mask.frontSortingLayerID = SortingLayer.NameToID("ScaledBarsUpper");
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
                            if (abilityTargetted != null)
                            {
                                FinishTargettingAbility(J, I);
                            }
                            else
                            {
                                var list = board.FloodCount(h.region.bigButtons.FindIndex(x => x.GetComponent<Highlightable>() == h), h.region.regionGroup.regions.IndexOf(h.region));
                                board.finishedMoving = true;
                                board.FloodDestroy(list);
                                Respawn("FriendlyBattleInfo");
                            }
                        },
                        (h) => ClearTargettingAbility());
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
            SetAnchor(TopRight, 0, -8);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            for (int i = board.team2.Count - 1; i >= 0; i--)
            {
                var index = i;
                var participant = board.participants[board.team2[index]];
                AddButtonRegion(() =>
                {
                    AddLine(participant.who.name);
                    AddLine(board.team2[index] + 1 + "", "DarkGray", "Right");
                    SpawnFloatingText(new Vector3(34, -9), participant.who.level - 10 > currentSave.player.level ? "??" : "" + participant.who.level, ColorEntityLevel(currentSave.player, participant.who.level), "DimGray", "Right");
                    var race = races.Find(x => x.name == participant.who.race);
                    var actionSet = participant.who.currentActionSet != "Default" ? ActionSet.actionSets.Find(x => x.name == participant.who.currentActionSet) : null;
                    AddBigButton(actionSet != null && race.side == "Horde" && actionSet.hordePortraitReplacement != null ? actionSet.hordePortraitReplacement : (actionSet != null && race.side == "Alliance" && actionSet.alliancePortraitReplacement != null ? actionSet.alliancePortraitReplacement : (participant.who.spec != null ? "Portrait" + participant.who.race.Clean() + participant.who.gender + participant.who.portraitID : (race.portrait == "" ? "OtherUnknown" : race.portrait + (race.genderedPortrait ? participant.who.gender : "")))), (h) =>
                    {
                        if (abilityTargetted != null) FinishTargettingAbility(participant);
                        else ChangeSpotlight(index);
                    },
                    (h) => ClearTargettingAbility(), (h) => () => participant.who.PrintEntityTooltip());
                    if (participant.who.IsStealthed())
                    {
                        var button = CDesktop.LBWindow().LBRegionGroup().LBRegion().LBBigButton().gameObject;
                        AddBigButtonOverlay(button, "SneakingBig", 0, 1);
                    }
                    BigButtonFlipX();
                    if (participant.who.dead) SetBigButtonToGrayscale();
                    if (participant == board.participants[board.whosTurn])
                    {
                        var arrow = AddSmallButtonOverlay(CDesktop.LBWindow().LBRegionGroup().LBRegion().gameObject, participant.who.IsControlledByHuman() ? "PlayerLocationFromBelow" : (participant.who.IsControlledByOppositeTeam()  ? "FriendLocationFromBelow" : "EnemyLocationFromBelow"), 0, 1);
                        arrow.transform.localPosition = new Vector3(0.5f, -20.5f, 0);
                        arrow.transform.localEulerAngles = new Vector3(0, 0, -90);
                    }
                    AddHealthBar(40, -19, board.team2[index], participant.who);
                },
                (h) =>
                {
                    if (abilityTargetted != null) FinishTargettingAbility(participant);
                    else ChangeSpotlight(index);
                },
                (h) => ClearTargettingAbility(),
                (h) => () => participant.who.PrintEntityTooltip());
                foreach (var actionBar in participant.who.actionSets[participant.who.currentActionSet])
                {
                    var abilityObj = abilities.Find(x => x.name == actionBar);
                    if (abilityObj == null || abilityObj.cost == null) continue;
                    AddButtonRegion(
                        () =>
                        {
                            ReverseButtons();
                            if (board.cooldowns[board.team2[index]].ContainsKey(actionBar))
                            {
                                AddLine(actionBar, "Black");
                                AddText(" \\ " + board.cooldowns[board.team2[index]][actionBar], "DimGray");
                            }
                            else AddLine(actionBar);
                            AddSmallButton(abilityObj.icon);
                            if (!abilityObj.EnoughResources(board.participants[board.team2[index]].who))
                            {
                                SetSmallButtonToGrayscale();
                                AddSmallButtonOverlay("OtherGridBlurred");
                            }
                            if (board.CooldownOn(board.team2[index], actionBar) > 0)
                                AddSmallButtonOverlay("AutoCast");
                        },
                        (h) =>
                        {
                            if (board.team2[index] == board.whosTurn)
                            {
                                if (abilityTargetted != null)
                                    ClearTargettingAbility(true);
                                else if (abilityObj.EnoughResources(board.participants[board.team2[index]].who))
                                    if (board.CooldownOn(board.team2[index], actionBar) <= 0)
                                        StartTargettingAbility(abilityObj);
                            }
                            else ClearTargettingAbility();
                        },
                        (h) => ClearTargettingAbility(),
                        (h) => () =>
                        {
                            PrintAbilityTooltip(board.participants[board.team2[index]].who, abilityObj, board.participants[board.team2[index]].who.abilities[abilityObj.name]);
                        }
                    );
                }
                var item = participant.who.equipment.ContainsKey("Trinket") ? participant.who.equipment["Trinket"] : null;
                if (item != null && item.abilities != null && item.combatUse)
                {
                    var ability = item.abilities.ToList()[index];
                    var abilityObj = abilities.Find(x => x.name == ability.Key);
                    AddButtonRegion(
                        () =>
                        {
                            if (board.cooldowns[board.team2[index]].ContainsKey(ability.Key))
                            {
                                AddLine("" + board.cooldowns[board.team2[index]][ability.Key] + " / ", "DimGray", "Right");
                                AddText(ability.Key, "Black");
                            }
                            else AddLine(ability.Key, "", "Right");
                            AddSmallButton(item.icon);
                            if (!abilityObj.EnoughResources(participant.who))
                            {
                                SetSmallButtonToGrayscale();
                                AddSmallButtonOverlay("OtherGridBlurred");
                            }
                            if (board.CooldownOn(board.team2[index], ability.Key) > 0)
                                AddSmallButtonOverlay("AutoCast");
                        },
                        null,
                        null,
                        (h) => () =>
                        {
                            PrintAbilityTooltip(participant.who, abilityObj, participant.who.abilities[abilityObj.name], item);
                        }
                    );
                }
                if (index > 0)
                {
                    AddSmallEmptyRegion();
                    AddSmallEmptyRegion();
                }
            }

            void ChangeSpotlight(int index)
            {
                if (index == 0) return;
                var switchBuffsWith = board.team2[0];
                var temp = board.team2[index];
                board.team2.RemoveAt(index);
                board.team2.Insert(0, temp);
                foreach (var res in board.participants[board.team2[0]].who.resources)
                {
                    CloseWindow("Enemy" + res.Key + "Resource");
                    SpawnWindowBlueprint("Enemy" + res.Key + "Resource");
                }
                board.temporaryBuffs[switchBuffsWith].ForEach(x => x.GetComponent<FlyingBuff>().InstantMove());
                board.temporaryBuffs[temp].ForEach(x => x.GetComponent<FlyingBuff>().InstantMove());
            }
        }),
        new("FriendlyBattleInfo", () => {
            SetAnchor(TopLeft, 0, -8);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            for (int i = board.team1.Count - 1; i >= 0; i--)
            {
                var index = i;
                var participant = board.participants[board.team1[index]];
                AddButtonRegion(() =>
                {
                    ReverseButtons();
                    AddLine(participant.who.name, "", "Right");
                    AddLine(board.team1[index] + 1 + "", "DarkGray");
                    SpawnFloatingText(new Vector3(158, -9), participant.who.level + "", "Gray", "DimGray", "Left");
                    var race = races.Find(x => x.name == participant.who.race);
                    var actionSet = participant.who.currentActionSet != "Default" ? ActionSet.actionSets.Find(x => x.name == participant.who.currentActionSet) : null;
                    if (participant.who.spec != null)
                        AddBigButton(actionSet != null && race.side == "Horde" && actionSet.hordePortraitReplacement != null ? actionSet.hordePortraitReplacement : (actionSet != null && race.side == "Alliance" && actionSet.alliancePortraitReplacement != null ? actionSet.alliancePortraitReplacement : ("Portrait" + participant.who.race.Clean() + participant.who.gender + participant.who.portraitID)), (h) =>
                        {
                            if (abilityTargetted != null) FinishTargettingAbility(participant);
                            else ChangeSpotlight(index);
                        },
                        (h) => ClearTargettingAbility(), (h) => () => participant.who.PrintEntityTooltip());
                    else
                    {
                        AddBigButton(actionSet != null && race.side == "Horde" && actionSet.hordePortraitReplacement != null ? actionSet.hordePortraitReplacement : (actionSet != null && race.side == "Alliance" && actionSet.alliancePortraitReplacement != null ? actionSet.alliancePortraitReplacement : (race.portrait == "" ? "OtherUnknown" : race.portrait + (race.genderedPortrait ? participant.who.gender : ""))), (h) =>
                        {
                            if (abilityTargetted != null) FinishTargettingAbility(board.participants[board.team1[index]]);
                            else ChangeSpotlight(index);
                        },
                        (h) => ClearTargettingAbility(), (h) => () => participant.who.PrintEntityTooltip());
                    }
                    if (participant.who.IsStealthed())
                    {
                        var button = CDesktop.LBWindow().LBRegionGroup().LBRegion().LBBigButton().gameObject;
                        AddBigButtonOverlay(button, "SneakingBig", 0, 1);
                    }
                    if (participant.who.dead) SetBigButtonToGrayscale();
                    if (participant == board.participants[board.whosTurn])
                    {
                        var arrow = AddSmallButtonOverlay(CDesktop.LBWindow().LBRegionGroup().LBRegion().gameObject, participant.who.IsControlledByHuman() ? "PlayerLocationFromBelow" : (participant.who.IsControlledByOppositeTeam()  ? "EnemyLocationFromBelow" : "FriendLocationFromBelow"), 0, 1);
                        arrow.transform.localPosition = new Vector3(191.5f, -20.5f, 0);
                        arrow.transform.localEulerAngles = new Vector3(0, 0, -90);
                        arrow.GetComponent<SpriteRenderer>().flipY = true;
                    }
                    AddHealthBar(2, -19, board.team1[index], participant.who);
                },
                (h) =>
                {
                    if (abilityTargetted != null) FinishTargettingAbility(participant);
                    else ChangeSpotlight(index);
                },
                (h) => ClearTargettingAbility(),
                (h) => () => participant.who.PrintEntityTooltip());
                foreach (var actionBar in participant.who.actionSets[participant.who.currentActionSet])
                {
                    var abilityObj = abilities.Find(x => x.name == actionBar);
                    if (abilityObj == null || abilityObj.cost == null) continue;
                    AddButtonRegion(
                        () =>
                        {
                            if (board.cooldowns[board.team1[index]].ContainsKey(actionBar))
                            {
                                AddLine(board.cooldowns[board.team1[index]][actionBar] + " / ", "DimGray", "Right");
                                AddText(actionBar, "Black");
                            }
                            else AddLine(actionBar, "", "Right");
                            AddSmallButton(abilityObj.icon);
                            if (!abilityObj.EnoughResources(board.participants[board.team1[index]].who))
                            {
                                SetSmallButtonToGrayscale();
                                AddSmallButtonOverlay("OtherGridBlurred");
                            }
                            if (board.CooldownOn(board.team1[index], actionBar) > 0)
                                AddSmallButtonOverlay("AutoCast");
                        },
                        (h) =>
                        {
                            if (board.team1[index] == board.whosTurn)
                            {
                                if (abilityTargetted != null)
                                    ClearTargettingAbility(true);
                                else if (abilityObj.EnoughResources(board.participants[board.team1[index]].who))
                                    if (board.CooldownOn(board.team1[index], actionBar) <= 0)
                                        StartTargettingAbility(abilityObj);
                            }
                            else ClearTargettingAbility();
                        },
                        (h) => ClearTargettingAbility(),
                        (h) => () =>
                        {
                            PrintAbilityTooltip(board.participants[board.team1[index]].who, abilityObj, board.participants[board.team1[index]].who.abilities.ContainsKey(abilityObj.name) ? board.participants[board.team1[index]].who.abilities[abilityObj.name] : 0);
                        }
                    );
                }
                var item = participant.who.equipment.ContainsKey("Trinket") ? participant.who.equipment["Trinket"] : null;
                if (item != null && item.abilities != null && item.combatUse)
                {
                    var ability = item.abilities.ToList()[0];
                    var abilityObj = abilities.Find(x => x.name == ability.Key);
                    AddButtonRegion(
                        () =>
                        {
                            if (board.cooldowns[board.team1[index]].ContainsKey(ability.Key))
                            {
                                AddLine("" + board.cooldowns[board.team1[index]][ability.Key] + " / ", "DimGray", "Right");
                                AddText(ability.Key, "Black");
                            }
                            else AddLine(ability.Key, "", "Right");
                            AddSmallButton(item.icon);
                            if (!abilityObj.EnoughResources(board.participants[board.team1[index]].who))
                            {
                                SetSmallButtonToGrayscale();
                                AddSmallButtonOverlay("OtherGridBlurred");
                            }
                            if (board.CooldownOn(board.team1[index], ability.Key) > 0)
                                AddSmallButtonOverlay("AutoCast");
                        },
                        (h) =>
                        {
                            var temp = abilityTargetted;
                            if (abilityTargetted != null)
                                ClearTargettingAbility(true);
                            else if (abilityObj.EnoughResources(board.participants[board.team1[index]].who) && board.CooldownOn(board.team1[index], ability.Key) <= 0)
                                StartTargettingAbility(abilityObj);
                        },
                        null,
                        (h) => () =>
                        {
                            PrintAbilityTooltip(board.participants[board.team1[index]].who, abilityObj, board.participants[board.team1[index]].who.abilities[abilityObj.name], item);
                        }
                    );
                }
                if (index > 0)
                {
                    AddSmallEmptyRegion();
                    AddSmallEmptyRegion();
                }
            }

            void ChangeSpotlight(int index)
            {
                if (index == 0) return;
                var switchBuffsWith = board.team1[0];
                var temp = board.team1[index];
                board.team1.RemoveAt(index);
                board.team1.Insert(0, temp);
                foreach (var res in board.participants[board.team1[0]].who.resources)
                {
                    CloseWindow("Friendly" + res.Key + "Resource");
                    SpawnWindowBlueprint("Friendly" + res.Key + "Resource");
                }
                board.temporaryBuffs[switchBuffsWith].ForEach(x => x.GetComponent<FlyingBuff>().InstantMove());
                board.temporaryBuffs[temp].ForEach(x => x.GetComponent<FlyingBuff>().InstantMove());
            }
        }),
        new("ItemQuickUse", () => {
            var entity = board.participants[board.whosTurn].who;
            if (entity.inventory.items.FindAll(x => x.combatUse).Count == 0)
            {
                SetAnchor(Bottom, 0, -31);
                AddRegionGroup();
                AddPaddingRegion(() => AddLine(""));
            }
            else
            {
                SetAnchor(Bottom, 0, 7);
                AddRegionGroup();
                AddPaddingRegion(() =>
                {
                    foreach (var item in entity.inventory.items.FindAll(x => x.combatUse))
                        if (item != null && item.abilities != null && item.combatUse)
                        {
                            var ability = item.abilities.ToList()[0];
                            var abilityObj = abilities.Find(x => x.name == ability.Key);
                            AddSmallButton(item.icon,
                            (h) =>
                            {
                                ClearTargettingAbility();
                                if (board.CooldownOn(0, ability.Key) <= 0)
                                    foreach (var participant in board.participants)
                                    {
                                        if (participant == board.participants[board.whosTurn]) board.CallEvents(participant.who, new() { { "Trigger", "ItemUsed" }, { "Triggerer", "Effector" }, { "ItemHash", item.GetHashCode() + "" } });
                                        else board.CallEvents(participant.who, new() { { "Trigger", "ItemUsed" }, { "Triggerer", "Other" }, { "ItemHash", item.GetHashCode() + "" } });
                                    }
                                else SpawnFallingText(new Vector2(0, 34), "This item is on cooldown", "Red");
                            },
                            null,
                            (h) => () => PrintItemTooltip(item));
                            if (board.CooldownOn(0, ability.Key) > 0)
                                AddSmallButtonOverlay("AutoCast");
                        }
                });
            }
        }),
        new("LocationInfo", () => {
            SetAnchor(Top);
            AddRegionGroup();
            AddHeaderRegion(
                () =>
                {
                    AddSmallButton("MenuFlee", (h) =>
                    {
                        ClearTargettingAbility();
                        board.EndCombat(CDesktop.title == "Game" ? "Flee" : "Quit");
                    },
                    (h) => ClearTargettingAbility());
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
                        ClearTargettingAbility();
                        PlaySound("DesktopMenuOpen", 0.6f);
                        SpawnDesktopBlueprint("GameMenu");
                    },
                    (h) => ClearTargettingAbility());
                }
            );
        }),
        new("TargettingInfo", () => {
            if (abilityTargetted == null) return;
            SetAnchor(-122, -108);
            AddRegionGroup();
            SetRegionGroupWidth(242);
            AddPaddingRegion(
                () =>
                {
                    AddLine("Casting " + abilityTargetted.name + "...", "Gray", "Center");
                }
            );
        }),

        //Character
        new("CharacterInfoWeapons", () => {
            SetAnchor(Bottom, 0, 35);
            AddHeaderGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() => AddLine("Weapon modifiers:"));
            var ranged = currentSave.player.GetItemInSlot("Ranged Weapon");
            var main = currentSave.player.GetItemInSlot("Main Hand");
            var off = currentSave.player.GetItemInSlot("Off Hand");
            var mainPower = Math.Round(main == null ? (off == null || off.minPower == 0 ? 1 : 0) : (main.minPower + main.maxPower) / 2, 2);
            if (off != null && off.minPower != 0)
            {
                mainPower /= defines.dividerForDualWield;
                mainPower = Math.Round(mainPower + Math.Round((off.minPower + off.maxPower) / 2, 2) / defines.dividerForDualWield, 2);
            }
            AddPaddingRegion(() =>
            {
                AddLine("Average MAP modifier:");
                AddLine(Math.Round(mainPower, 2).ToString("0.00").Replace(",", "."), "Melee", "Right");
                AddLine("Average SP modifier:");
                AddLine(Math.Round(mainPower, 2).ToString("0.00").Replace(",", "."), "Spell", "Right");
                AddLine("Average RAP modifier:");
                AddLine(ranged == null ? "1.00" : Math.Round((ranged.minPower + ranged.maxPower) / 2, 2).ToString("0.00").Replace(",", "."), "Ranged", "Right");
            });
        }, true),
        new("CharacterInfoDefences", () => {
            SetAnchor(Bottom, 0, 122);
            AddHeaderGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() => AddLine("Combat prowess:"));
            AddPaddingRegion(() =>
            {
                AddLine("Critical strike chance:", "Gray", "Left");
                AddLine(currentSave.player.CriticalStrike().ToString("0.00").Replace(",", ".") + "%", "Uncommon", "Right");
                AddLine("Spell critical chance:", "Gray", "Left");
                AddLine(currentSave.player.SpellCritical().ToString("0.00").Replace(",", ".") + "%", "Uncommon", "Right");
            });
        }, true),
        new("CharacterInfoPower", () => {
            SetAnchor(BottomLeft, 19, 35);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() => AddLine("Defence summary:"));
            AddPaddingRegion(() =>
            {
                AddLine("Max health:", "Gray", "Left");
                AddLine(currentSave.player.MaxHealth() + "", "Uncommon", "Right");
                AddLine("Physical resistance:", "Gray", "Left");
                AddLine((currentSave.player.PhysicalResistance() * 100).ToString("0.00").Replace(",", ".") + "%", "Uncommon", "Right");
                AddLine("Magic resistance:", "Gray", "Left");
                AddLine((currentSave.player.MagicResistance() * 100).ToString("0.00").Replace(",", ".") + "%", "Uncommon", "Right");
            });
            AddEmptyRegion();
            AddHeaderRegion(() => AddLine("Character power:"));
            AddPaddingRegion(() =>
            {
                AddLine("Melee attack power:", "Gray", "Left");
                var melee = currentSave.player.MeleeAttackPower();
                AddLine(melee + "", melee > 0 ? "Uncommon" : "", "Right");
                AddLine("Spell power:", "Gray", "Left");
                var spell = currentSave.player.SpellPower();
                AddLine(spell + "", spell > 0 ? "Uncommon" : "", "Right");
                AddLine("Ranged attack power:", "Gray", "Left");
                var ranged = currentSave.player.RangedAttackPower();
                AddLine(ranged + "", ranged > 0 ? "Uncommon" : "", "Right");
            });
        }, true),
        new("CharacterInfoStats", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(271);
            var rawStats = currentSave.player.Stats(true);
            var stats = currentSave.player.Stats();
            AddHeaderRegion(() => 
            {
                AddLine("General stats:");
                if (!WindowUp("CharacterInfoStatsExpanded"))
                    AddSmallButton("OtherBigger", (h) =>
                    {
                        CloseWindow("CharacterInfoMasteries");
                        CloseWindow("CharacterInfoWeapons");
                        CloseWindow("CharacterInfoDefences");
                        Respawn("CharacterInfoStatsExpanded");
                    });
                else
                    AddSmallButton("OtherSmaller", (h) =>
                    {
                        CloseWindow("CharacterInfoStatsExpanded");
                        Respawn("CharacterInfoMasteries");
                        Respawn("CharacterInfoWeapons");
                        Respawn("CharacterInfoDefences");
                    });
                SmallButtonFlipX();
            });
            AddPaddingRegion(() => 
            {
                foreach (var foo in stats)
                    if (!foo.Key.Contains("Mastery"))
                    {
                        AddLine(foo.Key + ":", "Gray", "Left");
                        AddLine(foo.Value + "", rawStats[foo.Key] == foo.Value ? "Gray" : "Uncommon", "Right");
                    }
            });
        }, true),
        new("CharacterInfoStatsExpanded", () => {
            SetAnchor(TopRight, -19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(391);
            SetRegionGroupHeight(271);
            var stats = currentSave.player.Stats();
            var spec = currentSave.player.Spec();
            AddHeaderRegion(() =>
            {
                AddLine("Each point in Stamina:");
                AddLine(spec.rules["Stamina per Level"] + " per level", "DarkGray", "Right");
            });
            AddPaddingRegion(() =>
            {
                AddLine("+" + spec.rules["Max Health per Stamina"] + " Max health", "Uncommon");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Each point in Strength:");
                AddLine(spec.rules["Strength per Level"] + " per level", "DarkGray", "Right");
            });
            AddPaddingRegion(() =>
            {
                AddLine("+" + spec.rules["Melee Attack Power per Strength"] + " Melee attack power", "Uncommon");
                if (spec.rules["Ranged Attack Power per Strength"] > 0)
                    AddLine("+" + spec.rules["Ranged Attack Power per Strength"] + " Ranged attack power", "Uncommon");
                if (spec.rules["Critical Strike per Strength"] > 0)
                    AddLine(("+" + spec.rules["Critical Strike per Strength"]).Replace(",", ".") + "% Critical strike", "Uncommon");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Each point in Agility:");
                AddLine(spec.rules["Agility per Level"] + " per level", "DarkGray", "Right");
            });
            AddPaddingRegion(() =>
            {
                AddLine("+" + spec.rules["Melee Attack Power per Agility"] + " Melee attack power", "Uncommon");
                if (spec.rules["Ranged Attack Power per Agility"] > 0)
                    AddLine("+" + spec.rules["Ranged Attack Power per Agility"] + " Ranged attack power", "Uncommon");
                if (spec.rules["Critical Strike per Agility"] > 0)
                    AddLine(("+" + spec.rules["Critical Strike per Agility"]).Replace(",", ".") + "% Critical strike chance", "Uncommon");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Each point in Intellect:");
                if (spec.rules["Intellect per Level"] > 0)
                    AddLine(spec.rules["Intellect per Level"] + " per level", "DarkGray", "Right");
            });
            AddPaddingRegion(() =>
            {
                AddLine("+" + spec.rules["Spell Power per Intellect"] + " Spell power", "Uncommon");
                if (spec.rules["Spell Critical per Intellect"] > 0)
                    AddLine(("+" + spec.rules["Spell Critical per Intellect"]).Replace(",", ".") + "% Spell critical chance", "Uncommon");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Each point in Spirit:");
                if (spec.rules["Spirit per Level"] > 0)
                    AddLine(spec.rules["Spirit per Level"] + " per level", "DarkGray", "Right");
            });
            AddPaddingRegion(() =>
            {
                AddLine(("+" + spec.rules["Magic Resistance per Spirit"]).Replace(",", ".") + "% Magic resistance", "Uncommon");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Each point in Armor:");
                if (spec.rules["Armor per Level"] > 0)
                    AddLine(spec.rules["Armor per Level"] + " per level", "DarkGray", "Right");
            });
            AddPaddingRegion(() =>
            {
                AddLine(("+" + spec.rules["Physical Resistance per Armor"]).Replace(",", ".") + "% Physical resistance", "Uncommon");
                SetRegionAsGroupExtender();
            });
        }, true),
        new("CharacterInfoMasteries", () => {
            SetAnchor(BottomRight, -19, 35);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var rawStats = currentSave.player.Stats(true);
            var stats = currentSave.player.Stats();
            AddHeaderRegion(() =>
            {
                AddLine("Element masteries:");
                if (!WindowUp("CharacterInfoMasteriesExpanded"))
                    AddSmallButton("OtherBigger", (h) =>
                    {
                        CloseWindow("CharacterInfoStats");
                        CloseWindow("CharacterInfoWeapons");
                        CloseWindow("CharacterInfoDefences");
                        CloseWindow("CharacterInfoPower");
                        Respawn("CharacterInfoMasteriesExpanded");
                    });
                else
                    AddSmallButton("OtherSmaller", (h) =>
                    {
                        CloseWindow("CharacterInfoMasteriesExpanded");
                        Respawn("CharacterInfoStats");
                        Respawn("CharacterInfoWeapons");
                        Respawn("CharacterInfoDefences");
                        Respawn("CharacterInfoPower");
                    });
                SmallButtonFlipX();
            });
            var ordered = stats.ToList().FindAll(x => x.Key.Contains("Mastery")).OrderBy(x => x.Key).OrderByDescending(x => x.Value).ToList();
            foreach (var foo in ordered)
                AddPaddingRegion(() =>
                {
                    AddLine(foo.Key + ":", "Gray", "Left");
                    AddLine(foo.Value.ToRoman(), rawStats[foo.Key] == foo.Value ? "Gray" : "Uncommon", "Right");
                    AddSmallButton("Element" + foo.Key.Split(" ")[0] + "Awakened");
                });
        }, true),
        new("CharacterInfoMasteriesExpanded", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(391);
            SetRegionGroupHeight(271);
            var stats = currentSave.player.Stats();
            AddHeaderRegion(() =>
            {
                AddLine("Tier I:");
            });
            AddPaddingRegion(() =>
            {
                AddLine("4 Max resource", "Uncommon");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Tier II:");
            });
            AddPaddingRegion(() =>
            {
                AddLine("5 Max resource", "Uncommon");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Tier III:");
            });
            AddPaddingRegion(() =>
            {
                AddLine("6 Max resource", "Uncommon");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Tier IV:");
            });
            AddPaddingRegion(() =>
            {
                AddLine("7 Max resource", "Uncommon");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Tier V:");
            });
            AddPaddingRegion(() =>
            {
                AddLine("8 Max resource", "Uncommon");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Tier VI:");
            });
            AddPaddingRegion(() =>
            {
                AddLine("9 Max resource", "Uncommon");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Tier VII:");
            });
            AddPaddingRegion(() =>
            {
                AddLine("10 Max resource", "Uncommon");
                SetRegionAsGroupExtender();
            });
        }, true),
        //new("CharacterRankingTop", () => 
        //{
        //    SetAnchor(-293, 153);
        //    DisableShadows();
        //    AddHeaderGroup();
        //    SetRegionGroupWidth(588);
        //    AddHeaderRegion(() =>
        //    {
        //        AddLine("Ranking", "Gray", "Center");
        //        AddSmallButton("OtherClose", (h) =>
        //        {
        //            CloseDesktop("RankingScreen");
        //        });
        //    });
        //    if (settings.selectedRealmRanking == "")
        //        settings.selectedRealmRanking = Realm.realms[0].name;
        //    foreach (var realmRef in Realm.realms)
        //    {
        //        var realm = realmRef;
        //        AddRegionGroup();
        //        SetRegionGroupWidth(147);
        //        if (settings.selectedRealmRanking == realm.name)
        //            AddPaddingRegion(() =>
        //            {
        //                AddLine(realm.name, "", "Center");
        //            });
        //        else
        //            AddButtonRegion(() =>
        //            {
        //                AddLine(realm.name, "", "Center");
        //            },
        //            (h) =>
        //            {
        //                settings.selectedRealmRanking = realm.name;
        //                Respawn("CharacterRankingList");
        //                Respawn("CharacterRankingListRight");
        //            });
        //    }
        //}),
        //new("CharacterRankingList", () => 
        //{
        //    SetAnchor(-293, 115);
        //    DisableShadows();
        //    AddRegionGroup();
        //    SetRegionGroupWidth(550);
        //    SetRegionGroupHeight(262);
        //    var slots = saves[settings.selectedRealmRanking].OrderByDescending(x => x.Score()).ToList();
        //    for (int i = 0; i < 7; i++)
        //        if (i < slots.Count)
        //        {
        //            var slot = slots[i];
        //            AddPaddingRegion(() =>
        //            {
        //                AddBigButton("Portrait" + slot.player.race.Clean() + (slot.player.Race().genderedPortrait ? slot.player.gender : ""));
        //                AddBigButton("Class" + slot.player.spec);
        //                AddLine(slot.player.name + ", a level " + slot.player.level + " ");
        //                AddText(slot.player.spec, slot.player.spec);
        //                AddLine("Score: " + slot.Score());
        //                if (slot.player.dead) AddText(", died fighting " + (slot.deathInfo.commonSource ? "a " : "") + slot.deathInfo.source + " in " + slot.deathInfo.area);
        //            });
        //        }
        //        else
        //            AddPaddingRegion(() =>
        //            {
        //                AddBigButton("OtherBlank");
        //                AddBigButton("OtherBlank");
        //            });
        //}),
        //new("CharacterRankingListRight", () => 
        //{
        //    SetAnchor(257, 115);
        //    DisableShadows();
        //    AddRegionGroup();
        //    SetRegionGroupWidth(38);
        //    SetRegionGroupHeight(262);
        //    var slots = saves[settings.selectedRealmRanking].OrderByDescending(x => x.Score()).ToList();
        //    for (int i = 0; i < 7; i++)
        //        if (i < slots.Count)
        //        {
        //            var slot = slots[i];
        //            AddPaddingRegion(() =>
        //            {
        //                AddBigButton("PVP" + (slot.player.Side() == "Alliance" ? "A" : "H") + slot.player.Rank().rank);
        //            });
        //        }
        //        else
        //            AddPaddingRegion(() =>
        //            {
        //                AddBigButton("OtherBlank");
        //            });
        //}),
        //new("CharacterRankingShadow", () => 
        //{
        //    SetAnchor(-293, 153);
        //    AddRegionGroup();
        //    SetRegionGroupWidth(588);
        //    SetRegionGroupHeight(300);
        //    AddPaddingRegion(() => { });
        //}),
        new("ExperienceBar", () => {
            SetAnchor(Bottom);
            if (board != null && board.results != null && board.results.experience != null && board.results.experience.ContainsKey(currentSave.player) && board.results.experience[currentSave.player] > 0)
            {
                var add = board.results.experience[currentSave.player];
                var experience = currentSave == null ? 0 : (int)(319 * ((currentSave.player.experience - add) / (double)currentSave.player.ExperienceNeeded()));
                var newExperience = currentSave == null ? 0 : (int)(319 * (add / (double)currentSave.player.ExperienceNeeded()));
                if (experience < 0)
                {
                    AddRegionGroup();
                    SetRegionGroupWidth((experience + newExperience - 1) * 2);
                    SetRegionGroupHeight(12);
                    AddPaddingRegion(() => { SetRegionBackground(ExperienceNew); });
                    AddRegionGroup();
                    SetRegionGroupWidth((319 - experience - newExperience + 1) * 2);
                    SetRegionGroupHeight(12);
                    AddPaddingRegion(() => { SetRegionBackground(ExperienceNone); });
                }
                else
                {
                    AddRegionGroup();
                    SetRegionGroupWidth(experience * 2);
                    SetRegionGroupHeight(12);
                    AddPaddingRegion(() => { SetRegionBackground(Experience); });
                    AddRegionGroup();
                    SetRegionGroupWidth(newExperience * 2);
                    SetRegionGroupHeight(12);
                    AddPaddingRegion(() => { SetRegionBackground(ExperienceNew); });
                    AddRegionGroup();
                    SetRegionGroupWidth((319 - experience - newExperience) * 2);
                    SetRegionGroupHeight(12);
                    AddPaddingRegion(() => { SetRegionBackground(ExperienceNone); });
                }
            }
            else
            {
                var experience = currentSave == null ? 0 : (int)(319 * (currentSave.player.experience / (double)currentSave.player.ExperienceNeeded()));
                AddRegionGroup();
                SetRegionGroupWidth(experience * 2);
                SetRegionGroupHeight(12);
                AddPaddingRegion(() => { SetRegionBackground(Experience); });
                AddRegionGroup();
                SetRegionGroupWidth((319 - experience) * 2);
                SetRegionGroupHeight(12);
                AddPaddingRegion(() => { SetRegionBackground(ExperienceNone); });
            }
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
            AddRegionGroup();
            if (area == null || !currentSave.siteProgress.ContainsKey(area.name) || !area.progression.Any(x => x.type == "Treasure")) return;
            if (area.progression.First(x => x.type == "Treasure").point > currentSave.siteProgress[area.name] || currentSave.openedChests.ContainsKey(area.name) && currentSave.openedChests[area.name].inventory.items.Count == 0) return;
            SetAnchor(-301, -103);
            Chest.SpawnChestObject(new Vector2(0, 0));
        }),
        new("ChestInfo", () => {
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(
                () =>
                {
                    AddLine(area.name + " spoils:");
                    AddSmallButton("OtherClose", (h) =>
                    {
                        var zone = zones.Find(x => x.name == area.zone);
                        var chestID = area.chestVariant != 0 ? area.chestVariant : (zone.chestVariant != 0 ? zone.chestVariant : 6);
                        PlaySound("DesktopChest" + chestID + "Close");
                        CloseDesktop("ChestLoot");
                        Respawn("Chest");
                    });
                }
            );
        }),
        new("ChestLoot", () => {
            SetAnchor(TopLeft, 19, -57);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            for (int i = 0; i < 2; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        var findItem = currentSave.openedChests[area.name].inventory.items.Find(x => x.y == index && x.x == j);
                        if (findItem != null) PrintLootItem(findItem);
                        else AddBigButton("OtherDisabled");
                    }
                });
            }
        }),

        //Login Screen
        new("ConfirmDeleteCharacter", () => {
            SetAnchor(Bottom, 0, 171);
            AddRegionGroup();
            SetRegionGroupWidth(248);
            AddHeaderRegion(() =>
            {
                AddLine("Confirm Character Removal:", "HalfGray");
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
        new("CharacterCreationFactionHorde", () => {
            SetAnchor(BottomLeft, 157, 19);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddLine("Horde Races:", "HalfGray", "Center");
            });
            AddHeaderRegion(() =>
            {
                ReverseButtons();
                AddBigButton("TabardOrgrimmar",
                (h) =>
                {
                    if (creationRace == "Orc") return;
                    creationRace = "Orc";
                    creationSpec = "";
                    creationPortrait = -1;
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Orc").previewSite).Background());
                    Respawn("CharacterCreationMasculinePortraits");
                    Respawn("CharacterCreationFemininePortraits");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("TitleScreenNewSaveFinish");
                    SpawnWindowBlueprint("TitleScreenNewSaveFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(BottomLeft, 119, 173);
                    AddRegionGroup();
                    SetRegionGroupWidth(400);
                    AddHeaderRegion(() => AddLine("Orcs of Orgrimmar", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Orcs are a proud and powerful race with warrior culture deeply rooted in honor and combat. Originally from the shattered world of Draenor, they now inhabit the harsh landscapes of Durotar, with their capital at Orgrimmar. They strive to overcome their dark past and build a new future for their people." }
                        }
                    } } } }.Print(null, 390, null);
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
                    creationPortrait = -1;
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Troll").previewSite).Background());
                    Respawn("CharacterCreationMasculinePortraits");
                    Respawn("CharacterCreationFemininePortraits");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("TitleScreenNewSaveFinish");
                    SpawnWindowBlueprint("TitleScreenNewSaveFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(BottomLeft, 119, 173);
                    AddRegionGroup();
                    SetRegionGroupWidth(400);
                    AddHeaderRegion(() => AddLine("Trolls of the Darkspear Tribe", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Trolls are a fierce and agile race with a long history of mysticism and shamanism. The Darkspear Tribe, having allied with the Horde, has established itself in the Echo Isles and the coastal regions of Durotar. Known for their cunning and resourcefulness, they are formidable warriors and mystics." }
                        }
                    } } } }.Print(null, 390, null);
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
                    creationPortrait = -1;
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Tauren").previewSite).Background());
                    Respawn("CharacterCreationMasculinePortraits");
                    Respawn("CharacterCreationFemininePortraits");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("TitleScreenNewSaveFinish");
                    SpawnWindowBlueprint("TitleScreenNewSaveFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(BottomLeft, 119, 173);
                    AddRegionGroup();
                    SetRegionGroupWidth(400);
                    AddHeaderRegion(() => AddLine("Tauren of Thunder Bluff", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Tauren are massive, bovine-like beings with a deep spiritual connection to nature and the Earth Mother. They dwell in the grassy plains of Mulgore, with their capital in Thunder Bluff. Renowned for their strength and wisdom, they serve as staunch protectors of the natural world." }
                        }
                    } } } }.Print(null, 390, null);
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
                    creationPortrait = -1;
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Forsaken").previewSite).Background());
                    Respawn("CharacterCreationMasculinePortraits");
                    Respawn("CharacterCreationFemininePortraits");
                    Respawn("CharacterCreationFactionAlliance");
                    CloseWindow("TitleScreenNewSaveFinish");
                    SpawnWindowBlueprint("TitleScreenNewSaveFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(BottomLeft, 119, 173);
                    AddRegionGroup();
                    SetRegionGroupWidth(400);
                    AddHeaderRegion(() => AddLine("Forsaken of the Undercity", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "The Forsaken are undead humans who have broken free from the Lich King's control, now seeking vengeance and a place in the world. They inhabit the eerie and decaying ruins of the Undercity, beneath the fallen kingdom of Lordaeron. Driven by a desire for autonomy and revenge, they are both feared and misunderstood by the living." }
                        }
                    } } } }.Print(null, 390, null);
                });
                if (creationRace != "Forsaken")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
            });
        }, true),
        new("CharacterCreationFactionAlliance", () => {
            SetAnchor(BottomRight, -157, 19);
            AddRegionGroup();
            SetRegionGroupWidth(152);
            AddHeaderRegion(() =>
            {
                AddLine("Alliance Races:", "HalfGray", "Center");
            });
            AddHeaderRegion(() =>
            {
                AddBigButton("TabardStormwind",
                (h) =>
                {
                    if (creationRace == "Human") return;
                    creationRace = "Human";
                    creationSpec = "";
                    creationPortrait = -1;
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Human").previewSite).Background());
                    Respawn("CharacterCreationMasculinePortraits");
                    Respawn("CharacterCreationFemininePortraits");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("TitleScreenNewSaveFinish");
                    SpawnWindowBlueprint("TitleScreenNewSaveFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");

                },
                null, (h) => () =>
                {
                    SetAnchor(BottomLeft, 119, 173);
                    AddRegionGroup();
                    SetRegionGroupWidth(400);
                    AddHeaderRegion(() => AddLine("Humans of Stormwind", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Humans are a resilient and versatile race known for their unyielding spirit and strong sense of justice. They have a rich history of surviving numerous wars and catastrophes, making them natural leaders in the Alliance. Their capital city is Stormwind, a bustling hub of trade and governance." }
                        }
                    } } } }.Print(null, 390, null);
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
                    creationPortrait = -1;
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Dwarf").previewSite).Background());
                    Respawn("CharacterCreationMasculinePortraits");
                    Respawn("CharacterCreationFemininePortraits");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("TitleScreenNewSaveFinish");
                    SpawnWindowBlueprint("TitleScreenNewSaveFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(BottomLeft, 119, 173);
                    AddRegionGroup();
                    SetRegionGroupWidth(400);
                    AddHeaderRegion(() => AddLine("Dwarfs of Ironforge", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Dwarves are hardy and stout creatures famed for their skills in mining and blacksmithing. They hail from the snowy peaks of Dun Morogh and are deeply connected to their ancestral homeland of Ironforge. Their adventurous nature drives them to uncover ancient relics and forgotten lore." }
                        }
                    } } } }.Print(null, 390, null);
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
                    creationPortrait = -1;
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Gnome").previewSite).Background());
                    Respawn("CharacterCreationMasculinePortraits");
                    Respawn("CharacterCreationFemininePortraits");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("TitleScreenNewSaveFinish");
                    SpawnWindowBlueprint("TitleScreenNewSaveFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(BottomLeft, 119, 173);
                    AddRegionGroup();
                    SetRegionGroupWidth(400);
                    AddHeaderRegion(() => AddLine("Gnomes of Gnomeregan", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Gnomes are brilliant inventors and tinkerers, known for their technological prowess and innovative gadgets. Originally from the subterranean city of Gnomeregan, many now reside with their Dwarven allies in Ironforge. Despite their small stature, they possess an insatiable curiosity and boundless energy." }
                        }
                    } } } }.Print(null, 390, null);
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
                    creationPortrait = -1;
                    SpawnTransition();
                    SetDesktopBackground(FindSite(y => y.name == races.Find(x => x.name == "Night Elf").previewSite).Background());
                    Respawn("CharacterCreationMasculinePortraits");
                    Respawn("CharacterCreationFemininePortraits");
                    Respawn("CharacterCreationFactionHorde");
                    CloseWindow("TitleScreenNewSaveFinish");
                    SpawnWindowBlueprint("TitleScreenNewSaveFinish");
                    CloseWindow("CharacterCreationSpec");
                    SpawnWindowBlueprint("CharacterCreationSpec");
                },
                null, (h) => () =>
                {
                    SetAnchor(BottomLeft, 119, 173);
                    AddRegionGroup();
                    SetRegionGroupWidth(400);
                    AddHeaderRegion(() => AddLine("Night Elfs of Darnassus", "", "Center"));
                    new Description()
                    { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                        {
                            { "Color", "DarkGray" },
                            { "Align", "Center" },
                            { "Text", "Night Elves are an ancient and mystical race with a profound connection to nature and druidic magic. They once lived in isolation in the lush forests of Kalimdor with their majestic city of Darnassus. Renowned for their agility and wisdom, they strive to protect the world of nature from harm." }
                        }
                    } } } }.Print(null, 390, null);
                });
                if (creationRace != "Night Elf")
                {
                    AddBigButtonOverlay("OtherGridBlurred");
                    SetBigButtonToGrayscale();
                }
            });
        }, true),
        new("CharacterCreationSpec", () => {
            if (creationRace == "") return;
            SetAnchor(Bottom, 0, 95);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Available Classes:", "HalfGray", "Center");
            });
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
                        CloseWindow("TitleScreenNewSaveFinish");
                        SpawnWindowBlueprint("TitleScreenNewSaveFinish");
                    },
                    null, (h) => () =>
                    {
                        SetAnchor(BottomLeft, 119, 173);
                        AddRegionGroup();
                        SetRegionGroupWidth(400);
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
                        desc?.Print(null, 390, null);
                    });
                    if (creationSpec != spec.name)
                    {
                        AddBigButtonOverlay("OtherGridBlurred");
                        SetBigButtonToGrayscale();
                    }
                }
            });
        }, true),
        new("CharacterCreationMasculinePortraits", () => {
            if (creationRace == "") return;
            SetAnchor(BottomLeft, 119, 173);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Masculine Portraits:", "HalfGray", "Center");
            });
            AddPaddingRegion(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    var index = i;
                    var a = "Portrait" + creationRace.Clean() + "Male" + index;
                    index = i;
                    AddBigButton("Portrait" + creationRace.Clean() + "Male" + index,
                    (h) =>
                    {
                        creationPortrait = index;
                        creationGender = "Male";
                        var oldName = String.creationName.Value();
                        var name = "";
                        var race = races.Find(x => x.name == creationRace);
                        name = race.maleNames[random.Next(race.maleNames.Count)];
                        String.creationName.Set(name);
                        Respawn("CharacterCreationFemininePortraits");
                        Respawn("TitleScreenNewSaveFinish");
                    });
                    if (creationPortrait != index || creationGender != "Male")
                    {
                        AddBigButtonOverlay("OtherGridBlurred");
                        SetBigButtonToGrayscale();
                    }
                }
            });
        }),
        new("CharacterCreationFemininePortraits", () => {
            if (creationRace == "") return;
            SetAnchor(BottomRight, -119, 173);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                AddLine("Feminine Portraits:", "HalfGray", "Center");
            });
            AddPaddingRegion(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    var index = i;
                    var a = "Portrait" + creationRace.Clean() + "Female" + index;
                    index = i;
                    AddBigButton("Portrait" + creationRace.Clean() + "Female" + index,
                    (h) =>
                    {
                        creationPortrait = index;
                        creationGender = "Female";
                        var oldName = String.creationName.Value();
                        var name = "";
                        var race = races.Find(x => x.name == creationRace);
                        name = race.femaleNames[random.Next(race.femaleNames.Count)];
                        String.creationName.Set(name);
                        Respawn("CharacterCreationMasculinePortraits");
                        Respawn("TitleScreenNewSaveFinish");
                    });
                    if (creationPortrait != index || creationGender != "Female")
                    {
                        AddBigButtonOverlay("OtherGridBlurred");
                        SetBigButtonToGrayscale();
                    }
                }
            });
        }),

        //Crafting Screen
        new("ProfessionListPrimary", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var professions = Profession.professions.FindAll(x => currentSave.player.professionSkills.ContainsKey(x.name));
            AddHeaderRegion(() =>
            {
                AddLine("Primary professions:");
            });
            var primary = professions.Where(x => x.primary && !x.gathering).ToList();
            for (int i = 0; i < primary.Count; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (primary.Count() > index)
                    {
                        var maxSkill = primary[index].levels.Where(x => currentSave.player.professionSkills[primary[index].name].Item2.Contains(x.name)).Max(x => x.maxSkill);
                        var skill = currentSave.player.professionSkills[primary[index].name].Item1;
                        AddLine(primary[index].name + " " + skill);
                        AddLine(maxSkill + "", "DimGray", "Right");
                        AddBigButton(primary[index].icon,
                        (h) =>
                        {
                            profession = primary[index];
                            if (profession.recipeType == null) return;
                            CloseWindow("ProfessionListPrimary");
                            CloseWindow("ProfessionListSecondary");
                            CloseWindow("ProfessionListGathering");
                            Respawn("CraftingList");
                            PlaySound("DesktopInstanceOpen");
                            SetDesktopBackground("Backgrounds/Profession");
                        });
                        AddSkillBar(40, -19, primary[index], currentSave.player);
                    }
                    else AddBigButton("OtherDisabled");
                });
            }
        }),
        new("ProfessionListGathering", () => {
            SetAnchor(TopRight, -19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var professions = Profession.professions.FindAll(x => currentSave.player.professionSkills.ContainsKey(x.name));
            AddHeaderRegion(() =>
            {
                AddLine("Gathering professions:");
            });
            var gathering = professions.Where(x => x.gathering).ToList();
            for (int i = 0; i < gathering.Count; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (gathering.Count() > index)
                    {
                        var maxSkill = gathering[index].levels.Where(x => currentSave.player.professionSkills[gathering[index].name].Item2.Contains(x.name)).Max(x => x.maxSkill);
                        var skill = currentSave.player.professionSkills[gathering[index].name].Item1;
                        AddLine(gathering[index].name + " " + skill);
                        AddLine(maxSkill + "", "DimGray", "Right");
                        AddBigButton(gathering[index].icon,
                        (h) =>
                        {
                            profession = gathering[index];
                            if (profession.recipeType == null) return;
                            CloseWindow("ProfessionListPrimary");
                            CloseWindow("ProfessionListSecondary");
                            CloseWindow("ProfessionListGathering");
                            Respawn("CraftingList");
                            PlaySound("DesktopInstanceOpen");
                            SetDesktopBackground("Backgrounds/Profession");
                        });
                        AddSkillBar(40, -19, gathering[index], currentSave.player);
                    }
                    else AddBigButton("OtherDisabled");
                });
            }
        }),
        new("ProfessionListSecondary", () => {
            SetAnchor(BottomRight, -19, 35);
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
                        var maxSkill = secondary[index].levels.Where(x => currentSave.player.professionSkills[secondary[index].name].Item2.Contains(x.name)).Max(x => x.maxSkill);
                        var skill = currentSave.player.professionSkills[secondary[index].name].Item1;
                        AddLine(secondary[index].name + " " + skill);
                        AddLine(maxSkill + "", "DimGray", "Right");
                        AddBigButton(secondary[index].icon,
                        (h) =>
                        {
                            profession = secondary[index];
                            if (profession.recipeType == null) return;
                            CloseWindow("ProfessionListPrimary");
                            CloseWindow("ProfessionListSecondary");
                            CloseWindow("ProfessionListGathering");
                            Respawn("CraftingList");
                            PlaySound("DesktopInstanceOpen");
                            SetDesktopBackground("Backgrounds/Profession");
                        });
                        AddSkillBar(40, -19, secondary[index], currentSave.player);
                    }
                    else AddBigButton("OtherDisabled");
                });
            }
        }),
        new("CraftingList", () => {
            var rowAmount = 11;
            var thisWindow = CDesktop.LBWindow();
            var recipes = currentSave.player.learnedRecipes[profession.name].Select(x => Recipe.recipes.Find(y => y.name == x)).Where(x => (!settings.onlyHavingMaterials.Value() || currentSave.player.CanCraft(x, true, true) > 0) && (!settings.onlySkillUp.Value() || x.skillUpGray > currentSave.player.professionSkills[profession.name].Item1)).ToList();
            var list = recipes;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddHeaderRegion(() =>
            {
                var maxSkill = profession.levels.Where(x => currentSave.player.professionSkills[profession.name].Item2.Contains(x.name)).Max(x => x.maxSkill);
                var skill = currentSave.player.professionSkills[profession.name].Item1;
                AddLine(profession.name + " " + skill);
                //AddLine(maxSkill + "", "DimGray", "Right");
                AddBigButton(profession.icon);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("CraftingRecipe");
                    CloseWindow("CraftingList");
                    Respawn("ProfessionListPrimary");
                    Respawn("ProfessionListSecondary");
                    Respawn("ProfessionListGathering");
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
                if (recipes.Count > index + thisWindow.pagination())
                {
                    AddButtonRegion(() =>
                    {
                        var recipe = recipes[index + thisWindow.pagination()];
                        AddLine(recipe.name, "Black");
                        var amountPossible = currentSave.player.CanCraft(recipe, false, true);
                        AddText(amountPossible > 0 ? " [" + amountPossible + "]" : "", "Black");
                        AddSmallButton(recipe.Icon());
                        if (settings.rarityIndicators.Value() && recipe.results.Count > 0)
                            AddSmallButtonOverlay("OtherRarity" + items.Find(x => x.name == recipe.results.ToList()[0].Key), 0, 2);
                    },
                    (h) =>
                    {
                        recipe = recipes[index + thisWindow.pagination()];
                        enchant = recipe.enchantment ? enchants.Find(x => x.name == recipe.name) : null;
                        if (enchantmentTarget != null && (enchant == null || enchant.type != enchantmentTarget.type))
                            enchantmentTarget = null;
                        Respawn("CraftingRecipe");
                        PlaySound("DesktopInstanceOpen");
                    });
                    var skill = currentSave.player.professionSkills[profession.name].Item1;
                    if (recipes[index + thisWindow.pagination()].skillUpYellow > skill)
                        SetRegionBackgroundAsImage("SkillUpOrange");
                    else if (recipes[index + thisWindow.pagination()].skillUpGreen > skill)
                        SetRegionBackgroundAsImage("SkillUpYellow");
                    else if (recipes[index + thisWindow.pagination()].skillUpGray > skill)
                        SetRegionBackgroundAsImage("SkillUpGreen");
                    else SetRegionBackgroundAsImage("SkillUpGray");
                }
                else if (recipes.Count == index + thisWindow.pagination())
                {
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
                }
            }
            AddPaginationLine();
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
                        SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * (results.IndexOf(result) % 5), result.amount + "", "", "", "Right");
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
                        SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * (reagents.IndexOf(reagent) % 5), playerPossesion + "/" + reagent.amount, playerPossesion < reagent.amount ? "DangerousRed" : "", "", "Right");
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
            AddHeaderRegion(() =>
            {
                AddLine("Time required:", "Gray");
                AddLine(FormatTime(60 * recipe.reagents.Sum(x => x.Value)), "Gray", "Right");
            });
            var canCraft = currentSave.player.CanCraft(recipe, true, false) > 0;
            var canCraftWithSpace = currentSave.player.CanCraft(recipe, true, true) > 0;
            if (canCraftWithSpace && (!recipe.enchantment || enchantmentTarget != null))
                AddButtonRegion(() => AddLine(recipe.enchantment ? "Enchant" : "Craft"),
                (h) =>
                {
                    currentSave.AddTime(60 * recipe.reagents.Sum(x => x.Value));
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
                        var item = currentSave.player.Unequip(new() { enchant.type });
                        enchantmentTarget.enchant = enchant;
                        enchantmentTarget = null;
                        if (item != null) enchantmentTarget.Equip(currentSave.player, true, false);
                        PlaySound("PutDownGems", 0.8f);
                    }
                    Respawn("CraftingList");
                    CloseWindow("CraftingRecipe");
                    SpawnWindowBlueprint("CraftingRecipe");
                });
            else if (canCraft && (!recipe.enchantment || enchantmentTarget != null))
                AddButtonRegion(() => AddLine(recipe.enchantment ? "Enchant" : "Craft"),
                (h) =>
                {
                    SpawnFallingText(new Vector2(0, 34), "Inventory is full", "Red");
                });
            else
                AddPaddingRegion(() => AddLine(recipe.enchantment ? "Enchant" : "Craft"));
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
            var rowAmount = 12;
            var thisWindow = CDesktop.LBWindow();
            var possibleItems = currentSave.player.inventory.items.Concat(currentSave.player.equipment.Select(x => x.Value)).Where(x => x.type == enchant.type).OrderBy(x => x.name).ToList();
            var list = possibleItems;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
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
                if (possibleItems.Count > index + thisWindow.pagination())
                    AddButtonRegion(() =>
                    {
                        var item = possibleItems[index + thisWindow.pagination()];
                        AddLine(item.name.CutTail());
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
                        var item = possibleItems[index + thisWindow.pagination()];
                        enchantmentTarget = item;
                        PlaySound("DesktopEnchantingTarget");
                        CloseWindow("EnchantingList");
                        Respawn("CraftingRecipe");
                        Respawn("CraftingList");
                    });
                else if (possibleItems.Count == index + thisWindow.pagination())
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
            }
            AddPaginationLine();
        }),

        //Quest Log
        new("QuestList", () => {
            var rowAmount = 11;
            var thisWindow = CDesktop.LBWindow();
            var quests = currentSave.player.currentQuests;
            var list = quests;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
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
                    Respawn("QuestList");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!WindowUp("QuestConfirmAbandon") && !WindowUp("QuestSettings") && !WindowUp("QuestSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("QuestSort");
                        Respawn("Quest", true);
                        Respawn("QuestList");
                    });
                else
                    AddSmallButton("OtherSortOff");
                if (!WindowUp("QuestConfirmAbandon") && !WindowUp("QuestSettings") && !WindowUp("QuestSort"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("QuestSettings");
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
                if (quests.Count > index + thisWindow.pagination())
                {
                    var questTemp = Quest.quests.Find(x => x.questID == quests[index + thisWindow.pagination()].questID);
                    AddButtonRegion(() =>
                    {
                        AddLine((settings.questLevel.Value() ? "[" + questTemp.questLevel + "] " : "") + questTemp.name, "Black");
                        AddSmallButton(questTemp.ZoneIcon());
                    },
                    (h) =>
                    {
                        quest = questTemp;
                        if (staticPagination.ContainsKey("Quest"))
                            staticPagination.Remove("Quest");
                        Respawn("Quest");
                        PlaySound("DesktopInstanceOpen");
                    });
                    var color = ColorQuestLevel(questTemp.questLevel);
                    if (color != null) SetRegionBackgroundAsImage("SkillUp" + color);
                }
                else if (quests.Count == index + thisWindow.pagination())
                    AddPaddingRegion(() =>
                    {
                        SetRegionAsGroupExtender();
                        AddLine("");
                    });
            }
            AddPaginationLine();
        }, true),
        new("Quest", () => {
            if (quest == null) return;
            SetAnchor(TopRight, -19, -38);
            quest.Print();
        }, true),
        new("QuestAdd", () => {
            if (quest == null) return;
            SetAnchor(TopRight, -19, -38);
            quest.Print("Add");
        }, true),
        new("QuestTurn", () => {
            if (quest == null) return;
            SetAnchor(TopRight, -19, -38);
            quest.Print("Turn");
        }, true),
        new("QuestConfirmAbandon", () => {
            SetDesktopBackground("Backgrounds/RuggedLeatherFull", true, true);
            SetAnchor(Top, 0, -38);
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
                Respawn("QuestList", true);
                SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
            });
            AddRegionGroup();
            SetRegionGroupWidth(91);
            AddButtonRegion(() => AddLine("Cancel", "", "Center"),
            (h) =>
            {
                PlaySound("DesktopMenuClose");
                CloseWindow("QuestConfirmAbandon");
                Respawn("QuestList", true);
                Respawn("Quest", true);
                SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
            });
        }, true),
        new("QuestSort", () => {
            SetDesktopBackground("Backgrounds/RuggedLeatherFull", true, true);
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort quests:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("QuestSort");
                    Respawn("QuestList", true);
                    Respawn("Quest", true);
                    SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
                });
            });
            AddButtonRegion(() => AddLine("By name", "Black"),
            (h) =>
            {
                currentSave.player.currentQuests = currentSave.player.currentQuests.OrderBy(x => quests.Find(y => y.questID == x.questID).name).ToList();
                CloseWindow("QuestSort");
                Respawn("QuestList", true);
                Respawn("Quest", true);
                PlaySound("DesktopInventorySort", 0.4f);
                SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
            });
            AddButtonRegion(() => AddLine("By quest level", "Black"),
            (h) =>
            {
                currentSave.player.currentQuests = currentSave.player.currentQuests.OrderBy(x => quests.Find(y => y.questID == x.questID).questLevel).ToList();
                CloseWindow("QuestSort");
                Respawn("QuestList", true);
                Respawn("Quest", true);
                PlaySound("DesktopInventorySort", 0.4f);
                SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
            });
            AddButtonRegion(() => AddLine("By zone", "Black"),
            (h) =>
            {
                currentSave.player.currentQuests = currentSave.player.currentQuests.OrderBy(x => quests.Find(y => y.questID == x.questID).zone).ToList();
                CloseWindow("QuestSort");
                Respawn("QuestList", true);
                Respawn("Quest", true);
                PlaySound("DesktopInventorySort", 0.4f);
                SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
            });
        }, true),
        new("QuestSettings", () => {
            SetDesktopBackground("Backgrounds/RuggedLeatherFull", true, true);
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Quest Log settings:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("QuestSettings");
                    Respawn("QuestList", true);
                    Respawn("Quest", true);
                    SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
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
                Respawn("QuestList", true);
                Respawn("Quest", true);
            });
            AddButtonRegion(() =>
            {
                AddLine("Show low level quests on map", "Black");
                AddCheckbox(settings.showLowLevelQuests);
            },
            (h) =>
            {
                settings.showLowLevelQuests.Invert();
                if (!settings.showLowLevelQuests.Value())
                    foreach (var site in sitesWithQuestMarkers.ToArray())
                        Respawn("Site: " + site.name);
                else
                {
                    var list = new List<string>();
                    foreach (var complex in complexes)
                        if (currentSave.player.AvailableQuestsAt(complex, true).Count > 0)
                            list.Add(complex.name);
                    foreach (var instance in instances)
                        if (!instance.complexPart)
                            if (currentSave.player.AvailableQuestsAt(instance, true).Count > 0)
                                list.Add(instance.name);
                    foreach (var area in areas)
                        if (!area.complexPart && !area.instancePart)
                            if (currentSave.player.AvailableQuestsAt(area, true).Count > 0)
                                list.Add(area.name);
                    list = list.Distinct().ToList();
                    foreach (var site in list)
                        Respawn("Site: " + site);
                }
                Respawn("QuestList", true);
                Respawn("Quest", true);
            });
        }, true),

        //Inventory
        new("PlayerEquipmentInfo", () => {
            if (CDesktop.title == "Map") return;
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(252);
            AddHeaderRegion(() => AddLine("Equipment:"));
            PrintEquipmentSlot("Head", currentSave.player.GetItemInSlot("Head"));
            PrintEquipmentSlot("Shoulders", currentSave.player.GetItemInSlot("Shoulders"));
            PrintEquipmentSlot("Back", currentSave.player.GetItemInSlot("Back"));
            PrintEquipmentSlot("Chest", currentSave.player.GetItemInSlot("Chest"));
            PrintEquipmentSlot("Wrists", currentSave.player.GetItemInSlot("Wrists"));
            PrintEquipmentSlot("Hands", currentSave.player.GetItemInSlot("Hands"));
            PrintEquipmentSlot("Waist", currentSave.player.GetItemInSlot("Waist"));
            PrintEquipmentSlot("Legs", currentSave.player.GetItemInSlot("Legs"));
            PrintEquipmentSlot("Feet", currentSave.player.GetItemInSlot("Feet"));
            AddEmptyRegion();
            AddHeaderRegion(() => AddLine("Jewelry:"));
            PrintEquipmentSlot("Neck", currentSave.player.GetItemInSlot("Neck"));
            PrintEquipmentSlot("Finger", currentSave.player.GetItemInSlot("Finger"));
            PrintEquipmentSlot("Trinket", currentSave.player.GetItemInSlot("Trinket"));
        }),
        new("PlayerWeaponsInfo", () => {
            if (CDesktop.title == "Map") return;
            SetAnchor(Bottom, 0, 35);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() => AddLine("Weapons:"));
            var main = currentSave.player.GetItemInSlot("Main Hand");
            var off = currentSave.player.GetItemInSlot("Off Hand");
            PrintEquipmentSlot("Main Hand", main);
            bool showOff = main == null || main != null && main.type != "Two Handed";
            if (showOff) PrintEquipmentSlot("Off Hand", off);
            var mainPower = Math.Round(main == null ? (off == null || off.minPower == 0 ? 1 : 0) : (main.minPower + main.maxPower) / 2, 2);
            if (off != null)
            {
                mainPower /= defines.dividerForDualWield;
                mainPower = Math.Round(mainPower + Math.Round((off.minPower + off.maxPower) / 2, 2) / defines.dividerForDualWield, 2);
            }
            var hasBowProficiency = currentSave.player.abilities.ContainsKey("Bow Proficiency");
            var hasCrossbowProficiency = currentSave.player.abilities.ContainsKey("Crossbow Proficiency");
            var hasGunProficiency = currentSave.player.abilities.ContainsKey("Gun Proficiency");
            if (hasBowProficiency || hasCrossbowProficiency || hasGunProficiency)
                PrintEquipmentSlot("Ranged Weapon", currentSave.player.GetItemInSlot("Ranged Weapon"));
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
                            else
                            {
                                var findItem = items.Find(x => x.y == index && x.x == j);
                                if (findItem != null) PrintInventoryItem(findItem);
                                else if (movingItem != null) AddBigButton("OtherEmpty", (h) => PutDownMovingItem(h));
                                else AddBigButton("OtherEmpty");
                            }
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
                    {
                        AddSmallButton(currentSave.player.inventory.bags[index].icon,
                            (h) =>
                            {
                                if (movingItem == null && currentSave.player.inventory.items.Count < currentSave.player.inventory.BagSpace() - currentSave.player.inventory.bags[index].bagSpace)
                                    currentSave.player.UnequipBag(index);
                            },
                            null,
                            (h) => () => PrintItemTooltip(currentSave.player.inventory.bags[index])
                        );
                        if (Cursor.cursor.color == "Pink")
                            if (!currentSave.player.inventory.bags[index].IsDisenchantable())
                                SetSmallButtonToGrayscale();
                            else SetSmallButtonToRed();
                    }
                    else
                        AddSmallButton("OtherEmpty", (h) =>
                        {
                            if (movingItem != null && movingItem.CanEquip(currentSave.player, true, true) && movingItem.type == "Bag")
                            {
                                movingItem.Equip(currentSave.player, true, false);
                                movingItem.x = -1;
                                movingItem.y = -1;
                                PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
                                movingItem = null;
                                Cursor.cursor.iconAttached.SetActive(false);
                            }
                        });
                }
            });
            PrintPriceRegion(currentSave.player.inventory.money, 38, 38, 57);
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
                currentSave.player.inventory.ApplySortOrder();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By amount", "Black"),
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderBy(x => x.amount).ToList();
                currentSave.player.inventory.ApplySortOrder();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By rarity", "Black"),
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.rarity == "Poor" ? 0 : (x.rarity == "Common" ? 1 : (x.rarity == "Uncommon" ? 2 : (x.rarity == "Rare" ? 3 : (x.rarity == "Epic" ? 4 : 5))))).ToList();
                currentSave.player.inventory.ApplySortOrder();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By item power", "Black"),
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.ilvl).ToList();
                currentSave.player.inventory.ApplySortOrder();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By price", "Black"),
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.price).ToList();
                currentSave.player.inventory.ApplySortOrder();
                CloseWindow("InventorySort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By type", "Black"),
            (h) =>
            {
                currentSave.player.inventory.items = currentSave.player.inventory.items.OrderByDescending(x => x.type).ToList();
                currentSave.player.inventory.ApplySortOrder();
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
                AddLine("You are about to destroy", "HalfGray", "Center");
                AddLine(itemToDestroy.name.CutTail() + (itemToDestroy.amount > 1 ? " x" + itemToDestroy.amount : ""), itemToDestroy.rarity, "Center");
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
                currentSave.player.inventory.RemoveItem(itemToDestroy);
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
                AddLine("You are about to disenchant", "HalfGray", "Center");
                AddLine(itemToDisenchant.name.CutTail() + (itemToDisenchant.amount > 1 ? " x" + itemToDisenchant.amount : ""), itemToDisenchant.rarity, "Center");
                AddLine("The item will be destroyed", "HalfGray", "Center");
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
                currentSave.player.inventory.RemoveItem(itemToDisenchant);
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
                itemToDisenchant = null;
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
            SetAnchor(TopLeft, 19, -57);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            for (int i = 0; i < 2; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            var findItem = openedItem.itemsInside.Find(x => x.y == index && x.x == j);
                            if (findItem != null) PrintLootItem(findItem);
                            else AddBigButton("OtherDisabled");
                        }
                    }
                );
            }
        }),
        new("PlayerMoney", () => {
            if (WindowUp("TownHostile")) return;
            if (WindowUp("Inventory")) return;
            if (WindowUp("Quest")) return;
            if (WindowUp("QuestAdd")) return;
            if (WindowUp("QuestTurn")) return;
            SetAnchor(BottomRight, -19, 35);
            PrintPriceRegion(currentSave.player.inventory.money, 38, 38, 57);
        }),

        //Combat Results
        new("CombatResults", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(262);
            SetRegionGroupHeight(91);
            AddHeaderRegion(() =>
            {
                AddLine("Combat Results", "", "Center");
                if (board.results.result == "Team1Won")
                    AddSmallButton("OtherClose", (h) =>
                    {
                        if (currentSave.permadeath.Value() && board.results.result == "Team2Won")
                        {
                            CloseSave();
                            SaveGames();
                            CloseDesktop("CombatResults");
                            board = null;
                            Respawn("ExperienceBar", true);
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
                                Respawn("Area");
                                Respawn("AreaProgress");
                                Respawn("AreaElites");
                                Respawn("Chest");
                                SetDesktopBackground(area.Background());
                            }
                            else if (area.complexPart)
                            {
                                CloseDesktop("Complex");
                                SpawnDesktopBlueprint("Complex");
                                Respawn("Area");
                                Respawn("AreaProgress");
                                Respawn("AreaElites");
                                Respawn("Chest");
                                SetDesktopBackground(area.Background());
                            }
                            else
                            {
                                CloseDesktop("Area");
                                SpawnDesktopBlueprint("Area");
                            }
                            CloseDesktop("CombatResults");
                            board = null;
                            Respawn("ExperienceBar", true);
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
                else if (currentSave.permadeath.Value())
                {
                    SetRegionBackground(ButtonRed);
                    AddLine("Game Over", "", "Center");
                }
                else AddLine("Release Spirit", "", "Center");
            },
            (h) =>
            {
                if (currentSave.permadeath.Value() && board.results.result == "Team2Won")
                {
                    CloseSave();
                    SaveGames();
                    CloseDesktop("Map");
                    CloseDesktop("Complex");
                    CloseDesktop("Instance");
                    CloseDesktop("Area");
                    CloseDesktop("CombatResults");
                    board = null;
                    Respawn("ExperienceBar", true);
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
                            Respawn("Area");
                            Respawn("AreaProgress");
                            Respawn("AreaElites");
                            Respawn("Chest");
                            SetDesktopBackground(area.Background());
                        }
                        else if (board.area.complexPart)
                        {
                            CloseDesktop("Complex");
                            SpawnDesktopBlueprint("Complex");
                            Respawn("Area");
                            Respawn("AreaProgress");
                            Respawn("AreaElites");
                            Respawn("Chest");
                            SetDesktopBackground(area.Background());
                        }
                        else
                        {
                            CloseDesktop("Area");
                            SpawnDesktopBlueprint("Area");
                        }
                        if (board.results.inventory.items.Count > 0)
                        {
                            PlaySound("DesktopInventoryOpen");
                            SpawnDesktopBlueprint("CombatResultsLoot");
                        }
                        else
                        {
                            CloseDesktop("CombatResults");
                            board = null;
                            Respawn("ExperienceBar", true);
                        }
                    }
                    else
                    {
                        CloseDesktop("CombatResults");
                        if (board.results.result == "Team2Won")
                        {
                            if (area.instancePart) CloseDesktop("Instance");
                            else if (area.complexPart) CloseDesktop("Complex");
                            else CloseDesktop("Area");
                            var curr = FindSite(x => x.name == currentSave.currentSite);
                            var vect = new Vector2(curr.x, curr.y);
                            var distances = SiteSpiritHealer.spiritHealers.Select(x => (x, Vector2.Distance(new Vector2(x.x, x.y), vect))).OrderBy(x => x.Item2).ToList();
                            var sites = distances.Select(y => FindSite(x => x.name == y.x.name)).ToList();
                            var top = sites.Take(5).OrderBy(x => FindPath(x, curr, true).Count).ToList();
                            distances.Find(x => x.x.name == top[0].name).x.QueueSiteOpen("SpiritHealer");
                        }
                        board = null;
                        Respawn("ExperienceBar", true);
                    }
                }
            });
        }),
        new("CombatResultsLoot", () => {
            SetAnchor(TopLeft, 19, -57);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            for (int i = 0; i < 2; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            var findItem = board.results.inventory.items.Find(x => x.y == index && x.x == j);
                            if (findItem != null) PrintLootItem(findItem);
                            else AddBigButton("OtherDisabled");
                        }
                    }
                );
            }
        }),
        new("CombatResultsSkinning1", () => {
            if (board.results.result != "Team1Won") return;
            if (board.results.skinningNodes.Count < 1) return;
            if (board.results.skinningLoots[0].items.Count == 0) return;
            if (board.results.skinningNodes.Count == 1) SetAnchor(-94, 142);
            if (board.results.skinningNodes.Count == 2) SetAnchor(-198, 142);
            if (board.results.skinningNodes.Count == 3) SetAnchor(-301, 142);
            AddRegionGroup();
            SetRegionGroupWidth(186);
            var can = currentSave.player.professionSkills.ContainsKey("Skinning") && board.results.skinningNodes[0].Item2 <= currentSave.player.professionSkills["Skinning"].Item1;
            AddHeaderRegion(() =>
            {
                AddLine("Skinning");
                AddSmallButton("TradeSkinning");
            });
            AddPaddingRegion(() =>
            {
                var drop = GeneralDrop.generalDrops.Find(x => x.category == board.results.skinningNodes[0].Item1 && x.tags.Contains("Main"));
                var item = items.Find(x => x.name == drop.item);
                AddLine(board.results.skinningNodes[0].Item3.name);
                AddLine("Required skill: ", "DarkGray");
                AddText("" + board.results.skinningNodes[0].Item2, can ? "Gray" : "DangerousRed");
                AddBigButton(board.results.skinningNodes[0].Item3.Race().portrait);
                if (can && !board.results.skinningSkillChange[0] && board.results.WillIncreaseSkinningSkill(0))
                    AddBigButtonOverlay("OtherItemUpgrade");
            });
            if (can)
                AddButtonRegion(() => AddLine("Gather"),
                (h) =>
                {
                    board.results.selectedSkinningLoot = 0;
                    PlaySound("SkinGather");
                    SpawnDesktopBlueprint("SkinningLoot");
                });
            else AddPaddingRegion(() => AddLine("Gather", "DarkGray"));
        }),
        new("CombatResultsSkinning2", () => {
            if (board.results.result != "Team1Won") return;
            if (board.results.skinningNodes.Count < 2) return;
            if (board.results.skinningLoots[1].items.Count == 0) return;
            if (board.results.skinningNodes.Count == 2) SetAnchor(9, 142);
            if (board.results.skinningNodes.Count == 3) SetAnchor(-94, 142);
            AddRegionGroup();
            SetRegionGroupWidth(186);
            var can = currentSave.player.professionSkills.ContainsKey("Skinning") && board.results.skinningNodes[1].Item2 <= currentSave.player.professionSkills["Skinning"].Item1;
            AddHeaderRegion(() =>
            {
                AddLine("Skinning");
                AddSmallButton("TradeSkinning");
            });
            AddPaddingRegion(() =>
            {
                var drop = GeneralDrop.generalDrops.Find(x => x.category == board.results.skinningNodes[1].Item1 && x.tags.Contains("Main"));
                var item = items.Find(x => x.name == drop.item);
                AddLine(board.results.skinningNodes[1].Item3.name);
                AddLine("Required skill: ", "DarkGray");
                AddText("" + board.results.skinningNodes[1].Item2, can ? "Gray" : "DangerousRed");
                AddBigButton(board.results.skinningNodes[1].Item3.Race().portrait);
                if (can && !board.results.skinningSkillChange[1] && board.results.WillIncreaseSkinningSkill(1))
                    AddBigButtonOverlay("OtherItemUpgrade");
            });
            if (can)
                AddButtonRegion(() => AddLine("Gather"),
                (h) =>
                {
                    board.results.selectedSkinningLoot = 1;
                    PlaySound("SkinGather" + random.Next(1, 4));
                    SpawnDesktopBlueprint("SkinningLoot");
                });
            else AddPaddingRegion(() => AddLine("Gather", "DarkGray"));
        }),
        new("CombatResultsSkinning3", () => {
            if (board.results.result != "Team1Won") return;
            if (board.results.skinningNodes.Count < 3) return;
            if (board.results.skinningLoots[2].items.Count == 0) return;
            SetAnchor(111, 142);
            AddRegionGroup();
            SetRegionGroupWidth(186);
            var can = currentSave.player.professionSkills.ContainsKey("Skinning") && board.results.skinningNodes[2].Item2 <= currentSave.player.professionSkills["Skinning"].Item1;
            AddHeaderRegion(() =>
            {
                AddLine("Skinning");
                AddSmallButton("TradeSkinning");
            });
            AddPaddingRegion(() =>
            {
                var drop = GeneralDrop.generalDrops.Find(x => x.category == board.results.skinningNodes[2].Item1 && x.tags.Contains("Main"));
                var item = items.Find(x => x.name == drop.item);
                AddLine(board.results.skinningNodes[2].Item3.name);
                AddLine("Required skill: ", "DarkGray");
                AddText("" + board.results.skinningNodes[2].Item2, can ? "Gray" : "DangerousRed");
                AddBigButton(board.results.skinningNodes[2].Item3.Race().portrait);
                if (can && !board.results.skinningSkillChange[2] && board.results.WillIncreaseSkinningSkill(2))
                    AddBigButtonOverlay("OtherItemUpgrade");
            });
            if (can)
                AddButtonRegion(() => AddLine("Gather"),
                (h) =>
                {
                    board.results.selectedSkinningLoot = 2;
                    PlaySound("SkinGather" + random.Next(1, 4));
                    SpawnDesktopBlueprint("SkinningLoot");
                });
            else AddPaddingRegion(() => AddLine("Gather", "DarkGray"));
        }),
        new("CombatResultsMining", () => {
            if (board.results.result != "Team1Won") return;
            if (board.results.miningNode.Item1 == null) return;
            if (board.results.miningLoot.items.Count == 0) return;
            if (board.results.herb.Item1 != null) SetAnchor(-198, -67);
            else SetAnchor(-94, -67);
            AddRegionGroup();
            SetRegionGroupWidth(188);
            var can = currentSave.player.professionSkills.ContainsKey("Mining") && board.results.miningNode.Item2 <= currentSave.player.professionSkills["Mining"].Item1;
            AddHeaderRegion(() =>
            {
                AddLine("Mining");
                AddSmallButton("TradeMining");
            });
            AddPaddingRegion(() =>
            {
                AddLine(board.results.miningNode.Item1);
                AddLine("Required skill: ", "DarkGray");
                AddText("" + board.results.miningNode.Item2, can ? "Gray" : "DangerousRed");
                AddBigButton("MiningNode" + board.results.miningNode.Item1.Clean());
                if (can && !board.results.miningSkillChange && board.results.WillIncreaseMiningSkill())
                    AddBigButtonOverlay("OtherItemUpgrade");
            });
            if (can)
            {
                AddButtonRegion(() => AddLine("Gather"),
                (h) =>
                {
                    PlaySound("VeinCrack", 0.6f);
                    SpawnDesktopBlueprint("MiningLoot");
                });
            }
            else AddPaddingRegion(() => AddLine("Gather", "DarkGray"));
        }),
        new("MiningLoot", () => {
            SetAnchor(TopLeft, 19, -57);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            for (int i = 0; i < 2; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            var findItem = board.results.miningLoot.items.Find(x => x.y == index && x.x == j);
                            if (findItem != null) PrintLootItem(findItem);
                            else AddBigButton("OtherDisabled");
                        }
                    }
                );
            }
        }),
        new("CombatResultsHerbalism", () => {
            if (board.results.result != "Team1Won") return;
            if (board.results.herb.Item1 == null) return;
            if (board.results.herbalismLoot.items.Count == 0) return;
            if (board.results.miningNode.Item1 != null) SetAnchor(9, -67);
            else SetAnchor(-94, -67);
            AddRegionGroup();
            SetRegionGroupWidth(188);
            var can = currentSave.player.professionSkills.ContainsKey("Herbalism") && board.results.herb.Item2 <= currentSave.player.professionSkills["Herbalism"].Item1;
            AddHeaderRegion(() =>
            {
                AddLine("Herbalism");
                AddSmallButton("TradeHerbalism");
            });
            AddPaddingRegion(() =>
            {
                AddLine(board.results.herb.Item1);
                AddLine("Required skill: ", "DarkGray");
                AddText("" + board.results.herb.Item2, can ? "Gray" : "DangerousRed");
                AddBigButton("Herb" + board.results.herb.Item1.Clean());
                if (can && !board.results.herbalismSkillChange && board.results.WillIncreaseHerbalismSkill())
                    AddBigButtonOverlay("OtherItemUpgrade");
            });
            if (can)
            {
                AddButtonRegion(() => AddLine("Gather"),
                (h) =>
                {
                    PlaySound("HerbGather");
                    SpawnDesktopBlueprint("HerbalismLoot");
                });
            }
            else AddPaddingRegion(() => AddLine("Gather", "DarkGray"));
        }),
        new("HerbalismLoot", () => {
            SetAnchor(TopLeft, 19, -57);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            for (int i = 0; i < 2; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            var findItem = board.results.herbalismLoot.items.Find(x => x.y == index && x.x == j);
                            if (findItem != null) PrintLootItem(findItem);
                            else AddBigButton("OtherDisabled");
                        }
                    }
                );
            }
        }),
        new("SkinningLoot", () => {
            SetAnchor(TopLeft, 19, -57);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            for (int i = 0; i < 2; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            var findItem = board.results.skinningLoots[board.results.selectedSkinningLoot].items.Find(x => x.y == index && x.x == j);
                            if (findItem != null) PrintLootItem(findItem);
                            else AddBigButton("OtherDisabled");
                        }
                    }
                );
            }
        }),
        new("DisenchantLoot", () => {
            SetAnchor(TopLeft, 19, -57);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            for (int i = 0; i < 2; i++)
            {
                var index = i;
                AddPaddingRegion(
                    () =>
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            var findItem = disenchantLoot.items.Find(x => x.y == index && x.x == j);
                            if (findItem != null) PrintLootItem(findItem);
                            else AddBigButton("OtherDisabled");
                        }
                    }
                );
            }
        }),
        new("CombatResultsChartButton", () => {
            SetAnchor(-132, 47);
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
                        CloseDesktop("CombatLog", false);
                        SpawnDesktopBlueprint("CombatLog", true, false);
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
                    CloseDesktop("CombatLog", false);
                    SpawnDesktopBlueprint("CombatLog", true, false);
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
                    CloseDesktop("CombatLog", false);
                    SpawnDesktopBlueprint("CombatLog", true, false);
                });
            });
        }, true),
        new("LootInfo", () => {
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
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
                        AddLine(board.results.skinningNodes[board.results.selectedSkinningLoot].Item3.name + ":");
                        AddSmallButton("OtherClose", (h) =>
                        {
                            PlaySound("DesktopInventoryClose");
                            CloseDesktop("SkinningLoot");
                            Respawn("CombatResultsSkinning1");
                            Respawn("CombatResultsSkinning2");
                            Respawn("CombatResultsSkinning3");
                        });
                    }
                    else if (CDesktop.title == "ContainerLoot")
                    {
                        AddLine(openedItem.name.CutTail() + ":");
                        AddSmallButton("OtherClose", (h) =>
                        {
                            if (openedItem.itemsInside.Count == 0)
                                currentSave.player.inventory.RemoveItem(openedItem);
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
                        AddLine("Combat spoils" + ":");
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
            if (complex == null) return;
            if (WindowUp("Quest")) return;
            if (WindowUp("QuestAdd")) return;
            if (WindowUp("QuestTurn")) return;
            var isNight = currentSave.IsNight();
            var music = isNight ? complex.musicDay : complex.musicNight;
            var ambience = isNight ? complex.ambienceDay : complex.ambienceNight;
            if (music == null)
            {
                var zone = zones.Find(x => x.name == complex.zone);
                if (zone != null) PlayMusic(isNight ? zone.musicNight : zone.musicDay);
                else StopMusic();
            }
            else PlayMusic(music);
            if (ambience == null)
            {
                var zone = zones.Find(x => x.name == complex.zone);
                if (zone != null) PlayAmbience(isNight ? zone.ambienceNight : zone.ambienceDay);
                else StopAmbience();
            }
            else PlayAmbience(ambience);
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
            var range = (99, 0);
            var areas = complex.sites.Where(x => x["SiteType"] == "HostileArea").Select(x => SiteArea.areas.Find(y => y.name == x["SiteName"]).recommendedLevel).Where(x => x[currentSave.playerSide] > 0).ToList();
            if (areas.Count > 0)
            {
                var min = areas.Min(x => x[currentSave.playerSide]);
                var max = areas.Max(x => x[currentSave.playerSide]);
                if (range.Item1 > min) range = (min, range.Item2);
                if (range.Item2 < max) range = (range.Item1, max);
            }
            var ranges = complex.sites.Where(x => x["SiteType"] == "Dungeon" || x["SiteType"] == "Raid").Select(x => instances.Find(y => y.name == x["SiteName"]).LevelRange()).ToList();
            if (ranges.Count > 0)
            {
                var min = ranges.Min(x => x.Item1);
                var max = ranges.Max(x => x.Item2);
                if (range.Item1 > min) range = (min, range.Item2);
                if (range.Item2 < max) range = (range.Item1, max);
            }
            if (range.Item2 > 0 && complex.type != "Capital")
                AddPaddingRegion(() =>
                {
                    AddLine("Level range: ", "DarkGray");
                    AddText(range.Item1 + "", ColorEntityLevel(currentSave.player, range.Item1));
                    AddText(" - ", "DarkGray");
                    AddText(range.Item2 + "", ColorEntityLevel(currentSave.player, range.Item2));
                });
            foreach (var site in complex.sites)
                PrintComplexSite(site);
        }),
        
        //Instance
        new("Instance", () =>
        {
            if (instance == null) return;
            if (WindowUp("Quest")) return;
            if (WindowUp("QuestAdd")) return;
            if (WindowUp("QuestTurn")) return;
            var isNight = currentSave.IsNight();
            var music = isNight ? instance.musicDay : instance.musicNight;
            var ambience = isNight ? instance.ambienceDay : instance.ambienceNight;
            if (music == null)
            {
                var zone = zones.Find(x => x.name == instance.zone);
                if (zone != null) PlayMusic(isNight ? zone.musicNight : zone.musicDay);
                else StopMusic();
            }
            else PlayMusic(music);
            if (ambience == null)
            {
                var zone = zones.Find(x => x.name == instance.zone);
                if (zone != null) PlayAmbience(isNight ? zone.ambienceNight : zone.ambienceDay);
                else StopAmbience();
            }
            else PlayAmbience(ambience);
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
            if (instance.wings.Count == 1)
            {
                AddPaddingRegion(() =>
                {
                    AddLine("Level range: ", "DarkGray");
                    var range = instance.LevelRange();
                    AddText(range.Item1 + "", ColorEntityLevel(currentSave.player, range.Item1));
                    AddText(" - ", "DarkGray");
                    AddText(range.Item2 + "", ColorEntityLevel(currentSave.player, range.Item2));
                });
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
                            SiteArea.area = find;
                            if (currentSave.player.QuestsAt(SiteArea.area).Count == 0)
                                CloseWindow("AreaQuestTracker");
                            else Respawn("AreaQuestTracker", true);
                            Respawn("Area");
                            Respawn("AreaProgress");
                            Respawn("AreaElites");
                            Respawn("AreaQuestAvailable");
                            Respawn("AreaQuestDone");
                            Respawn("Chest");
                            CloseWindow("Person");
                            CloseWindow("Persons");
                            SetDesktopBackground(find.Background());
                        });
                    else AddHeaderRegion(() => AddLine("?", "DimGray"));
                }
            }
            else
                for (int i = 0; i < instance.wings.Count; i++)
                {
                    var index = i;
                    var find = instance.wings[index];
                    if (showAreasUnconditional || find.areas.Any(x => x.ContainsKey("OpenByDefault") && x["OpenByDefault"] == "True" || currentSave.unlockedAreas.Contains(x["AreaName"])))
                    {
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
                        AddPaddingRegion(() =>
                        {
                            AddLine("Level range: ", "DarkGray");
                            var range = instance.LevelRange(index);
                            AddText(range.Item1 + "", ColorEntityLevel(currentSave.player, range.Item1));
                            AddText(" - ", "DarkGray");
                            AddText(range.Item2 + "", ColorEntityLevel(currentSave.player, range.Item2));
                        });
                    }
                    else
                    {
                        AddHeaderRegion(() => AddLine("?", "DarkGray"));
                        AddPaddingRegion(() => AddLine("Level range: ? - ?", "DimGray"));
                    }
                }
        }),
        new("InstanceWing", () =>
        {
            if (wing == null) return;
            if (WindowUp("Quest")) return;
            if (WindowUp("QuestAdd")) return;
            if (WindowUp("QuestTurn")) return;
            SetAnchor(TopRight, -19, -57);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() => 
            {
                AddLine(wing.name + ":", "Gray");
                AddSmallButton("OtherReverse",
                (h) =>
                {
                    wing = null;
                    area = null;
                    SetDesktopBackground(instance.Background());
                    CloseWindow("Area");
                    CloseWindow("AreaQuestTracker");
                    CloseWindow("AreaProgress");
                    CloseWindow("AreaElites");
                    CloseWindow("AreaQuestAvailable");
                    CloseWindow("AreaQuestDone");
                    CloseWindow("Chest");
                    CloseWindow("InstanceWing");
                    Respawn("Instance");
                });
            });
            AddPaddingRegion(() =>
            {
                AddLine("Level range: ", "DarkGray");
                var range = instance.LevelRange(instance.wings.IndexOf(wing));
                AddText(range.Item1 + "", ColorEntityLevel(currentSave.player, range.Item1));
                AddText(" - ", "DarkGray");
                AddText(range.Item2 + "", ColorEntityLevel(currentSave.player, range.Item2));
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
                        SiteArea.area = find;
                        if (currentSave.player.QuestsAt(SiteArea.area).Count == 0)
                            CloseWindow("AreaQuestTracker");
                        else Respawn("AreaQuestTracker", true);
                        Respawn("Area");
                        Respawn("AreaProgress");
                        Respawn("AreaElites");
                        Respawn("AreaQuestAvailable");
                        Respawn("AreaQuestDone");
                        Respawn("Chest");
                        CloseWindow("Person");
                        CloseWindow("Persons");
                        SetDesktopBackground(find.Background());
                    });
                else AddHeaderRegion(() => AddLine("?", "DimGray"));
            }
        }),

        //Area
        new("Area", () =>
        {
            var isNight = currentSave.IsNight();
            var music = isNight ? area.musicDay : area.musicNight;
            var ambience = isNight ? area.ambienceDay : area.ambienceNight;
            if (music == null)
            {
                var zone = zones.Find(x => x.name == area.zone);
                if (zone != null) PlayMusic(isNight ? zone.musicNight : zone.musicDay);
                else StopMusic();
            }
            else PlayMusic(music);
            if (ambience == null)
            {
                var zone = zones.Find(x => x.name == area.zone);
                if (zone != null) PlayAmbience(isNight ? zone.ambienceNight : zone.ambienceDay);
                else StopAmbience();
            }
            else PlayAmbience(ambience);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine(area.name, "Gray");
                AddSmallButton("OtherClose",
                (h) =>
                {
                    area = null;
                    CloseDesktop("Area");
                    PlaySound("DesktopInstanceClose");
                    if (instance != null || complex != null)
                    {
                        area = null;
                        CloseWindow("Area");
                        CloseWindow("AreaQuestTracker");
                        CloseWindow("AreaQuestAvailable");
                        CloseWindow("AreaQuestDone");
                        CloseWindow("AreaProgress");
                        CloseWindow("AreaElites");
                        CloseWindow("Persons");
                        CloseWindow("Chest");
                        PlaySound("DesktopButtonClose");
                    }
                    if (instance != null) SetDesktopBackground(wing != null ? wing.Background() : instance.Background());
                    else if (complex != null) SetDesktopBackground(complex.Background());
                    else SwitchDesktop("Map");
                });
                //if (area.fishing)
                //    AddSmallButton("OtherFish" + (!currentSave.player.professionSkills.ContainsKey("Fishing") ? "Off" : ""),
                //    (h) =>
                //    {
                //        if (currentSave.player.professionSkills.ContainsKey("Fishing"))
                //        {
                //            fishingSpot = fishingSpots.Find(x => x.name == area.name);
                //            SpawnDesktopBlueprint("FishingGame");
                //        }
                //    });
            });
            if (WindowUp("Persons")) return;
            if (transportationConnectedToSite.ContainsKey(area.name))
            {
                var transportOptions = transportationConnectedToSite[area.name];
                AddPaddingRegion(() => { AddLine("Transportation:", "HalfGray"); });
                foreach (var transport in transportOptions)
                {
                    if (transport.sites.Count < 2) continue;
                    var destination = FindSite(x => x.name != area.name && transport.sites.Contains(x.name));
                    if (destination == null) continue;
                    AddButtonRegion(() =>
                    {
                        AddLine(destination.convertDestinationTo ?? destination.name, "Black");
                        AddSmallButton("Transport" + transport.means);
                    },
                    (h) =>
                    {
                        //Set the destination
                        var destination = FindSite(x => x.name != area.name && transport.sites.Contains(x.name));

                        //Pay the toll
                        if (transport.price > 0)
                        {
                            if (transport.price > currentSave.player.inventory.money)
                            {
                                SpawnFallingText(new Vector2(0, 34), "Not enough money", "Red");
                                return;
                            }
                            PlaySound("DesktopTransportPay");
                            currentSave.player.inventory.money -= transport.price;
                        }

                        //Set the new site as current
                        currentSave.currentSite = destination.convertDestinationTo ?? destination.name;

                        currentSave.AddTime(transport.time);
                        transport.PlayPathEndSound();

                        //Close area screen as we're beginning to travel on map
                        CloseDesktop("Area");
                        CloseDesktop("Complex");
                        CloseDesktop("Instance");

                        //Switch desktop to map
                        SwitchDesktop("Map");

                        //Explore the site if it wasnt explored
                        if (!currentSave.Visited(currentSave.currentSite))
                        {
                            currentSave.siteVisits.Add(currentSave.currentSite, 0);
                            PlaySound("DesktopZoneDiscovered", 1f);
                            currentSave.player.ReceiveExperience(defines.expForExploration);
                            foreach (var connection in paths.FindAll(x => x.sites.Contains(currentSave.currentSite)).Where(x => x.onlyFor == null || x.onlyFor == currentSave.playerSide))
                            {
                                var site = connection.sites.Find(x => x != currentSave.currentSite);
                                if (!WindowUp("Site: " + site))
                                    if (!Respawn("Site: " + site))
                                        CDesktop.LBWindow().GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.gameObject.AddComponent<FadeIn>());
                            }
                        }

                        if (area.convertDestinationTo != null) Respawn("Site: " + area.convertDestinationTo);
                        else Respawn("Site: " + area.name);

                        var areaOfDestination = areas.Find(x => x.name == currentSave.currentSite);
                        if (areaOfDestination != null)
                        {
                            area = areaOfDestination;
                            if (area.convertDestinationTo != null) Respawn("Site: " + area.convertDestinationTo);
                            else Respawn("Site: " + currentSave.currentSite);

                            //Move camera to the newly visited area
                            CDesktop.cameraDestination = new Vector2(area.x, area.y);
                        }
                    },
                    null,
                    (h) => () => { transport.PrintTooltip(); });
                }
            }
            var validPeople = area.people == null ? new() : area.people.Where(x => !x.hidden && (x.faction == null || currentSave.player.Reputation(x.faction) >= ReputationRankToAmount("Neutral"))).OrderBy(x => x.type).ToList();
            if (validPeople.Count > 0)
            {
                var groups = validPeople.GroupBy(x => x.category).OrderBy(x => x.Count()).ThenBy(x => x.Key != null ? x.Key.priority : 0);
                AddPaddingRegion(() => { AddLine("Points of interest:", "HalfGray"); });
                foreach (var group in groups)
                    if (group.Key == null) continue;
                    else if (group.Key.category == "Flight Master")
                        foreach (var person in group)
                        {
                            var faction = factions.Find(x => x.name == person.faction);
                            faction ??= factions.Find(x => x.name == races.Find(y => y.name == person.race).faction);
                            faction ??= factions.Find(x => x.name == currentSave.player.faction);
                            if (faction.side == currentSave.playerSide)
                            {
                                var personType = personTypes.Find(x => x.type == person.type);
                                AddButtonRegion(() =>
                                {
                                    AddLine(person.name, "Black");
                                    AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? faction.side : "") : "OtherUnknown");
                                },
                                (h) =>
                                {
                                    quest = null;
                                    Person.person = person;
                                    CloseWindow(h.window.title);
                                    Respawn("Person");
                                    CloseWindow("Instance");
                                    CloseWindow("InstanceWing");
                                    CloseWindow("Complex");
                                    CloseWindow("QuestAdd");
                                    CloseWindow("QuestTurn");
                                    CloseWindow("AreaQuestAvailable");
                                    CloseWindow("AreaQuestDone");
                                    CloseWindow("AreaProgress");
                                    CloseWindow("AreaElites");
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
                                AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? factions.Find(x => x.name == area.faction).side : "") : "OtherUnknown");
                            },
                            (h) =>
                            {
                                quest = null;
                                personCategory = null;
                                Person.person = person;
                                CloseWindow(h.window.title);
                                Respawn("Person");
                                CloseWindow("Instance");
                                CloseWindow("InstanceWing");
                                CloseWindow("Complex");
                                CloseWindow("QuestAdd");
                                CloseWindow("QuestTurn");
                                CloseWindow("AreaQuestAvailable");
                                CloseWindow("AreaQuestDone");
                                CloseWindow("AreaProgress");
                                CloseWindow("AreaElites");
                                CloseWindow("Chest");
                                PlayVoiceLine(person.VoiceLine("Greeting"));
                                PlaySound("DesktopInstanceOpen");
                            });
                        }
                    else
                    {
                        var person = group.First();
                        AddButtonRegion(() =>
                        {
                            AddLine(group.Key.category + "s (" + group.Count() + ")");
                            AddSmallButton(person.category != null ? person.category.icon + (person.category.factionVariant ? factions.Find(x => x.name == area.faction).side : "") : "OtherUnknown");
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
            var levelHigherThanZero = area.recommendedLevel[currentSave.playerSide] > 0;
            var tracker = WindowUp("AreaQuestTracker");
            var questsHere = currentSave.player.QuestsAt(area).Count > 0;
            if (levelHigherThanZero || tracker || questsHere)
                AddPaddingRegion(() =>
                {
                    if (levelHigherThanZero)
                    {
                        AddLine("Recommended level: ", "HalfGray");
                        AddText(area.recommendedLevel[currentSave.playerSide] + "", ColorEntityLevel(currentSave.player, area.recommendedLevel[currentSave.playerSide]));
                    }
                    if (tracker) AddSmallButton("OtherQuestClose", (h) =>
                    {
                        CloseWindow("AreaQuestTracker");
                        Respawn("AreaQuestAvailable");
                    });
                    else if (questsHere) AddSmallButton("OtherQuestOpen", (h) =>
                    {
                        CloseWindow("AreaQuestAvailable");
                        Respawn("AreaQuestTracker");
                    });
                });
            var common = area.CommonEncounters(currentSave.playerSide);
            if (common != null && common.Count > 0)
                foreach (var encounter in common)
                    AddHeaderRegion(() =>
                    {
                        AddLine(encounter.who, "DarkGray", "Right");
                        var race = races.Find(x => x.name == encounter.who);
                        AddSmallButton(race == null ? "OtherUnknown" : race.portrait);
                    });
        }),
        new("AreaHostile", () =>
        {
            var isNight = currentSave.IsNight();
            var music = isNight ? area.musicDay : area.musicNight;
            var ambience = isNight ? area.ambienceDay : area.ambienceNight;
            if (music == null)
            {
                var zone = zones.Find(x => x.name == area.zone);
                if (zone != null) PlayMusic(isNight ? zone.musicNight : zone.musicDay);
                else StopMusic();
            }
            else PlayMusic(music);
            if (ambience == null)
            {
                var zone = zones.Find(x => x.name == area.zone);
                if (zone != null) PlayAmbience(isNight ? zone.ambienceNight : zone.ambienceDay);
                else StopAmbience();
            }
            else PlayAmbience(ambience);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine(area.name);
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
                var rank = currentSave.player.ReputationRank(area.faction);
                if (rank == "Hated")
                {
                    AddLine("This town's folk consider you", "HalfGray");
                    AddLine("to be their enemy", "HalfGray");
                }
                else if (rank == "Hostile")
                {
                    AddLine("This town's folk consider you", "HalfGray");
                    AddLine("to be an enemy", "HalfGray");
                }
                else if (rank == "Unfriendly")
                {
                    AddLine("This town's folk are reluctant", "HalfGray");
                    AddLine("towards you. Consider", "HalfGray");
                    AddLine("improving your reputation", "HalfGray");
                    AddLine("with ", "HalfGray");
                    AddText(area.faction, "Unfriendly");
                    AddLine("in order to be welcomed here", "HalfGray");
                }
            });
        }),
        new("AreaProgress", () =>
        {
            if (area.recommendedLevel[currentSave.playerSide] == 0) return;
            SetAnchor(BottomLeft, 19, 35);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var currentProgress = currentSave.siteProgress.ContainsKey(area.name) ? currentSave.siteProgress[area.name] : 0;
            AddButtonRegion(() =>
            {
                AddLine("Explore", "Black");
            },
            (h) =>
            {
                NewBoard(area.RollEncounters(area.instancePart ? 2 : 1), area);
                SpawnDesktopBlueprint("Game");
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
                        var setWidth = (i == area.areaSize - 1 ? 190 % area.areaSize : 0) + 190 / area.areaSize;
                        SetRegionGroupWidth(setWidth);
                        SetRegionGroupHeight(thickness);
                        AddPaddingRegion(() =>
                        {
                            if (currentProgress > index)
                            {
                                SetRegionBackground(ProgressDone);
                                positionOfElite = CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position.x + setWidth;
                            }
                            else SetRegionBackground(ProgressEmpty);
                        });
                    }
                }
        }),
        new("AreaElites", () =>
        {
            if (area == null || area.eliteEncounters == null || area.eliteEncounters.Count == 0) return;
            var boss = area.progression.Find(x => x.type == "Boss" && currentSave.siteProgress.ContainsKey(area.name) && x.point == currentSave.siteProgress[area.name]);
            if (boss == null) return;
            var bossName = boss.bossName;
            if (currentSave.generatedBossPools.ContainsKey(bossName))
                bossName = currentSave.generatedBossPools[bossName];
            if (boss == null || currentSave.elitesKilled.ContainsKey(bossName)) return;
            var encounter = area.eliteEncounters.Find(x => x.who == bossName);
            if (encounter == null) return;
            SetAnchor(-301 + positionOfElite - 19, -69);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                var race = races.Find(x => x.name == encounter.who);
                SpawnFloatingText(new Vector3(6, -9), encounter.levelMin - 10 > currentSave.player.level ? "??" : "" + encounter.levelMin, ColorEntityLevel(currentSave.player, encounter.levelMin), "DimGray", "Left");
                AddBigButton(race == null ? "OtherUnknown" : race.portrait,
                (h) =>
                {
                    NewBoard(new() { area.RollEncounter(encounter) }, area);
                    SpawnDesktopBlueprint("Game");
                },
                null,
                (h) => () =>
                {
                    SetAnchor(-301 + positionOfElite + 19, -69);
                    AddHeaderGroup();
                    SetRegionGroupHeight(36);
                    AddHeaderRegion(() => AddLine(race.name));
                    AddPaddingRegion(() => AddLine("Defeat this elite to progress  ", "DarkGray"));
                });
                var marker = new GameObject("EliteMarker", typeof(SpriteRenderer));
                marker.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/ProgressBossExpander");
                marker.GetComponent<SpriteRenderer>().sortingOrder = -1;
                marker.transform.parent = CDesktop.LBWindow().LBRegionGroup().LBRegion().transform;
                marker.transform.localPosition = new Vector3(20, -45);
            });
        }),
        new("AreaQuestTracker", () =>
        {
            SetAnchor(Top, 0, -38);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            var q = currentSave.player.QuestsAt(area);
            foreach (var quest in q)
            {
                var ogQuest = Quest.quests.Find(x => x.questID == quest.questID);
                var con = quest.conditions.FindAll(x => !x.IsDone() && x.Where(true).Contains(area));
                AddButtonRegion(() =>
                {
                    AddLine(ogQuest.name, "Black");
                    AddSmallButton(ogQuest.ZoneIcon());
                },
                (h) =>
                {
                    SwitchDesktop("Map");
                    PlaySound("DesktopInventoryOpen");
                    SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
                    Respawn("QuestList");
                    Respawn("MapToolbar");
                    Quest.quest = ogQuest;
                    if (staticPagination.ContainsKey("Quest"))
                        staticPagination.Remove("Quest");
                    Respawn("Quest");
                });
                var color = ColorQuestLevel(ogQuest.questLevel);
                if (color != null) SetRegionBackgroundAsImage("SkillUp" + color);
                if (con.Count > 0)
                    foreach (var condition in con)
                        condition.Print(false);
            }
        }),
        new("AreaQuestAvailable", () => 
        {
            var quests = currentSave.player.AvailableQuestsAt(area).OrderBy(x => x.questLevel).ToList();
            if (quests.Count == 0) return;
            SetAnchor(Top, 0, -38);
            AddQuestList(quests);
        }),
        new("AreaQuestDone", () => 
        {
            var quests = currentSave.player.QuestsDoneAt(area).Select(x => Quest.quests.Find(y => y.questID == x.questID)).OrderBy(x => x.questLevel).ToList();
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
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == area.faction).side : ""));
            });
            if (type.category == "Class Trainer")
            {
                if (person.type.ToLower().Contains(currentSave.player.spec.ToLower())) { }
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
                    CloseWindow("Area");
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
                            CloseWindow("Area");
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
                    if (!currentSave.banks.ContainsKey(area.name))
                        currentSave.banks.Add(area.name, new() { items = new() });
                    PlaySound("DesktopBankOpen", 0.4f);
                    CloseWindow(h.window);
                    CloseWindow("Area");
                    SpawnWindowBlueprint("Bank");
                    SpawnWindowBlueprint("Inventory");
                    Respawn("PlayerMoney");
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
                    auctionCategory = "";
                    UpdateAuctionGroupList();
                    PlaySound("DesktopAuctionOpen", 0.4f);
                    showSwords = new(true);
                    showAxes = new(true);
                    showMaces = new(true);
                    showPolearms = new(true);
                    showStaves = new(true);
                    showDaggers = new(true);
                    showWands = new(true);
                    showCloth = new(true);
                    showLeather = new(true);
                    showMail = new(true);
                    showPlate = new(true);
                    showNonShield = new(true);
                    showShield = new(true);
                    showBows = new(true);
                    showCrossbows = new(true);
                    showGuns = new(true);
                    showHead = new(true);
                    showShoulders = new(true);
                    showBack = new(true);
                    showChest = new(true);
                    showWrists = new(true);
                    showHands = new(true);
                    showWaist = new(true);
                    showLegs = new(true);
                    showFeet = new(true);
                    showNeck = new(true);
                    showFinger = new(true);
                    showTrinket = new(true);
                    showFood = new(true);
                    showScrolls = new(true);
                    showPotions = new(true);
                    showBattleElixirs = new(true);
                    SpawnWindowBlueprint("AuctionHouseOffersGroups");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                    SpawnWindowBlueprint("AuctionHouseFilteringMain");
                    CloseWindow(h.window);
                    CloseWindow("Area");
                });
            }
            else if (type.category == "Innkeeper")
            {
                AddButtonRegion(() =>
                {
                    AddLine("I want to rest in this inn");
                });
                if (currentSave.player.homeLocation != area.name)
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
                            currentSave.player.inventory.AddItem(item.CopyItem());
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
                    CloseWindow("Area");
                    SpawnWindowBlueprint("MountCollection");
                    if (mounts.Find(x => x.name == currentSave.player.mount) != null)
                        SpawnWindowBlueprint("CurrentMount");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
                if (mounts.Count(x => !currentSave.player.mounts.Contains(x.name) && x.factions != null && x.factions.Contains(person.faction == null ? area.faction : person.faction)) > 0)
                    AddButtonRegion(() =>
                    {
                        AddLine("I want to buy a new mount");
                    },
                    (h) =>
                    {
                        PlaySound("DesktopInventoryOpen");
                        CloseWindow(h.window);
                        CloseWindow("Area");
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
                    PlaySound("DesktopMenuOpen");
                    CloseWindow(h.window);
                    CloseWindow("Area");
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
                    if (!currentSave.vendorStock.ContainsKey(area.name + ":" + person.name) && person.itemsSold != null && person.itemsSold.Count > 0)
                        currentSave.vendorStock.Add(area.name + ":" + person.name, person.ExportStock());
                    PlayVoiceLine(person.VoiceLine("Vendor"));
                    PlaySound("DesktopInventoryOpen");
                    PlaySound("DesktopCharacterSheetOpen");
                    CloseWindow(h.window);
                    CloseWindow("Area");
                    SpawnWindowBlueprint("Vendor");
                    SpawnWindowBlueprint("Inventory");
                    Respawn("PlayerMoney");
                    Respawn("ExperienceBarBorder");
                    Respawn("ExperienceBar");
                });
            }
            AddButtonRegion(() => AddLine("Goodbye"),
            (h) =>
            {
                PlayVoiceLine(person.VoiceLine("Farewell"));
                PlaySound("DesktopInstanceClose");
                person = null;
                CloseWindow(h.window);
                if (personCategory != null) Respawn("Persons");
                else CloseWindow("Persons");
                Respawn("Area");
                Respawn("Complex");
                Respawn("Quest");
                Respawn("QuestAdd");
                Respawn("QuestTurn");
                Respawn("AreaQuestAvailable");
                Respawn("AreaQuestDone");
                if (!WindowUp("Persons"))
                {
                    Respawn("AreaProgress");
                    Respawn("AreaElites");
                    Respawn("Chest");
                }
            });
        }, true),
        new("Persons", () => {
            SetAnchor(TopLeft, 19, -57);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddPaddingRegion(() =>
            {
                AddLine(personCategory.category + "s:", "HalfGray");
                AddSmallButton("OtherReverse",
                (h) =>
                {
                    quest = null;
                    personCategory = null;
                    CloseWindow(h.window.title);
                    Respawn("Area");
                    Respawn("Complex");
                    Respawn("Quest");
                    Respawn("QuestAdd");
                    Respawn("QuestTurn");
                    Respawn("AreaProgress");
                    Respawn("AreaElites");
                    Respawn("AreaQuestAvailable");
                    Respawn("AreaQuestDone");
                    Respawn("Chest");
                    PlaySound("DesktopInstanceClose");
                });
            });
            var people = area.people.FindAll(x => x.category == personCategory && !x.hidden);
            foreach (var person in people)
            {
                var personType = personTypes.Find(x => x.type == person.type);
                AddButtonRegion(() =>
                {
                    AddLine(person.name, "Black");
                    AddSmallButton(personType != null ? personType.icon + (personType.factionVariant ? factions.Find(x => x.name == area.faction).side : "") : "OtherUnknown");
                },
                (h) =>
                {
                    quest = null;
                    Person.person = person;
                    Respawn("Person");
                    CloseWindow("Complex");
                    CloseWindow("Persons");
                    CloseWindow("Area");
                    CloseWindow("QuestAdd");
                    CloseWindow("QuestTurn");
                    CloseWindow("AreaProgress");
                    CloseWindow("AreaElites");
                    CloseWindow("AreaQuestAvailable");
                    CloseWindow("AreaQuestDone");
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
            var items = currentSave.vendorStock[area.name + ":" + person.name];
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("Vendor");
                    CloseWindow("Inventory");
                    CloseWindow("InventorySort");
                    Respawn("PlayerMoney");
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
                    CloseWindow("InventorySort");
                    Respawn("PlayerMoney");
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
        new("MakeInnHome", () => {
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            var type = personTypes.Find(x => x.type == person.type);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == area.faction).side : ""));
            });
            AddPaddingRegion(() =>
            {
                AddLine("Do you want to change your", "DarkGray");
                AddLine("home from ", "DarkGray");
                AddText(currentSave.player.homeLocation, "LightGray");
                AddLine("to ", "DarkGray");
                AddText(area.name, "LightGray");
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
                currentSave.player.homeLocation = area.name;
                CloseWindow("MakeInnHome");
                Respawn("Person");
            });
        }),
        new("MountCollection", () => {
            var rowAmount = 6;
            var thisWindow = CDesktop.LBWindow();
            var list = currentSave.player.mounts;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
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
            AddPaginationLine();
            var mounts = currentSave.player.mounts.Select(x => Mount.mounts.Find(y => y.name == x)).ToList();
            mounts.RemoveAll(x => x.name == currentSave.player.mount);
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                AddPaddingRegion(() =>
                {
                    if (mounts.Count > index + thisWindow.pagination())
                    {
                        var mount = mounts[index + thisWindow.pagination()];
                        AddLine(mount.name, mount.speed == 7 ? "Rare" : "Epic");
                        AddLine("Speed: ", "DarkGray");
                        AddText(mount.speed == 7 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"));
                        AddBigButton(mount.icon,
                            (h) =>
                            {
                                var mount = mounts[index + thisWindow.pagination()];
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
                                var mount = mounts[index + thisWindow.pagination()];
                                PrintMountTooltip(currentSave.player, mount);
                            }
                        );
                        if (currentSave.player.level < (mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts)) { SetBigButtonToRedscale(); AddBigButtonOverlay("OtherGridBlurred"); }
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
            var rowAmount = 6;
            var thisWindow = CDesktop.LBWindow();
            var mounts = Mount.mounts.FindAll(x => !currentSave.player.mounts.Contains(x.name) && x.factions != null && x.factions.Contains(person.faction == null ? area.faction : person.faction)).OrderBy(x => x.speed).ThenBy(x => x.price).ThenBy(x => x.name).ToList();
            var list = mounts;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
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
            AddPaginationLine();
            for (int i = 0; i < 6; i++)
            {
                var index = i;
                if (mounts.Count >= index + thisWindow.pagination())
                    AddPaddingRegion(() =>
                    {
                        if (mounts.Count > index + thisWindow.pagination())
                        {
                            var mount = mounts[index + thisWindow.pagination()];
                            AddLine(mount.name, mount.speed == 7 ? "Rare" : "Epic");
                            AddLine("Speed: ", "DarkGray");
                            AddText(mount.speed == 7 ? "Fast" : (mount.speed == 9 ? "Very Fast" : "Normal"));
                            AddBigButton(mount.icon,
                                (h) =>
                                {
                                    var mount = mounts[index + thisWindow.pagination()];
                                    if (currentSave.player.inventory.money >= mount.price)
                                    {
                                        currentSave.player.inventory.money -= mount.price;
                                        Respawn("PlayerMoney");
                                        currentSave.player.mounts.Add(mount.name);
                                        Respawn("MountVendor");
                                        PlaySound("DesktopTransportPay");
                                    }
                                },
                                null,
                                (h) => () =>
                                {
                                    var mount = mounts[index + thisWindow.pagination()];
                                    PrintMountTooltip(currentSave.player, mount);
                                }
                            );
                            if (currentSave.player.level < (mount.speed == 7 ? defines.lvlRequiredFastMounts : defines.lvlRequiredVeryFastMounts))
                            {
                                SetBigButtonToRedscale();
                                AddBigButtonOverlay("OtherGridBlurred");
                            }
                        }
                        else if (mounts.Count == index + thisWindow.pagination())
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
            AddHeaderGroup();
            SetRegionGroupHeight(281);
            var items = currentSave.banks[area.name].items;
            AddHeaderRegion(() =>
            {
                var type = personTypes.Find(x => x.type == person.type);
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("Bank");
                    CloseWindow("BankSort");
                    CloseWindow("Inventory");
                    CloseWindow("InventorySort");
                    Respawn("Person");
                    PlaySound("DesktopInventoryClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Bank:");
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
                        {
                            var findItem = items.Find(x => x.y == index && x.x == j);
                            if (findItem != null) PrintBankItem(findItem);
                            else if (movingItem != null) AddBigButton("OtherEmpty", (h) => PutDownMovingItem(h));
                            else AddBigButton("OtherEmpty");
                        }
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
                currentSave.banks[area.name].items = currentSave.banks[area.name].items.OrderBy(x => x.name).ToList();
                currentSave.banks[area.name].ApplySortOrder();
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
                currentSave.banks[area.name].items = currentSave.banks[area.name].items.OrderBy(x => x.amount).ToList();
                currentSave.banks[area.name].ApplySortOrder();
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
                currentSave.banks[area.name].items = currentSave.banks[area.name].items.OrderByDescending(x => x.ilvl).ToList();
                currentSave.banks[area.name].ApplySortOrder();
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
                currentSave.banks[area.name].items = currentSave.banks[area.name].items.OrderByDescending(x => x.price).ToList();
                currentSave.banks[area.name].ApplySortOrder();
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
                currentSave.banks[area.name].items = currentSave.banks[area.name].items.OrderByDescending(x => x.type).ToList();
                currentSave.banks[area.name].ApplySortOrder();
                CloseWindow("BankSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("FlightMaster", () => {
            var rowAmount = 12;
            var thisWindow = CDesktop.LBWindow();
            var side = currentSave.playerSide;
            var list = area.flightPaths[side].FindAll(x => x != area).OrderBy(x => !currentSave.siteVisits.ContainsKey(x.convertDestinationTo ?? x.name)).ThenBy(x => x.zone).ThenBy(x => x.name).ToList();
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
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
                    PlaySound("DesktopMenuClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Possible destinations:");
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                if (list.Count > index + thisWindow.pagination())
                    AddButtonRegion(() =>
                    {
                        var destination = list[index + thisWindow.pagination()];
                        if (currentSave.siteVisits.ContainsKey(destination.convertDestinationTo ?? destination.name))
                        {
                            AddLine(destination.convertDestinationTo ?? destination.name);
                            var zone = zones.Find(x => x.name == destination.zone);
                            AddSmallButton("Zone" + zone.icon.Clean());
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
                        var destination = list[index + thisWindow.pagination()];
                        var siteOfDestination = FindSite(x => x.name == (destination.convertDestinationTo ?? destination.name));
                        var fromWhere = FindSite(x => x.name == currentSave.currentSite);
                        currentSave.currentSite = destination.convertDestinationTo ?? destination.name;
                        var distance = Math.Abs(siteOfDestination.x - fromWhere.x) + Math.Abs(siteOfDestination.y - fromWhere.y);
                        var price = distance / 50 * 10;

                        if (price > 0)
                        {
                            if (price > currentSave.player.inventory.money)
                            { 
                                SpawnFallingText(new Vector2(0, 34), "Not enough money", "Red");
                                return;
                            }
                            PlaySound("DesktopTransportPay");
                            currentSave.player.inventory.money -= price;
                        }

                        //Close area screen as we're beginning to travel on map
                        CloseDesktop("Area");
                        CloseDesktop("Instance");
                        CloseDesktop("Complex");

                        //Switch desktop to map
                        SwitchDesktop("Map");

                        if (fromWhere.convertDestinationTo != null) Respawn("Site: " + fromWhere.convertDestinationTo);
                        else Respawn("Site: " + fromWhere.name);

                        if (siteOfDestination != null)
                        {
                            if (siteOfDestination.convertDestinationTo != null) Respawn("Site: " + siteOfDestination.convertDestinationTo);
                            else Respawn("Site: " + currentSave.currentSite);

                            //Move camera to the newly visited area
                            CDesktop.cameraDestination = new Vector2(siteOfDestination.x, siteOfDestination.y);
                        }
                    },
                    null,
                    (h) => () =>
                    {
                        var destination = list[index + thisWindow.pagination()];
                        var siteOfDestination = FindSite(x => x.name == (destination.convertDestinationTo ?? destination.name));
                        var fromWhere = FindSite(x => x.name == currentSave.currentSite);
                        var distance = Math.Abs(siteOfDestination.x - fromWhere.x) + Math.Abs(siteOfDestination.y - fromWhere.y);
                        var price = distance / 50 * 10;
                        SetAnchor(Center);
                        AddHeaderGroup();
                        SetRegionGroupWidth(182);
                        AddPaddingRegion(() =>
                        {
                            AddLine("Take a flight path to:", "DarkGray");
                        });
                        AddHeaderRegion(() =>
                        {
                            AddLine(destination.convertDestinationTo ?? destination.name);
                            var zone = zones.Find(x => x.name == destination.zone);
                            AddSmallButton("Zone" + zone.icon.Clean());
                        });
                        if (price > 0)
                        {
                            AddPaddingRegion(() =>
                            {
                                AddLine("For the price of:", "DarkGray");
                            });
                            PrintPriceRegion(price, 38, 38, 49);
                        }
                    });
                else AddPaddingRegion(() => AddLine());
            }
            AddPaginationLine();
        }),
        new("AuctionHouseOffersGroups", () => {
            var rowAmount = 6;
            var thisWindow = CDesktop.LBWindow();
            var list = Market.exploredAuctionsGroups.ToList();
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
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
                    CloseWindow("AuctionHouseFilteringMain");
                    CloseWindow("AuctionHouseFilteringTwoHandedWeapons");
                    CloseWindow("AuctionHouseFilteringOneHandedWeapons");
                    CloseWindow("AuctionHouseFilteringOffHands");
                    CloseWindow("AuctionHouseFilteringRangedWeapons");
                    CloseWindow("AuctionHouseFilteringArmorClass");
                    CloseWindow("AuctionHouseFilteringArmorType");
                    CloseWindow("AuctionHouseFilteringJewelry");
                    CloseWindow("AuctionHouseFilteringConsumeables");
                    for (int i = 0; i < 12; i++) { var index = i; CloseWindow("AuctionHousePrice" + index); }
                    Respawn("Person");
                    PlaySound("DesktopInstanceClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Auctioned items:");
                if (!WindowUp("AuctionHouseOffersSettings") && !WindowUp("AuctionHouseOffersSort"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        SpawnWindowBlueprint("AuctionHouseOffersSort");
                        Respawn("AuctionHouseOffersGroups", true);
                        for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                    });
                else
                    AddSmallButton("OtherSortOff");
                //if (!WindowUp("AuctionHouseOffersSettings") && !WindowUp("AuctionHouseOffersSort"))
                //    AddSmallButton("OtherSettings", (h) =>
                //    {
                //        SpawnWindowBlueprint("AuctionHouseOffersSettings");
                //        Respawn("AuctionHouseOffersGroups", true);
                //        for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                //    });
                //else
                //    AddSmallButton("OtherSettingsOff");
            });
            auctionPriceToDisplay = new int[12];
            auctionAmountToDisplay = new int[12];
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                if (list.Count > index + thisWindow.pagination())
                    AddButtonRegion(() =>
                    {
                        var offerGroupKey = Market.exploredAuctionsGroups.Keys.ToList()[index + thisWindow.pagination()];
                        var offerGroup = Market.exploredAuctionsGroups[offerGroupKey];
                        var offerGroupFirst = Market.exploredAuctionsGroups[offerGroupKey].OrderBy(x => x.price).ToList()[0];
                        auctionPriceToDisplay[index] = offerGroupFirst.price;
                        auctionAmountToDisplay[index] = offerGroup.Where(x => x.price == offerGroupFirst.price).Sum(x => x.item.amount);
                        AddLine(offerGroupFirst.item.name.CutTail() + " x" + offerGroup.Sum(x => x.item.amount));
                        AddSmallButton(offerGroupFirst.item.icon);
                        if (settings.rarityIndicators.Value())
                            AddSmallButtonOverlay("OtherRarity" + offerGroupFirst.item.rarity, 0, 2);
                    },
                    (h) =>
                    {
                        Market.auctionGroup = Market.exploredAuctionsGroups.Keys.ToList()[index + thisWindow.pagination()];
                        Market.exploredAuctions = Market.exploredAuctionsGroups[Market.auctionGroup].OrderBy(x => x.price).ToList();
                        CloseWindow("AuctionHouseOffersGroups", false);
                        SpawnWindowBlueprint("AuctionHouseOffers");
                        String.splitAmount.Set("1");
                        SpawnWindowBlueprint("AuctionHouseBuy");
                        SpawnWindowBlueprint("AuctionHouseChosenItem");
                        for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                    },
                    null, (h) => () =>
                    {
                        var offerGroupKey = Market.exploredAuctionsGroups.Keys.ToList()[index + thisWindow.pagination()];
                        PrintItemTooltip(Market.exploredAuctionsGroups[offerGroupKey][0].item);
                    });
                else
                    AddPaddingRegion(() => AddLine());
                AddPaddingRegion(() => AddLine());
            }
            AddPaginationLine();
        }),
        new("AuctionHouseFilteringMain", () => {
            var thisWindow = CDesktop.LBWindow();
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(262);
            AddHeaderRegion(() =>
            {
                AddLine("Auction categories:", "Gray");
            });
            AddButtonRegion(() =>
            {
                AddLine("Two handed weapons");
            },
            (h) =>
            {
                auctionCategory = "Two handed weapons";
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                CloseWindow("AuctionHouseFilteringMain");
                Respawn("AuctionHouseFilteringTwoHandedWeapons");
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddLine("One handed weapons");
            },
            (h) =>
            {
                auctionCategory = "One handed weapons";
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                CloseWindow("AuctionHouseFilteringMain");
                Respawn("AuctionHouseFilteringOneHandedWeapons");
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddLine("Off hands");
            },
            (h) =>
            {
                auctionCategory = "Off hands";
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                CloseWindow("AuctionHouseFilteringMain");
                Respawn("AuctionHouseFilteringOffHands");
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddLine("Ranged weapons");
            },
            (h) =>
            {
                auctionCategory = "Ranged weapons";
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                CloseWindow("AuctionHouseFilteringMain");
                Respawn("AuctionHouseFilteringRangedWeapons");
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddLine("Armor");
            },
            (h) =>
            {
                auctionCategory = "Armor";
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                CloseWindow("AuctionHouseFilteringMain");
                Respawn("AuctionHouseFilteringArmorClass");
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddLine("Jewelry");
            },
            (h) =>
            {
                auctionCategory = "Jewelry";
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                CloseWindow("AuctionHouseFilteringMain");
                Respawn("AuctionHouseFilteringJewelry");
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddLine("Consumeables");
            },
            (h) =>
            {
                auctionCategory = "Consumeables";
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                CloseWindow("AuctionHouseFilteringMain");
                Respawn("AuctionHouseFilteringConsumeables");
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddLine("Profession recipes");
            },
            (h) =>
            {
                auctionCategory = "Profession recipes";
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddLine("Trade goods");
            },
            (h) =>
            {
                auctionCategory = "Trade goods";
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddLine("Other");
            },
            (h) =>
            {
                auctionCategory = "Other";
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
            });
        }),
        new("AuctionHouseFilteringTwoHandedWeapons", () => {
            var thisWindow = CDesktop.LBWindow();
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(262);
            AddHeaderRegion(() =>
            {
                AddLine("Auction categories:", "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    auctionCategory = "";
                    UpdateAuctionGroupList();
                    CloseWindow(h.window.title);
                    Respawn("AuctionHouseOffersGroups", true);
                    Respawn("AuctionHouseFilteringMain");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine(auctionCategory + ":", "Gray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showSwords);
                AddLine("Swords");
                AddSmallButton("ItemSword19");
            },
            (h) =>
            {
                showSwords.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showAxes);
                AddLine("Axes");
                AddSmallButton("ItemAxe21");
            },
            (h) =>
            {
                showAxes.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showMaces);
                AddLine("Maces");
                AddSmallButton("ItemHammer17");
            },
            (h) =>
            {
                showMaces.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showPolearms);
                AddLine("Polearms");
                AddSmallButton("ItemSpear06");
            },
            (h) =>
            {
                showPolearms.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showStaves);
                AddLine("Staves");
                AddSmallButton("ItemStaff27");
            },
            (h) =>
            {
                showStaves.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
            });
        }),
        new("AuctionHouseFilteringOneHandedWeapons", () => {
            var thisWindow = CDesktop.LBWindow();
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(262);
            AddHeaderRegion(() =>
            {
                AddLine("Auction categories:", "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    auctionCategory = "";
                    UpdateAuctionGroupList();
                    CloseWindow(h.window.title);
                    Respawn("AuctionHouseOffersGroups", true);
                    Respawn("AuctionHouseFilteringMain");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine(auctionCategory + ":", "Gray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showSwords);
                AddLine("Swords");
                AddSmallButton("ItemSword20");
            },
            (h) =>
            {
                showSwords.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showAxes);
                AddLine("Axes");
                AddSmallButton("ItemAxe01");
            },
            (h) =>
            {
                showAxes.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showMaces);
                AddLine("Maces");
                AddSmallButton("ItemMace01");
            },
            (h) =>
            {
                showMaces.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showDaggers);
                AddLine("Daggers");
                AddSmallButton("ItemShortBlade14");
            },
            (h) =>
            {
                showDaggers.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showWands);
                AddLine("Wands");
                AddSmallButton("ItemWand02");
            },
            (h) =>
            {
                showWands.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
            });
        }),
        new("AuctionHouseFilteringOffHands", () => {
            var thisWindow = CDesktop.LBWindow();
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(262);
            AddHeaderRegion(() =>
            {
                AddLine("Auction categories:", "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    auctionCategory = "";
                    UpdateAuctionGroupList();
                    CloseWindow(h.window.title);
                    Respawn("AuctionHouseOffersGroups", true);
                    Respawn("AuctionHouseFilteringMain");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine(auctionCategory + ":", "Gray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showNonShield);
                AddLine("Non shield");
                AddSmallButton("ItemMiscLantern01");
            },
            (h) =>
            {
                showNonShield.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showShield);
                AddLine("Shield");
                AddSmallButton("ItemShield04");
            },
            (h) =>
            {
                showShield.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
            });
        }),
        new("AuctionHouseFilteringRangedWeapons", () => {
            var thisWindow = CDesktop.LBWindow();
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(262);
            AddHeaderRegion(() =>
            {
                AddLine("Auction categories:", "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    auctionCategory = "";
                    UpdateAuctionGroupList();
                    CloseWindow(h.window.title);
                    Respawn("AuctionHouseOffersGroups", true);
                    Respawn("AuctionHouseFilteringMain");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine(auctionCategory + ":", "Gray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showBows);
                AddLine("Bows");
                AddSmallButton("ItemBow02");
            },
            (h) =>
            {
                showBows.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showCrossbows);
                AddLine("Crossbows");
                AddSmallButton("ItemCrossbow02");
            },
            (h) =>
            {
                showCrossbows.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showGuns);
                AddLine("Guns");
                AddSmallButton("ItemRifle02");
            },
            (h) =>
            {
                showGuns.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
            });
        }),
        new("AuctionHouseFilteringArmorClass", () => {
            var thisWindow = CDesktop.LBWindow();
            SetAnchor(TopRight, -19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(233);
            AddHeaderRegion(() =>
            {
                AddLine("Auction categories:", "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    auctionCategory = "";
                    UpdateAuctionGroupList();
                    CloseWindow(h.window.title);
                    Respawn("AuctionHouseOffersGroups", true);
                    Respawn("AuctionHouseFilteringMain");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Armor class:", "Gray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showCloth);
                AddLine("Cloth");
                AddSmallButton("ItemMiscWartornScrapCloth");
            },
            (h) =>
            {
                showCloth.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showLeather);
                AddLine("Leather");
                AddSmallButton("ItemMiscWartornScrapLeather");
            },
            (h) =>
            {
                showLeather.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showMail);
                AddLine("Mail");
                AddSmallButton("ItemMiscWartornScrapChain");
            },
            (h) =>
            {
                showMail.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showPlate);
                AddLine("Plate");
                AddSmallButton("ItemMiscWartornScrapPlate");
            },
            (h) =>
            {
                showPlate.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
            });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Armor class", "", "Center"));
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Armor type", "", "Center"), (h) =>
            {
                CloseWindow("AuctionHouseFilteringArmorClass");
                Respawn("AuctionHouseFilteringArmorType");
            });
        }),
        new("AuctionHouseFilteringArmorType", () => {
            var thisWindow = CDesktop.LBWindow();
            SetAnchor(TopRight, -19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(233);
            AddHeaderRegion(() =>
            {
                AddLine("Auction categories:", "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    auctionCategory = "";
                    UpdateAuctionGroupList();
                    CloseWindow(h.window.title);
                    Respawn("AuctionHouseOffersGroups", true);
                    Respawn("AuctionHouseFilteringMain");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Armor type:", "Gray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showHead);
                AddLine("Head");
                AddSmallButton("ItemMiscDesecratedMailHelm");
            },
            (h) =>
            {
                showHead.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showShoulders);
                AddLine("Shoulders");
                AddSmallButton("ItemMiscDesecratedMailShoulder");
            },
            (h) =>
            {
                showShoulders.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showBack);
                AddLine("Back");
                AddSmallButton("ItemMiscDesecratedCape");
            },
            (h) =>
            {
                showBack.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showChest);
                AddLine("Chest");
                AddSmallButton("ItemMiscDesecratedMailChest");
            },
            (h) =>
            {
                showChest.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showWrists);
                AddLine("Wrists");
                AddSmallButton("ItemMiscDesecratedMailBracer");
            },
            (h) =>
            {
                showWrists.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showHands);
                AddLine("Hands");
                AddSmallButton("ItemMiscDesecratedMailGlove");
            },
            (h) =>
            {
                showHands.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showWaist);
                AddLine("Waist");
                AddSmallButton("ItemMiscDesecratedMailBelt");
            },
            (h) =>
            {
                showWaist.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showLegs);
                AddLine("Legs");
                AddSmallButton("ItemMiscDesecratedMailPants");
            },
            (h) =>
            {
                showLegs.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showFeet);
                AddLine("Feet");
                AddSmallButton("ItemMiscDesecratedMailBoots");
            },
            (h) =>
            {
                showFeet.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
            });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Armor class", "", "Center"), (h) =>
            {
                CloseWindow("AuctionHouseFilteringArmorType");
                Respawn("AuctionHouseFilteringArmorClass");
            });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Armor type", "", "Center"));
        }),
        new("AuctionHouseFilteringJewelry", () => {
            var thisWindow = CDesktop.LBWindow();
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(262);
            AddHeaderRegion(() =>
            {
                AddLine("Auction categories:", "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    auctionCategory = "";
                    UpdateAuctionGroupList();
                    CloseWindow(h.window.title);
                    Respawn("AuctionHouseOffersGroups", true);
                    Respawn("AuctionHouseFilteringMain");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine(auctionCategory + ":", "Gray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showNeck);
                AddLine("Neck");
                AddSmallButton("ItemJewelryNecklace15");
            },
            (h) =>
            {
                showNeck.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showFinger);
                AddLine("Finger");
                AddSmallButton("ItemJewelryRing19");
            },
            (h) =>
            {
                showFinger.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showTrinket);
                AddLine("Trinket");
                AddSmallButton("ItemBox03");
            },
            (h) =>
            {
                showTrinket.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
            });
        }),
        new("AuctionHouseFilteringConsumeables", () => {
            var thisWindow = CDesktop.LBWindow();
            SetAnchor(TopRight, -19, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(262);
            AddHeaderRegion(() =>
            {
                AddLine("Auction categories:", "Gray");
                AddSmallButton("OtherReverse", (h) =>
                {
                    auctionCategory = "";
                    UpdateAuctionGroupList();
                    CloseWindow(h.window.title);
                    Respawn("AuctionHouseOffersGroups", true);
                    Respawn("AuctionHouseFilteringMain");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine(auctionCategory + ":", "Gray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showFood);
                AddLine("Food");
                AddSmallButton("ItemMiscFood68");
            },
            (h) =>
            {
                showFood.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showScrolls);
                AddLine("Scrolls");
                AddSmallButton("ItemScroll07");
            },
            (h) =>
            {
                showScrolls.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showPotions);
                AddLine("Potions");
                AddSmallButton("ItemPotion33");
            },
            (h) =>
            {
                showPotions.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(showBattleElixirs);
                AddLine("Battle elixirs");
                AddSmallButton("ItemPotion51");
            },
            (h) =>
            {
                showBattleElixirs.Invert();
                UpdateAuctionGroupList();
                Respawn("AuctionHouseOffersGroups", true);
                for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            });
            AddPaddingRegion(() =>
            {
                SetRegionAsGroupExtender();
            });
        }),
        new("AuctionHouseOffers", () => {
            var rowAmount = 12;
            var thisWindow = CDesktop.LBWindow();
            var list = Market.exploredAuctions;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft, 19, -38);
            AddRegionGroup();
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
                    CloseWindow("AuctionHouseBuy");
                    CloseWindow("AuctionHouseChosenItem");
                    Respawn("AuctionHouseOffersGroups");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                    PlaySound("DesktopInstanceClose");
                });
            });
            AddHeaderRegion(() =>
            {
                AddLine("Available auctions:");
                AddLine("x" + Market.exploredAuctions.Sum(x => x.item.amount), "DarkGray", "Right");
            });
            auctionPriceToDisplay = new int[12];
            auctionAmountToDisplay = new int[12];
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                if (list.Count > index + thisWindow.pagination())
                {
                    var offer = Market.exploredAuctions[index + thisWindow.pagination()];
                    auctionPriceToDisplay[index] = offer.price;
                    auctionAmountToDisplay[index] = offer.item.amount;
                }
            }
            AddPaddingRegion(() => SetRegionAsGroupExtender());
            AddPaginationLine();
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
                Market.exploredAuctionsGroups = Market.exploredAuctionsGroups.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                CloseWindow("AuctionHouseOffersSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By minimum unit price", "Black"),
            (h) =>
            {
                Market.exploredAuctionsGroups = Market.exploredAuctionsGroups.OrderByDescending(x => x.Value.Min(y => y.price)).ToDictionary(x => x.Key, x => x.Value);
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
            //AddButtonRegion(() =>
            //{
            //    AddLine("Show sourced market", "Black");
            //    AddCheckbox(settings.sourcedMarket);
            //},
            //(h) =>
            //{
            //    settings.sourcedMarket.Invert();
            //    CDesktop.RespawnAll();
            //});
        }),
        new("AuctionHouseBuy", () => {
            SetAnchor(Bottom, 0, 35);
            AddHeaderGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Buyout amount:", "DarkGray");
                AddInputLine(String.splitAmount);
            });
            int planned = String.splitAmount.Value() == "" ? 0 : int.Parse(String.splitAmount.Value());
            int currentPrice = 0, emptied = 0;
            int toBuy = planned;
            while (toBuy > 0)
            {
                if (emptied == Market.exploredAuctions.Count)
                {
                    toBuy = 0;
                    String.splitAmount.Set("" + Market.exploredAuctions.Sum(x => x.item.amount));
                    inputLineMarker = String.splitAmount.Value().Length;
                    break;
                }
                var temp = Market.exploredAuctions[emptied++];
                for (int i = 0; toBuy > 0 && i < temp.item.amount; i++)
                    if (currentSave.player.inventory.money >= currentPrice + temp.price)
                    {
                        toBuy--;
                        currentPrice += temp.price;
                    }
                    else
                    {
                        String.splitAmount.Set(planned - toBuy + "");
                        toBuy = 0;
                        break;
                    }
            }
            AddButtonRegion(() => AddLine("Buy x" + (String.splitAmount.Value() == "" ? 0 : String.splitAmount.Value()), "Black"),
            (h) =>
            {
                int planned = String.splitAmount.Value() == "" ? 0 : int.Parse(String.splitAmount.Value());
                if (planned <= 0) return;
                var items = new List<Item>() { Market.exploredAuctions[0].item.CopyItem(1) }.Multilate(planned);
                if (!currentSave.player.inventory.CanAddItems(items))
                {
                    SpawnFallingText(new Vector2(0, 34), "Inventory is full", "Red");
                }
                else
                {
                    int currentPrice = 0, emptied = 0;
                    int toBuy = planned;
                    while (toBuy > 0)
                    {
                        var temp = Market.exploredAuctions[emptied++];
                        while (toBuy > 0 && temp.item.amount > 0)
                        {
                            toBuy--;
                            temp.item.amount--;
                            currentPrice += temp.price;
                        }
                    }
                    if (currentPrice > 0)
                        PlaySound("DesktopTransportPay");
                    foreach (var item in items)
                    {
                        currentSave.player.inventory.AddItem(item);
                        PlaySound(item.ItemSound("PutDown"), 0.8f);
                    }
                    currentSave.player.inventory.money -= currentPrice;
                    String.splitAmount.Set("1");
                    foreach (var market in currentSave.markets)
                        for (int i = market.auctions.Count - 1; i >= 0; i--)
                            if (market.auctions[i].item.amount <= 0)
                                market.auctions.RemoveAt(i);
                    Market.exploredAuctions.RemoveAll(x => x.item.amount == 0);
                    Market.exploredAuctionsGroups[Market.auctionGroup].RemoveAll(x => x.item.amount == 0);
                    if (Market.exploredAuctionsGroups[Market.auctionGroup].Count == 0)
                        Market.exploredAuctionsGroups.Remove(Market.auctionGroup);
                    if (Market.exploredAuctions.Count > 0)
                        Respawn("AuctionHouseOffers");
                    else
                    {
                        CloseWindow("AuctionHouseOffers");
                        CloseWindow("AuctionHouseBuy");
                        CloseWindow("AuctionHouseChosenItem");
                        Respawn("AuctionHouseOffersGroups");
                    }
                    CDesktop.RespawnAll();
                }
            });
            PrintPriceRegion(currentPrice, 38, 38, 49);
        }),
        new("ProfessionLevelTrainer", () => {
            var rowAmount = 6;
            var thisWindow = CDesktop.LBWindow();
            var type = personTypes.Find(x => x.type == person.type);
            var profession = professions.Find(x => x.name == type.profession);
            var levels = profession.levels.FindAll(x => x.requiredSkill <= type.skillCap).OrderBy(x => x.requiredSkill).ToList();
            if (currentSave.player.professionSkills.ContainsKey(profession.name))
                levels = levels.FindAll(x => !currentSave.player.professionSkills[profession.name].Item2.Contains(x.name));
            var list = levels;
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(288);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == area.faction).side : ""));
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
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                if (list.Count > index + thisWindow.pagination())
                    AddPaddingRegion(() =>
                    {
                        var key = levels[index + thisWindow.pagination()];
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
                                var key = levels[index + thisWindow.pagination()];

                                //If player is high enough level..
                                if (currentSave.player.level >= key.requiredLevel)
                                {
                                    //If player has the money..
                                    if (currentSave.player.inventory.money >= key.price)
                                    {
                                        //If has the profession and at a proper level..
                                        if (key.requiredSkill == 0 || currentSave.player.professionSkills.ContainsKey(type.profession) && currentSave.player.professionSkills[type.profession].Item1 >= key.requiredSkill)
                                        {
                                            //If doesnt have the level yet..
                                            if (!currentSave.player.professionSkills.ContainsKey(type.profession) || currentSave.player.professionSkills.ContainsKey(type.profession) && !currentSave.player.professionSkills[type.profession].Item2.Contains(key.name))
                                            {
                                                //Learn the level
                                                currentSave.player.inventory.money -= key.price;
                                                Respawn("PlayerMoney");
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
                                                SpawnFallingText(new Vector2(0, 34), "New skill learned", "Blue");
                                            }
                                        }
                                        else SpawnFallingText(new Vector2(0, 34), "Not enough skill in the profession", "Red");
                                    }
                                    else SpawnFallingText(new Vector2(0, 34), "Not enough money", "Red");
                                }
                                else SpawnFallingText(new Vector2(0, 34), "You don't meet the level requirements", "Red");
                            },
                            null,
                            (h) => () =>
                            {
                                var key = levels[index + thisWindow.pagination()];
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
                            SetBigButtonToRedscale();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                    });
                else
                    AddPaddingRegion(() =>
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled");
                    });
            }
            AddPaginationLine();
        }),
        new("ProfessionRecipeTrainer", () => {
            var rowAmount = 6;
            var thisWindow = CDesktop.LBWindow();
            var type = personTypes.Find(x => x.type == person.type);
            var profession = professions.Find(x => x.name == type.profession);
            var list = recipes.FindAll(x => x.profession == type.profession && (x.specialization == null || x.specialization == type.specialization) && x.price > 0 && (x.learnedAt <= type.skillCap || type.skillCap == 0));
            if (currentSave.player.learnedRecipes.ContainsKey(type.profession))
                list = list.FindAll(x => !currentSave.player.learnedRecipes[type.profession].Contains(x.name));
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopLeft, 19, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(288);
            AddHeaderRegion(() =>
            {
                AddLine(person.type + " ", "Gray");
                AddText(person.name);
                AddSmallButton(type.icon + (type.factionVariant ? factions.Find(x => x.name == area.faction).side : ""));
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
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                if (list.Count > index + thisWindow.pagination())
                    AddPaddingRegion(() =>
                    {
                        var key = list[index + thisWindow.pagination()];
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
                                var key = list[index + thisWindow.pagination()];

                                //If player has the profession..
                                if (currentSave.player.professionSkills.ContainsKey(key.profession))
                                {
                                    //If player has the profession at a proper level..
                                    if (currentSave.player.professionSkills[key.profession].Item1 >= key.learnedAt)
                                    {
                                        //If player has the money..
                                        if (currentSave.player.inventory.money >= key.price)
                                        {
                                            //If doesnt have the recipe..
                                            if (!currentSave.player.learnedRecipes.ContainsKey(type.profession) || currentSave.player.learnedRecipes.ContainsKey(type.profession) && !currentSave.player.learnedRecipes[type.profession].Contains(key.name))
                                            {
                                                //Add the recipe
                                                currentSave.player.inventory.money -= key.price;
                                                currentSave.player.LearnRecipe(key);
                                                Respawn("PlayerMoney");
                                                Respawn(h.window.title);
                                                PlaySound("DesktopSkillLearned");
                                                SpawnFallingText(new Vector2(0, 34), "New recipe learned", "Blue");
                                            }
                                            else SpawnFallingText(new Vector2(0, 34), "You already know this recipe", "Red");
                                        }
                                        else SpawnFallingText(new Vector2(0, 34), "Not enough money", "Red");
                                    }
                                    else SpawnFallingText(new Vector2(0, 34), "Not enough skill in the profession", "Red");
                                }
                                else SpawnFallingText(new Vector2(0, 34), "You don't know the required profession", "Red");
                            },
                            null,
                            (h) => () =>
                            {
                                var key = list[index + thisWindow.pagination()];
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
                            SetBigButtonToRedscale();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                    });
                else
                    AddPaddingRegion(() =>
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled");
                    });
            }
            AddPaginationLine();
        }),

        //Fishing
        new("FishingBoardFrame", () => {
            SetAnchor(-115, 146);
            AddRegionGroup();
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
                var zone = zones.Find(x => x.name == site.zone);
                AddBigButton("Zone" + zone.icon.Clean());
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
            AddHotkey("Open talents", () =>
            {
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                if (WindowUp("QuestList"))
                {
                    quest = null;
                    RemoveDesktopBackground();
                    CloseWindow("QuestList");
                    CloseWindow("QuestSettings");
                    CloseWindow("QuestSort");
                    CloseWindow("Quest");
                    CloseWindow("QuestAdd");
                    CloseWindow("QuestTurn");
                    CloseWindow("QuestConfirmAbandon");
                }
                if (CDesktop.title != "TalentScreen")
                {
                    PlaySound("DesktopTalentScreenOpen");
                    SpawnDesktopBlueprint("TalentScreen");
                }
                else
                {
                    CloseDesktop(CDesktop.title);
                    SwitchToMostImportantDesktop();
                    PlaySound("DesktopTalentScreenClose");
                }
            });
            AddHotkey("Open spellbook", () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                if (WindowUp("QuestList"))
                {
                    quest = null;
                    RemoveDesktopBackground();
                    CloseWindow("QuestList");
                    CloseWindow("QuestSettings");
                    CloseWindow("QuestSort");
                    CloseWindow("Quest");
                    CloseWindow("QuestAdd");
                    CloseWindow("QuestTurn");
                    CloseWindow("QuestConfirmAbandon");
                }
                if (CDesktop.title != "SpellbookScreen")
                    SpawnDesktopBlueprint("SpellbookScreen");
                else
                {
                    CloseDesktop(CDesktop.title);
                    SwitchToMostImportantDesktop();
                    PlaySound("DesktopSpellbookClose");
                }
            });
            AddHotkey("Open inventory", () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                if (WindowUp("QuestList"))
                {
                    quest = null;
                    RemoveDesktopBackground();
                    CloseWindow("QuestList");
                    CloseWindow("QuestSettings");
                    CloseWindow("QuestSort");
                    CloseWindow("Quest");
                    CloseWindow("QuestAdd");
                    CloseWindow("QuestTurn");
                    CloseWindow("QuestConfirmAbandon");
                }
                if (CDesktop.title != "ContainerLoot")
                {
                    if (CDesktop.title != "EquipmentScreen")
                        SpawnDesktopBlueprint("EquipmentScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        SwitchToMostImportantDesktop();
                        PlaySound("DesktopInventoryClose");
                    }
                }
            });
            AddHotkey("Open bestiary", () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                if (WindowUp("QuestList"))
                {
                    quest = null;
                    RemoveDesktopBackground();
                    CloseWindow("QuestList");
                    CloseWindow("QuestSettings");
                    CloseWindow("QuestSort");
                    CloseWindow("Quest");
                    CloseWindow("QuestAdd");
                    CloseWindow("QuestTurn");
                    CloseWindow("QuestConfirmAbandon");
                }
                if (CDesktop.title != "BestiaryScreen")
                    SpawnDesktopBlueprint("BestiaryScreen");
                else
                {
                    CloseDesktop(CDesktop.title);
                    SwitchToMostImportantDesktop();
                    PlaySound("DesktopInstanceClose");
                }
            });
            AddHotkey("Open professions", () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CharacterSheet");
                if (WindowUp("QuestList"))
                {
                    quest = null;
                    RemoveDesktopBackground();
                    CloseWindow("QuestList");
                    CloseWindow("QuestSettings");
                    CloseWindow("QuestSort");
                    CloseWindow("Quest");
                    CloseWindow("QuestAdd");
                    CloseWindow("QuestTurn");
                    CloseWindow("QuestConfirmAbandon");
                }
                if (CDesktop.title != "CraftingScreen")
                    SpawnDesktopBlueprint("CraftingScreen");
                else
                {
                    CloseDesktop(CDesktop.title);
                    SwitchToMostImportantDesktop();
                    PlaySound("DesktopSpellbookClose");
                }
            });
            AddHotkey("Open character sheet", () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                if (WindowUp("QuestList"))
                {
                    quest = null;
                    RemoveDesktopBackground();
                    CloseWindow("QuestList");
                    CloseWindow("QuestSettings");
                    CloseWindow("QuestSort");
                    CloseWindow("Quest");
                    CloseWindow("QuestAdd");
                    CloseWindow("QuestTurn");
                    CloseWindow("QuestConfirmAbandon");
                }
                if (CDesktop.title != "CharacterSheet")
                    SpawnDesktopBlueprint("CharacterSheet");
                else
                {
                    CloseDesktop(CDesktop.title);
                    SwitchToMostImportantDesktop();
                    PlaySound("DesktopCharacterSheetClose");
                }
            });
            AddHotkey("Open quest log", () =>
            {
                CloseDesktop("TalentScreen");
                CloseDesktop("SpellbookScreen");
                CloseDesktop("EquipmentScreen");
                CloseDesktop("BestiaryScreen");
                CloseDesktop("CraftingScreen");
                CloseDesktop("CharacterSheet");
                SwitchDesktop("Map");
                if (WindowUp("QuestList"))
                {
                    quest = null;
                    PlaySound("DesktopSpellbookClose");
                    RemoveDesktopBackground();
                    CloseWindow("QuestList");
                    CloseWindow("QuestSettings");
                    CloseWindow("QuestSort");
                    CloseWindow("Quest");
                    CloseWindow("QuestAdd");
                    CloseWindow("QuestTurn");
                    CloseWindow("QuestConfirmAbandon");
                    Respawn("WorldBuffs");
                    SwitchToMostImportantDesktop();
                    SwitchDesktop("CombatResults");
                }
                else
                {
                    SpawnTransition(true, 2, true);
                    PlaySound("DesktopInventoryOpen");
                    SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
                    Respawn("QuestList");
                    Respawn("MapToolbar");
                    CloseWindow("WorldBuffs");
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
                    if (WindowUp("QuestList"))
                    {
                        quest = null;
                        RemoveDesktopBackground();
                        CloseWindow("QuestList");
                        CloseWindow("QuestSettings");
                        CloseWindow("QuestSort");
                        CloseWindow("Quest");
                        CloseWindow("QuestAdd");
                        CloseWindow("QuestTurn");
                        CloseWindow("QuestConfirmAbandon");
                    }
                    if (CDesktop.title != "CharacterSheet")
                        SpawnDesktopBlueprint("CharacterSheet");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        SwitchToMostImportantDesktop();
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
                        if (WindowUp("QuestList"))
                        {
                            quest = null;
                            RemoveDesktopBackground();
                            CloseWindow("QuestList");
                            CloseWindow("QuestSettings");
                            CloseWindow("QuestSort");
                            CloseWindow("Quest");
                            CloseWindow("QuestAdd");
                            CloseWindow("QuestTurn");
                            CloseWindow("QuestConfirmAbandon");
                        }
                        if (desktops.All(x => x.title != "ContainerLoot"))
                            if (CDesktop.title != "EquipmentScreen")
                                SpawnDesktopBlueprint("EquipmentScreen");
                            else
                            {
                                CloseDesktop(CDesktop.title);
                                SwitchToMostImportantDesktop();
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
                    if (WindowUp("QuestList"))
                    {
                        quest = null;
                        RemoveDesktopBackground();
                        CloseWindow("QuestList");
                        CloseWindow("QuestSettings");
                        CloseWindow("QuestSort");
                        CloseWindow("Quest");
                        CloseWindow("QuestAdd");
                        CloseWindow("QuestTurn");
                        CloseWindow("QuestConfirmAbandon");
                    }
                    if (CDesktop.title != "SpellbookScreen")
                        SpawnDesktopBlueprint("SpellbookScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        SwitchToMostImportantDesktop();
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
                    if (WindowUp("QuestList"))
                    {
                        quest = null;
                        RemoveDesktopBackground();
                        CloseWindow("QuestList");
                        CloseWindow("QuestSettings");
                        CloseWindow("QuestSort");
                        CloseWindow("Quest");
                        CloseWindow("QuestAdd");
                        CloseWindow("QuestTurn");
                        CloseWindow("QuestConfirmAbandon");
                    }
                    if (CDesktop.title != "TalentScreen")
                    {
                        PlaySound("DesktopTalentScreenOpen");
                        SpawnDesktopBlueprint("TalentScreen");
                    }
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        SwitchToMostImportantDesktop();
                        PlaySound("DesktopTalentScreenClose");
                    }
                });
                AddSmallButton(WindowUp("QuestList") ? "OtherClose" : "MenuQuestLog", (h) =>
                {
                    CloseDesktop("TalentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("CraftingScreen");
                    CloseDesktop("CharacterSheet");
                    SwitchDesktop("Map");
                    if (WindowUp("QuestList"))
                    {
                        quest = null;
                        PlaySound("DesktopSpellbookClose");
                        RemoveDesktopBackground();
                        CloseWindow("QuestList");
                        CloseWindow("QuestSettings");
                        CloseWindow("QuestSort");
                        CloseWindow("Quest");
                        CloseWindow("QuestAdd");
                        CloseWindow("QuestTurn");
                        CloseWindow("QuestConfirmAbandon");
                        Respawn("WorldBuffs");
                        SwitchToMostImportantDesktop();
                    }
                    else
                    {
                        SpawnTransition(true, 2, true);
                        PlaySound("DesktopInventoryOpen");
                        SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
                        Respawn("QuestList");
                        CloseWindow("WorldBuffs");
                    }
                    Respawn("MapToolbar");
                });
                AddSmallButton(CDesktop.title == "CraftingScreen" ? "OtherClose" : "MenuCrafting", (h) =>
                {
                    CloseDesktop("TalentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("CharacterSheet");
                    if (WindowUp("QuestList"))
                    {
                        quest = null;
                        RemoveDesktopBackground();
                        CloseWindow("QuestList");
                        CloseWindow("QuestSettings");
                        CloseWindow("QuestSort");
                        CloseWindow("Quest");
                        CloseWindow("QuestAdd");
                        CloseWindow("QuestTurn");
                        CloseWindow("QuestConfirmAbandon");
                    }
                    if (CDesktop.title != "CraftingScreen")
                        SpawnDesktopBlueprint("CraftingScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        SwitchToMostImportantDesktop();
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
                    if (WindowUp("QuestList"))
                    {
                        quest = null;
                        RemoveDesktopBackground();
                        CloseWindow("QuestList");
                        CloseWindow("QuestSettings");
                        CloseWindow("QuestSort");
                        CloseWindow("Quest");
                        CloseWindow("QuestAdd");
                        CloseWindow("QuestTurn");
                        CloseWindow("QuestConfirmAbandon");
                    }
                    if (CDesktop.title != "BestiaryScreen")
                        SpawnDesktopBlueprint("BestiaryScreen");
                    else
                    {
                        CloseDesktop(CDesktop.title);
                        SwitchToMostImportantDesktop();
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
                ReverseButtons();
                AddLine("Level: ", "DarkGray");
                AddText(currentSave.player.level + "", "Gray");
                AddLine("Day " + (currentSave.day + 1), "", "Right");
                AddSmallButton("Portrait" + currentSave.player.race.Clean() + currentSave.player.gender + currentSave.player.portraitID);
                AddSmallButton("Class" + currentSave.player.spec);
            });
        }, true),
        new("MapToolbarClockRight", () => {
            SetAnchor(TopRight);
            DisableShadows();
            AddRegionGroup();
            SetRegionGroupWidth(252);
            AddPaddingRegion(() =>
            {
                AddLine(currentSave.hour + (currentSave.minute < 10 ? ":0" : ":") + currentSave.minute);
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
                //AddSmallButton("OtherSearch", (h) =>
                //{
                //    PlaySound("DesktopMenuOpen", 0.6f);
                //    SpawnDesktopBlueprint("GameMenu");
                //});
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
                AddLine("", "Gray", "Center");
                AddLine("Maladath", "Epic", "Center");
                AddLine("Chronicles", "Epic", "Center");
                AddLine("0.7.45", "DimGray", "Center");
                AddLine("", "Gray", "Center");
                AddLine("", "Gray", "Center");
                AddLine("", "Gray", "Center");
                AddLine("", "Gray", "Center");
                AddLine("", "Gray", "Center");
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
                SpawnWindowBlueprint("TitleScreenSingleplayer");
                var recentSave = saves.Count > 0 ? saves.OrderByDescending(x => x.lastLoaded).ToList()[0] : null;
                if (recentSave != null && !recentSave.player.dead)
                    SpawnWindowBlueprint("TitleScreenContinue");
                CloseWindow(h.window);
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
                AddLine("Keybinds", "", "Center");
            },
            (h) =>
            {
                SpawnWindowBlueprint("GameKeybinds");
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
                //PROGRAMMING and DESIGN:
                //Eerie

                //MUSIC, SOUNDS AND TEXTURES
                //Blizzard Entertainment

                //CONSULTATION AND ADVICE:
                //Ryved, FunkiMunki, LeKris, Tlaxcal
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
        new("TitleScreenSingleplayer", () => {
            SetAnchor(Top, 0, -19);
            AddRegionGroup();
            SetRegionGroupWidth(130);
            SetRegionGroupHeight(316);
            AddPaddingRegion(() =>
            {
                AddLine("", "Gray", "Center");
                AddLine("Maladath", "Epic", "Center");
                AddLine("Chronicles", "Epic", "Center");
                AddLine("0.7.45", "DimGray", "Center");
                AddLine("", "Gray", "Center");
                AddLine("", "Gray", "Center");
                AddLine("", "Gray", "Center");
                AddLine("", "Gray", "Center");
                AddLine("", "Gray", "Center");
            });
            AddHeaderRegion(() =>
            {
                AddLine("Singleplayer", "Gray", "Center");
            });
            var recentSave = saves.Count > 0 ? saves.OrderByDescending(x => x.lastLoaded).ToList()[0] : null;
            if (recentSave != null && !recentSave.player.dead)
                AddButtonRegion(() =>
                {
                    AddLine("Continue game", "", "Center");
                },
                (h) =>
                {
                    loadingBar = new GameObject[2];
                    selectedSave = recentSave;
                    Login();
                    SpawnDesktopBlueprint("Map");
                    CloseDesktop("LoginScreen");
                    CloseDesktop("TitleScreen");
                });
            else
                AddPaddingRegion(() =>
                {
                    AddLine("Continue game", "HalfGray", "Center");
                });
            if (saves.Count(x => !x.player.dead) < 25)
                AddButtonRegion(() =>
                {
                    AddLine("Start new game", "", "Center");
                },
                (h) =>
                {
                    creationRace = "";
                    creationSpec = "";
                    creationGender = "";
                    creationPortrait = -1;
                    creationPermadeath = new(false);
                    creationClassicItemStacking = new(false);
                    creationAutomaticallyDecidedBossLoot = new(false);
                    creationNoOpenWorldOppositeFactionEncounters = new(false);
                    String.creationName.Set("");
                    SetDesktopBackground("Backgrounds/Sky");
                    SpawnWindowBlueprint("CharacterCreationFactionHorde");
                    SpawnWindowBlueprint("CharacterCreationFactionHorde");
                    SpawnWindowBlueprint("CharacterCreationFactionAlliance");
                    SpawnWindowBlueprint("TitleScreenNewSaveBack");
                    SpawnWindowBlueprint("TitleScreenNewSaveFinish");
                    CloseWindow("TitleScreenContinue");
                    CloseWindow(h.window);
                });
            else
                AddPaddingRegion(() =>
                {
                    AddLine("Start new game", "HalfGray", "Center");
                });
            if (saves.Count(x => !x.player.dead) > 0)
                AddButtonRegion(() =>
                {
                    AddLine("Load game", "", "Center");
                },
                (h) =>
                {
                    CloseWindow("TitleScreenContinue");
                    CloseWindow(h.window);
                    selectedSave = saves.First(x => !x.player.dead);
                    var site = FindSite(x => x.name == selectedSave.currentSite);
                    if (site != null) SetDesktopBackground(site.Background());
                    else SetDesktopBackground("Backgrounds/Sky");
                    SpawnWindowBlueprint("TitleScreenLoadGameBack");
                    SpawnWindowBlueprint("TitleScreenLoadGameConfirm");
                    SpawnWindowBlueprint("TitleScreenLoadGameCharacterHeader");
                    SpawnWindowBlueprint("TitleScreenLoadGameCharacterInfo");
                    SpawnWindowBlueprint("TitleScreenLoadGameScroller");
                });
            else
                AddPaddingRegion(() =>
                {
                    AddLine("Load game", "HalfGray", "Center");
                });
            AddButtonRegion(() =>
            {
                AddLine("Quick battle", "", "Center");
            },
            (h) =>
            {

            });
            AddButtonRegion(() =>
            {
                AddLine("Back", "", "Center");
            },
            (h) =>
            {
                SpawnWindowBlueprint("TitleScreenMenu");
                CloseWindow("TitleScreenContinue");
                CloseWindow(h.window);
            });
            AddPaddingRegion(() => { });
            var maladathIcon = new GameObject("MaladathIcon", typeof(SpriteRenderer));
            maladathIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/MaladathIcon");
            maladathIcon.GetComponent<SpriteRenderer>().sortingLayerName = "Upper";
            maladathIcon.GetComponent<SpriteRenderer>().sortingOrder = 1;
            maladathIcon.transform.parent = CDesktop.LBWindow().transform;
            maladathIcon.transform.localPosition = new Vector3(69, -145);
        }, true),
        new("TitleScreenContinue", () => {
            SetAnchor(Top, 149, -168);
            AddRegionGroup();
            SetRegionGroupWidth(130);
            var recentSave = saves.Count > 0 ? saves.OrderByDescending(x => x.lastLoaded).ToList()[0] : null;
            if (recentSave != null && !recentSave.player.dead)
            AddHeaderRegion(() =>
            {
                ReverseButtons();
                AddLine(recentSave.player.name);
                AddSmallButton("Portrait" + recentSave.player.race.Clean() + recentSave.player.gender + recentSave.player.portraitID);
            });
            AddHeaderRegion(() =>
            {
                ReverseButtons();
                AddLine("Level: ", "DarkGray");
                AddText(recentSave.player.level + "", "Gray");
                AddSmallButton("Class" + recentSave.player.spec);
            });
            var continueNotch = new GameObject("ContinueNotch", typeof(SpriteRenderer));
            continueNotch.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/ContinueNotch");
            continueNotch.GetComponent<SpriteRenderer>().sortingLayerName = "Upper";
            continueNotch.GetComponent<SpriteRenderer>().sortingOrder = 1;
            continueNotch.transform.parent = CDesktop.LBWindow().transform;
            continueNotch.transform.localPosition = new Vector3(-7.5f, -21.5f);
        }, true),
        new("TitleScreenNewSaveBack", () => {
            SetAnchor(BottomLeft, 19, 19);
            AddRegionGroup();
            SetRegionGroupWidth(119);
            AddButtonRegion(() =>
            {
                AddLine("Back", "", "Center");
            },
            (h) =>
            {
                if (!WindowUp("TitleScreenNewSaveGameRules"))
                {
                    SpawnTransition();
                    RemoveDesktopBackground();
                    CloseWindow("CharacterCreationSpec");
                    CloseWindow("CharacterCreationMasculinePortraits");
                    CloseWindow("CharacterCreationFemininePortraits");
                    CloseWindow("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFactionAlliance");
                    CloseWindow("TitleScreenNewSaveBack");
                    CloseWindow("TitleScreenNewSaveFinish");
                    SpawnWindowBlueprint("TitleScreenSingleplayer");
                    var recentSave = saves.Count > 0 ? saves.OrderByDescending(x => x.lastLoaded).ToList()[0] : null;
                    if (recentSave != null && !recentSave.player.dead)
                        SpawnWindowBlueprint("TitleScreenContinue");
                }
                else
                {
                    SpawnTransition();
                    CloseWindow("TitleScreenNewSaveGameRules");
                    CloseWindow("TitleScreenNewSaveSummary");
                    Respawn("CharacterCreationSpec");
                    Respawn("CharacterCreationMasculinePortraits");
                    Respawn("CharacterCreationFemininePortraits");
                    Respawn("CharacterCreationFactionHorde");
                    Respawn("CharacterCreationFactionAlliance");
                    Respawn("TitleScreenNewSaveFinish");
                }
            });
        }, true),
        new("TitleScreenNewSaveSummary", () => {
            SetAnchor(Bottom, 0, 114);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddBigButton(specs.Find(x => x.name == creationSpec).icon);
            });
            AddRegionGroup();
            SetRegionGroupWidth(172);
            AddHeaderRegion(() =>
            {
                AddLine("Character Name:", "HalfGray", "Center");
            });
            AddPaddingRegion(() =>
            {
                AddInputLine(String.creationName, "", "Center");
            });
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddBigButton("Portrait" + creationRace.Clean() + creationGender + creationPortrait);
            });
        }, true),
        new("TitleScreenNewSaveGameRules", () => {
            SetAnchor(Bottom, 0, 19);
            AddRegionGroup();
            SetRegionGroupWidth(324);
            AddHeaderRegion(() =>
            {
                AddLine("Game Rules:", "DarkGray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(creationPermadeath);
                AddLine("Permadeath");
            },
            (h) =>
            {
                creationPermadeath.Invert();
                CDesktop.RespawnAll();
            },
            null, (h) => () =>
            {
                SetAnchor(BottomLeft, 119, 173);
                AddRegionGroup();
                SetRegionGroupWidth(400);
                AddHeaderRegion(() => AddLine("Permadeath", "", "Center"));
                new Description()
                { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                    {
                        { "Color", "DarkGray" },
                        { "Align", "Center" },
                        { "Text", "When turned on, this setting makes your character die permanently the first time it dies. This means that you will no longer be able to play on that character and it will be visible only in the rankings menu. This setting is only for seasoned players and those who are not afraid of losing a lot of progress. Choose at your own discretion." }
                    }
                } } } }.Print(null, 390, null);
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(creationClassicItemStacking);
                AddLine("Classic item stacking");
            },
            (h) =>
            {
                creationClassicItemStacking.Invert();
                CDesktop.RespawnAll();
            },
            null, (h) => () =>
            {
                SetAnchor(BottomLeft, 119, 173);
                AddRegionGroup();
                SetRegionGroupWidth(400);
                AddHeaderRegion(() => AddLine("Classic item stacking", "", "Center"));
                new Description()
                { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                    {
                        { "Color", "DarkGray" },
                        { "Align", "Center" },
                        { "Text", "When turned on, this setting makes items stack with classic stack sizes from World of Warcraft. With the setting disabled same item types will stack up to 999 in a single slot." }
                    }
                } } } }.Print(null, 390, null);
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(creationAutomaticallyDecidedBossLoot);
                AddLine("Automatically decided boss loot");
            },
            (h) =>
            {
                creationAutomaticallyDecidedBossLoot.Invert();
                CDesktop.RespawnAll();
            },
            null, (h) => () =>
            {
                SetAnchor(BottomLeft, 119, 173);
                AddRegionGroup();
                SetRegionGroupWidth(400);
                AddHeaderRegion(() => AddLine("Automatically decided boss loot", "", "Center"));
                new Description()
                { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                    {
                        { "Color", "DarkGray" },
                        { "Align", "Center" },
                        { "Text", "When turned on, this setting makes boss loot automatically decided for the player, by choosing a random item from the possible item pool. When this setting is turned off, elites having more than one item in their pool will drop a pouch instead, from which the player will be able to choose one item." }
                    }
                } } } }.Print(null, 390, null);
            });
            //AddButtonRegion(() =>
            //{
            //    AddCheckbox(creationNoOpenWorldOppositeFactionEncounters);
            //    AddLine("No open world opposite faction encounters");
            //},
            //(h) =>
            //{
            //    creationNoOpenWorldOppositeFactionEncounters.Invert();
            //    CDesktop.RespawnAll();
            //});
        }, true),
        new("TitleScreenNewSaveFinish", () => {
            SetAnchor(BottomRight, -19, 19);
            AddRegionGroup();
            SetRegionGroupWidth(119);
            if (WindowUp("TitleScreenNewSaveGameRules"))
            {
                if (String.creationName.Value().Length >= 3 && String.creationName.Value().Length < 13)
                {
                    AddButtonRegion(() =>
                    {
                        AddLine("Finish", "", "Center");
                    },
                    (h) =>
                    {
                        PlaySound("DesktopCreateCharacter");
                        AddNewSave();
                        SpawnTransition();
                        SaveGames();
                        CloseWindow("TitleScreenNewSaveBack");
                        CloseWindow("TitleScreenNewSaveFinish");
                        CloseWindow("TitleScreenNewSaveSummary");
                        CloseWindow("TitleScreenNewSaveGameRules");
                        selectedSave = saves.Last(x => !x.player.dead);
                        var site = FindSite(x => x.name == selectedSave.currentSite);
                        if (site != null) SetDesktopBackground(site.Background());
                        else SetDesktopBackground("Backgrounds/Sky");
                        SpawnWindowBlueprint("TitleScreenLoadGameBack");
                        SpawnWindowBlueprint("TitleScreenLoadGameConfirm");
                        SpawnWindowBlueprint("TitleScreenLoadGameCharacterHeader");
                        SpawnWindowBlueprint("TitleScreenLoadGameCharacterInfo");
                        SpawnWindowBlueprint("TitleScreenLoadGameScroller");
                    });
                }
                else
                    AddHeaderRegion(() =>
                    {
                        AddLine("Finish", "DarkGray", "Center");
                    });
            }
            else if (creationSpec != "" && creationGender != "" && creationRace != "")
                AddButtonRegion(() =>
                {
                    AddLine("Next", "", "Center");
                },
                (h) =>
                {
                    SpawnTransition();
                    CloseWindow("CharacterCreationSpec");
                    CloseWindow("CharacterCreationFactionHorde");
                    CloseWindow("CharacterCreationFactionAlliance");
                    CloseWindow("CharacterCreationMasculinePortraits");
                    CloseWindow("CharacterCreationFemininePortraits");
                    Respawn("TitleScreenNewSaveBack");
                    Respawn("TitleScreenNewSaveSummary");
                    Respawn("TitleScreenNewSaveGameRules");
                });
            else
                AddHeaderRegion(() =>
                {
                    AddLine("Next", "DarkGray", "Center");
                });
        }, true),
        new("TitleScreenLoadGameCharacterHeader", () => {
            SetAnchor(Bottom, 0, 114);
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddBigButton(specs.Find(x => x.name == selectedSave.player.spec).icon);
            });
            AddRegionGroup();
            SetRegionGroupWidth(172);
            AddHeaderRegion(() =>
            {
                AddLine(selectedSave.player.name, "", "Center");
            });
            AddPaddingRegion(() =>
            {
                //AddLine(selectedSave.player.spec, selectedSave.player.spec + " ", "Center");
                AddLine("Level: ", "DarkGray", "Center");
                AddText(selectedSave.player.level + "", "Gray");
            });
            //AddPaddingRegion(() =>
            //{
            //    AddLine("Time played: ", "DarkGray", "Center");
            //    AddText((selectedSave.timePlayed.Hours > 0 ? selectedSave.timePlayed.Hours + "h " : "") + selectedSave.timePlayed.Minutes + "m");
            //});
            AddRegionGroup();
            AddPaddingRegion(() =>
            {
                AddBigButton("Portrait" + selectedSave.player.race.Clean() + selectedSave.player.gender + selectedSave.player.portraitID);
            });
        }, true),
        new("TitleScreenLoadGameCharacterInfo", () => {
            SetAnchor(Bottom, 0, 19);
            AddRegionGroup();
            SetRegionGroupWidth(324);
            //AddPaddingRegion(() =>
            //{
            //    AddLine("Elites killed: ", "DarkGray");
            //    AddText(slot.elitesKilled.Sum(x => x.Value) + "");
            //    AddLine("Rares killed: ", "DarkGray");
            //    AddText(slot.raresKilled.Sum(x => x.Value) + "");
            //});
            AddHeaderRegion(() =>
            {
                AddLine("Game Rules:", "DarkGray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(selectedSave.permadeath);
                AddLine("Permadeath");
            },
            null,
            null, (h) => () =>
            {
                SetAnchor(BottomLeft, 119, 173);
                AddRegionGroup();
                SetRegionGroupWidth(400);
                AddHeaderRegion(() => AddLine("Permadeath", "", "Center"));
                new Description()
                { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                    {
                        { "Color", "DarkGray" },
                        { "Align", "Center" },
                        { "Text", "When turned on, this setting makes your character die permanently the first time it dies. This means that you will no longer be able to play on that character and it will be visible only in the rankings menu. This setting is only for seasoned players and those who are not afraid of losing a lot of progress. Choose at your own discretion." }
                    }
                } } } }.Print(null, 390, null);
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(selectedSave.classicItemStacking);
                AddLine("Classic item stacking");
            },
            null,
            null, (h) => () =>
            {
                SetAnchor(BottomLeft, 119, 173);
                AddRegionGroup();
                SetRegionGroupWidth(400);
                AddHeaderRegion(() => AddLine("Classic item stacking", "", "Center"));
                new Description()
                { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                    {
                        { "Color", "DarkGray" },
                        { "Align", "Center" },
                        { "Text", "When turned on, this setting makes items stack with classic stack sizes from World of Warcraft. With the setting disabled same item types will stack up to 999 in a single slot." }
                    }
                } } } }.Print(null, 390, null);
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(selectedSave.automaticallyDecidedBossLoot);
                AddLine("Automatically decided boss loot");
            },
            null,
            null, (h) => () =>
            {
                SetAnchor(BottomLeft, 119, 173);
                AddRegionGroup();
                SetRegionGroupWidth(400);
                AddHeaderRegion(() => AddLine("Automatically decided boss loot", "", "Center"));
                new Description()
                { regions = new() { new() { regionType = "Padding", contents = new() { new ()
                    {
                        { "Color", "DarkGray" },
                        { "Align", "Center" },
                        { "Text", "When turned on, this setting makes boss loot automatically decided for the player, by choosing a random item from the possible item pool. When this setting is turned off, elites having more than one item in their pool will drop a pouch instead, from which the player will be able to choose one item." }
                    }
                } } } }.Print(null, 390, null);
            });
        }, true),
        new("TitleScreenLoadGameBack", () => {
            SetAnchor(BottomLeft, 19, 19);
            AddRegionGroup();
            SetRegionGroupWidth(119);
            AddButtonRegion(() =>
            {
                AddLine("Back", "", "Center");
            },
            (h) =>
            {
                SpawnTransition();
                RemoveDesktopBackground();
                CloseWindow("TitleScreenLoadGameCharacterHeader");
                CloseWindow("TitleScreenLoadGameCharacterInfo");
                CloseWindow("TitleScreenLoadGameBack");
                CloseWindow("TitleScreenLoadGameConfirm");
                CloseWindow("TitleScreenLoadGameScroller");
                SpawnWindowBlueprint("TitleScreenSingleplayer");
                var recentSave = saves.Count > 0 ? saves.OrderByDescending(x => x.lastLoaded).ToList()[0] : null;
                if (recentSave != null && !recentSave.player.dead)
                    SpawnWindowBlueprint("TitleScreenContinue");
            });
        }, true),
        new("TitleScreenLoadGameConfirm", () => {
            SetAnchor(BottomRight, -19, 19);
            AddRegionGroup();
            SetRegionGroupWidth(119);
            AddButtonRegion(() =>
            {
                AddLine("Delete character", "", "Center");
            },
            (h) =>
            {
                String.promptConfirm.Set("");
                PlaySound("DesktopMenuOpen", 0.6f);
                SpawnWindowBlueprint("ConfirmDeleteCharacter");
                CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
            });
            AddButtonRegion(() =>
            {
                AddLine("Load game", "", "Center");
            },
            (h) =>
            {
                loadingBar = new GameObject[2];
                Login();
                SpawnDesktopBlueprint("Map");
                CloseDesktop("TitleScreen");
            });
        }, true),
        new("TitleScreenLoadGameScroller", () => {
            SetAnchor(Top, 0, -19);
            AddRegionGroup();
            AddHeaderRegion(() =>
            {
                ReverseButtons();
                AddSmallButton("OtherPreviousPage", (h) =>
                {
                    if (saves.Count(x => !x.player.dead) <= 1) return;
                    PlaySound("DesktopSwitchPage");
                    var saves2 = saves.Take(saves.IndexOf(selectedSave));
                    if (saves2.Count(x => !x.player.dead) == 0)
                        selectedSave = saves.Last(x => !x.player.dead);
                    else selectedSave = saves2.Last(x => !x.player.dead);
                    var site = FindSite(x => x.name == selectedSave.currentSite);
                    if (site != null) SetDesktopBackground(site.Background());
                    else SetDesktopBackground("Backgrounds/Sky");
                    Respawn("TitleScreenLoadGameCharacterHeader");
                    Respawn("TitleScreenLoadGameCharacterInfo");
                    SpawnTransition();
                });
                foreach (var save in saves.Where(x => !x.player.dead))
                {
                    AddSmallButton("Portrait" + save.player.race.Clean() + save.player.gender + save.player.portraitID, (h) =>
                    {
                        if (selectedSave != save)
                        {
                            selectedSave = save;
                            var site = FindSite(x => x.name == selectedSave.currentSite);
                            if (site != null) SetDesktopBackground(site.Background());
                            else SetDesktopBackground("Backgrounds/Sky");
                            Respawn("TitleScreenLoadGameCharacterHeader");
                            Respawn("TitleScreenLoadGameCharacterInfo");
                            SpawnTransition();
                        }
                    });
                    if (selectedSave != save)
                    {
                        SetSmallButtonToGrayscale();
                        AddSmallButtonOverlay("OtherGridBlurred");
                    }
                }
                AddSmallButton("OtherNextPage", (h) =>
                {
                    if (saves.Count(x => !x.player.dead) <= 1) return;
                    PlaySound("DesktopSwitchPage");
                    var saves2 = saves.Skip(saves.IndexOf(selectedSave) + 1);
                    if (saves2.Count(x => !x.player.dead) == 0)
                        selectedSave = saves.First(x => !x.player.dead);
                    else selectedSave = saves2.First(x => !x.player.dead);
                    var site = FindSite(x => x.name == selectedSave.currentSite);
                    if (site != null) SetDesktopBackground(site.Background());
                    else SetDesktopBackground("Backgrounds/Sky");
                    Respawn("TitleScreenLoadGameCharacterHeader");
                    Respawn("TitleScreenLoadGameCharacterInfo");
                    SpawnTransition();
                });
            });
        }),
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
                        Respawn("PlayerWeaponsInfo");
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
            AddPaddingRegion(() => AddLine("Keybinds", "DarkGray"));
            if (CanSave())
            {
                AddButtonRegion(() => AddLine("Save and return to main menu", "Black"),
                (h) =>
                {
                    CloseSave();
                    SaveGames();
                    CloseDesktop("Area");
                    CloseDesktop("Instance");
                    CloseDesktop("Complex");
                    CloseDesktop("FishingGame");
                    CloseDesktop("CharacterSheet");
                    CloseDesktop("TalentScreen");
                    CloseDesktop("SpellbookScreen");
                    CloseDesktop("EquipmentScreen");
                    CloseDesktop("BestiaryScreen");
                    CloseDesktop("CraftingScreen");
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
        new("TitleScreenFirstLaunch", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine("First time configuration:", "Gray");
                AddSmallButton("OtherClose", (h) =>
                {
                    Serialization.Serialize(settings, "settings", false, false, Serialization.prefix);
                    CloseWindow(h.window);
                    Respawn("TitleScreenMenu");
                });
            });
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddPaddingRegion(() =>
            {
                AddLine("Visuals:", "HalfGray");
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
                AddLine("Sound:", "HalfGray");
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
                AddCheckbox(settings.ambience);
                AddLine("Ambience");
            },
            (h) =>
            {
                settings.ambience.Invert();
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
            AddPaddingRegion(() =>
            {
                AddLine("Other:", "HalfGray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(settings.runsInBackground);
                AddLine("Runs in background");
            },
            (h) =>
            {
                settings.runsInBackground.Invert();
                Application.runInBackground = settings.runsInBackground.Value();
                CDesktop.RespawnAll();
            });
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
                AddLine("Gameplay:", "HalfGray");
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
                AddLine("Visuals:", "HalfGray");
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
                AddLine("Sound:", "HalfGray");
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
                AddCheckbox(settings.ambience);
                AddLine("Ambience");
            },
            (h) =>
            {
                settings.ambience.Invert();
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
            AddPaddingRegion(() =>
            {
                AddLine("Other:", "HalfGray");
            });
            AddButtonRegion(() =>
            {
                AddCheckbox(settings.runsInBackground);
                AddLine("Runs in background");
            },
            (h) =>
            {
                settings.runsInBackground.Invert();
                Application.runInBackground = settings.runsInBackground.Value();
                CDesktop.RespawnAll();
            });
        }, true),
        new("GameKeybinds", () => {
            SetAnchor(Center);
            AddHeaderGroup();
            SetRegionGroupWidth(570);
            AddHeaderRegion(() =>
            {
                AddLine("Keybinds:", "Gray");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow(h.window);
                    Respawn(CDesktop.title == "GameMenu" ? "GameMenu" : "TitleScreenMenu");
                });
            });
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddPaddingRegion(() => AddLine("General:", "HalfGray"));
            BindLine("Open menu / Back");
            BindLine("Open console");
            AddPaddingRegion(() => { });
            AddRegionGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(281);
            AddPaddingRegion(() => AddLine("Camera:", "HalfGray"));
            BindLine("Move camera north");
            BindLine("Move camera west");
            BindLine("Move camera south");
            BindLine("Move camera east");
            BindLine("Focus camera on player");
            AddPaddingRegion(() => { });
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddPaddingRegion(() => AddLine("Screen change:", "HalfGray"));
            BindLine("Open talents");
            BindLine("Open inventory");
            BindLine("Open spellbook");
            BindLine("Open quest log");
            BindLine("Open professions");
            BindLine("Open character sheet");
            BindLine("Open bestiary");

            void BindLine(string bind)
            {
                AddButtonRegion(() =>
                {
                    AddLine(bind);
                    AddSmallButton("OtherReverse", (h) => Keybinds.keybinds[bind].key = Keybinds.defaultKeybinds[bind].key);
                },
                (h) =>
                {
                    newBindFor = bind;
                    awaitingNewBind = true;
                    CDesktop.LockScreen();
                });
                AddHeaderRegion(() => AddLine(Keybinds.keybinds[bind].key.ToString()));
            }
        }, true),
        new("MapLocationInfo", () => {
            if (currentSave.currentSite != "") return;
            if (currentSave.currentSite == "") return;
            SetAnchor(BottomRight, 0, 16);
            AddRegionGroup();
            AddPaddingRegion(() => AddSmallButton("MenuFlag", (h) => FindSite(x => x.name == currentSave.currentSite).ExecutePath()));
        }, true),

        //Spellbook
        new("SpellbookAbilityListActivated", () => {
            var rowAmount = 6;
            var thisWindow = CDesktop.LBWindow();
            var activeAbilities = abilities.FindAll(x => x.icon != null && !x.hide && x.events.Any(y => y.triggers.Any(z => z["Trigger"] == "AbilityCast")) && x.cost != null && currentSave.player.abilities.ContainsKey(x.name)).Where(x => !settings.hideSetInvalidAbilities.Value() || ((x.allowedSets != null && x.allowedSets.Contains(currentSave.player.currentActionSet)) || (x.allowedSets == null && currentSave.player.currentActionSet == "Default"))).ToDictionary(x => x, x => currentSave.player.abilities[x.name]);
            var list = activeAbilities.ToList();
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopRight, 0, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(269);
            AddHeaderRegion(() =>
            {
                AddLine("Active abilities:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    abilities.Reverse();
                    Respawn("SpellbookAbilityListActivated");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!WindowUp("AbilitiesSort") && !WindowUp("SwitchActionSet") && !WindowUp("SpellbookSettings"))
                    AddSmallButton("OtherSort", (h) =>
                    {
                        Respawn("AbilitiesSort");
                        CloseWindow("PlayerSpellbookInfo");
                        Respawn("PlayerSpellbookInfo");
                    });
                else
                    AddSmallButton("OtherSortOff");
                if (!WindowUp("AbilitiesSort") && !WindowUp("SwitchActionSet") && !WindowUp("SpellbookSettings"))
                    AddSmallButton("OtherSettings", (h) =>
                    {
                        SpawnWindowBlueprint("SpellbookSettings");
                        CloseWindow("PlayerSpellbookInfo");
                        Respawn("PlayerSpellbookInfo");
                    });
                else
                    AddSmallButton("OtherSettingsOff");
            });
            var regionGroup = CDesktop.LBWindow().LBRegionGroup();
            AddPaginationLine();
            var actionSet = currentSave.player.actionSets[currentSave.player.currentActionSet];
            AddRegionGroup();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                if (list.Count > index + thisWindow.pagination())
                    AddPaddingRegion(() =>
                    {
                        var key = activeAbilities.ToList()[index + thisWindow.pagination()];
                        AddBigButton(key.Key.icon,
                            (h) =>
                            {
                                var key = activeAbilities.ToList()[index + thisWindow.pagination()];
                                var cur = ActionSet.actionSets.Find(x => x.name == currentSave.player.currentActionSet);
                                if (key.Key.allowedSets == null && currentSave.player.currentActionSet != "Default" || key.Key.allowedSets != null && !key.Key.allowedSets.Contains(currentSave.player.currentActionSet))
                                {
                                    SpawnFallingText(new Vector2(0, 34), "This ability cannot be in this action set", "Red");
                                    PlaySound("DesktopCantClick");
                                }
                                else if (currentSave.player.actionSets[currentSave.player.currentActionSet].Contains(key.Key.name))
                                {
                                    SpawnFallingText(new Vector2(0, 34), "This ability is already in this action set", "Red");
                                    PlaySound("DesktopCantClick");
                                }
                                else if (actionSet.Count >= currentSave.player.ActionSetMaxLength())
                                {
                                    SpawnFallingText(new Vector2(0, 34), "Current action set is already full", "Red");
                                    PlaySound("DesktopCantClick");
                                }
                                else
                                {
                                    actionSet.Add(key.Key.name);
                                    Respawn("PlayerSpellbookInfo");
                                    Respawn("SpellbookAbilityListActivated", true);
                                    PlaySound("DesktopActionBarAdd", 0.9f);
                                }
                            },
                            null,
                            (h) => () =>
                            {
                                var key = activeAbilities.ToList()[index + thisWindow.pagination()].Key;
                                currentSave.player.ResetResources();
                                foreach (var res in key.cost)
                                    if (res.Value < currentSave.player.MaxResource(res.Key))
                                        currentSave.player.resources[res.Key] = res.Value;
                                    else currentSave.player.resources[res.Key] = currentSave.player.MaxResource(res.Key);
                                spellbookResourceBars.ToList().ForEach(x => x.Value.UpdateFluidBar());
                                currentSave.player.ResetResources();
                                SetAnchor(Center);
                                PrintAbilityTooltip(currentSave.player, key, activeAbilities[key]);
                            }
                        );
                        if (actionSet.Contains(key.Key.name))
                        {
                            SetBigButtonToGrayscale();
                            AddBigButtonOverlay("OtherGridBlurred");
                            AddBigButtonOverlay("OtherAvailable");
                        }
                        else if (key.Key.allowedSets == null && currentSave.player.currentActionSet != "Default" || key.Key.allowedSets != null && !key.Key.allowedSets.Contains(currentSave.player.currentActionSet))
                        {
                            SetBigButtonToRedscale();
                            AddBigButtonOverlay("OtherGridBlurred");
                        }
                        //else if (actionSet.Count < currentSave.player.ActionSetMaxLength())
                        //    AddBigButtonOverlay("OtherGlowLearnable");
                    });
                else AddPaddingRegion(() => AddBigButton("OtherDisabled"));
            }
            AddRegionGroup();
            SetRegionGroupWidth(152);
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                if (list.Count > index + thisWindow.pagination())
                {
                    var key = activeAbilities.ToList()[index + thisWindow.pagination()];
                    AddHeaderRegion(() =>
                    {
                        AddLine(key.Key.name);
                        AddText(" " + (key.Value + 1).ToRoman());
                        if (settings.showSpellbookValidActionSets.Value())
                        {
                            if (key.Key.allowedSets != null)
                                foreach (var set in key.Key.allowedSets)
                                    if (set == "Default") AddSmallButton("AbilityDefault");
                                    else
                                    {
                                        var actionSet = ActionSet.actionSets.Find(x => x.name == set);
                                        if (actionSet.alwaysVisible || currentSave.player.abilities.ContainsKey(actionSet.abilityForVisibility))
                                            AddSmallButton(actionSet.icon);
                                    }
                            else AddSmallButton("AbilityDefault");
                        }
                    });
                    AddPaddingRegion(() =>
                    {
                        int counter = 8;
                        if (!settings.showSpellbookResourceIcons.Value() || key.Key.cost == null) AddLine();
                        else
                        {
                            ReverseButtons();
                            if (key.Key.cost.ContainsKey("Fire") && counter-- > 0) AddSmallButton("ElementFireRousing");
                            if (key.Key.cost.ContainsKey("Water") && counter-- > 0) AddSmallButton("ElementWaterRousing");
                            if (key.Key.cost.ContainsKey("Earth") && counter-- > 0) AddSmallButton("ElementEarthRousing");
                            if (key.Key.cost.ContainsKey("Air") && counter-- > 0) AddSmallButton("ElementAirRousing");
                            if (key.Key.cost.ContainsKey("Frost") && counter-- > 0) AddSmallButton("ElementFrostRousing");
                            if (key.Key.cost.ContainsKey("Lightning") && counter-- > 0) AddSmallButton("ElementLightningRousing");
                            if (key.Key.cost.ContainsKey("Arcane") && counter-- > 0) AddSmallButton("ElementArcaneRousing");
                            if (key.Key.cost.ContainsKey("Decay") && counter-- > 0) AddSmallButton("ElementDecayRousing");
                            if (key.Key.cost.ContainsKey("Order") && counter-- > 0) AddSmallButton("ElementOrderRousing");
                            if (key.Key.cost.ContainsKey("Shadow") && counter-- > 0) AddSmallButton("ElementShadowRousing");
                            if (counter == 8) AddLine();
                        }
                    });
                }
                else
                {
                    AddHeaderRegion(() => AddLine());
                    AddPaddingRegion(() => AddLine());
                }
            }
        }),
        new("SpellbookAbilityListActivatedBottom", () => {
            SetAnchor(BottomRight, 0, 35);
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Activated", "", "Center"));
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Passive", "", "Center"), (h) =>
            {
                CloseWindow("SpellbookAbilityListActivated");
                CloseWindow("SpellbookAbilityListActivatedBottom");
                Respawn("SpellbookAbilityListPassive");
                Respawn("SpellbookAbilityListPassiveBottom");
            });
        }),
        new("SpellbookAbilityListPassive", () => {
            var rowAmount = 6;
            var thisWindow = CDesktop.LBWindow();
            var passiveAbilities = abilities.FindAll(x => x.icon != null && !x.hide && x.cost == null && currentSave.player.abilities.ContainsKey(x.name)).ToDictionary(x => x, x => currentSave.player.abilities[x.name]);
            var list = passiveAbilities.ToList();
            thisWindow.SetPagination(() => list.Count, rowAmount);
            SetAnchor(TopRight, 0, -38);
            AddHeaderGroup();
            SetRegionGroupWidth(190);
            SetRegionGroupHeight(269);
            AddHeaderRegion(() =>
            {
                AddLine("Passive abilities:");
                AddSmallButton("OtherReverse", (h) =>
                {
                    abilities.Reverse();
                    Respawn("SpellbookAbilityListPassive");
                    PlaySound("DesktopInventorySort", 0.4f);
                });
                if (!WindowUp("AbilitiesSort") && !WindowUp("SwitchActionSet"))
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
            AddPaginationLine();
            AddRegionGroup();
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                if (list.Count > index + thisWindow.pagination())
                    AddPaddingRegion(() =>
                    {
                        var key = passiveAbilities.ToList()[index + thisWindow.pagination()];
                        AddBigButton(key.Key.icon,
                            null,
                            null,
                            (h) => () =>
                            {
                                SetAnchor(Center);
                                var key = passiveAbilities.ToList()[index + thisWindow.pagination()].Key;
                                PrintAbilityTooltip(currentSave.player, key, passiveAbilities[key]);
                            }
                        );
                    });
                else
                    AddPaddingRegion(() =>
                    {
                        SetRegionBackground(Padding);
                        AddBigButton("OtherDisabled");
                    });
            }
            AddRegionGroup();
            SetRegionGroupWidth(152);
            for (int i = 0; i < rowAmount; i++)
            {
                var index = i;
                if (list.Count > index + thisWindow.pagination())
                {
                    var key = passiveAbilities.ToList()[index + thisWindow.pagination()];
                    AddHeaderRegion(() =>
                    {
                        AddLine(key.Key.name);
                        AddText(" " + (key.Value + 1).ToRoman());
                    });
                    AddPaddingRegion(() => AddLine());
                }
                else
                {
                    AddHeaderRegion(() => AddLine());
                    AddPaddingRegion(() => AddLine());
                }
            }
        }),
        new("SpellbookAbilityListPassiveBottom", () => {
            SetAnchor(BottomRight, 0, 35);
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddButtonRegion(() => AddLine("Activated", "", "Center"), (h) =>
            {
                CloseWindow("SpellbookAbilityListPassive");
                CloseWindow("SpellbookAbilityListPassiveBottom");
                Respawn("SpellbookAbilityListActivated");
                Respawn("SpellbookAbilityListActivatedBottom");
            });
            AddRegionGroup();
            SetRegionGroupWidth(95);
            AddPaddingRegion(() => AddLine("Passive", "", "Center"));
        }),
        new("PlayerSpellbookInfo", () => {
            SetAnchor(TopLeft, 0, -38);
            AddRegionGroup();
            SetRegionGroupWidth(190);
            AddHeaderRegion(() =>
            {
                AddLine(currentSave.player.currentActionSet + " action set:");
                var list = ActionSet.actionSets.Where(x => x.alwaysVisible || currentSave.player.abilities.ContainsKey(x.abilityForVisibility));
                if (list.Count() > 0)
                    if (!WindowUp("AbilitiesSort") && !WindowUp("SwitchActionSet") && !WindowUp("SpellbookSettings"))
                        AddSmallButton("OtherSwitch", (h) =>
                        {
                            SpawnWindowBlueprint("SwitchActionSet");
                            CloseWindow("PlayerSpellbookInfo");
                            Respawn("PlayerSpellbookInfo");
                            if (CloseWindow("SpellbookAbilityListActivated"))
                                Respawn("SpellbookAbilityListActivated");
                            if (CloseWindow("SpellbookAbilityListPassive"))
                                Respawn("SpellbookAbilityListPassive");
                        });
                    else AddSmallButton("OtherSwitchOff");
            });
            AddButtonRegion(() =>
            {
                AddLine(currentSave.player.name, "", "Right");
                SpawnFloatingText(new Vector3(158, -9), currentSave.player.level + "", "Gray", "DimGray", "Left");
                ReverseButtons();
                AddBigButton("Portrait" + currentSave.player.race.Clean() + currentSave.player.gender + currentSave.player.portraitID, (h) => { });
                AddHealthBar(2, -19, -1, currentSave.player);
            },
            (h) => { });
            var actionSet = currentSave.player.actionSets[currentSave.player.currentActionSet];
            int length = currentSave.player.ActionSetMaxLength();
            for (int i = 0; i < length; i++)
            {
                var index = i;
                var abilityObj = actionSet.Count <= index ? null : abilities.Find(x => x.name == actionSet[index]);
                if (abilityObj != null)
                AddButtonRegion(
                    () =>
                    {
                        AddLine(abilityObj.name, "", "Right");
                        AddSmallButton(abilityObj.icon);
                    },
                    (h) =>
                    {
                        var temp = ActionSet.actionSets.Find(x => x.name == currentSave.player.currentActionSet);
                        if (temp != null && temp.forcedAbilities.Contains(actionSet[index]))
                        {
                            SpawnFallingText(new Vector2(0, 34), "This ability is enforced for this action set", "Red");
                            PlaySound("DesktopCantClick");
                        }
                        else
                        {
                            actionSet.RemoveAt(index);
                            Respawn("SpellbookAbilityListActivated", true);
                            Respawn("SpellbookAbilityListPassive", true);
                            Respawn("PlayerSpellbookInfo");
                            PlaySound("DesktopActionBarRemove", 0.9f);
                        }
                    },
                    null,
                    (h) => () =>
                    {
                        currentSave.player.ResetResources();
                        foreach (var res in abilityObj.cost)
                            if (res.Value < currentSave.player.MaxResource(res.Key))
                                currentSave.player.resources[res.Key] = res.Value;
                            else currentSave.player.resources[res.Key] = currentSave.player.MaxResource(res.Key);
                        spellbookResourceBars.ToList().ForEach(x => x.Value.UpdateFluidBar());
                        currentSave.player.ResetResources();
                        SetAnchor(Center);
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
            var item = currentSave.player.equipment.ContainsKey("Trinket") ? currentSave.player.equipment["Trinket"] : null;
            if (item != null && item.abilities != null && item.combatUse)
            {
                var ability = item.abilities.ToList()[0];
                var abilityObj = abilities.Find(x => x.name == ability.Key);
                AddButtonRegion(
                    () =>
                    {
                        AddLine(ability.Key, "", "Right");
                        AddSmallButton(item.icon);
                    },
                    null,
                    null,
                    (h) => () =>
                    {
                        PrintAbilityTooltip(currentSave.player, abilityObj, currentSave.player.abilities.ContainsKey(abilityObj.name) ? currentSave.player.abilities[abilityObj.name] : 0, item);
                    }
                );
            }
        }),
        new("SwitchActionSet", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Select action set:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("SwitchActionSet");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Default");
                AddSmallButton("AbilityDefault");
            },
            (h) =>
            {
                currentSave.player.currentActionSet = "Default";
                CloseWindow("SwitchActionSet");
                CDesktop.RespawnAll();
            });
            var list = ActionSet.actionSets.Where(x => x.alwaysVisible || currentSave.player.abilities.ContainsKey(x.abilityForVisibility));
            foreach (var set in list)
                AddButtonRegion(() =>
                {
                    AddLine(set.name);
                    AddSmallButton(set.icon);
                },
                (h) =>
                {
                    currentSave.player.currentActionSet = set.name;
                    if (currentSave.player.actionSets.ContainsKey(set.name) && set.forcedAbilities.Any(x => !currentSave.player.actionSets[set.name].Contains(x)))
                        currentSave.player.actionSets.Remove(set.name);
                    if (!currentSave.player.actionSets.ContainsKey(set.name))
                    {
                        currentSave.player.actionSets.Add(set.name, new());
                        foreach (var ability in set.forcedAbilities)
                            currentSave.player.actionSets[set.name].Add(ability);
                    }
                    CloseWindow("SwitchActionSet");
                    Respawn("SpellbookAbilityListActivated", true);
                    Respawn("SpellbookAbilityListPassive", true);
                    Respawn("PlayerSpellbookInfo");
                    foreach (var res in currentSave.player.resources)
                    Respawn("Spellbook" + res.Key + "Resource");
                });
        }),
        new("AbilitiesSort", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Sort abilities:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("AbilitiesSort");
                    CDesktop.RespawnAll();
                });
            });
            AddButtonRegion(() => AddLine("By name", "Black"),
            (h) =>
            {
                abilities = abilities.OrderBy(x => x.name).ToList();
                CloseWindow("AbilitiesSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By status", "Black"),
            (h) =>
            {
                abilities = abilities.OrderBy(x => currentSave.player.actionSets[currentSave.player.currentActionSet].Contains(x.name)).ToList();
                CloseWindow("AbilitiesSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() =>
            {
                AddLine("By rank", "Black");
            },
            (h) =>
            {
                abilities = abilities.OrderByDescending(x => currentSave.player.abilities.ContainsKey(x.name) ? currentSave.player.abilities[x.name] : 0).ToList();
                CloseWindow("AbilitiesSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By cost", "Black"),
            (h) =>
            {
                abilities = abilities.OrderByDescending(x => x.cost == null ? -1 : x.cost.Sum(y => y.Value)).ToList();
                CloseWindow("AbilitiesSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
            AddButtonRegion(() => AddLine("By cooldown", "Black"),
            (h) =>
            {
                abilities = abilities.OrderByDescending(x => x.cooldown).ToList();
                CloseWindow("AbilitiesSort");
                CDesktop.RespawnAll();
                PlaySound("DesktopInventorySort", 0.4f);
            });
        }),
        new("SpellbookSettings", () => {
            SetAnchor(Center);
            AddRegionGroup();
            SetRegionGroupWidth(182);
            AddHeaderRegion(() =>
            {
                AddLine("Ability list settings:");
                AddSmallButton("OtherClose", (h) =>
                {
                    CloseWindow("SpellbookSettings");
                    Respawn("PlayerSpellbookInfo", true);
                    Respawn("SpellbookAbilityListActivated", true);
                    Respawn("SpellbookAbilityListPassive", true);
                });
            });
            AddButtonRegion(() =>
            {
                AddLine("Show resource icons", "Black");
                AddCheckbox(settings.showSpellbookResourceIcons);
            },
            (h) =>
            {
                settings.showSpellbookResourceIcons.Invert();
                Respawn("SpellbookAbilityListActivated", true);
                Respawn("SpellbookAbilityListPassive", true);
            });
            AddButtonRegion(() =>
            {
                AddLine("Show valid action sets", "Black");
                AddCheckbox(settings.showSpellbookValidActionSets);
            },
            (h) =>
            {
                settings.showSpellbookValidActionSets.Invert();
                Respawn("SpellbookAbilityListActivated", true);
                Respawn("SpellbookAbilityListPassive", true);
            });
            AddButtonRegion(() =>
            {
                AddLine("Hide set-invalid abilities", "Black");
                AddCheckbox(settings.hideSetInvalidAbilities);
            },
            (h) =>
            {
                settings.hideSetInvalidAbilities.Invert();
                Respawn("SpellbookAbilityListActivated", true);
                Respawn("SpellbookAbilityListPassive", true);
            });
        }, true),

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
    };

    //List of all desktop blueprints that serve as playground for windows
    public static List<Blueprint> desktopBlueprints = new()
    {
        new("GameMenu", () =>
        {
            SetDesktopBackground("Backgrounds/StoneFull");
            SpawnWindowBlueprint("GameMenu");
            AddHotkey("Open menu / Back", () =>
            {
                if (CloseWindow("Settings"))
                {
                    PlaySound("DesktopButtonClose");
                    SpawnWindowBlueprint("GameMenu");
                }
                else if (CloseWindow("GameKeybinds"))
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
                        Respawn("PlayerWeaponsInfo");
                        Respawn("Inventory");
                    }
                }
            });
        }),
        new("CombatResultsLoot", () =>
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("CombatResultsLoot");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (movingItem != null)
                {
                    currentSave.player.inventory.items.Add(movingItem);
                    PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
                    Cursor.cursor.iconAttached.SetActive(false);
                    movingItem = null;
                    Respawn("Inventory", true);
                }
                else
                {
                    PlaySound("DesktopInventoryClose");
                    CloseDesktop("CombatResultsLoot");
                    SwitchDesktop("CombatResults");
                    Respawn("CombatResults");
                }
            });
        }),
        new("ContainerLoot", () =>
        {
            SetDesktopBackground("Backgrounds/Leather");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("ContainerLoot");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (movingItem != null)
                {
                    currentSave.player.inventory.items.Add(movingItem);
                    PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
                    Cursor.cursor.iconAttached.SetActive(false);
                    movingItem = null;
                    Respawn("Inventory", true);
                }
                else
                {
                    if (openedItem.itemsInside.Count == 0)
                        currentSave.player.inventory.items.Remove(openedItem);
                    openedItem = null;
                    PlaySound("DesktopInventoryClose");
                    CloseDesktop("ContainerLoot");
                    SpawnDesktopBlueprint("EquipmentScreen");
                }
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
                if (board.results.WillIncreaseMiningSkill())
                {
                    currentSave.player.professionSkills["Mining"] = (s.Item1 + 1, s.Item2);
                    SpawnFallingText(new Vector2(0, 34), "Mining increased to " + (s.Item1 + 1), "Blue");
                }
            }
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (movingItem != null)
                {
                    currentSave.player.inventory.items.Add(movingItem);
                    PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
                    Cursor.cursor.iconAttached.SetActive(false);
                    movingItem = null;
                    Respawn("Inventory", true);
                }
                else
                {
                    PlaySound("DesktopInventoryClose");
                    CloseDesktop("MiningLoot");
                    Respawn("CombatResultsMining");
                }
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
                if (board.results.WillIncreaseHerbalismSkill())
                {
                    currentSave.player.professionSkills["Herbalism"] = (s.Item1 + 1, s.Item2);
                    SpawnFallingText(new Vector2(0, 34), "Herbalism increased to " + (s.Item1 + 1), "Blue");
                }
            }
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (movingItem != null)
                {
                    currentSave.player.inventory.items.Add(movingItem);
                    PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
                    Cursor.cursor.iconAttached.SetActive(false);
                    movingItem = null;
                    Respawn("Inventory", true);
                }
                else
                {
                    PlaySound("DesktopInventoryClose");
                    CloseDesktop("HerbalismLoot");
                    Respawn("CombatResultsHerbalism");
                }
            });
        }),
        new("SkinningLoot", () =>
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("SkinningLoot");
            var s = currentSave.player.professionSkills["Skinning"];
            if (!board.results.skinningSkillChange[board.results.selectedSkinningLoot])
            {
                currentSave.AddTime(30);
                board.results.skinningSkillChange[board.results.selectedSkinningLoot] = true;
                if (board.results.WillIncreaseSkinningSkill(board.results.selectedSkinningLoot))
                {
                    currentSave.player.professionSkills["Skinning"] = (s.Item1 + 1, s.Item2);
                    SpawnFallingText(new Vector2(0, 34), "Skinning increased to " + (s.Item1 + 1), "Blue");
                }
            }
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (movingItem != null)
                {
                    currentSave.player.inventory.items.Add(movingItem);
                    PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
                    Cursor.cursor.iconAttached.SetActive(false);
                    movingItem = null;
                    Respawn("Inventory", true);
                }
                else
                {
                    PlaySound("DesktopInventoryClose");
                    CloseDesktop("SkinningLoot");
                    Respawn("CombatResultsSkinning1");
                    Respawn("CombatResultsSkinning2");
                    Respawn("CombatResultsSkinning3");
                }
            });
        }),
        new("ChestLoot", () =>
        {
            SetDesktopBackground(area.Background());
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("ChestInfo");
            SpawnWindowBlueprint("ChestLoot");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (movingItem != null)
                {
                    currentSave.player.inventory.items.Add(movingItem);
                    PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
                    Cursor.cursor.iconAttached.SetActive(false);
                    movingItem = null;
                    Respawn("Inventory", true);
                }
                else
                {
                    var zone = zones.Find(x => x.name == area.zone);
                    var chestID = area.chestVariant != 0 ? area.chestVariant : (zone.chestVariant != 0 ? zone.chestVariant : 6);
                    PlaySound("DesktopChest" + chestID + "Close");
                    CloseDesktop("ChestLoot");
                }
            });
        }),
        new("DisenchantLoot", () =>
        {
            SetDesktopBackground("Backgrounds/Leather");
            SpawnWindowBlueprint("DisenchantLoot");
            var s = currentSave.player.professionSkills["Enchanting"];
            if (!enchantingSkillChange)
            {
                currentSave.AddTime(30);
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
            SpawnWindowBlueprint("LootInfo");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (movingItem != null)
                {
                    currentSave.player.inventory.items.Add(movingItem);
                    PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
                    Cursor.cursor.iconAttached.SetActive(false);
                    movingItem = null;
                    Respawn("Inventory", true);
                }
                else
                {
                    disenchantLoot = null;
                    enchantingSkillChange = false;
                    CloseDesktop("DisenchantLoot");
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
            SpawnWindowBlueprint("CombatResultsChart");
            SpawnWindowBlueprint("CombatResultsChartLeftArrow");
            SpawnWindowBlueprint("CombatResultsChartRightArrow");
            FillChart();
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Move camera west", () =>
            {
                PlaySound("DesktopChartSwitch");
                if (chartPage == "Damage Taken") chartPage = "Damage Dealt";
                else if (chartPage == "Healing Received") chartPage = "Damage Taken";
                else if (chartPage == "Elements Used") chartPage = "Healing Received";
                else if (chartPage == "Damage Dealt") chartPage = "Elements Used";
                CloseDesktop("CombatLog");
                SpawnDesktopBlueprint("CombatLog");
            });
            AddHotkey("Move camera east", () =>
            {
                PlaySound("DesktopChartSwitch");
                if (chartPage == "Damage Dealt") chartPage = "Damage Taken";
                else if (chartPage == "Damage Taken") chartPage = "Healing Received";
                else if (chartPage == "Healing Received") chartPage = "Elements Used";
                else if (chartPage == "Elements Used") chartPage = "Damage Dealt";
                CloseDesktop("CombatLog");
                SpawnDesktopBlueprint("CombatLog");
            });
            AddHotkey("Open menu / Back", () =>
            {
                PlaySound("DesktopInstanceClose");
                CloseDesktop("CombatLog");
            });
        }),
        new("CombatResults", () =>
        {
            SetDesktopBackground(board.area.Background());
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("CombatResults");
            SpawnWindowBlueprint("CombatResultsChartButton");
            SpawnWindowBlueprint("CombatResultsMining");
            SpawnWindowBlueprint("CombatResultsHerbalism");
            SpawnWindowBlueprint("CombatResultsSkinning1");
            SpawnWindowBlueprint("CombatResultsSkinning2");
            SpawnWindowBlueprint("CombatResultsSkinning3");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (board.results.result == "Team1Won")
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("CombatResults");
                    board = null;
                    Respawn("ExperienceBar", true);
                    Respawn("AreaElites");
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
            SpawnWindowBlueprint("FriendlyBattleInfo");
            SpawnWindowBlueprint("LocationInfo");
            SpawnWindowBlueprint("EnemyBattleInfo");
            SpawnWindowBlueprint("ItemQuickUse");
            CDesktop.LBWindow().gameObject.AddComponent<MoveAwayQuickUse>();
            var elements = new List<string> { "Fire", "Water", "Earth", "Air", "Frost", "Lightning", "Arcane", "Decay", "Order", "Shadow" };
            foreach (var element in elements)
            {
                SpawnWindowBlueprint("Friendly" + element + "Resource");
                SpawnWindowBlueprint("Enemy" + element + "Resource");
            }
            AddHotkey(PageUp, () => {
                board.participants[board.team1[0]].who.resources = new Dictionary<string, int>
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
                Respawn("FriendlyBattleInfo");
                board.UpdateResourceBars(board.team1[0], elements);
            });
            AddHotkey(PageDown, () => {
                board.participants[board.team2[0]].who.resources = new Dictionary<string, int>
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
                board.UpdateResourceBars(board.team2[0], elements);
            });
            AddHotkey("Open menu / Back", () =>
            {
                if (abilityTargetted != null)
                {
                    PlaySound("DesktopInstanceClose");
                    Cursor.cursor.SetCursor(CursorType.Default);
                    Cursor.cursor.iconAttached.SetActive(false);
                    abilityTargetted = null;
                }
                else
                {
                    PlaySound("DesktopMenuOpen", 0.6f);
                    SpawnDesktopBlueprint("GameMenu");
                }
            });
            AddHotkey("Open console", () => { SpawnDesktopBlueprint("DevPanel"); });
            AddHotkey(KeypadMultiply, () => { board.EndCombat("Team1Won"); });
            AddHotkey(KeypadDivide, () => { currentSave.player.Die(); board.EndCombat("Team2Won"); });
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
            AddHotkey("Open menu / Back", () =>
            {
                PlaySound("DesktopMenuOpen", 0.6f);
                SpawnDesktopBlueprint("GameMenu");
            });
            AddHotkey(KeypadMultiply, () => { fishingSpot.EndFishing("Team1Won"); });
            AddHotkey(KeypadDivide, () => { fishingSpot.EndFishing("Team2Won"); });
        }),
        new("Complex", () =>
        {
            SetDesktopBackground(complex.Background());
            SpawnWindowBlueprint("Complex");
            SpawnWindowBlueprint("PlayerMoney");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (CloseWindow("AreaQuestTracker"))
                {
                    PlaySound("DesktopButtonClose");
                    Respawn("Area");
                    Respawn("AreaQuestAvailable");
                }
                else if (CloseWindow("QuestAdd") || CloseWindow("QuestTurn"))
                {
                    quest = null;
                    PlaySound("DesktopButtonClose");
                    Respawn("Area");
                    Respawn("AreaQuestAvailable");
                    Respawn("AreaQuestDone");
                    Respawn("Complex");
                    Respawn("PlayerMoney");
                }
                else if (CloseWindow("BankSort"))
                {
                    PlaySound("DesktopInstanceClose");
                }
                else if (CloseWindow("MountCollection"))
                {
                    PlaySound("DesktopInstanceClose");
                    CloseWindow("CurrentMount");
                    Respawn("Person");
                }
                else if (CloseWindow("Inventory"))
                {
                    PlaySound("DesktopInventoryClose");
                    CloseWindow("Bank");
                    if (CloseWindow("Vendor"))
                        PlaySound("DesktopCharacterSheetClose");
                    CloseWindow("VendorBuyback");
                    Respawn("PlayerMoney");
                    Respawn("Person");
                }
                else if (CloseWindow("MakeInnHome"))
                {
                    PlaySound("DesktopInstanceClose");
                    CloseWindow("MakeInnHome");
                    Respawn("Person");
                }
                else if (CloseWindow("FlightMaster"))
                {
                    PlaySound("DesktopMenuClose");
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
                else if (CloseWindow("AuctionHouseFilteringTwoHandedWeapons") || CloseWindow("AuctionHouseFilteringOneHandedWeapons") || CloseWindow("AuctionHouseFilteringOffHands") || CloseWindow("AuctionHouseFilteringRangedWeapons") || CloseWindow("AuctionHouseFilteringArmorClass") || CloseWindow("AuctionHouseFilteringArmorType") || CloseWindow("AuctionHouseFilteringJewelry") || CloseWindow("AuctionHouseFilteringConsumeables"))
                {
                    auctionCategory = "";
                    UpdateAuctionGroupList();
                    Respawn("AuctionHouseOffersGroups", true);
                    Respawn("AuctionHouseFilteringMain");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                }
                else if (CloseWindow("AuctionHouseOffersGroups"))
                {
                    PlaySound("DesktopInstanceClose");
                    CloseWindow("AuctionHouseFilteringMain");
                    CloseWindow("AuctionHouseFilteringTwoHandedWeapons");
                    CloseWindow("AuctionHouseFilteringOneHandedWeapons");
                    CloseWindow("AuctionHouseFilteringOffHands");
                    CloseWindow("AuctionHouseFilteringRangedWeapons");
                    CloseWindow("AuctionHouseFilteringArmorClass");
                    CloseWindow("AuctionHouseFilteringArmorType");
                    CloseWindow("AuctionHouseFilteringJewelry");
                    CloseWindow("AuctionHouseFilteringConsumeables");
                    for (int i = 0; i < 12; i++) { var index = i; CloseWindow("AuctionHousePrice" + index); }
                    Respawn("Person");
                }
                else if (CloseWindow("AuctionHouseOffers"))
                {
                    CloseWindow("AuctionHouseOffers");
                    CloseWindow("AuctionHouseBuy");
                    CloseWindow("AuctionHouseChosenItem");
                    Respawn("AuctionHouseOffersGroups");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                    PlaySound("DesktopInstanceClose");
                }
                else if (CloseWindow("Person"))
                {
                    PlaySound("DesktopInstanceClose");
                    person = null;
                    if (personCategory != null) Respawn("Persons");
                    else CloseWindow("Persons");
                    Respawn("Area");
                    Respawn("Complex");
                    Respawn("Quest");
                    Respawn("QuestAdd");
                    Respawn("QuestTurn");
                    Respawn("AreaQuestAvailable");
                    Respawn("AreaQuestDone");
                    if (!WindowUp("Persons"))
                    {
                        Respawn("AreaProgress");
                        Respawn("AreaElites");
                        Respawn("Chest");
                    }
                }
                else if (CloseWindow("Persons"))
                {
                    PlaySound("DesktopInstanceClose");
                    personCategory = null;
                    Respawn("Area");
                    Respawn("Complex");
                    Respawn("Quest");
                    Respawn("QuestAdd");
                    Respawn("QuestTurn");
                    Respawn("AreaProgress");
                    Respawn("AreaElites");
                    Respawn("AreaQuestAvailable");
                    Respawn("AreaQuestDone");
                    Respawn("Chest");
                }
                else if (CloseWindow("Area"))
                {
                    area = null;
                    PlaySound("DesktopButtonClose");
                    CloseWindow("AreaQuestAvailable");
                    CloseWindow("AreaQuestDone");
                    CloseWindow("AreaProgress");
                    CloseWindow("AreaElites");
                    CloseWindow("Chest");
                    SetDesktopBackground(complex.Background());
                }
                else
                {
                    PlaySound("DesktopInstanceClose");
                    CloseDesktop("Complex");
                }
            });
            AddPaginationHotkeys();
        }),
        new("Instance", () =>
        {
            SetDesktopBackground(instance.Background());
            if (wing != null) SpawnWindowBlueprint("InstanceWing");
            SpawnWindowBlueprint("Instance");
            SpawnWindowBlueprint("PlayerMoney");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (CloseWindow("AreaQuestTracker"))
                {
                    PlaySound("DesktopButtonClose");
                    Respawn("Area");
                    Respawn("AreaQuestAvailable");
                }
                else if (CloseWindow("QuestAdd") || CloseWindow("QuestTurn"))
                {
                    quest = null;
                    PlaySound("DesktopButtonClose");
                    Respawn("Area");
                    Respawn("AreaQuestAvailable");
                    Respawn("AreaQuestDone");
                    Respawn("InstanceWing");
                    Respawn("Instance");
                    Respawn("PlayerMoney");
                }
                else if (CloseWindow("BankSort"))
                {
                    PlaySound("DesktopInstanceClose");
                }
                else if (CloseWindow("MountCollection"))
                {
                    PlaySound("DesktopInstanceClose");
                    CloseWindow("CurrentMount");
                    Respawn("Person");
                }
                else if (CloseWindow("Inventory"))
                {
                    PlaySound("DesktopInventoryClose");
                    CloseWindow("Bank");
                    if (CloseWindow("Vendor"))
                        PlaySound("DesktopCharacterSheetClose");
                    CloseWindow("VendorBuyback");
                    Respawn("PlayerMoney");
                    Respawn("Person");
                }
                else if (CloseWindow("MakeInnHome"))
                {
                    PlaySound("DesktopInstanceClose");
                    CloseWindow("MakeInnHome");
                    Respawn("Person");
                }
                else if (CloseWindow("FlightMaster"))
                {
                    PlaySound("DesktopMenuClose");
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
                else if (CloseWindow("AuctionHouseFilteringTwoHandedWeapons") || CloseWindow("AuctionHouseFilteringOneHandedWeapons") || CloseWindow("AuctionHouseFilteringOffHands") || CloseWindow("AuctionHouseFilteringRangedWeapons") || CloseWindow("AuctionHouseFilteringArmorClass") || CloseWindow("AuctionHouseFilteringArmorType") || CloseWindow("AuctionHouseFilteringJewelry") || CloseWindow("AuctionHouseFilteringConsumeables"))
                {
                    auctionCategory = "";
                    UpdateAuctionGroupList();
                    Respawn("AuctionHouseOffersGroups", true);
                    Respawn("AuctionHouseFilteringMain");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                }
                else if (CloseWindow("AuctionHouseOffersGroups"))
                {
                    PlaySound("DesktopInstanceClose");
                    CloseWindow("AuctionHouseFilteringMain");
                    CloseWindow("AuctionHouseFilteringTwoHandedWeapons");
                    CloseWindow("AuctionHouseFilteringOneHandedWeapons");
                    CloseWindow("AuctionHouseFilteringOffHands");
                    CloseWindow("AuctionHouseFilteringRangedWeapons");
                    CloseWindow("AuctionHouseFilteringArmorClass");
                    CloseWindow("AuctionHouseFilteringArmorType");
                    CloseWindow("AuctionHouseFilteringJewelry");
                    CloseWindow("AuctionHouseFilteringConsumeables");
                    for (int i = 0; i < 12; i++) { var index = i; CloseWindow("AuctionHousePrice" + index); }
                    Respawn("Person");
                }
                else if (CloseWindow("AuctionHouseOffers"))
                {
                    CloseWindow("AuctionHouseOffers");
                    CloseWindow("AuctionHouseBuy");
                    CloseWindow("AuctionHouseChosenItem");
                    Respawn("AuctionHouseOffersGroups");
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                    PlaySound("DesktopInstanceClose");
                }
                else if (CloseWindow("Person"))
                {
                    PlaySound("DesktopInstanceClose");
                    person = null;
                    if (personCategory != null) Respawn("Persons");
                    else CloseWindow("Persons");
                    Respawn("Area");
                    Respawn("Instance");
                    Respawn("InstanceWing");
                    Respawn("Quest");
                    Respawn("QuestAdd");
                    Respawn("QuestTurn");
                    Respawn("AreaQuestAvailable");
                    Respawn("AreaQuestDone");
                    if (!WindowUp("Persons"))
                    {
                        Respawn("AreaProgress");
                        Respawn("AreaElites");
                        Respawn("Chest");
                    }
                }
                else if (CloseWindow("Persons"))
                {
                    PlaySound("DesktopInstanceClose");
                    personCategory = null;
                    Respawn("Area");
                    Respawn("Complex");
                    Respawn("Quest");
                    Respawn("QuestAdd");
                    Respawn("QuestTurn");
                    Respawn("AreaProgress");
                    Respawn("AreaElites");
                    Respawn("AreaQuestAvailable");
                    Respawn("AreaQuestDone");
                    Respawn("Chest");
                }
                else if (CloseWindow("Area"))
                {
                    area = null;
                    CloseWindow("AreaQuestTracker");
                    CloseWindow("AreaQuestAvailable");
                    CloseWindow("AreaQuestDone");
                    CloseWindow("AreaProgress");
                    CloseWindow("AreaElites");
                    CloseWindow("Chest");
                    PlaySound("DesktopButtonClose");
                    if (wing != null) SetDesktopBackground(wing.Background());
                    else SetDesktopBackground(instance.Background());
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
                    PlaySound("DesktopButtonClose");
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
        new("Area", () =>
        {
            personCategory = null;
            SetDesktopBackground(area.Background());
            if (currentSave.player.Reputation(area.faction) >= ReputationRankToAmount("Neutral"))
            {
                SpawnWindowBlueprint("Area");
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
                AddHotkey("Open menu / Back", () =>
                {
                    if (CloseWindow("AreaQuestTracker"))
                    {
                        PlaySound("DesktopButtonClose");
                        Respawn("Area");
                        Respawn("AreaQuestAvailable");
                    }
                    else if (CloseWindow("QuestAdd") || CloseWindow("QuestTurn"))
                    {
                        quest = null;
                        PlaySound("DesktopButtonClose");
                        Respawn("Area");
                        Respawn("AreaQuestAvailable");
                        Respawn("AreaQuestDone");
                        Respawn("Complex");
                        Respawn("PlayerMoney");
                    }
                    else if (CloseWindow("BankSort"))
                    {
                        PlaySound("DesktopInstanceClose");
                    }
                    else if (CloseWindow("MountCollection"))
                    {
                        PlaySound("DesktopInstanceClose");
                        CloseWindow("CurrentMount");
                        Respawn("Person");
                    }
                    else if (CloseWindow("Inventory"))
                    {
                        PlaySound("DesktopInventoryClose");
                        CloseWindow("Bank");
                        if (CloseWindow("Vendor"))
                            PlaySound("DesktopCharacterSheetClose");
                        CloseWindow("VendorBuyback");
                        Respawn("PlayerMoney");
                        Respawn("Person");
                    }
                    else if (CloseWindow("MakeInnHome"))
                    {
                        PlaySound("DesktopInstanceClose");
                        CloseWindow("MakeInnHome");
                        Respawn("Person");
                    }
                    else if (CloseWindow("FlightMaster"))
                    {
                        PlaySound("DesktopMenuClose");
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
                    else if (CloseWindow("AuctionHouseFilteringTwoHandedWeapons") || CloseWindow("AuctionHouseFilteringOneHandedWeapons") || CloseWindow("AuctionHouseFilteringOffHands") || CloseWindow("AuctionHouseFilteringRangedWeapons") || CloseWindow("AuctionHouseFilteringArmorClass") || CloseWindow("AuctionHouseFilteringArmorType") || CloseWindow("AuctionHouseFilteringJewelry") || CloseWindow("AuctionHouseFilteringConsumeables"))
                    {
                        auctionCategory = "";
                        UpdateAuctionGroupList();
                        Respawn("AuctionHouseOffersGroups", true);
                        Respawn("AuctionHouseFilteringMain");
                        for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                    }
                    else if (CloseWindow("AuctionHouseOffersGroups"))
                    {
                        PlaySound("DesktopInstanceClose");
                        CloseWindow("AuctionHouseFilteringMain");
                        CloseWindow("AuctionHouseFilteringTwoHandedWeapons");
                        CloseWindow("AuctionHouseFilteringOneHandedWeapons");
                        CloseWindow("AuctionHouseFilteringOffHands");
                        CloseWindow("AuctionHouseFilteringRangedWeapons");
                        CloseWindow("AuctionHouseFilteringArmorClass");
                        CloseWindow("AuctionHouseFilteringArmorType");
                        CloseWindow("AuctionHouseFilteringJewelry");
                        CloseWindow("AuctionHouseFilteringConsumeables");
                        for (int i = 0; i < 12; i++) { var index = i; CloseWindow("AuctionHousePrice" + index); }
                        Respawn("Person");
                    }
                    else if (CloseWindow("AuctionHouseOffers"))
                    {
                        CloseWindow("AuctionHouseOffers");
                        CloseWindow("AuctionHouseBuy");
                        CloseWindow("AuctionHouseChosenItem");
                        Respawn("AuctionHouseOffersGroups");
                        for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
                        PlaySound("DesktopInstanceClose");
                    }
                    else if (CloseWindow("Person"))
                    {
                        PlaySound("DesktopInstanceClose");
                        person = null;
                        if (personCategory != null) Respawn("Persons");
                        else CloseWindow("Persons");
                        Respawn("Area");
                        Respawn("Complex");
                        Respawn("Quest");
                        Respawn("QuestAdd");
                        Respawn("QuestTurn");
                        Respawn("AreaQuestAvailable");
                        Respawn("AreaQuestDone");
                        if (!WindowUp("Persons"))
                        {
                            Respawn("AreaProgress");
                            Respawn("AreaElites");
                            Respawn("Chest");
                        }
                    }
                    else if (CloseWindow("Persons"))
                    {
                        PlaySound("DesktopInstanceClose");
                        personCategory = null;
                        Respawn("Area");
                        Respawn("Complex");
                        Respawn("Quest");
                        Respawn("QuestAdd");
                        Respawn("QuestTurn");
                        Respawn("AreaProgress");
                        Respawn("AreaElites");
                        Respawn("AreaQuestAvailable");
                        Respawn("AreaQuestDone");
                        Respawn("Chest");
                    }
                    else
                    {
                        PlaySound("DesktopInstanceClose");
                        CloseDesktop("Area");
                        area = null;
                        if (complex != null)
                            SwitchDesktop("Complex");
                    }
                });
            }
            else
            {
                SpawnWindowBlueprint("AreaHostile");
                AddHotkey("Open menu / Back", () =>
                {
                    PlaySound("DesktopInstanceClose");
                    area = null;
                    CloseDesktop("Area");
                });
            }
            SpawnWindowBlueprint("AreaProgress");
            SpawnWindowBlueprint("AreaElites");
            SpawnWindowBlueprint("AreaQuestAvailable");
            SpawnWindowBlueprint("AreaQuestDone");
            SpawnWindowBlueprint("PlayerMoney");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            SpawnWindowBlueprint("Chest");
            AddPaginationHotkeys();
        }),
        new("CharacterSheet", () =>
        {
            PlaySound("DesktopCharacterSheetOpen");
            SetDesktopBackground("Backgrounds/RedWood");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("CharacterInfoStats");
            SpawnWindowBlueprint("CharacterInfoPower");
            SpawnWindowBlueprint("CharacterInfoWeapons");
            SpawnWindowBlueprint("CharacterInfoDefences");
            SpawnWindowBlueprint("CharacterInfoMasteries");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                PlaySound("DesktopCharacterSheetClose");
                CloseDesktop("CharacterSheet");
            });
        }),
        new("TalentScreen", () =>
        {
            SetDesktopBackground("Backgrounds/Stone");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
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
            AddHotkey("Move camera west", () =>
            {
                PlaySound("DesktopSwitchPage");
                currentSave.lastVisitedTalents--;
                if (currentSave.lastVisitedTalents < 0)
                    currentSave.lastVisitedTalents = currentSave.player.Spec().talentTrees.Count - 1;
                CloseDesktop("TalentScreen");
                SpawnDesktopBlueprint("TalentScreen");
            });
            AddHotkey("Move camera east", () =>
            {
                PlaySound("DesktopSwitchPage");
                currentSave.lastVisitedTalents++;
                if (currentSave.lastVisitedTalents >= currentSave.player.Spec().talentTrees.Count)
                    currentSave.lastVisitedTalents = 0;
                CloseDesktop("TalentScreen");
                SpawnDesktopBlueprint("TalentScreen");
            });
            AddHotkey("Open menu / Back", () => { CloseDesktop("TalentScreen"); PlaySound("DesktopTalentScreenClose"); });
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
            SpawnWindowBlueprint("SpellbookAbilityListActivated");
            SpawnWindowBlueprint("SpellbookAbilityListActivatedBottom");
            SpawnWindowBlueprint("PlayerSpellbookInfo");
            foreach (var res in currentSave.player.resources)
                SpawnWindowBlueprint("Spellbook" + res.Key + "Resource");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (CloseWindow("SpellbookSettings"))
                {
                    PlaySound("DesktopButtonClose");
                    SpawnWindowBlueprint("TitleScreenMenu");
                }
                else if (CloseWindow("AbilitiesSort"))
                {
                    PlaySound("DesktopButtonClose");
                    Respawn("SpellbookAbilityListActivated", true);
                    Respawn("SpellbookAbilityListPassive", true);
                }
                else
                {
                    CloseDesktop("SpellbookScreen");
                    PlaySound("DesktopSpellbookClose");
                }
            });
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
            SpawnWindowBlueprint("PlayerEquipmentInfo");
            SpawnWindowBlueprint("PlayerWeaponsInfo");
            SpawnWindowBlueprint("Inventory");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (CloseWindow("InventorySort"))
                {
                    PlaySound("DesktopMenuClose");
                    Respawn("Inventory", true);
                }
                else if (CloseWindow("InventorySettings"))
                {
                    PlaySound("DesktopMenuClose");
                    Respawn("Inventory", true);
                }
                else if (CloseWindow("ConfirmItemDestroy"))
                {
                    itemToDestroy = null;
                    PlaySound("DesktopMenuClose");
                    Respawn("Inventory", true);
                }
                else if (CloseWindow("ConfirmItemDisenchant"))
                {
                    itemToDisenchant = null;
                    PlaySound("DesktopMenuClose");
                    Respawn("Inventory", true);
                }
                else if (Cursor.cursor.color == "Pink")
                {
                    Cursor.cursor.ResetColor();
                    Respawn("PlayerEquipmentInfo", true);
                    Respawn("PlayerWeaponsInfo", true);
                    Respawn("Inventory", true);
                }
                else if (movingItem != null)
                {
                    currentSave.player.inventory.items.Add(movingItem);
                    PlaySound(movingItem.ItemSound("PutDown"), 0.8f);
                    Cursor.cursor.iconAttached.SetActive(false);
                    movingItem = null;
                    Respawn("PlayerEquipmentInfo", true);
                    Respawn("PlayerWeaponsInfo", true);
                    Respawn("Inventory", true);
                }
                else
                {
                    PlaySound("DesktopInventoryClose");
                    CloseDesktop("EquipmentScreen");
                }
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
            SpawnWindowBlueprint("BestiaryKalimdor");
            SpawnWindowBlueprint("BestiaryEasternKingdoms");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
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
            SpawnWindowBlueprint("ProfessionListGathering");
            SpawnWindowBlueprint("MapToolbarShadow");
            SpawnWindowBlueprint("MapToolbarClockLeft");
            SpawnWindowBlueprint("MapToolbar");
            SpawnWindowBlueprint("MapToolbarClockRight");
            SpawnWindowBlueprint("ExperienceBarBorder");
            SpawnWindowBlueprint("ExperienceBar");
            AddHotkey("Open menu / Back", () =>
            {
                if (CloseWindow("CraftingRecipe"))
                {
                    PlaySound("DesktopInstanceClose");
                }
                else if (CloseWindow("CraftingList"))
                {
                    Respawn("ProfessionListPrimary");
                    Respawn("ProfessionListSecondary");
                    Respawn("ProfessionListGathering");
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
        new("Map", () => 
        {
            PlaySound("DesktopOpenSave", 0.3f);
            var find = FindSite(x => x.name == currentSave.currentSite);
            if (find != null) CDesktop.cameraDestination = new Vector2(find.x, find.y);
            Cursor.cursor.transform.position += (Vector3)CDesktop.cameraDestination - CDesktop.screen.transform.position;
            CDesktop.screen.transform.localPosition = CDesktop.cameraDestination;
            SetDesktopBackground("LoadingScreens/" + (CDesktop.cameraDestination.x < 2470 ? "Kalimdor" : "EasternKingdoms"));
            loadingBar = new GameObject[2];
            loadingBar[0] = new GameObject("LoadingBarBegin", typeof(SpriteRenderer));
            loadingBar[0].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/LoadingBarEnd");
            loadingBar[0].transform.position = new Vector3(-1171, 854);
            loadingBar[1] = new GameObject("LoadingBar", typeof(SpriteRenderer));
            loadingBar[1].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Other/LoadingBarStretch");
            loadingBar[1].transform.position = new Vector3(-1168, 854);
            OrderLoadingMap();
            AddHotkey("Move camera north", () => { MoveCamera(new Vector2(0, Eueler())); }, false, false);
            AddHotkey("Move camera west", () => { MoveCamera(new Vector2(-Eueler(), 0)); }, false, false);
            AddHotkey("Move camera south", () => { MoveCamera(new Vector2(0, -Eueler())); }, false, false);
            AddHotkey("Move camera east", () => { MoveCamera(new Vector2(Eueler(), 0)); }, false, false);
            AddHotkey(UpArrow, () =>
            {
                if (!debug) return;
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.y += (int)Math.Sqrt(Eueler());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Remove(find);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey(LeftArrow, () =>
            {
                if (!debug) return;
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.x -= (int)Math.Sqrt(Eueler());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey(DownArrow, () =>
            {
                if (!debug) return;
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.y -= (int)Math.Sqrt(Eueler());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey(RightArrow, () =>
            {
                if (!debug) return;
                var site = FindSite(x => x.name == currentSave.currentSite);
                site.x += (int)Math.Sqrt(Eueler());
                var find = windowBlueprints.Find(x => x.title == "Site: " + site.name);
                windowBlueprints.Add(new Blueprint("Site: " + site.name, () => site.PrintSite()));
                CloseWindow("Site: " + site.name);
                SpawnWindowBlueprint("Site: " + site.name);
            }, false);
            AddHotkey("Open menu / Back", () =>
            {
                if (CloseWindow("QuestSort"))
                {
                    SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
                    PlaySound("DesktopInstanceClose");
                    Respawn("Quest", true);
                    Respawn("QuestList");
                }
                else if (CloseWindow("QuestSettings"))
                {
                    SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
                    PlaySound("DesktopInstanceClose");
                    Respawn("Quest", true);
                    Respawn("QuestList");
                }
                else if (CloseWindow("QuestConfirmAbandon"))
                {
                    SetDesktopBackground("Backgrounds/RuggedLeather", true, true);
                    PlaySound("DesktopMenuClose");
                    Respawn("Quest", true);
                    Respawn("QuestList");
                }
                else if (CloseWindow("Quest"))
                {
                    quest = null;
                    Respawn("QuestList");
                    PlaySound("DesktopInstanceClose");
                }
                else if (CloseWindow("QuestList"))
                {
                    PlaySound("DesktopSpellbookClose");
                    RemoveDesktopBackground();
                    Respawn("MapToolbar");
                    Respawn("WorldBuffs");
                    SwitchToMostImportantDesktop();
                }
                else
                {
                    PlaySound("DesktopMenuOpen", 0.6f);
                    SpawnDesktopBlueprint("GameMenu");
                }
            });
            AddHotkey(Delete, () =>
            {
                if (sitePathBuilder != null)
                {
                    UnityEngine.Object.Destroy(pathTest.Item2);
                    sitePathBuilder = null;
                }
            });
            AddHotkey("Open console", () =>
            {
                if (debug && SpawnWindowBlueprint("Console") != null)
                {
                    PlaySound("DesktopTooltipShow", 0.4f);
                    CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                }
            });
            AddHotkey("Focus camera on player", () =>
            {
                var whereTo = FindSite(x => x.name == currentSave.currentSite);
                CDesktop.cameraDestination = new Vector2(whereTo.x, whereTo.y);
            }, false);

            void MoveCamera(Vector2 amount)
            {
                if (WindowUp("QuestList")) return;
                var temp = CDesktop.cameraDestination + amount * 2;
                CDesktop.cameraDestination = new Vector2(temp.x, temp.y);
            }
        }),
        new("RankingScreen", () => 
        {
            SetDesktopBackground("Backgrounds/SkyRed");
            SpawnWindowBlueprint("CharacterRankingShadow");
            SpawnWindowBlueprint("CharacterRankingTop");
            SpawnWindowBlueprint("CharacterRankingList");
            SpawnWindowBlueprint("CharacterRankingListRight");
            AddHotkey("Open menu / Back", () =>
            {
                PlaySound("DesktopButtonClose");
                CloseDesktop("RankingScreen");
            });
        }),
        new("LoginScreen", () =>
        {
            SpawnWindowBlueprint("CharacterRoster");
            SpawnWindowBlueprint("CharacterInfo");
            AddHotkey("Open console", () =>
            {
                if (debug && SpawnWindowBlueprint("Console") != null)
                {
                    PlaySound("DesktopTooltipShow", 0.4f);
                    CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                }
            });
            AddHotkey("Open menu / Back", () =>
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
        new("TitleScreen", () =>
        {
            StopAmbience(false);
            PlayMusic(new() { "MusicMainScreen" }, 0.2f, true);
            if (firstLaunch)
            {
                firstLaunch = false;
                SpawnWindowBlueprint("TitleScreenFirstLaunch");
            }
            else SpawnWindowBlueprint("TitleScreenMenu");
            AddHotkey("Open console", () =>
            {
                if (debug && SpawnWindowBlueprint("Console") != null)
                {
                    PlaySound("DesktopTooltipShow", 0.4f);
                    CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                }
            });
            AddHotkey("Open menu / Back", () =>
            {
                if (CloseWindow("GameSettings"))
                {
                    PlaySound("DesktopButtonClose");
                    SpawnWindowBlueprint("TitleScreenMenu");
                }
                else if (CloseWindow("TitleScreenFirstLaunch"))
                {
                    Serialization.Serialize(settings, "settings", false, false, Serialization.prefix);
                    PlaySound("DesktopButtonClose");
                    SpawnWindowBlueprint("TitleScreenMenu");
                }
                else if (CloseWindow("GameKeybinds"))
                {
                    PlaySound("DesktopButtonClose");
                    SpawnWindowBlueprint("TitleScreenMenu");
                }
                else if (CloseWindow("TitleScreenSingleplayer"))
                {
                    CloseWindow("TitleScreenContinue");
                    PlaySound("DesktopButtonClose");
                    SpawnWindowBlueprint("TitleScreenMenu");
                }
            });
        })
    };

    //Adds pagination hotkeys to the desktop to help in manouvering around the windows
    public static void AddPaginationHotkeys()
    {
        AddHotkey("Move camera east", () =>
        {
            var window = CDesktop.windows.Find(x => x.maxPaginationReq != null);
            if (window == null) return;
            var temp = window.pagination();
            window.IncrementPagination();
            if (temp != window.pagination())
            {
                PlaySound("DesktopChangePage", 0.6f);
                if (WindowUp("AuctionHouseOffers") || WindowUp("AuctionHouseOffersGroups"))
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            }
            window.Respawn();
        });
        AddHotkey("Move camera east", () =>
        {
            var window = CDesktop.windows.Find(x => x.maxPaginationReq != null);
            if (window == null) return;
            var temp = window.pagination();
            window.IncrementPaginationEuler();
            if (temp != window.pagination())
            {
                PlaySound("DesktopChangePage", 0.6f);
                if (WindowUp("AuctionHouseOffers") || WindowUp("AuctionHouseOffersGroups"))
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            }
            window.Respawn();
        }, false);
        AddHotkey("Move camera west", () =>
        {
            var window = CDesktop.windows.Find(x => x.maxPaginationReq != null);
            if (window == null) return;
            var temp = window.pagination();
            window.DecrementPagination();
            if (temp != window.pagination())
            {
                PlaySound("DesktopChangePage", 0.6f);
                if (WindowUp("AuctionHouseOffers") || WindowUp("AuctionHouseOffersGroups"))
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            }
            window.Respawn();
        });
        AddHotkey("Move camera west", () =>
        {
            var window = CDesktop.windows.Find(x => x.maxPaginationReq != null);
            if (window == null) return;
            var temp = window.pagination();
            window.DecrementPaginationEuler();
            if (temp != window.pagination())
            {
                PlaySound("DesktopChangePage", 0.6f);
                if (WindowUp("AuctionHouseOffers") || WindowUp("AuctionHouseOffersGroups"))
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            }
            window.Respawn();
        }, false);
        AddHotkey(PageUp, () =>
        {
            var moved = false;
            var window = CDesktop.windows.Find(x => x.maxPaginationReq != null);
            if (window == null) return;
            for (int i = Input.GetKey(LeftShift) && !window.paginateFullPages ? window.perPage - 1 : 0; i >= 0; i--)
                if (window.pagination() > 0)
                {
                    moved = true;
                    window.DecrementPagination();
                }
                else break;
            if (moved)
            {
                PlaySound("DesktopChangePage", 0.6f);
                window.Respawn();
                if (WindowUp("AuctionHouseOffers") || WindowUp("AuctionHouseOffersGroups"))
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            }
        });
        AddHotkey(PageDown, () =>
        {
            var moved = false;
            var window = CDesktop.windows.Find(x => x.maxPaginationReq != null);
            if (window == null) return;
            for (int i = Input.GetKey(LeftShift) && !window.paginateFullPages ? window.perPage - 1 : 0; i >= 0; i--)
                if (window.pagination() < window.maxPagination())
                {
                    moved = true;
                    window.IncrementPagination();
                }
                else break;
            if (moved)
            {
                PlaySound("DesktopChangePage", 0.6f);
                window.Respawn();
                if (WindowUp("AuctionHouseOffers") || WindowUp("AuctionHouseOffersGroups"))
                    for (int i = 0; i < 12; i++) { var index = i; Respawn("AuctionHousePrice" + index); }
            }
        });
    }
}
