using MorganaChains.DataTransfer;
using System.Security.Cryptography;
using System.Text;

namespace MorganaChains;

public sealed class AESMorgana
{
    public readonly byte[] m_secretKey;
    public readonly byte[] m_publicKey;

    public AESMorgana(EncodedKeyPair settings)
    {
        m_secretKey = settings.SecretKey;
        m_publicKey = settings.PublicKey;
    }

    public string Encrypt(string text)
    {
        byte[] textByteArray = Encoding.UTF8.GetBytes(text);

        using (Aes aes = Aes.Create())
        {
            aes.IV = m_publicKey;
            aes.Key = m_secretKey;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new())
            using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(textByteArray, 0, textByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public bool TryDecrypt(string text, out string decrypted)
    {
        decrypted = "";
        text = text.Replace(" ", "+");
        byte[] textByteArray = Convert.FromBase64String(text);

        try
        {
            using (Aes aes = Aes.Create())
            {
                aes.IV = m_publicKey;
                aes.Key = m_secretKey;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new())
                using (CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read))
                {
                    cs.Write(textByteArray, 0, textByteArray.Length);
                    cs.FlushFinalBlock();
                    decrypted = Encoding.UTF8.GetString(ms.ToArray());
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    public static string GenerateRandomKey(string? characters = null)
    {
        if(string.IsNullOrWhiteSpace(characters))
            characters = Morgana.DefaultCharacters;

        Random rnd = new();
        int strLen = characters.Length;
        int len = rnd.Next(10, 100);

        for (int i = 0; i < len; i++)
            rnd.Next();

        StringBuilder sb = new();

        for (int i = 0; i < 16; i++)
        {
            int e = rnd.Next(strLen);
            sb.Append(characters[e]);
        }

        string result = sb.ToString();
        return result;
    }
}
