using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Spent.Commons.Services.Contracts;

namespace Spent.Client.Maui.Services;

public class MauiStorageService : IStorageService
{
    private readonly Dictionary<string, string> tempStorage = [];

    public async ValueTask<string> GetItem(string key)
    {
        tempStorage.TryGetValue(key, out string? value);
        return Preferences.Get(key, value);
    }

    public async ValueTask<bool> IsPersistent(string key)
    {
        return Preferences.ContainsKey(key);
    }

    public async ValueTask RemoveItem(string key)
    {
        Preferences.Remove(key);
        tempStorage.Remove(key);
    }

    public async ValueTask SetItem(string key, string? value, bool persistent = true)
    {
        if (persistent)
        {
            Preferences.Set(key, value);
        }
        else
        {
            if (tempStorage.TryAdd(key, value) is false)
            {
                tempStorage[key] = value;
            }
        }
    }
}
