using System.IO;

public class EanAnimation
{
    public int Offset;
    public int FrameCount;
    public int MipWidth;
    public int MipHeight;
    public int StartX;
    public int StartY;
    public ushort TileCount;
    public ushort TotalCount;
    public ushort CellWidth;
    public ushort CellHeight;
    public EanFrame[] Frames;

    public void Load(BinaryReader br, FileStream fs)
    {
        Offset = br.ReadInt32();
        Offset += 16;
        FrameCount = br.ReadInt32();
        MipWidth = br.ReadInt32();
        MipHeight = br.ReadInt32();
        StartX = br.ReadInt32();
        StartY = br.ReadInt32();
        TileCount = br.ReadUInt16();
        TotalCount = br.ReadUInt16();
        CellWidth = br.ReadUInt16();
        CellHeight = br.ReadUInt16();
        Frames = new EanFrame[TotalCount];
        long position = fs.Position;
        fs.Seek(Offset, SeekOrigin.Begin);
        for (int i = 0; i < TotalCount; i++)
        {
            Frames[i].X = br.ReadUInt16();
            Frames[i].Y = br.ReadUInt16();
            Frames[i].Width = br.ReadUInt16();
            Frames[i].Height = br.ReadUInt16();
        }
        fs.Seek(position, SeekOrigin.Begin);
    }
}
