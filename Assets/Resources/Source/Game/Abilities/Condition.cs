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
            var args = c.Split(" ").Select(x => board == null ? Foo(x) : Moo(x)).ToList();
            if (args.Count > 2 && new List<string> { ">", ">=", "==", "<=", "<", "!=", "<>" }.Contains(args[1]))
                if (int.TryParse(args[0], out int arg0) && int.TryParse(args[2], out int arg2))
                    return CompareValues(arg0, arg2, args[1]);
            else if (args.Count > 2 && new List<string> { "==", "!=", "<>" }.Contains(args[1]))
                return CompareValues(args[0], args[2], args[1]);
            return false;
        }

        string Foo(string input)
        {
            var effector = futureBoard.participants[futureBoard.whosTurn];
            var other = futureBoard.Target(effector.team);
            if (!input.StartsWith("@")) return input;
            else return input switch
            {
                //Board related
                "@effectorhealth%" => (double)effector.who.health / effector.who.MaxHealth() * 100 + "",
                "@otherhealth%" => (double)other.who.health / other.who.MaxHealth() * 100 + "",
                "@effectorhealth" => effector.who.health + "",
                "@otherhealth" => other.who.health + "",
                "@effectormaxhealth" => effector.who.MaxHealth() + "",
                "@othermaxhealth" => other.who.MaxHealth() + "",
                "@effectorclass" => effector.who.spec,
                "@otherclass" => other.who.spec,
                "@effectorlvl" => effector.who.level + "",
                "@otherlvl" => other.who.level + "",
                "@turn" => futureBoard.turn + "",
                "@effectorhighestresource" => effector.who.resources.Max(x => x.Value) + "",
                "@otherhighestresource" => other.who.resources.Max(x => x.Value) + "",
                "@effectorhighestmaxresource" => effector.who.resources.Max(x => x.Value) + "",
                "@otherhighestmaxresource" => other.who.resources.Max(x => x.Value) + "",
                "@effectormaxshadow" => effector.who.MaxResource("Shadow") + "",
                "@othermaxshadow" => other.who.MaxResource("Shadow") + "",
                "@effectorshadow" => effector.who.resources["Shadow"] + "",
                "@othershadow" => other.who.resources["Shadow"] + "",
                "@effectorshadow%" => ((double)effector.who.resources["Shadow"] / effector.who.MaxResource("Shadow") * 100) + "",
                "@othershadow%" => ((double)other.who.resources["Shadow"] / other.who.MaxResource("Shadow") * 100) + "",
                "@effectormaxfire" => effector.who.MaxResource("Fire") + "",
                "@othermaxfire" => other.who.MaxResource("Fire") + "",
                "@effectorfire" => effector.who.resources["Fire"] + "",
                "@otherfire" => other.who.resources["Fire"] + "",
                "@effectorfire%" => ((double)effector.who.resources["Fire"] / effector.who.MaxResource("Fire") * 100) + "",
                "@otherfire%" => ((double)other.who.resources["Fire"] / other.who.MaxResource("Fire") * 100) + "",
                "@effectormaxwater" => effector.who.MaxResource("Water") + "",
                "@othermaxwater" => other.who.MaxResource("Water") + "",
                "@effectorwater" => effector.who.resources["Water"] + "",
                "@otherwater" => other.who.resources["Water"] + "",
                "@effectorwater%" => ((double)effector.who.resources["Water"] / effector.who.MaxResource("Water") * 100) + "",
                "@otherwater%" => ((double)other.who.resources["Water"] / other.who.MaxResource("Water") * 100) + "",
                "@effectormaxearth" => effector.who.MaxResource("Earth") + "",
                "@othermaxearth" => other.who.MaxResource("Earth") + "",
                "@effectorearth" => effector.who.resources["Earth"] + "",
                "@otherearth" => other.who.resources["Earth"] + "",
                "@effectorearth%" => ((double)effector.who.resources["Earth"] / effector.who.MaxResource("Earth") * 100) + "",
                "@otherearth%" => ((double)other.who.resources["Earth"] / other.who.MaxResource("Earth") * 100) + "",
                "@effectormaxair" => effector.who.MaxResource("Air") + "",
                "@othermaxair" => other.who.MaxResource("Air") + "",
                "@effectorair" => effector.who.resources["Air"] + "",
                "@otherair" => other.who.resources["Air"] + "",
                "@effectorair%" => ((double)effector.who.resources["Air"] / effector.who.MaxResource("Air") * 100) + "",
                "@otherair%" => ((double)other.who.resources["Air"] / other.who.MaxResource("Air") * 100) + "",
                "@effectormaxarcane" => effector.who.MaxResource("Arcane") + "",
                "@othermaxarcane" => other.who.MaxResource("Arcane") + "",
                "@effectorarcane" => effector.who.resources["Arcane"] + "",
                "@otherarcane" => other.who.resources["Arcane"] + "",
                "@effectorarcane%" => ((double)effector.who.resources["Arcane"] / effector.who.MaxResource("Arcane") * 100) + "",
                "@otherarcane%" => ((double)other.who.resources["Arcane"] / other.who.MaxResource("Arcane") * 100) + "",
                "@effectormaxlightning" => effector.who.MaxResource("Lightning") + "",
                "@othermaxlightning" => other.who.MaxResource("Lightning") + "",
                "@effectorlightning" => effector.who.resources["Lightning"] + "",
                "@otherlightning" => other.who.resources["Lightning"] + "",
                "@effectorlightning%" => ((double)effector.who.resources["Lightning"] / effector.who.MaxResource("Lightning") * 100) + "",
                "@otherlightning%" => ((double)other.who.resources["Lightning"] / other.who.MaxResource("Lightning") * 100) + "",
                "@effectormaxfrost" => effector.who.MaxResource("Frost") + "",
                "@othermaxfrost" => other.who.MaxResource("Frost") + "",
                "@effectorfrost" => effector.who.resources["Frost"] + "",
                "@otherfrost" => other.who.resources["Frost"] + "",
                "@effectorfrost%" => ((double)effector.who.resources["Frost"] / effector.who.MaxResource("Frost") * 100) + "",
                "@otherfrost%" => ((double)other.who.resources["Frost"] / other.who.MaxResource("Frost") * 100) + "",
                "@effectormaxdecay" => effector.who.MaxResource("Decay") + "",
                "@othermaxdecay" => other.who.MaxResource("Decay") + "",
                "@effectordecay" => effector.who.resources["Decay"] + "",
                "@otherdecay" => other.who.resources["Decay"] + "",
                "@effectordecay%" => ((double)effector.who.resources["Decay"] / effector.who.MaxResource("Decay") * 100) + "",
                "@otherdecay%" => ((double)other.who.resources["Decay"] / other.who.MaxResource("Decay") * 100) + "",
                "@effectormaxorder" => effector.who.MaxResource("Order") + "",
                "@othermaxorder" => other.who.MaxResource("Order") + "",
                "@effectororder" => effector.who.resources["Order"] + "",
                "@otherorder" => other.who.resources["Order"] + "",
                "@effectororder%" => ((double)effector.who.resources["Order"] / effector.who.MaxResource("Order") * 100) + "",
                "@otherorder%" => ((double)other.who.resources["Order"] / other.who.MaxResource("Order") * 100) + "",

                //Save related
                "@maxhealth" => save != null ? save.player.MaxHealth() + "" : "",
                "@class" => save != null ? save.player.spec : "",
                "@lvl" => save != null ? save.player.level + "" : "",

                _ => input,
            };
        }

        string Moo(string input)
        {
            var effector = board.participants[board.whosTurn];
            var other = board.Target(effector.team);
            if (!input.StartsWith("@")) return input;
            else return input switch
            {
                //Board related
                "@effectorhealth%" => (int)((double)board.Target(effector.team).who.health / effector.who.MaxHealth() * 100) + "",
                "@otherhealth%" => (int)((double)other.who.health / other.who.MaxHealth() * 100) + "",
                "@effectorhealth" => effector.who.health + "",
                "@otherhealth" => other.who.health + "",
                "@effectormaxhealth" => effector.who.MaxHealth() + "",
                "@othermaxhealth" => other.who.MaxHealth() + "",
                "@effectorclass" => effector.who.spec,
                "@otherclass" => other.who.spec,
                "@effectorlvl" => effector.who.level + "",
                "@otherlvl" => other.who.level + "",
                "@turn" => board.turn + "",
                "@effectorhighestresource" => effector.who.resources.Max(x => x.Value) + "",
                "@otherhighestresource" => other.who.resources.Max(x => x.Value) + "",
                "@effectorhighestmaxresource" => effector.who.resources.Max(x => x.Value) + "",
                "@otherhighestmaxresource" => other.who.resources.Max(x => x.Value) + "",
                "@effectormaxshadow" => effector.who.MaxResource("Shadow") + "",
                "@othermaxshadow" => other.who.MaxResource("Shadow") + "",
                "@effectorshadow" => effector.who.resources["Shadow"] + "",
                "@othershadow" => other.who.resources["Shadow"] + "",
                "@effectorshadow%" => ((double)effector.who.resources["Shadow"] / effector.who.MaxResource("Shadow") * 100) + "",
                "@othershadow%" => ((double)other.who.resources["Shadow"] / other.who.MaxResource("Shadow") * 100) + "",
                "@effectormaxfire" => effector.who.MaxResource("Fire") + "",
                "@othermaxfire" => other.who.MaxResource("Fire") + "",
                "@effectorfire" => effector.who.resources["Fire"] + "",
                "@otherfire" => other.who.resources["Fire"] + "",
                "@effectorfire%" => ((double)effector.who.resources["Fire"] / effector.who.MaxResource("Fire") * 100) + "",
                "@otherfire%" => ((double)other.who.resources["Fire"] / other.who.MaxResource("Fire") * 100) + "",
                "@effectormaxwater" => effector.who.MaxResource("Water") + "",
                "@othermaxwater" => other.who.MaxResource("Water") + "",
                "@effectorwater" => effector.who.resources["Water"] + "",
                "@otherwater" => other.who.resources["Water"] + "",
                "@effectorwater%" => ((double)effector.who.resources["Water"] / effector.who.MaxResource("Water") * 100) + "",
                "@otherwater%" => ((double)other.who.resources["Water"] / other.who.MaxResource("Water") * 100) + "",
                "@effectormaxearth" => effector.who.MaxResource("Earth") + "",
                "@othermaxearth" => other.who.MaxResource("Earth") + "",
                "@effectorearth" => effector.who.resources["Earth"] + "",
                "@otherearth" => other.who.resources["Earth"] + "",
                "@effectorearth%" => ((double)effector.who.resources["Earth"] / effector.who.MaxResource("Earth") * 100) + "",
                "@otherearth%" => ((double)other.who.resources["Earth"] / other.who.MaxResource("Earth") * 100) + "",
                "@effectormaxair" => effector.who.MaxResource("Air") + "",
                "@othermaxair" => other.who.MaxResource("Air") + "",
                "@effectorair" => effector.who.resources["Air"] + "",
                "@otherair" => other.who.resources["Air"] + "",
                "@effectorair%" => ((double)effector.who.resources["Air"] / effector.who.MaxResource("Air") * 100) + "",
                "@otherair%" => ((double)other.who.resources["Air"] / other.who.MaxResource("Air") * 100) + "",
                "@effectormaxarcane" => effector.who.MaxResource("Arcane") + "",
                "@othermaxarcane" => other.who.MaxResource("Arcane") + "",
                "@effectorarcane" => effector.who.resources["Arcane"] + "",
                "@otherarcane" => other.who.resources["Arcane"] + "",
                "@effectorarcane%" => ((double)effector.who.resources["Arcane"] / effector.who.MaxResource("Arcane") * 100) + "",
                "@otherarcane%" => ((double)other.who.resources["Arcane"] / other.who.MaxResource("Arcane") * 100) + "",
                "@effectormaxlightning" => effector.who.MaxResource("Lightning") + "",
                "@othermaxlightning" => other.who.MaxResource("Lightning") + "",
                "@effectorlightning" => effector.who.resources["Lightning"] + "",
                "@otherlightning" => other.who.resources["Lightning"] + "",
                "@effectorlightning%" => ((double)effector.who.resources["Lightning"] / effector.who.MaxResource("Lightning") * 100) + "",
                "@otherlightning%" => ((double)other.who.resources["Lightning"] / other.who.MaxResource("Lightning") * 100) + "",
                "@effectormaxfrost" => effector.who.MaxResource("Frost") + "",
                "@othermaxfrost" => other.who.MaxResource("Frost") + "",
                "@effectorfrost" => effector.who.resources["Frost"] + "",
                "@otherfrost" => other.who.resources["Frost"] + "",
                "@effectorfrost%" => ((double)effector.who.resources["Frost"] / effector.who.MaxResource("Frost") * 100) + "",
                "@otherfrost%" => ((double)other.who.resources["Frost"] / other.who.MaxResource("Frost") * 100) + "",
                "@effectormaxdecay" => effector.who.MaxResource("Decay") + "",
                "@othermaxdecay" => other.who.MaxResource("Decay") + "",
                "@effectordecay" => effector.who.resources["Decay"] + "",
                "@otherdecay" => other.who.resources["Decay"] + "",
                "@effectordecay%" => ((double)effector.who.resources["Decay"] / effector.who.MaxResource("Decay") * 100) + "",
                "@otherdecay%" => ((double)other.who.resources["Decay"] / other.who.MaxResource("Decay") * 100) + "",
                "@effectormaxorder" => effector.who.MaxResource("Order") + "",
                "@othermaxorder" => other.who.MaxResource("Order") + "",
                "@effectororder" => effector.who.resources["Order"] + "",
                "@otherorder" => other.who.resources["Order"] + "",
                "@effectororder%" => ((double)effector.who.resources["Order"] / effector.who.MaxResource("Order") * 100) + "",
                "@otherorder%" => ((double)other.who.resources["Order"] / other.who.MaxResource("Order") * 100) + "",

                //Save related
                "@maxhealth" => save != null ? save.player.MaxHealth() + "" : "",
                "@class" => save != null ? save.player.spec : "",
                "@lvl" => save != null ? save.player.level + "" : "",

                _ => input,
            };
        }
    }
}
