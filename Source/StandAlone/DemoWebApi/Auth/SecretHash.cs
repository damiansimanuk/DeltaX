namespace DemoWebApi.Auth;
using System.Security.Cryptography;

public static class SecretHash
{
    private const int _saltSize = 16;
    private const int _keySize = 32;
    private const int _iterations = 50000;
    private const char segmentDelimiter = ':';
    private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;

    public static string Hash(string input)
    {
        var salt = RandomNumberGenerator.GetBytes(_saltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            _iterations,
            _algorithm,
            _keySize
        );
        return string.Join(
            segmentDelimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            _iterations,
            _algorithm
        );
    }

    public static bool Verify(string input, string hashString)
    {
        var segments = hashString.Split(segmentDelimiter);
        var hash = Convert.FromHexString(segments[0]);
        var salt = Convert.FromHexString(segments[1]);
        var iterations = int.Parse(segments[2]);
        var algorithm = new HashAlgorithmName(segments[3]);
        var inputHash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            iterations,
            algorithm,
            hash.Length
        );
        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }
}
