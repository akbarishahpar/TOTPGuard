using Dinja.ServiceTypes;

namespace TOTPGuard.Api.Options;

[Configuration(nameof(SymmetricCryptographyOptions))]
public class SymmetricCryptographyOptions
{
    public string Key { get; set; } = string.Empty;
}