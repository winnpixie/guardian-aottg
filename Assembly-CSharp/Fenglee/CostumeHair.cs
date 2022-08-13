public class CostumeHair
{
    public static CostumeHair[] MaleHairs;
    public static CostumeHair[] FemaleHairs;

    public string Hair = string.Empty;
    public string Texture = string.Empty;
    public bool HasCloth;
    public string Hair1 = string.Empty;
    public int Id;

    public static void Init()
    {
        // Male
        MaleHairs = new CostumeHair[11];
        for (int i = 0; i < MaleHairs.Length; i++)
        {
            MaleHairs[i] = new CostumeHair()
            {
                Id = i
            };
        }
        MaleHairs[0].Hair = (MaleHairs[0].Texture = "hair_boy1");
        MaleHairs[1].Hair = (MaleHairs[1].Texture = "hair_boy2");
        MaleHairs[2].Hair = (MaleHairs[2].Texture = "hair_boy3");
        MaleHairs[3].Hair = (MaleHairs[3].Texture = "hair_boy4");
        MaleHairs[4].Hair = (MaleHairs[4].Texture = "hair_eren");
        MaleHairs[5].Hair = (MaleHairs[5].Texture = "hair_armin");
        MaleHairs[6].Hair = (MaleHairs[6].Texture = "hair_jean");
        MaleHairs[7].Hair = (MaleHairs[7].Texture = "hair_levi");
        MaleHairs[8].Hair = (MaleHairs[8].Texture = "hair_marco");
        MaleHairs[9].Hair = (MaleHairs[9].Texture = "hair_mike");
        MaleHairs[10].Hair = (MaleHairs[10].Texture = string.Empty);

        // Female
        FemaleHairs = new CostumeHair[11];
        for (int i = 0; i < FemaleHairs.Length; i++)
        {
            FemaleHairs[i] = new CostumeHair()
            {
                Id = i
            };
        }
        FemaleHairs[0].Hair = (FemaleHairs[0].Texture = "hair_girl1");
        FemaleHairs[1].Hair = (FemaleHairs[1].Texture = "hair_girl2");
        FemaleHairs[2].Hair = (FemaleHairs[2].Texture = "hair_girl3");
        FemaleHairs[3].Hair = (FemaleHairs[3].Texture = "hair_girl4");
        FemaleHairs[4].Hair = (FemaleHairs[4].Texture = "hair_girl5");
        FemaleHairs[4].HasCloth = true;
        FemaleHairs[4].Hair1 = "hair_girl5_cloth";
        FemaleHairs[5].Hair = (FemaleHairs[5].Texture = "hair_annie");
        FemaleHairs[6].Hair = (FemaleHairs[6].Texture = "hair_hanji");
        FemaleHairs[6].HasCloth = true;
        FemaleHairs[6].Hair1 = "hair_hanji_cloth";
        FemaleHairs[7].Hair = (FemaleHairs[7].Texture = "hair_mikasa");
        FemaleHairs[8].Hair = (FemaleHairs[8].Texture = "hair_petra");
        FemaleHairs[9].Hair = (FemaleHairs[9].Texture = "hair_rico");
        FemaleHairs[10].Hair = (FemaleHairs[10].Texture = "hair_sasha");
        FemaleHairs[10].HasCloth = true;
        FemaleHairs[10].Hair1 = "hair_sasha_cloth";
    }
}
