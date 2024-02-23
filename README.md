# SteamLinkTool
This is a simple application to execute EpicGames games (lol) through steam (Using BigPicture/SteamLink)
I made this because i wanted to play some games from epic games using steamlink and bigpicture.

1- Add a new shortcut to steam

2- Select SteamLinkTool as EXE

3- Get the shortcut url from EpicGames and set it as parameter on steam app properties. example:
    com.epicgames.launcher://apps/0584d2013f0149a791e7b9bad0eec102%3A6e563a2c0f5f46e3b4e88b5f4ed50cca%3A9d2d0eb64d5c44529cece33fe2a46482?action=launch&silent=true
    
4- Parameters are like: [epic games url] [Executable game name] [Timeout in seconds:Optional]
    
5- Final Parameter
  com.epicgames.launcher://apps/0584d2013f0149a791e7b9bad0eec102%3A6e563a2c0f5f46e3b4e88b5f4ed50cca%3A9d2d0eb64d5c44529cece33fe2a46482?action=launch&silent=true GTA5

This app is doing a timeout (70 seconds default). SteamLink will launch Epic Games Launcher and execute the game..., this will take time (Timeout Secs) 
I made this app a few months ago. This is just a backup. SteamLink/EpicGames probably works good without this app.


### There is also a new feature, this could help users using RemotePlay, like, playing Need For Speed Carbon, but letting your friend to play the game through RemotePlay.

- Install DEAD (steam://install/952700) or any other game that lets you use the Steam API input or Xbox Controller, and have remoteplay activated.
- Replace DEAD.exe for SteamLinkTool executable
- Configure Config.ini and set as GamePath= value the executable game you would like to play, example C:\Games\MycoolGame.exe
- Start DEAD from steam, and invite your friend to the game


### How it works:

- Steam runs SteamlinkTool. If you run an app through SteamLink, SteamLink will activate the controller compatibility.
- SteamLinkTool open EpicGames launcher with the parameters (Game URL)
- EpicGamesLauncher launch 'FallGuys', steam hooks the controller to the game through SteamLinkTool.
- SteamLinkTool open twice but the second app is a slave that will check if the first steamlinktool is running.
- You open SteamLink menu and choose to End the game. Steam close first steamlinktool, slave steamlinktool detect their master is gone, and close the game and EpicGamesLauncher.
- You are no able to play another game.



