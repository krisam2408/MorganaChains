using MorganaChains.DataTransfer;
using System.Security.Cryptography;
using System.Text;

namespace MorganaChains;

public sealed class RSAMorgana
{
    public readonly RSAParameters m_info;

    public RSAMorgana(EncodedKeyPair settings)
    {
        m_info = new()
        {
            Modulus = settings.PublicKey,
            Exponent = settings.SecretKey
        };
    }

    public byte[] Encrypt(byte[] data, RSAPadding padding = RSAPadding.Pkcs)
    {
        using(RSA rsa = RSA.Create(m_info))
        {
            RSAEncryptionPadding rsaPadding = GetPadding(padding);
            return rsa.Encrypt(data, rsaPadding);
        }
    }

    public string EncryptString(string str, RSAPadding padding = RSAPadding.Pkcs)
    {
        byte[] data = Encoding.ASCII.GetBytes(str);
        byte[] encrypted = Encrypt(data, padding);
        return Convert.ToBase64String(encrypted);
    }

    public bool TryDecrypt(byte[] data, out byte[]? decrypted, RSAPadding padding = RSAPadding.Pkcs)
    {
        try
        {
            using(RSA rsa = RSA.Create(m_info))
            {
                RSAEncryptionPadding rsaPadding = GetPadding(padding);
                decrypted = rsa.Decrypt(data, rsaPadding);
                return true;
            }
        }
        catch
        {
            decrypted = null;
            return false;
        }
    }

    public bool TryDecryptString(string str, out string? decrypted, RSAPadding padding = RSAPadding.Pkcs)
    {
        decrypted = null;
        str = str.Replace(" ", "+");
        byte[] strByteArray = Convert.FromBase64String(str);

        if (TryDecrypt(strByteArray, out byte[]? decryptedData))
        {
            if (decryptedData is null)
                return false;

            decrypted = Encoding.UTF8.GetString(decryptedData);
            return true;
        }

        return false;
    }

    private static RSAEncryptionPadding GetPadding(RSAPadding padding)
    {
        if (padding == RSAPadding.OaepSHA3_512)
            return RSAEncryptionPadding.OaepSHA3_512;

        if (padding == RSAPadding.OaepSHA3_384)
            return RSAEncryptionPadding.OaepSHA3_384;

        if (padding == RSAPadding.OaepSHA3_256)
            return RSAEncryptionPadding.OaepSHA3_256;

        if (padding == RSAPadding.OaepSHA1)
            return RSAEncryptionPadding.OaepSHA1;

        if (padding == RSAPadding.OaepSHA512)
            return RSAEncryptionPadding.OaepSHA512;

        if (padding == RSAPadding.OaepSHA384)
            return RSAEncryptionPadding.OaepSHA384;

        if (padding == RSAPadding.OaepSHA256)
            return RSAEncryptionPadding.OaepSHA256;

        return RSAEncryptionPadding.Pkcs1;
    }

    public static KeyPair GenerateKeyPair()
    {
        using (RSA rsa = RSA.Create())
        {
            KeyPair result = new(rsa);
            return result;
        }
    }
}
