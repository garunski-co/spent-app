using System.Diagnostics;

namespace Spent.Client.Core.Services;

public abstract partial class ExceptionHandlerBase : IExceptionHandler
{
    [AutoInject]
    [UsedImplicitly]
    protected readonly IStringLocalizer<AppStrings> Localizer = default!;

    [AutoInject]
    [UsedImplicitly]
    protected readonly MessageBoxService MessageBoxService = default!;

    public virtual void Handle(Exception exception, IDictionary<string, object>? parameters = null)
    {
        var isDebug = BuildConfiguration.IsDebug();

        var exceptionMessage = (exception as KnownException)?.Message ??
                               (isDebug ? exception.ToString() : Localizer[nameof(AppStrings.UnknownException)]);

        if (isDebug)
        {
            _ = Console.Out.WriteLineAsync(exceptionMessage);
            Debugger.Break();
        }

        _ = MessageBoxService.Show(exceptionMessage, Localizer[nameof(AppStrings.Error)]);
    }
}
