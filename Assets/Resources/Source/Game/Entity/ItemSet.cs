using System.Collections.Generic;
using System.Linq;

public class ItemSet
{
    public ItemSet(string name, List<SetBonus> setBonuses)
    {
        this.name = name;
        this.setBonuses = setBonuses;
    }

    public string name;
    public List<SetBonus> setBonuses;

    public int EquippedPieces(Entity entity)
    {
        return entity.equipment.Count(x => x.Value.set == name);
    }

    public static List<ItemSet> itemSets = new()
    {
        #region Paladins
        
        //ALLIANCE PVP SET
        new ItemSet("Field Marshal's Aegis", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE SET 
        new ItemSet("Lieutenant Commander's Redoubt", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //NAXXRAMAS SET
        new ItemSet("Redemption Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),
        
        //TEMPLE OF AHN'QIRAJ SET
        new ItemSet("Avenger's Battlegear", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //BLACKWING LAIR SET
        new ItemSet("Judgement Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ZUL'GURUB SET
        new ItemSet("Freethinker's Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //MOLTEN CORE SET
        new ItemSet("Lawbringer Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET UPGRADED
        new ItemSet("Soulforge Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET
        new ItemSet("Lightforge Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion
        
        #region Warriors
        
        //ALLIANCE PVP SET
        new ItemSet("Field Marshal's Battlegear", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE SET 
        new ItemSet("Lieutenant Commander's Battlearmor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP SET
        new ItemSet("Warlord's Battlegear", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE SET 
        new ItemSet("Champion's Battlearmor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //NAXXRAMAS SET
        new ItemSet("Dreadnaught's Battlegear", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),
        
        //TEMPLE OF AHN'QIRAJ SET
        new ItemSet("Conqueror's Battlegear", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //BLACKWING LAIR SET
        new ItemSet("Battlegear of Wrath", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ZUL'GURUB
        new ItemSet("Vindicator's Battlegear", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //MOLTEN CORE SET
        new ItemSet("Battlegear of Might", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET UPGRADED
        new ItemSet("Battlegear of Heroism", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET
        new ItemSet("Battlegear of Valor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion
        
        #region Shamans
        
        //HORDE PVP SET
        new ItemSet("Warlord's Earthshaker", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE SET 
        new ItemSet("Champion's Stormcaller", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //NAXXRAMAS SET
        new ItemSet("The Earthshatterer", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),
        
        //TEMPLE OF AHN'QIRAJ SET
        new ItemSet("Stormcaller's Garb", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //BLACKWING LAIR SET
        new ItemSet("The Ten Storms", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ZUL'GURUB SET
        new ItemSet("Augur's Regalia", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //MOLTEN CORE SET
        new ItemSet("The Earthfury", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET UPGRADED
        new ItemSet("The Five Thunders", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET
        new ItemSet("The Elements", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion

        #region Hunters
        
        //ALLIANCE PVP SET
        new ItemSet("Field Marshal's Pursuit", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE SET 
        new ItemSet("Lieutenant Commander's Pursuance", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE OLD SET 
        new ItemSet("Lieutenant Commander's Pursuit", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP SET
        new ItemSet("Warlord's Pursuit", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE SET 
        new ItemSet("Champion's Pursuance", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE OLD SET 
        new ItemSet("Champion's Pursuit", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //NAXXRAMAS SET
        new ItemSet("Cryptstalker Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),
        
        //TEMPLE OF AHN'QIRAJ SET
        new ItemSet("Striker's Garb", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //BLACKWING LAIR SET
        new ItemSet("Dragonstalker Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ZUL'GURUB SET
        new ItemSet("Predator's Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //MOLTEN CORE SET
        new ItemSet("Giantstalker Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET UPGRADED
        new ItemSet("Beastmaster Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET
        new ItemSet("Beaststalker Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion
        
        #region Rogues
        
        //ALLIANCE PVP SET
        new ItemSet("Field Marshal's Vestments", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE SET 
        new ItemSet("Lieutenant Commander's Guard", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE OLD SET 
        new ItemSet("Lieutenant Commander's Vestments", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP SET
        new ItemSet("Warlord's Vestments", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE SET 
        new ItemSet("Champion's Guard", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE OLD SET 
        new ItemSet("Champion's Vestments", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //NAXXRAMAS SET
        new ItemSet("Bonescythe Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),
        
        //TEMPLE OF AHN'QIRAJ SET
        new ItemSet("Deathdealer's Embrace", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //BLACKWING LAIR SET
        new ItemSet("Bloodfang Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ZUL'GURUB SET
        new ItemSet("Madcap's Outfit", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //MOLTEN CORE SET
        new ItemSet("Nightslayer Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET UPGRADED
        new ItemSet("Darkmantle Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET
        new ItemSet("Shadowcraft Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion
        
        #region Druids
        
        //ALLIANCE PVP SET
        new ItemSet("Field Marshal's Sanctuary", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE SET 
        new ItemSet("Lieutenant Commander's Refuge", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE OLD SET 
        new ItemSet("Lieutenant Commander's Sanctuary", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP SET
        new ItemSet("Warlord's Sanctuary", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE SET 
        new ItemSet("Champion's Refuge", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE OLD SET 
        new ItemSet("Champion's Sanctuary", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //NAXXRAMAS SET
        new ItemSet("Dreamwalker Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),
        
        //TEMPLE OF AHN'QIRAJ SET
        new ItemSet("Genesis Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //BLACKWING LAIR SET
        new ItemSet("Stormrage Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ZUL'GURUB SET
        new ItemSet("Haruspex's Garb", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //MOLTEN CORE SET
        new ItemSet("Cenarion Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET UPGRADED
        new ItemSet("Feralheart Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET
        new ItemSet("Wildheart Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion
        
        #region Mages

        //ALLIANCE PVP SET
        new ItemSet("Field Marshal's Regalia", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE SET 
        new ItemSet("Lieutenant Commander's Arcanum", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE OLD SET 
        new ItemSet("Lieutenant Commander's Regalia", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP SET
        new ItemSet("Warlord's Regalia", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE SET 
        new ItemSet("Champion's Arcanum", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE OLD SET 
        new ItemSet("Champion's Regalia", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //NAXXRAMAS SET
        new ItemSet("Frostfire Regalia", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),
        
        //TEMPLE OF AHN'QIRAJ SET
        new ItemSet("Enigma Vestments", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //BLACKWING LAIR SET
        new ItemSet("Netherwind Regalia", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ZUL'GURUB SET
        new ItemSet("Illusionist's Attire", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //MOLTEN CORE SET
        new ItemSet("Arcanist Regalia", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET UPGRADED
        new ItemSet("Sorcerer's Regalia", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET
        new ItemSet("Magister's Regalia", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion
        
        #region Warlocks
        
        //ALLIANCE PVP SET
        new ItemSet("Field Marshal's Threads", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE SET
        new ItemSet("Lieutenant Commander's Dreadgear", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE OLD SET
        new ItemSet("Lieutenant Commander's Threads", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP SET
        new ItemSet("Warlord's Threads", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE SET
        new ItemSet("Champion's Dreadgear", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE OLD SET
        new ItemSet("Champion's Threads", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //NAXXRAMAS SET
        new ItemSet("Plagueheart Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),
        
        //TEMPLE OF AHN'QIRAJ SET
        new ItemSet("Doomcaller's Attire", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //BLACKWING LAIR SET
        new ItemSet("Nemesis Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ZUL'GURUB SET
        new ItemSet("Demoniac's Threads", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //MOLTEN CORE SET
        new ItemSet("Felheart Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET UPGRADED
        new ItemSet("Deathmist Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET
        new ItemSet("Dreadmist Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion
        
        #region Priests
        
        //ALLIANCE PVP SET
        new ItemSet("Field Marshal's Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE SET
        new ItemSet("Lieutenant Commander's Investiture", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ALLIANCE PVP BASE OLD SET
        new ItemSet("Lieutenant Commander's Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP SET
        new ItemSet("Warlord's Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE SET
        new ItemSet("Champion's Investiture", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //HORDE PVP BASE OLD SET
        new ItemSet("Champion's Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //NAXXRAMAS SET
        new ItemSet("Vestments of Faith", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //TEMPLE OF AHN'QIRAJ SET
        new ItemSet("Garments of the Oracle", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //BLACKWING LAIR SET
        new ItemSet("Vestments of Transcendence", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //ZUL'GURUB SET
        new ItemSet("Confessor's Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //MOLTEN CORE SET
        new ItemSet("Vestments of Prophecy", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET UPGRADED
        new ItemSet("Vestments of the Virtuous", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET
        new ItemSet("Vestments of the Devout", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion

        #region General Plate
        
        //BLACKSMITHING SET
        new ItemSet("Imperial Plate", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //SCHOLOMANCE SET
        new ItemSet("Deathbone Guardian", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //SCARLET MONASTERY SET
        new ItemSet("Chain of the Scarlet Crusade", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //UNDEAD EVENT SET
        new ItemSet("Battlegear of Undead Slaying", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //?
        new ItemSet("The Gladiator", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion

        #region General Mail

        //SCHOLOMANCE SET
        new ItemSet("Bloodmail Regalia", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //LEATHERWORKING SET
        new ItemSet("Black Dragon Mail", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //LEATHERWORKING SET
        new ItemSet("Blue Dragon Mail", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //LEATHERWORKING SET
        new ItemSet("Green Dragon Mail", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //UNDEAD EVENT SET
        new ItemSet("Garb of the Undead Slayer", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion

        #region General Leather

        //SCHOLOMANCE SET
        new ItemSet("Cadaverous Garb", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //LEATHERWORKING SET
        new ItemSet("Stormshroud Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //LEATHERWORKING SET
        new ItemSet("Devilsaur Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //LEATHERWORKING SET
        new ItemSet("Volcanic Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //LEATHERWORKING SET
        new ItemSet("Ironfeather Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //LEATHERWORKING SET
        new ItemSet("Blood Tiger Harness", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { })
        }),

        //DUNGEON SET
        new ItemSet("Defias Leather", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //DUNGEON SET
        new ItemSet("Embrace of the Viper", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //UNDEAD EVENT SET
        new ItemSet("Undead Slayer's Armor", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion

        #region General Cloth

        //SCHOLOMANCE SET
        new ItemSet("Ironweave Battlesuit", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //SCHOLOMANCE SET
        new ItemSet("Necropile Raiment", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //STRATHOLME SPECIAL SET
        new ItemSet("The Postmaster", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        //UNDEAD EVENT SET
        new ItemSet("Regalia of Undead Cleansing", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { }), new SetBonus(5, new() { "+15 Spirit" }, new() { })
        }),

        #endregion

        #region Other Sets

        //RAID TRINKET SET
        new ItemSet("Shard of the Gods", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { })
        }),

        //ZUL'GURUB WEAPON SET
        new ItemSet("The Twin Blades of Hakkari", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { })
        }),

        //BLACKROCK SPIRE WEAPON SET
        new ItemSet("Spider's Kiss", new()
        {
            new SetBonus(2, new() { "+10 Stamina" }, new() { })
        }),

        #endregion
    };
}

public class SetBonus
{
    public SetBonus(int requiredPieces, List<string> description, List<string> abilitiesProvided)
    {
        this.requiredPieces = requiredPieces;
        this.description = description;
        this.abilitiesProvided = abilitiesProvided;
    }

    public int requiredPieces;
    public List<string> description;
    public List<string> abilitiesProvided;
}
