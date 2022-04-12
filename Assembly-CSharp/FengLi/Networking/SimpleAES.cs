using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class SimpleAES
{
    private static byte[] key = new byte[32]
    {
        123,
        217,
        19,
        11,
        24,
        26,
        85,
        45,
        114,
        184,
        27,
        162,
        37,
        112,
        222,
        209,
        241,
        24,
        175,
        144,
        173,
        53,
        196,
        29,
        24,
        26,
        17,
        218,
        131,
        236,
        53,
        209
    };

    private static byte[] vector = new byte[16]
    {
        146,
        64,
        191,
        111,
        23,
        3,
        113,
        119,
        231,
        121,
        221,
        112,
        79,
        32,
        114,
        156
    };

    private ICryptoTransform encryptor;
    private ICryptoTransform decryptor;
    private UTF8Encoding encoder;

    public SimpleAES()
    {
        RijndaelManaged rijndaelManaged = new RijndaelManaged();
        encryptor = rijndaelManaged.CreateEncryptor(key, vector);
        decryptor = rijndaelManaged.CreateDecryptor(key, vector);
        encoder = new UTF8Encoding();
    }

    public string Encrypt(string unencrypted)
    {
        return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
    }

    public string Decrypt(string encrypted)
    {
        return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
    }

    public byte[] Encrypt(byte[] buffer)
    {
        return Transform(buffer, encryptor);
    }

    public byte[] Decrypt(byte[] buffer)
    {
        return Transform(buffer, decryptor);
    }

    protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
    {
        MemoryStream memoryStream = new MemoryStream();
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
        {
            cryptoStream.Write(buffer, 0, buffer.Length);
        }
        return memoryStream.ToArray();
    }
}
