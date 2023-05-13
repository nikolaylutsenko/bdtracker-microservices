namespace BdTracker.Shared.Settings;
public interface IAuthSettings
{
    string Key { get; init; }
    string Issuer { get; set; }
    string Audience { get; set; }
}

public record AuthSettings : IAuthSettings
{
    public const string SectionName = "Authentication";
    public string Key { get; init; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
}