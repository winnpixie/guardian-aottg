 [![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/A0A577AMK)
# Guardian
A free, open-source, and feature-rich modification for the Attack on Titan Tribute Game.

## Where do I get/download Guardian Mod?
Download "[Launcher.exe](https://aottg.winnpixie.xyz/clients/guardian/Launcher.exe)" and run it!
    - If Windows Smart Defender stops the launcher from running, click "More Info" and then click "Run Anyway", this is an issue I can/will not fix as I do not sign the executable.

## Features
This mod has (almost) everything RiceCake's mod (RC mod) has, and much more!

- Improved protection against abusive users!
    - Mod detection could be categorized under this, but I see it as more of a for-fun thing.
- A touched up in-game user-interface.
- In-game Voice Chat!
    - `V` to talk, baskslash (`\`) to open the configuration menu.
- Custom textures/sounds!
    - Help on these are available in the `README.TXT` file found when you download and run Guardian's launcher, or when you extract the ZIP file.
- A *boat-load* of new settings (ESCAPE key to open the configuration menu):
    - **Master Client**
        - AnnounceRoundTime - Announces how many seconds a full round took to finish.
        - AnnounceWaveTime - Announces how many seconds a survival wave took to finish.
        - EndlessTitans - Lets the game never have to restart by spawning new titans when one dies.
        - InfiniteRoom - Stops your room from expiring, thus allowing you to host forever.
        - OGPunkHair - Lets you play like its 2015 again when they still had brightly colored hair.
        - DeadlyHooks - Spice up gameplay by making hooks more 'realistic' by KILLING players instead of grappling to them.
        - HideNames - Turns off player nametags for everyone in the room with a compatible mod.
        - BombsKillTitans - Expands upon normal Bomb-PVP to work with titans.
        - UseSkyBarrier - Enables a "limit" of how high players can fly upwards during a Bomb-PVP match. (Insta-death box for your own collision)
    - **Assets**
        - ThunderSpearSkin - ThunderSpear left/right skin url.
        - LeftRopeSkin - Left ODMG rope/wire skin url.
        - LeftRopeTileScale - Left ODMG rope/wire skin tile scale.
        - RightRopeSkin - Right ODMG rope/wire skin url.
        - RightRopeTileScale - Right ODMG rope/wire skin tile scale.
    - **Player**
        - RawTPS-WOWInput - Enables the use of 'Raw' mouse input from Unity for TPS and WOW.
        - DoubleTapBurst - Change whether or not double-tapping a movement key will perform a gas burst.
        - AHSSIdle - Makes blade users look about 10x cooler when standing.
        - CrossBurst - Changes your burst particles from the original poof of gas to two intersecting red beams.
        - HideHookArrows - Hides those pesky double-hook indicators that fly across your screen violently as you pass by objects.
        - HoldForBladeTrails - Allows your blade trail to be seen when you're readying an attack, rather than only once you swing.
        - ReelOutScrollSmoothing - Makes scroll-wheel reel-out a viable option, it's literally magic and I don't understand how RiceCake did it.
        - OpacityOfOwnName - Change the name-tag opacity of yourself.
        - OpacityOfOtherNames - Change the name-tag opacity of others.
        - DirectionalFlares - Fire flares towards your cursor position instead of straight up.
            - I stole this one right out of Expedition Mod, #SorryNotSorry.
        - SuicideMessage - Display a custom message in the kill-feed when you press your Suicide/Reset key.
        - LavaDeathMessage - Display a custom message in the kill-feed when you die to Lava.
        - LocalMinDamage - Enforce an always-on requirement for yourself when killing titans.
            - Local Minimum *WILL NOT* override the current room's setting if it is lower.
    - **Chat**
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
    - **Visual**
        - *Render Settings*
            - Lerp - Enables visually smoother movement of the player (Rigidbody *Linear* Interpolation).
            - DrawDistance - See more or less of the map at once.
            - FieldOfView - Change field of view. (***DISABLED**/NEEDS RE-IMPLEMENTATION*)
            - Blur - Toggle camera blurring effects/depth of field.
            - UseMainLightColor - Toggle between custom lighting or map-set lighting.
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
    - **Miscellaneous**
        - DiscordPresence - Toggle Rich Presence to show as your status in [Discord](https://discord.com/) (requires a game/app restart).
        - PhotonAppId - Use a custom [Photon](https://photonengine.com/) Application Id for multiplayer servers.
        - PhotonUserId - Set your Photonian friend-name for user discovery services.
    - **Debug**
        - ShowFramerate - Toggles displaying the game's framerate in the debug menu.
        - ShowCoordinates - Toggles displaying the player's X/Y/Z in the debug menu.
        - MaxLogEntries - Change the max amount of log entries saved in history.
        - ShowLog - Toggle visibility of the log completely.
        - DrawBackground - Toggle visibility of the log window background.
- New Gamemodes
    - **Time-Bomb** - Kill titans to gain time and stay alive!
    - **Last Man Standing** - The last player with the most kills after each wave wins!
    - **Cage Fight** - Two players, two titans, each titan kill causes the opposing side to have another titan that they need to kill. Whoever dies first loses.
        - A ground-up re-creation of Feng's Cage Fight gamemode that had been long gone.
- New Map Options
    - These maps require Guardian to host/join, excluding Anarchy-Custom.
    - **The City II** (The City with 10s Respawn)
    - **The City IV** (The Forest II, but in The City)
    - **The City V** (The Forest III, but in The City)
    - **Multi-Map** (Allows you to change the map while in a room)
    - **Anarchy-Custom Map** (Custom Maps with support for Anarchy's racing objects/scripts)
- Improved/Fixed Functions
    - "Quickmatch" button now acts as a way to enter "Offline Mode".
- Notable and/or more Technical Changes
    - Ability to switch between a UDP or TCP connection to [Photon](https://photonengine.com/)

## In-game Commands
To view the list of available commands and how to use them, type `/help` in game!

## Special Thanks
- [Fenglee](http://fenglee.com/), without you, we wouldn't have the [base game](http://fenglee.com/game/aog/) that we all know and love.
- [RiceCake](https://github.com/rc174945), this mod probably wouldn't even be a thing if [RC](https://aotrc.weebly.com/) didn't exist either.
- [Order](https://github.com/aelariane) ([Anarchy](https://github.com/aelariane/Anarchy)-related features)
- [Elite Future/Kevin](https://github.com/kkim6109) ([Voice Chat for PUN](https://github.com/kkim6109/Mic-Integration-Old-Photon-))
- Fleur/Syal, Esli, Bahaa, Edz, and \[too\] many annoying players for helping me test my anti-abuse code.
- [Sadico](https://github.com/Mi-Sad), I've probably asked you for something at some point.
- [Mr. Lurkin](https://github.com/MrLurkin), [Zippy](https://github.com/ZippyStew45), [Alice](https://github.com/ExiMichi), and many more for ideas of what to add.
- Akiroshy, Milk, Lilim, and a multitude of users for giving me motivation to keep the project going.