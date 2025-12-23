using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
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
        return Base64UrlTextEncoder.Encode(await symmetricCryptographyProvider.EncryptAsync(payload.ToByteArray(), symmetricCryptographyOptions.Key, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GenerateOtpAsync(string encodedPayload,
        CancellationToken cancellationToken)
    {
        var payloadBytes = await symmetricCryptographyProvider.DecryptAsync(
            Base64UrlTextEncoder.Decode(encodedPayload),
            symmetricCryptographyOptions.Key,
            cancellationToken
        );

        var payload = PayloadEncodingRequest.FromByteArray(payloadBytes);

        if (DateTime.UtcNow > payload.ExpiresAtUtc)
            return NotFound();

        return Ok(otpProvider.Generate(payload));
    }
}