# MultiplayerMobileStrategy
 
Multiplayer Mobile Strategy Game Technical Explanation

Design Patterns used in the project are Singleton, State. I could’ve used dependency injection but for this project’s sake I just decided to go with the singleton pattern instead. State machine helped the lumberjack’s states to determine if they were idle waiting, moving to a target or chopping a tree.

<img width="114" alt="image" src="https://github.com/cpeakdot/MultiplayerMobileStrategy/assets/56278550/f90c6241-9912-44c3-84a0-da332cfcf543">

Networking with PUN - Main Menu - Lobby

I’ve used the Photon Unity Networking package for multiplayer system implementation as stated in the case documentation. It automatically connects to the master server and joins the lobby. The player can set their nickname, create a room or find and join by selecting a room. After starting the game, PlayerManager is being instantiated (each player given a random color and synced) for each of the players in the room. PlayerManager then instantiates the lumberjacks(TimberMan). 

<img width="203" alt="image" src="https://github.com/cpeakdot/MultiplayerMobileStrategy/assets/56278550/02f171d2-929f-49a5-81b1-4a5b391a282e">

Mechanic

Player can click on his own lumberjacks to select and deselect action. If the player clicks on the ground, all of the selected lumberjacks try to get a path to the destination from Pathfinding Manager which uses A star pathfinding algorithm. If the player clicks on a tree, all lumberjacks first try to get close to the tree and when they get close enough, start to chop the tree with small animation.

Obstacle Avoidance

<img width="303" alt="image" src="https://github.com/cpeakdot/MultiplayerMobileStrategy/assets/56278550/6c776e94-f7ac-464c-8f49-77bdae89ddb3">

After constructing the pathfinding class, the pathfinding manager fires boxcasts for each of the grids (from top of the grid to the grid). If boxcasting returns true with “unwalkable” layer parameter, it sets the grid to unwalkable (This feature is not designed to be changed on runtime, it only checks on the start of the game). The screen shot above shows which grids are unwalkable (red line) and which are walkable (green line).
