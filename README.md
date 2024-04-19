# Continuous Assessment 2 - Advanced 3D Game Development

## Links
- [Github Repository](https://github.com/cfarrell02/Adv3DGD-Assignment-2)
- [Youtube Demo](https://youtu.be/wEbpspuiPB8)
- [WebGL Build](https://advanced3dgd.000webhostapp.com/WebBuild/index.html) (Please excuse UI scaling issues)

## Features

### Quest System
- Quests are loaded in at runtime from a XML file
- XML file includes information for the quest levl, name, description, and objectives and also the experience reward for completing the quest.
- Quests are displayed in the UI and will automatically be removed from the active quest list when completed.
- Information displayed in the UI includes the quest name and current objective.
- These objectives can be certain types such as TAKE, TALK or GO. 
- These objectives can be interpreted by the code and recognised as completed when the player interacts with the correct object or NPC.
- The player is rewarded with experience after each objective.
- Once all objectives are completed the quest is marked as completed and if there are no quests left in the active quest list the next level is loaded.

### Database System
- Playerprefs are used to track information about the player such as health, experience and money in between levels.
- When the player quits or completes the game the playerprefs are updated and also stored in the database.
- The database is a SQL database hosted on 000webhost. It stores the player's name, password, highscore and information about the player for save/load purposes.
- The database is accessed using PHP scripts which are called from the game using the WWW class.
- Players login or register using the login/register UI and their information is stored in the database at the start of the game.
- The player's highscore is updated in the database when they complete the game.
- The player is able to resume their game from the last level they were on when they login.
- Highscores are displayed at the end in a leaderboard style screen.

### Inventory System
- The player has an inventory which is shown at the bottom of the screen in a 'hotbar' style.
- Each item in the inventory uses a scriptable object class to store information about the item such as name, description, icon and type.
- Icons are generated using a camera render texture and a UI image. (Code taken from my final year project)
- The player can pick up items by walking over them and they are added to the inventory.
- Food items can be used to restore health by selecting them in the hotbar and clicking the left mouse button.
- The player cannot carry more than the maximum amount of items for each type.
- Items can be bought in the in game shop using money, this can be earned by selling valuable items to the shop, such as gold coins or artifacts.

### Dialogue System
- Dialogue is loaded in at runtime from a XML file.
- Each dialogue has a certain amount of responses which can be selected by the player. These responses lead to different dialogue options or can trigger a quest.
- Dialogue can be triggered by the player walking up to an NPC and pressing the 'E' key.
- Dialogue is displayed in a UI box and the player cycle through options with the 'Tab' key, and select an option with the 'Enter' key.
- The player can restart their dialogue by pressing 'E' after the dialogue has finished.

## Levels

### Level 1 - Binary Tree Maze Algorithm
- The first level is a maze generated using a binary tree algorithm.
- All areas can be accessed
- Quest items are scattered around the maze and must be collected to complete the level.
- The player will move on to the next level when all quest items are collected.

### Level 2 - Perlin Noise Terrain Generation
- The second level is a terrain generated using perlin noise.
- Similar to the first level, quest items are scattered around the terrain and must be collected to complete the level.

### Level 3 - Dynamic Difficulty in Static Level
- The third level is a static level with a dynamic difficulty system.
- The player must collect a certain amount of quest items to complete the level.
- The quest's final goal is across a bridge.
- The game spawns enemies, these enemies will increase in number and difficulty as the player progresses through the level.
- The bridge also becomes wider or narrower depending on the player's skill level.


*Each level has a shop that can be accessed at any time. Items gathered in each level are carried over to the next level.*

