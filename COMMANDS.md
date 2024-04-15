# Commands
This file contains information regarding all commands added by Guardian

## (Imperfect) Guide:
- `<text>` means `text` is a *required* field.
- `[text]` means `text` is an *optional* field.
- `one|two` means either `one` or `two` is expected.

## MasterClient
These commands can only be ran if you are the MasterClient.
- `/difficulty <level>` - Sets the difficulty to **level**.
    - Arguments:
        1. `<level>` - type: `text`, expects: `training|normal|hard|abnormal`
- `/gamemode [mode]` - Changes the current game-mode, or displays a list of available ones.
    - Arguments:
        1. `[mode]` - type: `text`, expects: `game-mode name`
    - Aliases: `gm`, `mode`
- `/guestbegone` - Automagically kicks all GUESTs from the room.
    - Aliases: `gbg`
- `/kill <id> [reason]` - Attempts to kill the player **id** with the **reason** in the feed.
    - Arguments:
        1. `<id>` - type: `number`, expects: `ID of any player in room`
        2. `[reason]`, type: `text`
- `/scattertitans` - Scatters all non-player titans to random positions on the map.
    - Aliases: `scatter`
- `/setmap <map>` - Sets the current map to **map** (*ONLY WORKS WITH OTHER GUARDIAN USERS*)
    - Arguments:
        1. `<map>` - type: `text`, expects: `AoTTG map name`
    - Aliases: `map`
- `/settitans <type>` - Sets all non-player titans to **type**.
    - Arguments:
        1. `<type>` - type: `text`, expects: `normal|aberrant|jumper|crawler|punk`
- `/teleport <id|all|x y z> [id|x y z]` - Teleports player(s) around the map.
    - Arguments:
        1. `<id|all|x y z>` - type: `number|text|numbers`, expects: `ID of any player in room|all|3-d position`
        2. `[id|x y z]` - type: `number|numbers`, expects: `ID of any player in room|3-d position`
    - Aliases: `tp`

## General
These commands can be ran whenever, whether your the MasterClient or not.
- `/clear [target]` - Clears either the chat history, debug log history, or the chat history for **id**.
    - Arguments:
        1. `[target]` - type: `text|text|number`, expects: `global|log|ID of any player in room`
- `/emotes` - Lists available chat emotes.
- `/help [page|command]` - Display a list of commands, or help for **command**.
    - Arguments:
        1. `[page|command]` - type: `number|text`, expects: `page number|command name`
    - Aliases: `?`, `commands`
- `/ignore <id>` - Ignores (most) network packets from **id**.
    - Arguments:
        1. `<id>` - type: `number`, expects: `ID of any player in room`
- `/mute <id>` - Blocks messages from **id** in chat.
    - Arguments:
        1. `<id>` - type: `number`, expects: `ID of any player in room`
- `/ragequit` - Closes the game.
    - Aliases: `rq`, `quit`, `leave`
- `/rejoin` - Attempts to leave and re-connect to the room you are playing in.
    - Aliases: `relog`, `reconnect`
- `/reloadconfig` - Reloads Guardian's settings from the configuration file.
    - Aliases: `rlcfg`
- `/say [message]` - Sends **message** to chat, bypassing command execution.
    - Arguments:
        1. `[message]` - type: `text`
- `/screenshot [scale]` - Takes a screenshot (scaled by **scale**) and saves it to file.
    - Arguments:
        1. `[scale]` - type: `number`
    - Aliases: `ss`
- `/setguild [guild]` - Sets your guild to **guild**.
    - Arguments:
        1. `[guild]` - type: `text`
    - Aliases: `guild`
- `/setlighting <lightlevel>` - Changes the map lighting to **lightlevel**.
    - Arguments:
        1. `<lightlevel>` - type: `text`, expects: `day|dawn|night`
    - Aliases: `lighting`, `settime`, `time`
- `/setusername [name]` - Sets your username to **name**.
    - Arguments:
        1. `[name]` - type: `text`
    - Aliases: `username`, `setname`, `name`
- `/translate <langfrom> <langto> <message>` - Translates **message** from **langfrom** to **langto**
    - Arguments:
        1. `<langfrom>` - type: `text`, expects: `2-letter ISO country code`
        2. `<langto>` - type: `text`, expects `2-letter ISO country code`
        3. `<message>` - type: `text`
- `/unignore <id>` - Allow packets from **id** to be processed again.
    - Arguments:
        1. `<id>` - type: `number`, expects: `ID of any player in room`
- `/unmute <id>` - Allow messages from **id** to show up in chat again.
    - Arguments:
        1. `<id>` - type: `number`, expects: `ID of any player in room`
- `/whois <id>` - Display user information about **id**.
    - Arguments:
        1. `<id>` - type: `number`, expects: `ID of any player in room`

## Debug
These commands are for development purposes, they will most likely not provide you any benefit.
- `/horse <action>` - Toggles whether or not your horse will follow you.
    - Arguments:
        1. `<action>` - type: `text`, accepts: `follow|stay`
- `/logproperties <id>` - Logs Photon Player Properties of **id** to a file.
    - Arguments:
        1. `<id>` - type: `number`, valid: `ID of any player in room`
    - Aliases: `logpr`
- `/stopwatch <action>` - Starts or ends a temporary timer.
    - Arguments:
        1. `<action>` - type: `text`, valid: `start|end`
    - Aliases: `sw`, `timer`