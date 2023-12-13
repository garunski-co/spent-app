namespace Spent.Server.Settings;

[UsedImplicitly]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class EmailSettings
{
    public string Host { get; init; } = default!;

    /// <summary>
    ///     If true, the web app tries to store emails as .eml file in the bin/Debug/net8.0/sent-emails folder instead of
    ///     sending them using smtp server (recommended for testing purposes only).
    /// </summary>
    public bool UseLocalFolderForEmails => Host is "LocalFolder";

    public int Port { get; init; } = default!;

    public string UserName { get; init; } = default!;

    public string Password { get; init; } = default!;

    public string DefaultFromEmail { get; init; } = default!;

    public string DefaultFromName { get; init; } = default!;

    public bool HasCredential => string.IsNullOrEmpty(UserName) is false && string.IsNullOrEmpty(Password) is false;
}
