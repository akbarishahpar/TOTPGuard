using Microsoft.AspNetCore.Mvc;
using TOTPGuard.Api.Options;
using TOTPGuard.Api.Providers;
using TOTPGuard.Shared;

namespace TOTPGuard.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PayloadController(
    SymmetricCryptographyProvider symmetricCryptographyProvider,
    OtpProvider otpProvider,
    SymmetricCryptographyOptions symmetricCryptographyOptions
) : ControllerBase
{
    [HttpPost]
    public async Task<string> EncodeAsync(PayloadEncodingRequest payload, CancellationToken cancellationToken)
    {
        return await symmetricCryptographyProvider.EncryptAsync(payload, symmetricCryptographyOptions.Key, cancellationToken);
    }

    [HttpGet]
    public async Task<IActionResult> GenerateOtpAsync(string encodedPayload,
        CancellationToken cancellationToken)
    {
        var payload = await symmetricCryptographyProvider.DecryptAsync<PayloadEncodingRequest>(
            encodedPayload,
            symmetricCryptographyOptions.Key,
            cancellationToken
        );

        if (DateTime.UtcNow > payload.ExpiresAtUtc)
            return NotFound();

        return Ok(otpProvider.Generate(payload));
    }
}