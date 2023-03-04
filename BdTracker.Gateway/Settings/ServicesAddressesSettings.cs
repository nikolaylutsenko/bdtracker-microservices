namespace BdTracker.Shared.Settings;
public interface IServiceAddressesSettings
{
    string Groups { get; init; }
}

public record ServiceAddressesSettings : IServiceAddressesSettings
{
    public const string SectionName = "ServicesAddresses";
    public string Groups { get; init; } = default!;
}