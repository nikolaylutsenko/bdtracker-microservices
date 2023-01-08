namespace BdTracker.Back.Settings;

public interface IConnectionStringSettings
{
    string ConnectionName { get; init; }
}

public class ConnectionStringSettings : IConnectionStringSettings
{
    public const string SectionName = "ConnectionStrings";
    public string ConnectionName { get; init; } = default!;
}
