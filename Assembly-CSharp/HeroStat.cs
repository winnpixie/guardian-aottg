using System.Collections.Generic;

public class HeroStat
{
    public string name;
    public int SPD;
    public int GAS;
    public int BLA;
    public int ACL;
    public string skillId = "petra";
    private static bool Init;
    public static Dictionary<string, HeroStat> stats;

    public static HeroStat getInfo(string name)
    {
        InitData();
        return stats[name];
    }

    private static void InitData()
    {
        if (!Init)
        {
            Init = true;

            // Mikasa
            HeroStat mikasa = new HeroStat();
            mikasa.name = "MIKASA";
            mikasa.skillId = "mikasa";
            mikasa.SPD = 125;
            mikasa.GAS = 75;
            mikasa.BLA = 75;
            mikasa.ACL = 135;

            // Levi
            HeroStat levi = new HeroStat();
            levi.name = "LEVI";
            levi.skillId = "levi";
            levi.SPD = 95;
            levi.GAS = 100;
            levi.BLA = 100;
            levi.ACL = 150;

            // Armin
            HeroStat armin = new HeroStat();
            armin.name = "ARMIN";
            armin.skillId = "armin";
            armin.SPD = 75;
            armin.GAS = 150;
            armin.BLA = 125;
            armin.ACL = 85;

            // Marco
            HeroStat marco = new HeroStat();
            marco.name = "MARCO";
            marco.skillId = "marco";
            marco.SPD = 110;
            marco.GAS = 100;
            marco.BLA = 115;
            marco.ACL = 95;

            // Jean
            HeroStat jean = new HeroStat();
            jean.name = "JEAN";
            jean.skillId = "jean";
            jean.SPD = 100;
            jean.GAS = 150;
            jean.BLA = 80;
            jean.ACL = 100;

            // Eren
            HeroStat eren = new HeroStat();
            eren.name = "EREN";
            eren.skillId = "eren";
            eren.SPD = 100;
            eren.GAS = 90;
            eren.BLA = 90;
            eren.ACL = 100;

            // Petra
            HeroStat petra = new HeroStat();
            petra.name = "PETRA";
            petra.skillId = "petra";
            petra.SPD = 80;
            petra.GAS = 110;
            petra.BLA = 100;
            petra.ACL = 140;

            // Sasha
            HeroStat sasha = new HeroStat();
            sasha.name = "SASHA";
            sasha.skillId = "sasha";
            sasha.SPD = 140;
            sasha.GAS = 100;
            sasha.BLA = 100;
            sasha.ACL = 115;

            // Custom default
            HeroStat customDefault = new HeroStat();
            customDefault.skillId = "petra";
            customDefault.SPD = 100;
            customDefault.GAS = 100;
            customDefault.BLA = 100;
            customDefault.ACL = 100;

            // AHSS default
            HeroStat ahssDefault = new HeroStat();
            sasha.name = "AHSS";
            ahssDefault.skillId = "sasha";
            ahssDefault.SPD = 100;
            ahssDefault.GAS = 100;
            ahssDefault.BLA = 100;
            ahssDefault.ACL = 100;

            stats = new Dictionary<string, HeroStat>();
            stats.Add("MIKASA", mikasa);
            stats.Add("LEVI", levi);
            stats.Add("ARMIN", armin);
            stats.Add("MARCO", marco);
            stats.Add("JEAN", jean);
            stats.Add("EREN", eren);
            stats.Add("PETRA", petra);
            stats.Add("SASHA", sasha);
            stats.Add("CUSTOM_DEFAULT", customDefault);
            stats.Add("AHSS", ahssDefault);
        }
    }
}
