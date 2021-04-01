public class CostumeHair
{
    public static CostumeHair[] MaleHairs;
    public static CostumeHair[] FemaleHairs;

    public string hair = string.Empty;
    public string texture = string.Empty;
    public bool hasCloth;
    public string hair_1 = string.Empty;
    public int id;

    public static void Init()
    {
        // Male
        MaleHairs = new CostumeHair[11];
        for (int i = 0; i < MaleHairs.Length; i++)
        {
            MaleHairs[i] = new CostumeHair();
            MaleHairs[i].id = i;
        }
        MaleHairs[0].hair = (MaleHairs[0].texture = "hair_boy1");
        MaleHairs[1].hair = (MaleHairs[1].texture = "hair_boy2");
        MaleHairs[2].hair = (MaleHairs[2].texture = "hair_boy3");
        MaleHairs[3].hair = (MaleHairs[3].texture = "hair_boy4");
        MaleHairs[4].hair = (MaleHairs[4].texture = "hair_eren");
        MaleHairs[5].hair = (MaleHairs[5].texture = "hair_armin");
        MaleHairs[6].hair = (MaleHairs[6].texture = "hair_jean");
        MaleHairs[7].hair = (MaleHairs[7].texture = "hair_levi");
        MaleHairs[8].hair = (MaleHairs[8].texture = "hair_marco");
        MaleHairs[9].hair = (MaleHairs[9].texture = "hair_mike");
        MaleHairs[10].hair = (MaleHairs[10].texture = string.Empty);

        // Female
        FemaleHairs = new CostumeHair[11];
        for (int i = 0; i < FemaleHairs.Length; i++)
        {
            FemaleHairs[i] = new CostumeHair();
            FemaleHairs[i].id = i;
        }
        FemaleHairs[0].hair = (FemaleHairs[0].texture = "hair_girl1");
        FemaleHairs[1].hair = (FemaleHairs[1].texture = "hair_girl2");
        FemaleHairs[2].hair = (FemaleHairs[2].texture = "hair_girl3");
        FemaleHairs[3].hair = (FemaleHairs[3].texture = "hair_girl4");
        FemaleHairs[4].hair = (FemaleHairs[4].texture = "hair_girl5");
        FemaleHairs[4].hasCloth = true;
        FemaleHairs[4].hair_1 = "hair_girl5_cloth";
        FemaleHairs[5].hair = (FemaleHairs[5].texture = "hair_annie");
        FemaleHairs[6].hair = (FemaleHairs[6].texture = "hair_hanji");
        FemaleHairs[6].hasCloth = true;
        FemaleHairs[6].hair_1 = "hair_hanji_cloth";
        FemaleHairs[7].hair = (FemaleHairs[7].texture = "hair_mikasa");
        FemaleHairs[8].hair = (FemaleHairs[8].texture = "hair_petra");
        FemaleHairs[9].hair = (FemaleHairs[9].texture = "hair_rico");
        FemaleHairs[10].hair = (FemaleHairs[10].texture = "hair_sasha");
        FemaleHairs[10].hasCloth = true;
        FemaleHairs[10].hair_1 = "hair_sasha_cloth";
    }
}
