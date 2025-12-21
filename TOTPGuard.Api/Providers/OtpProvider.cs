using Dinja.ServiceTypes;
using OtpNet;
using TOTPGuard.Shared;

namespace TOTPGuard.Api.Providers;

[Scoped]
public class OtpProvider
{
    public OtpGenerationResponse Generate(PayloadEncodingRequest payload)
    {
        var utcNow = DateTime.UtcNow;
        var totp = new Totp(Base32Encoding.ToBytes(payload.Secret), step: payload.Step);
        return new OtpGenerationResponse
        {
            Label = payload.Label,
            Otp = totp.ComputeTotp(utcNow),
            ExpiresAtUtc = utcNow.AddSeconds(totp.RemainingSeconds())
        };
    }
}