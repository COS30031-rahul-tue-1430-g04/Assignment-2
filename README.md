# Game Description
‘Who Looks After This?’ is a 2D exploration and educational game designed in response to the Melton City Council Game Challenge. The game introduces players to the idea that different assets within a community can be managed and maintained by different parties. The main objective is to explore a neighbourhood, identify important community assets, and decide whether each asset is generally managed by the Council/Public sector or by a private owner.

The game is designed for a young audience and focuses on learning through observation, exploration and interaction rather than providing large amounts of written information. Players are encouraged to look carefully at their surroundings and use the context of each asset to make a decision. This allows the game to communicate that community infrastructure is diverse and that responsibility for maintaining an asset is not always immediately obvious.

The player controls a character who explores a series of neighbourhood environments. Throughout each level, certain objects are marked with a white star, indicating that they can be investigated. When the player approaches an asset and presses E, an interaction window appears asking:

**“Who generally looks after this?”**

# Connection to Melton City Council Game Challenge
The melton city council game challenge asks for us to create a game that teaches a young audience about who manages different assets. Our game enables this by having the player explore multiple levels finding different assets and determining whether they are managed by the council or privately
|                   Key messages                       |                          How we achieved them                              |
|------------------------------------------------------|----------------------------------------------------------------------------|
| Communities contain many different types of assets.  | Through a diverse selection of different assets throughout levels.         |
| Council manages and maintains many community assets. | By having many objects being identified as publicly managed.               |
| Not every asset is Council's responsibility          | By having many objects being identified as privately managed.              |
| Context helps you make a better decision.            | Having similar objects in  different contexts having different management. |
| Learning about local infrastructure can be engaging. | By enabling immediate condensed feedback from engaging action.             |

Other connections:
Level design - our levels were designed with the melton city in mind leading to our choice of levels being Rural, suburban and city precinct.
Custom asset - we designed our custom asset based on melton city council building.

# Controls
| Input Action |   Action Type   |          Current Keys Bound            |
|--------------|-----------------|----------------------------------------|
| Move         | Value - Vector2 | Left Stick, WASD, Primary2DAxis, Stick |
| Interact     | Button          | E, ButtonNorth                         |
| Sprint       | Button          | LeftShift, LeftStick                   |

# How to play
Explore the level for objects with white stars.

![assetExample](./assetExample.png)

When you find one, press the interact key (“E”) to bring up a menu asking you to classify it as a public/council or private. After making your selection your score will either increase or decrease depending on whether you got it right or wrong. Regardless of whether you were correct, the progress bar will increase, showing you how many more objects you have to find. Once you find them all you can progress to the next level.

# How to run
1. Download the zip from itch.io: [Who Looks After This? by cheddercheeese](https://cheddercheeese.itch.io/who-looks-after-this)
2. Extract it
3. Run `build/Assignment 2.exe`

# Key programming systems
There are three key programming systems. The first is Player Control. This is where the user input (except UI interaction) is given functionality. This includes adjusting player velocity and animation while also interacting with Asset Interaction. Asset Interaction is the system where the player interacts with nearby assets and chooses whether they are public or private. It updates the Game State System. The Game State System controls how far through the level the player is and what score they have. It also transfers the player between levels when they complete them.

# Team contributions
Darcy - custom asset, star identifiers, rural level design, prototyping some of the object interaction.

Hakan - level completion screen, UI/UX, level transition manager, physics material, community precinct level design, player collision handling

Nicholas - UI/UX, Feedback Panel, InteractionPanel, HUD, GameManager, AssetInteraction, MainMenuScene, EndGameScene, PausePanel, PlayerInteraction, LevelTransitionManager, GameProfiles

Judah - player control/movement/animation, camera tracking, finding/editing/importing assets, autotiles, tilemaps, tilepallets, prefab tiles (including scripts for prefabs), suburban level design

# Known issues
- Controllers can’t be used to navigate menus.
- The player doesn’t sink into water correctly.
- It is possible to leave the boundary sometimes.
- Some objects don’t have collision boxes.
- Street lights and rocks can display under players incorrectly.
