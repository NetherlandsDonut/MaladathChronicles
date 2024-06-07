using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using static Root;
using static Sound;
using static Defines;
using static SaveGame;
using static BufferBoard;
using static GameSettings;
using static CursorRemote;
using static FlyingElement;
using static SiteHostileArea;

public class Board
{
    #region Initialisation

    public Board(int x, int y, Entity enemy, SiteHostileArea area = null)
    {
        field = new int[x, y];
        player = currentSave.player;
        player.InitialiseCombat();
        this.enemy = enemy;
        playerTurn = true;
        this.area = area;
        playerCombatAbilities = player.AbilitiesInCombat();
        playerCooldowns = new();
        enemyCombatAbilities = this.enemy.AbilitiesInCombat();
        enemyCooldowns = new();
        temporaryElementsPlayer = new();
        temporaryElementsEnemy = new();
        temporaryBuffsPlayer = new();
        temporaryBuffsEnemy = new();
        flyingMissiles = new();
        actions = new List<Action>();
        log = new();
        healthBars = new();
        resourceBars = new();
        Reset();
    }

    public Board(int x, int y, Dictionary<Ability, int> abilities)
    {
        field = new int[x, y];
        player = new Entity(60, null);
        player.InitialiseCombat();
        enemy = new Entity(60, null);
        playerTurn = true;
        area = areas[random.Next(areas.Count)];
        playerCombatAbilities = abilities;
        playerCooldowns = new();
        enemyCombatAbilities = abilities;
        enemyCooldowns = new();
        temporaryElementsPlayer = new();
        temporaryElementsEnemy = new();
        temporaryBuffsPlayer = new();
        temporaryBuffsEnemy = new();
        flyingMissiles = new();
        actions = new List<Action>();
        log = new();
        healthBars = new();
        resourceBars = new();
        Reset();
    }

    public static void NewBoard(Entity entity, SiteHostileArea area)
    {
        board = new Board(6, 6, entity, area);
        bufferBoard = new BufferBoard();
        board.CallEvents(board.player, new() { { "Trigger", "CombatBegin" } });
        board.CallEvents(board.enemy, new() { { "Trigger", "CombatBegin" } });
    }

    //Spawns a new board that is intended to be used as a playtest site for a specific ability
    public static void NewBoard(Ability testingAbility)
    {
        board = new Board(6, 6, new() { { testingAbility, 0 } });
        bufferBoard = new BufferBoard();
        if (testingAbility.events != null)
            board.CallEvents(board.enemy, new() { { "Trigger", "AbilityCast" }, { "AbilityName", testingAbility.name }, { "Triggerer", "Effector" } });

        //This line automatically closed the simulation once the ability is done testing.
        //It was deactivated to make the dev see the after effects of the ability.
        //board.actions.Add(() => { CloseDesktop("GameSimulation"); CDesktop.UnlockScreen(); });
    }

    #endregion

    //STATIC REFERENCE TO THE BOARD
    //THERE CAN BE ONLY ONE AT A TIME THANKS TO STATIC REF
    public static Board board;

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

    //Player and enemy references for the combat. Player is always on the left side of the screen.
    public Entity player, enemy;

    //Health bars for player and the enemy
    public Dictionary<string, FluidBar> healthBars;

    //Resource bars for player and the enemy
    public Dictionary<string, Dictionary<string, FluidBar>> resourceBars;

    //Indicates whether it's currently the player's turn
    public bool playerTurn;

    //Tells whether the artificial time break for the enemy move was made already. For now it is used as an illusion that the enemy is thinking before making a move. It may not be useful in the future. We will see
    public bool breakForEnemy;

    //Indicates whether the enemy finished moving and whether turn can be switched to the player
    public bool enemyFinishedMoving;

    //Indicates whether the player finished moving and whether the turn can be switched to the enemy
    public bool playerFinishedMoving;

    //List of all flying elements that are docking in the player mana region
    public List<GameObject> temporaryElementsPlayer;

    //List of all flying elements that are docking in the enemy mana region
    public List<GameObject> temporaryElementsEnemy;

    //List of all flying buffs that are docking in the player buff region
    public List<GameObject> temporaryBuffsPlayer;

    //List of all flying buffs that are docking in the player mana region
    public List<GameObject> temporaryBuffsEnemy;

    //Abilities (Active and passive) that player has in the combat
    public Dictionary<Ability, int> playerCombatAbilities;

    //List of passive abilities owned by player that are on cooldown
    public Dictionary<string, int> playerCooldowns;

    //Abilities (Active and passive) that enemy has in the combat
    public Dictionary<Ability, int> enemyCombatAbilities;

    //List of passive abilities owned by the enemy that are on cooldown
    public Dictionary<string, int> enemyCooldowns;

    //Queue of actions to do on the board and the combatants
    public List<Action> actions;

    //Are where the combat takes place
    public SiteHostileArea area;

    public void PutOnCooldown(bool player, Ability ability)
    {
        var list = player ? playerCooldowns : enemyCooldowns;
        list.Remove(ability.name);
        list.Add(ability.name, ability.cooldown);
    }

    public int CooldownOn(bool player, string ability) => (player ? playerCooldowns : enemyCooldowns).Get(ability);

    //Cooldowns all action bar abilities and used passives by 1 turn
    public int Cooldown(bool player)
    {
        int off = 0;
        ref var abilities = ref (player ? ref playerCooldowns : ref enemyCooldowns);
        var names = (player ? playerCooldowns : enemyCooldowns).Keys.ToList();
        foreach (var name in names)
        {
            if (abilities[name] > 0)
            {
                off++;
                if (--abilities[name] <= 0)
                {
                    abilities.Remove(name);
                    board.CallEvents(player ? this.player : enemy, new() { { "Trigger", "Cooldown" }, { "Triggerer", "Effector" }, { "AbilityName", name } });
                }
            }
            else abilities.Remove(name);
        }
        return off;
    }

    public void CallEvents(Entity entity, Dictionary<string, string> trigger)
    {
        foreach (var ability in entity == player ? playerCombatAbilities : enemyCombatAbilities)
            ability.Key.ExecuteEvents(this, null, trigger, ability.Value, entity == player);
        foreach (var buff in entity.buffs)
            buff.Item1.ExecuteEvents(this, null, trigger, buff);
    }

    //ENDS THE CURRENT PLAYER'S TURN
    public void EndTurn()
    {
        if (playerTurn)
        {
            CallEvents(player, new() { { "Trigger", "TurnEnd" } });
            cursorEnemy.fadeIn = true;
            playerTurn = false;
            playerFinishedMoving = false;
            if (Cooldown(false) > 0) Respawn("EnemyBattleInfo");
            CallEvents(enemy, new() { { "Trigger", "TurnBegin" } });
            enemy.FlareBuffs();
        }
        else
        {
            CallEvents(enemy, new() { { "Trigger", "TurnEnd" } });
            cursorEnemy.fadeOut = true;
            playerTurn = true;
            enemyFinishedMoving = false;
            if (Cooldown(true) > 0) Respawn("PlayerBattleInfo");
            CallEvents(player, new() { { "Trigger", "TurnBegin" } });
            player.FlareBuffs();
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

    public void UpdateResourceBars(string forWho, List<string> elements)
    {
        foreach (var foo in resourceBars[forWho])
            if (elements.Contains(foo.Key))
                foo.Value.UpdateFluidBar();
    }

    public void EndCombat(string result)
    {
        cursorEnemy.fadeOut = true;
        CloseDesktop("Game");
        results = new CombatResults(result);
        if (result == "Won")
        {
            var enemyRace = Race.races.Find(x => x.name == enemy.race);
            if (currentSave.player.WillGetExperience(enemy.level) && currentSave.player.level < defines.maxPlayerLevel)
            {
                float amount = currentSave.player.ExperienceForEqualEnemy();
                if (Coloring.ColorEntityLevel(enemy.level) == "Green") amount *= 0.5f;
                else if (Coloring.ColorEntityLevel(enemy.level) == "DarkGray") amount *= 0;
                if (enemyRace.kind == "Elite") amount *= 2;
                else if (enemyRace.kind == "Rare") amount *= 1.5f;
                results.experience = (int)amount;
            }
            var progression = area.progression.FindAll(x => x.point == (currentSave.siteProgress.ContainsKey(area.name) ? currentSave.siteProgress[area.name] : 0));
            var nextProgression = area.progression.FindAll(x => x.point - 1 == (currentSave.siteProgress.ContainsKey(area.name) ? currentSave.siteProgress[area.name] : 0));
            var progBosses = progression.FindAll(x => x.type == "Boss");
            var nextProgBosses = nextProgression.FindAll(x => x.type == "Boss");

            //If you just defeated an enemy that wasn't a boss and none bosses block your way
            //in progression then increase your progression in the area by one point
            if (area != null && enemy.kind != "Elite" && progBosses.Count > 0 && progBosses.All(x => currentSave.elitesKilled.ContainsKey(x.bossName)) || progBosses.Count == 0)
            {
                if (!currentSave.siteProgress.ContainsKey(area.name))
                    currentSave.siteProgress.Add(area.name, 1);
                else currentSave.siteProgress[area.name]++;
            }

            //Make progress on quests requiring you to kill certain enemies
            player.QuestKill(enemy.name);

            //Add +1 to the amount of times you defeated this enemy
            //Depending on the rarity of the enemy add +1 to the right list
            switch (enemy.kind)
            {
                case "Common": currentSave.commonsKilled.Inc(enemy.name); break;
                case "Rare": currentSave.raresKilled.Inc(enemy.name); break;
                case "Elite": currentSave.elitesKilled.Inc(enemy.name); break;
            }

            //Unlock new areas
            foreach (var unlockArea in progression.FindAll(x => x.type == "Area"))
                if (!currentSave.unlockedAreas.Contains(unlockArea.areaName) && progBosses.Count > 0 && progBosses.All(x => currentSave.elitesKilled.ContainsKey(x.bossName)))
                    currentSave.unlockedAreas.Add(unlockArea.areaName);
            foreach (var unlockArea in nextProgression.FindAll(x => x.type == "Area"))
                if (!currentSave.unlockedAreas.Contains(unlockArea.areaName) && nextProgBosses.Count == 0)
                    currentSave.unlockedAreas.Add(unlockArea.areaName);

            //Exit board view
            if (area != null && area.instancePart) SwitchDesktop("Instance");
            else if (area != null && area.complexPart) SwitchDesktop("Complex");
            else SwitchDesktop("HostileArea");
            CDesktop.RespawnAll();

            //Drop items
            var directDrop = enemyRace.droppedItems.Select(x => Item.items.Find(y => y.name == x)).ToList();
            var worldDrop = Item.items.FindAll(x => (x.dropRange == null && x.lvl >= enemy.level - 6 && x.lvl <= enemy.level || x.dropRange != null && enemy.level >= int.Parse(x.dropRange.Split('-')[0]) && enemy.level <= int.Parse(x.dropRange.Split('-')[1])) && x.source == "RareDrop");
            var instance = area.instancePart ? SiteInstance.instances.Find(x => x.wings.Any(y => y.areas.Any(z => z["AreaName"] == area.name))) : null;
            var zoneDrop = instance == null || instance.zoneDrop == null ? new() : Item.items.FindAll(x => instance.zoneDrop.Contains(x.name));
            var everything = zoneDrop.Concat(worldDrop).Where(x => x.CanEquip(currentSave.player));
            var dropOther = directDrop.Where(x => (x.rarity == "Common" || x.rarity == "Poor") && (x.type == "Miscellaneous" || x.type == "Trade Good")).ToList();
            var dropGray = everything.Where(x => x.rarity == "Poor").ToList();
            var dropWhite = everything.Where(x => x.rarity == "Common").ToList();
            var dropGreen = everything.Where(x => x.rarity == "Uncommon").ToList();
            var dropBlue = everything.Where(x => x.rarity == "Rare").ToList();
            var dropPurple = everything.Where(x => x.rarity == "Epic").ToList();
            var equippable = directDrop.Where(x => x.CanEquip(currentSave.player)).ToList();
            var notEquippable = directDrop.Where(x => !equippable.Contains(x) && x.type != "Miscellaneous" && x.type != "Trade Good").ToList();
            if (equippable.Count + notEquippable.Count == 0)
            {
                if (dropPurple.Count > 0 && Roll(0.05))
                    results.inventory.AddItem(dropPurple[random.Next(dropPurple.Count)].CopyItem());
                else if (dropBlue.Count > 0 && Roll(1))
                    results.inventory.AddItem(dropBlue[random.Next(dropBlue.Count)].CopyItem());
                else if (dropGreen.Count > 0 && Roll(10))
                    results.inventory.AddItem(dropGreen[random.Next(dropGreen.Count)].CopyItem());
                else if (dropWhite.Count > 0 && Roll(5))
                    results.inventory.AddItem(dropWhite[random.Next(dropWhite.Count)].CopyItem());
                else if (dropGray.Count > 0 && Roll(3))
                    results.inventory.AddItem(dropGray[random.Next(dropGray.Count)].CopyItem());
            }
            else
            {
                var item = equippable.Count > 0 ? equippable[random.Next(equippable.Count)] : notEquippable[random.Next(notEquippable.Count)];
                results.inventory.AddItem(item.CopyItem());
            }
            if (dropOther.Count > 2 && Roll(60))
            {
                results.inventory.AddItem(dropOther[random.Next(dropOther.Count)].CopyItem());
                dropOther.Remove(results.inventory.items.Last());
                if (Roll(40)) results.inventory.AddItem(dropOther[random.Next(dropOther.Count)].CopyItem());
            }
            else if (dropOther.Count > 1 && Roll(50))
                results.inventory.AddItem(dropOther[random.Next(dropOther.Count)].CopyItem());
            else if (dropOther.Count > 0 && Roll(40))
                results.inventory.AddItem(dropOther[random.Next(dropOther.Count)].CopyItem());
            var generalDrops = GeneralDrop.generalDrops.FindAll(x => x.DoesLevelFit(enemy.level) && (x.requiredProfession == null || (player.professionSkills.ContainsKey(x.requiredProfession) && (x.requiredSkill == 0 || x.requiredSkill <= player.professionSkills[x.requiredProfession].Item1))) && (x.category == null || x.category == enemy.Race().category) && x.inclusive);
            if (generalDrops.Count > 0)
                foreach (var drop in generalDrops)
                    if (Roll(drop.rarity))
                    {
                        int amount = 1;
                        for (int i = 1; i < drop.dropCount; i++) amount += Roll(10) ? 1 : 0;
                        results.inventory.AddItem(Item.items.Find(x => x.name == drop.item).CopyItem(amount));
                    }
            var possibleGeneralDrops = GeneralDrop.generalDrops.FindAll(x => x.DoesLevelFit(enemy.level) && (x.requiredProfession == null || (player.professionSkills.ContainsKey(x.requiredProfession) && (x.requiredSkill == 0 || x.requiredSkill <= player.professionSkills[x.requiredProfession].Item1))) && (x.category == null || x.category == enemy.Race().category) && !x.inclusive);
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
            chartPage = "Damage Dealt";
            currentSave.player.ReceiveExperience(board.results.experience);
            SpawnDesktopBlueprint("CombatResults");
        }
        else if (result == "Lost")
        {
            currentSave.playerDead = true;
            PlaySound("Death");
            StopAmbience();
            if (Realm.realms.Find(x => x.name == settings.selectedRealm).hardcore)
            {
                currentSave.deathInfo = new(enemy.name, enemy.Race().kind == "Common", area.name);
            }
            else
            {
                SwitchDesktop("Map");
                grid.SwitchMapTexture(true);
                SpawnTransition();
                SpawnTransition();
                SpawnTransition();
                SpawnTransition();
                SpawnTransition();
            }
            chartPage = "Damage Dealt";
            SpawnDesktopBlueprint("CombatResults");
        }
        else if (result == "Fled")
        {
            PlaySound("RunAwayBitch");
            if (area != null && area.instancePart)
                SwitchDesktop("Instance");
            else if (area != null && area.complexPart)
                SwitchDesktop("Complex");
            else
                SwitchDesktop("HostileArea");
        }
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
                    if (empty > 0) window.LBRegionGroup.regions[j].bigButtons[i].gameObject.AddComponent<FallingElement>().Initiate(empty);
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
                if (list.Count >= 3)
                {
                    FloodDestroy(list);
                    return;
                }
            }

        //IF PLAYER DIED..
        if (player.health <= 0 && window.desktop.title == "Game")
            EndCombat("Lost");

        //IF ENEMY DIED..
        else if (enemy.health <= 0 && window.desktop.title == "Game")
            EndCombat("Won");

        //IF IT'S ENEMY'S TURN..
        else if (!playerTurn)
        {
            //IF ENEMY FINISHED MOVING END THEIR TURN
            if (enemyFinishedMoving)
            {
                //UNLESS THEY SCORED A BONUS MOVE
                if (bonusTurnStreak > 0)
                {
                    bonusTurnStreak = 0;
                    enemyFinishedMoving = false;
                }
                else
                    EndTurn();
            }

            //DO ANIMATION BREAK FOR ENEMY TO GIVE THEM TIME TO THINK
            else if (!breakForEnemy)
            {
                animationTime = (float)(random.Next(3, 5) / 10.0) + 0.3f;
                breakForEnemy = true;
            }

            //IF ENEMY WAS ALREADY ON THINKING BREAK MAKE THEM MOVE
            else
            {
                breakForEnemy = false;
                bonusTurnStreak = 0;
                temporaryElementsEnemy = new();

                //CALCULATE SOLVINGS
                var firstLayerBase = new FutureBoard(board);
                var firstLayer = firstLayerBase.CalculateLayer().OrderByDescending(x => x.MaxDesiredness(firstLayerBase)).ToList();
                firstLayerBase.enemyFinishedMoving = true;
                var baseDesiredness = firstLayerBase.Desiredness();
                firstLayerBase.enemyFinishedMoving = false;
                var currentLayer = firstLayer;
                firstLayer.ForEach(x => x.depth = defines.aiDepth);
                while (true)
                {
                    foreach (var solving in currentLayer)
                    {
                        solving.possibleSolves = solving.board.CalculateLayer();
                        foreach (var innerSolving in solving.possibleSolves)
                            if (innerSolving.depth > -1)
                                innerSolving.depth = innerSolving.board.player.health == 0 || innerSolving.board.enemy.health == 0 ? 0 : solving.depth - (solving.board.TurnEnded() ? 1 : 0);
                    }
                    currentLayer = currentLayer.SelectMany(x => x.possibleSolves.FindAll(y => y.depth > 0)).ToList();
                    if (currentLayer.Count == 0) break;
                }
                var results = new List<(FutureMove, double)>();
                foreach (var solving in firstLayer)
                    results.Add((solving, solving.MaxDesiredness(firstLayerBase, baseDesiredness)));
                results = results.OrderByDescending(x => x.Item2).ToList();

                //var message = "";
                //foreach (var solving1 in firstLayer)
                //{
                //    message += (solving1.board.playerTurn ? "P: " : "E: ") + solving1.desiredness.ToString("0.000") + " " + (solving1.x != -1 ? "(" + solving1.x + ", " + solving1.y + ") (" + boardNameDictionary[firstLayerBase.field[solving1.x, solving1.y]] + ")" : "") + (solving1.ability == "" ? "" : "<" + solving1.ability + ">") + "\n";
                //    if (solving1.possibleSolves.Count > 0) foreach (var solving2 in solving1.possibleSolves)
                //        {
                //            message += "   " + (solving2.board.playerTurn ? "P: " : "E: ") + solving2.desiredness.ToString("0.000") + " " + (solving2.x != -1 ? "(" + solving2.x + ", " + solving2.y + ") (" + boardNameDictionary[solving1.board.field[solving2.x, solving2.y]] + ")" : "") + (solving2.ability == "" ? "" : "<" + solving2.ability + ">") + "\n";
                //            if (solving2.possibleSolves.Count > 0) foreach (var solving3 in solving2.possibleSolves)
                //                {
                //                    message += "      " + (solving3.board.playerTurn ? "P: " : "E: ") + solving3.desiredness.ToString("0.000") + " " + (solving3.x != -1 ? "(" + solving3.x + ", " + solving3.y + ") (" + boardNameDictionary[solving2.board.field[solving3.x, solving3.y]] + ")" : "") + (solving3.ability == "" ? "" : "<" + solving3.ability + ">") + "\n";
                //                    if (solving3.possibleSolves.Count > 0) foreach (var solving4 in solving3.possibleSolves)
                //                        {
                //                            message += "         " + (solving4.board.playerTurn ? "P: " : "E: ") + solving4.desiredness.ToString("0.000") + " " + (solving4.x != -1 ? "(" + solving4.x + ", " + solving4.y + ") (" + boardNameDictionary[solving3.board.field[solving4.x, solving4.y]] + ")" : "") + (solving4.ability == "" ? "" : "<" + solving4.ability + ">") + "\n";
                //                            if (solving4.possibleSolves.Count > 0) foreach (var solving5 in solving4.possibleSolves)
                //                                {
                //                                    message += "            " + (solving5.board.playerTurn ? "P: " : "E: ") + solving5.desiredness.ToString("0.000") + " " + (solving5.x != -1 ? "(" + solving5.x + ", " + solving5.y + ") (" + boardNameDictionary[solving4.board.field[solving5.x, solving5.y]] + ")" : "") + (solving5.ability == "" ? "" : "<" + solving5.ability + ">") + "\n";
                //                                    if (solving5.possibleSolves.Count > 0) foreach (var solving6 in solving5.possibleSolves)
                //                                        {
                //                                            message += "               " + (solving6.board.playerTurn ? "P: " : "E: ") + solving6.desiredness.ToString("0.000") + " " + (solving6.x != -1 ? "(" + solving6.x + ", " + solving6.y + ") (" + boardNameDictionary[solving5.board.field[solving6.x, solving6.y]] + ")" : "") + (solving6.ability == "" ? "" : "<" + solving6.ability + ">") + "\n";
                //                                            if (solving6.possibleSolves.Count > 0) foreach (var solving7 in solving6.possibleSolves)
                //                                                {
                //                                                    message += "                  " + (solving7.board.playerTurn ? "P: " : "E: ") + solving7.desiredness.ToString("0.000") + " " + (solving7.x != -1 ? "(" + solving7.x + ", " + solving7.y + ") (" + boardNameDictionary[solving6.board.field[solving7.x, solving7.y]] + ")" : "") + (solving7.ability == "" ? "" : "<" + solving7.ability + ">") + "\n";
                //                                                }
                //                                        }
                //                                }
                //                        }
                //                }
                //        }
                //}
                ////File.WriteAllText("asd.txt", message);
                //Debug.Log(message);

                //EXECUTE
                var bestMove = results[0].Item1;
                if (bestMove.ability != "")
                {
                    var abilityObj = Ability.abilities.Find(x => x.name == bestMove.ability);
                    board.actions.Add(() =>
                    {
                        cursorEnemy.Move(CDesktop.windows.Find(x => x.title == "EnemyBattleInfo").regionGroups[0].regions[enemy.actionBars.IndexOf(abilityObj.name) + 2].transform.position + new Vector3(139, -10));
                        animationTime += defines.frameTime * 9;
                    });
                    board.actions.Add(() => { cursorEnemy.SetCursor(CursorType.Click); });
                    board.actions.Add(() =>
                    {
                        cursorEnemy.SetCursor(CursorType.Default);
                        AddRegionOverlay(CDesktop.windows.Find(x => x.title == "EnemyBattleInfo").regionGroups[0].regions[enemy.actionBars.IndexOf(abilityObj.name) + 2], "Black", 0.1f);
                        animationTime += defines.frameTime;
                        //PutOnCooldown(false, abilityObj);
                        board.CallEvents(board.enemy, new() { { "Trigger", "AbilityCast" }, {"Triggerer", "Effector" }, { "AbilityName", abilityObj.name } });
                        board.CallEvents(board.player, new() { { "Trigger", "AbilityCast" }, { "Triggerer", "Other" }, { "AbilityName", abilityObj.name } });
                        board.enemy.DetractResources(abilityObj.cost);
                    });
                }
                else
                {
                    board.actions.Add(() => { cursorEnemy.Move(window.LBRegionGroup.regions[bestMove.y].bigButtons[bestMove.x].transform.position); animationTime += defines.frameTime * 8; });
                    board.actions.Add(() => { cursorEnemy.SetCursor(CursorType.Click); });
                    board.actions.Add(() =>
                    {
                        cursorEnemy.SetCursor(CursorType.Default);
                        var list1 = board.FloodCount(bestMove.x, bestMove.y);
                        board.FloodDestroy(list1);
                        board.enemyFinishedMoving = true;
                    });
                }
            }
        }

        //IF IT's PLAYER'S TURN..
        else
        {
            //IF PLAYER FINISHED MOVING TURN END THEIR TURN
            if (playerFinishedMoving)
            {
                //UNLESS THEY SCORED A BONUS MOVE
                if (bonusTurnStreak > 0)
                {
                    bonusTurnStreak = 0;
                    playerFinishedMoving = false;
                    canUnlockScreen = true;
                }
                else
                    EndTurn();
            }

            //IF PLAYER IS STILL GOING TO MOVE UNLOCK THE SCREEN
            else canUnlockScreen = true;
        }
    }

    public int fieldGetCounterX = 0;
    public int fieldGetCounterY = 0;

    public string GetFieldName(int x, int y) => boardNameDictionary[field[x, y]].ToString();

    public string GetFieldButton()
    {
        var r = boardButtonDictionary[field[fieldGetCounterX, fieldGetCounterY]];
        fieldGetCounterX++;
        if (fieldGetCounterX == field.GetLength(0))
            (fieldGetCounterX, fieldGetCounterY) = (0, fieldGetCounterY + 1);
        if (fieldGetCounterY == field.GetLength(1))
            fieldGetCounterY = 0;
        return r;
    }

    //DESTROYS ALL ELEMENTS OF THE SAME KIND THAT ARE NEARBY OF THE TARGETED ONE
    public void FloodDestroy(List<(int, int, int)> list)
    {
        PlaySound(collectSoundDictionary[list[0].Item3].ToString(), 0.3f);
        if (list.Count > 3)
        {
            bonusTurnStreak++;
            PlaySound("BonusMove" + (bonusTurnStreak > 4 ? 4 : bonusTurnStreak), 0.4f);
            SpawnFallingText(new Vector2(0, 14), "Bonus Move", "White");
        }
        var types = list.Select(x => x.Item3 % 10).Distinct();
        var foo = types.ToDictionary(x => Resource(x), x => list.Sum(y => y.Item3 % 10 == x ? y.Item3 / 10 + 1 : 0));
        foreach (var a in list)
        {
            SpawnFlyingElement(1, 9, window.LBRegionGroup.regions[a.Item2].bigButtons[a.Item1].transform.position + new Vector3(-17.5f, -17.5f), boardButtonDictionary[a.Item3], board.playerTurn);
            field[a.Item1, a.Item2] = -1;
        }
        if (playerTurn) player.AddResources(foo);
        else enemy.AddResources(foo);
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
}
