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

    public Morgana(string? characters = null)
    {
        m_characters = characters;
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

        string finalString = sb.ToString();

        return finalString;
    }

    public static string Hash(string text, HashFormat format = HashFormat.MD5)
    {
        if (format == HashFormat.MD5)
            return HashMD5(text);

        if (format == HashFormat.SHA256)
            return HashSHA256(text);

        if (format == HashFormat.SHA384)
            return HashSHA384(text);

        if (format == HashFormat.SHA512)
            return HashSHA512(text);

        return text;
    }

    private static string HashMD5(string text)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(text);
        byte[] computedBuffer = MD5.HashData(buffer);
        return Convert.ToBase64String(computedBuffer);
    }

    private static string HashSHA256(string text)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(text);
        byte[] computedBuffer = SHA256.HashData(buffer);
        return Convert.ToBase64String(computedBuffer);
    }

    private static string HashSHA384(string text)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(text);
        byte[] computedBuffer = SHA384.HashData(buffer);
        return Convert.ToBase64String(computedBuffer);
    }

    private static string HashSHA512(string text)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(text);
        byte[] computedBuffer = SHA512.HashData(buffer);
        return Convert.ToBase64String(computedBuffer);
    }
}
