namespace Spent.Client.Core.Components.Pages.Identity;

public partial class DeleteAccountConfirmModal
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

    private async Task DeleteAccount()
    {
        await HttpClient.DeleteAsync("User/Delete", CurrentCancellationToken);

        await AuthenticationManager.SignOut();

        await CloseModal();
    }
}
