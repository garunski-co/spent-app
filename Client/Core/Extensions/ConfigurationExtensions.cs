namespace Spent.Client.Core.Extensions;

public static class ConfigurationExtensions
{
    public static string GetApiServerAddress(this IConfiguration configuration)
    {
        var apiServerAddress = configuration.GetValue("ApiServerAddress", "api/")!;

        return Uri.TryCreate(apiServerAddress, UriKind.RelativeOrAbsolute, out _)
            ? apiServerAddress
            : throw new InvalidOperationException($"Api server address {apiServerAddress} is invalid");
    }
}
