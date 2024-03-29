﻿using Spent.Client.Core.Extensions;
using Spent.Client.Web.Services;
using Microsoft.Extensions.Logging;

namespace Spent.Client.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientWebServices(this IServiceCollection services)
    {
        // Services being registered here can get injected in web (blazor web assembly & blazor server)

        services.AddTransient<IBitDeviceCoordinator, WebDeviceCoordinator>();
        services.AddTransient<IExceptionHandler, WebExceptionHandler>();

        services.AddClientSharedServices();

#pragma warning disable CA1416
        services.AddLogging(builder => builder.AddConsole());
#pragma warning restore CA1416

        return services;
    }
}
