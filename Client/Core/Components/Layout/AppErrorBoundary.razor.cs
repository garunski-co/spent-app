namespace Spent.Client.Core.Components.Layout;

/// <summary>
///     https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/handle-errors
/// </summary>
public partial class AppErrorBoundary
{
    [AutoInject]
    private IExceptionHandler _exceptionHandler = default!;

    [AutoInject]
    private NavigationManager _navigationManager = default!;

    private bool _showException;

    protected override void OnInitialized()
    {
        _showException = BuildConfiguration.IsDebug();
    }

    protected override async Task OnErrorAsync(Exception exception)
    {
        _exceptionHandler.Handle(exception);
    }

    private void Refresh()
    {
        _navigationManager.Refresh(true);
    }

    private void GoHome()
    {
        _navigationManager.NavigateTo("/", true);
    }
}
