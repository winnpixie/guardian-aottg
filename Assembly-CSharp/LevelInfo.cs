using UnityEngine;

public class LevelInfo
{
    public string name;
    public string mapName;
    public string description;
    public int enemyNumber;
    public bool supply = true;
    public bool teamTitan;
    public GAMEMODE type;
    public RespawnMode respawnMode;
    public bool noCrawler;
    public bool hint;
    public bool lavaMode;
    public bool horse;
    public bool punk = true;
    public bool pvp;
    public static LevelInfo[] Levels;
    private static bool Initialized;
    public Minimap.Preset minimapPreset;

    public static LevelInfo GetInfo(string name)
    {
        InitData();

        foreach (LevelInfo levelInfo in Levels)
        {
            if (levelInfo.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                return levelInfo;
            }
        }
        return null;
    }

    public static void InitData()
    {
        if (!Initialized)
        {
            Initialized = true;

            Levels = new LevelInfo[]
            {
                // Singkeplayer
                new LevelInfo
                {
                    name = "[S]Tutorial",
                    mapName= "tutorial",
                    description = "Learn the basic functionality of AoTTG.",
                    enemyNumber = 1,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    hint = true,
                    punk = false
                },
                new LevelInfo
                {
                    name = "[S]Battle training",
                    mapName = "tutorial 1",
                    description = "Basic offensive training course.",
                    enemyNumber = 7,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    punk = false
                },
                new LevelInfo
                {
                    name = "[S]City",
                    mapName = "The City I",
                    description = "Kill all 15 titans invading the city!",
                    enemyNumber = 15,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true
                },
                new LevelInfo
                {
                    name = "[S]City (Waves)",
                    mapName = "The City I",
                    description = "Survive 20 waves.",
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                },
                new LevelInfo
                {
                    name = "[S]Forest",
                    mapName = "The Forest",
                    description = "Kill all 15 titans!",
                    enemyNumber =15,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true
                },
                new LevelInfo
                {
                    name = "[S]Forest Survive(no crawler)",
                    mapName = "The Forest",
                    description = "Survive all 20 waves. (No crawlers)",
                    enemyNumber = 3,
                    type= GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    noCrawler = true,
                },
                new LevelInfo
                {
                    name = "[S]Forest Survive(no crawler no punk)",
                    mapName = "The Forest",
                    description = "Survive all 20 waves. (No crawlers, no punks)",
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    noCrawler =true,
                    punk = false
                },
                new LevelInfo
                {
                    name = "[S]Racing - Akina",
                    mapName = "track - Akina",
                    description = "Test your speed!",
                    enemyNumber = 0,
                    type = GAMEMODE.RACING,
                    respawnMode = RespawnMode.NEVER,
                    supply = false,
                    minimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f)
                },
                // Multiplayer
                // City
                new LevelInfo
                {
                    name = "The City",
                    mapName = "The City I",
                    description = "Kill all 10 titans invading the city! (Player titans, PvP, no respawns)",
                    enemyNumber = 10,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    teamTitan = true,
                    pvp = true,
                    minimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 731.9738f)
                },
                new LevelInfo
                {
                    name = "The City II",
                    mapName = "The City I",
                    description = "Kill all 10 titans invading the city! (Player titans, PvP, 10s respawn)",
                    enemyNumber = 10,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.DEATHMATCH,
                    supply = true,
                    teamTitan = true,
                    pvp = true
                },
                new LevelInfo
                {
                    name = "The City III",
                    mapName = "The City I",
                    description = "Capture each checkpoint to win!",
                    enemyNumber = 0,
                    type = GAMEMODE.PVP_CAPTURE,
                    supply = true,
                    teamTitan = true,
                    minimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 734.9738f)
                },
                new LevelInfo
                {
                    name = "The City IV",
                    mapName = "The City I",
                    description = "Survive all 20 waves. (No respawns)",
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                },
                new LevelInfo
                {
                    name = "The City V",
                    mapName = "The City I",
                    description = "Survive all 20 waves. (Respawn on each new wave)",
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEWROUND,
                    supply = true,
                },
                // Forest
                new LevelInfo
                {
                    name = "The Forest",
                    mapName = "The Forest",
                    description = "The Forest of Giant Trees. (Player titans, PvP, no respawns)",
                    enemyNumber = 5,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    teamTitan = true,
                    pvp = true
                },
                new LevelInfo
                {
                    name = "The Forest II",
                    mapName = "The Forest",
                    description = "Survive all 20 waves. (No respawns)",
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                },
                new LevelInfo
                {
                    name = "The Forest III",
                    mapName = "The Forest",
                    description = "Survive all 20 waves. (Respawn on each new wave)",
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEWROUND,
                    supply = true,
                },
                new LevelInfo
                {
                    name = "The Forest IV  - LAVA",
                    mapName = "The Forest",
                    description = "The floor is LAVA!\nSurvive all 20 waves WITHOUT touching the ground. (Respawn on each new wave, no crawlers)",
                    enemyNumber = 3,
                    type = GAMEMODE.SURVIVE_MODE,
                    respawnMode = RespawnMode.NEWROUND,
                    supply = true,
                    noCrawler = true,
                    lavaMode = true
                },
                // Outside the Walls
                new LevelInfo
                {
                    name = "Outside The Walls",
                    mapName = "OutSide",
                    description ="Capture each checkpoint to win! (Player titans, 10s respawn)",
                    enemyNumber = 0,
                    type = GAMEMODE.PVP_CAPTURE,
                    respawnMode = RespawnMode.DEATHMATCH,
                    supply = true,
                    horse = true,
                    teamTitan = true,
                    minimapPreset = new Minimap.Preset(new Vector3(2549.4f, 0f, 3042.4f), 3697.16f)
                },
                // Akina
                new LevelInfo
                {
                    name = "Racing - Akina",
                    mapName = "track - Akina",
                    description = "Test your speed!",
                    enemyNumber = 0,
                    type = GAMEMODE.RACING,
                    respawnMode = RespawnMode.NEVER,
                    supply = false,
                    pvp = true,
                    minimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f)
                },
                // Boss fights
                // Annie
                new LevelInfo
                {
                    name = "Annie",
                    mapName = "The Forest",
                    description = "Nape Armor/Ankle Armor:\nNormal:1000/50\nHard:2500/100\nAbnormal:4000/200\nYou only have 1 life. Be careful soldier!",
                    enemyNumber = 15,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    punk = false,
                    pvp = true
                },
                new LevelInfo
                {
                    name = "Annie II",
                    mapName = "The Forest",
                    description = "Nape Armor/Ankle Armor:\nNormal:1000/50\nHard:2500/100\nAbnormal:4000/200\n(10s respawn)",
                    enemyNumber = 15,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.DEATHMATCH,
                    punk = false,
                    pvp = true
                },
                // Colossal
                new LevelInfo
                {
                    name = "Colossal Titan",
                    mapName = "Colossal Titan",
                    description = "Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\nNape Armor:\nNormal:2000\nHard:3500\nAbnormal:5000\nYou only have 1 life. Be careful soldier!",
                    enemyNumber = 2,
                    type = GAMEMODE.BOSS_FIGHT_CT,
                    respawnMode = RespawnMode.NEVER,
                    minimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f)
                },
                new LevelInfo
                {
                    name = "Colossal Titan II",
                    mapName = "Colossal Titan",
                    description = "Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\nNape Armor:\n Normal:5000\nHard:8000\nAbnormal:12000\n(10s respawn)",
                    enemyNumber = 2,
                    type = GAMEMODE.BOSS_FIGHT_CT,
                    respawnMode = RespawnMode.DEATHMATCH,
                    minimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f)
                },
                // Trost
                new LevelInfo
                {
                    name = "Trost",
                    mapName = "Colossal Titan",
                    description = "Escort Titan Eren to seal the hole in the wall! (No respawns)",
                    enemyNumber = 2,
                    type = GAMEMODE.TROST,
                    respawnMode = RespawnMode.NEVER,
                    punk = false
                },
                new LevelInfo
                {
                    name = "Trost II",
                    mapName = "Colossal Titan",
                    description = "Escort Titan Eren to seal the hole in the wall! (10s respawn)",
                    enemyNumber = 2,
                    type = GAMEMODE.TROST,
                    respawnMode = RespawnMode.DEATHMATCH,
                    punk = false
                },
                // PvP
                // Cage Fighting
                new LevelInfo
                {
                    name = "Cage Fighting",
                    mapName = "Cage Fighting",
                    description = "2 Players in cages, each kill spawns 1 or more titans in the opposing cage.",
                    enemyNumber = 1,
                    type = GAMEMODE.CAGE_FIGHT,
                    respawnMode = RespawnMode.NEVER
                },
                // Cave Fight
                new LevelInfo
                {
                    name = "Cave Fight",
                    mapName = "CaveFight",
                    description = "***SPOILER ALERT***",
                    enemyNumber = 0,
                    type =GAMEMODE.PVP_AHSS,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    teamTitan = true,
                    pvp = true
                },
                // House Fight
                new LevelInfo
                {
                    name = "House Fight",
                    mapName = "HouseFight",
                    description = "***SPOILER ALERT***",
                    enemyNumber = 0,
                    type =GAMEMODE.PVP_AHSS,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    teamTitan = true,
                    pvp = true
                },
                // Custom
                new LevelInfo
                {
                    name = "Custom",
                    mapName = "The Forest",
                    description = "RC Custom Maps (Player titans allowed)",
                    enemyNumber = 1,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    pvp = true,
                    punk = true,
                    supply = true,
                    teamTitan = true
                },
                new LevelInfo
                {
                    name = "Custom (No PT)",
                    mapName = "The Forest",
                    description = "RC Custom Maps (No player titans)",
                    enemyNumber = 1,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    pvp = true,
                    punk = true,
                    supply = true,
                    teamTitan = false
                },
                // Guardian
                new LevelInfo
                {
                    name = "Multi-Map",
                    mapName = "The City I",
                    description = "Play any map at any point during the game.",
                    enemyNumber = 10,
                    type = GAMEMODE.KILL_TITAN,
                    respawnMode = RespawnMode.NEVER,
                    supply = true,
                    teamTitan = true,
                    pvp = true,
                    punk = true
                }
            };
        }
    }
}
