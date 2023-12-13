namespace Spent.Server.Settings;

[UsedImplicitly]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class IdentitySettings
{
    public TimeSpan BearerTokenExpiration { get; init; } = default!;

    public TimeSpan RefreshTokenExpiration { get; init; } = default!;

    public string Issuer { get; init; } = default!;

    public string Audience { get; init; } = default!;

    public string IdentityCertificatePassword { get; init; } = default!;

    public bool PasswordRequireDigit { get; init; } = default!;

    public int PasswordRequiredLength { get; init; } = default!;

    public bool PasswordRequireNonAlphanumeric { get; init; } = default!;

    public bool PasswordRequireUppercase { get; init; } = default!;

    public bool PasswordRequireLowercase { get; init; } = default!;

    public bool RequireUniqueEmail { get; init; } = default!;

    public TimeSpan ConfirmationEmailResendDelay { get; init; } = default!;

    public TimeSpan ResetPasswordEmailResendDelay { get; init; } = default!;
}
