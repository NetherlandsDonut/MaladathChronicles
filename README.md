# Maladath Chronicles
  <p><b>Maladath Chronicles</b> is a World of Warcraft puzzle fangame that reuses mmo's textures and sounds.<br>Game is written from scratch on Unity 2D in C#.
  
## Changelog

<details>
  
<summary>Version history for <b>v0.0</b></summary>

```
v0.0.1
    04.07 Added basic board							
v0.0.2
    05.07 Animation of falling elements on the board
          Sound of element impact
v0.0.3
    06.07 Elements for the board
          New shadows for windows
          Rewrite of tooltips
v0.0.4
    07.07 Improved expanding sprite database for UI
          Magnet anchoring for windows
          Class icons
          Added few racial portraits
v0.0.5
    09.07 Races
          Entity stats
          Kalimdor textures
          Horde and alliance icons
v0.0.6
    10.07 Rewrite of hotkeys
          Screen view boundary
          First sites on the map
v0.0.7
    11.07 Game settings
          Save game template
          Title screen map scroll animation
          First tries at animating enemy move
v0.0.8
    12.07 Eastern Kingdoms textures
          Combat init system
          Towns, instances and areas
          First items
          Basic equipment functions
v0.0.9
    13.07 Shatter effect on collecting elements
v0.0.10
    14.07 Separate sounds for element collecting
          Sounds for bonus moves
          Screenlock system
          Flying collected elements amass on the screen
v0.0.11
    15.07 Resource amounts displayed
          Improved shatter animation
          Refreshing floating elements off the screen
v0.0.12
    16.07 Ability basic system
v0.0.13
    18.07 Talent screen
v0.0.14
    19.07 Improved sound system
          Talent screen header
          First try at talent tooltips
          Spellbook base
v0.0.15
    20.07 Buff system
          Scaling combat interface with action bars
    21.07 Tooltips for buffs
v0.0.16
    22.07 Buffs can now be forced not to stack
v0.0.17
    24.07 Finished deep AI
v0.0.18
    25.07 Rewritten site system
          Added 340 sites to the map
v0.0.19
    27.07 Added enemy cursor
          Fixed buff calculation in future
          Added ability cooldowns
v0.0.20
    28.07 Fixed board cascading on the first move
v0.0.21
    01.08 Calculating weapon damage and secondary stats
          Ending combat when someone dies
          Solid release of AI
v0.0.22
    02.08 Custom backgrounds for every area
          Entity level coloring
          Board enhanced border
          Changed board size from 7x7 to 6x6
v0.0.23
    03.08 Basic inventory
          Stats scaled with level
v0.0.24
    04.08 Made text fit into regions
          Item abilities
          Camera transition effect
v0.0.25
    05.08 Instance screen
          Site progress while exploring
          Grayscale effect
          Price in item tooltips
v0.0.26
    06.08 Added complexes
          New UI sounds
v0.0.27
    10.08 Spells base effectiveness on stats now
          Fixed portraits not facing the same direction
v0.0.28
    11.08 Fixed importing crafted items
          Spirit now gives starting resources
          15000 imported items
          2600 new icons
v0.0.29
    12.08 Spellbook
          Adjustable action bars
          Class item restrictions
          Item class restrictions
v0.0.30
    13.08 Working inventory with equipping items
v0.0.31
    15.08 Area screen
          Lot of new backgrounds for areas
v0.0.32
    16.08 A lot of new encounters
v0.0.33
    17.08 Complexes now can contain areas
          Big encounter exppansion
v0.0.34
    18.08 Town screen added
v0.0.35
    19.08 Transport measures in towns
          Loading screen
v0.0.36
    20.08 Item sounds
          Smooth board animations
v0.0.37
    21.08 Item sets
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
    22.08 Modular abilities system
v0.1.1
          Moved all content to external files
    23.08 Unified event calling system
          Wordwrap for ability, buff and talent descriptions
          Random chance for events to happen
v0.1.2
    24.08 Added 87 new portraits
          New events and triggers
v0.1.3
    25.08 Data backup system
          Dev panel base
          Generalised buff and ability tooltips
          Added ambience system
          Fixed region group extenders
v0.1.4
    26.08 Automatic pagination
          Imported around 600 icons
          Split ability icons from item icons
          Added over 100 new portraits
          Searching for objects in dev panel
v0.1.5
    27.08 Respawning windows now retain pagination
          Racial backgrounds on logging screen
          Realm system
          Renamed bosses into elites
v0.1.6
    28.08 Deleting characters
          Improved realm list
v0.1.7
    29.08 Remade equipment screen
          Players can now equip off hand weapons
          Added padding for text lines
v0.1.8
    30.08 Missile animations
          Vendor base
          Fixed problems with Shatter speed and degree
v0.1.9
    31.08 Dev panel expansion
          Many new portraits
v0.1.10
    01.09 Missile trail effects
```

</details>

<details>

<summary>Version history for <b>v0.2</b></summary>

```
v0.2.0
    02.09 Map grid system
          New camera movement
          Panning camera to site before entering
          Sites now have coordinates
          Music in title screen
          Cursor lag fix
          Changed the way buttons behave
v0.2.1
    03.09 Ability cost managing in dev panel
          Cooldowns for abilities in dev panel
          Loading screens dependant on camera location
          Starting sites for different races
          Skybox in logging screen
v0.2.2
    04.09 Fixed experience being awarded to players even at max level
          Added many new portraits
v0.2.3
    05.09 Added some lacking sites
          Over 120 new encounters
          Class trainer NPC's and portal trainers
v0.2.4
    06.09 Banking system
          Tested mobile build
v0.2.5
    07.09 Reintroduced buff flare graphical effect
          Camera proximity system
          Fixed screen not locking properly
          Spirit healer base
          Stopped saving combat data into external files
          Added battlemasters to towns and one arena master
v0.2.6
    08.09 Spirit healers added onto the map
          Gendered portraits
          Ghost music while dead
          Textures for realm of the dead
v0.2.7
    09.09 Release spirit transitioning
          Fixed bug that made it impossible to save the game while dead
v0.2.8
    10.09 Reintroduced enemy cursor
          Changed Jintha'Alor into a complex
          Added around 120 new encounters
          AI will now focus less on collecting elements it already has
v0.2.9
    11.09 Initialisation methods for all objects
          Added around 100 new encounters
```

</details>

<details>

<summary>Version history for <b>v0.3</b></summary>

```
v0.3.0
    12.09 Aligned shatter effect on portraits properly
          Moved project to Unity 2023
v0.3.1
    13.09 Reload map command
          30 new portraits
v0.3.2
    15.09 Right clicks
          Removed window rebuilding
v0.3.3
    16.09 Fixed errors related to traveling from towns
          Removed unnecessary rebuilds of windows
          Simplied memory use on grid system
          Fixed errors regarding right clicking
v0.3.4
    18.09 Flight masters
v0.3.5
    20.09 Moved flight masters to left side of town screen
    21.09 Massive portrait update
          Default texture importer
          Fixed complexes not loading up
          Fixed color errors with highlightables after mouse exit
          Fixed unnecessary assignments to background for races
          Fixed lack of automatic faction creation from races
v0.3.6
    23.09 Automatic pagination during search
          Fixed input fields not having restricted input
v0.3.7
    29.09 Asigned basic drop information for items
          Added Defias Brotherhood portraits
          Login screen background for characters now depend on the site of logging out
v0.3.8
    06.10 Setting item stats in object manager
          Fixed bug with input lines not aligning vertically
          Changed class trainer icons and added battlemaster ones
v0.3.9
    09.10 Added experience bar
          Inventory now shows current money
v0.3.10
    11.10 Uldaman improvements
          Loot system
          Combat result screen
v0.3.11
    12.10 Option for effect to last 0 frames
          Fixed unnessecary serialising of flight paths in towns
          Fixed object manager to have long windows again
v0.3.12
    13.10 Destroying items
          Selling items
          Base for vendors
          Enemies now drop junk items
          Middle mouse button events
          Tooltip sound now only after succesful build
v0.3.13
    14.10 Item comparison in tooltip
          Random stat enchants for items
v0.3.14
    15.10 Razorfen Kraul and Downs improved
          Many new portraits
```

</details>

<details>

<summary>Version history for <b>v0.4</b></summary>

```
v0.4.0
    18.10 New talent system
v0.4.1
    19.10 Made shatter effect have bigger particles
          Green talent arrows
v0.4.2
    21.09 Molten Core and Blackwing Lair improvements
          Bonus move, damage, heal and buff floating text system
          Fixed particles colliding with UI
          Flash on portraits on received damage or healing
v0.4.3
    22.09 General zone music
          Some lacking portraits added
v0.4.4
    25.09 Added over 20 lacking sites
          Base for site pathing
v0.4.5
    26.09 Multi-point site pathing

</details>
