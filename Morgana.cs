using System.Security.Cryptography;
using System.Text;

namespace MorganaChains;

public sealed class Morgana
{
    internal const string DefaultCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private readonly string? m_characters;
    public string Characters
    {
        get
        {
            if (string.IsNullOrWhiteSpace(m_characters))
                return DefaultCharacters;
            return m_characters;
        }
    }

    public Morgana()
    {
        m_characters = null;
    }

    public Morgana(string characters, bool addToDefault = false)
    {
        if (!addToDefault)
        {
            m_characters = characters;
            return;
        }

        m_characters = "";
        string t = DefaultCharacters + characters;

        foreach(char c in t)
        {
            if (!m_characters.Contains(c))
                m_characters += c;
        }
    }

    public string GenerateRandomLengthPassword(int length = 10, int offset = 2)
    {
        Random random = new();
        int min = length - offset;
        int max = length + offset;
        int len = random.Next(min, max);

        return GeneratePassword(random, len);
    }

    public string GeneratePasswordOfLength(int length)
    {
        Random random = new();
        return GeneratePassword(random, length);
    }

    private string GeneratePassword(Random random, int length)
    {
        StringBuilder sb = new();

        for (int i = 0; i < length; i++)
            sb.Append(Characters[random.Next(Characters.Length)]);

        string result = sb.ToString();

        return result;
    }

    public static byte[] Hash(byte[] data, HashFormat format = HashFormat.MD5)
    {
        if (format == HashFormat.SHA256)
            return SHA256.HashData(data);

        if (format == HashFormat.SHA384)
            return SHA384.HashData(data);

        if (format == HashFormat.SHA512)
            return SHA512.HashData(data);

        return MD5.HashData(data);
    }

    public static string HashString(string str, HashFormat format = HashFormat.MD5)
    {
        byte[] data = Encoding.UTF8.GetBytes(str);
        byte[] hash = Hash(data, format);
        string result = Convert.ToBase64String(hash);
        return result;
    }

    public static byte[] Encrypt(byte[] data)
    {
        List<byte> result = [];

        foreach (byte b in data)
            result.Add((byte)(byte.MaxValue - b));

        return result.ToArray();
    }
}
