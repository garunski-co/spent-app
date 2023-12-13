namespace Spent.Client.Core.Services;

public partial class MessageBoxService
{
    [AutoInject]
    private readonly IPubSubService _pubSubService = default!;

    public async Task Show(string message, string title = "")
    {
        TaskCompletionSource<object?> tcs = new();
        _pubSubService.Publish(PubSubMessages.ShowMessage, (message, title, tcs));
        await tcs.Task;
    }
}
