using System;
using System.Collections.Generic;

public class PBitStream
{
    private List<byte> streamBytes;
    public int ByteCount => BytesForBits(BitCount);

    public int BitCount { get; private set; }

    public int Position
    {
        get;
        set;
    }

    public PBitStream()
    {
        streamBytes = new List<byte>(1);
    }

    public PBitStream(int bitCount)
    {
        streamBytes = new List<byte>(BytesForBits(bitCount));
    }

    public PBitStream(IEnumerable<byte> bytes, int bitCount)
    {
        streamBytes = new List<byte>(bytes);
        BitCount = bitCount;
    }

    public static int BytesForBits(int bitCount)
    {
        if (bitCount <= 0)
        {
            return 0;
        }
        return (bitCount - 1) / 8 + 1;
    }

    public void Add(bool val)
    {
        int num = BitCount / 8;
        if (num > streamBytes.Count - 1 || BitCount == 0)
        {
            streamBytes.Add(0);
        }
        if (val)
        {
            int num2 = 7 - BitCount % 8;
            List<byte> list;
            List<byte> list2 = list = streamBytes;
            int index;
            int index2 = index = num;
            byte b = list[index];
            list2[index2] = (byte)(b | (byte)(1 << num2));
        }
        BitCount++;
    }

    public byte[] ToBytes()
    {
        return streamBytes.ToArray();
    }

    public bool GetNext()
    {
        if (Position > BitCount)
        {
            throw new Exception("End of PBitStream reached. Can't read more.");
        }
        return Get(Position++);
    }

    public bool Get(int bitIndex)
    {
        int index = bitIndex / 8;
        int num = 7 - bitIndex % 8;
        return (streamBytes[index] & (byte)(1 << num)) > 0;
    }

    public void Set(int bitIndex, bool value)
    {
        int num = bitIndex / 8;
        int num2 = 7 - bitIndex % 8;
        List<byte> list;
        List<byte> list2 = list = streamBytes;
        int index;
        int index2 = index = num;
        byte b = list[index];
        list2[index2] = (byte)(b | (byte)(1 << num2));
    }
}
