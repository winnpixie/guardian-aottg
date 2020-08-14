public class CostumeHair
{
    public string hair = string.Empty;
    public string texture = string.Empty;
    public bool hasCloth;
    public string hair_1 = string.Empty;
    public static CostumeHair[] hairsM;
    public static CostumeHair[] hairsF;
    public int id;

    public static void init()
    {
        // Male
        hairsM = new CostumeHair[11];
        for (int i = 0; i < hairsM.Length; i++)
        {
            hairsM[i] = new CostumeHair();
            hairsM[i].id = i;
        }
        hairsM[0].hair = (hairsM[0].texture = "hair_boy1");
        hairsM[1].hair = (hairsM[1].texture = "hair_boy2");
        hairsM[2].hair = (hairsM[2].texture = "hair_boy3");
        hairsM[3].hair = (hairsM[3].texture = "hair_boy4");
        hairsM[4].hair = (hairsM[4].texture = "hair_eren");
        hairsM[5].hair = (hairsM[5].texture = "hair_armin");
        hairsM[6].hair = (hairsM[6].texture = "hair_jean");
        hairsM[7].hair = (hairsM[7].texture = "hair_levi");
        hairsM[8].hair = (hairsM[8].texture = "hair_marco");
        hairsM[9].hair = (hairsM[9].texture = "hair_mike");
        hairsM[10].hair = (hairsM[10].texture = string.Empty);

        // Female
        hairsF = new CostumeHair[11];
        for (int i = 0; i < hairsF.Length; i++)
        {
            hairsF[i] = new CostumeHair();
            hairsF[i].id = i;
        }
        hairsF[0].hair = (hairsF[0].texture = "hair_girl1");
        hairsF[1].hair = (hairsF[1].texture = "hair_girl2");
        hairsF[2].hair = (hairsF[2].texture = "hair_girl3");
        hairsF[3].hair = (hairsF[3].texture = "hair_girl4");
        hairsF[4].hair = (hairsF[4].texture = "hair_girl5");
        hairsF[4].hasCloth = true;
        hairsF[4].hair_1 = "hair_girl5_cloth";
        hairsF[5].hair = (hairsF[5].texture = "hair_annie");
        hairsF[6].hair = (hairsF[6].texture = "hair_hanji");
        hairsF[6].hasCloth = true;
        hairsF[6].hair_1 = "hair_hanji_cloth";
        hairsF[7].hair = (hairsF[7].texture = "hair_mikasa");
        hairsF[8].hair = (hairsF[8].texture = "hair_petra");
        hairsF[9].hair = (hairsF[9].texture = "hair_rico");
        hairsF[10].hair = (hairsF[10].texture = "hair_sasha");
        hairsF[10].hasCloth = true;
        hairsF[10].hair_1 = "hair_sasha_cloth";
    }
}
