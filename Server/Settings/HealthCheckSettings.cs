namespace Spent.Server.Settings;

[UsedImplicitly]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class HealthCheckSettings
{
    public bool EnableHealthChecks { get; init; } = default!;
}
