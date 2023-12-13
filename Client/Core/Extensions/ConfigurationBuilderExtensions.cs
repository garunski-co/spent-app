using System.Reflection;

namespace Spent.Client.Core.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static void AddClientConfigurations(this IConfigurationBuilder builder)
    {
        var assembly = Assembly.Load("Spent.Client.Core");
        builder.AddJsonStream(assembly.GetManifestResourceStream("Spent.Client.Core.appsettings.json")!);
    }
}
