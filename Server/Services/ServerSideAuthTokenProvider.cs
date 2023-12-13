using System.Reflection;
using Microsoft.JSInterop;
using Spent.Client.Core.Services;

namespace Spent.Server.Services;

/// <summary>
///     The <see cref="ClientSideAuthTokenProvider" /> reads the token from the local storage,
///     but during prerendering, there is no access to localStorage or the stored cookies.
///     However, the cookies are sent automatically in http request and The <see cref="ServerSideAuthTokenProvider" />
///     provides that token to the application.
/// </summary>
public partial class ServerSideAuthTokenProvider : IAuthTokenProvider
{
    private static readonly PropertyInfo IsInitializedProp = Assembly.Load("Microsoft.AspNetCore.Components.Server")
        .GetType("Microsoft.AspNetCore.Components.Server.Circuits.RemoteJSRuntime")!
        .GetProperty("IsInitialized")!;

    [AutoInject]
    private readonly IHttpContextAccessor _httpContextAccessor = default!;

    [AutoInject]
    private readonly IJSRuntime _jsRuntime = default!;

    [AutoInject]
    private readonly IStorageService _storageService = default!;

    public bool IsInitialized =>
        _jsRuntime.GetType().Name is not "UnsupportedJavaScriptRuntime" &&
        (bool)IsInitializedProp.GetValue(_jsRuntime)!;

    public async Task<string?> GetAccessTokenAsync()
    {
        if (IsInitialized)
        {
            return await _storageService.GetItem("access_token");
        }

        return _httpContextAccessor.HttpContext?.Request.Cookies["access_token"];
    }
}
