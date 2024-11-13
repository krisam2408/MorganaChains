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

    public byte[] Encrypt(byte[] data)
    {
        using (Aes aes = Aes.Create())
        {
            aes.IV = m_publicKey;
            aes.Key = m_secretKey;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new())
            using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }
    }

    public string EncryptString(string str)
    {
        byte[] strByteArray = Encoding.UTF8.GetBytes(str);

        byte[] encrypted = Encrypt(strByteArray);

        return Convert.ToBase64String(encrypted.ToArray());
    }

    public bool TryDecrypt(byte[] data, out byte[]? decryptedData)
    {
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
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    decryptedData = ms.ToArray();
                    return true;
                }
            }
        }
        catch
        {
            decryptedData = null;
            return false;
        }
    }

    public bool TryDecryptString(string str, out string? decrypted)
    {
        decrypted = null;
        str = str.Replace(" ", "+");
        byte[] strByteArray = Convert.FromBase64String(str);
        
        if(TryDecrypt(strByteArray, out byte[]? decryptedData))
        {
            if (decryptedData is null)
                return false;

            decrypted = Encoding.UTF8.GetString(decryptedData);
            return true;
        }

        return false;
    }

    public static KeyPair GenerateKeyPair()
    {
        using (Aes aes = Aes.Create())
        {
            KeyPair result = new(aes);
            return result;
        }
    }
}
