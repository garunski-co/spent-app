using Microsoft.Net.Http.Headers;

namespace Spent.Server.Extensions;

public static class HttpRequestExtensions
{
    /// <summary>
    ///     https://blog.elmah.io/how-to-get-base-url-in-asp-net-core/
    /// </summary>
    public static Uri GetBaseUrl(this HttpRequest req)
    {
        var uriBuilder = new UriBuilder(req.Scheme, req.Host.Host, req.Host.Port ?? -1);
        if (uriBuilder.Uri.IsDefaultPort)
        {
            uriBuilder.Port = -1;
        }

        return uriBuilder.Uri;
    }

    public static bool IsCrawlerClient(this HttpRequest request)
    {
        if (request.Headers.TryGetValue(HeaderNames.UserAgent, out var userAgentHeaderValue) is false)
        {
            return false;
        }

        var userAgent = userAgentHeaderValue.FirstOrDefault();

        if (userAgent is null)
        {
            return false;
        }

        if (userAgent.Contains("google", StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        if (userAgent.Contains("bing", StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        if (userAgent.Contains("lighthouse", StringComparison.InvariantCultureIgnoreCase))
        {
            return true;
        }

        return false;

    }
}
