namespace TOTPGuard.Shared;

public class PayloadEncodingRequest
{
    public string Label { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public int Step { get; set; } = 30;
    public DateTime ExpiresAtUtc { get; set; }
}