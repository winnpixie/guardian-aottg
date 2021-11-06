# Guardian [![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/T6T33LM92)
A free, open-source, and feature-rich modification for the Attack on Titan Tribute Game.

## Where do I get/download Guardian Mod?
Download "[Launcher.exe](https://alerithe.github.io/guardian/Launcher.exe)" and run it!
    - If Windows Smart Defender stops the launcher from running, click "More Info" and then click "Run Anyway", this is an issue I can/will not fix as I do not sign the executable.

## Features
This mod has everything RiceCake's mod (RC mod) has, and much more!

- Improved protection against abusive users!
    - Mod detection could be categorized under this, but I see it as more of a for-fun thing.
- A touched up in-game user-interface.
- In-game Voice Chat!
    - `V` to talk, baskslash (`\`) to open the configuration menu.
- Custom textures/sounds!
    - Help on these are available in the `README.TXT` file found when you download and run Guardian's launcher, or when you extract the ZIP file.
- A *boat-load* of new settings (ESCAPE key to open the configuration menu):
    - **Master Client**
        - EndlessTitans - Lets the game never have to restart by spawning new titans when one dies.
        - InfiniteRoom - Stops your room from expiring, thus allowing you to host forever.
        - OGPunkHair - Lets you play like its 2015 again when they still had brightly colored hair.
        - DeadlyHooks - Spice up gameplay by making hooks more 'realistic' by KILLING players instead of grappling to them.
        - BombsKillTitans - Expands upon normal Bomb-PVP to work with titans.
    - **Player**
        - RawTPS-WOWInput - Enables the use of 'Raw' mouse input from Unity for TPS and WOW.
        - DoubleTapBurst - Change whether or not double-tapping a movement key will perform a gas burst.
        - AHSSIdle - Makes blade users look about 10x cooler when standing.
        - CrossBurst - Changes your burst particles from the original poof of gas to two intersecting red beams.
        - HideHookArrows - Hides those pesky double-hook indicators that fly across your screen violently as you pass by objects.
        - HoldForBladeTrails - Allows your blade trail to be seen when you're readying an attack, rather than only once you swing.
        - Interpolation - Makes maneuvering around maps appear much smoother.
        - ReelOutScrollSmoothing - Makes scroll-wheel reel-out a viable option, it's literally magic and I don't understand how RiceCake did it.
        - OpacityOfOwnName/OpacityOfOtherNames - Change name-tag opacity of others and/or yourself to possibly improve visibility or PVP difficulty.
        - DirectionalFlares - Fire flares towards your cursor position instead of straight up.
            - I stole this one right out of Expedition Mod, #SorryNotSorry.
        - SuicideMessage - Display a custom message in the kill-feed when you press your Suicide/Reset key.
        - LavaDeathMessage - Display a custom message in the kill-feed when you die to Lava.
        - LocalMinDamage - Enforce an always-on requirement for yourself when killing titans.
            - Local Minimum *WILL NOT* override the current room's setting if it is lower.
    - **Chat**
        - MaxMessages - Change the max amount of messages saved in history.
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
        - TextColor - Color your text however you'd like.
        - TextPrefix/TextSuffix - Prepend and/or append text to your messages.
        - BoldName/ItalicName - Stylize your chat name to be bold and/or italic.
        - BoldText/ItalicText - Stylize your messages to be bold and/or italic.
    - **Visual**
        - *Render Settings*
            - DrawDistance - See more or less of the map at once.
            - Fog - Toggle fog visibility, this can make some maps like City pretty terrifying, or really bad, it's up to interpretation.
            - SoftShadows - Toggle between soft/hard shadows (EXPERIMENTAL).
        - Flare1Color/Flare2Color/Flare3Color - Break free from the original green, red, and black flare colors.
        - FPSCamera - Allows you to play AoTTG from a First-Person View (EXPERIMENTAL).
        - MultiplayerNapeMeat - Adds nape meat slices into multiplayer (BUGGY).
    - **Miscellaneous**
        - AppId - Use a custom [Photon](https://photonengine.com/) Application Id for multiplayer servers
        - DiscordPresence - Toggle Rich Presence to show as your status in [Discord](https://discord.com/) (requires a game/app restart).
    - **Logging**
        - MaxEntries - Change the max amount of log entries saved in history.
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
- RiceCake, this mod probably wouldn't even be a thing if [RC](https://aotrc.weebly.com/) didn't exist either.
- [Order](https://github.com/aelariane/) ([Anarchy](https://github.com/aelariane/Anarchy)-related features)
- [Elite Future/Kevin](https://github.com/kkim6109/) ([Voice Chat for PUN](https://github.com/kkim6109/Mic-Integration-Old-Photon-))
- Fleur/Syal, Esli, Bahaa, Edz, and \[too\] many annoying players for helping me test my anti-abuse code.
- [Sadico](https://github.com/Mi-Sad/), I've probably asked you for something at some point.
- [Mr. Lurkin](https://github.com/MrLurkin/), [Zippy](https://github.com/ZippyStew45), [Alice](https://github.com/ExiMichi/), and many more for ideas of what to add.
- Akiroshy, Milk, Lilim, and a multitude of users for giving me motivation to keep the project going.