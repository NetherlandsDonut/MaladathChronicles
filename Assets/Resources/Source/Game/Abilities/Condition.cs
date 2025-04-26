using System.Linq;
using System.Collections.Generic;

using static Root;

public class Condition
{
    //Contents of the condition
    public string condition;

    //Message displayed as falling text when condition failed
    public string failedMessage;

    //Sub conditions
    public List<Condition> subs;

    //Checks if condition is met
    public bool IsMet(Ability ability, int abilityRank, SaveGame save, Board board)
    {
        if (condition == null) return true;
        var c = condition;
        if (c == "") return true;
        else if (c == "all") return subs == null || subs.Count == 0 || subs.All(x => x.IsMet(ability, abilityRank, save, board));
        else if (c == "any") return subs == null || subs.Count == 0 || subs.Any(x => x.IsMet(ability, abilityRank, save, board));
        else
        {
            var args = c.Split(" ").Select(x => Moo(x)).ToList();
            if (args.Count > 2)
            {
                var variables = ability.RankVariables(abilityRank);
                if (args[0].StartsWith("#") && variables.ContainsKey(args[0]))
                    args[0] = variables[args[0]].Replace(".", ",");
                if (args[2].StartsWith("#") && variables.ContainsKey(args[2]))
                    args[2] = variables[args[2]].Replace(".", ",");
                if (new List<string> { ">", ">=", "==", "<=", "<", "!=", "<>" }.Contains(args[1]) && int.TryParse(args[0], out int arg0) && int.TryParse(args[2], out int arg2))
                    return CompareValues(arg0, arg2, args[1]);
                else if (new List<string> { "==", "!=", "<>" }.Contains(args[1]))
                    return CompareValues(args[0], args[2], args[1]);
            }
            return false;
        }

        string Moo(string input)
        {
            var effector = board.participants[board.whosTurn];
            var other = board.Target(effector.team);
            if (!input.StartsWith("@")) return input;
            else return input switch
            {
                //Board related 
                "@turn" => board.turn + "",

                //Caster related
                "@effectorteam" => effector.team + "",
                "@effectorhealth%" => (int)((double)effector.who.health / effector.who.MaxHealth() * 100) + "",
                "@effectorhealth" => effector.who.health + "",
                "@effectormaxhealth" => effector.who.MaxHealth() + "",
                "@effectorclass" => effector.who.spec,
                "@effectorlvl" => effector.who.level + "",
                "@effectorkind" => effector.who.kind,
                "@effectorcategory" => effector.who.Race().category,
                "@effectorsubcategory" => effector.who.Race().subcategory,
                "@effectorhighestresource" => effector.who.resources.Max(x => x.Value) + "",
                "@effectorhighestmaxresource" => effector.who.resources.Max(x => x.Value) + "",
                "@effectormaxshadow" => effector.who.MaxResource("Shadow") + "",
                "@effectorshadow" => effector.who.resources["Shadow"] + "",
                "@effectorshadow%" => ((double)effector.who.resources["Shadow"] / effector.who.MaxResource("Shadow") * 100) + "",
                "@effectormaxfire" => effector.who.MaxResource("Fire") + "",
                "@effectorfire" => effector.who.resources["Fire"] + "",
                "@effectorfire%" => ((double)effector.who.resources["Fire"] / effector.who.MaxResource("Fire") * 100) + "",
                "@effectormaxwater" => effector.who.MaxResource("Water") + "",
                "@effectorwater" => effector.who.resources["Water"] + "",
                "@effectorwater%" => ((double)effector.who.resources["Water"] / effector.who.MaxResource("Water") * 100) + "",
                "@effectormaxearth" => effector.who.MaxResource("Earth") + "",
                "@effectorearth" => effector.who.resources["Earth"] + "",
                "@effectorearth%" => ((double)effector.who.resources["Earth"] / effector.who.MaxResource("Earth") * 100) + "",
                "@effectormaxair" => effector.who.MaxResource("Air") + "",
                "@effectorair" => effector.who.resources["Air"] + "",
                "@effectorair%" => ((double)effector.who.resources["Air"] / effector.who.MaxResource("Air") * 100) + "",
                "@effectormaxarcane" => effector.who.MaxResource("Arcane") + "",
                "@effectorarcane" => effector.who.resources["Arcane"] + "",
                "@effectorarcane%" => ((double)effector.who.resources["Arcane"] / effector.who.MaxResource("Arcane") * 100) + "",
                "@effectormaxlightning" => effector.who.MaxResource("Lightning") + "",
                "@effectorlightning" => effector.who.resources["Lightning"] + "",
                "@effectorlightning%" => ((double)effector.who.resources["Lightning"] / effector.who.MaxResource("Lightning") * 100) + "",
                "@effectormaxfrost" => effector.who.MaxResource("Frost") + "",
                "@effectorfrost" => effector.who.resources["Frost"] + "",
                "@effectorfrost%" => ((double)effector.who.resources["Frost"] / effector.who.MaxResource("Frost") * 100) + "",
                "@effectormaxdecay" => effector.who.MaxResource("Decay") + "",
                "@effectordecay" => effector.who.resources["Decay"] + "",
                "@effectordecay%" => ((double)effector.who.resources["Decay"] / effector.who.MaxResource("Decay") * 100) + "",
                "@effectormaxorder" => effector.who.MaxResource("Order") + "",
                "@effectororder" => effector.who.resources["Order"] + "",
                "@effectororder%" => ((double)effector.who.resources["Order"] / effector.who.MaxResource("Order") * 100) + "",

                //Target related
                "@otherteam" => other.team + "",
                "@otherhealth%" => (int)((double)other.who.health / other.who.MaxHealth() * 100) + "",
                "@otherhealth" => other.who.health + "",
                "@othermaxhealth" => other.who.MaxHealth() + "",
                "@otherclass" => other.who.spec,
                "@otherlvl" => other.who.level + "",
                "@otherkind" => other.who.kind,
                "@othercategory" => other.who.Race().category,
                "@othersubcategory" => other.who.Race().subcategory,
                "@otherhighestresource" => other.who.resources.Max(x => x.Value) + "",
                "@otherhighestmaxresource" => other.who.resources.Max(x => x.Value) + "",
                "@othermaxshadow" => other.who.MaxResource("Shadow") + "",
                "@othershadow" => other.who.resources["Shadow"] + "",
                "@othershadow%" => ((double)other.who.resources["Shadow"] / other.who.MaxResource("Shadow") * 100) + "",
                "@othermaxfire" => other.who.MaxResource("Fire") + "",
                "@otherfire" => other.who.resources["Fire"] + "",
                "@otherfire%" => ((double)other.who.resources["Fire"] / other.who.MaxResource("Fire") * 100) + "",
                "@othermaxwater" => other.who.MaxResource("Water") + "",
                "@otherwater" => other.who.resources["Water"] + "",
                "@otherwater%" => ((double)other.who.resources["Water"] / other.who.MaxResource("Water") * 100) + "",
                "@othermaxearth" => other.who.MaxResource("Earth") + "",
                "@otherearth" => other.who.resources["Earth"] + "",
                "@otherearth%" => ((double)other.who.resources["Earth"] / other.who.MaxResource("Earth") * 100) + "",
                "@othermaxair" => other.who.MaxResource("Air") + "",
                "@otherair" => other.who.resources["Air"] + "",
                "@otherair%" => ((double)other.who.resources["Air"] / other.who.MaxResource("Air") * 100) + "",
                "@othermaxarcane" => other.who.MaxResource("Arcane") + "",
                "@otherarcane" => other.who.resources["Arcane"] + "",
                "@otherarcane%" => ((double)other.who.resources["Arcane"] / other.who.MaxResource("Arcane") * 100) + "",
                "@othermaxlightning" => other.who.MaxResource("Lightning") + "",
                "@otherlightning" => other.who.resources["Lightning"] + "",
                "@otherlightning%" => ((double)other.who.resources["Lightning"] / other.who.MaxResource("Lightning") * 100) + "",
                "@othermaxfrost" => other.who.MaxResource("Frost") + "",
                "@otherfrost" => other.who.resources["Frost"] + "",
                "@otherfrost%" => ((double)other.who.resources["Frost"] / other.who.MaxResource("Frost") * 100) + "",
                "@othermaxdecay" => other.who.MaxResource("Decay") + "",
                "@otherdecay" => other.who.resources["Decay"] + "",
                "@otherdecay%" => ((double)other.who.resources["Decay"] / other.who.MaxResource("Decay") * 100) + "",
                "@othermaxorder" => other.who.MaxResource("Order") + "",
                "@otherorder" => other.who.resources["Order"] + "",
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
