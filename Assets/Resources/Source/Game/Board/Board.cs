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
    public Dictionary<string, HealthBar> healthBars;

    //Indicates whether it's currently the player's turn
    public bool playerTurn;

    //Tells whether the artificial time break for the enemy move was made already. For now it is used as an illusion that the enemy is thinking before making a move. It may not be useful in the future. We will see
    public bool breakForEnemy;

    //Tells whether the time break was made in between element cascades on the board
    public bool breakForCascade;

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
    public List<(string, int)> playerCooldowns;

    //Abilities (Active and passive) that enemy has in the combat
    public Dictionary<Ability, int> enemyCombatAbilities;

    //List of passive abilities owned by the enemy that are on cooldown
    public List<(string, int)> enemyCooldowns;

    //Queue of actions to do on the board and the combatants
    public List<Action> actions;

    //Are where the combat takes place
    public SiteHostileArea area;

    public void PutOnCooldown(bool player, Ability ability)
    {
        var list = player ? playerCooldowns : enemyCooldowns;
        list.RemoveAll(x => x.Item1 == ability.name);
        list.Add((ability.name, ability.cooldown));
    }

    public int CooldownOn(bool player, string ability) => (player ? playerCooldowns : enemyCooldowns).Find(x => x.Item1 == ability).Item2;

    //Cooldowns all action bar abilities and used passives by 1 turn
    public void Cooldown(bool player)
    {
        ref var abilities = ref (player ? ref playerCooldowns : ref enemyCooldowns);
        for (int i = abilities.Count - 1; i >= 0; i--)
        {
            abilities[i] = (abilities[i].Item1, abilities[i].Item2 - 1);
            if (abilities[i].Item2 == 0)
                board.CallEvents(player ? this.player : enemy, new() { { "Trigger", "Cooldown" }, { "Triggerer", "Effector" }, { "AbilityName", abilities[i].Item1 } });
        }
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
            Cooldown(false);
            CallEvents(enemy, new() { { "Trigger", "TurnBegin" } });
            enemy.FlareBuffs();
        }
        else
        {
            CallEvents(enemy, new() { { "Trigger", "TurnEnd" } });
            cursorEnemy.fadeOut = true;
            playerTurn = true;
            enemyFinishedMoving = false;
            Cooldown(true);
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
                field[i, j] = 0;
        CDesktop.LockScreen();
    }

    public void UpdateHealthBars()
    {
        foreach (var foo in healthBars)
            foo.Value.UpdateHealthBar();
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
            if (area != null && enemy.kind != "Elite")
            {
                if (progBosses.Count > 0 && progBosses.All(x => currentSave.elitesKilled.ContainsKey(x.bossName)) || progBosses.Count == 0)
                {
                    if (!currentSave.siteProgress.ContainsKey(area.name))
                        currentSave.siteProgress.Add(area.name, 1);
                    else currentSave.siteProgress[area.name]++;
                }
            }
            if (enemy.kind == "Common")
            {
                if (!currentSave.commonsKilled.ContainsKey(enemy.name))
                    currentSave.commonsKilled.Add(enemy.name, 1);
                else currentSave.commonsKilled[enemy.name]++;
            }
            else if (enemy.kind == "Rare")
            {
                if (!currentSave.raresKilled.ContainsKey(enemy.name))
                    currentSave.raresKilled.Add(enemy.name, 1);
                else currentSave.raresKilled[enemy.name]++;
            }
            else if (enemy.kind == "Elite")
            {
                if (!currentSave.elitesKilled.ContainsKey(enemy.name))
                    currentSave.elitesKilled.Add(enemy.name, 1);
                else currentSave.elitesKilled[enemy.name]++;
            }
            foreach (var unlockArea in progression.FindAll(x => x.type == "Area"))
                if (!currentSave.unlockedAreas.Contains(unlockArea.areaName) && progBosses.Count > 0 && progBosses.All(x => currentSave.elitesKilled.ContainsKey(x.bossName)))
                    currentSave.unlockedAreas.Add(unlockArea.areaName);
            foreach (var unlockArea in nextProgression.FindAll(x => x.type == "Area"))
                if (!currentSave.unlockedAreas.Contains(unlockArea.areaName) && nextProgBosses.Count == 0)
                    currentSave.unlockedAreas.Add(unlockArea.areaName);
            if (area != null && area.instancePart)
                SwitchDesktop("Instance");
            else if (area != null && area.complexPart)
                SwitchDesktop("Complex");
            else
                SwitchDesktop("HostileArea");
            CDesktop.RespawnAll();

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
            if (possibleGeneralDrops.Count > 0)
                foreach (var drop in possibleGeneralDrops.Shuffle().OrderBy(x => x.rarity))
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
                currentSave.deathInfo = new(enemy.name, area.name);
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
            CDesktop.RebuildAll();
        }
        if (CDesktop.screenLocked)
            CDesktop.UnlockScreen();
    }

    public void AnimateBoard()
    {
        //MOVE ELEMENTS DOWN WITH GRAVITY
        for (int j = field.GetLength(1) - 2; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] != 0)
                {
                    var zeroes = 0;
                    for (int q = 0; q + j < field.GetLength(1); q++)
                        if (field[i, j + q] == 0) zeroes++;
                    (field[i, j], field[i, j + zeroes]) = (0, field[i, j]);
                    if (zeroes > 0) window.LBRegionGroup.regions[j].bigButtons[i].gameObject.AddComponent<FallingElement>().Initiate(zeroes);
                }

        //IF BOARD IS NOT YET FULL RETURN AND DO PREVIOUS STEPS AGAIN
        for (int j = field.GetLength(1) - 1; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == 0)
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
                    if (!breakForCascade)
                    {
                        FloodDestroy(list);
                        breakForCascade = true;
                    }
                    else
                    {
                        breakForCascade = false;
                        animationTime += defines.frameTime * 2;
                    }
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
        var types = list.Select(x => x.Item3).Distinct();
        var foo = types.ToDictionary(x => Resource(x), x => list.Sum(y => y.Item3 == x ? 1 : 0));
        foreach (var a in list)
        {
            SpawnFlyingElement(1, 9, window.LBRegionGroup.regions[a.Item2].bigButtons[a.Item1].transform.position + new Vector3(-17.5f, -17.5f), boardButtonDictionary[a.Item3], board.playerTurn);
            field[a.Item1, a.Item2] = 0;
        }
        if (playerTurn) player.AddResources(foo);
        else enemy.AddResources(foo);
        bufferBoard.Reset();
        CDesktop.LockScreen();
    }

    public string Resource(int id)
    {
        if (id == 11) return "Earth";
        else if (id == 12) return "Fire";
        else if (id == 13) return "Water";
        else if (id == 14) return "Air";
        else if (id == 15) return "Lightning";
        else if (id == 16) return "Frost";
        else if (id == 17) return "Decay";
        else if (id == 18) return "Arcane";
        else if (id == 19) return "Order";
        else if (id == 20) return "Shadow";
        else return "None";
    }

    public int ResourceReverse(string element)
    {
        if (element == "Earth") return 11;
        else if (element == "Fire") return 12;
        else if (element == "Water") return 13;
        else if (element == "Air") return 14;
        else if (element == "Lightning") return 15;
        else if (element == "Frost") return 16;
        else if (element == "Decay") return 17;
        else if (element == "Arcane") return 18;
        else if (element == "Order") return 19;
        else if (element == "Shadow") return 20;
        else return 0;
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
            if (field[i, j] != field[x, y] && field[i, j] != field[x, y] - 10 && field[i, j] - 10 != field[x, y] || positives.Contains((i, j, field[i, j]))) return;
            positives.Add((i, j, field[i, j]));
            if (i > 0) Flood(i - 1, j);
            if (j > 0) Flood(i, j - 1);
            if (i < field.GetLength(0) - 1) Flood(i + 1, j);
            if (j < field.GetLength(1) - 1) Flood(i, j + 1);
        }
    }

    public static Dictionary<int, string> boardNameDictionary = new()
    {
        { 00, "Empty" },
        { 01, "Awakened Earth" },
        { 02, "Awakened Fire" },
        { 03, "Awakened Water" },
        { 04, "Awakened Air" },
        { 05, "Awakened Lightning" },
        { 06, "Awakened Frost" },
        { 07, "Awakened Decay" },
        { 08, "Awakened Arcane" },
        { 09, "Awakened Order" },
        { 10, "Awakened Shadow" },
        { 11, "Rousing Earth" },
        { 12, "Rousing Fire" },
        { 13, "Rousing Water" },
        { 14, "Rousing Air" },
        { 15, "Rousing Lightning" },
        { 16, "Rousing Frost" },
        { 17, "Rousing Decay" },
        { 18, "Rousing Arcane" },
        { 19, "Rousing Order" },
        { 20, "Rousing Shadow" },
        { 21, "Soul of Earth" },
        { 22, "Soul of Fire" },
        { 23, "Soul of Water" },
        { 24, "Soul of Air" },
        { 25, "Soul of Lightning" },
        { 26, "Soul of Frost" },
        { 27, "Soul of Decay" },
        { 28, "Soul of Arcane" },
        { 29, "Soul of Order" },
        { 30, "Soul of Shadow" },
    };

    public static Dictionary<int, string> boardButtonDictionary = new()
    {
        { 00, null },
        { 01, "ElementEarthAwakened" },
        { 02, "ElementFireAwakened" },
        { 03, "ElementWaterAwakened" },
        { 04, "ElementAirAwakened" },
        { 05, "ElementLightningAwakened" },
        { 06, "ElementFrostAwakened" },
        { 07, "ElementDecayAwakened" },
        { 08, "ElementArcaneAwakened" },
        { 09, "ElementOrderAwakened" },
        { 10, "ElementShadowAwakened" },
        { 11, "ElementEarthRousing" },
        { 12, "ElementFireRousing" },
        { 13, "ElementWaterRousing" },
        { 14, "ElementAirRousing" },
        { 15, "ElementLightningRousing" },
        { 16, "ElementFrostRousing" },
        { 17, "ElementDecayRousing" },
        { 18, "ElementArcaneRousing" },
        { 19, "ElementOrderRousing" },
        { 20, "ElementShadowRousing" },
        { 21, "ElementEarthSoul" },
        { 22, "ElementFireSoul" },
        { 23, "ElementWaterSoul" },
        { 24, "ElementAirSoul" },
        { 25, "ElementLightningSoul" },
        { 26, "ElementFrostSoul" },
        { 27, "ElementDecaySoul" },
        { 28, "ElementArcaneSoul" },
        { 29, "ElementOrderSoul" },
        { 30, "ElementShadowSoul" },
    };

    public static Dictionary<int, string> collectSoundDictionary = new()
    {
        { 00, "" },
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
        { 30, "ElementShadow" },
    };
}
