# Configuration
This file contains information regarding all options added by Guardian

## Game-modes
- MaxTitanPoints - Max points titans must gain to win CTF missions.
- MaxHumanPoints - Max points humans must gain to win CTF missions.
- BombsKillTitans - Expands upon normal Bomb-PVP to work with titans.
- UseSkyBarrier - Enables a "limit" of how high players can fly upwards during a Bomb-PVP match. (Insta-death box for your own collision)

## Master Client
- AnnounceRoundTime - Announces how many seconds a full round took to finish.
- AnnounceWaveTime - Announces how many seconds a survival wave took to finish.
- EndlessTitans - Lets the game never have to restart by spawning new titans when one dies.
- InfiniteRoom - Stops your room from expiring, thus allowing you to host forever.
- OGPunkHair - Lets you play like its 2015 again when they still had brightly colored hair.
- DeadlyHooks - Spice up gameplay by making hooks more 'realistic' by KILLING players instead of grappling to them.
- FatalCollisions - Adds death by impact for those who can't control their ODMG.
- FatalSpeedDelta - The minimum speed a player must lose in one physics update to die by impact.
- HideNames - Turns off player nametags for everyone in the room with a compatible mod.
- ClearStatsOnReset - Wipes K/D/MD/TD on every match restart.

## Assets
- ThunderSpearSkin - ThunderSpear left/right skin url.
- LeftRopeSkin - Left ODMG rope/wire skin url.
- LeftRopeTileScale - Left ODMG rope/wire skin tile scale.
- RightRopeSkin - Right ODMG rope/wire skin url.
- RightRopeTileScale - Right ODMG rope/wire skin tile scale.

## Player
- UseRawInput - Enables the use of 'Raw' mouse input from Unity for TPS and WOW.
- DoubleTapBurst - Change whether or not double-tapping a movement key will perform a gas burst.
- ReelOutScrollSmoothing - Makes scroll-wheel reel-out a viable option, it's literally magic and I don't understand how RiceCake did it.
- ShowSkillTimer - Toggles the crosshair timer for when your Skill is ready to use.
- AHSSIdle - Makes blade users look about 10x cooler when standing.
- CrossBurst - Changes your burst particles from the original poof of gas to two intersecting red beams.
- HideHookArrows - Hides those pesky double-hook indicators that fly across your screen violently as you pass by objects.
- HoldForBladeTrails - Allows your blade trail to be seen when you're readying an attack, rather than only once you swing.
- OpacityOfOwnName - Change the name-tag opacity of yourself.
- OpacityOfOtherNames - Change the name-tag opacity of others.
- DirectionalFlares - Fire flares towards your cursor position instead of straight up.
    - I stole the code for this right out of Expedition Mod, #SorryNotSorry.
- SuicideMessage - Display a custom message in the kill-feed when you press your Suicide/Reset key.
- LavaDeathMessage - Display a custom message in the kill-feed when you die to Lava.
- LocalMinDamage - Enforce an always-on requirement for yourself when killing titans.
    - Local Minimum *WILL NOT* override the current room's setting if it is lower.

## Chat
- MaxMessages - Change the max amount of messages saved in history.
- Timestamps - Toggles timestamp display in history.
- DrawBackground - Toggle chat window background visibility.
- *Translator Settings*
    - *Languages must be in the **two-letter [ISO 639-1](https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes) format.***
    - TranslateIncoming - Allow incoming messages to be translated.
    - IncomingLanguage - The language all messages received are written in.
        - Use '`auto`' to tell Google to try and figure it out.
    - TranslateOutgoing - Allow outgoing messages to be translated.
    - OutgoingLanguage - The language all messages sent are to be translated to.
- JoinMessage - Send a message whenever you join a room.
- UserName - Let your messages have a different name than your in-game one.
- BoldName - Toggles bold chat name.
- ItalicName - Toggles italic chat name.
- TextColor - Color your text however you'd like.
- TextPrefix - Insert text before your messages.
- TextSuffix - Append text after your messages.
- BoldText - Toggles bold messages.
- ItalicText - Toggles italic messages.

## Visual
- *Render Settings*
    - Lerp - Enables visually smoother movement of the player (Rigidbody *Linear* Interpolation).
    - DrawDistance - See more or less of the map at once.
    - FieldOfView - Change field of view. (***DISABLED**/NEEDS RE-IMPLEMENTATION*)
    - Blur - Toggle camera blurring effects/depth of field.
    - CustomMainLightColor - Toggle between custom lighting or map-set lighting.
    - MainLightColor - Set HEX color for custom lighting.
    - Fog - Toggle fog visibility, this can make some maps like City pretty terrifying, or really bad, it's up to interpretation.
        - FogColor - Set custom fog HEX color.
        - FogDensity - Set density of map fog.
    - SoftShadows - Toggle between soft/hard shadows (EXPERIMENTAL).
- CameraTiltStrength - Change how far the camera tilt should lean when hooked to objects.
- Flare1/2/3Color - Break free from the original green, red, and black flare colors (HEX colors).
- EmissiveFlares - Toggle flares emitting light.
- ShowPlayerMods - Toggles visibility of player mods in playerlist.
- ShowPlayerPings - Toggles visibility of player pings in playerlist.
- FPSCamera - Allows you to play AoTTG from a First-Person View (EXPERIMENTAL).
- MultiplayerNapeMeat - Adds nape meat slices into multiplayer (BUGGY).

## Miscellaneous
- LimitUnfocusedFPS - Toggles an FPS limiter to save system resources when you're not tabbed into the game.
- MaxUnfocusedFPS - The maximum FPS goal for the game when you're not tabbed in (I recommend no less than **30** if you're the MasterClient)
- DiscordPresence - Toggle Rich Presence to show as your status in [Discord](https://discord.com/) (requires a game/app restart).
- PhotonAppId - Use a custom [Photon](https://photonengine.com/) Application Id for multiplayer servers.
- PhotonUserId - Set your Photonian friend-name for user discovery services.

## Debug
- ShowFramerate - Toggles displaying the game's framerate in the debug menu.
- ShowCoordinates - Toggles displaying the player's X/Y/Z in the debug menu.
- MaxLogEntries - Change the max amount of log entries saved in history.
- ShowLog - Toggle visibility of the log completely.
- DrawBackground - Toggle visibility of the log window background.