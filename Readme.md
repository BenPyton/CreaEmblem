# Final Exam at Créajeux

### Subject
Make a "Fire Emblem : Heroes"-like in 4 days. \
Gameplay video : https://youtu.be/ZukYebuKBbE

### Game Description :
- Tactical casual in kind of the mobile game « Fire Emblem : Heroes »;
- Turn based PC Game;
- 2 players in hotseat (players use the same mouse to play);
- No AI, no network (didn't have the time to do that)

### Gameplay :
- Tactical game : players move heroes on a grid map, each hero can do only one action during the turn ("move" or "move and attack");
- Each hero can be afoot (walk only on ground), ride a horse(twice the distance) or ride a pegasus or a dragon (less distance than the horse but but fly over rivers);
- Each hero has a wepon type (sword, spear, axe or bow) : the three first have a range of one tile and have a superiority cycle to increase or decrease damages by 20%  (sword > axe > spear > sword) and the bow has a range of two tile;
- Heroes have stats :
	- Attack (atk), the amount of damages (physical or magical);
	- Defense (def), the amount of physycal damage blocked;
	- Resistance (res), the amount of magical damage blocked;
	- Speed (spd), allow a hero to attack twice if its speed is 5 more than the opponent speed;
- A battle take place as follow: the attacker do the first attack, then if the opponent is alive he attack in return, finally one of them can do a second attack (with the speed stat);
- After each battle, the heroes gain 50 experience points for each of their attackattack, horeas stats increase with their level (this level is persistant throught all game) ;

### Game loop :
- When clicked on "Play" button in main menu, players arrive on the team selection screen : they can choose up to 4 heroes (a hero can't be chosen twice);
- After that, players are asked to select the level on which they want to play by clicking on it then click onj the "Play" button;
- While in the game they play one after each other (a turn ends when a player clicks on "End Turn" button or all heroes of the player did an action);
- The game end when all heroes of a player are dead or when a player clicks on the "Abandon" button in the pause menu;

### Controls :
All the game is played with the mouse by clicking on the buttons on screen.

### Technical informations :
The game was made with Unity 2018.1.14f1 and use asset bundles to manage all the data (levels, heroes, mounts, weapons, etc.), so it's possible to add content without rebuilding the game. \
Sounds and musics are played with FMOD, the FMOD Studio project is not provided here. \
A lot of musics, sounds and graphisms come from Fire Emblem games, others can come from the Internet and some are made by me.