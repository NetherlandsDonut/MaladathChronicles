# Maladath Chronicles
  <p><b>Maladath Chronicles</b> is a World of Warcraft puzzle fangame that reuses mmo's textures and sounds.<br>Game is written from scratch on Unity 2D in C#.

![Alt Text](https://github.com/NetherlandsDonut/MaladathChronicles/blob/main/Media/Image1.png)
![Alt Text](https://github.com/NetherlandsDonut/MaladathChronicles/blob/main/Media/Image2.png)
![Alt Text](https://github.com/NetherlandsDonut/MaladathChronicles/blob/main/Media/Image3.png)
![Alt Text](https://github.com/NetherlandsDonut/MaladathChronicles/blob/main/Media/Image4.png)
  
## Changelog

<details>
  
<summary>Version history for <b>v0.0</b></summary>

```
v0.0.1
 04.07.23 Added basic board							
v0.0.2
 05.07.23 Animation of falling elements on the board
          Sound of element impact
v0.0.3
 06.07.23 Elements for the board
          New shadows for windows
          Rewrite of tooltips
v0.0.4
 07.07.23 Improved expanding sprite database for UI
          Magnet anchoring for windows
          Class icons
          Added few racial portraits
v0.0.5
 09.07.23 Races
          Entity stats
          Kalimdor textures
          Horde and alliance icons
v0.0.6
 10.07.23 Rewrite of hotkeys
          Screen view boundary
          First sites on the map
v0.0.7
 11.07.23 Game settings
          Save game template
          Title screen map scroll animation
          First tries at animating enemy move
v0.0.8
 12.07.23 Eastern Kingdoms textures
          Combat init system
          Towns, instances and areas
          First items
          Basic equipment functions
v0.0.9
 13.07.23 Shatter effect on collecting elements
v0.0.10
 14.07.23 Separate sounds for element collecting
          Sounds for bonus moves
          Screenlock system
          Flying collected elements amass on the screen
v0.0.11
 15.07.23 Resource amounts displayed
          Improved shatter animation
          Refreshing floating elements off the screen
v0.0.12
 16.07.23 Ability basic system
v0.0.13
 18.07.23 Talent screen
v0.0.14
 19.07.23 Improved sound system
          Talent screen header
          First try at talent tooltips
          Spellbook base
v0.0.15
 20.07.23 Buff system
          Scaling combat interface with action bars
 21.07.23 Tooltips for buffs
v0.0.16
 22.07.23 Buffs can now be forced not to stack
v0.0.17
 24.07.23 Finished deep AI
v0.0.18
 25.07.23 Rewritten site system
          Added 340 sites to the map
v0.0.19
 27.07.23 Added enemy cursor
          Fixed buff calculation in future
          Added ability cooldowns
v0.0.20
 28.07.23 Fixed board cascading on the first move
v0.0.21
 01.08.23 Calculating weapon damage and secondary stats
          Ending combat when someone dies
          Solid release of AI
v0.0.22
 02.08.23 Custom backgrounds for every area
          Entity level coloring
          Board enhanced border
          Changed board size from 7x7 to 6x6
v0.0.23
 03.08.23 Basic inventory
          Stats scaled with level
v0.0.24
 04.08.23 Made text fit into regions
          Item abilities
          Camera transition effect
v0.0.25
 05.08.23 Instance screen
          Site progress while exploring
          Grayscale effect
          Price in item tooltips
v0.0.26
 06.08.23 Added complexes
          New UI sounds
v0.0.27
 10.08.23 Spells base effectiveness on stats now
          Fixed portraits not facing the same direction
v0.0.28
 11.08.23 Fixed importing crafted items
          Spirit now gives starting resources
          15000 imported items
          2600 new icons
v0.0.29
 12.08.23 Spellbook
          Adjustable action bars
          Class item restrictions
          Item class restrictions
v0.0.30
 13.08.23 Working inventory with equipping items
v0.0.31
 15.08.23 Area screen
          Lot of new backgrounds for areas
v0.0.32
 16.08.23 A lot of new encounters
v0.0.33
 17.08.23 Complexes now can contain areas
          Big encounter exppansion
v0.0.34
 18.08.23 Town screen added
v0.0.35
 19.08.23 Transport measures in towns
          Loading screen
v0.0.36
 20.08.23 Item sounds
          Smooth board animations
v0.0.37
 21.08.23 Item sets
          Inventory sorting and settings
          Fixed shields
          Class starting items
          Subtypes for areas
```

</details>
    
<details>

<summary>Version history for <b>v0.1</b></summary>

```
v0.1.0
 22.08.23 Modular abilities system
v0.1.1
          Moved all content to external files
 23.08.23 Unified event calling system
          Wordwrap for ability, buff and talent descriptions
          Random chance for events to happen
v0.1.2
 24.08.23 Added 87 new portraits
          New events and triggers
v0.1.3
 25.08.23 Data backup system
          Dev panel base
          Generalised buff and ability tooltips
          Added ambience system
          Fixed region group extenders
v0.1.4
 26.08.23 Automatic pagination
          Imported around 600 icons
          Split ability icons from item icons
          Added over 100 new portraits
          Searching for objects in dev panel
v0.1.5
 27.08.23 Respawning windows now retain pagination
          Racial backgrounds on logging screen
          Realm system
          Renamed bosses into elites
v0.1.6
 28.08.23 Deleting characters
          Improved realm list
v0.1.7
 29.08.23 Remade equipment screen
          Players can now equip off hand weapons
          Added padding for text lines
v0.1.8
 30.08.23 Missile animations
          Vendor base
          Fixed problems with Shatter speed and degree
v0.1.9
 31.08.23 Dev panel expansion
          Many new portraits
v0.1.10
 01.09.23 Missile trail effects
```

</details>

<details>

<summary>Version history for <b>v0.2</b></summary>

```
v0.2.0
 02.09.23 Map grid system
          New camera movement
          Panning camera to site before entering
          Sites now have coordinates
          Music in title screen
          Cursor lag fix
          Changed the way buttons behave
v0.2.1
 03.09.23 Ability cost managing in dev panel
          Cooldowns for abilities in dev panel
          Loading screens dependant on camera location
          Starting sites for different races
          Skybox in logging screen
v0.2.2
 04.09.23 Fixed experience being awarded to players even at max level
          Added many new portraits
v0.2.3
 05.09.23 Added some lacking sites
          Over 120 new encounters
          Class trainer NPC's and portal trainers
v0.2.4
 06.09.23 Banking system
          Tested mobile build
v0.2.5
 07.09.23 Reintroduced buff flare graphical effect
          Camera proximity system
          Fixed screen not locking properly
          Spirit healer base
          Stopped saving combat data into external files
          Added battlemasters to towns and one arena master
v0.2.6
 08.09.23 Spirit healers added onto the map
          Gendered portraits
          Ghost music while dead
          Textures for realm of the dead
v0.2.7
 09.09.23 Release spirit transitioning
          Fixed bug that made it impossible to save the game while dead
v0.2.8
 10.09.23 Reintroduced enemy cursor
          Changed Jintha'Alor into a complex
          Added around 120 new encounters
          AI will now focus less on collecting elements it already has
v0.2.9
 11.09.23 Initialisation methods for all objects
          Added around 100 new encounters
```

</details>

<details>

<summary>Version history for <b>v0.3</b></summary>

```
v0.3.0
 12.09.23 Aligned shatter effect on portraits properly
          Moved project to Unity 2023
v0.3.1
 13.09.23 Reload map command
          30 new portraits
v0.3.2
 15.09.23 Right clicks
          Removed window rebuilding
v0.3.3
 16.09.23 Fixed errors related to traveling from towns
          Removed unnecessary rebuilds of windows
          Simplied memory use on grid system
          Fixed errors regarding right clicking
v0.3.4
 18.09.23 Flight masters
v0.3.5
 20.09.23 Moved flight masters to left side of town screen
 21.09.23 Massive portrait update
          Default texture importer
          Fixed complexes not loading up
          Fixed color errors with highlightables after mouse exit
          Fixed unnecessary assignments to background for races
          Fixed lack of automatic faction creation from races
v0.3.6
 23.09.23 Automatic pagination during search
          Fixed input fields not having restricted input
v0.3.7
 29.09.23 Asigned basic drop information for items
          Added Defias Brotherhood portraits
          Login screen background for characters now depend on the site of logging out
v0.3.8
 06.10.23 Setting item stats in object manager
          Fixed bug with input lines not aligning vertically
          Changed class trainer icons and added battlemaster ones
v0.3.9
 09.10.23 Added experience bar
          Inventory now shows current money
v0.3.10
 11.10.23 Uldaman improvements
          Loot system
          Combat result screen
v0.3.11
 12.10.23 Option for effect to last 0 frames
          Fixed unnessecary serialising of flight paths in towns
          Fixed object manager to have long windows again
v0.3.12
 13.10.23 Destroying items
          Selling items
          Base for vendors
          Enemies now drop junk items
          Middle mouse button events
          Tooltip sound now only after succesful build
v0.3.13
 14.10.23 Item comparison in tooltip
          Random stat enchants for items
v0.3.14
 15.10.23 Razorfen Kraul and Downs improved
          Many new portraits
```

</details>

<details>

<summary>Version history for <b>v0.4</b></summary>

```
v0.4.0
 18.10.23 New talent system
v0.4.1
 19.10.23 Made shatter effect have bigger particles
          Green talent arrows
v0.4.2
 21.10.23 Molten Core and Blackwing Lair improvements
          Bonus move, damage, heal and buff floating text system
          Fixed particles colliding with UI
          Flash on portraits on received damage or healing
v0.4.3
 22.10.23 General zone music
          Some lacking portraits added
v0.4.4
 25.10.23 Added over 20 lacking sites
          Base for site pathing
v0.4.5
 26.10.23 Multi-point site pathing
v0.4.6
 29.10.23 Discovering sites
v0.4.7
 31.10.23 Pathfinding for traveling on the adventure map
v0.4.8
 01.11.23 Added over 20 new sites
          Connected most of Eastern Kingdoms with paths
v0.4.9
 02.11.23 Transportation system unification
v0.4.10
 03.11.23 Integrated talent screen with toolbar
          Unspent talent points indicator
v0.4.11
 07.11.23 Site progression visuals
v0.4.12
 10.11.23 Boss queue in areas
          Defines added to external file
v0.4.13
 17.11.23 New spellbook layout
          Bag system
          Blocked progression till elite is killed
v0.4.14
 29.03.24 Hearthstone at start
          Fixed shadows for windows with header group only
v0.4.15
 30.03.24 Hugely improved pathfinding
v0.4.16
 30.03.24 Added stable masters to the game
          Made buff tooltip the same size as the tooltip of abilites
          Added fade in's for sites on map when freshly explored
v0.4.17
 01.04.24 Fixed pathing information getting duplicated on asset reloads
          Fixed orchish starting position
          Added delay for shatter effect
v0.4.18
 02.04.24 Fixed a lot of black areas on the map
          Fixed sounds with toolbar
          Made unuseful npc do not show in towns
          Fixed window respawning in inventory screens
          Fixed spellbook not updating properly on sorting
          One new site in Un'Goro Crater
          Made banks work again
v0.4.19
 03.04.24 Added three new sites to Barrens
          Added reloading windows that were present on previous desktop for updating
          New experience scaling
          New experience bar design
v0.4.20
 04.04.24 Added five new portraits
          Added four new sites to Desolace
          Shaman backgrounds for enhancement and elemental
v0.4.21
 07.04.24 Fixed music in Ahn'Qiraj
          Fixed Jaedenar so now it appears on the map
          Added one site to Stonetalon and one to Ashenvale
          Removed achievments from the game
v0.4.22
 08.04.24 Added unlocking new parts of instances step by step
          Fixed boss button being visible after closing the area it's in
          Fixed shadows breaking on deleting characters
          Fixed a rare error with generatin permanent enchants for items
          Fixed talent screen reopen sound on switching specs
          Added indicator for fully explored areas in instances
v0.4.23
 09.04.24 Added ChangeElements effect
          Added Flame Shock, Freezing Nova and Deep Freeze
          Made enemies skip player abilities in calculations
v0.4.24
 10.04.24 Added Combat Log
          Fixed stacked transition effect
          Fixed text clipping in tooltips, again
          Added more ways of sorting abilities in spellbook
          Changed AIDepth from 5 to 7
v0.4.25
 11.04.24 Fixed transportation to work again
          Added shatter params to ChangeElements effect
```

</details>

<details>

<summary>Version history for <b>v0.5</b></summary>

```
v0.5.0
 12.04.24 Added sounds for ship, zeppelin and tram travel
          Added source information to items
          Added button to close the loot window
          Made cursor thicker
          Fixed journal opening from toolbar playing sound twice
          Fixed journal icon not changing on opening journal
          Fixed stacked shadow from console window
          Fixed Ghamoo-ra not having any drop
 13.04.24 Added dynamic footsteps sound during travel
          Added stacking items
v0.5.1
 14.04.24 Added mounts
          Started adding nighttime visuals
          Splitting item stacks
          Time passing with movement
v0.5.2
 16.04.24 Beta stable master interface
          Added more nighttime visuals
          Made map change color during nighttime
v0.5.3
 17.04.24 Added health bars
          Made the nighttime texture color appear smoothly
          Logging screen now handles nighttime visuals
          Four new sites in Eastern Kingdoms
          More nighttime visuals
          Two new sites in Dustwallow Marsh
v0.5.4
 18.04.24 Added price to mounts
          Fixed a bug that player could equip trade goods
          Class restrictions to items are back
          Finished Eastern Kingdoms nighttime visuals
          Reduced build size by 40%
v0.5.5
 19.04.24 Added cooldowns for passive abilities
v0.5.6
 20.04.24 Added Holy Bolts and Holy Fire
          Fixed that cooldown left was showing outside of combat
          High resolution Scholomance textures + filled with enemies
v0.5.7
 22.04.24 Added cloth types information to game
          Moved satyrs from humanoids to demons
v0.5.8
 23.04.24 Added recipes to the game
          Imported all recipes from the game
v0.5.9
 25.04.24 Added Quel'Thalas texture to the map
          Crafting menu is possible to open now but it's empty
v0.5.10
 26.04.24 Fixed death of hardcore characters
v0.5.11
 27.04.24 Fixed starting items having amount equal to 0
          Changed all prices from float to int to avoid floating point errors
          Added vendors with restocking supplies
          Polished town, bank and vendor UI
          Profession trainers now have different levels
          Player can now split items to vendors
v0.5.12
 28.04.24 Added visual effect for restocking items
          Made items available for buyback slowly decay
v0.5.13
 29.04.24 Added calculating score for characters
v0.5.14
 30.04.24 Added ranking system
          Introduced PVP ranks
v0.5.15
 02.05.24 Player can now learn new professions
          Player can now learn new recipes from trainers
v0.5.16
 04.05.24 Added prompt for making an inn a home
          Added character sheet screen
          Set starting hour to 7am
          Fixed bank not closing on pressing ESC
          Added recipe tooltips
          Current mount window added
          Fixed silver coins being showed even when there's zero of them
v0.5.17
 05.05.24 Added base for world abilities
v0.5.18
 06.05.24 Fixed size of the item destroy menu
          Beta world event system
          Finished work on hearthstone
          Fixed ability tooltip in combat not being aligned correctly
v0.5.19
 07.05.24 Marked sites with fishing capabilities
v0.5.20
 09.05.24 Ambience now varies depending on time of day
          Base for chest opening
v0.5.21
 12.05.24 Renamed cloth types into general drops
          Finished work on chests
v0.5.22
 14.05.24 Fixed quilboars being asigned beast category
          Fixed camera locking out randomly because of error with loading sites
          Aligned all sorting and settings windows to better fit the screen
          Added sorting items by rarity
          Added scrolls and potions to random drop
v0.5.23
 15.05.24 Game state is now saved whenever a character is created or deleted
          New name for characters don't repeat
          Changed sites on map to circles from squares
          Removed grid from the map
v0.5.24
 18.05.24 Removed possibility of disabling shadows by user
          Added option for slower falling of the elements on the screen
          Removed cascade await
v0.5.24
 19.05.24 Added bigger elements to the board
v0.5.25
 20.05.24 Moved project to Unity 6
          Made all of the UI rounded
          Hidden single rank talent dots
          Fixed errors with prefabs in Unity 6
          Polishing of talent screen
          Redid visuals for talent dots and talent dots
v0.5.26
 21.05.24 Improved health bars
          Finished rounded UI
v0.5.27
 22.05.24 Brang back flight masters
          Fixed text not fitting into boxes
          Rounded board corners
          Varied recipe types in the profession UI
v0.5.28
 23.05.24 Added buying mounts
          Added a fixed order for NPC's in towns
          Restricted access to towns based on reputation
          Added starting faction standings
          Fixed order of flight paths
          Remade site tooltip
          Added all profession trainers
          Fixed some zone icons
v0.5.29
 24.05.24 Added auctioneers, without function yet
          Added starting recipes for professions that are newly learned
          Added crafting items by players
          Profession skill raises now when crafting items
          Changed icon for enchanting recipes
          Moved level barriers required for riding to defines
v0.5.30
 25.05.24 Added enchanting
          Split blueprint file into two separate to improve performance
          Remade pagination system
          Polished the crafting screen
v0.5.31
 26.05.24 Two new sites for Hilsbrad
          Added unplayable goblin race
v0.5.32
 28.05.24 Redid character creation screen
          Custom names now possible for new characters
          Added center alignment to input lines and descriptions
          Rounded camera borders
v0.5.33
 29.05.24 Huge improvements to performance in combat
          Decreased the amount of windows
          Made shatter effects decay smoothly
          Added separate layers for shadows
          Fixed headers not having static pagination
v0.5.34
 30.05.24 Added reputation standing colors
v0.5.35
 01.06.24 Base work of quests
          Quest markers
          Making progress in quests
v0.5.36
 02.06.24 Imported all quests from WoW
v0.5.37
 03.06.24 Quest markers for instances and complexes
          Quest info in site tooltip
          Quest log settings
v0.5.38
 04.06.24 Smoothly updating Fluid Bars
          Quest tooltips now don't show parts of the quests that can't be done at the site
          Added zone icons for quests
v0.5.39
 06.06.24 Changed the combat UI
          Added new cooldown animation
          Fixed talents not updating on changes
          Removed collisions from buttons without interactions
v0.5.40
 07.06.24 Abilities with no cooldown no longer receive cooldown set to zero
 08.06.24 Added quest marker over sites that offer new quests
          Eight new sites for Eastern Kingdoms
          Increased size of sites on map from 19x19 to 20x20
          Added possibility of neutral non-faction towns
          Many fixes to UI
v0.5.41
 09.06.24 Added a lot of new portraits
          Removed instance wing headers
          Unique drop mechanism for items
          Indestructible mechanism for items
v0.5.42
 10.06.24 Added accepting and abandoning quests
          Added a random boss to Ring of Law
          Filled Stratholme
v0.5.43
 11.06.24 Added all nighttime visuals to the game
          Added icon for teleport in Darnassus
          Made player location and exclamation mark not overlap
          Rearranged sprite folder
          Made indestructible items not decay in buyback
v0.5.44
 12.06.24 Added unique backgrounds for each screen
          Stopped respawning neighboring sites when traveling for no reason
          Added a lot of rare enemies and portraits for them
v0.5.45
 13.06.24 Empty bag slots are no longer clickable
          Rares no longer show up in area tooltip
          Improved quest tooltips in sites
          Finished adding all rare encounters
          Added item rewards to quests
          One new site for Westfall and one for Ashenvale
v0.5.46
 14.06.24 Added glow indicating that a quest can be turned in
          One new site for Dun Morogh
          Fixed crash on hovering over anything while traveling
          Fixed alignment of icons in site tooltips
          Shatter dots from elements collected on the board move to entity portrait
          Adjusted layer of flying buffs
          Missiles have a more funky trajectory now
v0.5.47
 15.06.24 Added turning in quests
          New complex icon
          Updated spec backgrounds a little
          Disabled map padding when not in map
          Made chest fit into instances and complexes
          Removed limitation for max area amount in instances
          Added level range displayed in complex tooltips
          Added gray colored bars for quests and recipes
v0.5.48
 16.06.24 Added gathering herbs and mining ore
v0.5.49
 17.06.24 Added level ranges for herbs
          Fixed two bags not having tags
          Added sparkle sound when shatter effect touches portrait
          Remade title screen moving effect
          Combat results no longer close after taking all the loot
          Excluded Teldrassil from pathfinding while in other zones to avoid game freezing
          Fixed starting camera position when loading up a save
```
</details>

<details>

<summary>Version history for <b>v0.6</b></summary>

```
v0.6.0
 18.06.24 Added disenchanting items
          Loaded up all sound effects into memory on start
v0.6.1
 19.06.24 Added screen backgrounds for instance wings
          Separated areas in instances with wings
v0.6.2
 20.06.24 Fixed disappearing paths from the game
          Added all backgrounds for instance wings
          Added player money to towns in the bottom right corner
          Tweaked tooltips for recipes and profession levels
          Polished the profession screens
          Item comparison now includes armor, block and damage
          Changed how damage is displayed on weapons
          Made it possible to equip a lower armor class items
          Expanded item sounds system
v0.6.3
 21.06.24 Fixed learning recipes from items
          Fixed infinite money bug using buyback screen
v0.6.4
 22.06.24 Added a lot of NPC's
          Fixed recipes displaying how many bag slots they have
v0.6.5
 23.06.24 Added friendly NPC sounds
          Added skinning prompt and special skinning loot
          Made it so that only one voice line can play at a time
          Added stock to all innkeepers
          Added two missing innkeepers to towns
v0.6.6
 24.06.24 Added enemy voice lines
          Added keybinds for disabling music and sounds
v0.6.7
 25.06.24 Added hotkeys for toogling music and sound effects
          Added most of the enemy voice lines
          Added quest tracker for items
          Quest log now tracks the amount of items player currently has
          Added a setting to stop loot windows from closing automatically
          Added picking quest rewards
v0.6.8
 27.06.24 Added local quest tracker inside of an area
          Added popup text indicating progress on killing enemies
          Added a lot of vendors including ranged weapon vendors
          Fixed items that have " in their names
v0.6.9
 28.06.24 Added experience rewards to quests
          Added money rewards to quests tho not yet displayed
          Expanded quest description
v0.6.10
 29.06.24 Stopped displaying stat changes equal to zero
          Added guarranteed green drop to rares and elites
          Added accepting quests from items
          Fixed bug causing enemies to sometimes drop two items instead of one
v0.6.11
 02.07.24 Adjusted spirit healer positions
          Redid algorithm for finding nearest spirit healer
          Redrawed rarity indicators
v0.6.12
 03.07.24 Added side panels to fishing
          Added fishing spots with difficulty
v0.6.13
 05.07.24 Added teleportation effect for items
          Quest text is now formatted and paginated
          Added working world buffs
          Added passive gains for buffs
          Fixed that shown but not explored sites do not show quest markers
          Removed world events and unified them into normal events
v0.6.14
 06.07.24 Trinkets are now usable in combat
          Separated combat use items from the rest
v0.6.15
 07.07.24 Fixed world buff tooltip
          World buffs are now visible in adventure map
          Adjusted word wrap
v0.6.16
 08.07.24 Enemy again visually clicks on abilities when casting
          Fixed quick use items updating after use
v0.6.17
 09.07.24 Added scroll buffs
          Added all standard food buffs
v0.6.18
 10.07.24 Fixed world buffs appearing on loading screen
v0.6.19
 11.07.24 Quests now change reputation and respawn sites on changed ranks
          Added combat start effect for items
v0.6.20
 12.07.24 Added turn in map pointer to quest log
          Quests no longer glitch out when they lack description
          Quests now can have alternate previous quest requirement
v0.6.21
 13.07.24 Fixed ranged weapons not being able to be equipped
          Made chest disappear when opening quest panels
v0.6.22
 19.07.24 Finalised the text wrapping method
v0.6.23
 21.07.24 Added respecing at class trainers
          Added sound effect volume to effects
v0.6.24
 22.07 Profession levels are now capped for trainers
          Added Zandalar supplies
          Fixed gendered quest descriptions
          Fixed word wrap wrapping whitespaces
v0.6.25
 23.07.24 Added armor to stat display
          Added opening loot items like crates or bags
v0.6.26
 24.07.24 Added separate action bar sets and shifting between them in combat
          Disabled saving while in combat or when looting
v0.6.27
 25.07.24 Added event conditions for advanced ability making
v0.6.28
 26.07.24 Added more types of conditions
v0.6.29
 28.07.24 Added loot to sacks of gems
v0.6.30
 30.07.24 Added accounting for enchantment abilities and stat gains
          Added unlocking MC and BWL through exploration
v0.6.31
 06.08.24 Updated the sound system
          Fixes to spellbook
v0.6.32
 08.08.24 Fixed errors while testing abilities
          Made sites more circular
v0.6.33
 11.08.24 Entry work on auction houses
v0.6.34
 20.09.24 Destroyed unique items can be dropped again
          Added racial and class based restrictions for dropping items, mainly for quest items
          Game no longer saves buff on characters with full data, now it only includes the name for reference
          Small UI changes in the logging screen
          Fixed paths being drawn to sites on map before they appeared fully
v0.6.35
 22.09.24 Added basic auction house UI
v0.6.36
 25.09.24 Last row of the board is removed after all entities move
          Expanded Wailing Caverns a bit
          Further work on auction house UI
```
</details>

<details>

<summary>Version history for <b>v0.7</b></summary>

```
v0.7.0
 12.10.24 Multiple entity combat
          Flying elements no longer travel on the board if collector had full mana
v0.7.1
 14.10.24 Rewritten the AI into a much more simple and intuitive in design system
          Fixed errors related to player dying with buffs on
          Adjusted quest markers for now bigger sites on map
          Removed future calculation from the game which meant 14932 lines of code less :D
          Removed Stats class and just made it into single dictionary
v0.7.2
 15.10.24 Instances now have two enemies at the same time excluding bosses
          Fixed ability testing
          Removed shatter type completely
          Areas now show a fishing icon if fishing is possible
          Fixed AI "clicking" their abilities
          Fixed how the elements travel towards combatant portraits
 15.10.24 Asset cleanup
 17.10.24 Remade the login screen
v0.7.3
 19.10.24 Fixed errors related to receiving 0 experience after combat
          Reagents player lacks for crafting are now displayed in red
          Fixed top bar glitching out while looting
 21.10.24 Added possibility of quests being available inside of instance for specific areas
          Added a new site just for Rotgrip in Maraudon
          Little bit of code cleanup
v0.7.4
 28.03.25 Fixed floating combat text to be in the middle of the board
          Switching chart pages now doesn't transition the screen
 30.03.25 Modified pagination and line drawing systems
          Changed character screen to allow item tooltips and made stats more readable
          One new satyr portrait
          Fixed world buffs not working
          Cursor now cannot leave the are of the window
          Added an option in settings to run the app in background or not
          Proper Classic Void Terror portrait
          Items can be positioned in inventory now manually
          Fixed accepting and completing quests not refreshing the screen
          You can't unequip two weapons anymore when swapping for a two handed weapon with no free space in inventory
v0.7.5
 31.03.25 Player can now manually move items in the bank
          Fixed lack of sound on deleting a character
          Fixed moving item being moved still while outside of equipment screen
          Removed unused SoD recipes
          Added lacking Invoker's Mantle and Invoker's Cord items
          More restrictions on tooltips and clicks giving less space to break the game
v0.7.6
 01.04.25 Fixed buffs not working in combat anymore after multiple enemy system
          Changed login screen a bit
          Fixed gems not having sounds
          Fixed lack of escape keybind to quit moving items in loot windows
          Removed the ability to simply right click a buff to remove it
          Fixed local quest tracker not refreshing on quest accept and list of available quests not being reappearing after closing the quest tracker
v0.7.7
 03.04.25 Spell targetting
          Redesigned entity bars in combat
          Missiles, shatter effects and traveling particles now properly aim for portraits
          Buffs immediately move into position if spotlight is changed
          Friendly combatants always show action bars
          Fixed an error that sometimes caused falling/floating text to not be aligned properly
          Fixed player being able to infinitely level up for 0 exp, crashing the game, after reaching lvl 60
          Fixed game crashing out when saving to menu while in a desktop different than map
 04.04.25 Rank variables available for conditions
          Specific messages for specifically failed conditions
          Trying to cast an ability and failing to because of conditions now doesn't cost resources and doesn't refresh cooldown
v0.7.8
 05.04.25 Added 8 new sites to Eastern Kingdoms
          Added missing elites related to the big game hunt
v0.7.9
 07.04.25 Added an option for a quest not to eat the items needed
          Changed capital cities from just towns into towns with districts
          Fixed lacking ability for Scroll of Intellect I and fixed world buffs not replacing each other
          Added informative description to scrolls that they count as separate scrolls
          Added missing Horde flight master to Gadgetzan and Grom'gol, and few npc's to Undercity and Thunder Bluff
v0.7.10
 11.04.25 Added different cursor colors for friendly combatants and enemies
          Added an indicator who's turn it is now in combat
v0.7.11
 12.04.25 Made transports a separate system independent of flight paths and land paths
          Transport routes now cost in-game time
          Added prices to flight paths
          Fixed player being able to teleport to any site that isn't reachable by land movement
          Fixed death map and how the sites appear on it
v0.7.12
 14.04.25 Removed instances and complexes having quests available at entrance, made it so quests can be only inside areas or towns
          All site types now show if quests can be aquired, handed in or can be progressed in there
          Improved description system by being able to fit more than just a function in a single text
v0.7.13
 15.04.25 Teleporing/hearthing to a site that is inside a capital, a complex or an instance now doesn't clip player out of bounds
          Added physical and magic resistance
          Clicking on portraits in combat now also switches focus together with health bars and name bars
v0.7.14
 18.04.25 Made available elite align itself with the progress bar
          Fixed locations not unlocking in instances when defeating the elite
v0.7.15
 19.04.25 Fixed not being able to progress in sites with randomly chosen boss from a pool
          Adjusted word wrap
v0.7.16
 20.04.25 Simplified drawing process of the menu bar
          Made wings appear differently from normal areas inside instances
          Added stat and masteries explanation. Added colors for damage types
v0.7.17
 21.04.25 Added Wrath spell
          Fixed resource bars not drawing properly with trinkets and different action bar sets
          Fixed Valley of Trials not having background
          Fixed buff tooltips not closing on mouse exit
 22.04.25 Changed damage on weapons into Power modifier which is used to multiply all types of damage and healing
v0.7.18
 23.04.25 Created talent trees for Rogue and Hunter
          Small changes to Druid trees
v0.7.19
 24.04.25 Transportation is now available for areas
          Added a ship to Feathermoon Stronghold
          If a non human player moves first during combat the cursor now properly appears
          Added a lot of icons for rogue abilities
          Player can now unlearn talents on the spot
v0.7.20
 25.04.25 Fixed that encountering a rare enemy meant fighting two copies of it
          Recommended level for areas is now calculated separately for different sides
          Hotkeys can now be set not to close tooltips
          Added a bunch of lacking portraits
          Added a possibility of a common encounter being locked to a specific side
          Added many new sites to Barrens, Stonetalon Mountains, Ashenvale, Feralas, Darkshore and one lacking to Stromgarde Ruins.
v0.7.21
 26.04.25 Added beast taming
```
</details>
