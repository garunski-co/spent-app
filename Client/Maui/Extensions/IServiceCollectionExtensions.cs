using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Spent.Client.Core.Extensions;
using Spent.Client.Core.Services.Contracts;
using Spent.Client.Maui.Services;

namespace Spent.Client.Maui.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddClientMauiServices(this IServiceCollection services)
    {
        // Services registered in this class can be injected in Android, iOS, Windows, and macOS.

        services.TryAddTransient<MainPage>();
        services.TryAddSingleton<IBitDeviceCoordinator, MauiDeviceCoordinator>();
        services.TryAddTransient<IExceptionHandler, MauiExceptionHandler>();

#if ANDROID
        services.AddClientAndroidServices();
#elif iOS
        services.AddClientiOSServices();
#elif Mac
        services.AddClientMacServices();
#elif Windows
        services.AddClientWindowsServices();
#endif

        services.AddClientSharedServices();

        return services;
    }
}
