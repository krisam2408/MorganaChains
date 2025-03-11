using System.Security.Cryptography;
using System.Text;

namespace MorganaChains.DataTransfer;

public class KeyPair
{
    public string PublicKey { get; set; }
    public string SecretKey { get; set; }

    public KeyPair()
    {
        PublicKey = "";
        SecretKey = "";
    }

    public KeyPair(byte[] publicKey, byte[] secretKey)
    {
        PublicKey = Encoding.ASCII.GetString(publicKey);
        SecretKey = Encoding.ASCII.GetString(secretKey);
    }

    public KeyPair(string publicKey, string secretKey)
    {
        PublicKey = publicKey;
        SecretKey = secretKey;
    }

    public KeyPair(Aes aes)
    {
        aes.GenerateKey();
        aes.GenerateIV();

        PublicKey = Encoding.ASCII.GetString(aes.IV);
        SecretKey = Encoding.ASCII.GetString(aes.Key);
    }

    public KeyPair(RSA rsa)
    {
        PublicKey = "";
        SecretKey = "";

        RSAParameters info = rsa.ExportParameters(true);
        if(info.Modulus is not null)
            PublicKey = Encoding.ASCII.GetString(info.Modulus);

        if(info.Exponent is not null)
            SecretKey = Encoding.ASCII.GetString(info.Exponent);
    }
}
