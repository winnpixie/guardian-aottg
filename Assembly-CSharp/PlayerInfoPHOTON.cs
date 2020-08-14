using UnityEngine;

public class PlayerInfoPHOTON
{
    public string name = "Guest";
    public string guildname = string.Empty;
    public string id;
    public int kills;
    public int die;
    public int maxDamage;
    public int totalDamage;
    public int assistancePt;
    public bool dead;
    public string resourceId = "not choose";
    public bool SET;
    public int totalKills;
    public int totalDeaths;
    public int totalKillsInOneLifeNormal;
    public int totalKillsInOneLifeHard;
    public int totalKillsInOneLifeAB;
    public int airKills;
    public int totalCrawlerKills;
    public int totalJumperKills;
    public int totalNonAIKills;

    public void InitAsGuest()
    {
        name = "GUEST" + Random.Range(0, short.MaxValue * 2);
        kills = 0;
        die = 0;
        maxDamage = 0;
        totalDamage = 0;
        assistancePt = 0;
        dead = false;
        resourceId = "not choose";
        SET = false;
        totalKills = 0;
        totalDeaths = 0;
        totalKillsInOneLifeNormal = 0;
        totalKillsInOneLifeHard = 0;
        totalKillsInOneLifeAB = 0;
        airKills = 0;
        totalCrawlerKills = 0;
        totalJumperKills = 0;
        totalNonAIKills = 0;
    }
}
