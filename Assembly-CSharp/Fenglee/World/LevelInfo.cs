using Guardian.Features.Gamemodes;
using UnityEngine;

public class LevelInfo
{
    public static LevelInfo[] Levels;
    private static bool Initialized;

    public string DisplayName;
    private string[] Aliases = new string[0];
    public string MapName;
    public string Description;
    public int EnemyCount;
    public bool HasReSupply = true;
    public bool PlayerTitans;
    public GameMode Mode;
    public RespawnMode RespawnMode;
    public bool DisableCrawlers;
    public bool ShowHints;
    public bool Lava;
    public bool SpawnHorses;
    public bool HasPunks = true;
    public bool PVP;

    public Minimap.Preset MinimapPreset; // RiceCake
    public bool IsSelectable = true; // Guardian

    public static LevelInfo GetInfo(string name)
    {
        InitData();

        foreach (LevelInfo levelInfo in Levels)
        {
            if (levelInfo.DisplayName.Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                return levelInfo;
            }
            foreach (string alias in levelInfo.Aliases)
            {
                if (alias.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                {
                    return levelInfo;
                }
            }
        }

        return Levels[3];
    }

    public static void InitData()
    {
        if (!Initialized)
        {
            Initialized = true;

            Levels = new LevelInfo[]
            {
                // Legacy
                new LevelInfo
                {
                    DisplayName = "Cage Fighting",
                    MapName = "Cage Fighting",
                    Description = "Two players locked in cages.\nWhen you kill a titan, one or more will spawn in your opponent's cage.",
                    EnemyCount = 1,
                    Mode = GameMode.CageFight,
                    RespawnMode = RespawnMode.Never,
                    IsSelectable = false
                },
                // Singleplayer
                new LevelInfo
                {
                    DisplayName = "[S]Tutorial",
                    Aliases = new string[] { "tutorial" },
                    MapName = "tutorial",
                    Description = "Learn the basic functionality of the Attack on Titan Tribute Game.",
                    EnemyCount = 1,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true,
                    ShowHints = true,
                    HasPunks = false
                },
                new LevelInfo
                {
                    DisplayName = "[S]Battle training",
                    Aliases = new string[] { "battle training", "training" },
                    MapName = "tutorial 1",
                    Description = "Basic offensive training course.",
                    EnemyCount = 7,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true,
                    HasPunks = false
                },
                new LevelInfo
                {
                    DisplayName = "[S]City",
                    MapName = "The City I",
                    Description = "Kill all 15 titans invading the city!",
                    EnemyCount = 15,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true
                },
                new LevelInfo
                {
                    DisplayName = "[S]Forest",
                    MapName = "The Forest",
                    Description = "Kill all 15 titans!",
                    EnemyCount = 15,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true
                },
                new LevelInfo
                {
                    DisplayName = "[S]Forest Survive(no crawler)",
                    MapName = "The Forest",
                    Description = "Survive all 20 waves. (No crawlers)",
                    EnemyCount = 3,
                    Mode = GameMode.Survival,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true,
                    DisableCrawlers = true,
                },
                new LevelInfo
                {
                    DisplayName = "[S]Forest Survive(no crawler no punk)",
                    MapName = "The Forest",
                    Description = "Survive all 20 waves. (No crawlers, no punks)",
                    EnemyCount = 3,
                    Mode = GameMode.Survival,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true,
                    DisableCrawlers = true,
                    HasPunks = false
                },
                new LevelInfo
                {
                    DisplayName = "[S]Racing - Akina",
                    MapName = "track - Akina",
                    Description = "Test your speed!",
                    EnemyCount = 0,
                    Mode = GameMode.Racing,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = false,
                    MinimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f)
                },
                // Multiplayer
                // City
                new LevelInfo
                {
                    DisplayName = "The City",
                    Aliases = new string[] { "city" },
                    MapName = "The City I",
                    Description = "Kill all 10 titans invading the city! (Player titans, PvP, no respawns)",
                    EnemyCount = 10,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true,
                    PlayerTitans = true,
                    PVP = true,
                    MinimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 731.9738f)
                },
                new LevelInfo
                {
                    DisplayName = "The City II",
                    Aliases = new string[] { "city 2", "city ii" },
                    MapName = "The City I",
                    Description = "Kill all 10 titans invading the city! (Player titans, PvP, 10s respawn)",
                    EnemyCount = 10,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Deathmatch,
                    HasReSupply = true,
                    PlayerTitans = true,
                    PVP = true
                },
                new LevelInfo
                {
                    DisplayName = "The City III",
                    Aliases = new string[] { "city 3", "city iii" },
                    MapName = "The City I",
                    Description = "Capture each checkpoint to win!",
                    EnemyCount = 0,
                    Mode = GameMode.PvPCapture,
                    RespawnMode = RespawnMode.Deathmatch,
                    HasReSupply = true,
                    PlayerTitans = true,
                    MinimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 734.9738f)
                },
                new LevelInfo
                {
                    DisplayName = "The City IV",
                    Aliases = new string[] { "city 4", "city iv" },
                    MapName = "The City I",
                    Description = "Survive all 20 waves. (No respawns)",
                    EnemyCount = 3,
                    Mode = GameMode.Survival,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true,
                    IsSelectable = false
                },
                new LevelInfo
                {
                    DisplayName = "The City V",
                    Aliases = new string[] { "city 5", "city v" },
                    MapName = "The City I",
                    Description = "Survive all 20 waves. (Respawn on each new wave)",
                    EnemyCount = 3,
                    Mode = GameMode.Survival,
                    RespawnMode = RespawnMode.NewRound,
                    HasReSupply = true,
                    IsSelectable = false
                },
                // Forest
                new LevelInfo
                {
                    DisplayName = "The Forest",
                    Aliases = new string[] { "forest" },
                    MapName = "The Forest",
                    Description = "The Forest of Giant Trees. (Player titans, PvP, no respawns)",
                    EnemyCount = 10,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true,
                    PlayerTitans = true,
                    PVP = true
                },
                new LevelInfo
                {
                    DisplayName = "The Forest II",
                    Aliases = new string[] { "forest 2", "forest ii" },
                    MapName = "The Forest",
                    Description = "Survive all 20 waves in The Forest of Giant Trees. (No respawns)",
                    EnemyCount = 3,
                    Mode = GameMode.Survival,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true,
                },
                new LevelInfo
                {
                    DisplayName = "The Forest III",
                    Aliases = new string[] { "forest 3", "forst iii" },
                    MapName = "The Forest",
                    Description = "Survive all 20 waves in The Forest of Giant Trees. (Respawn on each new wave)",
                    EnemyCount = 3,
                    Mode = GameMode.Survival,
                    RespawnMode = RespawnMode.NewRound,
                    HasReSupply = true,
                },
                new LevelInfo
                {
                    DisplayName = "The Forest IV  - LAVA",
                    Aliases = new string[] { "the forest iv - lava", "forest 4", "forest iv", "forest 4 lava", "forest iv lava", "forest lava" },
                    MapName = "The Forest",
                    Description = "The floor is LAVA!\nSurvive all 20 waves in The Forest of Giant Trees WITHOUT touching the ground. (Respawn on each new wave, no crawlers)",
                    EnemyCount = 3,
                    Mode = GameMode.Survival,
                    RespawnMode = RespawnMode.NewRound,
                    HasReSupply = true,
                    DisableCrawlers = true,
                    Lava = true
                },
                // Outside the Walls
                new LevelInfo
                {
                    DisplayName = "Outside The Walls",
                    Aliases = new string[] { "otw" },
                    MapName = "OutSide",
                    Description ="Capture each checkpoint to win! (Player titans, 10s respawn)",
                    EnemyCount = 0,
                    Mode = GameMode.PvPCapture,
                    RespawnMode = RespawnMode.Deathmatch,
                    HasReSupply = true,
                    SpawnHorses = true,
                    PlayerTitans = true,
                    MinimapPreset = new Minimap.Preset(new Vector3(2549.4f, 0f, 3042.4f), 3697.16f)
                },
                // Akina
                new LevelInfo
                {
                    DisplayName = "Racing - Akina",
                    Aliases = new string[] { "akina" },
                    MapName = "track - Akina",
                    Description = "Test your speed!",
                    EnemyCount = 0,
                    Mode = GameMode.Racing,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = false,
                    PVP = true,
                    MinimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f)
                },
                // Boss fights
                // Annie
                new LevelInfo
                {
                    DisplayName = "Annie",
                    MapName = "The Forest",
                    Description = "You only have 1 life. Be careful soldier!\nNape & Ankle Armor:\nNormal: 1000 / 50\nHard: 2500 / 100\nAbnormal: 4000 / 200",
                    EnemyCount = 15,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Never,
                    HasPunks = false,
                    PVP = true
                },
                new LevelInfo
                {
                    DisplayName = "Annie II",
                    Aliases = new string[] { "annie 2" },
                    MapName = "The Forest",
                    Description = "Nape & Ankle Armor:\nNormal: 1000 / 50\nHard: 2500 / 100\nAbnormal: 4000 / 200\n(10s respawn)",
                    EnemyCount = 15,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Deathmatch,
                    HasPunks = false,
                    PVP = true
                },
                // Colossal
                new LevelInfo
                {
                    DisplayName = "Colossal Titan",
                    Aliases = new string[] { "ct" },
                    MapName = "Colossal Titan",
                    Description = "You only have 1 life. Be careful soldier!\nDefeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\nNape Armor:\nNormal: 2000\nHard: 3500\nAbnormal: 5000",
                    EnemyCount = 2,
                    Mode = GameMode.Colossal,
                    RespawnMode = RespawnMode.Never,
                    MinimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f)
                },
                new LevelInfo
                {
                    DisplayName = "Colossal Titan II",
                    Aliases = new string[] { "ct 2", "ct ii" },
                    MapName = "Colossal Titan",
                    Description = "Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\nNape Armor:\n Normal: 5000\nHard: 8000\nAbnormal: 12000\n(10s respawn)",
                    EnemyCount = 2,
                    Mode = GameMode.Colossal,
                    RespawnMode = RespawnMode.Deathmatch,
                    MinimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f)
                },
                // Trost
                new LevelInfo
                {
                    DisplayName = "Trost",
                    MapName = "Colossal Titan",
                    Description = "Escort Titan Eren to seal the hole in Wall Rose! (No respawns)",
                    EnemyCount = 2,
                    Mode = GameMode.Trost,
                    RespawnMode = RespawnMode.Never,
                    HasPunks = false
                },
                new LevelInfo
                {
                    DisplayName = "Trost II",
                    Aliases = new string[] { "trost 2" },
                    MapName = "Colossal Titan",
                    Description = "Escort Titan Eren to seal the hole in Wall Rose! (10s respawn)",
                    EnemyCount = 2,
                    Mode = GameMode.Trost,
                    RespawnMode = RespawnMode.Deathmatch,
                    HasPunks = false
                },
                // PvP
                // Cave Fight
                new LevelInfo
                {
                    DisplayName = "Cave Fight",
                    MapName = "CaveFight",
                    Description = "PVP combat in the cavern underneath the Reiss Chapel.",
                    EnemyCount = 0,
                    Mode = GameMode.TeamDeathmatch,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true,
                    PlayerTitans = true,
                    PVP = true
                },
                // House Fight
                new LevelInfo
                {
                    DisplayName = "House Fight",
                    MapName = "HouseFight",
                    Description = "PVP combat.",
                    EnemyCount = 0,
                    Mode = GameMode.TeamDeathmatch,
                    RespawnMode = RespawnMode.Never,
                    HasReSupply = true,
                    PlayerTitans = true,
                    PVP = true
                },
                // Custom
                new LevelInfo
                {
                    DisplayName = "Custom",
                    MapName = "The Forest",
                    Description = "RC Custom Maps (Player titans allowed)",
                    EnemyCount = 1,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Never,
                    PVP = true,
                    HasPunks = true,
                    HasReSupply = true,
                    PlayerTitans = true
                },
                new LevelInfo
                {
                    DisplayName = "Custom (No PT)",
                    MapName = "The Forest",
                    Description = "RC Custom Maps (No Player Titans)",
                    EnemyCount = 1,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Never,
                    PVP = true,
                    HasPunks = true,
                    HasReSupply = true,
                    PlayerTitans = false
                },
                // Anarchy
                new LevelInfo
                {
                    DisplayName = "Custom-Anarchy (No PT)",
                    Aliases = new string[] { "anarchy" },
                    MapName = "The Forest",
                    Description = "Custom Maps with Anarchy Mod Scripts (No Player Titans)",
                    EnemyCount = 1,
                    Mode = GameMode.KillTitans,
                    RespawnMode = RespawnMode.Never,
                    PVP = true,
                    HasReSupply = true,
                    PlayerTitans = false
                }
            };
        }
    }
}
