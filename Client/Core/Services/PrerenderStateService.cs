namespace Spent.Client.Core.Services;

/// <summary>
///     For more information <see cref="IPrerenderStateService" /> docs.
/// </summary>
public class PrerenderStateService : IPrerenderStateService, IAsyncDisposable
{
    private readonly PersistentComponentState? _persistentComponentState;

    private readonly PersistingComponentStateSubscription? _subscription;

    // ReSharper disable once CollectionNeverUpdated.Local
    private readonly ConcurrentDictionary<string, object?> _values = new();

    public PrerenderStateService(PersistentComponentState? persistentComponentState = null)
    {
        _subscription = persistentComponentState?.RegisterOnPersisting(PersistAsJson, AppRenderMode.Current);
        _persistentComponentState = persistentComponentState;
    }

    public async ValueTask DisposeAsync()
    {
#if PrerenderEnabled
        _subscription?.Dispose();
#endif
    }

    public Task<T?> GetValue<T>(string key, Func<Task<T?>> factory)
    {
#if PrerenderEnabled
        if (_persistentComponentState!.TryTakeFromJson(key, out T? value))
        {
            return value;
        }

        var result = await factory();
        Persist(key, result);
        return result;
#endif

        return factory();
    }

#if PrerenderEnabled
    private void Persist<T>(string key, T value)
    {
        _values.TryRemove(key, out var _);
        _values.TryAdd(key, value);
    }
#endif

    private async Task PersistAsJson()
    {
        foreach (var item in _values)
        {
            _persistentComponentState!.PersistAsJson(item.Key, item.Value);
        }
    }
}
