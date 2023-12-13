namespace Spent.Client.Core.Components.Layout;

public partial class SignOutConfirmModal
{
    [Parameter]
    public bool IsOpen { get; set; }

    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    private Task CloseModal()
    {
        IsOpen = false;

        return IsOpenChanged.InvokeAsync(false);
    }

    private async Task SignOut()
    {
        await AuthenticationManager.SignOut();

        await CloseModal();
    }
}
