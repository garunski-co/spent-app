using Spent.Client.Core.Extensions;

namespace Spent.Client.Core.Components.Layout;

public partial class ConfirmMessageBox
{
    private bool _isOpen;

    private string? _message;

    private TaskCompletionSource<bool>? _tcs;

    private string? _title;

    public async Task<bool> Show(string message, string title)
    {
        if (_tcs is not null)
        {
            await _tcs.Task;
        }

        _tcs = new TaskCompletionSource<bool>();

        await InvokeAsync(() =>
        {
            _ = JsRuntime.SetBodyOverflow(true);

            _isOpen = true;
            _title = title;
            _message = message;

            StateHasChanged();
        });

        return await _tcs.Task;
    }

    private async Task Confirm(bool value)
    {
        _isOpen = false;
        await JsRuntime.SetBodyOverflow(false);
        _tcs?.SetResult(value);
    }
}
