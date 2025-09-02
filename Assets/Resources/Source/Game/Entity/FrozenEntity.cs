using System.Collections.Generic;

using static Root;

public class FrozenEntity
{
    public FrozenEntity(Entity entity)
    {
        var participant = Board.board.participants.Find(x => x.who == entity);
        team = participant.team;
        human = participant.human;
        name = entity.name ?? entity.race;
        level = entity.level;
        weaponMod = entity.WeaponDamage("Melee");
        weaponModRanged = entity.WeaponDamage("Ranged");
        stats = entity.Stats();
        meleeAttackPower = entity.MeleeAttackPower();
        spellPower = entity.SpellPower();
        rangedAttackPower = entity.RangedAttackPower();
        physicalResistance = entity.PhysicalResistance();
        magicResistance = entity.MagicResistance();
        participantID = Board.board.participants.IndexOf(participant);
    }

    public double RollWeaponDamage(string type)
    {
        var damage = type == "Ranged" ? weaponModRanged : weaponMod;
        return random.Next((int)(damage.Item1 * 100), (int)(damage.Item2 * 100) + 1) / 100.0;
    }

    //Team of the entity
    public int team;

    //Whether controller of the entity is human controlled
    public bool human;

    //Name of the entity
    public string name;

    //Level of this entity
    public int level;

    //Weapon modifier
    public (double, double) weaponMod, weaponModRanged;

    //Powers of the entity
    public double meleeAttackPower, spellPower, rangedAttackPower;

    //Resistances of the entity
    public double physicalResistance, magicResistance;

    //ID of the participant
    public int participantID;

    //Full stats of the entity in that moment
    public Dictionary<string, int> stats;
}
