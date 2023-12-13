using Spent.Commons.Extensions;

namespace Spent.Client.Core.Components.Layout;

public partial class Header
{
    private bool _disposed;

    private bool _isUserAuthenticated;

    [Parameter]
    public EventCallback OnToggleMenu { get; set; }

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitAsync()
    {
        AuthenticationManager.AuthenticationStateChanged += VerifyUserIsAuthenticatedOrNot;

        _isUserAuthenticated = await PrerenderStateService.GetValue($"{nameof(Header)}-isUserAuthenticated",
            async () => (await AuthenticationStateTask).User.IsAuthenticated());

        await base.OnInitAsync();
    }

    private async void VerifyUserIsAuthenticatedOrNot(Task<AuthenticationState> task)
    {
        try
        {
            _isUserAuthenticated = (await task).User.IsAuthenticated();
        }
        catch (Exception ex)
        {
            ExceptionHandler.Handle(ex);
        }
        finally
        {
            StateHasChanged();
        }
    }

    private Task ToggleMenu()
    {
        return OnToggleMenu.InvokeAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        AuthenticationManager.AuthenticationStateChanged -= VerifyUserIsAuthenticatedOrNot;

        _disposed = true;
    }
}
