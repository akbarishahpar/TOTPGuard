using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Dinja.ServiceTypes;

namespace TOTPGuard.Api.Providers;

[Scoped]
public class SymmetricCryptographyProvider
{
    private readonly byte[] _iv =
    {
        0xFF, 0x00, 0x33, 0xDD, 0x66, 0x99, 0x77, 0x65,
        0x19, 0xEE, 0xBB, 0xC0, 0xE2, 0x25, 0x14, 0x99
    };

    public async Task<byte[]> EncryptAsync(byte[] plain, string password, CancellationToken cancellationToken)
    {
        //Initialing required tools
        using var aes = Aes.Create();
        using MemoryStream output = new();

        //Setting aes parameters
        aes.Key = DriveKeyFromPassword(password);
        aes.IV = _iv;

        //Encrypting data
        await using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
        await cryptoStream.WriteAsync(plain);
        await cryptoStream.FlushFinalBlockAsync();

        return output.ToArray();
    }

    public async Task<byte[]> DecryptAsync(byte[] cipher, string password, CancellationToken cancellationToken)
    {
        //Initialing required tools
        using var aes = Aes.Create();
        using MemoryStream output = new();
        using MemoryStream input = new(cipher);

        //Setting aes parameters
        aes.Key = DriveKeyFromPassword(password);
        aes.IV = _iv;

        //Decrypting data
        await using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
        await cryptoStream.CopyToAsync(output);

        return output.ToArray();
    }

    public async Task<string> EncryptAsync(string plain, string password, CancellationToken cancellationToken)
    {
        var output = await EncryptAsync(Encoding.Unicode.GetBytes(plain), password, cancellationToken);
        return Convert.ToHexString(output);
    }

    public async Task<string> DecryptAsync(string cipher, string password, CancellationToken cancellationToken)
    {
        var output = await DecryptAsync(Convert.FromHexString(cipher), password, cancellationToken);
        return Encoding.Unicode.GetString(output.ToArray());
    }

    public async Task<string> EncryptAsync<T>(T plain, string password, CancellationToken cancellationToken)
    {
        return await EncryptAsync(JsonSerializer.Serialize(plain), password, cancellationToken);
    }

    public async Task<T> DecryptAsync<T>(string cipher, string password, CancellationToken cancellationToken)
    {
        return JsonSerializer.Deserialize<T>(await DecryptAsync(cipher, password, cancellationToken))!;
    }

    private static byte[] DriveKeyFromPassword(string password)
    {
        var emptySalt = Array.Empty<byte>();
        var iterations = 1000;
        var desiredKeyLength = 16; // 16 bytes equal 128 bits.
        var hashMethod = HashAlgorithmName.SHA384;
        return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
            emptySalt,
            iterations,
            hashMethod,
            desiredKeyLength);
    }
}