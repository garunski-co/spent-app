using System.Net.Http.Headers;

namespace Spent.Client.Core.Services.HttpMessageHandlers;

public class AuthDelegatingHandler(
    IAuthTokenProvider tokenProvider,
    IServiceProvider serviceProvider,
    IStorageService storageService,
    RetryDelegatingHandler handler)
    : DelegatingHandler(handler)
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is null)
        {
            var accessToken = await tokenProvider.GetAccessTokenAsync();
            if (accessToken is not null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception _) when (_ is ForbiddenException or UnauthorizedException && tokenProvider.IsInitialized)
        {
            // Let's update the access token by refreshing it when a refresh token is available.
            // Following this procedure, the newly acquired access token may now include the necessary roles or claims.

            var authManager = serviceProvider.GetRequiredService<AuthenticationManager>();
            var refreshToken = await storageService.GetItem("refresh_token");

            if (refreshToken is not null)
            {
                // In the AuthenticationStateProvider, the access_token is refreshed using the refresh_token (if available).
                await authManager.RefreshToken();

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", await tokenProvider.GetAccessTokenAsync());

                return await base.SendAsync(request, cancellationToken);
            }

            await authManager.SignOut();

            throw;
        }
    }
}
