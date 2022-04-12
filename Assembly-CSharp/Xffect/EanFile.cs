using System.IO;

internal class EanFile
{
    public int Header;
    public int Version;
    public int Reserved;
    public int AnimCount;
    public EanAnimation[] Anims;

    public void Load(BinaryReader br, FileStream fs)
    {
        Header = br.ReadInt32();
        Version = br.ReadInt32();
        Reserved = br.ReadInt32();
        AnimCount = br.ReadInt32();
        Anims = new EanAnimation[AnimCount];
        for (int i = 0; i < AnimCount; i++)
        {
            Anims[i] = new EanAnimation();
            Anims[i].Load(br, fs);
        }
    }
}
