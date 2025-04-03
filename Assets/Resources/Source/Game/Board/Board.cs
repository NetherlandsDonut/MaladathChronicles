using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Sound;
using static Shatter;
using static Defines;
using static SaveGame;
using static BufferBoard;
using static GameSettings;
using static CursorRemote;
using static FlyingMissile;
using static SiteHostileArea;

public class Board
{
    #region Initialisation

    public Board(int x, int y, List<Entity> enemies, Site area = null)
    {
        turn = 1;
        whosTurn = 0;
        field = new int[x, y];
        this.area = area;
        log = new();
        healthBars = new();
        resourceBars = new();
        flyingMissiles = new();
        actions = new List<Action>();
        spotlightFriendly = new() { 0 };
        spotlightEnemy = new() { };
        participants = new() { new() };
        participants[0].team = 1;
        participants[0].who = currentSave.player;
        participants[0].human = true;
        participants[0].combatAbilities = currentSave.player.AbilitiesInCombat();
        participants[0].who.InitialiseCombat();
        foreach (var enemy in enemies)
        {
            enemy.InitialiseCombat();
            var newParticipant = new CombatParticipant
            {
                team = 2,
                who = enemy,
                combatAbilities = enemy.AbilitiesInCombat()
            };
            participants.Add(newParticipant);
            spotlightEnemy.Add(participants.Count - 1);
        }

        var possible = Race.races.Where(x => !x.genderedPortrait).ToList();
        var choice = new Entity(currentSave.player.level, possible[random.Next(possible.Count)]);
        choice.InitialiseCombat();
        var additionallAlly = new CombatParticipant
        {
            team = 1,
            who = choice,
            combatAbilities = choice.AbilitiesInCombat()
        };
        participants.Add(additionallAlly);
        spotlightFriendly.Add(participants.Count - 1);

        cooldowns = new();
        foreach (var poo in participants)
            cooldowns.Add(participants.IndexOf(poo), new());
        temporaryBuffs = new();
        foreach (var poo in participants)
            temporaryBuffs.Add(participants.IndexOf(poo), new());
        Reset();
    }

    public Board(int x, int y, Dictionary<Ability, int> abilities)
    {
        turn = 1;
        spotlightFriendly = new() { 0 };
        spotlightEnemy = new() { 1 };
        field = new int[x, y];
        var possible = Race.races.Where(x => !x.genderedPortrait).ToList();
        participants = new() { new(), new() };
        participants[0].who = new Entity(60, null);
        participants[0].human = true;
        participants[0].team = 1;
        participants[0].who.InitialiseCombat();
        participants[1].who = new Entity(60, possible[random.Next(possible.Count)]);
        whosTurn = 0;
        area = areas[random.Next(areas.Count)];
        participants[0].who.currentActionSet = "Default";
        participants[0].combatAbilities = participants[0].who.AbilitiesInCombat();
        foreach (var a in abilities)
            if (!participants[0].combatAbilities.ContainsKey(a.Key))
            {
                participants[0].combatAbilities.Add(a.Key, a.Value);
                if (a.Key.cost != null) participants[0].who.actionBars["Default"].Add(a.Key.name);
            }
        cooldowns = new() { { 0, new() }, { 1, new() } };
        participants[1].team = 2;
        participants[1].who.currentActionSet = "Default";
        participants[1].combatAbilities = participants[1].who.AbilitiesInCombat();
        temporaryBuffs = new() { { 0, new() }, { 1, new() } };
        flyingMissiles = new();
        actions = new List<Action>();
        log = new();
        healthBars = new();
        resourceBars = new();
        Reset();
    }

    public static void NewBoard(List<Entity> enemies, Site area)
    {
        PlayEnemyLine(enemies.First().EnemyLine("Aggro"));
        board = new Board(6, 6, enemies, area);
        bufferBoard = new BufferBoard();
        foreach (var foo in board.participants)
            board.CallEvents(foo.who, new() { { "Trigger", "CombatBegin" } });
    }

    //Spawns a new board that is intended to be used as a playtest site for a specific ability
    public static void NewBoard(Ability testingAbility)
    {
        board = new Board(6, 6, new() { { testingAbility, 0 } });
        bufferBoard = new BufferBoard();
        if (testingAbility.events != null)
            foreach (var participant in board.participants)
            {
                if (participant == board.participants[board.spotlightFriendly[0]]) board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, { "IgnoreConditions", "Yes" }, { "Triggerer", "Effector" }, { "AbilityName", testingAbility.name } });
                else board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, { "IgnoreConditions", "Yes" }, { "Triggerer", "Other" }, { "AbilityName", testingAbility.name } });
            }

        //This line automatically closed the simulation once the ability is done testing.
        //It was deactivated to make the dev see the after effects of the ability.
        //board.actions.Add(() => { CloseDesktop("GameSimulation"); CDesktop.UnlockScreen(); });
    }

    #endregion

    //Turn counter
    public int turn;

    //Stores the results of the combat
    public CombatResults results;

    //Stores the actions and stats of the combat
    public CombatLog log;

    //Indicates how many bonus moves were awarded this turn for the current player. This value is used only in sound effects
    public int bonusTurnStreak;

    //Array of all elements on the board
    public int[,] field;

    //Reference to the window that contains the drawn board
    public Window window;

    //All of the combat participants of both teams
    public List<CombatParticipant> participants;

    //Health bars for all combat participants
    public Dictionary<int, FluidBar> healthBars;

    //Resource bars for all combat participants
    public Dictionary<int, Dictionary<string, FluidBar>> resourceBars;

    //Indicates whos turn it currently is
    public int whosTurn;

    //Tells whether the artificial time break for the enemy move was made already. For now it is used as an illusion that the enemy is thinking before making a move. It may not be useful in the future. We will see
    public bool breakForMove;

    //Indicates whether the enemy finished moving and whether turn can be switched to the player
    public bool finishedMoving;

    //List of all flying buffs that are docking in the player mana region
    public Dictionary<int, List<GameObject>> temporaryBuffs;

    //List of cooldowns that are currently on the participants
    public Dictionary<int, Dictionary<string, int>> cooldowns;

    //Queue of actions to do on the board and the combatants
    public List<Action> actions;

    //Are where the combat takes place
    public Site area;

    //
    public List<int> spotlightFriendly;

    //
    public List<int> spotlightEnemy;

    public Vector3 PortraitPosition(int participantIndex)
    {
        var team = participants[participantIndex].team;
        var team1 = CDesktop.windows.Find(x => x.title == "PlayerBattleInfo").LBRegionGroup().regions.Where(x => x.bigButtons.Count == 1).ToList();
        var team2 = CDesktop.windows.Find(x => x.title == "EnemyBattleInfo").LBRegionGroup().regions.Where(x => x.bigButtons.Count == 1).ToList();
        var offset = new Vector3(19, -19);
        if (team == 1) offset += new Vector3(152, 0);
        return offset + (team == 1 ? team1 : team2)[(team == 1 ? spotlightFriendly : spotlightEnemy).Count - 1 - (team == 1 ?spotlightFriendly : spotlightEnemy).IndexOf(participantIndex)].transform.position;
    }

    public void PutOnCooldown(int participant, Ability ability)
    {
        var list = cooldowns[participant];
        list.Remove(ability.name);
        if (ability.cooldown > 0)
            list.Add(ability.name, ability.cooldown);
    }

    public int CooldownOn(int participant, string ability) => cooldowns[participant].Get(ability);

    //Cools down all action bar abilities and used passives by 1 turn
    public int Cooldown(int participant)
    {
        int off = 0;
        var abilities = cooldowns[participant];
        var names = cooldowns[participant].Keys.ToList();
        foreach (var name in names)
        {
            if (abilities[name] > 0)
            {
                off++;
                if (--abilities[name] <= 0)
                {
                    abilities.Remove(name);
                    board.CallEvents(participants[participant].who, new() { { "Trigger", "Cooldown" }, { "Triggerer", "Effector" }, { "AbilityName", name } });
                }
            }
            else abilities.Remove(name);
        }
        return off;
    }

    public void CallEvents(Entity who, Dictionary<string, string> trigger)
    {
        if (who.inventory != null)
            foreach (var item in who.inventory.items.Concat(who.equipment.Select(x => x.Value)).ToList())
                if (item.abilities != null)
                    foreach (var ability in item.abilities.Select(x => (Ability.abilities.Find(y => y.name == x.Key), x.Value)))
                        ability.Item1.ExecuteEvents(this, trigger, item, ability.Value, participants.FindIndex(x => x.who == who));
        foreach (var ability in participants.Find(x => x.who == who).combatAbilities)
            ability.Key.ExecuteEvents(this, trigger, null, ability.Value, participants.FindIndex(x => x.who == who));
        foreach (var buff in who.buffs.ToList())
            buff.buff.ExecuteEvents(this, trigger, buff);
    }

    public CombatParticipant Target(int ofTeam) => participantTargetted != null ? participantTargetted : participants[ofTeam == 1 ? spotlightEnemy[0] : spotlightFriendly[0]];

    //Ends a turn for a participant and makes somebody else begin theirs
    public void EndTurn()
    {
        //Increase turns by one as we are entering another
        turn++;

        //Call events for the participant that was moving now with TurnEnd trigger
        CallEvents(participants[whosTurn].who, new() { { "Trigger", "TurnEnd" } });

        //Change turns to next participants until one of them isn't dead
        do { whosTurn++; whosTurn %= participants.Count; }
        while (participants[whosTurn].who.dead);

        //If the current participant is human controlled fade out the enemy cursor
        if (participants[whosTurn].human) cursorEnemy.fadeOut = true;

        //Otherwise fade it in so we can see how enemy moves
        else cursorEnemy.fadeIn = true;

        //Set status of finished moving to false because we have just began a new turn
        finishedMoving = false;

        //Cooldown all abilities of the current entity
        if (Cooldown(whosTurn) > 0)
            if (spotlightEnemy.Contains(whosTurn)) Respawn("EnemyBattleInfo");
            else if (spotlightFriendly.Contains(whosTurn)) Respawn("PlayerBattleInfo");

        //Call events for the turn begin
        CallEvents(participants[whosTurn].who, new() { { "Trigger", "TurnBegin" } });

        //Flare all of the buffs for the current participant
        board.actions.Add(() => participants[whosTurn].who.FlareBuffs());

        //If we made a full cycle and we are starting the loop again, remove one row on the bottom of the board
        if (whosTurn == 0)
        {
            for (int i = 0; i < field.GetLength(0); i++)
                field[i, field.GetLength(1) - 1] = -1;
            bufferBoard.Generate("IllegalFirstRow");
        }
    }

    //RESETS THE BOARD TO BE EMPTY AND REFILLED AGAIN
    public void Reset()
    {
        field = new int[field.GetLength(0), field.GetLength(1)];
        for (int i = 0; i < field.GetLength(0); i++)
            for (int j = 0; j < field.GetLength(1); j++)
                field[i, j] = -1;
        CDesktop.LockScreen();
    }

    public void UpdateHealthBars()
    {
        foreach (var foo in healthBars)
            foo.Value.UpdateFluidBar();
    }

    public void UpdateResourceBars(int forWho, List<string> elements)
    {
        if (resourceBars.ContainsKey(forWho))
            foreach (var foo in resourceBars[forWho])
                if (elements.Contains(foo.Key))
                    foo.Value.UpdateFluidBar();
    }

    public void EndCombat(string result)
    {
        cursorEnemy.fadeOut = true;
        if (result != "Quit")
        {
            CloseDesktop("Game");
            currentSave.AddTime(turn * 15);
            results = new CombatResults(result, area.zone, area.recommendedLevel);
            if (result == "Team1Won")
            {
                var progression = area.progression.FindAll(x => x.point == (currentSave.siteProgress.ContainsKey(area.name) ? currentSave.siteProgress[area.name] : 0));
                var nextProgression = area.progression.FindAll(x => x.point - 1 == (currentSave.siteProgress.ContainsKey(area.name) ? currentSave.siteProgress[area.name] : 0));
                var progBosses = progression.FindAll(x => x.type == "Boss");
                var nextProgBosses = nextProgression.FindAll(x => x.type == "Boss");

                //If you just defeated an enemy that wasn't a boss and none bosses block your way
                //in progression then increase your progression in the area by one point
                if (area != null && participants.First(x => x.team == 2).who.kind != "Elite" && progBosses.Count > 0 && progBosses.All(x => currentSave.elitesKilled.ContainsKey(x.bossName)) || progBosses.Count == 0)
                    if (!currentSave.siteProgress.ContainsKey(area.name)) currentSave.siteProgress.Add(area.name, 1);
                    else currentSave.siteProgress[area.name]++;
                foreach (var unlockArea in progression.FindAll(x => x.type == "Area"))
                    if (!currentSave.unlockedAreas.Contains(unlockArea.areaName) && progBosses.Count > 0 && progBosses.All(x => currentSave.elitesKilled.ContainsKey(x.bossName)))
                        if (!unlockArea.all) currentSave.unlockedAreas.Add(unlockArea.areaName);
                        else UnlockArea(unlockArea);
                foreach (var unlockArea in nextProgression.FindAll(x => x.type == "Area"))
                    if (!currentSave.unlockedAreas.Contains(unlockArea.areaName) && nextProgBosses.Count == 0)
                        if (!unlockArea.all) currentSave.unlockedAreas.Add(unlockArea.areaName);
                        else UnlockArea(unlockArea);

                void UnlockArea(AreaProgression unlockArea)
                {
                    var temp = SiteInstance.instance.wings.SelectMany(x => x.areas).Select(x => areas.Find(y => y.name == x["AreaName"]));
                    var foo = temp.Select(x => (x.name, x.progression.Find(y => y.areaName == unlockArea.areaName))).ToList();
                    foo.RemoveAll(x => x.Item2 == null);
                    bool unlock = true;
                    foreach (var a in foo)
                    {
                        var elite = temp.First(y => y.name == a.name).progression.Find(x => x.type == "Boss" && x.point == a.Item2.point);
                        if (!currentSave.siteProgress.ContainsKey(a.name)) unlock = false;
                        else if (elite != null) { if (elite != null && (a.Item2.point > currentSave.siteProgress[a.name] || a.Item2.point == currentSave.siteProgress[a.name] && !currentSave.elitesKilled.ContainsKey(elite.bossName))) unlock = false; }
                        else if (elite == null) if (a.Item2.point > currentSave.siteProgress[a.name]) unlock = false;
                        if (!unlock) break;
                    }
                    if (unlock) currentSave.unlockedAreas.Add(unlockArea.areaName);
                }

                //Add +1 to the amount of times you defeated this enemy
                //Depending on the rarity of the enemy add +1 to the right list
                foreach (var defeatedEnemy in participants.Where(x => x.team == 2))
                {
                    var enemy = defeatedEnemy.who;
                    var enemyRace = Race.races.Find(x => x.name == enemy.race);

                    //Increase defeated amounts of enemies in your party
                    switch (defeatedEnemy.who.kind)
                    {
                        case "Common": currentSave.commonsKilled.Inc(defeatedEnemy.who.name); break;
                        case "Rare": currentSave.raresKilled.Inc(defeatedEnemy.who.name); break;
                        case "Elite": currentSave.elitesKilled.Inc(defeatedEnemy.who.name); break;
                    }

                    //Drop items
                    var directDrop = enemyRace.droppedItems.Select(x => Item.items.Find(y => y.name == x)).Where(x => !x.unique || !currentSave.player.uniquesGotten.Contains(x.name)).Where(x => x.specDropRestriction == null || x.specDropRestriction.Contains(participants[0].who.race) || x.specDropRestriction.Contains(participants[0].who.race)).Where(x => x.raceDropRestriction == null || x.raceDropRestriction.Contains(participants[0].who.race)).ToList();
                    var wearableDirect = directDrop.Where(x => x.IsWearable()).ToList();
                    var equipableDirect = wearableDirect.Where(x => x.CanEquip(currentSave.player)).ToList();

                    //One wearable item drop with priority on something you can equip
                    if (equipableDirect.Count > 0)
                    {
                        var item = equipableDirect[random.Next(equipableDirect.Count)];
                        results.inventory.AddItem(item.CopyItem());
                    }
                    else if (wearableDirect.Count > 0)
                    {
                        var item = wearableDirect[random.Next(wearableDirect.Count)];
                        results.inventory.AddItem(item.CopyItem());
                    }
                    else
                    {
                        var worldDrop = Item.items.FindAll(x => (x.dropRange == null && x.lvl >= enemy.level - 6 && x.lvl <= enemy.level || x.dropRange != null && enemy.level >= int.Parse(x.dropRange.Split('-')[0]) && enemy.level <= int.Parse(x.dropRange.Split('-')[1])) && x.source == "RareDrop");
                        var instance = area.instancePart ? SiteInstance.instances.Find(x => x.wings.Any(y => y.areas.Any(z => z["AreaName"] == area.name))) : null;
                        var zoneDrop = instance == null || instance.zoneDrop == null ? new() : Item.items.FindAll(x => instance.zoneDrop.Contains(x.name));
                        var everything = zoneDrop.Concat(worldDrop).Where(x => x.CanEquip(currentSave.player) && (!x.unique || !currentSave.player.uniquesGotten.Contains(x.name)));
                        var dropGray = everything.Where(x => x.rarity == "Poor").ToList();
                        var dropWhite = everything.Where(x => x.rarity == "Common").ToList();
                        var dropGreen = everything.Where(x => x.rarity == "Uncommon").ToList();
                        var dropBlue = everything.Where(x => x.rarity == "Rare").ToList();
                        var dropPurple = everything.Where(x => x.rarity == "Epic").ToList();
                        if (dropPurple.Count > 0 && Roll(0.05))
                            results.inventory.AddItem(dropPurple[random.Next(dropPurple.Count)].CopyItem());
                        else if (dropBlue.Count > 0 && Roll(1))
                            results.inventory.AddItem(dropBlue[random.Next(dropBlue.Count)].CopyItem());
                        else if (dropGreen.Count > 0 && (enemy.kind != "Common" || Roll(8)))
                            results.inventory.AddItem(dropGreen[random.Next(dropGreen.Count)].CopyItem());
                        else if (dropWhite.Count > 0 && (enemy.kind != "Common" || Roll(5)))
                            results.inventory.AddItem(dropWhite[random.Next(dropWhite.Count)].CopyItem());
                        else if (dropGray.Count > 0 && (enemy.kind != "Common" || Roll(3)))
                            results.inventory.AddItem(dropGray[random.Next(dropGray.Count)].CopyItem());
                    }

                    //All the other guaranteed items
                    var otherDirect = directDrop.Except(wearableDirect).ToList();
                    if (otherDirect.Count > 0)
                        foreach (var item in otherDirect)
                            results.inventory.AddItem(item.CopyItem());

                    //General drops
                    var generalDrops = GeneralDrop.generalDrops.FindAll(x => x.DoesLevelFit(enemy.level) && (x.requiredProfession == null || (participants[0].who.professionSkills.ContainsKey(x.requiredProfession) && (x.requiredSkill == 0 || x.requiredSkill <= participants[0].who.professionSkills[x.requiredProfession].Item1))) && (x.category == null || x.category == enemy.Race().category) && x.inclusive);
                    if (generalDrops.Count > 0)
                        foreach (var drop in generalDrops)
                            if (Roll(drop.rarity))
                            {
                                int amount = 1;
                                for (int i = 1; i < drop.dropCount; i++) amount += Roll(10) ? 1 : 0;
                                results.inventory.AddItem(Item.items.Find(x => x.name == drop.item).CopyItem(amount));
                            }
                    var possibleGeneralDrops = GeneralDrop.generalDrops.FindAll(x => x.DoesLevelFit(enemy.level) && (x.requiredProfession == null || (participants[0].who.professionSkills.ContainsKey(x.requiredProfession) && (x.requiredSkill == 0 || x.requiredSkill <= participants[0].who.professionSkills[x.requiredProfession].Item1))) && (x.category == null || x.category == enemy.Race().category) && !x.inclusive);
                    possibleGeneralDrops.Shuffle();
                    if (possibleGeneralDrops.Count > 0)
                        foreach (var drop in possibleGeneralDrops.OrderBy(x => x.rarity))
                            if (Roll(drop.rarity))
                            {
                                int amount = 1;
                                for (int i = 1; i < drop.dropCount; i++) amount += Roll(50) ? 1 : 0;
                                results.inventory.AddItem(Item.items.Find(x => x.name == drop.item).CopyItem(amount));
                                break;
                            }
                    results.inventory.items.ForEach(x => x.SetRandomEnchantment());
                    foreach (var item in results.inventory.items)
                        if (item.unique && !currentSave.player.uniquesGotten.Contains(item.name))
                            currentSave.player.uniquesGotten.Add(item.name);
                }

                //Exit board view
                if (area != null && area.instancePart) SwitchDesktop("Instance");
                else if (area != null && area.complexPart) SwitchDesktop("Complex");
                else SwitchDesktop("HostileArea");
                CDesktop.RespawnAll();

                //Grant experience for defeating the enemy
                foreach (var winParticipant in participants.Where(x => x.team == 1))
                    foreach (var lossParticipant in participants.Where(x => x.team == 2))
                    {
                        var enemy = lossParticipant.who;
                        var enemyRace = Race.races.Find(x => x.name == enemy.race);
                        if (winParticipant.who.WillGetExperience(enemy.level) && winParticipant.who.level < defines.maxPlayerLevel)
                        {
                            float amount = winParticipant.who.ExperienceForEqualEnemy();
                            if (Coloring.ColorEntityLevel(enemy.level) == "Green") amount *= 0.5f;
                            else if (Coloring.ColorEntityLevel(enemy.level) == "DarkGray") amount *= 0;
                            if (enemyRace.kind == "Elite") amount *= 2;
                            else if (enemyRace.kind == "Rare") amount *= 1.5f;
                            results.experience.Inc(winParticipant.who, (int)amount);
                        }
                        else results.experience.Inc(winParticipant.who, 0);
                    }

                //Open default page in the chart
                chartPage = "Damage Dealt";
                SpawnDesktopBlueprint("CombatResults");

                //Grant experience to all winning participants
                foreach (var winParticipant in participants.Where(x => x.team == 1))
                    winParticipant.who.ReceiveExperience(board.results.experience[winParticipant.who]);

                //Progress quests requiring killing
                foreach (var winParticipant in participants.Where(x => x.team == 1))
                    foreach (var lossParticipant in participants.Where(x => x.team == 2))
                        if (winParticipant.who.currentQuests != null)
                        {
                            var enemy = lossParticipant.who;
                            var enemyRace = Race.races.Find(x => x.name == enemy.race);
                            var output = enemy.name + ": ";
                            foreach (var quest in winParticipant.who.currentQuests)
                                foreach (var con in quest.conditions)
                                    if (con.type == "Kill" && con.name == enemy.name)
                                    {
                                        foreach (var site in con.Where())
                                            if (!Quest.sitesToRespawn.Contains(site))
                                                Quest.sitesToRespawn.Add(site);
                                        if (con.amountDone < con.amount)
                                        {
                                            if (output.EndsWith(" ")) output += con.amountDone + 1 + "/" + con.amount;
                                            else output += ", " + con.amountDone + 1 + "/" + con.amount;
                                        }
                                        var end = Site.FindSite(x => x.name == quest.siteEnd);
                                        if (!Quest.sitesToRespawn.Contains(end)) Quest.sitesToRespawn.Add(end);
                                    }
                            winParticipant.who.QuestKill(enemy.name);
                            if (!output.EndsWith(" ")) SpawnFallingText(new Vector2(0, -72), output, "Yellow");
                        }
            }
            else if (result == "Team2Won")
            {
                StopAmbience();
                PlaySound("Death");
                if (Realm.realms.Find(x => x.name == settings.selectedRealm).hardcore)
                {
                    var enemy = participants.Find(x => x.team == 2).who;
                    currentSave.deathInfo = new(enemy.name, enemy.Race().kind == "Common", area.name);
                }
                else
                {
                    SwitchDesktop("Map");
                    SpawnTransition(false);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    SpawnTransition(false);
                    SpawnTransition(false);
                }
                chartPage = "Damage Dealt";
                SpawnDesktopBlueprint("CombatResults");
            }
            else if (result == "Fled")
            {
                PlaySound("RunAwayBitch");
                if (area != null && area.instancePart) SwitchDesktop("Instance");
                else if (area != null && area.complexPart) SwitchDesktop("Complex");
                else SwitchDesktop("HostileArea");
                Respawn("MapToolbarClockLeft");
                Respawn("MapToolbarClockRight");
            }
        }
        else CloseDesktop("GameSimulation");
        if (CDesktop.screenLocked)
            CDesktop.UnlockScreen();
    }

    public void AnimateBoard()
    {
        //MOVE ELEMENTS DOWN WITH GRAVITY
        for (int j = field.GetLength(1) - 2; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] != -1)
                {
                    var empty = 0;
                    for (int q = 0; q + j < field.GetLength(1); q++)
                        if (field[i, j + q] == -1) empty++;
                    (field[i, j], field[i, j + empty]) = (-1, field[i, j]);
                    if (empty > 0) window.LBRegionGroup().regions[j].bigButtons[i].gameObject.AddComponent<FallingElement>().Initiate(empty, 0);
                }

        //IF BOARD IS NOT YET FULL RETURN AND DO PREVIOUS STEPS AGAIN
        for (int j = field.GetLength(1) - 1; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == -1)
                {
                    bufferBoard.FillBoard(field);
                    return;
                }

        //DO ONE SCHEDULED ACTION AND RETURN AFTER TO DO ONE AT A TIME
        if (actions.Count > 0)
        {
            actions[0]();
            actions.RemoveAt(0);
            return;
        }

        //CASCADE FOR CURRENT PLAYER
        for (int j = 0; j < field.GetLength(1); j++)
            for (int i = 0; i < field.GetLength(0); i++)
            {
                var list = FloodCount(i, j);
                if (list.Count >= defines.cascadeMinimum)
                {
                    FloodDestroy(list);
                    return;
                }
            }

        //If all friendly entities died or the player died while on hardcore..
        if ((participants.Where(x => x.team == 1).All(x => x.who.dead) || Realm.realms.Find(x => x.name == settings.selectedRealm).hardcore && participants[0].who.health <= 0) && window.desktop.title == "Game")
            EndCombat("Team2Won");

        //If all enemiesdied..
        else if (participants.Where(x => x.team == 2).All(x => x.who.dead) && window.desktop.title == "Game")
            EndCombat("Team1Won");

        //IF IT'S ENEMY'S TURN..
        else if (!participants[whosTurn].human)
        {
            //IF ENEMY FINISHED MOVING END THEIR TURN
            if (finishedMoving)
            {
                //UNLESS THEY SCORED A BONUS MOVE
                if (bonusTurnStreak > 0)
                {
                    bonusTurnStreak = 0;
                    finishedMoving = false;
                }
                else EndTurn();
            }

            //Do a moment of waiting for the enemy to "think"
            else if (!breakForMove)
            {
                animationTime = (float)(random.Next(3, 5) / 10.0) + 0.3f;
                breakForMove = true;
            }

            //IF ENEMY WAS ALREADY ON THINKING BREAK MAKE THEM MOVE
            else
            {
                breakForMove = false;
                bonusTurnStreak = 0;
                var differentFloodings = PossibleFloodings();
                var boardClick = (0, 0);
                var castAbility = "";
                var allAbilities = participants[whosTurn].who.abilityAITags == null ? new() : participants[whosTurn].who.abilityAITags.Select(x => (Ability.abilities.Find(y => y.name == x.Key), x.Value)).ToList();
                var emergencies = allAbilities.Where(x => x.Value == "Emergency").Select(x => x.Item1);
                var castableEmergencies = emergencies.Where(x => x.EnoughResources(participants[whosTurn].who) && CooldownOn(whosTurn, x.name) == 0);
                var cores = allAbilities.Where(x => x.Value == "Core").Select(x => x.Item1);
                var castableCores = cores.Where(x => x.EnoughResources(participants[whosTurn].who) && CooldownOn(whosTurn, x.name) == 0);
                var fillers = allAbilities.Where(x => x.Value == "Filler").Select(x => x.Item1);
                var castableFillers = fillers.Where(x => x.EnoughResources(participants[whosTurn].who) && CooldownOn(whosTurn, x.name) == 0);
                if (emergencies.Count() > 0 && participants[whosTurn].who.health < participants[whosTurn].who.MaxHealth() / (2.6 - random.NextDouble()))
                    if (castableEmergencies.Count() > 0)
                        castAbility = castableEmergencies.OrderByDescending(x => x.cooldown).First().name;
                if (cores.Count() > 0 && castAbility == "")
                    if (castableCores.Count() > 0)
                        castAbility = castableCores.OrderByDescending(x => x.cooldown).First().name;
                if (fillers.Count() > 0 && castAbility == "")
                    if (castableFillers.Count() > 0)
                        castAbility = castableFillers.OrderByDescending(x => x.cooldown).First().name;
                if (castAbility == "")
                {
                    var importanceTable = participants[whosTurn].who.resources.ToDictionary(x => x.Key, x => 1);
                    foreach (var resource in participants[whosTurn].who.resources)
                    {
                        var abilitiesUsingResource = allAbilities.Where(x => x.Item1.cost.ContainsKey(resource.Key)).OrderBy(x => x.Value == "Filler" ? 1 : 0);
                        var currentImportance = 1;
                        var subtractedAlready = 0;
                        foreach (var a in abilitiesUsingResource)
                            for (int i = 0; i < a.Item1.cost[resource.Key]; i++)
                                if (++subtractedAlready > resource.Value)
                                    currentImportance += a.Value == "Filler" ? 4 : 10;
                        importanceTable[resource.Key] = currentImportance;
                    }
                    differentFloodings.Shuffle();
                    var orderedFloodings = differentFloodings.OrderByDescending(x => x.Item3.Sum(y => (y.Item3 / 10 + 1) * importanceTable[Resource(y.Item3)])).ToList();
                    var chosenFlooding = orderedFloodings.First();
                    boardClick = (chosenFlooding.Item1, chosenFlooding.Item2);
                }
                if (castAbility != "")
                {
                    var abilityObj = Ability.abilities.Find(x => x.name == castAbility);
                    board.actions.Add(() =>
                    {
                        var temp = CDesktop.windows.Find(x => x.title == "EnemyBattleInfo").regionGroups[0].regions;
                        var whereToStart = 0;
                        if (spotlightEnemy.IndexOf(whosTurn) != spotlightEnemy.Count - 1)
                            for (int i = 0, counted = 0; i < temp.Count; i++, whereToStart++)
                                if (temp[i].currentHeight == 0 && ++counted / 2 == spotlightEnemy.Count - 1 - spotlightEnemy.IndexOf(whosTurn)) break;
                        if (whereToStart > 0) whereToStart++;
                        cursorEnemy.Move(temp[whereToStart + 2 + participants[whosTurn].who.actionBars[participants[whosTurn].who.currentActionSet].IndexOf(abilityObj.name)].transform.position + new Vector3(139, -10));
                        animationTime += defines.frameTime * 9;
                    });
                    board.actions.Add(() => { cursorEnemy.SetCursor(CursorType.Click); });
                    board.actions.Add(() =>
                    {
                        cursorEnemy.SetCursor(CursorType.Default);
                        var temp = CDesktop.windows.Find(x => x.title == "EnemyBattleInfo").regionGroups[0].regions;
                        var whereToStart = 0;
                        if (spotlightEnemy.IndexOf(whosTurn) != spotlightEnemy.Count - 1)
                            for (int i = 0, counted = 0; i < temp.Count; i++, whereToStart++)
                                if (temp[i].currentHeight == 0 && ++counted / 2 == spotlightEnemy.Count - 1 - spotlightEnemy.IndexOf(whosTurn)) break;
                        if (whereToStart > 0) whereToStart++;
                        AddRegionOverlay(temp[whereToStart + 2 + participants[whosTurn].who.actionBars[participants[whosTurn].who.currentActionSet].IndexOf(abilityObj.name)], "Window", 10f);
                        animationTime += defines.frameTime;
                        foreach (var participant in participants)
                        {
                            if (participant == participants[whosTurn]) board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, { "Triggerer", "Effector" }, { "AbilityName", abilityObj.name } });
                            board.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, { "Triggerer", "Other" }, { "AbilityName", abilityObj.name } });
                        }
                        board.participants[whosTurn].who.DetractResources(abilityObj.cost);
                    });
                }
                else
                {
                    board.actions.Add(() => { cursorEnemy.Move(window.LBRegionGroup().regions[boardClick.Item2].bigButtons[boardClick.Item1].transform.position); animationTime += defines.frameTime * 8; });
                    board.actions.Add(() => { cursorEnemy.SetCursor(CursorType.Click); });
                    board.actions.Add(() =>
                    {
                        cursorEnemy.SetCursor(CursorType.Default);
                        var list1 = board.FloodCount(boardClick.Item1, boardClick.Item2);
                        board.FloodDestroy(list1);
                        board.finishedMoving = true;
                    });
                }
            }
        }

        //IF IT's PLAYER'S TURN..
        else
        {
            //IF PLAYER FINISHED MOVING TURN END THEIR TURN
            if (finishedMoving)

                //UNLESS THEY SCORED A BONUS MOVE
                if (bonusTurnStreak > 0)
                {
                    bonusTurnStreak = 0;
                    finishedMoving = false;
                    canUnlockScreen = true;
                }
                else EndTurn();

            //IF PLAYER IS STILL GOING TO MOVE UNLOCK THE SCREEN
            else canUnlockScreen = true;
        }
    }

    //Returns a list of all possible moves on the board and it's effects
    //The returned list doesn't contain any duplicates
    public List<(int, int, List<(int, int, int)>)> PossibleFloodings()
    {
        var differentFloodings = new List<(int, int, List<(int, int, int)>)>();
        for (int i = 0; i < field.GetLength(0); i++)
            for (int j = 0; j < field.GetLength(1); j++)
                if (field[i, j] != -1)
                {
                    var list2 = FloodCount(i, j);
                    if (!differentFloodings.Exists(x => x.Item3.All(y => list2.Contains((y.Item1, y.Item2, y.Item3)))))
                        differentFloodings.Add((i, j, list2));
                }
        return differentFloodings;
    }

    //Destroys elements on the coordinates provided in the list
    public void FloodDestroy(List<(int, int, int)> list)
    {
        PlaySound(collectSoundDictionary[list[0].Item3].ToString(), 0.5f);
        if (list.Count > 3)
        {
            bonusTurnStreak++;
            PlaySound("BonusMove" + (bonusTurnStreak > 4 ? 4 : bonusTurnStreak), 0.6f);
            SpawnFallingText(new Vector2(0, 34), "Bonus Move", "White");
        }
        var types = list.Select(x => x.Item3 % 10).Distinct();
        var foo = types.ToDictionary(x => Resource(x), x => 0);
        foreach (var a in list)
        {
            var r = Resource(a.Item3 % 10);
            SpawnShatter(1, 9, window.LBRegionGroup().regions[a.Item2].bigButtons[a.Item1].transform.position + new Vector3(-17.5f, -17.5f), boardButtonDictionary[a.Item3], participants[whosTurn].who.resources[r] + foo[r] < participants[whosTurn].who.MaxResource(r));
            foo.Inc(r, a.Item3 / 10 + 1);
            field[a.Item1, a.Item2] = -1;
        }
        participants[whosTurn].who.AddResources(foo);
        bufferBoard.Generate();
        CDesktop.LockScreen();
    }

    public string Resource(int id)
    {
        if (id % 10 == 1) return "Earth";
        else if (id % 10 == 2) return "Fire";
        else if (id % 10 == 3) return "Water";
        else if (id % 10 == 4) return "Air";
        else if (id % 10 == 5) return "Lightning";
        else if (id % 10 == 6) return "Frost";
        else if (id % 10 == 7) return "Decay";
        else if (id % 10 == 8) return "Arcane";
        else if (id % 10 == 9) return "Order";
        else if (id % 10 == 0) return "Shadow";
        else return "None";
    }

    public int ResourceReverse(string element)
    {
        if (element == "Earth") return 1;
        else if (element == "Fire") return 2;
        else if (element == "Water") return 3;
        else if (element == "Air") return 4;
        else if (element == "Lightning") return 5;
        else if (element == "Frost") return 6;
        else if (element == "Decay") return 7;
        else if (element == "Arcane") return 8;
        else if (element == "Order") return 9;
        else if (element == "Shadow") return 0;
        else return -1;
    }

    //Returns a counted list of elements that can be collected
    //together when targeting an element located on the provided coords
    public List<(int, int, int)> FloodCount(int x, int y)
    {
        var visited = new List<(int, int)>();
        var positives = new List<(int, int, int)>();
        Flood(x, y);
        return positives;

        void Flood(int i, int j)
        {
            if (visited.Contains((i, j))) return;
            visited.Add((i, j));
            if (field[i, j] % 10 != field[x, y] % 10 || positives.Contains((i, j, field[i, j]))) return;
            positives.Add((i, j, field[i, j]));
            if (i > 0) Flood(i - 1, j);
            if (j > 0) Flood(i, j - 1);
            if (i < field.GetLength(0) - 1) Flood(i + 1, j);
            if (j < field.GetLength(1) - 1) Flood(i, j + 1);
        }
    }

    public static Dictionary<int, string> boardNameDictionary = new()
    {
        { -1, "None" },
        { 00, "Rousing Shadow" },
        { 01, "Rousing Earth" },
        { 02, "Rousing Fire" },
        { 03, "Rousing Water" },
        { 04, "Rousing Air" },
        { 05, "Rousing Lightning" },
        { 06, "Rousing Frost" },
        { 07, "Rousing Decay" },
        { 08, "Rousing Arcane" },
        { 09, "Rousing Order" },
        { 10, "Awakened Shadow" },
        { 11, "Awakened Earth" },
        { 12, "Awakened Fire" },
        { 13, "Awakened Water" },
        { 14, "Awakened Air" },
        { 15, "Awakened Lightning" },
        { 16, "Awakened Frost" },
        { 17, "Awakened Decay" },
        { 18, "Awakened Arcane" },
        { 19, "Awakened Order" },
        { 20, "Soul of Shadow" },
        { 21, "Soul of Earth" },
        { 22, "Soul of Fire" },
        { 23, "Soul of Water" },
        { 24, "Soul of Air" },
        { 25, "Soul of Lightning" },
        { 26, "Soul of Frost" },
        { 27, "Soul of Decay" },
        { 28, "Soul of Arcane" },
        { 29, "Soul of Order" },
    };

    //IDs of specific elements possible on the board
    public static Dictionary<int, string> boardButtonDictionary = new()
    {
        { -1, null },
        { 00, "ElementShadowRousing" },
        { 01, "ElementEarthRousing" },
        { 02, "ElementFireRousing" },
        { 03, "ElementWaterRousing" },
        { 04, "ElementAirRousing" },
        { 05, "ElementLightningRousing" },
        { 06, "ElementFrostRousing" },
        { 07, "ElementDecayRousing" },
        { 08, "ElementArcaneRousing" },
        { 09, "ElementOrderRousing" },
        { 10, "ElementShadowAwakened" },
        { 11, "ElementEarthAwakened" },
        { 12, "ElementFireAwakened" },
        { 13, "ElementWaterAwakened" },
        { 14, "ElementAirAwakened" },
        { 15, "ElementLightningAwakened" },
        { 16, "ElementFrostAwakened" },
        { 17, "ElementDecayAwakened" },
        { 18, "ElementArcaneAwakened" },
        { 19, "ElementOrderAwakened" },
        { 20, "ElementShadowSoul" },
        { 21, "ElementEarthSoul" },
        { 22, "ElementFireSoul" },
        { 23, "ElementWaterSoul" },
        { 24, "ElementAirSoul" },
        { 25, "ElementLightningSoul" },
        { 26, "ElementFrostSoul" },
        { 27, "ElementDecaySoul" },
        { 28, "ElementArcaneSoul" },
        { 29, "ElementOrderSoul" },
    };

    public static Dictionary<int, string> collectSoundDictionary = new()
    {
        { -1, "" },
        { 00, "ElementShadow" },
        { 01, "ElementEarth" },
        { 02, "ElementFire" },
        { 03, "ElementWater" },
        { 04, "ElementAir" },
        { 05, "ElementLightning" },
        { 06, "ElementFrost" },
        { 07, "ElementDecay" },
        { 08, "ElementArcane" },
        { 09, "ElementOrder" },
        { 10, "ElementShadow" },
        { 11, "ElementEarth" },
        { 12, "ElementFire" },
        { 13, "ElementWater" },
        { 14, "ElementAir" },
        { 15, "ElementLightning" },
        { 16, "ElementFrost" },
        { 17, "ElementDecay" },
        { 18, "ElementArcane" },
        { 19, "ElementOrder" },
        { 20, "ElementShadow" },
        { 21, "ElementEarth" },
        { 22, "ElementFire" },
        { 23, "ElementWater" },
        { 24, "ElementAir" },
        { 25, "ElementLightning" },
        { 26, "ElementFrost" },
        { 27, "ElementDecay" },
        { 28, "ElementArcane" },
        { 29, "ElementOrder" },
    };

    public static Board board;
}
