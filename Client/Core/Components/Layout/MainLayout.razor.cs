using Spent.Commons.Extensions;

namespace Spent.Client.Core.Components.Layout;

public partial class MainLayout : IDisposable
{
    [AutoInject]
    private AuthenticationManager _authManager = default!;

    private bool _disposed;

    // private ErrorBoundary _errorBoundaryRef = default!;
    [AutoInject]
    private IExceptionHandler _exceptionHandler = default!;

    private bool _isMenuOpen;

    private bool _isUserAuthenticated;

    [AutoInject]
    private IPrerenderStateService _prerenderStateService = default!;

    [CascadingParameter]
    public Task<AuthenticationState> AuthenticationStateTask { get; set; } = default!;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // protected override void OnParametersSet()
    // {
    //     // TODO: we can try to recover from exception after rendering the ErrorBoundary with this line.
    //     // but for now it's better to persist the error ui until a force refresh.
    //     // ErrorBoundaryRef.Recover();
    //
    //     base.OnParametersSet();
    // }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _authManager.AuthenticationStateChanged += VerifyUserIsAuthenticatedOrNot;

            _isUserAuthenticated = await _prerenderStateService.GetValue($"{nameof(MainLayout)}-isUserAuthenticated",
                async () => (await AuthenticationStateTask).User.IsAuthenticated());

            await base.OnInitializedAsync();
        }
        catch (Exception exp)
        {
            _exceptionHandler.Handle(exp);
        }
    }

    private async void VerifyUserIsAuthenticatedOrNot(Task<AuthenticationState> task)
    {
        try
        {
            _isUserAuthenticated = (await task).User.IsAuthenticated();
        }
        catch (Exception ex)
        {
            _exceptionHandler.Handle(ex);
        }
        finally
        {
            StateHasChanged();
        }
    }

    private void ToggleMenuHandler()
    {
        _isMenuOpen = !_isMenuOpen;
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _authManager.AuthenticationStateChanged -= VerifyUserIsAuthenticatedOrNot;

        _disposed = true;
    }
}
