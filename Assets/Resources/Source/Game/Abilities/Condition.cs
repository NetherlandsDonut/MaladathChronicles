using System.Linq;
using System.Collections.Generic;

using static Root;

public class Condition
{
    //Contents of the condition
    public string content;

    //Sub conditions
    public List<Condition> subs;

    //Checks if condition is met
    public bool IsMet(SaveGame save, Board board, FutureBoard futureBoard)
    {
        if (content == null) return true;
        var c = content.ToLower();
        if (c == "") return true;
        else if (c == "all") return subs == null || subs.Count == 0 || subs.All(x => x.IsMet(save, board, futureBoard));
        else if (c == "any") return subs == null || subs.Count == 0 || subs.Any(x => x.IsMet(save, board, futureBoard));
        else
        {
            var args = c.Split(" ").Select(x => Foo(x)).ToList();
            if (args.Count > 2 && new List<string> { ">", ">=", "==", "<=", "<", "!=", "<>" }.Contains(args[1]))
                if (int.TryParse(args[0], out int arg0) && int.TryParse(args[2], out int arg2))
                    return CompareValues(arg0, arg2, args[1]);
            else if (args.Count > 2 && new List<string> { "==", "!=", "<>" }.Contains(args[1]))
                return CompareValues(args[0], args[2], args[1]);
            return false;
        }

        string Foo(string input)
        {
            if (!input.StartsWith("@")) return input;
            else return input switch
            {
                //Board related
                "@playerhealth%" => (int)(board != null ? ((double)board.player.health / board.player.MaxHealth() * 100) : (futureBoard != null ? ((double)futureBoard.player.health / futureBoard.player.MaxHealth() * 100) : 0)) + "",
                "@enemyhealth%" => (int)(board != null ? ((double)board.enemy.health / board.enemy.MaxHealth() * 100) : (futureBoard != null ? ((double)futureBoard.enemy.health / futureBoard.enemy.MaxHealth() * 100) : 0)) + "",
                "@playerhealth" => (board != null ? board.player.health : (futureBoard != null ? futureBoard.player.health : 0)) + "",
                "@enemyhealth" => (board != null ? board.enemy.health : (futureBoard != null ? futureBoard.enemy.health : 0)) + "",
                "@playermaxhealth" => (board != null ? board.player.MaxHealth() : (futureBoard != null ? futureBoard.player.MaxHealth() : 0)) + "",
                "@enemymaxhealth" => (board != null ? board.enemy.MaxHealth() : (futureBoard != null ? futureBoard.enemy.MaxHealth() : 0)) + "",
                "@playerclass" => board != null ? board.player.spec : (futureBoard != null ? futureBoard.player.spec : ""),
                "@enemyclass" => board != null ? board.enemy.spec : (futureBoard != null ? futureBoard.enemy.spec : ""),
                "@playerlvl" => (board != null ? board.player.level : (futureBoard != null ? futureBoard.player.level : 0)) + "",
                "@enemylvl" => (board != null ? board.enemy.level : (futureBoard != null ? futureBoard.enemy.level : 0)) + "",
                "@turn" => (board != null ? board.turn : (futureBoard != null ? futureBoard.turn : 0)) + "",
                "@playerhighestresource" => (board != null ? board.player.resources.Max(x => x.Value) : (futureBoard != null ? futureBoard.player.resources.Max(x => x.Value) : 0)) + "",
                "@enemyhighestresource" => (board != null ? board.enemy.resources.Max(x => x.Value) : (futureBoard != null ? futureBoard.enemy.resources.Max(x => x.Value) : 0)) + "",
                "@playerhighestmaxresource" => (board != null ? board.player.resources.Max(x => x.Value) : (futureBoard != null ? futureBoard.player.resources.Max(x => x.Value) : 0)) + "",
                "@enemyhighestmaxresource" => (board != null ? board.enemy.resources.Max(x => x.Value) : (futureBoard != null ? futureBoard.enemy.resources.Max(x => x.Value) : 0)) + "",
                "@playermaxshadow" => (board != null ? board.player.MaxResource("Shadow") : (futureBoard != null ? futureBoard.player.MaxResource("Shadow") : 0)) + "",
                "@enemymaxshadow" => (board != null ? board.enemy.MaxResource("Shadow") : (futureBoard != null ? futureBoard.enemy.MaxResource("Shadow") : 0)) + "",
                "@playershadow" => (board != null ? board.player.resources["Shadow"] : (futureBoard != null ? futureBoard.player.resources["Shadow"] : 0)) + "",
                "@enemyshadow" => (board != null ? board.enemy.resources["Shadow"] : (futureBoard != null ? futureBoard.enemy.resources["Shadow"] : 0)) + "",
                "@playershadow%" => (board != null ? ((double)board.player.resources["Shadow"] / board.player.MaxResource("Shadow") * 100) : ((double)futureBoard.player.resources["Shadow"] / futureBoard.player.MaxResource("Shadow") * 100)) + "",
                "@enemyshadow%" => (board != null ? ((double)board.enemy.resources["Shadow"] / board.enemy.MaxResource("Shadow") * 100) : ((double)futureBoard.enemy.resources["Shadow"] / futureBoard.enemy.MaxResource("Shadow") * 100)) + "",
                "@playermaxfire" => (board != null ? board.player.MaxResource("Fire") : (futureBoard != null ? futureBoard.player.MaxResource("Fire") : 0)) + "",
                "@enemymaxfire" => (board != null ? board.enemy.MaxResource("Fire") : (futureBoard != null ? futureBoard.enemy.MaxResource("Fire") : 0)) + "",
                "@playerfire" => (board != null ? board.player.resources["Fire"] : (futureBoard != null ? futureBoard.player.resources["Fire"] : 0)) + "",
                "@enemyfire" => (board != null ? board.enemy.resources["Fire"] : (futureBoard != null ? futureBoard.enemy.resources["Fire"] : 0)) + "",
                "@playerfire%" => (board != null ? ((double)board.player.resources["Fire"] / board.player.MaxResource("Fire") * 100) : ((double)futureBoard.player.resources["Fire"] / futureBoard.player.MaxResource("Fire") * 100)) + "",
                "@enemyfire%" => (board != null ? ((double)board.enemy.resources["Fire"] / board.enemy.MaxResource("Fire") * 100) : ((double)futureBoard.enemy.resources["Fire"] / futureBoard.enemy.MaxResource("Fire") * 100)) + "",
                "@playermaxwater" => (board != null ? board.player.MaxResource("Water") : (futureBoard != null ? futureBoard.player.MaxResource("Water") : 0)) + "",
                "@enemymaxwater" => (board != null ? board.enemy.MaxResource("Water") : (futureBoard != null ? futureBoard.enemy.MaxResource("Water") : 0)) + "",
                "@playerwater" => (board != null ? board.player.resources["Water"] : (futureBoard != null ? futureBoard.player.resources["Water"] : 0)) + "",
                "@enemywater" => (board != null ? board.enemy.resources["Water"] : (futureBoard != null ? futureBoard.enemy.resources["Water"] : 0)) + "",
                "@playerwater%" => (board != null ? ((double)board.player.resources["Water"] / board.player.MaxResource("Water") * 100) : ((double)futureBoard.player.resources["Water"] / futureBoard.player.MaxResource("Water") * 100)) + "",
                "@enemywater%" => (board != null ? ((double)board.enemy.resources["Water"] / board.enemy.MaxResource("Water") * 100) : ((double)futureBoard.enemy.resources["Water"] / futureBoard.enemy.MaxResource("Water") * 100)) + "",
                "@playermaxearth" => (board != null ? board.player.MaxResource("Earth") : (futureBoard != null ? futureBoard.player.MaxResource("Earth") : 0)) + "",
                "@enemymaxearth" => (board != null ? board.enemy.MaxResource("Earth") : (futureBoard != null ? futureBoard.enemy.MaxResource("Earth") : 0)) + "",
                "@playerearth" => (board != null ? board.player.resources["Earth"] : (futureBoard != null ? futureBoard.player.resources["Earth"] : 0)) + "",
                "@enemyearth" => (board != null ? board.enemy.resources["Earth"] : (futureBoard != null ? futureBoard.enemy.resources["Earth"] : 0)) + "",
                "@playerearth%" => (board != null ? ((double)board.player.resources["Earth"] / board.player.MaxResource("Earth") * 100) : ((double)futureBoard.player.resources["Earth"] / futureBoard.player.MaxResource("Earth") * 100)) + "",
                "@enemyearth%" => (board != null ? ((double)board.enemy.resources["Earth"] / board.enemy.MaxResource("Earth") * 100) : ((double)futureBoard.enemy.resources["Earth"] / futureBoard.enemy.MaxResource("Earth") * 100)) + "",
                "@playermaxair" => (board != null ? board.player.MaxResource("Air") : (futureBoard != null ? futureBoard.player.MaxResource("Air") : 0)) + "",
                "@enemymaxair" => (board != null ? board.enemy.MaxResource("Air") : (futureBoard != null ? futureBoard.enemy.MaxResource("Air") : 0)) + "",
                "@playerair" => (board != null ? board.player.resources["Air"] : (futureBoard != null ? futureBoard.player.resources["Air"] : 0)) + "",
                "@enemyair" => (board != null ? board.enemy.resources["Air"] : (futureBoard != null ? futureBoard.enemy.resources["Air"] : 0)) + "",
                "@playerair%" => (board != null ? ((double)board.player.resources["Air"] / board.player.MaxResource("Air") * 100) : ((double)futureBoard.player.resources["Air"] / futureBoard.player.MaxResource("Air") * 100)) + "",
                "@enemyair%" => (board != null ? ((double)board.enemy.resources["Air"] / board.enemy.MaxResource("Air") * 100) : ((double)futureBoard.enemy.resources["Air"] / futureBoard.enemy.MaxResource("Air") * 100)) + "",
                "@playermaxarcane" => (board != null ? board.player.MaxResource("Arcane") : (futureBoard != null ? futureBoard.player.MaxResource("Arcane") : 0)) + "",
                "@enemymaxarcane" => (board != null ? board.enemy.MaxResource("Arcane") : (futureBoard != null ? futureBoard.enemy.MaxResource("Arcane") : 0)) + "",
                "@playerarcane" => (board != null ? board.player.resources["Arcane"] : (futureBoard != null ? futureBoard.player.resources["Arcane"] : 0)) + "",
                "@enemyarcane" => (board != null ? board.enemy.resources["Arcane"] : (futureBoard != null ? futureBoard.enemy.resources["Arcane"] : 0)) + "",
                "@playerarcane%" => (board != null ? ((double)board.player.resources["Arcane"] / board.player.MaxResource("Arcane") * 100) : ((double)futureBoard.player.resources["Arcane"] / futureBoard.player.MaxResource("Arcane") * 100)) + "",
                "@enemyarcane%" => (board != null ? ((double)board.enemy.resources["Arcane"] / board.enemy.MaxResource("Arcane") * 100) : ((double)futureBoard.enemy.resources["Arcane"] / futureBoard.enemy.MaxResource("Arcane") * 100)) + "",
                "@playermaxlightning" => (board != null ? board.player.MaxResource("Lightning") : (futureBoard != null ? futureBoard.player.MaxResource("Lightning") : 0)) + "",
                "@enemymaxlightning" => (board != null ? board.enemy.MaxResource("Lightning") : (futureBoard != null ? futureBoard.enemy.MaxResource("Lightning") : 0)) + "",
                "@playerlightning" => (board != null ? board.player.resources["Lightning"] : (futureBoard != null ? futureBoard.player.resources["Lightning"] : 0)) + "",
                "@enemylightning" => (board != null ? board.enemy.resources["Lightning"] : (futureBoard != null ? futureBoard.enemy.resources["Lightning"] : 0)) + "",
                "@playerlightning%" => (board != null ? ((double)board.player.resources["Lightning"] / board.player.MaxResource("Lightning") * 100) : ((double)futureBoard.player.resources["Lightning"] / futureBoard.player.MaxResource("Lightning") * 100)) + "",
                "@enemylightning%" => (board != null ? ((double)board.enemy.resources["Lightning"] / board.enemy.MaxResource("Lightning") * 100) : ((double)futureBoard.enemy.resources["Lightning"] / futureBoard.enemy.MaxResource("Lightning") * 100)) + "",
                "@playermaxfrost" => (board != null ? board.player.MaxResource("Frost") : (futureBoard != null ? futureBoard.player.MaxResource("Frost") : 0)) + "",
                "@enemymaxfrost" => (board != null ? board.enemy.MaxResource("Frost") : (futureBoard != null ? futureBoard.enemy.MaxResource("Frost") : 0)) + "",
                "@playerfrost" => (board != null ? board.player.resources["Frost"] : (futureBoard != null ? futureBoard.player.resources["Frost"] : 0)) + "",
                "@enemyfrost" => (board != null ? board.enemy.resources["Frost"] : (futureBoard != null ? futureBoard.enemy.resources["Frost"] : 0)) + "",
                "@playerfrost%" => (board != null ? ((double)board.player.resources["Frost"] / board.player.MaxResource("Frost") * 100) : ((double)futureBoard.player.resources["Frost"] / futureBoard.player.MaxResource("Frost") * 100)) + "",
                "@enemyfrost%" => (board != null ? ((double)board.enemy.resources["Frost"] / board.enemy.MaxResource("Frost") * 100) : ((double)futureBoard.enemy.resources["Frost"] / futureBoard.enemy.MaxResource("Frost") * 100)) + "",
                "@playermaxdecay" => (board != null ? board.player.MaxResource("Decay") : (futureBoard != null ? futureBoard.player.MaxResource("Decay") : 0)) + "",
                "@enemymaxdecay" => (board != null ? board.enemy.MaxResource("Decay") : (futureBoard != null ? futureBoard.enemy.MaxResource("Decay") : 0)) + "",
                "@playerdecay" => (board != null ? board.player.resources["Decay"] : (futureBoard != null ? futureBoard.player.resources["Decay"] : 0)) + "",
                "@enemydecay" => (board != null ? board.enemy.resources["Decay"] : (futureBoard != null ? futureBoard.enemy.resources["Decay"] : 0)) + "",
                "@playerdecay%" => (board != null ? ((double)board.player.resources["Decay"] / board.player.MaxResource("Decay") * 100) : ((double)futureBoard.player.resources["Decay"] / futureBoard.player.MaxResource("Decay") * 100)) + "",
                "@enemydecay%" => (board != null ? ((double)board.enemy.resources["Decay"] / board.enemy.MaxResource("Decay") * 100) : ((double)futureBoard.enemy.resources["Decay"] / futureBoard.enemy.MaxResource("Decay") * 100)) + "",
                "@playermaxorder" => (board != null ? board.player.MaxResource("Order") : (futureBoard != null ? futureBoard.player.MaxResource("Order") : 0)) + "",
                "@enemymaxorder" => (board != null ? board.enemy.MaxResource("Order") : (futureBoard != null ? futureBoard.enemy.MaxResource("Order") : 0)) + "",
                "@playerorder" => (board != null ? board.player.resources["Order"] : (futureBoard != null ? futureBoard.player.resources["Order"] : 0)) + "",
                "@enemyorder" => (board != null ? board.enemy.resources["Order"] : (futureBoard != null ? futureBoard.enemy.resources["Order"] : 0)) + "",
                "@playerorder%" => (board != null ? ((double)board.player.resources["Order"] / board.player.MaxResource("Order") * 100) : ((double)futureBoard.player.resources["Order"] / futureBoard.player.MaxResource("Order") * 100)) + "",
                "@enemyorder%" => (board != null ? ((double)board.enemy.resources["Order"] / board.enemy.MaxResource("Order") * 100) : ((double)futureBoard.enemy.resources["Order"] / futureBoard.enemy.MaxResource("Order") * 100)) + "",

                //Save related
                "@maxhealth" => (save != null ? save.player.MaxHealth() : 0) + "",
                "@class" => save != null ? save.player.spec : "",
                "@lvl" => (save != null ? save.player.level : 0) + "",

                _ => input,
            };
        }
    }
}
