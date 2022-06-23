using System.Collections.Generic;

public class HeroStat
{
    private static bool Init;
    public static Dictionary<string, HeroStat> StatCache;

    public string Name;
    public int Speed;
    public int Gas;
    public int Blade;
    public int Accel;
    public string SkillId = "petra";

    public static HeroStat GetInfo(string name)
    {
        InitData();
        return StatCache[name];
    }

    private static void InitData()
    {
        if (!Init)
        {
            Init = true;

            // Mikasa
            HeroStat mikasa = new HeroStat();
            mikasa.Name = "MIKASA";
            mikasa.SkillId = "mikasa";
            mikasa.Speed = 125;
            mikasa.Gas = 75;
            mikasa.Blade = 75;
            mikasa.Accel = 135;

            // Levi
            HeroStat levi = new HeroStat();
            levi.Name = "LEVI";
            levi.SkillId = "levi";
            levi.Speed = 95;
            levi.Gas = 100;
            levi.Blade = 100;
            levi.Accel = 150;

            // Armin
            HeroStat armin = new HeroStat();
            armin.Name = "ARMIN";
            armin.SkillId = "armin";
            armin.Speed = 75;
            armin.Gas = 150;
            armin.Blade = 125;
            armin.Accel = 85;

            // Marco
            HeroStat marco = new HeroStat();
            marco.Name = "MARCO";
            marco.SkillId = "marco";
            marco.Speed = 110;
            marco.Gas = 100;
            marco.Blade = 115;
            marco.Accel = 95;

            // Jean
            HeroStat jean = new HeroStat();
            jean.Name = "JEAN";
            jean.SkillId = "jean";
            jean.Speed = 100;
            jean.Gas = 150;
            jean.Blade = 80;
            jean.Accel = 100;

            // Eren
            HeroStat eren = new HeroStat();
            eren.Name = "EREN";
            eren.SkillId = "eren";
            eren.Speed = 100;
            eren.Gas = 90;
            eren.Blade = 90;
            eren.Accel = 100;

            // Petra
            HeroStat petra = new HeroStat();
            petra.Name = "PETRA";
            petra.SkillId = "petra";
            petra.Speed = 80;
            petra.Gas = 110;
            petra.Blade = 100;
            petra.Accel = 140;

            // Sasha
            HeroStat sasha = new HeroStat();
            sasha.Name = "SASHA";
            sasha.SkillId = "sasha";
            sasha.Speed = 140;
            sasha.Gas = 100;
            sasha.Blade = 100;
            sasha.Accel = 115;

            // Custom default
            HeroStat customDefault = new HeroStat();
            customDefault.SkillId = "jean";
            customDefault.Speed = 100;
            customDefault.Gas = 100;
            customDefault.Blade = 100;
            customDefault.Accel = 100;

            // AHSS default
            HeroStat ahssDefault = new HeroStat();
            ahssDefault.Name = "AHSS";
            ahssDefault.SkillId = "jean";
            ahssDefault.Speed = 100;
            ahssDefault.Gas = 100;
            ahssDefault.Blade = 100;
            ahssDefault.Accel = 100;

            StatCache = new Dictionary<string, HeroStat>();
            StatCache.Add("MIKASA", mikasa);
            StatCache.Add("LEVI", levi);
            StatCache.Add("ARMIN", armin);
            StatCache.Add("MARCO", marco);
            StatCache.Add("JEAN", jean);
            StatCache.Add("EREN", eren);
            StatCache.Add("PETRA", petra);
            StatCache.Add("SASHA", sasha);
            StatCache.Add("CUSTOM_DEFAULT", customDefault);
            StatCache.Add("AHSS", ahssDefault);
        }
    }
}
