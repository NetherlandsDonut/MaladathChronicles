using System.Collections.Generic;

public class Class
{
    public Class(string name, List<(string, int)> abilities, List<TalentTree> talentTrees)
    {
        this.name = name;
        this.abilities = abilities;
        this.talentTrees = talentTrees;
    }

    public string name;
    public List<(string, int)> abilities;
    public List<TalentTree> talentTrees;

    public static List<Class> classes = new()
    {
        new Class("Rogue",
            new List<(string, int)>
            {
                ("One Handed Axe Proficiency", 1),
                ("One Handed Mace Proficiency", 1),
                ("One Handed Sword Proficiency", 1),
                ("Fist Weapon Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Dagger Proficiency", 1),
                ("Leather Proficiency", 1),
                ("Envenom", 1),
                ("Kidney Shot", 1),
                ("Mutilate", 1),
                ("Evasion", 1),
                ("Garrote", 1),
                ("Rupture", 1)
            },
            new List<TalentTree>
            {
                new TalentTree("Assassination", new List<Talent> { }),
                new TalentTree("Subtlety", new List<Talent> { }),
                new TalentTree("Outlaw", new List<Talent> { }),
            }
        ),
        new Class("Hunter",
            new List<(string, int)>
            {
                ("Two Handed Axe Proficiency", 1),
                ("Two Handed Sword Proficiency", 1),
                ("One Handed Axe Proficiency", 1),
                ("One Handed Sword Proficiency", 1),
                ("Polearm Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Quiver Proficiency", 1),
                ("Pouch Proficiency", 1),
                ("Bow Proficiency", 1),
                ("Crossbow Proficiency", 1),
                ("Gun Proficiency", 1),
                ("Leather Proficiency", 1),
                ("Mail Proficiency", 40),
            },
            new List<TalentTree>
            {
                new TalentTree("Marksmanship", new List<Talent> { }),
                new TalentTree("Beast Mastery", new List<Talent> { }),
                new TalentTree("Survival", new List<Talent> { }),
            }
        ),
        new Class("Shaman",
            new List<(string, int)>
            {
                ("Two Handed Axe Proficiency", 1),
                ("One Handed Axe Proficiency", 1),
                ("One Handed Mace Proficiency", 1),
                ("Polearm Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Totem Proficiency", 1),
                ("Leather Proficiency", 1),
                ("Mail Proficiency", 40),
            },
            new List<TalentTree>
            {
                new TalentTree("Elemental", new List<Talent> { }),
                new TalentTree("Enhancement", new List<Talent> { }),
                new TalentTree("Restoration", new List<Talent> { }),
            }
        ),
        new Class("Warrior",
            new List<(string, int)>
            {
                ("Two Handed Axe Proficiency", 1),
                ("Two Handed Mace Proficiency", 1),
                ("Two Handed Sword Proficiency", 1),
                ("One Handed Axe Proficiency", 1),
                ("One Handed Mace Proficiency", 1),
                ("One Handed Sword Proficiency", 1),
                ("Fist Weapon Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Polearm Proficiency", 1),
                ("Shield Proficiency", 1),
                ("Mail Proficiency", 1),
                ("Plate Proficiency", 40),
            },
            new List<TalentTree>
            {
                new TalentTree("Arms", new List<Talent> { }),
                new TalentTree("Fury", new List<Talent> { }),
                new TalentTree("Protection", new List<Talent> { }),
            }
        ),
        new Class("Paladin",
            new List<(string, int)>
            {
                ("Two Handed Axe Proficiency", 1),
                ("Two Handed Mace Proficiency", 1),
                ("Two Handed Sword Proficiency", 1),
                ("One Handed Axe Proficiency", 1),
                ("One Handed Mace Proficiency", 1),
                ("One Handed Sword Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Polearm Proficiency", 1),
                ("Libram Proficiency", 1),
                ("Shield Proficiency", 1),
                ("Mail Proficiency", 1),
                ("Plate Proficiency", 40),
            },
            new List<TalentTree>
            {
                new TalentTree("Holy", new List<Talent> { }),
                new TalentTree("Retribution", new List<Talent> { }),
                new TalentTree("Protection", new List<Talent> { }),
            }
        ),
        new Class("Druid",
            new List<(string, int)>
            {
                ("Two Handed Mace Proficiency", 1),
                ("One Handed Mace Proficiency", 1),
                ("Fist Weapon Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Polearm Proficiency", 1), 
                ("Dagger Proficiency", 1),
                ("Staff Proficiency", 1),
                ("Idol Proficiency", 1),
                ("Leather Proficiency", 1),
            },
            new List<TalentTree>
            {
                new TalentTree("Feral", new List<Talent> { }),
                new TalentTree("Balance", new List<Talent> { }),
                new TalentTree("Restoration", new List<Talent> { }),
            }
        ),
        new Class("Priest",
            new List<(string, int)>
            {
                ("One Handed Mace Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Dagger Proficiency", 1),
                ("Staff Proficiency", 1),
                ("Wand Proficiency", 1),
                ("Cloth Proficiency", 1),
            },
            new List<TalentTree>
            {
                new TalentTree("Holy", new List<Talent> { }),
                new TalentTree("Discipline", new List<Talent> { }),
                new TalentTree("Shadow", new List<Talent> { }),
            }
        ),
        new Class("Warlock",
            new List<(string, int)>
            {
                ("One Handed Sword Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Dagger Proficiency", 1),
                ("Staff Proficiency", 1),
                ("Wand Proficiency", 1),
                ("Cloth Proficiency", 1),
            },
            new List<TalentTree>
            {
                new TalentTree("Affliction", new List<Talent> { }),
                new TalentTree("Demonology", new List<Talent> { }),
                new TalentTree("Destruction", new List<Talent> { }),
            }
        ),
        new Class("Mage",
            new List<(string, int)>
            {
                ("One Handed Sword Proficiency", 1),
                ("Off Hand Proficiency", 1),
                ("Dagger Proficiency", 1),
                ("Staff Proficiency", 1),
                ("Wand Proficiency", 1),
                ("Cloth Proficiency", 1),
            },
            new List<TalentTree>
            {
                new TalentTree("Fire", new List<Talent>
                {
                    new Talent(0, 1, "Fireball", false, true),
                    new Talent(1, 0, "Scorch"),
                    new Talent(1, 1, "Hot Streak", true),
                    new Talent(1, 2, "Blazing Barrier"),
                    new Talent(2, 0, "Searing Touch", true),
                    new Talent(2, 1, "Fire Blast"),
                    new Talent(3, 0, "Pyroblast"),
                    new Talent(3, 1, "Fervent Flickering", true),
                    new Talent(3, 2, "Flamestrike"),
                    new Talent(4, 0, "Pyromaniac", true),
                    new Talent(4, 2, "Convection"),
                    new Talent(5, 1, "Hyperthermia"),
                }),
                new TalentTree("Frost", new List<Talent>
                {
                    new Talent(0, 1, "Frostbolt", false, true),
                    new Talent(1, 0, "Brain Freeze"),
                    new Talent(1, 1, "Freezing Nova"),
                    new Talent(1, 2, "Frost Ward"),
                    new Talent(2, 0, "Ice Lance"),
                    new Talent(2, 1, "Wintertide", true),
                    new Talent(2, 2, "Fingers Of Frost"),
                    new Talent(3, 0, "Ice Shards", true),
                    new Talent(3, 2, "Blizzard"),
                    new Talent(4, 0, "Splitting Ice", true),
                    new Talent(4, 1, "Deep Freeze"),
                    new Talent(4, 2, "Hailstones", true),
                    new Talent(5, 1, "Permafrost"),
                }),
                new TalentTree("Arcane", new List<Talent>
                {
                    new Talent(0, 1, "Arcane Missiles", false, true),
                    new Talent(1, 0, "Arcane Tempo"),
                    new Talent(1, 1, "Concetration"),
                    new Talent(1, 2, "Prismatic Barrier"),
                    new Talent(2, 0, "Amplification"),
                    new Talent(2, 2, "Arcane Barrage"),
                    new Talent(3, 0, "Foresight"),
                    new Talent(3, 1, "Resonance"),
                    new Talent(3, 2, "Reverberate", true),
                    new Talent(4, 0, "Slipstream"),
                    new Talent(4, 2, "Presence of Mind"),
                    new Talent(5, 1, "Supernova"),
                    new Talent(5, 2, "Prodigious Savant"),
                }),
            }
        ),
    };
}
