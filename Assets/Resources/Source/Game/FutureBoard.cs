using System;
using System.Linq;
using System.Collections.Generic;

public class FutureBoard
{
    public FutureBoard(Board board)
    {
        bonusTurnStreak = board.bonusTurnStreak;
        field = new int[board.field.GetLength(0), board.field.GetLength(1)];
        for (int j = field.GetLength(1) - 1; j >= 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                field[i, j] = board.field[i, j];
        player = new FutureEntity(board.player);
        enemy = new FutureEntity(board.enemy);
        playerTurn = board.playerTurn;
        actions = new List<Action>();
        finishedAnimation = false;
    }

    public int bonusTurnStreak;
    public int[,] field;
    public FutureEntity player, enemy;
    public bool playerTurn, enemyFinishedMoving, playerFinishedMoving, finishedAnimation;
    public List<Action> actions;

    public double Desiredness(FutureEntity entity, Entity pastEntity, FutureEntity other, Entity pastOther, double baseDesiredness = 0)
    {
        var elementImportance = entity.ElementImportance(entity.health / pastEntity.MaxHealth(), other.health / pastOther.MaxHealth());
        var otherElementImportance = other.ElementImportance(other.health / pastOther.MaxHealth(), entity.health / pastEntity.MaxHealth());
        foreach (var resource in entity.resources)
            elementImportance[resource.Key] = elementImportance[resource.Key] > otherElementImportance[resource.Key] / 5 ? elementImportance[resource.Key] : otherElementImportance[resource.Key] / 5;
        var score = -baseDesiredness;
        var othrBuffs = (playerTurn ? enemy : player).buffs.Select(x => Buff.buffs.Find(y => x.Item1 == y.name)).ToList();
        if (othrBuffs.Exists(x => x.tags.Contains("Stun")))
            score += ((playerTurn ? enemy : player).buffs.FindAll(y => othrBuffs.FindAll(x => x.tags.Contains("Stun")).Exists(z => z.name == y.Item1)).Max(x => x.Item2) - 1) * 5;
        int flaring = 0;
        while (flaring < 2)
        {
            finishedAnimation = false;
            other.Cooldown();
            other.FlareBuffs(this);
            entity.Cooldown();
            entity.FlareBuffs(this);
            flaring++;
            do AnimateBoard();
            while (!finishedAnimation);
        }
        foreach (var resource in entity.resources)
        {
            var n = resource.Value - pastEntity.resources[resource.Key];
            var amountMultiplier = entity.AmountModifier(n);
            score += elementImportance[resource.Key] * amountMultiplier * (n < 0 ? -1 : 1);
        }
        foreach (var resource in other.resources)
        {
            var n = resource.Value - pastOther.resources[resource.Key];
            if (n == 0) continue;
            var amountMultiplier = other.AmountModifier(n);
            score -= otherElementImportance[resource.Key] * amountMultiplier * (n < 0 ? -1 : 1);
        }
        score += (entity.health - pastEntity.health) * 0.85;
        score -= (other.health - pastOther.health) * 0.85;
        if (score > 0 && (playerTurn && !playerFinishedMoving || !playerTurn && !enemyFinishedMoving))
            score += 10;
        return score;
    }

    public void AnimateBoard()
    {
        //MOVE ELEMENTS DOWN WITH GRAVITY
        for (int j = field.GetLength(1) - 1; j > 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == 0 && field[i, j - 1] != 0)
                    (field[i, j], field[i, j - 1]) = (field[i, j - 1], 0);

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
                if (field[i, j] != 0)
                {
                    var list = FloodCount(i, j);
                    if (list.Count >= 3)
                    {
                        FloodDestroy(list);
                        return;
                    }
                }

        //IF ENEMY SCORED BONUS MOVE THEY EARNED ANOTHER TURN
        if (bonusTurnStreak > 0)
            enemyFinishedMoving = false;
        finishedAnimation = true;
    }

    //DESTROYS ALL ELEMENTS OF THE SAME KIND THAT ARE NEARBY OF THE TARGETED ONE
    public void FloodDestroy(List<(int, int, int)> list)
    {
        if (list.Count > 3)
            bonusTurnStreak++;
        foreach (var a in list)
        {
            if (playerTurn) GiveResource(player, a.Item1, a.Item2);
            else GiveResource(enemy, a.Item1, a.Item2);
            field[a.Item1, a.Item2] = 0;
        }
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

    public void GiveResource(FutureEntity entity, int x, int y)
    {
        var element = "";
        switch (field[x, y] % 10)
        {
            case 1: element = "Earth"; break;
            case 2: element = "Fire"; break;
            case 3: element = "Water"; break;
            case 4: element = "Air"; break;
            case 5: element = "Lightning"; break;
            case 6: element = "Frost"; break;
            case 7: element = "Decay"; break;
            case 8: element = "Arcane"; break;
            case 9: element = "Order"; break;
            case 0: element = "Shadow"; break;
        }
        entity.resources[element]++;
    }
}
