# SteamLinkTool
This is a simple application to execute EpicGames games through steam (Using BigPicture/SteamLink)
I made this because i wanted play FallGuys or GTA V from my Epic Games account on my TV using SteamLink.

1- Add a new 'no-steam game' to steam library, and select the SteamLinkTool executable.

![image](https://github.com/th3y/SteamLinkTool/assets/1917871/1f18f93c-d13f-44bc-aef9-c2bae5ad1738)

2- Get the shortcut game url from EpicGames. Example:
    com.epicgames.launcher://apps/0584d2013f0149a791e7b9bad0eec102%3A6e563a2c0f5f46e3b4e88b5f4ed50cca%3A9d2d0eb64d5c44529cece33fe2a46482?action=launch&silent=true

3- Set the parameters and edit your 'no-steam' game.
    
4- Parameters from this no-steam game (LAUNCH OPTIONS): [epic games url] [Executable game name] [Timeout in seconds:Optional]

5- Final Parameter
  com.epicgames.launcher://apps/0584d2013f0149a791e7b9bad0eec102%3A6e563a2c0f5f46e3b4e88b5f4ed50cca%3A9d2d0eb64d5c44529cece33fe2a46482?action=launch&silent=true GTA5
  
![image](https://github.com/th3y/SteamLinkTool/assets/1917871/b44102b5-abee-4a70-adf2-9ee09e04f1f2)

#### This app is doing a timeout (70 seconds default). SteamLink will launch Epic Games Launcher and execute the game..., this will take time (Timeout Secs) 
#### I made this app a few months ago. This is just a backup. SteamLink/EpicGames probably works good without this app.


## RemotePlay for any game
#### This could help users using RemotePlay for playing (for example) Need For Speed Carbon, but letting your friend to play the game through RemotePlay, even using your keyboard and mouse if you want. Like Parsec but with Steam

- Install DEAD (steam://install/952700) or any other game that lets you use the Steam API input and/or Xbox Controller, and remoteplay available.
- Backup DEAD.exe, copy SteamLinkTool.exe and rename it as DEAD.exe (Steam will execute this like a game)
- Configure Config.ini and set as GamePath= value the executable game you would like to play, example C:\Games\MycoolGame.exe
- Run DEAD from Steam, wait for the game you set to start, and invite your friend.
- You can also set an epic games url, and it will run.

I know you can copy the whole game to the DEAD Folder, same as creating a bat file and convert it to exe. But they are not functional. This could be the fastest way to run a no-steam game and also activate remoteplay for this one.


## How it works:

- Steam runs SteamlinkTool (Master). If you run an app through SteamLink, SteamLink will activate the controller compatibility.
- Steam hooks the controller compatibility to the game through SteamLinkTool.
- SteamLinkTool open twice (Slave) that will check if the Master SteamLinkTool is running, this is because Steam run SteamLinkTool, but if you want to 'STOP' the game from the UI. Steam will not close the game, but the Master SLT, and Steam will be locked at 'User is playing'. Slave will check if SteamLinkTool closes, and it will close the game. So you can use Steam again.
- If you use SteamLinkTool to open a game from 'GamePath' using a steam-game with remoteplay+steaminputapi compatibility, like DEAD, the tool will works like a bridge. Steam will think you are running DEAD, but it will apply controller compatibility and also enable remoteplay to the game you want.



# This public project is intended for educational purposes only and should not be used commercially or for bad purposes.
