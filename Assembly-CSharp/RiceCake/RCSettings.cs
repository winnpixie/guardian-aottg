public static class RCSettings
{
    public static int BombMode;
    public static int TeamMode;
    public static int PointMode;
    public static int DisableRock;
    public static int ExplodeMode;
    public static int HealthMode;
    public static int HealthLower;
    public static int HealthUpper;
    public static int InfectionMode;
    public static int BanEren;
    public static int MoreTitans;
    public static int DamageMode;
    public static int SizeMode;
    public static float SizeLower;
    public static float SizeUpper;
    public static int SpawnMode;
    public static float NormalRate;
    public static float AberrantRate;
    public static float JumperRate;
    public static float CrawlerRate;
    public static float PunkRate;
    public static int GameType;
    public static int TitanCap;
    public static int HorseMode;
    public static int WaveModeOn;
    public static int WaveModeNum;
    public static int FriendlyMode;
    public static int PvPMode;
    public static int MaxWave;
    public static int EndlessMode;
    public static string Motd;
    public static int AhssReload;
    public static int PunkWaves;
    public static int GlobalDisableMinimap;
    public static int DeadlyCannons;
    public static int AsoPreserveKDR;
    public static int RacingStatic;

    public static int GetMaxWave()
    {
        return MaxWave == 0 ? 20 : MaxWave;
    }
}
