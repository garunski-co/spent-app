using Spent.Client.Core.Extensions;

namespace Spent.Client.Core.Components.Layout;

public partial class MessageBox
{
    private string? _body;

    private Action? _dispose;

    private bool _disposed = false;

    private bool _isOpen;

    private TaskCompletionSource<object?>? _tcs;

    private string? _title;

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private async Task OnCloseClick()
    {
        _isOpen = false;
        await JsRuntime.SetBodyOverflow(false);
        _tcs?.SetResult(null);
        _tcs = null;
    }

    private async Task OnOkClick()
    {
        _isOpen = false;
        await JsRuntime.SetBodyOverflow(false);
        _tcs?.SetResult(null);
        _tcs = null;
    }

    protected override Task OnInitAsync()
    {
        _dispose = PubSubService.Subscribe(PubSubMessages.ShowMessage, async args =>
        {
            var (message, title, tcs) = ((string message, string title, TaskCompletionSource<object?> tcs))args!;
            await (_tcs?.Task ?? Task.CompletedTask);
            _tcs = tcs;
            await ShowMessageBox(message, title);
        });

        return base.OnInitAsync();
    }

    private Task ShowMessageBox(string message, string title = "")
    {
        return InvokeAsync(() =>
        {
            _ = JsRuntime.SetBodyOverflow(true);

            _isOpen = true;
            _title = title;
            _body = message;

            StateHasChanged();
        });
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed || disposing is false)
        {
            return;
        }

        _tcs?.TrySetResult(null);
        _tcs = null;
        _dispose?.Invoke();

        _disposed = true;
    }
}
