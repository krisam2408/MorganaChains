using MorganaChains.DataTransfer;
using System.Security.Cryptography;

namespace MorganaChains;

public sealed class RSAMorgana
{
    public readonly RSAParameters m_info;

    public RSAMorgana(EncodedKeyPair settings) { }

    public byte[] Encrypt(byte[] data, RSAPadding padding = RSAPadding.Pkcs) => throw new NotImplementedException();

    public string EncryptString(string str, RSAPadding padding = RSAPadding.Pkcs) => throw new NotImplementedException();

    public bool TryDecrypt(byte[] data, out byte[]? decrypted, RSAPadding padding = RSAPadding.Pkcs) => throw new NotImplementedException();

    public bool TryDecryptString(string str, out string? decrypted, RSAPadding padding = RSAPadding.Pkcs) => throw new NotImplementedException();

    public static KeyPair GenerateKeyPair() => throw new NotImplementedException();
}
