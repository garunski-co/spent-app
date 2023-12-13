
using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Spent.Client.Core.Components.Layout;
using Spent.Client.Core.Extensions;
using Spent.Client.Core.Services.HttpMessageHandlers;
using Spent.Client.Maui.Extensions;
using Spent.Client.Maui.Services;
using Spent.Commons.Infra;
using Spent.Commons.Services.Contracts;

namespace Spent.Client.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        var assembly = typeof(MainLayout).GetTypeInfo().Assembly;

        builder
            .UseMauiApp<App>()
            .Configuration.AddClientConfigurations();

        var services = builder.Services;

        services.AddMauiBlazorWebView();

        if (BuildConfiguration.IsDebug())
        {
            services.AddBlazorWebViewDeveloperTools();
        }

        Uri.TryCreate(builder.Configuration.GetApiServerAddress(), UriKind.Absolute, out var apiServerAddress);

        services.AddTransient(sp =>
        {
            var handler = sp.GetRequiredService<RequestHeadersDelegationHandler>();
            HttpClient httpClient = new(handler)
            {
                BaseAddress = apiServerAddress
            };
            return httpClient;
        });

        services.AddTransient<IStorageService, MauiStorageService>();

        services.AddClientMauiServices();

        var mauiApp = builder.Build();

        return mauiApp;
    }
}
