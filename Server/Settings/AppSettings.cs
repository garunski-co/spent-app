namespace Spent.Server.Settings;

public class AppSettings
{
    public IdentitySettings IdentitySettings { get; init; } = default!;

    public EmailSettings EmailSettings { get; init; } = default!;

    public HealthCheckSettings HealthCheckSettings { get; init; } = default!;

    public string UserProfileImagesDir { get; init; } = default!;

    public string WebServerAddress { get; init; } = default!;
}
