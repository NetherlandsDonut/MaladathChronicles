using System.Linq;
using System.Collections.Generic;

using static Defines;

public class FutureBoard
{
    public FutureBoard(FutureBoard board)
    {
        field = board.field.Clone() as int[,];
        participants = new();
        foreach (var participant in board.participants)
            participants.Add(new FutureCombatParticipant()
            {
                who = new FutureEntity(participant.who),
                combatAbilities = participant.combatAbilities,
                human = participant.human,
                team = participant.team
            });
        turn = board.turn;
        whosTurn = board.whosTurn;
        finishedMoving = board.finishedMoving;
        finishedMoving = board.finishedMoving;
        cooldowns = board.cooldowns.ToDictionary(x => x.Key, x => x.Value);
        cooldowns = board.cooldowns.ToDictionary(x => x.Key, x => x.Value);
        if (TurnEnded()) EndTurn();
    }

    public FutureBoard(Board board)
    {
        field = board.field.Clone() as int[,];
        participants = new();
        foreach (var participant in board.participants)
            participants.Add(new FutureCombatParticipant()
            {
                who = new FutureEntity(participant.who),
                combatAbilities = participant.combatAbilities,
                human = participant.human,
                team = participant.team
            });
        turn = board.turn;
        whosTurn = board.whosTurn;
        finishedMoving = board.finishedMoving;
        cooldowns = board.cooldowns.ToDictionary(x => x.Key, x => x.Value.ToDictionary(y => y.Key, y => y.Value));
    }

    //Turn counter
    public int turn;
    public int bonusTurnStreak;
    public int[,] field;
    public List<FutureCombatParticipant> participants;
    public int whosTurn;
    public bool finishedMoving, finishedAnimation;
    public Dictionary<int, Dictionary<string, int>> cooldowns;

    public void PutOnCooldown(int participant, Ability ability)
    {
        var list = cooldowns[participant];
        list.Remove(ability.name);
        if (ability.cooldown > 0)
            list.Add(ability.name, ability.cooldown);
    }

    public int CooldownOn(int participant, string ability) => cooldowns[participant].Get(ability);

    //Cooldowns all action bar abilities and used passives by 1 turn
    public void Cooldown(int participant)
    {
        var abilities = cooldowns[participant];
        var names = cooldowns[participant].Keys.ToList();
        foreach (var name in names)
            if (abilities[name] > 0)
            {
                if (--abilities[name] <= 0)
                {
                    abilities.Remove(name);
                    CallEvents(participants[participant].who, new() { { "Trigger", "Cooldown" }, { "Triggerer", "Effector" }, { "AbilityName", name } });
                }
            }
            else abilities.Remove(name);
    }

    public double Desiredness(FutureBoard baseBoard = null)
    {
        //In case of not basing the future on any different future then just set this one as the base
        baseBoard ??= this;

        //Base score setting
        //Score higher than zero means it's good for the player
        //and score lower than zero means it's good for the opponent
        var score = 0.0;

        //Based on each entity's priorities in resources calculate how happy are they after recent resource changes
        foreach (var participant in participants)
            foreach (var resource in participant.who.resources)
            {
                var n = resource.Value - baseBoard.participants[participants.FindIndex(x => x == participant)].who.resources[resource.Key];
                var amountMultiplier = participant.who.AmountModifier(n);
                score += participant.who.ElementImportance()[resource.Key] * amountMultiplier * (n < 0 ? 1 : -1);
            }

        //Modify score by the difference in entity health
        foreach (var participant in participants)
            score += (participant.team != participants[whosTurn].team ? -1 : 1) * participant.who.health - baseBoard.participants[participants.FindIndex(x => x == participant)].who.health;

        //Impact score heavily when one of the entities dies in the prediction
        foreach (var participant in participants)
            if (participant.who.health <= 0 && baseBoard.participants[participants.FindIndex(x => x == participant)].who.dead)
            {
                if (participant.team != participants[whosTurn].team) score -= 1000;
                else score += 1000;
            }

        //Reverse the score if the prediction was being made for player and not AI
        return score * (participants[whosTurn].team == 1 ? -1 : 1);
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

    //Returns a list of all possible moves for the current entity
    public List<FutureMove> CalculateLayer()
    {
        var list = new List<FutureMove>();
        var temp = new FutureBoard(this);
        if (temp.finishedMoving) list.Add(new FutureMove("", temp));
        else
        {
            var entity = temp.participants[whosTurn].who;
            var abilities = entity.actionBars[entity.currentActionSet].Select(x => Ability.abilities.Find(y => y.name == x)).ToList();
            if (!temp.participants[whosTurn].human) //THIS PREVENTS ENEMY FROM CALCULATING PLAYER ABILITIES
                foreach (var ability in abilities)
                    if (CooldownOn(whosTurn, ability.name) <= 0 && ability.EnoughResources(entity) && ability.AreAnyConditionsMet("AbilityCast", SaveGame.currentSave, null, temp))
                    {
                        var futureBoard = new FutureBoard(this);
                        entity = futureBoard.participants[whosTurn].who;
                        list.Add(new FutureMove(ability.name, futureBoard));
                        foreach (var participant in futureBoard.participants)
                            if (futureBoard.participants.IndexOf(participant) == whosTurn) futureBoard.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, { "Triggerer", "Effector" }, { "AbilityName", ability.name } });
                            else futureBoard.CallEvents(participant.who, new() { { "Trigger", "AbilityCast" }, { "Triggerer", "Other" }, { "AbilityName", ability.name } });
                        entity.DetractResources(futureBoard, ability.cost);
                        while (!futureBoard.finishedAnimation)
                            futureBoard.AnimateBoard();
                    }
            var differentFloodings = PossibleFloodings();
            foreach (var flooding in differentFloodings)
            {
                var futureBoard = new FutureBoard(this);
                list.Add(new FutureMove(flooding.Item1, flooding.Item2, futureBoard));
                futureBoard.FloodDestroy(flooding.Item3);
                futureBoard.finishedMoving = true;
                while (!futureBoard.finishedAnimation)
                    futureBoard.AnimateBoard();
            }
        }
        list = list.OrderBy(x => (list[0].board.participants[list[0].board.whosTurn].team == 1 ? 1 : -1) * x.board.Desiredness(this)).ToList();
        var manualMoves = 0;
        for (int i = list.Count - 1; i >= 0; i--)
            if (list[i].ability == "" && defines.aiManualBranches <= manualMoves)
                list.RemoveAt(i);
            else if (list[i].ability == "")
                manualMoves++;
        return list;
    }

    //Call all events in combat that can be triggered by a specified trigger
    public void CallEvents(FutureEntity who, Dictionary<string, string> triggerData)
    {
        if (who.inventory != null)
            foreach (var item in who.inventory.items.Concat(who.equipment.Select(x => x.Value)).ToList())
                if (item.abilities != null)
                    foreach (var ability in item.abilities.Select(x => (Ability.abilities.Find(y => y.name == x.Key), x.Value)))
                        ability.Item1.ExecuteEvents(null, this, triggerData, item, ability.Value, whosTurn);
        foreach (var ability in participants.Find(x => x.who == who).combatAbilities)
            ability.Key.ExecuteEvents(null, this, triggerData, null, ability.Value, participants.FindIndex(x => x.who == who));
        foreach (var buff in who.buffs.ToList())
            buff.buff.ExecuteEvents(null, this, triggerData, buff);
    }

    public FutureCombatParticipant Target(int ofTeam) => participants.Last(x => x.team != ofTeam);

    public void EndTurn()
    {
        turn++;
        CallEvents(participants[whosTurn++].who, new() { { "Trigger", "TurnEnd" } });
        whosTurn %= participants.Count;
        finishedMoving = false;
        Cooldown(whosTurn);
        CallEvents(participants[whosTurn].who, new() { { "Trigger", "TurnBegin" } });
        participants[whosTurn].who.FlareBuffs(this);
        if (turn % participants.Count == 1)
            for (int i = 0; i < field.GetLength(0); i++)
                field[i, field.GetLength(1) - 1] = -1;
    }

    //CHECK IF THE TURN ENDED
    public bool TurnEnded() => finishedMoving;

    public void AnimateBoard()
    {
        //MOVE ELEMENTS DOWN WITH GRAVITY
        for (int j = field.GetLength(1) - 1; j > 0; j--)
            for (int i = field.GetLength(0) - 1; i >= 0; i--)
                if (field[i, j] == -1 && field[i, j - 1] != -1)
                    (field[i, j], field[i, j - 1]) = (field[i, j - 1], -1);

        //CASCADE FOR CURRENT PLAYER
        for (int j = 0; j < field.GetLength(1); j++)
            for (int i = 0; i < field.GetLength(0); i++)
                if (field[i, j] != -1)
                {
                    var list = FloodCount(i, j);
                    if (list.Count >= defines.cascadeMinimum)
                    {
                        FloodDestroy(list);
                        return;
                    }
                }

        //IF SCORED BONUS MOVE DON'T END TURN
        if (bonusTurnStreak > 0)
            if (finishedMoving)
                finishedMoving = false;

        //FINISH ANIMATING
        finishedAnimation = true;
    }

    //DESTROYS ALL ELEMENTS OF THE SAME KIND THAT ARE NEARBY OF THE TARGETED ONE
    public void FloodDestroy(List<(int, int, int)> list)
    {
        if (list.Count > 3) bonusTurnStreak++;
        var types = list.Select(x => x.Item3 % 10).Distinct();
        var foo = types.ToDictionary(x => Resource(x), x => list.Sum(y => y.Item3 % 10 == x ? y.Item3 / 10 + 1 : 0));
        foreach (var a in list)
            field[a.Item1, a.Item2] = -1;
        participants[whosTurn].who.AddResources(this, foo);
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
}
