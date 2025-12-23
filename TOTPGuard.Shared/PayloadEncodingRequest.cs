using System.Text;

namespace TOTPGuard.Shared;

public class PayloadEncodingRequest
{
    public string Label { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public int Step { get; set; } = 30;
    public DateTime ExpiresAtUtc { get; set; }

    public byte[] ToByteArray()
    {
        var bytes = new List<byte>();

        bytes.AddRange(BitConverter.GetBytes(Step)); // 4 bytes
        bytes.AddRange(BitConverter.GetBytes(ExpiresAtUtc.ToBinary())); // 8 bytes

        var labelBytes = Encoding.UTF8.GetBytes(Label);
        bytes.AddRange(BitConverter.GetBytes(labelBytes.Length)); // 4 bytes
        bytes.AddRange(labelBytes); // dynamic length

        var secretBytes= Encoding.UTF8.GetBytes(Secret);
        bytes.AddRange(BitConverter.GetBytes(secretBytes.Length)); // 4 bytes
        bytes.AddRange(secretBytes); // dynamic length


        return bytes.ToArray();
    }

    public static PayloadEncodingRequest FromByteArray(byte[] data)
    {
        var labelLength = BitConverter.ToInt32(data, 12);
        var secretLength = BitConverter.ToInt32(data, 16 + labelLength);
        return new PayloadEncodingRequest
        {
            Label = Encoding.UTF8.GetString(data, 16, labelLength),
            Secret = Encoding.UTF8.GetString(data, 20 + labelLength, secretLength),
            Step = BitConverter.ToInt32(data, 0),
            ExpiresAtUtc = DateTime.FromBinary(BitConverter.ToInt64(data, 4)),
        };
    }
}