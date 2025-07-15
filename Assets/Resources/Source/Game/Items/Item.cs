using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static Root;
using static Sound;
using static ItemSet;
using static Defines;
using static SaveGame;
using static SiteTown;
using static Coloring;
using static Inventory;
using static GameSettings;
using static PermanentEnchant;

public class Item
{
    //Initialisation method to fill automatic values
    //and remove empty collections to avoid serialising them later
    public void Initialise()
    {
        if (set != null)
            if (!itemSets.Exists(x => x.name == set))
                itemSets.Insert(0, new ItemSet()
                {
                    name = set,
                    setBonuses = new()
                });
        if (abilities != null)
            foreach (var ability in abilities)
                if (!Ability.abilities.Exists(x => x.name == ability.Key))
                    Ability.abilities.Insert(0, new Ability()
                    {
                        name = ability.Key,
                        icon = "Ability" + ability,
                        events = new(),
                        tags = new()
                    });
        if (name == "Nexus Crystal")
        {
            var names = "Cenarion Spaulders, Felheart Gloves, Arcanist Mantle, Felheart Shoulder Pads, Nightslayer Shoulder Pads, Nightslayer Gloves, Pants of Prophecy, Shadowstrike, Felheart Slippers, Arcanist Gloves, Giantstalker's Boots, Gloves of Prophecy, Robes of Prophecy, Felheart Horns, Pauldrons of Might, Circlet of Prophecy, Flamewaker Legplates, Manastorm Leggings, Nightslayer Boots, Seal of the Gurubashi Berserker, Arcanist Boots, Boots of Prophecy, Gauntlets of Might, Mantle of Prophecy, Nightslayer Cover, Nemesis Leggings, Sash of Whispered Secrets, Arcanist Leggings, Nightslayer Chestpiece, Core Forged Greaves, Arcane Infused Gem, Arcanist Crown, Black Brood Pauldrons, Eskhandar's Right Claw, Giantstalker's Helmet, Girdle of the Fallen Crusader, Gloves of Rapid Evolution, Stormrage Pauldrons, Jeklik's Crusher, Sabatons of Might, Breastplate of Might, Fire Runed Grimoire, Sorcerous Dagger, Stormrage Handguards, Medallion of Steadfast Might, Taut Dragonhide Gloves, Aegis of Preservation, Emberweave Leggings, Netherwind Boots, Primalist's Linked Waistguard, Shimmering Geta, Aurastone Hammer, Netherwind Belt, Stormrage Boots, Taut Dragonhide Shoulderpads, Fang of Venoxis, Judgement Bindings, Nemesis Gloves, Sapphiron Drape, Stormrage Bracers, The Black Book, Venomous Totem, Dragonstalker's Gauntlets, Elementium Threaded Cloak, Interlaced Shadow Jerkin, Stormrage Belt, Bloodfang Bracers, Dragonstalker's Belt, Dragonstalker's Greaves, Hyperthermically Insulated Lava Dredger, Nemesis Belt, Netherwind Robes, Aegis of the Blood God, Bindings of Transcendence, Bloodcaller, Bloodfang Belt, Choker of Enlightenment, Draconic Avenger, Dragonstalker's Spaulders, Essence of the Pure Flame, Judgement Spaulders, Nemesis Boots, Pauldrons of Transcendence, Therazane's Link, Bloodsoaked Legplates, Primalist's Linked Legguards, Stormrage Chestguard, Taut Dragonhide Belt, Band of Sulfuras, Bloodfang Gloves, Dragonstalker's Breastplate, Judgement Sabatons, Malfurion's Blessed Bulwark, Nemesis Bracers, Nemesis Robes, Netherwind Gloves, Rune of Metamorphosis, Band of Servitude, Black Ash Robe, Gutgore Ripper, Bloodfang Spaulders, Bloodlord's Defender, Boots of Transcendence, Dragonstalker's Bracers, Mana Igniting Cord, Nemesis Spaulders, Vambraces of Prophecy, Bloodfang Boots, Bloodfang Chestpiece, Cenarion Belt, Cenarion Bracers, Judgement Gauntlets, Netherwind Mantle, Obsidian Edged Blade, Staff of Dominance, Belt of Transcendence, Brain Hacker, Greaves of Ten Storms, Judgement Belt, Breastplate of Ten Storms, Pendant of the Fallen Dragon, Drillborer Disk, Girdle of Prophecy, Handguards of Transcendence, Will of Arlokk, Belt of Ten Storms, Felheart Bracers, Hide of the Wild, Pauldrons of Wrath, Robes of Transcendence, Bracers of Might, Heartstriker, Jin'do's Hexxer, Band of Dark Dominion, Draconic Maul, Essence Gatherer, Netherwind Bindings, Waistband of Wrath, Bracelets of Wrath, Felheart Belt, Barrage Shoulders, Belt of the Fallen Emperor, Boots of the Unwavering Will, Charm of the Shifting Sands, Gauntlets of New Life, Gauntlets of the Immovable, Gloves of Dark Wisdom, Gloves of Ebru, Gloves of the Swarm, Hammer of Ji'zhi, Helm of Domination, Hive Tunneler's Boots, Jin'do's Evil Eye, Leggings of Immersion, Mantle of the Horusath, Ring of Blackrock, Runic Stone Shoulders, Shroud of Pure Thought, Vek'lor's Gloves of Devastation";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Strange Dust")
        {
            var names = "Simple Blouse, Simple Branch, Bard's Belt, Battle Chain Pants, Blue Linen Robe, Journeyman's Robe, War Torn Pants, Willow Cape, Bingles' Flying Gloves, Ceremonial Buckler, Charger's Armor, Infantry Shield, Lupine Cord, Mystic's Cape, Banshee Armor, Bloodspattered Gloves, Burnt Leather Breeches, Disciple's Robe, Friar's Robes of the Light, Red Linen Robe, Battle Chain Tunic, Brackwater Boots, Burnt Leather Vest, Cadet Leggings, Cadet Shield, Grizzly Pants, Gustweald Cloak, Gypsy Tunic, Hunting Buckler, Journeyman's Stave, Native Branch, Orcish War Chain, Veteran Armor, Veteran Shield, Willow Boots, Aboriginal Sash, Ancestral Woollies, Barbaric Cloth Gloves, Beaded Britches, Bloodspattered Sash, Brackwater Shield, Brown Linen Robe, Cadet Vest, Ceremonial Leather Gloves, Cinched Belt, Disciple's Stein, Fortified Bracers, Gypsy Trousers, Harvester's Pants, Harvester's Robe, Infantry Leggings, Infantry Tunic, Pioneer Trousers, Simple Britches, Tribal Pants, Tribal Vest, Ancestral Robe, Ancestral Tunic, Bard's Bracers, Bloodspattered Wristbands, Blue Linen Vest, Defender Tunic, Disciple's Pants, Feral Cord, Gold-flecked Gloves, Grizzly Buckler, Handstitched Linen Britches, Journeyman's Pants, Rancher's Trousers, Shimmering Gloves, Tear of Grief, War Torn Shield, Warrior's Pants, Aboriginal Loincloth, Bandit Boots, Bard's Buckler, Burnished Girdle, Ceremonial Leather Loincloth, Crystalline Cuffs, Defender Bracers, Defender Gauntlets, Disciple's Vest, Forest Leather Mantle, Ghastly Trousers, Gypsy Buckler, Horn Ring, Ivycloth Bracelets, Mystic's Slippers, Native Pants, Pioneer Tunic, Scouting Boots, Simple Robe, Soldier's Shield, White Linen Robe, Willow Vest, Aboriginal Footwraps, Bandit Pants, Barbaric Linen Vest, Bounty Hunter's Ring, Brackwater Leggings, Dark Leather Belt, Defender Leggings, Feral Cloak, Forest Cloak, Forest Leather Boots, Greenweave Cloak, Lupine Buckler, Raider's Chestpiece, Runic Stave, Scouting Belt, Shimmering Cloak, Shimmering Trousers, Willow Bracers, Barbaric Cloth Boots, Black Whelp Boots, Bright Boots, Bright Cloak, Buccaneer's Boots, Buccaneer's Bracers, Buccaneer's Cape, Feral Bindings, Forest Leather Belt, Heavy Woolen Pants, Inscribed Cloak, Inscribed Leather Breastplate, Mystic's Gloves, Pagan Bands, Raider's Belt, Raider's Gauntlets, Scouting Tunic, Seer's Boots, Seer's Cuffs, Seer's Gloves, Shimmering Sash, Smelting Pants, Soldier's Gauntlets, Soldier's Girdle, Totem of Infliction, Woodworking Gloves, Bandit Gloves, Bandit Jerkin, Bard's Boots, Bard's Gloves, Barkeeper's Cloak, Bright Belt, Buccaneer's Gloves, Gloves of the Moon, Inscribed Leather Belt, Inscribed Leather Boots, Inscribed Leather Gloves, Lambent Scale Cloak, Mystic's Bracelets, Pagan Cape, Raider's Boots, Rough Bronze Leggings, Scouting Cloak, Soft-soled Linen Boots, Stonemason Cloak, Superior Belt, Superior Cloak, Willow Pants, Bandit Cloak, Bard's Trousers, Bard's Tunic, Buccaneer's Pants, Defender Cloak, Defias Renegade Ring, Fortified Belt, Heavy Woolen Gloves, Inscribed Leather Pants, Ivycloth Boots, Ivycloth Cloak, Raider's Bracers, Raider's Legguards, Scouting Bracers, Shimmering Bracers, Soldier's Boots, Willow Gloves, Apothecary Gloves, Bandit Cinch, Barbaric Loincloth, Bloody Apron, Buccaneer's Cord, Burnished Cloak, Fortified Gauntlets, Inscribed Leather Bracers, Ritual Bands, Seer's Pants, Settler's Leggings, Soldier's Armor, Soldier's Leggings, Willow Belt, Aboriginal Gloves, Bandit Bracers, Raider's Cloak, Scouting Trousers, Seer's Belt, Shimmering Boots, Spellbinder Pants";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Soul Dust")
        {
            var names = "Raincaller Boots, Swampland Trousers, Bright Mantle, Dwarven Guard Cloak, Hexed Bracers, Phalanx Girdle, Ring of Calm, Bright Robe, Combat Cloak, Dervish Gloves, Elder's Bracers, Emblazoned Boots, Forest Leather Chestpiece, Rigid Tunic, Ring of Forlorn Spirits, Silver-thread Sash, Silvered Bronze Boots, Basalt Ring, Chestnut Mantle, Durable Robe, Gemmed Gloves, Heart Ring, Insignia Buckler, Lambent Scale Legguards, Repairman's Cape, Sage's Boots, Sage's Sash, Silver-thread Cloak, Silver-thread Gloves, Superior Shoulders, Chrome Ring, Darkshire Mail Leggings, Dervish Bracers, Elder's Sash, Enchanter's Cowl, Girdle of the Blindwatcher, Gnomebot Operating Boots, Green Iron Leggings, Greenweave Vest, Infiltrator Gloves, Infiltrator Shoulders, Insignia Boots, Ivycloth Robe, Lambent Scale Bracers, Mail Combat Boots, Moonlit Amice, Nightsky Boots, Nightsky Cowl, Raincaller Cord, Renegade Bracers, Scaled Leather Bracers, Banded Girdle, Banded Helm, Barbaric Shoulders, Battleforge Gauntlets, Battleforge Shoulderguards, Cerulean Ring, Dervish Belt, Dervish Cape, Durable Cape, Durable Pants, Elder's Hat, Greenweave Mantle, Greenweave Robe, High Apothecary Cloak, Hillman's Shoulders, Infiltrator Bracers, Infiltrator Cord, Knight's Cloak, Lambent Scale Boots, Lesser Wizard's Robe, Malleable Chain Leggings, Plainsguard Leggings, Prelacy Cape, Renegade Belt, Sage's Circlet, Silver-thread Pants, Superior Bracers, Superior Tunic, Wolfmaster Cape, Archer's Bracers, Archer's Cloak, Archer's Gloves, Azure Silk Cloak, Battleforge Legguards, Battleforge Wristguards, Black Wolf Bracers, Bright Pants, Bright Sphere, Conjurer's Cloak, Conjurer's Shoes, Dervish Boots, Dervish Leggings, Dervish Tunic, Desert Shoulders, Durable Bracers, Durable Gloves, Emblazoned Belt, Emblazoned Cloak, Eye of Paleth, Forest Leather Pants, Glimmering Mail Gauntlets, Gloves of Meditation, Grizzled Boots, Insignia Belt, Inventor's League Ring, Ivycloth Mantle, Jacinth Circle, Lambent Scale Gloves, Mail Combat Armguards, Mail Combat Belt, Mantle of Honor, Phalanx Boots, Renegade Gauntlets, Sage's Pants, Scaled Cloak, Shepherd's Girdle, Shredder Operating Gloves, Silk Mantle of Gamn, Superior Gloves, Zodiac Gloves, Amber Hoop, Banded Armor, Battleforge Girdle, Crest of Darkshire, Dog Training Gloves, Durable Belt, Elder's Padded Armor, Emblazoned Leggings, Insignia Cloak, Ivycloth Sash, Mail Combat Gauntlets, Mail Combat Headguard, Nightsky Mantle, Nightsky Sash, Phalanx Gauntlets, Renegade Cloak, Sage's Mantle, Spinel Ring, Superior Buckler, Superior Leggings, Tundra Boots, Windsong Drape, Archer's Belt, Azure Silk Pants, Bloodmage Mantle, Durable Hat, Durable Tunic, Elder's Pants, Elder's Robe, Engineer's Cloak, Ghostly Mantle, Hillman's Leather Gloves, Infiltrator Pants, Insulated Sage Gloves, Ivycloth Gloves, Nightsky Cloak, Phalanx Bracers, Phalanx Breastplate, Rose Mantle, Sage's Bracers, Scaled Leather Leggings, Shield of the Faith, Spritekin Cloak, Torch of Holy Flame, Yeti Fur Cloak, Azure Silk Vest, Band of the Undercity, Banded Bracers, Durable Boots, Durable Shoulders, Elder's Cloak, Elder's Gloves, Emblazoned Bracers, Fenrus' Hide, Insignia Gloves, Insignia Leggings, Lambent Scale Girdle, Mail Combat Spaulders, Sage's Cloak, Sage's Gloves, Silver-thread Cuffs, Twilight Cape, Wildhunter Cloak, Elder's Mantle, Emblazoned Shoulders, Fortified Spaulders, Infiltrator Armor, Scaled Leather Belt, Battleforge Cloak, Beastmaster's Girdle, Silver-thread Boots, Superior Boots, Talbar Mantle, Bloodbone Band, Conjurer's Bracers, Conjurer's Cinch, Ivycloth Pants";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Vision Dust")
        {
            var names = "Wolf Rider's Wristbands, Thallium Hoop, Warbringer's Sabatons, Worn Running Boots, Captain's Cloak, Champion's Cape, Razzeric's Customized Seatbelt, Brigade Circlet, Chief Brigadier Boots, Darkmist Pants, Embossed Plate Gauntlets, Gallan Cuffs, Sentinel Girdle, Glyphed Cloak, Huntsman's Bands, Lunar Belt, Mistscape Cloak, Regal Robe, Sorcerer Bracelets, Aurora Gloves, Blackwater Tunic, Branded Leather Bracers, Brutish Armguards, Explorers' League Commendation, Glyphed Buckler, Gothic Sabatons, Imposing Gloves, Medicine Blanket, Nightsky Armor, Regal Gloves, Silksand Star, Thallium Choker, Wolf Rider's Shoulder Pads, Captain's Waistguard, Champion's Bracers, Conjurer's Robe, Glyphed Bracers, Jaina's Signet Ring, Jazeraint Belt, Jazeraint Bracers, Khan's Bindings, Knight's Boots, Knight's Headguard, Kodo Rustler Boots, Mail Combat Leggings, Mistscape Gloves, Ranger Cloak, Ravager's Armor, Regal Wizard Hat, Royal Bands, Sorcerer Sash, Southsea Mojo Boots, Tracker's Headband, Tyrant's Epaulets, Visionary Buckler, Amethyst Band, Archer's Cap, Archer's Shoulderpads, Bonefingers, Brigade Girdle, Captain's Bracers, Conjurer's Hood, Glyphed Belt, Gossamer Bracers, Gossamer Cape, Huntsman's Boots, Knight's Bracers, Marsh Ring, Ranger Gloves, Ravager's Mantle, Regal Cloak, Royal Sash, Scarlet Belt, Scarlet Gauntlets, Symbolic Crown, Windchaser Cinch, Windchaser Footpads, Aurora Bracers, Aurora Cloak, Belt of Corruption, Black Mageweave Robe, Black Mageweave Vest, Bloodwoven Mitts, Bonelink Gauntlets, Captain Rackmore's Wheel, Chief Brigadier Bracers, Conjurer's Breeches, Darkmist Wizard Hat, Falcon's Hook, Glyphed Helm, Knight's Gauntlets, Kodobone Necklace, Lodestone Necklace, Mail Combat Armor, Mantle of Doan, Nightsky Trousers, Ranger Wristguards, Renegade Chestguard, Renegade Circlet, Scarlet Wristguards, Scorpashi Wristbands, Sentinel Boots, Sentinel Trousers, Shadowy Bracers, Shriveled Heart, Twilight Boots, Archer's Jerkin, Black Mageweave Gloves, Bloodwoven Cord, Cabalist Bracers, Cabalist Cloak, Chief Brigadier Cloak, Conjurer's Gloves, Durtfeet Stompers, Glyphed Epaulets, Glyphed Mitts, Heavy Mithril Gauntlet, Heraldic Cloak, Huntsman's Belt, Insignia Chestguard, Jungle Boots, Knight's Pauldrons, Long Silken Cloak, Lunar Coronet, Lunar Handwraps, Lunar Mantle, Panther Hunter Leggings, Raptor Hunter Tunic, Regal Boots, Regal Sash, Renegade Boots, Renegade Pauldrons, Royal Boots, Royal Cape, Royal Gloves, Seawolf Gloves, Sentinel Bracers, Silkstream Cuffs, Sorcerer Slippers, Tiger Hunter Gloves, Tracker's Boots, Tyrant's Gauntlets, Warmonger's Cloak, White Bandit Mask, Archer's Boots, Aurora Boots, Bloodwoven Cloak, Brigade Breastplate, Conjurer's Mantle, Embossed Plate Armor, Green Silken Shoulders, Imposing Bracers, Jazeraint Cloak, Regal Leggings, Renegade Leggings, Robe of Doan, Sentinel Cloak, Twilight Gloves, Windchaser Handguards, Archer's Trousers, Black Mageweave Leggings, Darkmist Mantle, Darktide Cape, Dusty Mail Boots, Embossed Plate Leggings, Excelsior Boots, Hematite Link, Inquisitor's Shawl, Nightscape Headband, Sentinel Shoulders, Shinkicker Boots, Sorcerer Hat, Tracker's Belt, Twilight Pants, Aurora Sash, Bloodwoven Bracers, Doomsayer's Robe, Gossamer Boots, Huntsman's Cape, Iridium Circle, Regal Cuffs, Sentinel Cap, Sentinel Gloves, Sorcerer Cloak, Tracker's Gloves, Twilight Cowl, Twilight Mantle, Windchaser Cuffs, Gothic Plate Girdle, Gothic Plate Vambraces, Royal Amice, Royal Headband, Scorching Sash, Twilight Belt, Twilight Cuffs, Knight's Girdle";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Dream Dust")
        {
            var names = "Coldwater Ring, Royal Gown, Vanguard Sabatons, Alabaster Plate Girdle, Impenetrable Bindings, Bloodwoven Pads, Champion's Armor, Champion's Girdle, Heavy Lamellar Helm, Royal Blouse, Swashbuckler's Boots, Heavy Lamellar Girdle, Khan's Belt, Lodestone Hoop, Lofty Gauntlets, Merciless Belt, Warmonger's Gauntlets, Abjurer's Boots, Breezecloud Bracers, Chieftain's Headdress, Ebonhold Wristguards, Gaea's Handwraps, Glorious Bindings, Gossamer Headpiece, Gothic Plate Spaulders, Imperial Red Pants, Merciless Epaulets, Warmonger's Chestpiece, Abjurer's Cloak, Abjurer's Mantle, Arcane Cloak, Bloodlust Bracelets, Champion's Gauntlets, Champion's Helmet, Chieftain's Breastplate, Councillor's Sash, Earth Warder's Vest, Gossamer Gloves, Gossamer Pants, Heavy Lamellar Chestpiece, Imperial Red Bracers, Keeper's Wreath, Lord's Cape, Mystical Mantle, Nightscape Boots, Peerless Cloak, Pridelord Girdle, Rageclaw Belt, Righteous Spaulders, Templar Boots, Cabalist Leggings, Crusader's Belt, Crusader's Helm, Dalson Family Wedding Ring, Duskwoven Amice, Ebonhold Helmet, Emerald Girdle, Felstone Good Luck Charm, Gemshale Pauldrons, Gothic Plate Armor, Heavy Lamellar Pauldrons, Imperial Red Boots, Imperial Red Cloak, Imperial Red Gloves, Imperial Red Mantle, Lord's Crown, Lord's Gauntlets, Opulent Leggings, Resplendent Cloak, Revenant Girdle, Revenant Shoulders, Righteous Gloves, Righteous Helmet, Selenium Chain, Serpentskin Girdle, Swashbuckler's Bracers, Swashbuckler's Eyepatch, Templar Gauntlets, Wicked Leather Gauntlets, Abjurer's Bands, Abjurer's Hood, Abjurer's Robe, Alabaster Plate Pauldrons, Bloodband Bracers, Bonecaster's Boots, Cabalist Boots, Cabalist Chestpiece, Cabalist Gloves, Councillor's Gloves, Crusader's Armor, Hakkari Shroud, Heavy Lamellar Gauntlets, Highborne Gloves, Highborne Pauldrons, Lord's Breastplate, Lord's Girdle, Marble Necklace, Mystical Belt, Mystical Gloves, Ornate Bracers, Ornate Girdle, Overlord's Spaulders, Praetorian Girdle, Righteous Boots, Royal Trousers, Selenium Loop, Serpentskin Helm, Shadowy Belt, Slagplate Gauntlets, Wanderer's Gloves, Windchaser Coronet, Zorbin's Water Resistant Hat, Black Mageweave Headband, Bonecaster's Belt, Chieftain's Cloak, Chieftain's Gloves, Councillor's Cuffs, Crusader's Gauntlets, Duskwoven Cape, Duskwoven Pants, Duskwoven Sandals, Ebonhold Gauntlets, Engraved Cape, Gaea's Cuffs, Gossamer Belt, Gothic Plate Helmet, Heavy Lamellar Boots, Heavy Lamellar Leggings, Heavy Lamellar Vambraces, Highborne Cord, Lord's Boots, Nightscape Pants, Opulent Crown, Overlord's Crown, Praetorian Pauldrons, Revenant Chestplate, Revenant Gauntlets, Righteous Bracers, Righteous Waistguard, Runecloth Belt, Runecloth Gloves, Swashbuckler's Gloves, Templar Girdle, Wanderer's Belt, Wanderer's Cloak, Abjurer's Pants, Bonecaster's Bindings, Celestial Cape, Crusader's Armguards, Crusader's Cloak, Crusader's Leggings, Duskwoven Gloves, Leggings of the Ursa, Mystical Boots, Ornate Cloak, Overlord's Legplates, Swashbuckler's Cape, Templar Bracers, Wanderer's Bracers, Warmonger's Belt, Abjurer's Sash, Archaeologist's Quarry Boots, Black Mageweave Shoulders, Champion's Greaves, Chieftain's Bracers, Chieftain's Leggings, Crusader's Pauldrons, Duskwoven Bracers, Duskwoven Turban, Gossamer Shoulderpads, Imperial Plate Bracers, Mark of Hakkar, Mystical Cape, Mystical Headwrap, Overlord's Girdle, Overlord's Vambraces, Revenant Helmet, Righteous Armor, Righteous Cloak, Rugwood Mantle, Seared Mail Girdle, Swashbuckler's Belt, Swashbuckler's Shoulderpads, Wicked Leather Bracers, Abjurer's Gloves, Cabalist Belt, Councillor's Cloak, Duskwoven Tunic, Mystical Bracers, Tracker's Leggings, Traveler's Cloak, Warmonger's Bracers, Cabalist Helm, Champion's Pauldrons, Marble Circle, Righteous Leggings, Gothic Plate Gauntlets, Gothic Plate Leggings, Cabalist Spaulders";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Illusion Dust")
        {
            var names = "Abyssal Mail Handguards, Fangdrip Runners, Abyssal Leather Boots, Pridelord Pants, Resplendent Belt, Granite Necklace, Masterwork Circlet, Nightshade Helmet, Pridelord Boots, Adventurer's Belt, Adventurer's Bracers, Carrion Scorpid Helm, Elunarian Cuffs, Emerald Sabatons, Grand Armguards, Hero's Boots, Hero's Bracers, Hyena Hide Jerkin, Hyperion Legplates, Master's Boots, Master's Bracers, Masterwork Breastplate, Mighty Girdle, Mud Stained Boots, Abyssal Leather Gloves, Abyssal Plate Gauntlets, Abyssal Plate Greaves, Adventurer's Gloves, Bonecaster's Sarong, Celestial Belt, Celestial Handwraps, Celestial Slippers, Councillor's Shoulders, Ebonhold Leggings, Elegant Cloak, Emerald Breastplate, Emerald Gauntlets, Emerald Pauldrons, Hero's Band, Hyperion Girdle, Master's Belt, Master's Cloak, Master's Hat, Master's Mantle, Masterwork Gauntlets, Mercurial Bracers, Mighty Helmet, Obsidian Pendant, Oddly Magical Belt, Shaggy Leggings, Templar Legplates, Traveler's Spaulders, Wanderer's Leggings, Wanderer's Shoulders, Alabaster Breastplate, Alabaster Plate Leggings, Band of Vigor, Breakwater Legguards, Celestial Pauldrons, Clever Hat, Commander's Helm, Commander's Pauldrons, Commander's Vambraces, Elegant Belt, Elegant Boots, Elegant Gloves, Elegant Mantle, Engraved Breastplate, Engraved Gauntlets, Gloves of Shadowy Mist, Grand Belt, Hero's Cape, Hero's Gauntlets, Hero's Pauldrons, High Councillor's Circlet, High Councillor's Cloak, Hyperion Vambraces, Incendic Bracers, Jungle Ring, Master's Gloves, Masterwork Legplates, Mercurial Gauntlets, Mercurial Girdle, Mighty Armsplints, Mighty Cloak, Mystical Armor, Mystical Leggings, Mystical Robe, Nightshade Boots, Nightshade Cloak, Obsidian Band, Ornate Greaves, Ornate Pauldrons, Ring of the Heavens, Swamp Pendant, Swamp Ring, Swashbuckler's Breastplate, Traveler's Belt, Traveler's Boots, Wanderer's Armor, Wicked Leather Headband, Adventurer's Boots, Adventurer's Legguards, Adventurer's Tunic, Arcane Leggings, Arcane Pads, Bonecaster's Gloves, Bonecaster's Spaulders, Councillor's Robes, Councillor's Tunic, Elegant Bracers, Elegant Circlet, Elegant Leggings, Emerald Vambraces, Engraved Bracers, Engraved Leggings, Hero's Belt, Heroic Commendation Medal, High Councillor's Boots, High Councillor's Mantle, Hyperion Gauntlets, Hyperion Greaves, Hyperion Helm, Hyperion Pauldrons, Imbued Plate Leggings, Imbued Plate Vambraces, Jungle Necklace, Masterwork Bracers, Masterwork Cape, Masterwork Girdle, Mercurial Circlet, Mercurial Pauldrons, Nightshade Girdle, Nightshade Gloves, Nightshade Leggings, Nightshade Spaulders, Ornate Breastplate, Quicksilver Ring, Resplendent Bracelets, Templar Chestplate, Templar Crown, Traveler's Helm, Traveler's Jerkin, Adventurer's Shoulders, Arcane Boots, Arcane Gloves, Commander's Leggings, Crown of the Penitent, Deep River Cloak, Emerald Legplates, Engraved Girdle, Highborne Crown, Imbued Plate Gauntlets, Imbued Plate Girdle, Imbued Plate Greaves, Mighty Gauntlets, Mighty Spaulders, Ornate Circlet, Ornate Gauntlets, Ornate Legguards, Quicksilver Pendant, Riptide Shoes, Serpentskin Armor, Swashbuckler's Leggings, Wanderer's Boots, Adventurer's Bandana, Cerise Drape, Commander's Girdle, Councillor's Circlet, Councillor's Pants, Engraved Boots, High Councillor's Bracers, High Councillor's Gloves, High Councillor's Sash, Imbued Plate Pauldrons, Master's Leggings, Mercurial Greaves, Scarab Plate Helm, Templar Pauldrons, Traveler's Bracers, Traveler's Gloves, Wanderer's Hat, Adventurer's Cape, Arcane Bands, Arcane Cover, Arcane Sash, Councillor's Boots, Emerald Helm, Engraved Helm, Imbued Plate Helmet, Mercurial Cloak, Mighty Boots, Nightshade Armguards, Traveler's Leggings, Commander's Boots, Commander's Gauntlets, Dragonscale Band, Engraved Pauldrons";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Greater Magic Essence")
        {
            var names = "Raptor's End, Tunnel Pick, Reef Axe, Scrimshaw Dagger, Branding Rod, Samophlange Screwdriver, Staff of the Purifier, Lucine Longsword, Petrified Shinbone, Piercing Axe, Polished Walking Staff, Shadowhide Mace, Brackclaw, Lupine Axe, Owlsight Rifle, Scimitar of Atun, Smite's Reaver, Armor Piercer, Bow of Plunder, Cavalier Two-hander, Dwarven Tree Chopper, Hollowfang Blade, Moonstone Wand, Shadowhide Maul, Shiver Blade, Sizzle Stick, Barbaric Battle Axe, Black Metal War Axe, Consecrated Wand, Cookie's Tenderizer, Headbasher, Militant Shortsword, Moonbeam Wand, Ridge Cleaver, Tail Spike, Cold Iron Pick, Dire Wand, Dwarven Flamestick, Hunter's Muzzle Loader, Icicle Rod, Massive Battle Axe, Spark of the People's Militia, Staff of Horrors, Wicked Spiked Mace, Barbarian War Axe, Barbed Club, Battering Hammer, Black Metal Axe, Blackwater Cutlass, Deadly Blunderbuss, Decapitating Sword, Flaring Baton, Hefty Battlehammer, Hook Dagger, Impaling Harpoon, Magician Staff, Orc Crusher, Sable Wand, Short Ash Bow, Trogg Slicer, Twin-bladed Axe, Venom Web Fang, Viking Sword, Arced War Axe, Baron's Scepter, Blackrock Champion's Axe, Blazing Wand, Brute Hammer, Butcher's Slicer, Cliffrunner's Aim, Defias Rapier, Edged Bastard Sword, Honed Stiletto, Jagged Star, Long Battle Bow, Medicine Staff, Northern Shortsword, Opaque Wand, Phytoblade, Raider Shortsword, Sergeant's Warhammer, Wicked Blackjack, Zhovur Axe, Blackrock Mace, Chanting Blade, Clear Crystal Rod, Cross Dagger, Cursed Felblade, Dwarven Magestaff, Edge of the People's Militia, Engineer's Hammer, Firestarter, Gleaming Claymore, Light Bow, Logsplitter, Merc Sword, Oak Mallet, Polished Zweihander, Precision Bow, Serrated Knife, Spellcrafter Wand, Stout Battlehammer, Battle Slayer, Battlesmasher, Cinder Wand, Daryl's Shortsword, Dwarven Fishing Pole, Fighter Broadsword, Grunt Axe, Hardened Root Staff, Haunting Blade, Miner's Revenge, Privateer Musket, Redridge Machete, Shadowhide Battle Axe, Silver-plated Shotgun, Staff of Orgrimmar, Stingshot Wand, Thief's Blade, Torchlight Wand, Wand of Decay, War Knife, Wind Rider Staff, Battle Knife, Ceranium Rod, Daryl's Hunting Rifle, Demolition Hammer, Forester's Axe, Gnoll Punisher, Goblin Screwdriver, Woodsman Sword, Brutal War Axe, Charred Wand, Excavation Rod, Mechanic's Pipehammer, Gnoll Skull Basher";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Greater Astral Essence")
        {
            var names = "Pathfinder Footpads, Dreamer's Belt, Madwolf Bracers, Greenweave Branch, Slayer's Gloves, Ivory Band, Garneg's War Belt, Gnomeregan Band, Spiked Chain Breastplate, Azure Silk Gloves, Emblazoned Gloves, Lancer Boots, Wrangler's Gloves, Wrangler's Leggings, Bright Armor, Cobalt Ring, Dread Mage Hat, Hulking Shield, Sentry's Gloves, Spectral Necklace, Watcher's Boots, Wicked Chain Cloak, Beastmaster's Girdle, Cloak of the Faith, Fairywing Mantle, Glimmering Mail Legguards, Raincaller Mantle, Slayer's Slippers, Spiked Chain Shoulder Pads, Battleforge Armor, Dervish Buckler, Elder's Cloak, Glimmering Mail Bracers, Pagan Rod, Sanguine Handwraps, Silvered Bronze Gauntlets, Superior Boots, Talbar Mantle, Windborne Belt, Battleforge Boots, Draftsman Boots, Driftmire Shield, Emblazoned Bracers, Emblazoned Buckler, Emblazoned Shoulders, Fortified Spaulders, Ghostly Mantle, Ivycloth Pants, Lambent Scale Pauldrons, Mercenary Leggings, Panther Armor, Resilient Poncho, Rose Mantle, Shield of the Faith, Silver-thread Amice, Spiked Chain Leggings, Watcher's Handwraps, Azure Silk Vest, Battleforge Cloak, Battleforge Shield, Dervish Spaulders, Engineer's Cloak, Fenrus' Hide, Glimmering Cloak, Glowing Green Talisman, Ivy Orb, Ivycloth Tunic, Lambent Scale Girdle, Lambent Scale Shield, Pathfinder Bracers, Pathfinder Cloak, Resilient Cord, Robust Girdle, Sage's Cloak, Scaled Leather Belt, Silver-thread Boots, Silver-thread Cuffs, Soft Willow Cape, Voodoo Mantle, Vorrel's Boots, Watcher's Cuffs, Azora's Will, Azure Silk Pants, Banded Bracers, Black Wolf Bracers, Durable Belt, Greenweave Robe, Ivycloth Gloves, Ivycloth Sash, Lambent Scale Breastplate, Lambent Scale Gloves, Phalanx Cloak, Raincaller Cloak, Sage's Gloves, Silver-thread Pants, Spritekin Cloak, Superior Gloves, Banded Cloak, Battleforge Gauntlets, Bright Pants, Bright Sphere, Desert Shoulders, Greenweave Leggings, Greenweave Mantle, Grizzled Boots, Hillman's Leather Gloves, Sage's Bracers, Scaled Leather Bracers, Tundra Boots, Durable Cape, Gloves of Meditation, Hillman's Shoulders, Lesser Wizard's Robe, Plainsguard Leggings, Repairman's Cape, Silk Mantle of Gamn, Superior Bracers, Wolfmaster Cape, Amber Hoop, Darkshire Mail Leggings, Dervish Belt, Emblazoned Cloak, Ivycloth Mantle, Superior Shoulders, Battleforge Wristguards, Dervish Bracers, Girdle of the Blindwatcher";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Greater Mystic Essence")
        {
            var names = "Sidegunner Shottie, Korg Bat, Windstorm Hammer, Flash Rifle, Moonsteel Broadsword, Eyepoker, Sequoia Branch, Silithid Ripper, Blasting Hackbut, Fizzle's Zippy Lighter, Staff of Dar'Orahil, Headhunting Spear, Giant Club, Mercenary Blade, Burning Sliver, Greater Scythe, Nobles Brand, Rod of Sorrow, Savage Axe, Skullbreaker, Ballast Maul, Explosive Shotgun, Kodo Brander, Ryedol's Hammer, Stone Hammer, Captain Rackmore's Tiller, Deadly Kris, Gloom Reaper, Goblin Igniter, Smoothbore Gun, Tigerbane, Flash Wand, Gnomish Zapper, Knightly Longsword, Murphstar, Black Water Hammer, Dancing Flame, Huge Stone Club, Nail Spitter, Bookmaker's Scepter, Staff of Noh'Orahil, Shrapnel Blaster, Geomancer's Spaulders, Steadfast Cloak, Baroque Apron, Grom'gol Buckler, Aurora Mantle, Aurora Cowl, Conjurer's Vest, Knight's Girdle, Sentinel Cap, Huntsman's Cape, Sentinel Gloves, Twilight Belt, Twilight Gloves, Archer's Trousers, Conjurer's Mantle, Green Silken Shoulders, Twilight Cuffs";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Greater Nether Essence")
        {
            var names = "Runed Mithril Hammer, Dazzling Mithril Rapier, Nature's Breath, Vorpal Dagger, Dreadblade, Tusker Sword, Cairnstone Sliver, Charged Lightning Rod, Big Black Mace, Crescent Edge, Diamond-Tip Bludgeon, Gigantic War Axe, Greater Maul, Grinning Axe, Royal Mallet, Skullcrusher Mace, Spellshifter Rod, Staff of Lore, Strength of the Treant, Zorbin's Mega-Slicer, Ebonclaw Reaver, Galgann's Firehammer, Percussion Shotgun, Ricochet Blunderbuss, Gahz'rilla Fang, Sanctimonial Rod, Battlefield Destroyer, Chillnail Splinter, Conk Hammer, Nat Pagle's Extreme Angler FC-5000, Nimboya's Mystical Staff, Gryphon Mail Crown, Heavy Mithril Boots, Venomshroud Mantle, Arachnidian Bracelets, Overlord's Greaves, Lunar Raiment, Mithril Coif, Hibernal Sash, Topaz Ring, Tracker's Tunic, Chieftain's Belt, Emberscale Cape, Everwarm Handwraps, Gothic Plate Gauntlets, Tellurium Band, Gossamer Tunic, Tracker's Leggings, Bloodwoven Boots, Chieftain's Bracers, Duskwoven Sash, Lord's Armguards, Warmonger's Greaves, Cabalist Belt, Cabalist Spaulders, Champion's Greaves, Overlord's Gauntlets, Overlord's Vambraces, Warmonger's Pauldrons, Black Mageweave Shoulders, Cabalist Helm, Gothic Plate Leggings, Overlord's Legplates, Rugwood Mantle, Seared Mail Girdle, Warmonger's Belt, Slagplate Gauntlets, Black Mageweave Headband, Nightscape Pants";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Greater Eternal Essence")
        {
            var names = "Tidecrest Blade, Ogre Pocket Knife, Blade of Reckoning, Bloodstrike Dagger, Corrupted Blackwood Staff, Death Striker, Voone's Twitchbow, Black Crystal Dagger, Enchanted Azsharite Felbane Staff, Farmer Dalson's Shotgun, Linken's Sword of Mastery, Sarah's Guide, Sprinter's Sword, Archstrike Bow, Divine Warblade, Fierce Mauler, Holy War Sword, Lunar Wand, Burnside Rifle, Hawkeye Bow, Solstice Staff, Backbreaker, Battlefell Sabre, Brann's Trusty Pick, Colossal Great Axe, Crystal Sword, Dimensional Blade, Dragon Finger, Gallant Flamberge, Hameya's Slayer, Magus Long Staff, Massacre Sword, Ogre Toothpick Shooter, Razor Axe, Sharpshooter Harquebus, Shin Blade, Skullcracking Mace, Smokey's Explosive Launcher, Stoneflower Staff, Blasthorn Bow, Brutehammer, Demon Blade, Glowstar Rod, Intrepid Shortsword, Lethtendris's Wand, Painbringer, Felstone Reaver, Hunt Tracker Blade, Skilled Fighting Blade, Smokey's Fireshooter, Warlord's Axe, Thornflinger, Valiant Shortsword, Demon's Claw, Unsophisticated Hand Cannon, Jagged Bone Fist, Beasthunter Dagger, Well Balanced Axe";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Small Glimmering Shard")
        {
            var names = "Abomination Skin Leggings, Band of Purification, Black Malice, Black Pearl Ring, Blackened Defias Armor, Blackfang, Butcher's Cleaver, Cape of the Brotherhood, Chausses of Westfall, Cobrahn's Grasp, Cookie's Stirring Rod, Corsair's Overshirt, Crescent Staff, Cruel Barb, Darkweave Breeches, Deviate Scale Belt, Diamond Hammer, Drakewing Bands, Emberstone Staff, Face Smasher, Feet of the Lynx, Firebane Cloak, Firebelcher, Gargoyle's Bite, Gloomshroud Armor, Glowing Lizardscale Cloak, Gold-plated Buckler, Guillotine Axe, Ironpatch Blade, Keller's Girdle, Kresh's Back, Lavishly Jeweled Ring, Leggings of the Fang, Lil Timmy's Peashooter, Living Root, Magefist Gloves, Magician's Mantle, Mindthrust Bracers, Necrology Robes, Night Watch Shortsword, Phantom Armor, Prospector Axe, Pulsating Hydra Heart, Rakzur Club, Ranger Bow, Razor's Edge, Redbeard Crest, Ring of Defense, Searing Blade, Seedcloud Buckler, Sentry Cloak, Silver-linked Footguards, Skeletal Club, Skycaller, Smite's Mighty Hammer, Spidersilk Boots, Staff of the Friar, Staff of Westfall, Starsight Tunic, Stinging Viper, Stormbringer Belt, Taskmaster Axe, The Axe of Severing, Thorbia's Gauntlets, Tortoise Armor, Tunic of Westfall, Twisted Chanter's Staff, Venomstrike, Wingblade, Witching Stave, Antipodean Rod, Evocator's Blade";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Large Glimmering Shard")
        {
            var names = "Algae Fists, Amy's Blanket, Arctic Buckler, Bearded Boneaxe, Belt of Arugal, Bite of Serra'kis, Black Velvet Robes, Bloodpike, Brawler Gloves, Commander's Crest, Deanship Claymore, Deep Fathom Ring, Dense Triangle Mace, Double Link Tunic, Dreamsinger Legguards, Feline Mantle, Glowing Thresher Cape, Gravestone Scepter, Guardian Blade, Harbinger Boots, Headsplitter, Iron Knuckles, Jimmied Handcuffs, Killmaim, Meteor Shard, Moccasins of the White Hare, Mutant Scale Breastplate, Naga Battle Gloves, Odo's Ley Staff, Orb of Mistmantle, Outlaw Sabre, Prison Shank, Robes of Arugal, Rod of the Sleepwalker, Seal of Sylvanas, Shield of Thorsen, Shining Silver Breastplate, Silver-lined Belt, Silverlaine's Family Seal, Slime-encrusted Pads, The Queen's Jewel, Thunderwood, Toughened Leather Gloves, Troll's Bane Leggings, Twisted Sabre, Warsong Boots, Warsong Gauntlets, Warsong Sash, Witch's Finger, Yorgen Bracers";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Small Glowing Shard")
        {
            var names = "Acidic Walkers, Band of Allegiance, Barbaric Bracers, Batwing Mantle, Beguiler Robes, Blighted Leggings, Bloodspiller, Burning War Axe, Chesterfall Musket, Claw of the Shadowmancer, Cobalt Crusher, Combatant Claymore, Corpsemaker, Crystalpine Stinger, Death Speaker Scepter, Dreamslayer, Ebon Vise, Electrocutioner Lagnut, Electrocutioner Leg, Embalmed Shroud, Enduring Cap, Forest Tracker Epaulets, Frostreaver Crown, Ghostshard Talisman, Girdle of Golem Strength, Glass Shooter, Gloves of Old, Glowing Magical Bracelets, Gnarled Ash Staff, Grubbis Paws, Harpyclaw Short Bow, Holy Shroud, Hydrocane, Ironspine's Eye, Ironspine's Fist, Ironspine's Ribcage, Ironweaver, Kaleidoscope Chain, Leech Pants, Lonetree's Circle, Looming Gavel, Manual Crowd Pummeler, Morbid Dawn, Moss Cinch, Necromancer Leggings, Necrotic Wand, Nogg's Gold Ring, Ravasaur Scale Boots, Resplendent Guardian, River Pride Choker, Rod of Molten Fire, Scarlet Boots, Scorn's Focal Dagger, Seal of Wrynn, Starfaller, Strike of the Hydra, Stygian Bone Amulet, Sunblaze Coif, Swinetusk Shank, Talvash's Gold Ring, The Black Knight, The Butcher, Torturing Poker, Toxic Revenger, Unearthed Bands, Verigan's Fist, Viscous Hammer, Watchman Pauldrons, Wind Spirit Staff, Zealot Blade, Gnomeregan Amulet";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Large Glowing Shard")
        {
            var names = "Agamaggan's Clutch, Archaic Defender, Archon Chestpiece, Berylline Pads, Black Duskwood Staff, Blade of the Basilisk, Blush Ember Ring, Briar Tredders, Celestial Orb, Celestial Stave, Charged Gear, Civinad Robes, Deadwood Sledge, Dragonclaw Ring, Dual Reinforced Leggings, Earthen Rod, Electromagnetic Gigaflux Reactivator, Enchanted Gold Bloodrobe, Enchanted Kodo Bracers, Energy Cloak, Enormous Ogre Belt, Firemane Leggings, Gem-studded Leather Belt, Grim Reaper, Heart of Agamaggan, Hellslayer Battle Axe, Howling Blade, Hypnotic Blade, Icefury Wand, Icy Cloak, Illusionary Rod, Loksey's Training Stick, Marbled Buckler, Mark of Kern, Midnight Mace, Orb of Dar'Orahil, Orb of Noh'Orahil, Orb of the Forgotten Seer, Pads of the Venom Spider, Plaguerot Sprig, Poison-tipped Bone Spear, Pronged Reaver, Ragefire Wand, Reticulated Bone Gauntlets, Ring of the Underwood, Robe of Power, Ruthless Shiv, Scarlet Chestpiece, Scorpion Sting, Shadowskin Gloves, Skullance Shield, Skystriker Bow, Sliverblade, Stalvan's Reaper, Steelclaw Reaver, Stonefist Girdle, Stoneweaver Leggings, Sutarn's Ring, Swampchill Fetish, Swampwalker Boots, Swiftwind, Tainted Pierce, The Ziggler, Thermaplugg's Central Core, Thermaplugg's Left Arm, Triprunner Dungarees, Warchief Kilt, Whirlwind Axe, Windweaver Staff, Wing of the Whelpling, Wolffear Harness, Robe of the Magi, Staff of Protection";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Small Radiant Shard")
        {
            var names = "Aegis of the Scarlet Commander, Amberglow Talisman, Ankh of Life, Arachnid Gloves, Assault Band, Basilisk Hide Pants, Blazing Emblem, Boar Champion's Belt, Bonebiter, Carapace of Tuten'kash, Coldrage Dagger, Cragfists, Crushridge Bindings, Curve-bladed Ripper, Deathchill Armor, Deathmage Sash, Dragon's Blood Necklace, Dragonscale Gauntlets, Dreamweave Gloves, Dreamweave Vest, Elemental Raiment, Fleshhide Shoulders, Furen's Boots, Gauntlets of Divinity, Gazlowe's Charm, Glowing Eye of Mordresh, Hand of Righteousness, Heaven's Light, Herod's Shoulder, Icemetal Barbute, Ironaya's Bracers, Ironshod Bludgeon, Jaina's Firestarter, Khoo's Point, Mograine's Might, Mordresh's Lifeless Skull, Necklace of Calisea, Obsidian Greaves, Ogron's Sash, Orb of Lorica, Plated Fist of Hakoo, Polished Jazeraint Armor, Prophetic Cane, Raging Berserker's Helm, Ravager, Robes of the Lich, Savage Boar's Guard, Scarlet Leggings, Sheepshear Mantle, Silky Spider Cape, Skeletal Shoulders, Sword of Omen, Sword of Serenity, Sword of the Magistrate, Thornstone Sledgehammer, Triune Amulet, Umbral Crystal, Vanquisher's Sword, Whitemane's Chapeau, Witchfury, Wolfshead Helm, X'caliboar, Ardent Custodian, Boots of Avoidance, Nightblade, Southsea Lamp, Icemail Jerkin";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Large Radiant Shard")
        {
            var names = "Archaedic Stone, Arena Bands, Arena Bracers, Arena Vambraces, Arena Wristguards, Bad Mojo Mask, Belt of the Gladiator, Big Bad Pauldrons, Blackflame Cape, Blade of the Titans, Blanchard's Stout, Blight, Bludgeon of the Grinning Dog, Bonechewer, Cassandra's Grace, Changuk Smasher, Deathblow, Desertwalker Cane, Diabolic Skiver, Dreamweave Circlet, Elven Chain Boots, Elven Spirit Claws, Embrace of the Lycan, Engineer's Guild Headpiece, Feathered Breastplate, Flameseer Mantle, Forgotten Wraps, Gahz'rilla Scale Armor, Galgann's Fireblaster, Gauntlets of the Sea, Giantslayer Bracers, Golem Shard Leggings, Green Lens, Grimlok's Charge, Grimlok's Tribal Vestments, Guttbuster, Helm of Fire, High Bergg Helm, Infernal Trickster Leggings, Jang'thraze the Protector, Jarkal's Enhancing Necklace, Jinxed Hoodoo Kilt, Jinxed Hoodoo Skin, Jumanza Grips, Lifeblood Amulet, Masons Fraternity Ring, Mindseye Circle, Mistwalker Boots, Mountainside Buckler, Mug O' Hurt, Murkwater Gauntlets, Needle Threader, Ripsaw, Sandstalker Ankleguards, Sang'thraze the Deflector, Satyr's Lash, Satyrmane Sash, Serpent Slicer, Shortsword of Vengeance, Skibi's Pendant, Skull Splitting Crossbow, Slimescale Bracers, Stoneslayer, Talvash's Enhancing Necklace, Tanglewood Staff, The Chief's Enforcer, The Hand of Antu'sul, The Rockpounder, Truesilver Breastplate, Vice Grips, Wand of Allistarj, Widowmaker, Winged Helm, Winter's Bite, Zum'rah's Vexing Cane, Blackskull Shield, Ring of Saviors, Robes of Insight, Wall of the Dead";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Small Brilliant Shard")
        {
            var names = "Aegis of Stormwind, Albino Crocscale Boots, Arbiter's Blade, Aristocratic Cuffs, Atal'ai Breastplate, Atal'ai Gloves, Atal'ai Spaulders, Atal'alarion's Tusk Ring, Avenguard Helm, Axe of Rin'ji, Ban'thok Sash, Basilisk Bone, Battlecaller Gauntlets, Blackstone Ring, Blackveil Cape, Bloodfire Talons, Bloodshot Greaves, Bloomsprout Headpiece, Bonesnapper, Bracers of the Stone Princess, Braincage, Carapace of Anub'shiah, Charstone Dirk, Chief Architect's Monocle, Claw of Celebras, Cloud Stone, Coldstone Slippers, Cow King's Hide, Cyclopean Band, Dalewind Trousers, Dark Phantom Cape, Darkwater Bracers, Dawnspire Cord, Deep Woodlands Cloak, Deepfury Bracers, Drakeclaw Band, Drakefang Butcher, Drakestone, Dregmetal Spaulders, Earthslag Shoulders, Elemental Rockridge Leggings, Enthralled Sphere, Entrenching Boots, Eye of Adaegus, Eye of Theradras, Featherskin Cape, Firebreather, Fist of Stone, Flamestrider Robes, Fleetfoot Greaves, Foreman's Head Protector, Funeral Pyre Vestment, Fungus Shroud Armor, Gatorbite Axe, Gemburst Circlet, Gemshard Heart, Girdle of Beastial Fury, Gizlock's Hypertech Buckler, Gloves of the Atal'ai Prophet, Graverot Cape, Greaves of Withering Despair, Grizzle's Skinner, Grovekeeper's Drape, Hanzo Sword, Headspike, Heart of Noxxion, Helm of Exile, Helm of the Mountain, Hookfang Shanker, Horizon Choker, Hydralick Armor, Inventor's Focal Sword, Julie's Dagger, Kentic Amice, Kilt of the Atal'ai Prophet, Kindling Stave, Lead Surveyor's Mantle, Lifeforce Dirk, Living Shoulders, Manacle Cuffs, Mantle of Lost Hope, Megashot Rifle, Mixologist's Tunic, Mugthol's Helm, Nagmara's Whipping Belt, Nature's Embrace, Nightfall Drape, Noxious Shooter, Noxxion's Shackles, Ogreseer Fists, Phasing Boots, Phytoskin Spaulders, Princess Theradras' Scepter, Pyric Caduceus, Rainstrider Leggings, Resurgence Rod, Ribsplitter, Rockgrip Gauntlets, Rotgrip Mantle, Rubicund Armguards, Runed Golem Shackles, Sandals of the Insurgent, Searing Needle, Searingscale Leggings, Senior Designer's Pantaloons, Serenity Belt, Shadefiend Boots, Shard of the Green Flame, Silkweb Gloves, Six Demon Bag, Slitherscale Boots, Smoldering Claw, Soothsayer's Headdress, Soulkeeper, Spiderfang Carapace, Spire of Hakkar, Splinthide Shoulders, Spritecaster Cape, Stoneraven, Stonewall Girdle, The Judge's Gavel, Thrash Blade, Uther's Strength, Vestments of the Atal'ai Prophet, Viking Warhammer, Vinerot Sandals, Warbear Harness, Warmonger, Warrior's Embrace, Windscale Sarong, Woven Ivy Necklace, Wyrmslayer Spaulders, Stonerender Gauntlets, Stoneshell Guard, Gryphonwing Long Bow";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
        else if (name == "Large Brilliant Shard")
        {
            var names = "Anastari Heirloom, Angerforge's Battle Axe, Archivist Cape, Archlight Talisman, Ash Covered Boots, Band of Flesh, Band of the Ogre King, Barrage Girdle, Barrier Shield, Beaststalker's Bindings, Beaststalker's Boots, Belt of Untapped Power, Belt of Valor, Bindings of Elements, Blood of the Martyr, Blooddrenched Mask, Bloodmail Belt, Bloodmail Gauntlets, Bloodsoaked Gauntlets, Bloodsoaked Greaves, Bloodstained Greaves, Boneclenched Gauntlets, Boots of the Shrieker, Boots of Valor, Boreal Mantle, Bracers of Prosperity, Bracers of Valor, Brightly Glowing Stone, Cadaverous Belt, Cadaverous Gloves, Cadaverous Leggings, Cadaverous Walkers, Cape of the Fire Salamander, Carapace Spine Crossbow, Chillsteel Girdle, Chitinous Plate Legguards, Cinderhide Armsplints, Cloak of the Hakkari Worshipers, Clutch of Andros, Cord of Elements, Corpselight Greaves, Crimson Felt Hat, Crown of the Ogre King, Cyclone Spaulders, Deadwalker Mantle, Death Grips, Death's Clutch, Deathbone Gauntlets, Deathbone Girdle, Deathbone Legguards, Deathbone Sabatons, Deathdealer Breastplate, Devout Belt, Devout Bracers, Devout Crown, Devout Gloves, Devout Sandals, Devout Skirt, Dracorian Gauntlets, Dragoneye Coif, Dragonrider Boots, Drakesfire Epaulets, Dreadmist Belt, Dreadmist Bracers, Dreadmist Leggings, Dreadmist Mask, Dreadmist Sandals, Dreadmist Wraps, Dreamwalker Armor, Earthborn Kilt, Elder Wizard's Mantle, Elemental Plate Girdle, Emberplate Armguards, Energetic Rod, Energized Chestplate, Feathermoon Headdress, Felhide Cap, Feralsurge Girdle, Fervent Helm, Fiendish Machete, Fire Striders, Flameweave Cuffs, Flaming Band, Foresight Girdle, Freezing Lich Robes, Frostbite Girdle, Gargoyle Slashers, Gauntlets of Elements, Gauntlets of Valor, Ghoul Skin Leggings, Gilded Gauntlets, Girdle of Uther, Gloves of Restoration, Gloves of the Tormented, Gordok Bracers of Power, Grand Crusader's Helm, Grimgore Noose, Grimy Metal Boots, Halycon's Spiked Collar, Harmonious Gauntlets, Haunting Specter Leggings, Heart of the Fiend, Helm of Awareness, Husk of Nerub'enkan, Lady Alizabeth's Pendant, Lapidis Tankard of Tidesippe, Lightforge Belt, Lightforge Boots, Lightforge Bracers, Lightforge Gauntlets, Loomguard Armbraces, Lordly Armguards, Lorespinner, Luminary Kilt, Maelstrom Leggings, Mageflame Cloak, Magiskull Cuffs, Magister's Belt, Magister's Crown, Magister's Gloves, Magister's Mantle, Magistrate's Cuffs, Maleki's Footwraps, Marksman Bands, Mask of the Unforgiven, Merciful Greaves, Mindsurge Robe, Molten Fists, Necropile Boots, Necropile Cuffs, Necropile Leggings, Necropile Mantle, Necropile Robe, Ogre Forged Hauberk, Omnicast Boots, Pale Moon Cloak, Pyremail Wristguards, Rainbow Girdle, Razor Gauntlets, Redoubt Cloak, Ring of Demonic Guile, Ring of Demonic Potency, Robes of the Exalted, Robes of the Royal Crown, Royal Decorated Armor, Royal Tribunal Cloak, Sacrificial Gauntlets, Satyr's Bow, Screeching Bow, Serpentine Sash, Shadewood Cloak, Shadowcraft Belt, Shadowcraft Boots, Shadowcraft Bracers, Shadowy Laced Handwraps, Shadowy Mail Greaves, Shell Launcher Shotgun, Shivery Handwraps, Shroud of the Nathrezim, Skull of Burning Shadows, Slashclaw Bracers, Songbird Blouse, Soulstealer Mantle, Spellbound Tome, Starfire Tiara, Stoneform Shoulders, Stonegrip Gauntlets, Tarnished Elven Ring, Tearfall Bracers, Tempest Talisman, Thuzadin Sash, Timmy's Galoshes, Tome of Knowledge, Tribal War Feathers, Trueaim Gauntlets, Tunic of the Crescent Moon, Unbridled Leggings, Vigorsteel Vambraces, Wailing Nightbane Pauldrons, Warmaster Legguards, Warstrife Leggings, Waterspout Boots, Whipvine Cord, Wildfire Cape, Wildheart Belt, Wildheart Boots, Wildheart Bracers, Wildheart Gloves, Windreaver Greaves, Windrunner Legguards, Wolfshear Leggings, Woollies of the Prancing Minstrel, Wyrmtongue Shoulders, Zulian Headdress, Zulian Scepter of Rites, Leggings of Destruction, Seal of Rivendare, Vambraces of the Sadist, Might of the Tribe";
            var list = names.Split(",").Select(x => x.Trim().ToLower());
            var others = items.Where(x => list.Contains(x.name.ToLower()));
            price = (int)others.Average(x => x.price) / 5;
        }
    }

    //Position of the item in the inventory
    public int x, y;

    //Rarity of this item which can range from Poor to Legendary
    public string rarity;

    //Name of the item
    public string name;

    //Icon of the item in the inventory
    public string icon;

    //Icon of the item in the inventory
    public bool combatUse;

    //Determines wether instances of this item get a random enchant
    public bool randomEnchantment;

    //Detailed type of the item
    //EXAMPLE: "Axe" for item of "Two Handed" type
    public string detailedType;

    //Type of the item
    public string type;

    //Armor class of the armor piece
    //Can range from Cloth to Plate
    public string armorClass;

    //Set that this item is part of
    public string set;

    //Faction that this item belongs to
    public string faction;

    //Source where this item can be gotten from
    public string source;

    //Reputation standing required from the player to use this item
    public string reputationRequired;

    //Enchant applied by an enchanter
    public Enchant enchant;
    
    //Item power / level of this item, helps in calculating which item is better than other
    public int ilvl;

    //Minimum required level of the character for it to be able to equip or use this item
    public int lvl;

    //Minimum power modifier this weapon can roll
    public double minPower;

    //Maximum power modifier this weapon can roll
    public double maxPower;

    //Amount of armor provided to the wearer of this item
    public int armor;

    //Amount of block power provided to the wearer
    public int block;

    //Amount of this item
    public int amount;

    //Time left for this item to be removed from buyback
    public int minutesLeft;

    //Drop range, it's automatic if set to default
    public string dropRange;

    //Max amount of this item per stack
    public int maxStack;

    //Can player get rid of this item
    public bool indestructible;

    //Can player have more than one of these items
    public bool unique;

    //Items contained inside of the item
    public List<Item> itemsInside;

    //List of abilities provided to the wearer of this item
    public Dictionary<string, int> abilities;

    //Spec restrictions for this item
    //Specs listed in it are the specs that exclusively can use this item
    public List<string> specs;

    //Drop restrictions based on race
    public List<string> raceDropRestriction;

    //Drop restrictions based on character class
    public List<string> specDropRestriction;

    //List of quests that can be started by using this item
    public List<int> questsStarted;

    //Price of the item for it to be bought, the sell price is 1/4 of that
    public int price;

    //Amount of bag space provided
    public int bagSpace;
    
    //Stats provided to the wearer like Stamina or Intellect
    public Dictionary<string, int> stats;

    //This is a list of races that are eligible to drop this item
    public List<string> droppedBy;

    #region Equipping

    //Tells whether this item is generally wearable
    public bool IsWearable()
    {
        if (new List<string> { "Miscellaneous", "Trade Good", "Recipe", "Bag" }.Contains(type)) return false;
        return true;
    }

    //Equips this item on the chosen entity in a specific slot
    private void EquipInto(Entity entity, string slot)
    {
        if (slot == "Bag") entity.inventory.bags.Add(this);
        else entity.equipment[slot] = this;
        if (entity.inventory.items.Contains(this))
            entity.inventory.items.Remove(this);
        if (abilities != null)
            entity.abilities = entity.abilities.Concat(abilities).ToDictionary(x => x.Key, x => x.Value);
        if (enchant != null && enchant.abilities != null)
            entity.abilities = entity.abilities.Concat(enchant.abilities).ToDictionary(x => x.Key, x => x.Value);
    }

    //Equips this item on the chosen entity
    //Slot action determines how is the item equiped [Auto, ]
    public void Equip(Entity entity, bool autoSlotting, bool altSlot)
    {
        var unequiped = new List<Item>();
        if (type == "Two Handed")
        {
            if (entity.equipment.ContainsKey("Off Hand") && entity.equipment.ContainsKey("Main Hand") && entity.inventory.BagSpace() - entity.inventory.items.Count < 1)
                SpawnFallingText(new Vector2(0, 34), "Inventory full", "Red");
            else
            {
                unequiped.AddRange(entity.Unequip(new() { "Off Hand", "Main Hand" }));
                EquipInto(entity, "Main Hand");
            }
        }
        else if (type == "Off Hand")
        {
            var mainHand = entity.GetItemInSlot("Main Hand");
            if (mainHand != null && mainHand.type == "Two Handed")
                unequiped.AddRange(entity.Unequip(new() { "Main Hand" }));
            else unequiped.AddRange(entity.Unequip(new() { "Off Hand" }));
            EquipInto(entity, "Off Hand");
        }
        else if (type == "One Handed")
        {
            var mainHand = entity.GetItemInSlot("Main Hand");

            //If slot is chosen automatically..
            if (autoSlotting)

                //If there is a one handed weapon equipped and dual wielding is possible; equip the weapon in the off hand
                if (mainHand != null && mainHand.type != "Two Handed" && entity.abilities.ContainsKey("Dual Wielding Proficiency"))
                {
                    if (mainHand != null && mainHand.type == "Two Handed")
                        unequiped.AddRange(entity.Unequip(new() { "Main Hand" }));
                    else unequiped.AddRange(entity.Unequip(new() { "Off Hand" }));
                    EquipInto(entity, "Off Hand");
                }

                //Otherwise equip it into the main hand
                else
                {
                    unequiped.AddRange(entity.Unequip(new() { "Main Hand" }));
                    EquipInto(entity, "Main Hand");
                }

            //Otherwise equip it into the main slot or with LeftAlt into the offhand
            else
            {
                //If dual wielding is possible and LeftAlt is pressed; equip the weapon in the off hand
                if (altSlot && entity.abilities.ContainsKey("Dual Wielding Proficiency"))
                {
                    if (mainHand != null && mainHand.type == "Two Handed")
                        unequiped.AddRange(entity.Unequip(new() { "Main Hand" }));
                    else unequiped.AddRange(entity.Unequip(new() { "Off Hand" }));
                    EquipInto(entity, "Off Hand");
                }

                //Otherwise equip it into the main hand
                else
                {
                    unequiped.AddRange(entity.Unequip(new() { "Main Hand" }));
                    EquipInto(entity, "Main Hand");
                }
            }
        }
        else if (type == "Bag") EquipInto(entity, "Bag");
        else
        {
            if (type == null) Debug.Log(name);
            unequiped.AddRange(entity.Unequip(new() { type }));
            EquipInto(entity, type);
        }
        foreach (var item in unequiped)
            entity.inventory.AddItem(item);
    }

    //Checks whether a chosen entity can equip this item
    //While [downgradeArmor] is set to false this function does not allow
    //people downgrading their preferred armor class
    //For example it will say that a Paladin cannot equip a cloth item
    public bool HasProficiency(Entity entity, bool downgradeArmor = false)
    {
        if (type == "Miscellaneous" || type == "Trade Good" || type == "Recipe") return false;
        else if (armorClass != null)
        {
            if (downgradeArmor)
            {
                if (armorClass == "Plate")
                {
                    if (entity.abilities.ContainsKey("Plate Proficiency")) return true;
                    else return false;
                }
                if (armorClass == "Mail")
                {
                    if (entity.abilities.ContainsKey("Plate Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Mail Proficiency")) return true;
                    else return false;
                }
                if (armorClass == "Leather")
                {
                    if (entity.abilities.ContainsKey("Plate Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Mail Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Leather Proficiency")) return true;
                    else return false;
                }
                if (armorClass == "Cloth")
                {
                    if (entity.abilities.ContainsKey("Plate Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Mail Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Leather Proficiency")) return true;
                    else if (entity.abilities.ContainsKey("Cloth Proficiency")) return true;
                    else return false;
                }
                return true;
            }
            else return entity.abilities.ContainsKey(armorClass + " Proficiency");
        }
        else if (type == "Pouch") return entity.abilities.ContainsKey("Pouch Proficiency");
        else if (type == "Quiver") return entity.abilities.ContainsKey("Quiver Proficiency");
        else if (type == "Libram") return entity.abilities.ContainsKey("Libram Proficiency");
        else if (type == "Totem") return entity.abilities.ContainsKey("Totem Proficiency");
        else if (type == "Idol") return entity.abilities.ContainsKey("Idol Proficiency");
        else if (type == "Two Handed")
        {
            if (detailedType == "Sword") return entity.abilities.ContainsKey("Two Handed Sword Proficiency");
            else if (detailedType == "Axe") return entity.abilities.ContainsKey("Two Handed Axe Proficiency");
            else if (detailedType == "Mace") return entity.abilities.ContainsKey("Two Handed Mace Proficiency");
            else if (detailedType == "Polearm") return entity.abilities.ContainsKey("Polearm Proficiency");
            else if (detailedType == "Staff") return entity.abilities.ContainsKey("Staff Proficiency");
            else return true;
        }
        else if (type == "Off Hand")
        {
            if (detailedType == "Shield") return entity.abilities.ContainsKey("Shield Proficiency");
            else return entity.abilities.ContainsKey("Off Hand Proficiency");
        }
        else if (type == "Ranged Weapon")
        {
            if (detailedType == "Bow") return entity.abilities.ContainsKey("Bow Proficiency");
            else if (detailedType == "Crossbow") return entity.abilities.ContainsKey("Crossbow Proficiency");
            else if (detailedType == "Gun") return entity.abilities.ContainsKey("Gun Proficiency");
            else return true;
        }
        else if (type == "One Handed")
        {
            if (detailedType == "Dagger") return entity.abilities.ContainsKey("Dagger Proficiency");
            else if (detailedType == "Sword") return entity.abilities.ContainsKey("One Handed Sword Proficiency");
            else if (detailedType == "Axe") return entity.abilities.ContainsKey("One Handed Axe Proficiency");
            else if (detailedType == "Mace") return entity.abilities.ContainsKey("One Handed Mace Proficiency");
            else if (detailedType == "Wand") return entity.abilities.ContainsKey("Wand Proficiency");
            else if (detailedType == "Fist Weapon") return entity.abilities.ContainsKey("Fist Weapon Proficiency");
            else return true;
        }
        else return true;
    }

    //Checks whether a chosen entity can equip this item
    //While [downgradeArmor] is set to false this function does not allow
    //people downgrading their preferred armor class
    //For example it will say that a Paladin cannot equip a cloth item
    public bool CanEquip(Entity entity, bool downgradeArmor, bool showWhyNot)
    {
        if (type == "Miscellaneous" || type == "Trade Good" || type == "Recipe") return false;
        bool result = true;
        if (specs != null && !specs.Contains(entity.spec))
        {
            if (showWhyNot)
                SpawnFallingText(new Vector2(0, 34), "Your class can't equip this item", "Red");
            result = false;
        }
        if (result)
        {
            if (type == "Bag")
            {
                if (entity.inventory.bags.Count >= defines.maxBagsEquipped)
                {
                    if (showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "No more free bag slots", "Red");
                    result = false;
                }
            }
            else if (armorClass != null)
            {
                if (downgradeArmor)
                {
                    if (armorClass == "Plate")
                    {
                        if (!entity.abilities.ContainsKey("Plate Proficiency"))
                        {
                            if (showWhyNot)
                                SpawnFallingText(new Vector2(0, 34), "You can't wear plate armor", "Red");
                            result = false;
                        }
                    }
                    else if (armorClass == "Mail")
                    {
                        if (!entity.abilities.ContainsKey("Mail Proficiency") && !entity.abilities.ContainsKey("Plate Proficiency"))
                        {
                            if (showWhyNot)
                                SpawnFallingText(new Vector2(0, 34), "You can't wear mail armor", "Red");
                            result = false;
                        }
                    }
                    else if (armorClass == "Leather")
                    {
                        if (!entity.abilities.ContainsKey("Leather Proficiency") && !entity.abilities.ContainsKey("Mail Proficiency") && !entity.abilities.ContainsKey("Plate Proficiency"))
                        {
                            if (showWhyNot)
                                SpawnFallingText(new Vector2(0, 34), "You can't wear leather armor", "Red");
                            result = false;
                        }
                    }
                    else if (armorClass == "Cloth")
                    {
                        if (!entity.abilities.ContainsKey("Cloth Proficiency") && !entity.abilities.ContainsKey("Leather Proficiency") && !entity.abilities.ContainsKey("Mail Proficiency") && !entity.abilities.ContainsKey("Plate Proficiency"))
                        {
                            if (showWhyNot)
                                SpawnFallingText(new Vector2(0, 34), "You can't wear cloth armor", "Red");
                            result = false;
                        }
                    }
                }
                else if (!entity.abilities.ContainsKey(armorClass + " Proficiency"))
                {
                    if (showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't wear " + armorClass + " armor", "Red");
                    result = false;
                }
            }
            else if (type == "Pouch")
            {
                result = entity.abilities.ContainsKey("Pouch Proficiency");
                if (!result && showWhyNot)
                    SpawnFallingText(new Vector2(0, 34), "You can't use ammo pouches", "Red");
            }
            else if (type == "Quiver")
            {
                result = entity.abilities.ContainsKey("Quiver Proficiency");
                if (!result && showWhyNot)
                    SpawnFallingText(new Vector2(0, 34), "You can't use quivers", "Red");
            }
            else if (type == "Libram")
            {
                result = entity.abilities.ContainsKey("Libram Proficiency");
                if (!result && showWhyNot)
                    SpawnFallingText(new Vector2(0, 34), "You can't use librams", "Red");
            }
            else if (type == "Totem")
            {
                result = entity.abilities.ContainsKey("Totem Proficiency");
                if (!result && showWhyNot)
                    SpawnFallingText(new Vector2(0, 34), "You can't use totems", "Red");
            }
            else if (type == "Idol")
            {
                result = entity.abilities.ContainsKey("Idol Proficiency");
                if (!result && showWhyNot)
                    SpawnFallingText(new Vector2(0, 34), "You can't use idols", "Red");
            }
            else if (type == "Two Handed")
            {
                if (detailedType == "Sword")
                {
                    result = entity.abilities.ContainsKey("Two Handed Sword Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use two handed swords", "Red");
                }
                else if (detailedType == "Axe")
                {
                    result = entity.abilities.ContainsKey("Two Handed Axe Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use two handed axes", "Red");
                }
                else if (detailedType == "Mace")
                {
                    result = entity.abilities.ContainsKey("Two Handed Mace Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use two handed maces", "Red");
                }
                else if (detailedType == "Polearm")
                {
                    result = entity.abilities.ContainsKey("Polearm Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use polearms", "Red");
                }
                else if (detailedType == "Staff")
                {
                    result = entity.abilities.ContainsKey("Staff Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use staves", "Red");
                }
                else
                {
                    result = false;
                    if (showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use two handed weapons", "Red");
                }
            }
            else if (type == "Off Hand")
            {
                if (detailedType == "Shield") result = entity.abilities.ContainsKey("Shield Proficiency");
                else result = entity.abilities.ContainsKey("Off Hand Proficiency");
            }
            else if (type == "Ranged Weapon")
            {
                if (detailedType == "Bow")
                {
                    result = entity.abilities.ContainsKey("Bow Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use bows", "Red");
                }
                else if (detailedType == "Crossbow")
                {
                    result = entity.abilities.ContainsKey("Crossbow Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use crossbows", "Red");
                }
                else if (detailedType == "Gun")
                {
                    result = entity.abilities.ContainsKey("Gun Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use guns", "Red");
                }
                else
                {
                    result = false;
                    if (showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use ranged weapons", "Red");
                }
            }
            else if (type == "One Handed")
            {
                if (detailedType == "Dagger")
                {
                    result = entity.abilities.ContainsKey("Dagger Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use daggers", "Red");
                }
                else if (detailedType == "Sword")
                {
                    result = entity.abilities.ContainsKey("One Handed Sword Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use one handed swords", "Red");
                }
                else if (detailedType == "Axe")
                {
                    result = entity.abilities.ContainsKey("One Handed Axe Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use one handed axes", "Red");
                }
                else if (detailedType == "Mace")
                {
                    result = entity.abilities.ContainsKey("One Handed Mace Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use one handed maces", "Red");
                }
                else if (detailedType == "Wand")
                {
                    result = entity.abilities.ContainsKey("Wand Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use wands", "Red");
                }
                else if (detailedType == "Fist Weapon")
                {
                    result = entity.abilities.ContainsKey("Fist Weapon Proficiency");
                    if (!result && showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use fist weapons", "Red");
                }
                else
                {
                    result = false;
                    if (showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You can't use one handed weapons", "Red");
                }
            }
        }
        if (result && lvl > entity.level)
        {
            if (showWhyNot)
                SpawnFallingText(new Vector2(0, 34), "You don't meet the level requirements", "Red");
            result = false;
        }
        return result;
    }

    #endregion

    #region Using
    
    //Tells whether a chosen entity can use this item
    //This does not include equipping it
    public bool CanUse(Entity entity, bool showWhyNot)
    {
        if (questsStarted != null && questsStarted.Count > 0)
        {
            var quest = Quest.quests.FindAll(x => questsStarted.Contains(x.questID)).Find(x => currentSave.player.CanSeeItemQuest(x));
            if (currentSave.player.completedQuests.Contains(quest.questID))
            {
                if (showWhyNot)
                {
                    PlaySound("QuestFailed");
                    SpawnFallingText(new Vector2(0, 34), "Quest already completed", "Red");
                }
            }
            else if (quest.requiredLevel > currentSave.player.level)
            {
                if (showWhyNot)
                {
                    PlaySound("QuestFailed");
                    SpawnFallingText(new Vector2(0, 34), "Requires level " + quest.requiredLevel, "Red");
                }
            }
            else if (quest.faction != null && !currentSave.player.IsRankHighEnough(currentSave.player.ReputationRank(quest.faction), quest.requiredRank))
            {
                if (showWhyNot)
                {
                    PlaySound("QuestFailed");
                    SpawnFallingText(new Vector2(0, 34), "Requires " + quest.requiredRank + " with " + quest.faction, "Red");
                }
            }
            else return true;
        }
        else if (type == "Recipe")
        {
            var recipe = Recipe.recipes.Find(x => name.Contains(x.name));
            if (recipe != null)
            {
                if (!entity.professionSkills.ContainsKey(recipe.profession))
                {
                    if (showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You don't know the required profession", "Red");
                }
                else if (entity.professionSkills[recipe.profession].Item1 < recipe.learnedAt)
                {
                    if (showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "Not enough skill in the profession", "Red");
                }
                else if (entity.learnedRecipes[recipe.profession].Contains(recipe.name))
                {
                    if (showWhyNot)
                        SpawnFallingText(new Vector2(0, 34), "You already know this recipe", "Red");
                }
                else return true;
            }
            else Debug.Log("ERROR 007: Did not find a dedicated recipe to item: \"" + name + "\"");
        }
        else if (abilities != null)
            return CDesktop.title == "Game" == combatUse;
        return false;
    }

    //Uses this item on a chosen entity
    //This never equips any item, it uses it, but never equips it
    public void Use(Entity entity)
    {
        if (questsStarted != null)
        {
            var all = Quest.quests.FindAll(x => questsStarted.Contains(x.questID)).ToList();
            var quest = all.Find(x => currentSave.player.CanSeeItemQuest(x));
            if (quest == null)
            {
                PlaySound("QuestFailed");
                SpawnFallingText(new Vector2(0, 34), "Requirements not met", "Red");
            }
            else
            {
                SpawnDesktopBlueprint("QuestLog");
                CloseDesktop("EquipmentScreen");
                if (currentSave.player.currentQuests.Exists(x => x.questID == quest.questID))
                {
                    Quest.quest = Quest.quests.Find(x => x.questID == quest.questID);
                    SpawnWindowBlueprint("Quest");
                }
                else
                {
                    Quest.quest = quest;
                    SpawnWindowBlueprint("QuestAdd");
                }
            }
        }
        else if (type == "Recipe")
        {
            var recipe = Recipe.recipes.Find(x => name.Contains(x.name));
            entity.LearnRecipe(recipe);
            PlaySound("DesktopSkillLearned");
            SpawnFallingText(new Vector2(0, 34), "New recipe learned", "Blue");
            if (amount > 1) amount--;
            else entity.inventory.items.Remove(this);
        }
        else currentSave.CallEvents(new() { { "Trigger", "ItemUsed" }, { "ItemHash", GetHashCode() + "" } });
    }

    //Check whether a chosen entity meets requirements to buy this item
    public bool CanBuy(Entity entity)
    {
        return entity.inventory.money >= price && (faction == null || currentSave.player.IsRankHighEnough(currentSave.player.ReputationRank(faction), reputationRequired));
    }

    #endregion

    #region Professions

    //Indicates whether the item is disenchantable
    public bool IsDisenchantable()
    {
        if (new List<string> { "Miscellaneous", "Trade Good", "Recipe", "Bag" }.Contains(type)) return false;
        if (rarity != "Uncommon" && rarity != "Rare" && rarity != "Epic") return false;
        return true;
    }

    //Generates disenchanting loot based on this item
    public Inventory GenerateDisenchantLoot()
    {
        var rarities = new List<string>() { "Uncommon" };
        if (rarity == "Rare" || rarity == "Epic") rarities.Add("Rare");
        if (rarity == "Epic") rarities.Add("Epic");
        var drops = GeneralDrop.generalDrops.FindAll(x => x.dropStart <= itemToDisenchant.ilvl && x.dropEnd >= itemToDisenchant.ilvl && rarities.Any(y => x.category == "Disenchant " + y));
        var inv = new Inventory(true);
        if (drops.Count > 0)
            foreach (var drop in drops)
                if (Roll(drop.rarity))
                {
                    int amount = 1;
                    for (int i = 1; i < drop.dropCount; i++) amount += Roll(50) ? 1 : 0;
                    inv.AddItem(items.Find(x => x.name == drop.item).CopyItem(amount));
                }
        return inv;
    }


    #endregion

    #region Print

    public static void PrintBankItem(Item item)
    {
        AddBigButton(item.icon,
            (h) =>
            {
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                if (movingItem == null)
                {
                    if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                    {
                        String.splitAmount.Set("");
                        SpawnWindowBlueprint("SplitItem");
                        CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                        movingItemX = h.region.bigButtons.IndexOf(h.GetComponent<LineBigButton>());
                        movingItemY = h.window.headerGroup.regions.IndexOf(h.region) - 2;
                        splitDelegate = () =>
                        {
                            var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                            if (amount >= item.amount) PickUpMovingItem("Bank", null);
                            else PickUpMovingItem("Bank", null, amount);
                            Respawn("Bank", true);
                            Respawn("Inventory", true);
                        };
                    }
                    else PickUpMovingItem("Bank", h);
                }
                else if (movingItem != null)
                    SwapMovingItem(h);
            },
            (h) =>
            {
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                var canAdd = currentSave.player.inventory.CanAddItem(item);
                if (movingItem == null && canAdd)
                {
                    if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                    {
                        String.splitAmount.Set("");
                        SpawnWindowBlueprint("SplitItem");
                        CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                        splitDelegate = () =>
                        {
                            var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                            if (amount <= 0) return;
                            PlaySound(item.ItemSound("PickUp"), 0.8f);
                            if (amount >= item.amount)
                            {
                                currentSave.player.inventory.AddItem(item);
                                currentSave.banks[town.name].items.Remove(item);
                            }
                            else
                            {
                                currentSave.player.inventory.AddItem(item.CopyItem(amount));
                                item.amount -= amount;
                            }
                            Respawn("Inventory");
                            Respawn("Bank");
                        };
                    }
                    else
                    {
                        PlaySound(item.ItemSound("PickUp"), 0.8f);
                        currentSave.player.inventory.AddItem(item);
                        currentSave.banks[town.name].items.Remove(item);
                        Respawn("Inventory");
                        Respawn("Bank");
                    }
                }
                else if (movingItem == null && !canAdd) SpawnFallingText(new Vector2(0, 34), "Inventory is full", "Red");
            },
            (h) => () =>
            {
                if (item == null) return;
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                PrintItemTooltip(item);
            }
        );
        if (settings.rarityIndicators.Value())
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 3);
        if (item.maxStack > 1) SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * item.x, item.amount + "", "", "", "Right");
    }

    public static void PrintVendorItem(StockItem stockItem, Item buyback)
    {
        var item = stockItem != null ? items.Find(x => x.name == stockItem.item).CopyItem(stockItem.amount) : buyback;
        AddBigButton(item.icon,
            null,
            (h) =>
            {
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (movingItem == null && currentSave.player.inventory.CanAddItem(item))
                {
                    if (buyback != null)
                    {
                        if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                        {
                            String.splitAmount.Set("1");
                            SpawnWindowBlueprint("SplitItem");
                            CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                            splitDelegate = () =>
                            {
                                var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                                if (amount <= 0)
                                {
                                    SpawnFallingText(new Vector2(0, 34), "Invalid amount", "Red");
                                    return;
                                }
                                if (amount > item.amount) amount = item.amount;
                                if (currentSave.player.inventory.money >= item.price * amount)
                                {
                                    PlaySound("DesktopTransportPay");
                                    currentSave.player.inventory.money -= item.price * amount;
                                    if (amount == item.amount)
                                    {
                                        item.minutesLeft = 0;
                                        currentSave.buyback.items.Remove(item);
                                        currentSave.player.inventory.AddItem(item);
                                    }
                                    else
                                    {
                                        var newItem = item.CopyItem(amount);
                                        newItem.minutesLeft = 0;
                                        currentSave.player.inventory.AddItem(newItem);
                                        item.amount -= amount;
                                    }
                                    Respawn("Inventory");
                                    Respawn("VendorBuyback");
                                }
                                else SpawnFallingText(new Vector2(0, 34), "Not enough money to buy back", "Red");
                            };
                        }
                        else if (item.amount > 0 && currentSave.player.inventory.money >= item.price * item.amount)
                        {
                            PlaySound("DesktopTransportPay");
                            var price = item.price * item.amount;
                            currentSave.player.inventory.money -= price;
                            currentSave.buyback.items.Remove(item);
                            currentSave.player.inventory.AddItem(item);
                            Respawn("Inventory");
                        }
                        else if (item.amount > 0) SpawnFallingText(new Vector2(0, 34), "Not enough money to buy back", "Red");
                    }
                    else if (stockItem != null)
                    {
                        if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                        {
                            String.splitAmount.Set(item.amount + "");
                            SpawnWindowBlueprint("SplitItem");
                            CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                            splitDelegate = () =>
                            {
                                var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                                if (amount <= 0)
                                {
                                    SpawnFallingText(new Vector2(0, 34), "Invalid amount", "Red");
                                    return;
                                }
                                if (amount > item.amount) amount = item.amount;
                                if (currentSave.player.inventory.money >= item.price * amount * 4)
                                {
                                    PlaySound("DesktopTransportPay");
                                    stockItem.amount -= amount;
                                    if (stockItem.minutesLeft == 0) stockItem.minutesLeft = stockItem.restockSpeed;
                                    currentSave.player.inventory.AddItem(item.CopyItem(amount));
                                    currentSave.player.inventory.money -= item.price * amount * 4;
                                    Respawn("Inventory");
                                    Respawn("Vendor");
                                }
                                else SpawnFallingText(new Vector2(0, 34), "Not enough money", "Red");
                            };
                        }
                        else if (stockItem.amount > 0 && currentSave.player.inventory.money >= item.price * 4)
                        {
                            PlaySound("DesktopTransportPay");
                            stockItem.amount -= 1;
                            if (stockItem.minutesLeft == 0) stockItem.minutesLeft = stockItem.restockSpeed;
                            currentSave.player.inventory.AddItem(item.CopyItem(1));
                            currentSave.player.inventory.money -= item.price * 4;
                            Respawn("Inventory");
                        }
                        else if (stockItem.amount <= 0) SpawnFallingText(new Vector2(0, 34), "No more items in stock", "Red");
                        else SpawnFallingText(new Vector2(0, 34), "Not enough money", "Red");
                    }
                }
                else SpawnFallingText(new Vector2(0, 34), "Inventory is full", "Red");
            },
            (h) => () =>
            {
                if (item == null) return;
                if (WindowUp("InventorySort")) return;
                PrintItemTooltip(item, Input.GetKey(KeyCode.LeftShift), buyback == null ? 4 : 1);
            }
        );
        if (settings.rarityIndicators.Value())
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 3);
        if (item.questsStarted != null)
        {
            var all = Quest.quests.FindAll(x => item.questsStarted.Contains(x.questID)).ToList();
            var status = "Cant";
            foreach (var quest in all)
            {
                if (currentSave.player.completedQuests.Contains(quest.questID)) continue;
                if (quest.requiredLevel > currentSave.player.level) continue;
                if (currentSave.player.currentQuests.Exists(x => x.questID == quest.questID))
                { status = "Active"; break; }
                status = "Can"; break;
            }
            AddBigButtonOverlay("QuestStarter" + (status == "Can" ? "" : (status == "Active" ? "Active" : "Off")), 0, 4);
        }
        if (item.amount == 0) SetBigButtonToGrayscale();
        else if (item.IsWearable() && !item.HasProficiency(currentSave.player, true)) SetBigButtonToRed();
        if (stockItem != null || item.maxStack > 1) SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * ((buyback != null ? currentSave.buyback.items.IndexOf(buyback) : currentSave.vendorStock[town.name + ":" + Person.person.name].FindIndex(x => x.item == item.name)) % 5), item.amount + (false && buyback == null ?  "/" + currentSave.vendorStock[town.name + ":" + Person.person.name].Find(x => x.item == item.name).maxAmount : ""), "", "", "Right");
        if (stockItem != null && item.amount == 0 && stockItem.minutesLeft > 0) AddBigButtonCooldownOverlay(stockItem.minutesLeft / (double)stockItem.restockSpeed);
        else if (buyback != null && buyback.minutesLeft > 0) AddBigButtonBuybackOverlay(buyback.minutesLeft / (double)defines.buybackDecay);
    }

    public static void PrintInventoryItem(Item item)
    {
        AddBigButton(item.icon,
            (h) =>
            {
                if (WindowUp("ConfirmItemDisenchant")) return;
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                if (Cursor.cursor.color == "Pink")
                {
                    if (!h.GetComponent<SpriteRenderer>().material.name.Contains("Gray"))
                    {
                        itemToDisenchant = item;
                        Cursor.cursor.ResetColor();
                        PlaySound("DesktopMenuOpen", 0.6f);
                        Respawn("PlayerEquipmentInfo");
                        Respawn("PlayerWeaponsInfo");
                        SpawnWindowBlueprint("ConfirmItemDisenchant");
                    }
                }
                else if (movingItem == null)
                {
                    if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                    {
                        String.splitAmount.Set("");
                        SpawnWindowBlueprint("SplitItem");
                        CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                        movingItemX = h.region.bigButtons.IndexOf(h.GetComponent<LineBigButton>());
                        movingItemY = h.window.headerGroup.regions.IndexOf(h.region) - 1;
                        splitDelegate = () =>
                        {
                            var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                            if (amount >= item.amount) PickUpMovingItem("Inventory", null);
                            else PickUpMovingItem("Inventory", null, amount);
                            Respawn("Inventory", true);
                            Respawn("Bank", true);
                        };
                    }
                    else PickUpMovingItem("Inventory", h);
                }
                else if (movingItem != null)
                    SwapMovingItem(h);
            },
            (h) =>
            {
                if (item == null || itemToDisenchant == item) return;
                if (Cursor.cursor.color == "Pink") return;
                if (WindowUp("ConfirmItemDisenchant")) return;
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                if (CDesktop.windows.Exists(x => x.title.StartsWith("Vendor")))
                {
                    if (item.price > 0)
                        if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                        {
                            String.splitAmount.Set("1");
                            SpawnWindowBlueprint("SplitItem");
                            CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                            splitDelegate = () =>
                            {
                                var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                                if (amount <= 0) return;
                                PlaySound("DesktopTransportPay");
                                PlaySound(item.ItemSound("PutDown"));
                                if (amount > item.amount) amount = item.amount;
                                currentSave.buyback ??= new(true);
                                if (amount == item.amount)
                                {
                                    if (!item.indestructible)
                                        item.minutesLeft = defines.buybackDecay;
                                    currentSave.buyback.AddItem(item);
                                    currentSave.player.inventory.items.Remove(item);
                                }
                                else
                                {
                                    var newItem = item.CopyItem(amount);
                                    currentSave.buyback.AddItem(newItem);
                                    if (!newItem.indestructible)
                                        newItem.minutesLeft = defines.buybackDecay;
                                    item.amount -= amount;
                                }
                                currentSave.player.inventory.money += item.price * amount;
                                Respawn("Inventory");
                                CloseWindow("Vendor");
                                Respawn("VendorBuyback");
                            };
                        }
                        else
                        {
                            PlaySound("DesktopTransportPay");
                            PlaySound(item.ItemSound("PutDown"));
                            currentSave.buyback ??= new(true);
                            currentSave.player.inventory.money += item.price * item.amount;
                            if (!item.indestructible)
                                item.minutesLeft = defines.buybackDecay;
                            currentSave.buyback.AddItem(item);
                            currentSave.player.inventory.items.Remove(item);
                            CloseWindow("Vendor");
                            Respawn("VendorBuyback");
                        }
                }
                else if (WindowUp("Bank"))
                {
                    var canAdd = currentSave.banks[town.name].CanAddItem(item);
                    if (movingItem == null && canAdd)
                        if (item.amount > 1 && Input.GetKey(KeyCode.LeftShift))
                        {
                            String.splitAmount.Set("1");
                            SpawnWindowBlueprint("SplitItem");
                            CDesktop.LBWindow().LBRegionGroup().LBRegion().inputLine.Activate();
                            splitDelegate = () =>
                            {
                                var amount = int.Parse(String.splitAmount.value == "" ? "0" : String.splitAmount.value);
                                if (amount <= 0) return;
                                PlaySound(item.ItemSound("PutDown"), 0.8f);
                                if (amount > item.amount) amount = item.amount;
                                if (amount == item.amount)
                                {
                                    currentSave.banks[town.name].AddItem(item);
                                    currentSave.player.inventory.items.Remove(item);
                                }
                                else
                                {
                                    currentSave.banks[town.name].AddItem(item.CopyItem(amount));
                                    item.amount -= amount;
                                }
                                Respawn("Inventory");
                                Respawn("Bank");
                            };
                        }
                        else
                        {
                            PlaySound(item.ItemSound("PutDown"), 0.8f);
                            currentSave.banks[town.name].AddItem(item);
                            currentSave.player.inventory.items.Remove(item);
                            Respawn("Inventory");
                            Respawn("Bank");
                        }
                    else if (movingItem == null && !canAdd) SpawnFallingText(new Vector2(0, 34), "Bank space is full", "Red");
                }
                else
                {
                    if (item.CanEquip(currentSave.player, true, true))
                    {
                        PlaySound(item.ItemSound("PickUp"), 0.8f);
                        item.Equip(currentSave.player, false, Input.GetKey(KeyCode.LeftAlt));
                        Respawn("Inventory", true);
                        Respawn("PlayerEquipmentInfo", true);
                        Respawn("PlayerWeaponsInfo", true);
                    }
                    else if (item.CanUse(currentSave.player, true))
                    {
                        PlaySound(item.ItemSound("Use"), 0.8f);
                        item.Use(currentSave.player);
                        Respawn("Inventory", true);
                        Respawn("PlayerEquipmentInfo", true);
                        Respawn("PlayerWeaponsInfo", true);
                    }
                }
            },
            (h) => () =>
            {
                if (item == null) return;
                if (WindowUp("ConfirmItemDisenchant")) return;
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                PrintItemTooltip(item, Input.GetKey(KeyCode.LeftShift));
            },
            (h) =>
            {
                if (openedItem == item) return;
                if (item.indestructible) return;
                if (movingItem != null) return;
                if (WindowUp("ConfirmItemDisenchant")) return;
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                if (WindowUp("BankSort")) return;
                itemToDestroy = item;
                PlaySound("DesktopMenuOpen", 0.6f);
                SpawnWindowBlueprint("ConfirmItemDestroy");
            }
        );
        if (settings.rarityIndicators.Value())
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 3);
        if (item.questsStarted != null)
        {
            var all = Quest.quests.FindAll(x => item.questsStarted.Contains(x.questID)).ToList();
            var status = "Cant";
            foreach (var quest in all)
            {
                if (currentSave.player.completedQuests.Contains(quest.questID)) continue;
                if (quest.requiredLevel > currentSave.player.level) continue;
                if (currentSave.player.currentQuests.Exists(x => x.questID == quest.questID))
                { status = "Active"; break; }
                status = "Can"; break;
            }
            AddBigButtonOverlay("QuestStarter" + (status == "Can" ? "" : (status == "Active" ? "Active" : "Off")), 0, 4);
        }
        if (item.CanEquip(currentSave.player, false, false) && currentSave.player.IsItemNewSlot(item) && (settings.upgradeIndicators.Value() || settings.newSlotIndicators.Value()))
            AddBigButtonOverlay(settings.newSlotIndicators.Value() ? "OtherItemNewSlot" : "OtherItemUpgrade", 0, 2);
        else if (settings.upgradeIndicators.Value() && item.CanEquip(currentSave.player, false, false) && currentSave.player.IsItemAnUpgrade(item))
            AddBigButtonOverlay("OtherItemUpgrade", 0, 2);
        if (Cursor.cursor.color == "Pink" && !item.IsDisenchantable()) SetBigButtonToGrayscale();
        else if (Cursor.cursor.color == "Pink") AddBigButtonOverlay("OtherGlowDisenchantable" + item.rarity, 0, 2);
        if (openedItem == item || itemToDisenchant == item || itemToDestroy == item) { AddBigButtonOverlay("OtherGridBlurred", 0, 3); SetBigButtonToGrayscale(); }
        if (item.maxStack > 1) SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * item.x, item.amount + "", "", "", "Right");
    }

    public static void PrintLootItem(Item item)
    {
        AddBigButton(item.icon,
            (h) => Click(),
            (h) => Click(),
            (h) => () =>
            {
                if (item == null) return;
                if (WindowUp("ConfirmItemDestroy")) return;
                if (WindowUp("InventorySort")) return;
                PrintItemTooltip(item, Input.GetKey(KeyCode.LeftShift));
            }
        );
        if (Board.board != null && Board.board.results.exclusiveItems.Count > 1 && Board.board.results.exclusiveItems.Contains(item.name))
            AddBigButtonOverlay("OtherItemExclusive", 0, 2);
        if (settings.rarityIndicators.Value() && item.type != "Currency")
            AddBigButtonOverlay("OtherRarity" + item.rarity + (settings.bigRarityIndicators.Value() ? "Big" : ""), 0, 3);
        if (item.questsStarted != null)
        {
            var all = Quest.quests.FindAll(x => item.questsStarted.Contains(x.questID)).ToList();
            var status = "Cant";
            foreach (var quest in all)
            {
                if (currentSave.player.completedQuests.Contains(quest.questID)) continue;
                if (quest.requiredLevel > currentSave.player.level) continue;
                if (currentSave.player.currentQuests.Exists(x => x.questID == quest.questID))
                { status = "Active"; break; }
                status = "Can"; break;
            }
            AddBigButtonOverlay("QuestStarter" + (status == "Can" ? "" : (status == "Active" ? "Active" : "Off")), 0, 4);
        }
        if (item.CanEquip(currentSave.player, false, false) && currentSave.player.IsItemNewSlot(item) && (settings.upgradeIndicators.Value() || settings.newSlotIndicators.Value()))
            AddBigButtonOverlay(settings.newSlotIndicators.Value() ? "OtherItemNewSlot" : "OtherItemUpgrade", 0, 2);
        else if (settings.upgradeIndicators.Value() && item.CanEquip(currentSave.player, false, false) && currentSave.player.IsItemAnUpgrade(item))
            AddBigButtonOverlay("OtherItemUpgrade", 0, 2);
        if (item.maxStack > 1 && item.type != "Currency") SpawnFloatingText(CDesktop.LBWindow().LBRegionGroup().LBRegion().transform.position + new Vector3(32, -27) + new Vector3(38, 0) * item.x, item.amount + "", "", "", "Right");

        void Click()
        {
            if (movingItem == null && currentSave.player.inventory.CanAddItem(item))
            {
                PlaySound(item.ItemSound("PutDown"), 0.8f);
                if (CDesktop.title == "CombatResultsLoot")
                {
                    currentSave.player.inventory.AddItem(item);
                    Board.board.results.inventory.items.Remove(item);
                    if (Board.board.results.exclusiveItems.Contains(item.name))
                        Board.board.results.inventory.items.RemoveAll(x => Board.board.results.exclusiveItems.Contains(x.name));
                    Board.board.results.inventory.ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && Board.board.results.inventory.items.Count == 0)
                    {
                        CloseDesktop("CombatResultsLoot");
                        SwitchDesktop("CombatResults");
                        Respawn("CombatResults");
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("CombatResultsLoot");
                    }
                }
                else if (CDesktop.title == "MiningLoot")
                {
                    PlaySound("Mining" + random.Next(1, 6));
                    currentSave.player.inventory.AddItem(item);
                    Board.board.results.miningLoot.items.Remove(item);
                    Board.board.results.miningLoot.ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && Board.board.results.miningLoot.items.Count == 0)
                    {
                        CloseDesktop("MiningLoot");
                        Respawn("CombatResultsMining");
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("MiningLoot");
                    }
                }
                else if (CDesktop.title == "HerbalismLoot")
                {
                    PlaySound("HerbGather" + random.Next(1, 5));
                    currentSave.player.inventory.AddItem(item);
                    Board.board.results.herbalismLoot.items.Remove(item);
                    Board.board.results.herbalismLoot.ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && Board.board.results.herbalismLoot.items.Count == 0)
                    {
                        CloseDesktop("HerbalismLoot");
                        Respawn("CombatResultsHerbalism");
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("HerbalismLoot");
                    }
                }
                else if (CDesktop.title == "SkinningLoot")
                {
                    PlaySound("SkinGather" + random.Next(1, 4));
                    currentSave.player.inventory.AddItem(item);
                    Board.board.results.skinningLoots[Board.board.results.selectedSkinningLoot].items.Remove(item);
                    Board.board.results.skinningLoots[Board.board.results.selectedSkinningLoot].ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && Board.board.results.skinningLoots[Board.board.results.selectedSkinningLoot].items.Count == 0)
                    {
                        CloseDesktop("SkinningLoot");
                        Respawn("CombatResultsSkinning1");
                        Respawn("CombatResultsSkinning2");
                        Respawn("CombatResultsSkinning3");
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("SkinningLoot");
                    }
                }
                else if (CDesktop.title == "DisenchantLoot")
                {
                    currentSave.player.inventory.AddItem(item);
                    disenchantLoot.items.Remove(item);
                    disenchantLoot.ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && disenchantLoot.items.Count == 0)
                    {
                        CloseDesktop("DisenchantLoot");
                        CDesktop.RespawnAll();
                    }
                    else Respawn("Inventory");
                }
                else if (CDesktop.title == "ChestLoot")
                {
                    currentSave.player.inventory.AddItem(item);
                    currentSave.openedChests[SiteHostileArea.area.name].inventory.items.Remove(item);
                    currentSave.openedChests[SiteHostileArea.area.name].inventory.ApplySortOrder();
                    if (settings.autoCloseLoot.Value() && currentSave.openedChests[SiteHostileArea.area.name].inventory.items.Count == 0)
                    {
                        CloseDesktop("ChestLoot");
                        CDesktop.RespawnAll();
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("ChestLoot");
                    }
                }
                else if (CDesktop.title == "ContainerLoot")
                {
                    currentSave.player.inventory.AddItem(item);
                    openedItem.itemsInside.Remove(item);
                    ApplySortOrder(openedItem.itemsInside);
                    if (settings.autoCloseLoot.Value() && openedItem.itemsInside.Count == 0)
                    {
                        currentSave.player.inventory.items.Remove(openedItem);
                        openedItem = null;
                        CloseDesktop("ContainerLoot");
                    }
                    else
                    {
                        Respawn("Inventory");
                        Respawn("ContainerLoot");
                    }
                }
            }
            else SpawnFallingText(new Vector2(0, 34), "Inventory is full", "Red");
        }
    }

    //Prints an item's tooltip
    //Left Shift makes item compare appear in the tooltip if a compare is possible
    //Left Control makes the viewed item change to the recipe resulting item if possible
    public static void PrintItemTooltip(Item item, bool compare = false, double priceMultiplier = 1)
    {
        if (CDesktop.title == "Game") SetAnchor(Anchor.Bottom, 0, 37);
        else SetAnchor(-92, 142);
        AddHeaderGroup();
        SetRegionGroupWidth(182);
        if (Input.GetKey(KeyCode.LeftControl) && item.type == "Recipe")
        {
            var recipe = Recipe.recipes.Find(x => item.name.Contains(x.name));
            if (recipe != null) item = items.Find(x => x.name == recipe.results.First().Key).CopyItem(recipe.results.First().Value);
        }
        var split = item.name.Split(", ");
        AddHeaderRegion(() =>
        {
            AddLine(split[0], item.rarity);
            AddSmallButton(item.icon);
        });
        if (split.Length > 1) AddHeaderRegion(() => { AddLine("\"" + split[1] + "\"", item.rarity); });
        AddPaddingRegion(() =>
        {
            if (item.armorClass != null)
            {
                var copy = item.CopyItem(1);
                copy.specs = null;
                AddLine(item.armorClass + " ", currentSave != null && !copy.HasProficiency(currentSave.player, true) ? "DangerousRed" : "Gray");
                AddText(item.type);
            }
            else if (item.minPower > 0)
            {
                AddLine(item.type + " " + item.detailedType, currentSave != null && !currentSave.player.abilities.ContainsKey((new List<string> { "Polearm", "Staff", "Bow", "Crossbow", "Gun", "Dagger", "Fist Weapon", "Wand" }.Contains(item.detailedType) ? item.detailedType : item.type + " " + item.detailedType) + " Proficiency") ? "DangerousRed" : "Gray");
                AddLine((item.minPower + "").Replace(",", "."), "Gray");
                AddText(" - ", "HalfGray");
                AddText((item.maxPower + "").Replace(",", "."), "Gray");
                AddText(" Power modifier", "HalfGray");
                AddLine("Average modifier: ", "HalfGray");
                AddText((Math.Round((item.minPower + item.maxPower) / 2, 2) + "").Replace(",", "."), "Gray");
            }
            else if (item.bagSpace != 0) AddLine(item.bagSpace + " Slot Bag");
            else if (item.type == "Recipe")
            {
                var recipe = Recipe.recipes.Find(x => item.name.Contains(x.name));
                AddLine(recipe.profession + " " + item.name.Split(':')[0].ToLower(), currentSave != null && currentSave.player.professionSkills.ContainsKey(recipe.profession) ? "Gray" : "DangerousRed");
            }
            else if (item.type == "Off Hand") AddLine(item.type + (item.detailedType != null ? " " + item.detailedType : ""), currentSave != null && !currentSave.player.abilities.ContainsKey(item.detailedType == "Shield" ? "Shield Proficiency" : "Off Hand Proficiency") ? "DangerousRed" : "Gray");
            else AddLine(item.type ?? "");
            if (item.armor > 0) AddLine(item.armor + " Armor", "", "Right");
        });
        if (item.stats != null && item.stats.Count > 0)
            AddPaddingRegion(() =>
            {
                foreach (var stat in item.stats)
                    AddLine("+" + stat.Value + " " + stat.Key);
            });
        if (compare && item.IsWearable())
        {
            Item current = null;
            Item currentSecond = null;
            if (currentSave != null)
                if (item.type == "Two Handed" || item.type == "One Handed" && Input.GetKey(KeyCode.LeftAlt) || item.type == "Off Hand")
                {
                    current = currentSave.player.equipment.Get("Main Hand");
                    currentSecond = currentSave.player.equipment.Get("Off Hand");
                }
                else if (currentSave.player.equipment.ContainsKey(item.type))
                    current = currentSave.player.equipment[item.type];
            AddHeaderRegion(() => AddLine("Stat changes on equip:", "DarkGray"));
            AddPaddingRegion(() =>
            {
                var statsRecorded = new List<string>();
                var a1 = item.armor;
                var a2 = current == null ? 0 : current.armor;
                var a3 = currentSecond == null ? 0 : currentSecond.armor;
                if (a1 - a2 - a3 != 0)
                {
                    var balance = a1 - a2 - a3;
                    AddLine((balance > 0 ? "+" : "") + balance, balance > 0 ? "Uncommon" : "DangerousRed");
                    AddText(" Armor");
                }
                if (item.type == "Ranged Weapon")
                {
                    var newPower = item.minPower <= 0 ? 1 : Math.Round((item.minPower + item.maxPower) / 2, 2);
                    var oldPower = current == null || current.minPower <= 0 ? 1 : Math.Round((current.minPower + current.maxPower) / 2, 2);
                    if (newPower - oldPower != 0)
                    {
                        var balance = Math.Round(newPower - oldPower, 2);
                        AddLine(((balance > 0 ? "+" : "") + balance).Replace(",", "."), balance > 0 ? "Uncommon" : "DangerousRed");
                        AddText(" Power modifier");
                    }
                }
                else if (item.type == "Off Hand" || item.type == "One Handed" || item.type == "Two Handed")
                {
                    var newPower = item.minPower <= 0 ? 0 : (item.minPower + item.maxPower) / 2;
                    var b1d = Math.Round((item.type != "Off Hand" && !Input.GetKey(KeyCode.LeftAlt)) || item.type == "Two Handed" ? newPower : current == null || current.minPower <= 0 ? 0 : ((item.type == "Off Hand" || Input.GetKey(KeyCode.LeftAlt)) && current.type == "Two Handed" ? 0 : (current.minPower + current.maxPower) / 2), 2);
                    var b2d = Math.Round(item.type == "Two Handed" ? 0 : item.type == "Off Hand" || Input.GetKey(KeyCode.LeftAlt) ? newPower : currentSecond == null || currentSecond.minPower <= 0 ? 0 : (currentSecond.minPower + currentSecond.maxPower) / 2, 2);
                    if (b1d == 0 && b2d == 0) b1d = 1;
                    else if (b2d > 0)
                    {
                        b1d /= defines.dividerForDualWield;
                        b2d /= defines.dividerForDualWield;
                    }
                    var bd = Math.Round(b1d + b2d, 2);
                    var a1d = Math.Round(current == null || current.minPower <= 0 ? 0 : (current.minPower + current.maxPower) / 2, 2);
                    var a2d = Math.Round(currentSecond == null || currentSecond.minPower <= 0 ? 0 : (currentSecond.minPower + currentSecond.maxPower) / 2, 2);
                    if (a1d == 0 && a2d == 0) a1d = 1;
                    else if (a2d > 0)
                    {
                        a1d /= defines.dividerForDualWield;
                        a2d /= defines.dividerForDualWield;
                    }
                    var ad = Math.Round(a1d + a2d, 2);
                    if (bd - ad != 0)
                    {
                        var balance = Math.Round(bd - ad, 2);
                        AddLine(((balance > 0 ? "+" : "") + balance).Replace(",", "."), balance > 0 ? "Uncommon" : "DangerousRed");
                        AddText(" Power modifier");
                    }
                }
                a1 = item.block;
                a2 = current == null ? 0 : current.block;
                a3 = current == null ? 0 : current.block;
                if (a1 - a2 - a3 != 0)
                {
                    var balance = a1 - a2 - a3;
                    AddLine((balance > 0 ? "+" : "") + balance, balance > 0 ? "Uncommon" : "DangerousRed");
                    AddText(" Block");
                }
                if (item.stats != null)
                    foreach (var stat in item.stats)
                    {
                        statsRecorded.Add(stat.Key);
                        a2 = current != null && current.stats != null ? current.stats.Get(stat.Key) : 0;
                        a2 = currentSecond != null && currentSecond.stats != null ? current.stats.Get(stat.Key) : 0;
                        var balance = stat.Value - a2 - a3;
                        if (balance != 0)
                        {
                            AddLine((balance > 0 ? "+" : "") + balance, balance > 0 ? "Uncommon" : "DangerousRed");
                            AddText(" " + stat.Key);
                        }
                    }
                var fullLostStats = new Dictionary<string, int>();
                if (current != null && current.stats != null)
                    foreach (var stat in current.stats)
                        if (!statsRecorded.Contains(stat.Key))
                            fullLostStats.Inc(stat.Key, stat.Value);
                if (currentSecond != null && currentSecond.stats != null)
                    foreach (var stat in currentSecond.stats)
                        if (!statsRecorded.Contains(stat.Key))
                            fullLostStats.Inc(stat.Key, stat.Value);
                foreach (var stat in fullLostStats)
                {
                    AddLine("-" + stat.Value, "DangerousRed");
                    AddText(" " + stat.Key);
                }
                if (CDesktop.LBWindow().LBRegionGroup().LBRegion().lines.Count == 0)
                    AddLine("No changes", "Gray");
            });
        }
        if (item.specs != null)
            AddHeaderRegion(() =>
            {
                AddLine("Classes: ", "DarkGray");
                foreach (var spec in item.specs)
                {
                    AddText(spec, spec);
                    if (spec != item.specs.Last())
                        AddText(", ", "DarkGray");
                }
            });
        if (item.abilities != null)
            foreach (var ability in item.abilities)
            {
                var foo = Ability.abilities.Find(x => x.name == ability.Key);
                foo?.PrintDescription(currentSave.player, 182, ability.Value);
            }
        if (item.questsStarted != null) AddPaddingRegion(() => AddLine("Starts a quest", "HalfGray"));
        if (item.set != null)
        {
            var set = itemSets.Find(x => x.name == item.set);
            if (set != null)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Part of ", "DarkGray");
                    AddText(item.set, "Gray");
                });
                AddPaddingRegion(() =>
                {
                    foreach (var bonus in set.setBonuses)
                    {
                        var howMuch = currentSave != null && currentSave.player != null ? set.EquippedPieces(currentSave.player) : 0;
                        bool has = howMuch >= bonus.requiredPieces;
                        AddLine((has ? bonus.requiredPieces : howMuch) + "/" + bonus.requiredPieces + " Set: ", has ? "Uncommon" : "DarkGray");
                        if (bonus.description.Count > 0)
                            AddText(bonus.description[0], has ? "Uncommon" : "DarkGray");
                        for (int i = 0; i < bonus.description.Count - 1; i++)
                            AddLine(bonus.description[0], has ? "Uncommon" : "DarkGray");
                    }
                });
            }
        }
        if (item.type == "Recipe")
        {
            var recipe = Recipe.recipes.Find(x => item.name.Contains(x.name));
            if (recipe.results.Count > 0)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Results:", "DarkGray");
                });
                AddPaddingRegion(() =>
                {
                    foreach (var result in recipe.results)
                        AddLine(result.Key + " x" + result.Value);
                });
            }
            else if (recipe.enchantment)
            {
                AddHeaderRegion(() =>
                {
                    AddLine("Enchantment:", "DarkGray");
                });
                var e = Enchant.enchants.Find(x => x.name == recipe.name);
                AddPaddingRegion(() =>
                {
                    AddLine(e.type);
                    AddLine(e.Name());
                });
            }
            AddHeaderRegion(() =>
            {
                AddLine("Reagents:", "DarkGray");
            });
            AddPaddingRegion(() =>
            {
                foreach (var reagent in recipe.reagents)
                    AddLine(reagent.Key + " x" + reagent.Value);
            });
            AddHeaderRegion(() =>
            {
                AddLine("Required skill: ", "DarkGray");
                AddText(recipe.learnedAt + "", currentSave.player.professionSkills.ContainsKey(recipe.profession) && recipe.learnedAt <= currentSave.player.professionSkills[recipe.profession].Item1 ? "Uncommon" : "DangerousRed");
            });
        }
        if (item.enchant != null)
        {
            AddHeaderRegion(() =>
            {
                AddLine("Enchanted: " + item.enchant.Name(), "Uncommon");
            });
            //AddPaddingRegion(() =>
            //{
            //    foreach (var gain in item.enchant.gains)
            //        AddLine(gain.Key + " +" + gain.Value, "Uncommon");
            //});
        }
        if (item.lvl > 1)
            AddHeaderRegion(() =>
            {
                AddLine("Required level: ", "DarkGray");
                AddText(item.lvl + "", ColorRequiredLevel(item.lvl));
            });
        if (item.price > 0) PrintPriceRegion((int)(item.price * priceMultiplier) * (item.type != "Currency" ? 1 : item.amount), 38, 38, 49);
    }

    #endregion

    #region Other

    //This function returns the type of sound that this item makes when it is being manipulated
    public string ItemSound(string soundType)
    {
        string result;
        if (detailedType == "Staff") result = "WoodLarge";
        else if (detailedType == "Wand") result = "Wand";
        else if (detailedType == "Totem") result = "WoodLarge";
        else if (detailedType == "Bow") result = "WoodLarge";
        else if (detailedType == "Crossbow") result = "WoodLarge";
        else if (detailedType == "Gun") result = "MetalSmall";
        else if (detailedType == "Libram") result = "Ring";
        else if (detailedType == "Idol") result = "Ring";
        else if (detailedType == "Gem") result = "Gems";
        else if (detailedType == "Fish") result = "Meat";
        else if (detailedType == "Book") result = "Book";
        else if (detailedType == "Scepter") result = "Wand";
        else if (detailedType == "Lantern") result = "Wand";
        else if (detailedType == "Orb") result = "Wand";
        else if (detailedType == "Pouch") result = "Bag";
        else if (detailedType == "Potion") result = "Liquid";
        else if (detailedType == "Flowers") result = "Herb";
        else if (detailedType == "Torch") result = "WoodSmall";
        else if (detailedType == "Tool") result = "MetalSmall";
        else if (detailedType == "Quiver") result = "ClothLeather";
        else if (detailedType == "Shield") result = "MetalLarge";
        else if (detailedType == "Scroll") result = "ParchmentPaper";
        else if (detailedType == "Beak") result = "FoodGeneric";
        else if (detailedType == "Scale") result = "FoodGeneric";
        else if (detailedType == "Egg") result = "FoodGeneric";
        else if (detailedType == "Shell") result = "FoodGeneric";
        else if (detailedType == "Rune") result = "MetalSmall";
        else if (detailedType == "Dust") result = "Herb";
        else if (detailedType == "Rock") result = "RocksOre";
        else if (detailedType == "Ore") result = "RocksOre";
        else if (detailedType == "Bullet") result = "RocksOre";
        else if (detailedType == "Arrow") result = "WoodSmall";
        else if (detailedType == "Ingot") result = "MetalSmall";
        else if (detailedType == "Claw") result = "Meat";
        else if (detailedType == "Organ") result = "Meat";
        else if (detailedType == "Leather") result = "ClothLeather";
        else if (detailedType == "Essence") result = "Herb";
        else if (detailedType == "Box") result = "WoodSmall";
        else if (detailedType == "Cask") result = "WoodSmall";
        else if (detailedType == "Crate") result = "WoodSmall";
        else if (detailedType == "Crown") result = "MetalSmall";
        else if (detailedType == "Shard") result = "Gems";
        else if (detailedType == "Cloth") result = "ClothLeather";
        else if (detailedType == "Feather") result = "WoodSmall";
        else if (detailedType == "Letter") result = "ParchmentPaper";
        else if (detailedType == "Note") result = "ParchmentPaper";
        else if (detailedType == "Bandage") result = "ClothLeather";
        else if (detailedType == "Candle") result = "WoodSmall";
        else if (detailedType == "Drum") result = "WoodSmall";
        else if (detailedType == "Coin") result = "";
        else if (detailedType == "Key") result = "MetalSmall";
        else if (detailedType == "Horn") result = "MetalSmall";
        else if (detailedType == "Pick") result = "MetalLarge";
        else if (type == "Recipe") result = "ParchmentPaper";
        else if (type == "Bag") result = "Bag";
        else if (type == "Back") result = "ClothLeather";
        else if (type == "Neck") result = "Ring";
        else if (type == "Finger") result = "Ring";
        else if (type == "Trinket") result = "Ring";
        else if (type == "Off Hand") result = "Book";
        else if (type == "One Handed") result = "MetalSmall";
        else if (type == "Two Handed") result = "MetalLarge";
        else if (armorClass == "Cloth") result = "ClothLeather";
        else if (armorClass == "Leather") result = "ClothLeather";
        else if (armorClass == "Mail") result = "ChainLarge";
        else if (armorClass == "Plate") result = "ChainLarge";
        else result = "ClothLeather";
        return soundType + result;
    }

    //Sets a random permanent enchant for the item
    public void SetRandomEnchantment()
    {
        if (!randomEnchantment) return;
        var enchantment = GenerateEnchantment();
        if (enchantment == null) return;
        name += " " + enchantment.suffix;
        if (enchantment.stats.Count > 0)
        {
            stats = new();
            foreach (var stat in enchantment.stats)
                stats.Inc(stat.Key, EnchantmentStatGrowth(ilvl, stat.Value.Length));
        }

        PermanentEnchant GenerateEnchantment()
        {
            var containing = new List<PermanentEnchant>();
            var key = "";
            if (type == "One Handed" || type == "Two Handed") key = type + " " + detailedType;
            else if (armorClass != null) key = armorClass + " Armor";
            else if (type == "Off Hand") key = type;
            else if (detailedType != null) key = detailedType;
            else key = type;
            containing = pEnchants.FindAll(x => x.commonlyOn != null && x.commonlyOn.Contains(key) || x.rarelyOn != null && x.rarelyOn.Contains(key));
            if (Roll(10))
            {
                var rare = containing.FindAll(x => x.rarelyOn != null && x.rarelyOn.Contains(key));
                if (rare.Count > 0) return rare[random.Next(0, rare.Count)];
            }
            else
            {
                var common = containing.FindAll(x => x.commonlyOn != null && x.commonlyOn.Contains(key));
                if (common.Count > 0) return common[random.Next(0, common.Count)];
            }
            if (containing.Count == 0) return null;
            return containing[random.Next(0, containing.Count)];
        }
    }

    //Copies this item with a specific amount
    public Item CopyItem(int amount = 1)
    {
        var newItem = new Item();
        newItem.abilities = abilities?.ToDictionary(x => x.Key, x => x.Value);
        newItem.amount = amount;
        newItem.armor = armor;
        newItem.armorClass = armorClass;
        newItem.bagSpace = bagSpace;
        newItem.block = block;
        newItem.detailedType = detailedType;
        newItem.droppedBy = droppedBy;
        newItem.dropRange = dropRange;
        newItem.faction = faction;
        newItem.combatUse = combatUse;
        newItem.icon = icon;
        newItem.ilvl = ilvl;
        newItem.lvl = lvl;
        newItem.minPower = minPower;
        newItem.maxPower = maxPower;
        newItem.maxStack = maxStack;
        newItem.minutesLeft = minutesLeft;
        newItem.name = name;
        newItem.price = price;
        newItem.rarity = rarity;
        newItem.randomEnchantment = randomEnchantment;
        newItem.reputationRequired = reputationRequired;
        newItem.set = set;
        newItem.source = source;
        newItem.indestructible = indestructible;
        newItem.unique = unique;
        newItem.specs = specs?.ToList();
        newItem.questsStarted = questsStarted?.ToList();
        newItem.stats = stats != null ? stats.ToDictionary(x => x.Key, x => x.Value) : null;
        newItem.type = type;
        return newItem;
    }

    #endregion

    //Currently opened item
    public static Item item;
    
    //Item selected to be disenchanted
    public static Item itemToDisenchant;

    //Item chosen to be destroyed
    public static Item itemToDestroy;

    //Currently opened container item
    public static Item openedItem;

    //EXTERNAL FILE: List containing all buffs in-game
    public static List<Item> items;

    //List of all filtered items by input search
    public static List<Item> itemsSearch;
}
