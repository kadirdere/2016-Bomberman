# 2016-Bomberman

The current release is version 1.0.0.

For more information about the challenge see the [Challenge website](http://challenge.entelect.co.za/) .

In this project you will find everything you need to build and run a bot on your local machine.  This project contains the following:
1. Game Engine - The game engine is responsible for running matches between players.
2. Sample Bots - Sample bots can be used a starting point for your bot.
3. Reference Bot - The reference bot contains some AI logic that will play the game based on predefined rules.  You can use this to play against your bot for testing purposes.
4. Calibration Bots - Calibration bots are used to calibrate the game engine when running bots to determine the amount of time a player bot is allowed to make a desision before the game engine kills it.
5. Binaries - Binaries for the current version of the game.  The binaries can be used to run the game without having to compile it youself.

This project can be used to get a better understanding of the rules and to help debug your bot.

Improvements and enhancements may be made to the game engine code over time, but the rules should remain stable. Any changes made to the game engine or rules will be updated here, so check in here now and then to see the changes.

The game engine has been made available to the community for peer review and bug fixes, so if you find any bugs or have any concerns, please e-mail challenge@entelect.co.za, discuss it with us on the [Challenge forum](http://forum.entelect.co.za/) or submit a pull request on Github.

## Usage
The easiest way to start using the game engine is to download the [binary release zip](https://github.com/EntelectChallenge/2016-Bomberman/releases/download/1.0.0/2016-Bomberman-1.0.0-Windows.zip). You will also need the .NET framework if you don't have it installed already - you can get the offline installer for [.NET Framework 4.5.1 here](http://www.microsoft.com/en-za/download/details.aspx?id=40779).

Once you have installed .NET and downloaded the binary release zip file, extract it and open a new Command Prompt in the Binaries/{version}/Game Engine folder.

We have included the reference bot in the binaries version folder, so at this point you can simply run the Run.bat to see the bots play a match.

Once you have written your own bot you can you can use the command line arguments to specify the bots that should be run. You can see the available command line arguments by running `Bomberman.exe --help`:
```powershell
SpaceInvadersDuel 1.0.6.0                                                     
Copyright c Microsoft 2015                                                    
                                                                              
  -b, --bot         (Default: Empty String Array) Relative path to the folder containing
                     the bot player.  You can add multiple bots by separating each with a space.       
                                                                              
  -c, -console      (Default: 0) The amount of console players to add to the game.                      
                                                                              
  -r, --rules        (Default: False) Prints out the rules and saves them in  
                     markdown format to rules.md                              
                                                                              
  --clog        	(Default: False) Enables Console Logging.                                    
                                                                              
  --pretty   		(Default: False) Draws the game map to console for every round instead of showing logs                                    
                                                                              
  -l, --log          (Default: ) Relative path where you want the match replay
                     log files to be output (instead of the default           
                     Replays/{matchSeed}).                   
                                                                              
  -s --seed        	(Default: Random) The game seed to use for map generation.                                 
                                                                              
  --help             Display this help screen.                                
```

So for example you can do something like this to run your bot against the bundled reference bot: `Bomberman.exe --pretty -b "../Reference Bot" "../My Awesome Bot"`.

You might have to change the configurate file depending on your system in order to run the game.  The configuration file is in the game engine folder called `Bomberman.exe.config`.  You can modify the file to update where the game engine looks for the various runtime executables such as the java runtime to use.  All paths have to be absolute (unless the executable is in the system path).

## Bot Meta

The game engine requires that you have `bot.json` file.  This will tell the game engine how to compile and run your bot.  The file must contain the following:

```json
{
	"Author":"John Doe",
    "Email":"john.doe@example.com",
    "NickName" :"John",
    "BotType": "CSharp",
    "ProjectLocation" : "",
    "RunFile" : "Reference\\bin\\Debug\\Reference.exe"
}
```

1. Author - Your Name
2. Email - Your Email Address
3. Nickname - The nickname that will be used by visualizers
4. Bot Type - The type of bot
  * CSharp
  * Java
  * JavaScript
  * CPlusPlus
  * Python2
  * Python3
  * FSharp
5. Project Location - The root location of the project file.  For instance in C# solutions, that will point to folder containing the solution (.sln) file.  This will be used for bot compilation when you submit your bot.
6. Run File - This is the main entry point file for your bot that will be executed to play the game.
  * Java user have to ensure that the main class is specified in the manifest file
  
The game engine might in some scenarios limit the total memory allocation for some bots depending on the bot type.

The following package managers are supported by the game engine:
* Microsoft Languages - Nuget.  (Requires the nuget.exe to be present in the project location path)
* Java - Maven.  (Requires that the project contains a pom file in the project location path)
* JavaScript - NPM.  (Requires that project contains a package.json file in the project location path)
* Python - Python Package Index.  (Requires that the project contains a requirements.txt file in the project location path)


### Tests
We have written a number of automated tests to ensure that the game logic and rules have been implemented correctly - if you make any changes to the test harness you should run the tests to ensure that everything is still working correctly before submitting a pull request.

If you add a new feature you should add tests to cover it.

## Release Notes

### Version 1.0.0 - 11 April 2016
Change Log:
1.Initial release

# Dem Rules
### Map Generation

The maps in the game will be generated randomly based on seed provided to the game engine.  The game seed will be random for each match, but can be the same if matches need to be re-run.  The map will be divided into four quadrants for generation purposes.

1. The map will be surrounded with indestructible walls
2. The default map size for 2-4 players will be 21x21 blocks
3. Every second square, starting from the outer boundary, will be an indestructible wall.  The only exception to this rule will be the centre block on the map.
4. Each quadrant will be generated such that the entire map will be symmetrical, with each quadrant appearing the same from each users perspective.
5. Players will always be placed in a corner of the map.  In case the map contains more than 4 players, the remaining players be placed equidistant from the other players along the sides of the map.
6. Every player on the map will have a 2 block safe zone horizontally and vertically.
7. The centre of the map will always contain a Super power up in the centre, in place of the indestructible wall.
8. The centre power up will always be surrounded by a 5x5 area of destructible walls.
9. Power ups will be placed randomly across the map, with each quadrant of the map receiving the same amount and type of power ups.  When four players are present, a fairness algorithm will be applied to ensure players have the same chance of finding a power up within a certain distance from them.
10. Power ups on the map will be determined with the following algorithm
  1. Two bomb bag power ups will be placed on the map per player.
  2. Four bomb radius power ups will be placed on the map per player.
11. The tournament will only have 2 or 4 players per map.  But in some scenarios more players will be placed on the maps, in which case the map size (Width/Height) will dynamically change to accommodate more players.

### Player Rules

Players can either be consol players or bots.  Both follow the same game engine rules.  When playing on Unity, the rules will follow the actual game as close as possible, with exceptions made for real time play.

1. Players will only be able submit one command per round.  The game engine will reject any additional commands sent by the player.
2. Only one of the following commands can be submitted by the player during a round:
  1. Move Command – Left, Right, Up, Down.
  2. Place Bomb Command – Places a bomb underneath the player.
  3. Reduce Bomb Timer – Reduces the timer of the bomb with the lowest timer for the player to 1.
  4. Do Nothing Command – Player skips the round and remains on the same block.
3. Players will start with a bomb bag containing 1 bomb.
4. Players will start with a bomb radius of 1.
5. Players will start with a bomb timer of 4 rounds.
6. Bot players will have the following additional rules
  1. Bot processes will be terminated after 4 seconds
  2. Bots will not be allowed to exceed a total processor time of 2 seconds
  3. Bots processes will run with elevated processor priority. (For this reason the game has to be run with administrator privileges)
  4. Calibrations will be done at the start of a game to determine additional processor time.  So if the calibration bot takes 200ms to read the files and make a move descision then your bot will be allowed an additional 200ms to complete.
7. Malfunctioning bots or bots that exceed their time limit will send back a do nothing command.
8. Bot players that post more than 20 do nothing commands in a row will automatically place a bomb to kill themselves in an attempt to save the game

### Game Engine Rules

The following rules describe how the game engine will run and process the game

1. The game engine contains the following entities:
  1. Indestructible Wall
  2. Destructible Wall
  3. Player
  4. Bomb
  5. Power Ups
    1. Bomb Bag
    2. Bomb Radius
    3. Super Power Up
2. A game block can only have one of the following entities at a single time:
  1. Indestructible Wall
  2. Destructible Wall
  3. Player
  4. Bomb
  5. Bomb with a player on top after planting
3. Power ups will only be revealed once the destructible wall has been destroyed as a result of a bomb blast.
4. The game engine will process rounds in the following order:
  1. Remove old explosions from the map
  2. Decrease all bomb timers
  3. Detonate bombs with a timer value of 0
  4. Trigger bombs that fall within the explosion range of another bomb
  5. Mark entities for destruction (Any players within a bomb blast at this moment will be killed)
  6. Process player commands
  7. Mark entities for destruction (If a player moved into a bomb blast, they will be killed)
  8. Apply power ups
  9. Remove marked (Killed/Destroyed) entities from the map
  10. Apply player movement bonus
5. A player entity will not able to move to a space containing another entity, with the exception of power ups.
6. A player can only plant a bomb if they have bombs available in their bomb bag.  Planting a bomb removes a bomb from the bomb bag and will be returned once a bomb explodes.
7. Two player entities will not be able to move onto the same space during a round, if this does happen the game engine will randomly choose a player whose move will be discarded.
8. Bombs will start with a timer based on the players current bomb bag. The formula is (bombag size * 3) + 1.  The bomb timers will be capped to 10.
9. Bomb timers will decrease by 1 every round.
10. Bomb radius will equal the radius bonus of the player at the time of planting. Obtaining a radius power up afterwards will not increase bomb radius of bombs currently on the map.
11. Destructible Walls can only be destroyed if they fall within the blast radius of a bomb.
12. Indestructible Walls will absorb the damage from a bomb and prevent it from continuing past the wall.
13. Bombs will absorb the damage from other bombs and prevent it from continuing past the bomb, this will however will cause the affected bomb to detonate causing a chain of detonations.
14. If a player is in the range of a bomb blast radius at the start of the round and is killed as result, their commands for that round will be ignored.
15. If a player moves into the range of a bomb blast during a round, the player will be killed as a result.
16. The game engine will be restricted to a certain amount of rounds.  The max rounds for each map will be calculated as follows (map width * map height).
17. The leader board for the game will be based on the following
  1. Players alive
  2. Then points for the players
  3. Then the round the players were killed

### Power Ups

Power ups can be collected by players to improve their players abilities

1. The bomb bag power up will give the player an additional bomb to plant on the map while the timers on other bombs are decreasing.
2. The bomb radius power up will multiply the current bomb radius of the player by two.
3. The special power up will give the following bonuses:
  1. Bomb bag power up
  2. Bomb radius power up
  3. 50 points

### Points

Players will collect points during game play.  Points will be used (along with other conditions) to determine the player leaderboard and ultimately the winner

1. Players will receive 10 points for destroying destructible walls.
  1. If two bombs hit the same wall, both players will receive 10 points for destroying the wall.  Unless the wall was destroyed as result of a chain explosion.
2. Players will receive points for killing another player based on the following equation (100 + (Max point per map for destructible walls / players on map)).  So on map with 10 destructible walls with 4 players the points for killing a player will be 50.
  1. If two bombs hit the another player, both players will receive points for killing the player.  Unless the player was killed as result of chain explosion.
3. Players will receive points based on map coverage:
  1. Points will only be calculated for each new block touched by a player.
  2. Points will determine player coverage on the map, with a map coverage of 100% giving the player 100 points.
4. Players obtaining the Super Power up will receive additional points.
5. When multiple player bombs are triggered in a bomb chain, all players with bombs forming part of the chain will recieve the points for all entities destroyed in the chain.
6. The round in which a player is killed will cause the player to forfeit all points earned in that round, and the player will lose points equal to the points earned when killing another player.