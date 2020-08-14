using UnityEngine;

public class LevelInfo
{
    public string name;
    public string mapName;
    public string desc;
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
    private static bool Init;
    public Minimap.Preset minimapPreset;

    public static LevelInfo getInfo(string name)
    {
        initData2();
        foreach (LevelInfo levelInfo in Levels)
        {
            if (levelInfo.name == name)
            {
                return levelInfo;
            }
        }
        return null;
    }

    private static void initData2()
    {
        if (!Init)
        {
            Init = true;
            Levels = new LevelInfo[27]
            {
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo(),
                new LevelInfo()
            };

            // The City I
            Levels[0].name = "The City";
            Levels[0].mapName = "The City I";
            Levels[0].desc = "kill all the titans with your friends. (No RESPAWN/SUPPLY/PLAY AS TITAN)";
            Levels[0].enemyNumber = 10;
            Levels[0].type = GAMEMODE.KILL_TITAN;
            Levels[0].respawnMode = RespawnMode.NEVER;
            Levels[0].supply = true;
            Levels[0].teamTitan = true;
            Levels[0].pvp = true;

            // The City II
            Levels[1].name = "The City II";
            Levels[1].mapName = "The City I";
            Levels[1].desc = "Fight the titans with your friends. (RESPAWN AFTER 10 SECONDS/SUPPLY/TEAM TITAN)";
            Levels[1].enemyNumber = 10;
            Levels[1].type = GAMEMODE.KILL_TITAN;
            Levels[1].respawnMode = RespawnMode.DEATHMATCH;
            Levels[1].supply = true;
            Levels[1].teamTitan = true;
            Levels[1].pvp = true;

            // Cage Fighting (never used?)
            Levels[2].name = "Cage Fighting";
            Levels[2].mapName = "Cage Fighting";
            Levels[2].desc = "2 players in different cages. when you kill a titan, one or more titan will spawn to your opponent's cage.";
            Levels[2].enemyNumber = 1;
            Levels[2].type = GAMEMODE.CAGE_FIGHT;
            Levels[2].respawnMode = RespawnMode.NEVER;

            // The Forest I
            Levels[3].name = "The Forest";
            Levels[3].mapName = "The Forest";
            Levels[3].desc = "The Forest Of Giant Trees. (No RESPAWN/SUPPLY/PLAY AS TITAN)";
            Levels[3].enemyNumber = 5;
            Levels[3].type = GAMEMODE.KILL_TITAN;
            Levels[3].respawnMode = RespawnMode.NEVER;
            Levels[3].supply = true;
            Levels[3].teamTitan = true;
            Levels[3].pvp = true;

            // The Forest II
            Levels[4].name = "The Forest II";
            Levels[4].mapName = "The Forest";
            Levels[4].desc = "Survive for 20 waves.";
            Levels[4].enemyNumber = 3;
            Levels[4].type = GAMEMODE.SURVIVE_MODE;
            Levels[4].respawnMode = RespawnMode.NEVER;
            Levels[4].supply = true;

            // The Forest III
            Levels[5].name = "The Forest III";
            Levels[5].mapName = "The Forest";
            Levels[5].desc = "Survive for 20 waves. Players will respawn in every new wave";
            Levels[5].enemyNumber = 3;
            Levels[5].type = GAMEMODE.SURVIVE_MODE;
            Levels[5].respawnMode = RespawnMode.NEWROUND;
            Levels[5].supply = true;

            // Annie I
            Levels[6].name = "Annie";
            Levels[6].mapName = "The Forest";
            Levels[6].desc = "Nape Armor/Ankle Armor:\nNormal:1000/50\nHard:2500/100\nAbnormal:4000/200\nYou only have 1 life. Don't do this alone.";
            Levels[6].enemyNumber = 15;
            Levels[6].type = GAMEMODE.KILL_TITAN;
            Levels[6].respawnMode = RespawnMode.NEVER;
            Levels[6].punk = false;
            Levels[6].pvp = true;

            // Annie II
            Levels[7].name = "Annie II";
            Levels[7].mapName = "The Forest";
            Levels[7].desc = "Nape Armor/Ankle Armor:\nNormal:1000/50\nHard:3000/200\nAbnormal:6000/1000\n(RESPAWN AFTER 10 SECONDS)";
            Levels[7].enemyNumber = 15;
            Levels[7].type = GAMEMODE.KILL_TITAN;
            Levels[7].respawnMode = RespawnMode.DEATHMATCH;
            Levels[7].punk = false;
            Levels[7].pvp = true;

            // Colossal Titan I
            Levels[8].name = "Colossal Titan";
            Levels[8].mapName = "Colossal Titan";
            Levels[8].desc = "Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\nNape Armor:\nNormal:2000\nHard:3500\nAbnormal:5000\n";
            Levels[8].enemyNumber = 2;
            Levels[8].type = GAMEMODE.BOSS_FIGHT_CT;
            Levels[8].respawnMode = RespawnMode.NEVER;

            // Colossal Titan II
            Levels[9].name = "Colossal Titan II";
            Levels[9].mapName = "Colossal Titan";
            Levels[9].desc = "Defeat the Colossal Titan.\nPrevent the abnormal titan from running to the north gate.\nNape Armor:\n Normal:5000\nHard:8000\nAbnormal:12000\n(RESPAWN AFTER 10 SECONDS)";
            Levels[9].enemyNumber = 2;
            Levels[9].type = GAMEMODE.BOSS_FIGHT_CT;
            Levels[9].respawnMode = RespawnMode.DEATHMATCH;

            // Trost I
            Levels[10].name = "Trost";
            Levels[10].mapName = "Colossal Titan";
            Levels[10].desc = "Escort Titan Eren.";
            Levels[10].enemyNumber = 2;
            Levels[10].type = GAMEMODE.TROST;
            Levels[10].respawnMode = RespawnMode.NEVER;
            Levels[10].punk = false;

            // Trost II
            Levels[11].name = "Trost II";
            Levels[11].mapName = "Colossal Titan";
            Levels[11].desc = "Escort Titan Eren. (RESPAWN AFTER 10 SECONDS)";
            Levels[11].enemyNumber = 2;
            Levels[11].type = GAMEMODE.TROST;
            Levels[11].respawnMode = RespawnMode.DEATHMATCH;
            Levels[11].punk = false;

            // The City I (SINGLEPLAYER)
            Levels[12].name = "[S]City";
            Levels[12].mapName = "The City I";
            Levels[12].desc = "Kill all 15 Titans.";
            Levels[12].enemyNumber = 15;
            Levels[12].type = GAMEMODE.KILL_TITAN;
            Levels[12].respawnMode = RespawnMode.NEVER;
            Levels[12].supply = true;

            // The Forest I (SINGLEPLAYER)
            Levels[13].name = "[S]Forest";
            Levels[13].mapName = "The Forest";
            Levels[13].desc = string.Empty;
            Levels[13].enemyNumber = 15;
            Levels[13].type = GAMEMODE.KILL_TITAN;
            Levels[13].respawnMode = RespawnMode.NEVER;
            Levels[13].supply = true;

            // The Forest I (SINGLEPLAYER)
            Levels[14].name = "[S]Forest Survive(no crawler)";
            Levels[14].mapName = "The Forest";
            Levels[14].desc = string.Empty;
            Levels[14].enemyNumber = 3;
            Levels[14].type = GAMEMODE.SURVIVE_MODE;
            Levels[14].respawnMode = RespawnMode.NEVER;
            Levels[14].supply = true;
            Levels[14].noCrawler = true;
            Levels[14].punk = true;

            // Tutorial (SINGLEPLAYER)
            Levels[15].name = "[S]Tutorial";
            Levels[15].mapName = "tutorial";
            Levels[15].desc = string.Empty;
            Levels[15].enemyNumber = 1;
            Levels[15].type = GAMEMODE.KILL_TITAN;
            Levels[15].respawnMode = RespawnMode.NEVER;
            Levels[15].supply = true;
            Levels[15].hint = true;
            Levels[15].punk = false;

            // Battle Training (SINGLEPLAYER)
            Levels[16].name = "[S]Battle training";
            Levels[16].mapName = "tutorial 1";
            Levels[16].desc = string.Empty;
            Levels[16].enemyNumber = 7;
            Levels[16].type = GAMEMODE.KILL_TITAN;
            Levels[16].respawnMode = RespawnMode.NEVER;
            Levels[16].supply = true;
            Levels[16].punk = false;

            // The Forest IV
            Levels[17].name = "The Forest IV  - LAVA";
            Levels[17].mapName = "The Forest";
            Levels[17].desc = "Survive for 20 waves. Players will respawn in every new wave.\nNO CRAWLERS\n***YOU CAN'T TOUCH THE GROUND!***";
            Levels[17].enemyNumber = 3;
            Levels[17].type = GAMEMODE.SURVIVE_MODE;
            Levels[17].respawnMode = RespawnMode.NEWROUND;
            Levels[17].supply = true;
            Levels[17].noCrawler = true;
            Levels[17].lavaMode = true;

            // Racing (SINGLEPLAYER)
            Levels[18].name = "[S]Racing - Akina";
            Levels[18].mapName = "track - akina";
            Levels[18].desc = string.Empty;
            Levels[18].enemyNumber = 0;
            Levels[18].type = GAMEMODE.RACING;
            Levels[18].respawnMode = RespawnMode.NEVER;
            Levels[18].supply = false;

            // Racing
            Levels[19].name = "Racing - Akina";
            Levels[19].mapName = "track - akina";
            Levels[19].desc = string.Empty;
            Levels[19].enemyNumber = 0;
            Levels[19].type = GAMEMODE.RACING;
            Levels[19].respawnMode = RespawnMode.NEVER;
            Levels[19].supply = false;
            Levels[19].pvp = true;

            // Outside The Walls
            Levels[20].name = "Outside The Walls";
            Levels[20].mapName = "OutSide";
            Levels[20].desc = "Capture Checkpoint mode.";
            Levels[20].enemyNumber = 0;
            Levels[20].type = GAMEMODE.PVP_CAPTURE;
            Levels[20].respawnMode = RespawnMode.DEATHMATCH;
            Levels[20].supply = true;
            Levels[20].horse = true;
            Levels[20].teamTitan = true;

            // The City III
            Levels[21].name = "The City III";
            Levels[21].mapName = "The City I";
            Levels[21].desc = "Capture Checkpoint mode.";
            Levels[21].enemyNumber = 0;
            Levels[21].type = GAMEMODE.PVP_CAPTURE;
            Levels[21].respawnMode = RespawnMode.DEATHMATCH;
            Levels[21].supply = true;
            Levels[21].horse = false;
            Levels[21].teamTitan = true;

            // Cave Fight
            Levels[22].name = "Cave Fight";
            Levels[22].mapName = "CaveFight";
            Levels[22].desc = "***Spoiler Alarm!***";
            Levels[22].enemyNumber = -1;
            Levels[22].type = GAMEMODE.PVP_AHSS;
            Levels[22].respawnMode = RespawnMode.NEVER;
            Levels[22].supply = true;
            Levels[22].horse = false;
            Levels[22].teamTitan = true;
            Levels[22].pvp = true;

            // House Fight
            Levels[23].name = "House Fight";
            Levels[23].mapName = "HouseFight";
            Levels[23].desc = "***Spoiler Alarm!***";
            Levels[23].enemyNumber = -1;
            Levels[23].type = GAMEMODE.PVP_AHSS;
            Levels[23].respawnMode = RespawnMode.NEVER;
            Levels[23].supply = true;
            Levels[23].horse = false;
            Levels[23].teamTitan = true;
            Levels[23].pvp = true;

            // The Forest I (SINGLEPLAYER)
            Levels[24].name = "[S]Forest Survive(no crawler no punk)";
            Levels[24].mapName = "The Forest";
            Levels[24].desc = string.Empty;
            Levels[24].enemyNumber = 3;
            Levels[24].type = GAMEMODE.SURVIVE_MODE;
            Levels[24].respawnMode = RespawnMode.NEVER;
            Levels[24].supply = true;
            Levels[24].noCrawler = true;
            Levels[24].punk = false;

            // Custom (RC)
            Levels[25].name = "Custom";
            Levels[25].mapName = "The Forest";
            Levels[25].desc = "Custom Map.";
            Levels[25].enemyNumber = 1;
            Levels[25].type = GAMEMODE.KILL_TITAN;
            Levels[25].respawnMode = RespawnMode.NEVER;
            Levels[25].supply = true;
            Levels[25].teamTitan = true;
            Levels[25].pvp = true;
            Levels[25].punk = true;

            // Custom (RC)
            Levels[26].name = "Custom (No PT)";
            Levels[26].mapName = "The Forest";
            Levels[26].desc = "Custom Map (No Player Titans).";
            Levels[26].enemyNumber = 1;
            Levels[26].type = GAMEMODE.KILL_TITAN;
            Levels[26].respawnMode = RespawnMode.NEVER;
            Levels[26].pvp = true;
            Levels[26].punk = true;
            Levels[26].supply = true;
            Levels[26].teamTitan = false;

            Levels[0].minimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 731.9738f);
            Levels[8].minimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f);
            Levels[9].minimapPreset = new Minimap.Preset(new Vector3(8.8f, 0f, 65f), 765.5751f);
            Levels[18].minimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f);
            Levels[19].minimapPreset = new Minimap.Preset(new Vector3(443.2f, 0f, 1912.6f), 1929.042f);
            Levels[20].minimapPreset = new Minimap.Preset(new Vector3(2549.4f, 0f, 3042.4f), 3697.16f);
            Levels[21].minimapPreset = new Minimap.Preset(new Vector3(22.6f, 0f, 13f), 734.9738f);
        }
    }
}
