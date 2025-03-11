using Microsoft.Extensions.Configuration;
using System.Text;

namespace MorganaChains.DataTransfer;

public sealed class EncodedKeyPair
{
    public byte[] PublicKey { get; private set; }
    public byte[] SecretKey { get; private set; }

    public EncodedKeyPair(IConfigurationSection configuration)
    {
        string? publicKey = configuration.GetSection("PublicKey").Value;
        string? secretKey = configuration.GetSection("SecretKey").Value;

        if (string.IsNullOrWhiteSpace(publicKey))
            throw new NullReferenceException(nameof(publicKey));

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new NullReferenceException(nameof(secretKey));

        PublicKey = Encoding.ASCII.GetBytes(publicKey);
        SecretKey = Encoding.ASCII.GetBytes(secretKey);
    }

    public EncodedKeyPair(string publicKey, string secretKey)
    {
        PublicKey = Encoding.ASCII.GetBytes(publicKey);
        SecretKey = Encoding.ASCII.GetBytes(secretKey);
    }

    public EncodedKeyPair(KeyPair keyPair) : this(keyPair.PublicKey, keyPair.SecretKey) { }
}
