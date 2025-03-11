namespace MorganaChains;

public enum HashFormat
{
    MD5 = 128,
    SHA256 = 256,
    SHA384 = 384,
    SHA512 = 512,
}

public enum RSAPadding
{
    Pkcs,
    OaepSHA256,
    OaepSHA384,
    OaepSHA512,
    OaepSHA1,
    OaepSHA3_256,
    OaepSHA3_384,
    OaepSHA3_512
}
