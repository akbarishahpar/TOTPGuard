namespace TOTPGuard.Shared;

public class OtpGenerationResponse
{
    public string Label { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
}