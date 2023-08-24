using System.Linq;
using System.Collections.Generic;

public class FutureBoard
{
    public FutureBoard(FutureBoard board)
    {
        field = board.field.Clone() as int[,];
        player = new FutureEntity(board.player);
        enemy = new FutureEntity(board.enemy);
        playerTurn = board.playerTurn;
        enemyFinishedMoving = board.enemyFinishedMoving;
        playerFinishedMoving = board.playerFinishedMoving;
        if (TurnEnded()) EndTurn();
    }

    public FutureBoard(Board board)
    {
        field = board.field.Clone() as int[,];
        player = new FutureEntity(board.player);
        enemy = new FutureEntity(board.enemy);
        playerTurn = board.playerTurn;
        enemyFinishedMoving = board.enemyFinishedMoving;
        playerFinishedMoving = board.playerFinishedMoving;
    }

    public int bonusTurnStreak;
    public int[,] field;
    public FutureEntity player, enemy;
    public bool playerTurn, enemyFinishedMoving, playerFinishedMoving, finishedAnimation;
    public Dictionary<string, double> playerElementImportance, enemyElementImportance;

    public Dictionary<string, double> EnemyElementImportance()
    {
        enemyElementImportance ??= enemy.ElementImportance(enemy.health / enemy.MaxHealth(), player.health / player.MaxHealth());
        return enemyElementImportance;
    }

    public Dictionary<string, double> PlayerElementImportance()
    {
        playerElementImportance ??= player.ElementImportance(player.health / player.MaxHealth(), enemy.health / enemy.MaxHealth());
        return playerElementImportance;
    }

    public double Desiredness(FutureBoard baseBoard = null)
    {
        var entity = playerTurn ? player : enemy;
        var other = playerTurn ? enemy : player;
        baseBoard ??= this;
        var pastEntity = playerTurn ? baseBoard.player : baseBoard.enemy;
        var pastOther = playerTurn ? baseBoard.enemy : baseBoard.player;
        var score = 0.0;
        var entityElementImportance = playerTurn ? baseBoard.PlayerElementImportance() : baseBoard.EnemyElementImportance();
        var otherElementImportance = playerTurn ? baseBoard.EnemyElementImportance() : baseBoard.PlayerElementImportance();
        foreach (var resource in entity.resources)
        {
            var n = resource.Value - pastEntity.resources[resource.Key];
            var amountMultiplier = entity.AmountModifier(n);
            score += entityElementImportance[resource.Key] * amountMultiplier * (n < 0 ? -1 : 1);
        }
        foreach (var resource in other.resources)
        {
            var n = resource.Value - pastOther.resources[resource.Key];
            var amountMultiplier = entity.AmountModifier(n);
            score -= otherElementImportance[resource.Key] * amountMultiplier * (n < 0 ? -1 : 1);
        }
        score += entity.health - pastEntity.health;
        score -= other.health - pastOther.health;
        if (entity.health <= 0) score -= 1000;
        else if (other.health <= 0) score += 1000;
        return score * (playerTurn ? -1 : 1);
    }

    public List<(int, int, List<(int, int, int)>)> PossibleFloodings()
    {
        var differentFloodings = new List<(int, int, List<(int, int, int)>)>();
        for (int i = 0; i < field.GetLength(0); i++)
            for (int j = 0; j < field.GetLength(1); j++)
                if (field[i, j] != 0)
                {
                    var list2 = FloodCount(i, j);
                    if (!differentFloodings.Exists(x => x.Item3.All(y => list2.Contains((y.Item1, y.Item2, y.Item3)))))
                        differentFloodings.Add((i, j, list2));
                }
        return differentFloodings;
    }

    public List<FutureMove> CalculateLayer()
    {
        var list = new List<FutureMove>();
        var temp = new FutureBoard(this);
        if (temp.playerTurn && temp.playerFinishedMoving || !temp.playerTurn && temp.enemyFinishedMoving)
            list.Add(new FutureMove("", temp));
        else
        {
            var entity = temp.playerTurn ? player : enemy;
            var abilities = entity.actionBars.Select(x => (x, Ability.abilities.Find(y => y.name == x.ability))).ToList();
            foreach (var ability in abilities)
                if (ability.x.cooldown == 0 && ability.Item2.EnoughResources(entity))
                {
                    var board = new FutureBoard(this);
                    entity = board.playerTurn ? board.player : board.enemy;
                    list.Add(new FutureMove(ability.Item2.name, board));
                    entity.actionBars.Find(x => x.ability == ability.Item2.name).cooldown = ability.Item2.cooldown;
                    CallEvents(entity, board, new() { { "Trigger", "AbilityCast" }, { "Triggerer", "Effector" }, { "AbilityName", ability.Item2.name } });
                    CallEvents(entity == player ? enemy : player, board, new() { { "Trigger", "AbilityCast" }, { "Triggerer", "Effector" }, { "AbilityName", ability.Item2.name } });
                    entity.DetractResources(board, ability.Item2.cost);
                    while (!board.finishedAnimation)
                        board.AnimateBoard();
                }
            var differentFloodings = PossibleFloodings();
            foreach (var flooding in differentFloodings)
            {
                var board = new FutureBoard(this);
                list.Add(new FutureMove(flooding.Item1, flooding.Item2, board));
                board.FloodDestroy(flooding.Item3);
                if (board.playerTurn) board.playerFinishedMoving = true;
                else board.enemyFinishedMoving = true;
                while (!board.finishedAnimation)
                    board.AnimateBoard();
            }
        }
        list = list.OrderBy(x => (list[0].board.playerTurn ? 1 : -1) * x.board.Desiredness(this)).ToList();
        bool oneBoardMove = false;
        for (int i = 1; i < list.Count; i++)
            if (list[i].ability == "" && oneBoardMove)
                list[i].depth = -1;
            else if (list[i].ability == "")
                oneBoardMove = true;
        list.RemoveAll(x => x.depth == -1);
        return list;
    }

    public void CallEvents(FutureEntity entity, FutureBoard board, Dictionary<string, string> triggerData)
    {
        foreach (var ability in entity == player ? Board.board.playerCombatAbilities : Board.board.enemyCombatAbilities)
            ability.ExecuteEvents(null, board, triggerData);
        foreach (var buff in entity.buffs)
            buff.Item1.ExecuteEvents(null, board, triggerData);
    }

    public void EndTurn()
    {
        if (playerTurn)
        {
            CallEvents(player, this, new() { { "Trigger", "TurnEnd" } });
            playerTurn = false;
            playerFinishedMoving = false;
            enemy.Cooldown(this);
            CallEvents(enemy, this, new() { { "Trigger", "TurnBegin" } });
            enemy.FlareBuffs(this);
        }
        else
        {
            CallEvents(enemy, this, new() { { "Trigger", "TurnEnd" } });
            playerTurn = true;
            enemyFinishedMoving = false;
            player.Cooldown(this);
            CallEvents(player, this, new() { { "Trigger", "TurnBegin" } });
            player.FlareBuffs(this);
        }
    }

    //CHECK IF THE TURN ENDED
    public bool TurnEnded() => playerTurn && playerFinishedMoving || !playerTurn && enemyFinishedMoving;

    public void AnimateBoard()
    {
        //MOVE ELEMENTS DOWN WITH GRAVITY
        for (int j = field.GetLength(1) - 1; j > 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == 0 && field[i, j - 1] != 0)
                    (field[i, j], field[i, j - 1]) = (field[i, j - 1], 0);

        //CASCADE FOR CURRENT PLAYER
        for (int j = 0; j < field.GetLength(1); j++)
            for (int i = 0; i < field.GetLength(0); i++)
                if (field[i, j] != 0)
                {
                    var list = FloodCount(i, j);
                    if (list.Count >= 3)
                    {
                        FloodDestroy(list);
                        return;
                    }
                }

        //IF SCORED BONUS MOVE DON'T END TURN
        if (bonusTurnStreak > 0)
            if (playerTurn && playerFinishedMoving)
                playerFinishedMoving = false;
            else if (!playerTurn && enemyFinishedMoving)
                enemyFinishedMoving = false;

        //FINISH ANIMATING
        finishedAnimation = true;
    }

    //DESTROYS ALL ELEMENTS OF THE SAME KIND THAT ARE NEARBY OF THE TARGETED ONE
    public void FloodDestroy(List<(int, int, int)> list)
    {
        if (list.Count > 3) bonusTurnStreak++;
        var types = list.Select(x => x.Item3).Distinct();
        var foo = types.ToDictionary(x => Resource(x), x => list.Sum(y => y.Item3 == x ? 1 : 0));
        foreach (var a in list)
            field[a.Item1, a.Item2] = 0;
        if (playerTurn) player.AddResources(this, foo);
        else enemy.AddResources(this, foo);
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
            if (field[i, j] != field[x, y] || positives.Contains((i, j, field[i, j]))) return;
            positives.Add((i, j, field[i, j]));
            if (i > 0) Flood(i - 1, j);
            if (j > 0) Flood(i, j - 1);
            if (i < field.GetLength(0) - 1) Flood(i + 1, j);
            if (j < field.GetLength(1) - 1) Flood(i, j + 1);
        }
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
}
