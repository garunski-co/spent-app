﻿using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using Elsa.EntityFrameworkCore.Common;
using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Going.Plaid;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.ResponseCompression;
using Spent.Client.Core.Extensions;
using Spent.Server.Extensions;
using Spent.Server.Services;
using Spent.Server.Settings;
using Spent.Server.Workflows;

namespace Spent.Server.Startup;

public static class Services
{
    public static void Add(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
    {
        // Services being registered here can get injected into controllers and services in Server project.

        var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()!;

        services.AddExceptionHandler<ServerExceptionHandler>();

        services.AddBlazor(configuration);

        services.AddClientSharedServices();

        services.AddCors();
        
        services.AddLogging(builder => builder.AddConsole());

        services
            .AddControllers()
            .AddOData(options => options.EnableQueryFeatures())
            .AddDataAnnotationsLocalization(options =>
                options.DataAnnotationLocalizerProvider = StringLocalizerProvider.ProvideLocalizer)
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context => throw new ResourceValidationException(context
                    .ModelState
                    .Select(ms =>
                        (ms.Key, ms.Value!.Errors.Select(e => new LocalizedString(e.ErrorMessage, e.ErrorMessage))
                            .ToArray()
                        )
                    ).ToArray());
            });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
            options.ForwardedHostHeaderName = "X-Host";
        });

        services.AddResponseCaching();

        services.AddHttpContextAccessor();

        services.AddResponseCompression(opts =>
            {
                opts.EnableForHttps = true;
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["application/octet-stream"]).ToArray();
                opts.Providers.Add<BrotliCompressionProvider>();
                opts.Providers.Add<GzipCompressionProvider>();
            })
            .Configure<BrotliCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest)
            .Configure<GzipCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest);

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"),
                dbOptions => { dbOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); });
        });

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

        services.AddTransient(sp => sp.GetRequiredService<IOptionsSnapshot<AppSettings>>().Value);

        services.AddTransient<GetPlaidDataActivity>();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddIdentity(configuration);

        services.AddPlaid(configuration.GetSection($"{nameof(AppSettings)}:{nameof(PlaidSettings)}"));
        
        services.AddHealthChecks(env, configuration);

        services.AddTransient<HtmlRenderer>();

        services.AddElsa(elsa =>
        {
            var dbContextOptions = new ElsaDbContextOptions();
            var postgresConnectionString = configuration.GetConnectionString("Default")!;

            elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore(ef => ef.UsePostgreSql(postgresConnectionString, dbContextOptions)));
            elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore(ef => ef.UsePostgreSql(postgresConnectionString, dbContextOptions)));

            elsa.UseQuartz();
            elsa.UseScheduling(scheduling => scheduling.UseQuartzScheduler());

            elsa.AddWorkflowsFrom<Program>();

            elsa.AddActivitiesFrom<Program>();
        });

        var fluentEmailServiceBuilder = services.AddFluentEmail(appSettings.EmailSettings.DefaultFromEmail,
            appSettings.EmailSettings.DefaultFromName);

        if (appSettings.EmailSettings.UseLocalFolderForEmails)
        {
            var sentEmailsFolderPath = Path.Combine(AppContext.BaseDirectory, "sent-emails");

            Directory.CreateDirectory(sentEmailsFolderPath);

            fluentEmailServiceBuilder.AddSmtpSender(() => new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = sentEmailsFolderPath
            });
        }
        else
        {
            if (appSettings.EmailSettings.HasCredential)
            {
                fluentEmailServiceBuilder.AddSmtpSender(() =>
                    new(appSettings.EmailSettings.Host, appSettings.EmailSettings.Port)
                    {
                        Credentials = new NetworkCredential(appSettings.EmailSettings.UserName,
                            appSettings.EmailSettings.Password)
                    });
            }
            else
            {
                fluentEmailServiceBuilder.AddSmtpSender(appSettings.EmailSettings.Host, appSettings.EmailSettings.Port);
            }
        }
    }
}
