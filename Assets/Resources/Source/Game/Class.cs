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
                new TalentTree("Affliction", new List<Talent>
                {
                    new Talent(00, 1, "Corruption", false, true),
                    new Talent(01, 0, "Curse Of Agony"),
                    new Talent(01, 2, "Demon Skin"),
                    new Talent(02, 2, "Curse Of Weakness"),
                    new Talent(03, 1, "Drain Soul"),
                    new Talent(04, 1, "Agonizing Corruption"),
                    new Talent(04, 2, "Haunt"),
                    new Talent(05, 0, "Curse Of Tongues"),
                    new Talent(06, 2, "Creeping Death", true),
                    new Talent(07, 0, "Unstable Affliction"),
                    new Talent(07, 1, "Nightfall"),
                    new Talent(08, 2, "Kindled Malice"),
                    new Talent(08, 1, "Profane Bargain"),
                    new Talent(10, 0, "Xavius Gambit"),
                }),
                new TalentTree("Demonology", new List<Talent>
                {
                    new Talent(00, 1, "Summon Imp", false, true),
                    new Talent(01, 2, "Fel Armor"),
                    new Talent(02, 0, "Summon Voidwalker"),
                    new Talent(02, 1, "Health Funnel"),
                    new Talent(03, 1, "Summon Felhunter"),
                    new Talent(04, 1, "Demonic Embrace"),
                    new Talent(06, 1, "Summon Fel Guard"),
                    new Talent(07, 0, "Master Summoner"),
                }),
                new TalentTree("Destruction", new List<Talent>
                {
                    new Talent(00, 1, "Shadow Bolt", false, true),
                    new Talent(01, 0, "Immolate"),
                    new Talent(03, 0, "Pyrogenics", true),
                    new Talent(04, 0, "Chaos Bolt"),
                    new Talent(05, 1, "Rain Of Fire"),
                    new Talent(06, 1, "Eradication"),
                    new Talent(07, 1, "Havoc"),
                    new Talent(07, 2, "Ritual Of Ruin"),
                    new Talent(08, 1, "Summon Infernal"),
                    new Talent(09, 1, "Infernal Brand", true),
                    new Talent(10, 1, "Channel Demonfire"),
                }),
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
                ("Shadow Bolt", 1),
                ("Healing Wave", 1),
            },
            new List<TalentTree>
            {
                new TalentTree("Fire", new List<Talent>
                {
                    new Talent(00, 1, "Fireball", false, true),
                    new Talent(01, 0, "Scorch"),
                    new Talent(01, 1, "Hot Streak", true),
                    new Talent(01, 2, "Blazing Barrier"),
                    new Talent(02, 0, "Searing Touch", true),
                    new Talent(02, 1, "Fire Blast"),
                    new Talent(02, 2, "Flamestrike"),
                    new Talent(03, 0, "Fiery Rush"),
                    new Talent(03, 1, "Fervent Flickering", true),
                    new Talent(04, 0, "Pyroblast"),
                    new Talent(04, 2, "Flame Patch", true),
                    new Talent(05, 0, "Pyroclasm", true),
                    new Talent(05, 1, "Firemind"),
                    new Talent(05, 2, "Dragon\'s Breath"),
                    new Talent(06, 1, "Blast Wave"),
                    new Talent(06, 2, "Conflagration", true),
                    new Talent(07, 1, "Combustion"),
                    new Talent(07, 2, "Pyromaniac"),
                    new Talent(08, 0, "Convection"),
                    new Talent(08, 2, "Critical Mass"),
                    new Talent(09, 0, "Volatile Detonation"),
                    new Talent(09, 1, "Hyperthermia", true),
                    new Talent(10, 0, "Meteor"),
                    new Talent(10, 1, "Deep Impact"),
                    new Talent(10, 2, "Sun King\'s Blessing", true),
                }),
                new TalentTree("Frost", new List<Talent>
                {
                    new Talent(00, 1, "Frostbolt", false, true),
                    new Talent(01, 0, "Brain Freeze"),
                    new Talent(01, 1, "Freezing Nova"),
                    new Talent(01, 2, "Frost Ward"),
                    new Talent(02, 0, "Ice Lance"),
                    new Talent(02, 1, "Wintertide", true),
                    new Talent(02, 2, "Fingers Of Frost"),
                    new Talent(03, 0, "Ice Shards", true),
                    new Talent(03, 2, "Blizzard"),
                    new Talent(04, 0, "Splitting Ice", true),
                    new Talent(04, 1, "Frostbite"),
                    new Talent(04, 2, "Hailstones", true),
                    new Talent(05, 0, "Icy Flows"),
                    new Talent(05, 1, "Permafrost"),
                    new Talent(05, 2, "Frost Channeling"),
                    new Talent(06, 1, "Piercing Ice"),
                    new Talent(06, 2, "Ice Block"),
                    new Talent(07, 0, "Deep Freeze"),
                    new Talent(07, 2, "Flash Freeze", true),
                    new Talent(08, 1, "Icy Veins"),
                    new Talent(08, 2, "Perpetual Winter"),
                    new Talent(09, 0, "Cruel Winter", true),
                    new Talent(09, 1, "Deep Shatter"),
                    new Talent(10, 0, "Bone Chilling"),
                    new Talent(10, 1, "Cryopathy", true),
                }),
                new TalentTree("Arcane", new List<Talent>
                {
                    new Talent(00, 1, "Arcane Missiles", false, true),
                    new Talent(01, 0, "Arcane Tempo"),
                    new Talent(01, 1, "Concetration"),
                    new Talent(01, 2, "Prismatic Barrier"),
                    new Talent(02, 0, "Amplification"),
                    new Talent(02, 2, "Arcane Barrage"),
                    new Talent(03, 0, "Foresight"),
                    new Talent(03, 1, "Mana Adept"),
                    new Talent(03, 2, "Reverberate", true),
                    new Talent(04, 0, "Slipstream"),
                    new Talent(04, 2, "Presence of Mind"),
                    new Talent(05, 0, "Resonance", true),
                    new Talent(05, 2, "Prodigious Savant"),
                    new Talent(06, 0, "Concentrated Power"),
                    new Talent(06, 1, "Arcing Cleaves"),
                    new Talent(06, 2, "Evocation"),
                    new Talent(07, 1, "Cascading Power"),
                    new Talent(07, 2, "Enlightenment", true),
                    new Talent(08, 1, "Arcane Bombardment"),
                    new Talent(09, 0, "Nether Precision", true),
                    new Talent(09, 1, "Supernova"),
                    new Talent(09, 2, "Arcane Harmony", true),
                    new Talent(10, 1, "Time Warp"),
                    new Talent(10, 2, "Harmonic Echo", true),
                }),
            }
        ),
    };
}
